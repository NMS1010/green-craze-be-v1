﻿using green_craze_be_v1.Application.Model.Paging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace green_craze_be_v1.Application.Model.ProductCategory
{
    public class GetProductCategoryPagingRequest : PagingRequest 
    { 
        public long? ParentCategoryId { get; set; }
    }
}
