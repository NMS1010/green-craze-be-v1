﻿using green_craze_be_v1.Application.Model.Unit;
using green_craze_be_v1.Application.Model.Variant;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace green_craze_be_v1.Application.Specification.Variant
{
    public class VariantSpecification : BaseSpecification<Domain.Entities.Variant>
    {
        public VariantSpecification(GetVariantPagingRequest query, bool isPaging = false)
        {
            var keyword = query.Search;

            if (!string.IsNullOrEmpty(keyword))
            {
                Criteria = x => x.Name == keyword;
            }
            if (query.IsSortAccending)
            {
                if (query.ColumnName == nameof(Domain.Entities.Variant.Name))
                {
                    AddOrderBy(x => x.Name);
                }
                else if (query.ColumnName == nameof(Domain.Entities.Variant.CreatedAt))
                {
                    AddOrderBy(x => x.CreatedAt);
                }
                else if (query.ColumnName == nameof(Domain.Entities.Variant.UpdatedAt))
                {
                    AddOrderBy(x => x.UpdatedAt);
                }
                else
                {
                    AddOrderBy(x => x.Id);
                }
            }
            else
            {
                if (query.ColumnName == nameof(Domain.Entities.Variant.Name))
                {
                    AddOrderByDescending(x => x.Name);
                }
                else if (query.ColumnName == nameof(Domain.Entities.Variant.CreatedAt))
                {
                    AddOrderByDescending(x => x.CreatedAt);
                }
                else if (query.ColumnName == nameof(Domain.Entities.Variant.UpdatedAt))
                {
                    AddOrderByDescending(x => x.UpdatedAt);
                }
                else
                {
                    AddOrderByDescending(x => x.Id);
                }
            }
            if (!isPaging) return;
            int skip = (query.PageIndex - 1) * query.PageSize;
            int take = query.PageSize;
            ApplyPaging(take, skip);
        }
    }
}