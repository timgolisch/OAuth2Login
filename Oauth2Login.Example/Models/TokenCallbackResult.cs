﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MultipleOauth2Mvc.Models
{
    public class TokenCallbackResult
    {
        public string ErrorText { get; set; }

        public string RedirectUrl { get; set; }
    }
}