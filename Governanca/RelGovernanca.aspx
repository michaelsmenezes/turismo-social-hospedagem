<%@ Page Title="Relatório - Governança" Language="VB" MasterPageFile="~/TurismoSocial.master"
    AutoEventWireup="false" EnableEventValidation="false"  CodeFile="RelGovernanca.aspx.vb" Inherits="RelGovernanca" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=11.0.0.0, Culture=neutral, PublicKeyToken=89845dcd8080cc91" Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <style type="text/css">
        .imgLupa {
            height: 26px;
        }
    </style>
    <link href="../stylesheet.css" rel="stylesheet" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="conPlaHolTurismoSocial" runat="Server">
    <asp:UpdatePanel ID="updGeral" runat="server">
        <ContentTemplate>
            <asp:Panel ID="pnlConsulta" runat="server" DefaultButton="btnConsultar">
                <asp:Panel ID="pnlTitulo" runat="server">
                    <br />
                    <br />
                    <br />
                    <br />
                    <br />
                    <br />
                    <br />
                    <asp:Label ID="lblTituloGovernanca" runat="server" CssClass="formLabelTitulo" Font-Bold="False"
                        Font-Italic="False" Text="Relatório - Governança"></asp:Label>
                    <br />
                </asp:Panel>
                &nbsp;&nbsp;&nbsp;<table>
                    <tr>
                        <td>
                            Data
                        </td>
                        <td>
                            <asp:Label ID="lblTituloBloco" runat="server" Text="Bloco"></asp:Label>
                        </td>
                        <td>
                            Ala
                        </td>
                        <td>
                            Camareira
                        </td>
                        <td>
                            &nbsp;
                        </td>
                        <td>
                            Status
                        </td>
                        <td>&nbsp;</td>
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
                    <tr>
                        <td>
                            <asp:TextBox ID="txtData" runat="server" Width="90px"></asp:TextBox>
                            <cc1:CalendarExtender ID="txtData_CalendarExtender" runat="server" Enabled="True"
                                FirstDayOfWeek="Sunday" Format="dd/MM/yyyy" TargetControlID="txtData">
                            </cc1:CalendarExtender>
                        </td>
                        <td>
                            <asp:DropDownList ID="drpBloco" runat="server" AutoPostBack="True">
                                <asp:ListItem Value="0">Todos</asp:ListItem>
                                <asp:ListItem Value="1">Rio Tocantins</asp:ListItem>
                                <asp:ListItem Value="2">Rio Araguaia</asp:ListItem>
                                <asp:ListItem Value="3">Rio Paranaiba</asp:ListItem>
                                <asp:ListItem Value="33">Rio Vermelho</asp:ListItem>
                            </asp:DropDownList>
                        </td>
                        <td>
                            <asp:DropDownList ID="drpAla" runat="server" AutoPostBack="True">
                            </asp:DropDownList>
                        </td>
                        <td>
                            <asp:DropDownList ID="drpCamareira" runat="server" AutoPostBack="True" Enabled="False">
                            </asp:DropDownList>
                        </td>
                        <td>
                            <asp:ImageButton ID="btnCadCamareira" runat="server" ImageUrl="~/images/Camareira.png" />
                            <cc1:ModalPopupExtender ID="btnCadCamareira_ModalPopupExtender" runat="server" BackgroundCssClass="modalBackground"
                                CancelControlID="btnVoltarCad" DynamicServicePath="" Enabled="True" PopupControlID="pnlCadastroCam"
                                PopupDragHandleControlID="btnSalvarCad" TargetControlID="btnCadCamareira">
                            </cc1:ModalPopupExtender>
                        </td>
                        <td>
                            <asp:DropDownList ID="drpStatus" runat="server">
                                <asp:ListItem Value="0">Todos</asp:ListItem>
                                <asp:ListItem Value="P">Prioridade</asp:ListItem>
                                <asp:ListItem Value="A">Arrumação</asp:ListItem>
                                <asp:ListItem Value="R">Revisão</asp:ListItem>
                                <asp:ListItem Value="T">Atendimento</asp:ListItem>
                                <asp:ListItem Value="X">Total de Saídas-PAX</asp:ListItem>
                            </asp:DropDownList>
                        </td>
                        <td>
                            &nbsp;</td>
                        <td>
                            <asp:Button ID="btnConsultar" runat="server" CssClass="imgLupa" Text="Consultar"
                                Width="100px" />
                        </td>
                        <td>
                            <asp:Button ID="btnVoltar" runat="server" CssClass="imgVoltar" Text="   Voltar" />
                        </td>
                        <td rowspan="2" valign="top">
                            &nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td align="center" colspan="9">
                            &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;
                            <br>
                        </td>
                    </tr>
                </table>
            </asp:Panel>
            <div  class="PosicionaProgresso" align="center">
                <asp:UpdateProgress ID="updProgresso" runat="server" AssociatedUpdatePanelID="updGeral">
                    <ProgressTemplate>
                        <asp:Image ID="imgProgresso" runat="server" ImageUrl="~/images/Aguarde.gif" />
                        <br />
                        <asp:Label ID="lblAguarde" runat="server" Font-Bold="True" Font-Size="Small" ForeColor="#99CCFF"
                            Text="Processando..."></asp:Label>
                    </ProgressTemplate>
                </asp:UpdateProgress>
            </div>
            
            <cc1:RoundedCornersExtender ID="pnlConsulta_RoundedCornersExtender" runat="server"
                Enabled="True" SkinID="pnlConsulta" TargetControlID="pnlConsulta">
            </cc1:RoundedCornersExtender>
        </ContentTemplate>
    </asp:UpdatePanel>
    <br>
    <asp:Panel ID="pnlCadastroCam" runat="server" BackColor="White" Height="100px" Width="530px" style="display:none">
        <table>
            <tr>
                <td class="formRuler" colspan="3">
                    Cadastro de Camareiras
                </td>
            </tr>
            <tr>
                <td>
                    Nome
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
                    <asp:TextBox ID="txtNomeCad" runat="server" MaxLength="50" Width="300px"></asp:TextBox>
                </td>
                <td>
                    <asp:Button ID="btnSalvarCad" runat="server" CssClass="imgGravar" OnClientClick="if(confirm('Confirma operação?'))
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
                                };" Text="   Salvar" />
                </td>
                <td>
                    <asp:Button ID="btnVoltarCad" runat="server" CssClass="imgVoltar" Text="   Voltar" />
                </td>
            </tr>
        </table>
    </asp:Panel>
    <br />
    <asp:HiddenField ID="hddProcessando" runat="server" />
    <div style="display: none">
        <asp:Button ID="btnUnidade" runat="server" Text="UnidadeOp" />
    </div>
    <table style="width: 189px">
        <tr>
            <td style="display: none">
                <asp:Button ID="btnExecRelatorio" runat="server" Text="ExecutaRelatorio" />
                <asp:Button ID="btnEscondeRel" runat="server" Text="EscondeRel" />
            </td>
            <td>
                &nbsp;
            </td>
            <td>
                &nbsp;
            </td>
        </tr>
    </table>
    <rsweb:ReportViewer ID="rptAtendimento" runat="server" Width="100%" Visible="False"
        Font-Names="Verdana" Font-Size="8pt" Height="" SizeToReportContent="True" ZoomMode="PageWidth">
        <LocalReport ReportPath="Governanca\RelAtendimento.rdlc">
        </LocalReport>
    </rsweb:ReportViewer>
    <rsweb:ReportViewer ID="rptTodos" runat="server" Font-Names="Verdana" Font-Size="8pt"
        Visible="False" Width="100%" Height="" SizeToReportContent="True" ZoomMode="PageWidth">
        <LocalReport ReportPath="Governanca\RelGovernaca.rdlc">
        </LocalReport>
    </rsweb:ReportViewer>
    <rsweb:ReportViewer ID="rptArrumacao" runat="server" Font-Names="Verdana" Font-Size="8pt"
        Visible="False" Width="100%" Height="" SizeToReportContent="True" ZoomMode="PageWidth">
        <LocalReport ReportPath="Governanca\RelArrumacao.rdlc">
        </LocalReport>
    </rsweb:ReportViewer>
    <rsweb:ReportViewer ID="rptPrioridade" runat="server" Width="100%" Font-Names="Verdana"
        Font-Size="8pt" Visible="False" Height="" SizeToReportContent="True" ZoomMode="PageWidth">
        <LocalReport ReportPath="Governanca\RelPrioridade.rdlc">
        </LocalReport>
    </rsweb:ReportViewer>
    <rsweb:ReportViewer ID="rptRevisao" runat="server" Font-Names="Verdana" Font-Size="8pt"
        Visible="False" Width="100%" Height="" SizeToReportContent="True" ZoomMode="PageWidth">
        <LocalReport ReportPath="Governanca\RelRevisao.rdlc">
        </LocalReport>
    </rsweb:ReportViewer>
    <rsweb:ReportViewer ID="rptTotalPax" runat="server" Font-Names="Verdana" Font-Size="8pt"
        Visible="False" Width="100%" Height="" SizeToReportContent="True" ZoomMode="PageWidth" WaitMessageFont-Names="Verdana" WaitMessageFont-Size="14pt">
        <LocalReport ReportPath="Governanca\RelTotPax.rdlc">
        </LocalReport>
    </rsweb:ReportViewer>
</asp:Content>
