using System;
using System.Collections.Generic;
using UIFramework.Examples.Extras;
using Utils;
using UnityEngine;

namespace UIFramework.Examples
{
    public class UIDemoController : MonoBehaviour
    {
        [SerializeField] private UISettings defaultUISettings = null;
        [SerializeField] private FakePlayerData fakePlayerData = null;
        [SerializeField] private Camera cam = null;
        /// <summary>
        /// 标签跟随的物品
        /// </summary>
        [SerializeField] private Transform transformToFollow = null;

        private UIFrame uiFrame;

        private void Awake() {
            uiFrame = defaultUISettings.CreateUIInstance();
            Signals.Get<StartDemoSignal>().AddListener(OnStartDemo);
            Signals.Get<NavigateToWindowSignal>().AddListener(OnNavigateToWindow);
            Signals.Get<ShowConfirmationPopupSignal>().AddListener(OnShowConfirmationPopup);
            Signals.Get<InWindowSignal>().AddListener(uiFrame.OpenWindow);
            
            Signals.Get<OutWindowSignal>().AddListener(uiFrame.CloseWindow);
        }

        private void OnDestroy() {
            Signals.Get<StartDemoSignal>().RemoveListener(OnStartDemo);
            Signals.Get<NavigateToWindowSignal>().RemoveListener(OnNavigateToWindow);
            Signals.Get<ShowConfirmationPopupSignal>().RemoveListener(OnShowConfirmationPopup);
            Signals.Get<InWindowSignal>().RemoveListener(uiFrame.OpenWindow);
            Signals.Get<OutWindowSignal>().RemoveListener(uiFrame.CloseWindow);

        }

        private void Start() {
            uiFrame.OpenWindow(ScreenIds.StartGameWindow);
        }

        private void OnStartDemo() {
            uiFrame.ShowPanel(ScreenIds.NavigationPanel);
            uiFrame.ShowPanel(ScreenIds.ToastPanel);
        }
        /// <summary>
        /// 打开导航上各种窗口的事件
        /// </summary>
        /// <param name="windowId"></param>
        private void OnNavigateToWindow(string windowId) {
            uiFrame.CloseCurrentWindow();

            switch (windowId) {
                case ScreenIds.PlayerWindow:
                    uiFrame.OpenWindow(windowId, new PlayerWindowProperties(fakePlayerData.LevelProgress));
                    break;
                case ScreenIds.CameraProjectionWindow:
                    transformToFollow.parent.gameObject.SetActive(true);
                    uiFrame.OpenWindow(windowId, new CameraProjectionWindowProperties(cam, transformToFollow));
                    break;
                default:
                    uiFrame.OpenWindow(windowId);
                    break;
            }
        }
        private void OnShowConfirmationPopup(ConfirmationPopupProperties popupPayload) {
            uiFrame.OpenWindow(ScreenIds.ConfirmationPopup, popupPayload);
        }
    }
}