<%@ Page Title="" Language="VB" MasterPageFile="~/TurismoSocial.master" AutoEventWireup="false" CodeFile="frmCortesiasHospedagem.aspx.vb" Inherits="InformacoesGerenciais_frmCortesiasHospedagem" %>

<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=11.0.0.0, Culture=neutral, PublicKeyToken=89845dcd8080cc91" Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>

<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="asp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="conPlaHolTurismoSocial" Runat="Server">
    <p>
        <br />
    </p>
    <p>
        <br/>
        <br/>
        <br/>
        <br/>
        <br/>
    <div runat="server" id="divOpcaoConsulta">
        <table>
            <tr>
                <td style="font-size: x-large; font-weight: bold" colspan="5">Relatório de cortesias em hospedagem</td>
                <td>&nbsp;</td>
            </tr>
            <tr>
                <td>Data Inicial</td>
                <td>Data Final</td>
                <td>Forma de Pagamento</td>
                <td>Status</td>
                <td>Característica</td>
                <td>&nbsp;</td>
            </tr>
            <tr>
                <td>
                    <asp:TextBox ID="txtDataInicial"  runat="server" Width="100px"></asp:TextBox> 
                    <asp:CalendarExtender ID="txtDataInicial_CalendarExtender" runat="server" Enabled="True" Format="dd/MM/yyyy" TargetControlID="txtDataInicial">
                    </asp:CalendarExtender>
                </td>
                <td>
                    <asp:TextBox ID="txtDataFinal" runat="server" Width="100px"></asp:TextBox>
                    <asp:CalendarExtender ID="txtDataFinal_CalendarExtender" runat="server" Enabled="True" Format="dd/MM/yyyy" TargetControlID="txtDataFinal">
                    </asp:CalendarExtender>
                </td>
                <td>
                    <asp:DropDownList ID="drpFormaPagto" runat="server">
                        <asp:ListItem Selected="True" Value="FC">Free/Cortesia</asp:ListItem>
                        <asp:ListItem Value="F">Free</asp:ListItem>
                        <asp:ListItem Value="C">Cortesia</asp:ListItem>
                    </asp:DropDownList>
                </td>
                <td>
                    <asp:DropDownList ID="drpStatus" runat="server">
                        <asp:ListItem Value="T">Todas</asp:ListItem>
                        <asp:ListItem Value="W">Todas(Menos as canceladas)</asp:ListItem>
                        <asp:ListItem Value="C">Canceladas</asp:ListItem>
                        <asp:ListItem Value="R">Confirmadas</asp:ListItem>
                        <asp:ListItem Value="E">Em Estada</asp:ListItem>
                        <asp:ListItem Value="F">Finalizadas</asp:ListItem>
                        <asp:ListItem Value="P">Pendente de Pagamento</asp:ListItem>
                    </asp:DropDownList>
                </td>
                <td>
                    <asp:DropDownList ID="drpCaracteristica" runat="server">
                        <asp:ListItem Selected="True" Value="I">Individuais</asp:ListItem>
                        <asp:ListItem Value="E">Excursões</asp:ListItem>
                    </asp:DropDownList>
                </td>
                <td>
                    <asp:Button ID="btnImprimir" CssClass="btnImprimir" runat="server" Text="Imprimir" />
                </td>
            </tr>
        </table>
        </div>
    </p>
    <p>
        <div runat="server" id="divReport" visible="false" style="text-align: center" >
            <rsweb:ReportViewer ID="rptCortesias" runat="server" Font-Names="Verdana" Font-Size="8pt" Height="" SizeToReportContent="True" WaitMessageFont-Names="Verdana" WaitMessageFont-Size="14pt" Width="" ZoomMode="PageWidth">
                <LocalReport ReportPath="InformacoesGerenciais\rptCortesiasHospedagem.rdlc">
                </LocalReport>
            </rsweb:ReportViewer>
        </div>
    </p>
</asp:Content>

