using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
	public static GameManager instance = null;              //Static instance of GameManager which allows it to be accessed by any other script.
	//private BoardManager boardScript;                       //Store a reference to our BoardManager which will set up the level.
	//private int level = 3;                                  //Current level number, expressed in game as "Day 1".
	public GameObject mobKilledCanvas;
	private Text mobKilledText;

	public GameObject portalHealthCanvas;
	private Text portalHealthText;

	public GameObject winGameText;
	public GameObject loseGameText;
	public GameObject crossHair;

	public int mobsKilled = 0;
	public int mobsSpawned = 0;
	public int mobsDestroyed = 0;
	public int portalHealth = 10;

	public bool lose;

    private bool win;
    public bool Win { get { return win; } }


    public AudioClip soundWinSong;
    public float volSoundWinSong;
    public AudioClip soundWinShout;
    public float volSoundWinShout;
    public AudioClip soundLoseSong;
    public float volSoundLoseSong;
    private AudioSource source;
       

    //Awake is always called before any Start functions
    void Awake(){
        source = GetComponent<AudioSource>();
        win = false;
		lose = false;
		crossHair.SetActive (true); // TODO: disable on lobby

        //Check if instance already exists
        if (instance == null)

			//if not, set instance to this
			instance = this;

		//If instance already exists and it's not this:
		else if (instance != this)

			//Then destroy this. This enforces our singleton pattern, meaning there can only ever be one instance of a GameManager.
			Destroy(gameObject);    

		//Sets this to not be destroyed when reloading scene
//		DontDestroyOnLoad(gameObject);


		//Call the InitGame function to initialize the first level 
		InitGame();

		mobKilledText = mobKilledCanvas.GetComponent<Text> ();
		portalHealthText = portalHealthCanvas.GetComponent<Text> ();
		portalHealthText.text = portalHealth.ToString ();
	}

	//Initializes the game for each level.
	void InitGame(){


	}
		
	//Update is called every frame.
	void Update(){

	}

	public void countMobSpawned(){
		mobsSpawned++;
	}

	public void countMobKilled(){
		mobsKilled++;
		if (mobKilledText) {
			mobKilledText.text = mobsKilled.ToString();
		}
	}

	public void DamagePortal(int damage){
		portalHealth -= damage;
		if (portalHealth <= 0) {
			portalHealth = 0;
			if (!win && !lose) {
				LoseGame();
            }
		}

		if (portalHealthText) {
			portalHealthText.text = portalHealth.ToString();
		}
	}

	public void countMobDestroyed(){
		mobsDestroyed++;
	}

	public void WinGame(){
        win = true;
		winGameText.SetActive (true);
		crossHair.SetActive (false);
        source.Stop();
        source.PlayOneShot(soundWinSong, volSoundWinSong);
        source.PlayOneShot(soundWinShout, volSoundWinShout);
    }

	public void LoseGame(){
		lose = true;

		loseGameText.SetActive (true);
		crossHair.SetActive (false);
        source.Stop();
        source.PlayOneShot(soundLoseSong, volSoundLoseSong);
	}
}
	
