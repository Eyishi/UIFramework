using System;
using UnityEngine;
using Utils;

namespace UIFramework.Examples
{
    public class InWindowSignal : ASignal<string> { }
    /// <summary>
    /// 这是一个内窗口
    /// </summary>
    public class InWindowController : WindowController
    {
        public void UI_Close()
        {
            Signals.Get<OutWindowSignal>().Dispatch(ScreenIds.InWindow);
        }
    }
}