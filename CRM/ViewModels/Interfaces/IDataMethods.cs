using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SteinbachCRM.ViewModels.Interfaces
{
    public interface IDataMethods
    {
        bool isDirty { get; set; }
         bool SaveChanges();
         bool RejectChanges();
         event System.Action DoRejectChanges;
 
    }


}
