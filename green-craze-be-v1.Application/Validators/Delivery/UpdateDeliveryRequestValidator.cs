﻿using FluentValidation;
using green_craze_be_v1.Application.Model.Delivery;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace green_craze_be_v1.Application.Validators.Delivery
{
    public class UpdateDeliveryRequestValidator : AbstractValidator<UpdateDeliveryRequest>
    {
        public UpdateDeliveryRequestValidator()
        {
            RuleFor(x => x.Price).NotEmpty().NotNull();
            RuleFor(x => x.Name).NotEmpty().NotNull();
            RuleFor(x => x.Description).NotEmpty().NotNull();
            RuleFor(x => x.Status).NotNull();
        }
    }
}