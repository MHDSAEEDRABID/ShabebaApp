using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Models
{
    public class MemberMapping
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string FatherName { get; set; }
        public string MotherName { get; set; }
        public string PhoneNumber { get; set; }
        public string AffiliationDate { get; set; }
        public string Address { get; set; }
        public string School { get; set; }
        public string Description { get; set; }
    }
}
