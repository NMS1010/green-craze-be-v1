﻿using green_craze_be_v1.Application.Model.Delivery;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace green_craze_be_v1.Application.Specification.Delivery
{
    public class DeliverySpecification : BaseSpecification<Domain.Entities.Delivery>
    {
        public DeliverySpecification(GetDeliveryPagingRequest request, bool isPaging = false)
        {
            var keyword = request.Search;
            if (!string.IsNullOrEmpty(keyword))
            {
                if (request.Status)
                {
                    Criteria = x => x.Status && (x.Name.ToLower().Contains(keyword)
                    || x.Price.ToString().Contains(keyword));
                }
                else
                {
                    Criteria = x => x.Name.ToLower().Contains(keyword)
                    || x.Price.ToString().Contains(keyword);
                }
            }
            else
            {
                if (request.Status)
                {
                    Criteria = x => x.Status;
                }
                else
                {
                    Criteria = x => true;
                }
            }
            if (string.IsNullOrEmpty(request.ColumnName))
                request.ColumnName = "CreatedAt";
            AddSorting(request.ColumnName, request.IsSortAscending);
            if (!isPaging) return;
            int skip = (request.PageIndex - 1) * request.PageSize;
            int take = request.PageSize;
            ApplyPaging(take, skip);
        }
    }
}