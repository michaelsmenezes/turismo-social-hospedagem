<%@ Page Title="" Language="VB" MasterPageFile="~/TurismoSocial.master" AutoEventWireup="True" CodeFile="frmLanchonetes.aspx.vb" Inherits="frmLanchonetes" %>

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
            <div style="top: 150px; left: 0px; z-index: 10; width: 100%" align="center">
            <asp:Panel ID="pnlGeral" runat="server" Width="60%">
            <table style="width:100%">
                <tr>
                    <td class="formRuler" colspan="4" style="font-size: medium">
                        Lanchonetes</td></tr><tr>
                    <td align="center" style="font-size: medium">
                        <asp:ImageButton ID="imgConfiguracao" runat="server" 
                            ImageUrl="../Images/Consumo.png" Width="80px" />
                        <br />
                        Consumo</td><td align="center">
                        <asp:ImageButton ID="imgConsultaPratos" runat="server" 
                            ImageUrl="../Images/ConsultaPratosR.png" Width="80px" />
                        <br />
                        <asp:Label ID="lblConsultaP" runat="server" 
                            Text="Consulta de Pratos <br> Rápidos" Font-Size="Medium"></asp:Label></td><td align="center">
                        <asp:ImageButton ID="imgEntregaR" runat="server" 
                            ImageUrl="../Images/EntregadePratos.png" Width="80px" />
                        <br />
                        <asp:Label ID="lblEntregaP" runat="server" 
                            Text="Entrega de Pratos <br> Rápidos " Font-Size="Medium"></asp:Label><br />
                    </td>
                    <td align="center" style="font-size: medium">
                        <asp:ImageButton ID="imgPonto" runat="server" 
                            ImageUrl="~/Images/Registro de ponto.png" />
                        <br />
                        Registro de Ponto</td>
                </tr>
                <tr>
                    <td colspan="2">
                        &nbsp;</td><td align="right">
                        &nbsp;</td>
                    <td align="right">
                        <asp:Button ID="btnVoltar" runat="server" CssClass="imgVoltar" Text="   Voltar" />
                    </td>
                </tr>
            </table>
            </asp:Panel>
            </div>
            <asp:RoundedCornersExtender ID="pnlGeral_RoundedCornersExtender" runat="server" 
                BorderColor="ActiveBorder" Enabled="True" Radius="7" TargetControlID="pnlGeral">
            </asp:RoundedCornersExtender>
            <br />
            <div align="center" class="formLabel">
                <asp:UpdateProgress ID="updProRecepcao" runat="server" 
                    AssociatedUpdatePanelID="updGeral">
                    <ProgressTemplate>
                        Processando sua solicitação...<br />
                        &nbsp;<img alt="Processando..." src="../images/Aguarde.gif" />
                    </ProgressTemplate>
                </asp:UpdateProgress>
            </div>
            <br />
            <br />
        </ContentTemplate>
    </asp:UpdatePanel>
    <p>
    </p>
</asp:Content>

