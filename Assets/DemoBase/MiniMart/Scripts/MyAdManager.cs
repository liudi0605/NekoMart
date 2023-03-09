using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;

public class MyAdManager : MonoBehaviour
{

    [HideInInspector]
    public static MyAdManager Instance;

    private void Awake()
    {
        if (Instance != null)
            Destroy(gameObject);
        else
            Instance = this;

        DontDestroyOnLoad(gameObject);
    }

    public void Start()
    {

     
    }

   

    private void RequestInterstitial()
    {
       
    }
    public void ShowInterstitialAd()
    {
    }

    public void ShowRewardVideo()
    {
    }

    private void RequestRewardBasedVideo()
    {
       
    }

}



