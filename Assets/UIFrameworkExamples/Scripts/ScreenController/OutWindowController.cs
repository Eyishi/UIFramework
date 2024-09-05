using Utils;

namespace UIFramework.Examples
{
    public class OutWindowSignal : ASignal<string> { }
    public class OutWindowController : WindowController
    {
        public void UI_Open()
        {
            Signals.Get<InWindowSignal>().Dispatch(ScreenIds.InWindow);
        }
    }
}