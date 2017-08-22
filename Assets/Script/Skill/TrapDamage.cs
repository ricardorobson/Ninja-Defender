using UnityEngine;
using System.Collections;

public class TrapDamage : MonoBehaviour {

    [SerializeField]
    private int damage = 1;
    [SerializeField]
    public float damageInterval;
    private float lastDamageTime;
    [SerializeField]
	private int manaCost;
    public int ManaCost
    {
		get { return manaCost; }
		set { manaCost = value; }
    }

    public int time;
    

    void Start() {
        lastDamageTime = Time.time;

		damage = SkillConfig.TrapDamage.damage;
		damageInterval = SkillConfig.TrapDamage.damageInterval;
		manaCost = SkillConfig.TrapDamage.manaCost;
    }
    
    void OnTriggerStay(Collider collider) {
		if(collider.tag == "Mob" && !collider.isTrigger){
            if (Time.time > lastDamageTime + damageInterval){
                collider.gameObject.GetComponent<Mob>().takeDamage(damage);
                lastDamageTime = Time.time;
            }
        }
    }
}
