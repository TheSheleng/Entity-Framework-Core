﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity_Framework_Core.EfContext
{
    public class Sale
    {
        public Guid Id { get; set; }
        public int Quantity { get; set; }
        public Guid ProductId { get; set; }
        public Guid ManagerId { get; set; }
        public DateTime SaleDt { get; set; }
        public DateTime? DeleteDt { get; set; }

        public Manager Manager { get; set; }
        public Product Product { get; set; }
    }
}
