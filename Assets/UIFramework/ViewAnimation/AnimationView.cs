using System;
using System.Collections;
using UIFramework.ViewAnimation;
using UnityEngine;

namespace UIFramework.Examples
{
    /// <summary>
    /// 具体动画实现，自己封装DoTween之类的都行，这个随意。
    /// </summary>
    public class AnimationView : AniComponent
    {
        [SerializeField] private AnimationClip clip = null;
        [SerializeField] private bool playReverse = false;

        /// <summary>
        /// 执行完成  关闭之前
        /// </summary>
        private Action previousCallbackWhenFinished;
        
        public override void Animate(Transform target, Action callWhenFinished) {
            FinishPrevious();
            var targetAnimation = target.GetComponent<Animation>();
            if (targetAnimation == null) {
                Debug.LogError("[LegacyAnimationScreenTransition] No Animation component in " + target);
                if (callWhenFinished != null) {
                    callWhenFinished();
                }

                return;
            }

            targetAnimation.clip = clip;
            StartCoroutine(PlayAnimationRoutine(targetAnimation, callWhenFinished));
        }

        /// <summary>
        /// 协程执行动画
        /// </summary>
        /// <param name="targetAnimation"></param>
        /// <param name="callWhenFinished"></param>
        /// <returns></returns>
        private IEnumerator PlayAnimationRoutine(Animation targetAnimation, Action callWhenFinished) {
            previousCallbackWhenFinished = callWhenFinished;
            foreach (AnimationState state in targetAnimation) {
                state.time = playReverse ? state.clip.length : 0f;
                state.speed = playReverse ? -1f : 1f;
            }

            targetAnimation.Play(PlayMode.StopAll);
            yield return new WaitForSeconds(targetAnimation.clip.length);
            FinishPrevious();
        }
        
        /// <summary>
        /// 执行完成 关闭之前
        /// </summary>
        private void FinishPrevious() {
            if (previousCallbackWhenFinished != null) {
                previousCallbackWhenFinished();
                previousCallbackWhenFinished = null;
            }

            StopAllCoroutines();
        }
    }
}