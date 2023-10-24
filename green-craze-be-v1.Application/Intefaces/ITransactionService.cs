﻿using green_craze_be_v1.Application.Dto;
using green_craze_be_v1.Application.Model.Paging;
using green_craze_be_v1.Application.Model.Transaction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace green_craze_be_v1.Application.Intefaces
{
    public interface ITransactionService
    {
        Task<PaginatedResult<TransactionDto>> GetListTransaction(GetTransactionPagingRequest request);

        Task<TransactionDto> GetTransaction(long id);
    }
}