using System;
using System.Collections.Generic;
using System.Text;

namespace ExceptionTracker.Test.Models
{
    public class FooBar
    {
        public string FooName { get; set; }

        public decimal FooValue { get; set; }

        public DateTime FooTime { get; set; }

        public IEnumerable<Bar> BarItems { get; set; }
    }
}
