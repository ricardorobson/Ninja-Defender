using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MobConfig : MonoBehaviour {
	public class Weigth { // TODO: weight
		public static float heavy = 2f;
		public static float medium = 1f;
		public static float light = 0.5f;
	}

	//TODO: change to subclasses for better modularization

	// Mob regular (1)
	public class MobRegularConfig
	{
		public static float health = 20;
		public static float damage = 10;
		public static float attackTime = 2;
		public static float attackRange = 3;
		public static float speed = 6;
		public static float acceleration = 6;
		public static bool persuitPlayer =  true;
		public static float weight = Weigth.medium;
		public static int portalDamage = 1;
	}

	// Mob fast (2)
	public class MobFastConfig
	{
		public static float health = 10;
		public static float damage = 0;
		public static float attackTime = 0;
		public static float attackRange = 0;
		public static float speed = 10;
		public static float acceleration = 8;
		public static bool persuitPlayer =  false;
		public static float weight = Weigth.light;
		public static int portalDamage = 1;
	}

	// Mob slow (3)
	public class MobSlowConfig
	{
		public static float health = 100;
		public static float damage = 20;
		public static float attackTime = 2;
		public static float attackRange = 3;
		public static float speed = 3;
		public static float acceleration = 10;
		public static bool persuitPlayer =  true;
		public static float weight = Weigth.heavy;
		public static int portalDamage = 1;
	}

	// Mob golem (4)
	public class MobGolemConfig
	{
		public static float health = 50;
		public static float damage = 20;
		public static float attackTime = 2;
		public static float attackRange = 3;
		public static float speed = 4;
		public static float acceleration = 10;
		public static bool persuitPlayer =  false;
		public static float weight = Weigth.heavy;
		public static int portalDamage = 5;
	}
}
