using System;
using UIFramework.Panel;
using UnityEngine;
using UnityEngine.UI;

namespace UIFramework
{
    /// <summary>
    /// 所有对外接口在这，相当于UIManager之类的
    /// </summary>
    public class UIFrame : MonoBehaviour
    {
        [Tooltip("手动初始化UI框架，设为false")] [SerializeField]
        private bool initializeOnAwake = true;

        private PanelUILayer panelLayer;

        private Canvas mainCanvas;
        private GraphicRaycaster graphicRaycaster;

        /// <summary>
        /// 主Canvas
        /// </summary>
        public Canvas MainCanvas
        {
            get
            {
                if (mainCanvas == null)
                {
                    mainCanvas = GetComponent<Canvas>();
                }

                return mainCanvas;
            }
        }
        /// <summary>
        /// 主Camera
        /// </summary>
        public Camera UICamera
        {
            get
            {
                return mainCanvas.worldCamera;
            }
        }

        private void Awake()
        {
            if (initializeOnAwake)
            {
                Initialize();
            }
        }

        /// <summary>
        /// 初始化
        /// </summary>
        private void Initialize()
        {
            if (panelLayer == null)
            {
                panelLayer = GetComponentInChildren<PanelUILayer>();
                if (panelLayer == null)
                {
                    Debug.LogError("[UI Frame ] UI Frame lacks Panel Layer!");
                }
                else
                {
                    panelLayer.Initialize();
                }

                graphicRaycaster = mainCanvas.GetComponent<GraphicRaycaster>();
            }
        }
    }
}