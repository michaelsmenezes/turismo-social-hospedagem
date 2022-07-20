<%@ Page Title="" Language="VB" MasterPageFile="~/TurismoSocial.master" AutoEventWireup="True"
    CodeFile="Reserva.aspx.vb" ResponseEncoding="iso-8859-1" Inherits="Reserva" EnableEventValidation="false" UICulture="Auto" Culture="pt-BR" %>

<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=11.0.0.0, Culture=neutral, PublicKeyToken=89845dcd8080cc91" Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Assembly="System.Web.DynamicData, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.DynamicData" TagPrefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script src="JScript.js" type="text/javascript"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="conPlaHolTurismoSocial" runat="Server">
    <br />
    <div class="PosicionaProgresso" align="center">
        <asp:UpdateProgress ID="updProReserva" runat="server" AssociatedUpdatePanelID="updPnlReserva"
            EnableViewState="True">
            <ProgressTemplate>
                Processando sua solicitação...<br />
                &nbsp;<img alt="Processando..." src="images/Aguarde.gif" />
            </ProgressTemplate>
        </asp:UpdateProgress>

    </div>
    Integrante<asp:UpdatePanel ID="updPnlReserva" runat="server">
        <ContentTemplate>
            <asp:Panel ID="pnlMsg" runat="server" Height="0px">
                <div id="lblDataInicialAjuda" style="overflow: hidden;">
                    Informe a data inicial para consultar as solicitações.
                </div>
                <div id="lblDataFinalReservaAjuda" style="overflow: hidden;">
                    Informe a data final para consultar as solicitações.
                </div>
                <div id="lblResponsavelAjuda" style="overflow: hidden;">
                    Filtre informando parte do nome desejado para consultar as solicitações.
                </div>
            </asp:Panel>
            <table class="formLabel" id="tbReserva" runat="server" align="center" width="100%">
                <tr>
                    <td colspan="2">
                        <br />
                        <br />
                        <br />
                        <br />
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <asp:Panel ID="pnlCabecalho" runat="server" Width="100%" CssClass="tableRowOdd" DefaultButton="btnReserva">
                            <table width="97%">
                                <tr>
                                    <td>
                                        <asp:Label ID="lblTipo" runat="server" Text="Tipo"></asp:Label>
                                        <asp:DropDownList ID="cmbTipo" runat="server" CssClass="ArrendodarBorda">
                                            <asp:ListItem Value="T">Todos</asp:ListItem>
                                            <asp:ListItem Value="I">Individual</asp:ListItem>
                                            <asp:ListItem Value="B">Balcão</asp:ListItem>
                                            <asp:ListItem Value="H">HospedeJá</asp:ListItem>
                                            <asp:ListItem Value="E">Emissivo/Pacote</asp:ListItem>
                                            <asp:ListItem Value="N">Internet</asp:ListItem>
                                            <asp:ListItem Value="P">Passeio</asp:ListItem>
                                            <asp:ListItem Value="F">Federação</asp:ListItem>
                                        </asp:DropDownList>
                                        <asp:Label ID="lblDataInicialReserva" runat="server" Text="Data Inicial"></asp:Label>
                                        <asp:TextBox ID="txtDataInicialReserva" runat="server" Columns="6" Width="70px" MaxLength="10" CssClass="ArrendodarBorda"></asp:TextBox>
                                        <asp:CalendarExtender ID="txtDataInicialReserva_CalendarExtender" runat="server"
                                            Enabled="True" Format="dd/MM/yyyy" TargetControlID="txtDataInicialReserva">
                                        </asp:CalendarExtender>
                                        <asp:Label ID="lblDataFinalReserva" runat="server" Text="Data Final"></asp:Label>
                                        <asp:TextBox ID="txtDataFinalReserva" runat="server" Columns="6" Width="70px" MaxLength="10" CssClass="ArrendodarBorda"></asp:TextBox>
                                        <asp:CalendarExtender ID="txtDataFinalReserva_CalendarExtender" runat="server" Enabled="True"
                                            Format="dd/MM/yyyy" TargetControlID="txtDataFinalReserva">
                                        </asp:CalendarExtender>
                                        <asp:Label ID="lblResponsavel" runat="server" Text="Nome"></asp:Label>
                                        <asp:TextBox ID="txtResponsavel" runat="server" Columns="15" CssClass="ArrendodarBorda"></asp:TextBox>
                                        CPF/CNPJ/Reserva<asp:TextBox ID="txtConCPF" runat="server" CssClass="ArrendodarBorda" ToolTip="Informe o CPF ou o CNPJ ou o nº da reserva desejada"></asp:TextBox>
                                        <asp:ImageButton ID="imgConsultaNascimento" runat="server" ImageUrl="~/images/lupapeq.png" ToolTip="Consulta data de nascimento pelo CPF (Cadastrada no TurWeb)" />
                                        &nbsp;<asp:Label ID="lblSituacao" runat="server" Text="Situação"></asp:Label>
                                        <asp:DropDownList ID="cmbSituacao" runat="server" CssClass="ArrendodarBorda">
                                            <asp:ListItem Value="T">Todas só ativas</asp:ListItem>
                                            <asp:ListItem Value="0">Todas</asp:ListItem>
                                            <asp:ListItem Value="S">Solicitação</asp:ListItem>
                                            <asp:ListItem Value="I">Falta Integrante</asp:ListItem>
                                            <asp:ListItem Value="P">Falta Pagamento</asp:ListItem>
                                            <asp:ListItem Value="R">Confirmada</asp:ListItem>
                                            <asp:ListItem Value="C">Cancelada</asp:ListItem>
                                            <asp:ListItem Value="E">Em Estada</asp:ListItem>
                                            <asp:ListItem Value="F">Finalizada</asp:ListItem>
                                            <%--<asp:ListItem Value="L">Lista de Espera</asp:ListItem>
                                <asp:ListItem Value="A">Pesquisar Antecipações</asp:ListItem>
                                <asp:ListItem Value="T">Total por Período</asp:ListItem>
                                <asp:ListItem Value="D">Cobranças Pendentes 70%</asp:ListItem>
                                <asp:ListItem Value="1">Grupos Não Confirmados</asp:ListItem>
                                <asp:ListItem Value="2">Grupos Pgto Sinal Atrasado</asp:ListItem>
                                <asp:ListItem Value="3">Grupos Room List Atrasado</asp:ListItem>--%>
                                        </asp:DropDownList>
                                        <asp:Label ID="lblOrgao" runat="server" Text="Órgao"></asp:Label>
                                        <asp:DropDownList ID="cmbOrgao" runat="server" CssClass="ArrendodarBorda">
                                        </asp:DropDownList>
                                        <asp:Label ID="lblCategoriaReserva" runat="server" Text="Categoria"></asp:Label>
                                        <asp:DropDownList ID="cmbCategoriaIntegrante" runat="server" CssClass="ArrendodarBorda">
                                        </asp:DropDownList>
                                        <asp:Label ID="lblTipoAcomodacao" runat="server" Text="Acomodação"></asp:Label>
                                        <asp:DropDownList ID="cmbAcomodacao" runat="server" CssClass="ArrendodarBorda">
                                        </asp:DropDownList>
                                        <asp:Label ID="lblBoleto" runat="server" Text="NºBoleto"></asp:Label>
                                        <asp:TextBox ID="txtBoleto" runat="server" Columns="11" MaxLength="11" CssClass="ArrendodarBorda"></asp:TextBox>
                                        <asp:FilteredTextBoxExtender ID="txtBoleto_FilteredTextBoxExtender" runat="server"
                                            Enabled="True" FilterType="Numbers" TargetControlID="txtBoleto">
                                        </asp:FilteredTextBoxExtender>
                                        <asp:Label ID="lblTipoPgto" runat="server" Text="Tipo"></asp:Label>
                                        <asp:DropDownList ID="cmbTipoPgto" runat="server" CssClass="ArrendodarBorda">
                                            <asp:ListItem Value="0">Todos</asp:ListItem>
                                            <asp:ListItem Value="B">Boleto</asp:ListItem>
                                            <asp:ListItem Value="M">Manual</asp:ListItem>
                                            <asp:ListItem Value="C">Caixa</asp:ListItem>
                                            <asp:ListItem Value="T">Cartão de Crédito</asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                    <td align="center" nowrap="nowrap" valign="middle">
                                        <asp:ImageButton ID="imgBtnReservaNova" runat="server" CssClass="ArrendodarBorda" ImageAlign="AbsMiddle" ImageUrl="~/images/Reserva_add_azul.png"
                                            ToolTip="Nova Reserva" />
                                        <asp:Label ID="lblNovaReserva" runat="server" Text="Nova Reserva"></asp:Label>
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
                        <asp:RoundedCornersExtender ID="pnlCabecalho_RoundedCornersExtender" runat="server"
                            Enabled="True" TargetControlID="pnlCabecalho">
                        </asp:RoundedCornersExtender>
                    </td>
                </tr>
                <tr>
                    <td id="TdReserva" width="60%" runat="server">
                        <asp:Panel ID="pnlReserva" runat="server" CssClass="ArrendodarBorda" Height="250px" DefaultButton="btnReserva"
                            ScrollBars="Auto">
                            <table width="97%">
                                <tr>
                                    <td align="center" class="tableRowOdd">
                                        <asp:Label ID="lblReserva" runat="server" Text="Reservas" Font-Bold="True" Font-Italic="True"
                                            Font-Overline="False" Font-Size="Medium"></asp:Label>
                                        &nbsp;
                                        <asp:Button ID="btnReserva" runat="server" CssClass="imgLupa ArrendodarBorda" Text="  Consultar"
                                            CommandArgument="0" />
                                    </td>
                                    <td align="center" class="tableRowOdd">&nbsp;<asp:ImageButton ID="imgBtnReservaMaximizar" CssClass="ArrendodarBorda" runat="server" ImageAlign="AbsMiddle"
                                        ImageUrl="~/images/Maximize.png" ToolTip="Maximizar" Width="24px" />
                                        &nbsp;<asp:ImageButton ID="imgBtnReservaMinimizar" CssClass="ArrendodarBorda" runat="server" ImageAlign="AbsMiddle"
                                            ImageUrl="~/images/Minimize.png" ToolTip="Minimizar" Visible="False" />
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2" align="center">
                                        <asp:GridView ID="gdvReserva1" runat="server" AutoGenerateColumns="False" CellPadding="4"
                                            DataKeyNames="resId,resStatus,resDataFim,resValorPagoAntecipado,resNome,resCaracteristica,resDataIni,resValorPago,resUsuario,resDtLimiteRetorno,resDtInsercao,ResUsuario,ResUsuarioData,qtdAcomodacao,ResObs"
                                            ShowFooter="True" Width="97%" EmptyDataText="Atenção! Não existem reservas registradas.">
                                            <AlternatingRowStyle CssClass="tableRowOdd" />
                                            <EmptyDataRowStyle Height="100px" HorizontalAlign="Center" VerticalAlign="Middle"
                                                BorderStyle="Solid" BorderColor="White" />
                                            <Columns>
                                                <asp:TemplateField HeaderText="Responsável">
                                                    <ItemTemplate>
                                                        <asp:LinkButton ID="lnkResponsavel" runat="server" CommandName="select" OnClick="lnkResponsavel_Click"
                                                            Text='<%# Bind("resNomeComplemento") %>' ToolTip="Ir para Responsável"></asp:LinkButton>
                                                    </ItemTemplate>
                                                    <HeaderTemplate>
                                                        <asp:Image ID="imgReservaInfo" runat="server" ImageUrl="~/images/Info.png" ToolTip="Clique na linha desejada para acessar as informações" />
                                                        <asp:LinkButton ID="lnkhdrResponsavel" runat="server" ForeColor="White" OnClick="lnkhdrResponsavel_Click"
                                                            ToolTip="Ordenar" CommandArgument="0">Responsável</asp:LinkButton>
                                                        <asp:ImageButton ID="imgResponsavel" runat="server" ImageAlign="AbsMiddle" ImageUrl="~/images/AZ.png"
                                                            OnClick="imgResponsavel_Click" Visible="False" CommandArgument="0" />
                                                    </HeaderTemplate>
                                                    <FooterStyle CssClass="formLabel" HorizontalAlign="Left" />
                                                    <HeaderStyle BorderWidth="0px" CssClass="formRuler" />
                                                    <ItemStyle CssClass="gridBorderColor" HorizontalAlign="Left" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Status">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblReserva" runat="server" Text='<%# Bind("resStatusDesc") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <HeaderTemplate>
                                                        <asp:LinkButton ID="lnkhdrReserva" runat="server" ForeColor="White" OnClick="lnkhdrResponsavel_Click"
                                                            ToolTip="Ordenar" CommandArgument="0">Reserva</asp:LinkButton>
                                                        <asp:ImageButton ID="imgReserva" runat="server" ImageAlign="AbsMiddle" ImageUrl="~/images/AZ.png"
                                                            OnClick="imgResponsavel_Click" Visible="False" CommandArgument="0" />
                                                    </HeaderTemplate>
                                                    <FooterStyle CssClass="formLabel" HorizontalAlign="Center" />
                                                    <HeaderStyle BorderWidth="0px" CssClass="formRuler" />
                                                    <ItemStyle CssClass="gridBorderColor" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Período">
                                                    <ItemTemplate>
                                                        <asp:LinkButton ID="lnkDisponibilidade" runat="server" CommandName="select" Text='<%# Bind("resDataIni") %>'
                                                            ToolTip="Filtrar"></asp:LinkButton>
                                                    </ItemTemplate>
                                                    <HeaderTemplate>
                                                        <asp:LinkButton ID="lnkhdrPeriodo" runat="server" ForeColor="White" OnClick="lnkhdrResponsavel_Click"
                                                            ToolTip="Ordenar" CommandArgument="0">Período</asp:LinkButton>
                                                        <asp:ImageButton ID="imgPeriodo" runat="server" ImageAlign="AbsMiddle" ImageUrl="~/images/AZ.png"
                                                            OnClick="imgResponsavel_Click" Visible="False" CommandArgument="0" />
                                                    </HeaderTemplate>
                                                    <HeaderStyle BorderWidth="0px" CssClass="formRuler" />
                                                    <ItemStyle CssClass="gridBorderColor" Wrap="False" HorizontalAlign="Center" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Integrantes">
                                                    <ItemTemplate>
                                                        <asp:LinkButton ID="lnkIntegrante" runat="server" CommandName="select" Text='<%# Bind("qtdHospede") %>'
                                                            ToolTip="Filtrar"></asp:LinkButton>
                                                    </ItemTemplate>
                                                    <HeaderTemplate>
                                                        <asp:LinkButton ID="lnkhdrIntegrante" runat="server" ForeColor="White" OnClick="lnkhdrResponsavel_Click"
                                                            ToolTip="Ordenar" CommandArgument="0">Integrantes</asp:LinkButton>
                                                        <asp:ImageButton ID="imgIntegrante" runat="server" ImageAlign="AbsMiddle" ImageUrl="~/images/AZ.png"
                                                            OnClick="imgResponsavel_Click" Visible="False" CommandArgument="0" />
                                                    </HeaderTemplate>
                                                    <FooterStyle CssClass="formLabel" HorizontalAlign="Center" />
                                                    <HeaderStyle BorderWidth="0px" CssClass="formRuler" />
                                                    <ItemStyle CssClass="gridBorderColor" HorizontalAlign="Center" />
                                                </asp:TemplateField>
                                                <asp:BoundField HeaderText="Diárias">
                                                    <FooterStyle HorizontalAlign="Center" />
                                                    <HeaderStyle CssClass="formRuler" />
                                                    <ItemStyle HorizontalAlign="Center" />
                                                </asp:BoundField>
                                                <asp:TemplateField HeaderText="Apto">
                                                    <ItemTemplate>
                                                        <asp:LinkButton ID="lnkApto" runat="server" CommandName="select" Text='<%# Bind("qtdAcomodacao") %>'
                                                            ToolTip="Filtrar"></asp:LinkButton>
                                                    </ItemTemplate>
                                                    <HeaderTemplate>
                                                        <asp:LinkButton ID="lnkhdrApto" runat="server" ForeColor="White" OnClick="lnkhdrResponsavel_Click"
                                                            ToolTip="Ordenar" CommandArgument="0">Apto</asp:LinkButton>
                                                        <asp:ImageButton ID="imgApto" runat="server" ImageAlign="AbsMiddle" ImageUrl="~/images/AZ.png"
                                                            OnClick="imgResponsavel_Click" Visible="False" CommandArgument="0" />
                                                    </HeaderTemplate>
                                                    <FooterStyle CssClass="formLabel" HorizontalAlign="Center" />
                                                    <HeaderStyle BorderWidth="0px" CssClass="formRuler" />
                                                    <ItemStyle CssClass="gridBorderColor" HorizontalAlign="Center" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Valor Pago">
                                                    <ItemTemplate>
                                                        <asp:LinkButton ID="lnkValores" runat="server" CommandName="select" Text='<%# Eval("resValorPago", "{0:n}") %>'
                                                            ToolTip="Filtrar"></asp:LinkButton>
                                                    </ItemTemplate>
                                                    <HeaderTemplate>
                                                        <asp:LinkButton ID="lnkhdrValores" runat="server" ForeColor="White" OnClick="lnkhdrResponsavel_Click"
                                                            ToolTip="Ordenar" CommandArgument="0">Valor Pago</asp:LinkButton>
                                                        <asp:ImageButton ID="imgValores" runat="server" ImageAlign="AbsMiddle" ImageUrl="~/images/AZ.png"
                                                            OnClick="imgResponsavel_Click" Visible="False" CommandArgument="0" />
                                                    </HeaderTemplate>
                                                    <FooterStyle HorizontalAlign="Right" />
                                                    <HeaderStyle BorderWidth="0px" CssClass="formRuler" />
                                                    <ItemStyle CssClass="gridBorderColor" HorizontalAlign="Right" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Servidor" Visible="False">
                                                    <ItemTemplate>
                                                        <asp:LinkButton ID="lnkServidor" runat="server" CommandName="select" ToolTip="Filtrar"></asp:LinkButton>
                                                    </ItemTemplate>
                                                    <HeaderTemplate>
                                                        <asp:LinkButton ID="lnkhdrServidor" runat="server" ForeColor="White" OnClick="lnkhdrResponsavel_Click"
                                                            ToolTip="Ordenar" CommandArgument="0">Servidor</asp:LinkButton>
                                                        <asp:ImageButton ID="imgServidor" runat="server" ImageAlign="AbsMiddle" ImageUrl="~/images/AZ.png"
                                                            OnClick="imgResponsavel_Click" Visible="False" CommandArgument="0" />
                                                    </HeaderTemplate>
                                                    <HeaderStyle BorderWidth="0px" CssClass="formRuler" />
                                                    <ItemStyle CssClass="gridBorderColor" HorizontalAlign="Center" />
                                                </asp:TemplateField>
                                            </Columns>
                                            <HeaderStyle ForeColor="White" />
                                            <SelectedRowStyle Font-Bold="True" />
                                        </asp:GridView>
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
                    </td>
                    <td width="60%" id="tdFinanceiro">
                        <asp:Panel ID="pnlFinanceiro" runat="server" CssClass="ArrendodarBorda" Height="250px" DefaultButton="btnFinanceiro"
                            ScrollBars="Auto" HorizontalAlign="Center">
                            <table width="97%">
                                <tr>
                                    <td align="center" class="tableRowOdd" valign="bottom">
                                        <asp:Label ID="lblFinanceiro" runat="server" Text="Pagamentos" Font-Bold="True" Font-Italic="True"
                                            Font-Size="Medium"></asp:Label>
                                        &nbsp;
                                        <asp:Button ID="btnFinanceiro" runat="server" CssClass="imgLupa ArrendodarBorda" Text="  Consultar"
                                            CommandArgument="0" />
                                        &nbsp;<asp:ImageButton ID="btnRelFinanceiro" runat="server" ImageUrl="~/images/Report.png" ToolTip="Relatório de pagamento em cartões de crédito" />
                                    </td>
                                    <td align="center" class="tableRowOdd">&nbsp;<asp:ImageButton ID="imgBtnPagamentoMaximizar" CssClass="ArrendodarBorda" runat="server" ImageAlign="AbsMiddle"
                                        ImageUrl="~/images/Maximize.png" ToolTip="Maximizar" />
                                        &nbsp;<asp:ImageButton ID="imgBtnPagamentoMinimizar" runat="server" ImageAlign="AbsMiddle" CssClass="ArrendodarBorda"
                                            ImageUrl="~/images/Minimize.png" ToolTip="Minimizar" Visible="False" />
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2" align="center" width="60%">
                                        <asp:GridView ID="gdvReserva4" runat="server" AutoGenerateColumns="False" CellPadding="4"
                                            DataKeyNames="resId,venId,venValor,venStatus,bolImpNossoNumero,venUsuario" ShowFooter="True"
                                            Width="97%" HorizontalAlign="Center" AllowSorting="True" EmptyDataText="Atenção! Não existem pagamentos registrados.">
                                            <AlternatingRowStyle CssClass="tableRowOdd" />
                                            <EmptyDataRowStyle BorderColor="White" BorderStyle="Solid" Height="100px" HorizontalAlign="Center" />
                                            <Columns>
                                                <asp:TemplateField HeaderText="Boleto">
                                                    <ItemTemplate>
                                                        <asp:LinkButton ID="lnkNossoNumero" runat="server" CommandName="select" Text='<%# Eval("bolImpNossoNumero") %>'
                                                            ToolTip="Ir para Pagamento" OnClick="lnkNossoNumero_Click"></asp:LinkButton>
                                                    </ItemTemplate>
                                                    <HeaderTemplate>
                                                        <asp:Image ID="imgCobrancaInfo" runat="server" ImageUrl="~/images/Info.png" ToolTip="Clique na linha desejada para acessar as informações" />
                                                        <asp:LinkButton ID="lnkhdrCobranca" runat="server" ForeColor="White" OnClick="lnkhdrCobranca_Click"
                                                            ToolTip="Ordenar" CommandArgument="1">Boleto</asp:LinkButton>
                                                        <asp:ImageButton ID="imgCobranca" runat="server" ImageAlign="AbsMiddle" ImageUrl="~/images/AZ.png"
                                                            OnClick="imgCobranca_Click" Visible="False" CommandArgument="1" />
                                                    </HeaderTemplate>
                                                    <FooterStyle CssClass="formLabel" HorizontalAlign="Left" />
                                                    <HeaderStyle BorderWidth="0px" CssClass="formRuler" />
                                                    <ItemStyle CssClass="gridBorderColor" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Vencimento">
                                                    <ItemTemplate>
                                                        <asp:LinkButton ID="lnkDtVencimento" runat="server" CommandName="select" Text='<%# Bind("bolImpDtVencimento") %>'
                                                            ToolTip="Filtrar"></asp:LinkButton>
                                                    </ItemTemplate>
                                                    <HeaderTemplate>
                                                        <asp:LinkButton ID="lnkhdrVencimento" runat="server" ForeColor="White" OnClick="lnkhdrCobranca_Click"
                                                            ToolTip="Ordenar" CommandArgument="1">Vencimento</asp:LinkButton>
                                                        <asp:ImageButton ID="imgVencimento" runat="server" ImageAlign="AbsMiddle" ImageUrl="~/images/AZ.png"
                                                            OnClick="imgCobranca_Click" Visible="False" CommandArgument="1" />
                                                    </HeaderTemplate>
                                                    <HeaderStyle BorderWidth="0px" CssClass="formRuler" />
                                                    <ItemStyle CssClass="gridBorderColor" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Pagamento">
                                                    <ItemTemplate>
                                                        <asp:LinkButton ID="lnkDtPagamento" runat="server" CommandName="select" Text='<%# Bind("venData") %>'
                                                            ToolTip="Filtrar"></asp:LinkButton>
                                                    </ItemTemplate>
                                                    <HeaderTemplate>
                                                        <asp:LinkButton ID="lnkhdrPagamento" runat="server" ForeColor="White" OnClick="lnkhdrCobranca_Click"
                                                            ToolTip="Ordenar" CommandArgument="1">Pagamento</asp:LinkButton>
                                                        <asp:ImageButton ID="imgPagamento" runat="server" ImageAlign="AbsMiddle" ImageUrl="~/images/AZ.png"
                                                            OnClick="imgCobranca_Click" Visible="False" CommandArgument="1" />
                                                    </HeaderTemplate>
                                                    <HeaderStyle BorderWidth="0px" CssClass="formRuler" />
                                                    <ItemStyle CssClass="gridBorderColor" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Pago">
                                                    <ItemTemplate>
                                                        <asp:LinkButton ID="lnkVlrPago" runat="server" CommandName="select" Text='' ToolTip="Filtrar"></asp:LinkButton>
                                                    </ItemTemplate>
                                                    <HeaderTemplate>
                                                        <asp:LinkButton ID="lnkhdrVlrPago" runat="server" ForeColor="White" OnClick="lnkhdrCobranca_Click"
                                                            ToolTip="Ordenar" CommandArgument="1">Pago</asp:LinkButton>
                                                        <asp:ImageButton ID="imgVlrPago" runat="server" ImageAlign="AbsMiddle" ImageUrl="~/images/AZ.png"
                                                            OnClick="imgCobranca_Click" Visible="False" CommandArgument="1" />
                                                    </HeaderTemplate>
                                                    <HeaderStyle BorderWidth="0px" CssClass="formRuler" />
                                                    <ItemStyle CssClass="gridBorderColor" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Forma">
                                                    <ItemTemplate>
                                                        <asp:LinkButton ID="lnkFormaPgto" runat="server" CommandName="select" Text='' ToolTip="Filtrar"></asp:LinkButton>
                                                    </ItemTemplate>
                                                    <HeaderTemplate>
                                                        <asp:LinkButton ID="lnkhdrForma" runat="server" ForeColor="White" OnClick="lnkhdrCobranca_Click"
                                                            ToolTip="Ordenar" CommandArgument="1">Forma</asp:LinkButton>
                                                        <asp:ImageButton ID="imgForma" runat="server" ImageAlign="AbsMiddle" ImageUrl="~/images/AZ.png"
                                                            OnClick="imgCobranca_Click" Visible="False" CommandArgument="1" />
                                                    </HeaderTemplate>
                                                    <HeaderStyle BorderWidth="0px" CssClass="formRuler" />
                                                    <ItemStyle CssClass="gridBorderColor" />
                                                </asp:TemplateField>
                                            </Columns>
                                            <HeaderStyle ForeColor="White" />
                                            <RowStyle HorizontalAlign="Center" />
                                            <SelectedRowStyle Font-Bold="True" />
                                        </asp:GridView>
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
                    </td>
                </tr>
                <tr>
                    <td width="60%" id="TdIntegrantes">
                        <asp:Panel ID="pnlIntegrante" CssClass="ArrendodarBorda" runat="server" Height="250px" ScrollBars="Auto">
                            <table width="97%">
                                <tr>
                                    <td align="center" class="tableRowOdd">
                                        <asp:Label ID="lblIntegrante" runat="server" Text="Integrantes" Font-Bold="True"
                                            Font-Italic="True" Font-Size="Medium"></asp:Label>
                                        &nbsp;
                                        <asp:Button ID="btnIntegrante" runat="server" CssClass="imgLupa ArrendodarBorda" Text="  Consultar"
                                            CommandArgument="0" />
                                    </td>
                                    <td align="center" class="tableRowOdd">
                                        <asp:ImageButton ID="imgBtnIntegranteMaximizar" runat="server" ImageAlign="AbsMiddle" CssClass="ArrendodarBorda"
                                            ImageUrl="~/images/Maximize.png" ToolTip="Maximizar" />
                                        &nbsp;<asp:ImageButton ID="imgBtnIntegranteMinimizar" runat="server" ImageAlign="AbsMiddle" CssClass="ArrendodarBorda"
                                            ImageUrl="~/images/Minimize.png" ToolTip="Minimizar" Visible="False" />
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2" align="center" style="margin-left: 40px">
                                        <asp:GridView ID="gdvReserva3" runat="server" AutoGenerateColumns="False" CellPadding="4"
                                            DataKeyNames="intId,resId,solId,hosValorDevido,hosValorPago,intExcluido,catLink,intUsuario,intVinculoId,NomeResponsavel"
                                            ShowFooter="True" Width="97%" EmptyDataText="Atenção! Não existem integrantes registrados.">
                                            <AlternatingRowStyle CssClass="tableRowOdd" />
                                            <EmptyDataRowStyle BorderColor="White" BorderStyle="Solid" Height="100px" HorizontalAlign="Center" />
                                            <Columns>
                                                <asp:TemplateField HeaderText="Nome">
                                                    <ItemTemplate>
                                                        <asp:Image ID="imgDependente" runat="server" ImageAlign="AbsMiddle" ImageUrl="~/images/DependenteReponsavel.png"
                                                            ToolTip="Vínculo" Visible="False" />
                                                        <asp:LinkButton ID="lnkIntNome" runat="server" CommandName="select" Text='<%# Eval("intNome") %>'
                                                            ToolTip="Ir para Integrante" OnClick="lnkIntNome_Click"></asp:LinkButton>
                                                    </ItemTemplate>
                                                    <HeaderTemplate>
                                                        <asp:Image ID="imgIntegranteInfo" runat="server" ImageUrl="~/images/Info.png" ToolTip="Clique na linha desejada para acessar as informações" />
                                                        <asp:LinkButton ID="lnkhdrIntegrante" runat="server" ForeColor="White" OnClick="lnkhdrIntegrante_Click"
                                                            ToolTip="Ordenar" CommandArgument="1">Nome</asp:LinkButton>
                                                        <asp:ImageButton ID="imgIntegrante" runat="server" ImageAlign="AbsMiddle" ImageUrl="~/images/AZ.png"
                                                            OnClick="imgIntegrante_Click" Visible="False" CommandArgument="1" />
                                                    </HeaderTemplate>
                                                    <FooterStyle CssClass="formLabel" HorizontalAlign="Left" />
                                                    <HeaderStyle BorderWidth="0px" CssClass="formRuler" />
                                                    <ItemStyle CssClass="gridBorderColor" Width="60%" HorizontalAlign="Left" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Categoria">
                                                    <ItemTemplate>
                                                        <asp:LinkButton ID="lnkCategoriaIntegrante" runat="server" CommandName="select" Text='<%# Bind("catDescricao") %>'
                                                            ToolTip="Filtrar"></asp:LinkButton>
                                                    </ItemTemplate>
                                                    <HeaderTemplate>
                                                        <asp:LinkButton ID="lnkhdrCategoria" runat="server" ForeColor="White" OnClick="lnkhdrIntegrante_Click"
                                                            ToolTip="Ordenar" CommandArgument="1">Categoria</asp:LinkButton>
                                                        <asp:ImageButton ID="imgCategoria" runat="server" ImageAlign="AbsMiddle" ImageUrl="~/images/AZ.png"
                                                            OnClick="imgIntegrante_Click" Visible="False" CommandArgument="1" />
                                                    </HeaderTemplate>
                                                    <HeaderStyle BorderWidth="0px" CssClass="formRuler" />
                                                    <ItemStyle CssClass="gridBorderColor" Width="10%" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Início">
                                                    <ItemTemplate>
                                                        <asp:LinkButton ID="lnkCheckInIntegrante" runat="server" CommandName="select" Text='<%# Bind("intDiaIni") %>'
                                                            ToolTip="Filtrar"></asp:LinkButton>
                                                    </ItemTemplate>
                                                    <HeaderTemplate>
                                                        <asp:LinkButton ID="lnkhdrCheckInIntegrante" runat="server" ForeColor="White" OnClick="lnkhdrIntegrante_Click"
                                                            ToolTip="Ordenar" CommandArgument="1">Início</asp:LinkButton>
                                                        <asp:ImageButton ID="imgCheckInIntegrante" runat="server" ImageAlign="AbsMiddle"
                                                            ImageUrl="~/images/AZ.png" OnClick="imgIntegrante_Click" Visible="False" CommandArgument="1" />
                                                    </HeaderTemplate>
                                                    <HeaderStyle BorderWidth="0px" CssClass="formRuler" />
                                                    <ItemStyle CssClass="gridBorderColor" Width="10%" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Término">
                                                    <ItemTemplate>
                                                        <asp:LinkButton ID="lnkCheckOutIntegrante" runat="server" CommandName="select" Text='<%# Bind("intDiaFim") %>'
                                                            ToolTip="Filtrar"></asp:LinkButton>
                                                    </ItemTemplate>
                                                    <HeaderTemplate>
                                                        <asp:LinkButton ID="lnkhdrCheckOutIntegrante" runat="server" ForeColor="White" OnClick="lnkhdrIntegrante_Click"
                                                            ToolTip="Ordenar" CommandArgument="1">Término</asp:LinkButton>
                                                        <asp:ImageButton ID="imgCheckOutIntegrante" runat="server" ImageAlign="AbsMiddle"
                                                            ImageUrl="~/images/AZ.png" OnClick="imgIntegrante_Click" Visible="False" CommandArgument="1" />
                                                    </HeaderTemplate>
                                                    <HeaderStyle BorderWidth="0px" CssClass="formRuler" />
                                                    <ItemStyle CssClass="gridBorderColor" Width="10%" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="A Pagar">
                                                    <ItemTemplate>
                                                        <asp:LinkButton ID="lnkHosValorDevido" runat="server" CommandName="select" Text='<%# Bind("hosValorDevido") %>'
                                                            ToolTip="Filtrar"></asp:LinkButton>
                                                    </ItemTemplate>
                                                    <HeaderTemplate>
                                                        <asp:LinkButton ID="lnkhdrVlrDevidoIntegrante" runat="server" ForeColor="White" OnClick="lnkhdrIntegrante_Click"
                                                            ToolTip="Ordenar" CommandArgument="1">A Pagar</asp:LinkButton>
                                                        <asp:ImageButton ID="imgVlrDevidoIntegrante" runat="server" ImageAlign="AbsMiddle"
                                                            ImageUrl="~/images/AZ.png" OnClick="imgIntegrante_Click" Visible="False" CommandArgument="1" />
                                                    </HeaderTemplate>
                                                    <HeaderStyle BorderWidth="0px" CssClass="formRuler" />
                                                    <ItemStyle CssClass="gridBorderColor" Width="10%" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Pago">
                                                    <ItemTemplate>
                                                        <asp:LinkButton ID="lnkHosValorPago" runat="server" CommandName="select" Text='<%# Bind("hosValorPago") %>'
                                                            ToolTip="Filtrar"></asp:LinkButton>
                                                    </ItemTemplate>
                                                    <HeaderTemplate>
                                                        <asp:LinkButton ID="lnkhdrVlrPagoIntegrante" runat="server" ForeColor="White" OnClick="lnkhdrIntegrante_Click"
                                                            ToolTip="Ordenar" CommandArgument="1">Pago</asp:LinkButton>
                                                        <asp:ImageButton ID="imgVlrPagoIntegrante" runat="server" ImageAlign="AbsMiddle"
                                                            ImageUrl="~/images/AZ.png" OnClick="imgIntegrante_Click" Visible="False" CommandArgument="1" />
                                                    </HeaderTemplate>
                                                    <HeaderStyle BorderWidth="0px" CssClass="formRuler" />
                                                    <ItemStyle CssClass="gridBorderColor" Width="10%" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Servidor">
                                                    <ItemTemplate>
                                                        <asp:LinkButton ID="lnkServidor" runat="server" CommandName="select" ToolTip="Filtrar"></asp:LinkButton>
                                                    </ItemTemplate>
                                                    <HeaderTemplate>
                                                        <asp:LinkButton ID="lnkhdrServidor" runat="server" ForeColor="White" OnClick="lnkhdrIntegrante_Click"
                                                            ToolTip="Ordenar" CommandArgument="1">Servidor</asp:LinkButton>
                                                        <asp:ImageButton ID="imgServidor" runat="server" ImageAlign="AbsMiddle" ImageUrl="~/images/AZ.png"
                                                            OnClick="imgIntegrante_Click" Visible="False" CommandArgument="1" />
                                                    </HeaderTemplate>
                                                    <HeaderStyle BorderWidth="0px" CssClass="formRuler" />
                                                    <ItemStyle CssClass="gridBorderColor" Width="10%" />
                                                </asp:TemplateField>
                                            </Columns>
                                            <HeaderStyle ForeColor="White" />
                                            <RowStyle HorizontalAlign="Center" />
                                            <SelectedRowStyle Font-Bold="True" />
                                        </asp:GridView>
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
                    </td>
                    <td width="60%" id="tdAcomodacao">
                        <asp:Panel ID="pnlAcomodacao" runat="server" CssClass="ArrendodarBorda" Height="250px" ScrollBars="Auto">
                            <table width="97%">
                                <tr>
                                    <td align="center" class="tableRowOdd">
                                        <asp:Label ID="lblAcomodacao" runat="server" Text="Acomodações" Font-Bold="True"
                                            Font-Italic="True" Font-Size="Medium"></asp:Label>
                                        &nbsp;
                                        <asp:Button ID="btnAcomodacao" runat="server" CssClass="imgLupa ArrendodarBorda" Text="  Consultar"
                                            CommandArgument="0" />
                                    </td>
                                    <td align="center" class="tableRowOdd">
                                        <asp:ImageButton ID="imgBtnAcomodacaoMaximizar" runat="server" ImageAlign="AbsMiddle" CssClass="ArrendodarBorda"
                                            ImageUrl="~/images/Maximize.png" ToolTip="Maximizar" />
                                        &nbsp;<asp:ImageButton ID="imgBtnAcomodacaoMinimizar" runat="server" ImageAlign="AbsMiddle" CssClass="ArrendodarBorda"
                                            ImageUrl="~/images/Minimize.png" ToolTip="Minimizar" Visible="False" />
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2" align="center">
                                        <asp:GridView ID="gdvReserva2" runat="server" AutoGenerateColumns="False" CellPadding="4"
                                            DataKeyNames="solId,resId,acmId,acmDescricao,solExcluido,solUsuario" ShowFooter="True"
                                            Width="97%" ToolTip="Clique na linha desejada para acessar as informações" EmptyDataText="Atenção! Não existem acomodações registradas.">
                                            <AlternatingRowStyle CssClass="tableRowOdd" />
                                            <EmptyDataRowStyle BorderColor="White" BorderStyle="Solid" Height="100px" HorizontalAlign="Center"
                                                VerticalAlign="Middle" Wrap="True" />
                                            <Columns>
                                                <asp:TemplateField HeaderText="Acomodação">
                                                    <ItemTemplate>
                                                        <asp:LinkButton ID="lnkAcomodacao" runat="server" CommandName="select" OnClick="lnkAcomodacao_Click"
                                                            Text='<%# Eval("acmDescricao") %>' ToolTip="Ir para Acomodação"></asp:LinkButton>
                                                    </ItemTemplate>
                                                    <HeaderTemplate>
                                                        <asp:Image ID="imgAcomodacaoInfo" runat="server" ImageUrl="~/images/Info.png" ToolTip="Clique na linha desejada para acessar as informações" />
                                                        <asp:LinkButton ID="lnkhdrAcomodacao" runat="server" ForeColor="White" OnClick="lnkhdrAcomodacao_Click"
                                                            ToolTip="Ordenar" CommandArgument="1">Acomodação</asp:LinkButton>
                                                        <asp:ImageButton ID="imgAcomodacao" runat="server" ImageAlign="AbsMiddle" ImageUrl="~/images/AZ.png"
                                                            Visible="False" CommandArgument="1" />
                                                    </HeaderTemplate>
                                                    <FooterStyle CssClass="formLabel" HorizontalAlign="Left" />
                                                    <HeaderStyle BorderWidth="0px" CssClass="formRuler" />
                                                    <ItemStyle CssClass="gridBorderColor" HorizontalAlign="Left" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Check In">
                                                    <ItemTemplate>
                                                        <asp:LinkButton ID="lnkDiaCheckInAcomodacao" runat="server" CommandName="select"
                                                            Text='<%# Bind("solDiaIni") %>' ToolTip="Filtrar"></asp:LinkButton>
                                                    </ItemTemplate>
                                                    <HeaderTemplate>
                                                        <asp:LinkButton ID="lnkhdrDiaCheckInAcomodacao" runat="server" ForeColor="White"
                                                            OnClick="lnkhdrAcomodacao_Click" ToolTip="Ordenar" CommandArgument="1">Check In</asp:LinkButton>
                                                        <asp:ImageButton ID="imgDiaCheckInAcomodacao" runat="server" ImageAlign="AbsMiddle"
                                                            ImageUrl="~/images/AZ.png" Visible="False" CommandArgument="1" />
                                                    </HeaderTemplate>
                                                    <HeaderStyle BorderWidth="0px" CssClass="formRuler" />
                                                    <ItemStyle CssClass="gridBorderColor" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Dia">
                                                    <ItemTemplate>
                                                        <asp:LinkButton ID="lnkDataCheckInAcomodacao" runat="server" CommandName="select"
                                                            Text='<%# Bind("solDataIni") %>' ToolTip="Filtrar"></asp:LinkButton>
                                                    </ItemTemplate>
                                                    <HeaderTemplate>
                                                        <asp:LinkButton ID="lnkhdrDataCheckInAcomodacao" runat="server" ForeColor="White"
                                                            OnClick="lnkhdrAcomodacao_Click" ToolTip="Ordenar" CommandArgument="1">Dia</asp:LinkButton>
                                                        <asp:ImageButton ID="imgDataCheckInAcomodacao" runat="server" ImageAlign="AbsMiddle"
                                                            ImageUrl="~/images/AZ.png" Visible="False" CommandArgument="1" />
                                                    </HeaderTemplate>
                                                    <HeaderStyle BorderWidth="0px" CssClass="formRuler" />
                                                    <ItemStyle CssClass="gridBorderColor" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Check Out">
                                                    <ItemTemplate>
                                                        <asp:LinkButton ID="lnkDiaCheckOutAcomodacao" runat="server" CommandName="select"
                                                            Text='<%# Bind("solDiaFim") %>' ToolTip="Filtrar"></asp:LinkButton>
                                                    </ItemTemplate>
                                                    <HeaderTemplate>
                                                        <asp:LinkButton ID="lnkhdrDiaCheckOutAcomodacao" runat="server" ForeColor="White"
                                                            OnClick="lnkhdrAcomodacao_Click" ToolTip="Ordenar" CommandArgument="1">Check Out</asp:LinkButton>
                                                        <asp:ImageButton ID="imgDiaCheckOutAcomodacao" runat="server" ImageAlign="AbsMiddle"
                                                            ImageUrl="~/images/AZ.png" Visible="False" CommandArgument="1" />
                                                    </HeaderTemplate>
                                                    <HeaderStyle BorderWidth="0px" CssClass="formRuler" />
                                                    <ItemStyle CssClass="gridBorderColor" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Dia">
                                                    <ItemTemplate>
                                                        <asp:LinkButton ID="lnkDataCheckOutAcomodacao" runat="server" CommandName="select"
                                                            Text='<%# Bind("solDataFim") %>' ToolTip="Filtrar"></asp:LinkButton>
                                                    </ItemTemplate>
                                                    <HeaderTemplate>
                                                        <asp:LinkButton ID="lnkhdrDataCheckOutAcomodacao" runat="server" ForeColor="White"
                                                            OnClick="lnkhdrAcomodacao_Click" ToolTip="Ordenar" CommandArgument="1">Dia</asp:LinkButton>
                                                        <asp:ImageButton ID="imgDataCheckOutAcomodacao" runat="server" ImageAlign="AbsMiddle"
                                                            ImageUrl="~/images/AZ.png" Visible="False" CommandArgument="1" />
                                                    </HeaderTemplate>
                                                    <HeaderStyle BorderWidth="0px" CssClass="formRuler" />
                                                    <ItemStyle CssClass="gridBorderColor" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Servidor">
                                                    <ItemTemplate>
                                                        <asp:LinkButton ID="lnkServidor" runat="server" CommandName="select" ToolTip="Filtrar"></asp:LinkButton>
                                                    </ItemTemplate>
                                                    <HeaderTemplate>
                                                        <asp:LinkButton ID="lnkhdrServidor" runat="server" ForeColor="White" OnClick="lnkhdrAcomodacao_Click"
                                                            ToolTip="Ordenar" CommandArgument="1">Servidor</asp:LinkButton>
                                                        <asp:ImageButton ID="imgServidor" runat="server" ImageAlign="AbsMiddle" ImageUrl="~/images/AZ.png"
                                                            Visible="False" CommandArgument="1" />
                                                    </HeaderTemplate>
                                                    <HeaderStyle BorderWidth="0px" CssClass="formRuler" />
                                                    <ItemStyle CssClass="gridBorderColor" />
                                                </asp:TemplateField>
                                            </Columns>
                                            <HeaderStyle ForeColor="White" />
                                            <RowStyle HorizontalAlign="Center" />
                                            <SelectedRowStyle Font-Bold="True" />
                                        </asp:GridView>
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
                        <asp:Panel ID="pnlPlanilha" runat="server" Height="250px" Visible="False" CssClass="ArrendodarBorda">
                            <table width="97%">
                                <tr>
                                    <td align="center" class="tableRowOdd">
                                        <asp:Label ID="lblPrecoVenda" runat="server" Text="Preço de Venda" Font-Bold="True"
                                            Font-Italic="True" Font-Size="Medium"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="center">
                                        <table>
                                            <tr>
                                                <td>&nbsp;
                                                </td>
                                                <td>&nbsp;
                                                </td>
                                                <td>&nbsp;
                                                </td>
                                                <td>&nbsp;
                                                </td>
                                                <td>&nbsp;
                                                </td>
                                                <td>&nbsp;
                                                </td>
                                                <td>&nbsp;
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>Categoria
                                                </td>
                                                <td>&nbsp;
                                                </td>
                                                <td>Adulto
                                                </td>
                                                <td>&nbsp;
                                                </td>
                                                <td>Criança
                                                </td>
                                                <td>&nbsp;
                                                </td>
                                                <td>Isento
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>&nbsp;
                                                </td>
                                                <td>&nbsp;
                                                </td>
                                                <td>&nbsp;
                                                </td>
                                                <td>&nbsp;
                                                </td>
                                                <td>&nbsp;
                                                </td>
                                                <td>&nbsp;
                                                </td>
                                                <td>&nbsp;
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>Comerciário
                                                </td>
                                                <td>&nbsp;
                                                </td>
                                                <td>
                                                    <asp:Label ID="lblComAdulto" runat="server" Text="0,00"></asp:Label>
                                                </td>
                                                <td>&nbsp;
                                                </td>
                                                <td>
                                                    <asp:Label ID="lblComCrianca" runat="server" Text="0,00"></asp:Label>
                                                </td>
                                                <td>&nbsp;
                                                </td>
                                                <td>
                                                    <asp:Label ID="lblComIsento" runat="server" Text="0,00"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>Conveniado
                                                </td>
                                                <td>&nbsp;
                                                </td>
                                                <td>
                                                    <asp:Label ID="lblConvAdulto" runat="server" Text="0,00"></asp:Label>
                                                </td>
                                                <td>&nbsp;
                                                </td>
                                                <td>
                                                    <asp:Label ID="lblConvCrianca" runat="server" Text="0,00"></asp:Label>
                                                </td>
                                                <td>&nbsp;
                                                </td>
                                                <td>
                                                    <asp:Label ID="lblConvIsento" runat="server" Text="0,00"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>Usuário
                                                </td>
                                                <td>&nbsp;
                                                </td>
                                                <td>
                                                    <asp:Label ID="lblUsuAdulto" runat="server" Text="0,00"></asp:Label>
                                                </td>
                                                <td>&nbsp;
                                                </td>
                                                <td>
                                                    <asp:Label ID="lblUsuCrianca" runat="server" Text="0,00"></asp:Label>
                                                </td>
                                                <td>&nbsp;
                                                </td>
                                                <td>
                                                    <asp:Label ID="lblUsuIsento" runat="server" Text="0,00"></asp:Label>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <asp:HiddenField ID="hddResId" runat="server" Value="0" />
                        <asp:HiddenField ID="hddResDataIni" runat="server" />
                        <asp:HiddenField ID="hddResDataFim" runat="server" />
                        <asp:HiddenField ID="hddResCaracteristica" runat="server" />
                        <asp:HiddenField ID="hddResStatus" runat="server" />
                        <asp:HiddenField ID="hddConsulta" runat="server" Value="0" />
                        <asp:HiddenField ID="hddtxtConsultaRecepcao" runat="server" />
                        <asp:HiddenField ID="hddcmbBloco" runat="server" />
                        <asp:HiddenField ID="hddckbEntrada" runat="server" />
                        <asp:HiddenField ID="hddckbEstada" runat="server" />
                        <asp:HiddenField ID="hddckbSaida" runat="server" />
                        <asp:HiddenField ID="hddckbTransferencia" runat="server" />
                        <asp:HiddenField ID="hddOrgGrupo" runat="server" Value="N" />
                        <asp:HiddenField ID="hddOrgLotacao" runat="server" />
                        <asp:HiddenField ID="hddResDtLimiteRetorno" runat="server" />
                        <asp:Panel ID="pnlPlanilhaCusto" runat="server" Width="100%" Visible="False">
                            <table>
                                <tr>
                                    <td align="center" class="tableRowOdd">
                                        <asp:Label ID="lblPlanilhaCusto" runat="server" Text="Planilha de Custo" Font-Bold="True"
                                            Font-Italic="True" Font-Size="Medium"></asp:Label>
                                    </td>
                                    <td align="center" class="tableRowOdd">
                                        <asp:ImageButton ID="imgBtnPlanilhaCustoVoltar" runat="server" ImageAlign="AbsMiddle"
                                            ImageUrl="~/images/VoltarAzul.png" ToolTip="Voltar" Visible="True" CausesValidation="False" />
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2">Quantidade
                                        <asp:TextBox ID="txtPlcQtde" runat="server" Columns="1" MaxLength="3"></asp:TextBox>
                                        Capacidade
                                        <asp:TextBox ID="txtPlcCapacidade" runat="server" Columns="1" MaxLength="3"></asp:TextBox>
                                        Guia
                                        <asp:TextBox ID="txtPlcGuia" runat="server" Columns="1" MaxLength="1"></asp:TextBox>
                                        Motorista
                                        <asp:TextBox ID="txtPlcMotorista" runat="server" Columns="1" MaxLength="1"></asp:TextBox>
                                        % Conveniado
                                        <asp:TextBox ID="txtPlcPercentualConveniado" runat="server" Columns="1" MaxLength="6"></asp:TextBox>
                                        % Usuário
                                        <asp:TextBox ID="txtPlcPercentualUsuario" runat="server" Columns="1" MaxLength="6"></asp:TextBox>
                                        Margem
                                        <asp:TextBox ID="txtPlcMargem" runat="server" Columns="1" MaxLength="6"></asp:TextBox>
                                        Idade Colo até
                                        <asp:TextBox ID="txtPlcColo" runat="server" Columns="1" MaxLength="6"></asp:TextBox>
                                        &nbsp;Idade Isento
                                        <asp:TextBox ID="TxtPlcIsento" runat="server" Columns="1"></asp:TextBox>
                                        &nbsp;Idade Criança<asp:TextBox ID="TxtPlcCrianca" runat="server" Columns="1" MaxLength="6"></asp:TextBox>
                                        &nbsp;anos incompletos |
                                        <asp:CheckBox ID="ckbPlcValorado" runat="server" Text="MultiValorado" TextAlign="Left" />
                                        <asp:CheckBox ID="ckbPlcAutorizaConveniado" Text="Autoriza Conveniado" runat="server"
                                            TextAlign="Left" />
                                        <asp:CheckBox ID="ckbPlcAutorizaUsuario" Text="Autoriza Usuário" runat="server" TextAlign="Left" />
                                        <asp:Button ID="btnPlanilhaCustoGravar" runat="server" Text="    Salvar" CssClass="imgGravar"
                                            AccessKey="S" ToolTip="Alt+S" />
                                        <asp:Button ID="btnPlanilhaCustoItem" runat="server" Text="  Itens" CssClass="imgLupa"
                                            AccessKey="I" ToolTip="Alt+I" CausesValidation="False" />
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2" align="center">
                                        <asp:GridView ID="gdvReserva13" runat="server" AutoGenerateColumns="False" CellPadding="4"
                                            DataKeyNames="PliId,PciId,PciValorTotal,PciValorComAdulto,PciValorComCrianca,PciValorComIsento,PciValorConvAdulto,PciValorConvCrianca,PciValorConvIsento,PciValorUsuAdulto,PciValorUsuCrianca,PciValorUsuIsento"
                                            ShowFooter="True" Width="99%" EmptyDataText="Atenção! Não existem itens de planilha de custo registrados.">
                                            <AlternatingRowStyle CssClass="tableRowOdd" />
                                            <Columns>
                                                <asp:TemplateField HeaderText="Descrição">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblPliDescricao" runat="server" Text='<%# Eval("pliDescricao") %>'></asp:Label>
                                                        <asp:ImageButton ID="imgBtnCalcularCusto" runat="server" ImageAlign="AbsMiddle" ImageUrl="~/images/calcular.png"
                                                            ToolTip="Calcular" Visible="False" OnClick="imgBtnCalcularCusto_Click" />
                                                    </ItemTemplate>
                                                    <FooterStyle CssClass="formRuler" />
                                                    <HeaderStyle BorderWidth="0px" CssClass="formRuler" />
                                                    <ItemStyle CssClass="gridBorderColor" HorizontalAlign="Left" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Total">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="txtPciValorTotal" runat="server" Text='<%# Eval("PciValorTotal") %>'
                                                            Columns="5" ForeColor="Blue"></asp:TextBox>
                                                        <asp:Label ID="lblPciValorTotal" runat="server" Text='<%# Eval("PciValorTotal") %>'
                                                            ForeColor="Blue" Visible="False"></asp:Label>
                                                        <asp:ImageButton ID="imgBtnCalcular" runat="server" ImageAlign="AbsMiddle" ImageUrl="~/images/calcular.png"
                                                            ToolTip="Calcular" Visible="False" CommandName="select" OnClick="imgBtnCalcularCusto_Click" />
                                                    </ItemTemplate>
                                                    <FooterStyle CssClass="formRuler" />
                                                    <HeaderStyle BorderWidth="0px" CssClass="formRuler" />
                                                    <ItemStyle CssClass="gridBorderColor" Wrap="False" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Comerciário Adulto">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="txtPciValorComAdulto" runat="server" Text='<%# Eval("PciValorComAdulto") %>'
                                                            Columns="5" ForeColor="Green"></asp:TextBox>
                                                        <asp:Label ID="lblPciValorComAdulto" runat="server" Text='<%# Eval("PciValorComAdulto") %>'
                                                            ForeColor="Green" Visible="False"></asp:Label>
                                                    </ItemTemplate>
                                                    <FooterStyle CssClass="formRuler" />
                                                    <HeaderStyle BorderWidth="0px" CssClass="formRuler" />
                                                    <ItemStyle CssClass="gridBorderColor" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Comerciário Criança">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="txtPciValorComCrianca" runat="server" Text='<%# Eval("PciValorComCrianca") %>'
                                                            Columns="5" ForeColor="Green"></asp:TextBox>
                                                        <asp:Label ID="lblPciValorComCrianca" runat="server" Text='<%# Eval("PciValorComCrianca") %>'
                                                            ForeColor="Green" Visible="False"></asp:Label>
                                                    </ItemTemplate>
                                                    <FooterStyle CssClass="formRuler" />
                                                    <HeaderStyle BorderWidth="0px" CssClass="formRuler" />
                                                    <ItemStyle CssClass="gridBorderColor" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Comerciário Isento">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="txtPciValorComIsento" runat="server" Text='<%# Eval("PciValorComIsento") %>'
                                                            Columns="5" ForeColor="Green"></asp:TextBox>
                                                        <asp:Label ID="lblPciValorComIsento" runat="server" Text='<%# Eval("PciValorComIsento") %>'
                                                            ForeColor="Green" Visible="False"></asp:Label>
                                                    </ItemTemplate>
                                                    <FooterStyle CssClass="formRuler" />
                                                    <HeaderStyle BorderWidth="0px" CssClass="formRuler" />
                                                    <ItemStyle CssClass="gridBorderColor" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Conveniado Adulto">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="txtPciValorConvAdulto" runat="server" Text='<%# Eval("PciValorConvAdulto") %>'
                                                            Columns="5" ForeColor="Chocolate"></asp:TextBox>
                                                        <asp:Label ID="lblPciValorConvAdulto" runat="server" Text='<%# Eval("PciValorConvAdulto") %>'
                                                            ForeColor="Chocolate" Visible="False"></asp:Label>
                                                    </ItemTemplate>
                                                    <FooterStyle CssClass="formRuler" />
                                                    <HeaderStyle BorderWidth="0px" CssClass="formRuler" />
                                                    <ItemStyle CssClass="gridBorderColor" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Conveniado Criança">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="txtPciValorConvCrianca" runat="server" Text='<%# Eval("PciValorConvCrianca") %>'
                                                            Columns="5" ForeColor="Chocolate"></asp:TextBox>
                                                        <asp:Label ID="lblPciValorConvCrianca" runat="server" Text='<%# Eval("PciValorConvCrianca") %>'
                                                            ForeColor="Chocolate" Visible="False"></asp:Label>
                                                    </ItemTemplate>
                                                    <FooterStyle CssClass="formRuler" />
                                                    <HeaderStyle BorderWidth="0px" CssClass="formRuler" />
                                                    <ItemStyle CssClass="gridBorderColor" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Conveniado Isento">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="txtPciValorConvIsento" runat="server" Text='<%# Eval("PciValorConvIsento") %>'
                                                            Columns="5" ForeColor="Chocolate"></asp:TextBox>
                                                        <asp:Label ID="lblPciValorConvIsento" runat="server" Text='<%# Eval("PciValorConvIsento") %>'
                                                            ForeColor="Chocolate" Visible="False"></asp:Label>
                                                    </ItemTemplate>
                                                    <FooterStyle CssClass="formRuler" />
                                                    <HeaderStyle BorderWidth="0px" CssClass="formRuler" />
                                                    <ItemStyle CssClass="gridBorderColor" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Usuário Adulto">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="txtPciValorUsuAdulto" runat="server" Text='<%# Eval("PciValorUsuAdulto") %>'
                                                            Columns="5" ForeColor="Red"></asp:TextBox>
                                                        <asp:Label ID="lblPciValorUsuAdulto" runat="server" Text='<%# Eval("PciValorUsuAdulto") %>'
                                                            ForeColor="Red" Visible="False"></asp:Label>
                                                    </ItemTemplate>
                                                    <FooterStyle CssClass="formRuler" />
                                                    <HeaderStyle BorderWidth="0px" CssClass="formRuler" />
                                                    <ItemStyle CssClass="gridBorderColor" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Usuário Criança">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="txtPciValorUsuCrianca" runat="server" Text='<%# Eval("PciValorUsuCrianca") %>'
                                                            Columns="5" ForeColor="Red"></asp:TextBox>
                                                        <asp:Label ID="lblPciValorUsuCrianca" runat="server" Text='<%# Eval("PciValorUsuCrianca") %>'
                                                            ForeColor="Red" Visible="False"></asp:Label>
                                                    </ItemTemplate>
                                                    <FooterStyle CssClass="formRuler" />
                                                    <HeaderStyle BorderWidth="0px" CssClass="formRuler" />
                                                    <ItemStyle CssClass="gridBorderColor" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Usuário Isento">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="txtPciValorUsuIsento" runat="server" Text='<%# Eval("PciValorUsuIsento") %>'
                                                            Columns="5" ForeColor="Red"></asp:TextBox>
                                                        <asp:Label ID="lblPciValorUsuIsento" runat="server" Text='<%# Eval("PciValorUsuIsento") %>'
                                                            ForeColor="Red" Visible="False"></asp:Label>
                                                    </ItemTemplate>
                                                    <FooterStyle CssClass="formRuler" />
                                                    <HeaderStyle BorderWidth="0px" CssClass="formRuler" />
                                                    <ItemStyle CssClass="gridBorderColor" />
                                                </asp:TemplateField>
                                            </Columns>
                                            <HeaderStyle ForeColor="White" />
                                            <FooterStyle ForeColor="White" />
                                            <SelectedRowStyle Font-Bold="True" />
                                        </asp:GridView>
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
                        <asp:RoundedCornersExtender ID="pnlPlanilhaCusto_RoundedCornersExtender" runat="server"
                            Enabled="True" TargetControlID="pnlPlanilhaCusto">
                        </asp:RoundedCornersExtender>
                        <asp:Panel ID="pnlPlanilhaItem" runat="server" Width="100%" Visible="false">
                            <table>
                                <tr>
                                    <td align="center" class="tableRowOdd">
                                        <asp:Label ID="lblPlanilhaItem" runat="server" Text="Registros da Planilha de Custo"
                                            Font-Bold="True" Font-Italic="True" Font-Size="Medium"></asp:Label>
                                    </td>
                                    <td align="center" class="tableRowOdd">
                                        <asp:ImageButton ID="imgBtnPlanilhaItemVoltar" runat="server" ImageAlign="AbsMiddle"
                                            ImageUrl="~/images/VoltarAzul.png" ToolTip="Voltar" Visible="True" CausesValidation="False" />
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                        <a>Descrição</a>
                                        <asp:RequiredFieldValidator ID="rfvPliDescricao" runat="server" ControlToValidate="txtPliDescricao"
                                            ErrorMessage=" Obrigatória" ToolTip="Peencha a Descrição" Width="0px" Display="Dynamic"></asp:RequiredFieldValidator>
                                        &nbsp;<asp:TextBox ID="txtPliDescricao" runat="server" Columns="50" MaxLength="50"
                                            AccessKey="C" ToolTip="Alt+C"></asp:TextBox>
                                        &nbsp;<asp:Button ID="btnPlanilhaItemNovo" runat="server" Text="    Novo" CssClass="imgNovo"
                                            AccessKey="N" ToolTip="Alt+N" CausesValidation="False" />
                                        <asp:Button ID="btnPlanilhaItemGravar" runat="server" Text="    Salvar" CssClass="imgGravar"
                                            AccessKey="S" ToolTip="Alt+S" OnClientClick="if(confirm('Deseja salvar agora?'))
                                {
                                    if(ctl00_conPlaHolTurismoSocial_hddProcessando.value=='S')
                                {
                                    alert('Este processo está em execução. Por favor, aguarde.');
                                    return false;
                                }
                            else
                                {
                                    ctl00_conPlaHolTurismoSocial_hddProcessando.value='S';
                                    return true;
                                }
                                }

                            else
                                {
                                    return false;
                                };" />
                                        <asp:Button ID="btnPlanilhaItemExcluir" runat="server" Text="    Excluir" CssClass="imgExcluir"
                                            AccessKey="E" ToolTip="Alt+E" OnClientClick="if(confirm('Confirma a exclusão?'))
                                {
                                    if(ctl00_conPlaHolTurismoSocial_hddProcessando.value=='S')
                                {
                                    alert('Este processo está em execução. Por favor, aguarde.');
                                    return false;
                                }
                            else
                                {
                                    ctl00_conPlaHolTurismoSocial_hddProcessando.value='S';
                                    return true;
                                }
                                }

                            else
                                {
                                    return false;
                                };" />
                                        <asp:TextBox ID="txtPliId" runat="server" Visible="False"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2" align="center">
                                        <asp:GridView ID="gdvReserva5" runat="server" AutoGenerateColumns="False" CellPadding="4"
                                            DataKeyNames="pliId,pliDescricao" ShowFooter="True" Width="99%" AllowSorting="True"
                                            EmptyDataText="Atenção! Não existem itens de planilha de custo registrados.">
                                            <AlternatingRowStyle CssClass="tableRowOdd" />
                                            <Columns>
                                                <asp:TemplateField HeaderText="Descrição">
                                                    <ItemTemplate>
                                                        <asp:LinkButton ID="lnkPliDescricao" runat="server" CommandName="select" Text='<%# Eval("pliDescricao") %>'
                                                            ToolTip="Acessar" CausesValidation="False"></asp:LinkButton>
                                                    </ItemTemplate>
                                                    <FooterStyle CssClass="formRuler" />
                                                    <HeaderStyle BorderWidth="0px" CssClass="formRuler" />
                                                    <ItemStyle CssClass="gridBorderColor" Width="100%" HorizontalAlign="Left" />
                                                </asp:TemplateField>
                                            </Columns>
                                            <HeaderStyle ForeColor="White" />
                                            <FooterStyle ForeColor="White" />
                                            <SelectedRowStyle Font-Bold="True" />
                                        </asp:GridView>
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
                        <asp:RoundedCornersExtender ID="pnlPlanilhaItem_RoundedCornersExtender" runat="server"
                            Enabled="True" TargetControlID="pnlPlanilhaItem">
                        </asp:RoundedCornersExtender>
                        <asp:HiddenField ID="hddSolId" runat="server" />
                        <asp:HiddenField ID="hddSolIdNovo" runat="server" />
                        <asp:HiddenField ID="hddIntId" runat="server" Value="0" />
                        <asp:HiddenField ID="hddHosId" runat="server" />
                        <asp:HiddenField ID="hddBolImpId" runat="server" />
                        <asp:Panel ID="pnlReservaAcao" CssClass="ArrendodarBorda" runat="server" Width="100%" Visible="False">
                            <table>
                                <tr>
                                    <td align="center" class="tableRowOdd">
                                        <div id="ExpandirReservaAcao" class="ColocaHand">
                                            <asp:ImageButton ID="imgBtnReservaAcao" runat="server" CausesValidation="False" />
                                            <asp:Label ID="lblReservaAcao" runat="server" Text="Solicitação" Font-Bold="True"
                                                Font-Italic="True" Font-Size="Medium"></asp:Label>
                                        </div>
                                    </td>
                                    <td align="center" class="tableRowOdd">&nbsp;
                                    </td>
                                    <td align="center" class="tableRowOdd" valign="middle">
                                        <asp:ImageButton ID="imgBtnNovaReserva" runat="server" ImageAlign="AbsMiddle" CssClass="ArrendodarBorda" ImageUrl="~/images/Reserva_add_azul.png"
                                            ToolTip="Nova Reserva" AlternateText="Nova Reserva" />
                                        <asp:Label ID="lblNovaReservaAcao" runat="server" Text="Nova Reserva" Width="50px"></asp:Label>
                                        &nbsp;<asp:ImageButton ID="imgBtnReservaAcaoVoltar" runat="server" ImageAlign="AbsMiddle"
                                            ImageUrl="~/images/VoltarAzul.png" ToolTip="Voltar Reserva - Ctrl+R" Visible="True" CausesValidation="False" AccessKey="G" />
                                        <asp:ImageButton ID="imgBtnReservaAcaoVoltarRecepcao" runat="server" ImageAlign="AbsMiddle"
                                            ImageUrl="~/images/VoltarAzul.png" ToolTip="Voltar Recepção" Visible="False"
                                            CausesValidation="False" PostBackUrl="" />
                                        <asp:Label ID="lblVoltarReserva" runat="server" Text="Voltar"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="3">
                                        <asp:Panel ID="pnlSolicitacaoAcao" runat="server">
                                            &nbsp;Iniciando em
                                            <asp:TextBox ID="txtDataInicialSolicitacao" runat="server" Width="70px" AutoPostBack="True" CssClass="ArrendodarBorda" MaxLength="10"></asp:TextBox>
                                            <asp:CalendarExtender ID="txtDataInicialSolicitacao_CalendarExtender" runat="server"
                                                Enabled="True" TargetControlID="txtDataInicialSolicitacao" Format="dd/MM/yyyy"
                                                StartDate="2014-06-13">
                                            </asp:CalendarExtender>
                                            &nbsp;Finalizando em&nbsp; &nbsp;<asp:TextBox ID="txtDataFinalSolicitacao" runat="server" Width="70px" CssClass="ArrendodarBorda" AutoPostBack="True" MaxLength="10"></asp:TextBox>
                                            <asp:CalendarExtender ID="txtDataFinalSolicitacao_CalendarExtender" runat="server"
                                                Enabled="True" TargetControlID="txtDataFinalSolicitacao" Format="dd/MM/yyyy">
                                            </asp:CalendarExtender>
                                            &nbsp;<asp:Button ID="btnHospedagemNova" runat="server" AccessKey="H" CssClass="imgLupa ArrendodarBorda"
                                                Text="    Hospedagem" ToolTip="Alt+H" ValidationGroup="ValidacaoSolicitacao" />
                                            <asp:DropDownList ID="cmbHospedagem" runat="server" Visible="False" CssClass="ArrendodarBorda">
                                                <asp:ListItem Value="0">Todos</asp:ListItem>
                                                <asp:ListItem Value="E">Especiais</asp:ListItem>
                                                <asp:ListItem Value="S">Fecomércio</asp:ListItem>
                                                <asp:ListItem Value="N">Normal</asp:ListItem>
                                                <asp:ListItem Value="D">RT</asp:ListItem>
                                                <asp:ListItem Value="R">RTM</asp:ListItem>
                                            </asp:DropDownList>
                                            <asp:Button ID="btnEmissivoNova" runat="server" AccessKey="E" CssClass="imgLupa ArrendodarBorda"
                                                Text="    Emissivo" ToolTip="Alt+E" />
                                            <asp:ValidationSummary ID="vlsSolicitacao" runat="server" ShowMessageBox="True" ShowSummary="False"
                                                ValidationGroup="ValidacaoSolicitacao" DisplayMode="List" />
                                        </asp:Panel>
                                        <asp:CollapsiblePanelExtender ID="pnlSolicitacaoAcao_CollapsiblePanelExtender" runat="server"
                                            Enabled="True" TargetControlID="pnlSolicitacaoAcao" CollapseControlID="ExpandirReservaAcao"
                                            CollapsedText="Solicitação" ExpandControlID="ExpandirReservaAcao" ExpandedText="Solicitação"
                                            TextLabelID="lblReservaAcao" CollapsedImage="~/images/expand.jpg" ExpandedImage="~/images/collapse.jpg"
                                            ImageControlID="imgBtnReservaAcao">
                                        </asp:CollapsiblePanelExtender>
                                        <asp:Panel ID="pnlResponsavelTitulo" runat="server">
                                            <table>
                                                <tr>
                                                    <td>
                                                        <div id="ExpandirResponsavelTitulo">
                                                            <asp:Image ID="imgResponsavelTitulo" runat="server" CssClass="ColocaHand" />
                                                            <asp:Label ID="lblResponsavelTitulo" runat="server" Text="Responsável" CssClass="ColocaHand"
                                                                Font-Italic="True" Font-Bold="True" Font-Size="Medium"></asp:Label>
                                                            <asp:CollapsiblePanelExtender ID="pnlResponsavelTitulo_CollapsiblePanelExtender"
                                                                runat="server" Enabled="True" TargetControlID="pnlResponsavelAcao" CollapseControlID="ExpandirResponsavelTitulo"
                                                                CollapsedText="Reserva" ExpandControlID="ExpandirResponsavelTitulo" ExpandedText="Responsável"
                                                                TextLabelID="lblResponsavelTitulo" CollapsedImage="~/images/expand.jpg" ExpandedImage="~/images/collapse.jpg"
                                                                ImageControlID="imgResponsavelTitulo">
                                                            </asp:CollapsiblePanelExtender>
                                                        </div>
                                                    </td>
                                                    <td>&nbsp;&nbsp;
                                                    </td>
                                                </tr>
                                            </table>
                                        </asp:Panel>
                                        <asp:Panel ID="pnlResponsavelAcao" CssClass="ArrendodarBorda" runat="server">
                                            <br />
                                            <asp:Label ID="lblResMatricula" runat="server" Text="Matrícula"></asp:Label>
                                            <asp:TextBox ID="txtResMatricula" runat="server" MaxLength="12" Width="110px" AutoCompleteType="Disabled"></asp:TextBox>
                                            <asp:Button ID="imgBtnResMatricula" runat="server" CssClass="btnConsultarReserva"
                                                TabIndex="-1" />
                                            <asp:MaskedEditExtender ID="txtResMatricula_MaskedEditExtender" runat="server" ClearMaskOnLostFocus="False"
                                                ClearTextOnInvalid="True" CultureAMPMPlaceholder="" CultureCurrencySymbolPlaceholder=""
                                                CultureDateFormat="" CultureDatePlaceholder="" CultureDecimalPlaceholder="" CultureThousandsPlaceholder=""
                                                CultureTimePlaceholder="" Enabled="False" InputDirection="RightToLeft" Mask="99999 999999 9"
                                                MaskType="Number" PromptCharacter=" " TargetControlID="txtResMatricula">
                                            </asp:MaskedEditExtender>
                                            <asp:Label ID="lblResCPF" runat="server" Text="CPF/CNPJ"></asp:Label>
                                            <asp:TextBox ID="txtResCPF" runat="server" MaxLength="14" Width="140px"></asp:TextBox>
                                            <asp:Button ID="imgBtnResCPF" runat="server" CssClass="btnConsultarReserva" />
                                            <asp:Label ID="lblResDtLimiteRetorno" runat="server" Text="Bloquear até"></asp:Label>
                                            <asp:TextBox ID="txtResDtLimiteRetorno" runat="server" Width="70px" AutoPostBack="True" MaxLength="10"></asp:TextBox>
                                            <asp:CalendarExtender ID="txtResDtLimiteRetorno_CalendarExtender" runat="server"
                                                Enabled="True" TargetControlID="txtResDtLimiteRetorno" Format="dd/MM/yyyy">
                                            </asp:CalendarExtender>
                                            <asp:DropDownList ID="cmbResHrLimiteRetorno" runat="server">
                                                <asp:ListItem Value="00">0</asp:ListItem>
                                                <asp:ListItem Value="01">1</asp:ListItem>
                                                <asp:ListItem Value="02">2</asp:ListItem>
                                                <asp:ListItem Value="03">3</asp:ListItem>
                                                <asp:ListItem Value="04">4</asp:ListItem>
                                                <asp:ListItem Value="05">5</asp:ListItem>
                                                <asp:ListItem Value="06">6</asp:ListItem>
                                                <asp:ListItem Value="07">7</asp:ListItem>
                                                <asp:ListItem Value="08">8</asp:ListItem>
                                                <asp:ListItem Value="09">9</asp:ListItem>
                                                <asp:ListItem Value="10">10</asp:ListItem>
                                                <asp:ListItem Value="11">11</asp:ListItem>
                                                <asp:ListItem Value="12">12</asp:ListItem>
                                                <asp:ListItem Value="13">13</asp:ListItem>
                                                <asp:ListItem Value="14">14</asp:ListItem>
                                                <asp:ListItem Value="15">15</asp:ListItem>
                                                <asp:ListItem Value="16">16</asp:ListItem>
                                                <asp:ListItem Value="17">17</asp:ListItem>
                                                <asp:ListItem Value="18">18</asp:ListItem>
                                                <asp:ListItem Value="19">19</asp:ListItem>
                                                <asp:ListItem Value="20">20</asp:ListItem>
                                                <asp:ListItem Value="21">21</asp:ListItem>
                                                <asp:ListItem Value="22">22</asp:ListItem>
                                                <asp:ListItem Value="23">23</asp:ListItem>
                                            </asp:DropDownList>
                                            <asp:Label ID="ResHrLimiteRetorno" runat="server" Text="h"></asp:Label>
                                            <asp:Button ID="btnReservaGravar" runat="server" AccessKey="G" CssClass="imgGravar"
                                                Text="    Salvar" ToolTip="Alt+G" ValidationGroup="ValidacaoReserva" Visible="False"
                                                CommandArgument="0" OnClientClick="if(confirm('Salvar agora?'))
                                {
                                    if(ctl00_conPlaHolTurismoSocial_hddProcessando.value=='S')
                                {
                                    alert('Este processo está em execução. Por favor, aguarde.');
                                    return false;
                                }
                            else
                                {
                                    ctl00_conPlaHolTurismoSocial_hddProcessando.value='S';
                                    return true;
                                }
                                }

                            else
                                {
                                    return false;
                                };" />
                                            <asp:CheckBox ID="ckbResponsavel" runat="server" Text="Exportar para integrante?" />
                                            <asp:Button ID="btnReservaCancelar" runat="server" AccessKey="E" CssClass="imgExcluir"
                                                Text="    Cancelar" ToolTip="Alt+E" Visible="False" OnClientClick="if(confirm('Confirma o cancelamento da reserva?'))
                                {
                                    if(ctl00_conPlaHolTurismoSocial_hddProcessando.value=='S')
                                {
                                    alert('Este processo está em execução. Por favor, aguarde.');
                                    return false;
                                }
                            else
                                {
                                    ctl00_conPlaHolTurismoSocial_hddProcessando.value='S';
                                    return true;
                                }
                                }

                            else
                                {
                                    return false;
                                };"
                                                CommandArgument="0" />
                                            <asp:Button ID="btnReservaReativar" runat="server" AccessKey="R" CssClass="imgGravar"
                                                Text="    Reativar" ToolTip="Alt+R" Visible="False" OnClientClick="if(confirm('Deseja reativar a reserva?'))
                                {
                                    if(ctl00_conPlaHolTurismoSocial_hddProcessando.value=='S')
                                {
                                    alert('Este processo está em execução. Por favor, aguarde.');
                                    return false;
                                }
                            else
                                {
                                    ctl00_conPlaHolTurismoSocial_hddProcessando.value='S';
                                    return true;
                                }
                                }
                            else
                                {
                                    return false;
                                };"
                                                CommandArgument="0" />

                                            <asp:Button ID="btnInformarRestituicao" runat="server" AccessKey="R" CssClass="imgCaixa"
                                                Text="    Confirmar Devolução da Reserva" Visible="False" OnClientClick="if(confirm('Confirma a finalização do processo de devolução da reserva?'))
                                {
                                    if(ctl00_conPlaHolTurismoSocial_hddProcessando.value=='S')
                                {
                                    alert('Este processo está em execução. Por favor, aguarde.');
                                    return false;
                                }
                            else
                                {
                                    ctl00_conPlaHolTurismoSocial_hddProcessando.value='S';
                                    return true;
                                }
                                }

                            else
                                {
                                    return false;
                                };"
                                                CommandArgument="0" />

                                            <asp:Button ID="btnImprimeComprovante" runat="server" AccessKey="I" CssClass="imgImprimir"
                                                Text="  Imprimir" ToolTip="Alt+I - Imprime comprovante de reserva para entrega no ato do check-in" Visible="False" OnClientClick="if(confirm('Deseja Imprimir o comprovante da reserva para entrega no ato do check-in?'))
                                {
                                    if(ctl00_conPlaHolTurismoSocial_hddProcessando.value=='S')
                                {
                                    alert('Este processo está em execução. Por favor, aguarde.');
                                    return false;
                                }
                            else
                                {
                                    ctl00_conPlaHolTurismoSocial_hddProcessando.value='S';
                                    return true;
                                }
                                }

                            else
                                {
                                    return false;
                                };"
                                                CommandArgument="0" />
                                            <asp:Button ID="BtnNovaTabela" runat="server" CssClass="imgGravar"
                                                Text="Nova tabela" Visible="False" Width="120px" ToolTip="Alterar data de inserção para 01/09/2018?"
                                                OnClientClick="if(confirm('Deseja mesmo alterar a data de inserção para 01/09/2018?'))
                                {
                                    if(ctl00_conPlaHolTurismoSocial_hddProcessando.value=='S')
                                {
                                    alert('Este processo está em execução. Por favor, aguarde.');
                                    return false;
                                }
                            else
                                {
                                    ctl00_conPlaHolTurismoSocial_hddProcessando.value='S';
                                    return true;
                                }
                                }

                            else
                                {
                                    return false;
                                };" />
                                            <p>
                                                <%--<asp:RegularExpressionValidator ID="revtxtResMatricula" runat="server" ErrorMessage="Matrícula informada não é válida."
                                                    ControlToValidate="txtResMatricula" Display="Dynamic" SetFocusOnError="True"
                                                    ValidationExpression="\d{4} \d{6} \d{1}" ValidationGroup="ValidacaoReserva">*</asp:RegularExpressionValidator>--%>
                                                <%--<asp:RequiredFieldValidator ID="rfvtxtResCPF" runat="server" ErrorMessage="CPF não foi preenchido."
                                                    ControlToValidate="txtResCPF" Display="Dynamic" SetFocusOnError="True" ValidationGroup="ValidacaoReserva"
                                                    Enabled="False">*</asp:RequiredFieldValidator>--%><asp:Label ID="lblResNome" runat="server"
                                                        Text="Nome"></asp:Label>
                                                <asp:RequiredFieldValidator ID="rfvtxtResNome" runat="server" ControlToValidate="txtResNome"
                                                    Display="Dynamic" ErrorMessage="Responsável não foi preenchido." SetFocusOnError="True"
                                                    ValidationGroup="ValidacaoReserva">*</asp:RequiredFieldValidator>
                                                <asp:TextBox ID="txtResNome" runat="server" MaxLength="100" Width="300px"></asp:TextBox>
                                                &nbsp;<asp:Label ID="lblOrgId" runat="server" Text="UO"></asp:Label>
                                                <asp:DropDownList ID="cmbOrgId" runat="server" AutoPostBack="True">
                                                </asp:DropDownList>
                                                <asp:Label ID="lblResContato" runat="server" Text="Contato de emergência"></asp:Label>
                                                <asp:TextBox ID="txtResContato" runat="server" MaxLength="50" Width="250px"></asp:TextBox>
                                            </p>
                                            <p>
                                                <asp:Label ID="lblResEmail" runat="server" Text="Email"></asp:Label>
                                                <label style="color: red">
                                                    *</label><%--<asp:RequiredFieldValidator ID="rfvtxtResEmail" runat="server" ErrorMessage="Email não foi preenchido."
                                                    ControlToValidate="txtResEmail" Display="Dynamic" SetFocusOnError="True" ValidationGroup="ValidacaoReserva">*</asp:RequiredFieldValidator>--%><asp:TextBox ID="txtResEmail" runat="server" MaxLength="40" Width="200px" AutoPostBack="True"></asp:TextBox>
                                                <asp:Label ID="lblResNascimento" runat="server" Text="Nascimento"></asp:Label>
                                                <asp:TextBox ID="txtResDtNascimento" runat="server" Width="70px" AutoPostBack="True" MaxLength="10"></asp:TextBox>
                                                <asp:Label ID="lblResSexo" runat="server" Text="Sexo"></asp:Label>
                                                <asp:DropDownList ID="cmbResSexo" runat="server">
                                                    <asp:ListItem Value="F">Feminino</asp:ListItem>
                                                    <asp:ListItem Value="M">Masculino</asp:ListItem>
                                                </asp:DropDownList>
                                                <asp:Label ID="lblResCatId" runat="server" Text="Categoria"></asp:Label>
                                                <asp:DropDownList ID="cmbResCatId" runat="server">
                                                </asp:DropDownList>
                                                <asp:CalendarExtender ID="txtResDtNascimento_CalendarExtender" runat="server" Enabled="True"
                                                    TargetControlID="txtResDtNascimento" Format="dd/MM/yyyy">
                                                </asp:CalendarExtender>
                                                <asp:Label ID="lblResCatCobranca" runat="server" Text="Categoria Cobrança"></asp:Label>
                                                <asp:DropDownList ID="cmbResCatCobranca" runat="server">
                                                    <asp:ListItem Value="1">Comerciário</asp:ListItem>
                                                    <asp:ListItem Value="3">Conveniado</asp:ListItem>
                                                    <asp:ListItem Value="4">Usuário</asp:ListItem>
                                                </asp:DropDownList>
                                            </p>
                                            <p>
                                                <label style="color: red">*</label><asp:Image ID="imgResFone" runat="server" ToolTip="Obrigatório a inserção de um número de telefone." ImageUrl="~/images/Telefone.png" ImageAlign="Middle"
                                                    TabIndex="-1" />
                                                <asp:Label ID="lblResFoneComercial" runat="server" Text="Comercial"></asp:Label>
                                                <asp:TextBox ID="txtResFoneComercial" runat="server" MaxLength="15" Width="110px"></asp:TextBox>
                                                <asp:Label ID="lblResFoneResidencial" runat="server" Text="Residencial"></asp:Label>
                                                <asp:TextBox ID="txtResFoneResidencial" runat="server" MaxLength="15" Width="110px"></asp:TextBox>
                                                <asp:Label ID="lblResCelular" runat="server" Text="Celular"></asp:Label>
                                                <asp:TextBox ID="txtResCelular" runat="server" MaxLength="15" Width="110px"></asp:TextBox>
                                                <asp:Label ID="lblResFax" runat="server" Text="Fax"></asp:Label>
                                                <asp:TextBox ID="txtResFax" runat="server" MaxLength="15" Width="110px"></asp:TextBox>
                                            </p>
                                            <p>
                                                <asp:Label ID="lblResLogradouro" runat="server" Text="Logradouro"></asp:Label>
                                                <label style="color: red">
                                                    *</label><asp:TextBox ID="txtResLogradouro" runat="server" Width="200px" MaxLength="40"></asp:TextBox>
                                                <asp:Label ID="lblResNumero" runat="server" Text="Número"></asp:Label>
                                                <asp:TextBox ID="txtResNumero" runat="server" MaxLength="10" Width="50px"></asp:TextBox>
                                                <asp:Label ID="lblResQuadra" runat="server" Text="Quadra"></asp:Label>
                                                <asp:TextBox ID="txtResQuadra" runat="server" MaxLength="10" Width="50px"></asp:TextBox>
                                                <asp:Label ID="lblResLote" runat="server" Text="Lote"></asp:Label>
                                                <asp:TextBox ID="txtResLote" runat="server" MaxLength="10" Width="50px"></asp:TextBox>
                                                <asp:Label ID="lblResComplemento" runat="server" Text="Complemento"></asp:Label>
                                                <asp:TextBox ID="txtResComplemento" runat="server" MaxLength="40" Width="100px"></asp:TextBox>
                                            </p>
                                            <p>
                                                <asp:Label ID="lblResBairro" runat="server" Text="Bairro"></asp:Label>
                                                <label style="color: red">
                                                    *</label><asp:TextBox ID="txtResBairro" runat="server" MaxLength="40" Width="100px"></asp:TextBox>
                                                <asp:Label ID="lblResCep" runat="server" Text="CEP"></asp:Label>
                                                <%--<asp:RequiredFieldValidator ID="rfvtxtResCep" runat="server" ErrorMessage="Cep não foi preenchido."
                                                    ControlToValidate="txtResCep" Display="Dynamic" SetFocusOnError="True" ValidationGroup="ValidacaoReserva">*</asp:RequiredFieldValidator>--%>
                                                <asp:TextBox ID="txtResCep" runat="server" MaxLength="9" Width="80px"></asp:TextBox>
                                                <asp:Label ID="lblEstId" runat="server" Text="Estado/País"></asp:Label>
                                                <label style="color: red">
                                                    *</label><asp:DropDownList ID="cmbEstId" runat="server" AutoPostBack="True">
                                                    </asp:DropDownList>
                                                <asp:Label ID="lblResCidade" runat="server" Text="Cidade"></asp:Label>
                                                <asp:RequiredFieldValidator ID="rfvtxtResCidade" runat="server" ControlToValidate="txtResCidade"
                                                    Display="Dynamic" ErrorMessage="Cidade não foi preenchida." SetFocusOnError="True"
                                                    Text="*" ValidationGroup="ValidacaoReserva"></asp:RequiredFieldValidator>
                                                <asp:DropDownList ID="cmbResCidade" runat="server" AutoPostBack="True">
                                                </asp:DropDownList>
                                                <asp:TextBox ID="txtResCidade" runat="server" MaxLength="40" Visible="False" Width="200px"></asp:TextBox>
                                            </p>
                                            <p />
                                            <asp:Label ID="lblResSalario" runat="server" Text="Faixa"></asp:Label>
                                            <asp:DropDownList ID="cmbResSalario" runat="server">
                                                <asp:ListItem Value="1">1 Salário</asp:ListItem>
                                                <asp:ListItem Value="2">2 Salários</asp:ListItem>
                                                <asp:ListItem Value="3">3 Salários</asp:ListItem>
                                                <asp:ListItem Value="4">4 Salários</asp:ListItem>
                                                <asp:ListItem Value="5">5 Salários</asp:ListItem>
                                                <asp:ListItem Value="6">6 Salários</asp:ListItem>
                                                <asp:ListItem Value="7">7 Salários</asp:ListItem>
                                                <asp:ListItem Value="8">8 Salários</asp:ListItem>
                                                <asp:ListItem Value="9">9 Salários</asp:ListItem>
                                                <asp:ListItem Value="10">10 Salários</asp:ListItem>
                                                <asp:ListItem Value="11">Acima de 10</asp:ListItem>
                                                <asp:ListItem Value="12">Sem renda</asp:ListItem>
                                            </asp:DropDownList>
                                            <asp:Label ID="lblResEscolaridade" runat="server" Text="Escolaridade"></asp:Label>
                                            <asp:DropDownList ID="cmbResEscolaridade" runat="server">
                                                <asp:ListItem Value="1">Sem Escolaridade</asp:ListItem>
                                                <asp:ListItem Value="2">Analfabeto</asp:ListItem>
                                                <asp:ListItem Value="3">Alfabetizado</asp:ListItem>
                                                <asp:ListItem Value="4">Ensino Fundamental Completo</asp:ListItem>
                                                <asp:ListItem Value="5">Ensino Fundamental Incompleto</asp:ListItem>
                                                <asp:ListItem Value="6">Ensino Médio Completo</asp:ListItem>
                                                <asp:ListItem Value="7">Ensino Médio Incompleto</asp:ListItem>
                                                <asp:ListItem Value="8">Superior Completo</asp:ListItem>
                                                <asp:ListItem Value="9">Superior Incompleto</asp:ListItem>
                                                <asp:ListItem Value="10">Pós-Graduação Completa</asp:ListItem>
                                                <asp:ListItem Value="11">Pós-Graduação Incompleta</asp:ListItem>
                                                <asp:ListItem Value="12">Ensino Técnico Completo</asp:ListItem>
                                                <asp:ListItem Value="13">Ensino Técnico Incompleto</asp:ListItem>
                                                <asp:ListItem Value="0">Sem Informação</asp:ListItem>
                                            </asp:DropDownList>
                                            <asp:Label ID="lblResEstadoCivil" runat="server" Text="Estado Civil"></asp:Label>
                                            <asp:DropDownList ID="cmbResEstadoCivil" runat="server">
                                                <asp:ListItem Value="0">Solteiro</asp:ListItem>
                                                <asp:ListItem Value="1">Casado</asp:ListItem>
                                                <asp:ListItem Value="2">Viúvo</asp:ListItem>
                                                <asp:ListItem Value="3">Divorciado</asp:ListItem>
                                                <asp:ListItem Value="4">Separado</asp:ListItem>
                                                <asp:ListItem Value="5">União Estável</asp:ListItem>

                                            </asp:DropDownList>
                                            Documento de identificação<asp:DropDownList ID="cmbResDocIdentificacao" runat="server">
                                                <asp:ListItem>RG</asp:ListItem>
                                                <asp:ListItem Value="CN">Cert Nascimento</asp:ListItem>
                                                <asp:ListItem Value="CC">Cert Casamento</asp:ListItem>
                                                <asp:ListItem Value="OU">Outro</asp:ListItem>
                                            </asp:DropDownList>
                                            <asp:TextBox ID="txtResDocIdentificacao" runat="server" MaxLength="20" Width="130px"></asp:TextBox>
                                            <p>
                                                <asp:Label ID="lblResMemorando" runat="server" Text="Memorando"></asp:Label>
                                                <asp:TextBox ID="txtResMemorando" runat="server" MaxLength="100" Width="200px"></asp:TextBox>
                                                <asp:Label ID="lblResEmissor" runat="server" Text="Emissor"></asp:Label>
                                                <asp:DropDownList ID="cmbResEmissor" runat="server">
                                                    <asp:ListItem Value="0">_</asp:ListItem>
                                                    <asp:ListItem Value="1">GP</asp:ListItem>
                                                    <asp:ListItem Value="2">DR</asp:ListItem>
                                                </asp:DropDownList>

                                                <asp:Label ID="lblResHoraSaida" runat="server" Text="Check-out" Visible="False"></asp:Label>
                                                <asp:DropDownList ID="cmbReservaHoraSaida" runat="server" Visible="False">
                                                    <asp:ListItem Value="10">10 h</asp:ListItem>
                                                    <asp:ListItem Value="12">12 h</asp:ListItem>
                                                </asp:DropDownList>
                                                &nbsp;
                                                <asp:CheckBox ID="ckbOrganizadoSESC" runat="server" Text="Organizado SESC Goiás" CssClass="ColocaHand" />
                                                &nbsp;
                                                <asp:CheckBox ID="chkRecreandoEscolar" Text="Recreando escolar" ToolTip="Se selecionado, não irá solicitar a aplicação de cartão de consumo no ato do check in." runat="server" CssClass="ColocaHand" />
                                            </p>
                                            <p>
                                                <asp:Label ID="lblResObs" runat="server" Text="Observação"></asp:Label>
                                                <asp:TextBox ID="txtResObs" runat="server" TextMode="MultiLine" MaxLength="200" Width="400px"></asp:TextBox>
                                                &nbsp;<asp:ImageButton ID="imgRelatosObs" runat="server" CssClass="ColocaHand" ImageUrl="~/images/verObservacao.png" ToolTip="Clique para visualizar observações" />
                                            </p>
                                            <asp:Panel ID="pnlGrupo" runat="server" Visible="False">
                                                <asp:Label ID="lblResDtGrupoConfirmacao" runat="server" Text="Confirmar até"></asp:Label>
                                                <asp:TextBox ID="txtResDtGrupoConfirmacao" runat="server" Width="70px" ValidationGroup="ValidacaoReserva" MaxLength="10"></asp:TextBox>
                                                <asp:CalendarExtender ID="txtResDtGrupoConfirmacao_CalendarExtender" runat="server"
                                                    Enabled="True" TargetControlID="txtResDtGrupoConfirmacao" Format="dd/MM/yyyy">
                                                </asp:CalendarExtender>
                                                <asp:Image ID="imgResGrupoConfirmacao" runat="server" ImageUrl="~/images/nok.gif"
                                                    ToolTip="Não confirmado!" TabIndex="-1" />
                                                <asp:Label ID="lblResDtGrupoPgtoSinal" runat="server" Text="Pagar sinal até"></asp:Label>
                                                <asp:TextBox ID="txtResDtGrupoPgtoSinal" runat="server" Width="70px" MaxLength="10"></asp:TextBox>
                                                <asp:CalendarExtender ID="txtResDtGrupoPgtoSinal_CalendarExtender" runat="server"
                                                    Enabled="True" TargetControlID="txtResDtGrupoPgtoSinal" Format="dd/MM/yyyy">
                                                </asp:CalendarExtender>
                                                <asp:Label ID="lblResDtGrupoListagem" runat="server" Text="Digitar até"></asp:Label>
                                                <asp:TextBox ID="txtResDtGrupoListagem" runat="server" Width="70px" MaxLength="10"></asp:TextBox>
                                                <asp:CalendarExtender ID="txtResDtGrupoListagem_CalendarExtender" runat="server"
                                                    Enabled="True" TargetControlID="txtResDtGrupoListagem" Format="dd/MM/yyyy">
                                                </asp:CalendarExtender>
                                                <asp:Label ID="lblResDtGrupoPgtoTotal" runat="server" Text="Quitar até"></asp:Label>
                                                <asp:TextBox ID="txtResDtGrupoPgtoTotal" runat="server" Width="70px" MaxLength="10"></asp:TextBox>
                                                <asp:CalendarExtender ID="txtResDtGrupoPgtoTotal_CalendarExtender" runat="server"
                                                    Enabled="True" TargetControlID="txtResDtGrupoPgtoTotal" Format="dd/MM/yyyy">
                                                </asp:CalendarExtender>
                                                <asp:Label ID="lblResIdWeb" runat="server" Text="Usuário Internet"></asp:Label>
                                                <asp:DropDownList ID="cmbResIdWeb" runat="server">
                                                </asp:DropDownList>
                                                <asp:Button ID="btnResEmailGrupo" runat="server" CssClass="imgEmail" Text="     Salvar e Enviar Email"
                                                    ToolTip="Enviar e-mail solicitando a confirmação do Grupo" ValidationGroup="ValidacaoReserva"
                                                    OnClientClick="if(confirm('Salvar e enviar e-mail?'))
                                {
                                    if(ctl00_conPlaHolTurismoSocial_hddProcessando.value=='S')
                                {
                                    alert('Este processo está em execução. Por favor, aguarde.');
                                    return false;
                                }
                            else
                                {
                                    ctl00_conPlaHolTurismoSocial_hddProcessando.value='S';
                                    return true;
                                }
                                }

                            else
                                {
                                    return false;
                                };" />
                                                &nbsp;<asp:ImageButton ID="imgBoletoSinal" runat="server" ImageUrl="~/images/BoletoSinal.png" ToolTip="Imprime Boleto 50%" Height="28px" />
                                            </asp:Panel>
                                            <asp:Panel ID="pnlDestinoGrupo" runat="server" Visible="False">
                                                <asp:Label ID="lblDestino" runat="server" Text="Destino"></asp:Label>
                                                <asp:DropDownList ID="cmbDestino" runat="server" AutoPostBack="True">
                                                </asp:DropDownList>
                                                <asp:Label ID="lblDestinoEstado" runat="server" Text="Estado"></asp:Label>
                                                <asp:DropDownList ID="cmbDestinoEstado" runat="server" AutoPostBack="True">
                                                </asp:DropDownList>
                                                <asp:Label ID="lblDestinoCidade" runat="server" Text="Cidade"></asp:Label>
                                                <asp:DropDownList ID="cmbDestinoCidade" runat="server">
                                                </asp:DropDownList>
                                                <asp:Label ID="lblDestinoHotel" runat="server" Text="Hotel"></asp:Label>
                                                <asp:TextBox ID="txtResHoteDestino" runat="server" Width="200px"></asp:TextBox>
                                                <asp:Label ID="lblLocalSaida" runat="server" Text="Local e Horário de Saída"></asp:Label>
                                                <asp:TextBox ID="txtResLocalSaida" runat="server" MaxLength="200" Width="300px"></asp:TextBox>
                                                <asp:Label ID="lblHorario" runat="server" Text="Chegada"></asp:Label>
                                                <asp:DropDownList ID="cmbResHoraSaida" runat="server">
                                                    <asp:ListItem Value="0">0 h</asp:ListItem>
                                                    <asp:ListItem Value="1">1 h</asp:ListItem>
                                                    <asp:ListItem Value="2">2 h</asp:ListItem>
                                                    <asp:ListItem Value="3">3 h</asp:ListItem>
                                                    <asp:ListItem Value="4">4 h</asp:ListItem>
                                                    <asp:ListItem Value="5">5 h</asp:ListItem>
                                                    <asp:ListItem Value="6">6 h</asp:ListItem>
                                                    <asp:ListItem Value="7">7 h</asp:ListItem>
                                                    <asp:ListItem Value="8">8 h</asp:ListItem>
                                                    <asp:ListItem Value="9">9 h</asp:ListItem>
                                                    <asp:ListItem Value="10">10 h</asp:ListItem>
                                                    <asp:ListItem Value="11">11 h</asp:ListItem>
                                                    <asp:ListItem Value="12">12 h</asp:ListItem>
                                                    <asp:ListItem Value="13">13 h</asp:ListItem>
                                                    <asp:ListItem Value="14">14 h</asp:ListItem>
                                                    <asp:ListItem Value="15">15 h</asp:ListItem>
                                                    <asp:ListItem Value="16">16 h</asp:ListItem>
                                                    <asp:ListItem Value="17">17 h</asp:ListItem>
                                                    <asp:ListItem Value="18">18 h</asp:ListItem>
                                                    <asp:ListItem Value="19">19 h</asp:ListItem>
                                                    <asp:ListItem Value="20">20 h</asp:ListItem>
                                                    <asp:ListItem Value="21">21 h</asp:ListItem>
                                                    <asp:ListItem Value="22">22 h</asp:ListItem>
                                                    <asp:ListItem Value="23">23 h</asp:ListItem>
                                                </asp:DropDownList>
                                                <asp:Label ID="lblPratoRapido0" runat="server" Text="Refeição" Visible="False"></asp:Label>
                                                <asp:DropDownList ID="cmbPratoRapido0" runat="server" Visible="False">
                                                </asp:DropDownList>
                                                <asp:Label ID="lblResValorDesconto" runat="server" Text="Desconto %" Visible="False"></asp:Label>
                                                <asp:RangeValidator ID="rvlResValorDesconto" runat="server" ControlToValidate="txtResValorDesconto"
                                                    Display="Dynamic" ErrorMessage="Percentual entre 0 e 100" MaximumValue="100"
                                                    MinimumValue="0" SetFocusOnError="True" Type="Integer" ValidationGroup="ValidacaoReserva">*</asp:RangeValidator>
                                                <asp:TextBox ID="txtResValorDesconto" runat="server" Columns="3" MaxLength="3" Visible="False">0</asp:TextBox>
                                                <asp:MaskedEditExtender ID="txtResValorDesconto_MaskedEditExtender" runat="server"
                                                    AutoComplete="False" CultureAMPMPlaceholder="" CultureCurrencySymbolPlaceholder=""
                                                    CultureDateFormat="" CultureDatePlaceholder="" CultureDecimalPlaceholder="" CultureThousandsPlaceholder=""
                                                    CultureTimePlaceholder="" Enabled="True" Mask="999" MaskType="Number" TargetControlID="txtResValorDesconto">
                                                </asp:MaskedEditExtender>
                                                <asp:Button ID="btnReservaCalculo" runat="server" CssClass="imgCalculo" Text="     Planilha de Custo"
                                                    ValidationGroup="ValidacaoReserva" Visible="False" />
                                            </asp:Panel>
                                            <asp:ValidationSummary ID="vlsReserva" runat="server" DisplayMode="List" ShowMessageBox="True"
                                                ShowSummary="False" ValidationGroup="ValidacaoReserva" />
                                            </p>
                                        </asp:Panel>
                                        <asp:Panel ID="pnlAcomodacaoTitulo" runat="server" Visible="False">
                                            <table style="width: 100%;">
                                                <tr>
                                                    <td>
                                                        <div id="ExpandirAcomodacaoTitulo" class="ColocaHand">
                                                            <asp:ImageButton ID="imgAcomodacaoTitulo" runat="server" />
                                                            <asp:Label ID="lblAcomodacaoTitulo" runat="server" Text="Acomodação" Font-Italic="True"
                                                                Font-Bold="True" Font-Size="Medium"></asp:Label>
                                                            <asp:CollapsiblePanelExtender ID="pnlAcomodacaoTitulo_CollapsiblePanelExtender" runat="server"
                                                                Enabled="True" TargetControlID="pnlSolicitacaoSelecionada" CollapseControlID="ExpandirAcomodacaoTitulo"
                                                                CollapsedText="Acomodação" ExpandControlID="ExpandirAcomodacaoTitulo" ExpandedText="Acomodação"
                                                                TextLabelID="lblAcomodacaoTitulo" CollapsedImage="~/images/expand.jpg" ExpandedImage="~/images/collapse.jpg"
                                                                ImageControlID="imgAcomodacaoTitulo">
                                                            </asp:CollapsiblePanelExtender>
                                                        </div>
                                                    </td>
                                                </tr>
                                            </table>
                                        </asp:Panel>
                                        <asp:Panel ID="pnlSolicitacaoSelecionada" runat="server">
                                            <br />
                                            <asp:Panel ID="pnlReservaMsg" runat="server" Visible="False">
                                                <table>
                                                    <caption>
                                                        <br />
                                                        <br />
                                                        <tr>
                                                            <td>
                                                                <asp:Label ID="lblMsgReservaAcao" runat="server" Text="Atenção! Não existem disponibilidades no período solicitado."></asp:Label>
                                                            </td>
                                                        </tr>
                                                    </caption>
                                                </table>
                                            </asp:Panel>
                                            <asp:GridView ID="gdvReserva8" runat="server" AutoGenerateColumns="False" CellPadding="4"
                                                DataKeyNames="acmId,solId,acmStatus,solDataIni,solDataFim,solUsuario,acmFederacao"
                                                HorizontalAlign="Center" Width="99%">
                                                <AlternatingRowStyle CssClass="tableRowOdd" />
                                                <RowStyle HorizontalAlign="Center" />
                                                <Columns>
                                                    <asp:TemplateField HeaderText="Acomodação selecionada">
                                                        <ItemTemplate>
                                                            <asp:LinkButton ID="lnkAcomodacao" runat="server" CommandName="select" Text='<%# Bind("acmDescricao") %>'
                                                                ToolTip="Clique na acomodação para excluir" OnClientClick="return confirm('Excluir acomodação?')"></asp:LinkButton>
                                                        </ItemTemplate>
                                                        <FooterStyle CssClass="formLabel" HorizontalAlign="Right" />
                                                        <HeaderStyle BorderWidth="0px" CssClass="formRuler" />
                                                        <ItemStyle CssClass="gridBorderColor" HorizontalAlign="Left" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Começando">
                                                        <ItemTemplate>
                                                            <asp:LinkButton ID="lnkDiaInicial" runat="server" CommandName="select" Text='<%# Bind("solDiaIni") %>'
                                                                ToolTip="Clique na acomodação para excluir" OnClientClick="return confirm('Excluir acomodação?')"></asp:LinkButton>
                                                        </ItemTemplate>
                                                        <FooterStyle CssClass="formLabel" HorizontalAlign="Center" />
                                                        <HeaderStyle BorderWidth="0px" CssClass="formRuler" />
                                                        <ItemStyle CssClass="gridBorderColor" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Dia">
                                                        <ItemTemplate>
                                                            <asp:ImageButton ID="imgBtnDtCheckInMenos" runat="server" CssClass="ColocaHand" ImageAlign="AbsMiddle"
                                                                ImageUrl="~/images/minus.gif" ToolTip="Antecipa o período inicial em 1 dia" OnClick="imgBtnDtCheckInMenos_Click"
                                                                AccessKey="1" OnClientClick="return confirm('Deseja antecipar o período inicial em 1 dia?')" />
                                                            <asp:LinkButton ID="lnkDtInicial" runat="server" CommandName="select" Text='<%# Bind("solDataIni") %>'
                                                                ToolTip="Clique na acomodação para excluir" OnClientClick="return confirm('Excluir acomodação?')"></asp:LinkButton>
                                                            <asp:ImageButton ID="imgBtnDtCheckInMais" runat="server" CssClass="ColocaHand" ImageAlign="AbsMiddle"
                                                                ImageUrl="~/images/plus.gif" ToolTip="Prorroga o período inicial em 1 dia" AccessKey="2" OnClick="imgBtnDtCheckInMenos_Click"
                                                                OnClientClick="return confirm('Deseja prorrogar o período inicial em 1 dia?')" />
                                                        </ItemTemplate>
                                                        <HeaderStyle BorderWidth="0px" CssClass="formRuler" />
                                                        <ItemStyle CssClass="gridBorderColor" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Terminando">
                                                        <ItemTemplate>
                                                            <asp:LinkButton ID="lnkDiaFinal" runat="server" CommandName="select" Text='<%# Bind("solDiaFim") %>'
                                                                ToolTip="Clique na acomodação para excluir" OnClientClick="return confirm('Excluir acomodação?')"></asp:LinkButton>
                                                        </ItemTemplate>
                                                        <FooterStyle CssClass="formLabel" HorizontalAlign="Center" />
                                                        <HeaderStyle BorderWidth="0px" CssClass="formRuler" />
                                                        <ItemStyle CssClass="gridBorderColor" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Dia">
                                                        <ItemTemplate>
                                                            <asp:ImageButton ID="imgBtnDtCheckOutMenos" runat="server" CssClass="ColocaHand"
                                                                ImageAlign="AbsMiddle" ImageUrl="~/images/minus.gif" ToolTip="Antecipa o período final em 1 dia"
                                                                AccessKey="3" OnClick="imgBtnDtCheckInMenos_Click" OnClientClick="return confirm('Deseja antecipar o período final em 1 dia?')" />
                                                            <asp:LinkButton ID="lnkDtFinal" runat="server" CommandName="select" Text='<%# Bind("solDataFim") %>'
                                                                ToolTip="Clique na acomodação para excluir" OnClientClick="return confirm('Excluir acomodação?')"></asp:LinkButton>
                                                            <asp:ImageButton ID="imgBtnDtCheckOutMais" runat="server" CsssClass="ColocaHand" ImageUrl="~/images/plus.gif"
                                                                ToolTip="Prorroga o período final em 1 dia" ImageAlign="AbsMiddle" AccessKey="4" OnClick="imgBtnDtCheckInMenos_Click"
                                                                OnClientClick="return confirm('Deseja prorrogar o período final em 1 dia?')" />
                                                        </ItemTemplate>
                                                        <FooterStyle CssClass="formLabel" HorizontalAlign="Center" />
                                                        <HeaderStyle BorderWidth="0px" CssClass="formRuler" />
                                                        <ItemStyle CssClass="gridBorderColor" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Servidor">
                                                        <ItemTemplate>
                                                            <asp:LinkButton ID="lnkServidor" runat="server" CommandName="select" ToolTip="Clique na acomodação para excluir"
                                                                OnClientClick="return confirm('Excluir acomodação?')"></asp:LinkButton>
                                                        </ItemTemplate>
                                                        <FooterStyle CssClass="formLabel" HorizontalAlign="Center" />
                                                        <HeaderStyle BorderWidth="0px" CssClass="formRuler" />
                                                        <ItemStyle CssClass="gridBorderColor" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Diárias" Visible="False">
                                                        <ItemTemplate>
                                                            <asp:LinkButton ID="lnkDiaria" runat="server" Text=''></asp:LinkButton>
                                                        </ItemTemplate>
                                                        <FooterStyle CssClass="formLabel" HorizontalAlign="Center" />
                                                        <HeaderStyle BorderWidth="0px" CssClass="formRuler" />
                                                        <ItemStyle CssClass="gridBorderColor" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Hóspedes">
                                                        <ItemTemplate>
                                                            <asp:LinkButton ID="lnkHospedes" runat="server" CommandName="select" Text='<%# Bind("hospedes") %>'
                                                                ToolTip="Clique na acomodação para excluir" OnClientClick="return confirm('Excluir acomodação?')"></asp:LinkButton>
                                                        </ItemTemplate>
                                                        <FooterStyle CssClass="formLabel" HorizontalAlign="Center" />
                                                        <HeaderStyle BorderWidth="0px" CssClass="formRuler" />
                                                        <ItemStyle CssClass="gridBorderColor" />
                                                    </asp:TemplateField>
                                                </Columns>
                                                <HeaderStyle ForeColor="White" HorizontalAlign="Center" />
                                                <SelectedRowStyle Font-Bold="True" />
                                            </asp:GridView>
                                            <br />
                                            <asp:GridView ID="gdvReserva6" runat="server" AutoGenerateColumns="False" CellPadding="4"
                                                DataKeyNames="acmId,dtInicial,hrInicial,dtFinal,hrFinal,acmStatus,aptosLivre,acmDescricao,acmLimpo,diarias,apaId,apaDesc,acmFederacao"
                                                Width="99%" HorizontalAlign="Center">
                                                <AlternatingRowStyle CssClass="tableRowOdd" />
                                                <RowStyle HorizontalAlign="Center" />
                                                <Columns>
                                                    <asp:TemplateField HeaderText="Acomodação disponível para bloqueio">
                                                        <ItemTemplate>
                                                            <asp:LinkButton ID="lnkAcomodacao" runat="server" CommandName="select" Text='<%# Bind("acmDescricao") %>'
                                                                ToolTip="Selecionar acomodação"></asp:LinkButton>
                                                            <asp:ImageButton ID="imgBtnApto" runat="server" ImageAlign="Middle" Visible="False" />
                                                        </ItemTemplate>
                                                        <FooterStyle CssClass="formLabel" />
                                                        <HeaderStyle BorderWidth="0px" CssClass="formRuler" />
                                                        <ItemStyle CssClass="gridBorderColor" HorizontalAlign="Left" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Qtde/Limpo/Livre">
                                                        <HeaderTemplate>
                                                            Qtde/Limpo/Livre
                                                            <asp:ImageButton ID="imgSobeTodos" runat="server" ImageUrl="~/images/BloquearApto.png" OnClick="imgSobeTodos_Click" ToolTip="Bloqueia todos os apartamentos selecionados" OnClientClick="return confirm('Deseja bloquear todos os apartamentos selecionados?')" />
                                                        </HeaderTemplate>
                                                        <ItemTemplate>
                                                            <asp:TextBox ID="txtQtdeAcomodacao" runat="server" MaxLength="2" Width="20px">1</asp:TextBox>
                                                            <asp:Label ID="lblLimpoAcomodacao" runat="server" Text=" / 1"></asp:Label>
                                                            <asp:Label ID="lblQtdeAcomodacao" runat="server" Text=" / 1"></asp:Label>
                                                            &nbsp;<asp:CheckBox ID="chkSobeApto" runat="server" ToolTip="Selecione se desejar bloquear vários apartamentos de uma só vez." />
                                                        </ItemTemplate>
                                                        <FooterStyle CssClass="formLabel" HorizontalAlign="Center" />
                                                        <HeaderStyle BorderWidth="0px" CssClass="formRuler" />
                                                        <ItemStyle CssClass="gridBorderColor" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Começando">
                                                        <ItemTemplate>
                                                            <asp:LinkButton ID="lnkDiaInicial" runat="server" CommandName="select" Text='<%# Bind("diaInicial") %>'
                                                                ToolTip="Selecionar acomodação"></asp:LinkButton>
                                                        </ItemTemplate>
                                                        <FooterStyle CssClass="formLabel" HorizontalAlign="Center" />
                                                        <HeaderStyle BorderWidth="0px" CssClass="formRuler" />
                                                        <ItemStyle CssClass="gridBorderColor" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Dia">
                                                        <ItemTemplate>
                                                            <asp:LinkButton ID="lnkDtInicial" runat="server" CommandName="select" Text='<%# Bind("dtInicial") %>'
                                                                ToolTip="Selecionar acomodação"></asp:LinkButton>
                                                        </ItemTemplate>
                                                        <HeaderStyle BorderWidth="0px" CssClass="formRuler" />
                                                        <ItemStyle CssClass="gridBorderColor" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Terminando">
                                                        <ItemTemplate>
                                                            <asp:LinkButton ID="lnkDiaFinal" runat="server" CommandName="select" Text='<%# Bind("diaFinal") %>'
                                                                ToolTip="Selecionar acomodação"></asp:LinkButton>
                                                        </ItemTemplate>
                                                        <FooterStyle CssClass="formLabel" HorizontalAlign="Center" />
                                                        <HeaderStyle BorderWidth="0px" CssClass="formRuler" />
                                                        <ItemStyle CssClass="gridBorderColor" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Dia">
                                                        <ItemTemplate>
                                                            <asp:LinkButton ID="lnkDtFinal" runat="server" CommandName="select" Text='<%# Bind("dtFinal") %>'
                                                                ToolTip="Selecionar acomodação"></asp:LinkButton>
                                                        </ItemTemplate>
                                                        <FooterStyle CssClass="formLabel" HorizontalAlign="Center" />
                                                        <HeaderStyle BorderWidth="0px" CssClass="formRuler" />
                                                        <ItemStyle CssClass="gridBorderColor" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Diárias">
                                                        <ItemTemplate>
                                                            <asp:LinkButton ID="lnkDiaria" runat="server" CommandName="select" Text='<%# Bind("diarias") %>'
                                                                ToolTip="Selecionar acomodação"></asp:LinkButton>
                                                        </ItemTemplate>
                                                        <FooterStyle CssClass="formLabel" HorizontalAlign="Center" />
                                                        <HeaderStyle BorderWidth="0px" CssClass="formRuler" />
                                                        <ItemStyle CssClass="gridBorderColor" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Hóspedes" Visible="False">
                                                        <ItemTemplate>
                                                            <asp:LinkButton ID="lnkHospedes" runat="server" CommandName="select" ToolTip="Clique na acomodação para excluir"
                                                                OnClientClick="return confirm('Excluir acomodação?')"></asp:LinkButton>
                                                        </ItemTemplate>
                                                        <FooterStyle CssClass="formLabel" HorizontalAlign="Center" />
                                                        <HeaderStyle BorderWidth="0px" CssClass="formRuler" />
                                                        <ItemStyle CssClass="gridBorderColor" />
                                                    </asp:TemplateField>
                                                </Columns>
                                                <HeaderStyle ForeColor="White" />
                                                <SelectedRowStyle Font-Bold="True" />
                                            </asp:GridView>
                                            <asp:Panel ID="pnlDisponibilidadeAlternativaAux" runat="server">
                                                <div id="ExpandirDisponibilidadeAlternativa" class="ColocaHand">
                                                    <asp:ImageButton ID="imgDisponibilidadeAlternativa" runat="server" />
                                                    <asp:Label ID="lblDisponibilidadeAlternativa" runat="server" Font-Italic="True" Text="Disponibilidade alternativa"
                                                        Font-Size="Medium"></asp:Label>
                                                </div>
                                                <asp:CollapsiblePanelExtender ID="pnlDisponibilidadeAlternativa_CollapsiblePanelExtender"
                                                    runat="server" CollapseControlID="ExpandirDisponibilidadeAlternativa" Collapsed="True"
                                                    CollapsedImage="~/images/expand.jpg" CollapsedText="Disponibilidade alternativa"
                                                    Enabled="True" ExpandControlID="ExpandirDisponibilidadeAlternativa" ExpandedImage="~/images/collapse.jpg"
                                                    ExpandedText="Disponibilidade alternativa" ImageControlID="imgDisponibilidadeAlternativa"
                                                    TargetControlID="pnlDisponibilidadeAlternativa" TextLabelID="lblDisponibilidadeAlternativa">
                                                </asp:CollapsiblePanelExtender>
                                            </asp:Panel>
                                            <asp:Panel ID="pnlDisponibilidadeAlternativa" runat="server">
                                                <asp:GridView ID="gdvReserva7" runat="server" AutoGenerateColumns="False" CellPadding="4"
                                                    DataKeyNames="acmId,dtInicial,hrInicial,dtFinal,hrFinal,acmStatus,aptosLivre,acmDescricao,acmLimpo,diarias,apaId,apaDesc,acmFederacao"
                                                    Width="99%" HorizontalAlign="Center">
                                                    <AlternatingRowStyle CssClass="tableRowOdd" />
                                                    <RowStyle HorizontalAlign="Center" />
                                                    <Columns>
                                                        <asp:TemplateField HeaderText="Acomodação alternativa para bloqueio">
                                                            <ItemTemplate>
                                                                <asp:LinkButton ID="lnkAcomodacao" runat="server" CommandName="select" Text='<%# Bind("acmDescricao") %>'
                                                                    ToolTip="Selecionar acomodação"></asp:LinkButton>
                                                            </ItemTemplate>
                                                            <FooterStyle CssClass="formLabel" HorizontalAlign="Right" />
                                                            <HeaderStyle BorderWidth="0px" CssClass="formRuler" />
                                                            <ItemStyle CssClass="gridBorderColor" HorizontalAlign="Left" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Qtde/Limpo/Livre">
                                                            <HeaderTemplate>
                                                                Qtde/Limpo/Livre
                                                                <asp:ImageButton ID="imgSobeTodos" runat="server" ImageUrl="~/images/BloquearApto.png" OnClick="imgSobeTodos_Click1" OnClientClick="return confirm('Deseja bloquear todos os apartamentos selecionados?')" ToolTip="Bloqueia todos os apartamentos selecionados" />
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                                <asp:TextBox ID="txtQtdeAcomodacao" runat="server" MaxLength="2" Width="20px">1</asp:TextBox>
                                                                <asp:Label ID="lblLimpoAcomodacao" runat="server" Text=" / 1"></asp:Label>
                                                                <asp:Label ID="lblQtdeAcomodacao" runat="server" Text=" / 1"></asp:Label>
                                                                &nbsp;<asp:CheckBox ID="chkSobeApto" runat="server" ToolTip="Selecione se desejar bloquear vários apartamentos de uma só vez." />
                                                            </ItemTemplate>
                                                            <FooterStyle CssClass="formLabel" HorizontalAlign="Center" />
                                                            <HeaderStyle BorderWidth="0px" CssClass="formRuler" />
                                                            <ItemStyle CssClass="gridBorderColor" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Começando">
                                                            <ItemTemplate>
                                                                <asp:LinkButton ID="lnkDiaInicial" runat="server" CommandName="select" Text='<%# Bind("diaInicial") %>'
                                                                    ToolTip="Selecionar acomodação"></asp:LinkButton>
                                                            </ItemTemplate>
                                                            <FooterStyle CssClass="formLabel" HorizontalAlign="Center" />
                                                            <HeaderStyle BorderWidth="0px" CssClass="formRuler" />
                                                            <ItemStyle CssClass="gridBorderColor" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Dia">
                                                            <ItemTemplate>
                                                                <asp:LinkButton ID="lnkDtInicial" runat="server" CommandName="select" Text='<%# Bind("dtInicial") %>'
                                                                    ToolTip="Selecionar acomodação"></asp:LinkButton>
                                                            </ItemTemplate>
                                                            <HeaderStyle BorderWidth="0px" CssClass="formRuler" />
                                                            <ItemStyle CssClass="gridBorderColor" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Terminando">
                                                            <ItemTemplate>
                                                                <asp:LinkButton ID="lnkDiaFinal" runat="server" CommandName="select" Text='<%# Bind("diaFinal") %>'
                                                                    ToolTip="Selecionar acomodação"></asp:LinkButton>
                                                            </ItemTemplate>
                                                            <FooterStyle CssClass="formLabel" HorizontalAlign="Center" />
                                                            <HeaderStyle BorderWidth="0px" CssClass="formRuler" />
                                                            <ItemStyle CssClass="gridBorderColor" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Dia">
                                                            <ItemTemplate>
                                                                <asp:LinkButton ID="lnkDtFinal" runat="server" CommandName="select" Text='<%# Bind("dtFinal") %>'
                                                                    ToolTip="Selecionar acomodação"></asp:LinkButton>
                                                            </ItemTemplate>
                                                            <FooterStyle CssClass="formLabel" HorizontalAlign="Center" />
                                                            <HeaderStyle BorderWidth="0px" CssClass="formRuler" />
                                                            <ItemStyle CssClass="gridBorderColor" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Diárias">
                                                            <ItemTemplate>
                                                                <asp:LinkButton ID="lnkDiaria" runat="server" CommandName="select" Text='<%# Bind("diarias") %>'
                                                                    ToolTip="Selecionar acomodação"></asp:LinkButton>
                                                            </ItemTemplate>
                                                            <FooterStyle CssClass="formLabel" HorizontalAlign="Center" />
                                                            <HeaderStyle BorderWidth="0px" CssClass="formRuler" />
                                                            <ItemStyle CssClass="gridBorderColor" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Hóspedes" Visible="False">
                                                            <ItemTemplate>
                                                                <asp:LinkButton ID="lnkHospedes" runat="server" CommandName="select" ToolTip="Clique na acomodação para excluir"
                                                                    OnClientClick="return confirm('Excluir acomodação?')"></asp:LinkButton>
                                                            </ItemTemplate>
                                                            <FooterStyle CssClass="formLabel" HorizontalAlign="Center" />
                                                            <HeaderStyle BorderWidth="0px" CssClass="formRuler" />
                                                            <ItemStyle CssClass="gridBorderColor" />
                                                        </asp:TemplateField>
                                                    </Columns>
                                                    <HeaderStyle ForeColor="White" />
                                                    <SelectedRowStyle Font-Bold="True" />
                                                </asp:GridView>
                                            </asp:Panel>
                                            <br />
                                        </asp:Panel>
                                        <asp:RoundedCornersExtender ID="pnlSolicitacaoSelecionada_RoundedCornersExtender"
                                            runat="server" Enabled="True" TargetControlID="pnlSolicitacaoSelecionada">
                                        </asp:RoundedCornersExtender>
                                        <asp:Panel ID="pnlIntegranteTitulo" runat="server" Visible="True">
                                            <table>
                                                <tr>
                                                    <td>
                                                        <div id="ExpandirIntegranteTitulo" class="ColocaHand">
                                                            <asp:ImageButton ID="imgIntegranteTitulo" runat="server"
                                                                CssClass="ColocaHand" />
                                                            <asp:Label ID="lblIntegranteTitulo" runat="server" Text="Integrante" Font-Italic="True"
                                                                Font-Bold="True" Font-Size="Medium"></asp:Label>
                                                            <asp:CollapsiblePanelExtender ID="pnlIntegranteTitulo_CollapsiblePanelExtender" runat="server"
                                                                Enabled="True" TargetControlID="pnlIntegranteGeral" CollapseControlID="ExpandirIntegranteTitulo"
                                                                CollapsedText="Integrante" ExpandControlID="ExpandirIntegranteTitulo" ExpandedText="Integrante"
                                                                TextLabelID="lblIntegranteTitulo" CollapsedImage="~/images/expand.jpg" ExpandedImage="~/images/collapse.jpg"
                                                                ImageControlID="imgIntegranteTitulo">
                                                            </asp:CollapsiblePanelExtender>
                                                        </div>
                                                    </td>
                                                    <td>
                                                        <asp:ImageButton ID="imgBtnIntegranteNovoAcao" runat="server" Height="25px" ImageAlign="AbsMiddle"
                                                            ImageUrl="~/images/Integrante.png" ToolTip="Novo Integrante" Width="25px"
                                                            Visible="False" OnClientClick="scroll(0,0)" />
                                                        &nbsp;&nbsp;
                                                        <asp:Button ID="btnIntegranteGravar" runat="server" AccessKey="S" CssClass="imgGravar"
                                                            Text="    Salvar" ToolTip="Alt+S" ValidationGroup="ValidacaoIntegrante" Visible="False"
                                                            OnClientClick="if(confirm('Deseja salvar o integrante agora?'))
                                {
                                    if(ctl00_conPlaHolTurismoSocial_hddProcessando.value=='S')
                                {
                                    alert('Este processo está em execução. Por favor, aguarde.');
                                    return false;
                                }
                            else
                                {
                                    ctl00_conPlaHolTurismoSocial_hddProcessando.value='S';
                                    return true;
                                }
                                }

                            else
                                {
                                    return false;
                                };" />
                                                        <asp:Button ID="btnIntegranteExcluir" runat="server" AccessKey="E" CssClass="imgExcluir"
                                                            Text="    Excluir" ToolTip="Excluir Integrante" Visible="False" OnClientClick="if(confirm('Confirma a exclusão?'))
                                {
                                    if(ctl00_conPlaHolTurismoSocial_hddProcessando.value=='S')
                                {
                                    alert('Este processo está em execução. Por favor, aguarde.');
                                    return false;
                                }
                            else
                                {
                                    ctl00_conPlaHolTurismoSocial_hddProcessando.value='S';
                                    return true;
                                }
                                }

                            else
                                {
                                    return false;
                                };" />
                                                        <asp:Button ID="btnIntUltimoRegistro" runat="server"
                                                            CssClass="btnIrUltimoRegistro" Text="   Posicionador"
                                                            ToolTip="Posiciona o usuário no último integrante clicado."
                                                            Visible="False" />
                                                        &nbsp;<asp:Label ID="lblIntegranteOver" runat="server" Text="ATENÇÃO! Limite de integrantes foi atingido."
                                                            Visible="False"></asp:Label>
                                                    </td>
                                                </tr>
                                            </table>
                                        </asp:Panel>
                                        <asp:ValidationSummary ID="vlsIntegrante" runat="server" DisplayMode="List" ShowMessageBox="True"
                                            ShowSummary="False" ValidationGroup="ValidacaoIntegrante" Height="28px" />
                                        <asp:Panel ID="pnlIntegranteGeral" runat="server">
                                            <br />
                                            <asp:Panel ID="pnlEdicaoIntegrante" runat="server" Visible="False" Width="100%">
                                                <p />
                                                <asp:Label ID="lblIntMatricula" runat="server" Text="Matrícula"></asp:Label>
                                                <asp:TextBox ID="txtIntMatricula" runat="server" Width="120px" MaxLength="14" AutoCompleteType="Disabled"></asp:TextBox>
                                                &nbsp;<asp:Label ID="lblIntVencMatricula" runat="server"></asp:Label>
                                                <asp:Button ID="btnIntMatricula" runat="server" CssClass="btnConsultarReserva" Height="25px"
                                                    TabIndex="-1" />
                                                <asp:Label ID="lblIntCPF" runat="server" Text="CPF"></asp:Label>
                                                <asp:TextBox ID="txtIntCPF" runat="server" MaxLength="14" Width="100px" AutoCompleteType="Disabled"></asp:TextBox>
                                                <asp:Button ID="btnIntCPF" runat="server" CssClass="btnConsultarReserva" TabIndex="-1" />
                                                <asp:Label ID="lblIntNome" runat="server" Text="Nome"></asp:Label>
                                                <asp:TextBox ID="txtIntNome" runat="server" Width="200px" MaxLength="80" AutoPostBack="False"></asp:TextBox>
                                                <asp:Label ID="lblIntNascimento" runat="server" Text="Nascimento"></asp:Label>
                                                <asp:TextBox ID="txtIntNascimento" runat="server" Width="70px" AutoPostBack="True"
                                                    AutoCompleteType="Disabled" MaxLength="10"></asp:TextBox>
                                                <asp:CalendarExtender ID="txtIntNascimento_CalendarExtender" runat="server" Enabled="True"
                                                    TargetControlID="txtIntNascimento" Format="dd/MM/yyyy">
                                                </asp:CalendarExtender>
                                                <asp:Label ID="lblIntSexo" runat="server" Text="Sexo"></asp:Label>
                                                <asp:DropDownList ID="cmbIntSexo" runat="server">
                                                    <asp:ListItem Value="F">Feminino</asp:ListItem>
                                                    <asp:ListItem Value="M">Masculino</asp:ListItem>
                                                </asp:DropDownList>
                                                <p />
                                                <asp:Label ID="lblIntFormaPagamento" runat="server" Text="Forma Pagamento"></asp:Label>
                                                <asp:DropDownList ID="cmbIntFormaPagamento" runat="server">
                                                </asp:DropDownList>
                                                <asp:ImageButton ID="imgBtnAlterarPagamento" runat="server" AlternateText="Alterar todos"
                                                    ImageAlign="AbsMiddle" ImageUrl="~/images/ChangeAll.png" ToolTip="Alterar todos"
                                                    ValidationGroup="ValidacaoIntegrante" OnClientClick="return confirm('Confirma Forma de Pagamento para todos integrantes?')"
                                                    TabIndex="-1" />
                                                <asp:Label ID="lblIntMemorando" runat="server" Text="Memorando"></asp:Label>
                                                <asp:TextBox ID="txtIntMemorando" runat="server" Width="70px" MaxLength="100"></asp:TextBox>
                                                <asp:Label ID="lblIntEmissor" runat="server" Text="Emissor"></asp:Label>
                                                <asp:DropDownList ID="cmbIntEmissor" runat="server">
                                                    <asp:ListItem Value="0">_</asp:ListItem>
                                                    <asp:ListItem Value="1">GP</asp:ListItem>
                                                    <asp:ListItem Value="2">DR</asp:ListItem>
                                                </asp:DropDownList>
                                                <asp:ImageButton ID="imgBtnAlterarMemorando" runat="server" AlternateText="Alterar todos"
                                                    ImageAlign="AbsMiddle" ImageUrl="~/images/ChangeAll.png" ToolTip="Alterar todos"
                                                    ValidationGroup="ValidacaoIntegrante" Style="width: 13px" OnClientClick="return confirm('Confirma Memorando e Emissor para todos integrantes?')"
                                                    TabIndex="-1" />
                                                <asp:Label ID="lblIntCatId" runat="server" Text="Categoria"></asp:Label>
                                                <asp:DropDownList ID="cmbIntCatId" runat="server">
                                                </asp:DropDownList>
                                                <asp:Label ID="lblIntCatCobranca" runat="server" Text="Cobrança"></asp:Label>
                                                <asp:DropDownList ID="cmbIntCatCobranca" runat="server">
                                                    <asp:ListItem Value="1">Comerciário</asp:ListItem>
                                                    <asp:ListItem Value="3">Conveniado</asp:ListItem>
                                                    <asp:ListItem Value="4">Usuário</asp:ListItem>
                                                </asp:DropDownList>
                                                <asp:ImageButton ID="imgBtnAlterarCategoria" runat="server" AlternateText="Alterar todos"
                                                    ImageAlign="AbsMiddle" ImageUrl="~/images/ChangeAll.png" ToolTip="Alterar todos"
                                                    ValidationGroup="ValidacaoIntegrante" OnClientClick="return confirm('Confirma Categoria de Cobrança para todos integrantes?')"
                                                    TabIndex="-1" Style="height: 13px" />
                                                <p />
                                                <asp:Label ID="lblIntEstId" runat="server" Text="Estado/País"></asp:Label>
                                                <asp:DropDownList ID="cmbIntEstId" runat="server" AutoPostBack="True">
                                                </asp:DropDownList>
                                                <asp:Label ID="lblIntCidade" runat="server" Text="Cidade"></asp:Label>
                                                <asp:DropDownList ID="cmbIntCidade" runat="server" AutoPostBack="True">
                                                </asp:DropDownList>
                                                <asp:TextBox ID="txtIntCidade" runat="server" MaxLength="40" Width="200px" Visible="False"></asp:TextBox>
                                                <asp:DropDownList ID="cmbIntEstIdSemCA" runat="server" Visible="False">
                                                </asp:DropDownList>
                                                <asp:Label ID="lblIntSalario" runat="server" Text="Faixa"></asp:Label>
                                                <asp:DropDownList ID="cmbIntSalario" runat="server">
                                                    <asp:ListItem Value="1">1 Salário</asp:ListItem>
                                                    <asp:ListItem Value="2">2 Salários</asp:ListItem>
                                                    <asp:ListItem Value="3">3 Salários</asp:ListItem>
                                                    <asp:ListItem Value="4">4 Salários</asp:ListItem>
                                                    <asp:ListItem Value="5">5 Salários</asp:ListItem>
                                                    <asp:ListItem Value="6">6 Salários</asp:ListItem>
                                                    <asp:ListItem Value="7">7 Salários</asp:ListItem>
                                                    <asp:ListItem Value="8">8 Salários</asp:ListItem>
                                                    <asp:ListItem Value="9">9 Salários</asp:ListItem>
                                                    <asp:ListItem Value="10">10 Salários</asp:ListItem>
                                                    <asp:ListItem Value="11">Acima de 10</asp:ListItem>
                                                    <asp:ListItem Value="12">Sem renda</asp:ListItem>
                                                </asp:DropDownList>
                                                <p />
                                                <asp:Label ID="lblIntEscolaridade" runat="server" Text="Escolaridade"></asp:Label>
                                                <asp:DropDownList ID="cmbIntEscolaridade" runat="server">
                                                    <asp:ListItem Value="1">Sem Escolaridade</asp:ListItem>
                                                    <asp:ListItem Value="2">Analfabeto</asp:ListItem>
                                                    <asp:ListItem Value="3">Alfabetizado</asp:ListItem>
                                                    <asp:ListItem Value="4">Ensino Fundamental Completo</asp:ListItem>
                                                    <asp:ListItem Value="5">Ensino Fundamental Incompleto</asp:ListItem>
                                                    <asp:ListItem Value="6">Ensino Médio Completo</asp:ListItem>
                                                    <asp:ListItem Value="7">Ensino Médio Incompleto</asp:ListItem>
                                                    <asp:ListItem Value="8">Superior Completo</asp:ListItem>
                                                    <asp:ListItem Value="9">Superior Incompleto</asp:ListItem>
                                                    <asp:ListItem Value="10">Pós-Graduação Completa</asp:ListItem>
                                                    <asp:ListItem Value="11">Pós-Graduação Incompleta</asp:ListItem>
                                                    <asp:ListItem Value="12">Ensino Técnico Completo</asp:ListItem>
                                                    <asp:ListItem Value="13">Ensino Técnico Incompleto</asp:ListItem>
                                                    <asp:ListItem Value="0">Sem Informação</asp:ListItem>
                                                </asp:DropDownList>
                                                <asp:Label ID="lblIntEstadoCivil" runat="server" Text="Estado Civil"></asp:Label>
                                                <asp:DropDownList ID="cmbIntEstadoCivil" runat="server">
                                                    <asp:ListItem Value="0">Solteiro</asp:ListItem>
                                                    <asp:ListItem Value="1">Casado</asp:ListItem>
                                                    <asp:ListItem Value="2">Viúvo</asp:ListItem>
                                                    <asp:ListItem Value="3">Divorciado</asp:ListItem>
                                                    <asp:ListItem Value="4">Separado</asp:ListItem>
                                                    <asp:ListItem Value="5">União Estável</asp:ListItem>
                                                </asp:DropDownList>
                                                <asp:Label ID="lblIntRG" runat="server" Text="Documento Identificação"></asp:Label>
                                                <asp:DropDownList ID="cmbIntRG" runat="server">
                                                    <asp:ListItem>RG</asp:ListItem>
                                                    <asp:ListItem Value="CN">Cert Nascimento</asp:ListItem>
                                                    <asp:ListItem Value="CC">Cert Casamento</asp:ListItem>
                                                    <asp:ListItem Value="OU">Outro</asp:ListItem>
                                                </asp:DropDownList>
                                                <asp:TextBox ID="txtIntRG" runat="server" MaxLength="25" Width="150px" Columns="0"></asp:TextBox>
                                                <asp:Panel ID="pnlIntegranteHospedagem" runat="server">
                                                    <asp:CheckBoxList ID="ckbRefeicao" runat="server" RepeatColumns="4" RepeatDirection="Horizontal"
                                                        RepeatLayout="Flow" TextAlign="Left" AutoPostBack="True">
                                                        <asp:ListItem Value="I">Check In com Almoço</asp:ListItem>
                                                        <asp:ListItem Value="I">Jantar</asp:ListItem>
                                                        <asp:ListItem Value="O">&nbsp;&nbsp;&nbsp;Check Out com Almoço</asp:ListItem>
                                                        <asp:ListItem Value="O">Jantar</asp:ListItem>
                                                    </asp:CheckBoxList>
                                                    <asp:ImageButton ID="imgBtnAlterarRefeicao" runat="server" AlternateText="Alterar todos"
                                                        ImageAlign="AbsMiddle" ImageUrl="~/images/ChangeAll.png" ToolTip="Alterar todos"
                                                        ValidationGroup="ValidacaoIntegrante" OnClientClick="return confirm('Confirma Refeições para todos integrantes?')"
                                                        TabIndex="-1" />
                                                    <p />
                                                    <asp:Label ID="lblAcomodacaoEscolhida" runat="server" Text="Acomodação" Width="300px"></asp:Label>
                                                    <asp:DropDownExtender ID="lblAcomodacaoEscolhida_DropDownExtender" runat="server"
                                                        DropDownControlID="radAcomodacao" DynamicServicePath="" Enabled="True" TargetControlID="lblAcomodacaoEscolhida">
                                                    </asp:DropDownExtender>
                                                    <asp:Label ID="lblPeriodo" runat="server" Text="Período"></asp:Label>
                                                    <asp:ImageButton ID="imgBtnDtCheck_InMenos" runat="server" ImageAlign="AbsMiddle"
                                                        ImageUrl="~/images/minus.gif" ToolTip="Diminuir 1 dia" CssClass="ColocaHand"
                                                        OnClientClick="return confirm('Deseja diminuir o período em 1 dia?')" />
                                                    <asp:TextBox ID="txtHosDataIniSol" runat="server" Width="70px" MaxLength="10"></asp:TextBox>
                                                    <asp:ImageButton ID="imgBtnDtCheck_InMais" runat="server" ImageUrl="~/images/plus.gif"
                                                        ToolTip="Acrescentar 1 dia" CssClass="ColocaHand" ImageAlign="AbsMiddle" OnClientClick="return confirm('Deseja acrescentar o período em 1 dia?')"
                                                        Height="16px" />
                                                    <asp:Label ID="lbla" runat="server" Text="a"></asp:Label>
                                                    <asp:ImageButton ID="imgBtnDtCheck_OutMenos" runat="server" ImageUrl="~/images/minus.gif"
                                                        ToolTip="Diminuir 1 dia" CssClass="ColocaHand" ImageAlign="AbsMiddle" OnClientClick="return confirm('Deseja diminuir o período em 1 dia?')" />
                                                    <asp:TextBox ID="txtHosDataFimSol" runat="server" Width="70px" MaxLength="10"></asp:TextBox>
                                                    <asp:ImageButton ID="imgBtnDtCheck_OutMais" runat="server" ImageAlign="AbsMiddle"
                                                        ImageUrl="~/images/plus.gif" ToolTip="Acrescentar 1 dia" CssClass="ColocaHand"
                                                        OnClientClick="return confirm('Deseja acrescentar o período em 1 dia?')" />
                                                    <asp:Label ID="lblDas" runat="server" Text="das"></asp:Label>
                                                    <asp:ImageButton ID="imgBtnHrCheck_InMenos" runat="server" ImageAlign="AbsMiddle"
                                                        ImageUrl="~/images/minus.gif" ToolTip="Diminuir 1 hora" Enabled="False" Visible="False"
                                                        CssClass="ColocaHand" />
                                                    <asp:TextBox ID="txtHosHoraIniSol" runat="server" Width="20px"></asp:TextBox>
                                                    <asp:ImageButton ID="imgBtnHrCheck_InMais" runat="server" ImageAlign="AbsMiddle"
                                                        ImageUrl="~/images/plus.gif" ToolTip="Acrescentar 1 hora" Enabled="False" Visible="False"
                                                        CssClass="ColocaHand" />
                                                    <asp:Label ID="lblAs" runat="server" Text="as"></asp:Label>
                                                    <asp:ImageButton ID="imgBtnHrCheck_OutMenos" runat="server" ImageAlign="AbsMiddle"
                                                        ImageUrl="~/images/minus.gif" ToolTip="Diminuir 1 hora" Enabled="False" Visible="False"
                                                        CssClass="ColocaHand" />
                                                    <asp:TextBox ID="txtHosHoraFimSol" runat="server" Width="20px"></asp:TextBox>
                                                    <asp:ImageButton ID="imgBtnHrCheck_OutMais" runat="server" ImageAlign="AbsMiddle"
                                                        ImageUrl="~/images/plus.gif" ToolTip="Acrescentar 1 hora" Enabled="False" Visible="False"
                                                        CssClass="ColocaHand" />
                                                    <p />
                                                    <asp:Label ID="lblAcomodacaoCobranca" runat="server" Text="Cobrança"></asp:Label>
                                                    <asp:DropDownList ID="cmbAcomodacaoCobranca" runat="server">
                                                    </asp:DropDownList>
                                                    <asp:RadioButtonList ID="radAcomodacao" runat="server" AutoPostBack="True" CssClass="tableRowOdd">
                                                    </asp:RadioButtonList>
                                                    <asp:HiddenField ID="hddHosDataIniSol" runat="server" />
                                                    <asp:HiddenField ID="hddHosDataFimSol" runat="server" />
                                                    <asp:HiddenField ID="hddHosHoraIniSol" runat="server" />
                                                    <asp:HiddenField ID="hddHosHoraFimSol" runat="server" />
                                                    <asp:HiddenField ID="hddAcomodacaoCobranca" runat="server" />
                                                    <asp:HiddenField ID="hddDataInAntes" runat="server" EnableViewState="False" />
                                                    <asp:HiddenField ID="hddDataOutAntes" runat="server" />
                                                    <asp:HiddenField ID="hddDataInDepois" runat="server" />
                                                    <asp:HiddenField ID="hddDataOutDepois" runat="server" />
                                                    <asp:HiddenField ID="hddIntCPFAntes" runat="server" />
                                                    <asp:GridView ID="gdvReserva12" runat="server" AutoGenerateColumns="False" CellPadding="4"
                                                        DataKeyNames="hosId,hosValorPago,hosValorDevido" HorizontalAlign="Center" Width="99%">
                                                        <AlternatingRowStyle CssClass="tableRowOdd" />
                                                        <RowStyle HorizontalAlign="Center" />
                                                        <Columns>
                                                            <asp:TemplateField HeaderText="Hospedagem">
                                                                <ItemTemplate>
                                                                    <asp:LinkButton ID="lnkAcomodacao0" runat="server" CommandName="select" Text='<%# Bind("acmDescricao") %>'
                                                                        ToolTip="Selecionar acomodação"></asp:LinkButton>
                                                                </ItemTemplate>
                                                                <FooterStyle CssClass="formLabel" HorizontalAlign="Right" />
                                                                <HeaderStyle BorderWidth="0px" CssClass="formRuler" />
                                                                <ItemStyle CssClass="gridBorderColor" HorizontalAlign="Left" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Começando">
                                                                <ItemTemplate>
                                                                    <asp:LinkButton ID="lnkDiaInicial0" runat="server" CommandName="select" Text='<%# Bind("solDiaIni") %>'
                                                                        ToolTip="Selecionar acomodação"></asp:LinkButton>
                                                                </ItemTemplate>
                                                                <FooterStyle CssClass="formLabel" HorizontalAlign="Center" />
                                                                <HeaderStyle BorderWidth="0px" CssClass="formRuler" />
                                                                <ItemStyle CssClass="gridBorderColor" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Dia">
                                                                <ItemTemplate>
                                                                    <asp:LinkButton ID="lnkDtInicial0" runat="server" CommandName="select" Text='<%# Bind("solDataIni") %>'
                                                                        ToolTip="Selecionar acomodação"></asp:LinkButton>
                                                                </ItemTemplate>
                                                                <HeaderStyle BorderWidth="0px" CssClass="formRuler" />
                                                                <ItemStyle CssClass="gridBorderColor" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Hora" Visible="False">
                                                                <ItemTemplate>
                                                                    <asp:LinkButton ID="lnkHrInicial0" runat="server" CommandName="select" Text='<%# Bind("solHoraIni") %>'
                                                                        ToolTip="Selecionar acomodação"></asp:LinkButton>
                                                                </ItemTemplate>
                                                                <FooterStyle CssClass="formLabel" HorizontalAlign="Center" />
                                                                <HeaderStyle BorderWidth="0px" CssClass="formRuler" />
                                                                <ItemStyle CssClass="gridBorderColor" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Terminando">
                                                                <ItemTemplate>
                                                                    <asp:LinkButton ID="lnkDiaFinal0" runat="server" CommandName="select" Text='<%# Bind("solDiaFim") %>'
                                                                        ToolTip="Selecionar acomodação"></asp:LinkButton>
                                                                </ItemTemplate>
                                                                <FooterStyle CssClass="formLabel" HorizontalAlign="Center" />
                                                                <HeaderStyle BorderWidth="0px" CssClass="formRuler" />
                                                                <ItemStyle CssClass="gridBorderColor" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Dia">
                                                                <ItemTemplate>
                                                                    <asp:LinkButton ID="lnkDtFinal0" runat="server" CommandName="select" Text='<%# Bind("solDataFim") %>'
                                                                        ToolTip="Selecionar acomodação"></asp:LinkButton>
                                                                </ItemTemplate>
                                                                <FooterStyle CssClass="formLabel" HorizontalAlign="Center" />
                                                                <HeaderStyle BorderWidth="0px" CssClass="formRuler" />
                                                                <ItemStyle CssClass="gridBorderColor" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Hora" Visible="False">
                                                                <ItemTemplate>
                                                                    <asp:LinkButton ID="lnkHrFinal0" runat="server" CommandName="select" Text='<%# Bind("solHoraFim") %>'
                                                                        ToolTip="Selecionar acomodação"></asp:LinkButton>
                                                                </ItemTemplate>
                                                                <HeaderStyle BorderWidth="0px" CssClass="formRuler" />
                                                                <ItemStyle CssClass="gridBorderColor" HorizontalAlign="Center" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Valor">
                                                                <ItemTemplate>
                                                                    <asp:LinkButton ID="lnkHosValorDevido" runat="server" CommandName="select" Text='<%# Bind("hosValorDevido") %>'
                                                                        ToolTip="Selecionar acomodação"></asp:LinkButton>
                                                                </ItemTemplate>
                                                                <HeaderStyle BorderWidth="0px" CssClass="formRuler" />
                                                                <ItemStyle CssClass="gridBorderColor" HorizontalAlign="Center" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Pago">
                                                                <ItemTemplate>
                                                                    <asp:LinkButton ID="lnkHosValorPago" runat="server" CommandName="select" Text='<%# Bind("hosValorPago") %>'
                                                                        ToolTip="Selecionar acomodação"></asp:LinkButton>
                                                                </ItemTemplate>
                                                                <HeaderStyle BorderWidth="0px" CssClass="formRuler" />
                                                                <ItemStyle CssClass="gridBorderColor" HorizontalAlign="Center" />
                                                            </asp:TemplateField>
                                                        </Columns>
                                                        <HeaderStyle ForeColor="White" HorizontalAlign="Center" />
                                                        <SelectedRowStyle Font-Bold="True" />
                                                        <AlternatingRowStyle CssClass="tableRowOdd" />
                                                        <RowStyle HorizontalAlign="Center" />
                                                    </asp:GridView>
                                                    <asp:GridView ID="gdvReserva11" runat="server" AutoGenerateColumns="False" CellPadding="4"
                                                        DataKeyNames="acmId,solId,acmStatus,solDataIni,solDataFim,solHoraIni,solHoraFim,acmDescricao"
                                                        HorizontalAlign="Center" Width="99%">
                                                        <AlternatingRowStyle CssClass="tableRowOdd" />
                                                        <RowStyle HorizontalAlign="Center" />
                                                        <Columns>
                                                            <asp:TemplateField HeaderText="Acomodação disponível para o integrante">
                                                                <ItemTemplate>
                                                                    <asp:LinkButton ID="lnkAcomodacao0" runat="server" CommandName="select" Text='<%# Bind("acmDescricao") %>'
                                                                        ToolTip="Selecionar acomodação"></asp:LinkButton>
                                                                </ItemTemplate>
                                                                <FooterStyle CssClass="formLabel" HorizontalAlign="Right" />
                                                                <HeaderStyle BorderWidth="0px" CssClass="formRuler" />
                                                                <ItemStyle CssClass="gridBorderColor" HorizontalAlign="Left" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Começando">
                                                                <ItemTemplate>
                                                                    <asp:LinkButton ID="lnkDiaInicial0" runat="server" CommandName="select" Text='<%# Bind("solDiaIni") %>'
                                                                        ToolTip="Selecionar acomodação"></asp:LinkButton>
                                                                </ItemTemplate>
                                                                <FooterStyle CssClass="formLabel" HorizontalAlign="Center" />
                                                                <HeaderStyle BorderWidth="0px" CssClass="formRuler" />
                                                                <ItemStyle CssClass="gridBorderColor" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Dia">
                                                                <ItemTemplate>
                                                                    <asp:LinkButton ID="lnkDtInicial0" runat="server" CommandName="select" Text='<%# Bind("solDataIni") %>'
                                                                        ToolTip="Selecionar acomodação"></asp:LinkButton>
                                                                </ItemTemplate>
                                                                <HeaderStyle BorderWidth="0px" CssClass="formRuler" />
                                                                <ItemStyle CssClass="gridBorderColor" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Hora">
                                                                <ItemTemplate>
                                                                    <asp:LinkButton ID="lnkHrInicial0" runat="server" CommandName="select" Text='<%# Bind("solHoraIni") %>'
                                                                        ToolTip="Selecionar acomodação"></asp:LinkButton>
                                                                </ItemTemplate>
                                                                <FooterStyle CssClass="formLabel" HorizontalAlign="Center" />
                                                                <HeaderStyle BorderWidth="0px" CssClass="formRuler" />
                                                                <ItemStyle CssClass="gridBorderColor" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Terminando">
                                                                <ItemTemplate>
                                                                    <asp:LinkButton ID="lnkDiaFinal0" runat="server" CommandName="select" Text='<%# Bind("solDiaFim") %>'
                                                                        ToolTip="Selecionar acomodação"></asp:LinkButton>
                                                                </ItemTemplate>
                                                                <FooterStyle CssClass="formLabel" HorizontalAlign="Center" />
                                                                <HeaderStyle BorderWidth="0px" CssClass="formRuler" />
                                                                <ItemStyle CssClass="gridBorderColor" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Dia">
                                                                <ItemTemplate>
                                                                    <asp:LinkButton ID="lnkDtFinal0" runat="server" CommandName="select" Text='<%# Bind("solDataFim") %>'
                                                                        ToolTip="Selecionar acomodação"></asp:LinkButton>
                                                                </ItemTemplate>
                                                                <FooterStyle CssClass="formLabel" HorizontalAlign="Center" />
                                                                <HeaderStyle BorderWidth="0px" CssClass="formRuler" />
                                                                <ItemStyle CssClass="gridBorderColor" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Hora">
                                                                <ItemTemplate>
                                                                    <asp:LinkButton ID="lnkHrFinal0" runat="server" CommandName="select" Text='<%# Bind("solHoraFim") %>'
                                                                        ToolTip="Selecionar acomodação"></asp:LinkButton>
                                                                </ItemTemplate>
                                                                <HeaderStyle BorderWidth="0px" CssClass="formRuler" />
                                                                <ItemStyle CssClass="gridBorderColor" HorizontalAlign="Center" />
                                                            </asp:TemplateField>
                                                        </Columns>
                                                        <HeaderStyle ForeColor="White" HorizontalAlign="Center" />
                                                        <SelectedRowStyle Font-Bold="True" />
                                                    </asp:GridView>
                                                    <p />
                                                </asp:Panel>
                                                <asp:Panel ID="pnlIntegranteEmissivo" runat="server">
                                                    <p />
                                                    <asp:Label ID="lblIntVinculoId" runat="server" Text="Responsável"></asp:Label>
                                                    <asp:CustomValidator ID="ctvcmbIntVinculoId" runat="server" ErrorMessage="Isento necessita um responsável."
                                                        ValidationGroup="ValidacaoIntegrante" ControlToValidate="cmbIntVinculoId" ClientValidationFunction="ValidaIdade"
                                                        Display="Dynamic" SetFocusOnError="True" ValidateEmptyText="True">*</asp:CustomValidator>
                                                    <asp:DropDownList ID="cmbIntVinculoId" runat="server">
                                                    </asp:DropDownList>
                                                    <asp:Label ID="lblIntFoneResponsavelExc" runat="server" Text="Fone"></asp:Label>
                                                    <asp:CustomValidator ID="ctvcmbIntVinculoId0" runat="server" ClientValidationFunction="ValidaIdade" ControlToValidate="cmbIntVinculoId" Display="Dynamic" ErrorMessage="Isento necessita um responsável." SetFocusOnError="True" ValidateEmptyText="True" ValidationGroup="ValidacaoIntegrante">*</asp:CustomValidator>
                                                    <asp:TextBox ID="txtIntFoneResponsavelExc" runat="server" MaxLength="15" Width="120px"></asp:TextBox>
                                                    <p />
                                                    <asp:Label ID="lblIntLocalTrabalhoResponsavelExc" runat="server" Text="Local de Trabalho"></asp:Label>
                                                    <asp:TextBox ID="txtIntLocalTrabalhoResponsavelExc" runat="server" MaxLength="40"
                                                        Width="150px"></asp:TextBox>
                                                    <asp:Label ID="lblIntEnderecoResponsavelExc" runat="server" Text="Endereço"></asp:Label>
                                                    <asp:TextBox ID="txtIntEnderecoResponsavelExc" runat="server" MaxLength="60" Width="200px"></asp:TextBox>
                                                    <asp:Label ID="lblIntBairroResponsavelExc" runat="server" Text="Bairro/Cidade"></asp:Label>
                                                    <asp:TextBox ID="txtIntBairroResponsavelExc" runat="server" MaxLength="40" Width="150px"></asp:TextBox>
                                                    <p />
                                                    <asp:Label ID="lblIntValorUnitarioExc" runat="server" Text="Valor"></asp:Label>
                                                    <asp:TextBox ID="txtIntValorUnitarioExc" runat="server" Columns="10" Enabled="False"></asp:TextBox>
                                                    <asp:Label ID="lblIntPoltronaExc" runat="server" Text="Poltrona"></asp:Label>
                                                    <asp:TextBox ID="txtIntPoltronaExc" runat="server" MaxLength="3" Width="30px"></asp:TextBox>
                                                    <asp:CheckBox ID="ckbColo" runat="server" Text="Colo" TextAlign="Left" Enabled="False" />
                                                    <asp:Label ID="lblIntApartamentoExc" runat="server" Text="Apartamento"></asp:Label>
                                                    <asp:TextBox ID="txtIntApartamentoExc" runat="server" MaxLength="4" Width="50px"></asp:TextBox>
                                                    <asp:Label ID="lblPratoRapido" runat="server" Text="Refeição" Visible="False"></asp:Label>
                                                    <asp:DropDownList ID="cmbPratoRapido" runat="server" Visible="False">
                                                    </asp:DropDownList>
                                                    <table>
                                                        <tr>
                                                            <td colspan="7" align="center">Preço de Venda
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td align="center">Categoria
                                                            </td>
                                                            <td>&nbsp;
                                                            </td>
                                                            <td align="center">Adulto
                                                            </td>
                                                            <td>&nbsp;
                                                            </td>
                                                            <td align="center">Criança
                                                            </td>
                                                            <td>&nbsp;
                                                            </td>
                                                            <td align="center">Isento
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>&nbsp;
                                                            </td>
                                                            <td>&nbsp;
                                                            </td>
                                                            <td>&nbsp;
                                                            </td>
                                                            <td>&nbsp;
                                                            </td>
                                                            <td>&nbsp;
                                                            </td>
                                                            <td>&nbsp;
                                                            </td>
                                                            <td>&nbsp;
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td align="center">Comerciário
                                                            </td>
                                                            <td>&nbsp;
                                                            </td>
                                                            <td align="center">
                                                                <asp:Label ID="lblComAdultoVlr" runat="server" Text="0,00"></asp:Label>
                                                            </td>
                                                            <td>&nbsp;
                                                            </td>
                                                            <td align="center">
                                                                <asp:Label ID="lblComCriancaVlr" runat="server" Text="0,00"></asp:Label>
                                                            </td>
                                                            <td>&nbsp;
                                                            </td>
                                                            <td align="center">
                                                                <asp:Label ID="lblComIsentoVlr" runat="server" Text="0,00"></asp:Label>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td align="center">Conveniado
                                                            </td>
                                                            <td>&nbsp;
                                                            </td>
                                                            <td align="center">
                                                                <asp:Label ID="lblConvAdultoVlr" runat="server" Text="0,00"></asp:Label>
                                                            </td>
                                                            <td>&nbsp;
                                                            </td>
                                                            <td align="center">
                                                                <asp:Label ID="lblConvCriancaVlr" runat="server" Text="0,00"></asp:Label>
                                                            </td>
                                                            <td>&nbsp;
                                                            </td>
                                                            <td align="center">
                                                                <asp:Label ID="lblConvIsentoVlr" runat="server" Text="0,00"></asp:Label>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td align="center">Usuário
                                                            </td>
                                                            <td>&nbsp;
                                                            </td>
                                                            <td align="center">
                                                                <asp:Label ID="lblUsuAdultoVlr" runat="server" Text="0,00"></asp:Label>
                                                            </td>
                                                            <td>&nbsp;
                                                            </td>
                                                            <td align="center">
                                                                <asp:Label ID="lblUsuCriancaVlr" runat="server" Text="0,00"></asp:Label>
                                                            </td>
                                                            <td>&nbsp;
                                                            </td>
                                                            <td align="center">
                                                                <asp:Label ID="lblUsuIsentoVlr" runat="server" Text="0,00"></asp:Label>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td align="center">&nbsp;
                                                            </td>
                                                            <td>&nbsp;
                                                            </td>
                                                            <td align="center">&nbsp;
                                                            </td>
                                                            <td>&nbsp;
                                                            </td>
                                                            <td align="center">&nbsp;
                                                            </td>
                                                            <td>&nbsp;
                                                            </td>
                                                            <td align="center">&nbsp;
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </asp:Panel>
                                            </asp:Panel>
                                            <asp:Panel ID="pnlIntegranteAcao" runat="server">
                                                <asp:ValidationSummary ID="vlsDiasPrazo" runat="server" DisplayMode="List" ShowMessageBox="True"
                                                    ShowSummary="False" ValidationGroup="ValidacaoDiasPrazo" Height="28px" Width="1330px" />
                                                <asp:GridView ID="gdvReserva9" runat="server" AutoGenerateColumns="False" CellPadding="4"
                                                    DataKeyNames="intId,resId,solId,catLink,intDiaIni,intDiaFim,hosDataIniSol,hosDataFimSol,hosHoraIniSol,hosHoraFimSol,hosId,hosValorDevido,hosValorPago,intFormaPagamento,intCriancaColoExc,apaDesc,intUsuario,intVinculoId,hosStatus,SolDiaIni,SolDiaFim,intStatus,IntDtNascimento,IntCPF,NomeResponsavel,BolPrimeiraParcela,BolPrimeiraStatusRegistro,BolSegundaParcela,BolSegundaStatusRegistro,BolParcelaUnica,BolUnicaStatusRegistro"
                                                    Width="99%" HorizontalAlign="Center" ShowFooter="True">
                                                    <AlternatingRowStyle CssClass="tableRowOdd" />
                                                    <RowStyle HorizontalAlign="Center" />
                                                    <Columns>
                                                        <asp:TemplateField>
                                                            <HeaderTemplate>
                                                                <asp:CheckBox ID="ckbHeadIntegrante" runat="server" AutoPostBack="True" OnCheckedChanged="ckbHeadIntegrante_CheckedChanged" />
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                                <asp:CheckBox ID="ckbItemIntegrante" runat="server" AutoPostBack="True" OnCheckedChanged="ckbItemIntegrante_CheckedChanged" />
                                                            </ItemTemplate>
                                                            <FooterStyle CssClass="gridBorderColor" />
                                                            <HeaderStyle BorderWidth="0px" CssClass="formRuler" />
                                                            <ItemStyle CssClass="gridBorderColor" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField>
                                                            <HeaderTemplate>
                                                                <asp:RadioButtonList ID="rblDiasPrazo" runat="server" RepeatColumns="4" RepeatDirection="Horizontal"
                                                                    RepeatLayout="Flow" OnSelectedIndexChanged="rblDiasPrazo_SelectedIndexChanged"
                                                                    Visible="False">
                                                                    <asp:ListItem Value="0" Selected="True">0</asp:ListItem>
                                                                    <asp:ListItem Value="1">1</asp:ListItem>
                                                                </asp:RadioButtonList>
                                                                <asp:TextBox ID="txtDiasPrazo" runat="server" Width="70px" Visible="False" MaxLength="10" AutoPostBack="True" OnTextChanged="txtDiasPrazo_TextChanged"></asp:TextBox>
                                                                <asp:ImageButton ID="imgBoleto50" runat="server" ImageUrl="~/images/BoletoParcelado.jpg"
                                                                    ImageAlign="AbsMiddle" CssClass="ColocaHand" ToolTip="Valor parcelado" CommandArgument="50"
                                                                    OnClick="imgBoleto_Click" Height="50px" Visible="False" ValidationGroup="ValidacaoDiasPrazo" />
                                                                <asp:ImageButton ID="imgBoleto" runat="server" ImageUrl="~/images/BoletoAvista.jpg"
                                                                    ImageAlign="AbsMiddle" CssClass="ColocaHand" ToolTip="Valor total devido" OnClick="imgBoleto_Click"
                                                                    CommandArgument="100" Visible="False" Height="50px" ValidationGroup="ValidacaoDiasPrazo" />
                                                                <asp:ImageButton ID="imgBoletoIndividual" runat="server" ImageUrl="~/images/BoletoIndividual.png"
                                                                    ImageAlign="AbsMiddle" CssClass="ColocaHand" ToolTip="Valor total devido" Visible="False"
                                                                    CommandArgument="100" OnClick="imgBoletoIndividual_Click" ValidationGroup="ValidacaoDiasPrazo" Height="50px" />
                                                                <asp:ImageButton ID="imgCupom" runat="server" ImageAlign="AbsMiddle" ImageUrl="~/images/Cupom.png"
                                                                    CssClass="ColocaHand" Visible="False" CommandArgument="100" OnClick="imgCupom_Click"
                                                                    ToolTip="Valor total devido" ValidationGroup="ValidacaoDiasPrazo" />
                                                                <asp:ImageButton ID="imgCupomIndividual" runat="server" ImageAlign="AbsMiddle" ImageUrl="~/images/CupomIndividual.png"
                                                                    CssClass="ColocaHand" Visible="False" ToolTip="Valor total devido" CommandArgument="100"
                                                                    OnClick="imgCupomIndividual_Click" ValidationGroup="ValidacaoDiasPrazo" />
                                                                <asp:LinkButton ID="lnkhdriIntegrante" runat="server" ForeColor="White" OnClick="lnkhdriIntegrante_Click"
                                                                    ToolTip="Ordenar" CommandArgument="1">Nome</asp:LinkButton>
                                                                <asp:ImageButton ID="imgiIntegrante" runat="server" ImageAlign="AbsMiddle" OnClick="imgiIntegrante_Click"
                                                                    Visible="False" CommandArgument="1" />
                                                                <asp:ImageButton ID="imgListaIntegrante" runat="server" ImageUrl="~/images/impressora.png"
                                                                    AlternateText="Imprimir" ToolTip="Imprimir relação de integrantes" Visible="False" />
                                                                <asp:CalendarExtender ID="txtDiasPrazo_CalendarExtender" runat="server" Enabled="True"
                                                                    TargetControlID="txtDiasPrazo" Format="dd/MM/yyyy">
                                                                </asp:CalendarExtender>
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                                <asp:Image ID="imgDependente" runat="server" ImageAlign="AbsMiddle" ImageUrl="~/images/DependenteReponsavel.png"
                                                                    ToolTip="Vínculo" Visible="False" />
                                                                <asp:LinkButton ID="lnkIntNome" runat="server" CommandName="select" OnClick="lnkIntNome_Click1"
                                                                    Text='<%# Eval("intNome") %>' ToolTip="Ir para Integrante" OnClientClick="scroll(0,0)"></asp:LinkButton>
                                                            </ItemTemplate>
                                                            <FooterStyle CssClass="gridBorderColor" HorizontalAlign="Right" />
                                                            <HeaderStyle BorderWidth="0px" CssClass="formRuler" HorizontalAlign="Left" />
                                                            <ItemStyle CssClass="gridBorderColor" HorizontalAlign="Left" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Categoria">
                                                            <HeaderTemplate>
                                                                <asp:LinkButton ID="lnkhdriCategoria" runat="server" CommandArgument="1" ForeColor="White"
                                                                    OnClick="lnkhdriIntegrante_Click" ToolTip="Ordenar">Categoria</asp:LinkButton>
                                                                <asp:ImageButton ID="imgiCategoria" runat="server" CommandArgument="1" ImageAlign="AbsMiddle"
                                                                    ImageUrl="~/images/AZ.png" OnClick="imgiIntegrante_Click" Visible="False" />
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                                <asp:LinkButton ID="lnkCategoriaIntegrante" runat="server" CommandName="select" Text='<%# Bind("catDescricao") %>'
                                                                    ToolTip="Ir para Integrante" OnClick="lnkIntNome_Click1"></asp:LinkButton>
                                                            </ItemTemplate>
                                                            <FooterStyle CssClass="gridBorderColor" />
                                                            <HeaderStyle BorderWidth="0px" CssClass="formRuler" />
                                                            <ItemStyle CssClass="gridBorderColor" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Acomodação">
                                                            <HeaderTemplate>
                                                                <asp:LinkButton ID="lnkhdriAcomodacao" runat="server" CommandArgument="1" ForeColor="White"
                                                                    OnClick="lnkhdriIntegrante_Click" ToolTip="Ordenar">Acomodação</asp:LinkButton>
                                                                <asp:ImageButton ID="imgiAcomodacao" runat="server" CommandArgument="1" ImageAlign="AbsMiddle"
                                                                    ImageUrl="~/images/AZ.png" OnClick="imgiIntegrante_Click" Visible="False" />
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                                <asp:LinkButton ID="lnkAcomodacaoIntegrante" runat="server" CommandName="select"
                                                                    Text='<%# Bind("acmDescricao") %>' ToolTip="Ir para Integrante" OnClick="lnkIntNome_Click1"></asp:LinkButton>
                                                            </ItemTemplate>
                                                            <FooterStyle CssClass="gridBorderColor" />
                                                            <HeaderStyle BorderWidth="0px" CssClass="formRuler" />
                                                            <ItemStyle CssClass="gridBorderColor" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField>
                                                            <HeaderTemplate>
                                                                <asp:ImageButton ID="imgTiraAcomodacao" runat="server" ImageUrl="~/images/Troca.png"
                                                                    OnClientClick="if(confirm('Deseja mesmo desvincular as acomodações dos integrantes selecionados?'))
   {
     if(aspnetForm.ctl00_conPlaHolTurismoSocial_hddProcessando.value=='S')
   {
     alert('Este processo está em execução. Por favor, aguarde.');
     return false;
   }
     else
       {
         aspnetForm.ctl00_conPlaHolTurismoSocial_hddProcessando.value='S';
         return true;
       }
         }
  
 else
   {
     return false;
   };"
                                                                    ToolTip="Clique para desvincular as acomodações selecionadas" OnClick="imgTiraAcomodacao_Click" />
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                                <asp:CheckBox ID="chkTiraAcomodacao" runat="server" />
                                                            </ItemTemplate>
                                                            <FooterStyle CssClass="gridBorderColor" />
                                                            <HeaderStyle CssClass="formRuler" HorizontalAlign="Center" />
                                                            <ItemStyle HorizontalAlign="Center" CssClass="gridBorderColor" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Início">
                                                            <HeaderTemplate>
                                                                <asp:LinkButton ID="lnkhdriCheckInIntegrante" runat="server" CommandArgument="1"
                                                                    ForeColor="White" OnClick="lnkhdriIntegrante_Click" ToolTip="Ordenar">Início</asp:LinkButton>
                                                                <asp:ImageButton ID="imgiCheckInIntegrante" runat="server" CommandArgument="1" ImageAlign="AbsMiddle"
                                                                    ImageUrl="~/images/AZ.png" OnClick="imgiIntegrante_Click" Visible="False" />
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                                <asp:ImageButton ID="imgBtnDtCheckInMenos" runat="server" CssClass="ColocaHand" ImageAlign="AbsMiddle"
                                                                    ImageUrl="~/images/minus.gif" ToolTip="Diminuir 1 dia" AccessKey="1" OnClick="imgBtnDtCheckInMais_Click"
                                                                    OnClientClick="return confirm('Deseja acrescentar o período em 1 dia?')" />
                                                                <asp:LinkButton ID="lnkCheckInIntegrante" runat="server" CommandName="select" Text='<%# Bind("intDiaIni") %>'
                                                                    ToolTip="Ir para Integrante" OnClick="lnkIntNome_Click1"></asp:LinkButton>
                                                                <asp:ImageButton ID="imgBtnDtCheckInMais" runat="server" CssClass="ColocaHand" ImageAlign="AbsMiddle"
                                                                    ImageUrl="~/images/plus.gif" ToolTip="Acrescentar 1 dia" AccessKey="2" OnClick="imgBtnDtCheckInMais_Click"
                                                                    OnClientClick="return confirm('Deseja diminuir o período em 1 dia?')" />
                                                            </ItemTemplate>
                                                            <HeaderStyle BorderWidth="0px" CssClass="formRuler" />
                                                            <FooterStyle CssClass="gridBorderColor" HorizontalAlign="Center" />
                                                            <ItemStyle CssClass="gridBorderColor" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Término">
                                                            <HeaderTemplate>
                                                                <asp:LinkButton ID="lnkhdriCheckOutIntegrante" runat="server" CommandArgument="1"
                                                                    ForeColor="White" OnClick="lnkhdriIntegrante_Click" ToolTip="Ordenar">Término</asp:LinkButton>
                                                                <asp:ImageButton ID="imgiCheckOutIntegrante" runat="server" CommandArgument="1" ImageAlign="AbsMiddle"
                                                                    ImageUrl="~/images/AZ.png" OnClick="imgiIntegrante_Click" Visible="False" />
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                                <asp:ImageButton ID="imgBtnDtCheckOutMenos" runat="server" CssClass="ColocaHand"
                                                                    ImageAlign="AbsMiddle" ImageUrl="~/images/minus.gif" ToolTip="Diminuir 1 dia"
                                                                    AccessKey="3" OnClick="imgBtnDtCheckInMais_Click" OnClientClick="return confirm('Deseja diminuir o período em 1 dia?')" />
                                                                <asp:LinkButton ID="lnkCheckOutIntegrante" runat="server" CommandName="select" Text='<%# Bind("intDiaFim") %>'
                                                                    ToolTip="Ir para Integrante" OnClick="lnkIntNome_Click1"></asp:LinkButton>
                                                                <asp:ImageButton ID="imgBtnDtCheckOutMais" runat="server" CssClass="ColocaHand" ImageAlign="AbsMiddle"
                                                                    ImageUrl="~/images/plus.gif" ToolTip="Acrescentar 1 dia" AccessKey="4" OnClick="imgBtnDtCheckInMais_Click"
                                                                    OnClientClick="return confirm('Deseja acrescentar o período em 1 dia?')" />
                                                            </ItemTemplate>
                                                            <HeaderStyle BorderWidth="0px" CssClass="formRuler" />
                                                            <FooterStyle CssClass="gridBorderColor" HorizontalAlign="Center" />
                                                            <ItemStyle CssClass="gridBorderColor" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Valor">
                                                            <HeaderTemplate>
                                                                <asp:LinkButton ID="lnkhdriVlrDevidoIntegrante" runat="server" CommandArgument="1"
                                                                    ForeColor="White" OnClick="lnkhdriIntegrante_Click" ToolTip="Ordenar">A Pagar</asp:LinkButton>
                                                                <asp:ImageButton ID="imgiVlrDevidoIntegrante" runat="server" CommandArgument="1"
                                                                    ImageAlign="AbsMiddle" ImageUrl="~/images/AZ.png" OnClick="imgiIntegrante_Click"
                                                                    Visible="False" />
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                                <asp:LinkButton ID="lnkHosValorDevido" runat="server" CommandName="select" Text='<%# Bind("hosValorDevido") %>'
                                                                    ToolTip="Ir para Integrante" OnClick="lnkIntNome_Click1"></asp:LinkButton>
                                                            </ItemTemplate>
                                                            <HeaderStyle BorderWidth="0px" CssClass="formRuler" />
                                                            <FooterStyle CssClass="gridBorderColor" HorizontalAlign="Right" />
                                                            <ItemStyle CssClass="gridBorderColor" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Pago">
                                                            <HeaderTemplate>
                                                                <asp:LinkButton ID="lnkhdriVlrPagoIntegrante" runat="server" CommandArgument="1"
                                                                    ForeColor="White" OnClick="lnkhdriIntegrante_Click" ToolTip="Ordenar">Pago</asp:LinkButton>
                                                                <asp:ImageButton ID="imgiVlrPagoIntegrante" runat="server" CommandArgument="1" ImageAlign="AbsMiddle"
                                                                    ImageUrl="~/images/AZ.png" OnClick="imgiIntegrante_Click" Visible="False" />
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                                <asp:LinkButton ID="lnkHosValorPago" runat="server" CommandName="select" Text='<%# Bind("hosValorPago") %>'
                                                                    ToolTip="Ir para Integrante" OnClick="lnkIntNome_Click1"></asp:LinkButton>
                                                            </ItemTemplate>
                                                            <HeaderStyle BorderWidth="0px" CssClass="formRuler" />
                                                            <FooterStyle CssClass="gridBorderColor" HorizontalAlign="Right" />
                                                            <ItemStyle CssClass="gridBorderColor" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Refeição">
                                                            <ItemTemplate>
                                                                <asp:LinkButton ID="lnkRefeicao" runat="server" CommandName="select" Text='<%# Bind("refPratoDesc") %>'
                                                                    ToolTip="Ir para Integrante" OnClick="lnkIntNome_Click1"></asp:LinkButton>
                                                            </ItemTemplate>
                                                            <FooterStyle CssClass="gridBorderColor" />
                                                            <HeaderStyle BorderWidth="0px" CssClass="formRuler" />
                                                            <ItemStyle CssClass="gridBorderColor" HorizontalAlign="Left" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Servidor">
                                                            <HeaderTemplate>
                                                                <asp:LinkButton ID="lnkhdriServidor" runat="server" CommandArgument="1" ForeColor="White"
                                                                    OnClick="lnkhdriIntegrante_Click" ToolTip="Ordenar">Servidor</asp:LinkButton>
                                                                <asp:ImageButton ID="imgiServidor" runat="server" CommandArgument="1" ImageAlign="AbsMiddle"
                                                                    ImageUrl="~/images/AZ.png" OnClick="imgiIntegrante_Click" Visible="False" />
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                                <asp:LinkButton ID="lnkServidor" runat="server" CommandName="select" ToolTip="Ir para Integrante"
                                                                    OnClick="lnkIntNome_Click1"></asp:LinkButton>
                                                            </ItemTemplate>
                                                            <FooterStyle CssClass="gridBorderColor" />
                                                            <HeaderStyle BorderWidth="0px" CssClass="formRuler" />
                                                            <ItemStyle CssClass="gridBorderColor" HorizontalAlign="Center" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Voucher">
                                                            <ItemTemplate>
                                                                <asp:ImageButton ID="imbVoucher" ToolTip="Imprimir voucher" runat="server" ImageUrl="~/images/Voucher.png" OnClick="imbVoucher_Click" />
                                                            </ItemTemplate>
                                                            <FooterStyle CssClass="gridBorderColor" />
                                                            <HeaderStyle CssClass="formRuler" />
                                                            <ItemStyle CssClass="gridBorderColor" />
                                                        </asp:TemplateField>
                                                    </Columns>
                                                    <HeaderStyle ForeColor="White" />
                                                    <SelectedRowStyle Font-Bold="True" />
                                                </asp:GridView>
                                            </asp:Panel>
                                            <br />
                                        </asp:Panel>
                                        <asp:RoundedCornersExtender ID="pnlIntegranteGeral_RoundedCornersExtender" runat="server"
                                            Enabled="True" TargetControlID="pnlIntegranteGeral">
                                        </asp:RoundedCornersExtender>
                                        <asp:Panel ID="pnlFinanceiroTitulo" runat="server" Visible="True">
                                            <table>
                                                <tr>
                                                    <td>
                                                        <div id="ExpandirFinanceiroTitulo">
                                                            <asp:Image ID="imgFinanceiroTitulo" runat="server" CssClass="ColocaHand" />
                                                            <asp:Label ID="lblFinanceiroTitulo" runat="server" Text="Financeiro" CssClass="ColocaHand"
                                                                Font-Italic="True" Font-Bold="True" Font-Size="Medium"></asp:Label>
                                                            <asp:CollapsiblePanelExtender ID="pnlFinanceiroTitulo_CollapsiblePanelExtender" runat="server"
                                                                Enabled="True" TargetControlID="pnlFinanceiroAcao" CollapseControlID="ExpandirFinanceiroTitulo"
                                                                CollapsedText="Financeiro" ExpandControlID="ExpandirFinanceiroTitulo" ExpandedText="Financeiro"
                                                                TextLabelID="lblFinanceiroTitulo" CollapsedImage="~/images/expand.jpg" ExpandedImage="~/images/collapse.jpg"
                                                                ImageControlID="imgFinanceiroTitulo">
                                                            </asp:CollapsiblePanelExtender>
                                                        </div>
                                                    </td>
                                                    <td>
                                                        <asp:Button ID="btnCaixa" runat="server" Text="     Recebimento Caixa" CssClass="imgCaixa"
                                                            OnClientClick="if(confirm('Enviar valores para pagamento no caixa?'))
                                {
                                    if(ctl00_conPlaHolTurismoSocial_hddProcessando.value=='S')
                                {
                                    alert('Este processo está em execução. Por favor, aguarde.');
                                    return false;
                                }
                            else
                                {
                                    ctl00_conPlaHolTurismoSocial_hddProcessando.value='S';
                                    return true;
                                }
                                }

                            else
                                {
                                    return false;
                                };" />
                                                    </td>
                                                </tr>
                                            </table>
                                        </asp:Panel>
                                        <br />
                                        <asp:Panel ID="pnlFinanceiroAcao" runat="server">
                                            <asp:Panel ID="pnlRessarcimento" runat="server">
                                                <asp:GridView ID="gdvRessarcimento" runat="server" AutoGenerateColumns="False" CellPadding="4"
                                                    DataKeyNames="VenId,venStatus,venValor" AllowSorting="True" Width="99%" HorizontalAlign="Center">
                                                    <AlternatingRowStyle CssClass="tableRowOdd" />
                                                    <RowStyle HorizontalAlign="Center" />
                                                    <Columns>
                                                        <asp:BoundField DataField="VenValor" HeaderText="Valor">
                                                            <HeaderStyle CssClass="formRuler" />
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="venUsuario" HeaderText="Ressarcimento">
                                                            <HeaderStyle CssClass="formRuler" />
                                                        </asp:BoundField>
                                                    </Columns>
                                                    <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                                                    <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
                                                    <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                                                    <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                                                    <EditRowStyle BackColor="#2461BF" />
                                                    <AlternatingRowStyle BackColor="White" CssClass="tableRowOdd" />
                                                </asp:GridView>
                                            </asp:Panel>
                                            <br />
                                            <asp:GridView ID="gdvReserva10" runat="server" AutoGenerateColumns="False" CellPadding="4"
                                                DataKeyNames="resId,venId,venValor,venStatus,bolImpNossoNumero,venData,bolImpDtVencimento,venUsuario,venUsuarioData,BolImpValor,BolTipo,BolImpDtDocumento,BolStatusRemessaCaixa,BolImpTipoParcela,BolImpSacado"
                                                Width="99%" HorizontalAlign="Center" AllowSorting="True">
                                                <AlternatingRowStyle CssClass="tableRowOdd" />
                                                <RowStyle HorizontalAlign="Center" />
                                                <Columns>
                                                    <asp:TemplateField HeaderText="Boleto">
                                                        <ItemTemplate>
                                                            <asp:LinkButton ID="lnkNossoNumero" runat="server" CommandName="select" Text='<%# Eval("bolImpNossoNumero") %>'
                                                                OnClientClick="return confirm('Excluir Conciliação Manual?')"></asp:LinkButton>
                                                            <asp:LinkButton ID="lnkNossoNumeroInclusao" runat="server" CommandName="select" Text='<%# Eval("bolImpNossoNumero") %>'
                                                                Visible="False" OnClientClick="return confirm('Confirma Conciliação Manual?')"></asp:LinkButton>
                                                            <asp:Label ID="lblNossoNumero" runat="server" Font-Strikeout="False"></asp:Label>
                                                            <asp:Image ID="imgBoletoCartao" runat="server" ImageUrl="~/images/CartaoCreditoPequeno.png" />
                                                        </ItemTemplate>
                                                        <FooterStyle CssClass="formLabel" />
                                                        <HeaderStyle BorderWidth="0px" CssClass="formRuler" HorizontalAlign="Center" />
                                                        <ItemStyle CssClass="gridBorderColor" HorizontalAlign="Left" />
                                                    </asp:TemplateField>
                                                    <asp:BoundField DataField="BolImpSacado" HeaderText="Sacado">
                                                        <HeaderStyle CssClass="formRuler" />
                                                        <ItemStyle CssClass="gridBorderColor" />
                                                    </asp:BoundField>
                                                    <asp:TemplateField HeaderText="Vencimento">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblDtVencimento" runat="server"></asp:Label>
                                                        </ItemTemplate>
                                                        <HeaderStyle BorderWidth="0px" CssClass="formRuler" />
                                                        <ItemStyle CssClass="gridBorderColor" />
                                                    </asp:TemplateField>
                                                    <asp:BoundField DataField="BolImpValor" HeaderText="Valor Devido">
                                                        <HeaderStyle CssClass="formRuler" Width="0px" />
                                                        <ItemStyle CssClass="gridBorderColor" />
                                                    </asp:BoundField>
                                                    <asp:TemplateField HeaderText="Pagamento">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblDtPagamento" runat="server"></asp:Label>
                                                        </ItemTemplate>
                                                        <HeaderStyle BorderWidth="0px" CssClass="formRuler" />
                                                        <ItemStyle CssClass="gridBorderColor" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Pago">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblVlrPago" runat="server"></asp:Label>
                                                        </ItemTemplate>
                                                        <HeaderStyle BorderWidth="0px" CssClass="formRuler" />
                                                        <ItemStyle CssClass="gridBorderColor" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Forma">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblFormaPgto" runat="server"></asp:Label>
                                                        </ItemTemplate>
                                                        <HeaderStyle BorderWidth="0px" CssClass="formRuler" />
                                                        <ItemStyle CssClass="gridBorderColor" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Situação">
                                                        <ItemTemplate>
                                                            <asp:ImageButton ID="imgRegistro" runat="server" ImageUrl="~/images/BoletoBronze.png" Width="22px" OnClick="imgRegistro_Click" />
                                                            &nbsp;<asp:ImageButton ToolTip="Apaga boletos gerado sem pagamento" ID="imgApagaBoleto" runat="server" ImageUrl="~/images/Delete.gif" OnClick="imgApagaBoleto_Click" Width="24px" OnClientClick="if(confirm('Confirma a exclusão do boleto?'))
                                {
                                    if(ctl00_conPlaHolTurismoSocial_hddProcessando.value=='S')
                                {
                                    alert('Este processo está em execução. Por favor, aguarde.');
                                    return false;
                                }
                            else
                                {
                                    ctl00_conPlaHolTurismoSocial_hddProcessando.value='S';
                                    return true;
                                }
                                }

                            else
                                {
                                    return false;
                                };" />
                                                        </ItemTemplate>
                                                        <HeaderStyle CssClass="formRuler" BorderWidth="0px" />
                                                        <ItemStyle CssClass="gridBorderColor" />
                                                    </asp:TemplateField>
                                                </Columns>
                                                <HeaderStyle ForeColor="White" />
                                                <SelectedRowStyle Font-Bold="True" />
                                            </asp:GridView>
                                            <br />
                                        </asp:Panel>
                                        <asp:RoundedCornersExtender ID="pnlFinanceiroAcao_RoundedCornersExtender" runat="server"
                                            Enabled="True" TargetControlID="pnlFinanceiroAcao">
                                        </asp:RoundedCornersExtender>

                                        <br />
                                        <asp:HiddenField ID="hddIntervaloSolicitacao" runat="server" />
                                        <asp:HiddenField ID="hddHoraInicioPernoite" runat="server" />
                                        <asp:HiddenField ID="hddHoraFimPernoite" runat="server" />
                                        <asp:HiddenField ID="hddColo" runat="server" Value="0" />
                                        <asp:HiddenField ID="hddDiasPrazo" runat="server" Value="0" />
                                        <asp:HiddenField ID="hddMultiValorado" runat="server" Value="S" />
                                        <asp:HiddenField ID="hddCapacidade" runat="server" Value="45" />
                                        <asp:HiddenField ID="hddIdadeAdulto" runat="server" Value="10" />
                                        <asp:HiddenField ID="hddIdadeCrianca" runat="server" Value="5" />
                                        <asp:HiddenField ID="hddIdadeIsento" runat="server" Value="0" />
                                        <asp:HiddenField ID="hddIdade" runat="server" Value="0" />
                                        <asp:HiddenField ID="hddIdadeColo" runat="server" Value="1000" />
                                        <asp:HiddenField ID="hddDataFaixaEtaria" runat="server" />
                                        <asp:HiddenField ID="hddAutorizaConveniado" runat="server" Value="S" />
                                        <asp:HiddenField ID="hddAutorizaUsuario" runat="server" Value="S" />
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
                    </td>
                </tr>
            </table>
            <asp:HiddenField ID="hddProcessando" runat="server" />
            <asp:HiddenField ID="hddIntStatus" runat="server" />
            <asp:HiddenField ID="hddEstadoPais" runat="server" />
            <br />
            <asp:HiddenField ID="hddResTipo" runat="server" />
            <table style="width: 100%;">
                <tr>
                    <td style="display: none">
                        <asp:Button ID="btnMoveFocoResNome" runat="server" Text="AuxMoveFoco" />
                        <asp:DropDownList ID="cmbEstadoAux" runat="server">
                        </asp:DropDownList>
                    </td>
                    <td>&nbsp;
                    </td>
                    <td>&nbsp;
                    </td>
                </tr>
            </table>
            <br />
        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="imgBtnReservaAcaoVoltarRecepcao"></asp:PostBackTrigger>
            <asp:PostBackTrigger ControlID="btnReservaCancelar"></asp:PostBackTrigger>
        </Triggers>
    </asp:UpdatePanel>
    <br />
</asp:Content>
