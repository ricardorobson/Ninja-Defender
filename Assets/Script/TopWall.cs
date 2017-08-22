using UnityEngine;
using System.Collections;

public class TopWall : Tile {
	public GameObject pillar;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void insertPillar(GameObject objectToSpawn){
		pillar = insertObject (objectToSpawn);
	}

	public void insertTrap(GameObject objectToSpawn){
//		trap = insertObject (objectToSpawn);
	}

	private GameObject insertObject(GameObject objectToSpawn){
		float tileHeight = GetComponent<MeshFilter>().mesh.bounds.extents.y * transform.localScale.y;
		float trapHeight = objectToSpawn.GetComponent<MeshFilter>().sharedMesh.bounds.extents.y * objectToSpawn.transform.localScale.y;

		Vector3 spawnPos = transform.position + new Vector3 (0, tileHeight + trapHeight + 0.1f, 0);
		return Instantiate (objectToSpawn, spawnPos, Quaternion.identity) as GameObject;
	}
}
