using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrefabAnalyzer
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Class, AllowMultiple = false)]
    public class UnityAttribute : Attribute
    {
        public string titlePattern { get; set; }
        public UnityAttribute(string pattern) { this.titlePattern = pattern; }
    }

    public enum enmObjectType
    {
        Prefab = 1,
        GameObject = 2,
        RectTransform = 3,
        MonoBehaviour = 4,
    }

    public abstract class UnityObject : IDisposable
    {
        public ulong guid;
        public enmObjectType ObjType { get; protected set; }
        
        public RectTransform mTransform;
        public List<UnityObject> mChildren;
        public List<MonoBehaviour> mComponents;

        public virtual string MName
        {
            get; set;
        }
        public virtual bool MIsActive
        {
            get; set;
        }

        private List<ulong> mComponentIdList;
        public IReadOnlyList<ulong> MComponentIdList { get { return mComponentIdList; } }
        public void AddComponentId(ulong id_)
        {
            if (mComponentIdList == null) mComponentIdList = new List<ulong>();

            if (mComponentIdList.Contains(id_))
                return;

            mComponentIdList.Add(id_);
        }

        public void Dispose()
        {
        }
    }
}
