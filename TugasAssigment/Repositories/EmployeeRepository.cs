using Microsoft.EntityFrameworkCore;
using Org.BouncyCastle.Pqc.Crypto.Lms;
using System.Text.RegularExpressions;
using TugasAssigment.Context;
using TugasAssigment.Model;
using TugasAssigment.ViewModels;

namespace TugasAssigment.Repositories
{
    public class EmployeeRepository : IEmployeRepository
    {
        private readonly MyContext context;
        private int nomor = 0;

        public EmployeeRepository(MyContext context)
        {
            this.context = context;
        }
        public int Delete(string NIK)
        {
            var entity = context.Employees.Find(NIK);
            entity.IsActive = false;
            var result = context.SaveChanges();
            return result;
        }

        public IEnumerable<GetEmployeeAndDepartmentVM> Get()
        {
            return getEmployee().ToList();
        }

            public IQueryable<GetEmployeeAndDepartmentVM> getEmployee()
            {
                var data = context.Employees
                    .Join(context.departments,
                            ed => ed.Dept_id,
                            dept => dept.Dept_id,
                            (ed, dept) => new GetEmployeeAndDepartmentVM
                            {
                                NIK = ed.NIK,
                                FirstName = ed.FirstName,
                                LastName = ed.LastName,
                                Email = ed.Email,
                                PhoneNumber = ed.Phone,
                                Address = ed.Address,
                                IsActive = ed.IsActive,
                                department = new DepartmentWithIdVM
                                {
                                    Dept_ID = dept.Dept_id,
                                    Name = dept.Name_Dept
                                }
                            });
                return data;
            }

        public Employee Get(string NIK)
        {
            var result = context.Employees.Find(NIK);
            return result;
        }

        public int Insert(GetEmployeeData Employee)
        {
            string date = DateTime.Now.ToString("ddMMyy");
            string newNIK = "";

            // cek data terakhir di database
            var lastData = context.Employees.OrderBy(data => data.NIK).LastOrDefault();
            if (lastData == null)
            {
                // kalau ternyata gak ada data di database, otomatis urutan 001
                newNIK = date + "001";
            }
            else
            {
                // ada data terakhir, ambil 3 karakter string dari NIK (nomor urut)
                var nikLastData = lastData.NIK;
                string lastThree = nikLastData.Substring(nikLastData.Length - 3);

                // convert jadi int terus tambah satu
                int nextSequence = int.Parse(lastThree) + 1;
                newNIK = date + nextSequence.ToString("000"); // convert jadi string
            }

            // generate email FirstNameLastName@berca.co.id (tambahin angka di belakangnya kalau ada nama yang sama)
            string domain = "@berca.co.id";
            string fullName = Employee.FirstName + Employee.LastName;
            string newEmail = "";

            // cek apakah email sudah digunakan
            var emailData = context.Employees.Where(e => e.Email.Contains(fullName)).OrderBy(data => data.NIK).LastOrDefault(); ;
            if (emailData == null)
            {
                newEmail = fullName + domain;
            }
            else
            {
                // pisahkan nama email dengan domainnya
                string[] emailSplit = Regex.Split(emailData.Email, "@");
                string emailName = (string)emailSplit[0];

                // ambil 3 karakter terakhir
                string lastThree = emailName.Substring(emailName.Length - 3);
                Console.WriteLine(lastThree);
                if (int.TryParse(lastThree, out int number))
                {
                    // jika 3 karakter terakhir adalah angka
                    int nextSequence = number + 1;
                    newEmail = fullName + nextSequence.ToString("000") + domain;
                }
                else
                {
                    newEmail = fullName + "001" + domain;
                }
            }

            Employee employeeData = new Employee
            {
                NIK = newNIK,
                FirstName = Employee.FirstName,
                LastName = Employee.LastName,
                Email = newEmail.ToLower(),
                Phone = Employee.Phone,
                Address = Employee.Address,
                IsActive = Employee.IsActive,
                Dept_id = Employee.Department_id
            };
            context.Employees.Add(employeeData);
            var save = context.SaveChanges();
            return save;
        }

        public int Update(string NIK, GetEmployeeData Employee)
        {
            //context.Entry(Employee).State = EntityState.Modified;
            var data = context.Employees.Find(NIK);
            data.FirstName = Employee.FirstName;
            data.LastName = Employee.LastName;
            data.Phone = Employee.Phone;
            data.Address = Employee.Address;
            data.IsActive = Employee.IsActive;
            data.Dept_id = Employee.Department_id;
            var result = context.SaveChanges();
            return result;
        }

        public bool CheckPhone(string phone, string NIK)
        {
            var data = context.Employees.AsNoTracking().SingleOrDefault(employee => employee.Phone == phone);
            if (data == null)
            {
                return false;
            }
            return true;
        }

        public bool CheckNIK(string NIK)
        {
            var data = context.Employees.AsNoTracking().SingleOrDefault(employee => employee.NIK == NIK);
            if (data == null)
            {
                return false;
            }
            return true;
        }

        private string GeneratedEmail(string FirstName, string LastName)
        {
            string baseEmail = $"{FirstName.ToLower()}.{LastName.ToLower()}";
            string generatedEmail = $"{baseEmail}{"@berca.co.id"}";
            int counter = 1;

            // Check if the generated username already exists in the database
            while (context.Employees.Any(u => u.Email == generatedEmail))
            {
                generatedEmail = $"{baseEmail}{counter:D3}"; // Append a three-digit number
                counter++;

                // To avoid infinite loops, you can add a maximum number of retries here.
                if (counter > 999)
                {
                    throw new Exception("Unable to generate a unique username.");
                }
            }

            return generatedEmail;
        }

        public IEnumerable<GetEmployeeAndDepartmentVM> GetActiveEmployee()
        {
            var employees = getEmployee().Where(e => e.IsActive == true).ToList();
            return (IEnumerable<GetEmployeeAndDepartmentVM>)employees;
        }

        public IEnumerable<GetEmployeeAndDepartmentVM> GetDeactiveEmployee()
        {
            var employees = getEmployee().Where(e => e.IsActive == false).ToList();
            return (IEnumerable<GetEmployeeAndDepartmentVM>)employees;
        }

        public IEnumerable<GetEmployeeAndDepartmentVM> GetActiveEmployeePerDepartment(string department_id)
        {
            var employees = getEmployee().Where(e => e.IsActive == true && e.department.Dept_ID == department_id).ToList();
            return (IEnumerable<GetEmployeeAndDepartmentVM>)employees;
        }

        public IEnumerable<GetEmployeeAndDepartmentVM> GetDeactiveEmployeePerDepartment(string department_id)
        {
            var employees = getEmployee().Where(e => e.IsActive == false && e.department.Dept_ID == department_id).ToList();
            return (IEnumerable<GetEmployeeAndDepartmentVM>)employees;
        }

        //public IEnumerable<object> GetActiveEmployee()
        //{
        //    var employees = getEmployee().Where(e => e.IsActive == true).ToList();
        //    return (IEnumerable<GetEmployeeAndDepartmentVM>)employees;
        //}

        //public IEnumerable<object> GetDeactiveEmployee()
        //{
        //    var employees = getEmployee().Where(e => e.IsActive == false).ToList();
        //    return (IEnumerable<GetEmployeeAndDepartmentVM>)employees;
        //}
        //public IEnumerable<object> GetActiveEmployeePerDepartment(string department_id)
        //{
        //    var employees = getEmployee().Where(e => e.IsActive == true && e.department.Dept_id == department_id).ToList();
        //    return (IEnumerable<GetEmployeeAndDepartmentVM>)employees;
        //}
        //public IEnumerable<object> GetDeactiveEmployeePerDepartment(string department_id)
        //{
        //    var employees = getEmployee().Where(e => e.IsActive == false && e.department.Dept_id == department_id).ToList();
        //    return (IEnumerable<GetEmployeeAndDepartmentVM>)employees;
        //}



        //public IEnumerable<object> GetTotalActivePerDepartment()
        //{
        //    var deptCount = context.Employees.Where(e => e.IsActive == true).GroupBy(e => e.department.Name_Dept).Select(g => new GroupByCountDepartment
        //    {
        //        DepartmentName = g.Key,
        //        CountEmployee = g.Count(),
        //    }).ToList();
        //    return deptCount;
        //}

        //public IEnumerable<object> GetTotalDeactivePerDepartment()
        //{
        //    var deptCount = context.Employees.Where(e => e.IsActive == false).GroupBy(e => e.department.Name_Dept).Select(g => new GroupByCountDepartment
        //    {
        //        DepartmentName = g.Key,
        //        CountEmployee = g.Count(),
        //    }).ToList();
        //    return deptCount;
        //}
    }

    public interface IEmployeRepository
    {
        IEnumerable<GetEmployeeAndDepartmentVM> Get();
        Employee Get(string NIK);
        int Insert(GetEmployeeData Employee);
        int Update(string NIK, GetEmployeeData Employee);
        int Delete(string NIK);
        IEnumerable<GetEmployeeAndDepartmentVM> GetActiveEmployee();
        IEnumerable<GetEmployeeAndDepartmentVM> GetDeactiveEmployee();
        IEnumerable<GetEmployeeAndDepartmentVM> GetActiveEmployeePerDepartment(string department_id);
        IEnumerable<GetEmployeeAndDepartmentVM> GetDeactiveEmployeePerDepartment(string department_id);
    }
}
