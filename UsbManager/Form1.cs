using MediaDevices;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Windows.Forms;

namespace UsbManager
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

        }

        private Thread thread;

        private List<Device> GetDevices()
        {
            List<Device> devices = new List<Device>();
            foreach (MediaDevice mediaDevice in MediaDevice.GetDevices())
            {
                mediaDevice.Connect();
                IEnumerable<MediaDirectoryInfo> root = mediaDevice.GetRootDirectory().EnumerateDirectories();
                List<Volume> volumes = new List<Volume>();
                foreach (MediaDirectoryInfo info in root)
                {
                    if (mediaDevice.DeviceType == DeviceType.Generic)
                    {
                        DriveInfo driveInfo = new DriveInfo(info.Name);
                        volumes.Add(new Volume(info.Name, driveInfo.TotalSize, driveInfo.TotalFreeSpace));
                    }
                    else
                    {
                        volumes.Add(new Volume(mediaDevice.FriendlyName, 0, 0));
                    }
                }
                devices.Add(new Device(mediaDevice.FriendlyName, mediaDevice.DeviceType, volumes));
                mediaDevice.Disconnect();
            }
            return devices;
        }


        private double ConvertBytes(long bytes)
        {
            return Math.Round((double)bytes / 1024 / 1024 / 1024, 2);
        }
        private void UpdateDeviceTable()
        {
            while (true)
            {

                this.Invoke((MethodInvoker)(delegate
                {
                    List<Device> devices = GetDevices();
                    foreach (Device device in devices)
                    {
                        dataGridView1.ColumnCount = 4;
                        dataGridView1.Columns[0].Name = "Name";
                        dataGridView1.Columns[1].Name = "Total space";
                        dataGridView1.Columns[2].Name = "Free space";
                        dataGridView1.Columns[3].Name = "Used space";
                        string[] str = new string[] { device.Name, Convert.ToString(ConvertBytes(device.Volumes[0].TotalSpace)),
                        Convert.ToString(ConvertBytes(device.Volumes[0].FreeSpace)), Convert.ToString(ConvertBytes(device.Volumes[0].UsedSpace))};
                        bool isContain = false;

                        if (dataGridView1.Rows.Count > 0)
                        {
                            for (int i = 0; i < dataGridView1.Rows.Count; i++)
                            {
                                if (dataGridView1.Rows[i].Cells[0].FormattedValue.ToString() == device.Name)
                                {
                                    isContain = true;
                                    break;
                                }

                            }
                            if (!isContain)
                                dataGridView1.Rows.Add(str);
                        }
                        else
                        {
                            dataGridView1.Rows.Add(str);
                        }


                    }
                    
                    if(dataGridView1.Rows.Count > 0)
                    {
                        int j = 0;
                        while (j < dataGridView1.Rows.Count -1)
                        {
                            string name = dataGridView1.Rows[j].Cells[0].FormattedValue.ToString();
                            Console.WriteLine("Name: " + name);
                            if (devices.FindIndex(device => device.Name == name) > -1)
                            {
                                j++;
                            }
                            else
                            {
                                dataGridView1.Rows.RemoveAt(j);
                            }

                        }
                    }
                }));

                Thread.Sleep(1000);
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            thread = new Thread(UpdateDeviceTable);
            thread.Start();
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            thread.Abort();
        }
    }
}
