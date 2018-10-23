using System.Windows;
using AutoMapper;
using DAL;
using SteinbachCRM.ViewModels;
using System.Globalization;

namespace SteinbachCRM
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {

        public App()
        {
            Application.Current.DispatcherUnhandledException += new System.Windows.Threading.DispatcherUnhandledExceptionEventHandler(Current_DispatcherUnhandledException);

            Mapper.CreateMap<Personen_Telefon, Firmen_TelefonViewModel>();
            Mapper.CreateMap<Firmen_TelefonViewModel, Personen_Telefon>();

            // Mapper.AssertConfigurationIsValid();

        }

        void Current_DispatcherUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {
#if DEBUG   // In debug mode do not custom-handle the exception, let Visual Studio handle it

    e.Handled = false;

#else

            CommonTools.Tools.ErrorMethods.ShowErrorMessageToUser(e.Exception, "");

#endif


        }

        private void Application_Startup(object sender, StartupEventArgs e)
        {
            FrameworkElement.LanguageProperty.OverrideMetadata(
                typeof(FrameworkElement),
                new FrameworkPropertyMetadata(
                System.Windows.Markup.XmlLanguage.GetLanguage(CultureInfo.CurrentUICulture.IetfLanguageTag)));
        }




    }


}
