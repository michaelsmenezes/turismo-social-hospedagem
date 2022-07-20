<%@ Page Title="" Language="VB" MasterPageFile="~/TurismoSocial.master" AutoEventWireup="True"
    CodeFile="PortariaRestaurante.aspx.vb" Inherits="PortariaRestaurante" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="conPlaHolTurismoSocial" runat="Server">
    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;    <asp:UpdatePanel ID="updPnlPortariaRestaurante" runat="server">
        <ContentTemplate>
            <br />
            <br />
            <br />
            <br />
            <br />
            <br />
            <br />
            <asp:Panel ID="pnlPortariaRestaurante" runat="server" CssClass="formLabel">
                <div class="formLabel" align="center">
                    <asp:UpdateProgress ID="updProRecepcao" runat="server" AssociatedUpdatePanelID="updPnlPortariaRestaurante">
                        <ProgressTemplate>
                            Processando sua solicitação...<br />
                            &nbsp;<img alt="Processando..." src="images/Aguarde.gif" /></ProgressTemplate>
                    </asp:UpdateProgress>
                </div>
                <asp:Label ID="lblCartao" runat="server" Text="Passe o Cartão" BorderColor="White"
                    BorderWidth="5px"></asp:Label>
                <asp:TextBox ID="txtConsulta" runat="server" AutoPostBack="True" CausesValidation="True"
                    Columns="6" MaxLength="6" TextMode="Password" ValidationGroup="Cartao"></asp:TextBox>
                <asp:Button ID="btnIsentoMais" runat="server" Text="+ Isento" />
                <asp:Button ID="btnIsentoMenos" runat="server" Text="- Isento" />
                <asp:Label ID="lblIsento" runat="server" BorderColor="White" BorderWidth="5px" Text=" "></asp:Label>
                <p>
                </p>
                <asp:Label ID="lblNome" runat="server" Text=" " BorderColor="White" BorderWidth="5px"></asp:Label>
                <p>
                </p>
                <asp:Label ID="lblSituacao" runat="server" Text=" " BorderColor="White" BorderWidth="5px"></asp:Label>
                <p>
                </p>
                <table>
                    <tr>
                        <td>
                            <asp:Button ID="btnSituacao" runat="server" Text="Situação" />
                        </td>
                        <td>
                            <asp:Label ID="lblHospede" runat="server" Text="Hóspedes | " Visible="False"></asp:Label>
                        </td>
                        <td>
                            <asp:Label ID="lblMeia" runat="server" Text="Meia Diária sem Pernoite" Visible="False"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="lblPrevisao" runat="server" Text="Previsão" Visible="False"></asp:Label>
                        </td>
                        <td align="center">
                            <asp:Label ID="lblPrevisaoHospede" runat="server" Text=" "></asp:Label>
                        </td>
                        <td align="center">
                            <asp:Label ID="lblPrevisaoMeia" runat="server" Text=" "></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="lblIngressaram" runat="server" Text="Ingressaram" Visible="False"></asp:Label>
                        </td>
                        <td align="center">
                            <asp:Label ID="lblIngressaramHospede" runat="server" Text=" "></asp:Label>
                        </td>
                        <td align="center">
                            <asp:Label ID="lblIngressaramMeia" runat="server" Text=" "></asp:Label>
                        </td>
                    </tr>
                </table>
                <asp:HiddenField ID="hddOperacao" runat="server" Value="false" />
                <asp:HiddenField ID="hddPeriodo1" runat="server" />
                <asp:HiddenField ID="hddPeriodo2" runat="server" />
            </asp:Panel>
            <asp:AlwaysVisibleControlExtender ID="pnlPortariaRestaurante_AlwaysVisibleControlExtender"
                runat="server" Enabled="True" HorizontalSide="Center" TargetControlID="pnlPortariaRestaurante"
                VerticalSide="Middle">
            </asp:AlwaysVisibleControlExtender>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
