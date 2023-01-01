using UnityEngine;

public class Tags : MonoBehaviour
{
    public static Tags Instance { get; private set; }

    [Header("Tags")]
    [Tooltip("Player tag reference.")]
    [SerializeField]
    string _playerTag = "Player";
    [Tooltip("Enemy tag reference.")]
    [SerializeField]
    string _enemyTag = "Enemy";

    [Header("Animation States")]
    [Tooltip("Player movement animation reference.")]
    [SerializeField]
    string _playerMovementAnimation = "player_movement_anim";
    [Tooltip("Player hurt animation reference.")]
    [SerializeField]
    string _playerHurtAnimation = "player_hurt_anim";
    [Tooltip("Player death animation reference")]
    [SerializeField]
    string _playerDeathAnimation = "player_death_anim";


    [Header("Ads String References")]
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
    public string PlayerMovementAnimation { get { return _playerMovementAnimation; } }
    public string PlayerHurtAnimation { get { return _playerHurtAnimation; } }
    public string PlayerDeathAnimation { get { return _playerDeathAnimation; } }
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
