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
using System.Windows;
using System.IO.Ports;
using System.Threading;

namespace Adam_module.ViewModel
{
   class PageOneVM : ObservableObject, IPage
   {
       #region properties
       

        BackgroundWorker bw;
        bool[] b = new bool[5];
        ApplicationVM appvm = App.Current.MainWindow.DataContext as ApplicationVM;
        private List<SmartCard.Roles> _roles;

        public List<SmartCard.Roles> Roles
        {
            get { return _roles; }
            set { _roles = value; OnPropertyChanged("Roles"); }
        }

        private SmartCard.Roles _selectedItem;

        public SmartCard.Roles SelectedItem
        {
            get { return _selectedItem; }
            set { _selectedItem = value; OnPropertyChanged("SelectedItem"); }
        }
        

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

        private bool[] _devices;
        public bool[] Devices
        {
            get { return _devices; }
            set { _devices = value; OnPropertyChanged("Devices"); }
        }

        private bool _admin;

        public bool Admin
        {
            get { return _admin; }
            set { _admin = value; OnPropertyChanged("Admin"); }
        }
        

        public Modbus Mb;

        private int[] _buttons;
        public int[] Buttons
        {
            get { return _buttons; }
            set { _buttons = value; OnPropertyChanged("Devices"); }
        }
        public SerialPort serialPort;

        bool ClickedButton = false;

        private SmartCard.Roles _loggedInUser;

        public SmartCard.Roles LoggedInUser
        {
            get { return _loggedInUser; }
            set { _loggedInUser = value; OnPropertyChanged("LoggedInUser"); }
        }
        
        
       #endregion

        public PageOneVM()
        {
            try
            {
                //ConnectToSerialPort();
                LoggedInUser = appvm.huidigeGebruiker;
                ConnectADAM();
                Roles = new List<SmartCard.Roles>();
                Roles.Add(SmartCard.Roles.Administrator);
                Roles.Add(SmartCard.Roles.Staff);
                Roles.Add(SmartCard.Roles.User);
                if (appvm.huidigeGebruiker == SmartCard.Roles.Administrator)
                {
                    Admin = true;
                }
            }
            catch (Exception)
            {
                
                throw;
            }
        }
       public void ConnectADAM()
       { try
                {
           
           
                    
                    
                    Buttons = new int[5];
                    for (int i = 0; i < 5; i++)
                    {
                        Buttons[i] = i;
                    }
                    ModWinsCard mwc = new ModWinsCard();

                    AdamSocket adso = new AdamSocket();

                    Succeeded = adso.Connect(AdamType.Adam6000, "172.23.49.102", ProtocolType.Tcp);
                    if (Succeeded)
                    {

                        Mb = new Modbus(adso);
                         Devices = new bool[5];
                        Mb.ReadCoilStatus(0x11, 5, out b);
                        Devices = b;
                        bw = new BackgroundWorker();
                        bw.WorkerReportsProgress = true;
                        bw.WorkerSupportsCancellation = true;

                        bw.DoWork += bw_DoWork;

                        bw.RunWorkerCompleted += bw_RunWorkerCompleted;

                        bw.RunWorkerAsync();
                    }  }
                catch (Exception ex)
                {

                    MessageBox.Show(ex.InnerException.Message);
                }

       }
        void bw_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            BackgroundWorker worker = sender as BackgroundWorker;
            try
            {
                if (e.Cancelled == false)
                {
                    Devices = (bool[])e.Result;
                    
                    worker.RunWorkerAsync();


                }
                
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.InnerException.Message);
            }
            

            
        }

        void bw_DoWork(object sender, DoWorkEventArgs e)
        {
            if (ClickedButton == false) { 
            bool[] c = new bool[5];
            bool[] d = new bool[5];
            
           
            BackgroundWorker worker = sender as BackgroundWorker;
           
            do{
                if ((worker.CancellationPending == true))
                {
                    e.Cancel = true;

                }

                Mb.ReadCoilStatus(0x11, 5, out d);
               
                if (!Devices.SequenceEqual(d))
                {
                    e.Result = d;
                   
                    
                    break;
                } 
          

                 Thread.Sleep(1000); 
          }
            while(true);
            
           
            }
            else {}
            
        }
        public void Test(int i)
        {
           
           
            //bw = null;
            ClickedButton = true;


            if (Devices[i])
            {

                switch (i)
                {
                    case 0: Mb.ForceSingleCoil(0x11, false);

                        break;

                    case 1: Mb.ForceSingleCoil(0x12, false);

                        break;

                    case 2: Mb.ForceSingleCoil(0x13, false);

                        break;

                    case 3: Mb.ForceSingleCoil(0x14, false);

                        break;

                    case 4: Mb.ForceSingleCoil(0x15, false);

                        break;
                }

                ClickedButton = false;

            }



            else
            {
                switch (i)
                {
                    case 0: Mb.ForceSingleCoil(0x11, true);

                        break;

                    case 1: Mb.ForceSingleCoil(0x12, true);

                        break;

                    case 2: Mb.ForceSingleCoil(0x13, true);

                        break;

                    case 3: Mb.ForceSingleCoil(0x14, true);

                        break;

                    case 4: Mb.ForceSingleCoil(0x15, true);

                        break;
                }
                ClickedButton = false;

            }

                
                
            

            
        }

        public ICommand CommandConnect
        {
            get { return new RelayCommand<int>(Test); } 
        }

        public ICommand LogoutCommand
        {
            get { return new RelayCommand(Logout); }
        }
        public void Logout()
        {
            appvm.ChangePage(new LoginVM());
            bw.CancelAsync();
        }
        public ICommand NewCardCommand
        {
            get { return new RelayCommand(NewCard); }
        }
       
        public void NewCard()
        {
             SmartCard.CreateNewCard(SelectedItem);
            
            
 
        }

      
       

    }
}
