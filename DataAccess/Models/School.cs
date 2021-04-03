using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Models
{
    public class School
    {
        public int Id { get; set; }
        public string name { get; set; }
        public string Address { get; set; }
        public string Manager { get; set; }
        public string ManagerPhone { get; set; }
        public string SchoolPhone { get; set; }
        public int NumberOfMembers { get; set; }
        public void FillData(int Id, string name, string Address, string Manager, string ManagerPhone, string SchoolPhone)
        {
            this.Id = Id;
            this.name = name;
            this.Address = Address;
            this.Manager = Manager;
            this.ManagerPhone = ManagerPhone;
            this.SchoolPhone = SchoolPhone;
        }
    }
}
