<%@ Page Title="Previsão de Estada" Language="VB" MasterPageFile="~/TurismoSocial.master"
    AutoEventWireup="false" CodeFile="frmPrevisaoEstada.aspx.vb" Inherits="InformacoesGerenciais_frmPrevisaoEstada" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="conPlaHolTurismoSocial" runat="Server">
    <p>
        <br />
    </p>
    <p>
        &nbsp;</p>
    <p>
        &nbsp;</p>
    <p>
        &nbsp;</p>

    <script src="../JScript.js" type="text/javascript"></script>

    <asp:UpdatePanel ID="updGeral" runat="server">
        <ContentTemplate>
            <asp:Label ID="lblTitulo" runat="server" CssClass="formRuler" Font-Bold="True" Text="Previsão de ocupação por acomodação"
                Width="98%" Font-Size="X-Large"></asp:Label>
            <asp:RoundedCornersExtender ID="lblTitulo_RoundedCornersExtender" runat="server"
                BorderColor="ActiveBorder" Color="ActiveBorder" Enabled="True" TargetControlID="lblTitulo">
            </asp:RoundedCornersExtender>
            <br />
            <br />
            <asp:Panel ID="pnlPesquisa" runat="server" DefaultButton="btnConsultar" Width="98%">
                <br />
                <table>
                    <tr>
                        <td>
                            Data</td>
                        <td style="display: none">
                            Data Final
                        </td>
                        <td>
                            Bloco
                        </td>
                        <td>
                            Acomodação
                        </td>
                        <td>
                            &nbsp;
                        </td>
                        <td>
                            &nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:TextBox ID="txtDataIni" runat="server" Width="80px"></asp:TextBox>
                            <asp:CalendarExtender ID="txtDataIni_CalendarExtender" runat="server" Enabled="True"
                                FirstDayOfWeek="Sunday" Format="dd/MM/yyyy" TargetControlID="txtDataIni">
                            </asp:CalendarExtender>
                        </td>
                        <td style="display: none">
                            <asp:TextBox ID="txtDataFim" runat="server" Width="80px" Visible="False"></asp:TextBox>
                            <asp:CalendarExtender ID="txtDataFim_CalendarExtender" runat="server" Enabled="True"
                                FirstDayOfWeek="Sunday" Format="dd/MM/yyyy" TargetControlID="txtDataFim">
                            </asp:CalendarExtender>
                        </td>
                        <td>
                            <asp:DropDownList ID="drpBloco" runat="server">
                                <asp:ListItem Value="0">Todos</asp:ListItem>
                                <asp:ListItem Value="1">Rio Tocantins</asp:ListItem>
                                <asp:ListItem Value="2">Rio Araguaia</asp:ListItem>
                                <asp:ListItem Value="3">Rio Paranaiba</asp:ListItem>
                                <asp:ListItem Value="4">Rio Vermelho</asp:ListItem>
                            </asp:DropDownList>
                        </td>
                        <td>
                            <asp:DropDownList ID="drpAcomodacoes" runat="server">
                                <asp:ListItem Value="T">Todas</asp:ListItem>
                                <asp:ListItem Value="N">Central de Reserva</asp:ListItem>
                                <asp:ListItem Value="S">Federação</asp:ListItem>
                                <asp:ListItem Value="E">PNE</asp:ListItem>
                                <asp:ListItem Value="R">DR</asp:ListItem>
                                <asp:ListItem Value="F">Flutuante</asp:ListItem>
                            </asp:DropDownList>
                        </td>
                        <td>
                            <asp:Button ID="btnConsultar" runat="server" CssClass="imgLupa" Text="  Consultar" />
                        </td>
                        <td>
                            <asp:Button ID="btnVoltar" runat="server" CssClass="imgVoltar" Text="  Voltar" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            &nbsp;
                        </td>
                        <td>
                            &nbsp;
                        </td>
                        <td>
                            &nbsp;
                        </td>
                        <td>
                            &nbsp;
                        </td>
                        <td>
                            &nbsp;
                        </td>
                        <td>
                            &nbsp;
                        </td>
                    </tr>
                </table>
            </asp:Panel>
            <br />
            <div align="center">
                <asp:UpdateProgress ID="updProRecepcao" runat="server" AssociatedUpdatePanelID="updGeral">
                    <ProgressTemplate>
                        Processando sua solicitação...<br />
                        &nbsp;<img alt="Processando..." src="../images/Aguarde.gif" />
                    </ProgressTemplate>
                </asp:UpdateProgress>
            </div>
            <asp:Panel ID="pnlPrevisao" runat="server" Width="98%">
                <asp:GridView ID="gdvPrevisao" runat="server" AutoGenerateColumns="False" CellPadding="3"
                    EmptyDataText="Não existem dados a serem exibidos" GridLines="Vertical" BackColor="White" BorderColor="#999999" BorderStyle="None" BorderWidth="1px" DataKeyNames="Bloco,Caracteristica,Data,prvTotalLeitos,prvTotalPrevisao,AptoE,AptoR,AptoP,prvQtdeTotalAptos,prvQtdeAptos,prvBloqueados,AptoM,Livre" ShowFooter="True" Width="100%">
                    <RowStyle BackColor="#EEEEEE" ForeColor="Black" />
                    <Columns>
                        <asp:BoundField DataField="Caracteristica" HeaderText="Bloco">
                            <FooterStyle Font-Bold="True" Font-Size="Medium" HorizontalAlign="Left" />
                            <HeaderStyle CssClass="formRuler" HorizontalAlign="Center" Font-Size="Medium" />
                            <ItemStyle Font-Bold="True" Font-Size="Medium" />
                        </asp:BoundField>
                        <asp:BoundField DataField="prvTotalLeitos" HeaderText="Leitos">
                        <FooterStyle Font-Bold="True" Font-Size="Medium" HorizontalAlign="Center" />
                        <HeaderStyle CssClass="formRuler" Font-Size="Medium" />
                        <ItemStyle Font-Size="Medium" HorizontalAlign="Center" />
                        </asp:BoundField>
                        <asp:BoundField DataField="prvTotalPrevisao" HeaderText="Previsão">
                            <FooterStyle Font-Bold="True" Font-Size="Medium" HorizontalAlign="Right" />
                            <HeaderStyle CssClass="formRuler" HorizontalAlign="Center" Font-Size="Medium" />
                            <ItemStyle HorizontalAlign="Right" Font-Bold="True" Font-Size="Medium" />
                        </asp:BoundField>
                        <asp:BoundField DataField="AptoE" HeaderText="Em estada">
                            <FooterStyle Font-Bold="True" Font-Size="Medium" HorizontalAlign="Right" />
                            <HeaderStyle CssClass="formRuler" HorizontalAlign="Center" Font-Size="Medium" />
                            <ItemStyle HorizontalAlign="Right" Font-Size="Medium" />
                        </asp:BoundField>
                        <asp:BoundField DataField="AptoR" HeaderText="Confirmados">
                            <FooterStyle Font-Bold="True" Font-Size="Medium" HorizontalAlign="Right" />
                            <HeaderStyle CssClass="formRuler" HorizontalAlign="Center" Font-Size="Medium" />
                            <ItemStyle HorizontalAlign="Right" Font-Size="Medium" />
                        </asp:BoundField>
                        <asp:BoundField DataField="AptoP" HeaderText="A pagar">
                            <FooterStyle Font-Bold="True" Font-Size="Medium" HorizontalAlign="Right" />
                            <HeaderStyle CssClass="formRuler" HorizontalAlign="Center" Font-Size="Medium" />
                            <ItemStyle HorizontalAlign="Right" Font-Size="Medium" />
                        </asp:BoundField>
                        <asp:BoundField DataField="prvQtdeTotalAptos" HeaderText="Apartamentos">
                        <FooterStyle Font-Bold="True" Font-Size="Medium" HorizontalAlign="Center" />
                        <HeaderStyle CssClass="formRuler" Font-Size="Medium" />
                        <ItemStyle Font-Size="Medium" HorizontalAlign="Center" />
                        </asp:BoundField>
                        <asp:BoundField DataField="prvBloqueados" HeaderText="Aptos bloqueados">
                            <FooterStyle Font-Bold="True" Font-Size="Medium" HorizontalAlign="Right" />
                            <HeaderStyle CssClass="formRuler" HorizontalAlign="Center" Font-Size="Medium" />
                            <ItemStyle HorizontalAlign="Right" Font-Size="Medium" />
                        </asp:BoundField>
                        <asp:BoundField DataField="AptoM" HeaderText="Aptos manutenção">
                            <FooterStyle Font-Bold="True" Font-Size="Medium" HorizontalAlign="Right" />
                            <HeaderStyle CssClass="formRuler" HorizontalAlign="Center" Font-Size="Medium" />
                            <ItemStyle HorizontalAlign="Right" Font-Size="Medium" />
                        </asp:BoundField>
                        <asp:BoundField DataField="Livre" HeaderText="Aptos livres">
                            <FooterStyle Font-Bold="True" Font-Size="Medium" HorizontalAlign="Right" />
                            <HeaderStyle CssClass="formRuler" HorizontalAlign="Center" Font-Size="Medium" />
                            <ItemStyle HorizontalAlign="Right" Font-Bold="True" Font-Size="Medium" />
                        </asp:BoundField>
                    </Columns>
                    <FooterStyle BackColor="#CCCCCC" ForeColor="Black" />
                    <PagerStyle BackColor="#999999" ForeColor="Black" HorizontalAlign="Center" />
                    <SelectedRowStyle BackColor="#008A8C" Font-Bold="True" ForeColor="White" />
                    <HeaderStyle BackColor="#000084" Font-Bold="True" ForeColor="White" />
                    <AlternatingRowStyle BackColor="Gainsboro" />
                    <SortedAscendingCellStyle BackColor="#F1F1F1" />
                    <SortedAscendingHeaderStyle BackColor="#0000A9" />
                    <SortedDescendingCellStyle BackColor="#CAC9C9" />
                    <SortedDescendingHeaderStyle BackColor="#000065" />
                </asp:GridView>
            </asp:Panel>
            <br />
            Observações
            <br />
            Apartamentos em Manutenção podem gerar overbook.<asp:RoundedCornersExtender ID="pnlPrevisao_RoundedCornersExtender"
                runat="server" BorderColor="ActiveBorder" Color="ActiveBorder" Enabled="True"
                TargetControlID="pnlPrevisao">
            </asp:RoundedCornersExtender>
            <br />
        </ContentTemplate>
    </asp:UpdatePanel>
    <p>
    </p>
</asp:Content>
