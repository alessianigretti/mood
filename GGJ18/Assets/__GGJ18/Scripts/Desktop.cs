using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Desktop : MonoBehaviour {

    [SerializeField]
    CanvasGroup _canvasGroup;

	public void Open()
    {
        _canvasGroup.DOFade(0f, 1f).OnComplete(() =>
        {
            _canvasGroup.gameObject.SetActive(false);
            InnerGameController.Instance.InitializeGame();
        });
    }

    public void Close()
    {
        if (InnerGameController.Instance.IsActive) return;

        InnerGameController.Instance.Finish();
            ;
        _canvasGroup.gameObject.SetActive(true);
        _canvasGroup.DOFade(1f, 1f);
    }
}
