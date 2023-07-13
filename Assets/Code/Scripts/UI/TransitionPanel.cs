using System;
using DG.Tweening;
using UnityEngine;

public class TransitionPanel : MonoBehaviour
{
    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private float duration;

    public void Transient(Action onComplete = null)
    {
        canvasGroup.DOFade(1, duration).OnComplete(() =>
            {
                onComplete?.Invoke();
                canvasGroup.DOFade(0, 0).SetUpdate(true);
            })
            .SetUpdate(true);
    }
}