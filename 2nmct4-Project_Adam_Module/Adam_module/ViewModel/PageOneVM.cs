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

        private bool[] _devices=new bool[5]{false,false,false,false,false};
        public bool[] Devices
        {
            get { return _devices; }
            set { _devices = value; OnPropertyChanged("Devices"); }
        }

        public Modbus Mb;

        private int[] _buttons;
        public int[] Buttons
        {
            get { return _buttons; }
            set { _buttons = value; OnPropertyChanged("Devices"); }
        }
        public SerialPort serialPort;

        public PageOneVM()
        {
            //ConnectToSerialPort();
            ConnectADAM();

               
              
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
                        Mb.ReadCoilStatus(0x11, 4, out b);
                        Devices = b;

                        bw.WorkerReportsProgress = true;
                        bw.WorkerSupportsCancellation = true;

                        bw.DoWork += bw_DoWork;

                        bw.RunWorkerCompleted += bw_RunWorkerCompleted;

                        bw.RunWorkerAsync();
                    }  }
                catch (Exception ex)
                {

                    MessageBox.Show(ex.Message);
                }

       }
        void bw_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            try
            {
                if (e.Cancelled == false)
                {
                    Devices = (bool[])e.Result;
                    Console.WriteLine(Devices[0]);
                    bw.RunWorkerAsync();


                }
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
            

            
        }

        void bw_DoWork(object sender, DoWorkEventArgs e)
        {  
            bool[] c = new bool[5];
            bool[] d = new bool[5];
            
            Mb.ReadCoilStatus(0x11, 4, out c);
            BackgroundWorker worker = sender as BackgroundWorker;
           
            do{
                if ((worker.CancellationPending == true))
                {
                    e.Cancel = true;

                }

                Mb.ReadCoilStatus(0x11, 4, out d);

                if (!c.SequenceEqual(d))
                {
                    e.Result = d;
                    
                    break;
                } 
          

                
          }
            while(true);
            
           
            
            
        }
        public void Test(int i)
        {
            try
            {
                bool[] b = new bool[5];
                //Mb.ReadCoilStatus(0x11, 4, out b);
                //Devices = b;


                Console.WriteLine(Devices[0]);
                Console.WriteLine(Devices[1]);
                Console.WriteLine(Devices[2]);
                Console.WriteLine(Devices[3]);
                Console.WriteLine(Devices[4]);



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

                }
            }
            catch (Exception)
            {
                
                MessageBox.Show("Geen verbinding, maak opnieuw verbinding");
            }
                
            

            
        }

        public ICommand CommandConnect
        {
            get { return new RelayCommand<int>(Test); } 
        }

        public ICommand CommandConnectCOM
        {
            get { return new RelayCommand(ConnectToSerialPort); }
        }

       public void ConnectToSerialPort()
       {
           try
           {
               serialPort = new SerialPort();
               serialPort.PortName = "COM2";
               serialPort.BaudRate = 9600;
               serialPort.DataBits = 8;
               serialPort.Parity = Parity.None;
               serialPort.ReadTimeout = 300;
               serialPort.WriteTimeout = 300;
               serialPort.StopBits = StopBits.One;
               serialPort.Handshake = Handshake.None;
               serialPort.Open();
               if (serialPort.IsOpen == true)
               {
                   MessageBox.Show("Succesvol");
               }
               else
               {
                   MessageBox.Show("Niet succesvol");
               }
           }
           catch (Exception ex)
           {
               MessageBox.Show(ex.Message);
               
           }
           
       }

      
       

    }
}
