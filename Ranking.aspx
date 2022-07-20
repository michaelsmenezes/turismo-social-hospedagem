<%@ Page Title="" Language="VB" MasterPageFile="~/TurismoSocial.master" AutoEventWireup="True" CodeFile="Ranking.aspx.vb" Inherits="Ranking" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="conPlaHolTurismoSocial" runat="Server">
    <br />
    <div class="formLabel" align="center">
        <asp:panel id="pnlProgresso" runat="server" cssclass="PosicionaProgresso">
            <asp:UpdateProgress ID="updProRecepcao" runat="server" AssociatedUpdatePanelID="updPnlRanking">
                <ProgressTemplate>
                    Processando sua solicitação...<br />
                    &nbsp;<img alt="Processando..." src="images/Aguarde.gif" />
                </ProgressTemplate>
            </asp:UpdateProgress>
        </asp:panel>
    </div>

    <asp:updatepanel id="updPnlRanking" runat="server">
        <ContentTemplate>
            <br />
            <br />
            <br />
            <br />
            <br />
            <br />
            <table class="formLabel" cellpadding="0" cellspacing="0" width="100%">
                <tr>
                    <td>
                        <asp:Panel ID="pnlRanking" runat="server" CssClass="tableRowOdd" Width="100%" DefaultButton="btnConsulta">
                            <table cellpadding="0" cellspacing="0" width="100%">
                                <tr>
                                    <td></td>
                                    <td></td>
                                    <td></td>
                                    <td></td>
                                    <td></td>
                                    <td></td>
                                </tr>
                                <tr>
                                    <td width="3%"></td>
                                    <td width="15%">
                                        <asp:Label ID="lblRankingOpcao" runat="server" Text="Opção desejada"
                                            Font-Size="Medium" CssClass="formLabel2"></asp:Label>
                                        <br />
                                        <br />
                                        <asp:RadioButtonList ID="rblRankingOpcao" runat="server" CssClass="formLabel2" Font-Size="Medium" AutoPostBack="True">
                                            <asp:ListItem Selected="True" Value="S"> Ranking</asp:ListItem>
                                            <asp:ListItem Value="N"> Enviadas</asp:ListItem>
                                            <asp:ListItem Value="L"> Lista de Espera</asp:ListItem>
                                            <asp:ListItem Value="T"> Lista de Trabalho</asp:ListItem>
                                        </asp:RadioButtonList>

                                    </td>

                                    <td width="20%">
                                        <asp:Label ID="lblRankingDestino" runat="server" Text="Destino"
                                            Font-Size="Medium" CssClass="formLabel2"></asp:Label>
                                        <br />
                                        <br />
                                        <asp:RadioButtonList ID="rblRankingDestino" runat="server" CssClass="formLabel2" Font-Size="Medium" AutoPostBack="True">
                                            <asp:ListItem Selected="True" Value="C"> SESC Caldas Novas</asp:ListItem>
                                            <asp:ListItem Value="P"> Pousada SESC Pirenópolis</asp:ListItem>
                                        </asp:RadioButtonList>
                                        <br />
                                        <br />
                                        <asp:Label ID="lblRankingPercentual" runat="server" Text="Percentual"
                                            Font-Size="Medium" CssClass="formLabel2"></asp:Label>
                                        <br />
                                        <br />
                                        <div style="float: left; margin-right: 10px;">
                                            <asp:TextBox ID="txtRankingPercentual" runat="server">10</asp:TextBox>
                                            <asp:SliderExtender ID="txtRankingPercentual_SliderExtender" runat="server" Enabled="True" Maximum="100" Minimum="0" TargetControlID="txtRankingPercentual" BoundControlID="txtRankingPercentualChar">
                                            </asp:SliderExtender>

                                        </div>
                                        <div style="float: left;">
                                            <asp:TextBox ID="txtRankingPercentualChar" runat="server" Width="30px">10</asp:TextBox>
                                            <asp:Label ID="lblRankingPercentualChar" runat="server" Text="%" Font-Size="Medium" CssClass="formLabel2"></asp:Label>
                                        </div>
                                    </td>
                                    <td width="15%" align="center">
                                        <asp:Label ID="lblChegada" runat="server" Text="Chegada"
                                            Font-Size="Medium" CssClass="formLabel2"></asp:Label>
                                        <asp:Calendar ID="calChegada" runat="server" BackColor="White" BorderColor="White" BorderWidth="0px" Font-Names="Verdana" Font-Size="9pt" ForeColor="Black" Height="190px" NextMonthText="&gt;&gt;" PrevMonthText="&lt;&lt;" CaptionAlign="Top" Width="250px" NextPrevFormat="ShortMonth">
                                            <SelectedDayStyle BackColor="#333399" ForeColor="White" />
                                            <TodayDayStyle BackColor="#CCCCCC" />
                                            <OtherMonthDayStyle ForeColor="#999999" />
                                            <NextPrevStyle Font-Size="8pt" ForeColor="#333333" Font-Bold="True" VerticalAlign="Bottom" />
                                            <DayHeaderStyle Font-Bold="True" Font-Size="8pt" />
                                            <TitleStyle BackColor="White" Font-Bold="False" Font-Size="12pt" ForeColor="#333399" BorderColor="Black" BorderWidth="1px" />
                                        </asp:Calendar>
                                    </td>
                                    <td width="3%"></td>
                                    <td width="15%" align="center">
                                        <asp:Label ID="lblSaida" runat="server" Text="Saída"
                                            Font-Size="Medium" CssClass="formLabel2"></asp:Label>
                                        <asp:Calendar ID="calSaida" runat="server" BackColor="White" BorderColor="White" BorderWidth="0px" Font-Names="Verdana" Font-Size="9pt" ForeColor="Black" Height="190px" NextMonthText="&gt;&gt;" PrevMonthText="&lt;&lt;" CaptionAlign="Top" Width="250px" NextPrevFormat="ShortMonth">
                                            <SelectedDayStyle BackColor="#333399" ForeColor="White" />
                                            <TodayDayStyle BackColor="#CCCCCC" />
                                            <OtherMonthDayStyle ForeColor="#999999" />
                                            <NextPrevStyle Font-Size="8pt" ForeColor="#333333" Font-Bold="True" VerticalAlign="Bottom" />
                                            <DayHeaderStyle Font-Bold="True" Font-Size="8pt" />
                                            <TitleStyle BackColor="White" Font-Bold="False" Font-Size="12pt" ForeColor="#333399" BorderColor="Black" BorderWidth="1px" />
                                        </asp:Calendar>
                                    </td>
                                    <td width="19%" align="center" valign="bottom">
                                        <asp:Button ID="btnConsulta" runat="server" BackColor="#0066FF" Font-Bold="True" Font-Size="Medium" ForeColor="White" Height="35px" Text="Consultar" Width="130px" OnClientClick="this.disabled = true;" UseSubmitBehavior="False" />

                                        <br />
                                        <br />
                                        <br />
                                        <br />
                                        <br />

                                        <div style="vertical-align: bottom; margin-left: 15px;">
                                            <asp:ImageButton ID="imgHistorico" runat="server" CssClass="ColocaHand" ImageUrl="~/images/HistoricoHospede.png" ToolTip="Histórico e Grupos" Height="32px" Width="32px" />
                                            &nbsp;&nbsp;&nbsp;
                                            <asp:ImageButton ID="imgPercentual" runat="server" CssClass="ColocaHand" ImageUrl="~/images/Pizza.png" ToolTip="Percentual de Ocupação" Height="32px" Width="32px" Visible="False" />
                                            &nbsp;&nbsp;&nbsp;
                                            <asp:ImageButton ID="imgConfiguracao" runat="server" CssClass="ColocaHand" ImageUrl="~/images/distribuir.png" ToolTip="Configurações do sistema" Height="32px" Width="32px" Visible="False" />
                                        </div>
                                    </td>

                                </tr>
                                <tr>
                                    <td>&nbsp;</td>
                                </tr>
                                <tr align="center">
                                    <td colspan="7">
                                        <asp:Panel ID="pnlRankingAcao" runat="server">
                                            <asp:GridView ID="gdvRankingAcao" runat="server" AutoGenerateColumns="False" CellPadding="4" DataKeyNames="resId,solId,resSexo,resEmail,resNome,dtIni,dtFim,intNome1,catId1,intNome2,catId2,intNome3,catId3,intNome4,catId4,intNome5,catId5,intNome6,catId6,intNome7,catId7,intDtNascimento1,intDtNascimento2,intDtNascimento3,intDtNascimento4,intDtNascimento5,intDtNascimento6,intDtNascimento7,intNome1Atendido,intNome2Atendido,intNome3Atendido,intNome4Atendido,intNome5Atendido,intNome6Atendido,intNome7Atendido,intCatId1Atendido,intCatId2Atendido,intCatId3Atendido,intCatId4Atendido,intCatId5Atendido,intCatId6Atendido,intCatId7Atendido,intDtNascimento1Atendido,intDtNascimento2Atendido,intDtNascimento3Atendido,intDtNascimento4Atendido,intDtNascimento5Atendido,intDtNascimento6Atendido,intDtNascimento7Atendido,resDataIni,resDataFim,intRepetido1,intRepetido2,intRepetido3,intRepetido4,intRepetido5,intRepetido6,intRepetido7,intRepetidoAtendido1,intRepetidoAtendido2,intRepetidoAtendido3,intRepetidoAtendido4,intRepetidoAtendido5,intRepetidoAtendido6,intRepetidoAtendido7,situacao,SolDtListaEspera,SolDtSemDisponibilidade,SolDtEnviado,ResFonePrefixo,ResCelular,resDtLimiteRetorno"
                                                EmptyDataText="Atenção! Não foi encontrada nenhuma solicitação." ShowFooter="True" Width="99%" BackColor="White" CssClass="formLabel" Font-Size="Medium">
                                                <AlternatingRowStyle CssClass="tableRowOdd" />
                                                <EmptyDataRowStyle BorderColor="White" BorderStyle="Solid" Height="100px" HorizontalAlign="Center" VerticalAlign="Middle" Font-Size="Medium" />
                                                <Columns>
                                                    <asp:TemplateField>
                                                        <HeaderTemplate>
                                                            <asp:CheckBox ID="ckbHeadResponsavel" runat="server" AutoPostBack="True" OnCheckedChanged="ckbHeadResponsavel_CheckedChanged" />
                                                        </HeaderTemplate>
                                                        <ItemTemplate>
                                                            <asp:CheckBox ID="ckbItemResponsavel" runat="server" AutoPostBack="True" OnCheckedChanged="ckbItemResponsavel_CheckedChanged" BorderWidth="1px" />
                                                        </ItemTemplate>
                                                        <FooterStyle CssClass="gridBorderColor" />
                                                        <HeaderStyle BorderWidth="0px" CssClass="formRuler" />
                                                        <ItemStyle />
                                                    </asp:TemplateField>

                                                    <asp:TemplateField HeaderText="Responsável">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lnkResponsavel" runat="server" Text='<%# Bind("resNome") %>'></asp:Label>
                                                            <asp:ImageButton ID="imgBtnResponsavel" runat="server" ImageAlign="AbsMiddle" ImageUrl="~/images/Info.png" ToolTip="Informações" OnClick="imgBtnResponsavel_Click" />
                                                        </ItemTemplate>
                                                        <FooterTemplate>
                                                            <asp:Label ID="lblFtrSolicitacao" runat="server" Text="Total de Solicitações"></asp:Label>
                                                        </FooterTemplate>
                                                        <HeaderTemplate>
                                                            <asp:Label ID="lblHeadResponsavel" runat="server" ForeColor="White">Responsável</asp:Label>
                                                        </HeaderTemplate>
                                                        <FooterStyle CssClass="formLabel" HorizontalAlign="Center" />
                                                        <HeaderStyle BorderWidth="0px" CssClass="formRuler" />
                                                        <ItemStyle CssClass="formLabelEscuro" HorizontalAlign="Left" />
                                                    </asp:TemplateField>

                                                    <asp:TemplateField HeaderText="Histórico">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblHistorico" runat="server"></asp:Label>
                                                        </ItemTemplate>

                                                        <HeaderTemplate>
                                                            <asp:Label ID="lblHeadHistorico" runat="server" ForeColor="White">Histórico</asp:Label>
                                                        </HeaderTemplate>
                                                        <FooterStyle CssClass="formLabel" HorizontalAlign="Center" />
                                                        <HeaderStyle BorderWidth="0px" CssClass="formRuler" />
                                                        <ItemStyle CssClass="formLabelEscuro" HorizontalAlign="Center" />
                                                    </asp:TemplateField>

                                                    <asp:TemplateField HeaderText="Acomodação">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblAcomodacao" runat="server" Text='<%# Bind("aWebDescricao") %>'></asp:Label>
                                                        </ItemTemplate>
                                                        <HeaderTemplate>
                                                            <asp:Label ID="lblHeadAcomodacao" runat="server" ForeColor="White">Acomodação</asp:Label>
                                                        </HeaderTemplate>
                                                        <FooterStyle CssClass="formLabel" HorizontalAlign="Center" />
                                                        <HeaderStyle BorderWidth="0px" CssClass="formRuler" />
                                                        <ItemStyle CssClass="formLabelEscuro" HorizontalAlign="Center" />
                                                    </asp:TemplateField>

                                                    <asp:TemplateField HeaderText="Período">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblPeriodo" runat="server"></asp:Label>
                                                            &nbsp;
                                                        </ItemTemplate>

                                                        <HeaderTemplate>
                                                            <asp:Label ID="lblHeadPeriodo" runat="server" ForeColor="White">Período</asp:Label>
                                                        </HeaderTemplate>
                                                        <FooterStyle CssClass="formLabel" HorizontalAlign="Center" />
                                                        <HeaderStyle BorderWidth="0px" CssClass="formRuler" />
                                                        <ItemStyle CssClass="formLabelEscuro" HorizontalAlign="Center" />
                                                    </asp:TemplateField>

                                                    <asp:TemplateField HeaderText="Integrantes">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblIntegrantes" runat="server"></asp:Label>
                                                        </ItemTemplate>

                                                        <HeaderTemplate>
                                                            <asp:Label ID="lblHeadBtnIntegrantes" runat="server" ForeColor="White">Integrantes</asp:Label>
                                                        </HeaderTemplate>
                                                        <FooterStyle CssClass="formLabel" HorizontalAlign="Center" />
                                                        <HeaderStyle BorderWidth="0px" CssClass="formRuler" />
                                                        <ItemStyle CssClass="formLabelEscuro" HorizontalAlign="Left" />
                                                    </asp:TemplateField>

                                                    <asp:TemplateField HeaderText="Limite">
                                                        <HeaderTemplate>
                                                        </HeaderTemplate>
                                                        <ControlStyle />
                                                        <FooterStyle BorderWidth="0px" CssClass="formRuler" />
                                                        <HeaderStyle BorderWidth="0px" CssClass="formRuler" />
                                                        <ItemStyle CssClass="formRuler" BorderWidth="0px" />
                                                    </asp:TemplateField>

                                                    <asp:TemplateField HeaderText="Atendidos">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblIntegrantesAtendidos" runat="server"></asp:Label>
                                                        </ItemTemplate>
                                                        <HeaderTemplate>
                                                            <asp:Label ID="lblHeadItegrantesAtendidos" runat="server" ForeColor="White">Atendidos</asp:Label>
                                                        </HeaderTemplate>
                                                        <FooterStyle CssClass="formLabel" HorizontalAlign="Center" />
                                                        <HeaderStyle BorderWidth="0px" CssClass="formRuler" />
                                                        <ItemStyle CssClass="formLabelEscuro" HorizontalAlign="Left" />
                                                    </asp:TemplateField>

                                                    <asp:TemplateField HeaderText="Reserva">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblReserva" runat="server" Text='<%# Bind("aWebDescricaoAtendido")%>'></asp:Label>
                                                        </ItemTemplate>
                                                        <HeaderTemplate>
                                                            <asp:Label ID="lblHeadReserva" runat="server" ForeColor="White">Reserva</asp:Label>
                                                        </HeaderTemplate>
                                                        <FooterStyle CssClass="formLabel" HorizontalAlign="Center" />
                                                        <HeaderStyle BorderWidth="0px" CssClass="formRuler" />
                                                        <ItemStyle CssClass="formLabelEscuro" HorizontalAlign="Center" />
                                                    </asp:TemplateField>

                                                    <asp:TemplateField HeaderText="Situação">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblSituacao" runat="server" Text='<%# Bind("condicao")%>'></asp:Label>
                                                        </ItemTemplate>
                                                        <HeaderTemplate>
                                                            <asp:Label ID="lblHeadSituacao" runat="server" ForeColor="White" ToolTip="Ordenar">Situação</asp:Label>
                                                        </HeaderTemplate>
                                                        <FooterStyle CssClass="formLabel" HorizontalAlign="Center" />
                                                        <HeaderStyle BorderWidth="0px" CssClass="formRuler" />
                                                        <ItemStyle CssClass="formLabelEscuro" HorizontalAlign="Center" />
                                                    </asp:TemplateField>
                                                </Columns>
                                                <HeaderStyle ForeColor="White" />
                                                <SelectedRowStyle Font-Bold="True" />
                                            </asp:GridView>
                                        </asp:Panel>
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
                        <asp:RoundedCornersExtender ID="pnlRanking_RoundedCornersExtender" runat="server" Enabled="True" TargetControlID="pnlRanking">
                        </asp:RoundedCornersExtender>
                    </td>
                </tr>
                <tr>
                    <td>&nbsp;</td>
                </tr>
                <tr>
                    <td>
                        <asp:Panel ID="pnlAcao" runat="server" Width="100%" CssClass="tableRowOdd" Visible="False" Height="50px">
                            <table width="100%" style="height: 50px">
                                <tr>
                                    <td align="right">
                                        <asp:Button ID="btnAtender" runat="server" Height="35px" Text="Atender" BackColor="Green" Font-Bold="True" Font-Size="Medium" ForeColor="White" Width="140px" OnClientClick="return confirm('Confirma atendimento?')" />
                                    </td>
                                    <td align="center">
                                        <asp:Button ID="btnFila" runat="server" Height="35px" Text="Lista de Espera" BackColor="Orange" Font-Bold="True" Font-Size="Medium" ForeColor="White" Width="140px" OnClientClick="return confirm('Confirma transferência para Lista de Espera?')" ToolTip="Voltará ao ranking em breve" />
                                    </td>
                                    <td>
                                        <asp:Button ID="btnPostergar" runat="server" Height="35px" Text="Descartar" BackColor="Red" Font-Bold="True" Font-Size="Medium" ForeColor="White" Width="140px" OnClientClick="return confirm('Confirma transferência para Lista de Trabalho?')" ToolTip="Vai para lista de trabalho" />
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
                        <asp:AlwaysVisibleControlExtender ID="pnlAcao_AlwaysVisibleControlExtender" runat="server" Enabled="True" TargetControlID="pnlAcao" VerticalSide="Bottom">
                        </asp:AlwaysVisibleControlExtender>
                    </td>
                </tr>
                <tr>
                    <td align="center" width="100%">

                        <asp:Panel runat="server" Visible="False" ID="pnlPercentual" CssClass="formLabel2" Font-Size="Medium" Width="100%">
                            <asp:Label ID="lblPercentual" runat="server" CssClass="formLabel2" Font-Size="Medium" Text=""></asp:Label>
                            <br />
                            <br />
                            <asp:GridView ID="gdvPercentual" runat="server" AutoGenerateColumns="False" BackColor="White" CellPadding="4" CssClass="formLabel" DataKeyNames="aptoE,aptoI,aptoM,aptoP,aptoR,acmQtde" Font-Size="Medium" HorizontalAlign="Center" >
                                <AlternatingRowStyle CssClass="tableRowOdd" />
                                <EmptyDataRowStyle BorderColor="White" BorderStyle="Solid" Font-Size="Medium" Height="100px" HorizontalAlign="Center" VerticalAlign="Middle" />
                                <Columns>
                                    <asp:TemplateField HeaderText="Bloco">
                                        <ItemTemplate>
                                            <asp:Label ID="lblBloco" runat="server" Text='<%# Bind("bloco")%>'></asp:Label>
                                        </ItemTemplate>
                                        <HeaderTemplate>
                                            <asp:Label ID="lblHeadBloco" runat="server" ForeColor="White">Bloco</asp:Label>
                                        </HeaderTemplate>
                                        <FooterStyle CssClass="formLabel" HorizontalAlign="Center" />
                                        <HeaderStyle BorderWidth="0px" CssClass="formRuler" />
                                        <ItemStyle CssClass="formLabelEscuro" HorizontalAlign="Center" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Percentual">
                                        <ItemTemplate>
                                            <asp:Label ID="lblPercentual" runat="server"></asp:Label>
                                        </ItemTemplate>
                                        <HeaderTemplate>
                                            <asp:Label ID="lblHeadPercentual" runat="server" ForeColor="White">Percentual</asp:Label>
                                        </HeaderTemplate>
                                        <FooterStyle CssClass="formLabel" HorizontalAlign="Center" />
                                        <HeaderStyle BorderWidth="0px" CssClass="formRuler" />
                                        <ItemStyle CssClass="formLabelEscuro" HorizontalAlign="Center" />
                                    </asp:TemplateField>
                                </Columns>
                                <HeaderStyle ForeColor="White" />
                                <SelectedRowStyle Font-Bold="True" />
                            </asp:GridView>
                        </asp:Panel>
                        <asp:Panel runat="server" HorizontalAlign="Center" Visible="False" ID="pnlConfiguracao" CssClass="formLabel2" Font-Size="Medium">
                            <asp:Label ID="lblConfiguracao" runat="server" CssClass="formLabel2" Font-Size="Medium" Text=""></asp:Label>
                        </asp:Panel>

                        <asp:Panel ID="pnlHistoricoPesquisa" runat="server" HorizontalAlign="Center" Visible="False" Font-Size="Medium" CssClass="formLabel2" DefaultButton="btnConsultaHistorico">
                            <asp:Label ID="lblConsulta" runat="server" CssClass="formLabel" Font-Size="Medium" Text="Nome/E-mail"></asp:Label>
                            &nbsp;<asp:TextBox ID="txtConsulta" runat="server" Width="300px" Font-Size="Medium"></asp:TextBox>
                            &nbsp;<asp:Button ID="btnConsultaHistorico" runat="server" BackColor="#0066FF" Font-Bold="True" Font-Size="Medium" ForeColor="White" Height="35px" OnClientClick="this.disabled = true;" Text="Consultar" UseSubmitBehavior="False" Width="130px" />
                            <table width="100%">
                                <tr>
                                    <td>&nbsp;</td>
                                    <td>&nbsp;</td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                        <asp:GridView ID="gdvRankingUsuario" runat="server" AutoGenerateColumns="False" BackColor="White" CellPadding="4" CssClass="formLabel2" DataKeyNames="resId,resGrupo,resSexo,resEmail,resNome" EmptyDataText="Atenção! Não foi encontrado ninguém com esse nome" Font-Size="Medium" Width="99%" AllowPaging="True" HorizontalAlign="Center">
                                            <AlternatingRowStyle CssClass="tableRowOdd" />
                                            <EmptyDataRowStyle BorderColor="White" BorderStyle="Solid" Font-Size="Medium" Height="100px" HorizontalAlign="Center" VerticalAlign="Middle" />
                                            <Columns>
                                                <asp:TemplateField HeaderText="Nome">
                                                    <ItemTemplate>
                                                        <asp:LinkButton ID="lnkBtnNome" runat="server" Text='<%# Bind("resNome")%>' OnClick="lnkBtnNome_Click"></asp:LinkButton>
                                                    </ItemTemplate>
                                                    <HeaderTemplate>
                                                        <asp:Label ID="lblHeadNome" runat="server" ForeColor="White">Nome</asp:Label>
                                                    </HeaderTemplate>
                                                    <FooterStyle CssClass="formLabel" HorizontalAlign="Center" />
                                                    <HeaderStyle BorderWidth="0px" CssClass="formRuler" />
                                                    <ItemStyle CssClass="formLabelEscuro" HorizontalAlign="Center" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Email">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblEmail" runat="server" Text='<%# Bind("resEmail")%>'></asp:Label>
                                                    </ItemTemplate>
                                                    <HeaderTemplate>
                                                        <asp:Label ID="lblHeadEmail" runat="server" ForeColor="White">Email</asp:Label>
                                                    </HeaderTemplate>
                                                    <FooterStyle CssClass="formLabel" HorizontalAlign="Center" />
                                                    <HeaderStyle BorderWidth="0px" CssClass="formRuler" />
                                                    <ItemStyle CssClass="formLabelEscuro" HorizontalAlign="Center" />
                                                </asp:TemplateField>

                                                <asp:TemplateField HeaderText="Nascimento">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblNascimento" runat="server" Text='<%# Bind("resDtNascimento")%>'></asp:Label>
                                                    </ItemTemplate>
                                                    <HeaderTemplate>
                                                        <asp:Label ID="lblHeadNascimento" runat="server" ForeColor="White">Nascimento</asp:Label>
                                                    </HeaderTemplate>
                                                    <FooterStyle CssClass="formLabel" HorizontalAlign="Center" />
                                                    <HeaderStyle BorderWidth="0px" CssClass="formRuler" />
                                                    <ItemStyle CssClass="formLabelEscuro" HorizontalAlign="Center" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="CPF">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblCPF" runat="server" Text='<%# Bind("resCPFCGC")%>'></asp:Label>
                                                    </ItemTemplate>
                                                    <HeaderTemplate>
                                                        <asp:Label ID="lblHeadCPF" runat="server" ForeColor="White">CPF</asp:Label>
                                                    </HeaderTemplate>
                                                    <FooterStyle CssClass="formLabel" HorizontalAlign="Center" />
                                                    <HeaderStyle BorderWidth="0px" CssClass="formRuler" />
                                                    <ItemStyle CssClass="formLabelEscuro" HorizontalAlign="Center" />
                                                </asp:TemplateField>

                                                <asp:TemplateField HeaderText="Grupo">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblGrupo" runat="server"></asp:Label>
                                                        <asp:LinkButton ID="lnkBtnReset" runat="server" Text='Resetar Senha' OnClick="lnkBtnReset_Click" OnClientClick="return confirm('Confirmar o envio de nova senha padrão?')"></asp:LinkButton>
                                                    </ItemTemplate>
                                                    <HeaderTemplate>
                                                        <asp:Label ID="lblHeadGrupo" runat="server" ForeColor="White">Grupo</asp:Label>
                                                    </HeaderTemplate>
                                                    <FooterStyle CssClass="formLabel" HorizontalAlign="Center" />
                                                    <HeaderStyle BorderWidth="0px" CssClass="formRuler" />
                                                    <ItemStyle CssClass="formLabelEscuro" HorizontalAlign="Center" />
                                                </asp:TemplateField>

                                                <asp:TemplateField HeaderText="DDD">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblPrefixo" runat="server" Text='<%# Bind("resFonePrefixo")%>'></asp:Label>
                                                    </ItemTemplate>
                                                    <HeaderTemplate>
                                                        <asp:Label ID="lblHeadPrefixo" runat="server" ForeColor="White">DDD</asp:Label>
                                                    </HeaderTemplate>
                                                    <FooterStyle CssClass="formLabel" HorizontalAlign="Center" />
                                                    <HeaderStyle BorderWidth="0px" CssClass="formRuler" />
                                                    <ItemStyle CssClass="formLabelEscuro" HorizontalAlign="Center" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Comercial">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblComercial" runat="server" Text='<%# Bind("resFoneComercial")%>'></asp:Label>
                                                    </ItemTemplate>
                                                    <HeaderTemplate>
                                                        <asp:Label ID="lblHeadComercial" runat="server" ForeColor="White">Comercial</asp:Label>
                                                    </HeaderTemplate>
                                                    <FooterStyle CssClass="formLabel" HorizontalAlign="Center" />
                                                    <HeaderStyle BorderWidth="0px" CssClass="formRuler" />
                                                    <ItemStyle CssClass="formLabelEscuro" HorizontalAlign="Center" />
                                                </asp:TemplateField>

                                                <asp:TemplateField HeaderText="Residencial">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblResidencial" runat="server" Text='<%# Bind("resFoneResidencial")%>'></asp:Label>
                                                    </ItemTemplate>
                                                    <HeaderTemplate>
                                                        <asp:Label ID="lblHeadResidencial" runat="server" ForeColor="White">Residencial</asp:Label>
                                                    </HeaderTemplate>
                                                    <FooterStyle CssClass="formLabel" HorizontalAlign="Center" />
                                                    <HeaderStyle BorderWidth="0px" CssClass="formRuler" />
                                                    <ItemStyle CssClass="formLabelEscuro" HorizontalAlign="Center" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Celular">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblCelular" runat="server" Text='<%# Bind("resCelular")%>'></asp:Label>
                                                    </ItemTemplate>
                                                    <HeaderTemplate>
                                                        <asp:Label ID="lblHeadCelular" runat="server" ForeColor="White">Celular</asp:Label>
                                                    </HeaderTemplate>
                                                    <FooterStyle CssClass="formLabel" HorizontalAlign="Center" />
                                                    <HeaderStyle BorderWidth="0px" CssClass="formRuler" />
                                                    <ItemStyle CssClass="formLabelEscuro" HorizontalAlign="Center" />
                                                </asp:TemplateField>

                                                <asp:TemplateField HeaderText="Cidade">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblCidade" runat="server" Text='<%# Bind("resCidade")%>'></asp:Label>
                                                    </ItemTemplate>
                                                    <HeaderTemplate>
                                                        <asp:Label ID="lblHeadCidade" runat="server" ForeColor="White">Cidade</asp:Label>
                                                    </HeaderTemplate>
                                                    <FooterStyle CssClass="formLabel" HorizontalAlign="Center" />
                                                    <HeaderStyle BorderWidth="0px" CssClass="formRuler" />
                                                    <ItemStyle CssClass="formLabelEscuro" HorizontalAlign="Center" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Estado">
                                                    <ItemTemplate>
                                                        <asp:Label ID="Label1" runat="server" Text='<%# Bind("estDescricao")%>'></asp:Label>
                                                    </ItemTemplate>
                                                    <HeaderTemplate>
                                                        <asp:Label ID="Label2" runat="server" ForeColor="White">Estado</asp:Label>
                                                    </HeaderTemplate>
                                                    <FooterStyle CssClass="formLabel" HorizontalAlign="Center" />
                                                    <HeaderStyle BorderWidth="0px" CssClass="formRuler" />
                                                    <ItemStyle CssClass="formLabelEscuro" HorizontalAlign="Center" />
                                                </asp:TemplateField>
                                            </Columns>
                                            <HeaderStyle ForeColor="White" />
                                            <SelectedRowStyle Font-Bold="True" />
                                        </asp:GridView>
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
                        <asp:Panel ID="pnlHistorico" runat="server" Visible="False">
                            <table align="center">
                                <tr>
                                    <td></td>
                                    <td align="right">
                                        <asp:Label ID="lblNome" runat="server" CssClass="formLabel" Font-Size="Medium" Text="Nome"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:Label ID="lbltxtNome" runat="server" CssClass="formLabelEscuro" Font-Size="Medium" Text=""></asp:Label>
                                    </td>
                                    <td></td>
                                </tr>
                                <tr>
                                    <td>&nbsp;</td>
                                    <td align="right">
                                        <asp:Label ID="lblNascimento" runat="server" CssClass="formLabel" Font-Size="Medium" Text="Nascimento"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:Label ID="lbltxtNascimento" runat="server" CssClass="formLabelEscuro" Font-Size="Medium"></asp:Label>
                                    </td>
                                    <td>&nbsp;</td>
                                </tr>
                                <tr>
                                    <td>&nbsp;</td>
                                    <td align="right">
                                        <asp:Label ID="lblCPF" runat="server" CssClass="formLabel" Font-Size="Medium" Text="CPF"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:Label ID="lbltxtCPF" runat="server" CssClass="formLabelEscuro" Font-Size="Medium"></asp:Label>
                                    </td>
                                    <td>&nbsp;</td>
                                </tr>
                                <tr>
                                    <td></td>
                                    <td align="right">
                                        <asp:Label ID="lblEndereco" runat="server" CssClass="formLabel" Font-Size="Medium" Text="Endereço"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:Label ID="lbltxtEndereco" runat="server" CssClass="formLabelEscuro" Font-Size="Medium" Text=""></asp:Label>
                                    </td>
                                    <td></td>
                                </tr>
                                <tr>
                                    <td></td>
                                    <td align="right">
                                        <asp:Label ID="lblEmail" runat="server" CssClass="formLabel" Font-Size="Medium" Text="Email"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:Label ID="lbltxtEmail" runat="server" CssClass="formLabelEscuro" Font-Size="Medium"></asp:Label>
                                    </td>
                                    <td></td>
                                </tr>
                                <tr>
                                    <td>&nbsp;</td>
                                    <td align="right">
                                        <asp:Label ID="lblFones" runat="server" CssClass="formLabel" Font-Size="Medium" Text="Fones"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:Label ID="lbltxtPrefixo" runat="server" CssClass="formLabelEscuro" Font-Size="Medium" Text=""></asp:Label>
                                        &nbsp;<asp:Label ID="lbltxtFone1" runat="server" CssClass="formLabelEscuro" Font-Size="Medium" Text=""></asp:Label>
                                        &nbsp;<asp:Label ID="lbltxtFone2" runat="server" CssClass="formLabelEscuro" Font-Size="Medium" Text=""></asp:Label>
                                    </td>
                                    <td>&nbsp;</td>
                                </tr>
                                <tr>
                                    <td>&nbsp;</td>
                                    <td align="right">
                                        <asp:Label ID="lblCadastro" runat="server" CssClass="formLabel" Font-Size="Medium" Text="Cadastro"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:Label ID="lbltxtCadastro" runat="server" CssClass="formLabelEscuro" Font-Size="Medium" Text=""></asp:Label>
                                    </td>
                                    <td>&nbsp;</td>
                                </tr>
                                <tr>
                                    <td>&nbsp;</td>
                                    <td align="right">
                                        <asp:Label ID="lblGrupo" runat="server" CssClass="formLabel" Font-Size="Medium" Text="Grupo"></asp:Label>
                                    </td>
                                    <td align="left">
                                        <asp:RadioButtonList ID="rblGrupo" runat="server" CssClass="formLabelEscuro" Font-Size="Medium" RepeatDirection="Horizontal" RepeatLayout="Flow">
                                            <asp:ListItem Value="S">Sim</asp:ListItem>
                                            <asp:ListItem Value="N">Não</asp:ListItem>
                                        </asp:RadioButtonList>
                                        &nbsp;
                                        <asp:ImageButton ID="imgBtnGrupo" runat="server" Height="20px" ImageAlign="AbsMiddle" ImageUrl="~/images/Ok.png" OnClientClick="return confirm('Alterar?')" Width="30px" />
                                    </td>
                                    <td>&nbsp;</td>
                                </tr>
                                <tr>
                                    <td>&nbsp;</td>
                                    <td align="right">&nbsp;</td>
                                    <td>
                                        <asp:HiddenField ID="hddResId" runat="server" />
                                    </td>
                                    <td>&nbsp;</td>
                                </tr>
                                <tr>
                                    <td>&nbsp;</td>
                                    <td align="right" colspan="2">
                                        <asp:GridView ID="gdvRankingHistoricoCaldas" runat="server" AutoGenerateColumns="False" BackColor="White" CellPadding="4" CssClass="formLabel" DataKeyNames="resId,solId,resSexo,resEmail,resNome,dtIni,dtFim,intNome1,catId1,intNome2,catId2,intNome3,catId3,intNome4,catId4,intNome5,catId5,intNome6,catId6,intNome7,catId7,intDtNascimento1,intDtNascimento2,intDtNascimento3,intDtNascimento4,intDtNascimento5,intDtNascimento6,intDtNascimento7,intNome1Atendido,intNome2Atendido,intNome3Atendido,intNome4Atendido,intNome5Atendido,intNome6Atendido,intNome7Atendido,intCatId1Atendido,intCatId2Atendido,intCatId3Atendido,intCatId4Atendido,intCatId5Atendido,intCatId6Atendido,intCatId7Atendido,intDtNascimento1Atendido,intDtNascimento2Atendido,intDtNascimento3Atendido,intDtNascimento4Atendido,intDtNascimento5Atendido,intDtNascimento6Atendido,intDtNascimento7Atendido,resDataIni,resDataFim,intRepetido1,intRepetido2,intRepetido3,intRepetido4,intRepetido5,intRepetido6,intRepetido7,intRepetidoAtendido1,intRepetidoAtendido2,intRepetidoAtendido3,intRepetidoAtendido4,intRepetidoAtendido5,intRepetidoAtendido6,intRepetidoAtendido7,situacao,solDtEnviado,solDtListaEspera,solDtSemDisponibilidade,solDtReservado" EmptyDataText="Atenção! Não foi encontrada nenhuma solicitação para Caldas Novas" Font-Size="Medium" ShowFooter="True" Width="99%" Caption="SESC Caldas Novas">
                                            <AlternatingRowStyle CssClass="tableRowOdd" />
                                            <EmptyDataRowStyle BorderColor="White" BorderStyle="Solid" Font-Size="Medium" Height="100px" HorizontalAlign="Center" VerticalAlign="Middle" />
                                            <Columns>
                                                <asp:TemplateField HeaderText="Período">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblPeriodoC" runat="server"></asp:Label>
                                                    </ItemTemplate>
                                                    <HeaderTemplate>
                                                        <asp:Label ID="lblHeadPeriodoC" runat="server" ForeColor="White">Período</asp:Label>
                                                    </HeaderTemplate>
                                                    <FooterStyle CssClass="formLabel" HorizontalAlign="Center" />
                                                    <HeaderStyle BorderWidth="0px" CssClass="formRuler" />
                                                    <ItemStyle CssClass="formLabelEscuro" HorizontalAlign="Center" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Integrantes">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblIntegrantesC" runat="server"></asp:Label>
                                                    </ItemTemplate>
                                                    <HeaderTemplate>
                                                        <asp:Label ID="lblHeadBtnIntegrantesC" runat="server" ForeColor="White">Integrantes</asp:Label>
                                                    </HeaderTemplate>
                                                    <FooterStyle CssClass="formLabel" HorizontalAlign="Center" />
                                                    <HeaderStyle BorderWidth="0px" CssClass="formRuler" />
                                                    <ItemStyle CssClass="formLabelEscuro" HorizontalAlign="Left" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Enviada">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblDtEnviadaC" runat="server"></asp:Label>
                                                    </ItemTemplate>
                                                    <HeaderTemplate>
                                                        <asp:Label ID="lblHeadDtEnviadaC" runat="server" ForeColor="White">Enviada</asp:Label>
                                                    </HeaderTemplate>
                                                    <FooterStyle CssClass="formLabel" HorizontalAlign="Center" />
                                                    <HeaderStyle BorderWidth="0px" CssClass="formRuler" />
                                                    <ItemStyle CssClass="formLabelEscuro" HorizontalAlign="Center" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Confirmada">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblDtReservadoC" runat="server"></asp:Label>
                                                    </ItemTemplate>
                                                    <HeaderTemplate>
                                                        <asp:Label ID="lblHeadReservadoC" runat="server" ForeColor="White">Confirmada</asp:Label>
                                                    </HeaderTemplate>
                                                    <FooterStyle CssClass="formLabel" HorizontalAlign="Center" />
                                                    <HeaderStyle BorderWidth="0px" CssClass="formRuler" />
                                                    <ItemStyle CssClass="formLabelEscuro" HorizontalAlign="Center" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Espera">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblEsperaC" runat="server"></asp:Label>
                                                    </ItemTemplate>
                                                    <HeaderTemplate>
                                                        <asp:Label ID="lblHeadEsperaC" runat="server" ForeColor="White" ToolTip="Ordenar">Espera</asp:Label>
                                                    </HeaderTemplate>
                                                    <FooterStyle CssClass="formLabel" HorizontalAlign="Center" />
                                                    <HeaderStyle BorderWidth="0px" CssClass="formRuler" />
                                                    <ItemStyle CssClass="formLabelEscuro" HorizontalAlign="Center" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Trabalho">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblTrabalhoC" runat="server"></asp:Label>
                                                    </ItemTemplate>
                                                    <HeaderTemplate>
                                                        <asp:Label ID="lblHeadTrabalho" runat="server" ForeColor="White" ToolTip="Ordenar">Trabalho</asp:Label>
                                                    </HeaderTemplate>
                                                    <FooterStyle CssClass="formLabel" HorizontalAlign="Center" />
                                                    <HeaderStyle BorderWidth="0px" CssClass="formRuler" />
                                                    <ItemStyle CssClass="formLabelEscuro" HorizontalAlign="Center" />
                                                </asp:TemplateField>
                                            </Columns>
                                            <HeaderStyle ForeColor="White" />
                                            <SelectedRowStyle Font-Bold="True" />
                                        </asp:GridView>
                                    </td>
                                    <td>&nbsp;</td>
                                </tr>
                                <tr>
                                    <td>&nbsp;</td>
                                    <td align="right">&nbsp;</td>
                                    <td>&nbsp;</td>
                                    <td>&nbsp;</td>
                                </tr>
                                <tr>
                                    <td>&nbsp;</td>
                                    <td align="right" colspan="2">
                                        <asp:GridView ID="gdvRankingHistoricoPiri" runat="server" AutoGenerateColumns="False" BackColor="White" Caption="Pousada SESC Pirenópolis" CellPadding="4" CssClass="formLabel" DataKeyNames="resId,solId,resSexo,resEmail,resNome,dtIni,dtFim,intNome1,catId1,intNome2,catId2,intNome3,catId3,intNome4,catId4,intNome5,catId5,intNome6,catId6,intNome7,catId7,intDtNascimento1,intDtNascimento2,intDtNascimento3,intDtNascimento4,intDtNascimento5,intDtNascimento6,intDtNascimento7,intNome1Atendido,intNome2Atendido,intNome3Atendido,intNome4Atendido,intNome5Atendido,intNome6Atendido,intNome7Atendido,intCatId1Atendido,intCatId2Atendido,intCatId3Atendido,intCatId4Atendido,intCatId5Atendido,intCatId6Atendido,intCatId7Atendido,intDtNascimento1Atendido,intDtNascimento2Atendido,intDtNascimento3Atendido,intDtNascimento4Atendido,intDtNascimento5Atendido,intDtNascimento6Atendido,intDtNascimento7Atendido,resDataIni,resDataFim,intRepetido1,intRepetido2,intRepetido3,intRepetido4,intRepetido5,intRepetido6,intRepetido7,intRepetidoAtendido1,intRepetidoAtendido2,intRepetidoAtendido3,intRepetidoAtendido4,intRepetidoAtendido5,intRepetidoAtendido6,intRepetidoAtendido7,situacao,solDtEnviado,solDtListaEspera,solDtSemDisponibilidade,solDtReservado" EmptyDataText="Atenção! Não foi encontrada nenhuma solicitação para Pirenópolis" Font-Size="Medium" ShowFooter="True" Width="99%">
                                            <AlternatingRowStyle CssClass="tableRowOdd" />
                                            <EmptyDataRowStyle BorderColor="White" BorderStyle="Solid" Font-Size="Medium" Height="100px" HorizontalAlign="Center" VerticalAlign="Middle" />
                                            <Columns>
                                                <asp:TemplateField HeaderText="Período">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblPeriodoP" runat="server"></asp:Label>
                                                    </ItemTemplate>
                                                    <HeaderTemplate>
                                                        <asp:Label ID="lblHeadPeriodoP" runat="server" ForeColor="White">Período</asp:Label>
                                                    </HeaderTemplate>
                                                    <FooterStyle CssClass="formLabel" HorizontalAlign="Center" />
                                                    <HeaderStyle BorderWidth="0px" CssClass="formRuler" />
                                                    <ItemStyle CssClass="formLabelEscuro" HorizontalAlign="Center" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Integrantes">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblIntegrantesP" runat="server"></asp:Label>
                                                    </ItemTemplate>
                                                    <HeaderTemplate>
                                                        <asp:Label ID="lblHeadBtnIntegrantesP" runat="server" ForeColor="White">Integrantes</asp:Label>
                                                    </HeaderTemplate>
                                                    <FooterStyle CssClass="formLabel" HorizontalAlign="Center" />
                                                    <HeaderStyle BorderWidth="0px" CssClass="formRuler" />
                                                    <ItemStyle CssClass="formLabelEscuro" HorizontalAlign="Left" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Enviada">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblDtEnviadaP" runat="server"></asp:Label>
                                                    </ItemTemplate>
                                                    <HeaderTemplate>
                                                        <asp:Label ID="lblHeadDtEnviadaP" runat="server" ForeColor="White">Enviada</asp:Label>
                                                    </HeaderTemplate>
                                                    <FooterStyle CssClass="formLabel" HorizontalAlign="Center" />
                                                    <HeaderStyle BorderWidth="0px" CssClass="formRuler" />
                                                    <ItemStyle CssClass="formLabelEscuro" HorizontalAlign="Center" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Confirmada">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblDtReservadoP" runat="server"></asp:Label>
                                                    </ItemTemplate>
                                                    <HeaderTemplate>
                                                        <asp:Label ID="lblHeadReservadoP" runat="server" ForeColor="White">Confirmada</asp:Label>
                                                    </HeaderTemplate>
                                                    <FooterStyle CssClass="formLabel" HorizontalAlign="Center" />
                                                    <HeaderStyle BorderWidth="0px" CssClass="formRuler" />
                                                    <ItemStyle CssClass="formLabelEscuro" HorizontalAlign="Center" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Espera">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblEsperaP" runat="server"></asp:Label>
                                                    </ItemTemplate>
                                                    <HeaderTemplate>
                                                        <asp:Label ID="lblHeadEsperaP" runat="server" ForeColor="White" ToolTip="Ordenar">Espera</asp:Label>
                                                    </HeaderTemplate>
                                                    <FooterStyle CssClass="formLabel" HorizontalAlign="Center" />
                                                    <HeaderStyle BorderWidth="0px" CssClass="formRuler" />
                                                    <ItemStyle CssClass="formLabelEscuro" HorizontalAlign="Center" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Trabalho">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblTrabalhoP" runat="server"></asp:Label>
                                                    </ItemTemplate>
                                                    <HeaderTemplate>
                                                        <asp:Label ID="lblHeadTrabalhoP" runat="server" ForeColor="White" ToolTip="Ordenar">Trabalho</asp:Label>
                                                    </HeaderTemplate>
                                                    <FooterStyle CssClass="formLabel" HorizontalAlign="Center" />
                                                    <HeaderStyle BorderWidth="0px" CssClass="formRuler" />
                                                    <ItemStyle CssClass="formLabelEscuro" HorizontalAlign="Center" />
                                                </asp:TemplateField>
                                            </Columns>
                                            <HeaderStyle ForeColor="White" />
                                            <SelectedRowStyle Font-Bold="True" />
                                        </asp:GridView>
                                    </td>
                                    <td>&nbsp;</td>
                                </tr>
                                <tr>
                                    <td align="center" colspan="4">&nbsp;</td>
                                </tr>
                            </table>
                        </asp:Panel>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Panel ID="pnlVoltarHistorico" runat="server" Visible="False">
                            <asp:Button ID="btnVoltarHistorico" runat="server" BackColor="#999999" Font-Bold="True" Font-Size="Medium" ForeColor="White" Height="35px" OnClientClick="this.disabled = true;" Text="Voltar" UseSubmitBehavior="False" Width="130px" />
                        </asp:Panel>
                        <asp:AlwaysVisibleControlExtender ID="AlwaysVisibleControlExtender1" runat="server" BehaviorID="avcpnlVoltarHistorico" Enabled="True" HorizontalSide="Center" TargetControlID="pnlVoltarHistorico" VerticalSide="Bottom">
                        </asp:AlwaysVisibleControlExtender>
                    </td>
                </tr>
            </table>

        </ContentTemplate>
    </asp:updatepanel>
</asp:Content>

