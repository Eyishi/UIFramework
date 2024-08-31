using System.Collections.Generic;
using UnityEngine;

namespace UIFramework.Panel
{
    /// <summary>
    /// 面板的层级，方便管理
    /// </summary>
    public enum PanelPriority
    {
        None=0,
        Prioritary = 1,
        Tutorial = 2,
        Blocker = 3,
    }

    /// <summary>
    /// 不同的面板的对应的优先级
    /// </summary>
    [System.Serializable]
    public class PanelPriorityLayerListEntry
    {
        [SerializeField]
        [Tooltip("执行下面板的优先级")]
        private PanelPriority priority;
        
        [SerializeField]
        [Tooltip("此优先级下所有面板的父节点")]
        private Transform targetParent;
        
        public PanelPriority Priority
        {
            get { return priority; }
            set { priority = value; }
        }
        public Transform TargetParent
        {
            get { return targetParent; }
            set { targetParent = value; }
        }
    }
    /// <summary>
    /// 管理所有entry
    /// </summary>
    [System.Serializable]
    public class PanelPriorityLayerList
    {
        [SerializeField] 
        [Tooltip("根据面板的优先级查找并存储对应的GameObject。渲染的优先级由这些GameObject在层级结构中的顺序决定")]
        private List<PanelPriorityLayerListEntry> paraLayer = null;

        public PanelPriorityLayerList(List<PanelPriorityLayerListEntry> entries)
        {
            this.paraLayer = entries;
        }

        /// <summary>
        /// 这个 优先级对应的父节点
        /// </summary>
        private Dictionary<PanelPriority, Transform> lookup;

        public Dictionary<PanelPriority, Transform> ParaLayerLookup
        {
            get
            {
                if (lookup == null || lookup.Count == 0)
                {
                    CacheLookup();
                }

                return lookup;
            }
        }

        /// <summary>
        /// 初始化
        /// </summary>
        private void CacheLookup()
        {
            lookup = new Dictionary<PanelPriority, Transform>();
            for (var i = 0; i < paraLayer.Count; i++)
            {
                lookup.Add(paraLayer[i].Priority,paraLayer[i].TargetParent);
            }
        }
    }
}