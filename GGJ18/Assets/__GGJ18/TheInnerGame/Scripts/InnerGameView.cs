using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using TMPro;
using MEC;

public class InnerGameView : TeamTheDream.Singleton<InnerGameView> {

    [SerializeField]
    Image _overlay;

    [SerializeField]
    Image _logo;

    [SerializeField]
    TMP_Text _searchingMatchText;

    [SerializeField]
    TMP_Text _gameOverText;

    [SerializeField]
    Image _damageImage;

    [SerializeField]
    Image _girl;

    [SerializeField]
    Sprite _normalPose;

    [SerializeField]
    Sprite _losePoseGirl;

    [SerializeField]
    Sprite [] _takeDamageGirl;

    Tweener _damageTweener;
    CoroutineHandle? _damageCoroutine;

    private void Start()
    {
        InnerGameController.Instance.OnPlayerDie += ChangeImageGirlLose;
        InnerGameController.Instance.OnPlayerKillsEnemy += ChangeImageGirlTakeDamage;
    }

    void ChangeImageGirlLose(){

        _girl.sprite = _losePoseGirl;
        Timing.CallDelayed(0.4f, () => {
            _girl.sprite = _normalPose;
        });
    }

    void ChangeImageGirlTakeDamage(int number)
    {
        _girl.sprite = _takeDamageGirl[Random.Range(0,_takeDamageGirl.Length)];
        Timing.CallDelayed(0.4f, ()=>{
            _girl.sprite = _normalPose;
        });
    }

    // Use this for initialization
    public void Hide (TweenCallback onComplete) {
        Timing.CallDelayed(1f, () =>
        {
            _searchingMatchText.gameObject.SetActive(true);
            Timing.CallDelayed(5f, () =>
            {
                _searchingMatchText.gameObject.SetActive(false);
                _logo.DOFade(0, 2f).OnComplete(() => _overlay.DOFade(0, 1f).OnComplete(onComplete));
            });
        });
    }

    public void Show(TweenCallback onComplete)
    {
        _gameOverText.gameObject.SetActive(true);
        _overlay.DOFade(1f, 2f);
        _gameOverText.DOFade(1f, 2f).OnComplete(() =>
       {
           Timing.CallDelayed(2f, () =>
           {
               _gameOverText.DOFade(0, 2f).OnComplete(() =>
               {
                   _gameOverText.gameObject.SetActive(false);
                   _logo.DOFade(1f, 1f).OnComplete(onComplete);
               });
           });
       });
    }

    public void Damage()
    {
        if (_damageTweener != null)
        {
            _damageTweener.Kill();
        }
        if (_damageCoroutine.HasValue)
        {
            Timing.KillCoroutines(_damageCoroutine.Value);
        }

        _damageTweener = _damageImage.DOFade(1f, 0.1f).SetEase(Ease.Flash).OnComplete(() =>
        {
            _damageTweener = null;
            _damageCoroutine = Timing.CallDelayed(1f, () =>
            {
                _damageCoroutine = null;
                _damageTweener = _damageImage.DOFade(0, 2f).SetEase(Ease.InOutBounce).OnComplete(() =>
                {
                    _damageTweener = null;
                });
            });
        });
    }
}
