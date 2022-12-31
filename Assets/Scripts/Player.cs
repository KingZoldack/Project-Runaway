using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(Collider2D))]
[RequireComponent(typeof(Animator), typeof(SpriteRenderer))]
public class Player : MonoBehaviour
{
    SpriteRenderer _spriteRenderer;
    Animator _animator;

    [Header("Game Objects")]
    [Tooltip("Tags game objeect")]
    [SerializeField]
    Tags _tags;
    [Tooltip("Trail particles game objeect")]
    [SerializeField]
    GameObject _playerTrailParticles;

    [Header("Positions and Offsets")]
    [Tooltip("Y position of player not flipped.")]
    [SerializeField]
    float _notFlippedPlayerPosY = 0.65f;
    [Tooltip("Y position of player flipped.")]
    [SerializeField]
    float _flippedPlayerPosY = -1.9f;
    [Tooltip("Y position of the player's particle trail not flipped.")]
    [SerializeField]
    float _notFlippedParticlePosY = -2.8f;
    [Tooltip("Y position of the player's particle trail flipped.")]
    [SerializeField]
    float _flippedParticlePosY = 1.536f;

    float _playerPosY, _particlePosY;
    bool _isFlipped, _canClick = false;

    //Double-tap variables
    float _firstTapTime;
    float _tapDelay = 0.5f;
    int _timesTapped = 0;
    bool _canCheckTap = true;

    void Awake()
    {
        if (_animator == null)
            _animator = GetComponent<Animator>();

        if (_spriteRenderer == null)
            _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Start()
    {
        _playerPosY = transform.position.y;
        _particlePosY = _playerTrailParticles.transform.position.y;
    }


    void Update()
    {
        HandleDoubleTap();
        HandleRunParticles();
    }

    void HandleDoubleTap()
    {
        if (Input.GetMouseButtonDown(0) && !AudioManager.Instance.IsPaused())
        {
            _timesTapped += 1;
        }
        if (_timesTapped == 1 && _canCheckTap)
        {
            _firstTapTime = Time.time;
            StartCoroutine(DetectDoubleTapRoutine());
        }
    }

    IEnumerator DetectDoubleTapRoutine()
    {
        _canCheckTap = false;

        while (Time.time < _firstTapTime + _tapDelay)
        {
            if (_timesTapped == 2)
            {
                FlipPlayer();
                break;
            }
            yield return new WaitForEndOfFrame();
        }
        _timesTapped = 0;
        _canCheckTap = true;
    }

    void FlipPlayer()
    {
        if (!GameManager.Instance.isGameOver())
        {
            if (_canClick)
            {
                _isFlipped = !_isFlipped;
                AudioManager.Instance.PlaySwishSound();

                if (!_isFlipped)
                {
                    _playerPosY = _notFlippedPlayerPosY;
                    _particlePosY = _notFlippedParticlePosY;
                    _spriteRenderer.flipY = false;
                }
                else if (_isFlipped)
                {
                    _playerPosY = _flippedPlayerPosY;
                    _particlePosY = _flippedParticlePosY;
                    _spriteRenderer.flipY = true;
                }

                //Handles player's and trail's position after flip
                Vector3 _playerTrailParticlePos = new Vector3(transform.position.x, _particlePosY, transform.position.z);
                Vector3 _playerPosition = new Vector3(transform.position.x, _playerPosY, transform.position.z);

                _playerTrailParticles.transform.position = _playerTrailParticlePos;
                transform.position = _playerPosition;

                GameManager.Instance.UpdateScore();
            }
        }
    }

    void HandleRunParticles()
    {
        if (!GameManager.Instance.isGameOver())
        {
            if (!_playerTrailParticles.activeInHierarchy)
                _playerTrailParticles.SetActive(true);
        }
        else if (GameManager.Instance.isGameOver())
        {
            if (_playerTrailParticles.activeInHierarchy)
                _playerTrailParticles.SetActive(false);
        }
    }

    public void CantClick() => _canClick = false;

    public void CanClick() => _canClick= true;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == _tags.EnemyTag)
        {
            Destroy(collision.gameObject);
            _animator.SetBool(_tags.HurtAnimation, true);
            StartCoroutine(StopHurtAnimationRoutine());
            GameManager.Instance.UpdateLives();
            GameManager.Instance.Shake();
            AudioManager.Instance.PlayHurtSound();
        }
    }

    IEnumerator StopHurtAnimationRoutine()
    {
        yield return new WaitForSeconds(0.5f);
        _animator.SetBool(_tags.HurtAnimation, false);
    }
}
