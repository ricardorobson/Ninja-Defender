using UnityEngine;

public class Colision : MonoBehaviour {

	[SerializeField]
	private float damage = 1;

    void OnCollisionEnter (Collision Colider)
    {
        if ((Colider.gameObject.tag == "Mob"))
        {
            gameObject.GetComponent<Mob>().takeDamage(damage);
        }
    }
}
