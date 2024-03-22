using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity_Framework_Core.Models
{
    public class IdName
    {
        public Guid Id { get; set; }
        public string Name { get; set; }

        public override string ToString() => Name;
    }
}
