using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkeletonGameMaker
{
    public class InputException : Exception
    {
        public object Param;

        public InputException(string message, object param) : base(message)
        {
            Param = param;
        }

        public InputException(string message) : base(message) { }
    }
}
