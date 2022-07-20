<%@ Page Title="Menu Recepção" Language="VB" MasterPageFile="~/TurismoSocial.master" AutoEventWireup="false"
    CodeFile="frmMenuRecepcao.aspx.vb" Inherits="InformacoesGerenciais_frmMenuRecepcao" %>

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
                                            <asp:ImageButton ID="imgRetiraCaixa" runat="server" 
                                                ImageUrl="~/images/RetiradaCaixa.png" />
                                            <br />
                                            <asp:Label ID="lblRetiradaCaixa" runat="server" Text="Retirada de Caixa"></asp:Label>
                                        </td>
                                        <td>
                                            <br />
                                            <asp:ImageButton ID="imgCartaoConsumo" runat="server" ImageUrl="~/images/CartaoConsumo.png" />
                                            <br />
                                            <asp:Label ID="lblCartaoConsumo" runat="server" Text="Cartão de Consumo"></asp:Label>
                                        </td>
                                        <td>
                                            <br />
                                            <br />
                                            <asp:ImageButton ID="imgCaixaFinanceiro" runat="server" ImageUrl="~/images/CaixaFinanceiro.png" />
                                            <br />
                                            <asp:Label ID="lblCaixaFinanceiro" runat="server" Text="Caixa Financeiro"></asp:Label>
                                        </td>
                                        <td valign="bottom">
                                            &nbsp;<asp:ImageButton ID="imgConsultaSPC" runat="server" ImageUrl="~/images/ConsultaSPC.png" />
                                            &nbsp;<br />
                                            <asp:Label ID="lblConsultaSPC" runat="server" Text="Consulta SPC"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <br />
                                            <br />
                                            <asp:ImageButton ID="ImageButton1" runat="server" 
                                                ImageUrl="~/images/Reserva.png" />
                                            <br />
                                            <asp:Label ID="lblReserva" runat="server" Text="Reservas"></asp:Label>
                                        </td>
                                        <td colspan="2" rowspan="2">
                                            <asp:Image ID="imgSESC" runat="server" ImageUrl="~/images/sescDivulgacao.jpg" />
                                        </td>
                                        <td>
                                            <asp:ImageButton ID="imgPassantes" runat="server" ImageUrl="~/images/Passantes.png" />
                                            <br />
                                            <asp:Label ID="lblPassantes" runat="server" Text="Passante"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:ImageButton ID="imgRecepcaoNet" runat="server" 
                                                ImageUrl="~/images/RecepcaoNet.png" ToolTip="Abre o novo sistema de recepção" />
                                            <br />
                                            <asp:Label ID="lblRecepcaoNet" runat="server" Text="Recepção"></asp:Label>
                                            <br />
                                            <br />
                                        </td>
                                        <td valign="bottom">
                                            <asp:ImageButton ID="imgOpinario" runat="server" ImageUrl="~/images/Opinario.png" />
                                            <br />
                                            <asp:Label ID="lblOpinario" runat="server" Text="Opinário"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            &nbsp;
                                            <br />
                                        </td>
                                        <td colspan="2">
                                            <br />
                                            <asp:ImageButton ID="imgPortariaCaldas" runat="server" 
                                                ImageUrl="~/images/PortariaCaldas.jpg" />
                                            <br />
                                            <asp:Label ID="lblPortariaCaldas" runat="server" Text="Portaria Caldas"></asp:Label>
                                        </td>
                                        <td>
                                            <br />
                                            <br />
                                            <br />
                                        </td>
                                    </tr>
                                </table>
                            </div>
                        </p>
                        <p>
                        </p>
</asp:Content>
