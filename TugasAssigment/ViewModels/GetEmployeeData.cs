using TugasAssigment.Model;

namespace TugasAssigment.ViewModels
{
    public class GetEmployeeData
    {
        public string NIK { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public bool IsActive { get; set; } // sebagai tanda karyawan masih aktif atau tidak
        public string Department_id { get; set; } // sebagai foreign key untuk tabel departmen
    }

    public class GetEmployeeAndDepartmentVM
    {
        public string NIK { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Address { get; set; }
        public bool IsActive { get; set; }
        public DepartmentWithIdVM department { get; set; }
    }

    public class DepartmentWithIdVM
    {
        public string Dept_ID { get; set; }
        public string Name { get; set; }
    }
    public class GroupByCountDepartment
    {
        public string DepartmentName { get; set; }
        public int CountEmployee { get; set; }
    }
}
