using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillConfig : MonoBehaviour {

	// Skills

	// Mark of the storm
	public static class MarkOfTheStormConfig {
		public static float baseDamage = 10f;
		public static float stackDamage = 15f;
		public static float duration = 10f;
		public static float cooldown = 3f;
		public static float range = 5f;
		public static int stackLimit = 5;
		public static int manaCost = 20;

		public static void Damage(Vector3 position){
			Collider[] colliders = Physics.OverlapSphere (position, range);
			Mob mob;
			MarkOfTheStorm mark;

			foreach (Collider c in colliders){
				if (c.gameObject.tag == "Mob") {
					mob = c.gameObject.GetComponent<Mob> ();
					mark = mob.markOfTheStorm;
					mob.takeDamage (mark.CalculateDamage ());
					mark.IncreaseStack ();
				}
			}
		}
	}

	public class MarkOfTheStorm {
		public int stacks;
		public float elapsedTime;

		public float CalculateDamage(){
			return MarkOfTheStormConfig.baseDamage + MarkOfTheStormConfig.stackDamage * stacks;
		}

		public void CheckStacks(float deltaTime){
			elapsedTime += deltaTime;

			if (elapsedTime > MarkOfTheStormConfig.duration) {
				DecreaseStack ();
				elapsedTime -= MarkOfTheStormConfig.duration;
			}
		}

		public void IncreaseStack(){
			elapsedTime = 0;

			if (stacks < MarkOfTheStormConfig.stackLimit)
				stacks++;
		}

		public void DecreaseStack(){
			if (stacks > 0)
				stacks--;
		}
	}

	//Pillars

	public class Pillar {
		public static float cooldown = 1f;
	}

	public class ExplosivePillar {
		public static int damage = 20;
		public static float radius = 5f;
		public static int manaCost = 20;
	}

	//Traps

	// Trap Damage
	public class TrapDamage {
		public static int damage = 5;
		public static float damageInterval = 1f;
		public static int manaCost = 20;
	}

	// Trap Slow
	public class TrapSlow {
		public static float slow = 0.1f;
		public static int manaCost = 15;
	}

	// Trap Stun
	public class TrapStun {
		public static float stunTime = 5f;
		public static float stunInterval = 0f;
		public static int manaCost = 20;
	}


	//Bullets

	// Bullet
	public class BaseBullet {
		public static int damage = 10;
		public static float cooldown = 0.2f;
		public static float speed = 5f;
	}

	// Stun Bullet
	public class StunBullet {
		public static int damage = 20;
		public static int manaCost = 20;
		public static float stunTime = 2;
		public static float cooldown = 0.5f;
	}

	// Area Bullet
	public class AreaBullet {
		
	}
}
