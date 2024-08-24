using System;
using UIFramework.Panel;
using UIFramework.Window;

namespace UIFramework.Core
{
    /// <summary>
    /// 所有的UI界面必须 实现的接口 ，统一的风格
    /// </summary>
    public interface IScreenController
    {
        string ScreenId { get; set; } //界面id
        bool IsVisible { get; set; }//是否在显示中
        
        void Show(IScreenProperties props = null);//显示  
        
        void Hide(bool animate = true);

        Action<IScreenController> ScreenDestroyed { get; set; }
    }

    /// <summary>
    /// 所有面板的接口
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
        bool HideOnForegroundLost { get; }
        bool IsPopup { get; }
        WindowPriority WindowPriority { get; }
    }
}