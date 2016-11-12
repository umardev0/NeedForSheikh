﻿using UnityEngine;
using System.Collections;
using System.IO;
using System.Runtime.InteropServices;


public class GeneralSharing : MonoBehaviour
{
	#region PUBLIC_VARIABLES
	public Texture2D MyImage;
	#endregion
	
	#region UNITY_DEFAULT_CALLBACKS
	public void OnEnable ()
	{
		ScreenshotHandler.ScreenshotFinishedSaving += ScreenshotSaved;
	}
	
	void OnDisable ()
	{
		ScreenshotHandler.ScreenshotFinishedSaving -= ScreenshotSaved;
	}
	#endregion
	
	#region DELEGATE_EVENT_LISTENER
	void ScreenshotSaved ()
	{
		#if UNITY_IPHONE || UNITY_IPAD
		GeneralSharingiOSBridge.ShareTextWithImage (ScreenshotHandler.savedImagePath, "Kerb Ball");
		#endif
	}
	#endregion
	
	#region CO_ROUTINES
	IEnumerator ShareAndroidText ()
	{
		yield return new WaitForEndOfFrame ();
		#if UNITY_ANDROID

		//instantiate the class Intent
		AndroidJavaClass intentClass = new AndroidJavaClass("android.content.Intent");

		//instantiate the object Intent
		AndroidJavaObject intentObject = new AndroidJavaObject("android.content.Intent");

		//call setAction setting ACTION_SEND as parameter
		intentObject.Call<AndroidJavaObject>("setAction", intentClass.GetStatic<string>("ACTION_SEND"));

		intentObject.Call<AndroidJavaObject>("setType", "text/plain");
		intentObject.Call<AndroidJavaObject>("putExtra", intentClass.GetStatic<string>("EXTRA_SUBJECT"), GameConstants.SHARE_SUBJECT);
		intentObject.Call<AndroidJavaObject>("putExtra", intentClass.GetStatic<string>("EXTRA_TITLE"), GameConstants.SHARE_TITLE);
		intentObject.Call<AndroidJavaObject>("putExtra", intentClass.GetStatic<string>("EXTRA_TEXT"), GameConstants.SHARE_TEXT);

		//instantiate the class UnityPlayer
		AndroidJavaClass unity = new AndroidJavaClass("com.unity3d.player.UnityPlayer");

		//instantiate the object currentActivity
		AndroidJavaObject currentActivity = unity.GetStatic<AndroidJavaObject>("currentActivity");

		//call the activity with our Intent
		AndroidJavaObject jChooser = intentClass.CallStatic<AndroidJavaObject>("createChooser", intentObject, "Share Via");
		currentActivity.Call("startActivity", jChooser);
		
		#endif
	}
	
	IEnumerator SaveAndShare ()
	{
		yield return new WaitForEndOfFrame ();
		#if UNITY_ANDROID
		
		byte[] bytes = MyImage.EncodeToPNG();
		string path = Application.persistentDataPath + "/MyImage.png";
		File.WriteAllBytes(path, bytes);
		
		AndroidJavaClass intentClass = new AndroidJavaClass("android.content.Intent");
		AndroidJavaObject intentObject = new AndroidJavaObject("android.content.Intent");
		
		intentObject.Call<AndroidJavaObject>("setAction", intentClass.GetStatic<string>("ACTION_SEND"));
		intentObject.Call<AndroidJavaObject>("setType", "image/*");
		intentObject.Call<AndroidJavaObject>("putExtra", intentClass.GetStatic<string>("EXTRA_SUBJECT"), "Media Sharing ");
		intentObject.Call<AndroidJavaObject>("putExtra", intentClass.GetStatic<string>("EXTRA_TITLE"), "Media Sharing ");
		intentObject.Call<AndroidJavaObject>("putExtra", intentClass.GetStatic<string>("EXTRA_TEXT"), "Media Sharing Android Demo");
		
		AndroidJavaClass uriClass = new AndroidJavaClass("android.net.Uri");
		AndroidJavaClass fileClass = new AndroidJavaClass("java.io.File");
		
		AndroidJavaObject fileObject = new AndroidJavaObject("java.io.File", path);// Set Image Path Here
		
		AndroidJavaObject uriObject = uriClass.CallStatic<AndroidJavaObject>("fromFile", fileObject);
		
		//			string uriPath =  uriObject.Call<string>("getPath");
		bool fileExist = fileObject.Call<bool>("exists");
		Debug.Log("File exist : " + fileExist);
		if (fileExist)
			intentObject.Call<AndroidJavaObject>("putExtra", intentClass.GetStatic<string>("EXTRA_STREAM"), uriObject);
		
		AndroidJavaClass unity = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
		AndroidJavaObject currentActivity = unity.GetStatic<AndroidJavaObject>("currentActivity");
		currentActivity.Call("startActivity", intentObject);
		#endif
		
	}
	#endregion
	
	#region BUTTON_CLICK_LISTENER
	
	public void OnShareSimpleText ()
	{
		#if UNITY_ANDROID
		StartCoroutine (ShareAndroidText ());
		#elif UNITY_IPHONE || UNITY_IPAD
		GeneralSharingiOSBridge.ShareSimpleText ("Hello !!! \nThis is post from TheAppGuruz");
		#endif
	}
	
	public void OnShareTextWithImage ()
	{
		Debug.Log ("Media Share");
		#if UNITY_ANDROID
		StartCoroutine (SaveAndShare ());
		#elif UNITY_IPHONE || UNITY_IPAD
		byte[] bytes = MyImage.EncodeToPNG ();
		string path = Application.persistentDataPath + "/MyImage.png";
		File.WriteAllBytes (path, bytes);
		string path_ = "MyImage.png";
		
		StartCoroutine (ScreenshotHandler.Save (path_, "Media Share", true));
		#endif
	}
	#endregion
	
}
