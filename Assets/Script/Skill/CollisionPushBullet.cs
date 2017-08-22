using UnityEngine;
using System.Collections;

public class CollisionPushBullet : MonoBehaviour {

    [SerializeField]
    private int mana;
    public int Mana
    {
        get { return mana; }
        set { mana = value; }
    }

    void OnCollisionEnter(Collision collider){
		if ((collider.gameObject.tag != "Player")) {
			Destroy(gameObject);
		}
	}
}
