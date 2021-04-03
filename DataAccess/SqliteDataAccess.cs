using System;
using System.Collections.Generic;
using System.Linq;
using System.Configuration;
using System.Data;
using System.Data.SQLite;
using DataAccess.Models;
using Dapper;
using System.Windows.Forms;
namespace DataAccess
{
    public static class SqliteDataAccess
    {
        public static  List<MemberMapping> LoadMember(string connectionstring)
        {
            List<MemberMapping> members = new List<MemberMapping>();
            using (IDbConnection conn = new SQLiteConnection(connectionstring))
            {
                string sql = @"
SELECT Members.Id,Members.FirstName,Members.LastName,Members.FatherName,Members.MotherName,Members.PhoneNumber,Members.AffiliationDate,Members.Address, Schools.name as 'School',Members.Description
FROM [Members]
JOIN [Schools]
ON Members.SchoolId = Schools.Id";
                var result = conn.Query(sql).ToList();
                return members;
            }
        }
        public static List<Member> LoadPeople(string connectionstring)
        {
            List<Member> members = new List<Member>();
            using (IDbConnection conn = new SQLiteConnection(connectionstring))
            {
                var membe = conn.Query("SELECT * FROM Members");
            }
            return members;
        }
        public static  DataTable LoadMembers()
        {
            SQLiteConnection connection = new SQLiteConnection();
            SQLiteCommand cmd = new SQLiteCommand("SELECT * FROM Members",connection);
            connection.Open();
            SQLiteDataReader data = cmd.ExecuteReader();
            DataTable table = new DataTable();
            table.Load(data);
            connection.Close();
            return table;
        }
    }
}
