using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.Composition;
using Caliburn.Micro;


namespace SteinbachCRM.ViewModels
{
    [Export(typeof(TestViewModel))]
    public class TestViewModel : PropertyChangedBase
    {

        public TestViewModel()
        {


        }
    }
}
