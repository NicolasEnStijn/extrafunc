using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI.WebControls;
using System.Net.NetworkInformation;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Linq;
using System.IO;
using System.Xml.Serialization;
using System.Xml;
using System.Text.RegularExpressions;

namespace WebApplication3
{
    public static class data
    {
        public static remoteprojects rp = null;
        private static int order = 0;
        //private static string Xmlplaats = @"C:\Users\Nicolas\Documents\VIVES\stage\remoteprojects.xml";
        //private static string Xmlplaats = @"C:\Users\LAPTOP-Stijn\Desktop\stage\remoteprojects.xml";
        private static string Xmlplaats =Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + @"\remoteprojects.xml";

        public static remoteprojectsProject[] ordertable(object sender)
        {
            Button button = sender as Button;
            if (button == null)
            {
                TextBox tb = sender as TextBox;
                if (tb == null)
                {
                    if (rp == null)
                    {
                        XmlSerializer serializer = new XmlSerializer(typeof(remoteprojects));
                        FileStream fs = new FileStream(Xmlplaats, FileMode.Open);
                        XmlReader reader = XmlReader.Create(fs);
                        rp = (remoteprojects)serializer.Deserialize(reader);
                        fs.Close();
                        for (int i = 0; i < rp.projects.Length; i++)
                        {
                            rp.projects[i].index = i;
                        }
                        Task.Run(() => Connectcheck());
                        return rp.projects;
                    }
                }
                else if (tb.ID.Equals("TextBoxNaam"))
                {
                    var q = from x in rp.projects
                            where x.name.ToLower().Contains(tb.Text.ToLower())
                            select x;
                    return (remoteprojectsProject[])q.ToArray();

                }
                else if (tb.ID.Equals("TextBoxMunicipality"))
                {
                    var q = from x in rp.projects
                            where x.municipality.ToLower().StartsWith(tb.Text.ToLower())
                            select x;
                    return (remoteprojectsProject[])q.ToArray();
                }
            }
            else if (button.ID.Equals("name"))
            {
                if (order == 1)
                {
                    order = 0;
                    return rp.projects.OrderByDescending(u => u.name).ToArray();

                }
                else
                {
                    order = 1;
                    return rp.projects.OrderBy(u => u.name).ToArray();
                }
            }
            else if (button.ID.Equals("municipality"))
            {
                if (order == 2)
                {
                    order = 3;
                    return rp.projects.OrderByDescending(u => u.municipality).ToArray();
                }
                else
                {
                    order = 2;
                    return rp.projects.OrderBy(u => u.municipality).ToArray();
                }
            }
            else if(Regex.IsMatch(button.ID, @"^\d+$"))
            {
                switch(order)
                {
                    case 0:
                        return rp.projects.OrderByDescending(u => u.name).ToArray();
                        break;
                    case 1:
                        return rp.projects.OrderBy(u => u.name).ToArray();
                        break;
                    case 2:
                        return rp.projects.OrderBy(u => u.municipality).ToArray();
                        break;
                    case 3:
                        return rp.projects.OrderByDescending(u => u.municipality).ToArray();
                        break;
                    default:
                        return rp.projects;

                }
            }
            return null;
        }

        
        public static void Connectcheck()
        {
            try {
                int i = 1;
                for (i = 0; i < rp.projects.Count(); i++)
                {
                    if (rp.projects[i].notitions.Equals(""))
                    {
                        Socket sock = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                        sock.SetSocketOption(SocketOptionLevel.IP, SocketOptionName.TypeOfService, 0x03);
                        String ip = "";
                        if (rp.projects[i].ipaddress.Equals(""))
                        {
                            ip = rp.projects[i].hostname;
                        }
                        else
                        {
                            ip = rp.projects[i].ipaddress;
                        }

                        try
                        {
                            var result = sock.BeginConnect(ip, Convert.ToInt16(rp.projects[i].tcpport), null, null);
                            bool success = result.AsyncWaitHandle.WaitOne(1000, true);
                            if (success)
                            {
                                sock.EndConnect(result);
                                byte[] bytes = new byte[8];
                                sock.Receive(bytes);
                                String txt = bytes[1].ToString();
                                if (txt.Equals(""))
                                {
                                    rp.projects[i].berijkbaar = remoteprojectsProject.bereikbaarheid.nietberijkbaar;
                                }
                                {
                                    rp.projects[i].berijkbaar = remoteprojectsProject.bereikbaarheid.berijkbaar;
                                }
                            }
                            else
                            {
                                throw new SocketException(10060); // Connection timed out.
                            }
                        }
                        catch
                        {
                            rp.projects[i].berijkbaar = remoteprojectsProject.bereikbaarheid.nietberijkbaar;
                        }
                        finally
                        {
                            sock.Close();
                        }
                    }
                    else
                    {
                        rp.projects[i].berijkbaar = remoteprojectsProject.bereikbaarheid.geeninfo;
                    }
                }
            }catch(IndexOutOfRangeException)
            {

            }
        }

        public static String getName(int index)
        {
            var q = from x in rp.projects
                    where x.index == index
                    select x;
            remoteprojectsProject[] rpp = (remoteprojectsProject[])q.ToArray();
            return rpp[0].name;
        }

        public static String getMunicipality(int index)
        {
            var q = from x in rp.projects
                    where x.index == index
                    select x;
            remoteprojectsProject[] rpp = (remoteprojectsProject[])q.ToArray();
            return rpp[0].municipality;
        }

        public static string getHostname(int index)
        {
            var q = from x in rp.projects
                    where x.index == index
                    select x;
            remoteprojectsProject[] rpp = (remoteprojectsProject[])q.ToArray();
            return rpp[0].hostname;
        }

        public static String getIPAdress(int index)
        {
            var q = from x in rp.projects
                    where x.index == index
                    select x;
            remoteprojectsProject[] rpp = (remoteprojectsProject[])q.ToArray();
            return rpp[0].ipaddress;
        }

        public static String getProgramToLaunch(int index)
        {
            var q = from x in rp.projects
                    where x.index == index
                    select x;
            remoteprojectsProject[] rpp = (remoteprojectsProject[])q.ToArray();
            return rpp[0].programtolaunch;
        }

        public static String getNotification(int index)
        {
            var q = from x in rp.projects
                    where x.index == index
                    select x;
            remoteprojectsProject[] rpp = (remoteprojectsProject[])q.ToArray();
            return rpp[0].notitions;
        }

        public static remoteprojectsProject.bereikbaarheid getBereikbaarheid(int index)
        {
            var q = from x in rp.projects
                    where x.index == index
                    select x;
            remoteprojectsProject[] rpp = (remoteprojectsProject[])q.ToArray();
            return rpp[0].berijkbaar;
        } 

        public static void init()
        {
            rp = null;
        }

        public static void wissen(int index)
        {
            rp.projects = rp.projects.Where(p => p.index != index).ToArray();
            save();
        }

        public static void save()
        {
            XmlSerializer x = new XmlSerializer(rp.GetType());
            FileStream fs = new FileStream(Xmlplaats, FileMode.Create);
            x.Serialize(fs, rp);
            fs.Close();
        }

        public static int create()
        {
            remoteprojectsProject[] na = new remoteprojectsProject[(rp.projects.Count() + 1)];
            Array.Copy(rp.projects, na, rp.projects.Count());
            int index;
            bool b = true;
            for (index = rp.projects.Count(); b; index++)
            {
                var q = from x in rp.projects where x.index.Equals(index) select x;
                remoteprojectsProject[] trpp = (remoteprojectsProject[])q.ToArray();
                if (trpp.Count()==0)
                {
                    b = false;
                }

            }
            remoteprojectsProject nrpp= new remoteprojectsProject();
            nrpp.index = index;
            na[na.Count() - 1] = nrpp;
            rp.projects = na;
            return index;
        }
    }
}