

namespace UIFramework
{
    /// <summary>
    /// 窗口记录和队列
    /// </summary>
    public struct WindowHistoryEntry
    {
        public readonly IWindowController Screen;
        public readonly IWindowProperties Properties;

        public WindowHistoryEntry(IWindowController screen, IWindowProperties properties)
        {
            Screen = screen;
            Properties = properties;
        }

        /// <summary>
        /// 显示界面
        /// </summary>
        public void Show()
        {
            Screen.Show(Properties);
        }
    }
}