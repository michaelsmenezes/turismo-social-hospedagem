<%@ Page Title="Histórico de Refeições" Language="VB" MasterPageFile="~/TurismoSocial.master" AutoEventWireup="true" CodeFile="frmHistoricoRefeicoes.aspx.vb" Inherits="frmHistoricoRefeicoes" EnableEventValidation="false" %>

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
                <div align="center" class="ArrendodarBorda formRuler" style="font-weight:bold; font-size: x-large; color: #FFFFFF;"  >Histórico de Refeições</div>
                <table>
                    <tr valign="bottom"  >
                        <td >
                            Mês</td>
                        <td>
                            &nbsp;</td>
                        <td >
                            Ano</td>
                        <td style="display: none">
                            Inserção</td>
                        <td>Origem dos dados</td>
                        <td>
                            &nbsp;</td>
                        <td>
                            Dados
                            <br />
                            Estatísticos</td>
                        <td>
                            &nbsp;</td>
                        <td align="center">
                            Total Geral<br />
                            de Atendimentos</td>
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
                        <td style="display: none">
                            <asp:DropDownList ID="drpTipoRefeicao" runat="server" CssClass="ArrendodarBorda">
                                <asp:ListItem Value="T">Todas</asp:ListItem>
                                <asp:ListItem Value="M">Manual</asp:ListItem>
                            </asp:DropDownList>
                        </td>
                        <td>
                            <asp:DropDownList ID="drpOrigemDados" runat="server" CssClass="ArrendodarBorda">
                                <asp:ListItem Value="C" Selected="True">Hóspedes e Passantes</asp:ListItem> <%--Observação: Aqui entra também as refeições dos servidores--%>
                                <asp:ListItem Value="H">Hóspedes</asp:ListItem>
                                <asp:ListItem Value="P">Passantes</asp:ListItem>
                            </asp:DropDownList>
                        </td>
                        <td>
                            <asp:Button ID="btnConsultar" runat="server" Height="22px" CssClass="imgLupa ArrendodarBorda" 
                                Text="  Consultar" />
                        </td>
                        <td>
                            <asp:Button ID="btnEstatistica" runat="server" Height="22px" CssClass="imgEstatistica ArrendodarBorda" 
                                Text="   Gerar" />
                        </td>
                        <td>
                            <asp:Button ID="btnVoltar" runat="server" Height="22px" CssClass="imgVoltar ArrendodarBorda" Text="   Voltar" />
                        </td>
                        <td align="center" class="ArrendodarBorda">
                            <asp:Label ID="lblTotRefeicao" runat="server"></asp:Label>
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
                <br/>
                <table align="center" >
                    <tr>
                        <td align="center"  class="ArrendodarBorda formRuler" style="font-size: medium; font-weight: normal"  width="25%">
                            Desjejum<br />
                            Atendimento</td>
                        <td align="center" class="ArrendodarBorda formRuler" colspan="3" style="font-size: medium; font-weight: normal" width="25%">
                            Almoço<br />
                            Atendimento</td>
                        <td align="center" class="ArrendodarBorda formRuler" colspan="5" style="font-size: medium; font-weight: normal" width="25%">
                            Jantar<br />
                            Atendimento</td>
                    </tr>
                    <tr>
                        <td colspan="9">
                            <asp:GridView ID="gdvHistoricoRef" runat="server"  AutoGenerateColumns="False" 
                                CellPadding="4" ForeColor="#333333" GridLines="None" 
                                
                                DataKeyNames="Dia,DesCom,DesDep,DesCon,DesUsu,DesIse,DesMen5,DesSer,AlmCom,AlmDep,AlmCon,AlmUsu,AlmIse,AlmMen5,AlmSer,JanCom,JanDep,JanCon,JanUsu,JanIse,JanMen5,JanSer" 
                                EmptyDataText="Não existem informações a serem exibidas" Width="100%" ShowFooter="True">
                                <RowStyle BackColor="#EFF3FB" CssClass="forRuller" />
                                <Columns>
                                    <asp:BoundField DataField="Dia" HeaderText="Dia">
                                        <HeaderStyle CssClass="formRuler" />
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="DesCom" HeaderText="Com">
                                        <HeaderStyle CssClass="formRuler" />
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="DesDep" HeaderText="Dep">
                                        <HeaderStyle CssClass="formRuler" />
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="DesUsu" HeaderText="Usu">
                                        <HeaderStyle CssClass="formRuler" />
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:BoundField>
                                    <asp:BoundField HeaderText="Total">
                                        <HeaderStyle CssClass="formRuler" />
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="DesIse" HeaderText="Não Indenizado">
                                        <HeaderStyle CssClass="formRuler" />
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:BoundField>
                                    <asp:BoundField>
                                        <ControlStyle CssClass="formRuler" />
                                        <HeaderStyle CssClass="formRuler" />
                                        <ItemStyle CssClass="formRuler" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="Dia" HeaderText="Dia">
                                        <HeaderStyle CssClass="formRuler" />
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="AlmCom" HeaderText="Com">
                                        <HeaderStyle CssClass="formRuler" />
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="AlmDep" HeaderText="Dep">
                                        <HeaderStyle CssClass="formRuler" />
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="AlmUsu" HeaderText="Usu">
                                        <HeaderStyle CssClass="formRuler" />
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:BoundField>
                                    <asp:BoundField HeaderText="Total">
                                        <HeaderStyle CssClass="formRuler" />
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="AlmIse" HeaderText="Não Indenizado">
                                        <HeaderStyle CssClass="formRuler" />
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:BoundField>
                                    <asp:BoundField>
                                        <ControlStyle CssClass="formRuler" />
                                        <HeaderStyle CssClass="formRuler" />
                                        <ItemStyle CssClass="formRuler" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="Dia" HeaderText="Dia">
                                        <HeaderStyle CssClass="formRuler" />
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="JanCom" HeaderText="Com">
                                        <HeaderStyle CssClass="formRuler" />
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="JanDep" HeaderText="Dep">
                                        <HeaderStyle CssClass="formRuler" />
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="JanUsu" HeaderText="Usu">
                                        <HeaderStyle CssClass="formRuler" />
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:BoundField>
                                    <asp:BoundField HeaderText="Total">
                                        <HeaderStyle CssClass="formRuler" />
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="JanIse" HeaderText="Não Indenizado">
                                        <HeaderStyle CssClass="formRuler" />
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:BoundField>
                                    <asp:BoundField>
                                        <ControlStyle CssClass="formRuler" />
                                        <HeaderStyle CssClass="formRuler" />
                                        <ItemStyle CssClass="formRuler" />
                                    </asp:BoundField>
                                </Columns>
                                <FooterStyle CssClass="formRuler" Font-Bold="True" ForeColor="White" />
                                <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
                                <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                                <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                                <EditRowStyle BackColor="#2461BF" />
                                <AlternatingRowStyle BackColor="White" />
                            </asp:GridView>
                        </td>
                    </tr>
                    <tr>
                        <td valign="top">
                            Assinatura-Impresso em:
                            <asp:Label ID="lblDataImpressao" runat="server" Text="Label"></asp:Label>
                        </td>
                        <td align="center" style="display: none">
                            Total de refeições fornecidas<br />
                            &nbsp;a servidores</td>
                        <td align="right" style="display: none">
                            Desjejum:</td>
                        <td style="display: none">
                            <asp:Label ID="lblDesjejumServ" runat="server" Font-Bold="True" Text="Label"></asp:Label>
                        </td>
                        <td style="display: none">
                            &nbsp;</td>
                        <td align="right" style="display: none">
                            Almoço:</td>
                        <td style="display: none">
                            <asp:Label ID="lblAlmocoServ" runat="server" Font-Bold="True" Text="Label"></asp:Label>
                        </td>
                        <td align="right" style="display: none">
                            Jantar:</td>
                        <td style="display: none">
                            <asp:Label ID="lblJantarServ" runat="server" Font-Bold="True" Text="Label"></asp:Label>
                        </td>
                    </tr>
                </table>
                <br />
             </asp:Panel>
            <rsweb:ReportViewer ID="rptEstatistica" runat="server" Font-Names="Verdana" 
                Font-Size="8pt" Visible="False" Width="100%" Height="" SizeToReportContent="True" ZoomMode="PageWidth">
                <LocalReport ReportPath="AlimentosEBebidas\rptDadosEstatisticosRestaurante.rdlc">
                </LocalReport>
            </rsweb:ReportViewer>

            <rsweb:ReportViewer ID="rptEstatisticaPiri" runat="server" Font-Names="Verdana" 
                Font-Size="8pt" Visible="False" Width="100%" Height="" WaitMessageFont-Names="Verdana" WaitMessageFont-Size="14pt">
                <LocalReport ReportPath="AlimentosEBebidas\rptDadosEstatisticosRestaurante.rdlc">
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

