<%@ Page Title="" Language="VB" MasterPageFile="~/TurismoSocial.master" AutoEventWireup="false" CodeFile="frmAlimentosBebidas.aspx.vb" Inherits="frmAlimentosBebidas" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="conPlaHolTurismoSocial" Runat="Server">
    <p>
        <br />
    </p>
    <p>
        &nbsp;</p>
    <p>
        &nbsp;</p>
    <p>
        &nbsp;</p>
        <div style="top: 150px; left: 0px; z-index: 10; width: 100%" align="center">
        <table style="width:70%" >
            <tr>
                <td>
                    &nbsp;</td>
                <td align="center">
                    <br />
                    <asp:ImageButton ID="imgMeiaDiaria" runat="server" 
                        ImageUrl="~/Images/MeiaDiaria.png" />
                    <br />
                    <asp:HyperLink ID="HyperLink1" runat="server">Meia Diária sem Pernoite</asp:HyperLink>
                </td>
                <td align="center">
                    <asp:ImageButton ID="imgPrevisao" runat="server" 
                        ImageUrl="~/Images/Previsao.png" />
                    <br />
                    <br />
                    Previsão do <br> Restaurante</td>
                <td>
                    &nbsp;</td>
            </tr>
            <tr>
                <td align="center">
                    <asp:ImageButton ID="imgLanchonete" runat="server" 
                        ImageUrl="~/images/Lanchonete.png" />
                    <br />
                    Lanchonetes</td>
                <td colspan="2" rowspan="2" align="center">
                    <asp:ImageButton ID="imgSESC" runat="server" 
                        ImageUrl="~/images/sescDivulgacao.png" />
                </td>
                <td align="center">
                    <asp:ImageButton ID="imgPasseios" runat="server" 
                        ImageUrl="~/images/Passeio.png" />
                    <br />
                    Passeios com almoço<br />
&nbsp;incluso<br />
                    <br />
                    </td>
            </tr>
            <tr>
                <td align="center">
                    <br />
                    <asp:ImageButton ID="imgVM" runat="server" 
                        ImageUrl="~/images/VendingMarchine.jpg" />
                    <br />
                    Vending Marchines</td>
                <td align="center">
                    <asp:ImageButton ID="ImgRegManual" runat="server" 
                        ImageUrl="~/images/RegistroManual.png" />
                    <br />
                    Registro Manual de Refeições<br />
                </td>
            </tr>
            <tr>
                <td align="center">
                    <asp:ImageButton ID="imbCortesiaConsumo" runat="server" ImageUrl="~/images/Consumo.png" />
                    <br />
                    Cortesias em Consumo</td>
                <td align="center">
                    <a href="frmHistoricoRefIndividual.aspx">
                    </a></a>
                    <asp:ImageButton ID="imgHistRefeicoes" runat="server" 
                        ImageUrl="~/images/Historico.png" />
                    <br />
                    Histórico de Refeições/Estatística<br />
                    <asp:HyperLink ID="HyperLink2" runat="server" NavigateUrl="~/AlimentosEBebidas/frmHistoricoComplementoDiariaRefeicoes.aspx">Serviço Extra de Diária</asp:HyperLink>
                    <br />
                </td>
                <td align="center">
                    <br />
                    <asp:ImageButton ID="imgHistRefeicao" runat="server" 
                        ImageUrl="~/images/HistoricoPorTipo.png" />
                    <br />
                    Histórico de Refeições por Tipo</td>
                <td>
                    &nbsp;</td>
            </tr>
        </table>
    </div>
    <p>
        &nbsp;</p>
    <p>
        &nbsp;</p>
    <p>
    </p>
    <p>
    </p>
    <p>
    </p>
</asp:Content>

