<%@ Page Title="Consulta de Caldos" Language="VB" MasterPageFile="~/TurismoSocial.master" AutoEventWireup="false" CodeFile="frmConsultaCaldo.aspx.vb" Inherits="InformacoesGerenciais_frmConsultaCaldo" %>

<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="asp" %>

<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=11.0.0.0, Culture=neutral, PublicKeyToken=89845dcd8080cc91" Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="conPlaHolTurismoSocial" Runat="Server">

    <script src="../JScript.js" type="text/javascript"></script>
    <p>
        <br />
    </p>
    <p>
        &nbsp;</p>
    <p>
        &nbsp;</p>
    <p>
        &nbsp;</p>
    <div align="center">
            <asp:Panel ID="pnlConsulta" runat="server" Width="50%">
                <table>
                    <tr>
                        <td class="formRuler" colspan="3" style="font-weight: bold">
                            CONSULTA DE CALDOS VENDIDOS</td>
                    </tr>
                    <tr>
                        <td>
                            Data Inicial<br />
                            <asp:TextBox ID="Data1" runat="server" Width="80px"></asp:TextBox>
                            <asp:MaskedEditExtender ID="Data1_MaskedEditExtender" runat="server" 
                                AutoComplete="False" ClearMaskOnLostFocus="False" CultureAMPMPlaceholder="" 
                                CultureCurrencySymbolPlaceholder="" CultureDateFormat="" 
                                CultureDatePlaceholder="" CultureDecimalPlaceholder="" 
                                CultureThousandsPlaceholder="" CultureTimePlaceholder="" Enabled="True" 
                                Mask="99/99/9999" TargetControlID="Data1">
                            </asp:MaskedEditExtender>
                            <asp:CalendarExtender ID="Data1_CalendarExtender" runat="server" Enabled="True" 
                                FirstDayOfWeek="Sunday" Format="dd/MM/yyyy" TargetControlID="Data1">
                            </asp:CalendarExtender>
                        </td>
                        <td>
                            Data Final<br />
                            <asp:TextBox ID="Data2" runat="server" Width="80px"></asp:TextBox>
                            <asp:MaskedEditExtender ID="Data2_MaskedEditExtender" runat="server" 
                                AutoComplete="False" ClearMaskOnLostFocus="False" CultureAMPMPlaceholder="" 
                                CultureCurrencySymbolPlaceholder="" CultureDateFormat="" 
                                CultureDatePlaceholder="" CultureDecimalPlaceholder="" 
                                CultureThousandsPlaceholder="" CultureTimePlaceholder="" Enabled="True" 
                                Mask="99/99/9999" TargetControlID="Data2">
                            </asp:MaskedEditExtender>
                            <asp:CalendarExtender ID="Data2_CalendarExtender" runat="server" Enabled="True" 
                                FirstDayOfWeek="Sunday" Format="dd/MM/yyyy" TargetControlID="Data2">
                            </asp:CalendarExtender>
                        </td>
                        <td valign="bottom">
                            <asp:Button ID="btnConsultar" runat="server" AccessKey="c" CssClass="imgLupa" 
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
                    </tr>
                </table>
            </asp:Panel>
            <asp:RoundedCornersExtender ID="pnlConsulta_RoundedCornersExtender" 
                runat="server" BorderColor="ActiveBorder" Color="ActiveBorder" Enabled="True" 
                TargetControlID="pnlConsulta">
            </asp:RoundedCornersExtender>
            <asp:Panel ID="pnlGridCaldos" runat="server" Visible="False" Width="50%">
                <table>
                    <tr>
                        <td rowspan="2">
                            <asp:GridView ID="gdvGridCaldos" runat="server" AutoGenerateColumns="False" 
                                CellPadding="4" DataKeyNames="Data,Categoria,Qtde,Valor" ForeColor="#333333" 
                                GridLines="None">
                                <RowStyle BackColor="#EFF3FB" />
                                <Columns>
                                    <asp:BoundField DataField="Data" HeaderText="Data">
                                        <HeaderStyle CssClass="formRuler" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="Categoria" HeaderText="Categoria">
                                        <HeaderStyle CssClass="formRuler" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="Qtde" HeaderText="Qtde">
                                        <HeaderStyle CssClass="formRuler" />
                                        <ItemStyle HorizontalAlign="Right" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="Valor" HeaderText="Valor">
                                        <HeaderStyle CssClass="formRuler" />
                                        <ItemStyle HorizontalAlign="Right" />
                                    </asp:BoundField>
                                </Columns>
                                <FooterStyle BackColor="LightSteelBlue" Font-Bold="True" ForeColor="Black" />
                                <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
                                <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                                <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                                <EditRowStyle BackColor="lightsteelblue" />
                                <AlternatingRowStyle BackColor="White" />
                            </asp:GridView>
                        </td>
                        <td valign="top">
                            <asp:Button ID="btnImprimirCima" runat="server" Text="   Imprimir" 
                                CssClass="imgRelatorio" />
                        </td>
                        <td rowspan="2">
                            &nbsp;</td>
                    </tr>
                    <tr>
                        <td valign="bottom">
                            <asp:Button ID="btnImprimirBaixo" runat="server" Text="   Imprimir" 
                                CssClass="imgRelatorio" />
                        </td>
                    </tr>
                </table>
            </asp:Panel>
                <asp:RoundedCornersExtender ID="pnlGridCaldos_RoundedCornersExtender" 
                    runat="server" BorderColor="ActiveBorder" Color="ActiveBorder" Enabled="True" 
                    TargetControlID="pnlGridCaldos">
                </asp:RoundedCornersExtender>
                <rsweb:ReportViewer ID="rptCaldos" runat="server" Font-Names="Verdana" 
                Font-Size="8pt" Visible="False" Width="100%">
                    <LocalReport ReportPath="InformacoesGerenciais\RelConsultaCaldos.rdlc">
                    </LocalReport>
                </rsweb:ReportViewer>
            <br />
            <br />
            <br />
        </div>
    <br />
    </p>
</asp:Content>

