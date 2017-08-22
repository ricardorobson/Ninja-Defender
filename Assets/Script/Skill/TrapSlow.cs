using UnityEngine;
using System.Collections;

public class TrapSlow : MonoBehaviour {

    [SerializeField]
    private float slow = 0.5f;
    [SerializeField]
	private int manaCost;
	public int ManaCost
    {
		get { return manaCost; }
		set { manaCost = value; }
    }

    public int time;

    public AudioClip soundSlow;
    public float volSoundSlow;
    private AudioSource source;

    void Start(){
        source = GetComponent<AudioSource>();
        slow = SkillConfig.TrapSlow.slow;
		manaCost = SkillConfig.TrapSlow.manaCost;
	}

    void OnTriggerEnter(Collider collider){
		if (collider.tag == "Mob" && !collider.isTrigger){
            //mudar depois
            collider.gameObject.GetComponent<SimpleNavScript>().ActualSpeed = slow;
            source.PlayOneShot(soundSlow, volSoundSlow);
        }
    }

    void OnTriggerStay(Collider collider) {
		if (collider.tag == "Mob" && !collider.isTrigger) {
            //mudar depois
            collider.gameObject.GetComponent<SimpleNavScript>().ActualSpeed = slow;
        }
    }

    void OnTriggerExit(Collider collider) {
		if (collider.tag == "Mob" && !collider.isTrigger) {
            //mudar depois
            collider.gameObject.GetComponent<SimpleNavScript>().ActualSpeed = 1;
        }
    }

}
