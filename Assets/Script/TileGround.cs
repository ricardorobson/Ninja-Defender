using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class TileGround :  NetworkBehaviour
{

	public GameObject trap;
	public GameObject pillar;
    private GameObject previewPillar;
    private GameObject previewTrap;

		
	public void insertPillar(GameObject objectToSpawn, int duration)
    {
		pillar = insertObject (objectToSpawn);
		Destroy(pillar, duration);
    }

    public void insertPreviewPillar(GameObject objectToSpawn){
        if (pillar == null && previewPillar==null){
            previewPillar = insertPreviewObject(objectToSpawn);
        }
    }
    public void insertPreviewTrap(GameObject objectToSpawn){
        if (trap == null && previewTrap==null){
            previewTrap = insertPreviewObject(objectToSpawn);
        }
    }

    public void cleanPreview() {
        if (previewTrap != null) Destroy(previewTrap);
        if(previewPillar != null) Destroy(previewPillar);
    }

	public void insertTrap(GameObject objectToSpawn, int duration){
        if (trap != null) return;
		trap = insertObject (objectToSpawn);
		//Destroy(trap, duration);
	}

    private GameObject insertPreviewObject(GameObject objectToSpawn)
    {
        float tileHeight = GetComponent<MeshFilter>().mesh.bounds.extents.y * transform.localScale.y;
        float trapHeight = objectToSpawn.GetComponent<MeshFilter>().sharedMesh.bounds.extents.y * objectToSpawn.transform.localScale.y;

        Vector3 spawnPos = transform.position + new Vector3(0, tileHeight + trapHeight + 0.1f, 0);

        GameObject objReturn = Instantiate(objectToSpawn, spawnPos, Quaternion.identity) as GameObject;
        return objReturn;
    }

    private GameObject insertObject(GameObject objectToSpawn){

        GameObject objReturn = insertPreviewObject(objectToSpawn);
        NetworkServer.Spawn(objReturn);
        return objReturn;
    }

}
