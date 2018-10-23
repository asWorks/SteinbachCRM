using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using DAL;
using SteinbachCRM.ViewModels;

namespace SteinbachCRM.ObjectCollections
{
    public class TestCollection :ObservableCollection<Firmen_TelefonViewModel>
    {



        protected override void InsertItem(int index, Firmen_TelefonViewModel item)
        {
            base.InsertItem(index, item);
        } 
    }
}
