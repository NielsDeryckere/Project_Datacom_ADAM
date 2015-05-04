using GalaSoft.MvvmLight.CommandWpf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Adam_module.ViewModel
{
    class LoginVM : ObservableObject, IPage
    {
        ApplicationVM appvm = App.Current.MainWindow.DataContext as ApplicationVM;
        SmartCard.Roles cardRole;
        TimeSpan start = new TimeSpan(07, 0, 0);
        TimeSpan end = new TimeSpan(17, 0, 0);
        
        

        public string Name
        {
            get { return "First page"; }
        }

        private void ReadCard()
        {
            //TimeSpan now = DateTime.Now.TimeOfDay;
            //cardRole = SmartCard.ReadCard();
            //if (cardRole != SmartCard.Roles.Reset)
            //{
            //    switch(cardRole)
            //    {
            //        case SmartCard.Roles.Staff:
            //            if(start<now && end>now)
            //            {
            //                appvm.huidigeGebruiker = SmartCard.Roles.Staff;
            //                appvm.ChangePage(new PageOneVM());
            //            }
            //            else
            //            {
            //                MessageBox.Show("Je bent niet toegelaten buiten de afgesproken uren");
            //            }
            //            break;
            //        case SmartCard.Roles.Administrator:
            //            appvm.huidigeGebruiker = SmartCard.Roles.Administrator;
            //            appvm.ChangePage(new PageOneVM());
            //             break;

            //        case SmartCard.Roles.User:
            //            appvm.huidigeGebruiker = SmartCard.Roles.User;
            //            appvm.ChangePage(new PageOneVM());
            //            break;
            //        default: MessageBox.Show("Je kaart is niet geldig");
            //            break;
              // }
                appvm.huidigeGebruiker = SmartCard.Roles.Administrator;
                appvm.ChangePage(new PageOneVM());
           // }
                
        }

        public ICommand ReadCardCommand
        {
            get { return new RelayCommand(ReadCard); }
        }
       

    }
}
