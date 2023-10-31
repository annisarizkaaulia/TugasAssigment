using System.ComponentModel.DataAnnotations;

namespace TugasAssigment.Model
{
    public class Department
    {
        [Key]
        public string Dept_id { get; set; }
        public string Name_Dept { get; set; }
    }
}
