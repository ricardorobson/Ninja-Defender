using UnityEngine;
using System.Collections;

public class Mob : MonoBehaviour {

	public enum MobType {REGULAR, FAST, SLOW, GOLEM};
	public MobType mobType;

	private bool stunned = false;
    public bool Stunned{
        get{ return stunned; }
    }

	private float stunTime = 0;

	//[SerializeField]
	public float health = 10;

	[SerializeField]
	private float damage = 20;

	private Rigidbody body;
	public Transform target;
	public bool attackMode;
	private Renderer rend;
	private Color mobColor;

	[SerializeField]
	private float attackTime;
	[SerializeField]
	private float attackRange;

	private float elapsed;

	public SkillConfig.MarkOfTheStorm markOfTheStorm;
    Animator anim;

    public AudioClip soundDamage;
    public float volSoundDamage;
    private AudioSource source;

    virtual protected void Start()
    {
        source = GetComponent<AudioSource>();
        GameManager.instance.countMobSpawned ();

        anim = GetComponentInChildren<Animator>();
		body = GetComponent<Rigidbody> ();
		markOfTheStorm = new SkillConfig.MarkOfTheStorm ();
		rend = GetComponent<Renderer>();
		mobColor = rend.material.color;

		switch (mobType) {
		case MobType.REGULAR:
			this.health = MobConfig.MobRegularConfig.health;
			this.damage = MobConfig.MobRegularConfig.damage;
			this.attackRange = MobConfig.MobRegularConfig.attackRange;
			this.attackTime = MobConfig.MobRegularConfig.attackTime;
			this.body.mass = MobConfig.MobRegularConfig.weight;
			break;

		case MobType.FAST:
			this.health = MobConfig.MobFastConfig.health;
			this.damage = MobConfig.MobFastConfig.damage;
			this.attackRange = MobConfig.MobFastConfig.attackRange;
			this.attackTime = MobConfig.MobFastConfig.attackTime;
			this.body.mass = MobConfig.MobFastConfig.weight;
			break;

		case MobType.SLOW:
			this.health = MobConfig.MobSlowConfig.health;
			this.damage = MobConfig.MobSlowConfig.damage;
			this.attackRange = MobConfig.MobSlowConfig.attackRange;
			this.attackTime = MobConfig.MobSlowConfig.attackTime;
			this.body.mass = MobConfig.MobSlowConfig.weight;
			break;

		case MobType.GOLEM:
			this.health = MobConfig.MobGolemConfig.health;
			this.damage = MobConfig.MobGolemConfig.damage;
			this.attackRange = MobConfig.MobGolemConfig.attackRange;
			this.attackTime = MobConfig.MobGolemConfig.attackTime;
			this.body.mass = MobConfig.MobGolemConfig.weight;
			break;
		}
	}

	void Update () {
        anim.speed = stunned ? 0 : 1;
        if (stunCount())
			return;
        //se não estiver estunado, fazer o resto das ações abaixo
        

        markOfTheStorm.CheckStacks (Time.deltaTime);
		if (attackMode) {
			elapsed += Time.deltaTime;

			if (elapsed >= attackTime) {
				elapsed = 0;
				float distance = Vector3.Distance (body.position, target.position);
				if (distance <= attackRange) {
					Attack (target.gameObject);
				}
			}
		}

	}

	private bool stunCount() {
		if (stunned) {
			stunTime -= Time.deltaTime;
			if (stunTime <= 0) {
				stunned = false;
			}
		}
		return stunned;
	}

	public void Stun(float time) {
		stunned = true;
		stunTime = time;
	}

	public void takeDamage(float damage){
        source.PlayOneShot(soundDamage, volSoundDamage);
        health -= damage;
        /*
		rend.material.color = Color.red;
		StartCoroutine(TakeDamageColorChange (0.15f));
        */
        anim.SetTrigger("Damage");

        if (health <= 0) {
			SimpleNavScript navScript = GetComponent<SimpleNavScript> ();
			if (navScript) {
				navScript.enabled = false;
			}
			StartCoroutine(WaitToDie (0.2f));
		}
	}

	public void Attack(GameObject gameObject){
        anim.SetTrigger("Attack");
		gameObject.GetComponent<Player>().takeDamage(damage);
	}

	void OnCollisionEnter(Collision collider){
		if ((collider.gameObject.tag == "Player")){
			//Attack (collider.gameObject);
		}
	}

	void OnDestroy(){
		GameManager.instance.countMobDestroyed ();
	}

	public virtual void Die(){
		GameManager.instance.countMobKilled ();
		Destroy (gameObject.transform.parent.gameObject);
	}

	IEnumerator WaitToDie(float delay)
	{
		yield return new WaitForSeconds(delay);

		Die ();
	}

	IEnumerator TakeDamageColorChange(float delay)
	{
		yield return new WaitForSeconds(delay);

		rend.material.color = mobColor;
	}

}
