<%@ Page Title="" Language="VB" MasterPageFile="~/TurismoSocial.master" AutoEventWireup="true"
    CodeFile="frmMenuGerencial.aspx.vb" Inherits="InformacoesGerenciais_frmMenuGerencial" UICulture="Auto" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <link href="../stylesheet.css" rel="stylesheet" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="conPlaHolTurismoSocial" runat="Server">
    <br />
    <br />
    <br />
    <br />
    <br />
    <br />
    &nbsp;<div align="center">
        <table style="width: 70%;" align="center">
            <tr>
                <td align="center">
                    <br />
                    <br />
                    <asp:ImageButton ID="px" runat="server"
                        ImageUrl="~/images/Aniversariantes.png" Height="64px" Width="64px" />
                    <br />
                    <asp:Label ID="lblAniversariantes" runat="server" Text="Hóspedes Aniversariantes"></asp:Label>
                </td>
                <td align="center">
                    <br />
                    <asp:ImageButton ID="imgPrevEstada" runat="server"
                        ImageUrl="~/images/PrevisaodeEstada.png" />
                    <br />
                    <asp:Label ID="lblPrevEstada" runat="server" Text="Previsão de Estada"></asp:Label>
                </td>
                <td align="center">
                    <br />
                    <br />
                    <asp:ImageButton ID="imgOverBooking" runat="server"
                        ImageUrl="~/images/OverBooking.png" Width="65px" />
                    <br />
                    <asp:Label ID="lblOverBook" runat="server" Text="OverBooking"></asp:Label>
                    <br />
                </td>
                <td align="center">
                    <br />
                    <br />
                    <asp:ImageButton ID="imgTransferencia" runat="server"
                        ImageUrl="~/images/Relatoriotransferencia.png" Width="65px" ToolTip="Relação de transferência de hospedagem" />
                    <br />
                    <asp:Label ID="lblOverBook0" runat="server" Text="Transferência"></asp:Label>
                </td>
                <td align="center">
                    <br />
                    <asp:Panel ID="pnlRelGerenciaCaldas" runat="server" Visible="False">
                        <table align="center">
                            <tr>
                                <td>
                                    <asp:Image ID="imgAnhanguera" runat="server"
                                        ImageUrl="~/images/AptoA.png" ToolTip="Edifício Rio Tocantins"
                                        CssClass="ColocaHand" />
                                </td>
                                <td>
                                    <asp:Image ID="imgBambui" runat="server" ImageUrl="~/images/AptoB.png"
                                        ToolTip="Edifício Rio Araguaia" CssClass="ColocaHand" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Image ID="imgWilton" runat="server" ImageUrl="~/images/AptoW.png"
                                        ToolTip="Edifício Rio Vermelho" CssClass="ColocaHand" />
                                </td>
                                <td align="center">
                                    <asp:Image ID="imgKilzer" runat="server" ImageUrl="~/images/AptoK.png"
                                        ToolTip="Edifício Rio Paranaíba" CssClass="ColocaHand" />
                                </td>
                            </tr>
                            <tr>
                                <td align="center" colspan="2">
                                    <asp:ImageButton ID="imgTodos" runat="server" ImageUrl="~/images/Todos.png"
                                        ToolTip="Todos os Blocos" />
                                </td>
                            </tr>
                        </table>
                    </asp:Panel>
                    <asp:Panel ID="PnlRelGerencialPiri" runat="server">
                        <asp:ImageButton ID="imgRelatorioHospedagem" runat="server"
                            ImageUrl="~/images/RelatorioHospedagem.png" />
                    </asp:Panel>
                    <asp:ImageButton ID="imgRelHospedagemNew" runat="server" ImageUrl="~/images/RelatorioHospedagemNew.png" ToolTip="Novo relatório hospedagem" />
                    <br />
                    <asp:Label ID="lblRelatorioHospedagem" runat="server"
                        Text="Relatório de Hospedagem"></asp:Label>
                </td>

                <td align="center">
                    <br />
                    <asp:ImageButton ID="imgDemanda" runat="server" ImageUrl="~/images/Demanda.png" />
                    <br />
                    <br />
                    <asp:Label ID="lblAnaliseDemanda" runat="server" Text="Análise de Demanda<br/>Turismo Emissivo"></asp:Label>
                    <br />
                    <asp:ImageButton ID="imgCaldosVendidos" runat="server"
                        ImageUrl="~/images/CaldosPiri.png" />
                    <br />
                    <asp:Label ID="lblCaldo" runat="server" Text="Relatório de Caldos<br>Vendidos"></asp:Label>
                </td>
            </tr>
            <tr>
                <td align="center">
                    <asp:ImageButton ID="imgHistHospede" runat="server"
                        ImageUrl="~/images/HistoricoHospede.png" />
                    <br />
                    <asp:Label ID="lblHistorioHospedes" runat="server" Text="Histórico de Hospedes"></asp:Label>
                </td>
                <td colspan="4" rowspan="2" align="center">
                    <asp:Image ID="imgSESC" runat="server" ImageUrl="~/images/sescDivulgacao.jpg" />
                </td>
                <td align="center">
                    <asp:ImageButton ID="imgFreqPassante" runat="server"
                        ImageUrl="~/images/ConsultaPassante.png" />
                    <br />
                    <asp:Label ID="lblFreqPassante" runat="server"
                        Text="Pesquisa de Frequência<br>de Passantes"></asp:Label>
                </td>
            </tr>
            <tr>
                <td align="center">
                    <asp:ImageButton ID="imgHistPassantes" runat="server"
                        ImageUrl="~/images/HistoricoPassante.png" />
                    <br />
                    <asp:Label ID="lblHistoricoPassante" runat="server"
                        Text="Histórico de Passantes"></asp:Label>
                </td>
                <td align="center">
                    <asp:ImageButton ID="imgHospPorCategoria" runat="server"
                        ImageUrl="~/images/HopedePorCategoria.png" />
                    <br />
                    <asp:Label ID="lblHospedePorCategoria" runat="server"
                        Text="Consulta de Hóspedes&lt;br&gt;por Categoria"></asp:Label>
                </td>
            </tr>
            <tr>
                <td align="center">
                    <asp:ImageButton ID="imgBI" runat="server" ImageUrl="~/images/BI.png" ToolTip="Visualização de relatórios gerenciais" />
                    <br />
                    <asp:Label ID="lblBi" runat="server" Text="BI&lt;br&gt;Ferramenta de Gestão"></asp:Label>
                </td>
                <td align="center">
                    <br />
                    <asp:ImageButton ID="imgOpinario" runat="server"
                        ImageUrl="~/images/Opinario.png" />
                    <br />
                    <asp:Label ID="lblOpinario" runat="server" Text="Opinário"></asp:Label>
                    <br />
                </td>
                <td align="center">
                    <asp:ImageButton ID="imgCortesiasHospedagem" runat="server" ImageUrl="~/images/RelatorioCortesias.png" Width="64px" Height="64px" />
                    <br />
                    Cortesias em<br />
                    &nbsp;Hospedagem</td>
                <td align="center" valign="bottom">
                    <asp:ImageButton ID="imgEmbratur" runat="server"
                        ImageUrl="~/images/Embratur.png" />
                    <br />
                    <asp:Label ID="lblEmbratur" runat="server"
                        Text="Boletim de&lt;br&gt;Ocupação Hoteleira"></asp:Label>
                </td>
                <td align="center">
                    <asp:ImageButton ID="imgRelReservasCanceladas" runat="server"
                        ImageUrl="~/images/ReservasCanceladas.png" />
                    <br />
                    <asp:Label ID="lblReservasCanceladas" runat="server" Text="Relatório de Reservas<br>Canceladas"></asp:Label>
                </td>
                
                <td align="center">
                    <asp:ImageButton ID="imgPgtoCartaoCredito" runat="server"
                        ImageUrl="~/images/CartaoConsumo.png" />
                    <br />
                    <asp:Label ID="lblPgtoCCred" runat="server" Text="Relatório de Pagamentos<br>Cartões de Crédito"></asp:Label>
                </td>
            </tr>
        </table>
    </div>
    <p>
    </p>
</asp:Content>

