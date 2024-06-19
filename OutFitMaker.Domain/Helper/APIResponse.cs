using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OutFitMaker.Domain.Helper
{
    public class APIResponse<T>
    {
        public int Status { get; set; }

        public string Message { get; set; }

        public T Data { get; set; }
    }
}
