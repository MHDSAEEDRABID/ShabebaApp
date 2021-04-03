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
        private DataTable LoadMemberForm()
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
        private void MemberForm_Load(object sender, EventArgs e)
        {
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
                Filldgv(LoadMemberForm(), dgv);
                btnReset.PerformClick();
                MessageBox.Show("تمت إضافة العضو");
            }
        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            txtName.Clear();
            txtLastName.Clear();
            txtId.Clear();
            txtAddress.Clear();
            txtFatherName.Clear();
            txtPhoneNumber.Clear();
            dtp.Value = DateTime.Now;
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
                using (IDbConnection dbConnection = new SQLiteConnection (ConnectionString()))
                {
                    int Id = Convert.ToInt32(txtId.Text);
                    dbConnection.Execute("DELETE FROM [Members] Where Id=@Id", new { Id });
                }
                Filldgv(LoadMemberForm(), dgv);
                btnReset.PerformClick();
                MessageBox.Show("تمت عملية الحذف بنجاح", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
    }
}
