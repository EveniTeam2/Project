using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using GoogleMobileAds.Api;
using UnityEditor;
using UnityEditor.PackageManager.Requests;

public class AdsManager : MonoBehaviour
{
    private RewardedAd RewardedAd;
    
    // Start is called before the first frame update
    public event Action OnInitCallback;
    public event Action OnShowCallback;
    public event Action<Reward> OnAdsCompleteCallback;


    private void Start()
    {
        Init();
    }

    public void Init()
    {
        string adUnitId;
        //MobileAds.Initialize(initStatus => { });
        
        
#if UNITY_ANDROID
        adUnitId = "ca-app-pub-3940256099942544/5224354917";
#elif UNITY_IPHONE
        adUnitId = "ca-app-pub-3940256099942544/1712485313";
#else
        adUnitId = "unexpected_platform";
#endif
        RewardedAd.Load(adUnitId,  new AdRequest(), InitCallback);
    }

    private void InitCallback(RewardedAd arg1, LoadAdError arg2)
    {
        if (arg1 != null)
        {
            RewardedAd = arg1;
            OnInitCallback?.Invoke();
        }
        else
        {
            Debug.Log(arg2.GetMessage());
        }
    }
    
    public void ShowAds()
    {
        if (RewardedAd.CanShowAd())
        {
            RewardedAd.Show(GetReward);
            OnShowCallback?.Invoke();
        }
        else
        {
            Debug.Log("대기 필요");
        }
    }

    private void GetReward(Reward reward)
    {
        OnAdsCompleteCallback?.Invoke(reward);
        OnAdsCompleteCallback = null;
    }
}
