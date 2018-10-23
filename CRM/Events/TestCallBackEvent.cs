using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SteinbachCRM.Events
{
    public class TestCallBackEvent
    {
        public int Value { get; set; }
        public int Return { get; set; }

        public TestCallBackEvent(int Value)
        {
            this.Value = Value;

        }
    }
}
