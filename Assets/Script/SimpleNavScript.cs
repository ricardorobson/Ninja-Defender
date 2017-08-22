using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using UnityEngine.AI;

//

public class SimpleNavScript : NetworkBehaviour {
	public Transform[] possiblePaths;
	public float randomizedDistance = 0;

	private Transform path;
	private int pathIndex = 1; // Zero is the Path parent itself
	public int PathIndex {
		set{pathIndex = value;}
		get{return pathIndex;}
	}

	private int pathLength = 0;
	private Vector3 destination;
	private Transform[] waypoints;
	public Transform[] Waypoints {
		set{waypoints = value;}
		get{return waypoints;}
	}

	public float speed;
	public float acceleration;

	public bool persuitPlayer;

	private float distance;

	private Rigidbody body;
	private Vector3 currentVelocity = Vector3.zero;
	private NavMeshAgent agent;
	private NavMeshPath agentPath;

	public enum State {SEEK, PERSUIT, ATTACK};
	public State state;

	private Transform target;

    private float actualSpeed;
    public float ActualSpeed{
        set{
            actualSpeed = speed * value;
        }
    }

    private Mob mob;

	// Use this for initialization
	void Start () {
//		if (!isServer) {
//			return;
//		}
		agent = GetComponent<NavMeshAgent> ();

		agent.updatePosition = false;
		agent.updateRotation = false;
		agent.avoidancePriority = Random.Range (1, 99);
		agentPath = new NavMeshPath ();
        //agent.stoppingDistance = randomizedDistance/2 + 0.5f;

        mob = GetComponent<Mob>();

		int length = possiblePaths.Length;

		if (length == 0) {
			this.enabled = false;
			print (gameObject.name + " navscript disabled");
			return;
		}

		if (waypoints == null || waypoints.Length == 0) {
			int pathChoice = Random.Range (0, length);
			path = possiblePaths [pathChoice];

			waypoints = path.GetComponentsInChildren<Transform> ();
		}
		pathLength = waypoints.Length;

		body = GetComponent<Rigidbody> ();
		destination = body.position;

		switch (GetComponent<Mob>().mobType) {
		case Mob.MobType.REGULAR:
			this.speed = MobConfig.MobRegularConfig.speed;
			this.acceleration = MobConfig.MobRegularConfig.acceleration;
			this.persuitPlayer = MobConfig.MobRegularConfig.persuitPlayer;
			break;

		case Mob.MobType.FAST:
			this.speed = MobConfig.MobFastConfig.speed;
			this.acceleration = MobConfig.MobFastConfig.acceleration;
			this.persuitPlayer = MobConfig.MobFastConfig.persuitPlayer;
			break;

		case Mob.MobType.SLOW:
			this.speed = MobConfig.MobSlowConfig.speed;
			this.acceleration = MobConfig.MobSlowConfig.acceleration;
			this.persuitPlayer = MobConfig.MobSlowConfig.persuitPlayer;
			break;

		case Mob.MobType.GOLEM:
			this.speed = MobConfig.MobGolemConfig.speed;
			this.acceleration = MobConfig.MobGolemConfig.acceleration;
			this.persuitPlayer = MobConfig.MobGolemConfig.persuitPlayer;
			break;
		}

        actualSpeed = speed;
	}

	void FixedUpdate () {
//		if (!isServer) {
//			return;
//		}

		if (pathIndex == pathLength) {
			agent.enabled = false;
			this.enabled = false;
			return;
		}

		if (!IsOnGround() || mob.Stunned) {
			agent.enabled = false;
			agent.enabled = true;
			return;
		}

		if (state == State.SEEK) {
			NavMeshHit myDestinationHit, myPositionHit;
			bool canReachDestination = NavMesh.SamplePosition (destination, out myDestinationHit, 2f, NavMesh.AllAreas);

			if (Reached () || PassedWaypoint() || (!canReachDestination && pathIndex != pathLength - 1) ) {

				destination = waypoints [pathIndex].position;
				Vector3 randomized = Random.insideUnitCircle * randomizedDistance;
				randomized.y = 0;
				destination += randomized;
				NavMeshHit randomizedDestinationHit = new NavMeshHit ();
				NavMesh.SamplePosition (destination, out randomizedDestinationHit, randomizedDistance + 2f, NavMesh.AllAreas);
				destination = randomizedDestinationHit.position;
				//agent.SetDestination (destination);
				string message = string.Format ("{0} going from {1} to {2} {3}({4})\n",
					                 name, 
					                 transform.position,
					                 destination,
					                 waypoints [pathIndex].name,
					                 pathIndex);
				//print (message);
				if (pathIndex < pathLength-1) {
					pathIndex++;
					return;
				}

			} else {
				Move (destination, true, true);
			}
		} else if (state == State.PERSUIT) {
			Move (target.position, true, false);
            if (target.gameObject.GetComponentInParent<Player>().Dead){
                state = State.SEEK;
            }
		}

	}

	void SearchPlayer(){
		Collider[] colliders = Physics.OverlapSphere (body.position, 2f);
		for (int i = 0; i < colliders.Length; i++) {
			if (colliders [i].tag == "Player" && !colliders[i].gameObject.GetComponentInParent<Player>().Dead) {
				state = State.ATTACK;
				target = colliders [i].transform;
			}
		}
	}

	void Move(Vector3 position, bool avoidWalls, bool avoidPlayer){
		agent.nextPosition = body.position;
		agent.SetDestination (position);

		Vector3 direction = agent.desiredVelocity.normalized;
		body.transform.LookAt (body.position + (direction + body.transform.forward)/2f);

		if (avoidWalls || avoidPlayer) {
			RaycastHit frontRay, leftRay, rightRay;
			bool hasHitFront, hasHitLeft, hasHitRight;

			hasHitFront = Physics.SphereCast (body.position, 0.5f, direction, out frontRay, 1f);

			if (hasHitFront && (avoidPlayer || !(frontRay.collider.tag == "Player") ) ) {
				Vector3 leftDirection = RotateLeft (direction);
				Vector3 rightDirection = RotateRight (direction);
				hasHitLeft = Physics.SphereCast (body.position, 0.5f, leftDirection, out leftRay, 1f);
				hasHitRight = Physics.SphereCast (body.position, 0.5f, rightDirection, out rightRay, 1f);

				if (hasHitLeft && hasHitRight) {
					if (leftRay.distance > rightRay.distance) {
						direction = leftDirection;
					} else {
						direction = rightDirection;
					}
				} else if (hasHitLeft) {
					direction = rightDirection;
				} else {
					direction = leftDirection;
				}
			} 
		}

		if (body.velocity.magnitude < actualSpeed)
			body.AddForce (direction * acceleration * 10 * body.mass);
		else if (body.velocity.magnitude > actualSpeed)
			body.velocity = direction * actualSpeed;

		Vector3 targetPosition = body.position + body.velocity;
		Debug.DrawLine (body.position, targetPosition, Color.blue);
		Debug.DrawLine (body.position, destination, Color.red);
		currentVelocity = body.velocity;
	}

	bool Reached(){
		Vector3 from = body.position;
		from.y = 0;
		Vector3 to = destination;
		to.y = 0;
		distance = Vector3.Distance (from, to);
		return distance <= 1f;
	}

	bool IsOnGround(){
		return Mathf.Abs (body.velocity.y) < 0.1f;
	}

	Vector3 RotateLeft(Vector3 vector){
		return Quaternion.Euler (0, -45, 0) * vector;
	}

	Vector3 RotateRight(Vector3 vector){
		return Quaternion.Euler (0, 45, 0) * vector;
	}

	bool RandomBoolean(){
		return Random.value > 0.5f;
	}

	void OnTriggerEnter(Collider other) {
		//print (string.Format ("{0} colidiu com {1}\n", gameObject.name, other.name));
		if (other.tag == "Player") {
			if (persuitPlayer) {
				state = State.PERSUIT;
			}
			target = other.transform;
			GetComponent<Mob> ().attackMode = true;
			GetComponent<Mob> ().target = target;
		}
	}

	void OnTriggerExit(Collider other) {
		if (other.tag == "Player") {
			target = other.transform;
			state = State.SEEK;
			GetComponent<Mob> ().attackMode = false;
		}
	}

	bool PassedWaypoint(){
		if (pathIndex >= pathLength - 1)
			return false;
		
		return Vector3.Distance(waypoints [pathIndex+1].position, body.position) < Vector3.Distance(destination, body.position) ;
	}

	public void SetWaypoints(Transform[] waypoints, int pathIndex){
		this.waypoints = waypoints;
		this.pathIndex = pathIndex;
		this.pathLength = waypoints.Length;
		this.destination = waypoints [pathIndex].position;
	}
		
}
