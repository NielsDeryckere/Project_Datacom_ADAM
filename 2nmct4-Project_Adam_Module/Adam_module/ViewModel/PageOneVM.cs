using Advantech.Adam;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Adam_module.ViewModel
{
    class PageOneVM : ObservableObject, IPage
    {
        string sCard;
        int retCode, Protocol, hContext, hCard, Readercount;
        byte[] ReaderListBuff;
        byte[] ReaderGroupBuff;
        bool diFlag;
        ModWinsCard.SCARD_IO_REQUEST ioRequest;
        int sendLen, RecvLen;
        byte[] RecvBuff = new byte[262];
        byte[] SendBuff = new byte[262];

        enum Roles { Administrator, User, Staff, Reset };
        
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
            }

            
        }

        #region Smartcard
        private void SmartCardReadTest()
        {
            MessageBox.Show(ReadCard().ToString());
        }
        
        private void SmartCardWriteTest()
        {
            CreateNewCard(Roles.Administrator);
        }

        private void InitialiseSmartCardReader()
        {
            Array.Clear(SendBuff, 0, 262);
            Array.Clear(RecvBuff, 0, 262);
            
            //Establishes the context
            retCode = ModWinsCard.SCardEstablishContext(ModWinsCard.SCARD_SCOPE_USER, 0, 0, ref hContext);

            if (retCode != ModWinsCard.SCARD_S_SUCCESS)
                throw new Exception(retCode.ToString());

            //Verify how many bytes does the reader name has
            retCode = ModWinsCard.SCardListReaders(hContext, null, null, ref Readercount);

            if (retCode != ModWinsCard.SCARD_S_SUCCESS)
                throw new Exception(retCode.ToString());

            //Creates the byte array to receive the name
            ReaderListBuff = new byte[Readercount];
            //Gets the readers name
            retCode = ModWinsCard.SCardListReaders(hContext, ReaderGroupBuff, ReaderListBuff, ref Readercount);

            if (retCode != ModWinsCard.SCARD_S_SUCCESS)
                throw new Exception(retCode.ToString());

            //Decodify the readers name
            sCard = System.Text.Encoding.ASCII.GetString(ReaderListBuff);

            //Connects to the reader
            retCode = ModWinsCard.SCardConnect(hContext, sCard, ModWinsCard.SCARD_SHARE_SHARED, ModWinsCard.SCARD_PROTOCOL_T0, ref hCard, ref Protocol);

            if (retCode != ModWinsCard.SCARD_S_SUCCESS)
                throw new Exception(retCode.ToString());
        }

        private void StartTransaction()
        {
            Array.Clear(SendBuff, 0, 262);
            Array.Clear(RecvBuff, 0, 262);
            
            //Prepare Buffer
            SendBuff[0] = 0xFF;//start
            SendBuff[2] = 0x20; //memory adres
            SendBuff[3] = 0x0;
            SendBuff[4] = 0x01;
            SendBuff[5] = 0x1;//I2c
            sendLen = 6;
            RecvLen = RecvBuff.Length;

            ioRequest.dwProtocol = Protocol;
            ioRequest.cbPciLength = 8;

            retCode = ModWinsCard.SCardBeginTransaction(hCard);
            if (retCode != ModWinsCard.SCARD_S_SUCCESS)
                throw new Exception(retCode.ToString());

            retCode = ModWinsCard.SCardTransmit(hCard, ref ioRequest, ref SendBuff[0], sendLen, ref ioRequest, ref RecvBuff[0], ref RecvLen);
            if (retCode != ModWinsCard.SCARD_S_SUCCESS)
                throw new Exception(retCode.ToString());
        }

        private void EndTransaction()
        {
            ModWinsCard.SCardEndTransaction(hCard, ModWinsCard.SCARD_LEAVE_CARD);
            ModWinsCard.SCardDisconnect(hCard, ModWinsCard.SCARD_UNPOWER_CARD);
            ModWinsCard.SCardReleaseContext(hContext);
        }

        private Roles ReadCard()
        {
            Array.Clear(SendBuff, 0, 262);
            Array.Clear(RecvBuff, 0, 262);
            
            InitialiseSmartCardReader();
            StartTransaction();

            SendBuff[0] = 0xFF;//start
            SendBuff[1] = 0xB0;//instruction Read
            SendBuff[2] = 0x0; //memory adres
            SendBuff[4] = 0x01;
            sendLen = 6;

            retCode = ModWinsCard.SCardTransmit(hCard, ref ioRequest, ref SendBuff[0], sendLen, ref ioRequest, ref RecvBuff[0], ref RecvLen);
            if (retCode != ModWinsCard.SCARD_S_SUCCESS)
                throw new Exception(retCode.ToString());

            EndTransaction();

            return GetCardRole();
        }

        private Roles GetCardRole()
        {
            switch(RecvBuff[0])
            {
                case 0x66: return Roles.Administrator;
                case 0x69: return Roles.User;
                case 0x99: return Roles.Staff;
                default: return Roles.Reset;
            }
        }

        private void CreateNewCard(Roles r)
        {
            Array.Clear(SendBuff, 0, 262);
            Array.Clear(RecvBuff, 0, 262);

            InitialiseSmartCardReader();
            StartTransaction();

            SendBuff[0] = 0xFF;//start
            SendBuff[1] = 0xD0;//instruction write
            SendBuff[2] = 0x0; //memory adres
            SendBuff[3] = 0x0;
            SendBuff[4] = 0x01;
            
            switch(r)
            {
                case Roles.Administrator: SendBuff[5] = 0x66;
                    break;

                case Roles.User: SendBuff[5] = 0x69;
                    break;

                case Roles.Staff: SendBuff[5] = 0x99;
                    break;

                case Roles.Reset: SendBuff[5] = 0x00;
                    break;
            }

            sendLen = 6;

            retCode = ModWinsCard.SCardTransmit(hCard, ref ioRequest, ref SendBuff[0], sendLen, ref ioRequest, ref RecvBuff[0], ref RecvLen);
            if (retCode != ModWinsCard.SCARD_S_SUCCESS)
                throw new Exception(retCode.ToString());

            EndTransaction();
        }

        public ICommand CommandConnect
        {
            get{return new RelayCommand(Test);} 
        }

        public ICommand SmartCardReadCommand
        {
            get { return new RelayCommand(SmartCardReadTest); }
        }

        public ICommand SmartCardWriteCommand
        {
            get { return new RelayCommand(SmartCardWriteTest); }
        }

        #endregion


    }
}
