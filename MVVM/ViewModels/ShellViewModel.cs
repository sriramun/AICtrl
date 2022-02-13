using AICtrl.MVVM.Models;
using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AICtrl.MVVM.ViewModels
{
    public class ShellViewModel : Screen
    {
        private ProfileConfigModel profileConfig;
        private DeviceCtrlModel deviceCtrl;
        private HandGestureModel handGesture;

        public ShellViewModel()
        {

            profileConfig = new ProfileConfigModel();
            deviceCtrl = new DeviceCtrlModel();
            handGesture = new HandGestureModel(profileConfig, deviceCtrl);
        }

        public void StartButton()
        {
            deviceCtrl.StartDevice();
        }

        public void StopButton()
        {
            deviceCtrl.StopDevice();
        }

        public void TestButton()
        {
            Task task1 = new Task(() => handGesture.RunDetection());
            Task task2 = new Task(() => handGesture.GetDetectionData());

            task1.Start();
            task2.Start();
        }
    }
}
