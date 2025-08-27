﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymMgmt.Application.Common.Exceptions
{
    public class ApplicationLayerException : Exception
    {
        public ApplicationLayerException()
        {
        }

        public ApplicationLayerException(string? message)
            : base(message)
        {
        }

        public ApplicationLayerException(string? message, Exception? innerException)
            : base(message, innerException)
        {
        }
    }
}
