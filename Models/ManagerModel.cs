using Entity_Framework_Core.EfContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity_Framework_Core.Models
{
    public class ManagerModel
    {
        public Guid Id { get; init; }
        public String Surname { get; set; }
        public String Name { get; set; }
        public String Secname { get; set; }
        public IdName MainDep { get; set; }
        public IdName? SecDep { get; set; }
        public IdName? Chief { get; set; }

        public List<IdName> Departments { get; set; }
        public List<IdName> Chiefs { get; set; }

        //public DateTime? Birthday { get; set; }
        //public DateTime? DeleteDt { get; set; }

        public static ManagerModel FromEntity(Manager entity)
        {
            ManagerModel model = new()
            {
                Id = entity.Id,
                Surname = entity.Surname,
                Name = entity.Name,
                Secname = entity.Secname,

                MainDep = new IdName() { 
                    Id = entity.MainDepartment.Id, 
                    Name = entity.MainDepartment.Name 
                },

                SecDep = entity.SecondaryDepartment == null ? null 
                : new IdName() {
                    Id = entity.SecondaryDepartment.Id,
                    Name = entity.SecondaryDepartment.Name
                },

                Chief = entity.Chierf == null ? null
                : new IdName()
                {
                    Id = entity.Chierf.Id,
                    Name = entity.Chierf.Name
                }
            };

            model.Departments = 
                App.EfDataContext.Departments
                .Where(d => d.DeleteDt == null)
                .Select(d => new IdName() { Id = d.Id, Name = d.Name })
                .ToList();

            model.Chiefs =
                App.EfDataContext.Managers
                .Where(m => m.DeleteDt == null)
                .Select(m => new IdName() { Id = m.Id, Name = m.Name })
                .ToList();

            return model;
        }
    }
}
