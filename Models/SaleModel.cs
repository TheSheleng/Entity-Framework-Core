using Entity_Framework_Core.EfContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity_Framework_Core.Models
{
    public class SaleModel
    {
        public Guid Id { get; init; }
        public int Quantity { get; set; }
        public IdName Product { get; set; }
        public IdName Manager { get; set; }

        public List<IdName> Products { get; set; }
        public List<IdName> Managers { get; set; }

        public static SaleModel FromEntity(Sale entity)
        {
            SaleModel model = new()
            {
                Id = entity.Id,
                Quantity = entity.Quantity,

                Product = new IdName()
                {
                    Id = entity.Product.Id,
                    Name = entity.Product.Name
                },

                Manager = new IdName()
                {
                    Id = entity.Manager.Id,
                    Name = entity.Manager.Name
                },
            };

            model.Products =
                App.EfDataContext.Products
                .Where(p => p.DeleteDt == null)
                .Select(p => new IdName() { Id = p.Id, Name = p.Name })
                .ToList();

            model.Managers =
                App.EfDataContext.Managers
                .Where(m => m.DeleteDt == null)
                .Select(m => new IdName() { Id = m.Id, Name = m.Name })
                .ToList();

            return model;
        }
    }
}
