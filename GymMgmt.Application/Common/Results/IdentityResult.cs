using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymMgmt.Application.Common.Results
{
    public class IdentityResult
    {
        public bool Succeeded { get; private set; }
        public IEnumerable<string> Errors { get; private set; }

        private IdentityResult(bool succeeded, IEnumerable<string>? errors = null)
        {
            Succeeded = succeeded;
            Errors = errors ?? Enumerable.Empty<string>();
        }

        public static IdentityResult Success() => new(true);
        public static IdentityResult Failure(params string[] errors) => new(false, errors);
    }
}
