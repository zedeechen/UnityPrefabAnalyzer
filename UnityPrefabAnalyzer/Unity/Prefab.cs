using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrefabAnalyzer
{
    [UnityAttribute("--- !u!1001 &(.+)")]
    public class Prefab : UnityObject
    {
        public Prefab()
        {
            ObjType = enmObjectType.Prefab;
        }

        public GameObject gameObject;
                
        [UnityAttribute("m_ObjectHideFlags: (.+)")]
        public bool hideFlag { get; set; }
        [UnityAttribute("m_ParentPrefab: {fileID: (.+)}")]
        public ulong parentPrefabId { get; set; }
        [UnityAttribute("m_RootGameObject: {fileID: (.+)}")]
        public ulong rootGameObjectId { get; set; }
        [UnityAttribute("m_IsPrefabParent: (.+)")]
        public bool isPrefabParent { get; set; }

    }
}
