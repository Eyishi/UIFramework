﻿using System;
using System.Collections.Generic;
using UIFramework.Core;
using UnityEngine;

namespace UIFramework.Window
{
    /// <summary>
    /// 这个layer层控制所有的窗口
    /// 有显示记录和队列的，并且一次只显示一个
    /// </summary>
    public class WindowUILayer : UILayer<IWindowController>
    {
        [SerializeField] private WindowParaLayer priorityParaLayer = null;
        public IWindowController CurrentWindow { get; set; }
        
        private Queue<WindowHistoryEntry> windowQueue;
        private Stack<WindowHistoryEntry> windowHistory;

        public event Action RequestScreenBlock;
        public event Action RequestScreenUnblock;

        private HashSet<IScreenController> screensTransitioning;
        public override void ShowScreen(IWindowController screen)
        {
            throw new System.NotImplementedException();
        }

        public override void ShowScreen<Tprops>(IWindowController screen, Tprops properties)
        {
            throw new System.NotImplementedException();
        }

        public override void HideScreen(IWindowController screen)
        {
            throw new System.NotImplementedException();
        }

        public override void Initialize()
        {
            base.Initialize();
            registerScreen = new Dictionary<string, IWindowController>();
            
            windowQueue = new Queue<WindowHistoryEntry>();
            windowHistory = new Stack<WindowHistoryEntry>();
            screensTransitioning = new HashSet<IScreenController>();
        }
    }
}