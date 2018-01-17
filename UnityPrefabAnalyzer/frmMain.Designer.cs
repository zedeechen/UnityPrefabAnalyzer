namespace PrefabAnalyzer
{
    partial class frmMain
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmMain));
            this.treeView1 = new System.Windows.Forms.TreeView();
            this.label2 = new System.Windows.Forms.Label();
            this.txtModuleName = new System.Windows.Forms.TextBox();
            this.btnExportLua = new System.Windows.Forms.Button();
            this.txtPrefabPath = new System.Windows.Forms.TextBox();
            this.btnAnalyzePrefab = new System.Windows.Forms.Button();
            this.btnChoosePrefab = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // treeView1
            // 
            this.treeView1.CheckBoxes = true;
            this.treeView1.Location = new System.Drawing.Point(32, 64);
            this.treeView1.Name = "treeView1";
            this.treeView1.Size = new System.Drawing.Size(243, 307);
            this.treeView1.TabIndex = 28;
            this.treeView1.MouseDown += new System.Windows.Forms.MouseEventHandler(this.treeView1_MouseDown);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(30, 393);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(41, 12);
            this.label2.TabIndex = 27;
            this.label2.Text = "模块名";
            // 
            // txtModuleName
            // 
            this.txtModuleName.Location = new System.Drawing.Point(77, 388);
            this.txtModuleName.Name = "txtModuleName";
            this.txtModuleName.Size = new System.Drawing.Size(136, 21);
            this.txtModuleName.TabIndex = 26;
            // 
            // btnExportLua
            // 
            this.btnExportLua.Location = new System.Drawing.Point(398, 388);
            this.btnExportLua.Name = "btnExportLua";
            this.btnExportLua.Size = new System.Drawing.Size(90, 23);
            this.btnExportLua.TabIndex = 25;
            this.btnExportLua.Text = "导出Lua代码";
            this.btnExportLua.UseVisualStyleBackColor = true;
            this.btnExportLua.Click += new System.EventHandler(this.btnExportLua_Click);
            // 
            // txtPrefabPath
            // 
            this.txtPrefabPath.Location = new System.Drawing.Point(32, 20);
            this.txtPrefabPath.Name = "txtPrefabPath";
            this.txtPrefabPath.Size = new System.Drawing.Size(298, 21);
            this.txtPrefabPath.TabIndex = 22;
            // 
            // btnAnalyzePrefab
            // 
            this.btnAnalyzePrefab.Location = new System.Drawing.Point(427, 20);
            this.btnAnalyzePrefab.Name = "btnAnalyzePrefab";
            this.btnAnalyzePrefab.Size = new System.Drawing.Size(61, 23);
            this.btnAnalyzePrefab.TabIndex = 24;
            this.btnAnalyzePrefab.Text = "解析";
            this.btnAnalyzePrefab.UseVisualStyleBackColor = true;
            this.btnAnalyzePrefab.Click += new System.EventHandler(this.btnAnalyzePrefab_Click);
            // 
            // btnChoosePrefab
            // 
            this.btnChoosePrefab.Location = new System.Drawing.Point(336, 20);
            this.btnChoosePrefab.Name = "btnChoosePrefab";
            this.btnChoosePrefab.Size = new System.Drawing.Size(78, 23);
            this.btnChoosePrefab.TabIndex = 23;
            this.btnChoosePrefab.Text = "打开Prefab";
            this.btnChoosePrefab.UseVisualStyleBackColor = true;
            this.btnChoosePrefab.Click += new System.EventHandler(this.btnChoosePrefab_Click);
            // 
            // frmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(530, 443);
            this.Controls.Add(this.treeView1);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.txtModuleName);
            this.Controls.Add(this.btnExportLua);
            this.Controls.Add(this.txtPrefabPath);
            this.Controls.Add(this.btnAnalyzePrefab);
            this.Controls.Add(this.btnChoosePrefab);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "frmMain";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TreeView treeView1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtModuleName;
        private System.Windows.Forms.Button btnExportLua;
        private System.Windows.Forms.TextBox txtPrefabPath;
        private System.Windows.Forms.Button btnAnalyzePrefab;
        private System.Windows.Forms.Button btnChoosePrefab;
    }
}

