using MediaDevices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UsbEject;

namespace UsbManager
{
    class Device
    {
        private string name;
        private DeviceType deviceType;
        private List<Volume> volumes;
        
        public Device(string name, DeviceType deviceType, List<Volume> volumes)
        {
            this.name = name;
            this.deviceType = deviceType;
            this.volumes = volumes;
        }

        public string Name
        {
            set { name = value; }
            get { return name; }
        }

        public DeviceType DeviceType
        {
            set { deviceType = value; }
            get { return deviceType; }
        }

        public List<Volume> Volumes
        {
            set { volumes = value; }
            get { return volumes; }
        }

        public void Eject()
        {
            var elem = new VolumeDeviceClass();
            foreach (UsbEject.Volume volume in elem.Volumes)
            {
                if(volumes.FindIndex(vol => vol.Name == volume.LogicalDrive) > -1)
                {
                    volume.Eject(false);
                    break;
                }
            }
        }
    }
}
