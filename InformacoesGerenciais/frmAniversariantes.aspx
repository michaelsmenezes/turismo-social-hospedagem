<%@ Page Title="Hóspedes Aniversariantes" Language="VB" MasterPageFile="~/TurismoSocial.master" AutoEventWireup="false" CodeFile="frmAniversariantes.aspx.vb" Inherits="InformacoesGerenciais_frmAniversariantes" %>

<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="asp" %>

<%@ Register assembly="Microsoft.ReportViewer.WebForms, Version=11.0.0.0, Culture=neutral, PublicKeyToken=89845dcd8080cc91" namespace="Microsoft.Reporting.WebForms" tagprefix="rsweb" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="conPlaHolTurismoSocial" Runat="Server">
    <link href="../stylesheet.css" rel="stylesheet" />
    <br />
    <br />
    <br />
    <br />
    <br />
    <br />
    <br />
    <br />
    <asp:Panel ID="pnlConsulta" runat="server">
        <div class="formRuler" style="font-size: large; font-weight: bold">
            &nbsp;&nbsp;Lista de Hóspedes Aniversariantes</div>
        <br />
        &nbsp;Data Inicial&nbsp;&nbsp;<asp:TextBox ID="txtDataInicial" runat="server" MaxLength="10" Width="80px" ToolTip="Será exibido, apenas hóspedes em estada."></asp:TextBox>
        Data Final
        <asp:TextBox ID="txtDataFinal" runat="server" Width="80px"></asp:TextBox>
        <asp:CalendarExtender ID="txtDataFinal_CalendarExtender" runat="server" DefaultView="Days" Enabled="True" FirstDayOfWeek="Sunday" Format="dd/MM/yyyy" PopupPosition="BottomLeft" TargetControlID="txtDataFinal">
        </asp:CalendarExtender>
        <asp:CalendarExtender ID="txtDataInicial_CalendarExtender" runat="server" Enabled="True" FirstDayOfWeek="Monday" Format="dd/MM/yyyy" TargetControlID="txtDataInicial">
        </asp:CalendarExtender>
        &nbsp;<asp:Button ID="btnConsultar" runat="server" CssClass="imgLupa" Text="Consultar" />
        <br />
        <br />
    </asp:Panel>
    <asp:RoundedCornersExtender ID="pnlConsulta_RoundedCornersExtender" runat="server" BorderColor="ActiveBorder" Color="ActiveBorder" Enabled="True" Radius="2" TargetControlID="pnlConsulta">
    </asp:RoundedCornersExtender>
    <br />
&nbsp;&nbsp;&nbsp;
    <rsweb:ReportViewer ID="rptAniversariantes" runat="server" Height="" SizeToReportContent="True" Visible="False" Width="100%" ZoomMode="PageWidth" Font-Names="Verdana" Font-Size="8pt" WaitMessageFont-Names="Verdana" WaitMessageFont-Size="14pt">
        <LocalReport ReportPath="InformacoesGerenciais\RelAniversariantes.rdlc">
        </LocalReport>
    </rsweb:ReportViewer>
    <p>
        <br />
    </p>
  
</asp:Content>

