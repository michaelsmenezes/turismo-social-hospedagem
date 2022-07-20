<%@ Page Title="Histórico de Passantes" Language="VB" MasterPageFile="~/TurismoSocial.master" AutoEventWireup="false" CodeFile="frmHistoricoPassantes.aspx.vb" Inherits="InformacoesGerenciais_frmHistoricoPassantes" %>

<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="asp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <link href="../stylesheet.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
        .style1
        {
            height: 30px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="conPlaHolTurismoSocial" Runat="Server">
    <br />
    <br />
    <br />
    <br />
    <br />
    <br />
    <br />
    <asp:UpdatePanel ID="updGeral" runat="server">
        <ContentTemplate>
            <asp:Label ID="lblTituloGeral" runat="server" CssClass="formRuler" 
                Font-Bold="True" Text="HISTÓRICO DE PASSANTES" Width="98%"></asp:Label>
            <asp:RoundedCornersExtender ID="lblTituloGeral_RoundedCornersExtender" 
                runat="server" BorderColor="ActiveBorder" Color="ActiveBorder" Enabled="True" 
                TargetControlID="lblTituloGeral" Radius="1">
            </asp:RoundedCornersExtender>
        <br />
            <asp:Panel ID="pnlConsulta" runat="server" DefaultButton="btnConsultar" 
                Width="98%">
                <br />
                <table>
                    <tr>
                        <td bordercolor="#99CCFF" class="formRuler">
                            Data Inicial:</td>
                        <td bordercolor="#99CCFF" class="formRuler">
                            Data Final:</td>
                        <td bordercolor="#99CCFF" class="formRuler">
                            Nome Passante/Passeio</td>
                        <td bordercolor="#99CCFF" class="formRuler">
                            Categoria</td>
                        <td bordercolor="#99CCFF" class="formRuler">
                            Passantes da</td>
                        <td bordercolor="#99CCFF" class="formRuler">
                            Servidores de</td>
                        <td bordercolor="#99CCFF" class="formRuler" colspan="3">
                            Idade</td>
                        <td bordercolor="#99CCFF" class="formRuler">
                            Total de Passantes</td>
                    </tr>
                    <tr>
                        <td align="right" bordercolor="#99CCFF">
                            <asp:TextBox ID="txtDataIni" runat="server" Width="80px" AutoPostBack="True"></asp:TextBox>
                            <asp:CalendarExtender ID="txtDataIni_CalendarExtender" runat="server" 
                                Enabled="True" FirstDayOfWeek="Sunday" Format="dd/MM/yyyy" 
                                TargetControlID="txtDataIni">
                            </asp:CalendarExtender>
                        </td>
                        <td align="right" bordercolor="#99CCFF">
                            <asp:TextBox ID="txtDataFim" runat="server" Width="80px" AutoPostBack="True"></asp:TextBox>
                            <asp:CalendarExtender ID="txtDataFim_CalendarExtender" runat="server" 
                                Enabled="True" FirstDayOfWeek="Sunday" Format="dd/MM/yyyy" 
                                TargetControlID="txtDataFim">
                            </asp:CalendarExtender>
                        </td>
                        <td bordercolor="#99CCFF">
                            <asp:TextBox ID="txtPassante" runat="server"></asp:TextBox>
                        </td>
                        <td bordercolor="#99CCFF">
                            <asp:DropDownList ID="drpCategoria" runat="server">
                                <asp:ListItem Value="0">Todas</asp:ListItem>
                                <asp:ListItem Value="1">Comerciário</asp:ListItem>
                                <asp:ListItem Value="2">Dependente</asp:ListItem>
                                <asp:ListItem Value="3">Conveniado</asp:ListItem>
                                <asp:ListItem Value="4">Usuário</asp:ListItem>
                            </asp:DropDownList>
                        </td>
                        <td align="center" bordercolor="#99CCFF">
                            <asp:CheckBox ID="chkCaldasNovas" runat="server" CssClass="ColocaHand" 
                                AutoPostBack="True" Text="Unidade" />
                        </td>
                        <td align="center" bordercolor="#99CCFF">
                            <asp:CheckBox ID="chkFuncionarios" runat="server" Text="Caldas Novas" />
                        </td>
                        <td align="left" bordercolor="#99CCFF">
                            <asp:DropDownList ID="drpIdade" runat="server">
                                <asp:ListItem Value="T">Todos</asp:ListItem>
                                <asp:ListItem Value="4">0 a 4 anos</asp:ListItem>
                                <asp:ListItem Value="9">5 a 9 anos</asp:ListItem>
                            </asp:DropDownList>
                        </td>
                        <td bordercolor="#99CCFF">
                            <asp:Button ID="btnConsultar" runat="server" CssClass="imgLupa" 
                                Text="  Consultar" />
                        </td>
                        <td bordercolor="#99CCFF">
                            <asp:Button ID="btnVoltarMenu" runat="server" CssClass="imgVoltar" 
                                Text="   Voltar" />
                        </td>
                        <td align="center" bordercolor="#99CCFF" class="style1">
                            <asp:Label ID="lblTotPassantes" runat="server" Font-Size="Medium" Text="0"></asp:Label>
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
                        <td>
                            &nbsp;</td>
                        <td>
                            &nbsp;</td>
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
            <asp:RoundedCornersExtender ID="pnlConsulta_RoundedCornersExtender" 
                runat="server" BorderColor="ActiveBorder" Color="ActiveBorder" Enabled="True" 
                TargetControlID="pnlConsulta" Radius="1">
            </asp:RoundedCornersExtender>
            <div align="center" class="PosicionaProgresso">
                <asp:UpdateProgress ID="updProRecepcao" runat="server" 
                    AssociatedUpdatePanelID="updGeral">
                    <ProgressTemplate>
                        Processando sua solicitação...<br />
                        &nbsp;<img alt="Processando..." src="../images/Aguarde.gif" />
                    </ProgressTemplate>
                </asp:UpdateProgress>
            </div>
            <asp:Panel ID="pnlGridHistorico" runat="server" Visible="False" Width="98%">
                <asp:GridView ID="gdvHistorico" runat="server" AutoGenerateColumns="False" 
                    CellPadding="4" DataKeyNames="IntId,IntNome,VinculoRefeicao" 
                    EmptyDataText="Não existem dados a serem exibidos." ForeColor="#333333" 
                    GridLines="None" AllowSorting="True">
                    <RowStyle BackColor="#EFF3FB" />
                    <Columns>
                        <asp:ButtonField CommandName="Select" DataTextField="IntNome" HeaderText="Nome" 
                            SortExpression="IntNome">
                        <HeaderStyle CssClass="formRuler" />
                        </asp:ButtonField>
                        <asp:TemplateField HeaderText="Refeição">
                            <ItemTemplate>
                                <asp:Image ID="imgRefeicao" runat="server" ImageUrl="~/images/Refeicao.png" 
                                    Visible="False" />
                            </ItemTemplate>
                            <HeaderStyle CssClass="formRuler" />
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateField>
                        <asp:BoundField DataField="IntDtNascimento" HeaderText="Nascimento" 
                            SortExpression="IntDtNascimento">
                        <HeaderStyle CssClass="formRuler" />
                        </asp:BoundField>
                        <asp:BoundField DataField="Idade" HeaderText="Idade" SortExpression="Idade">
                        <HeaderStyle CssClass="formRuler" />
                        <ItemStyle HorizontalAlign="Center" />
                        </asp:BoundField>
                        <asp:BoundField DataField="Categoria" SortExpression="Categoria" HeaderText="Categoria">
                        <HeaderStyle CssClass="formRuler" />
                        </asp:BoundField>
                        <asp:BoundField DataField="Observacao" HeaderText="Observação">
                        <HeaderStyle CssClass="formRuler" />
                        </asp:BoundField>
                        <asp:BoundField DataField="Placa" HeaderText="Placa">
                        <HeaderStyle CssClass="formRuler" />
                        </asp:BoundField>
                        <asp:BoundField DataField="DataIniReal" SortExpression="DataIniReal" HeaderText="Entrada">
                        <HeaderStyle CssClass="formRuler" />
                        </asp:BoundField>
                        <asp:BoundField DataField="HoraIniReal" SortExpression="HoraIniReal" HeaderText="H">
                        <HeaderStyle CssClass="formRuler" />
                        </asp:BoundField>
                        <asp:BoundField DataField="DataFimReal" SortExpression="DataFimReal" HeaderText="Saída">
                        <HeaderStyle CssClass="formRuler" />
                        </asp:BoundField>
                        <asp:BoundField DataField="HoraFimReal" SortExpression="HoraFimReal" HeaderText="H">
                        <HeaderStyle CssClass="formRuler" />
                        </asp:BoundField>
                        <asp:BoundField DataField="DataFimReal" SortExpression="DataFimReal" 
                            HeaderText="Check Out" Visible="False">
                        <HeaderStyle CssClass="formRuler" />
                        </asp:BoundField>
                        <asp:BoundField DataField="HoraFimReal" SortExpression="HoraFimReal" 
                            HeaderText="H" Visible="False">
                        <HeaderStyle CssClass="formRuler" />
                        </asp:BoundField>
                        <asp:BoundField DataField="Situacao" SortExpression="Situacao" HeaderText="Situação">
                        <HeaderStyle CssClass="formRuler" />
                        </asp:BoundField>
                        <asp:BoundField DataField="Atendente" SortExpression="Atendente" HeaderText="Atendente">
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
            </asp:Panel>
            <asp:RoundedCornersExtender ID="pnlGridHistorico_RoundedCornersExtender" 
                runat="server" BorderColor="ActiveBorder" Color="ActiveBorder" Enabled="True" 
                TargetControlID="pnlGridHistorico" Radius="1">
            </asp:RoundedCornersExtender>
            <asp:Panel ID="pnlInformacoes" runat="server" Visible="False" Width="98%">
                <div align="right" style="width: 98%"   >
                <asp:Button ID="btnVoltar" runat="server" CssClass="imgVoltar" 
                    Text="   Voltar" />
                </div>    
                <asp:Label ID="lblTituloInformacoes" runat="server" CssClass="formRuler" 
                    Font-Bold="True" Text="Informações de" Width="98%"></asp:Label>
                <asp:RoundedCornersExtender ID="lblTituloInformacoes_RoundedCornersExtender" 
                    runat="server" BorderColor="ActiveBorder" Color="ActiveBorder" Enabled="True" 
                    TargetControlID="lblTituloInformacoes" Radius="1">
                </asp:RoundedCornersExtender>
                <br />
                <br />
                <asp:Label ID="lblInformacoesHospedagem" runat="server" CssClass="formRuler" 
                    Font-Bold="True" Text="Informações de Passante" Width="98%"></asp:Label>
                <asp:RoundedCornersExtender ID="lblInformacoesHospedagem_RoundedCornersExtender" 
                    runat="server" BorderColor="ActiveBorder" Color="ActiveBorder" Enabled="True" 
                    TargetControlID="lblInformacoesHospedagem" Radius="1">
                </asp:RoundedCornersExtender>
                <br />
                <asp:GridView ID="gdvDadosPassante" runat="server" 
                    AutoGenerateColumns="False" CellPadding="4" ForeColor="#333333" 
                    GridLines="None" 
                    
                    
                    
                    DataKeyNames="IntNome,Categoria,Matricula,RG,CPF,DataIniReal,HoraIniReal,DataFimReal,HoraFimReal,ComerciarioCaldasNovas,Isento,CortesiaCaucao,CortesiaPqAquatico,CortesiaLanchonetes,CortesiaPermanenciaPQ,CortesiaRestaurante,ResponsavelCortesia,CategoriaCobranca,DocMemorando,MotivoEmissor,Situacao,Placa,UF,Cidade" 
                    EmptyDataText="Não existem informações a serem exibidas">
                    <RowStyle BackColor="#EFF3FB" />
                    <Columns>
                        <asp:ButtonField DataTextField="IntNome" CommandName="Select" 
                            HeaderText="Nome" >
                        <HeaderStyle CssClass="formRuler" />
                        </asp:ButtonField>
                        <asp:BoundField DataField="Matricula" HeaderText="Matrícula">
                        <HeaderStyle CssClass="formRuler" />
                        </asp:BoundField>
                        <asp:BoundField DataField="RG" HeaderText="RG">
                        <HeaderStyle CssClass="formRuler" />
                        <ItemStyle HorizontalAlign="Right" />
                        </asp:BoundField>
                        <asp:BoundField DataField="CPF" HeaderText="CPF">
                        <HeaderStyle CssClass="formRuler" />
                        <ItemStyle HorizontalAlign="Center" />
                        </asp:BoundField>
                        <asp:BoundField DataField="DataIniReal" HeaderText="Entrada">
                        <HeaderStyle CssClass="formRuler" />
                        </asp:BoundField>
                        <asp:BoundField DataField="HoraIniReal" HeaderText="H">
                        <HeaderStyle CssClass="formRuler" />
                        </asp:BoundField>
                        <asp:BoundField DataField="DataFimReal" HeaderText="Saída">
                        <HeaderStyle CssClass="formRuler" />
                        </asp:BoundField>
                        <asp:BoundField DataField="HoraFimReal" HeaderText="H">
                        <HeaderStyle CssClass="formRuler" />
                        </asp:BoundField>
                        <asp:BoundField DataField="Categoria" HeaderText="Categoria">
                        <HeaderStyle CssClass="formRuler" />
                        <ItemStyle HorizontalAlign="Center" />
                        </asp:BoundField>
                        <asp:BoundField DataField="VinculoRefeicao" HeaderText="Vinculo da Refeição">
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
                <br />
                <asp:Label ID="lblInformacoesConsumo" runat="server" CssClass="formRuler" 
                Font-Bold="True" Text="Informações de Consumo" Width="98%"></asp:Label>

                <asp:RoundedCornersExtender ID="lblInformacoesConsumo_RoundedCornersExtender" 
                    runat="server" BorderColor="ActiveBorder" Color="ActiveBorder" Enabled="True" 
                    TargetControlID="lblInformacoesConsumo" Radius="1">
                </asp:RoundedCornersExtender>

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

                <br />

                <asp:Label ID="lblInformacoesFinanceira" runat="server" CssClass="formRuler" 
                    Font-Bold="True" Text="Informações de Movimentação Financeira" Width="98%"></asp:Label>
                <asp:RoundedCornersExtender ID="lblInformacoesFinanceira_RoundedCornersExtender" 
                    runat="server" BorderColor="ActiveBorder" Color="ActiveBorder" Enabled="True" 
                    TargetControlID="lblInformacoesFinanceira" Radius="1">
                </asp:RoundedCornersExtender>
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

                <br />

                <br />

                <div align="right" style="width: 98%">
                    <asp:Button ID="btnVoltarBaixo" runat="server" CssClass="imgVoltar" 
                        Text="   Voltar" />
                </div>
            </asp:Panel>
            <br />
            <asp:RoundedCornersExtender ID="pnlInformacoes_RoundedCornersExtender" 
                runat="server" BorderColor="ActiveBorder" Color="ActiveBorder" Enabled="True" 
                TargetControlID="pnlInformacoes" Radius="1">
            </asp:RoundedCornersExtender>
            <asp:Panel ID="pnlInformacoesPassante" runat="server" Visible="False" 
                Width="60%">
                <table width="100%">
                    <tr>
                        <td class="toolbarRecBtnCell" colspan="2">
                            Nome</td>
                        <td class="toolbarRecBtnCell" colspan="2">
                            Categoria</td>
                        <td class="toolbarRecBtnCell">
                            Situação</td>
                        <td class="toolbarRecBtnCell">
                            Placa</td>
                        <td align="left" class="toolbarRecBtnCell">
                            Matricula</td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <asp:Label ID="lblNome" runat="server" Text="Label"></asp:Label>
                        </td>
                        <td colspan="2">
                            <asp:Label ID="lblCategoria" runat="server" Text="Label"></asp:Label>
                        </td>
                        <td>
                            <asp:Label ID="lblSituacao" runat="server" Text="Label"></asp:Label>
                        </td>
                        <td>
                            <asp:Label ID="lblPlaca" runat="server" Text="Label"></asp:Label>
                        </td>
                        <td>
                            <asp:Label ID="lblMatricula" runat="server" Text="Label"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td class="toolbarRecBtnCell">
                            Entrada</td>
                        <td class="toolbarRecBtnCell">
                            Hora</td>
                        <td class="toolbarRecBtnCell">
                            Saída</td>
                        <td class="toolbarRecBtnCell">
                            Hora</td>
                        <td rowspan="2" valign="top">
                            <asp:Label ID="lblComerciaCnv" runat="server" Text="Label"></asp:Label>
                            <br />
                            <asp:Label ID="lblIsento" runat="server" Text="Label"></asp:Label>
                        </td>
                        <td class="toolbarRecBtnCell">
                            RG</td>
                        <td class="toolbarRecBtnCell">
                            CPF</td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="lblEntrada" runat="server" Text="Label"></asp:Label>
                        </td>
                        <td>
                            <asp:Label ID="lblHoraEntrada" runat="server" Text="Label"></asp:Label>
                        </td>
                        <td>
                            <asp:Label ID="lblSaida" runat="server" Text="Label"></asp:Label>
                        </td>
                        <td>
                            <asp:Label ID="lblHoraSaida" runat="server" Text="Label"></asp:Label>
                        </td>
                        <td>
                            <asp:Label ID="lblRG" runat="server" Text="Label"></asp:Label>
                        </td>
                        <td>
                            <asp:Label ID="lblCPF" runat="server" Text="Label"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            &nbsp;</td>
                        <td colspan="2">
                            &nbsp;</td>
                        <td>
                            &nbsp;</td>
                        <td colspan="2">
                            &nbsp;</td>
                    </tr>
                    <tr>
                        <td class="toolbarRecBtnCell" colspan="2">
                            Cortesias/Concessões</td>
                        <td colspan="2" class="toolbarRecBtnCell">
                            &nbsp;</td>
                        <td class="toolbarRecBtnCell">
                            Responsável pela Cortesia</td>
                        <td class="toolbarRecBtnCell" colspan="2">
                            Categoria de Cobrança</td>
                    </tr>
                    <tr>
                        <td valign="top">
                            <asp:Label ID="lblCortesiaCaucao" runat="server" Text="Label"></asp:Label>
                            <br />
                            <asp:Label ID="lblCortesiaPqAquatico" runat="server" Text="Label"></asp:Label>
                            <br />
                            <asp:Label ID="lblCortesiaLanchonete" runat="server" Text="Label"></asp:Label>
                            <br />
                            <asp:Label ID="lblCortesiaRestaurante" runat="server" Text="Label"></asp:Label>
                            <br />
                            <asp:Label ID="lblCortesiaPermPqAuquatico" runat="server" Text="Label"></asp:Label>
                        </td>
                        <td>
                            &nbsp;</td>
                        <td colspan="2" valign="top">
                            &nbsp;</td>
                        <td valign="top">
                            <asp:Label ID="lblResponsavelCortesia" runat="server" Text="Label"></asp:Label>
                        </td>
                        <td colspan="2" valign="top">
                            <asp:Label ID="lblCategoriaCobranca" runat="server" Text="Label"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td class="toolbarRecBtnCell" colspan="2">
                            Doc./Memorando</td>
                        <td class="toolbarRecBtnCell" colspan="2">
                            Motivo/Emissor</td>
                        <td>
                            &nbsp;</td>
                        <td class="toolbarRecBtnCell">
                            Cidade:</td>
                        <td class="toolbarRecBtnCell">
                            UF:</td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <asp:TextBox ID="txtDocMemorando" runat="server" ReadOnly="True" 
                                TextMode="MultiLine"></asp:TextBox>
                        </td>
                        <td colspan="3" valign="top">
                            <asp:Label ID="lblMotivoEmissor" runat="server" Text="Label"></asp:Label>
                        </td>
                        <td align="left" valign="top">
                            <asp:Label ID="lblCidade" runat="server" Text="Label"></asp:Label>
                        </td>
                        <td align="left" valign="top">
                            <asp:Label ID="lblUF" runat="server" Text="Label"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            &nbsp;</td>
                        <td colspan="2">
                            &nbsp;</td>
                        <td>
                            &nbsp;</td>
                        <td colspan="2" align="right">
                            <asp:Button ID="btnVoltarInformacoes" runat="server" CssClass="imgVoltar" 
                                Text="   Voltar" />
                        </td>
                    </tr>
                </table>
            </asp:Panel>
            <asp:RoundedCornersExtender ID="pnlInformacoesPassante_RoundedCornersExtender" 
                runat="server" BorderColor="ActiveBorder" Color="ActiveBorder" Enabled="True" 
                Radius="1" TargetControlID="pnlInformacoesPassante">
            </asp:RoundedCornersExtender>
        </ContentTemplate>
    </asp:UpdatePanel>
    <p>
    </p>
    <p>
        <br />
    </p>
</asp:Content>

