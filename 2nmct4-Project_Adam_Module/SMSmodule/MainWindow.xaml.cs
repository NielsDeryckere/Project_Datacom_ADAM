using Advantech.Adam;
using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Net.Sockets;
using System.Text;
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
            SerialPort spL = (SerialPort)sender;
            string data = serial.ReadExisting();

            CommandControle(data);
        }

        private void CommandControle(string recievedData)
        {
            if (recievedData != "OK")
            {
                char[] splitters = { ':' };
                string[] data = recievedData.Split(splitters, 2);

                switch (data[0])
                {
                    //Nieuwe SMS ontvangen
                    case "AT+CMTI": Console.WriteLine(data);
                        data = data[1].Split(',');
                        SendData("AT+CMGR=" +data[1]);
                        break;
                    //SMS Lezen
                    case "+CMGR": Console.WriteLine(data);
                        data = data[1].Split(',');
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

                switch (data[4])
                {
                    case "\r\nLichtenAanOK": output[0] = 0x0F;
                        Mb.ForceMultiCoils(0x11, 4, 1, output);
                        break;
                    case "\r\nLichtenUitOK": output[0] = 0x00;
                        Mb.ForceMultiCoils(0x11, 4, 1, output);
                        break;
                }
                adso.Disconnect();
            }
            else
                Console.WriteLine("ERROR: Connection to ADAM failed");
        }

        private void SendData(string s)
        {
            serial.WriteLine(s);
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
