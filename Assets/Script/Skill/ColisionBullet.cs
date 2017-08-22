using UnityEngine;
using System.Collections;

public class ColisionBullet : MonoBehaviour
{
    [SerializeField]
	private int damage;

    //public AudioClip soundImpact;
   // public float volSoundImpact;
   // private AudioSource source;

    void Start(){
        //source = GetComponent<AudioSource>();
        damage = SkillConfig.BaseBullet.damage;
	}

    void OnCollisionEnter(Collision collider)
    {
        //source.PlayOneShot(soundImpact, volSoundImpact);

        if ((collider.gameObject.tag != "Player") && (collider.gameObject.tag == "Mob"))
        {
			collider.gameObject.GetComponentInParent<Mob>().takeDamage(damage);
			//print ("bala colidiu com " + collider.gameObject.name);
        }

        //TO DO - Destruir quando todar no player
        if (collider.gameObject.tag != "Player"){
            Destroy(gameObject, 0.2f);
        }
    }
}
