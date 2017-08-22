using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class GateToTheHell : MonoBehaviour {

    private void OnTriggerEnter(Collider other){
		if(!other.isTrigger && other.gameObject.tag == "Mob"){
			Mob mob = other.gameObject.GetComponent<Mob> ();

			int portalDamage = 0;
			switch (mob.mobType) {
			case (Mob.MobType.REGULAR):
				portalDamage = MobConfig.MobRegularConfig.portalDamage;
				break;
			case (Mob.MobType.SLOW):
				portalDamage = MobConfig.MobSlowConfig.portalDamage;
				break;
			case (Mob.MobType.FAST):
				portalDamage = MobConfig.MobFastConfig.portalDamage;
				break;
			case (Mob.MobType.GOLEM):
				portalDamage = MobConfig.MobGolemConfig.portalDamage;
				break;

			}
				
			GameManager.instance.DamagePortal (portalDamage);
				
			Destroy(other.transform.parent.gameObject);
        }
    }
}
