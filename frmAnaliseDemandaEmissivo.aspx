<%@ Page Title="" Language="VB" MasterPageFile="~/TurismoSocial.master" AutoEventWireup="false" CodeFile="frmAnaliseDemandaEmissivo.aspx.vb" Inherits="frmAnaliseDemandaEmissivo" %>

<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=11.0.0.0, Culture=neutral, PublicKeyToken=89845dcd8080cc91" Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>

<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="asp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="conPlaHolTurismoSocial" Runat="Server">
    <br/>
    <br/>
    <br/>
    <br/>
    <br/>
    <br/>
    <div align="left"  class="TituloLegal ArrendodarBorda">
        Análise de Demanda</div>
    <div style="border:1px solid #C0C0C0;" class="ArrendodarBorda">
        <br />
        Tipo
        <asp:DropDownList ID="drpTipoDemanda" AutoPostBack="true" runat="server" ToolTip="Setor Social: Relatório para saber quantidade de pessoas na unidade, no dia.">
            <asp:ListItem Value="E">Turismo Emissivo</asp:ListItem>
            <asp:ListItem Value="H">Hospedagem</asp:ListItem>
           <%-- <asp:ListItem Value="N">Setor Social</asp:ListItem>--%>
        </asp:DropDownList>
&nbsp;
       Data Inicial <asp:TextBox ID="txtDataInicial" runat="server" MaxLength="11" Width="100px" AutoPostBack="True" ></asp:TextBox>
        <asp:CalendarExtender ID="txtDataInicial_CalendarExtender" runat="server" Enabled="True" FirstDayOfWeek="Sunday" Format="dd/MM/yyyy" TargetControlID="txtDataInicial">
        </asp:CalendarExtender>
&nbsp;<asp:Label ID="lblDataFinal" runat="server" Text="Data Final"></asp:Label>
        &nbsp;<asp:TextBox ID="txtDataFinal" runat="server" MaxLength="11" Width="100px"></asp:TextBox>
        <asp:CalendarExtender ID="txtDataFinal_CalendarExtender" runat="server" Enabled="True" FirstDayOfWeek="Sunday" Format="dd/MM/yyyy" TargetControlID="txtDataFinal">
        </asp:CalendarExtender>
        &nbsp;
&nbsp;<asp:Button ID="btnConsultar" CssClass="btnConsultar" runat="server" Text="Consultar" />
        <br />
        <br />
    </div>
    <br/>
    <rsweb:ReportViewer ID="rptAnaliseDemanda" runat="server" Height="" SizeToReportContent="True" CssClass="ArrendodarBorda" Width="" ZoomMode="PageWidth" Font-Names="Verdana" Font-Size="8pt" Visible="False" WaitMessageFont-Names="Verdana" WaitMessageFont-Size="14pt">
        <LocalReport ReportPath="rptAnaliseDemandaEmissivo.rdlc">
        </LocalReport>
    </rsweb:ReportViewer>
</asp:Content>

