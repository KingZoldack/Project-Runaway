using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(AudioSource))]
public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    [Header("Audio Source")]
    [Tooltip("Audio Source")]
    [SerializeField]
    AudioSource _audioSource;

    [Header("Game Audio")]
    [Tooltip("Background Music")]
    [SerializeField]
    AudioClip _mainMenuAudio;
    [Tooltip("Background Music")]
    [SerializeField]
    AudioClip _mainGameAudio;
    [Tooltip("In-game Sound FX")]
    [SerializeField]
    AudioClip _hurtAudio, _swishAudio;

    [Header("Mute Button Image Properties")]
    [Tooltip("Sprite for the mute button.")]
    [SerializeField]
    Sprite[] _muteButtonSprite; //0 = SoundOff sprite, 1 = SoundOn sprite
    int _soundOffSprite = 0, _soundOnSprite = 1;
    [Tooltip("Image field for the mute button.")]
    [SerializeField]
    Image[] _muteButtonImage;

    [Header("Audio Volume")]
    [Tooltip("Controls the volume level for the swish sound effect.")]
    [SerializeField]
    [Range(0, 1)]
    float _swishAudioVolume;
    [Tooltip("Controls the volume level for the hurt sound effect.")]
    [SerializeField]
    [Range(0, 1)]
    float _hurtAudioVolume;
    [Tooltip("Controls the volume level for the main menu background audio.")]
    [SerializeField]
    [Range(0, 1)]
    float _mainMenuAudioVolume = 0.035f;
    [Tooltip("Controls the volume level for the main game background audio.")]
    [SerializeField]
    [Range(0, 1)]
    float _mainGameAudioVolume = 0.25f;

    [Header("Audio Switch")]
    [Tooltip("Determins if the audio is turned on")]
    [SerializeField]
    bool _audioTurnedOn = true;
    bool _isPaused = false; //Game pause handled here not in GameManager


    void Awake()
    {
        if (Instance == null)
            Instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.Instance.IsMainMenu())
            HandleBGM(_mainMenuAudio);

        else if (!GameManager.Instance.IsMainMenu())
            HandleBGM(_mainGameAudio);
    }

    void HandleBGM(AudioClip sound)
    {
        if (_audioTurnedOn && !_audioSource.isPlaying && !_isPaused)
        {
            _audioSource.loop = true;

            if (_audioSource.volume <= 0)
                _audioSource.volume = 0;

            else
                _audioSource.volume = _mainMenuAudioVolume;

            _audioSource.clip = sound;
            _audioSource.Play();
        }
        else if (_audioTurnedOn && _audioSource.isPlaying && !_isPaused)
        {
            if (_audioSource.clip == null)
            {
                //Handle side effect (do nothing).
            }
            else if (_audioSource.clip.name == sound.name) //Side effect. Get NullReferenceException because AudioSource.clip = null when changing sounds
            {
                return;
            }
            else
            {
                _audioSource.loop = true;

                if (_audioSource.volume <= 0)
                    _audioSource.volume = 0;

                else
                    _audioSource.volume = _mainGameAudioVolume;

                _audioSource.clip = sound;
                _audioSource.Play();
            }
        }
        else if (!_audioTurnedOn && _audioSource.isPlaying)
        {
            _audioSource.Stop();
        }
    }
    public void PlayHurtSound() => _audioSource.PlayOneShot(_hurtAudio, _hurtAudioVolume);

    public void PlaySwishSound() => _audioSource.PlayOneShot(_swishAudio, _swishAudioVolume);

    public void PauseMusic()
    {
        if (_isPaused)
            _audioSource.Pause();
    }

    public void UnPauseMusic()
    {
        if (!_isPaused)
            _audioSource.UnPause();
    }

    public void PauseAudio() => _isPaused = true;

    public void ResumeAudio() => _isPaused = false;

    public bool IsPaused() => _isPaused;

    public void ToggleMute()
    {
        if (_audioSource.volume > 0)
        {
            _audioSource.volume = 0;

            foreach (var image in _muteButtonImage)
            {
                image.sprite = _muteButtonSprite[_soundOffSprite];
            }
        }
        else
        {
            if (_audioSource.clip == _mainMenuAudio)
            {
                _audioSource.volume = _mainMenuAudioVolume;
            }
            else if (_audioSource.clip == _mainGameAudio)
            {
                _audioSource.volume = _mainGameAudioVolume;
            }

            foreach (var image in _muteButtonImage)
            {
                image.sprite = _muteButtonSprite[_soundOnSprite];
            }
        }
    }

}
