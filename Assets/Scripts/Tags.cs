using UnityEngine;

public class Tags : MonoBehaviour
{
    public static Tags Instance { get; private set; }

    [Header("Tags and Animation References:")]
    [Tooltip("Player tag reference.")]
    [SerializeField]
    string _playerTag = "Player";
    [Tooltip("Enemy tag reference.")]
    [SerializeField]
    string _enemyTag = "Enemy";
    [Tooltip("Main scene name reference.")]
    [SerializeField]
    string _mainScene = "Game";
    [Tooltip("Run animation boolean reference.")]
    [SerializeField]
    string _runAnimation = "hasGameStarted";
    [Tooltip("Hurt animation boolean reference.")]
    [SerializeField]
    string _hurtAnimation = "gotHurt";

    [Header("Ads String References:")]
    [Tooltip("Android ID reference.")]
    [SerializeField]
    string _androidGameID = "5094765";
    [Tooltip("Android Rewards Ad Unit ID reference.")]
    [SerializeField]
    string _androidRewardedAdUnitID = "Rewarded_Android";
    [Tooltip("iOS ID reference.")]
    [SerializeField]
    string _iosGameID = "5094764";
    [Tooltip("iOS Rewards Ad Unit ID reference.")]
    [SerializeField]
    string _iosRewardedAdUnitID = "Rewarded_iOS";

    //Properties
    public string PlayerTag { get { return _playerTag; } }
    public string EnemyTag { get { return _enemyTag; } }
    public string MainScene { get { return _mainScene; } }
    public string RunAnimation { get { return _runAnimation; } }
    public string HurtAnimation { get { return _hurtAnimation; } }
    public string AndroidGameID { get { return _androidGameID; } }
    public string IOSGameID { get { return _iosGameID; } }
    public string AndroidRewardedAdUnitID { get { return _androidRewardedAdUnitID; } }
    public string IOSRewardedAdUnitID { get { return _iosRewardedAdUnitID; } }

    void Awake()
    {
        if (Instance == null)
            Instance = this;
    }
}
