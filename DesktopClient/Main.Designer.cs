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
            this.columnHeaderServer = new System.Windows.Forms.ColumnHeader();
            this.columnHeaderStatus = new System.Windows.Forms.ColumnHeader();
            this.columnHeaderSchedule = new System.Windows.Forms.ColumnHeader();
            this.buttonAddDb = new System.Windows.Forms.Button();
            this.buttonRestore = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // buttonAddFile
            // 
            this.buttonAddFile.Location = new System.Drawing.Point(13, 12);
            this.buttonAddFile.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.buttonAddFile.Name = "buttonAddFile";
            this.buttonAddFile.Size = new System.Drawing.Size(98, 42);
            this.buttonAddFile.TabIndex = 1;
            this.buttonAddFile.Text = "Резервировать файл";
            this.buttonAddFile.Click += new System.EventHandler(this.buttonAddFile_Click);
            // 
            // buttonEdit
            // 
            this.buttonEdit.Enabled = false;
            this.buttonEdit.Location = new System.Drawing.Point(225, 13);
            this.buttonEdit.Name = "buttonEdit";
            this.buttonEdit.Size = new System.Drawing.Size(75, 41);
            this.buttonEdit.TabIndex = 3;
            this.buttonEdit.Text = "Изменить";
            this.buttonEdit.UseVisualStyleBackColor = true;
            this.buttonEdit.Click += new System.EventHandler(this.buttonEdit_Click);
            // 
            // buttonDelete
            // 
            this.buttonDelete.Enabled = false;
            this.buttonDelete.Location = new System.Drawing.Point(306, 13);
            this.buttonDelete.Name = "buttonDelete";
            this.buttonDelete.Size = new System.Drawing.Size(75, 41);
            this.buttonDelete.TabIndex = 4;
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
            this.columnHeaderServer,
            this.columnHeaderStatus,
            this.columnHeaderSchedule});
            this.listView1.FullRowSelect = true;
            this.listView1.GridLines = true;
            this.listView1.Location = new System.Drawing.Point(12, 60);
            this.listView1.MultiSelect = false;
            this.listView1.Name = "listView1";
            this.listView1.Size = new System.Drawing.Size(485, 264);
            this.listView1.TabIndex = 6;
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
            // columnHeaderServer
            // 
            this.columnHeaderServer.Text = "Сервер";
            this.columnHeaderServer.Width = 80;
            // 
            // columnHeaderStatus
            // 
            this.columnHeaderStatus.Text = "Статус";
            this.columnHeaderStatus.Width = 80;
            // 
            // columnHeaderSchedule
            // 
            this.columnHeaderSchedule.Text = "Расписание";
            this.columnHeaderSchedule.Width = 120;
            // 
            // buttonAddDb
            // 
            this.buttonAddDb.Location = new System.Drawing.Point(118, 13);
            this.buttonAddDb.Name = "buttonAddDb";
            this.buttonAddDb.Size = new System.Drawing.Size(101, 41);
            this.buttonAddDb.TabIndex = 2;
            this.buttonAddDb.Text = "Резервировать БД";
            this.buttonAddDb.UseVisualStyleBackColor = true;
            this.buttonAddDb.Click += new System.EventHandler(this.buttonAddDb_Click);
            // 
            // buttonRestore
            // 
            this.buttonRestore.Enabled = false;
            this.buttonRestore.Location = new System.Drawing.Point(387, 13);
            this.buttonRestore.Name = "buttonRestore";
            this.buttonRestore.Size = new System.Drawing.Size(90, 41);
            this.buttonRestore.TabIndex = 5;
            this.buttonRestore.Text = "Восстановить";
            this.buttonRestore.UseVisualStyleBackColor = true;
            this.buttonRestore.Click += new System.EventHandler(this.buttonRestore_Click);
            // 
            // Main
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(509, 336);
            this.Controls.Add(this.buttonRestore);
            this.Controls.Add(this.buttonAddDb);
            this.Controls.Add(this.listView1);
            this.Controls.Add(this.buttonDelete);
            this.Controls.Add(this.buttonEdit);
            this.Controls.Add(this.buttonAddFile);
            this.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.Name = "Main";
            this.Text = "Задачи сервера";
            this.Load += new System.EventHandler(this.Main_Load);
            this.ResumeLayout(false);

        }
        private System.Windows.Forms.Button buttonAddFile;

        #endregion

        private Button buttonEdit;
        private Button buttonDelete;
        private ListView listView1;
        private ColumnHeader columnHeaderType;
        private ColumnHeader columnHeaderServer;
        private ColumnHeader columnHeaderStatus;
        private ColumnHeader columnHeaderSchedule;
        private Button buttonAddDb;
        private Button buttonRestore;
        private ColumnHeader columnHeaderId;
    }
}