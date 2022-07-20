<%@ Page Title="" Language="VB" MasterPageFile="~/TurismoSocial.master" AutoEventWireup="false" CodeFile="frmHistoricoHospedes.aspx.vb" Inherits="InformacoesGerenciais_frmHistoricoHospedes" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <link href="../stylesheet.css" rel="stylesheet" type="text/css" />
<style type="text/css">
    .auto-style1 {
        width: 88px;
    }
</style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="conPlaHolTurismoSocial" runat="Server">
    <br />
    <br />
    <br />
    <asp:UpdatePanel ID="updGeral" runat="server">
        <ContentTemplate>
            <br />
            <br />
            <br />
            <br />
            <asp:Label ID="lblTituloGeral" runat="server" CssClass="formRuler"
                Font-Bold="True" Text="HISTÓRICO DE HÓSPEDES" Width="98%"></asp:Label>
            <br />
            <asp:Panel ID="pnlConsulta" CssClass="ArrendodarBorda" runat="server" DefaultButton="btnConsultar"
                Width="98%">
                <br />
                <table>
                    <tr>
                        <td>
                            &nbsp;</td>
                        <td colspan="2">&nbsp;</td>
                        <td>&nbsp;</td>
                        <td>&nbsp;</td>
                        <td>&nbsp;</td>
                        <td>&nbsp;</td>
                        <td>&nbsp;</td>
                        <td>&nbsp;</td>
                    </tr>
                    <tr>
                        <td class="auto-style1">
                            Data Inicial</td>
                        <td colspan="2">Data Final</td>
                        <td>Nº Apto/Placa</td>
                        <td>Hóspede/Responsável</td>
                        <td>Servidores</td>
                        <td>&nbsp;</td>
                        <td>&nbsp;</td>
                        <td>Total de Hóspedes</td>
                    </tr>
                    <tr>
                        <td align="right">
                            <asp:TextBox ID="txtDataIni" runat="server" Width="80px" AutoCompleteType="Disabled"></asp:TextBox>
                            <asp:CalendarExtender ID="txtDataIni_CalendarExtender" runat="server"
                                Enabled="True" FirstDayOfWeek="Sunday" Format="dd/MM/yyyy"
                                TargetControlID="txtDataIni">
                            </asp:CalendarExtender>
                        </td>
                        <td align="right" colspan="2">
                            &nbsp;<asp:TextBox ID="txtDataFim" runat="server" Width="80px" AutoCompleteType="Disabled"></asp:TextBox>
                            <asp:CalendarExtender ID="txtDataFim_CalendarExtender" runat="server"
                                Enabled="True" FirstDayOfWeek="Sunday" Format="dd/MM/yyyy"
                                TargetControlID="txtDataFim">
                            </asp:CalendarExtender>
                        </td>
                        <td>
                            <asp:TextBox ID="txtAptoPlaca" runat="server" Width="90px"></asp:TextBox>
                        </td>
                        <td>
                            <asp:TextBox ID="txtHospede" runat="server"></asp:TextBox>
                        </td>
                        <td align="center">
                            <asp:CheckBox ID="chkServidores" runat="server" CssClass="ColocaHand"
                                AutoPostBack="True" />
                        </td>
                        <td>
                            <asp:Button ID="btnConsultar" runat="server" CssClass="imgLupa"
                                Text="  Consultar" />
                        </td>
                        <td>
                            <asp:Button ID="btnVoltarMenu" runat="server" CssClass="imgVoltar"
                                Text="   Voltar" />
                        </td>
                        <td align="center">
                            <asp:Label ID="lblTotHospedes" runat="server" Font-Size="Medium" Text="0"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td>&nbsp;</td>
                        <td>&nbsp;</td>
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
            <div align="center" class="PosicionaProgresso">
                <asp:UpdateProgress ID="updProRecepcao" runat="server"
                    AssociatedUpdatePanelID="updGeral">
                    <ProgressTemplate>
                        Processando sua solicitação...<br />
                        &nbsp;<img alt="Processando..." src="../images/Aguarde.gif" />
                    </ProgressTemplate>
                </asp:UpdateProgress>
            </div>
            <asp:Panel ID="pnlGridHistorico" CssClass="ArrendodarBorda" runat="server" Visible="False" Width="98%">
                <asp:GridView ID="gdvHistorico" runat="server" AutoGenerateColumns="False"
                    CellPadding="4"
                    DataKeyNames="IntId,ResId,IntNome,ResNome,ApaId,ApaDesc,DataIni,HoraIni,DataIniReal,HoraIniReal,DataFim,HoraFim,DataFimReal,HoraFimReal"
                    EmptyDataText="Não existem dados a serem exibidos." ForeColor="#333333"
                    GridLines="None" AllowSorting="True" Width="100%">
                    <RowStyle BackColor="#EFF3FB" />
                    <Columns>
                        <asp:ButtonField CommandName="Select" DataTextField="IntNome"
                            HeaderText="Hóspede" SortExpression="IntNome">
                            <HeaderStyle CssClass="formRuler" HorizontalAlign="Left" />
                        </asp:ButtonField>
                        <asp:BoundField DataField="ApaDesc" HeaderText="Apto" SortExpression="ApaDesc">
                            <HeaderStyle CssClass="formRuler" HorizontalAlign="Left" />
                        </asp:BoundField>
                        <asp:BoundField DataField="DataIni" HeaderText="Data Inicial"
                            SortExpression="I.IntDataIni">
                            <HeaderStyle CssClass="formRuler" HorizontalAlign="Left" />
                        </asp:BoundField>
                        <asp:BoundField DataField="HoraIni" HeaderText="H" SortExpression="HoraIni">
                            <HeaderStyle CssClass="formRuler" HorizontalAlign="Left" />
                        </asp:BoundField>
                        <asp:BoundField DataField="DataIniReal" HeaderText="Check-In"
                            SortExpression="I.IntDataIniReal">
                            <HeaderStyle CssClass="formRuler" HorizontalAlign="Left" />
                        </asp:BoundField>
                        <asp:BoundField DataField="HoraIniReal" HeaderText="H"
                            SortExpression="HoraIniReal">
                            <HeaderStyle CssClass="formRuler" HorizontalAlign="Left" />
                        </asp:BoundField>
                        <asp:BoundField DataField="DataFim" HeaderText="Data Final"
                            SortExpression="I.IntDataFim">
                            <HeaderStyle CssClass="formRuler" HorizontalAlign="Left" />
                        </asp:BoundField>
                        <asp:BoundField DataField="HoraFim" HeaderText="H" SortExpression="HoraFim">
                            <HeaderStyle CssClass="formRuler" HorizontalAlign="Left" />
                        </asp:BoundField>
                        <asp:BoundField DataField="DataFimReal" HeaderText="Check Out"
                            SortExpression="I.IntDataFimReal">
                            <HeaderStyle CssClass="formRuler" HorizontalAlign="Left" />
                        </asp:BoundField>
                        <asp:BoundField DataField="HoraFimReal" HeaderText="H"
                            SortExpression="HoraFimReal">
                            <HeaderStyle CssClass="formRuler" HorizontalAlign="Left" />
                        </asp:BoundField>
                        <asp:TemplateField HeaderText="Reserva" SortExpression="ResNome">
                            <EditItemTemplate>
                                <asp:TextBox ID="TextBox1" runat="server" Text='<%# Bind("ResNome") %>'></asp:TextBox>
                            </EditItemTemplate>
                            <ItemTemplate>
                                <asp:ImageButton ID="imgFiltraReserva" runat="server" ImageUrl="~/images/Filtro.png" OnClick="imgFiltraReserva_Click" Width="16px" />
                                &nbsp;<asp:Label ID="Label1" runat="server" Text='<%# Bind("ResNome") %>'></asp:Label>
                            </ItemTemplate>
                            <HeaderStyle CssClass="formRuler" HorizontalAlign="Left" />
                        </asp:TemplateField>
                    </Columns>
                    <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                    <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
                    <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                    <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                    <EditRowStyle BackColor="#2461BF" />
                    <AlternatingRowStyle BackColor="White" />
                </asp:GridView>
            </asp:Panel>
            <asp:Panel ID="pnlInformacoes" CssClass="ArrendodarBorda" runat="server" Visible="False" Width="98%">
                <div align="right" style="width: 98%">
                    <asp:Button ID="btnVoltar" runat="server" CssClass="imgVoltar"
                        Text="   Voltar" />
                </div>

                <div class="ArrendodarBorda ">
                <asp:Label ID="lblTituloInformacoes" runat="server" CssClass="formRuler"
                    Font-Bold="True" Text="Informações de" Width="98%"></asp:Label>
                <br />
                <asp:GridView ID="gdvSituacaoReserva" runat="server"
                    AutoGenerateColumns="False" CellPadding="4" ForeColor="#333333"
                    GridLines="None">
                    <RowStyle BackColor="#EFF3FB" />
                    <Columns>
                        <asp:BoundField DataField="DataIniReal" HeaderText="Check-in">
                            <HeaderStyle CssClass="formRuler" />
                        </asp:BoundField>
                        <asp:BoundField DataField="HoraIniReal" HeaderText="H">
                            <HeaderStyle CssClass="formRuler" />
                        </asp:BoundField>
                        <asp:BoundField DataField="DataFimReal" HeaderText="Check-Out">
                            <HeaderStyle CssClass="formRuler" />
                        </asp:BoundField>
                        <asp:BoundField DataField="HoraFimReal" HeaderText="H">
                            <HeaderStyle CssClass="formRuler" />
                        </asp:BoundField>
                        <asp:BoundField DataField="Status" HeaderText="Status">
                            <HeaderStyle CssClass="formRuler" />
                        </asp:BoundField>
                        <asp:BoundField DataField="IntSaldo" HeaderText="Saldo Cartão">
                            <HeaderStyle CssClass="formRuler" />
                            <ItemStyle HorizontalAlign="Right" />
                        </asp:BoundField>
                        <asp:BoundField DataField="IntCortesiaCaucao" HeaderText="Cortesia Caução">
                            <HeaderStyle CssClass="formRuler" />
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:BoundField>
                        <asp:BoundField DataField="IntCortesiaConsumo" HeaderText="Cortesia Consumo">
                            <HeaderStyle CssClass="formRuler" />
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:BoundField>
                        <asp:BoundField DataField="IntCortesiaRestaurante"
                            HeaderText="Cortesia Restaurante">
                            <HeaderStyle CssClass="formRuler" />
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:BoundField>
                        <asp:BoundField DataField="FormaPagto" HeaderText="Forma Pagamento">
                            <HeaderStyle CssClass="formRuler" />
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
                </div>

                <br />
                <div class="ArrendodarBorda">
                <asp:Label ID="lblInformacoesHospedagem" runat="server" CssClass="formRuler"
                    Font-Bold="True" Text="Informações de Hospedagem" Width="98%"></asp:Label>
                <br />
                <asp:GridView ID="gdvInfHospedagem" runat="server" AutoGenerateColumns="False"
                    CellPadding="4" EmptyDataText="Não existem informações a serem exibidas."
                    ForeColor="#333333" GridLines="None">
                    <RowStyle BackColor="#EFF3FB" />
                    <Columns>
                        <asp:BoundField DataField="ApaDesc" HeaderText="Apto">
                            <HeaderStyle CssClass="formRuler" />
                        </asp:BoundField>
                        <asp:BoundField DataField="Acomodacao" HeaderText="Acomodação">
                            <HeaderStyle CssClass="formRuler" />
                        </asp:BoundField>
                        <asp:BoundField DataField="AcomodacaoCobranca" HeaderText="Acomodação Cobrança">
                            <HeaderStyle CssClass="formRuler" />
                        </asp:BoundField>
                        <asp:BoundField DataField="HosDataIniSol" HeaderText="Data Inicial">
                            <HeaderStyle CssClass="formRuler" />
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:BoundField>
                        <asp:BoundField DataField="HosHoraIniSol" HeaderText="H">
                            <HeaderStyle CssClass="formRuler" />
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:BoundField>
                        <asp:BoundField DataField="DataIniReal" HeaderText="Check-In">
                            <HeaderStyle CssClass="formRuler" />
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:BoundField>
                        <asp:BoundField DataField="HoraIniReal" HeaderText="H">
                            <HeaderStyle CssClass="formRuler" />
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:BoundField>
                        <asp:BoundField DataField="HosDataFimSol" HeaderText="Data Final">
                            <HeaderStyle CssClass="formRuler" />
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:BoundField>
                        <asp:BoundField DataField="HosHoraFimSol" HeaderText="H">
                            <HeaderStyle CssClass="formRuler" />
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:BoundField>
                        <asp:BoundField DataField="DataFimReal" HeaderText="Check-Out">
                            <HeaderStyle CssClass="formRuler" />
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:BoundField>
                        <asp:BoundField DataField="HoraFimReal" HeaderText="H">
                            <HeaderStyle CssClass="formRuler" />
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:BoundField>
                        <asp:BoundField DataField="HosValorDevido" HeaderText="Valor Devido">
                            <HeaderStyle CssClass="formRuler" />
                            <ItemStyle HorizontalAlign="Right" />
                        </asp:BoundField>
                        <asp:BoundField DataField="HosValorPago" HeaderText="Valor Pago">
                            <HeaderStyle CssClass="formRuler" />
                            <ItemStyle HorizontalAlign="Right" />
                        </asp:BoundField>
                        <asp:BoundField DataField="HosStatus" HeaderText="Situação">
                            <HeaderStyle CssClass="formRuler" />
                        </asp:BoundField>
                    </Columns>
                    <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                    <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
                    <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                    <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                    <EditRowStyle BackColor="#2461BF" />
                    <AlternatingRowStyle BackColor="White" />
                </asp:GridView>
                </div>
                <br />


                <div class="ArrendodarBorda">
                <asp:Label ID="lblInformacoesConsumo" runat="server" CssClass="formRuler"
                    Font-Bold="True" Text="Informações de Consumo" Width="98%"></asp:Label>

                <br />
                <asp:GridView ID="gdvConsumo" runat="server" AutoGenerateColumns="False"
                    CellPadding="4" ForeColor="#333333" GridLines="None"
                    DataKeyNames="ValorTotal"
                    EmptyDataText="Não existem informações a serem exibidas.">
                    <RowStyle BackColor="#EFF3FB" />
                    <Columns>
                        <asp:BoundField DataField="Dia" HeaderText="Dia">
                            <HeaderStyle CssClass="formRuler" />
                        </asp:BoundField>
                        <asp:BoundField DataField="Hora" HeaderText="Hora">
                            <HeaderStyle CssClass="formRuler" />
                        </asp:BoundField>
                        <asp:BoundField DataField="Descricao" HeaderText="Produto">
                            <HeaderStyle CssClass="formRuler" />
                        </asp:BoundField>
                        <asp:BoundField DataField="PrecoVenda" HeaderText="Valor">
                            <HeaderStyle CssClass="formRuler" />
                            <ItemStyle HorizontalAlign="Right" />
                        </asp:BoundField>
                        <asp:BoundField DataField="Quantidade" HeaderText="Qtde">
                            <HeaderStyle CssClass="formRuler" />
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:BoundField>
                        <asp:BoundField DataField="UnidadeMedida" HeaderText="Unid">
                            <HeaderStyle CssClass="formRuler" />
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:BoundField>
                        <asp:BoundField DataField="ValorTotal" HeaderText="Total">
                            <HeaderStyle CssClass="formRuler" />
                            <ItemStyle HorizontalAlign="Right" />
                        </asp:BoundField>
                        <asp:BoundField DataField="FormaConsumo" HeaderText="Consumo">
                            <HeaderStyle CssClass="formRuler" />
                        </asp:BoundField>
                        <asp:BoundField DataField="LocalConsumo" HeaderText="Local">
                            <HeaderStyle CssClass="formRuler" />
                        </asp:BoundField>
                        <asp:BoundField DataField="Operador" HeaderText="Atendente">
                            <HeaderStyle CssClass="formRuler" />
                        </asp:BoundField>
                    </Columns>
                    <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                    <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
                    <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                    <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                    <EditRowStyle BackColor="#2461BF" />
                    <AlternatingRowStyle BackColor="White" />
                </asp:GridView>
                    </div>
                <br />

                <div class="ArrendodarBorda">
                <asp:Label ID="lblInformacoesFinanceira" runat="server" CssClass="formRuler"
                    Font-Bold="True" Text="Informações de Movimentação Financeira" Width="98%"></asp:Label>
                <asp:GridView ID="gdvFinanceiro" runat="server" AutoGenerateColumns="False"
                    CellPadding="4" EmptyDataText="Não existem informações a serem exibidas."
                    ForeColor="#333333" GridLines="None">
                    <RowStyle BackColor="#EFF3FB" />
                    <Columns>
                        <asp:BoundField DataField="Caixa" HeaderText="Caixa">
                            <HeaderStyle CssClass="formRuler" />
                        </asp:BoundField>
                        <asp:BoundField DataField="Operacao" HeaderText="Operação">
                            <HeaderStyle CssClass="formRuler" />
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:BoundField>
                        <asp:BoundField DataField="Dia" HeaderText="Dia">
                            <HeaderStyle CssClass="formRuler" />
                        </asp:BoundField>
                        <asp:BoundField DataField="Hora" HeaderText="Hora">
                            <HeaderStyle CssClass="formRuler" />
                        </asp:BoundField>
                        <asp:BoundField DataField="Descricao" HeaderText="Descrição">
                            <HeaderStyle CssClass="formRuler" />
                        </asp:BoundField>
                        <asp:BoundField DataField="ValorTotal" HeaderText="Valor">
                            <HeaderStyle CssClass="formRuler" />
                            <ItemStyle HorizontalAlign="Right" />
                        </asp:BoundField>
                        <asp:BoundField DataField="FormaPagto" HeaderText="Forma">
                            <HeaderStyle CssClass="formRuler" />
                        </asp:BoundField>
                        <asp:BoundField DataField="Observacao" HeaderText="Observação">
                            <HeaderStyle CssClass="formRuler" />
                        </asp:BoundField>
                        <asp:BoundField DataField="Operador" HeaderText="Atendente">
                            <HeaderStyle CssClass="formRuler" />
                        </asp:BoundField>
                    </Columns>
                    <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                    <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
                    <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                    <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                    <EditRowStyle BackColor="#2461BF" />
                    <AlternatingRowStyle BackColor="White" />
                </asp:GridView>
                    </div>

                <div class="ArrendodarBorda">
                    <br />
                    <asp:Label ID="lblLog" runat="server" CssClass="formRuler" Font-Bold="True"
                        Text="Log de Eventos" Width="98%"></asp:Label>
                    <asp:GridView ID="gdvLog" runat="server" AutoGenerateColumns="False"
                        CellPadding="4" EmptyDataText="Não existem informações a serem exibidas."
                        ForeColor="#333333" GridLines="None">
                        <RowStyle BackColor="#EFF3FB" />
                        <Columns>
                            <asp:BoundField DataField="Descricao" HeaderText="Evento">
                                <HeaderStyle CssClass="formRuler" />
                            </asp:BoundField>
                            <asp:BoundField DataField="Operador" HeaderText="Usuário">
                                <HeaderStyle CssClass="formRuler" />
                            </asp:BoundField>
                            <asp:BoundField DataField="Dia" HeaderText="Data">
                                <HeaderStyle CssClass="formRuler" />
                            </asp:BoundField>
                            <asp:BoundField DataField="Hora" HeaderText="Horário">
                                <HeaderStyle CssClass="formRuler" />
                            </asp:BoundField>
                        </Columns>
                        <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                        <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
                        <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                        <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                        <EditRowStyle BackColor="#2461BF" />
                        <AlternatingRowStyle BackColor="White" />
                    </asp:GridView>
                </div>
                <br />


                <div runat="server" id="divHistoricoEnxoval" class="ArrendodarBorda">
                    <asp:Label ID="lblInformacaoReserva" runat="server" CssClass="formRuler"
                        Font-Bold="True" Text="Histórico de enxoval completado" Width="100%"></asp:Label>
                    <asp:GridView ID="gdvHistoricoEnxoval" runat="server" AutoGenerateColumns="False" CellPadding="4" DataKeyNames="resId,enxCamaExtra,enxCamaExtraAtendida,enxBerco,enxBercoAtendido,enxBanheira,enxBanheiraAtendida,enxUsuarioSolicitante,enxUsuarioDataSolicitante,enxUsuarioAtendimento,enxUsuarioDataAtendimento,enxMotivoNaoAtendimento,excUsuarioDataLog,excUsuarioLog,solId" ForeColor="#333333" GridLines="None">
                        <AlternatingRowStyle BackColor="White" />
                        <Columns>
                            <asp:BoundField DataField="enxCamaExtra" HeaderText="Cama extra">
                                <HeaderStyle CssClass="formRuler" />
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:BoundField>
                            <asp:BoundField DataField="enxCamaExtraAtendida" HeaderText="Atendimento">
                                <HeaderStyle CssClass="formRuler" />
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:BoundField>
                            <asp:BoundField DataField="enxBerco" HeaderText="Berço">
                                <HeaderStyle CssClass="formRuler" />
                            </asp:BoundField>
                            <asp:BoundField DataField="enxBercoAtendido" HeaderText="Atendimento">
                                <HeaderStyle CssClass="formRuler" />
                            </asp:BoundField>
                            <asp:BoundField DataField="enxBanheira" HeaderText="Banheira">
                                <HeaderStyle CssClass="formRuler" />
                            </asp:BoundField>
                            <asp:BoundField DataField="enxBanheiraAtendida" HeaderText="Atendimento">
                                <HeaderStyle CssClass="formRuler" />
                            </asp:BoundField>
                            <asp:BoundField DataField="enxUsuarioSolicitante" HeaderText="Usuário solicitante">
                                <HeaderStyle CssClass="formRuler" />
                            </asp:BoundField>
                            <asp:BoundField DataField="enxUsuarioDataSolicitante" HeaderText="Data da solicitação">
                                <HeaderStyle CssClass="formRuler" />
                            </asp:BoundField>
                            <asp:BoundField DataField="enxUsuarioAtendimento" HeaderText="Atendido">
                                <HeaderStyle CssClass="formRuler" />
                            </asp:BoundField>
                            <asp:BoundField DataField="enxUsuarioDataAtendimento" HeaderText="Data do atendimento">
                                <HeaderStyle CssClass="formRuler" />
                            </asp:BoundField>
                            <asp:BoundField DataField="enxMotivoNaoAtendimento" HeaderText="Motivo do não atendimento">
                                <HeaderStyle CssClass="formRuler" />
                            </asp:BoundField>
                            <asp:BoundField DataField="enxAcao" HeaderText="Ação">
                                <HeaderStyle CssClass="formRuler" />
                            </asp:BoundField>
                        </Columns>
                        <EditRowStyle BackColor="#2461BF" />
                        <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                        <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                        <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
                        <RowStyle BackColor="#EFF3FB" />
                        <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                        <SortedAscendingCellStyle BackColor="#F5F7FB" />
                        <SortedAscendingHeaderStyle BackColor="#6D95E1" />
                        <SortedDescendingCellStyle BackColor="#E9EBEF" />
                        <SortedDescendingHeaderStyle BackColor="#4870BE" />
                    </asp:GridView>
                </div>
                <br />

                <div align="right" style="width: 98%">
                    <asp:Button ID="btnVoltarBaixo" runat="server" CssClass="imgVoltar"
                        Text="   Voltar" />
                </div>
            </asp:Panel>
            <br />
            <br />
            <br />
        </ContentTemplate>
    </asp:UpdatePanel>
    <p>
    </p>
    <p>
        <br />
    </p>
</asp:Content>

