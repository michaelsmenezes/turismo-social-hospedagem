<%@ Page Language="VB" AutoEventWireup="false" CodeFile="formRelIntegrantesPasseio.aspx.vb" Inherits="formRelIntegrantesPasseio" %>

<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=11.0.0.0, Culture=neutral, PublicKeyToken=89845dcd8080cc91" Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<!DOCTYPE html>
<link href="stylesheet.css" rel="stylesheet" />
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Relação de Integrantes</title>
</head>
<body>
    <meta http-equiv="X-UA-Compatible" content="IE=8" />
    <form id="form1" runat="server">
        <h2>Relatório de Integrantes</h2>
        <div id="divTipoRelatorio" runat="server">
            <asp:RadioButtonList ID="rdSelecao" runat="server" AutoPostBack="True">
                <asp:ListItem Selected="True" Value="LI">Lista simples - Ordem por nome (Passeio)</asp:ListItem>
                <asp:ListItem Value="LA">Lista simples - Ordem por Apartamento (Passeio)</asp:ListItem>
                <asp:ListItem Value="HL">Lista com valores e apartamentos (Grupo com hospedagem)</asp:ListItem>
            </asp:RadioButtonList>
            <br/>
            <asp:Button ID="btnImprimir" runat="server" Text="Imprimir" CssClass="btnImprimir" />
        </div>
        <br/>

        <div class="TextCenter">
            <asp:ToolkitScriptManager ID="scpReport" runat="server">
            </asp:ToolkitScriptManager>
            <rsweb:ReportViewer ID="rptIntegrantePasseio" runat="server" Font-Names="Verdana" Font-Size="8pt" Height="" Visible="False" WaitMessageFont-Names="Verdana" WaitMessageFont-Size="14pt" Width="100%" SizeToReportContent="True" ZoomMode="PageWidth">
                <LocalReport ReportPath="rptRelacaoIntegrantes.rdlc">
                </LocalReport>
            </rsweb:ReportViewer>

            <br />
            <rsweb:ReportViewer ID="rptHomeList" runat="server" Height="" SizeToReportContent="True" Visible="False" Width="" ZoomMode="PageWidth" Font-Names="Verdana" Font-Size="8pt" WaitMessageFont-Names="Verdana" WaitMessageFont-Size="14pt">
                <LocalReport ReportPath="rptHomeListIntegrantes.rdlc">
                </LocalReport>
            </rsweb:ReportViewer>

        </div>
    </form>
</body>
</html>
