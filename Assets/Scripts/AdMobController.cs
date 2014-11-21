using System;
using UnityEngine;
using System.Collections;

public class AdMobController : MonoBehaviour
{
    public static AdMobController Instance;

    private void Awake()
    {
        Instance = this;
    }

#if UNITY_ANDROID
    //    private static AdMobController s_Controller;
    //    private static AndroidJavaObject bannerAdsAndroidJavaObject;

    private static AndroidJavaObject screenAdsAndroidJavaObject;
    private static bool _adsLoaded;
    private static bool _adsShown;


    //    public GameObject AdsButton;

    void Start()
    {
        //s_Controller = this;

        StartCoroutine(ShowAds());
    }

    private IEnumerator ShowAds()
    {
        while (Application.isPlaying)
        {
            try
            {
                //                if(bannerAdsAndroidJavaObject == null) bannerAdsAndroidJavaObject = new AndroidJavaObject("com.bestidea.googleplayplugin.playads");
                if (screenAdsAndroidJavaObject == null || _adsShown)
                {
                    screenAdsAndroidJavaObject = new AndroidJavaObject("com.bestidea.googleplayplugin.FullScreenAds");
                    _adsShown = false;
                }
                if (screenAdsAndroidJavaObject != null) _adsLoaded = screenAdsAndroidJavaObject.Get<bool>("bIsInterstitialLoaded");
            }
            catch (Exception)// e)
            {
                //              Debug.LogError("Coroutine Exception: "+e.Message);
            }

            //            if(AdsButton != null) AdsButton.SetActive(_adsLoaded);
            yield return new WaitForSeconds(1); // пауза между попытками загрузить рекламу
        }
    }

    public void OnClick()
    {
        if (!_adsLoaded)
        {
            Debug.Log("Ads!");
            return;
        }

        try
        {
            screenAdsAndroidJavaObject.Call("displayInterstitial");
            _adsShown = true;
        }
        catch (Exception e)
        {
            Debug.LogError("displayInterstitial Exception: " + e.Message);
        }
    }

    //private float direction = 1;
    //private float delta = 0;

    //void OnGUI()
    //{
    //    if (delta > 500 || delta < -500) direction *= -1;
    //    delta += direction;
    //    var loaded = false;
    //    try
    //    {
    //        if(screenAdsAndroidJavaObject != null) loaded = screenAdsAndroidJavaObject.Get<bool>("bIsInterstitialLoaded");
    //    }
    //    catch (Exception e)
    //    {
    //        Debug.LogError("OnGUI Exception: " + e.Message);
    //    }
    //    if (!loaded)
    //    {
    //        GUI.Label(new Rect(Screen.width/2 - 25 + delta/10, Screen.height/2 - 25, 80, 50), "Реклама не загружена");
    //        return;
    //    }
    //    if (!GUI.Button(new Rect(Screen.width/2 - 55, Screen.height/2 - 55, 150, 100), "Показать рекламу")) return;
    //    try
    //    {
    //        screenAdsAndroidJavaObject.Call("displayInterstitial");
    //    }
    //    catch (Exception e)
    //    {
    //        Debug.LogError("displayInterstitial Exception: " + e.Message);
    //    }
    //}
#else
    public void OnClick()
    {
        Debug.Log("OnClick");
    }
#endif
}



