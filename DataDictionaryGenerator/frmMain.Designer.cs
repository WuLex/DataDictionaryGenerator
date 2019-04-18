namespace DataDictionaryGenerator
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
            this.label1 = new System.Windows.Forms.Label();
            this.txtCnnString = new System.Windows.Forms.TextBox();
            this.btnBulid = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.dgvData = new System.Windows.Forms.DataGridView();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvData)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 24);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(137, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "SQL Server连接字符串：";
            // 
            // txtCnnString
            // 
            this.txtCnnString.Location = new System.Drawing.Point(155, 15);
            this.txtCnnString.Name = "txtCnnString";
            this.txtCnnString.Size = new System.Drawing.Size(678, 21);
            this.txtCnnString.TabIndex = 1;
            this.txtCnnString.Text = "Data Source=ServerIP;Initial Catalog=DBName;UID=sa;PWD=password";
            // 
            // btnBulid
            // 
            this.btnBulid.Location = new System.Drawing.Point(758, 59);
            this.btnBulid.Name = "btnBulid";
            this.btnBulid.Size = new System.Drawing.Size(75, 23);
            this.btnBulid.TabIndex = 2;
            this.btnBulid.Text = "生成";
            this.btnBulid.UseVisualStyleBackColor = true;
            this.btnBulid.Click += new System.EventHandler(this.btnBulid_Click);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.btnBulid);
            this.panel1.Controls.Add(this.txtCnnString);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(897, 88);
            this.panel1.TabIndex = 3;
            // 
            // dgvData
            // 
            this.dgvData.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvData.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvData.Location = new System.Drawing.Point(0, 88);
            this.dgvData.Name = "dgvData";
            this.dgvData.RowTemplate.Height = 23;
            this.dgvData.Size = new System.Drawing.Size(897, 386);
            this.dgvData.TabIndex = 4;
            // 
            // frmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(897, 474);
            this.Controls.Add(this.dgvData);
            this.Controls.Add(this.panel1);
            this.Name = "frmMain";
            this.Text = "数据字典生成器";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvData)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtCnnString;
        private System.Windows.Forms.Button btnBulid;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.DataGridView dgvData;
    }
}

