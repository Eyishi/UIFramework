using System.Collections.Generic;
using UnityEngine;

namespace UIFramework
{
    
    /// <summary>
    /// ’辅助‘ 层级，以便显示优先级更高的窗口
    /// 默认情况下，包含任何标记为弹出窗口的窗口，它由 WindowUILayer 控制   可以是提示小弹窗
    /// </summary>
    public class WindowParaLayer : MonoBehaviour
    {
        [SerializeField] private GameObject darknBgObject;

        private List<GameObject> containedScreens = new List<GameObject>();

        public void AddScreen(Transform screenRectTranform)
        {
            screenRectTranform.SetParent(transform,false);
            containedScreens.Add(screenRectTranform.gameObject);
        }

        public void RefreshDarken()
        {
            for (int i = 0; i < containedScreens.Count; i++)
            {
                if (containedScreens[i] !=null)
                {
                    if (containedScreens[i].activeSelf)
                    {
                        darknBgObject.SetActive(true);
                        return;
                    }
                }
            }
            darknBgObject.SetActive(false);
        }

        public void DarkenBG()
        {
            darknBgObject.SetActive(true);
            darknBgObject.transform.SetAsLastSibling();
        }
    }
}