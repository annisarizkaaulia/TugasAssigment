using Microsoft.AspNetCore.Mvc;
using System.Net;
using TugasAssigment.Model;
using TugasAssigment.Repositories;
using TugasAssigment.ViewModels;
using System.Linq.Dynamic.Core;

namespace TugasAssigment.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        private readonly EmployeeRepository respository;
        private string NIK;

        public EmployeeController(EmployeeRepository respository)
        {
            this.respository = respository;
        }

        [HttpGet]
        public ActionResult Get()
        {
            var get = respository.Get();
            if (get.Count() != 0)
            {
                return Ok(new { status = HttpStatusCode.OK, message = "Data berhasil ditemukan.", data = get });
            }
            else
            {
                return NotFound(new { status = HttpStatusCode.NotFound, message = "Data tidak ditemukan." });
            }
        }
        [HttpGet("{NIK}")]
        public ActionResult Get(string NIK)
        {
            var get = respository.Get(NIK);
            if (get == null) //data ada di db
            {
                return NotFound(new { status = HttpStatusCode.NotFound, message = "Data tidak ditemukan." });
            }
            return Ok(new { 
                status = HttpStatusCode.OK, 
                message = "Data berhasil ditemukan.", 
                data = get });
        }
        [HttpPost]
        public virtual ActionResult Insert(GetEmployeeData employee)
        {
            if (employee == null)
            {
                //return BadRequest("data tidak boleh kosong");
                return BadRequest(new { status = HttpStatusCode.BadRequest, message = "Data tidak boleh kosong", Data = employee });
            }
            else if (respository.CheckPhone(employee.Phone, NIK) == true)
            {
                //return BadRequest("Nomor HP tidak boleh sama");
                return BadRequest(new
                {
                    status = HttpStatusCode.BadRequest,
                    message = "Nomor HP tidak boleh sama",
                    Data = respository.CheckPhone(employee.Phone, NIK)
                });
            }
            else
            {
                var result = respository.Insert(employee);
                //return Accepted("Data berhasil ditambahkan", result);
                return Ok(new
                {
                    status = HttpStatusCode.OK,
                    message = "Data berhasil ditemukan.",
                    data = result
                });
            }
        }
        [HttpPut]
        public virtual ActionResult Update(string NIK, GetEmployeeData employee)
        {
            if (respository.CheckNIK(NIK) == false)
            {
                return NotFound(new { status = HttpStatusCode.NotFound, message = "NIK tidak ditemukan." });
            }
            else if (respository.CheckPhone(employee.Phone, NIK) == true)
            {
                return BadRequest(new { status = HttpStatusCode.BadRequest, message = "Nomor ponsel sudah terdaftar." });
            }

            respository.Update(NIK, employee);
            return Ok(new { status = HttpStatusCode.OK, message = "Data berhasil diubah." });
        }
        [HttpDelete]
        public virtual ActionResult Delete(string NIK)
        {
            if (respository.CheckNIK(NIK) == false)
            {
                return NotFound(new { status = HttpStatusCode.NotFound, message = "NIK tidak ditemukan." });
            }
            respository.Delete(NIK);
            return Ok(new { status = HttpStatusCode.OK, message = "Data berhasil dihapus." });
        }
        [HttpGet("GetActiveEmployee")]
        public virtual ActionResult GetActiveEmployee()
        {
            var data = respository.GetActiveEmployee(); // mengambil fungsi dari repositori
            if (data.Count() != 0)
            {
                return Ok(new { status = HttpStatusCode.OK, message = "Data berhasil ditemukan.", data = data });
            }
            else
            {
                return NotFound(new { status = HttpStatusCode.NotFound, message = "Data tidak ditemukan." });
            }
        }
        [HttpGet("GetDeactiveEmployee")]
        public virtual ActionResult GetDeactiveEmployee()
        {
            var get = respository.GetDeactiveEmployee();
            if (get.Count() != 0)
            {
                return StatusCode(200, new { status = HttpStatusCode.OK, message = get.Count() + " Data Ditemukan", Data = get });
            }
            else
            {
                return StatusCode(404, new { status = HttpStatusCode.NotFound, message = "Data Tidak Ditemukan" });
            }
        }

        [HttpGet("GetActiveEmployeePerDepartment")]
        public virtual ActionResult GetActiveEmployeePerDepartment(string DeptId)
        {
            var get = respository.GetActiveEmployeePerDepartment(DeptId);
            if (get.Count() != 0)
            {
                return StatusCode(200, new { status = HttpStatusCode.OK, message = get.Count() + " Data Ditemukan", Data = get });
            }
            else
            {
                return StatusCode(404, new { status = HttpStatusCode.NotFound, message = "Data Tidak Ditemukan" });
            }
        }
        [HttpGet("GetDeactiveEmployeePerDepartment")]
        public virtual ActionResult GetDeactiveEmployeePerDepartment(string DeptId)
        {
            var get = respository.GetDeactiveEmployeePerDepartment(DeptId);
            if (get.Count() != 0)
            {
                return StatusCode(200, new { status = HttpStatusCode.OK, message = get.Count() + " Data Ditemukan", Data = get });
            }
            else
            {
                return StatusCode(404, new { status = HttpStatusCode.NotFound, message = "Data Tidak Ditemukan" });
            }
        }

        //[HttpGet("GetTotalActivePerDepartment")]
        //public virtual ActionResult GetTotalActivePerDepartment()
        //{
        //    var get = respository.GetTotalActivePerDepartment();
        //    if (get != null)
        //    {
        //        return StatusCode(200, new { status = HttpStatusCode.OK, message = get });
        //    }
        //    else
        //    {
        //        return StatusCode(404, new { status = HttpStatusCode.NotFound, message = "Data Tidak Ditemukan" });
        //    }
        //}

        //[HttpGet("GetTotalDeactivePerDepartment")]
        //public virtual ActionResult GetTotalDeactivePerDepartment()
        //{
        //    var get = respository.GetTotalDeactivePerDepartment();
        //    if (get != null)
        //    {
        //        return StatusCode(200, new { status = HttpStatusCode.OK, message = get });
        //    }
        //    else
        //    {
        //        return StatusCode(404, new { status = HttpStatusCode.NotFound, message = "Data Tidak Ditemukan" });
        //    }
        //}

        [HttpGet("TestCors")]
        public ActionResult TestCors()
        {
            return Ok("Test Cors berhasil");
        }

        [HttpPost("Paging")]
        public ActionResult GetEmployeeList()
        {
            int totalRecord = 0;
            int filterRecord = 0;
            var draw = Request.Form["draw"].FirstOrDefault();
            var sortColumn = Request.Form["columns[" + Request.Form["order[0][column]"].FirstOrDefault() + "][name]"].FirstOrDefault();
            var sortColumnDirection = Request.Form["order[0][dir]"].FirstOrDefault();
            var searchValue = Request.Form["search[value]"].FirstOrDefault();
            int pageSize = Convert.ToInt32(Request.Form["length"].FirstOrDefault() ?? "0");
            int skip = Convert.ToInt32(Request.Form["start"].FirstOrDefault() ?? "0");
            var data = respository.Get().AsQueryable();
            //get total count of data in table
            totalRecord = data.Count();
            // search data when search value found
            if (!string.IsNullOrEmpty(searchValue))
            {
                data = data.Where(x => x.FirstName.ToLower().Contains(searchValue.ToLower()));
            }
            // get total count of records after search
            filterRecord = data.Count();
            //sort data
            if (!string.IsNullOrEmpty(sortColumn) && !string.IsNullOrEmpty(sortColumnDirection)) data = data.OrderBy(sortColumn + " " + sortColumnDirection);
            //pagination
            var empList = data.Skip(skip).Take(pageSize).ToList();
            var returnObj = new
            {
                draw = draw,
                recordsTotal = totalRecord,
                recordsFiltered = filterRecord,
                data = empList
            };
            return Ok(returnObj);
        }
    }
}
