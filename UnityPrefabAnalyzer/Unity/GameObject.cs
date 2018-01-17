using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrefabAnalyzer
{
    [UnityAttribute("--- !u!1 &(.+)")]
    public class GameObject : UnityObject
    {
        public GameObject()
        {
            ObjType = enmObjectType.GameObject;
        }

        [UnityAttribute("m_ObjectHideFlags: (.+)")]
        public bool hideFlag { get; set; }
        [UnityAttribute("m_PrefabParentObject: {fileID: (.+)}")]
        public ulong prefabParentObjectId { get; set; }
        [UnityAttribute("m_PrefabInternal: {fileID: (.+)}")]
        public ulong prefabInternalId { get; set; }
        [UnityAttribute("m_Name: (.+)")]
        public override string MName { get; set; }
        [UnityAttribute("m_IsActive: (.+)")]
        public override bool MIsActive { get; set; }
    }
}
