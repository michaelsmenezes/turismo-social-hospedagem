<%@ Page Title="" Language="VB" MasterPageFile="~/TurismoSocial.master" AutoEventWireup="false" CodeFile="frmHistoricoRefIndividual.aspx.vb" Inherits="frmHistoricoRefIndividual" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <style type="text/css">
        .imgLupa {
            height: 26px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="conPlaHolTurismoSocial" runat="Server">
    <asp:UpdatePanel ID="updGeral" runat="server">
        <ContentTemplate>
            <br />
            <br />
            <br />
            <br />
            <br />
            <br />
            <br />
            <asp:Panel ID="pnlPesquisa" runat="server" Width="95%">
                <table>
                    <tr>
                        <td>Mês</td>
                        <td>Ano</td>
                        <td>&nbsp;</td>
                        <td>&nbsp;</td>
                        <td>&nbsp;</td>
                        <td>&nbsp;</td>
                        <td align="center">&nbsp;</td>
                    </tr>
                    <tr>
                        <td>
                            <asp:DropDownList ID="drpMes" runat="server">
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
                            <asp:DropDownList ID="drpAno" runat="server">
                            </asp:DropDownList>
                        </td>
                        <td>
                            <asp:DropDownList ID="drpTipo" runat="server">
                                <asp:ListItem Value="D">Desjejum</asp:ListItem>
                                <asp:ListItem Value="A">Almoco</asp:ListItem>
                                <asp:ListItem Value="J">Jantar</asp:ListItem>
                            </asp:DropDownList>
                        </td>
                        <td>
                            <asp:Button ID="btnConsultar" runat="server" CssClass="imgLupa"
                                Text="  Consultar" />
                        </td>
                        <td>
                            <asp:Button ID="btnImprimir" runat="server" CssClass="imgRelatorio"
                                OnClientClick="print();" Text="   Imprimir" Height="26px" />
                        </td>
                        <td>
                            <asp:Button ID="btnVoltar" runat="server" CssClass="imgVoltar" Text="   Voltar" />
                        </td>
                        <td align="center">&nbsp;</td>
                    </tr>
                    <tr>
                        <td>&nbsp;</td>
                        <td>&nbsp;</td>
                        <td>&nbsp;</td>
                        <td>&nbsp;</td>
                        <td>&nbsp;</td>
                        <td>&nbsp;</td>
                        <td>&nbsp;</td>
                    </tr>
                </table>
            </asp:Panel>
            <asp:RoundedCornersExtender ID="pnlPesquisa_RoundedCornersExtender"
                runat="server" Enabled="True" TargetControlID="pnlPesquisa" Radius="7" BorderColor="ActiveBorder">
            </asp:RoundedCornersExtender>
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
            <asp:Panel ID="pnlGridHistorico" runat="server" Width="95%">
                <br />
                <table style="width: 100%;">
                    <tr>
                        <td align="center" colspan="10" style="font-size: large; font-weight: normal">
                            <asp:Label ID="lblTipoRefeicao" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="10">
                            <asp:GridView ID="gdvHistoricoRef" runat="server" AutoGenerateColumns="False"
                                CellPadding="4" ForeColor="#333333" GridLines="None" ShowFooter="True">
                                <RowStyle BackColor="#EFF3FB" />
                                <Columns>
                                    <asp:BoundField DataField="Dia" HeaderText="Dia">
                                        <HeaderStyle CssClass="formRuler" />
                                        <FooterStyle CssClass="formRuler" />
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="Comerciario" HeaderText="Comerciário">
                                        <HeaderStyle CssClass="formRuler" />
                                        <FooterStyle CssClass="formRuler" />
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="PasComerciario" HeaderText="Passante Comerciario">
                                        <HeaderStyle CssClass="formRuler" />
                                        <FooterStyle CssClass="formRuler" />
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="InsercaoManual" HeaderText="Insercão Manual">
                                        <HeaderStyle CssClass="formRuler" />
                                        <FooterStyle CssClass="formRuler" />
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="Servidores" HeaderText="Servidores">
                                        <HeaderStyle CssClass="formRuler" />
                                        <FooterStyle CssClass="formRuler" />
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="Dependente" HeaderText="Dependente">
                                        <HeaderStyle CssClass="formRuler" />
                                        <FooterStyle CssClass="formRuler" />
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="PasDependente" HeaderText="Passante Dependente">
                                        <HeaderStyle CssClass="formRuler" />
                                        <FooterStyle CssClass="formRuler" />
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="Menor5" HeaderText="Menor 5 Anos">
                                        <HeaderStyle CssClass="formRuler" />
                                        <FooterStyle CssClass="formRuler" />
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="Usuario" HeaderText="Usuário">
                                        <HeaderStyle CssClass="formRuler" />
                                        <FooterStyle CssClass="formRuler" />
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="PasUsuario" HeaderText="Passante Usuario">
                                        <HeaderStyle CssClass="formRuler" />
                                        <FooterStyle CssClass="formRuler" />
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="Conveniado" HeaderText="Conveniado">
                                        <HeaderStyle CssClass="formRuler" />
                                        <FooterStyle CssClass="formRuler" />
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="TotalLinha" HeaderText="Soma" ItemStyle-Font-Bold="true" >
                                        <HeaderStyle CssClass="formRuler" />
                                        <FooterStyle CssClass="formRuler" />
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="Cortesias" HeaderText="Cortesias">
                                        <HeaderStyle CssClass="formRuler" />
                                        <FooterStyle CssClass="formRuler" />
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:BoundField>
                                </Columns>
                                <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                                <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
                                <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                                <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                                <EditRowStyle BackColor="#2461BF" />
                                <AlternatingRowStyle BackColor="White" />
                            </asp:GridView>
                        </td>
                    </tr>
                    <tr>
                        <td valign="top">&nbsp;</td>
                        <td>&nbsp;</td>
                        <td align="center">&nbsp;</td>
                        <td align="right">&nbsp;</td>
                        <td></td>
                        <td>&nbsp;</td>
                        <td align="right">&nbsp;</td>
                        <td>&nbsp;</td>
                        <td align="right">&nbsp;</td>
                        <td>&nbsp;</td>
                    </tr>
                </table>
            </asp:Panel>
            <asp:RoundedCornersExtender ID="pnlGridHistorico_RoundedCornersExtender"
                runat="server" Enabled="True" TargetControlID="pnlGridHistorico" Radius="7" BorderColor="ActiveBorder">
            </asp:RoundedCornersExtender>
            <br />
            <br />
            <br />
            <br />
            <br />
            <br />
            <br />
        </ContentTemplate>
    </asp:UpdatePanel>
    <p>
        <br />
    </p>
    <p>
    </p>
    <p>
    </p>
</asp:Content>

