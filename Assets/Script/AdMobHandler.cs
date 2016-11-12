using UnityEngine;
using System.Collections;
using GoogleMobileAds.Api;

public class AdMobHandler {

	#if UNITY_IPHONE
	private static string ad_mob_banner_id;
	private static string ad_mob_interstatial_id;
	#endif
	#if UNITY_ANDROID
	private static string ad_mob_banner_id;
	private static string ad_mob_interstatial_id;
	#endif

	private BannerView bannerView = null;
	private InterstitialAd interstitial = null;
	static AdMobHandler m_instance = null;
	static bool m_isInitialize = false;

	private static AdMobHandler getInstance()
	{
		if (m_instance == null) {
			m_instance = new AdMobHandler();
		}
		return m_instance;
	}

	// Use this for initialization
	public static void initialize (string banner_id,string interstatial_id) 
	{

		ad_mob_banner_id = banner_id;
		ad_mob_interstatial_id = interstatial_id;

		if (m_isInitialize) {
			return;
		}
		m_isInitialize = true;
		if(ad_mob_banner_id != "")
		{
			getInstance().bannerView = new BannerView(ad_mob_banner_id, 
				AdSize.Banner, AdPosition.Bottom);

			AdRequest bannerRequest = new AdRequest.Builder().Build();
			getInstance().bannerView.LoadAd(bannerRequest);
			getInstance().bannerView.Hide ();
		}
		if(ad_mob_interstatial_id != "")
		{
			getInstance().interstitial = new InterstitialAd(ad_mob_interstatial_id);
			AdRequest interstitialRequest = new AdRequest.Builder().Build();
			getInstance().interstitial.LoadAd(interstitialRequest);
		}
	}

	public static bool isInterstatialCached() {
		if (getInstance().interstitial.IsLoaded()) {
			Debug.Log("Arslan::Admob::isCached: true");
			return true;
		}
		Debug.Log("Arslan::Admob::isCached: false");
		cacheAd ();
		return false;
	}

	public static bool showAdmobInterstatial() {
		if (isInterstatialCached ()) {
			Debug.Log("Arslan::Admob::showInterstatialAd: true");
			getInstance().interstitial.Show();
			cacheAd();
			return true;
		}
		Debug.Log("Arslan::Admob::showInterstatialAd: false");
		return false;
	}

	public static bool showAdmobBanner() {
		Debug.Log("Arslan::Admob::showAdmobBanner: true");
		getInstance().bannerView.Show();
		return true;
	}

	public static bool hideAdmobBanner() {
		Debug.Log("Arslan::Admob::hideAdmobBanner: true");
		getInstance().bannerView.Hide();
		return true;
	}

	private static void cacheAd()
	{
		getInstance().interstitial = new InterstitialAd(ad_mob_interstatial_id);
		AdRequest interstitialRequest = new AdRequest.Builder().Build();
		getInstance().interstitial.LoadAd(interstitialRequest);
	}
	

}
