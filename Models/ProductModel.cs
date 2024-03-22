using Entity_Framework_Core.EfContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity_Framework_Core.Models
{
    public class ProductModel
    {
        public Guid Id { get; private set; }
        public String Name { get; set; }
        public double Price { get; set; }

        public static ProductModel FromEntity(Product entity)
        {
            return new()
            {
                Id = entity.Id,
                Name = entity.Name,
                Price = entity.Price,
            };
        }
    }
}
