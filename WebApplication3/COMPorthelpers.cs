using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Web;
using System.ServiceProcess;
using System.Threading;
using System.Diagnostics;
using System.Web.UI;

namespace WebApplication3
{
    public class COMPorthelpers
    {
        const String launchprogram0 = @"\\w2008-webtool\rdp files\pc_md2000.rdp";
        const String launchprogram1 = @"\\w2008-webtool\rdp files\pc_md2400.rdp";
        const String launchprogram2 = @"\\w2008-webtool\rdp files\pc_md2400_v2.exe";

        public static bool changecomport(String launchprogram, String ipadress)
        {
            int instance = setregistery(launchprogram);
            setregisteryIP(ipadress,instance);
            return true;
        }

        public static bool changehostname(String launchprogram, String hostname)
        {
            int instance = setregistery(launchprogram);
            setregisteryHostname(hostname,instance);
            return true;

        }

        public static bool IsServiceRunning()
        {
            ServiceController service = new ServiceController("COMredirectSrv");
            string text = service.Status.ToString();

            if (text == "Running")
            {
                RestartService(service);
                return true;
            }
            else
            {
                startService(service);
                return false;
            }

        }

        public static bool startService(ServiceController service2)
        {
            try
            {
                int timeoutMilliseconds = 500;
                int millisec1 = Environment.TickCount;
                TimeSpan timeout = TimeSpan.FromMilliseconds(timeoutMilliseconds);

                //service2.Stop();
                //service2.WaitForStatus(ServiceControllerStatus.Stopped, timeout);

                // count the rest of the timeout
                int millisec2 = Environment.TickCount;
                timeout = TimeSpan.FromMilliseconds(timeoutMilliseconds - (millisec2 - millisec1));

                service2.Start();
                service2.WaitForStatus(ServiceControllerStatus.Running, timeout);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }

        }

        public static bool RestartService(ServiceController service2)
        {
            try
            {
                int timeoutMilliseconds = 500;
                int millisec1 = Environment.TickCount;
                TimeSpan timeout = TimeSpan.FromMilliseconds(timeoutMilliseconds);

                service2.Stop();
                service2.WaitForStatus(ServiceControllerStatus.Stopped, timeout);

                // count the rest of the timeout
                int millisec2 = Environment.TickCount;
                timeout = TimeSpan.FromMilliseconds(timeoutMilliseconds - (millisec2 - millisec1));

                service2.Start();
                service2.WaitForStatus(ServiceControllerStatus.Running, timeout);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }

        }

        public static void setregisteryHostname(string hostname,int instance )
        {
            RegistryKey mykey = Registry.LocalMachine.OpenSubKey("SYSTEM\\ControlSet001\\Enum\\Root\\MULTIPORTSERIAL\\000" + instance.ToString() + "\\Device Parameters", true);
            if (mykey != null)
            {

                mykey.SetValue("TerminalServerName", hostname, RegistryValueKind.String);
                mykey.SetValue("AddressConfigType", 2, RegistryValueKind.DWord);

            }
            mykey.Close();

            RegistryKey mykey2 = Registry.LocalMachine.OpenSubKey("SYSTEM\\ControlSet001\\Enum\\Root\\MULTIPORTSERIAL\\000" + instance.ToString(), true);
            if (mykey2 != null)
            {
                mykey2.SetValue("FriendlyName", "MD2400 RABBIT 2000 (" + hostname + ")", RegistryValueKind.String);
            }
            mykey2.Close();
        }

        public static void setregisteryIP(string ipadress , int instance)
        {
            RegistryKey mykey = Registry.LocalMachine.OpenSubKey("SYSTEM\\ControlSet001\\Enum\\Root\\MULTIPORTSERIAL\\000" + instance.ToString() + "\\Device Parameters", true);
            if (mykey != null)
            {
                mykey.SetValue("AddressConfigType", 0, RegistryValueKind.DWord);
                mykey.SetValue("IPAddress", ipadress, RegistryValueKind.String); //verander IP-adres in registry

            }
            mykey.Close();

            RegistryKey mykey2 = Registry.LocalMachine.OpenSubKey("SYSTEM\\ControlSet001\\Enum\\Root\\MULTIPORTSERIAL\\000" + instance.ToString(), true);
            if (mykey2 != null)
            {
                mykey2.SetValue("FriendlyName", "MD2400 RABBIT 2000 (" + ipadress + ")", RegistryValueKind.String); //verander Friendly Name in registry
            }
            mykey2.Close();
        }

        public static int setregistery(string launchprogram)
        {
            int instance = 5;
            Guid remotedirectguid = new Guid("{50906cb8-ba12-11d1-bf5d-0000f805f530}");
            if (launchprogram.Equals(launchprogram0))
            {
                instance = 0;
            }
            else if (launchprogram.Equals(launchprogram1))
            {
                instance = 1;
            }
            else if (launchprogram.Equals(launchprogram2))
            {
                instance = 2;
            }

            string instancePath = @"ROOT\MULTIPORTSERIAL\000" + instance.ToString();

            DeviceHelper.SetDeviceEnabled(remotedirectguid, instancePath, false); // disable de coressponderende driver van de multi port serial
            Thread.Sleep(100);
            Thread.Sleep(100);

            if (!IsServiceRunning()) // check of de service 
            {
                throw new Exception("blackbox is gestopt");
            }

            DeviceHelper.SetDeviceEnabled(remotedirectguid, instancePath, true); // driver terug inschakelen
            return instance;
          
        }

    }
}