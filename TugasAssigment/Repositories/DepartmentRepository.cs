using Microsoft.EntityFrameworkCore;
using Org.BouncyCastle.Pqc.Crypto.Lms;
using TugasAssigment.Context;
using TugasAssigment.Model;

namespace TugasAssigment.Repositories
{
    public class DepartmentRepository : IDepartmentRepository
    {
        private readonly MyContext context;

        public DepartmentRepository(MyContext context)
        {
            this.context = context;
        }
        public int Delete(string Dept_id)
        {
            var entity = context.departments.Find(Dept_id);
            if (entity != null)
            {
                context.departments.Remove(entity);
            }
            var result = context.SaveChanges();
            return result;
        }

        public IEnumerable<Department> Get()
        {
            return context.departments.ToList();
        }

        public Department Get(string Dept_id)
        {
            var result = context.departments.Find(Dept_id);
            return result;
        }

        public int Insert(Department department)
        {
            string Id = "";
            var lastData = context.departments.OrderBy(data => data.Dept_id).LastOrDefault();
            if (lastData == null)
            {
                // kalau ternyata gak ada data di database, otomatis urutan 001
                Id = "D" + "001";
            }
            else
            {
                // ada data terakhir, ambil 3 karakter string dari NIK (nomor urut)
                var lastId = lastData.Dept_id;
                string lastThree = lastId.Substring(lastId.Length - 3);

                // convert jadi int terus tambah satu
                int nextSequence = int.Parse(lastThree) + 1;
                Id = "D" + nextSequence.ToString("000"); // convert jadi string
            }

            var department1 = new Department
            {
                Dept_id = Id,
                Name_Dept = department.Name_Dept
            };
            context.departments.Add(department1);
            var save = context.SaveChanges();
            return save;
        }

        public int Update(Department department)
        {
            context.Entry(department).State = EntityState.Modified;
            var result = context.SaveChanges();
            return result;
        }
    }

    public interface IDepartmentRepository
    {
        IEnumerable<Department> Get();
        Department Get(string Dept_id);
        int Insert(Department department);
        int Update(Department department);
        int Delete(string Dept_id);
    }
}
