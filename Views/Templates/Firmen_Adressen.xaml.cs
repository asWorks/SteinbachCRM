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
using System.Windows.Navigation;
using System.Windows.Shapes;
using CommonTools.Tools;
using SteinbachCRM.ObjectCollections;
using System.Collections;
using DAL;

namespace SteinbachCRM.Templates
{
    /// <summary>
    /// Interaction logic for Firmen_Adressen.xaml
    /// </summary>
    public partial class Firmen_Adressen : UserControl
    {

        LinkViewsource<UserControl> Eintraege;
        LinkViewsource<UserControl> AdressTyp;

        SteinbachEntities db;

        public Firmen_Adressen()
        {
            InitializeComponent();
            db = new SteinbachEntities();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {

            var query = new AuswahlEintraegeCollection(db.AuswahlEintraege.Where(g => g.Gruppe == "Land"), db);
            var Typ = new AuswahlEintraegeCollection(db.AuswahlEintraege.Where(g => g.Gruppe == "TypFirmenAdresse"), db);
            //var Typ = CommonTools.Tools.HelperTools.GetAuswahlEintraege("TypFirmenAdressen");

            Eintraege = new LinkViewsource<UserControl>(this, query, "EintraegeLandLookup");
            AdressTyp = new LinkViewsource<UserControl>(this,Typ,"TypLookup");

        }

     











    }
}
