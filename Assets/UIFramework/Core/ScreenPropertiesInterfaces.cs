using UIFramework.Window;

namespace UIFramework.Core
{
    /// <summary>
    /// 界面的属性接口
    /// </summary>
    public interface IScreenProperties
    {
        
    }

    /// <summary>
    /// 窗口属性的接口
    /// </summary>
    public interface IWindowProperties : IScreenProperties
    {
        WindowPriority windowQueuePriority { get; set; }
        bool HideOnForegroundLost { get; set; } //是直接把前面的窗口隐藏掉
        bool IsPopup { get; set; }//队列
        bool SuppressPrefabProperties { get; set; }
    }
}