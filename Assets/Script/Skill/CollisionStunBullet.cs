using UnityEngine;

public class CollisionStunBullet : MonoBehaviour {

    [SerializeField]
    private float stunTime = 1;
    [SerializeField]
    private int damage;
    [SerializeField]
	private int manaCost;
    public int ManaCost
    {
		get { return manaCost; }
		set { manaCost = value; }
    }

    //public AudioClip soundImpact;
    //public float volSoundImpact;
    //private AudioSource source;

    void Start()
    {
        //source = GetComponent<AudioSource>();
        damage = SkillConfig.StunBullet.damage;
		stunTime = SkillConfig.StunBullet.stunTime;
		manaCost = SkillConfig.StunBullet.manaCost;
	}

    void OnCollisionEnter(Collision collider) {
        //source.PlayOneShot(soundImpact, volSoundImpact);

        if ((collider.gameObject.tag != "Player") && (collider.gameObject.tag == "Mob"))
        {
            collider.gameObject.GetComponent<Mob>().Stun(stunTime);
            collider.gameObject.GetComponentInParent<Mob>().takeDamage(damage);
        }

        if (collider.gameObject.tag != "Player")
        {
            Destroy(gameObject,0.2f);
        }
    }
}
