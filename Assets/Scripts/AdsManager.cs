using UnityEngine;
using UnityEngine.Advertisements;
using UnityEngine.UI;

public class AdsManager : MonoBehaviour, IUnityAdsInitializationListener, IUnityAdsLoadListener, IUnityAdsShowListener
{
    public static AdsManager Instance { get; private set; }

    [SerializeField]
    Button _adButton;

    [SerializeField]
    bool _testMode = true;

    [Tooltip("Determins how many times users can watch ads.")]
    [SerializeField]
    int _adsShowCount = 3;

    string _adUnitID = null; // This will remain null for unsupported platforms
    string _gameID;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }

        InitializeAds();

        // Get the Ad Unit ID for the current platform:
#if UNITY_IOS
        _adUnitID = Tags.instance.IOSRewardedAdUnitID;
#elif UNITY_ANDROID
        _adUnitID = Tags.Instance.AndroidRewardedAdUnitID;
#endif
        //Disable the button until the ad is ready to show:
        _adButton.interactable = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (_adsShowCount <= 0)
            _adButton.interactable = false;
    }

    public void InitializeAds()
    {
        _gameID = (Application.platform == RuntimePlatform.IPhonePlayer)
            ? Tags.Instance.IOSGameID
            : Tags.Instance.AndroidGameID;
        Advertisement.Initialize(_gameID, _testMode, this);
    }

    // Load content to the Ad Unit:
    public void LoadAd()
    {
        // IMPORTANT! Only load content AFTER initialization
        Advertisement.Load(_adUnitID, this);
    }

    // If the ad successfully loads, add a listener to the button and enable it:
    public void OnUnityAdsAdLoaded(string adUnitID)
    {
        if (adUnitID.Equals(_adUnitID))
        {
            // Configure the button to call the ShowAd() method when clicked:
            //_adButton.onClick.AddListener(ShowRewardedAd); Grants multiple rewards keep disabled

            // Enable the button for users to click:
            _adButton.interactable = true;
        }
    }

    // Implement a method to execute when the user clicks the button:
    public void ShowRewardedAd()
    {
        // Disable the button:
        _adButton.interactable = false;
        // Then show the ad:
        Advertisement.Show(_adUnitID, this);

        _adsShowCount--;
        GameManager.Instance.TogglePause();
    }

    public void DisableAdsButton()
    {
        _adButton.interactable = false;
    }

    public void EnableAdsButton()
    {
        _adButton.interactable = true;
    }

    public void ResetAdsCount()
    {
        _adsShowCount = 3;
    }

    public void OnUnityAdsShowComplete(string adUnitID, UnityAdsShowCompletionState showCompletionState)
    {
        if (adUnitID.Equals(_adUnitID) && showCompletionState.Equals(UnityAdsShowCompletionState.COMPLETED))
        {
            // Grant a reward.
            GameManager.Instance.IncreaseLife();

            // Load another ad:
            Advertisement.Load(_adUnitID, this);
            GameManager.Instance.TogglePause();
        }
    }

    public void OnUnityAdsFailedToLoad(string adUnitID, UnityAdsLoadError error, string message)
    {
        // Use the error details to determine whether to try to load another ad.
        Debug.Log($"Error loading Ad Unit {adUnitID}: {error.ToString()} - {message}");
    }

    public void OnUnityAdsShowFailure(string adUnitID, UnityAdsShowError error, string message)
    {
        // Use the error details to determine whether to try to load another ad.
        Debug.Log($"Error showing Ad Unit {adUnitID}: {error.ToString()} - {message}");
    }

    public void OnUnityAdsShowStart(string placementId) { }

    public void OnUnityAdsShowClick(string placementId) { }

    void OnDestroy()
    {
        // Clean up the button listeners:
        _adButton.onClick.RemoveAllListeners();
    }

    public void OnInitializationComplete() { }

    public void OnInitializationFailed(UnityAdsInitializationError error, string message)
    {
        Debug.Log($"Unity Ads Initialization Failed: {error.ToString()} - {message}");
    }
}
