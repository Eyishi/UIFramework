using System.Collections;
using System.Collections.Generic;
using UIFramework.Core;
using UnityEngine;

namespace UIFramework.Core
{
    /// <summary>
    /// 基础的UI layer层  
    /// </summary>
    /// <typeparam name="TScreen"></typeparam>
    public abstract class UILayer<TScreen> :MonoBehaviour where TScreen : IScreenController
    {
        /// <summary>
        /// 注册的界面
        /// </summary>
        protected Dictionary<string, TScreen> registeredScreens;

        #region 抽象接口
        /// <summary>
        /// 显示界面
        /// </summary>
        /// <param name="screen"></param>
        public abstract void ShowScreen(TScreen screen);

        /// <summary>
        /// 显示界面
        /// </summary>
        /// <param name="screen">界面</param>
        /// <param name="properties">属性</param>
        /// <typeparam name="Tprops"></typeparam>
        public abstract void ShowScreen<Tprops>(TScreen screen,Tprops properties) where Tprops : IScreenProperties;

        /// <summary>
        /// 隐藏界面
        /// </summary>
        /// <param name="screen"></param>
        public abstract void HideScreen(TScreen screen);
        #endregion
        public void ShowScreenById(string screenId)
        {
            TScreen ctl;
            if (registeredScreens.TryGetValue(screenId,out ctl))
            {
                ShowScreen(ctl);
            }
            else
            {
                Debug.LogError("screen Id no register:" + screenId);
            }
        }

        public void ShowScreenById<Tprops>(string screenId, Tprops properties)
            where Tprops : IScreenProperties
        {
            TScreen ctl;
            if (registeredScreens.TryGetValue(screenId,out ctl))
            {
                ShowScreen(ctl,properties);
            }
            else
            {
                Debug.LogError("screen Id no register:" + screenId);
            }
        }

        /// <summary>
        /// 隐藏 通过id
        /// </summary>
        /// <param name="screenId"></param>
        public void HideScreenById(string screenId)
        {
            TScreen ctl;
            if (registeredScreens.TryGetValue(screenId,out ctl))
            {
                HideScreen(ctl);
            }
            else
            {
                Debug.LogError("screen Id no register:" + screenId);
            }
        }

        /// <summary>
        /// 关闭所有界面
        /// </summary>
        /// <param name="shouldAnimateWhenHiding"></param>
        public virtual void HideAll(bool shouldAnimateWhenHiding)
        {
            
        }
        /// <summary>
        /// 是否包含这个id
        /// </summary>
        /// <param name="screenid"></param>
        /// <returns></returns>
        public bool IsScreenRegistered(string screenid)
        {
            return registeredScreens.ContainsKey(screenid);
        }

        /// <summary>
        /// 初始化
        /// </summary>
        public virtual void Initialize()
        {
            registeredScreens = new Dictionary<string, TScreen>();
        }
        /// <summary>
        /// 传进来的界面当做层的子节点
        /// </summary>
        /// <param name="controller"></param>
        /// <param name="screenTransform"></param>
        public virtual void ReparentScreen(IScreenController controller, Transform screenTransform)
        {
            screenTransform.SetParent(transform,false);
        }
        /// <summary>
        /// 注册一个screen
        /// </summary>
        /// <param name="screenId"></param>
        /// <param name="controller"></param>
        public void RegisterScreen(string screenId, TScreen controller)
        {
            if (!registeredScreens.ContainsKey(screenId))
            {
                //注册 这个界面
                ProcessScreenRegister(screenId, controller);
            }
            else
            {
                Debug.LogError("screen controller alread register id:"+screenId);
            }
        }
        /// <summary>
        /// 注销这个界面
        /// </summary>
        /// <param name="screen"></param>
        /// <param name="screenScreenId"></param>
        public void UnregisterScreen( string screenId,TScreen screen)
        {
            if (registeredScreens.ContainsKey(screenId))
            {
                ProcessScreenUnRegister(screenId,screen);
            }
            else
            {
                Debug.LogError("Screen controller not register id:"+screenId);
            }
        }
        /// <summary>
        /// 注册一个界面
        /// </summary>
        /// <param name="screenId"></param>
        /// <param name="controller"></param>
        protected virtual void ProcessScreenRegister(string screenId, TScreen controller)
        {
            controller.ScreenId = screenId;
            registeredScreens.Add(screenId,controller);
            controller.ScreenDestroyed += OnScreenDestroyed;
        }
        /// <summary>
        /// 注销一个界面
        /// </summary>
        /// <param name="screenId"></param>
        /// <param name="controller"></param>
        protected virtual void ProcessScreenUnRegister(string screenId, TScreen controller)
        {
            controller.ScreenDestroyed -= OnScreenDestroyed;
            registeredScreens.Remove(screenId);
        }
        
        /// <summary>
        /// 销毁这个界面
        /// </summary>
        /// <param name="screen"></param>
        /// <exception cref="NotImplementedException"></exception>
        private void OnScreenDestroyed(IScreenController screen)
        {
            if (!string.IsNullOrEmpty(screen.ScreenId) && 
                registeredScreens.ContainsKey(screen.ScreenId))
            {
                UnregisterScreen(screen.ScreenId,(TScreen)screen);
            }
        }
        
    }
    
}

