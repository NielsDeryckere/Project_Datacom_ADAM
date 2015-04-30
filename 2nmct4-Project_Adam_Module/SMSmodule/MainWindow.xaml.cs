using Advantech.Adam;
using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SMSmodule
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        SerialPort serial = new SerialPort();
        List<String> poortenLijst = new List<String>();
        
        public MainWindow()
        {
            InitializeComponent();

            poortenLijst = SerialPort.GetPortNames().ToList();
            for (int i = 0; i < poortenLijst.Count; i++)
            {
                cboCom.Items.Add(poortenLijst[i]);
            }
        }

        #region SerialConnection
        private void initSerialConnection()
        {
            serial.PortName = cboCom.SelectedItem.ToString();
            serial.BaudRate = Convert.ToInt32(txtBaudRate.Text);
            serial.Handshake = System.IO.Ports.Handshake.None;
            serial.Parity = Parity.None;
            serial.DataBits = Convert.ToInt32(txtDatabits.Text);

            switch (txtStopbits.Text)
            {
                case "0.5":
                    serial.StopBits = StopBits.OnePointFive;
                    break;
                case "1":
                    serial.StopBits = StopBits.One;
                    break;
                case "2":
                    serial.StopBits = StopBits.Two;
                    break;
                default:
                    serial.StopBits = StopBits.None;
                    break;
            }

            serial.ReadTimeout = 10;
            serial.WriteTimeout = 50;
            serial.DataReceived += serial_DataReceived;
        }

        private void serial_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            Thread.Sleep(250);
            SerialPort spL = (SerialPort)sender;
            string data = serial.ReadExisting();

            CommandControle(data);
        }

        private void CommandControle(string recievedData)
        {
            if (recievedData != "OK")
            {
                char[] splitters = { ':' };
                string[] splittersString = { "\r\n" };
                string[] data = recievedData.Split(splitters);

                switch (data[0])
                {
                    //Nieuwe SMS ontvangen
                    case "\r\n+CMT": data = data[3].Split(splittersString, StringSplitOptions.None);
                        CommandCheck(data);
                        break;
                }
                
            }
            else Console.WriteLine(recievedData);
        }

        private void CommandCheck(string[] data)
        {
            ModWinsCard mwc = new ModWinsCard();

            AdamSocket adso = new AdamSocket();

            bool Succeeded = adso.Connect(AdamType.Adam6000, "172.23.49.102", ProtocolType.Tcp);
            if (Succeeded)      
            {
                Modbus Mb = new Modbus(adso);
                byte[] output = { 0x00 };

                switch (data[1])
                {
                    case "LichtenAan": output[0] = 0x0F;
                        Mb.ForceMultiCoils(0x12, 4, 1, output);
                        break;
                    case "LichtenUit": output[0] = 0x00;
                        Mb.ForceMultiCoils(0x12, 4, 1, output);
                        break;
                    case "AircoAan": Mb.ForceSingleCoil(0x11, true);
                        break;
                    case "AircoUit": Mb.ForceSingleCoil(0x11, true);
                        break;
                    case "Status": bool[] b = new bool[5]; //Bureau: 1, Zitruimte: 2, Keuken: 3, Garage: 4
                        Mb.ReadCoilStatus(0x11, 5, out b);
                        SendData("AT+CMGS=”+32474678203”\r\nAirco: " + b[0] + "\r\nBureau: " + b[1] + "\r\nZitruimte: " + b[2] + "\r\nKeuken: " + b[3] + "\r\nGarage: " + b[4] + (char)26);
                        break;
                }
                adso.Disconnect();
            }
            else
                Console.WriteLine("ERROR: Connection to ADAM failed");
        }

        private void SendData(string s)
        {
            serial.WriteLine(s +"\r\n");
        }

        private void openSerialConnection()
        {
            serial.Open();
        }

        private void closeConnection()
        {
            serial.Close();
        }
        #endregion


        private void btnConnect_Click(object sender, RoutedEventArgs e)
        {
            initSerialConnection();
            openSerialConnection();

            txtBaudRate.IsEnabled = false;
            txtDatabits.IsEnabled = false;
            txtStopbits.IsEnabled = false;
            cboCom.IsEnabled = false;
        }

        private void btnDisconnect_Click(object sender, RoutedEventArgs e)
        {
            closeConnection();

            txtBaudRate.IsEnabled = true;
            txtDatabits.IsEnabled = true;
            txtStopbits.IsEnabled = true;
            cboCom.IsEnabled = true;
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            closeConnection();
        }
    }
}
