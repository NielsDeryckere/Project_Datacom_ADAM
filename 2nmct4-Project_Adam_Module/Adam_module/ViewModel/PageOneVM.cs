using Advantech.Adam;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Adam_module.ViewModel
{
    class PageOneVM : ObservableObject, IPage
    {
        public string Name
        {
            get { return "First page"; }
        }

        private string _demo;

        public string Demo
        {
            get { return _demo; }
            set { _demo = value; OnPropertyChanged("Demo"); }

        }

        private bool _succeeded;

        public bool Succeeded
        {
            get { return _succeeded; }
            set { _succeeded = value; OnPropertyChanged("Succeeded"); }
        }
        

      

        public void Test()
        {
            ModWinsCard mwc = new ModWinsCard();

            AdamSocket adso=new AdamSocket();
           
            Succeeded = adso.Connect(AdamType.Adam6000, "172.23.49.102",ProtocolType.Tcp);
            if(Succeeded)
            {

                Modbus Mb = new Modbus(adso);
                bool test=Mb.ForceSingleCoil(17, true);
                bool[] b;
                bool read = Mb.ReadCoilStatus(1, 1, out b);
                

                Console.WriteLine(test);
                Console.WriteLine("CoilStatus: "+b[0]);
                Console.WriteLine("Read Succesfull: " + read);
            }

            
        }

        public ICommand CommandConnect
        {
            get{return new RelayCommand(Test);} 
        }
       

    }
}
