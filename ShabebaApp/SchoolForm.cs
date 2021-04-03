using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Data.SQLite;
using System.Windows.Forms;
using System.IO;
using Dapper;
using DataAccess.Models;
using static DataAccess.HelperFunctions;
using System.Text.RegularExpressions;

namespace ShabebaApp
{
    public partial class SchoolForm : UserControl
    {
        public SchoolForm()
        {
            InitializeComponent();
        }
        
        private static string ConnectionString()
        {
            string file = @"Resources\ShabebaDB.db";
            string dir = Path.GetFullPath(file);
            return dir;
        }
        public DataTable GetSchools()
        {
            string sql = "SELECT * FROM Schools";
            List<School> schools = new List<School>();
            using (IDbConnection connection = new SQLiteConnection($@"Data Source={ConnectionString()}; Version = 3"))
            {
                schools = connection.Query<School>(sql).ToList();
                schools.ForEach(item =>
                {
                    item.NumberOfMembers = connection.QueryFirst<int>("SELECT COUNT(SchoolId) FROM [Members] WHERE SchoolId=@Id", new { item.Id });
                });
            }

            return ToDataTable(schools);
        }
        private bool ValidControl()
        {
            if (txtId.Text == "")
            {
                MessageBox.Show("ادخل رقم المدرسة", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            else if (txtName.Text == "")
            {
                MessageBox.Show("ادخل اسم المدرسة", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            else if (txtAddress.Text == "")
            {
                MessageBox.Show("ادخل عنوان المدرسة", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            else if (txtNameManager.Text == "")
            {
                MessageBox.Show("ادخل اسم المدير", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            else if (txtNumberOfManager.Text == "")
            {
                MessageBox.Show("ادخل اسم المدرسة", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            else if (txtNumberOfSchool.Text =="")
            {
                MessageBox.Show("ادخل رقم المدرسة","", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return true;
            }
            else return true;
        }
        private void SchoolForm_Load(object sender, EventArgs e)
        {
            Filldgv(GetSchools(),dgv);
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (!ValidControl())
                return;
            else
            {
                using (IDbConnection dbConnection = new SQLiteConnection($@"Data Source={ConnectionString()}; Version = 3"))
                {
                    string name = txtName.Text;
                    bool exist = dbConnection.ExecuteScalar<bool>("select count(1) from Schools where name = @name", new { name });
                    if (exist)
                    {
                        MessageBox.Show("المدرسة موجودة بالفعل");
                        return;
                    }
                    string insert = "INSERT INTO Schools values(@Id,@name,@address,@Manager,@ManagerPhone,@SchoolPhone,0)";
                    School school = new School();
                    school.FillData(Convert.ToInt32(txtId.Text), Regex.Replace(txtName.Text, @"\s+", " "), Regex.Replace(txtAddress.Text, @"\s+", " "), Regex.Replace(txtNameManager.Text, @"\s+", " "), txtNumberOfManager.Text, txtNumberOfSchool.Text);
                    dbConnection.Execute(insert, school);
                }
                string namemessage = txtName.Text;
                btnReset.PerformClick();
                Filldgv(GetSchools(), dgv);
                MemberForm frm = new MemberForm();
                Filldgv(frm.LoadMemberForm(), frm.dgv);
                frm.LoadComboBox();
                MessageBox.Show($"تم إضافة مدرسة {namemessage} ");
            }
        }

        private void txtId_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar) && !char.IsWhiteSpace(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void txtName_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsLetter(e.KeyChar) && !char.IsControl(e.KeyChar) && !char.IsWhiteSpace(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            txtId.ReadOnly = false;
            btnAdd.Enabled = true;
            txtId.Clear();
            txtName.Clear();
            txtAddress.Clear();
            txtNameManager.Clear();
            txtNumberOfManager.Clear();
            txtNumberOfSchool.Clear();
            txtSearch.Clear();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("هل تريد حذف هذا السجل", "حذف", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
            {
                SQLiteConnection connection = new SQLiteConnection($@"Data Source={ConnectionString()}; Version = 3");
                string DeleteCommand = "DELETE FROM [Schools] WHERE Id = @Id";
                SQLiteCommand cmd = new SQLiteCommand(DeleteCommand, connection);
                cmd.Parameters.AddWithValue("@Id", Convert.ToInt32(txtId.Text));
                connection.Open();
                cmd.ExecuteNonQuery();
                connection.Close();
                Filldgv(GetSchools(), dgv);
                MemberForm frm = new MemberForm();
                Filldgv(frm.LoadMemberForm(), frm.dgv);
                frm.LoadComboBox();
                btnReset.PerformClick();
                MessageBox.Show("تمت عملية الحذف بنجاح", "نجاح", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                btnReset.PerformClick();
            }
        }

        private void dgv_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            txtId.ReadOnly = true;
            btnAdd.Enabled = false;
            txtId.Text = dgv.Rows[e.RowIndex].Cells[0].FormattedValue.ToString();
            txtName.Text = dgv.Rows[e.RowIndex].Cells[1].FormattedValue.ToString();
            txtAddress.Text = dgv.Rows[e.RowIndex].Cells[2].FormattedValue.ToString();
            txtNameManager.Text = dgv.Rows[e.RowIndex].Cells[3].FormattedValue.ToString();
            txtNumberOfManager.Text = dgv.Rows[e.RowIndex].Cells[4].FormattedValue.ToString();
            txtNumberOfSchool.Text = dgv.Rows[e.RowIndex].Cells[5].FormattedValue.ToString();
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (!ValidControl())
                return;
            else
            {
                txtId.ReadOnly = false;
                btnAdd.Enabled = true;
                SQLiteConnection connection = new SQLiteConnection($@"Data Source={ConnectionString()}; Version = 3");
                try
                {
                    string UpdateCommand = "UPDATE Schools SET name=@name ,address=@address,manager=@manager,managerPhone=@managerPhone,schoolPhone=@schoolPhone WHERE Id=@Id";
                    SQLiteCommand cmd = new SQLiteCommand(UpdateCommand, connection);
                    cmd.Parameters.AddWithValue("@name", txtName.Text);
                    cmd.Parameters.AddWithValue("@address", txtAddress.Text);
                    cmd.Parameters.AddWithValue("@manager", txtNameManager.Text);
                    cmd.Parameters.AddWithValue("@managerPhone", txtNumberOfManager.Text);
                    cmd.Parameters.AddWithValue("@schoolPhone", txtNumberOfSchool.Text);
                    cmd.Parameters.AddWithValue("@id", txtId.Text);
                    connection.Open();
                    cmd.ExecuteNonQuery();
                    Filldgv(GetSchools(), dgv);
                    MemberForm frm = new MemberForm();
                    Filldgv(frm.LoadMemberForm(), frm.dgv);
                    frm.LoadComboBox();
                    btnReset.PerformClick();
                    MessageBox.Show("تم إجراء التعديل بنجاح", "نجاح", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                    
                }
                finally
                {
                    connection.Close();
                }
            }
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtSearch.Text))
                Filldgv(GetSchools(), dgv);
            else
            {
                SQLiteConnection connection = new SQLiteConnection($@"Data Source={ConnectionString()}; Version = 3");
                string search = Regex.Replace(txtSearch.Text, @"\s+", " ");
                SQLiteCommand cmd = new SQLiteCommand("SELECT * FROM Schools WHERE name like '%" + search + "%'", connection);
                connection.Open();
                SQLiteDataReader data = cmd.ExecuteReader();
                DataTable dt = new DataTable();
                dt.Load(data);
                Filldgv(dt, dgv);
                connection.Close();
            }
        }
    }
}
