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
using System.Diagnostics;
using System.Data.Odbc;

namespace WebApplication3
{
    public partial class brandcentrales : System.Web.UI.Page
    {

        FileUpload ofd;

        public enum Mode
        {
            open,
            wissen,
            aanpassen,

        }

        public void Page_Load(object sender, EventArgs e)
        {

            string filepath = Server.MapPath("~/") + "remoteprojects.xml";
            data.XMLPlaats = filepath;
            remoteprojectsProject[] rpold = (remoteprojectsProject[])Session["rpold"]; //we maken dit aan om 
            for (int k = (Table1.Controls.Count - 1); k > 0; k--)
            {
                Table1.Controls.RemoveAt(k);
            }
            if (!Page.IsPostBack)
            {
                data.init();
                Session["wissenstatuus"] = -1;  
                Session["mode"] = Mode.open;
            }


            Button send = sender as Button;
            if (send != null)
            {
                if (send.ID.Equals("wissen"))
                {
                    Session["mode"] = Mode.wissen;
                }
                else if (send.ID.Equals("open"))
                {
                    Session["mode"] = Mode.open;
                }
                else if (send.ID.Equals("aanpassen"))
                {
                    Session["mode"] = Mode.aanpassen;
                }
            }

            int i;
            
            if (data.XMLPlaats.Equals("")) // als de xml niet gevonden is dan toont de pagina 
            {
                TableRow tRow = new TableRow();
                Table1.Rows.Add(tRow);
                TableCell tcb = new TableCell();
                tRow.Cells.Add(tcb);
                TableCell tcn = new TableCell();
                tcn.Text = "geen Xml gevonden";
                tcn.CssClass = "naam";
                tcn.ID = "xmlinfo";
                tRow.Cells.Add(tcn);

                TableRow tRow2 = new TableRow();
                Table1.Rows.Add(tRow2);
                Button btnconfirm = new Button();
                TableCell tbc = new TableCell();
                btnconfirm.Text = "confirm";
                btnconfirm.Click += new EventHandler(instellenxml_click);
                btnconfirm.ID = "confirmbtn";
                tbc.Controls.Add(btnconfirm);
                tRow2.Cells.Add(tbc);

                TableCell tcc = new TableCell();
                ofd = new FileUpload();
                tcc.Controls.Add(ofd);
                tRow2.Cells.Add(tcc);
                
                TableCell tbf = new TableCell();
                tbf.Text = "vul hier de xml waarde in";
                tbf.ID = "request";
                tRow2.Cells.Add(tbf);
            }
            else
            {
                remoteprojectsProject[] rp = data.ordertable(sender);

                if (rp != null)
                {
                    rpold = rp;
                }
                for (i = 0; i < rpold.Length; i++)
                {
                    TableRow tRow = new TableRow();
                    Table1.Rows.Add(tRow);
                    Button btn = new Button();
                    TableCell tcb = new TableCell();
                    if ((Mode)Session["mode"] == Mode.wissen)
                    {
                        if (rpold[i].index == Convert.ToInt16(Session["wissenstatuus"]))
                        {
                            TextBox tb = new TextBox();
                            tb.TextMode = TextBoxMode.Password;
                            tb.Attributes.Add("placeholder", "hint");
                            tb.TextChanged += new EventHandler(verwijder);
                            tb.ID = rpold[i].index.ToString();
                            tcb.Controls.Add(tb);
                        }
                        else
                        {
                            btn.ID = rpold[i].index.ToString();
                            btn.Text = "wissen";
                            btn.Click += new EventHandler(verwijder_Click);
                            btn.UseSubmitBehavior = false;
                            tcb.Controls.Add(btn);
                        }
                    }
                    else if ((Mode)Session["mode"] == Mode.open)
                    {
                        btn.ID = rpold[i].index.ToString();
                        btn.Text = "open";
                        btn.Click += new EventHandler(SelectButton_Click);
                        btn.UseSubmitBehavior = false;
                        tcb.Controls.Add(btn);
                    }
                    else if ((Mode)Session["mode"] == Mode.aanpassen)
                    {
                        btn.ID = rpold[i].index.ToString();
                        btn.Text = "aanpassen";
                        btn.Click += new EventHandler(projectaanpassen);
                        btn.UseSubmitBehavior = false;
                        tcb.Controls.Add(btn);
                    }
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
            Session["rpold"] = rpold;
        }

        public void SelectButton_Click(object sender, EventArgs e)
        {
            Button button = sender as Button;
            //String query = @"INSERT INTO[Webportaal].[dbo].[requests](naam,IpAdres,Hostname,notitions,RequestTime)VALUES('"+ data.getName(Convert.ToInt32(button.ID))+"','"+ data.getIPAdress(Convert.ToInt32(button.ID))+"','"+ data.getHostname(Convert.ToInt32(button.ID))+"','"+ data.getNotification(Convert.ToInt32(button.ID)) + "','"+DateTime.Now+"')";
            String query = @"INSERT INTO[Webportaal].[dbo].[requests](naam,IpAdres,Hostname,notitions,RequestTime)VALUES('text','dfdfds','dsffd','dssddfsfd',)";

            String Database = "LocalServer";
            writeSQLDB(Database, query);
            MsgBox("het programma wordt opgestart " + data.getProgramToLaunch(Convert.ToInt32(button.ID)), this.Page, this);
            if (data.getNotification(Convert.ToInt32(button.ID)).Equals(""))
            {
                if (data.getIPAdress(Convert.ToInt32(button.ID)).Equals(""))
                {
                    try
                    {
                        Task.Factory.StartNew(() => COMPorthelpers.changehostname(data.getProgramToLaunch(Convert.ToInt32(button.ID)), data.getHostname(Convert.ToInt32(button.ID)))); // vul  het juiste hostname  in bij de juiste comport
                        startprogram(data.getProgramToLaunch(Convert.ToInt32(button.ID)));

                    }
                    catch (Exception ex)
                    {
                        MsgBox(ex.Message, this.Page, this);
                    }
                }
                else
                {

                    try
                    {
                        Task.Factory.StartNew(() => COMPorthelpers.changecomport(data.getProgramToLaunch(Convert.ToInt32(button.ID)), data.getIPAdress(Convert.ToInt32(button.ID)))); // vul het juist IP-adres in bij de juiste comport
                        startprogram(data.getProgramToLaunch(Convert.ToInt32(button.ID)));
                    }
                    catch (Exception ex)
                    {
                        MsgBox(ex.Message, this.Page, this);
                    }

                }
            }else if(data.getNotification(Convert.ToInt32(button.ID)).Equals("Deze installatie is enkel bereikbaar via remote desktop naar 194.78.14.42 PAS OP: VERSIE 2 MD2400"))
            {
                startprogram("vanmarcke");
            }
            else if (data.getNotification(Convert.ToInt32(button.ID)).Equals("remote desktop naar ts.sjbtd.be, user kztech\ardovlam en pasw Adv8850"))
            {
                startprogram("baptist");
            }
            else if (data.getNotification(Convert.ToInt32(button.ID)).Equals("gebruik internet explorer door te surfen naar de hostname, login op bureaublad"))
            {
                Response.Redirect("https://citrix.libertpaints.com/vpn/index.html");
            }
            else if (data.getNotification(Convert.ToInt32(button.ID)).Equals("Deze installatie is enkele bereikbaar dmv een citrix server connectie"))
            {
                Response.Redirect("https://citrix.sterckx.com/Citrix/XenApp/auth/login.aspx");
            }
        }

        protected void startprogram(string launchprogram)
        {
            Response.Clear();
            string filePath;
            switch (launchprogram)
            {
                case @"\\w2008-webtool\rdp files\pc_md2000.rdp":
                    filePath = Server.MapPath("/exports/pc_md2000.rdp");
                    break;
                case @"\\w2008-webtool\rdp files\pc_md2400.rdp":
                    filePath = Server.MapPath("/exports/pc_md2400.rdp");
                    break;
                case @"\\w2008-webtool\rdp files\pc_md2400_v2.exe":
                    filePath = Server.MapPath("/exports/pc_md2400_v2.rdp");
                    break;
                case "vanmarcke":
                    filePath = Server.MapPath("/exports/VanMarcke.rdp");
                    break;
                case "baptist":
                    filePath = Server.MapPath("/exports/SJBTD connectie.rdp");
                    break;
                default:
                    filePath = Server.MapPath("/exports/pc_md2000.rdp");
                    break;


            }
            if (Request.Browser != null && Request.Browser.Browser == "IE")
                filePath = HttpUtility.UrlPathEncode(filePath);

            // Response.Cache.SetCacheability(HttpCacheability.Public); // that's upon you

            Response.AddHeader("Content-Disposition", "attachment;filename=\"" + "rdpverbinding.rdp" + "\"");
            Response.AddHeader("Content-type", "application/rdp");
            Response.AddHeader("Content-Length", new FileInfo(filePath).Length.ToString()); // upon you 
            Response.WriteFile(filePath);

            Response.Flush();

            string path = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\nicoEnstijn";
            Directory.CreateDirectory(path);
            Response.End();
        }  //in deze functie zal de juiste rdp downloaden 

        protected void check(object sender, EventArgs e)
        {
            Task.Factory.StartNew(() => data.Connectcheck());
        }   //check of er connectie kan gemaakt worden

        public void MsgBox(String ex, Page pg, Object obj)    // maakt het mogelijk om berichten weer te geven
        {
            string s = "<SCRIPT language='javascript'>alert('" + ex.Replace("\r\n", "\\n").Replace("'", "") + "'); </SCRIPT>";
            Type cstype = obj.GetType();
            ClientScriptManager cs = pg.ClientScript;
            cs.RegisterClientScriptBlock(cstype, s, s.ToString());
        }

        public void verwijder_Click(object sender, EventArgs e)
        {
            
            Button button = sender as Button;
            Session["wissenstatuus"] = Convert.ToInt16(button.ID);
            Page_Load(sender, e);
        }

        public void verwijder(object sender, EventArgs e)
        {
            TextBox tb = sender as TextBox;
            //implement serious security
            if (tb.Text.Equals("admin"))
            {
                data.wissen(Convert.ToInt32(tb.ID));
            }
            else
            {
                MsgBox("paswoord is fout", this.Page, this);
                //extra bevijligen tegen brute force attacks
            }
            Session["wissenstatuus"] = -1;
            Button btn = new Button();
            btn.ID = tb.ID;
            Page_Load(btn, e);
        }

        public void projectaanpassen(object sender, EventArgs e)
        {
            Button button = sender as Button;
            Response.Redirect("AanPassenMaken.aspx?index=" + button.ID);
            //Response.Write("<script>window.open('AanPassenMaken.aspx?index=" + button.ID +"');</script>");
        }

        public void instellenxml_click(object sender, EventArgs e)
        {
            UploadButton_Click(null, null);
            Control ctr = new Control();
            Page_Load(ctr, e);
            
        }

        protected void aanmaken_Click(object sender, EventArgs e)
        {
            Response.Redirect("AanPassenMaken.aspx?index=" + data.create().ToString());
        }

        protected void UploadButton_Click(object sender, EventArgs e)
        {
            if (ofd.HasFile)
            {
                try
                {
                    string filename = Path.GetFileName(ofd.FileName);
                    string filepath = Server.MapPath("~/") + filename;
                     ofd.SaveAs(filepath);
                    data.XMLPlaats = filepath;
                }
                catch (Exception ex)
                {
                    MsgBox(ex.Message, this.Page, this);
                }
            }
        }

        public static void writeSQLDB(string Database, string Query)
        {
            OdbcConnection MyConn = new OdbcConnection();
            MyConn.ConnectionString = "DSN=" + Database + ";uid=administrator;pwd=Ardovlam1.0;";
            MyConn.Open();
            OdbcCommand cmd = new OdbcCommand(Query, MyConn);
            cmd.ExecuteNonQuery();

        }

    }
}