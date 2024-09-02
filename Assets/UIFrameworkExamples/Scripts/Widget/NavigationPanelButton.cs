using System;
using UnityEngine;
using UnityEngine.UI;


namespace UIFramework.Examples
{
    /// <summary>
    /// 组件
    /// </summary>
    [RequireComponent(typeof(Button))]
    public class NavigationPanelButton :MonoBehaviour
    {
        [SerializeField] 
        private Text buttonLabel = null;

        [SerializeField] private Image icon = null;

        public Action<NavigationPanelButton> ButtonClicked;

        private NavigationPanelEntry navigationData = null;
        private Button _button = null;

        private Button button
        {
            get
            {
                if (_button == null)
                {
                    _button = GetComponent<Button>();
                }
                return _button;
            }
        }
        /// <summary>
        /// 目标跳转的界面
        /// </summary>
        /// <returns>跳转界面的id</returns>
        public string Target {
            get { return navigationData.TargetScreen; }
        }

        public void SetData(NavigationPanelEntry target)
        {
            navigationData = target;
            buttonLabel.text = target.ButtonText;
            icon.sprite = target.Sprite;
        }
        /// <summary>
        /// 是否可以选中当前按钮
        /// </summary>
        /// <param name="selectedButton"></param>
        public void SetCurrentNavigationTarget(NavigationPanelButton selectedButton)
        {
            button.interactable = selectedButton != this;
        }
        public void SetCurrentNavigationTarget(string screenId) {
            if (navigationData != null) {
                button.interactable = navigationData.TargetScreen == screenId;
            }
        }
        public void UI_Click()
        {
            if (ButtonClicked != null)
            {
                ButtonClicked(this);
            }
        }
    }
}