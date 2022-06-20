namespace DesktopClient
{
    partial class Main
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }

            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.buttonAddFile = new System.Windows.Forms.Button();
            this.buttonEdit = new System.Windows.Forms.Button();
            this.buttonDelete = new System.Windows.Forms.Button();
            this.listView1 = new System.Windows.Forms.ListView();
            this.columnHeaderId = new System.Windows.Forms.ColumnHeader();
            this.columnHeaderType = new System.Windows.Forms.ColumnHeader();
            this.columnHeaderAddress = new System.Windows.Forms.ColumnHeader();
            this.columnHeaderStatus = new System.Windows.Forms.ColumnHeader();
            this.columnHeaderSchedule = new System.Windows.Forms.ColumnHeader();
            this.buttonAddDb = new System.Windows.Forms.Button();
            this.buttonRestore = new System.Windows.Forms.Button();
            this.buttonDisable = new System.Windows.Forms.Button();
            this.progressBarQuota = new System.Windows.Forms.ProgressBar();
            this.buttonQuota = new System.Windows.Forms.Button();
            this.buttonHistory = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.buttonAddPgSql = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // buttonAddFile
            // 
            this.buttonAddFile.Location = new System.Drawing.Point(7, 22);
            this.buttonAddFile.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.buttonAddFile.Name = "buttonAddFile";
            this.buttonAddFile.Size = new System.Drawing.Size(51, 23);
            this.buttonAddFile.TabIndex = 1;
            this.buttonAddFile.Text = "Файл";
            this.buttonAddFile.Click += new System.EventHandler(this.buttonAddFile_Click);
            // 
            // buttonEdit
            // 
            this.buttonEdit.Enabled = false;
            this.buttonEdit.Location = new System.Drawing.Point(255, 13);
            this.buttonEdit.Name = "buttonEdit";
            this.buttonEdit.Size = new System.Drawing.Size(75, 23);
            this.buttonEdit.TabIndex = 4;
            this.buttonEdit.Text = "Изменить";
            this.buttonEdit.UseVisualStyleBackColor = true;
            this.buttonEdit.Click += new System.EventHandler(this.buttonEdit_Click);
            // 
            // buttonDelete
            // 
            this.buttonDelete.Enabled = false;
            this.buttonDelete.Location = new System.Drawing.Point(255, 40);
            this.buttonDelete.Name = "buttonDelete";
            this.buttonDelete.Size = new System.Drawing.Size(75, 23);
            this.buttonDelete.TabIndex = 5;
            this.buttonDelete.Text = "Удалить";
            this.buttonDelete.UseVisualStyleBackColor = true;
            this.buttonDelete.Click += new System.EventHandler(this.buttonDelete_Click);
            // 
            // listView1
            // 
            this.listView1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.listView1.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeaderId,
            this.columnHeaderType,
            this.columnHeaderAddress,
            this.columnHeaderStatus,
            this.columnHeaderSchedule});
            this.listView1.FullRowSelect = true;
            this.listView1.GridLines = true;
            this.listView1.Location = new System.Drawing.Point(12, 69);
            this.listView1.MultiSelect = false;
            this.listView1.Name = "listView1";
            this.listView1.Size = new System.Drawing.Size(534, 265);
            this.listView1.TabIndex = 9;
            this.listView1.UseCompatibleStateImageBehavior = false;
            this.listView1.View = System.Windows.Forms.View.Details;
            this.listView1.SelectedIndexChanged += new System.EventHandler(this.listView1_SelectedIndexChanged);
            // 
            // columnHeaderId
            // 
            this.columnHeaderId.Text = "№";
            // 
            // columnHeaderType
            // 
            this.columnHeaderType.Text = "Тип";
            this.columnHeaderType.Width = 100;
            // 
            // columnHeaderAddress
            // 
            this.columnHeaderAddress.Text = "Адрес";
            this.columnHeaderAddress.Width = 100;
            // 
            // columnHeaderStatus
            // 
            this.columnHeaderStatus.Text = "Статус";
            this.columnHeaderStatus.Width = 120;
            // 
            // columnHeaderSchedule
            // 
            this.columnHeaderSchedule.Text = "Расписание";
            this.columnHeaderSchedule.Width = 120;
            // 
            // buttonAddDb
            // 
            this.buttonAddDb.Location = new System.Drawing.Point(65, 22);
            this.buttonAddDb.Name = "buttonAddDb";
            this.buttonAddDb.Size = new System.Drawing.Size(74, 23);
            this.buttonAddDb.TabIndex = 2;
            this.buttonAddDb.Text = "SQL Server";
            this.buttonAddDb.UseVisualStyleBackColor = true;
            this.buttonAddDb.Click += new System.EventHandler(this.buttonAddDb_Click);
            // 
            // buttonRestore
            // 
            this.buttonRestore.Enabled = false;
            this.buttonRestore.Location = new System.Drawing.Point(423, 13);
            this.buttonRestore.Name = "buttonRestore";
            this.buttonRestore.Size = new System.Drawing.Size(107, 50);
            this.buttonRestore.TabIndex = 8;
            this.buttonRestore.Text = "Выпонить восстановление";
            this.buttonRestore.UseVisualStyleBackColor = true;
            this.buttonRestore.Click += new System.EventHandler(this.buttonRestore_Click);
            // 
            // buttonDisable
            // 
            this.buttonDisable.Enabled = false;
            this.buttonDisable.Location = new System.Drawing.Point(336, 13);
            this.buttonDisable.Name = "buttonDisable";
            this.buttonDisable.Size = new System.Drawing.Size(81, 23);
            this.buttonDisable.TabIndex = 6;
            this.buttonDisable.Text = "Отключить";
            this.buttonDisable.UseVisualStyleBackColor = true;
            this.buttonDisable.Click += new System.EventHandler(this.buttonDisable_Click);
            // 
            // progressBarQuota
            // 
            this.progressBarQuota.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.progressBarQuota.Location = new System.Drawing.Point(12, 340);
            this.progressBarQuota.MarqueeAnimationSpeed = 0;
            this.progressBarQuota.Name = "progressBarQuota";
            this.progressBarQuota.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.progressBarQuota.Size = new System.Drawing.Size(453, 23);
            this.progressBarQuota.Step = 1;
            this.progressBarQuota.TabIndex = 12;
            this.progressBarQuota.Value = 100;
            // 
            // buttonQuota
            // 
            this.buttonQuota.Location = new System.Drawing.Point(471, 340);
            this.buttonQuota.Name = "buttonQuota";
            this.buttonQuota.Size = new System.Drawing.Size(75, 23);
            this.buttonQuota.TabIndex = 10;
            this.buttonQuota.Text = "Квота";
            this.buttonQuota.UseVisualStyleBackColor = true;
            this.buttonQuota.Click += new System.EventHandler(this.buttonQuota_Click);
            // 
            // buttonHistory
            // 
            this.buttonHistory.Enabled = false;
            this.buttonHistory.Location = new System.Drawing.Point(336, 40);
            this.buttonHistory.Name = "buttonHistory";
            this.buttonHistory.Size = new System.Drawing.Size(81, 23);
            this.buttonHistory.TabIndex = 7;
            this.buttonHistory.Text = "История";
            this.buttonHistory.UseVisualStyleBackColor = true;
            this.buttonHistory.Click += new System.EventHandler(this.buttonHistory_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.buttonAddPgSql);
            this.groupBox1.Controls.Add(this.buttonAddFile);
            this.groupBox1.Controls.Add(this.buttonAddDb);
            this.groupBox1.Location = new System.Drawing.Point(12, 11);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(237, 52);
            this.groupBox1.TabIndex = 11;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Резервирование";
            // 
            // buttonAddPgSql
            // 
            this.buttonAddPgSql.Location = new System.Drawing.Point(145, 22);
            this.buttonAddPgSql.Name = "buttonAddPgSql";
            this.buttonAddPgSql.Size = new System.Drawing.Size(84, 23);
            this.buttonAddPgSql.TabIndex = 3;
            this.buttonAddPgSql.Text = "PostgreSQL";
            this.buttonAddPgSql.UseVisualStyleBackColor = true;
            this.buttonAddPgSql.Click += new System.EventHandler(this.buttonAddPgSql_Click);
            // 
            // Main
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(558, 374);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.buttonHistory);
            this.Controls.Add(this.buttonQuota);
            this.Controls.Add(this.progressBarQuota);
            this.Controls.Add(this.buttonDisable);
            this.Controls.Add(this.buttonRestore);
            this.Controls.Add(this.listView1);
            this.Controls.Add(this.buttonDelete);
            this.Controls.Add(this.buttonEdit);
            this.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.Name = "Main";
            this.Text = "Задания сервера";
            this.Load += new System.EventHandler(this.Main_Load);
            this.groupBox1.ResumeLayout(false);
            this.ResumeLayout(false);

        }
        private System.Windows.Forms.Button buttonAddFile;

        #endregion

        private Button buttonEdit;
        private Button buttonDelete;
        private ListView listView1;
        private ColumnHeader columnHeaderType;
        private ColumnHeader columnHeaderAddress;
        private ColumnHeader columnHeaderStatus;
        private ColumnHeader columnHeaderSchedule;
        private Button buttonAddDb;
        private Button buttonRestore;
        private ColumnHeader columnHeaderId;
        private Button buttonDisable;
        private ProgressBar progressBarQuota;
        private Button buttonQuota;
        private Button buttonHistory;
        private GroupBox groupBox1;
        private Button buttonAddPgSql;
    }
}