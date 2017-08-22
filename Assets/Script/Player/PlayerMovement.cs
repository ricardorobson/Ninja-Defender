using UnityEngine;
using UnityEngine.Networking;

[RequireComponent(typeof(Player))]
public class PlayerMovement : NetworkBehaviour {
    public float speed;
    public float jump;

	// Componesnts
	private Player player;

    private bool grounded;
    private bool hanging;
    private int floorCount;

	private Rigidbody body;

	private Vector2 movementInput;
	private bool jumpInput;
	private bool jumpTriggered;

	private Vector3 currentVelocity;
	[SerializeField]
	private float slow = 1;
	public float Slow {
		set{slow = value;}
	}

    private Animator anim;
    private GameManager gameManager;

    public AudioClip soundJump;
    public float volSoundJump;
    private AudioSource source;

    void Start() {
        source = GetComponent<AudioSource>();

        body = GetComponent<Rigidbody>();
		player = GetComponent<Player>();
        anim = GetComponentInChildren<Animator>();

		body.mass = MobConfig.Weigth.medium;
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();

    }

    void Update(){
		if (!isLocalPlayer || player.Dead || gameManager.Win)
			return;
		
		grounded  = Physics.Raycast(transform.position + transform.forward * 0.45f, -transform.up, 1.4f) 
			|| Physics.Raycast(transform.position - transform.forward * 0.45f, -transform.up, 1.4f) 
			|| Physics.Raycast(transform.position + transform.right * 0.45f, -transform.up, 1.4f) 
			|| Physics.Raycast(transform.position - transform.right * 0.45f, -transform.up, 1.4f) ;

		movementInput = new Vector2( Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        anim.SetFloat("Horizontal", movementInput.x);
        anim.SetFloat("Vertical", movementInput.y);
        jumpInput = Input.GetButtonDown ("Jump") && grounded;
        if (jumpInput)
        {
            anim.SetTrigger("Jump");
            source.PlayOneShot(soundJump, volSoundJump);
        }
        transform.rotation = new Quaternion(0, player.Cam.transform.rotation.y, 0, player.Cam.transform.rotation.w);
	}

    void FixedUpdate() {
        if (!isLocalPlayer || player.Dead || gameManager.Win)
            return;
		float currentSpeed = speed * slow;
		Vector3 targetPosition = body.position + (((transform.forward * movementInput.y) + (transform.right * movementInput.x)) * currentSpeed);
		Vector3 smoothedTargetPosition = Vector3.SmoothDamp (body.position, targetPosition, ref currentVelocity, 0.2f, currentSpeed, Time.fixedDeltaTime);
		body.MovePosition (smoothedTargetPosition);
		if (jumpInput) {
			body.AddForce (Vector3.up * jump, ForceMode.VelocityChange);
			jumpInput = false;
		}	
    }
}