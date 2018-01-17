using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrefabAnalyzer
{
    public abstract class Component : UnityObject
    {
        public GameObject gameObject;
    }

    [UnityAttribute("--- !u!224 &(.+)")]
    public class RectTransform : Component
    {
        public RectTransform()
        {
            ObjType = enmObjectType.RectTransform;
        }

        [UnityAttribute("m_PrefabParentObject: {fileID: (.+)}")]
        public ulong prefabParentObjectId { get; set; }
        [UnityAttribute("m_GameObject: {fileID: (.+)}")]
        public ulong gameObjectId { get; set; }
        [UnityAttribute("m_Father: {fileID: (.+)}")]
        public ulong parentTransformId { get; set; }
    }

    [UnityAttribute("--- !u!114 &(.+)")]
    public class MonoBehaviour : Component
    {
        public MonoBehaviour()
        {
            ObjType = enmObjectType.MonoBehaviour;
        }

        public void GetDeclarationCode(ref StringBuilder sb)
        {
            sb.AppendLine(string.Format("\t\t{0} = nil,", MName));
        }
        public virtual void GetFillDataCode(ref StringBuilder sb) { }
        public virtual void GetInitCode(ref StringBuilder sb, ref List<string> sbFunctionDeclare) { }
        public virtual void GetDisposeCode(ref StringBuilder sb) { }

        [UnityAttribute("m_Enabled: (.+)")]
        public override bool MIsActive { get; set; }

        [UnityAttribute("m_OnClick:")]
        public bool IsButton { get; set; }
        [UnityAttribute("m_fontMaterials: *")]
        public bool IsText { get; set; }
        [UnityAttribute("onValueChanged:")]
        public bool IsToggle { get; set; }
        [UnityAttribute("m_Sprite:*")]
        public bool IsImage { get; set; }
        [UnityAttribute("m_ScrollSensitivity:*")]
        public bool IsScrollBar { get; set; }
    }

    public class Text : MonoBehaviour
    {
        public override void GetFillDataCode(ref StringBuilder sb)
        {
            sb.AppendLine(string.Format("\t\t--self.{0}.text = nil;--TODO:set value to text", MName));
        }

        public override void GetInitCode(ref StringBuilder sb, ref List<string> sbFunctionDeclare)
        {
            sb.AppendLine(string.Format("\t\tself.{0} = water.getUIComponentWithNameAndType(\"{0}\",TextMeshProUGUI.GetClassType(), self.transform)", MName));
        }

        public override void GetDisposeCode(ref StringBuilder sb)
        {
            return;
        }
    }

    public class Button : MonoBehaviour
    {
        public override void GetDisposeCode(ref StringBuilder sb)
        {
            sb.AppendLine(string.Format("\t\tself.{0}.onClick:RemoveListener(self.OnClick_{0})", MName));
        }

        public override void GetFillDataCode(ref StringBuilder sb)
        {
            return;
        }

        public override void GetInitCode(ref StringBuilder sb, ref List<string> sbFunctionDeclare)
        {
            sb.AppendLine(string.Format("\t\tself.{0} = water.getUIComponentWithNameAndType(\"{0}\",WaterButton.GetClassType(), self.transform)", MName));
            sb.AppendLine(string.Format("\t\tself.{0}.onClick:AddListener(self.OnClick_{0})", MName));
            
            sbFunctionDeclare.Add(string.Format("OnClick_{0}", MName));
        }
    }
    public class Image : MonoBehaviour
    {
        public override void GetFillDataCode(ref StringBuilder sb)
        {
            sb.AppendLine(
                string.Format("\t\tWater.ScriptUIManager.Instance:LoadSpriteWithOwner(function(sp) self.{0}.sprite = sp end,getIconPath(), self.gameObject, true) -- TODO:implement function getIconPath"
                , MName));
        }

        public override void GetInitCode(ref StringBuilder sb, ref List<string> sbFunctionDeclare)
        {
            sb.AppendLine(string.Format("\t\tself.{0} = water.getUIComponentWithNameAndType(\"{0}\", Image.GetClassType(), self.transform)", MName));
        }

        public override void GetDisposeCode(ref StringBuilder sb)
        {
            return;
        }
    }
    public class Toggle : MonoBehaviour
    {
        public override void GetFillDataCode(ref StringBuilder sb)
        {
            return;
        }

        public override void GetInitCode(ref StringBuilder sb, ref List<string> sbFunctionDeclare)
        {
            sb.AppendLine(string.Format("\t\tself.{0} = water.getUIComponentWithNameAndType(\"{0}\", Toggle.GetClassType(), self.transform)", MName));
            sb.AppendLine(string.Format("\t\tClickEventTrigger.Get(self.{0}.gameObject).onClick = self.OnClick_{0} --TODO:implement click function", MName));
        }

        public override void GetDisposeCode(ref StringBuilder sb)
        {
            sb.AppendLine(string.Format("\t\tClickEventTrigger.Get(self.{0}.gameObject).onClick = nil", MName));
        }
    }
    public class ScrollRect : MonoBehaviour
    {
        public override void GetDisposeCode(ref StringBuilder sb)
        {
            
        }

        public override void GetFillDataCode(ref StringBuilder sb)
        {
            
        }

        public override void GetInitCode(ref StringBuilder sb, ref List<string> sbFunctionDeclare)
        {
            
        }
    }
}
