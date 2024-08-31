using System;
using UIFramework.Panel;
using UIFramework.Window;

namespace UIFramework.Core
{
    /// <summary>
    /// 所有的UI界面必须 实现的接口，统一的风格
    /// </summary>
    public interface IScreenController
    {
        string ScreenId { get; set; } //界面id
        bool IsVisible { get;  }//是否在显示
        
        void Show(IScreenProperties props = null);//显示  
        
        void Hide(bool animate = true);

        //销毁界面时候
        Action<IScreenController> ScreenDestroyed { get; set; }
        //动画退出后完成
        Action<IScreenController> OutTransitionFinished { get; set; }
        //关闭界面
        Action<IScreenController> CloseRequest { get; set; }
        //动画进来后完成
        Action<IScreenController> InTransitionFinished { get; set; }
    }

    /// <summary>
    /// 面板的接口
    /// </summary>
    public interface IPanelController : IScreenController
    {
        PanelPriority Priority { get; }
    }

    /// <summary>
    /// 所有窗口的接口
    /// </summary>
    public interface IWindowController : IScreenController
    {
        /// <summary>
        /// 是否可以被直接打开的界面覆盖掉
        /// </summary>
        bool HideOnForegroundLost { get; }
        /// <summary>
        /// 是否显示在顶层
        /// </summary>
        bool IsPopup { get; }
        WindowPriority WindowPriority { get; }
    }
}