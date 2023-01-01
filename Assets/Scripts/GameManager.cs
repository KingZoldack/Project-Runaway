using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("The Spaawn Manager")]
    [Tooltip("Spawn manager game object.")]
    [SerializeField]
    GameObject _spawnManager;

    [Header("UI Canvas Objects")]
    [Tooltip("Main menu UI game object.")]
    [SerializeField]
    GameObject _mainMenuUI;
    [Tooltip("Main game UI game object.")]
    [SerializeField]
    GameObject _mainGameUI;
    [Tooltip("Game over panel game object.")]
    [SerializeField]
    GameObject _gameOverPanel;

    [Header("Player Properties")]
    [Tooltip("Player game object.")]
    [SerializeField]
    Player _player;
    [Tooltip("Player animator.")]
    [SerializeField]
    Animator _playerAnimator;
    [Tooltip("Set the number of lives for the player.")]
    [SerializeField]
    int _playerLives = 2;

    [Header("UI Text")]
    [Tooltip("Lives text.")]
    [SerializeField]
    TextMeshProUGUI _livesText;
    [Tooltip("Score text.")]
    [SerializeField] 
    TextMeshProUGUI _scoreText;

    [Header("Pause/Play Button Image Properties")]
    [Tooltip("Sprite for the pause/play button.")]
    [SerializeField]
    Sprite[] _pausePlaySprite; //0 = Pause sprite, 1 = Play sprite
    int _pauseSprite = 0, _playSprite = 1;
    [Tooltip("Image field for the pause/play button.")]
    [SerializeField]
    Image _pausePlayButtonImage;

    [Header("Game States")]
    [Tooltip("Set if the game is over")]
    [SerializeField]
    bool _isGameOver = true;
    [Tooltip("Set if user is at the main menu.")]
    [SerializeField]
    bool _isMainMenu = true;
    bool _canSpawn = true;

    int _score = 0;
    int _randomScoreToAdd;

    Vector3 _originalCamPos;

    void Awake()
    {
        if (Instance == null)
            Instance = this;
    }

    void Start() => _originalCamPos = Camera.main.transform.position;

    public void StartGame()
    {
        _isGameOver = false;
        _isMainMenu = false;

        if (!_isGameOver)
        {
            _mainMenuUI.SetActive(false);
            _mainGameUI.SetActive(true);
            _spawnManager.SetActive(true);
            _player.CanClick();
            AdsManager.Instance.EnableAdsButton();
            AdsManager.Instance.ResetAdsCount();
        }   
    }

    void GameOver()
    {
        _isGameOver = true;
        _player.CantClick();
        _gameOverPanel.SetActive(true);
        AdsManager.Instance.DisableAdsButton();
        AnimatorManager.Instance.ChangeAnimationState(Tags.Instance.PlayerDeathAnimation);
    }

    public void RestartLevel()
    {
        StartGame();
        _canSpawn = true;
        _gameOverPanel.SetActive(false);
        _playerLives = 2;
        DisplayLives();
        _score = 0;
        DisplayScore();
    }

    public bool isGameOver() => _isGameOver;

    public void IncreaseLife()
    {
        _playerLives++;
        DisplayLives();
    }

    public void UpdateLives()
    {
        if (_playerLives <=  0)
            GameOver();

        else
            _playerLives--;

        DisplayLives();
    }

    void DisplayLives() => _livesText.text = "Extra Lives: " + _playerLives;

    public void UpdateScore()
    {
        _randomScoreToAdd = Random.Range(1, 11);
        _score += _randomScoreToAdd;

        if (_score >= 1000000000)
            _score = 1000000000;

        DisplayScore();
    }

    void DisplayScore() => _scoreText.text = "Score: " + _score;

    public void QuitGame() => Application.Quit();

    public void Shake() => StartCoroutine(CameraShakeRoutine());

    IEnumerator CameraShakeRoutine() 
    {
        for (int i = 0; i < 5; i++)
        {
            Vector2 randomPos = Random.insideUnitCircle * 0.5f;

            Camera.main.transform.position = new Vector3(randomPos.x, randomPos.y, _originalCamPos.z);

            yield return null;
        }

        Camera.main.transform.position = _originalCamPos;
    }

    public bool IsMainMenu() => _isMainMenu;

    public void TogglePause()
    {
        if (Time.timeScale > 0)
        {
            Time.timeScale = 0;
            AudioManager.Instance.PauseAudio();
            AudioManager.Instance.PauseMusic();
            AdsManager.Instance.DisableAdsButton();
            _pausePlayButtonImage.sprite = _pausePlaySprite[_playSprite];
        }
        else
        {
            Time.timeScale = 1;
            AudioManager.Instance.ResumeAudio();
            AudioManager.Instance.UnPauseMusic();
            AdsManager.Instance.EnableAdsButton();
            _pausePlayButtonImage.sprite = _pausePlaySprite[_pauseSprite];
        }
    }

    public bool CanSpawn() => _canSpawn;

    public void CantSpawn() => _canSpawn = false;
}
