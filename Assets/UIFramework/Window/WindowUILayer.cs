using System;
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
        
        //队列和栈
        private Queue<WindowHistoryEntry> windowQueue;
        private Stack<WindowHistoryEntry> windowHistory;

        public event Action RequestScreenBlock;
        public event Action RequestScreenUnblock;

        /// <summary>
        /// 当前的界面
        /// </summary>
        private HashSet<IScreenController> screensTransitioning; 
        
        /// <summary>
        /// 是不是正在做动画
        /// </summary>
        private bool IsScreenTransitionInProgress {
            get { return screensTransitioning.Count != 0; }
        }
        
        public override void Initialize()
        {
            base.Initialize();
            registerScreen = new Dictionary<string, IWindowController>();
            
            windowQueue = new Queue<WindowHistoryEntry>();
            windowHistory = new Stack<WindowHistoryEntry>();
            screensTransitioning = new HashSet<IScreenController>();
        }

        /// <summary>
        /// 注册界面
        /// </summary>
        /// <param name="screenId"></param>
        /// <param name="controller"></param>
        protected override void ProcessScreenRegister(string screenId, IWindowController controller)
        {
            base.ProcessScreenRegister(screenId, controller);
            controller.InTransitionFinished += OnInAnimationFinished;
            controller.OutTransitionFinished += OnOutAnimationFinished;
            controller.CloseRequest += OnCloseRequestedByWindow;
        }
        /// <summary>
        /// 注销
        /// </summary>
        /// <param name="screenId"></param>
        /// <param name="controller"></param>
        protected override void ProcessScreenUnRegister(string screenId, IWindowController controller)
        {
            base.ProcessScreenUnRegister(screenId, controller);
            controller.InTransitionFinished -= OnInAnimationFinished;
            controller.OutTransitionFinished -= OnOutAnimationFinished;
            controller.CloseRequest -= OnCloseRequestedByWindow;
        }
        public override void ShowScreen(IWindowController screen)
        {
            ShowScreen<IWindowProperties>(screen,null);
        }

        public override void ShowScreen<Tprops>(IWindowController screen, Tprops properties)
        {
            IWindowProperties windowProp = properties as IWindowProperties;
            if (ShouldEnqueue(screen, windowProp))
            {
                EnqueueWindow(screen, windowProp);
            }
            else
            {
                DoShow(screen, windowProp);
            }
        }

        public override void HideScreen(IWindowController screen)
        {
            if (screen == CurrentWindow)
            {
                windowHistory.Pop();
                AddTransition(screen);
                screen.Hide();

                CurrentWindow = null;
                //展示下一个
                if (windowQueue.Count > 0)
                {
                    ShowNextInQueue();
                }
                //历史记录
                else if (windowHistory.Count > 0)
                {
                    ShowPreviousInHistory();
                }
            }
            else
            {
                Debug.LogError(string.Format(
                    "[WindowUILayer] Hide requested on WindowId {0} but that's not the currently open one ({1})! Ignore",
                    screen.ScreenId,CurrentWindow!=null? CurrentWindow:"current window is null"));
            }
        }
        

        /// <summary>
        /// 关闭所有
        /// </summary>
        /// <param name="shouldAnimateWhenHiding"></param>
        public override void HideAll(bool shouldAnimateWhenHiding = true)
        {
            base.HideAll(shouldAnimateWhenHiding);
            CurrentWindow = null;
            priorityParaLayer.RefreshDarken();
            windowHistory.Clear();
        }

        public override void ReparentScreen(IScreenController controller, Transform screenTransform)
        {
            IWindowController window = controller as IWindowController;
            if (window == null)
            {
                Debug.LogError("[WindowUILayer] Screen" + screenTransform.name + "is not a Window");
            }
            else
            {
                if (window.IsPopup)
                {
                    priorityParaLayer.AddScreen(screenTransform);
                    return;
                }
            }
            base.ReparentScreen(controller, screenTransform);
            
        }
        /// <summary>
        /// 入队
        /// </summary>
        /// <param name="screen">界面</param>
        /// <param name="properties">属性</param>
        /// <typeparam name="TProp">IWindowProperties</typeparam>
        private void EnqueueWindow<TProp>(IWindowController screen, TProp properties) where TProp : IScreenProperties
        {
            windowQueue.Enqueue(new WindowHistoryEntry(screen,(IWindowProperties)properties));
        }
        
        /// <summary>
        /// 是否应该进队列
        /// </summary>
        /// <param name="screen"></param>
        /// <param name="windowProp"></param>
        /// <returns></returns>
        private bool ShouldEnqueue(IWindowController screen, IWindowProperties windowProp)
        {
            //直接显示
            if (CurrentWindow == null && windowQueue.Count ==0)
            {
                return false;
            }
            //如果windowProp不是   ForceForeground
            if (windowProp !=null && windowProp.SuppressPrefabProperties)
            {
                return windowProp.WindowQueuePriority != WindowPriority.ForceForeground;
            }
            //
            if (screen.WindowPriority !=WindowPriority.ForceForeground)
            {
                return true;
            }

            return false;
        }
        /// <summary>
        /// 显示历史记录的界面
        /// </summary>
        private void ShowPreviousInHistory()
        {
            if (windowHistory.Count >0)
            {
                WindowHistoryEntry window = windowHistory.Pop();
                DoShow(window);
            }
        }
        /// <summary>
        /// 显示队列的下一个
        /// </summary>
        private void ShowNextInQueue()
        {
            if (windowQueue.Count >0)
            {
                WindowHistoryEntry window = windowQueue.Dequeue();
                DoShow(window);
            }
        }
        /// <summary>
        /// 显示界面
        /// </summary>
        /// <param name="screen"></param>
        /// <param name="properties"></param>
        private void DoShow(IWindowController screen, IWindowProperties properties) {
            DoShow(new WindowHistoryEntry(screen, properties));
        }

        private void DoShow(WindowHistoryEntry windowEntry) {
            if (CurrentWindow == windowEntry.Screen) {
                Debug.LogWarning(
                    string.Format(
                        "[WindowUILayer] The requested WindowId ({0}) is already open! This will add a duplicate to the " +
                        "history and might cause inconsistent behaviour. It is recommended that if you need to open the same" +
                        "screen multiple times (eg: when implementing a warning message pop-up), it closes itself upon the player input" +
                        "that triggers the continuation of the flow."
                        , CurrentWindow.ScreenId));
            }
            else if (CurrentWindow != null
                     && CurrentWindow.HideOnForegroundLost
                     && !windowEntry.Screen.IsPopup) {
                CurrentWindow.Hide();
            }

            windowHistory.Push(windowEntry);
            AddTransition(windowEntry.Screen);

            if (windowEntry.Screen.IsPopup) {
                priorityParaLayer.DarkenBG();
            }

            windowEntry.Show();

            CurrentWindow = windowEntry.Screen;
        }
        
        private void OnInAnimationFinished(IScreenController screen) {
            RemoveTransition(screen);
        }

        private void OnOutAnimationFinished(IScreenController screen) {
            RemoveTransition(screen);
            var window = screen as IWindowController;
            if (window.IsPopup) {
                priorityParaLayer.RefreshDarken();
            }
        }
        private void OnCloseRequestedByWindow(IScreenController screen) {
            HideScreen(screen as IWindowController);
        }
        /// <summary>
        /// 添加窗口界面
        /// </summary>
        /// <param name="screen"></param>
        private void AddTransition(IScreenController screen) {
            screensTransitioning.Add(screen);
            if (RequestScreenBlock != null) {
                RequestScreenBlock();
            }
        }
        /// <summary>
        /// 移除窗口界面
        /// </summary>
        /// <param name="screen"></param>
        private void RemoveTransition(IScreenController screen) {
            screensTransitioning.Remove(screen);
            if (!IsScreenTransitionInProgress) {
                if (RequestScreenUnblock != null) {
                    RequestScreenUnblock();
                }
            }
        }
        
    }
}