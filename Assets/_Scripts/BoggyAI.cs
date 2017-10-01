using System.Collections;
using UnityEngine;
using UnityEngine.AI;


namespace UnityStandardAssets.Characters.ThirdPerson
{

    public class BoggyAI : MonoBehaviour
    {

        //for audio
        public AudioClip chaseSound;
        AudioSource audioPoint;
        public bool alreadyPlayed = false;

        public NavMeshAgent agent;
        public ThirdPersonCharacter character;

        public enum State
        {
            PATROL,
            CHASE,
            AVOID
        }

        public State state;
        private bool alive;

        // Variables for Patrolling
        public GameObject[] waypoints;
        private int waypointInd;
        public float patrolSpeed = 0.5f;

        // Variables for Chasing 
        public float chaseSpeed = 1f;
        public GameObject target;

        // Use this for initialization
        void Start()
        {
            agent = GetComponent<NavMeshAgent>();
            character = GetComponent<ThirdPersonCharacter>();
            audioPoint = GetComponent<AudioSource>();

            agent.updatePosition = true;
            agent.updateRotation = false;

            waypoints = GameObject.FindGameObjectsWithTag("waypoint");
#pragma warning disable CS0618 // Type or member is obsolete
            waypointInd = Random.RandomRange(0, waypoints.Length);
#pragma warning restore CS0618 // Type or member is obsolete

            // must state with an state, or it will not start
            state = BoggyAI.State.PATROL;

            alive = true;

            // start FSM
            //StartCoroutine(FSM());
        }

        void Update()
        {
            StartCoroutine(FSM());
        }

        IEnumerator FSM()
        {
            while (alive)
            {
                switch (state)
                {
                    case State.PATROL:
                        Patrol();
                        break;
                    case State.CHASE:
                        Chase();
                        break;
                    case State.AVOID:
                        break;
                }
                yield return null;
            }
        }


        void Patrol()
        {
            agent.speed = patrolSpeed;
            if (Vector3.Distance (this.transform.position, waypoints[waypointInd].transform.position) >= 2)
            {
                agent.SetDestination(waypoints[waypointInd].transform.position);
                character.Move(agent.desiredVelocity, false, false);
            }
            else if (Vector3.Distance(this.transform.position, waypoints[waypointInd].transform.position) <= 2)
            {
#pragma warning disable CS0618 // Type or member is obsolete
                waypointInd = Random.RandomRange(0, waypoints.Length);
#pragma warning restore CS0618 // Type or member is obsolete
            }
            else
            {
                character.Move(Vector3.zero, false, false);
            }
        }

        void Chase()
        {
            agent.speed = chaseSpeed;
            agent.SetDestination(target.transform.position);
            character.Move(agent.desiredVelocity, false, false);

        }

        void Avoid()
        {
            agent.speed = 0;
            agent.SetDestination(this.transform.position);
            transform.LookAt(target.transform.position);
            character.Move(Vector3.zero, false, false);
        }

        private void OnTriggerEnter(Collider col)
        {
            if (col.tag == "Player")
            {
                if (!alreadyPlayed)
                {
                    audioPoint.PlayOneShot(chaseSound);
                    alreadyPlayed = true;
                }
                state = BoggyAI.State.CHASE;
                target = col.gameObject;
            }
        }
    }
}

