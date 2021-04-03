using System;
using System.Collections.Generic;
using System.Linq;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Data.SQLite;
using DataAccess.Models;
using Dapper;

namespace DataAccess
{
    public static class SqliteDataAccess
    {
        public static  List<MemberMapping> LoadMembers()
        {
            List<MemberMapping> members = new List<MemberMapping>();
            using (IDbConnection conn = new SQLiteConnection(ConnectionString()))
            {
                string sql = @"
SELECT Members.Id,Members.FirstName,Members.FatherName,Members.MotherName,Members.LastName,Members.PhoneNumber,Members.AffiliationDate,Members.Address, Schools.name as N'School',Members.Description
FROM [Members]
JOIN [Schools]
ON Members.SchoolId = Schools.Id";
                members = conn.Query<MemberMapping>(sql).ToList();
                return members;
            }
        }
        public static List<Member> LoadPeople()
        {
            List<Member> members = new List<Member>();
            using (IDbConnection conn = new SQLiteConnection (ConnectionString()))
            {
                members.Add(new Member { Id = 1, FirstName = "saeed", FatherName = "frefef", MotherName = "grgr", LastName = "grrgrg", Address = "grgrgrg", AffiliationDate = "fefefef", PhoneNumber = "freger", SchoolId = 2, Description = "frgr" });
            }
            return members;
        }
        private static string ConnectionString()
        {
            return ConfigurationManager.ConnectionStrings["connection"].ConnectionString;
        }

    }
}
