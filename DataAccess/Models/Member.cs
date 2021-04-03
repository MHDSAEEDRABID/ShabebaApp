using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Models
{
    public class Member
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string FatherName { get; set; }
        public string MotherName { get; set; }
        public string PhoneNumber { get; set; }
        public string AffiliationDate { get; set; }
        public string Address { get; set; }
        public int SchoolId { get; set; }
        public string Description { get; set; }
        public void FillMember(int Id, string FirstName, string FatherName, string MotherName, string LastName, string PhoneNumber, string AffiliationDate, string Address, int SchoolId, string Description)
        {
            this.Id = Id;
            this.FirstName = FirstName;
            this.FatherName = FatherName;
            this.MotherName = MotherName;
            this.LastName = LastName;
            this.PhoneNumber = PhoneNumber;
            this.AffiliationDate = AffiliationDate;
            this.Address = Address;
            this.SchoolId = SchoolId;
            this.Description = Description;
        }
    }
}
