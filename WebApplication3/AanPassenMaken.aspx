<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AanPassenMaken.aspx.cs" Inherits="WebApplication3.AanPassenMaken" %>
<link href="Aanpassen.css" rel="stylesheet" />
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div class="content">
      <div class="inputpage"> <p>id</p><asp:TextBox ID="id" runat="server"></asp:TextBox> </div>
      <div class="inputpage"> <p>naam</p><asp:TextBox ID="naam" runat="server"></asp:TextBox></div>
      <div class="inputpage"> <p>straat</p><asp:TextBox ID="straat" runat="server"></asp:TextBox></div>
      <div class="inputpage"> <p>gemeente</p><asp:TextBox ID="gemeente" runat="server"></asp:TextBox></div>
      <div class="inputpage"> <p>centralenummer</p><asp:TextBox ID="nummer" runat="server"></asp:TextBox></div>
      <div class="inputpage"> <p>IP-adres</p><asp:TextBox ID="ipadress" runat="server"></asp:TextBox></div>
      <div class="inputpage"> <p>hostname</p><asp:TextBox ID="hostname" runat="server"></asp:TextBox></div>
      <div class="inputpage"> <p>serielepoortnummer</p><asp:TextBox ID="serielepoortnummer" runat="server"></asp:TextBox></div>
      <div class="inputpage"> <p>tcppoort</p><asp:TextBox ID="tcppoort" runat="server"></asp:TextBox></div>
      <div class="inputpage"> <p>connectionmode</p><asp:TextBox ID="connectionmode" runat="server"></asp:TextBox></div>
      <div class="inputpage"> <p>typeofsysteem</p><asp:TextBox ID="typeofsysteem" runat="server"></asp:TextBox></div>
      <div class="inputpage"> <p>programtolaunch</p><asp:TextBox ID="programtolaunch" runat="server"></asp:TextBox></div>
      <div class="inputpage"> <p>notitions</p><asp:TextBox ID="notitions" runat="server"></asp:TextBox></div>
      <div class="inputpage"> <p>registryreference</p><asp:TextBox ID="registryreference" runat="server"></asp:TextBox></div>
      <div class="inputpage"> <asp:Button ID="verander" runat="server" Text="verander" onclick="verander_Click"/>

    </div>
    </form>
</body>
</html>
