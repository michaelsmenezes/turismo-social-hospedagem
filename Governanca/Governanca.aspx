<%@ Page Language="VB" MasterPageFile="~/TurismoSocial.master" AutoEventWireup="true"
    CodeFile="Governanca.aspx.vb" Inherits="Governanca" UICulture="Auto" Title="Controle de Governança" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <meta http-equiv="X-UA-Compatible" content="IE=9" />
    <object id="Impressao" height="1" width="1" classid="clsid:8D6CC4E9-1AE1-4909-94AF-8A4CDC10C466">
    </object>
    <script type="text/javascript" src="../JScript.js">
        function ticketgovernanca() {
            texto = 'SESC - SERVIÇO SOCIAL DO COMÉRCIO' + eval("String.fromCharCode(0x0A)");
            texto += aspnetForm.ctl00_conPlaHolTurismoSocial_hddImpressao.value + String.fromCharCode(10);

            iRetorno = Impressao.imprimeSimples(texto);
        }
    </script>
    <link href="../stylesheet.css" rel="stylesheet" />
    <style type="text/css">
        .style1 {
            height: 12px;
        }

        .auto-style1 {
            width: 53px;
        }

        .auto-style2 {
            height: 24px;
        }

        </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="conPlaHolTurismoSocial" runat="Server">
    <asp:UpdatePanel ID="updPnlGovernanca" runat="server">
        <ContentTemplate>
            <asp:Panel ID="pnlConsulta" runat="server" DefaultButton="btnConsultar" Width="90%" CssClass="ArrendodarBorda">
                &nbsp;<br />
                <br />
                <br />
                <br />
                <br />
                <br />
                <br />
                <table style="width: 100%;">
                    <tr>
                        <td>
                            <table>
                                <tr>
                                    <td>Data
                                    </td>
                                    <td>Apartamento
                                    </td>
                                    <td>Federação
                                    </td>
                                    <td>Visualizar
                                    </td>
                                    <td>
                                        <asp:Label ID="Label13" runat="server" Text="Label"></asp:Label>
                                    </td>
                                    <td>&nbsp;
                                    </td>
                                    <td>&nbsp;
                                    </td>
                                    <td>&nbsp;
                                    </td>
                                </tr>
                                <tr>
                                    <td rowspan="2">
                                        <asp:TextBox ID="txtDataPrevisao" runat="server" Width="90px"></asp:TextBox>
                                        <cc1:CalendarExtender ID="txtDataPrevisao_CalendarExtender" runat="server" Enabled="True"
                                            FirstDayOfWeek="Sunday" Format="dd/MM/yyyy" TargetControlID="txtDataPrevisao">
                                        </cc1:CalendarExtender>
                                    </td>
                                    <td rowspan="2">
                                        <asp:TextBox ID="txtApartamento" runat="server" Width="80px"></asp:TextBox>
                                    </td>
                                    <td rowspan="2">
                                        <asp:DropDownList ID="txtFederacao" runat="server" AutoPostBack="True" CssClass="ColocaHand">
                                            <asp:ListItem>Todos</asp:ListItem>
                                            <asp:ListItem>Sim</asp:ListItem>
                                            <asp:ListItem Value="Não"></asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                    <td>
                                        <asp:CheckBox ID="chkPrioridade" runat="server" Checked="True" Text="Prioridade" />
                                        <cc1:ToggleButtonExtender ID="chkPrioridade_ToggleButtonExtender" runat="server"
                                            Enabled="True" TargetControlID="chkPrioridade" UncheckedImageUrl="~/images/un-checked.gif"
                                            CheckedImageUrl="~/images/Checked.gif" ImageHeight="20" ImageWidth="20">
                                        </cc1:ToggleButtonExtender>
                                    </td>
                                    <td>
                                        <asp:CheckBox ID="chkOcupado" runat="server" Text="Ocupado" Checked="True" />
                                        <cc1:ToggleButtonExtender ID="chkOcupado_ToggleButtonExtender" runat="server" Enabled="True"
                                            TargetControlID="chkOcupado" UncheckedImageUrl="~/images/un-checked.gif" CheckedImageUrl="~/images/Checked.gif"
                                            ImageHeight="20" ImageWidth="20">
                                        </cc1:ToggleButtonExtender>
                                    </td>
                                    <td>
                                        <asp:CheckBox ID="chkLimpo" runat="server" Text="Limpo" Checked="True" />
                                        <cc1:ToggleButtonExtender ID="chkLimpo_ToggleButtonExtender" runat="server" Enabled="True"
                                            TargetControlID="chkLimpo" UncheckedImageUrl="~/images/un-checked.gif" CheckedImageUrl="~/images/Checked.gif"
                                            ImageHeight="20" ImageWidth="20">
                                        </cc1:ToggleButtonExtender>
                                    </td>
                                    <td rowspan="2">
                                        <asp:Button ID="btnConsultar" runat="server" CssClass="imgLupa" Text="  Consultar"
                                            Width="100px" />
                                    </td>
                                    <td rowspan="2">
                                        <asp:Button ID="btnRelatorios" runat="server" CssClass="imgRelatorio" Text="  Relatórios" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:CheckBox ID="chkArrumacao" runat="server" Checked="True" Text="Arrumação" />
                                        <cc1:ToggleButtonExtender ID="chkArrumacao_ToggleButtonExtender" runat="server" Enabled="True"
                                            TargetControlID="chkArrumacao" UncheckedImageUrl="~/images/un-checked.gif" CheckedImageUrl="~/images/Checked.gif"
                                            ImageHeight="20" ImageWidth="20">
                                        </cc1:ToggleButtonExtender>
                                    </td>
                                    <td>
                                        <asp:CheckBox ID="chkManutencao" runat="server" Text="Manutenção" />
                                        <cc1:ToggleButtonExtender ID="chkManutencao_ToggleButtonExtender" runat="server"
                                            Enabled="True" TargetControlID="chkManutencao" UncheckedImageUrl="~/images/un-checked.gif"
                                            CheckedImageUrl="~/images/Checked.gif" ImageHeight="20" ImageWidth="20">
                                        </cc1:ToggleButtonExtender>
                                    </td>
                                    <td>
                                        <asp:CheckBox ID="chkConferencia" runat="server" Checked="True" Text="Conferência" />
                                        <cc1:ToggleButtonExtender ID="chkConferencia_ToggleButtonExtender" runat="server"
                                            Enabled="True" TargetControlID="chkConferencia" UncheckedImageUrl="~/images/un-checked.gif"
                                            CheckedImageUrl="~/images/Checked.gif" ImageHeight="20" ImageWidth="20">
                                        </cc1:ToggleButtonExtender>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="center" colspan="8" style="display: none">
                                        <asp:Button ID="btnConsultaEnxoval" runat="server" Text="btnConsultaEnxoval" />
                                    </td>
                                </tr>
                            </table>
                        </td>
                        <td align="right" valign="top">
                            <table style="width: 100%;">
                                <tr>
                                    <td align="left" colspan="2" style="font-weight: bold">Legenda:
                                    </td>
                                    <td align="left" style="font-weight: bold">
                                        <asp:ImageButton ID="imgConfiguracao" runat="server" CssClass="ColocaHand" ImageUrl="~/images/configure.png"
                                            PostBackUrl="~/Governanca/frmConfiguracoes.aspx" ToolTip="Configurações do sistema" />
                                    </td>
                                </tr>
                                <tr>
                                    <td style="font-weight: bold">
                                        <asp:Image ID="Image1" runat="server" ImageUrl="~/images/Estrela Laranja.png" />
                                    </td>
                                    <td align="left">Aguardando Check Out
                                    </td>
                                    <td align="left">&nbsp;
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Image ID="Image2" runat="server" ImageUrl="~/images/Estrela Vermelha.png" />
                                    </td>
                                    <td align="left">Prioridade Máxima
                                    </td>
                                    <td align="left">&nbsp;
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Image ID="Image3" runat="server" ImageUrl="~/images/Estrela Lilas.png" />
                                    </td>
                                    <td align="left">Aguardando Transferência
                                    </td>
                                    <td align="left">&nbsp;
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Image ID="Image4" runat="server" ImageUrl="~/images/Estrela Preta.png" />
                                    </td>
                                    <td align="left">Saído de Manutenção
                                    </td>
                                    <td align="left">&nbsp;
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
            </asp:Panel>
            <div align="center" class="PosicionaProgresso">
                <asp:UpdateProgress ID="upgProgressGeral" runat="server" AssociatedUpdatePanelID="updPnlGovernanca">
                    <ProgressTemplate>
                        <asp:Image ID="imgProgresso" runat="server" ImageUrl="~/images/Aguarde.gif" Width="33px" />
                        <br />
                        <asp:Label ID="lblAguarde0" runat="server" Font-Bold="True" Font-Size="Small" ForeColor="#99CCFF"
                            Text="Processando..."></asp:Label>
                    </ProgressTemplate>
                </asp:UpdateProgress>
            </div>

            <table>
                <tr>
                    <td valign="top">
                        <asp:Panel ID="pnlPopOcupado" runat="server">
                            <table width="200px">
                                <tr>
                                    <td style="font-size: small; font-weight: bold" nowrap="nowrap">Apartamento:
                                        <asp:TextBox ID="txtApto" runat="server" BorderStyle="None" CssClass="aplicaTransparencia"
                                            ReadOnly="True"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:LinkButton ID="lnkAtendimentoOcupado" runat="server" OnClick="lnkAtendimentoOcupado_Click">Atendimento</asp:LinkButton>
                                        &nbsp;
                                        <asp:Image ID="imgAtendimentoOcupado" runat="server" ImageUrl="~/images/Cruz.gif" />
                                        <asp:HiddenField ID="hddAptoOcupado" runat="server" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:LinkButton ID="lnkIntegrantesOcupado" runat="server">Integrantes</asp:LinkButton>
                                        <asp:Image ID="imgIntegrantesOcupado" runat="server" ImageUrl="~/images/Integrante.png" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:LinkButton ID="lnkManutencaoOcupado" runat="server" EnableTheming="True">Manutenção</asp:LinkButton>
                                        <asp:Image ID="imgManutencaoOcupado" runat="server" ImageUrl="~/images/StatusAptoCinza.gif" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:LinkButton ID="lnkLimpezaO" runat="server">Liberação como limpo</asp:LinkButton>
                                        &nbsp;<asp:Image ID="imbLimpezaO" runat="server" ImageUrl="~/images/limpar.png" />
                                    </td>
                                </tr>
                            </table>
                            <br />
                        </asp:Panel>
                    </td>
                    <td valign="top">
                        <asp:Panel ID="pnlPopArrumacao" runat="server">
                            <table width="200px">
                                <tr>
                                    <td style="font-size: small; font-weight: bold" class="auto-style2" nowrap="nowrap">Apartamento:
                                        <asp:TextBox ID="txtAptoArrumacao" runat="server" BorderStyle="None" CssClass="aplicaTransparencia"
                                            ReadOnly="True"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:LinkButton ID="lnkLimpoArrumacao" runat="server">Limpo</asp:LinkButton>
                                        &nbsp;
                                        <asp:Image ID="imgAtendimentoOcupado0" runat="server" ImageUrl="~/images/StatusAptoBranco.gif" />
                                        <asp:HiddenField ID="hddAptoArrumacao" runat="server" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:LinkButton ID="lnkAtendimentoArrumacao" runat="server" EnableTheming="True">Atendimento</asp:LinkButton>
                                        &nbsp;
                                        <asp:Image ID="imgManutencaoOcupado0" runat="server" ImageUrl="~/images/Cruz.gif" />
                                    </td>
                                </tr>
                                <tr>
                                    <td valign="top">
                                        <asp:LinkButton ID="lnkManutencaoArrumacao" runat="server">Manutenção</asp:LinkButton>
                                        &nbsp;
                                        <asp:Image ID="imgIntegrantesOcupado0" runat="server" ImageUrl="~/images/StatusAptoCinza.gif" />
                                    </td>
                                </tr>
                                <tr>
                                    <td valign="top">
                                        <asp:LinkButton ID="lnkLimpezaA" runat="server">Liberação como limpo</asp:LinkButton>
                                        &nbsp;<asp:Image ID="imbLimpezaA" runat="server" ImageUrl="~/images/limpar.png" />
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
                    </td>
                    <td valign="top">
                        <asp:Panel ID="pnlPopLimpo" runat="server">
                            <table width="200px">
                                <tr>
                                    <td style="font-size: small; font-weight: bold" nowrap="nowrap">Apartamento:
                                        <asp:TextBox ID="txtAptoLimpo" runat="server" BorderStyle="None" CssClass="aplicaTransparencia"
                                            ReadOnly="True"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:LinkButton ID="lnkArrumacaoLimpo" runat="server">Arrumação</asp:LinkButton>
                                        &nbsp;
                                        <asp:Image ID="imgAtendimentoOcupado1" runat="server" ImageUrl="~/images/StatusAptoAmarelo.gif" />
                                        <asp:HiddenField ID="hddAptoLimpo" runat="server" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:LinkButton ID="lnkRevisaoLimpo" runat="server" EnableTheming="True">Revisão</asp:LinkButton>
                                        &nbsp;
                                        <asp:Image ID="imgManutencaoOcupado1" runat="server" ImageUrl="~/images/lupa.png" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:LinkButton ID="lnkManutencaoLimpo" runat="server">Manutenção</asp:LinkButton>
                                        &nbsp;
                                        <asp:Image ID="imgIntegrantesOcupado1" runat="server" ImageUrl="~/images/StatusAptoCinza.gif" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:LinkButton ID="lnkLimpezaL" runat="server">Liberação como limpo</asp:LinkButton>
                                        &nbsp;<asp:Image ID="imbLimpezaL" runat="server"
                                            ImageUrl="~/images/limpar.png" />
                                        &nbsp;
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
                    </td>
                    <td valign="top">
                        <asp:Panel ID="pnlPopManutencao" runat="server">
                            <table width="200px">
                                <tr>
                                    <td style="font-size: small; font-weight: bold" nowrap="nowrap">Apartamento:
                                        <asp:TextBox ID="txtAptoManutencao" runat="server" BorderStyle="None" CssClass="aplicaTransparencia"
                                            ReadOnly="True"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:LinkButton ID="lnkManutencaoMan" runat="server">Manutenção</asp:LinkButton>
                                        &nbsp;
                                        <asp:Image ID="imgAtendimentoOcupado2" runat="server" ImageUrl="~/images/StatusAptoCinza.gif" />
                                        <asp:HiddenField ID="hddAptoManutencao" runat="server" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:LinkButton ID="lnkLimpezaM" runat="server">Liberação como limpo</asp:LinkButton>
                                        &nbsp;<asp:Image ID="imbLimpezaM" runat="server" ImageUrl="~/images/limpar.png" />
                                    </td>
                                </tr>
                                <tr>
                                    <td valign="top">&nbsp;<asp:TextBox ID="txtDescricaoManut" runat="server" AutoCompleteType="Disabled"
                                        CssClass="aplicaTransparencia" Font-Bold="False" Font-Size="Larger" Height="90px"
                                        TextMode="MultiLine" Width="205px"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>&nbsp;
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
                    </td>
                    <td valign="top">
                        <asp:Panel ID="pnlPopPrioridade" runat="server">
                            <table width="200px">
                                <tr>
                                    <td style="font-size: small; font-weight: bold" nowrap="nowrap">Apartamento:
                                        <asp:TextBox ID="txtAptoPrioridade" runat="server" BorderStyle="None" CssClass="aplicaTransparencia"
                                            Font-Bold="True" ReadOnly="True" Font-Size="Small"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td valign="top">
                                        <asp:LinkButton ID="lnkPrioridadeAten" runat="server">Atendimento</asp:LinkButton>
                                        &nbsp;
                                        <asp:Image ID="imgAtendimentoOcupado3" runat="server" ImageUrl="~/images/Cruz.gif" />
                                        <asp:HiddenField ID="hddAptoPrioridade" runat="server" />
                                    </td>
                                </tr>
                                <tr>
                                    <td valign="top">
                                        <asp:TextBox ID="txtReposicaoPrior" runat="server" CssClass="aplicaTransparencia"
                                            Font-Size="Small" TextMode="MultiLine" Height="90px" Width="200px" AutoCompleteType="Disabled"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>&nbsp;
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
                        &nbsp;
                    </td>
                    <td>&nbsp;
                    </td>
                </tr>
            </table>
            <asp:Panel ID="pnlStatus" runat="server">
                <table style="height: 249px">
                    <tr>
                        <td valign="top">
                            <asp:Panel ID="pnlPrioridade" runat="server" Visible="False" CssClass="ArrendodarBorda">
                                <table style="width: 100%;">
                                    <tr>
                                        <td align="center" class="formRuler" colspan="4" valign="bottom">
                                            <asp:Label ID="lblTotPrioridade" runat="server"></asp:Label>
                                            &nbsp;<asp:Label ID="lblTituloPrior" runat="server" Text="Prioridade"></asp:Label>
                                            &nbsp;<asp:Image ID="imgPrioridade" runat="server" ImageUrl="~/images/StatusAptoVermelho.gif"
                                                Height="23px" Width="35px" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td valign="top">
                                            <asp:GridView ID="gdvPrioridade1" runat="server" AutoGenerateColumns="False" CellPadding="4"
                                                DataKeyNames="ApaId,ApaDesc,ApaHoras,ApaCamaCasal,ApaCamaSolteiro,ApaCamaEspecial,ApaCamaExtra,ApaBercoEspecial,ApaBerco,ApaFronha,ApaJogoToalha,ApaConferencia"
                                                ForeColor="#333333" GridLines="None" ShowHeader="False">
                                                <RowStyle BackColor="#EFF3FB" />
                                                <Columns>
                                                    <asp:TemplateField ShowHeader="False" ItemStyle-CssClass="zoom" ItemStyle-Wrap="False">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblPrioridade1" runat="server" CssClass="ColocaHand"
                                                                Text="lblPrior1"></asp:Label>
                                                            <cc1:BalloonPopupExtender ID="lblPrioridade1_BalloonPopupExtender" runat="server"
                                                                BalloonPopupControlID="pnlPopPrioridade" BalloonSize="Medium" CustomCssUrl=""
                                                                DynamicServicePath="" Enabled="True" ExtenderControlID="" ScrollBars="None" TargetControlID="lblPrioridade1">
                                                            </cc1:BalloonPopupExtender>
                                                        </ItemTemplate>
                                                        <ItemStyle HorizontalAlign="Left" />
                                                    </asp:TemplateField>
                                                </Columns>
                                                <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                                                <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
                                                <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                                                <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                                                <EditRowStyle BackColor="#2461BF" />
                                                <AlternatingRowStyle BackColor="White" />
                                            </asp:GridView>
                                        </td>
                                        <td valign="top">
                                            <asp:GridView ID="gdvPrioridade2" runat="server" AutoGenerateColumns="False" CellPadding="4"
                                                DataKeyNames="ApaId,ApaDesc,ApaHoras,ApaCamaCasal,ApaCamaSolteiro,ApaCamaEspecial,ApaCamaExtra,ApaBercoEspecial,ApaBerco,ApaFronha,ApaJogoToalha,ApaConferencia"
                                                ForeColor="#333333" GridLines="None" ShowHeader="False">
                                                <RowStyle BackColor="#EFF3FB" />
                                                <Columns>
                                                    <asp:TemplateField ShowHeader="False" ItemStyle-CssClass="zoom" ItemStyle-Wrap="False">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblPrioridade2" runat="server" CssClass="ColocaHand"
                                                                Text="lblPrior2"></asp:Label>
                                                            <cc1:BalloonPopupExtender ID="lblPrioridade2_BalloonPopupExtender" runat="server"
                                                                BalloonPopupControlID="pnlPopPrioridade" BalloonSize="Medium" CustomCssUrl=""
                                                                DynamicServicePath="" Enabled="True" ExtenderControlID="" ScrollBars="None" TargetControlID="lblPrioridade2">
                                                            </cc1:BalloonPopupExtender>
                                                        </ItemTemplate>
                                                        <ItemStyle HorizontalAlign="Left" />
                                                    </asp:TemplateField>
                                                </Columns>
                                                <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                                                <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
                                                <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                                                <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                                                <EditRowStyle BackColor="#2461BF" />
                                                <AlternatingRowStyle BackColor="White" />
                                            </asp:GridView>
                                        </td>
                                        <td valign="top">
                                            <asp:GridView ID="gdvPrioridade3" runat="server" AutoGenerateColumns="False" CellPadding="4"
                                                DataKeyNames="ApaId,ApaDesc,ApaHoras,ApaCamaCasal,ApaCamaSolteiro,ApaCamaEspecial,ApaCamaExtra,ApaBercoEspecial,ApaBerco,ApaFronha,ApaJogoToalha,ApaConferencia"
                                                ForeColor="#333333" GridLines="None" ShowHeader="False">
                                                <RowStyle BackColor="#EFF3FB" />
                                                <Columns>
                                                    <asp:TemplateField ShowHeader="False" ItemStyle-CssClass="zoom" ItemStyle-Wrap="False">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblPrioridade3" runat="server" CssClass="ColocaHand"
                                                                Text="lblPrior3"></asp:Label>
                                                            <cc1:BalloonPopupExtender ID="lblPrioridade3_BalloonPopupExtender" runat="server"
                                                                BalloonPopupControlID="pnlPopPrioridade" BalloonSize="Medium" CustomCssUrl=""
                                                                DynamicServicePath="" Enabled="True" ExtenderControlID="" ScrollBars="None" TargetControlID="lblPrioridade3">
                                                            </cc1:BalloonPopupExtender>
                                                        </ItemTemplate>
                                                        <ItemStyle HorizontalAlign="Left" />
                                                    </asp:TemplateField>
                                                </Columns>
                                                <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                                                <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
                                                <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                                                <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                                                <EditRowStyle BackColor="#2461BF" />
                                                <AlternatingRowStyle BackColor="White" />
                                            </asp:GridView>
                                        </td>
                                        <td valign="top">
                                            <asp:GridView ID="gdvPrioridade4" runat="server" AutoGenerateColumns="False" CellPadding="4"
                                                DataKeyNames="ApaId,ApaDesc,ApaHoras,ApaCamaCasal,ApaCamaSolteiro,ApaCamaEspecial,ApaCamaExtra,ApaBercoEspecial,ApaBerco,ApaFronha,ApaJogoToalha,ApaConferencia"
                                                ForeColor="#333333" GridLines="None" ShowHeader="False">
                                                <RowStyle BackColor="#EFF3FB" />
                                                <Columns>
                                                    <asp:TemplateField ShowHeader="False" ItemStyle-CssClass="zoom" ItemStyle-Wrap="False">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblPrioridade4" runat="server" CssClass="ColocaHand"
                                                                Text="lblPrior4"></asp:Label>
                                                            <cc1:BalloonPopupExtender ID="lblPrioridade4_BalloonPopupExtender" runat="server"
                                                                BalloonPopupControlID="pnlPopPrioridade" BalloonSize="Medium" CustomCssUrl=""
                                                                DynamicServicePath="" Enabled="True" ExtenderControlID="" ScrollBars="None" TargetControlID="lblPrioridade4">
                                                            </cc1:BalloonPopupExtender>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
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
                                        <td>&nbsp;
                                        </td>
                                        <td>&nbsp;
                                        </td>
                                        <td>&nbsp;
                                        </td>
                                        <td>&nbsp;
                                        </td>
                                    </tr>
                                </table>
                            </asp:Panel>
                            <br />
                        </td>
                        <td valign="top">
                            <asp:Panel ID="pnlArrumacao" runat="server" Visible="False" CssClass="ArrendodarBorda">
                                <table>
                                    <tr>
                                        <td align="center" class="formRuler" colspan="4" valign="bottom">
                                            <asp:Label ID="lblTotArrumacao" runat="server" Text=" "></asp:Label>
                                            &nbsp;<asp:LinkButton ID="lnkArrumacao" runat="server">Arrumação</asp:LinkButton>
                                            &nbsp;<asp:Image ID="imgArrumacao" runat="server" Height="23px" ImageUrl="~/images/StatusAptoAmarelo.gif"
                                                Width="35px" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td valign="top">
                                            <asp:GridView ID="gdvArrumacao1" runat="server" AutoGenerateColumns="False" CellPadding="4"
                                                DataKeyNames="ApaId,ApaDesc,SaidoManutencao,AgoId,ApaCCusto,ApaArea" ForeColor="#333333"
                                                GridLines="None" ShowHeader="False">
                                                <RowStyle BackColor="#EFF3FB" />
                                                <Columns>
                                                    <asp:TemplateField ShowHeader="False" ItemStyle-CssClass="zoom" ItemStyle-Wrap="False">
                                                        <ItemTemplate>
                                                            <asp:CheckBox ID="chkLimpo1" runat="server" CssClass="ColocaHand" ToolTip="SELECIONE PARA LIMPAR" />
                                                            <cc1:ToggleButtonExtender ID="chkLimpo1_ToggleButtonExtender" runat="server" CheckedImageUrl="~/images/Checked.gif" Enabled="True" ImageHeight="20" ImageWidth="20" TargetControlID="chkLimpo1" UncheckedImageUrl="~/images/un-checked.gif">
                                                            </cc1:ToggleButtonExtender>
                                                            <asp:Label ID="lblArrumacao1" runat="server" Text="lblArru1"></asp:Label>
                                                            <cc1:BalloonPopupExtender ID="lblArrumacao1_BalloonPopupExtender" runat="server" BalloonPopupControlID="pnlPopArrumacao" BalloonSize="Medium" Enabled="True" ExtenderControlID="" ScrollBars="None" TargetControlID="lblArrumacao1" DisplayOnMouseOver="False" DisplayOnFocus="False" BalloonStyle="Rectangle">
                                                            </cc1:BalloonPopupExtender>
                                                        </ItemTemplate>
                                                        <ItemStyle HorizontalAlign="Left" />
                                                    </asp:TemplateField>
                                                </Columns>
                                                <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                                                <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
                                                <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                                                <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                                                <EditRowStyle BackColor="#2461BF" />
                                                <AlternatingRowStyle BackColor="White" />
                                            </asp:GridView>
                                        </td>
                                        <td valign="top">
                                            <asp:GridView ID="gdvArrumacao2" runat="server" AutoGenerateColumns="False" CellPadding="4"
                                                DataKeyNames="ApaId,ApaDesc,SaidoManutencao,AgoId,ApaCCusto,ApaArea" ForeColor="#333333"
                                                GridLines="None" ShowHeader="False">
                                                <RowStyle BackColor="#EFF3FB" />
                                                <Columns>
                                                    <asp:TemplateField ShowHeader="False" ItemStyle-CssClass="zoom" ItemStyle-Wrap="False">
                                                        <ItemTemplate>
                                                            <asp:CheckBox ID="chkLimpo2" runat="server" CssClass="ColocaHand" ToolTip="SELECIONE PARA LIMPAR" />
                                                            <cc1:ToggleButtonExtender ID="chkLimpo2_ToggleButtonExtender" runat="server" CheckedImageUrl="~/images/Checked.gif"
                                                                Enabled="True" ImageHeight="20" ImageWidth="20" TargetControlID="chkLimpo2" UncheckedImageUrl="~/images/un-checked.gif">
                                                            </cc1:ToggleButtonExtender>
                                                            <asp:Label ID="lblArrumacao2" runat="server" CssClass="ColocaHand" Text="lblArru2"></asp:Label>
                                                            <cc1:BalloonPopupExtender ID="lblArrumacao2_BalloonPopupExtender" runat="server"
                                                                BalloonPopupControlID="pnlPopArrumacao" BalloonSize="Medium" CustomCssUrl=""
                                                                DynamicServicePath="" Enabled="True" ExtenderControlID="" ScrollBars="None" TargetControlID="lblArrumacao2">
                                                            </cc1:BalloonPopupExtender>
                                                        </ItemTemplate>
                                                        <ItemStyle HorizontalAlign="Left" />
                                                    </asp:TemplateField>
                                                </Columns>
                                                <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                                                <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
                                                <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                                                <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                                                <EditRowStyle BackColor="#2461BF" />
                                                <AlternatingRowStyle BackColor="White" />
                                            </asp:GridView>
                                        </td>
                                        <td valign="top">
                                            <asp:GridView ID="gdvArrumacao3" runat="server" AutoGenerateColumns="False" CellPadding="4"
                                                DataKeyNames="ApaId,ApaDesc,SaidoManutencao,AgoId,ApaCCusto,ApaArea" ForeColor="#333333"
                                                GridLines="None" ShowHeader="False">
                                                <RowStyle BackColor="#EFF3FB" />
                                                <Columns>
                                                    <asp:TemplateField ShowHeader="False" ItemStyle-CssClass="zoom" ItemStyle-Wrap="False">
                                                        <ItemTemplate>
                                                            <asp:CheckBox ID="chkLimpo3" runat="server" CssClass="ColocaHand" ToolTip="SELECIONE PARA LIMPAR" />
                                                            <cc1:ToggleButtonExtender ID="chkLimpo3_ToggleButtonExtender" runat="server" CheckedImageUrl="~/images/Checked.gif"
                                                                Enabled="True" ImageHeight="20" ImageWidth="20" TargetControlID="chkLimpo3" UncheckedImageUrl="~/images/un-checked.gif">
                                                            </cc1:ToggleButtonExtender>
                                                            <asp:Label ID="lblArrumacao3" runat="server" CssClass="ColocaHand" Text="lblArru3"></asp:Label>
                                                            <cc1:BalloonPopupExtender ID="lblArrumacao3_BalloonPopupExtender" runat="server"
                                                                BalloonPopupControlID="pnlPopArrumacao" BalloonSize="Medium" CustomCssUrl=""
                                                                DynamicServicePath="" Enabled="True" ExtenderControlID="" ScrollBars="None" TargetControlID="lblArrumacao3">
                                                            </cc1:BalloonPopupExtender>
                                                        </ItemTemplate>
                                                        <ItemStyle HorizontalAlign="Left" />
                                                    </asp:TemplateField>
                                                </Columns>
                                                <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                                                <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
                                                <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                                                <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                                                <EditRowStyle BackColor="#2461BF" />
                                                <AlternatingRowStyle BackColor="White" />
                                            </asp:GridView>
                                        </td>
                                        <td valign="top">
                                            <asp:GridView ID="gdvArrumacao4" runat="server" AutoGenerateColumns="False" CellPadding="4"
                                                DataKeyNames="ApaId,ApaDesc,SaidoManutencao,AgoId,ApaCCusto,ApaArea" ForeColor="#333333"
                                                GridLines="None" ShowHeader="False">
                                                <RowStyle BackColor="#EFF3FB" />
                                                <Columns>
                                                    <asp:TemplateField ShowHeader="False" ItemStyle-CssClass="zoom" ItemStyle-Wrap="False">
                                                        <ItemTemplate>
                                                            <asp:CheckBox ID="chkLimpo4" runat="server" CssClass="ColocaHand" ToolTip="SELECIONE PARA LIMPAR" />
                                                            <cc1:ToggleButtonExtender ID="chkLimpo4_ToggleButtonExtender" runat="server" CheckedImageUrl="~/images/Checked.gif"
                                                                Enabled="True" ImageHeight="20" ImageWidth="20" TargetControlID="chkLimpo4" UncheckedImageUrl="~/images/un-checked.gif">
                                                            </cc1:ToggleButtonExtender>
                                                            <asp:Label ID="lblArrumacao4" runat="server" CssClass="ColocaHand" Text="lblArru4"></asp:Label>
                                                            <cc1:BalloonPopupExtender ID="lblArrumacao4_BalloonPopupExtender" runat="server"
                                                                BalloonPopupControlID="pnlPopArrumacao" BalloonSize="Medium" CustomCssUrl=""
                                                                DynamicServicePath="" Enabled="True" ExtenderControlID="" ScrollBars="None" TargetControlID="lblArrumacao4">
                                                            </cc1:BalloonPopupExtender>
                                                        </ItemTemplate>
                                                        <ItemStyle HorizontalAlign="Left" />
                                                    </asp:TemplateField>
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
                                        <td align="left" valign="bottom" class="zoom">
                                            <asp:CheckBox ID="chkLimpaTodos" runat="server" AutoPostBack="True" CssClass="ColocaHand"
                                                Text="Todos" />
                                            <cc1:ToggleButtonExtender ID="chkLimpaTodos_ToggleButtonExtender" runat="server"
                                                CheckedImageUrl="~/images/Checked.gif" Enabled="True" ImageHeight="20" ImageWidth="20"
                                                TargetControlID="chkLimpaTodos" UncheckedImageUrl="~/images/un-checked.gif">
                                            </cc1:ToggleButtonExtender>
                                        </td>
                                        <td></td>
                                        <td></td>
                                        <td align="right" valign="bottom">&nbsp;
                                            <asp:Button ID="btnLimpar" runat="server" CssClass="imgGravar" OnClientClick="if(confirm('Confirma limpeza dos apartamentos selecionados?'))
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
                                                Text="   Limpar" />
                                        </td>
                                    </tr>
                                </table>
                            </asp:Panel>
                        </td>
                        <td valign="top">
                            <asp:Panel ID="pnlOcupado" runat="server" Visible="False" CssClass="ArrendodarBorda">
                                <table>
                                    <tr>
                                        <td align="center" class="formRuler" colspan="4" valign="bottom">
                                            <asp:Label ID="lblTotOcupado" runat="server"></asp:Label>
                                            &nbsp;<asp:LinkButton ID="lnkOcupado" runat="server">Ocupado</asp:LinkButton>
                                            &nbsp;<asp:Image ID="imgOcupado" runat="server" ImageUrl="~/images/StatusAptoAzul.gif"
                                                Style="height: 20px" Height="23px" Width="35px" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td valign="top">
                                            <asp:GridView ID="gdvOcupado1" runat="server" AutoGenerateColumns="False" CellPadding="4"
                                                DataKeyNames="ApaId,ApaDesc,HosDataFimSol,Emprestimos,HosDataFimReal,CSeOrigem,CSeDescricao,ApaCCusto,ApaArea"
                                                ForeColor="#333333" GridLines="None" ShowHeader="False">
                                                <RowStyle BackColor="#EFF3FB" />
                                                <Columns>
                                                    <asp:TemplateField ShowHeader="False" ItemStyle-CssClass="zoom" ItemStyle-Wrap="False">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblOcupado1" runat="server" CssClass="ColocaHand" EnableTheming="True">lblOcu1</asp:Label>
                                                            <cc1:BalloonPopupExtender ID="lblOcupado1_BalloonPopupExtender" runat="server" BalloonPopupControlID="pnlPopOcupado"
                                                                BalloonSize="Medium" CustomCssUrl="" DynamicServicePath="" Enabled="True" ExtenderControlID=""
                                                                ScrollBars="None" TargetControlID="lblOcupado1">
                                                            </cc1:BalloonPopupExtender>
                                                            <asp:Image ID="imgEmprestimos" runat="server" Height="15px" ImageUrl="~/images/Emprestimo.gif"
                                                                ToolTip="Empréstimos" CssClass="ColocaHand" />
                                                        </ItemTemplate>
                                                        <ItemStyle HorizontalAlign="Left" />
                                                    </asp:TemplateField>
                                                </Columns>
                                                <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                                                <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
                                                <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                                                <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                                                <EditRowStyle BackColor="#2461BF" />
                                                <AlternatingRowStyle BackColor="White" />
                                            </asp:GridView>
                                        </td>
                                        <td valign="top">
                                            <asp:GridView ID="gdvOcupado2" runat="server" AutoGenerateColumns="False" CellPadding="4"
                                                DataKeyNames="ApaId,ApaDesc,HosDataFimSol,Emprestimos,HosDataFimReal,CSeOrigem,CSeDescricao,ApaCCusto,ApaArea"
                                                ForeColor="#333333" GridLines="None" ShowHeader="False">
                                                <RowStyle BackColor="#EFF3FB" />
                                                <Columns>
                                                    <asp:TemplateField ShowHeader="False" ItemStyle-CssClass="zoom" ItemStyle-Wrap="False">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblOcupado2" runat="server" CssClass="ColocaHand" Text="lblOcu2"></asp:Label>
                                                            <cc1:BalloonPopupExtender ID="lblOcupado2_BalloonPopupExtender" runat="server" BalloonPopupControlID="pnlPopOcupado"
                                                                BalloonSize="Medium" CustomCssUrl="" DynamicServicePath="" Enabled="True" ExtenderControlID=""
                                                                TargetControlID="lblOcupado2">
                                                            </cc1:BalloonPopupExtender>
                                                            <asp:Image ID="imgEmprestimos" runat="server" Height="15px" ImageUrl="~/images/Emprestimo.gif"
                                                                ToolTip="Empréstimos" CssClass="ColocaHand" />
                                                        </ItemTemplate>
                                                        <ItemStyle HorizontalAlign="Left" />
                                                    </asp:TemplateField>
                                                </Columns>
                                                <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                                                <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
                                                <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                                                <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                                                <EditRowStyle BackColor="#2461BF" />
                                                <AlternatingRowStyle BackColor="White" />
                                            </asp:GridView>
                                        </td>
                                        <td valign="top">
                                            <asp:GridView ID="gdvOcupado3" runat="server" AutoGenerateColumns="False" CellPadding="4"
                                                DataKeyNames="ApaId,ApaDesc,HosDataFimSol,Emprestimos,HosDataFimReal,CSeOrigem,CSeDescricao,ApaCCusto,ApaArea"
                                                ForeColor="#333333" GridLines="None" ShowHeader="False">
                                                <RowStyle BackColor="#EFF3FB" />
                                                <Columns>
                                                    <asp:TemplateField ShowHeader="False" ItemStyle-CssClass="zoom" ItemStyle-Wrap="False">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblOcupado3" runat="server" CssClass="ColocaHand" Text="lblOcu3"></asp:Label>
                                                            <cc1:BalloonPopupExtender ID="lblOcupado3_BalloonPopupExtender" runat="server" BalloonPopupControlID="pnlPopOcupado"
                                                                BalloonSize="Medium" CustomCssUrl="" DynamicServicePath="" Enabled="True" ExtenderControlID=""
                                                                ScrollBars="None" TargetControlID="lblOcupado3">
                                                            </cc1:BalloonPopupExtender>
                                                            <asp:Image ID="imgEmprestimos" runat="server" Height="15px" ImageUrl="~/images/Emprestimo.gif"
                                                                ToolTip="Empréstimos" CssClass="ColocaHand" />
                                                        </ItemTemplate>
                                                        <ItemStyle HorizontalAlign="Left" />
                                                    </asp:TemplateField>
                                                </Columns>
                                                <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                                                <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
                                                <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                                                <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                                                <EditRowStyle BackColor="#2461BF" />
                                                <AlternatingRowStyle BackColor="White" />
                                            </asp:GridView>
                                        </td>
                                        <td valign="top">
                                            <asp:GridView ID="gdvOcupado4" runat="server" AutoGenerateColumns="False" CellPadding="4"
                                                DataKeyNames="ApaId,ApaDesc,HosDataFimSol,Emprestimos,HosDataFimReal,CSeOrigem,CSeDescricao,ApaCCusto,ApaArea"
                                                ForeColor="#333333" GridLines="None" ShowHeader="False">
                                                <RowStyle BackColor="#EFF3FB" />
                                                <Columns>
                                                    <asp:TemplateField ShowHeader="False" ItemStyle-CssClass="zoom" ItemStyle-Wrap="False">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblOcupado4" runat="server" CssClass="ColocaHand" Text="lblOcu4"></asp:Label>
                                                            <cc1:BalloonPopupExtender ID="lblOcupado4_BalloonPopupExtender" runat="server" BalloonPopupControlID="pnlPopOcupado"
                                                                BalloonSize="Medium" CustomCssUrl="" DynamicServicePath="" Enabled="True" ExtenderControlID=""
                                                                ScrollBars="None" TargetControlID="lblOcupado4">
                                                            </cc1:BalloonPopupExtender>
                                                            <asp:Image ID="imgEmprestimos" runat="server" Height="15px" ImageUrl="~/images/Emprestimo.gif"
                                                                ToolTip="Empréstimos" CssClass="ColocaHand" />
                                                        </ItemTemplate>
                                                        <ItemStyle HorizontalAlign="Left" />
                                                    </asp:TemplateField>
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
                                        <td>&nbsp;
                                        </td>
                                        <td>&nbsp;
                                        </td>
                                        <td>&nbsp;
                                        </td>
                                        <td valign="top">&nbsp;
                                        </td>
                                    </tr>
                                </table>
                            </asp:Panel>
                        </td>
                        <td valign="top">
                            <asp:Panel ID="pnlManutencao" runat="server" Visible="False" CssClass="ArrendodarBorda">
                                <table style="width: 100%;">
                                    <tr>
                                        <td align="center" valign="bottom" colspan="2" class="formRuler">&nbsp;<asp:Label ID="lblTotManutencao" runat="server" Text=""></asp:Label>
                                            &nbsp;<asp:LinkButton ID="lnkManutencao" runat="server">Manutenção</asp:LinkButton>
                                            &nbsp;<asp:Image ID="imgManutencao" runat="server" ImageUrl="~/images/StatusAptoCinza.gif"
                                                Height="23px" Width="35px" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td valign="top" class="auto-style1">
                                            <asp:GridView ID="gdvManutencao1" runat="server" AutoGenerateColumns="False" CellPadding="4"
                                                ForeColor="#333333" GridLines="None" ShowHeader="False" DataKeyNames="ApaId,ApaDesc,ManDescricaoManut,ManDataAbertura,SolDataFim,ApaCCusto,ApaArea">
                                                <RowStyle BackColor="#EFF3FB" />
                                                <Columns>
                                                    <asp:TemplateField ShowHeader="False" ItemStyle-CssClass="zoom" ItemStyle-Wrap="False">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblManutencao1" runat="server" CssClass="ColocaHand"
                                                                Text="lblMan1"></asp:Label>
                                                            <cc1:BalloonPopupExtender ID="lblManutencao1_BalloonPopupExtender" runat="server"
                                                                BalloonPopupControlID="pnlPopManutencao" BalloonSize="Medium" CustomCssUrl=""
                                                                DynamicServicePath="" Enabled="True" ExtenderControlID="" ScrollBars="None" TargetControlID="lblManutencao1">
                                                            </cc1:BalloonPopupExtender>
                                                        </ItemTemplate>
                                                        <ItemStyle HorizontalAlign="Left" />
                                                    </asp:TemplateField>
                                                </Columns>
                                                <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                                                <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
                                                <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                                                <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                                                <EditRowStyle BackColor="#2461BF" />
                                                <AlternatingRowStyle BackColor="White" />
                                            </asp:GridView>
                                        </td>
                                        <td valign="top">
                                            <asp:GridView ID="gdvManutencao2" runat="server" AutoGenerateColumns="False" CellPadding="4"
                                                DataKeyNames="ApaId,ApaDesc,ManDescricaoManut,ManDataAbertura,SolDataFim,ApaCCusto,ApaArea"
                                                ForeColor="#333333" GridLines="None" ShowHeader="False">
                                                <RowStyle BackColor="#EFF3FB" />
                                                <Columns>
                                                    <asp:TemplateField ShowHeader="False" ItemStyle-CssClass="zoom" ItemStyle-Wrap="False">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblManutencao2" runat="server" CssClass="ColocaHand"
                                                                Text="lblMan2"></asp:Label>
                                                            <cc1:BalloonPopupExtender ID="lblManutencao2_BalloonPopupExtender" runat="server"
                                                                BalloonPopupControlID="pnlPopManutencao" BalloonSize="Medium" CustomCssUrl=""
                                                                DynamicServicePath="" Enabled="True" ExtenderControlID="" ScrollBars="None" TargetControlID="lblManutencao2">
                                                            </cc1:BalloonPopupExtender>
                                                        </ItemTemplate>
                                                        <ItemStyle HorizontalAlign="Left" />
                                                    </asp:TemplateField>
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
                                        <td class="auto-style1">&nbsp;
                                        </td>
                                        <td>&nbsp;
                                        </td>
                                    </tr>
                                </table>
                                <br />
                            </asp:Panel>
                        </td>
                        <td valign="top">
                            <asp:Panel ID="pnlLimpo" runat="server" Visible="False" CssClass="ArrendodarBorda">
                                <table>
                                    <tr>
                                        <td align="center" class="formRuler" colspan="4" valign="bottom">
                                            <asp:Label ID="lblTotLimpo" runat="server"></asp:Label>
                                            <asp:Label ID="lblTituloLimpo" runat="server" Text="Limpo"></asp:Label>
                                            &nbsp;<asp:LinkButton ID="LinkButton1" runat="server" Visible="False">Limpo</asp:LinkButton>
                                            &nbsp;<asp:Image ID="imgLimpo" runat="server" Height="23px" ImageUrl="~/images/StatusAptoBranco.gif"
                                                Width="35px" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td valign="top">
                                            <asp:GridView ID="gdvLimpo1" runat="server" AutoGenerateColumns="False" CellPadding="4"
                                                DataKeyNames="ApaId,ApaDesc,DataLimpoLog,UsuarioLimpoLog,ApaCCusto,ApaArea" ForeColor="#333333"
                                                GridLines="None" ShowHeader="False">
                                                <RowStyle BackColor="#EFF3FB" />
                                                <Columns>
                                                    <asp:TemplateField ShowHeader="False" ItemStyle-CssClass="zoom" ItemStyle-Wrap="False">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblLimpo1" runat="server" CssClass="ColocaHand" Text="lblLim1"></asp:Label>
                                                            <cc1:BalloonPopupExtender ID="lblLimpo1_BalloonPopupExtender" runat="server" BalloonPopupControlID="pnlPopLimpo"
                                                                BalloonSize="Medium" CustomCssUrl="" DynamicServicePath="" Enabled="True" ExtenderControlID=""
                                                                ScrollBars="None" TargetControlID="lblLimpo1">
                                                            </cc1:BalloonPopupExtender>
                                                        </ItemTemplate>
                                                        <ItemStyle HorizontalAlign="Left" />
                                                    </asp:TemplateField>
                                                </Columns>
                                                <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                                                <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
                                                <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                                                <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                                                <EditRowStyle BackColor="#2461BF" />
                                                <AlternatingRowStyle BackColor="White" />
                                            </asp:GridView>
                                        </td>
                                        <td valign="top">
                                            <asp:GridView ID="gdvLimpo2" runat="server" AutoGenerateColumns="False" CellPadding="4"
                                                DataKeyNames="ApaId,ApaDesc,DataLimpoLog,UsuarioLimpoLog,ApaCCusto,ApaArea" ForeColor="#333333"
                                                GridLines="None" ShowHeader="False">
                                                <RowStyle BackColor="#EFF3FB" />
                                                <Columns>
                                                    <asp:TemplateField ShowHeader="False" ItemStyle-CssClass="zoom" ItemStyle-Wrap="False">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblLimpo2" runat="server" CssClass="ColocaHand" Text="lblLim2"></asp:Label>
                                                            <cc1:BalloonPopupExtender ID="lblLimpo2_BalloonPopupExtender" runat="server" BalloonPopupControlID="pnlPopLimpo"
                                                                BalloonSize="Medium" CustomCssUrl="" DynamicServicePath="" Enabled="True" ExtenderControlID=""
                                                                ScrollBars="None" TargetControlID="lblLimpo2">
                                                            </cc1:BalloonPopupExtender>
                                                        </ItemTemplate>
                                                        <ItemStyle HorizontalAlign="Left" />
                                                    </asp:TemplateField>
                                                </Columns>
                                                <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                                                <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
                                                <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                                                <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                                                <EditRowStyle BackColor="#2461BF" />
                                                <AlternatingRowStyle BackColor="White" />
                                            </asp:GridView>
                                        </td>
                                        <td valign="top">
                                            <asp:GridView ID="gdvLimpo3" runat="server" AutoGenerateColumns="False" CellPadding="4"
                                                DataKeyNames="ApaId,ApaDesc,DataLimpoLog,UsuarioLimpoLog,ApaCCusto,ApaArea" ForeColor="#333333"
                                                GridLines="None" ShowHeader="False">
                                                <RowStyle BackColor="#EFF3FB" />
                                                <Columns>
                                                    <asp:TemplateField ShowHeader="False" ItemStyle-CssClass="zoom" ItemStyle-Wrap="False">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblLimpo3" runat="server" CssClass="ColocaHand" Text="lblLim3"></asp:Label>
                                                            <cc1:BalloonPopupExtender ID="lblLimpo3_BalloonPopupExtender" runat="server" BalloonPopupControlID="pnlPopLimpo"
                                                                BalloonSize="Medium" CustomCssUrl="" DynamicServicePath="" Enabled="True" ExtenderControlID=""
                                                                ScrollBars="None" TargetControlID="lblLimpo3">
                                                            </cc1:BalloonPopupExtender>
                                                        </ItemTemplate>
                                                        <ItemStyle HorizontalAlign="Left" />
                                                    </asp:TemplateField>
                                                </Columns>
                                                <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                                                <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
                                                <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                                                <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                                                <EditRowStyle BackColor="#2461BF" />
                                                <AlternatingRowStyle BackColor="White" />
                                            </asp:GridView>
                                        </td>
                                        <td valign="top">
                                            <asp:GridView ID="gdvLimpo4" runat="server" AutoGenerateColumns="False" CellPadding="4"
                                                DataKeyNames="ApaId,ApaDesc,DataLimpoLog,UsuarioLimpoLog,ApaCCusto,ApaArea" ForeColor="#333333"
                                                GridLines="None" ShowHeader="False">
                                                <RowStyle BackColor="#EFF3FB" />
                                                <Columns>
                                                    <asp:TemplateField ShowHeader="False" ItemStyle-CssClass="zoom" ItemStyle-Wrap="False">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblLimpo4" runat="server" CssClass="ColocaHand" Text="lblLim4"></asp:Label>
                                                            <cc1:BalloonPopupExtender ID="lblLimpo4_BalloonPopupExtender" runat="server" BalloonPopupControlID="pnlPopLimpo"
                                                                BalloonSize="Medium" CustomCssUrl="" DynamicServicePath="" Enabled="True" ExtenderControlID=""
                                                                ScrollBars="None" TargetControlID="lblLimpo4">
                                                            </cc1:BalloonPopupExtender>
                                                        </ItemTemplate>
                                                        <ItemStyle HorizontalAlign="Left" />
                                                    </asp:TemplateField>
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
                                        <td>&nbsp;
                                        </td>
                                        <td>&nbsp;
                                        </td>
                                        <td>&nbsp;
                                        </td>
                                        <td>&nbsp;
                                        </td>
                                    </tr>
                                </table>
                            </asp:Panel>

                            <asp:Panel ID="pnlCompletarEnxoval" runat="server" CssClass="ArrendodarBorda" Visible="False">
                                <table style="width: 100%;">
                                    <tr>
                                        <td align="center" class="formRuler" colspan="2">Completar enxoval</td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:GridView ID="gdvCompletarEnxoval1" runat="server" AutoGenerateColumns="False" CellPadding="4" DataKeyNames="resId,enxCamaExtra,enxCamaExtraAtendida,enxBerco,enxBercoAtendido,enxBanheira,enxBanheiraAtendida,enxUsuarioSolicitante,enxUsuarioDataSolicitante,enxUsuarioAtendimento,enxUsuarioDataAtendimento,enxMotivoNaoAtendimento,apaDesc,solId" ForeColor="#333333" GridLines="None" ShowHeader="False">
                                                <AlternatingRowStyle BackColor="White" />
                                                <Columns>
                                                    <asp:TemplateField HeaderText="Apartamento">
                                                        <ItemTemplate>
                                                            <asp:CheckBox ID="chkEnxoval1" runat="server" />
                                                            <cc1:ToggleButtonExtender ID="chkEnxoval1_ToggleButtonExtender" runat="server" BehaviorID="chkEnxoval1_ToggleButtonExtender" CheckedImageUrl="~/images/Checked.gif" ImageHeight="20" ImageWidth="20" TargetControlID="chkEnxoval1" UncheckedImageUrl="~/images/un-checked.gif">
                                                            </cc1:ToggleButtonExtender>
                                                            <asp:Label ID="lblAptoEnxoval1" runat="server">lblAptoEnxoval1</asp:Label>
                                                        </ItemTemplate>
                                                        <HeaderStyle CssClass="formRuller" />
                                                        <ItemStyle CssClass="zoom" Wrap="False" />
                                                    </asp:TemplateField>
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
                                        </td>
                                        <td>
                                            <asp:GridView ID="gdvCompletarEnxoval2" runat="server" AutoGenerateColumns="False" CellPadding="4" DataKeyNames="resId,enxCamaExtra,enxCamaExtraAtendida,enxBerco,enxBercoAtendido,enxBanheira,enxBanheiraAtendida,enxUsuarioSolicitante,enxUsuarioDataSolicitante,enxUsuarioAtendimento,enxUsuarioDataAtendimento,enxMotivoNaoAtendimento,apaDesc,solId" ForeColor="#333333" GridLines="None" ShowHeader="False">
                                                <AlternatingRowStyle BackColor="White" />
                                                <Columns>
                                                    <asp:TemplateField HeaderText="Apartamento">
                                                        <ItemTemplate>
                                                            <asp:CheckBox ID="chkEnxoval2" runat="server" />
                                                            <cc1:ToggleButtonExtender ID="chkEnxoval2_ToggleButtonExtender" runat="server" BehaviorID="chkEnxoval2_ToggleButtonExtender" CheckedImageUrl="~/images/Checked.gif" ImageHeight="20" ImageWidth="20" TargetControlID="chkEnxoval2" UncheckedImageUrl="~/images/un-checked.gif">
                                                            </cc1:ToggleButtonExtender>
                                                            <asp:Label ID="lblAptoEnxoval2" runat="server">lblAptoEnxoval2</asp:Label>
                                                        </ItemTemplate>
                                                        <HeaderStyle CssClass="formRuller" />
                                                        <ItemStyle CssClass="zoom" Wrap="False" />
                                                    </asp:TemplateField>
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
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="left" valign="bottom">
                                            <asp:CheckBox ID="chkTodosEnxoval" runat="server" AutoPostBack="True" Text="Todos" />
                                            <asp:ImageButton ID="imgAtualizaTodosEnxoval" runat="server" CssClass="ColocaHand" ImageUrl="~/images/Refresh.png" ToolTip="Clique para atualizar a lista" />
                                            <cc1:ToggleButtonExtender ID="chkTodosEnxoval_ToggleButtonExtender" runat="server" BehaviorID="chkTodosEnxoval_ToggleButtonExtender" CheckedImageUrl="~/images/Checked.gif" ImageHeight="20" ImageWidth="20" TargetControlID="chkTodosEnxoval" UncheckedImageUrl="~/images/un-checked.gif">
                                            </cc1:ToggleButtonExtender>
                                        </td>
                                        <td>
                                            <asp:Button ID="btnConfirmarEnxoval" runat="server" CssClass="imgGravar" Text="  Completar" ToolTip="Completar todos selecionados"
                                                OnClientClick="if(confirm('Os enxovais serão completados para os apartamentos selecionados.\n\nConfirma?'))
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
                                        };" />
                                        </td>
                                    </tr>
                                </table>
                            </asp:Panel>

                            <div runat="server" id="divEnxAtendido" class="ArrendodarBorda">
                                <table style="width: 100%;">
                                    <tr>
                                        <td align="center" class="formRuler" colspan="4">Enxoval completado</td>
                                    </tr>
                                    <tr>
                                        <td valign="top">
                                            <asp:GridView ID="gdvEnxAtendido1" runat="server" AutoGenerateColumns="False" DataKeyNames="resId,enxCamaExtra,enxCamaExtraAtendida,enxBerco,enxBercoAtendido,enxBanheira,enxBanheiraAtendida,enxUsuarioSolicitante,enxUsuarioDataSolicitante,enxUsuarioAtendimento,enxUsuarioDataAtendimento,enxMotivoNaoAtendimento,apaDesc,solId" CellPadding="4" ForeColor="#333333" GridLines="None" ShowHeader="False">
                                                <AlternatingRowStyle BackColor="White" />
                                                <Columns>
                                                    <asp:TemplateField>
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblEnxCompletado1" runat="server"></asp:Label>
                                                        </ItemTemplate>
                                                        <ItemStyle CssClass="zoom" Wrap="False" />
                                                    </asp:TemplateField>
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
                                        </td>
                                        <td valign="top">
                                            <asp:GridView ID="gdvEnxAtendido2" runat="server" AutoGenerateColumns="False" DataKeyNames="resId,enxCamaExtra,enxCamaExtraAtendida,enxBerco,enxBercoAtendido,enxBanheira,enxBanheiraAtendida,enxUsuarioSolicitante,enxUsuarioDataSolicitante,enxUsuarioAtendimento,enxUsuarioDataAtendimento,enxMotivoNaoAtendimento,apaDesc,solId" CellPadding="4" ForeColor="#333333" GridLines="None" ShowHeader="False">
                                                <AlternatingRowStyle BackColor="White" />
                                                <Columns>
                                                    <asp:TemplateField>
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblEnxCompletado2" runat="server"></asp:Label>
                                                        </ItemTemplate>
                                                        <ItemStyle CssClass="zoom" Wrap="False" />
                                                    </asp:TemplateField>
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
                                        </td>
                                        <td valign="top">
                                            <asp:GridView ID="gdvEnxAtendido3" runat="server" AutoGenerateColumns="False" DataKeyNames="resId,enxCamaExtra,enxCamaExtraAtendida,enxBerco,enxBercoAtendido,enxBanheira,enxBanheiraAtendida,enxUsuarioSolicitante,enxUsuarioDataSolicitante,enxUsuarioAtendimento,enxUsuarioDataAtendimento,enxMotivoNaoAtendimento,apaDesc,solId" CellPadding="4" ForeColor="#333333" GridLines="None" ShowHeader="False">
                                                <AlternatingRowStyle BackColor="White" />
                                                <Columns>
                                                    <asp:TemplateField>
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblEnxCompletado3" runat="server"></asp:Label>
                                                        </ItemTemplate>
                                                        <ItemStyle CssClass="zoom" Wrap="False" />
                                                    </asp:TemplateField>
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
                                        </td>
                                        <td valign="top">
                                            <asp:GridView ID="gdvEnxAtendido4" runat="server" AutoGenerateColumns="False" DataKeyNames="resId,enxCamaExtra,enxCamaExtraAtendida,enxBerco,enxBercoAtendido,enxBanheira,enxBanheiraAtendida,enxUsuarioSolicitante,enxUsuarioDataSolicitante,enxUsuarioAtendimento,enxUsuarioDataAtendimento,enxMotivoNaoAtendimento,apaDesc,solId" CellPadding="4" ForeColor="#333333" GridLines="None" ShowHeader="False">
                                                <AlternatingRowStyle BackColor="White" />
                                                <Columns>
                                                    <asp:TemplateField>
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblEnxCompletado4" runat="server"></asp:Label>
                                                        </ItemTemplate>
                                                        <ItemStyle CssClass="zoom" Wrap="False" />
                                                    </asp:TemplateField>
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
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>&nbsp;</td>
                                        <td>&nbsp;</td>
                                        <td>&nbsp;</td>
                                        <td>&nbsp;</td>
                                    </tr>
                                </table>
                            </div>
                        </td>
                        <td valign="top">
                            <asp:Panel ID="PnlConferencia" runat="server" Visible="False" CssClass="ArrendodarBorda">
                                <table style="width: 100%;">
                                    <tr>
                                        <td align="center" valign="bottom" colspan="2" class="formRuler">&nbsp;<asp:Label ID="lblTotConferencia" runat="server"></asp:Label>
                                            &nbsp;Conferência
                                            <asp:Image ID="imgConferencia" runat="server" Height="23px" ImageUrl="~/images/StatusCamaRosa.gif"
                                                Width="35px" />
                                            &nbsp;
                                        </td>
                                    </tr>
                                    <tr>
                                        <td valign="top">
                                            <asp:GridView ID="gdvConferencia1" runat="server" AutoGenerateColumns="False" CellPadding="4"
                                                ForeColor="#333333" GridLines="None" ShowHeader="False" DataKeyNames="ApaId,ApaDesc,Emprestimos,DescEmprestimos">
                                                <RowStyle BackColor="#EFF3FB" />
                                                <Columns>
                                                    <asp:TemplateField ItemStyle-CssClass="zoom" ItemStyle-Wrap="False">
                                                        <ItemTemplate>
                                                            <asp:CheckBox ID="chkConferencia1" runat="server" CssClass="ColocaHand" />
                                                            <cc1:ToggleButtonExtender ID="chkConferencia1_ToggleButtonExtender" runat="server"
                                                                Enabled="True" TargetControlID="chkConferencia1" UncheckedImageUrl="~/images/un-checked.gif"
                                                                CheckedImageUrl="~/images/Checked.gif" ImageHeight="20" ImageWidth="20">
                                                            </cc1:ToggleButtonExtender>
                                                            <asp:ImageButton ID="imgEmprestimo1C" runat="server" ImageUrl="~/images/Emprestimo.gif"
                                                                CssClass="ColocaHand" Height="15px" />
                                                        </ItemTemplate>
                                                        <ItemStyle HorizontalAlign="Left" />
                                                    </asp:TemplateField>
                                                </Columns>
                                                <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                                                <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
                                                <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                                                <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                                                <EditRowStyle BackColor="#2461BF" />
                                                <AlternatingRowStyle BackColor="White" />
                                            </asp:GridView>
                                        </td>
                                        <td valign="top">
                                            <asp:GridView ID="gdvConferencia2" runat="server" AutoGenerateColumns="False" CellPadding="4"
                                                ForeColor="#333333" GridLines="None" ShowHeader="False" DataKeyNames="ApaId,ApaDesc,Emprestimos,DescEmprestimos">
                                                <RowStyle BackColor="#EFF3FB" />
                                                <Columns>
                                                    <asp:TemplateField ItemStyle-CssClass="zoom" ItemStyle-Wrap="False">
                                                        <ItemTemplate>
                                                            <asp:CheckBox ID="chkConferencia2" runat="server" CssClass="ColocaHand" />
                                                            <cc1:ToggleButtonExtender ID="chkConferencia2_ToggleButtonExtender" runat="server"
                                                                Enabled="True" TargetControlID="chkConferencia2" UncheckedImageUrl="~/images/un-checked.gif"
                                                                CheckedImageUrl="~/images/Checked.gif" ImageHeight="20" ImageWidth="20">
                                                            </cc1:ToggleButtonExtender>
                                                            <asp:ImageButton ID="imgEmprestimo2C" runat="server" ImageUrl="~/images/Emprestimo.gif"
                                                                CssClass="ColocaHand" Height="15px" />
                                                        </ItemTemplate>
                                                        <ItemStyle HorizontalAlign="Left" />
                                                    </asp:TemplateField>
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
                                        <td valign="bottom" align="left">
                                            <asp:CheckBox ID="chkSelecionaTodos" runat="server" AutoPostBack="True" Text="Todos" />
                                            <asp:ImageButton ID="btnAtualizaConf" runat="server" CssClass="ColocaHand" ImageUrl="~/images/Refresh.png"
                                                ToolTip="Clique para atualizar a conferência" />
                                            <cc1:ToggleButtonExtender ID="chkSelecionaTodos_ToggleButtonExtender" runat="server"
                                                Enabled="True" TargetControlID="chkSelecionaTodos" UncheckedImageUrl="~/images/un-checked.gif"
                                                CheckedImageUrl="~/images/Checked.gif" ImageHeight="20" ImageWidth="20">
                                            </cc1:ToggleButtonExtender>
                                        </td>
                                        <td align="right" valign="bottom">&nbsp;<asp:Button ID="btnConferenciaSelecionados" runat="server" CssClass="imgGravar"
                                            OnClientClick="if(confirm('Confirma conferência dos apartamentos selecionados?'))
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
                                            Text="   Conferir" />
                                        </td>
                                    </tr>
                                </table>
                            </asp:Panel>
                        </td>
                    </tr>
                </table>
            </asp:Panel>
            <asp:Panel ID="pnlAptoManutencao" runat="server" BackColor="White" DefaultButton="btnConfirmarMan"
                Width="600px" Height="450px" CssClass="ArrendodarBorda">
                <table align="left">
                    <tr>
                        <td align="center" class="FonteMedioApartamento" colspan="3">&nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td align="center" class="FonteMedioApartamento" colspan="3">
                            <asp:TextBox ID="txtDescricaoMotivoOcu" runat="server" CssClass="aplicaTransparencia"
                                Font-Size="Large" Width="100%"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td align="center" class="FonteMedioApartamento" colspan="3" style="display: none">
                            <asp:Button ID="btnAuxilarMan" runat="server" Text="Consulta Bem" />
                            <asp:TextBox ID="txtPegaOrigemMan" runat="server" Width="20px"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td align="left">Setor:
                        </td>
                        <td align="left" colspan="2">
                            <asp:DropDownList ID="drpSetor" runat="server">
                                <asp:ListItem Value="M">Manutenção</asp:ListItem>
                                <asp:ListItem Value="S">Serviços Gerais</asp:ListItem>
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td align="left">
                            <asp:CheckBox ID="chkBemPatrimonial" runat="server" Text="Bem Patrimonial" AutoPostBack="True"
                                Checked="True" />
                        </td>
                        <td align="left" colspan="2">
                            <asp:DropDownList ID="txtBem" runat="server" Width="380px">
                            </asp:DropDownList>
                            <asp:ImageButton ID="btnCadItemConserto" runat="server" Visible="false" ImageUrl="~/images/Lampada.png"
                                ToolTip="Cadastrar novo item de conserto" />
                        </td>
                    </tr>
                    <tr>
                        <td align="left">
                            <asp:Label ID="Label12" runat="server" Text="Assunto:"></asp:Label>
                            &nbsp;
                        </td>
                        <td align="left">
                            <asp:TextBox ID="txtAssuntoManutencao" runat="server" MaxLength="40" ToolTip="Informe o motivo da manutenção."
                                Width="200px"></asp:TextBox>
                        </td>
                        <td align="left">
                            <asp:CheckBox ID="chkBloquearApto" runat="server" Text="Bloquear" />
                            <cc1:ToggleButtonExtender ID="chkBloquearApto_ToggleButtonExtender" runat="server"
                                CheckedImageUrl="~/images/Checked.gif" Enabled="True" ImageHeight="20" ImageWidth="20"
                                TargetControlID="chkBloquearApto" UncheckedImageUrl="~/images/un-checked.gif">
                            </cc1:ToggleButtonExtender>
                        </td>
                    </tr>
                    <tr>
                        <td align="left" valign="top">Descrição:<br />
                            &nbsp;
                        </td>
                        <td align="left" colspan="2">
                            <asp:TextBox ID="txtDescricaoManutencao" runat="server" Width="100%" Height="150px"
                                TextMode="MultiLine"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td align="right" colspan="3">
                            <asp:Button ID="btnConfirmarMan" runat="server" CssClass="imgGravar" OnClientClick="if(confirm('Confirma operação?'))
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
                                Text="   Enviar" />
                            <asp:Button ID="btnCancelarMan" runat="server" CssClass="imgExcluir" Text="   Sair" />
                        </td>
                    </tr>
                    <tr>
                        <td align="left" colspan="3">
                            <asp:Panel ID="pnlCadastroItemConserto" runat="server" BackColor="White" Width="400px"
                                Visible="False">
                                <table style="width: 100%;">
                                    <tr>
                                        <td class="formRuler">CADASTRO DE ITENS PARA CONSERTO
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="left">Descrição
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:TextBox ID="txtDescricaoItemConserto" runat="server" Width="300px"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="right" valign="bottom">
                                            <asp:Button ID="btnConfirmarItemConserto" runat="server" CssClass="imgGravar" Text="   Salvar" />
                                            &nbsp;<asp:Button ID="btnCancelarItemConserto" runat="server" CssClass="imgVoltar"
                                                Text="   Voltar" />
                                            &#39;</td>
                                    </tr>
                                </table>
                            </asp:Panel>
                        </td>
                    </tr>
                </table>
                <br />
                <br />
                <br />
                <br />
            </asp:Panel>
            <br />
            <asp:Panel ID="PnlAtendimento" runat="server" Visible="False" Font-Bold="False" Width="90%" CssClass="ArrendodarBorda">
                <asp:Panel ID="pnlConsultaAtendimento" runat="server" BackColor="White" CssClass="formLabel">
                    &nbsp;<br />
                    <asp:Label ID="lblBloco" runat="server" Text="Bloco"></asp:Label>
                    <asp:DropDownList ID="cmbBlocoAtendimento" runat="server" AutoPostBack="True">
                        <asp:ListItem Value="0">Selecione...</asp:ListItem>
                        <asp:ListItem Value="1">Rio Tocantins</asp:ListItem>
                        <asp:ListItem Value="2">Rio Araguaia</asp:ListItem>
                        <asp:ListItem Value="3">Rio Paranaiba</asp:ListItem>
                        <asp:ListItem Value="4">Rio Vermelho</asp:ListItem>
                    </asp:DropDownList>
                    <asp:Label ID="lblFederacao" runat="server" Text="Federação: "></asp:Label>
                    <asp:DropDownList ID="cmbFederacao" runat="server" AutoPostBack="True">
                        <asp:ListItem Value="T">Todos</asp:ListItem>
                        <asp:ListItem Value="S">Sim</asp:ListItem>
                        <asp:ListItem Value="N">Não</asp:ListItem>
                    </asp:DropDownList>
                    <asp:Label ID="lblAptoInicial" runat="server" Text="Apartamento Inicial:"></asp:Label>
                    <asp:DropDownList ID="cmbAptoIni" runat="server" AutoPostBack="True">
                    </asp:DropDownList>
                    <asp:Label ID="lblAptoFinal" runat="server" Text="Apartamento Final:"></asp:Label>
                    <asp:DropDownList ID="cmbAptoFinal" runat="server">
                    </asp:DropDownList>
                    &nbsp;<asp:Button ID="btnConsultarAtendimento" runat="server" CssClass="imgLupa"
                        Text="     Consultar" Enabled="False" />
                    <asp:Button ID="btnConsultaApto" runat="server" Text="ContultaApto" Visible="False" />
                    <asp:Label ID="lblTotalApartamentos" runat="server"></asp:Label>
                    <br />
                </asp:Panel>
                <asp:Panel ID="pnlGridsOcupadoA" runat="server" Visible="False">
                    <table>
                        <tr>
                            <td valign="top" align="center" class="FonteMedioApartamento" colspan="5">Ocupado
                            </td>
                            <td align="center" class="FonteMedioApartamento" valign="top">&nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td valign="top">
                                <asp:GridView ID="gdv1OcupadoA" runat="server" AutoGenerateColumns="False" DataKeyNames="ApaId,ApaDesc">
                                    <Columns>
                                        <asp:BoundField DataField="ApaDesc" HeaderText="Apartamento">
                                            <HeaderStyle CssClass="formRuler" />
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:BoundField>
                                        <asp:TemplateField HeaderText="Atendimento">
                                            <ItemTemplate>
                                                <asp:ImageButton ID="imgApaId1" runat="server" ImageUrl="~/images/editar.gif" OnClick="imgApaId1_Click" />
                                            </ItemTemplate>
                                            <HeaderStyle CssClass="formRuler" />
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                            </td>
                            <td valign="top">&nbsp;
                            </td>
                            <td valign="top">
                                <asp:GridView ID="gdv2OcupadoA" runat="server" AutoGenerateColumns="False" DataKeyNames="ApaId,ApaDesc">
                                    <Columns>
                                        <asp:BoundField DataField="ApaDesc" HeaderText="Apartamento">
                                            <HeaderStyle CssClass="formRuler" />
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:BoundField>
                                        <asp:TemplateField HeaderText="Atendimento">
                                            <ItemTemplate>
                                                <asp:ImageButton ID="imgApaId2" runat="server" ImageUrl="~/images/editar.gif" OnClick="imgApaId2_Click"
                                                    Style="height: 15px" />
                                            </ItemTemplate>
                                            <HeaderStyle CssClass="formRuler" />
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                            </td>
                            <td valign="top">
                                <asp:GridView ID="gdv3OcupadoA" runat="server" AutoGenerateColumns="False" DataKeyNames="ApaId,ApaDesc">
                                    <Columns>
                                        <asp:BoundField DataField="ApaDesc" HeaderText="Apartamento">
                                            <HeaderStyle CssClass="formRuler" />
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:BoundField>
                                        <asp:TemplateField HeaderText="Atendimento">
                                            <ItemTemplate>
                                                <asp:ImageButton ID="imgApaId3" runat="server" ImageUrl="~/images/editar.gif" OnClick="imgApaId3_Click" />
                                            </ItemTemplate>
                                            <HeaderStyle CssClass="formRuler" />
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                            </td>
                            <td valign="top">
                                <asp:GridView ID="gdv4OcupadoA" runat="server" AutoGenerateColumns="False" DataKeyNames="ApaId,ApaDesc">
                                    <Columns>
                                        <asp:BoundField DataField="ApaDesc" HeaderText="Apartamento">
                                            <HeaderStyle CssClass="formRuler" />
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:BoundField>
                                        <asp:TemplateField HeaderText="Atendimento">
                                            <ItemTemplate>
                                                <asp:ImageButton ID="imgApaId4" runat="server" ImageUrl="~/images/editar.gif" OnClick="imgApaId4_Click" />
                                            </ItemTemplate>
                                            <HeaderStyle CssClass="formRuler" />
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                            </td>
                            <td valign="top">
                                <asp:Button ID="btnVoltarOcupado" runat="server" CssClass="imgVoltar" Text="  Voltar" />
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
            </asp:Panel>
            <div style="display: none">
                <asp:Button ID="btnChamaA" runat="server" Text="  Atendimento" />
                <cc1:ModalPopupExtender ID="btnChamaA_ModalPopupExtender" runat="server" BackgroundCssClass="modalBackground"
                    CancelControlID="btnSairA" DynamicServicePath="" Enabled="True" PopupControlID="pnlCadastraAtendimento"
                    PopupDragHandleControlID="btnSalvarA" TargetControlID="btnChamaA" DropShadow="True">
                </cc1:ModalPopupExtender>
                <asp:Button ID="btnChamaL" runat="server" Text="   Limpo" />
                <cc1:ModalPopupExtender ID="btnChamaL_ModalPopupExtender" runat="server" BackgroundCssClass="modalBackground"
                    CancelControlID="btnSairL" DynamicServicePath="" Enabled="True" PopupControlID="pnlCadastroLimpoA"
                    PopupDragHandleControlID="btnSalvarL" TargetControlID="btnChamaL" DropShadow="True">
                </cc1:ModalPopupExtender>
                <asp:Button ID="btnChamaArrumacao" runat="server" Text="Arrumacao" />
                <cc1:ModalPopupExtender ID="btnChamaArrumacao_ModalPopupExtender" runat="server"
                    BackgroundCssClass="modalBackground" CancelControlID="btnSairAr" DropShadow="True"
                    DynamicServicePath="" Enabled="True" PopupControlID="pnlAlterandoAr" PopupDragHandleControlID="btnSalvarAr"
                    TargetControlID="btnChamaArrumacao">
                </cc1:ModalPopupExtender>
                <asp:Button ID="btnChamaManutencao" runat="server" Text="Manutencao" />
                <cc1:ModalPopupExtender ID="btnChamaManutencao_ModalPopupExtender" runat="server"
                    CancelControlID="btnCancelarMan" DropShadow="True" DynamicServicePath="" Enabled="True"
                    PopupControlID="pnlAptoManutencao" PopupDragHandleControlID="btnConfirmarMan"
                    TargetControlID="btnChamaManutencao" BackgroundCssClass="modalBackground">
                </cc1:ModalPopupExtender>
                <asp:TextBox ID="txtAuxilioManutencao" runat="server"></asp:TextBox>
                <asp:Button ID="btnUnidade" runat="server" Text="UnidadeOp" />
                <asp:Button ID="btnQuemLimpou" runat="server" Text="QuemLimpou" />
                <cc1:ModalPopupExtender ID="btnQuemLimpou_ModalPopupExtender" runat="server"
                    BackgroundCssClass="modalBackground" CancelControlID="imgFecharQLimpou" DropShadow="True"
                    DynamicServicePath="" Enabled="True" PopupControlID="pnlQuemLimpou" PopupDragHandleControlID="imgFecharQLimpou"
                    TargetControlID="btnQuemLimpou">
                </cc1:ModalPopupExtender>
            </div>
            <asp:Panel ID="pnlCadastraAtendimento" runat="server" BackColor="White" Width="650px" CssClass="ArrendodarBorda">
                <table style="width: 100%">
                    <tr>
                        <td class="menuToolBarSelectedItem">Camareira
                        </td>
                        <td>Lençol Solteiro
                        </td>
                        <td>Lençol Berço
                        </td>
                        <td>Cama Casal
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:DropDownList ID="drpCamareiraArrumacao" runat="server">
                            </asp:DropDownList>
                            &nbsp;
                        </td>
                        <td>
                            <asp:TextBox ID="TxtLenSolteiroArrumacao" runat="server" Width="30px"></asp:TextBox>
                            &nbsp;
                        </td>
                        <td>
                            <asp:TextBox ID="txtlenBercoArrumacao" runat="server" Width="30px"></asp:TextBox>
                            &nbsp;
                        </td>
                        <td>
                            <asp:TextBox ID="txtCamaCasalArrumacao" runat="server" Width="30px"></asp:TextBox>
                            &nbsp;
                        </td>
                        <tr>
                            <td>&nbsp;
                            </td>
                            <td>&nbsp;Cama Solteiro
                            </td>
                            <td>&nbsp;Cama Extra
                            </td>
                            <td>&nbsp;Berço
                            </td>
                            <tr>
                                <td align="center" rowspan="2">Apartamento
                                </td>
                                <td>
                                    <asp:TextBox ID="txtCamaSolteiroArrumacao" runat="server" Width="30px"></asp:TextBox>
                                    &nbsp;
                                </td>
                                <td>
                                    <asp:TextBox ID="txtCamaExtraArrumacao" runat="server" Width="30px"></asp:TextBox>
                                    &nbsp;
                                </td>
                                <td>
                                    <asp:TextBox ID="txtBercoArrumacao" runat="server" Width="30px"></asp:TextBox>
                                    &nbsp;
                                </td>
                            </tr>
                            <tr>
                                <td>&nbsp;Fronha Travesseiro
                                </td>
                                <td>&nbsp;Jogo de Toalhas
                                </td>
                                <td>&nbsp;Rolo de Papel
                                </td>
                                <tr>
                                    <td align="center" class="FonteGrandeApartamento" rowspan="5" valign="top">
                                        <asp:Label ID="lblAptoArrumacao" runat="server" Text="Label"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtFronhaArrumacao" runat="server" Width="30px"></asp:TextBox>
                                        &nbsp;
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtToalhaArrumacao" runat="server" Width="30px"></asp:TextBox>
                                        &nbsp;
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtPapelArrumacao" runat="server" Width="30px"></asp:TextBox>
                                        &nbsp;
                                    </td>
                                    <tr>
                                        <td>Saco de Lixo
                                        </td>
                                        <td>Sabonete
                                        </td>
                                        <td>&nbsp;Trocou Tapete
                                        </td>
                                    </tr>
                                </tr>
                            </tr>
                        </tr>
                    </tr>
                    <tr>
                        <td>
                            <asp:TextBox ID="txtLixoArrumacao" runat="server" Width="30px"></asp:TextBox>
                        </td>
                        <td>
                            <asp:TextBox ID="txtSaboneteArrumacao" runat="server" Width="30px"></asp:TextBox>
                        </td>
                        <td>
                            <asp:RadioButtonList ID="rdTapeteArrumacao" runat="server" RepeatColumns="2">
                                <asp:ListItem Value="S">Sim</asp:ListItem>
                                <asp:ListItem Selected="True" Value="N">Não</asp:ListItem>
                            </asp:RadioButtonList>
                        </td>
                    </tr>
                    <tr>
                        <td>Observação
                        </td>
                        <td class="menuToolBarSelectedItem">&nbsp;
                        </td>
                        <td>&nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:TextBox ID="txtObsArrumacao" runat="server" MaxLength="200"></asp:TextBox>
                        </td>
                        <td>
                            <asp:Button ID="brnSalvarA" runat="server" CssClass="imgGravar" OnClientClick="if(confirm('Confirma operação?'))
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
                                Text="  Salvar" />
                        </td>
                        <td>
                            <asp:Button ID="btnSairA" runat="server" CssClass="imgVoltar" Text="   Voltar" />
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
                    </tr>
                </table>
            </asp:Panel>
            <asp:Panel ID="pnlGridsLimpoA" runat="server" Visible="False" Width="90%">
                <table>
                    <tr>
                        <td align="center" class="formRuler" colspan="4">APARTAMENTOS EM ARRUMAÇÃO QUE IRÃO PARA LIMPO
                        </td>
                        <td valign="top">&nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td valign="top" rowspan="2">
                            <asp:GridView ID="gdv1LimpoA" runat="server" AutoGenerateColumns="False" DataKeyNames="ApaId,ApaDesc,AgoId">
                                <Columns>
                                    <asp:TemplateField HeaderText="Limpar">
                                        <ItemTemplate>
                                            <asp:CheckBox ID="chkLimpar1" runat="server" />
                                        </ItemTemplate>
                                        <HeaderStyle CssClass="formRuler" />
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="ApaDesc" HeaderText="Apartamento">
                                        <HeaderStyle CssClass="formRuler" />
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:BoundField>
                                    <asp:TemplateField HeaderText="Atendimento">
                                        <ItemTemplate>
                                            <asp:ImageButton ID="img1LimpoA" runat="server" ImageUrl="~/images/editar.gif" OnClick="img1LimpoA_Click" />
                                        </ItemTemplate>
                                        <HeaderStyle CssClass="formRuler" />
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                        </td>
                        <td valign="top" rowspan="2">
                            <asp:GridView ID="gdv2LimpoA" runat="server" AutoGenerateColumns="False" DataKeyNames="ApaId,ApaDesc,AgoId">
                                <Columns>
                                    <asp:TemplateField HeaderText="Limpar">
                                        <ItemTemplate>
                                            <asp:CheckBox ID="chkLimpar2" runat="server" />
                                        </ItemTemplate>
                                        <HeaderStyle CssClass="formRuler" />
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="ApaDesc" HeaderText="Apartamento">
                                        <HeaderStyle CssClass="formRuler" />
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:BoundField>
                                    <asp:TemplateField HeaderText="Atendimento">
                                        <ItemTemplate>
                                            <asp:ImageButton ID="img2LimpoA" runat="server" ImageUrl="~/images/editar.gif" OnClick="img2LimpoA_Click" />
                                        </ItemTemplate>
                                        <HeaderStyle CssClass="formRuler" />
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                        </td>
                        <td valign="top" rowspan="2">
                            <asp:GridView ID="gdv3LimpoA" runat="server" AutoGenerateColumns="False" DataKeyNames="ApaId,ApaDesc,AgoId">
                                <Columns>
                                    <asp:TemplateField HeaderText="Limpar">
                                        <ItemTemplate>
                                            <asp:CheckBox ID="chkLimpar3" runat="server" />
                                        </ItemTemplate>
                                        <HeaderStyle CssClass="formRuler" />
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="ApaDesc" HeaderText="Apartamento">
                                        <HeaderStyle CssClass="formRuler" />
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:BoundField>
                                    <asp:TemplateField HeaderText="Atendimento">
                                        <ItemTemplate>
                                            <asp:ImageButton ID="img3LimpoA" runat="server" ImageUrl="~/images/editar.gif" OnClick="img3LimpoA_Click" />
                                        </ItemTemplate>
                                        <HeaderStyle CssClass="formRuler" />
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                        </td>
                        <td valign="top" rowspan="2">
                            <asp:GridView ID="gdv4LimpoA" runat="server" AutoGenerateColumns="False" DataKeyNames="ApaId,ApaDesc,AgoId">
                                <Columns>
                                    <asp:TemplateField HeaderText="Limpar">
                                        <ItemTemplate>
                                            <asp:CheckBox ID="chkLimpar4" runat="server" />
                                        </ItemTemplate>
                                        <HeaderStyle CssClass="formRuler" />
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="ApaDesc" HeaderText="Apartamento">
                                        <HeaderStyle CssClass="formRuler" />
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:BoundField>
                                    <asp:TemplateField HeaderText="Atendimento">
                                        <ItemTemplate>
                                            <asp:ImageButton ID="img4LimpoA" runat="server" ImageUrl="~/images/editar.gif" OnClick="img4LimpoA_Click" />
                                        </ItemTemplate>
                                        <HeaderStyle CssClass="formRuler" />
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                        </td>
                        <td valign="top">
                            <asp:Button ID="btnVoltaLimpoA" runat="server" CssClass="imgVoltar" Text="   Voltar"
                                Visible="False" />
                        </td>
                    </tr>
                    <tr>
                        <td valign="bottom">
                            <asp:Button ID="btnSalvarLimpo" runat="server" CssClass="imgGravar" OnClientClick="if(confirm('Confirma operação?'))
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
                                Text="   Salvar" />
                        </td>
                    </tr>
                </table>
            </asp:Panel>
            <asp:Panel ID="pnlCadastroLimpoA" runat="server" BackColor="White" Width="650px" CssClass="ArrendodarBorda">
                <table style="width: 100%">
                    <tr>
                        <td>Camareira
                        </td>
                        <td>Cama Casal
                        </td>
                        <td>&nbsp;Cama Solteiro
                        </td>
                        <td>&nbsp;Berço
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:DropDownList ID="drpCamareiraL" runat="server">
                            </asp:DropDownList>
                            &nbsp;
                        </td>
                        <td>&nbsp;
                            <asp:TextBox ID="txtCamaCasalL" runat="server" Style="margin-right: 0px" Width="30px"></asp:TextBox>
                        </td>
                        <td>&nbsp;
                            <asp:TextBox ID="txtCamaSolteiroL" runat="server" Width="30px"></asp:TextBox>
                        </td>
                        <td>&nbsp;
                            <asp:TextBox ID="txtBercoL" runat="server" Width="30px"></asp:TextBox>
                        </td>
                        <tr>
                            <td class="style1">&nbsp;
                            </td>
                            <td>&nbsp;&nbsp;Fronha Travesseiro
                            </td>
                            <td>&nbsp;&nbsp;Jogo de Toalhas
                            </td>
                            <td>&nbsp;&nbsp;Rolo de Papel
                            </td>
                            <tr>
                                <td align="center" rowspan="2">Apartamento
                                </td>
                                <td>&nbsp;
                                    <asp:TextBox ID="txtFronhaL" runat="server" Width="30px"></asp:TextBox>
                                </td>
                                <td>&nbsp;
                                    <asp:TextBox ID="txtToalhaL" runat="server" Width="30px"></asp:TextBox>
                                </td>
                                <td>&nbsp;
                                    <asp:TextBox ID="txtPapelL" runat="server" Width="30px"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>Saco de Lixo
                                </td>
                                <td>Sabonete
                                </td>
                                <td>&nbsp;Cama Extra
                                </td>
                                <tr>
                                    <td align="center" class="FonteGrandeApartamento" rowspan="5" valign="top">
                                        <asp:Label ID="lblAptoL" runat="server" Text="Label"></asp:Label>
                                        <br />
                                    </td>
                                    <td>&nbsp;
                                        <asp:TextBox ID="txtLixoL" runat="server" Width="30px"></asp:TextBox>
                                    </td>
                                    <td>&nbsp;
                                        <asp:TextBox ID="txtSaboneteL" runat="server" Width="30px"></asp:TextBox>
                                    </td>
                                    <td>&nbsp;
                                        <asp:TextBox ID="txtCamaExtraL" runat="server" Width="30px"></asp:TextBox>
                                    </td>
                                    <tr>
                                        <td>&nbsp;Trocou Tapete
                                        </td>
                                        <td>Observação
                                        </td>
                                        <td class="menuToolBarSelectedItem">&nbsp;
                                        </td>
                                    </tr>
                                </tr>
                            </tr>
                        </tr>
                    </tr>
                    <tr>
                        <td>
                            <asp:RadioButtonList ID="rdTapeteL" runat="server" RepeatColumns="2">
                                <asp:ListItem Value="S">Sim</asp:ListItem>
                                <asp:ListItem Selected="True" Value="N">Não</asp:ListItem>
                            </asp:RadioButtonList>
                        </td>
                        <td>
                            <asp:TextBox ID="txtObsL" runat="server" MaxLength="200"></asp:TextBox>
                        </td>
                        <td>&nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td class="menuToolBarSelectedItem">&nbsp;
                        </td>
                        <td class="menuToolBarSelectedItem">
                            <asp:Button ID="brnSalvarL" runat="server" CssClass="imgGravar" OnClientClick="if(confirm('Confirma operação?'))
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
                                Text="   Salvar" />
                        </td>
                        <td>
                            <asp:Button ID="btnSairL" runat="server" CssClass="imgVoltar" Text="   Voltar" />
                        </td>
                    </tr>
                    <tr>
                        <td>&nbsp;
                        </td>
                        <td>&nbsp;
                        </td>
                        <td>&nbsp;
                        </td>
                    </tr>
                </table>
            </asp:Panel>
            <asp:Panel ID="pnlGridArrumacao" runat="server" ScrollBars="Auto" Visible="False"
                Width="90%" CssClass="ArrendodarBorda">
                <asp:Label ID="lblTituloHistAtendimento" runat="server" CssClass="formRuler" Text="HISTÓRICO DOS ATENDIMENTOS:"
                    Font-Bold="True"></asp:Label>
                <br />
                <asp:Panel ID="pnlTituloArrumacao" runat="server">
                    <table>
                        <tr>
                            <td>Data Inicial:
                            </td>
                            <td>Data Final:
                            </td>
                            <td>Apartamento:
                            </td>
                            <td>Camareira:
                            </td>
                            <td>&nbsp;
                            </td>
                            <td>&nbsp;
                            </td>
                            <td>&nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:TextBox ID="txtData1Arrumacao" runat="server" Width="80px"></asp:TextBox>
                                <cc1:CalendarExtender ID="calData1Arrumacao_CalendarExtender" runat="server" Enabled="True"
                                    FirstDayOfWeek="Sunday" Format="dd/MM/yyyy" TargetControlID="txtData1Arrumacao">
                                </cc1:CalendarExtender>
                            </td>
                            <td>
                                <asp:TextBox ID="txtData2Arrumacao" runat="server" Width="80px"></asp:TextBox>
                                <cc1:CalendarExtender ID="calData2Arrumacao_CalendarExtender" runat="server" Enabled="True"
                                    FirstDayOfWeek="Sunday" Format="dd/MM/yyyy" TargetControlID="txtData2Arrumacao">
                                </cc1:CalendarExtender>
                            </td>
                            <td>
                                <asp:DropDownList ID="drpAptoArrumacao" runat="server">
                                </asp:DropDownList>
                            </td>
                            <td>
                                <asp:DropDownList ID="drpCamArrumacao" runat="server">
                                </asp:DropDownList>
                            </td>
                            <td>
                                <asp:Button ID="btnConfirmarArrumacao" runat="server" CssClass="imgLupa" Text="  Consultar" />
                            </td>
                            <td>
                                <asp:Button ID="btnVoltarArrumacao" runat="server" CssClass="imgVoltar" Text="  Voltar" />
                            </td>
                            <td>&nbsp;
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
                    </table>
                </asp:Panel>
                <asp:Panel ID="pnlGridsArrumacaoA" runat="server" Visible="False">
                    <table>
                        <tr>
                            <td align="center" class="formRuler" colspan="5" valign="top">ARRUMAÇÃO
                            </td>
                            <td align="center" class="formRuler" valign="top">&nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td valign="top">
                                <asp:GridView ID="gdv1AtendimentoA" runat="server" AutoGenerateColumns="False" DataKeyNames="ApaId,ApaDesc,AgoId">
                                    <Columns>
                                        <asp:BoundField DataField="ApaDesc" HeaderText="Apartamento">
                                            <HeaderStyle CssClass="formRuler" />
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="CheckOut" HeaderText="Data">
                                            <HeaderStyle CssClass="formRuler" />
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="Hora" HeaderText="Hora">
                                            <HeaderStyle CssClass="formRuler" />
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:BoundField>
                                        <asp:TemplateField HeaderText="Atendimento">
                                            <ItemTemplate>
                                                <asp:ImageButton ID="img1AtendimentoA" runat="server" ImageUrl="~/images/editar.gif"
                                                    OnClick="img1AtendimentoA_Click" Style="height: 15px" />
                                            </ItemTemplate>
                                            <HeaderStyle CssClass="formRuler" />
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                            </td>
                            <td valign="top">
                                <asp:GridView ID="gdv2AtendimentoA" runat="server" AutoGenerateColumns="False" DataKeyNames="ApaId,ApaDesc,AgoId">
                                    <Columns>
                                        <asp:BoundField DataField="ApaDesc" HeaderText="Apartamento">
                                            <HeaderStyle CssClass="formRuler" />
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="CheckOut" HeaderText="Data">
                                            <HeaderStyle CssClass="formRuler" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="Hora" HeaderText="Hora">
                                            <HeaderStyle CssClass="formRuler" />
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:BoundField>
                                        <asp:TemplateField HeaderText="Atendimento">
                                            <ItemTemplate>
                                                <asp:ImageButton ID="img2AtendimentoA" runat="server" ImageUrl="~/images/editar.gif"
                                                    OnClick="img2AtendimentoA_Click" />
                                            </ItemTemplate>
                                            <HeaderStyle CssClass="formRuler" />
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                            </td>
                            <td valign="top">&nbsp;</td>
                            <td valign="top">
                                <asp:GridView ID="gdv3AtendimentoA" runat="server" AutoGenerateColumns="False" DataKeyNames="ApaId,ApaDesc,AgoId">
                                    <Columns>
                                        <asp:BoundField DataField="ApaDesc" HeaderText="Apartamento">
                                            <HeaderStyle CssClass="formRuler" />
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="CheckOut" HeaderText="Data">
                                            <HeaderStyle CssClass="formRuler" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="Hora" HeaderText="Hora">
                                            <HeaderStyle CssClass="formRuler" />
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:BoundField>
                                        <asp:TemplateField HeaderText="Atendimento">
                                            <ItemTemplate>
                                                <asp:ImageButton ID="img3AtendimentoA" runat="server" ImageUrl="~/images/editar.gif"
                                                    OnClick="img3AtendimentoA_Click" />
                                            </ItemTemplate>
                                            <HeaderStyle CssClass="formRuler" />
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                            </td>
                            <td valign="top">
                                <asp:GridView ID="gdv4AtendimentoA" runat="server" AutoGenerateColumns="False" DataKeyNames="ApaId,ApaDesc,AgoId">
                                    <Columns>
                                        <asp:BoundField DataField="ApaDesc" HeaderText="Apartamento">
                                            <HeaderStyle CssClass="formRuler" />
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="CheckOut" HeaderText="Data">
                                            <HeaderStyle CssClass="formRuler" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="Hora" HeaderText="Hora">
                                            <HeaderStyle CssClass="formRuler" HorizontalAlign="Center" />
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:BoundField>
                                        <asp:TemplateField HeaderText="Atendimento">
                                            <ItemTemplate>
                                                <asp:ImageButton ID="img4AtendimentoA" runat="server" ImageUrl="~/images/editar.gif" OnClick="img4AtendimentoA_Click" />
                                            </ItemTemplate>
                                            <HeaderStyle CssClass="formRuler" />
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                            </td>
                            <td valign="top">&nbsp;</td>
                            <td valign="top">&nbsp;
                            </td>
                            <td valign="top">&nbsp;
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
                <div align="center">
                    <asp:Label ID="lblMensagemArrumacao" runat="server" CssClass="formLabelTitulo"></asp:Label>
                </div>
            </asp:Panel>
            <asp:Panel ID="pnlAlterandoAr" runat="server" BackColor="White" Visible="True" Width="650px" CssClass="ArrendodarBorda">
                <table style="width: 100%">
                    <tr>
                        <td>Camareira
                        </td>
                        <td>Cama Casal
                        </td>
                        <td>&nbsp;Cama Solteiro
                        </td>
                        <td>&nbsp;Berço
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:DropDownList ID="drpCamareiraAr" runat="server">
                            </asp:DropDownList>
                            &nbsp;
                        </td>
                        <td>&nbsp;
                            <asp:TextBox ID="txtCamaCasalAr" runat="server" Style="margin-right: 0px" Width="30px"></asp:TextBox>
                        </td>
                        <td>&nbsp;
                            <asp:TextBox ID="txtCamaSolteiroAr" runat="server" Width="30px"></asp:TextBox>
                        </td>
                        <td>&nbsp;
                            <asp:TextBox ID="txtBercoAr" runat="server" Width="30px"></asp:TextBox>
                        </td>
                        <tr>
                            <td class="style1">&nbsp;
                            </td>
                            <td>&nbsp;&nbsp;Fronha Travesseiro
                            </td>
                            <td>&nbsp;&nbsp;Jogo de Toalhas
                            </td>
                            <td>&nbsp;&nbsp;Rolo de Papel
                            </td>
                            <tr>
                                <td align="center" rowspan="2">Apartamento
                                </td>
                                <td>&nbsp;
                                    <asp:TextBox ID="txtFronhaAr" runat="server" Width="30px"></asp:TextBox>
                                </td>
                                <td>&nbsp;
                                    <asp:TextBox ID="txtToalhaAr" runat="server" Width="30px"></asp:TextBox>
                                </td>
                                <td>&nbsp;
                                    <asp:TextBox ID="txtPapelAr" runat="server" Width="30px"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>Saco de Lixo
                                </td>
                                <td>Sabonete
                                </td>
                                <td>&nbsp;Trocou Tapete
                                </td>
                                <tr>
                                    <td align="center" class="FonteGrandeApartamento" rowspan="5" valign="top">
                                        <asp:Label ID="lblAptoAr" runat="server" Text="Label"></asp:Label>
                                    </td>
                                    <td>&nbsp;
                                        <asp:TextBox ID="txtLixoAr" runat="server" Width="30px"></asp:TextBox>
                                    </td>
                                    <td>&nbsp;
                                        <asp:TextBox ID="txtSaboneteAr" runat="server" Width="30px"></asp:TextBox>
                                    </td>
                                    <td>&nbsp;
                                        <asp:RadioButtonList ID="rdTapeteAr" runat="server" RepeatColumns="2">
                                            <asp:ListItem Value="S">Sim</asp:ListItem>
                                            <asp:ListItem Selected="True" Value="N">Não</asp:ListItem>
                                        </asp:RadioButtonList>
                                    </td>
                                    <tr>
                                        <td>Observação
                                        </td>
                                        <td class="menuToolBarSelectedItem">&nbsp;
                                        </td>
                                        <td class="menuToolBarSelectedItem">&nbsp;
                                        </td>
                                    </tr>
                                </tr>
                            </tr>
                        </tr>
                    </tr>
                    <tr>
                        <td>
                            <asp:TextBox ID="txtObsAr" runat="server" MaxLength="200"></asp:TextBox>
                        </td>
                        <td>
                            <asp:Button ID="brnSalvarAr" runat="server" CssClass="imgGravar" OnClientClick="if(confirm('Confirma operação?'))
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
                                Text="   Salvar" />
                        </td>
                        <td>
                            <asp:Button ID="btnSairAr" runat="server" CssClass="imgVoltar" Text="   Voltar" />
                        </td>
                    </tr>
                    <tr>
                        <td class="menuToolBarSelectedItem">&nbsp;
                        </td>
                        <td class="menuToolBarSelectedItem">&nbsp;
                        </td>
                        <td>&nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td>&nbsp;
                        </td>
                        <td>&nbsp;
                        </td>
                        <td>&nbsp;
                        </td>
                    </tr>
                </table>
            </asp:Panel>
            <asp:Label ID="lblAuxCamposTemp" runat="server"></asp:Label>
            <asp:HiddenField ID="hddBalaoOcupado" runat="server" />
            <asp:HiddenField ID="hddBalaoArrumacao" runat="server" />
            <asp:HiddenField ID="hddBalaoLimpo" runat="server" />
            <cc1:BalloonPopupExtender ID="hddBalaoLimpo_BalloonPopupExtender" runat="server"
                BalloonPopupControlID="pnlPopLimpo" BalloonSize="Medium" CustomCssUrl="" DisplayOnFocus="True"
                DisplayOnMouseOver="True" DynamicServicePath="" Enabled="True" ExtenderControlID=""
                ScrollBars="None" TargetControlID="hddBalaoLimpo">
            </cc1:BalloonPopupExtender>
            <cc1:BalloonPopupExtender ID="hddBalaoArrumacao_BalloonPopupExtender" runat="server"
                BalloonPopupControlID="pnlPopArrumacao" BalloonSize="Medium" CustomCssUrl=""
                DisplayOnFocus="True" DisplayOnMouseOver="True" DynamicServicePath="" Enabled="True"
                ExtenderControlID="" ScrollBars="None" TargetControlID="hddBalaoArrumacao">
            </cc1:BalloonPopupExtender>
            <cc1:BalloonPopupExtender ID="hddBalaoOcupado_BalloonPopupExtender" runat="server"
                BalloonPopupControlID="pnlPopOcupado" BalloonSize="Medium" CustomCssUrl="" DisplayOnFocus="True"
                DisplayOnMouseOver="True" DynamicServicePath="" Enabled="True" ExtenderControlID=""
                ScrollBars="None" TargetControlID="hddBalaoOcupado">
            </cc1:BalloonPopupExtender>
            <asp:HiddenField ID="hddBalaoManutencao" runat="server" />
            <asp:HiddenField ID="hddBalaoPrioridade" runat="server" />
            <cc1:BalloonPopupExtender ID="hddBaloonPrioridade" runat="server" BalloonPopupControlID="pnlPopPrioridade"
                BalloonSize="Medium" CustomCssUrl="" DisplayOnFocus="True" DisplayOnMouseOver="True"
                DynamicServicePath="" Enabled="True" ExtenderControlID="" TargetControlID="hddBalaoPrioridade">
            </cc1:BalloonPopupExtender>
            <cc1:BalloonPopupExtender ID="hddBalaoManutencao_BalloonPopupExtender" runat="server"
                BalloonPopupControlID="pnlPopManutencao" BalloonSize="Medium" CustomCssUrl=""
                DisplayOnFocus="True" DisplayOnMouseOver="True" DynamicServicePath="" Enabled="True"
                ExtenderControlID="" ScrollBars="None" TargetControlID="hddBalaoManutencao">
            </cc1:BalloonPopupExtender>
            <asp:Panel ID="pnlIntegrantes" runat="server" Visible="False" Width="90%" CssClass="ArrendodarBorda">
                <table>
                    <tr>
                        <td align="left" colspan="2">
                            <asp:Label ID="lblTituloEmprestimo" runat="server" CssClass="formRuler" Text="APARTAMENTO:"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:GridView ID="gdvIntegrantes" runat="server" AutoGenerateColumns="False" DataKeyNames="ApaId,Nome,CheckOut,Emprestimos,IntId,ValorEmprestimos">
                                <Columns>
                                    <asp:BoundField DataField="Nome" HeaderText="Hóspedes">
                                        <HeaderStyle CssClass="formRuler" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="CheckOut" HeaderText="Check Out">
                                        <HeaderStyle CssClass="formRuler" />
                                    </asp:BoundField>
                                    <asp:TemplateField HeaderText="Empréstimo">
                                        <ItemTemplate>
                                            <asp:ImageButton ID="imgEmprestimos" runat="server" ImageUrl="~/images/Emprestimo.gif"
                                                OnClick="imgEmprestimos_Click" Style="height: 17px" />
                                        </ItemTemplate>
                                        <HeaderStyle CssClass="formRuler" />
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Inserir">
                                        <ItemTemplate>
                                            <asp:ImageButton ID="imgInserir" runat="server" ImageUrl="~/images/InserirVerde.png"
                                                OnClick="imgInserir_Click" ToolTip="Inserir empréstimos" />
                                        </ItemTemplate>
                                        <HeaderStyle CssClass="formRuler" />
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="ValorEmprestimos" HeaderText="Valor">
                                        <HeaderStyle CssClass="formRuler" />
                                        <ItemStyle HorizontalAlign="Right" />
                                    </asp:BoundField>
                                </Columns>
                            </asp:GridView>
                        </td>
                        <td valign="top">
                            <asp:Button ID="btnFecharIntegrante" runat="server" CssClass="imgVoltar" Text="   Voltar" />
                            <br />
                        </td>
                    </tr>
                </table>
                <asp:HiddenField ID="hddCSeId" runat="server" />
                <asp:HiddenField ID="hddIntId" runat="server" />
                <asp:Panel ID="pnlEmprestimosIntegrante" runat="server" Visible="False">
                    <asp:Panel ID="pnlDadosIntegrante" runat="server">
                        <br />
                        <asp:Label ID="lblDadosEmprestimo" runat="server" CssClass="formRuler" Font-Bold="False"
                            Font-Italic="False"></asp:Label>
                        <br />
                    </asp:Panel>
                    <asp:GridView ID="gdvEmprestimosIntegrante" runat="server" AutoGenerateColumns="False"
                        DataKeyNames="IntId,EmpData,EmpDescricao,EmpValor,EmpQuantidade,EmpOperacao,EmpSituacao,EmpUsuario,EmpDataUsuario,CseId">
                        <Columns>
                            <asp:TemplateField HeaderText="Data" ShowHeader="False">
                                <ItemTemplate>
                                    <asp:LinkButton ID="lnkData" runat="server" CausesValidation="false" CommandName="Select"
                                        OnClick="lnkData_Click" Text='<%# Eval("EmpData") %>'></asp:LinkButton>
                                    <asp:ImageButton ID="imgUsuario" runat="server" ImageUrl="~/images/Usuario.png" />
                                </ItemTemplate>
                                <HeaderStyle CssClass="formRuler" />
                            </asp:TemplateField>
                            <asp:BoundField DataField="EmpDescricao" HeaderText="Consumo/Empréstimo">
                                <HeaderStyle CssClass="formRuler" />
                            </asp:BoundField>
                            <asp:BoundField DataField="EmpValor" HeaderText="Valor">
                                <HeaderStyle CssClass="formRuler" />
                                <ItemStyle HorizontalAlign="Right" />
                            </asp:BoundField>
                            <asp:BoundField DataField="EmpQuantidade" HeaderText="Quantidade">
                                <HeaderStyle CssClass="formRuler" />
                                <ItemStyle HorizontalAlign="Right" />
                            </asp:BoundField>
                            <asp:BoundField DataField="EmpOperacao" HeaderText="Operação">
                                <HeaderStyle CssClass="formRuler" />
                            </asp:BoundField>
                            <asp:BoundField DataField="EmpSituacao" HeaderText="Situação">
                                <HeaderStyle CssClass="formRuler" />
                            </asp:BoundField>
                            <asp:TemplateField HeaderText="Apagar">
                                <ItemTemplate>
                                    <asp:ImageButton ID="imgApagarEmp" runat="server" ImageUrl="~/images/Delete.gif"
                                        OnClick="imgApagarEmp_Click" Style="height: 16px" OnClientClick="if(confirm('Confirma operação?'))
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
                                };" />
                                </ItemTemplate>
                                <HeaderStyle CssClass="formRuler" />
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                    <asp:Panel ID="pnlCadastroEmprestimo" runat="server" DefaultButton="btnGravar" Width="95%">
                        <table>
                            <tr>
                                <td>Data
                                </td>
                                <td>&nbsp;
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:TextBox ID="txtEmpData" runat="server" Width="80px"></asp:TextBox>
                                    <cc1:CalendarExtender ID="txtEmpData_CalendarExtender" runat="server" Enabled="True"
                                        FirstDayOfWeek="Sunday" Format="dd/MM/yyyy" TargetControlID="txtEmpData">
                                    </cc1:CalendarExtender>
                                </td>
                                <td>&nbsp;
                                </td>
                            </tr>
                            <tr>
                                <td>Item Consumido/Item Empréstimo
                                </td>
                                <td>&nbsp;
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                    <asp:TextBox ID="txtEmpDescricao" runat="server" MaxLength="40" Width="300px"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>Valor Unitário
                                </td>
                                <td>Quantidade
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:TextBox ID="txtEmpValor" runat="server" CssClass="AlinhaTextoDireita" Width="80px">0</asp:TextBox>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtQuantidade" runat="server" CssClass="AlinhaTextoDireita" Width="80px">0</asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>Operação
                                </td>
                                <td>&nbsp;
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                    <asp:DropDownList ID="drpOperacoes" runat="server" Width="250px">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td>Tipo
                                </td>
                                <td>&nbsp;
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:RadioButtonList ID="rbtTipo" runat="server">
                                        <asp:ListItem Value="Convenio">Consumo de Serviço</asp:ListItem>
                                        <asp:ListItem Selected="True" Value="Emprestimo">Empréstimo</asp:ListItem>
                                    </asp:RadioButtonList>
                                </td>
                                <td>&nbsp;
                                </td>
                            </tr>
                            <tr>
                                <td align="right">
                                    <asp:Button ID="btnGravar" runat="server" CssClass="imgGravar" OnClientClick="if(confirm('Confirma operação?'))
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
                                        Text="   Salvar" />
                                </td>
                                <td>
                                    <asp:Button ID="btnCancelar" runat="server" CssClass="imgVoltar" Text="   Voltar" />
                                </td>
                            </tr>
                            <tr>
                                <td align="center" colspan="2">&nbsp;
                                </td>
                            </tr>
                        </table>
                    </asp:Panel>
                </asp:Panel>
            </asp:Panel>
            <asp:HiddenField ID="hddApaId" runat="server" />
            <asp:HiddenField ID="hddApaDesc" runat="server" />
            <asp:HiddenField ID="hddAgoId" runat="server" />
            <asp:HiddenField ID="hddApaCCusto" runat="server" />
            <asp:HiddenField ID="hddApaArea" runat="server" />
            <asp:HiddenField ID="hddResId" runat="server" />
            <br />
            <asp:Panel ID="pnlHistoricoManutencao" runat="server" DefaultButton="btnConsultaHistoricoMan"
                Visible="False" Width="90%" CssClass="ArrendodarBorda">
                <asp:Label ID="lblHistoricodasManutencoes" runat="server" CssClass="formRuler" Text="HISTÓRICO DAS MANUTENÇÕES"></asp:Label>
                <br />
                <asp:Panel ID="pnlPesquisaHistoricoManutencao" runat="server" DefaultButton="btnConsultaHistoricoMan">
                    <asp:Label ID="lblHistoricoData1" runat="server" Text="Data Inicial"></asp:Label>
                    <asp:TextBox ID="txtHistoricoManutencaoD1" runat="server" Width="80px"></asp:TextBox>
                    <cc1:CalendarExtender ID="calHistoricoManD1" runat="server" Enabled="True" FirstDayOfWeek="Sunday"
                        Format="dd/MM/yyyy" TargetControlID="txtHistoricoManutencaoD1">
                    </cc1:CalendarExtender>
                    <asp:Label ID="lblHistoricoData2" runat="server" Text="Data Final"></asp:Label>
                    <asp:TextBox ID="txtHistoricoManutencaoD2" runat="server" Width="80px"></asp:TextBox>
                    <cc1:CalendarExtender ID="calHistoricoManD2" runat="server" Enabled="True" FirstDayOfWeek="Sunday"
                        Format="dd/MM/yyyy" TargetControlID="txtHistoricoManutencaoD2">
                    </cc1:CalendarExtender>
                    <asp:Label ID="lblHistoricoApartamento" runat="server" Text="Apartamento"></asp:Label>
                    <asp:DropDownList ID="cmbHistoricoApartamentos" runat="server">
                    </asp:DropDownList>
                    <asp:Button ID="btnConsultaHistoricoMan" runat="server" CssClass="imgLupa" Text="   Consultar" />
                    <asp:Button ID="btnVoltarHistoricoMan" runat="server" CssClass="imgVoltar" Text="   Voltar" />
                    <br />
                    <br />
                </asp:Panel>
                <asp:GridView ID="gdvHistoricoManutencao" runat="server" AutoGenerateColumns="False">
                    <Columns>
                        <asp:BoundField DataField="ApaDesc" HeaderText="Apto">
                            <HeaderStyle CssClass="formRuler" />
                        </asp:BoundField>
                        <asp:BoundField DataField="ManAbertura" HeaderText="Abertura">
                            <HeaderStyle CssClass="formRuler" />
                        </asp:BoundField>
                        <asp:BoundField DataField="ManHorasAbertura" HeaderText="H">
                            <HeaderStyle CssClass="formRuler" />
                        </asp:BoundField>
                        <asp:BoundField DataField="ManConclusao" HeaderText="Conclusão">
                            <HeaderStyle CssClass="formRuler" />
                        </asp:BoundField>
                        <asp:BoundField DataField="ManHorasConclusao" HeaderText="H">
                            <HeaderStyle CssClass="formRuler" />
                        </asp:BoundField>
                        <asp:BoundField DataField="TempoDiaConserto" HeaderText="Tempo do Conserto">
                            <HeaderStyle CssClass="formRuler" />
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:BoundField>
                        <asp:BoundField DataField="Usuario" HeaderText="Requisitante">
                            <HeaderStyle CssClass="formRuler" />
                        </asp:BoundField>
                        <asp:BoundField DataField="ManDescricaoManut" HeaderText="Resquisitado">
                            <HeaderStyle CssClass="formRuler" />
                        </asp:BoundField>
                        <asp:BoundField DataField="ManRealizado" HeaderText="Realizado">
                            <HeaderStyle CssClass="formRuler" />
                        </asp:BoundField>
                    </Columns>
                </asp:GridView>
                <div align="center">
                    <asp:Label ID="lblMsnHistManutencao" runat="server" CssClass="formLabelTitulo"></asp:Label>
                </div>
            </asp:Panel>
            <asp:Panel ID="pnlQuemLimpou" runat="server" BackColor="White" CssClass="ArrendodarBorda">
                <table>
                    <tr>
                        <td class="formRuler" align="center">Últimas liberações como limpo<br />
                            <asp:Label ID="lblAptoLiberado" runat="server" Text="Label"></asp:Label>
                        </td>
                        <td align="left" valign="top" rowspan="2">
                            <asp:ImageButton ID="imgFecharQLimpou" runat="server"
                                ImageUrl="~/images/Fechar.png" />
                        </td>
                    </tr>
                    <tr>
                        <td align="center">
                            <asp:GridView ID="gdvQuemLimpouA" runat="server" AutoGenerateColumns="False"
                                CellPadding="4" ForeColor="#333333" GridLines="None">
                                <RowStyle BackColor="#EFF3FB" />
                                <Columns>
                                    <asp:BoundField DataField="UsuarioLimpoLog" HeaderText="Servidor(a)">
                                        <HeaderStyle CssClass="formRuler" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="DataLimpoLog" HeaderText="Data e Hora">
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
                        </td>
                    </tr>
                </table>
            </asp:Panel>

            <div runat="server" id="divModalEnxoval" visible="false">
                <asp:Panel ID="pnlCompletarEnxovais" CssClass="ArrendodarBordaNegra" runat="server" Width="20%">
                    <div style="position: relative; font-size: 15px; text-align: center; font-weight: bold; left: 10px; top: 10px; width: 95%;">
                        Completar enxoval
                        <asp:Label ID="lblApaDescEnxoval" runat="server" Text=""></asp:Label>
                    </div>
                    <div style="position: relative; border-bottom: solid; border-bottom-color: Window; border-bottom-width: 1px; border-top: solid; border-top-color: Window; border-top-width: 1px; left: 10px; top: 20px; width: 95%;">
                        <br />
                        <asp:Label ID="lblTituloSolicitado" Font-Bold="true" runat="server" Text="Solicitado:"></asp:Label><br />
                        <p style="overflow-wrap: normal;">
                            Cama Extra:<asp:RadioButtonList ID="rdSolCamaExtra" Enabled="false" runat="server" RepeatDirection="Horizontal">
                                <asp:ListItem Value="1"></asp:ListItem>
                                <asp:ListItem Value="2"></asp:ListItem>
                                <asp:ListItem Value="3"></asp:ListItem>
                                <asp:ListItem Value="4"></asp:ListItem>
                            </asp:RadioButtonList>

                            <asp:CheckBox ID="chkSolBerco" Text="Berço" Enabled="false" TextAlign="Left" runat="server" />
                            <asp:CheckBox ID="chksolBanheira" Text="Banheira" Enabled="false" TextAlign="Left" runat="server" />
                    </div>
                    <br />
                    <div style="position: relative; left: 10px;">
                        <br />
                        <asp:Label Font-Bold="true" ID="Label1" runat="server" Text="Atendimento:"></asp:Label><br />
                        <p style="overflow-wrap: normal;">
                            Cama Extra:<asp:RadioButtonList ID="rdAteCamaExtra" runat="server" RepeatDirection="Horizontal">
                                <asp:ListItem Value="1"></asp:ListItem>
                                <asp:ListItem Value="2"></asp:ListItem>
                                <asp:ListItem Value="3"></asp:ListItem>
                                <asp:ListItem Value="4"></asp:ListItem>
                            </asp:RadioButtonList>

                            <asp:CheckBox ID="chkAteBerco" Text="Berço" runat="server" TextAlign="Left" />
                            <asp:CheckBox ID="chkAteBanheira" Text="Banheira" runat="server" TextAlign="Left" />
                            <br />
                            <br />
                            Motivo:
                        <asp:TextBox ID="txtAteMotivoNaoCompletar" MaxLength="50" Width="91%" runat="server"></asp:TextBox>
                            <br />
                    </div>
                    <p />
                </asp:Panel>
                <div runat="server" id="DivBotoesEnxoval">
                    <asp:ImageButton ID="imgConfirmarEnxoval" ImageUrl="~/images/ConfirmarEnxoval.png" ToolTip="Confirmar atendimento" runat="server" />
                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                    <asp:ImageButton ID="imgCancelarEnxoval" ImageUrl="~/images/CancelarEnxoval.png" ToolTip="Cancelar e voltar a tela anterior." runat="server" />
                </div>
            </div>
            <br />
            <asp:HiddenField ID="hddProcessando" runat="server" />
            <asp:HiddenField ID="hddImpressao" runat="server" />
            <asp:HiddenField ID="hddsolId" runat="server" />
            </div>
            <br>
        </ContentTemplate>
    </asp:UpdatePanel>
    <br />
    &nbsp;<div id="Hint" style="position: fixed; top: 389px; left: 0px; width: 1px;"
        onmouseover="overdiv=1;" onmouseout="overdiv=0; setTimeout('hideLayer()',1000)">
        &nbsp;&nbsp;
    </div>
    <br />
</asp:Content>
