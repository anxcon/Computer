using System;
using System.Collections.Generic;
using System.IO;
using System.Management;
using System.Runtime.InteropServices;

using Computer.API;

namespace ModuleDrives
{
    public class ModuleDrives : ModuleBase
    {
        [DllImport("winmm.dll")]
        protected static extern int mciSendString(string command, string buffer, int bufferSize, IntPtr hwndCallback);

        private ManagementEventWatcher watcher;

        protected override void Load()
        {
            WqlEventQuery query = new WqlEventQuery();
            query.EventClassName = "__InstanceModificationEvent";
            query.WithinInterval = new TimeSpan(0, 0, 1);
            query.Condition = @"TargetInstance ISA 'Win32_LogicalDisk' and TargetInstance.DriveType = 5";
            
            ConnectionOptions options = new ConnectionOptions();
            options.EnablePrivileges = true;
            options.Authority = null;
            options.Authentication = AuthenticationLevel.Default;
            //opt.Username = "Administrator";
            //opt.Password = "";
            ManagementScope scope = new ManagementScope("\\root\\CIMV2", options);

            this.watcher = new ManagementEventWatcher(scope, query);
            watcher.EventArrived += new EventArrivedEventHandler(EventArrived);
        }

        protected override void Start()
        {
            this.watcher.Start();
        }

        public List<DriveInfo> GetDriveList(DriveType type)
        {
            List<DriveInfo> list = new List<DriveInfo>();
            List<DriveInfo> drives = new List<DriveInfo>(DriveInfo.GetDrives());
            for (int i = 0; i < drives.Count; i++)
            {
                DriveInfo drive = drives[i];
                if (drive.DriveType != type) continue;
                list.Add(drive);
            }
            return list.Count == 0 ? null : list;
        }

        private void EventArrived(object sender, EventArrivedEventArgs e)
        {
            ManagementBaseObject wmiDevice = (ManagementBaseObject)e.NewEvent["TargetInstance"];
            if (wmiDevice.Properties["VolumeName"].Value == null)
            {
                Log($"CD drive {wmiDevice["DeviceID"]} ejected");
            }
            else
            {
                Log($"CD drive {wmiDevice["DeviceID"]} inserted with volume '{wmiDevice.Properties["VolumeName"].Value}'");
            }
            Console.WriteLine((string)wmiDevice["Name"]); //E:
        }

        public void CloseDrive()
        {
            mciSendString(@"set CDAudio!E:\ door closed", null, 0, IntPtr.Zero);
        }
        public void OpenDrive()
        {
            mciSendString(@"set CDAudio!E:\ door open", null, 0, IntPtr.Zero);
        }
    }
}
