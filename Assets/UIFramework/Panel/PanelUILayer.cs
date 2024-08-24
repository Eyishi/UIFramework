using UIFramework.Core;
using UnityEngine;

namespace UIFramework.Panel
{
    public class PanelUILayer : UILayer<IPanelController>
    {
        [SerializeField] [Tooltip("优先级并行层的设置，注册到此的面板将根据和器优先级重新归属到不同的并行层对象。")]
        private PanelPriorityLayerList priorityLayers = null;
        public override void ShowScreen(IPanelController screen)
        {
            screen.Show();
        }

        public override void ShowScreen<Tprops>(IPanelController screen, Tprops properties)
        {
            screen.Show(properties);
        }

        public override void HideScreen(IPanelController screen)
        {
            screen.Hide();
        }

        /// <summary>
        /// 设置屏  这个屏幕的父节点
        /// </summary>
        /// <param name="controller"></param>
        /// <param name="screenTransform"></param>
        public override void ReparentScreen(IScreenController controller, Transform screenTransform)
        {
            var ctl = controller as IPanelController;
            if (ctl !=null)
            {
                ReparentToParaLayer(ctl.Priority,screenTransform);
            }
            else
            {
                base.ReparentScreen(controller, screenTransform);
            }
        }
        /// <summary>
        /// 把面板放到不同的 层级下
        /// </summary>
        /// <param name="priority">这个面板的层级</param>
        /// <param name="screenTransform">你要放置的面板</param>
        /// <exception cref="NotImplementedException"></exception>
        private void ReparentToParaLayer(PanelPriority priority, Transform screenTransform)
        {
            Transform trans;
            if (!priorityLayers.ParaLayerLookup.TryGetValue(priority, out trans)) ;
            {
                trans = transform;
            }
            screenTransform.SetParent(trans,false);
        }
        
    }
}