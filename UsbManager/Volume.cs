using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UsbManager
{
    class Volume
    {
        private string name;
        private long freeSpace;
        private long usedSpace;
        private long totalSpace;

        public Volume(string name, long totalSpace, long freeSpace)
        {
            this.totalSpace = totalSpace;
            this.freeSpace = freeSpace;
            this.usedSpace = totalSpace - freeSpace;
        }

        public string Name
        {
            set { name = value;}
            get { return name; }
        }
        public long FreeSpace
        {
            set { freeSpace = value; }
            get { return freeSpace; }
        }

        public long UsedSpace
        {
            set { usedSpace = value; }
            get { return usedSpace; }
        }

        public long TotalSpace
        {
            set { totalSpace = value; }
            get { return totalSpace; }
        }
    }
}
