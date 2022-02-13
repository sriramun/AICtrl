using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nefarius.ViGEm.Client;
using Nefarius.ViGEm.Client.Targets;
using Nefarius.ViGEm.Client.Exceptions;
using Nefarius.ViGEm.Client.Targets.Xbox360;

namespace AICtrl.MVVM.Models
{
    public class DeviceCtrlModel
    {

        public ViGEmClient client;
        public IXbox360Controller controller;
        public bool isConnected;

        public DeviceCtrlModel()
        {
            isConnected = false;
            InitClient();
            Console.WriteLine("client init!");
        }

        public bool InitClient()
        {
            try
            {
                client = new ViGEmClient();
                controller = client.CreateXbox360Controller();

                Console.WriteLine("client created!");

                return true;
            }
            catch (VigemBusNotFoundException)
            {
                Console.WriteLine("vigembus not found!");
                return false;
            }
            catch (DllNotFoundException)
            {
                Console.WriteLine("dll not found!");
                return false;
            }
        }

        public void StartDevice()
        {
            if(isConnected)
            {
                return;
            }

            controller.Connect();
            isConnected = true;

            Console.WriteLine("controller started!");
        }

        public void StopDevice()
        {
            if (!isConnected)
            {
                return;
            }

            controller.Disconnect();
            isConnected = false;
            Console.WriteLine("stopping device!");
        }

        ~DeviceCtrlModel()
        {
            client.Dispose();
            Console.WriteLine("device destructor called!");
        }
    }
}
