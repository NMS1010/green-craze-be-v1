﻿using green_craze_be_v1.Application.Intefaces;

namespace green_craze_be_v1.Infrastructure.Services
{
    public class DateTimeService : IDateTimeService
    {
        public DateTime Current => DateTime.Now;
    }
}