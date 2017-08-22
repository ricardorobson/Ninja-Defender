using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pillar : MonoBehaviour {
    [SerializeField]
    private int mana;
    public int Mana
    {
        get { return mana; }
        set { mana = value; }
    }

    public int time;
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
