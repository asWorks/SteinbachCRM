using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using SteinbachCRM.ViewModels;
using SteinbachCRM.ViewModels.Interfaces;

namespace SteinbachCRM.Views
{
    /// <summary>
    /// Interaction logic for StandardPopupView.xaml
    /// </summary>
    public partial class StandardPopupView : Window 
    {

        StandardPopupViewModel vModel;
        public StandardPopupView()
        {
            InitializeComponent();
        }

        public StandardPopupView(int id,StandardPopupViewModel.EnumPopupType Type)
        {
            InitializeComponent();
            vModel = new StandardPopupViewModel(id, Type);
            vModel.DoRejectChanges += () => RejectChanges();
            Caliburn.Micro.ViewModelBinder.Bind(vModel, this, null);

        }


        public StandardPopupView(int id, StandardPopupViewModel.EnumPopupType Type,string titel,double height,double width)
        {
            InitializeComponent();
            this.Height = height;
            this.Width = width;
            this.Title = titel;
            vModel = new StandardPopupViewModel(id, Type);
            vModel.DoRejectChanges += () => RejectChanges();
            Caliburn.Micro.ViewModelBinder.Bind(vModel, this, null);
            

        }

        public StandardPopupView(int id, StandardPopupViewModel.EnumPopupType Type, string titel, double height, double width,DateTime Von,DateTime Bis)
        {
            InitializeComponent();
            this.Height = height;
            this.Width = width;
            this.Title = titel;

            vModel = new StandardPopupViewModel(id, Type,new Tools.TransferTimespan(Von,Bis));
            vModel.DoRejectChanges += () => RejectChanges();
            Caliburn.Micro.ViewModelBinder.Bind(vModel, this, null);

        }


        public StandardPopupView(StandardPopupViewModel.EnumPopupType Type,string titel,double height,double width, object AdditionalData)
        {
            InitializeComponent();
            this.Height = height;
            this.Width = width;
            this.Title = titel;

            vModel = new StandardPopupViewModel(Type, AdditionalData);
            vModel.DoRejectChanges += ()=> RejectChanges();

            Caliburn.Micro.ViewModelBinder.Bind(vModel, this, null);

        }


        public void RejectChanges()
        {
            this.Close();

        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {

            if (vModel.isDirty == true)
            {
                MessageBoxResult res =MessageBox.Show("Änderungen speichern ?","",MessageBoxButton.YesNoCancel) ;
                if (res == MessageBoxResult.Cancel )
                {
                    e.Cancel = true;
                }
                else if (res == MessageBoxResult.No)
                {

                }
                else
                {
                    vModel.SaveChanges();
                }
            }
        }


    
    }
}
