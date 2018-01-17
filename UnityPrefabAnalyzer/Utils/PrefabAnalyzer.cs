using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace PrefabAnalyzer
{
    public class PrefabAnalyzer
    {
        public static Dictionary<ulong, Prefab> mPrefabObjects;
        private static Dictionary<ulong, UnityObject> mDicObjects;

        private static Dictionary<Type, UnityAttribute> mClassPatterns;

        public static UnityObject GetObjectById(ulong guid_)
        {
            if (mDicObjects == null) return null;
            UnityObject obj;
            mDicObjects.TryGetValue(guid_, out obj);
            return obj;
        }
        
        public static void DoAnalyze(string content)
        {
            InitData();
            
            string[] lineArr = content.Split('\n');

            string line = null;
            UnityObject currentObj = null;
            PropertyInfo[] propArr = null;
            object[] propPatterns = null;
            Match match;
            UnityAttribute attr;
            for (int i = 2, lineLen = lineArr.Length; i < lineLen; i++)
            {
                line = lineArr[i].TrimStart();

                // start with a new object
                if (line.StartsWith("--- "))
                {
                    currentObj = null;
                }

                //init new object
                foreach (Type t in mClassPatterns.Keys)
                {
                    attr = mClassPatterns[t];
                    match = Regex.Match(line, attr.titlePattern);
                    if (match.Success)
                    {
                        currentObj = (UnityObject)Activator.CreateInstance(t, true);
                        currentObj.guid = ulong.Parse(match.Groups[1].Value);
                        propArr = currentObj.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);

                        if (currentObj.ObjType == enmObjectType.Prefab)
                        {
                            mPrefabObjects[currentObj.guid] = currentObj as Prefab;
                        }
                        else
                        {
                            mDicObjects[currentObj.guid] = currentObj;
                        }
                        break;
                    }
                }

                //add children ids
                if (currentObj != null)
                {
                    match = Regex.Match(line, @"^- component: {fileID: (.+)}$");
                    if (match.Success)
                    {
                        currentObj.AddComponentId(ulong.Parse(match.Groups[1].Value));
                        continue;
                    }

                    match = Regex.Match(line, @"^- {fileID: (.+)}$");
                    if (match.Success)
                    {
                        currentObj.AddComponentId(ulong.Parse(match.Groups[1].Value));
                        continue;
                    }
                }

                // analyze properties
                if (currentObj != null && propArr != null)
                {
                    for (int j = 0, fieldLen = propArr.Length; j < fieldLen; j++)
                    {
                        propPatterns = propArr[j].GetCustomAttributes(typeof(UnityAttribute), false);
                        if (propPatterns != null && propPatterns.Length > 0)
                        {
                            match = Regex.Match(line, (propPatterns[0] as UnityAttribute).titlePattern);
                            if (match.Success)
                            {
                                if (match.Groups.Count > 1)
                                {
                                    propArr[j].SetValue(currentObj, ParsePropValue(propArr[j], match.Groups[1].Value));
                                }
                                else
                                {
                                    propArr[j].SetValue(currentObj, ParsePropValue(propArr[j], "1"));
                                }
                                break;
                            }
                            
                        }
                    }
                }
            }

            // build tree struct with each object
            GenerateTreeStruct();

            Console.WriteLine("Fin");
        }

        private static void GenerateTreeStruct()
        {
            foreach (Prefab prefab in mPrefabObjects.Values)
            {
                prefab.gameObject = GetObjectById(prefab.rootGameObjectId) as GameObject;
                GenerateTree(prefab.gameObject, null);
            }

            MonoBehaviour child;
            bool active;
            foreach (UnityObject obj in mDicObjects.Values)
            {
                if (obj.ObjType == enmObjectType.GameObject)
                {
                    if (obj.MComponentIdList != null)
                    {
                        for (int i = 0; i < obj.MComponentIdList.Count; ++i)
                        {
                            if (obj.mTransform != null && obj.MComponentIdList[i] == obj.mTransform.guid)
                                continue;

                            if (obj.mComponents == null)
                                obj.mComponents = new List<MonoBehaviour>();
                            child = GetObjectById(obj.MComponentIdList[i]) as MonoBehaviour;
                            if (child != null)
                            {
                                active = child.MIsActive;
                                if (child.IsScrollBar)
                                {
                                    child = new ScrollRect();
                                }else if (child.IsToggle)
                                {
                                    child = new Toggle();
                                }else if (child.IsButton)
                                {
                                    child = new Button();
                                }else if (child.IsImage)
                                {
                                    child = new Image();
                                }else if (child.IsText)
                                {
                                    child = new Text();
                                }

                                child.MIsActive = active;
                                child.MName = obj.MName;

                                obj.mComponents.Add(child);
                            }
                        }
                    }
                }
            }
        }

        private static void GenerateTree(GameObject parentGo, Component child)
        {
            if (child == null)
            {
                if (parentGo.MComponentIdList != null)
                {
                    for (int i = 0; i < parentGo.MComponentIdList.Count; ++i)
                    {
                        child = GetObjectById(parentGo.MComponentIdList[i]) as Component;
                        GenerateTree(parentGo, child);
                    }
                } 
            }
            else
            {
                if (child.ObjType == enmObjectType.RectTransform)
                {
                    using (RectTransform trans = (child as RectTransform))
                    {
                        if (trans.gameObjectId == parentGo.guid)
                        {
                            trans.gameObject = parentGo;
                            parentGo.mTransform = trans;
                        }
                        else
                        {
                            trans.gameObject = GetObjectById(trans.gameObjectId) as GameObject;
                            trans.gameObject.mTransform = trans;
                            if (parentGo.mChildren == null)
                            {
                                parentGo.mChildren = new List<UnityObject>();
                            }
                            parentGo.mChildren.Add(trans.gameObject);
                        }

                        if (trans.MComponentIdList != null)
                        {
                            for (int i = 0; i < trans.MComponentIdList.Count; ++i)
                            {
                                GenerateTree(trans.gameObject, GetObjectById(trans.MComponentIdList[i]) as Component);
                            }
                        }
                    }
                }
                else if (child.ObjType == enmObjectType.MonoBehaviour)
                {
                    if (parentGo.mComponents == null)
                        parentGo.mComponents = new List<MonoBehaviour>();

                    parentGo.mComponents.Add(child as MonoBehaviour);
                }
            }
        }

        private static void InitData()
        {
            if (mClassPatterns == null)
            {
                mClassPatterns = new Dictionary<Type, UnityAttribute>();
                object[] tempObj = typeof(Prefab).GetCustomAttributes(typeof(UnityAttribute), false);
                if (tempObj.Length > 0)
                {
                    mClassPatterns[typeof(Prefab)] = tempObj[0] as UnityAttribute;
                }

                tempObj = typeof(GameObject).GetCustomAttributes(typeof(UnityAttribute), false);
                if (tempObj.Length > 0)
                {
                    mClassPatterns[typeof(GameObject)] = tempObj[0] as UnityAttribute;
                }

                tempObj = typeof(RectTransform).GetCustomAttributes(typeof(UnityAttribute), false);
                if (tempObj.Length > 0)
                {
                    mClassPatterns[typeof(RectTransform)] = tempObj[0] as UnityAttribute;
                }

                tempObj = typeof(MonoBehaviour).GetCustomAttributes(typeof(UnityAttribute), false);
                if (tempObj.Length > 0)
                {
                    mClassPatterns[typeof(MonoBehaviour)] = tempObj[0] as UnityAttribute;
                }
            }
            if (mPrefabObjects == null)
            {
                mPrefabObjects = new Dictionary<ulong, Prefab>();
            }
            else
            {
                mPrefabObjects.Clear();
            }
            if (mDicObjects == null)
            {
                mDicObjects = new Dictionary<ulong, UnityObject>();
            }
            else
            {
                mDicObjects.Clear();
            }
        }

        private static object ParsePropValue(PropertyInfo field, string valueString)
        {
            if (field.PropertyType == typeof(sbyte))
            {
                return string.IsNullOrEmpty(valueString) ? default(sbyte) : sbyte.Parse(valueString);
            }
            else if (field.PropertyType == typeof(byte))
            {
                return string.IsNullOrEmpty(valueString) ? default(byte) : byte.Parse(valueString);
            }
            else if (field.PropertyType == typeof(short))
            {
                return string.IsNullOrEmpty(valueString) ? default(short) : short.Parse(valueString);
            }
            else if (field.PropertyType == typeof(ushort))
            {
                return string.IsNullOrEmpty(valueString) ? default(ushort) : ushort.Parse(valueString);
            }
            else if (field.PropertyType == typeof(int))
            {
                return string.IsNullOrEmpty(valueString) ? default(int) : int.Parse(valueString);
            }
            else if (field.PropertyType == typeof(Int64))
            {
                return string.IsNullOrEmpty(valueString) ? default(Int64) : Int64.Parse(valueString);
            }
            else if (field.PropertyType == typeof(uint))
            {
                return string.IsNullOrEmpty(valueString) ? default(uint) : uint.Parse(valueString);
            }
            else if (field.PropertyType == typeof(long))
            {
                return string.IsNullOrEmpty(valueString) ? default(long) : long.Parse(valueString);
            }
            else if (field.PropertyType == typeof(ulong))
            {
                return string.IsNullOrEmpty(valueString) ? default(ulong) : ulong.Parse(valueString);
            }
            else if (field.PropertyType == typeof(float))
            {
                return string.IsNullOrEmpty(valueString) ? default(float) : float.Parse(valueString);
            }
            else if (field.PropertyType.IsEnum)
            {
                return int.Parse(valueString);
            }
            else if (field.PropertyType == typeof(bool))
            {
                return valueString != "0";
            }
            else if (field.PropertyType == typeof(string))
            {
                return valueString;
            }
            else
            {
                throw new Exception("CSVUtil_ERROR: have not process type" + field.PropertyType);
            }
        }
    }
}
