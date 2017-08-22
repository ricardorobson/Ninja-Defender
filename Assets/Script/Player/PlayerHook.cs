using UnityEngine;

public class PlayerHook : MonoBehaviour {

	public float launchSpeed;
	public float ropeLength;
	public float ropeForce;
	public float weight;

	public GameObject player;
	public GameObject target;
	private Rigidbody hookBody;
	private SpringJoint ropeEffect;
    private Rigidbody playerRigidBody;
    /* Hook pull direction is defined by weight difference between target and player
	 * 	target is pulled if targetWeight < playerWeight
	 * 	player is pulled if playerWeight < targetWeight
	 * 	hook is pulled if playerWeight == targetWeight
	 */
    private int hookPullDirection;
	private const int PULL_TARGET = 1;
	private const int PULL_PLAYER = -1;
	private const int PULL_HOOK = 0;
	private float pullSpeed = 25;

	private float playerDistance;

	private bool launchRope;
	public static bool ropeCollided;
    private LineRenderer lrRope;

    public AudioClip soundHookStay;
    public float volSoundHookStay;
    public AudioClip soundHookImpact;
    public float volSoundHookImpact;
    private AudioSource source;

    void Start () {
        source = GetComponent<AudioSource>();

		hookBody = GetComponent<Rigidbody>();
		ropeEffect = player.GetComponent<SpringJoint>();
        playerRigidBody = player.GetComponent<Rigidbody>();
        launchRope = true;
		ropeCollided = false;

        lrRope = GetComponent<LineRenderer>();
        lrRope.SetWidth(0.05f, 0.05f);
        lrRope.SetColors(Color.blue, Color.blue);
    }

    void Update () {
		playerDistance = Vector3.Distance(transform.position, player.transform.position);
        bool input = Input.GetMouseButtonDown(0);

        if ( input || (ropeCollided && target==null)){
			launchRope = false;
		}else if(!input && target != null && target.tag=="Mob"){
            transform.position = target.transform.position;
        }

        if (Input.GetMouseButtonDown(1) ){
            Cancel();
        }else if (launchRope) {
            LaunchHook();
        } else {
            RecallHook();
        }

        lrRope.SetPosition(0, transform.position);
        lrRope.SetPosition(1, player.transform.position);

    }

    void Cancel(){
        launchRope = false;
        transform.position = Vector3.MoveTowards(transform.position, player.transform.position, pullSpeed * Time.deltaTime);
        target = null;
    }
    
    void OnTriggerExit(Collider coll){
        if (coll.tag == "Pillar"){
            Destroy(coll);
        }
    }

    

    void OnTriggerEnter(Collider coll){
        if (coll.isTrigger || target!=null) {
            return;
        }
		if(coll.tag != "Player"){
            source.PlayOneShot(soundHookImpact, volSoundHookImpact);

            target = coll.gameObject;
			Rigidbody targetRigidBody = target.GetComponent<Rigidbody> ();
			if (targetRigidBody && playerRigidBody) {
				if (targetRigidBody.mass < playerRigidBody.mass) {
					hookPullDirection = PULL_TARGET;
					gameObject.transform.SetParent (target.transform);
                    GetComponent<Rigidbody>().transform.SetParent(target.transform);
				} else if (targetRigidBody.mass > playerRigidBody.mass) {
					hookPullDirection = PULL_PLAYER;
				} else {
					hookPullDirection = PULL_HOOK;
				}
			} else {
				hookPullDirection = 0;
			}
			ropeCollided = true;
		}
	}

	public void LaunchHook(){
		if(playerDistance <= ropeLength){
            source.loop = true;
            source.PlayOneShot(soundHookStay, volSoundHookStay);

            if (!ropeCollided){
				transform.Translate(0, 0, launchSpeed*Time.deltaTime);
			}

			else{
				ropeEffect.connectedBody = hookBody;
				ropeEffect.spring = ropeForce;
				ropeEffect.damper = weight;
			}
		}

		if(playerDistance > ropeLength){
			launchRope = false;
		}
	}

	public void RecallHook(){
        if (target == null){
            Cancel();
        }else if (hookPullDirection == PULL_TARGET) {
			target.transform.position = Vector3.MoveTowards(target.transform.position, player.transform.position, pullSpeed*Time.deltaTime);
		} else if (hookPullDirection == PULL_PLAYER) {
			player.transform.position = Vector3.MoveTowards(player.transform.position, target.transform.position, pullSpeed*Time.deltaTime);
		} else if (hookPullDirection == PULL_HOOK) {
			transform.position = Vector3.MoveTowards(transform.position, player.transform.position, pullSpeed*Time.deltaTime);
		}

		ropeCollided = false;

		if(playerDistance <= 2){
            Destroy(gameObject);
        }
    }
} 