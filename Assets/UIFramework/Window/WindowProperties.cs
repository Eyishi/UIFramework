using UIFramework.Core;
using UnityEngine;

namespace UIFramework
{
    /// <summary>
    /// 窗口的通用属性
    /// </summary>
    public class WindowProperties : IWindowProperties
    {
        [SerializeField] protected bool hideOnForegroundLost = true;//只显示自己

        [SerializeField] private WindowPriority windowQueuePriority = WindowPriority.ForceForeground; //队列的优先级
        
        [SerializeField] private bool isPopup = false;

        public WindowProperties()
        {
            hideOnForegroundLost = true;
            windowQueuePriority = WindowPriority.ForceForeground;
            isPopup = true;
        }
        
        
        /// <summary>
        /// 如果另一个窗口打开，此窗口如何表现？
        /// </summary>
        /// </value>  ForceForeground 会立即打开它。 Enqueue 会将他排队，以便在当前窗口关闭后立即打开
        public WindowPriority WindowQueuePriority
        {
            get {
                return windowQueuePriority;
            }
            set
            {
                windowQueuePriority = value;
            }
        }

        

        /// <summary>
        /// 其他窗口被置前的时候，自己是否隐藏
        /// </summary>
        public bool HideOnForegroundLost
        {
            get
            {
                return hideOnForegroundLost;
            }
            set
            {
                hideOnForegroundLost = value;
            }
        }
        /// <summary>
        /// 当在Open()调用中传递属性时，是否应该覆盖在viewPrefab中配置的属性
        /// </summary>
        public bool SuppressPrefabProperties { get; set; }

        /// <summary>
        /// 弹出窗口在它们后面显示一个黑色背景，并在所有其他窗口的前面显示0
        /// </summary>
        public bool IsPopup
        {
            get { return isPopup; }
            set { isPopup = value; }
        } 

        public WindowProperties(bool suppressPrefabProperties = false)
        {
            WindowQueuePriority = WindowPriority.ForceForeground;
            HideOnForegroundLost = false;
            SuppressPrefabProperties = suppressPrefabProperties;
        }

        public WindowProperties(WindowPriority priority,bool hideOnForegroundLost = false, bool suppressPrefabProperties=false)
        {
            WindowQueuePriority = priority;
            HideOnForegroundLost = hideOnForegroundLost;
            SuppressPrefabProperties = suppressPrefabProperties;
        }
    }
}