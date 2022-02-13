using System;
using System.IO;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AICtrl.MVVM.Models
{
    class HandGestureModel
    {
        private ProfileConfigModel profileConfig;
        private DeviceCtrlModel deviceCtrl;

        int[] wrist = new int[3];
        int[] thumb_tip = new int[3];
        int[] index_finger_mcp = new int[3];
        int[] pinky_mcp = new int[3];

        public HandGestureModel(ProfileConfigModel profileConfig, DeviceCtrlModel deviceCtrl)
        {
            this.profileConfig = profileConfig;
            this.deviceCtrl = deviceCtrl;
        }

        public void RunDetection()
        {
            var psi = new ProcessStartInfo();

            // change to relative
            psi.FileName = @"C:\Users\srira\AppData\Local\Microsoft\WindowsApps\python3.exe";

            var script = @"..\..\ext\detect.py";

            psi.Arguments = $"\"{script}\"";

            psi.UseShellExecute = false;
            psi.CreateNoWindow = true;
            psi.RedirectStandardOutput = true;
            psi.RedirectStandardError = true;

            var errors = "";
            var results = "";

            using (var process = Process.Start(psi))
            {
                errors = process.StandardError.ReadToEnd();
                results = process.StandardOutput.ReadToEnd();

                Console.WriteLine(results);
            }


            Console.WriteLine("ERRORS:");
            Console.WriteLine(errors);
            Console.WriteLine();
            Console.WriteLine("Results:");
            Console.WriteLine(results);
        }

        public bool GetDetectionData()
        {
            string path = @"..\..\ext\data.bin";

            if(!File.Exists(path))
            {
                return false;
            }

            for(int i = 0; ; i++)
            { 
                // Console.WriteLine("hey, {0}", i);
                Task.Delay(100).Wait();

                //byte[] fileBytes = File.ReadAllBytes(path);
                //StringBuilder sb = new StringBuilder();

                try {
                    using (BinaryReader binary_reader = new BinaryReader(File.Open(path, FileMode.Open)))
                    {
                        //user code
                        wrist[0] = binary_reader.ReadInt32();
                        wrist[1] = binary_reader.ReadInt32();
                        wrist[2] = binary_reader.ReadInt32();

                        thumb_tip[0] = binary_reader.ReadInt32();
                        thumb_tip[1] = binary_reader.ReadInt32();
                        thumb_tip[2] = binary_reader.ReadInt32();

                        index_finger_mcp[0] = binary_reader.ReadInt32();
                        index_finger_mcp[1] = binary_reader.ReadInt32();
                        index_finger_mcp[2] = binary_reader.ReadInt32();

                        pinky_mcp[0] = binary_reader.ReadInt32();
                        pinky_mcp[1] = binary_reader.ReadInt32();
                        pinky_mcp[2] = binary_reader.ReadInt32();
                    }
                } 
                catch (System.IO.IOException)
                {
                    Console.WriteLine("io exception!");
                }

                // use profile config functions instead
                if(deviceCtrl.isConnected)
                {
                    //Console.WriteLine("thumbTipX: {0}, wristX: {1}", thumb_tip[0], wrist[0]);

                    deviceCtrl.controller.SetButtonState(11, (thumb_tip[0] > wrist[0]));

                    double resX = (double)(wrist[0] - 50) / 50f;
                    resX *= short.MaxValue;

                    double resY = (double)(wrist[1] - 50) / 50f;
                    resY *= short.MaxValue;

                    deviceCtrl.controller.SetAxisValue(0, (short)-resX);
                    deviceCtrl.controller.SetAxisValue(1, (short)-resY);
                }

                //Console.WriteLine(thumb_tip[2]);
                //deviceCtrl.controller.SetAxisValue;
            }

            return true;
        }
    }
}
