using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using GooglePlayGames;
using GoogleMobileAds;

public class SoundManager : MonoBehaviour
{

	public AudioClip jumpSound, clickSound;
	public AudioClip[] failSound;
	public AudioClip[] startSound;
	public AudioClip[] bgSounds;

	public bool canPlay = false;
	public int gamesPlayedInSession = 0;
	AudioSource audioS;

	int audioIndex = 0;

    private static SoundManager _instance;

    public static SoundManager Instance
    {
        get
        {
            if(_instance == null)
            {
                _instance = GameObject.FindObjectOfType<SoundManager>();

                //Tell unity not to destroy this object when loading a new scene!
                DontDestroyOnLoad(_instance.gameObject);
            }

            return _instance;
        }
    }

    void Awake() 
    {
        Debug.Log("Awake Called");
        if(_instance == null)
        {
            //If I am the first instance, make me the Singleton
            _instance = this;
            DontDestroyOnLoad(this);
        }
        else
        {
            //If a Singleton already exists and you find
            //another reference in scene, destroy it!
            if(this != _instance)
                Destroy(gameObject);
        }
    }

	void Start()
	{
		PlayerPrefs.SetInt("session", PlayerPrefs.GetInt("session", 0)+1);
		audioS = gameObject.GetComponent<AudioSource>();
		AdMobHandler.initialize(GameConstants.ADMOB_BANNER_ANDROID, GameConstants.ADMOB_INTERSTITIAL_ANDROID);
		AdMobHandler.showAdmobBanner();
		PlayGamesPlatform.Activate();
		Social.localUser.Authenticate ((bool success) =>
			{
				if (success) {
					Debug.Log ("Login Sucess");
				} else {
					Debug.Log ("Login failed");
				}
			});
	}


	void Update() 
	{
		if (!audioS.isPlaying) 
		{
			audioS.clip = bgSounds[audioIndex];
			audioS.Play();
			audioIndex++;
			if(audioIndex > 2)
				audioIndex = 0;
		}
	}

	public void PlayFailSound()
	{
		int rand = Random.Range(1,100)%failSound.Length;
		AudioSource.PlayClipAtPoint(failSound[rand] ,transform.position);
	}

	public void PlayStartSound()
	{
		int rand = Random.Range(1,100)%startSound.Length;
        AudioSource.PlayClipAtPoint(startSound[rand] ,transform.position);
	}

	public void PlayJumpSound()
	{
		AudioSource.PlayClipAtPoint(jumpSound ,transform.position);
	}

	public void PlayClickSound()
	{
		AudioSource.PlayClipAtPoint(clickSound ,transform.position);
	}
}
