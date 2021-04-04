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
using System.Data.SQLite;
using System.IO;
using Dapper;
using ClosedXML;
using System.Text.RegularExpressions;
using ClosedXML.Excel;

namespace ShabebaApp
{
    public partial class MemberForm : UserControl
    {
        public MemberForm()
        {
            InitializeComponent();
        }
        
        public bool ControlIsValid()
        {
            bool answer = false;
            foreach (Control item in this.Controls)
            {
                if (item is TextBox)
                {
                    if (item.Text != "")
                        answer = true;
                    else
                    {
                        answer = false;
                    }
                }
            }
            return answer;
        }
        private static string ConnectionString()
        {
            string file = @"Resources\ShabebaDB.db";
            string dir = Path.GetFullPath(file);
            return dir;
        }
        List<Member> members = new List<Member>();
        public DataTable LoadMemberForm()
        {
            SQLiteConnection connection = new SQLiteConnection($@"Data Source={ConnectionString()}; Version = 3");
            SQLiteCommand cmd = new SQLiteCommand("SELECT Members.Id, Members.FirstName, Members.LastName, Members.FatherName, Members.MotherName, Members.PhoneNumber, Members.AffiliationDate, Members.Address, Schools.name , Members.Description FROM[Members] JOIN [Schools] ON Members.SchoolId = Schools.Id", connection);
            connection.Open();
            SQLiteDataReader data = cmd.ExecuteReader();
            DataTable table = new DataTable();
            table.Load(data);
            connection.Close();
            return table;
        }
        public void LoadComboBox()
        {
            List<School> schools = new List<School>();
            using (IDbConnection dbConnection = new SQLiteConnection($@"Data Source={ConnectionString()}; Version = 3"))
            {
                schools = dbConnection.Query<School>("SELECT * FROM Schools").ToList();
            }
            if (schools.Count > 0)
            {
                cbxSchool.DataSource = schools;
                cbxSchool.ValueMember = "Id";
                cbxSchool.DisplayMember = "name";
                cbxSchool.AutoCompleteSource = AutoCompleteSource.ListItems;
            }
            else { }
        }
        private void MemberForm_Load(object sender, EventArgs e)
        {
            LoadComboBox();
            cbxSchool.SelectedIndex = 0;
            Filldgv(LoadMemberForm(), dgv);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (!ControlIsValid())
            {
                MessageBox.Show("يرجى تعبئة الحقول الفارغة");
                return;
            }
            else
            {
                SQLiteConnection connection = new SQLiteConnection($@"Data Source={ConnectionString()}; Version = 3");
                using (IDbConnection dbConnection = new SQLiteConnection($@"Data Source={ConnectionString()}; Version = 3"))
                {
                    int Id = Convert.ToInt32(txtId.Text);
                    bool exist = dbConnection.ExecuteScalar<bool>("select count(1) from Schools where Id = @Id", new { Id });
                    if (exist)
                    {
                        MessageBox.Show("يرجى تغيير رقم العضو", "");
                        return;
                    }
                }
                string InsertCommand = "Insert into [Members] values(@Id,@FirstName,@LastName,@FatherName,@MotherName,@PhoneNumber,@AffiliationDate,@Address,@SchoolId,@Description)";
                SQLiteCommand cmd = new SQLiteCommand(InsertCommand, connection);
                string Months = dtp.Value.Month.ToString();
                string Days = dtp.Value.Day.ToString();
                string Year = dtp.Value.Year.ToString();
                cmd.Parameters.AddWithValue("@Id", Convert.ToInt32(txtId.Text));
                cmd.Parameters.AddWithValue("@FirstName", Regex.Replace(txtName.Text, @"\s+", " "));
                cmd.Parameters.AddWithValue("@LastName", Regex.Replace(txtLastName.Text, @"\s+", " "));
                cmd.Parameters.AddWithValue("@FatherName", Regex.Replace(txtFatherName.Text, @"\s+", " "));
                cmd.Parameters.AddWithValue("@MotherName", Regex.Replace(txtMotherName.Text, @"\s+", " "));
                cmd.Parameters.AddWithValue("@PhoneNumber", Regex.Replace(txtPhoneNumber.Text, @"\s+", ""));
                cmd.Parameters.AddWithValue("@AffiliationDate", $"{Year}-{Months}-{Days}");
                cmd.Parameters.AddWithValue("@Address", Regex.Replace(txtAddress.Text, @"\s+", " "));
                cmd.Parameters.AddWithValue("@SchoolId", cbxSchool.SelectedIndex + 1);
                cmd.Parameters.AddWithValue("@Description", Regex.Replace(txtDescription.Text, @"\s+", " "));
                connection.Open();
                cmd.ExecuteNonQuery();
                connection.Close();
                Filldgv(LoadMemberForm(), dgv);
                SchoolForm frm = new SchoolForm();
                Filldgv(frm.GetSchools(), frm.dgv);
                btnReset.PerformClick();
                MessageBox.Show("تمت إضافة العضو");
            }
        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            txtName.Clear();
            txtLastName.Clear();
            txtId.Clear();
            txtMotherName.Clear();
            txtAddress.Clear();
            txtFatherName.Clear();
            txtPhoneNumber.Clear();
            dtp.Value = DateTime.Now;
            cbxSchool.SelectedIndex = 0;
            txtAddress.Clear(); txtDescription.Text = "لا يوجد وصف";
            txtId.ReadOnly = false;
            btnAdd.Enabled = true;
        }

        private void btnExport_Click(object sender, EventArgs e)
        {
            DataTable Memebertable = new DataTable();
            Memebertable = GetFromDataGridView(dgv);
            using (SaveFileDialog sfd = new SaveFileDialog() { Filter = "مصنف اكسيل|*.xlsx", InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) })
            {
                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        using (XLWorkbook workbook = new XLWorkbook())
                        {
                            workbook.Worksheets.Add(Memebertable, "الأعضاء");
                            workbook.SaveAs(sfd.FileName);
                        }
                        MessageBox.Show("تمّ تصدير البيانات إلى جدول اكسيل", "نجاح", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    catch (Exception ex)
                    {

                        MessageBox.Show(ex.Message);
                    }
                }
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("هل تريد حذف هذا السجل", "", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
            {
                SQLiteConnection connection = new SQLiteConnection($@"Data Source={ConnectionString()}; Version = 3");
                string DeleteCommand = "DELETE FROM Members WHERE Id =@Id";
                SQLiteCommand cmd = new SQLiteCommand(DeleteCommand, connection);
                cmd.Parameters.AddWithValue("@Id", Convert.ToInt32(txtId.Text));
                connection.Open();
                cmd.ExecuteNonQuery();
                connection.Close();
                Filldgv(LoadMemberForm(), dgv);
                SchoolForm frm = new SchoolForm();
                Filldgv(frm.GetSchools(), frm.dgv);
                btnReset.PerformClick();
                MessageBox.Show("تمت عملية الحذف بنجاح", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void dgv_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            txtId.ReadOnly = true;
            btnAdd.Enabled = false;
            txtId.Text = dgv.Rows[e.RowIndex].Cells[0].FormattedValue.ToString();
            txtName.Text = dgv.Rows[e.RowIndex].Cells[1].FormattedValue.ToString();
            txtLastName.Text = dgv.Rows[e.RowIndex].Cells[2].FormattedValue.ToString();
            txtFatherName.Text = dgv.Rows[e.RowIndex].Cells[3].FormattedValue.ToString();
            txtMotherName.Text = dgv.Rows[e.RowIndex].Cells[4].FormattedValue.ToString();
            txtPhoneNumber.Text = dgv.Rows[e.RowIndex].Cells[5].FormattedValue.ToString();
            dtp.Value = Convert.ToDateTime(dgv.Rows[e.RowIndex].Cells[6].Value);
            txtAddress.Text = dgv.Rows[e.RowIndex].Cells[7].FormattedValue.ToString();
            cbxSchool.Text = dgv.Rows[e.RowIndex].Cells[8].FormattedValue.ToString();
            txtDescription.Text = dgv.Rows[e.RowIndex].Cells[9].FormattedValue.ToString();
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            txtId.ReadOnly = false;
            btnAdd.Enabled = true;
            string UpdateCommand = @"UPDATE [Members] SET FirstName=@FirstName ,LastName=@LastName,FatherName=@FatherName , MotherName=@MotherName,PhoneNumber=@PhoneNumber,AffiliationDate=@AffiliationDate,Address=@Address,SchoolId=@SchoolId,Description=@Description WHERE Id=@Id";
            SQLiteConnection connection = new SQLiteConnection($@"Data Source={ConnectionString()}; Version = 3");
            SQLiteCommand cmd = new SQLiteCommand(UpdateCommand,connection);
            cmd.Parameters.AddWithValue("@FirstName", txtName.Text);
            cmd.Parameters.AddWithValue("@FatherName", txtFatherName.Text);
            cmd.Parameters.AddWithValue("@MotherName", txtMotherName.Text);
            cmd.Parameters.AddWithValue("@LastName", txtLastName.Text);
            cmd.Parameters.AddWithValue("@PhoneNumber", txtPhoneNumber.Text);
            string Months = dtp.Value.Month.ToString();
            string Days = dtp.Value.Day.ToString();
            string Year = dtp.Value.Year.ToString();
            cmd.Parameters.AddWithValue("@AffiliationDate", $"{Year}-{Months}-{Days}");
            cmd.Parameters.AddWithValue("@Address", txtAddress.Text);
            cmd.Parameters.AddWithValue("@SchoolId", cbxSchool.SelectedIndex+1);
            cmd.Parameters.AddWithValue(@"Description", txtDescription.Text);
            cmd.Parameters.AddWithValue("@Id", Convert.ToInt32(txtId.Text));
            connection.Open();
            cmd.ExecuteNonQuery();
            connection.Close();
            Filldgv(LoadMemberForm(), dgv);
            SchoolForm frm = new SchoolForm();
            Filldgv(frm.GetSchools(), frm.dgv);
            btnReset.PerformClick();
            MessageBox.Show("تمت تعديل بيانات العضو ","نجاح",MessageBoxButtons.OK,MessageBoxIcon.Information);
        }

        private void txtId_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void txtName_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsLetter(e.KeyChar) && !(char.IsControl(e.KeyChar)) && !char.IsWhiteSpace(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void txtPhoneNumber_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar) && !char.IsWhiteSpace(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtSearch.Text))
                Filldgv(LoadMemberForm(), dgv);
            else
            {
                SQLiteConnection connection = new SQLiteConnection($@"Data Source={ConnectionString()}; Version = 3");
                string search = Regex.Replace(txtSearch.Text, @"\s+", " ");
                SQLiteCommand cmd = new SQLiteCommand("SELECT Members.Id, Members.FirstName, Members.LastName, Members.FatherName, Members.MotherName, Members.PhoneNumber, Members.AffiliationDate, Members.Address, Schools.name , Members.Description FROM[Members] JOIN [Schools] ON Members.SchoolId = Schools.Id WHERE Members.FirstName like '%"+search+"%'", connection);
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

