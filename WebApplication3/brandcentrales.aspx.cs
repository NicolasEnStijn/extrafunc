using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;
using System.Text.RegularExpressions;

namespace WebApplication3
{
    public partial class brandcentrales : System.Web.UI.Page
    {

        public enum Mode
        {
            open,
            wissen,
            aanpassen,
            
        }

        private static Mode mode;

        private static remoteprojectsProject[] rpold;

        public void Page_Load(object sender, EventArgs e)
        {
            for (int k = (Table1.Controls.Count -1);  k > 0;  k--)
            {
                Table1.Controls.RemoveAt(k);
            }
            if (!Page.IsPostBack)
            {
                data.init();
            }
            remoteprojectsProject[] rp = data.ordertable(sender);

            if (rp != null)
            {
                rpold = rp;             
            }
            Button send = sender as Button;
            if (send != null)
            {
                if (send.ID.Equals("wissen"))
                {
                    mode = Mode.aanpassen;
                }
            }

            int i;
            for (i = 0; i < rpold.Length; i++)
            {
                TableRow tRow = new TableRow();
                Table1.Rows.Add(tRow);
                Button btn = new Button();
                    if (mode == Mode.wissen)
                    {
                        //btn.OnClientClick = "return confirm('ben je zeker dat je dit item wil wissen')";
                        btn.ID = rpold[i].index.ToString();                       
                        btn.Text = "wissen";
                        btn.Click += new EventHandler(verwijder);
                    }
                    else if(mode == Mode.open)
                    {
                        btn.ID = rpold[i].index.ToString();
                        btn.Text = "open";
                        btn.Click += new EventHandler(SelectButton_Click);
                    }else if(mode == Mode.aanpassen)
                {
                    btn.ID = rpold[i].index.ToString();
                    btn.Text = "aanpassen";
                    btn.Click += new EventHandler(aanpassen);
                }

                btn.UseSubmitBehavior = false;
                TableCell tcb = new TableCell();
                tcb.Controls.Add(btn);
                tcb.CssClass = "open";
                tRow.Cells.Add(tcb);
                TableCell tcn = new TableCell();
                tcn.Text = rpold[i].name;
                tcn.CssClass = "naam";
                tRow.Cells.Add(tcn);
                TableCell tcs = new TableCell();
                tcs.Text = rpold[i].municipality;
                tcs.CssClass = "stad";
                tRow.Cells.Add(tcs);
                TableCell tcp = new TableCell();
                tcp.ID = "tcp" + i.ToString();
                tcp.CssClass = "icon";
                if (rpold[i].berijkbaar == remoteprojectsProject.bereikbaarheid.berijkbaar)
                {
                    tcp.Text = string.Format("<img src='{0}' />", "https://image.flaticon.com/icons/svg/63/63586.svg");
                }
                else if (rpold[i].berijkbaar == remoteprojectsProject.bereikbaarheid.nietberijkbaar)
                {
                    tcp.Text = string.Format("<img src='{0}' />", "https://image.flaticon.com/icons/svg/63/63596.svg");
                }
                else if (rpold[i].berijkbaar == remoteprojectsProject.bereikbaarheid.geeninfo)
                {
                    tcp.Text = string.Format("<img src='{0}' />", "https://image.flaticon.com/icons/svg/63/63923.svg");
                }

                tRow.Cells.Add(tcp);
            }
        }




        public void SelectButton_Click(object sender, EventArgs e)
        {
            MsgBox("het programma wordt opgestart", this.Page, this);
            Button button = sender as Button;
            if (data.getIPAdress(Convert.ToInt32(button.ID)).Equals(""))
            {
                //COMPorthelpers.changehostname(data.getProgramToLaunch(Convert.ToInt32(button.ID)), data.getHostname(Convert.ToInt32(button.ID))); // vul  het juiste hostname  in bij de juiste comport
            }
            else
            {
                //COMPorthelpers.changecomport(data.getProgramToLaunch(Convert.ToInt32(button.ID)), data.getIPAdress(Convert.ToInt32(button.ID))); // vul het juist IP-adres in bij de juiste comport
            }
        }

        protected void check(object sender, EventArgs e)
        {
            Task.Run(() => data.Connectcheck());
        }

        public void MsgBox(String ex, Page pg, Object obj)
        {
            string s = "<SCRIPT language='javascript'>alert('" + ex.Replace("\r\n", "\\n").Replace("'", "") + "'); </SCRIPT>";
            Type cstype = obj.GetType();
            ClientScriptManager cs = pg.ClientScript;
            cs.RegisterClientScriptBlock(cstype, s, s.ToString());
        }

        public void verwijder(object sender, EventArgs e)
        {
            Button button = sender as Button;
            data.wissen(Convert.ToInt32(button.ID));
        }

        public void aanpassen(object sender, EventArgs e)
        {
            Button button = sender as Button;
            Response.Redirect("AanPassenMaken.aspx?index=" + button.ID);
            //Response.Write("<script>window.open('AanPassenMaken.aspx?index=" + button.ID +"');</script>");
        }

        protected void aanmaken_Click(object sender, EventArgs e)
        {
            Response.Redirect("AanPassenMaken.aspx?index=" + data.create().ToString());
        }
    }

    internal static class ParseHelpers
    {

        public static Stream ToStream(this string @this)
        {
            var stream = new MemoryStream();
            var writer = new StreamWriter(stream);
            writer.Write(@this);
            writer.Flush();
            stream.Position = 0;
            return stream;
        }

        public static T ParseXML<T>(this string @this) where T : class
        {
            var reader = XmlReader.Create(@this.Trim().ToStream(), new XmlReaderSettings() { ConformanceLevel = ConformanceLevel.Document });
            return new XmlSerializer(typeof(T)).Deserialize(reader) as T;
        }
    }
}