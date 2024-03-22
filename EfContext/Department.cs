using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity_Framework_Core.EfContext
{
    public class Department
    {
        public Guid Id { get; set; }
        public String Name { get; set; }
        public DateTime? DeleteDt { get; set; }
        public string? InternationalName { get; set; }

        public List<Manager> MainManagers { get; set; }
        public IEnumerable<Manager> SecondaryManagers { get; set; }
    }
}