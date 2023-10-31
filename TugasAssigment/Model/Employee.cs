using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace TugasAssigment.Model
{
    public class Employee
    {

        [Key]
        public string NIK { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public bool IsActive { get; set; } // sebagai tanda karyawan masih aktif atau tidak
        public string Dept_id { get; set; } // sebagai fk untuk tabel department
        [ForeignKey("Department")]
        public Department department { get; set; }
    }
}
