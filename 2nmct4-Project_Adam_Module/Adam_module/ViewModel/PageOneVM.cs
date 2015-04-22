using Advantech.Adam;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Adam_module.ViewModel
{
    class PageOneVM : ObservableObject, IPage
    {
        BackgroundWorker bw = new BackgroundWorker();
        bool[] b = new bool[5];

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

        private bool[] _ventilator;
        public bool[] Ventilator
        {
            get { return _ventilator; }
            set { _ventilator = value; OnPropertyChanged("Ventilator"); }
        }

        public Modbus Mb;

        public PageOneVM()
        { 
            ModWinsCard mwc = new ModWinsCard();

            AdamSocket adso=new AdamSocket();
           
            Succeeded = adso.Connect(AdamType.Adam6000, "172.23.49.102",ProtocolType.Tcp);
            if (Succeeded)
            {
                
                Mb = new Modbus(adso);
                Ventilator = new bool[5];
                Mb.ReadCoilStatus(0x11, 5, out b);
                Ventilator = b;

                bw.WorkerReportsProgress = true;
                bw.WorkerSupportsCancellation = true;
              
                bw.DoWork += bw_DoWork;

                bw.RunWorkerCompleted += bw_RunWorkerCompleted;

                bw.RunWorkerAsync();
            }
        }

        void bw_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if(e.Cancelled==false){ 
                Ventilator =(bool[]) e.Result;
            Console.WriteLine(Ventilator[0]);
            bw.RunWorkerAsync();


            }
            

            
        }

        void bw_DoWork(object sender, DoWorkEventArgs e)
        {  
            bool[] c = new bool[5];
            bool[] d = new bool[5];
            
            Mb.ReadCoilStatus(0x11, 5, out c);
            BackgroundWorker worker = sender as BackgroundWorker;
           
            do{
                if ((worker.CancellationPending == true))
                {
                    e.Cancel = true;

                }

                Mb.ReadCoilStatus(0x11, 5, out d);

                if (!c.SequenceEqual(d))
                {
                    e.Result = d;
                    
                    break;
                } 
          

                
          }
            while(true);
           
            
            
        }
        public void Test()
        {
            
           
           
                
                Console.WriteLine(Ventilator[0]);
                if (Ventilator[0])
                {
                    Mb.ForceSingleCoil(0x11, false);

                }

                else { Mb.ForceSingleCoil(0x11, true); }
            

            
        }

        public ICommand CommandConnect
        {
            get{return new RelayCommand(Test);} 
        }
       

    }
}
