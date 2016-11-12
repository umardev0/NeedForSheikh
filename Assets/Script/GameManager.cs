using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using GameAnalyticsSDK;

public class GameManager : MonoBehaviour
{
	public GameObject road;
	public GameObject[] hurdlesPrefabs;

	public GameObject GameOverPanel;
	public Text ScoreTxt;
	public GameObject withCigar, withoutCigar;

	int score = 0;
	float speed = 7.5f;
	float generateInterval = 3f;

	public static GameManager instance = null;

	List<GameObject> platformsList;
	float platformLength, platformHeight;

	ScrollingScript[] scrollScriptArr;

	void Awake()
	{
		if(instance == null)
		{
			instance = this;
			Time.timeScale = 1;
		}
	}

	// Use this for initialization
	void Start()
	{
		ScoreTxt.text = score.ToString();
		InvokeRepeating("GenerateHurdles", 0, generateInterval);

		scrollScriptArr = GameObject.FindObjectsOfType<ScrollingScript>();

		if(PlayerPrefs.GetInt("cigar", 0) == 1)
		{
			withCigar.SetActive(true);
			withoutCigar.SetActive(false);
		}
	}

	void GenerateHurdles()
	{
		if(!SoundManager.Instance.canPlay)
			return;

		int prob = Random.Range(1,100) % 5;
		if(prob<4)
		{
			int index = Random.Range(1, 100) % hurdlesPrefabs.Length;
			GameObject hurdle = (GameObject) Instantiate(hurdlesPrefabs[index]);
			hurdle.GetComponent<MoveScript>().speed = new Vector2(speed, speed);
		}
	}

	public void UpdateScore()
	{
		score += 10;
		ScoreTxt.text = score.ToString();
		if(score%50 == 0)
		{
			speed += 0.5f;
			road.GetComponent<ScrollingScript>().speed = new Vector2(speed, speed);
			if(generateInterval > 1.5f)
			{
				generateInterval -= 0.2f;
				CancelInvoke();
				InvokeRepeating("GenerateHurdles", 2, generateInterval);
			}
		}
	}

	public void GameOver()
	{
		SoundManager.Instance.PlayFailSound();
		SoundManager.Instance.canPlay = false;

		for(int i=0; i<scrollScriptArr.Length; i++)
		{
			scrollScriptArr[i].speed = Vector3.zero;
		}

		MoveScript[] ms = GameObject.FindObjectsOfType<MoveScript>();
		for(int i=0; i<ms.Length; i++)
		{
			ms[i].speed = Vector3.zero;
		}

		GameOverPanel.SetActive(true);

		if(score >= 1000)
		{
			PlayerPrefs.SetInt("cigar", 1);
		}

		if (Social.localUser.authenticated)
		{
			Social.ReportScore (score, "CgkI8OXJmtsYEAIQBg", (bool success) =>
				{
					if (success) {
						Debug.Log ("Update Score Success");

					} else {
						Debug.Log ("Update Score Fail");
					}
				});
		}
	}

	void OnApplicationQuit()
	{
		GameAnalytics.NewProgressionEvent(GameAnalyticsSDK.GAProgressionStatus.Complete, "GameOver",
			"Session No. - " + PlayerPrefs.GetInt("session", 1).ToString(),
			"Games in Session - " + SoundManager.Instance.gamesPlayedInSession.ToString());
	}
}
