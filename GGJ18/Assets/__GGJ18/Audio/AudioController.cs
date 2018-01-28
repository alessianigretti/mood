using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class AudioController : TeamTheDream.Singleton<AudioController> {

    #region Private Fields

    protected bool _isMusicOn;
    protected bool _isSoundOn;

    protected bool _isPlayingMusic = false;
    protected bool _isPlayingAmbiental = false;

    protected AudioSource _musicSource;
    protected AudioSource _ambientalSource;
    protected AudioSource _fxSoundsSource;
    protected AudioSource _uiSoundsSource;

    [SerializeField]
    AudioClip _gunSound;

    [SerializeField]
    AudioClip _enemy0Sound;

    [SerializeField]
    AudioClip _enemy1Sound;

    [SerializeField]
    AudioClip _startGameMusic;

    [SerializeField]
    AudioClip _loseGameMusic;

    [SerializeField]
    AudioClip _musicGame;

    #endregion

    #region Public properties

    public bool IsMusicOn
    {
        get
        {
            return _isMusicOn;
        }

        set
        {
            _isMusicOn = value;

            if (_isMusicOn)
            {
                if (_isPlayingMusic)
                {
                    _musicSource.Play();
                }
                PlayerPrefs.SetInt("Music", 1);
            }
            else
            {
                _musicSource.Stop();
                PlayerPrefs.SetInt("Music", 0);
            }


        }
    }

    public bool IsSoundOn
    {
        get
        {
            return _isSoundOn;
        }

        set
        {
            _isSoundOn = value;

            if (_isSoundOn)
            {
                if (_isPlayingAmbiental)
                {
                    _ambientalSource.Play();
                }
                PlayerPrefs.SetInt("Sound", 1);
            }
            else
            {
                _ambientalSource.Stop();
                _uiSoundsSource.Stop();
                _fxSoundsSource.Stop();
                PlayerPrefs.SetInt("Sound", 0);
            }
        }
    }

    public bool IsPlayingMusic
    {
        get
        {
            return _isPlayingMusic;
        }
    }

    public bool IsPlayingAmbiental
    {
        get
        {
            return _isPlayingAmbiental;
        }
    }

    #endregion

    #region MonoBehaviour Methods

     void Awake()
    {
        _musicSource = gameObject.AddComponent<AudioSource>();
        _musicSource.loop = true;

        _ambientalSource = gameObject.AddComponent<AudioSource>();
        _ambientalSource.loop = true;

        _fxSoundsSource = gameObject.AddComponent<AudioSource>();
        _fxSoundsSource.loop = false;

        _uiSoundsSource = gameObject.AddComponent<AudioSource>();
        _uiSoundsSource = gameObject.AddComponent<AudioSource>();

        IsMusicOn = PlayerPrefs.GetInt("Music", 1) == 1;
        IsSoundOn = PlayerPrefs.GetInt("Sound", 1) == 1;
    }
    #endregion

    #region Public Methods

    public void SwitchSound()
    {
        IsSoundOn = !_isSoundOn;
    }

    public void SwitchMusic()
    {
        IsMusicOn = !_isMusicOn;
    }

    public void PlayMusic(AudioClip music)
    {
        _isPlayingMusic = true;

        if (_isMusicOn)
        {
            _musicSource.Stop();
            _musicSource.clip = music;
            _musicSource.Play();
        }
        else
        {
            _musicSource.clip = music;
            _musicSource.volume = 0.2f;
        }
        //TODO: transitions?
    }

    public void StopMusic()
    {
        _isPlayingMusic = false;
        _musicSource.Stop();
    }

    public void FadeMusic(float endVolume, float time, TweenCallback onComplete = null)
    {
        if (onComplete == null)
        {
            _musicSource.DOFade(endVolume, time);
        }
        else
        {
            _musicSource.DOFade(endVolume, time).OnComplete(onComplete);
        }
    }

    public void FadeMusic(float startVolume, float endVolume, float time, TweenCallback onComplete = null)
    {
        _musicSource.volume = startVolume;
        if (onComplete == null)
        {
            _musicSource.DOFade(endVolume, time);
        }
        else
        {
            _musicSource.DOFade(endVolume, time).OnComplete(onComplete);
        }
    }

    public void PlayAmbiental(AudioClip ambiental)
    {
        _isPlayingAmbiental = true;

        if (_isSoundOn)
        {
            _ambientalSource.Stop();
            _ambientalSource.clip = ambiental;
            _ambientalSource.Play();
        }
        else
        {
            _ambientalSource.clip = ambiental;
        }
        //TODO: transitions?
    }

    public void StopAmbiental()
    {
        _isPlayingAmbiental = false;
        _ambientalSource.Stop();
    }

    public void FadeAmbiental(float endVolume, float time, TweenCallback onComplete = null)
    {
        if (onComplete == null)
        {
            _ambientalSource.DOFade(endVolume, time);
        }
        else
        {
            _ambientalSource.DOFade(endVolume, time).OnComplete(onComplete);
        }
    }

    public void FadeAmbiental(float startVolume, float endVolume, float time, TweenCallback onComplete = null)
    {
        _ambientalSource.volume = startVolume;
        if (onComplete == null)
        {
            _ambientalSource.DOFade(endVolume, time);
        }
        else
        {
            _ambientalSource.DOFade(endVolume, time).OnComplete(onComplete);
        }
    }

    public void PlayFxSound(AudioClip sound)
    {
        if (_isSoundOn)
        {
            _fxSoundsSource.Stop();
            _fxSoundsSource.clip = sound;
            _fxSoundsSource.Play();
        }
    }

    public void PlayUiSound(AudioClip sound)
    {
        if (_isSoundOn)
        {
            _uiSoundsSource.Stop();
            _uiSoundsSource.clip = sound;
            _uiSoundsSource.Play();
        }
    }

    #endregion

    public void PlayGunSound    (){
        PlayFxSound(_gunSound);
    }

    public void PlayEnemy0Sound()
    {
        PlayFxSound(_enemy0Sound);
    }

    public void PlayEnemy1Sound()
    {
        PlayFxSound(_enemy1Sound);
    }

    public void PlayMusic(){
        PlayMusic(_musicGame);
    }

    public void PlayLoseMusic()
    {
        PlayMusic(_loseGameMusic);
    }
}