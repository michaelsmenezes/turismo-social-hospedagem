<%@ Page Title="Serviço Extra de Diária" Language="VB" MasterPageFile="~/TurismoSocial.master" AutoEventWireup="true" CodeFile="frmHistoricoComplementoDiariaRefeicoes.aspx.vb" Inherits="frmHistoricoComplementoDiariaRefeicoes" EnableEventValidation="false"%>

<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="asp" %>

<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=11.0.0.0, Culture=neutral, PublicKeyToken=89845dcd8080cc91" Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <style type="text/css">
        .ArrendodarBorda {}
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="conPlaHolTurismoSocial" Runat="Server">
    <asp:UpdatePanel ID="updGeral" runat="server">
        <ContentTemplate>
            <br />
            <br />
            <br />
            <br />
            <br />
            <br />
            <br />
            <asp:Panel ID="pnlPesquisa" runat="server" Width="95%" CssClass="ArrendodarBorda" >
                <div align="center" class="ArrendodarBorda formRuler" style="font-weight:bold; font-size: x-large; color: #FFFFFF;"  >Serviço Extra de Diária</div>
                <table>
                    <tr valign="bottom"  >
                        <td >
                            Mês</td>
                        <td>
                            &nbsp;</td>
                        <td >
                            Ano</td>
                        <td>
                            Dia Semana</td>
                        <td style="display: none">Tipos de Reservas</td>
                        <td>Adquirido até às </td>
                        <td>
                            &nbsp;</td>
                        <td>
                            &nbsp;</td>
                        <td>
                            &nbsp;</td>
                        <td align="center">
                            Total de<br />
                            SED</td>
                    </tr>
                    <tr>
                        <td>
                            <asp:DropDownList ID="drpMes" runat="server" CssClass="ArrendodarBorda">
                                <asp:ListItem Value="1">Janeiro</asp:ListItem>
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
                                <asp:ListItem Value="12">Dezembro</asp:ListItem>
                            </asp:DropDownList>
                        </td>
                        <td>
                            &nbsp;</td>
                        <td>
                            <asp:DropDownList ID="drpAno" runat="server" CssClass="ArrendodarBorda">
                            </asp:DropDownList>
                        </td>
                        <td>
                            <asp:DropDownList ID="drpDiaSemana" runat="server" CssClass="ArrendodarBorda">
                                <asp:ListItem Value="T">Todos</asp:ListItem>
                                <asp:ListItem Value="1">Domingo</asp:ListItem>
                                <asp:ListItem Value="2">Segunda</asp:ListItem>
                                <asp:ListItem Value="3">Terça</asp:ListItem>
                                <asp:ListItem Value="4">Quarta</asp:ListItem>
                                <asp:ListItem Value="5">Quinta</asp:ListItem>
                                <asp:ListItem Value="6">Sexta</asp:ListItem>
                                <asp:ListItem Value="7">Sábado</asp:ListItem>
                            </asp:DropDownList>
                        </td>
                        <td style="display: none">
                            <asp:DropDownList ID="drpCaracteristica" runat="server" CssClass="ArrendodarBorda" Enabled="False">
                                <asp:ListItem Value="T">Todos</asp:ListItem>
                                <asp:ListItem Value="E">Excurções/Passeios</asp:ListItem>
                                <asp:ListItem Value="I" Selected="True">Individual</asp:ListItem>
                            </asp:DropDownList>
                        </td>
                        <td align="center">
                            <asp:DropDownList ID="drpHoraAquisicao" runat="server" CssClass="ArrendodarBorda">
                                <asp:ListItem Value="0">Horas</asp:ListItem>
                                <asp:ListItem>1</asp:ListItem>
                                <asp:ListItem>2</asp:ListItem>
                                <asp:ListItem>3</asp:ListItem>
                                <asp:ListItem>4</asp:ListItem>
                                <asp:ListItem>5</asp:ListItem>
                                <asp:ListItem>6</asp:ListItem>
                                <asp:ListItem>7</asp:ListItem>
                                <asp:ListItem>8</asp:ListItem>
                                <asp:ListItem>9</asp:ListItem>
                                <asp:ListItem>10</asp:ListItem>
                                <asp:ListItem>11</asp:ListItem>
                                <asp:ListItem>12</asp:ListItem>
                                <asp:ListItem>13</asp:ListItem>
                                <asp:ListItem>14</asp:ListItem>
                                <asp:ListItem>15</asp:ListItem>
                                <asp:ListItem>16</asp:ListItem>
                                <asp:ListItem>17</asp:ListItem>
                                <asp:ListItem>18</asp:ListItem>
                                <asp:ListItem>19</asp:ListItem>
                                <asp:ListItem>20</asp:ListItem>
                                <asp:ListItem>21</asp:ListItem>
                                <asp:ListItem>22</asp:ListItem>
                                <asp:ListItem>23</asp:ListItem>
                                <asp:ListItem>24</asp:ListItem>
                            </asp:DropDownList>
                            </td>
                        <td>
                            <asp:Button ID="btnConsultar" runat="server" Height="22px" CssClass="imgLupa ArrendodarBorda" 
                                Text="  Consultar" />
                        </td>
                        <td>
                            <asp:Button ID="btnImprimir" runat="server" Height="22px" CssClass="imgImprimir ArrendodarBorda" 
                                Text="   Imprimir" />
                        </td>
                        <td>
                            <asp:Button ID="btnVoltar" runat="server" Height="22px" CssClass="imgVoltar ArrendodarBorda" Text="   Voltar" />
                        </td>
                        <td align="center" class="ArrendodarBorda">
                            <asp:Label ID="lblTotRefeicao" runat="server" Font-Size="Large"></asp:Label>
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
                        <td>&nbsp;</td>
                        <td>
                            &nbsp;</td>
                        <td>
                            &nbsp;</td>
                        <td>
                            &nbsp;</td>
                        <td>
                            &nbsp;</td>
                    </tr>
                </table>
            </asp:Panel>
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
            <asp:Panel ID="pnlGridHistorico" Width="95%" CssClass="ArrendodarBorda" runat="server">
                <asp:GridView ID="gdvHistoricoRef" runat="server" AutoGenerateColumns="False" CellPadding="4" DataKeyNames="Quantidade  ,Data      ,CheckOut    ,DiaSemana     ,TotalAlmoco" EmptyDataText="Não existem informações a serem exibidas" Font-Bold="False" ForeColor="#333333" GridLines="None" ShowFooter="True" Width="100%">
                    <RowStyle BackColor="#EFF3FB" CssClass="forRuller" />
                    <Columns>
                        <asp:BoundField DataField="Data" HeaderText="Data">
                        <HeaderStyle CssClass="formRuler" />
                        <ItemStyle HorizontalAlign="Center" />
                        </asp:BoundField>
                        <asp:BoundField DataField="DiaSemana" HeaderText="Dia da semana">
                        <HeaderStyle CssClass="formRuler" />
                        <ItemStyle HorizontalAlign="Center" />
                        </asp:BoundField>
                        <asp:BoundField DataField="Quantidade" HeaderText="Quantidade">
                        <FooterStyle HorizontalAlign="Center" />
                        <HeaderStyle CssClass="formRuler" />
                        <ItemStyle HorizontalAlign="Center" />
                        </asp:BoundField>
                        <asp:BoundField DataField="TotalAlmoco" HeaderText="R$" Visible="False">
                        <FooterStyle HorizontalAlign="Right" />
                        <HeaderStyle CssClass="formRuler" />
                        <ItemStyle HorizontalAlign="Right" />
                        </asp:BoundField>
                        <asp:BoundField HeaderText="% Check-out">
                        <HeaderStyle CssClass="formRuler" HorizontalAlign="Center" />
                        <ItemStyle HorizontalAlign="Center" />
                        </asp:BoundField>
                    </Columns>
                    <FooterStyle CssClass="formRuler" Font-Bold="True" ForeColor="Black" />
                    <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
                    <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                    <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="Black" />
                    <EditRowStyle BackColor="#2461BF" />
                    <AlternatingRowStyle BackColor="White" />
                </asp:GridView>
                <br/>
                <br />
             </asp:Panel>
            <rsweb:ReportViewer ID="rptEstatistica" runat="server" Font-Names="Verdana" 
                Font-Size="8pt" Visible="False" Width="100%" Height="" WaitMessageFont-Names="Verdana" WaitMessageFont-Size="14pt">
                <LocalReport ReportPath="AlimentosEBebidas\rptComplementoDiaria.rdlc">
                </LocalReport>
            </rsweb:ReportViewer>

            <br />
        </ContentTemplate>
    </asp:UpdatePanel>
    <div style="display: none" >
    <asp:Button ID="btnGeraEstatistica" runat="server" Text="Gerar Estatistica" 
        Visible="true" />
    </div>    
 </asp:Content>

