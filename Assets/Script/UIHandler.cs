using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using GooglePlayGames;

public class UIHandler : MonoBehaviour
{
	public GameObject MainMenu, GeneralMenu;
	public Image StartFader, EndFader;
	public GameObject GOPlay, GOPlayWithTxt;
	public Text cigarText;
	public Image soundBtn, pauseBtn;

	public Sprite soundOn, soundOff, resume, pause;
	public string androidRate, iOSRate;

	bool isPaused = false;

	// Use this for initialization
	void Start()
	{
		StartCoroutine("FadeOut");
		if(PlayerPrefs.GetInt("sound", 1) == 1)
		{
			soundBtn.sprite = soundOn;
			AudioListener.volume = 1;
		}
		else
		{
			soundBtn.sprite = soundOff;
			AudioListener.volume = 0;
		}

		if(PlayerPrefs.GetInt("cigar", 0) == 1)
		{
			GOPlay.SetActive(true);
			GOPlayWithTxt.SetActive(false);
			cigarText.text = "GAME OVER";
		}
	}

	public void OnPlayClick()
	{
		MainMenu.SetActive(false);
		GeneralMenu.SetActive(true);
		SoundManager.Instance.canPlay = true;
		SoundManager.Instance.PlayStartSound();
		SoundManager.Instance.gamesPlayedInSession++;
	}

	public void OnLeaderBoardClick()
	{
		SoundManager.Instance.PlayClickSound();
		if (Social.localUser.authenticated)
		{
			((PlayGamesPlatform)Social.Active).ShowLeaderboardUI ("CgkI8OXJmtsYEAIQBg");
		}
		else
		{
			Social.localUser.Authenticate ((bool success) =>
				{
					if (success) {
						Debug.Log ("Login Sucess");
						((PlayGamesPlatform)Social.Active).ShowLeaderboardUI ("CgkI8OXJmtsYEAIQBg");
					} else {
						Debug.Log ("Login failed");
					}
				});
		}
	}

	public void OnSoundClick()
	{
		SoundManager.Instance.PlayClickSound();
		if(PlayerPrefs.GetInt("sound", 1) == 1)
		{
			AudioListener.volume = 0;
			soundBtn.sprite = soundOff;
			PlayerPrefs.SetInt("sound", 0);
		}
		else
		{
			AudioListener.volume = 1;
			soundBtn.sprite = soundOn;
			PlayerPrefs.SetInt("sound", 1);
		}
	}

	public void OnRateClick()
	{
		SoundManager.Instance.PlayClickSound();
		#if UNITY_ANDROID
		Application.OpenURL(androidRate);
		#elif UNITY_IPHONE
		Application.OpenURL(iOSRate);
		#endif
	}

	public void OnShareClick()
	{
		SoundManager.Instance.PlayClickSound();
		SoundManager.Instance.gameObject.GetComponent<GeneralSharing>().OnShareSimpleText();
	}

	public void OnPauseClick()
	{
		SoundManager.Instance.PlayClickSound();
		if(!isPaused)
		{
			isPaused = true;
			Time.timeScale = 0;
			SoundManager.Instance.canPlay = false;
			pauseBtn.sprite = resume;
		}
		else
		{
			isPaused = false;
			Time.timeScale = 1;
			SoundManager.Instance.canPlay = true;
			pauseBtn.sprite = pause;
		}
	}

	public void OnGameOverPlayClick()
	{
		SoundManager.Instance.PlayClickSound();
		StartCoroutine("FadeIn");
		AdMobHandler.showAdmobInterstatial();
	}
		
	IEnumerator FadeOut()
	{
		while(StartFader.color.a > 0)
		{
			Color tempColor = new Color(StartFader.color.r , StartFader.color.g , StartFader.color.b , StartFader.color.a - 0.1f);
			StartFader.color = tempColor;

			yield return new WaitForSeconds(0.1f);
		}
	}

	IEnumerator FadeIn()
	{
		while(EndFader.color.a < 1)
		{
			Color tempColor = new Color(EndFader.color.r , EndFader.color.g , EndFader.color.b , EndFader.color.a + 0.1f);
			EndFader.color = tempColor;

			yield return new WaitForSeconds(0.1f);
		}
		Application.LoadLevel("GamePlay");
	}
}
