using System.ComponentModel.DataAnnotations;

namespace DemoWebApi.ViewModel
{
    public class DeptInfoVM
    {
        [Key]
        public string Name { get; set; }
        public int? Count { get; set; }
        public int? TotalSalary { get; set; }
    }
}
