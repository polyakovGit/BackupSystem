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
            this.buttonAdd = new System.Windows.Forms.Button();
            this.buttonEdit = new System.Windows.Forms.Button();
            this.buttonDelete = new System.Windows.Forms.Button();
            this.listView1 = new System.Windows.Forms.ListView();
            this.columnHeaderType = new System.Windows.Forms.ColumnHeader();
            this.columnHeaderServer = new System.Windows.Forms.ColumnHeader();
            this.columnHeaderStatus = new System.Windows.Forms.ColumnHeader();
            this.columnHeaderSchedule = new System.Windows.Forms.ColumnHeader();
            this.SuspendLayout();
            // 
            // buttonAdd
            // 
            this.buttonAdd.Location = new System.Drawing.Point(13, 12);
            this.buttonAdd.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.buttonAdd.Name = "buttonAdd";
            this.buttonAdd.Size = new System.Drawing.Size(75, 23);
            this.buttonAdd.TabIndex = 2;
            this.buttonAdd.Text = "Новая";
            this.buttonAdd.Click += new System.EventHandler(this.buttonAdd_Click);
            // 
            // buttonEdit
            // 
            this.buttonEdit.Enabled = false;
            this.buttonEdit.Location = new System.Drawing.Point(95, 12);
            this.buttonEdit.Name = "buttonEdit";
            this.buttonEdit.Size = new System.Drawing.Size(75, 23);
            this.buttonEdit.TabIndex = 3;
            this.buttonEdit.Text = "Изменить";
            this.buttonEdit.UseVisualStyleBackColor = true;
            this.buttonEdit.Click += new System.EventHandler(this.buttonEdit_Click);
            // 
            // buttonDelete
            // 
            this.buttonDelete.Enabled = false;
            this.buttonDelete.Location = new System.Drawing.Point(176, 12);
            this.buttonDelete.Name = "buttonDelete";
            this.buttonDelete.Size = new System.Drawing.Size(75, 23);
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
            this.columnHeaderType,
            this.columnHeaderServer,
            this.columnHeaderStatus,
            this.columnHeaderSchedule});
            this.listView1.FullRowSelect = true;
            this.listView1.GridLines = true;
            this.listView1.Location = new System.Drawing.Point(12, 41);
            this.listView1.MultiSelect = false;
            this.listView1.Name = "listView1";
            this.listView1.Size = new System.Drawing.Size(431, 237);
            this.listView1.TabIndex = 5;
            this.listView1.UseCompatibleStateImageBehavior = false;
            this.listView1.View = System.Windows.Forms.View.Details;
            this.listView1.SelectedIndexChanged += new System.EventHandler(this.listView1_SelectedIndexChanged);
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
            // Main
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(455, 290);
            this.Controls.Add(this.listView1);
            this.Controls.Add(this.buttonDelete);
            this.Controls.Add(this.buttonEdit);
            this.Controls.Add(this.buttonAdd);
            this.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.Name = "Main";
            this.Text = "Задачи сервера";
            this.Load += new System.EventHandler(this.Main_Load);
            this.ResumeLayout(false);

        }
        private System.Windows.Forms.Button buttonAdd;

        #endregion

        private Button buttonEdit;
        private Button buttonDelete;
        private ListView listView1;
        private ColumnHeader columnHeaderType;
        private ColumnHeader columnHeaderServer;
        private ColumnHeader columnHeaderStatus;
        private ColumnHeader columnHeaderSchedule;
    }
}