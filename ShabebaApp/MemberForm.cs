using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DataAccess.Models;
using static DataAccess.HelperFunctions;
using static DataAccess.SqliteDataAccess;
using System.Configuration;
namespace ShabebaApp
{
    public partial class MemberForm : UserControl
    {
        public MemberForm()
        {
            InitializeComponent();
        }

        List<Member> members = new List<Member>();
        private void MemberForm_Load(object sender, EventArgs e)
        {
            /* List<Member> members = new List<Member>();
             members = LoadPeople();*/
            Filldgv(LoadMembers(), dgv);
        }
    }
}
