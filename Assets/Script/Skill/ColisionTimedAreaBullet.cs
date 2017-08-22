using UnityEngine;

public class ColisionTimedAreaBullet : MonoBehaviour
{
    [SerializeField]
    private int damage;
	[SerializeField]
	private float radius;
	[SerializeField]
	private float delay;

    void OnCollisionEnter(Collision collider)
    {
        if ((collider.gameObject.tag != "Player")){
			Invoke ("AreaDamage", delay);
			gameObject.SetActive (false); // Disabling to stop any other actions other than AreaDamage
        }
    }

	void AreaDamage(){
		Collider[] colliders = Physics.OverlapSphere (gameObject.transform.position, radius);

		foreach (Collider c in colliders){
			if (c.gameObject.tag == "Mob") {
				c.gameObject.GetComponent<Mob>().takeDamage(damage);
			}
		}
		Destroy(gameObject);
	}
}
