using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PrefabAnalyzer
{
    public partial class frmMain : Form
    {
        private string[] mIgnoreObjNames = null;

        public frmMain()
        {
            InitializeComponent();

            InitConfig();
        }

        private void InitConfig()
        {
            string names = Properties.Resources.IgnoreNodeName;
            if (!string.IsNullOrEmpty(names))
            {
                mIgnoreObjNames = names.Split(',');
            }
        }

        private void btnChoosePrefab_Click(object sender, EventArgs e)
        {
            OpenFileDialog fileDialog = new OpenFileDialog();
            fileDialog.Multiselect = false;
            fileDialog.Title = "请选择文件";
            fileDialog.Filter = "Prefab文件(*.prefab)|*.prefab";
            if (fileDialog.ShowDialog() == DialogResult.OK)
            {
                txtPrefabPath.Text = fileDialog.FileName;
            }
        }

        private void btnAnalyzePrefab_Click(object sender, EventArgs e)
        {
            string fileContent = File.ReadAllText(txtPrefabPath.Text, Encoding.UTF8);
            PrefabAnalyzer.DoAnalyze(fileContent);

            treeView1.Nodes.Clear();
            foreach (Prefab prefab in PrefabAnalyzer.mPrefabObjects.Values)
            {
                GenerateTreeNew(prefab, null);
            }

            treeView1.ExpandAll();
        }

        private void treeView1_MouseDown(object sender, MouseEventArgs e)
        {
            TreeNode node = treeView1.GetNodeAt(e.X, e.Y);
            if (node != null)
            {
                treeView1.SelectedNode = node;

                ulong guid = ulong.Parse(node.Tag.ToString());
                UnityObject obj = PrefabAnalyzer.GetObjectById(guid);

                GameObject go = obj as GameObject;
                
                if (go != null && go.mComponents != null)
                {
                    for (int i = 0; i < go.mComponents.Count; i++)
                    {
                        Console.WriteLine(go.mComponents[i].GetType());
                    }
                }
            }
        }

        private void GenerateTreeNew(UnityObject parentObj, TreeNode rootNode)
        {
            TreeNode node;
            if (rootNode == null)
            {
                GameObject rootGo = PrefabAnalyzer.GetObjectById((parentObj as Prefab).rootGameObjectId) as GameObject;
                node = treeView1.Nodes.Add(rootGo.MName);
                node.Tag = rootGo.guid;
                node.Checked = rootGo.MIsActive;
                
                GenerateTreeNew(rootGo, node);
                return;
            }

            if (parentObj.mChildren != null)
            {
                foreach (UnityObject child in parentObj.mChildren)
                {
                    node = rootNode.Nodes.Add(child.MName);
                    node.Tag = child.guid;
                    node.Checked = parentObj.MIsActive;

                    GenerateTreeNew(child, node);
                }
            }
        }

        private void btnExportLua_Click(object sender, EventArgs e)
        {
            List<MonoBehaviour> list = new List<MonoBehaviour>();
            GetMonoBehaviours(treeView1.Nodes[0], ref list);

            Clipboard.SetDataObject(LuaUtil.GenerateUILuaCode(txtModuleName.Text, list));
            MessageBox.Show("Codepiece generated to clipboard");
        }

        private void GetMonoBehaviours(TreeNode node, ref List<MonoBehaviour> list)
        {
            if (!node.Checked)
                return;
            
            ulong guid = ulong.Parse(node.Tag.ToString());
            UnityObject obj = PrefabAnalyzer.GetObjectById(guid);

            GameObject go = obj as GameObject;
            
            if (go != null && go.mComponents != null && (mIgnoreObjNames == null || !mIgnoreObjNames.Contains<string>(go.MName)))
            {
                for (int i = 0; i < go.mComponents.Count; i++)
                {
                    Console.WriteLine(go.mComponents[i].GetType());
                    list.Add(go.mComponents[i]);
                }
            }

            for (int i = 0; i < node.Nodes.Count; i ++)
            {
                GetMonoBehaviours(node.Nodes[i], ref list);
            }            
        }
    }
}
