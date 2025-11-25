using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class AdManager : MonoBehaviour
{
    public AdsInitializer adsInitializer;
    public InterstitialAd interstitialAd;
    [SerializeField] bool turnOffInterstitialAd = false;
    private bool firstAdShown = false;

    public RewardedAds rewardedAds;
    public HanoiRewardedAds hanoiRewardedAds;
    [SerializeField] bool turnOffRewardedAds = false;

    public BannerAd bannerAd;
    [SerializeField] bool turnOffBannerAd = false;

    public static AdManager Instance { get; private set; }


    private void Awake()
    {
        if (adsInitializer == null)
            adsInitializer = FindFirstObjectByType<AdsInitializer>();

        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        DontDestroyOnLoad(gameObject);

        adsInitializer.OnAdsInitialized += HandleAdsInitialized;
    }

    private void HandleAdsInitialized()
    {
        if (!turnOffInterstitialAd && interstitialAd != null)
        {
            interstitialAd.OnInterstitialAdReady += HandleInterstitialReady;
            interstitialAd.LoadAd();
        }

        if (!turnOffRewardedAds && rewardedAds != null)
        {
            rewardedAds.LoadAd();
        }

        if (!turnOffRewardedAds && hanoiRewardedAds != null)
        {
            hanoiRewardedAds.LoadAd();
        }

        if (!turnOffBannerAd && bannerAd != null)
        {
            bannerAd.LoadBanner();
        }
    }

    private void HandleInterstitialReady()
    {
        if (!firstAdShown)
        {
            Debug.Log("Showing first time interstitial ad automatically!");
            interstitialAd.ShowAd();
            firstAdShown = true;

        }
        else
        {
            Debug.Log("Next interstitial ad is ready for manual show!");
        }
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private bool firstSceneLoad = false;
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (interstitialAd == null)
            interstitialAd = FindFirstObjectByType<InterstitialAd>();

        GameObject interstitialButtonObj = GameObject.FindGameObjectWithTag("interstitialAdButton");
        if (interstitialButtonObj != null && interstitialAd != null)
        {
            Button interstitialButton = interstitialButtonObj.GetComponent<Button>();
            if (interstitialButton != null)
            {
                interstitialAd.SetButton(interstitialButton);

                if (scene.name == "City scene" || scene.name == "Tornis")
                {
                    interstitialButton.gameObject.SetActive(false);
                }
            }
        }


        if (rewardedAds == null)
            rewardedAds = FindFirstObjectByType<RewardedAds>();

        if (hanoiRewardedAds == null)
            hanoiRewardedAds = FindFirstObjectByType<HanoiRewardedAds>();

        if (bannerAd == null)
            bannerAd = FindFirstObjectByType<BannerAd>();

        GameObject rewardedAdButtonObj = GameObject.FindGameObjectWithTag("RewardedButton");
        if (rewardedAdButtonObj != null)
        {
            Button rewardedAdButton = rewardedAdButtonObj.GetComponent<Button>();
            if (rewardedAdButton != null)
            {
                if (hanoiRewardedAds != null)
                {
                    hanoiRewardedAds.SetButton(rewardedAdButton);
                }
                else if (rewardedAds != null)
                {
                    rewardedAds.SetButton(rewardedAdButton);
                }
            }
        }

        GameObject bannerButtonObj = GameObject.FindGameObjectWithTag("BannerButton");
        if (bannerButtonObj != null && bannerAd != null)
        {
            Button bannerButton = bannerButtonObj.GetComponent<Button>();
            if (bannerButton != null)
            {
                bannerAd.SetButton(bannerButton);
            }
        }

        if (!firstSceneLoad)
        {
            firstSceneLoad = true;
            Debug.Log("First time scene loaded!");
            return;
        }

        Debug.Log("Scene loaded!");

        if (!turnOffInterstitialAd && interstitialAd != null && interstitialAd.isReady)
        {
            Debug.Log("Showing interstitial ad on scene transition!");
            interstitialAd.ShowAd();
        }
        else if (!turnOffInterstitialAd && interstitialAd != null)
        {
            Debug.Log("Interstitial ad not ready, loading...");
            interstitialAd.LoadAd();
        }

        if (!turnOffRewardedAds)
        {
            if (hanoiRewardedAds != null)
            {
                Debug.Log("Loading Hanoi rewarded ad on scene load...");
                hanoiRewardedAds.LoadAd();
            }
            else if (rewardedAds != null)
            {
                Debug.Log("Loading rewarded ad on scene load...");
                rewardedAds.LoadAd();
            }
        }

        if (!turnOffBannerAd && bannerAd != null)
        {
            Debug.Log("Loading banner ad on scene load...");
            bannerAd.LoadBanner();
        }

    }
}