using Microsoft.AspNetCore.Mvc;
using System.Net;
using TugasAssigment.Model;
using TugasAssigment.Repositories;

namespace TugasAssigment.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DepartmentController : ControllerBase
    {
        private readonly DepartmentRepository respository;
        public DepartmentController(DepartmentRepository respository)
        {
            this.respository = respository;
        }

        [HttpGet]
        public ActionResult Get()
        {
            var get = respository.Get();
            if (get == null) //data tidak ada di db
            {
                return NotFound(new
                {
                    status = HttpStatusCode.NotFound,
                    message = "Data tidak ditemukan."
                });
            }
            else
            {
                return Ok(new { status = HttpStatusCode.OK, message = "Data berhasil ditemukan.", data = get }); // data ada di db 
            }
        }
        [HttpGet("{Dept_id}")]
        public ActionResult Get(string Dept_id)
        {
            var get = respository.Get(Dept_id);
            if (get == null) //data ada di db
            {
                return NotFound(new { status = HttpStatusCode.NotFound, message = "Data tidak ditemukan." });
            }
            return Ok(new
            {
                status = HttpStatusCode.OK,
                message = "Data berhasil ditemukan.",
                data = get
            });
        }
        [HttpPost]
        public virtual ActionResult Insert(Department department)
        {
            var Dept_id = respository.Get(department.Dept_id);
            if (department == null)
            {
                //return BadRequest("data tidak boleh kosong");
                return BadRequest(new { status = HttpStatusCode.BadRequest, message = "Data tidak boleh kosong", Data = department });
            }
            else if (Dept_id != null)
            {
                //return BadRequest("NIK sudah ada");
                return BadRequest(new
                {
                    status = HttpStatusCode.BadRequest,
                    message = "Dept_id sudah ada",
                    Data = department
                });
            }
            else
            {
                var result = respository.Insert(department);
                //return Accepted("Data berhasil ditambahkan", result);
                return Ok(new
                {
                    status = HttpStatusCode.OK,
                    message = "Data berhasil ditambahkan.",
                    data = result
                });
            }
        }
        [HttpPut]
        public virtual ActionResult Update(Department department)
        {
            var result = respository.Update(department);
            //return Accepted("Data berhasil diubah", result);
            return Ok(new
            {
                status = HttpStatusCode.OK,
                message = "Data berhasil diubah.",
                data = result
            });
        }
        [HttpDelete]
        public virtual ActionResult Delete(string Dept_id)
        {
            var department = respository.Get(Dept_id);
            if (department == null)
            {
                //return BadRequest("Data tidak ditemukan");
                return NotFound(new
                {
                    status = HttpStatusCode.NotFound,
                    message = "Data tidak ditemukan.",
                    data = department
                });
            }
            var result = respository.Delete(Dept_id);
            //return BadRequest("Data berhasil dihapus");
            return Ok(new
            {
                status = HttpStatusCode.OK,
                message = "Data berhasil dihapus",
                Data = result
            });
        }
    }
}
