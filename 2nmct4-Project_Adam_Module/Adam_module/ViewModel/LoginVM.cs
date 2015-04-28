using GalaSoft.MvvmLight.CommandWpf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Adam_module.ViewModel
{
    class LoginVM : ObservableObject, IPage
    {
        ApplicationVM appvm = App.Current.MainWindow.DataContext as ApplicationVM;
        SmartCard.Roles cardRole;
        

        public string Name
        {
            get { return "First page"; }
        }

        private void ReadCard()
        {
            cardRole = SmartCard.ReadCard();
            if(cardRole != SmartCard.Roles.Reset)
            {
                appvm.huidigeGebruiker = cardRole;
                appvm.ChangePage(new PageOneVM());
            }
                
        }

        public ICommand ReadCardCommand
        {
            get { return new RelayCommand(ReadCard); }
        }
    }
}
