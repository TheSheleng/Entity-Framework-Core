using Entity_Framework_Core.EfContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity_Framework_Core.Models
{
    public class DepartamentModel
    {
        public Guid Id { get; private set; }
        public string Name { get; set; }
        public string? InternationalName { get; set; }

        public static DepartamentModel FromEntity(Department entity)
        {
            return new()
            {
                Id = entity.Id,
                Name = entity.Name,
                InternationalName = entity.InternationalName,
            };
        }
    }
}
