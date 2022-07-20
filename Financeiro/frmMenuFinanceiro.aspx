<%@ Page Title="Menu Financeiro" Language="VB" MasterPageFile="~/TurismoSocial.master" AutoEventWireup="false" CodeFile="frmMenuFinanceiro.aspx.vb" Inherits="frmMenuFinanceiro" %>

<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="asp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="conPlaHolTurismoSocial" runat="Server">
    <p>
        <br />
        <p>
            &nbsp;<p>
                &nbsp;<p>
                    &nbsp;<p>
                            &nbsp;<div align="center">
                                <table style="width: 70%;">
                                    <tr>
                                        <td valign="bottom">
                                            &nbsp;&nbsp;
                                            <br />
                                            <br />
                                        </td>
                                        <td valign="bottom">
                                            <asp:ImageButton ID="imgCaixaFinanceiro" runat="server" ImageUrl="~/images/CaixaFinanceiro.png" />
                                            <br />
                                            <asp:Label ID="lblCaixaFinanceiro" runat="server" Text="Caixa Financeiro"></asp:Label>
                                        </td>
                                        <td>
                                            <br />
                                            <br />
                                            <br />
                                            <asp:ImageButton ID="imgFechamamentoCxaConsumo" runat="server" 
                                                ImageUrl="~/images/FechamentoCxaConsumo.png" />
                                            <br />
                                            <asp:Label ID="lblOpinario" runat="server" 
                                                Text="Fechamento de Caixa<br>de Consumo"></asp:Label>
                                        </td>
                                        <td valign="bottom">
                                            &nbsp;&nbsp;<br />
                                            <br />
                                            <br />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:ImageButton ID="imgCartaoConsumo" runat="server" ImageUrl="~/images/CartaoConsumo.png" />
                                            <br />
                                            <asp:Label ID="lblCartaoConsumo" runat="server" Text="Cartão de Consumo"></asp:Label>
                                        </td>
                                        <td colspan="2">
                                            <asp:Image ID="imgSESC" runat="server" ImageUrl="~/images/sescDivulgacao.jpg" />
                                        </td>
                                        <td>
                                            <br />
                                            <br />
                                            <asp:ImageButton ID="imgRetiraCaixa" runat="server" 
                                                ImageUrl="~/images/RetiradaCaixa.png" />
                                            <br />
                                            <asp:Label ID="lblRetiradaCaixa" runat="server" Text="Retirada de Caixa"></asp:Label>
                                            <br />
                                            <br />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            &nbsp;
                                            <br />
                                        </td>
                                        <td colspan="2">
                                            <br />
                                            <br />
                                            <br />
                                            <br />
                                        </td>
                                        <td>
                                            <br />
                                        </td>
                                    </tr>
                                </table>
                            </div>
                        </p>
                        <p>
                        </p>
</asp:Content>
