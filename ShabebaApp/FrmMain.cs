using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ShabebaApp
{
    public partial class FrmMain : Form
    {
        public FrmMain()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            btnSchool.BackColor = Color.Gray;
            btnMembers.BackColor = Color.DimGray;
            this.memberForm1.BringToFront();
        }

        private void btnSchool_Click(object sender, EventArgs e)
        {
            btnSchool.BackColor = Color.DimGray;
            btnMembers.BackColor = Color.Gray;
            this.schoolForm1.BringToFront();
        }

        private void FrmMain_Load(object sender, EventArgs e)
        {
            btnSchool.BackColor = Color.DimGray;
        }
    }
}
