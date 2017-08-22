using UnityEngine;
using UnityEngine.Networking;

public class MobGolem : Mob {
	public GameObject miniGolem;
	private SimpleNavScript golemNavScript;

	override protected void Start(){
		base.Start ();
		golemNavScript = GetComponent<SimpleNavScript> ();
	}

	override public void Die(){
		Vector3 position = transform.position;
		Vector3 randomVector1 = Random.insideUnitSphere;
		Vector3 randomVector2 = Random.insideUnitSphere;
		randomVector1.y = randomVector2.y = 0;

		GameObject spawnedObject1 = Instantiate (miniGolem, position + randomVector1, Quaternion.identity) as GameObject;
		GameObject spawnedObject2 = Instantiate (miniGolem, position + randomVector2, Quaternion.identity) as GameObject;
		//spawnedObject.GetComponentInChildren<Rigidbody> ().transform = position + randomVector1;
		UpdateNavScript(spawnedObject1);
		NetworkServer.Spawn(spawnedObject1);
		UpdateNavScript(spawnedObject2);
		//spawnedObject.GetComponentInChildren<Rigidbody> ().transform = position + randomVector2;
		NetworkServer.Spawn(spawnedObject2);
		GameManager.instance.countMobKilled ();
		Destroy (gameObject.transform.parent.gameObject);
	}

	private void UpdateNavScript(GameObject miniGolem){
		SimpleNavScript miniGolemNavScript = miniGolem.GetComponentInChildren<SimpleNavScript> ();
		if (golemNavScript && miniGolemNavScript) {
			miniGolemNavScript.SetWaypoints (golemNavScript.Waypoints, golemNavScript.PathIndex);
			//Destroy (miniGolemNavScript);
		}
		//miniGolem.AddComponent<SimpleNavScript> (miniGolemNavScript);
	}

}
