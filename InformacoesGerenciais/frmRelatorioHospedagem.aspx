<%@ Page Title="" Language="VB" MasterPageFile="~/TurismoSocial.master" AutoEventWireup="false" CodeFile="frmRelatorioHospedagem.aspx.vb" Inherits="InformacoesGerenciais_frmRelatorioHospedagem"  Culture="pt-BR" uiCulture="pt-BR" %>

<%@ Register assembly="Microsoft.ReportViewer.WebForms, Version=11.0.0.0, Culture=neutral, PublicKeyToken=89845dcd8080cc91" namespace="Microsoft.Reporting.WebForms" tagprefix="rsweb" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="conPlaHolTurismoSocial" Runat="Server">
    <p>
        <br/>
        <br/>
        <br/>
        <br/>
        <br/>
        <br/>

            <asp:Panel ID="pnlPesquisa" runat="server" Width="95%" CssClass="ArrendodarBorda" >
                <div align="center" class="ArrendodarBorda formRuler" style="font-weight:bold; font-size: x-large; color: #FFFFFF;"  >Relatório de Hospedagem</div>
                <table>
                    <tr valign="bottom"  >
                        <td >
                            Mês</td>
                        <td>
                            &nbsp;</td>
                        <td >
                            Ano</td>
                        <td runat="server" id="tdBlocos">
                            Blocos</td>
                        <td>&nbsp;</td>
                    </tr>
                    <tr>
                        <td>
                            <asp:DropDownList ID="drpMes" runat="server" CssClass="ArrendodarBorda">
                               <%-- <asp:ListItem Value="1">Janeiro</asp:ListItem>
                                <asp:ListItem Value="2">Fevereiro</asp:ListItem>
                                <asp:ListItem Value="3">Março</asp:ListItem>
                                <asp:ListItem Value="4">Abril</asp:ListItem>
                                <asp:ListItem Value="5">Maio</asp:ListItem>
                                <asp:ListItem Value="6">Junho</asp:ListItem>
                                <asp:ListItem Value="7">Julho</asp:ListItem>
                                <asp:ListItem Value="8">Agosto</asp:ListItem>
                                <asp:ListItem Value="9">Setembro</asp:ListItem>
                                <asp:ListItem Value="10">Outubro</asp:ListItem>
                                <asp:ListItem Value="11">Novembro</asp:ListItem>
                                <asp:ListItem Value="12">Dezembro</asp:ListItem>--%>
                            </asp:DropDownList>
                        </td>
                        <td>
                            &nbsp;</td>
                        <td>
                            <asp:DropDownList ID="drpAno" runat="server" CssClass="ArrendodarBorda" AutoPostBack="True">
                            </asp:DropDownList>
                        </td>
                        <td>
                            <asp:DropDownList ID="drpBlocos" runat="server" CssClass="ArrendodarBorda">
                               <%-- <asp:ListItem Value="0">Todos</asp:ListItem>
                                <asp:ListItem Value="1">Anhanguera</asp:ListItem>
                                <asp:ListItem Value="2">Bambuí</asp:ListItem>
                                <asp:ListItem Value="3">Oswaldo Kilzer</asp:ListItem>
                                <asp:ListItem Value="4">Wilton Honorato</asp:ListItem>--%>
                            </asp:DropDownList>
                        </td>
                        <td>
                            <asp:Button ID="btnConsultar" runat="server" Height="22px" CssClass="imgLupa ArrendodarBorda" 
                                Text="  Consultar" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            &nbsp;</td>
                        <td>
                            &nbsp;</td>
                        <td>
                            &nbsp;</td>
                        <td>
                            &nbsp;</td>
                        <td>&nbsp;</td>
                    </tr>
                </table>
            </asp:Panel>
            <br />
    <rsweb:ReportViewer ID="rptRelatorioHospedagem" runat="server" Height="" Width="" ZoomMode="PageWidth" Font-Names="Verdana" Font-Size="8pt" Visible="False" WaitMessageFont-Names="Verdana" WaitMessageFont-Size="14pt" SizeToReportContent="True">
        <LocalReport ReportPath="InformacoesGerenciais\rptRelatorioHospedagem.rdlc">
        </LocalReport>
    </rsweb:ReportViewer>
    <p>
    </p>
</asp:Content>

