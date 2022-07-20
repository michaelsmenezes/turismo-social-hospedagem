<%@ Page Title="" Language="VB" MasterPageFile="~/TurismoSocial.master" AutoEventWireup="true" CodeFile="frmEmissivo.aspx.vb"
    Inherits="Emissivo_frmEmissivo" UICulture="auto" Culture="pt-BR" EnableEventValidation="false" %>

<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=11.0.0.0, Culture=neutral, PublicKeyToken=89845dcd8080cc91" Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Assembly="System.Web.DynamicData, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.DynamicData" TagPrefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <meta http-equiv="X-UA-Compatible" content="IE=9" />
    <script src="../BootStrap/js/jQuery.js" type="text/javascript"></script>
    <script src="../BootStrap/js/bootstrap.js" type="text/javascript"></script>
    <script src="../BootStrap/js/bootstrap.min.js" type="text/javascript"></script>
    <link href="../BootStrap/css/bootstrap.min.css" rel="stylesheet" />
    <link href="../BootStrap/css/bootstrap.css" rel="stylesheet" />
    <script src="../JScript.js" type="text/javascript"></script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="conPlaHolTurismoSocial" runat="Server">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <br />
            <br />
            <br />
            <br />
            <br />
            <div runat="server" id="divConConsultarPasseio" align="left" style="width: 99%">
                <asp:Panel ID="pnlConConsultarPasseio" runat="server" class="form-control" Height="100%" Width="100%" DefaultButton="btnConConsultar">
                    <div id="divStatusPasseio" runat="server" class="BarraTituloEmissivo ArrendodarBorda" align="center" style="width: 100%">
                        <asp:Label ID="lblStatusPasseio" runat="server" Text="Consultar passeios e excursões"></asp:Label>
                    </div>
                    <br />
                    <div class="row ">
                        <div class="form-group">
                            <div class="col-md-2">
                                Tipo
                    <asp:DropDownList ID="drpConTipoPasseio" Width="100%" CssClass=" form-control" runat="server" AutoPostBack="True">
                        <asp:ListItem Value="PE">Todos</asp:ListItem>
                        <asp:ListItem Value="P">Passeio</asp:ListItem>
                        <%--Utilizado pelo Laércio--%>
                        <asp:ListItem Value="X">Excursão</asp:ListItem>
                        <%--Utilizado pelo Eduardo--%>
                        <asp:ListItem Value="E">Grupos</asp:ListItem>
                        <%--Utilizado pela Luzia--%>
                    </asp:DropDownList>
                            </div>

                            <div class="col-md-2 ">
                                Situação
                        <asp:DropDownList ID="drpConSituacao" Width="100%" CssClass="form-control" runat="server">
                            <asp:ListItem Value="TA">Todas ativas</asp:ListItem>
                            <asp:ListItem Value="T">Todas</asp:ListItem>
                            <asp:ListItem Value="I">Falta integrante</asp:ListItem>
                            <asp:ListItem Value="P">Falta pagamento</asp:ListItem>
                            <asp:ListItem Value="R">Confirmada</asp:ListItem>
                            <asp:ListItem Value="C">Cancelada</asp:ListItem>
                            <asp:ListItem Value="E">Em estada</asp:ListItem>
                            <asp:ListItem Value="F">Finalizada</asp:ListItem>
                        </asp:DropDownList>
                            </div>

                            <div class="col-md-3">
                                Nome
                <asp:TextBox ID="txtConNomePasseio" CssClass="form-control" Width="100%" runat="server"></asp:TextBox>
                            </div>
                            <div class="col-md-1 text-nowrap">
                                <asp:Label ID="lblConDataInicial" CssClass="text-nowrap" Text="Data Inicial" runat="server"></asp:Label>
                                <asp:TextBox ID="txtConDataInicial" CssClass="form-control" Width="125%" runat="server" MaxLength="10"></asp:TextBox>
                                <asp:CalendarExtender ID="txtConDataInicial_CalendarExtender" runat="server" BehaviorID="txtConDataInicial_CalendarExtender" FirstDayOfWeek="Sunday" Format="dd/MM/yyyy" TargetControlID="txtConDataInicial">
                                </asp:CalendarExtender>
                            </div>
                            <div class="col-md-1 text-nowrap">
                                <asp:Label ID="lblConDataFinal" CssClass="text-nowrap" Text="Data Final" runat="server"></asp:Label>
                                <asp:TextBox ID="txtConDataFinal" CssClass="form-control" Width="125%" runat="server" MaxLength="10"></asp:TextBox>
                                <asp:CalendarExtender ID="txtConDataFinal_CalendarExtender" runat="server" BehaviorID="txtConDataFinal_CalendarExtender" FirstDayOfWeek="Sunday" Format="dd/MM/yyyy" TargetControlID="txtConDataFinal">
                                </asp:CalendarExtender>
                            </div>

                            <div class="col-md-1">
                                <br />
                                <asp:Button ID="btnConConsultar" runat="server" Width="100px" CssClass="btn-primary form-control" Text="Consultar" />
                            </div>

                            <div class="col-md-1">
                                <br />
                                <asp:Button ID="btnConNovo" runat="server" Width="100px" Text="Novo" CssClass="btn-primary form-control" />
                            </div>

                            <br />
                            <br />
                            <br />
                        </div>
                    </div>

                    <div class="row container-fluid">
                        <div class="col-md-12">
                            <asp:GridView ID="gdvConReservas" CssClass="form-control" runat="server" CellPadding="4" DataKeyNames="resId,resStatus,resDataFim,resValorPagoAntecipado,resNome,resCaracteristica,resDataIni,resValorPago,resUsuario,resDtLimiteRetorno,resDtInsercao,ResUsuario,ResUsuarioData" ForeColor="#333333" GridLines="None" AutoGenerateColumns="False" ShowFooter="True">
                                <AlternatingRowStyle BackColor="White" />
                                <Columns>
                                    <asp:TemplateField HeaderText="Responsável">
                                        <ItemTemplate>
                                            <asp:LinkButton ID="lnkResponsavel" runat="server" OnClick="lnkResponsavel_Click">LinkButton</asp:LinkButton>
                                        </ItemTemplate>
                                        <ItemStyle CssClass="ColocaHand" />
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="ResStatusDesc" HeaderText="Status">
                                        <ItemStyle CssClass="ColocaHand" />
                                    </asp:BoundField>
                                    <asp:TemplateField HeaderText="Período">
                                        <ItemTemplate>
                                            <asp:Label ID="lblPeriodo" runat="server"></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle CssClass="ColocaHand" />
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="qtdHospede" HeaderText="Integrante">
                                        <ItemStyle HorizontalAlign="Center" CssClass="ColocaHand" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="qtdAcomodacao" HeaderText="Apartamento">
                                        <ItemStyle HorizontalAlign="Center" CssClass="ColocaHand" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="resValorPago" HeaderText="Valor Pago">
                                        <FooterStyle HorizontalAlign="Right" />
                                        <ItemStyle HorizontalAlign="Right" CssClass="ColocaHand" />
                                    </asp:BoundField>
                                    <asp:TemplateField HeaderText="Usuário">
                                        <ItemTemplate>
                                            <asp:Image ID="imgUsuario" runat="server" ImageUrl="~/images/Responsavel.png" />
                                        </ItemTemplate>
                                        <ItemStyle CssClass="ColocaHand" HorizontalAlign="Center" />
                                    </asp:TemplateField>
                                </Columns>
                                <EditRowStyle BackColor="#2461BF" />
                                <FooterStyle Font-Bold="True" ForeColor="White" CssClass="BarraTituloEmissivo ArrendodarBorda" Font-Italic="False" Font-Size="16px" />
                                <HeaderStyle Font-Bold="True" ForeColor="White" CssClass="BarraTituloEmissivo ArrendodarBorda" Font-Italic="False" Font-Size="18px" />
                                <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
                                <RowStyle BackColor="#EFF3FB" />
                                <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                                <SortedAscendingCellStyle BackColor="#F5F7FB" />
                                <SortedAscendingHeaderStyle BackColor="#6D95E1" />
                                <SortedDescendingCellStyle BackColor="#E9EBEF" />
                                <SortedDescendingHeaderStyle BackColor="#4870BE" />
                            </asp:GridView>
                        </div>
                </asp:Panel>
                <br />
                <asp:Panel ID="pnlDadosPasseio" CssClass="ArrendodarBordaEmissivo" Height="100%" runat="server" Visible="false">
                    <asp:Panel ID="pnlTodosDadosReserva" runat="server">
                        <div class="form-group container-fluid">
                            <div id="div1" runat="server" class="BarraTituloEmissivo ArrendodarBorda container" align="center" style="width: 100%; height: 40px;">
                                <asp:Label ID="lblDadosPasseio" CssClass="container" Width="100%" runat="server" Text="Dados do passeio"></asp:Label>
                            </div>
                            <br />
                            <div runat="server" align="left">
                                <%-- Primeira linha dos Dados Cadastrais --%>
                                <div class="row">
                                    <div class="col-md-2">
                                        Tipo
                            <asp:DropDownList ID="drpResTipoPasseio" Width="100%" runat="server" CssClass="form-control " AutoPostBack="True">
                                <asp:ListItem Value="P">Passeio</asp:ListItem>
                                <asp:ListItem Value="X">Excursão</asp:ListItem>
                                <%--Passeios Laercio--%>
                                <asp:ListItem Value="E">Grupos</asp:ListItem>
                                <%--Grupos da Luzia--%>
                            </asp:DropDownList>
                                    </div>

                                    <div class="col-md-3">
                                        Nome
                            <asp:TextBox ID="txtResNomePasseio" runat="server" CssClass="form-control" ToolTip="Digite o novo nome para o passeio e clique em salvar" Visible="False" Width="100%"></asp:TextBox>
                                        <asp:DropDownList ID="drpResNomePasseio" CssClass="form-control" runat="server" Width="100%" AutoPostBack="True">
                                        </asp:DropDownList>
                                    </div>

                                    <div class="col-md-1" runat="server" id="divBotoesNovoPasseio">
                                        <br />
                                        <div class="col-md-6" runat="server" id="divSalvarNovoPasseio">
                                            <asp:Button ID="btnAdicionaNomePasseio" runat="server" CssClass="btn-primary form-control" Width="110%" Text="+" ToolTip="Adicionar novos nomes de passeio na base de dados" TabIndex="-1" />
                                        </div>
                                        <div class="col-md-6" runat="server" id="divCancelaNovoPasseio" visible="false">
                                            <asp:Button ID="btnCancelaNovoNomePasseio" runat="server" CssClass="btn-primary form-control" Width="110%" Text="Cancelar" Visible="true" TabIndex="-1" />
                                        </div>
                                    </div>

                                    <div class="col-md-2 text-nowrap">
                                        <asp:Label ID="lblDadosDataInicialPasseio" runat="server" CssClass="" Text="Data do passeio"></asp:Label>
                                        <asp:TextBox ID="txtResDataInicialPasseio" CssClass="form-control" runat="server" Width="100%" MaxLength="10"></asp:TextBox>
                                    </div>

                                    <div class="col-md-2">
                                        <asp:Label ID="lblDadosDataFinalPasseio" runat="server" Text="Data final" Visible="false"></asp:Label>
                                        <asp:TextBox ID="txtResDataFinalPasseio" CssClass="form-control" runat="server" Width="100%" Visible="false" MaxLength="10"></asp:TextBox>
                                    </div>
                                    <asp:CalendarExtender ID="txtResDataFinalPasseio_CalendarExtender" runat="server" BehaviorID="txtResDataFinalPasseio_CalendarExtender" FirstDayOfWeek="Sunday" Format="dd/MM/yyyy" TargetControlID="txtResDataFinalPasseio">
                                    </asp:CalendarExtender>
                                    <asp:CalendarExtender ID="txtResDataInicialPasseio_CalendarExtender" runat="server" BehaviorID="txtResDataPasseio_CalendarExtender" FirstDayOfWeek="Sunday" Format="dd/MM/yyyy" TargetControlID="txtResDataInicialPasseio">
                                    </asp:CalendarExtender>

                                    <div class="col-md-2 text-nowrap">
                                        <br />
                                        <asp:CheckBox ID="chkDadosOrganizadoSesc" Text=" Organizado pelo Sesc" CssClass="form-control" Width="100%" runat="server" AutoPostBack="True" Checked="True" />
                                    </div>

                                    <%--<div class="col-md-2 text-nowrap">
                            Base para geração do boleto
                            <asp:TextBox runat="server" ID="txtResBaseBoleto" CssClass="form-control" Width="100%"></asp:TextBox>
                            <asp:CalendarExtender ID="txtResBaseBoleto_CalendarExtender" runat="server" BehaviorID="txtResBaseBoleto_CalendarExtender" FirstDayOfWeek="Sunday" Format="dd/MM/yyyy" TargetControlID="txtResBaseBoleto">
                            </asp:CalendarExtender>
                        </div>--%>
                                </div>


                                <%-- Segunda linha dos dados cadastrais --%>
                                <div class="row">
                                    <div class="col-md-4">
                                        Responsável
                            <asp:TextBox runat="server" ID="txtResResponsavel" CssClass="form-control" Width="100%"></asp:TextBox>
                                        <asp:DropDownList runat="server" ID="drpResResponsavel" CssClass="form-control" Width="100%" AutoPostBack="true" Visible="false"></asp:DropDownList>
                                    </div>

                                    <div class="col-md-2">
                                        Telefone Comercial
                            <asp:TextBox runat="server" ID="txtResTelComercial" CssClass="form-control" Width="100%" MaxLength="14"></asp:TextBox>
                                    </div>


                                    <div class="col-md-2">
                                        Telefone Residencial
                            <asp:TextBox runat="server" CssClass="form-control" ID="txtResTelResidencial" Width="100%" MaxLength="14"></asp:TextBox>
                                    </div>

                                    <div class="col-md-2">
                                        Telefone Celular
                            <asp:TextBox runat="server" CssClass="form-control" ID="txtResTelCelular" Width="100%" MaxLength="14"></asp:TextBox>
                                    </div>

                                    <div class="col-md-2">
                                        Categoria padrão
                            <asp:DropDownList runat="server" CssClass="form-control" ID="drpResCategoria" Width="100%" AutoPostBack="True"></asp:DropDownList>
                                    </div>
                                </div>

                                <%-- terceira linha do dados cadastrais --%>
                                <div class="row">
                                    <div class="col-md-2 text-nowrap">
                                        Categoria de Cobrança 
                            <asp:DropDownList ID="drpResCategoriaCobranca" CssClass="form-control" Width="100%" runat="server"></asp:DropDownList>
                                    </div>

                                    <div class="col-md-2">
                                        Memorando
                            <asp:TextBox ID="txtResMemorando" CssClass="form-control" Width="100%" runat="server"></asp:TextBox>
                                    </div>

                                    <div class="col-md-1">
                                        Emissor
                            <asp:DropDownList ID="drpResEmissor" CssClass="form-control" Width="100%" AutoPostBack="true" runat="server">
                                <asp:ListItem Value="0">_</asp:ListItem>
                                <asp:ListItem Value="1">GP</asp:ListItem>
                                <asp:ListItem Value="2">DR</asp:ListItem>
                            </asp:DropDownList>
                                    </div>

                                    <div class="col-md-7">
                                        Observação
                            <asp:TextBox ID="txtResObservacao" CssClass="form-control" Width="100%" runat="server"></asp:TextBox>
                                    </div>


                                </div>

                                <div class="row">
                                    <div class="col-md-5" runat="server" id="divResLocalRefeicao">
                                        Local da refeição
                             <asp:DropDownList ID="drpResLocalRefeicaoPadrao" CssClass="form-control" Width="100%" runat="server" AutoPostBack="True"></asp:DropDownList>
                                    </div>
                                    <div class="col-md-3">
                                        Estado
                            <asp:DropDownList ID="drpResEstado" CssClass="form-control" Width="100%" runat="server" AutoPostBack="True"></asp:DropDownList>
                                    </div>
                                    <div class="col-md-4">
                                        Destino
                            <asp:DropDownList ID="drpResCidade" CssClass="form-control" Width="100%" AutoPostBack="true" runat="server"></asp:DropDownList>
                                    </div>
                                </div>

                                <div class="row" runat="server" id="divResDadosHotelSaidaHorasSaida">
                                    <div class="col-md-3 text-nowrap" runat="server" id="divResHotel">
                                        Hotel 
                            <asp:TextBox ID="txtResHotel" CssClass="form-control" Width="100%" runat="server"></asp:TextBox>
                                    </div>
                                    <div class="col-md-3 text-nowrap" runat="server" id="divResLocalSaida">
                                        Local de Saída
                            <asp:DropDownList ID="drpResLocalSaida" AutoPostBack="true" CssClass="form-control" Width="100%" runat="server"></asp:DropDownList>
                                    </div>
                                    <div class="col-md-1 text-nowrap" runat="server" id="divResHoraSaida">
                                        Horas 
                            <asp:DropDownList ID="drpResHoraSaida" CssClass="form-control" AutoPostBack="true" Width="100%" runat="server">
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
                                    </div>

                                    <div class="col-md-5">
                                        <label runat="server" id="lblDadosTipoRefeicaoPadrao" visible="false">Refeição padrão</label>
                                        <asp:DropDownList ID="drpResTipoRefeicaoPadrao" runat="server" CssClass="form-control" Width="100%" Visible="False"></asp:DropDownList>
                                    </div>
                                </div>

                                <asp:Panel runat="server" ID="pnlCamposGrupo" Visible="false">
                                    <div class="row">
                                        <div class="col-md-2">
                                            Confirmar até
                                <asp:TextBox ID="txtResConfirmar" runat="server" CssClass="form-control" Width="100%" MaxLength="10"></asp:TextBox>
                                            <asp:CalendarExtender ID="txtResConfirmar_CalendarExtender" runat="server" BehaviorID="txtResConfirmar_CalendarExtender" FirstDayOfWeek="Sunday" Format="dd/MM/yyyy" TargetControlID="txtResConfirmar">
                                            </asp:CalendarExtender>
                                        </div>
                                        <div class="col-md-2">
                                            pagar sinal até
                                <asp:TextBox ID="txtResPagarSinal" runat="server" CssClass="form-control" Width="100%" MaxLength="10"></asp:TextBox>
                                            <asp:CalendarExtender ID="txtResPagarSinal_CalendarExtender" runat="server" BehaviorID="txtResPagarSinal_CalendarExtender" FirstDayOfWeek="Sunday" Format="dd/MM/yyyy" TargetControlID="txtResPagarSinal">
                                            </asp:CalendarExtender>
                                        </div>
                                        <div class="col-md-2">
                                            Digitar até
                                <asp:TextBox ID="txtResDigitar" runat="server" CssClass="form-control" Width="100%" MaxLength="10"></asp:TextBox>
                                            <asp:CalendarExtender ID="txtResDigitar_CalendarExtender" runat="server" BehaviorID="txtResDigitar_CalendarExtender" FirstDayOfWeek="Sunday" Format="dd/MM/yyyy" TargetControlID="txtResDigitar">
                                            </asp:CalendarExtender>
                                        </div>
                                        <div class="col-md-2">
                                            Quitar até
                                <asp:TextBox ID="txtResQuitar" runat="server" CssClass="form-control" Width="100%" MaxLength="10"></asp:TextBox>
                                            <asp:CalendarExtender ID="txtResQuitar_CalendarExtender" runat="server" BehaviorID="txtResQuitar_CalendarExtender" FirstDayOfWeek="Sunday" Format="dd/MM/yyyy" TargetControlID="txtResQuitar">
                                            </asp:CalendarExtender>
                                        </div>
                                        <div class="col-md-4">
                                            Usuário internet
                                <asp:DropDownList ID="drpResUsuarioInternet" runat="server" CssClass="form-control" Width="100%"></asp:DropDownList>
                                        </div>
                                    </div>

                                    <%--<div class="row">
                            <div class="col-md-4">
                                Usuário internet
                                <asp:TextBox ID="txtResObs" runat="server" CssClass="form-control" Width="100%"></asp:TextBox>
                            </div>
                        </div>--%>
                                </asp:Panel>
                                <%--Fechando divTodosDadosReserva--%>
                            </div>
                    </asp:Panel>

                <div class="row">
                    <br />
                    <div class="col-md-1">
                        <asp:Button ID="btnSalvarDadosPasseio" runat="server" CssClass="btn-primary form-control" Text="Salvar" Width="90px" />
                    </div>

                    <div class="col-md-1">
                        <asp:Button ID="btnCancelarReserva" runat="server" Width="90px" Text="Cancelar" CssClass="btn-primary form-control" />
                    </div>
                    <div class="col-md-1">
                        <asp:Button ID="btnReativarReserva" runat="server" Width="90px" Text="Ativar" CssClass="btn-primary form-control" />
                    </div>
                    <div class="col-md-1">
                        <asp:Button ID="btnVoltaDadosPasseio" runat="server" CssClass="btn-primary form-control" Text="Voltar" Width="90px" />
                    </div>
                </div>
                </asp:Panel>

            </div>
            <asp:HiddenField ID="hddResId" runat="server" />
            <asp:HiddenField ID="hddResCaracteristica" runat="server" />
            <asp:HiddenField ID="hddResStatus" runat="server" />
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>

