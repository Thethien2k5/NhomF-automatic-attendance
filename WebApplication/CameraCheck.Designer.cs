using System.Drawing;
using System.Windows.Forms;

namespace WebApplication
{
    partial class CameraCheck
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CameraCheck));
            this.cbCamera = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.Notification = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.ptbIMG = new System.Windows.Forms.PictureBox();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.attentionTable = new System.Windows.Forms.DataGridView();
            this.ClassList = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.closeClass = new CustomControls.RJControls.bButton();
            this.openClass = new CustomControls.RJControls.bButton();
            this.btStop = new CustomControls.RJControls.bButton();
            this.btStart = new CustomControls.RJControls.bButton();
            ((System.ComponentModel.ISupportInitialize)(this.ptbIMG)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.attentionTable)).BeginInit();
            this.SuspendLayout();
            // 
            // cbCamera
            // 
            this.cbCamera.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbCamera.FormattingEnabled = true;
            this.cbCamera.Location = new System.Drawing.Point(192, 411);
            this.cbCamera.Name = "cbCamera";
            this.cbCamera.Size = new System.Drawing.Size(238, 24);
            this.cbCamera.TabIndex = 10;
            this.cbCamera.SelectedIndexChanged += new System.EventHandler(this.cbCamera_SelectedIndexChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.Color.Black;
            this.label2.Location = new System.Drawing.Point(49, 411);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(120, 20);
            this.label2.TabIndex = 11;
            this.label2.Text = "Select Camera";
            // 
            // Notification
            // 
            this.Notification.AutoSize = true;
            this.Notification.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Notification.Location = new System.Drawing.Point(76, 514);
            this.Notification.Name = "Notification";
            this.Notification.Size = new System.Drawing.Size(93, 20);
            this.Notification.TabIndex = 12;
            this.Notification.Text = "Notification";
            this.Notification.Click += new System.EventHandler(this.Notification_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.Red;
            this.label1.Location = new System.Drawing.Point(122, 12);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(241, 31);
            this.label1.TabIndex = 13;
            this.label1.Text = "Điểm danh tự động";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(433, 25);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(137, 16);
            this.label3.TabIndex = 14;
            this.label3.Text = "Danh sách điểm danh";
            // 
            // ptbIMG
            // 
            this.ptbIMG.BackColor = System.Drawing.SystemColors.Control;
            this.ptbIMG.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("ptbIMG.BackgroundImage")));
            this.ptbIMG.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.ptbIMG.Location = new System.Drawing.Point(53, 46);
            this.ptbIMG.Name = "ptbIMG";
            this.ptbIMG.Size = new System.Drawing.Size(377, 359);
            this.ptbIMG.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.ptbIMG.TabIndex = 17;
            this.ptbIMG.TabStop = false;
            // 
            // timer1
            // 
            this.timer1.Interval = 1000;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // attentionTable
            // 
            this.attentionTable.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(62)))), ((int)(((byte)(218)))), ((int)(((byte)(216)))));
            this.attentionTable.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.attentionTable.Location = new System.Drawing.Point(453, 46);
            this.attentionTable.Name = "attentionTable";
            this.attentionTable.RowHeadersWidth = 20;
            this.attentionTable.RowTemplate.Height = 24;
            this.attentionTable.Size = new System.Drawing.Size(800, 700);
            this.attentionTable.TabIndex = 18;
            this.attentionTable.Visible = false;
            this.attentionTable.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.attentionTable_CellContentClick);
            // 
            // ClassList
            // 
            this.ClassList.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ClassList.FormattingEnabled = true;
            this.ClassList.Location = new System.Drawing.Point(192, 554);
            this.ClassList.Name = "ClassList";
            this.ClassList.Size = new System.Drawing.Size(238, 24);
            this.ClassList.TabIndex = 19;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(50, 554);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(82, 16);
            this.label4.TabIndex = 21;
            this.label4.Text = "Select Class";
            // 
            // closeClass
            // 
            this.closeClass.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(62)))), ((int)(((byte)(218)))), ((int)(((byte)(216)))));
            this.closeClass.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(62)))), ((int)(((byte)(218)))), ((int)(((byte)(216)))));
            this.closeClass.BorderColor = System.Drawing.Color.PaleVioletRed;
            this.closeClass.BorderRadius = 20;
            this.closeClass.BorderSize = 0;
            this.closeClass.FlatAppearance.BorderSize = 0;
            this.closeClass.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.closeClass.ForeColor = System.Drawing.Color.Black;
            this.closeClass.Location = new System.Drawing.Point(264, 584);
            this.closeClass.Name = "closeClass";
            this.closeClass.Size = new System.Drawing.Size(166, 54);
            this.closeClass.TabIndex = 22;
            this.closeClass.Text = "Đóng Lớp";
            this.closeClass.TextColor = System.Drawing.Color.Black;
            this.closeClass.UseVisualStyleBackColor = false;
            this.closeClass.Click += new System.EventHandler(this.closeClass_Click);
            // 
            // openClass
            // 
            this.openClass.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(62)))), ((int)(((byte)(218)))), ((int)(((byte)(216)))));
            this.openClass.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(62)))), ((int)(((byte)(218)))), ((int)(((byte)(216)))));
            this.openClass.BorderColor = System.Drawing.Color.PaleVioletRed;
            this.openClass.BorderRadius = 20;
            this.openClass.BorderSize = 0;
            this.openClass.FlatAppearance.BorderSize = 0;
            this.openClass.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.openClass.ForeColor = System.Drawing.Color.Black;
            this.openClass.Location = new System.Drawing.Point(53, 584);
            this.openClass.Name = "openClass";
            this.openClass.Size = new System.Drawing.Size(166, 54);
            this.openClass.TabIndex = 20;
            this.openClass.Text = "Xác nhận";
            this.openClass.TextColor = System.Drawing.Color.Black;
            this.openClass.UseVisualStyleBackColor = false;
            this.openClass.Click += new System.EventHandler(this.openClass_Click);
            // 
            // btStop
            // 
            this.btStop.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(62)))), ((int)(((byte)(218)))), ((int)(((byte)(216)))));
            this.btStop.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(62)))), ((int)(((byte)(218)))), ((int)(((byte)(216)))));
            this.btStop.BorderColor = System.Drawing.Color.PaleVioletRed;
            this.btStop.BorderRadius = 20;
            this.btStop.BorderSize = 0;
            this.btStop.FlatAppearance.BorderSize = 0;
            this.btStop.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btStop.ForeColor = System.Drawing.Color.Black;
            this.btStop.Location = new System.Drawing.Point(264, 441);
            this.btStop.Name = "btStop";
            this.btStop.Size = new System.Drawing.Size(166, 54);
            this.btStop.TabIndex = 16;
            this.btStop.Text = "Kết thúc điểm danh";
            this.btStop.TextColor = System.Drawing.Color.Black;
            this.btStop.UseVisualStyleBackColor = false;
            this.btStop.Click += new System.EventHandler(this.btStop_Click);
            // 
            // btStart
            // 
            this.btStart.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(62)))), ((int)(((byte)(218)))), ((int)(((byte)(216)))));
            this.btStart.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(62)))), ((int)(((byte)(218)))), ((int)(((byte)(216)))));
            this.btStart.BorderColor = System.Drawing.Color.PaleVioletRed;
            this.btStart.BorderRadius = 20;
            this.btStart.BorderSize = 0;
            this.btStart.FlatAppearance.BorderSize = 0;
            this.btStart.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btStart.ForeColor = System.Drawing.Color.Black;
            this.btStart.Location = new System.Drawing.Point(53, 441);
            this.btStart.Name = "btStart";
            this.btStart.Size = new System.Drawing.Size(166, 54);
            this.btStart.TabIndex = 15;
            this.btStart.Text = "Bắt đầu điểm danh";
            this.btStart.TextColor = System.Drawing.Color.Black;
            this.btStart.UseVisualStyleBackColor = false;
            this.btStart.Click += new System.EventHandler(this.btStart_Click);
            // 
            // CameraCheck
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1409, 1055);
            this.Controls.Add(this.closeClass);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.openClass);
            this.Controls.Add(this.ClassList);
            this.Controls.Add(this.attentionTable);
            this.Controls.Add(this.ptbIMG);
            this.Controls.Add(this.btStop);
            this.Controls.Add(this.btStart);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.Notification);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.cbCamera);
            this.Name = "CameraCheck";
            this.Text = "Form1";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            ((System.ComponentModel.ISupportInitialize)(this.ptbIMG)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.attentionTable)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.ComboBox cbCamera;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label Notification;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label3;
        private CustomControls.RJControls.bButton btStart;
        private CustomControls.RJControls.bButton btStop;
        private System.Windows.Forms.PictureBox ptbIMG;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.DataGridView attentionTable;
        private ComboBox ClassList;
        private CustomControls.RJControls.bButton openClass;
        private Label label4;
        private CustomControls.RJControls.bButton closeClass;
    }
}

