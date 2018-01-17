using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrefabAnalyzer
{
    public class LuaUtil
    {
        public static string GenerateUILuaCode(string moduleName, List<MonoBehaviour> componentList)
        {
            StringBuilder sbFull = new StringBuilder();

            sbFull.AppendLine("module(\"water\", package.seeall)");

            GenerateMainFuncName(moduleName, componentList, ref sbFull);

            return sbFull.ToString();
        }

        private static void GenerateMainFuncName(string moduleName, List<MonoBehaviour> componentList, ref StringBuilder sb)
        {
            StringBuilder sbDeclare = new StringBuilder();
            StringBuilder sbFillData = new StringBuilder();
            StringBuilder sbInitView = new StringBuilder();
            StringBuilder sbDispose = new StringBuilder();

            List<string> sbFunctionDeclare = new List<string>();

            if (componentList != null)
            {
                foreach (MonoBehaviour obj in componentList)
                {
                    if (!obj.MIsActive) continue;

                    obj.GetDeclarationCode(ref sbDeclare);
                    obj.GetFillDataCode(ref sbFillData);
                    obj.GetInitCode(ref sbInitView, ref sbFunctionDeclare);
                    obj.GetDisposeCode(ref sbDispose);
                }
            }

            //switch (fileType)
            //{
            //    case enmFileType.Mediator:
            //        sb.AppendLine(string.Format("function Create{0}Mediator()", moduleName));
            //        break;
            //    case enmFileType.UIView:
                    sb.AppendLine(string.Format("function Create{0}UIView()", moduleName));
            //        break;
            //    case enmFileType.SubView:
            //        sb.AppendLine(string.Format("function Create{0}()", moduleName));
            //        break;
            //}
            GenerateDeclarations(moduleName, sbDeclare.ToString(), sbFunctionDeclare, ref sb);
            //if (fileType == enmFileType.Mediator)
            //{
            //    sb.AppendLine(string.Format("\tfunction {0}:Init()", moduleName));
            //    sb.AppendLine("\tend");
            //    sb.AppendLine(string.Format("\tfunction {0}:Dispose()", moduleName));
            //    sb.AppendLine("\tend");
            //}
            //else
            //{
            GenerateFunctions(moduleName, sbFunctionDeclare, ref sb);
                GenerateFillData(moduleName, sbFillData.ToString(), ref sb);
                GenerateInitView(moduleName, sbInitView.ToString(), ref sb);
                GenerateDispose(moduleName, sbDispose.ToString(), ref sb);
            //}


            sb.AppendLine(string.Format("\treturn {0}", moduleName));
            sb.AppendLine("end");
        }

        private static void GenerateFunctions(string moduleName, List<string> sbFunctionDeclare, ref StringBuilder sb)
        {
            for (int i = 0; i < sbFunctionDeclare.Count; i++)
            {
                sb.AppendLine(string.Format("\t{0}.{1} = function()", moduleName, sbFunctionDeclare[i]));
                sb.AppendLine("\tend");
            }
        }

        private static void GenerateDeclarations(string moduleName, string content, List<string> sbFunctionDeclare, ref StringBuilder sb)
        {
            sb.Append(string.Format("\tlocal {0} = ", moduleName)).AppendLine("{");//.Append("{").AppendLine("}");
            sb.AppendLine(string.Format("{0}", content));
            for (int i = 0; i < sbFunctionDeclare.Count; i++)
            {
                sb.AppendLine(string.Format("\t\t{0} = nil,", sbFunctionDeclare[i]));
            }
            
            sb.AppendLine("\t}");
        }
        private static void GenerateFillData(string moduleName, string content, ref StringBuilder sb)
        {
            sb.AppendLine(string.Format("\tfunction {0}:FillData(param_)", moduleName));
            sb.AppendLine(string.Format("{0}", content));
            sb.AppendLine("\tend");
        }
        private static void GenerateInitView(string moduleName, string content, ref StringBuilder sb)
        {
            sb.AppendLine(string.Format("\tfunction {0}:InitUIView(param_)", moduleName));
            sb.AppendLine(string.Format("{0}", content));
            sb.AppendLine("\tend");
        }
        private static void GenerateDispose(string moduleName, string content, ref StringBuilder sb)
        {
            sb.AppendLine(string.Format("\tfunction {0}:Dispose()", moduleName));
            sb.AppendLine(string.Format("{0}", content));
            sb.AppendLine("\tend");
        }
    }
}
