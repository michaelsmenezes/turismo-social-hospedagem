<%@ Page Title="B.O.H-Boletim de Ocupação Hoteleira" Language="VB" MasterPageFile="~/TurismoSocial.master" AutoEventWireup="false" CodeFile="frmMinisterioTurismo.aspx.vb" Inherits="InformacoesGerenciais_frmMinisterioTurismo" %>

<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="asp" %>

<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=11.0.0.0, Culture=neutral, PublicKeyToken=89845dcd8080cc91" Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <link href="../stylesheet.css" rel="stylesheet" type="text/css" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="conPlaHolTurismoSocial" Runat="Server">
    <p>
        <br />
    </p>
    <p>
        &nbsp;</p>
    <p>
        &nbsp;</p>
    <p>
        &nbsp;</p>
    <div class="PosicionaProgresso" align="center">
        <asp:UpdateProgress ID="updProReserva" runat="server" AssociatedUpdatePanelID="updGeral"
            EnableViewState="True">
            <ProgressTemplate>
                Processando sua solicitação...<br />
                &nbsp;
                <asp:Image ID="imgProcessando" runat="server" ImageUrl="~/images/Aguarde.gif" />
            </ProgressTemplate>
        </asp:UpdateProgress>
    </div>
       
    <asp:UpdatePanel ID="updGeral" runat="server">
        <ContentTemplate>
            <asp:Panel ID="pnlConsultar" runat="server">
                <table>
                    <tr>
                        <td class="formRuler" colspan="3">
                            B.O.H - Boletim de Ocupação Hoteleira</td>
                    </tr>
                    <tr>
                        <td>
                            Mês<br />
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
                            Ano<br />
                            <asp:DropDownList ID="drpAno" runat="server">
                            </asp:DropDownList>
                        </td>
                        <td>
                            <br />
                            <asp:Button ID="btnConsultar" runat="server" CommandArgument="Consultar" 
                                CssClass="imgLupa" Text="   Consultar" />
                            <asp:Button ID="btnImprimir" runat="server" CommandArgument="Imprimir" 
                                CssClass="imgRelatorio" Text="  Imprimir" />
                        </td>
                    </tr>
                </table>
                <br />
            </asp:Panel>
            <asp:RoundedCornersExtender ID="pnlConsultar_RoundedCornersExtender" 
                runat="server" BorderColor="ActiveBorder" Color="ActiveBorder" Enabled="True" 
                TargetControlID="pnlConsultar">
            </asp:RoundedCornersExtender>
            <asp:Panel ID="pnlGridMinisterio" runat="server">
                <asp:GridView ID="gdvMinisterio" runat="server" AutoGenerateColumns="False" 
                    CellPadding="4" ForeColor="#333333" GridLines="None" 
                    DataKeyNames="Data,Entradas,Saidas,QtdePessoasDiaMesAnterior,Apartamentos,Leitos,TotLeitos,Hospedados,SaidasUH,EntradasUH" 
                    ShowFooter="True">
                    <RowStyle BackColor="#EFF3FB" />
                    <Columns>
                        <asp:BoundField DataField="Data" HeaderText="Dias|">
                        <FooterStyle CssClass="formRuler" />
                        <HeaderStyle CssClass="formRuler" />
                        <ItemStyle HorizontalAlign="Center" />
                        </asp:BoundField>
                        <asp:BoundField DataField="Entradas" HeaderText="Entradas|">
                        <FooterStyle CssClass="formRuler" />
                        <HeaderStyle CssClass="formRuler" />
                        <ItemStyle HorizontalAlign="Center" />
                        </asp:BoundField>
                        <asp:BoundField DataField="Saidas" HeaderText="Saídas|">
                        <FooterStyle CssClass="formRuler" />
                        <HeaderStyle CssClass="formRuler" />
                        <ItemStyle HorizontalAlign="Center" />
                        </asp:BoundField>
                        <asp:BoundField DataField="EntradasUH" HeaderText="Entradas UHs|">
                        <FooterStyle CssClass="formRuler" />
                        <HeaderStyle CssClass="formRuler" />
                        <ItemStyle HorizontalAlign="Center" />
                        </asp:BoundField>
                        <asp:BoundField DataField="SaidasUH" HeaderText="Saídas UHs|">
                        <FooterStyle CssClass="formRuler" />
                        <HeaderStyle CssClass="formRuler" />
                        <ItemStyle HorizontalAlign="Center" />
                        </asp:BoundField>
                        <asp:BoundField DataField="Apartamentos" HeaderText="UHS Ocupadas|">
                        <FooterStyle CssClass="formRuler" />
                        <HeaderStyle CssClass="formRuler" />
                        <ItemStyle HorizontalAlign="Center" />
                        </asp:BoundField>
                        <asp:BoundField DataField="Leitos" HeaderText="Leitos Ocupados|">
                        <FooterStyle CssClass="formRuler" />
                        <HeaderStyle CssClass="formRuler" />
                        <ItemStyle HorizontalAlign="Center" />
                        </asp:BoundField>
                        <asp:BoundField DataField="Hospedados" HeaderText="Hospedados|">
                        <FooterStyle CssClass="formRuler" HorizontalAlign="Center" />
                        <HeaderStyle CssClass="formRuler" HorizontalAlign="Center" />
                        <ItemStyle HorizontalAlign="Center" />
                        </asp:BoundField>
                        <asp:BoundField HeaderText="Média de Ocupação em UHs|">
                        <FooterStyle CssClass="formRuler" />
                        <FooterStyle CssClass="formRuler" HorizontalAlign="Center" />
                        <HeaderStyle CssClass="formRuler" />
                        <ItemStyle HorizontalAlign="Center" />
                        </asp:BoundField>
                        <asp:BoundField HeaderText="Média de Ocupação em Leitos">
                        <FooterStyle CssClass="formRuler" />
                        <FooterStyle CssClass="formRuler" HorizontalAlign="Center" />
                        <HeaderStyle CssClass="formRuler" />
                        <ItemStyle HorizontalAlign="Center" />
                        </asp:BoundField>
                    </Columns>
                    <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="Black" HorizontalAlign="Center" />
                    <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
                    <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                    <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="Black" />
                    <EditRowStyle BackColor="#2461BF" />
                    <AlternatingRowStyle BackColor="White" />
                </asp:GridView>
            </asp:Panel>
            <rsweb:ReportViewer ID="rptBHO" runat="server" Font-Names="Verdana" 
                Font-Size="8pt" Height="" Visible="False" Width="100%" WaitMessageFont-Names="Verdana" WaitMessageFont-Size="14pt">
                <LocalReport ReportPath="InformacoesGerenciais\rptBoletimOcupacao.rdlc">
                </LocalReport>
            </rsweb:ReportViewer>
            <br />
            <br />
        </ContentTemplate>
    </asp:UpdatePanel>
    </asp:Content>

