<%@ Page Title="" Language="VB" MasterPageFile="~/TurismoSocial.master" AutoEventWireup="True" CodeFile="frmVendingMarchines.aspx.vb" Inherits="frmVendingMarchines" %>

<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="asp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="conPlaHolTurismoSocial" Runat="Server">
    <p>
        <br />
    </p>
    <asp:UpdatePanel ID="updGeral" runat="server">
        <ContentTemplate>
            <br />
            <br />
            <br />
            <br />
            <br />
            <br />
            <br />
            <div style="top: 150px; left: 0px; z-index: 10; width: 100%" align="center" >
            <asp:panel ID="pnlGeral" runat="server" Width="60%">
            <table style="width:60%;">
                <tr align="left">
                    <td align="center" class="formRuler" colspan="2" style="font-size: medium">
                        Vending Machines</td></tr><tr>
                    <td align="center">
                        &nbsp;</td><td align="center">
                        &nbsp;</td></tr><tr>
                    <td align="center">
                        &nbsp;</td><td align="center">
                        &nbsp;</td></tr><tr>
                    <td align="center" style="font-size: medium">
                        <asp:ImageButton ID="imgConfiguracao" runat="server" 
                            ImageUrl="../Images/ConfiguracaoVM.png" Width="80px" />
                        <br />
                        Configurações</td><td align="center" style="font-size: medium">
                        <asp:ImageButton ID="imgRelatorioVM" runat="server" 
                            ImageUrl="../Images/ReportVM.png" Width="80px" />
                        <br />
                        Relatórios</td></tr><tr>
                    <td>
                        &nbsp;</td><td align="right">
                        <asp:Button ID="btnVoltar" runat="server" CssClass="imgVoltar" Text="   Voltar" />
                    </td>
                </tr>
            </table>
            </asp:panel>
                <asp:RoundedCornersExtender ID="pnlGeral_RoundedCornersExtender" runat="server" 
                    BorderColor="ActiveBorder" Enabled="True" Radius="7" TargetControlID="pnlGeral">
                </asp:RoundedCornersExtender>
            </div>
            <div align="center" class="formLabel">
                <asp:UpdateProgress ID="updProcesso" runat="server" 
                    AssociatedUpdatePanelID="updGeral">
                    <ProgressTemplate>
                        Processando sua solicitação...<br />
                        &nbsp;<img alt="Processando..." src="../images/Aguarde.gif" />
                    </ProgressTemplate>
                </asp:UpdateProgress>
            </div>
            <br />
        </ContentTemplate>
    </asp:UpdatePanel>
    <p>
    </p>
</asp:Content>

