﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace green_craze_be_v1.Application.Dto
{
    public class OrderItemDto : BaseAuditableDto<long>
    {
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal TotalPrice { get; set; }
        public string ProductSlug { get; set; }
        public string ProductUnit { get; set; }
        public string ProductImage { get; set; }
        public string ProductName { get; set; }
        public string VariantName { get; set; }
        public int VariantQuantity { get; set; }
    }
}