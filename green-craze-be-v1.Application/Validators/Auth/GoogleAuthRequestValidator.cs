﻿using FluentValidation;
using green_craze_be_v1.Application.Model.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace green_craze_be_v1.Application.Validators.Auth
{
    public class GoogleAuthRequestValidator : AbstractValidator<GoogleAuthRequest>
    {
        public GoogleAuthRequestValidator()
        {
            RuleFor(x => x.GoogleToken).NotEmpty().NotNull();
        }
    }
}