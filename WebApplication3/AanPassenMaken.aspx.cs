using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebApplication3
{
    public partial class AanPassenMaken : System.Web.UI.Page
    {
        private static int index;

        protected void Page_Load(object sender, EventArgs e)
        {
            index = Convert.ToInt32(Request.QueryString["index"]);
            int i = Array.FindIndex(data.rp.projects, indexofelement);
            if (!Page.IsPostBack)
            {
                id.Text = data.rp.projects[i].ID.ToString();
                naam.Text = data.rp.projects[i].name;
                straat.Text = data.rp.projects[i].street;
                gemeente.Text = data.rp.projects[i].municipality;
                nummer.Text = data.rp.projects[i].number;
                ipadress.Text = data.rp.projects[i].ipaddress;
                hostname.Text = data.rp.projects[i].hostname;
                serielepoortnummer.Text = data.rp.projects[i].serialportnumber;
                tcppoort.Text = data.rp.projects[i].tcpport.ToString();
                connectionmode.Text = data.rp.projects[i].connectionmode;
                typeofsysteem.Text = data.rp.projects[i].typeofsystem.ToString();
                programtolaunch.Text = data.rp.projects[i].programtolaunch;
                notitions.Text = data.rp.projects[i].notitions;
                registryreference.Text = data.rp.projects[i].registryreference.ToString();
            }
        }

        protected void verander_Click(object sender, EventArgs e)
        {
            int i = Array.FindIndex(data.rp.projects, indexofelement);
            data.rp.projects[i].ID = Convert.ToByte(id.Text);
            data.rp.projects[i].name = naam.Text;
            data.rp.projects[i].street = straat.Text;
            data.rp.projects[i].municipality = gemeente.Text;
            data.rp.projects[i].number = nummer.Text;
            data.rp.projects[i].ipaddress = ipadress.Text;
            data.rp.projects[i].hostname = hostname.Text;
            data.rp.projects[i].serialportnumber = serielepoortnummer.Text;
            data.rp.projects[i].tcpport = ushort.Parse(tcppoort.Text);
            data.rp.projects[i].connectionmode = connectionmode.Text;
            data.rp.projects[i].typeofsystem = byte.Parse(typeofsysteem.Text);
            data.rp.projects[i].programtolaunch = programtolaunch.Text;
            data.rp.projects[i].notitions = notitions.Text;
            data.rp.projects[i].registryreference = byte.Parse(registryreference.Text);
            data.save();
            Response.Redirect("brandcentrales.aspx");
        }

        private static bool indexofelement(remoteprojectsProject rpp)
        {
            return rpp.index == index;
        }
    }

}