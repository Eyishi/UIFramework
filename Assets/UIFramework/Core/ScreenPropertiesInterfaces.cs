
namespace UIFramework
{
    /// <summary>
    /// 所有界面的属性接口
    /// </summary>
    public interface IScreenProperties
    {
        
    }
    /// <summary>
    /// 面板属性的接口
    /// </summary>
    public interface IPanelProperties : IScreenProperties
    {
        PanelPriority Priority { get; set; }
    }
    /// <summary>
    /// 窗口属性的接口
    /// </summary>
    public interface IWindowProperties : IScreenProperties
    {
        WindowPriority WindowQueuePriority { get; set; }
        bool HideOnForegroundLost { get; set; } //是直接把前面的窗口隐藏掉
        bool IsPopup { get; set; }//队列
        bool SuppressPrefabProperties { get; set; }
    }
}