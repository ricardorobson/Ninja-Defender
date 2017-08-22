using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using UnityEditor;
using UnityEngine.UI;

public class Spawner : NetworkBehaviour {

	public Transform[] possiblePaths;

	private float elapsed;

	public Wave[] waves;
	private int waveIndex;
	public static int count;
	private int waveSize;

	private int groupIndex;
	private SpawnGroup[] groups;

	private Spawn[] spawns;
	private int spawnIndex;
	private int spawnCnt;

	private float waveInterval;
	private float groupInterval;
	private float spawnInterval;
	private GameObject spawnObject;
	private int spawnLimit;

	public GameObject waveNumberCanvasText;

	[System.Serializable]
	public struct Spawn {
		public GameObject objectToSpawn;
		public int quantity;
		public float spawnInterval;
	}

	[System.Serializable]
	public struct SpawnGroup {
		public Spawn[] objectsToSpawn;
		public bool randomize; //TODO
		public int loops; // TODO
		public float groupInterval;
	}

	[System.Serializable]
	public struct Wave {
		public SpawnGroup[] spawnGroups;
		public float waveInterval;
	}
		

	// Use this for initialization
	void Start () {
		elapsed = 0;
		spawnCnt = 0;
		GetCurrentSpawn ();
		ShowWaveNumber ();
	}

	// Update is called once per frame
	void Update () {
		if (!isServer) {
			return;
		}

		if (GameManager.instance.lose || GameManager.instance.Win) {
			this.enabled = false;
			return;
		}

		if (waveIndex == waves.Length) {
			if (!GameManager.instance.lose) {
				GameManager.instance.WinGame ();
			}
			return;
		}
			
		if (groupIndex == waves [waveIndex].spawnGroups.Length) {
			elapsed += Time.deltaTime;
			if (GameManager.instance.mobsDestroyed == GameManager.instance.mobsSpawned) { // TODO: or elapsed > waveInterval?
				elapsed = 0;
				waveIndex++;

				if (waveIndex == waves.Length) {
					if (!GameManager.instance.lose) {
						GameManager.instance.WinGame ();
					}
					return;
				}

				groupIndex = 0;
				spawnIndex = 0;
				spawnCnt = 0;
				GetCurrentSpawn ();
				ShowWaveNumber ();
			}
		} else {
			if (spawnIndex == waves [waveIndex].spawnGroups [groupIndex].objectsToSpawn.Length) {
				elapsed += Time.deltaTime;
				if (elapsed > groupInterval) {
					elapsed -= groupInterval;
					groupIndex++;
					spawnIndex = 0;
					spawnCnt = 0;
					GetCurrentSpawn ();
				}
			} else {
				if (spawnCnt == spawnLimit) {
					spawnIndex++;
					spawnCnt = 0;
					GetCurrentSpawn ();
				} else {
					elapsed += Time.deltaTime;

					if (elapsed > spawnInterval) {
						elapsed -= spawnInterval;
						GameObject spawnedObject = Instantiate (spawnObject, gameObject.transform.position, Quaternion.identity) as GameObject;
						NetworkServer.Spawn (spawnedObject);
						spawnCnt++;
					}
				}
			}
		}

	}

	private void GetCurrentSpawn(){
		if (waveIndex == waves.Length 
			|| groupIndex == waves [waveIndex].spawnGroups.Length
			|| spawnIndex == waves [waveIndex].spawnGroups [groupIndex].objectsToSpawn.Length)
			return;
		
		spawnObject = waves [waveIndex].spawnGroups [groupIndex].objectsToSpawn [spawnIndex].objectToSpawn;
		spawnLimit = waves [waveIndex].spawnGroups [groupIndex].objectsToSpawn [spawnIndex].quantity;
		spawnInterval = waves [waveIndex].spawnGroups [groupIndex].objectsToSpawn [spawnIndex].spawnInterval;

		groupInterval = waves [waveIndex].spawnGroups [groupIndex].groupInterval;
		waveInterval = waves [waveIndex].waveInterval;

		SimpleNavScript navScript = spawnObject.GetComponentInChildren<SimpleNavScript> ();
		if (navScript) {
			navScript.possiblePaths = possiblePaths;
		} else {
			print(string.Format("não achou o script do {0}\n",spawnObject.name));
		}
	}

	private void ShowWaveNumber(){
		if (waveNumberCanvasText) {
			waveNumberCanvasText.GetComponent<Text> ().text = "Wave " + (waveIndex+1);
			waveNumberCanvasText.SetActive (true);
			StartCoroutine(DisableWaveText(3));
		} else {
			print ("Did not found wave number canvas");
		}
	}

	IEnumerator DisableWaveText(float delay)
	{
		yield return new WaitForSeconds(delay);

		if (waveNumberCanvasText) {
			waveNumberCanvasText.SetActive (false);
		}
	}
}