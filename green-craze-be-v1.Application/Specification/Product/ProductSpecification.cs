﻿using green_craze_be_v1.Application.Common.Enums;
using green_craze_be_v1.Application.Model.Product;

namespace green_craze_be_v1.Application.Specification.Product
{
    public class ProductSpecification : BaseSpecification<Domain.Entities.Product>
    {
        public ProductSpecification()
        {
            AddInclude(x => x.Images);
            AddInclude(x => x.Category);
            AddInclude(x => x.Sale);
            AddInclude(x => x.Brand);
            AddInclude(x => x.Unit);
            AddInclude(x => x.Variants);
        }

        public ProductSpecification(long id) : base(x => x.Id == id)
        {
            AddInclude(x => x.Images);
            AddInclude(x => x.Category);
            AddInclude(x => x.Sale);
            AddInclude(x => x.Brand);
            AddInclude(x => x.Unit);
            AddInclude(x => x.Variants);
        }

        public ProductSpecification(string slug) : base(x => x.Slug == slug)
        {
            AddInclude(x => x.Images);
            AddInclude(x => x.Category);
            AddInclude(x => x.Sale);
            AddInclude(x => x.Brand);
            AddInclude(x => x.Unit);
            AddInclude(x => x.Variants);
        }

        public ProductSpecification(long categoryId, bool category = true) : base(x => x.Category.Id == categoryId)
        {
            AddInclude(x => x.Images);
            AddInclude(x => x.Category);
            AddInclude(x => x.Sale);
            AddInclude(x => x.Brand);
            AddInclude(x => x.Unit);
            AddInclude(x => x.Variants);
        }

        public ProductSpecification(long saleId, bool category = true, bool sale = true)
            : base(x => x.Sale.Id == saleId)
        {
            AddInclude(x => x.Images);
            AddInclude(x => x.Category);
            AddInclude(x => x.Sale);
            AddInclude(x => x.Brand);
            AddInclude(x => x.Unit);
            AddInclude(x => x.Variants);
        }

        public ProductSpecification(int limit, bool sortBy)
        {
            AddOrderByDescending(x => x.Sold);
            ApplyPaging(limit, 0);
            AddInclude(x => x.Variants);
        }

        public ProductSpecification(GetProductPagingRequest query, bool isPaging = false)
        {
            var keyword = query.Search;

            if (!string.IsNullOrEmpty(keyword))
            {
                if (query.Status)
                {
                    if (!string.IsNullOrEmpty(query.CategorySlug))
                    {
                        Criteria = x => x.Name.ToLower().Contains(keyword) && x.Category.Slug == query.CategorySlug && x.Status != PRODUCT_STATUS.INACTIVE;
                    }
                    else
                    {
                        Criteria = x => x.Name.ToLower().Contains(keyword) && x.Status != PRODUCT_STATUS.INACTIVE;
                    }
                }
                else
                {
                    if (!string.IsNullOrEmpty(query.CategorySlug))
                    {
                        Criteria = x => x.Name.ToLower().Contains(keyword) && x.Category.Slug == query.CategorySlug;
                    }
                    else
                    {
                        Criteria = x => x.Name.ToLower().Contains(keyword);
                    }

                }
            }
            else
            {
                if (query.Status)
                {

                    if (!string.IsNullOrEmpty(query.CategorySlug))
                    {
                        Criteria = x => x.Category.Slug == query.CategorySlug && x.Status != PRODUCT_STATUS.INACTIVE;
                    }
                    else
                    {
                        Criteria = x => true && x.Status != PRODUCT_STATUS.INACTIVE;
                    }
                }
                else
                {

                    if (!string.IsNullOrEmpty(query.CategorySlug))
                    {
                        Criteria = x => x.Category.Slug == query.CategorySlug;
                    }
                    else
                    {
                        Criteria = x => true;
                    }
                }
            }
            var columnName = query.ColumnName.ToLower();
            if (columnName == nameof(Domain.Entities.Product.Category).ToLower())
            {
                if (query.IsSortAscending)
                    AddOrderBy(x => x.Category.Name);
                else
                    AddOrderByDescending(x => x.Category.Name);
            }
            else
            {
                if (string.IsNullOrEmpty(query.ColumnName))
                    query.ColumnName = "Id";
                AddSorting(query.ColumnName, query.IsSortAscending);
            }

            AddInclude(x => x.Images);
            AddInclude(x => x.Category);
            AddInclude(x => x.Sale);
            AddInclude(x => x.Brand);
            AddInclude(x => x.Unit);
            AddInclude(x => x.Variants);
            if (!isPaging) return;
            int skip = (query.PageIndex - 1) * query.PageSize;
            int take = query.PageSize;
            ApplyPaging(take, skip);
        }
    }
}