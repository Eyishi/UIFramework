using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UIFramework.Examples.Extras;
using UnityEngine;
using Utils;

namespace UIFramework.Examples
{
    public class ToastPanelController : PanelController
    {
        [SerializeField] private RectTransform toastRect = null;
        [SerializeField] private float toastDuration = 0.5f;
        [SerializeField] private float toastPause = 2f;
        [SerializeField] private Ease toastEase = Ease.Linear;//显示的方式

        private bool isToasting;//正在弹出

        protected override void AddListeners() {
            Signals.Get<PlayerDataUpdatedSignal>().AddListener(OnDataUpdated);
        }

        protected override void RemoveListeners() {
            Signals.Get<PlayerDataUpdatedSignal>().RemoveListener(OnDataUpdated);
        }

        private void OnDataUpdated(List<PlayerDataEntry> data) {
            if (isToasting) {
                return;
            }
    
            StartCoroutine(YieldForDOTween());
        }
        /// <summary>
        /// 弹窗弹出
        /// </summary>
        /// <returns></returns>
        private IEnumerator YieldForDOTween() {
            yield return null;
    
            isToasting = true;
            Sequence seq = DOTween.Sequence();
            
            //窗口移动
            seq.Append(toastRect.DOAnchorPosY(0f, toastDuration).SetEase(toastEase));
            seq.AppendInterval(toastPause);
            seq.Append(toastRect.DOAnchorPosY(toastRect.rect.height, toastDuration).SetEase(toastEase));
            seq.OnComplete(() => isToasting = false);

            seq.Play();
        }
    }
}