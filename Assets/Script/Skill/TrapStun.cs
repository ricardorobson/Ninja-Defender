using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapStun : MonoBehaviour {
    public int time;
    public float stunTime;
    public float stunInterval;
    private float lastStunTime;
    [SerializeField]
	private int manaCost;
    public int ManaCost
    {
		get { return manaCost; }
		set { manaCost = value; }
    }

    public AudioClip soundStun;
    public float volSoundStun;
    private AudioSource source;

    void Start()
    {
        source = GetComponent<AudioSource>();
		manaCost = SkillConfig.TrapStun.manaCost;
		stunTime = SkillConfig.TrapStun.stunTime;
		stunInterval = SkillConfig.TrapStun.stunInterval;
	}


    void OnTriggerEnter(Collider collider)
    {

		if (collider.tag == "Mob" && !collider.isTrigger)
        {
            if (Time.time > lastStunTime + stunInterval)
            {
				collider.gameObject.GetComponent<Mob>().Stun(stunTime);
                source.PlayOneShot(soundStun, volSoundStun);
                lastStunTime = Time.time;
            }
        }
    }
}
