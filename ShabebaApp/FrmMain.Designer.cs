using System.Data.SQLite;
namespace ShabebaApp
{
    partial class FrmMain
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
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.btnMembers = new System.Windows.Forms.Button();
            this.btnSchool = new System.Windows.Forms.Button();
            this.schoolForm1 = new ShabebaApp.SchoolForm();
            this.memberForm1 = new ShabebaApp.MemberForm();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.White;
            this.panel1.Controls.Add(this.schoolForm1);
            this.panel1.Controls.Add(this.memberForm1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Margin = new System.Windows.Forms.Padding(5, 6, 5, 6);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1180, 700);
            this.panel1.TabIndex = 0;
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.Color.Gray;
            this.panel2.Controls.Add(this.pictureBox1);
            this.panel2.Controls.Add(this.btnMembers);
            this.panel2.Controls.Add(this.btnSchool);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Right;
            this.panel2.Location = new System.Drawing.Point(1182, 0);
            this.panel2.Margin = new System.Windows.Forms.Padding(5, 6, 5, 6);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(131, 700);
            this.panel2.TabIndex = 1;
            // 
            // pictureBox1
            // 
            this.pictureBox1.Location = new System.Drawing.Point(0, 0);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(131, 129);
            this.pictureBox1.TabIndex = 3;
            this.pictureBox1.TabStop = false;
            // 
            // btnMembers
            // 
            this.btnMembers.FlatAppearance.BorderSize = 0;
            this.btnMembers.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnMembers.ForeColor = System.Drawing.Color.White;
            this.btnMembers.Location = new System.Drawing.Point(0, 214);
            this.btnMembers.Margin = new System.Windows.Forms.Padding(5, 6, 5, 6);
            this.btnMembers.Name = "btnMembers";
            this.btnMembers.Size = new System.Drawing.Size(131, 55);
            this.btnMembers.TabIndex = 2;
            this.btnMembers.Text = "الأعضاء";
            this.btnMembers.UseVisualStyleBackColor = true;
            this.btnMembers.Click += new System.EventHandler(this.button1_Click);
            // 
            // btnSchool
            // 
            this.btnSchool.FlatAppearance.BorderSize = 0;
            this.btnSchool.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSchool.ForeColor = System.Drawing.Color.White;
            this.btnSchool.Location = new System.Drawing.Point(0, 300);
            this.btnSchool.Margin = new System.Windows.Forms.Padding(5, 6, 5, 6);
            this.btnSchool.Name = "btnSchool";
            this.btnSchool.Size = new System.Drawing.Size(131, 55);
            this.btnSchool.TabIndex = 1;
            this.btnSchool.Text = "المدارس";
            this.btnSchool.UseVisualStyleBackColor = true;
            this.btnSchool.Click += new System.EventHandler(this.btnSchool_Click);
            // 
            // schoolForm1
            // 
            this.schoolForm1.Font = new System.Drawing.Font("Tajawal", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.schoolForm1.Location = new System.Drawing.Point(0, 0);
            this.schoolForm1.Margin = new System.Windows.Forms.Padding(5, 6, 5, 6);
            this.schoolForm1.Name = "schoolForm1";
            this.schoolForm1.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.schoolForm1.Size = new System.Drawing.Size(1180, 700);
            this.schoolForm1.TabIndex = 1;
            // 
            // memberForm1
            // 
            this.memberForm1.Font = new System.Drawing.Font("Tajawal", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.memberForm1.Location = new System.Drawing.Point(0, 0);
            this.memberForm1.Margin = new System.Windows.Forms.Padding(5, 6, 5, 6);
            this.memberForm1.Name = "memberForm1";
            this.memberForm1.Size = new System.Drawing.Size(1180, 700);
            this.memberForm1.TabIndex = 0;
            // 
            // FrmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 26F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1313, 700);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Font = new System.Drawing.Font("Tajawal", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Margin = new System.Windows.Forms.Padding(5, 6, 5, 6);
            this.Name = "FrmMain";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.FrmMain_Load);
            this.panel1.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Button btnMembers;
        private System.Windows.Forms.Button btnSchool;
        private System.Windows.Forms.PictureBox pictureBox1;
        private MemberForm memberForm1;
        private SchoolForm schoolForm1;
    }
}

