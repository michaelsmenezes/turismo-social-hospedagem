<%@ Page Title="" Language="VB" MasterPageFile="~/TurismoSocial.master" AutoEventWireup="True"
    CodeFile="Valor.aspx.vb" Inherits="Valor" UICulture="Auto" %>

<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=11.0.0.0, Culture=neutral, PublicKeyToken=89845dcd8080cc91" Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Assembly="System.Web.DynamicData, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.DynamicData" TagPrefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <style type="text/css">
        .auto-style1 {
            height: 29px;
        }

        .auto-style2 {
            height: 21px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="conPlaHolTurismoSocial" runat="Server">
    atta<br />
    <div class="PosicionaProgresso" align="center">
        <asp:UpdateProgress ID="updProValor" runat="server" AssociatedUpdatePanelID="updPnlValor"
            EnableViewState="True">
            <ProgressTemplate>
                Processando sua solicitação...<br />
                &nbsp;<img alt="Processando..." src="images/Aguarde.gif" />
            </ProgressTemplate>
        </asp:UpdateProgress>
    </div>
    <asp:UpdatePanel ID="updPnlValor" runat="server">
        <ContentTemplate>
            <asp:Panel ID="pnlValor" runat="server" Height="0px" Width="100%">

                <table class="formLabel" width="100%" align="center">
                    <tr>
                        <td>&nbsp;</td>
                        <td align="center" colspan="9">
                            <br />
                            <br />
                            <br />
                            <br />
                            <br />
                            <br />
                        </td>
                    </tr>
                    <tr>
                        <td>&nbsp;</td>
                        <td align="center" colspan="9">
                            <asp:Label ID="lblBloco" runat="server" Text="Tabela de Valores de Hospedagem " CssClass="formLabelWeb"></asp:Label>
                            <asp:DropDownList ID="cmbTipo" runat="server" AutoPostBack="True">
                            </asp:DropDownList>
                            <asp:DropDownList ID="cmbAcmId" runat="server" Visible="False">
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td>&nbsp;</td>
                        <td colspan="8">&nbsp;</td>
                        <td align="right">
                            <asp:Button ID="btnAltaTemporada" runat="server" CssClass="formButton" Text="   Alta Temporada" />
                        </td>
                    </tr>
                    <tr>
                        <td>&nbsp;</td>
                        <td align="center" colspan="9" class="TituloLegalMenor">
                            <asp:Label ID="lblNovosValores" runat="server" CssClass="formLabelWeb" Text="Valores atuais"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td>&nbsp;</td>
                        <td colspan="3" align="right" width="33%">
                            <asp:GridView ID="gdvComerciario" runat="server" AutoGenerateColumns="False" CellPadding="4" Caption="<font size=3 color=Green>Comerciário</font>" DataKeyNames="catDescricao,valValor,catId,valQtde">
                                <AlternatingRowStyle CssClass="tableRowOdd" />
                                <EmptyDataRowStyle BorderColor="White" BorderStyle="Solid" Height="100px" HorizontalAlign="Center" VerticalAlign="Middle" />
                                <Columns>
                                    <asp:TemplateField HeaderText="">
                                        <ItemTemplate>
                                            <asp:Label ID="lblComNor" runat="server" Text='<%# Bind("catDescricao") %>' ForeColor="Green"></asp:Label>
                                        </ItemTemplate>
                                        <HeaderTemplate>
                                            <asp:LinkButton ID="lnkhdrComNor" runat="server" CommandArgument="0" ForeColor="White">Classificação</asp:LinkButton>
                                        </HeaderTemplate>
                                        <ControlStyle />
                                        <FooterStyle CssClass="formLabel" HorizontalAlign="Center" />
                                        <HeaderStyle BorderWidth="0px" CssClass="formRuler" Width="33%" />
                                        <ItemStyle CssClass="gridBorderColor" Width="33%" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Valor por pessoa">
                                        <ItemTemplate>
                                            <%--<asp:TextBox ID="TextBox1" runat="server" onblur="this.value = CalculaSuites(this.value,1.30)" Columns="5"></asp:TextBox>--%>
                                            <asp:TextBox ID="txtVlrComNor" runat="server" Columns="5"></asp:TextBox>
                                        </ItemTemplate>
                                        <HeaderTemplate>
                                            <asp:LinkButton ID="lnkhdrVlrComNor" runat="server" CommandArgument="0" ForeColor="White">Valor por pessoa</asp:LinkButton>
                                        </HeaderTemplate>
                                        <ControlStyle />
                                        <FooterStyle CssClass="formLabel" HorizontalAlign="Center" />
                                        <HeaderStyle BorderWidth="0px" CssClass="formRuler" Width="33%" />
                                        <ItemStyle CssClass="gridBorderColor" HorizontalAlign="Right" Width="33%" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Suíte">
                                        <ItemTemplate>
                                            <asp:Label ID="lblVlrComSui" runat="server" ForeColor="Green"></asp:Label>
                                        </ItemTemplate>
                                        <HeaderTemplate>
                                            <asp:LinkButton ID="lnkhdrVlrComSui" runat="server" CommandArgument="0" ForeColor="White">Suíte</asp:LinkButton>
                                        </HeaderTemplate>
                                        <ControlStyle />
                                        <FooterStyle CssClass="formLabel" HorizontalAlign="Center" />
                                        <HeaderStyle BorderWidth="0px" CssClass="formRuler" Width="33%" />
                                        <ItemStyle CssClass="gridBorderColor" HorizontalAlign="Right" Width="33%" />
                                    </asp:TemplateField>
                                </Columns>
                                <HeaderStyle ForeColor="White" Font-Bold="False" />
                                <SelectedRowStyle Font-Bold="True" />
                            </asp:GridView>
                        </td>
                        <td colspan="3" align="center" width="33%">
                            <asp:GridView ID="gdvConveniado" runat="server" AutoGenerateColumns="False" Caption="<font size=3 color=Chocolate>Conveniado</font>" CellPadding="4" DataKeyNames="catDescricao,valValor,catId,valQtde">
                                <AlternatingRowStyle CssClass="tableRowOdd" />
                                <EmptyDataRowStyle BorderColor="White" BorderStyle="Solid" Height="100px" HorizontalAlign="Center" VerticalAlign="Middle" />
                                <Columns>
                                    <asp:TemplateField HeaderText="">
                                        <ItemTemplate>
                                            <asp:Label ID="lblConNor" runat="server" Text='<%# Bind("catDescricao") %>' ForeColor="Chocolate"></asp:Label>
                                        </ItemTemplate>
                                        <HeaderTemplate>
                                            <asp:LinkButton ID="lnkhdrConNor" runat="server" CommandArgument="0" ForeColor="White">Classificação</asp:LinkButton>
                                        </HeaderTemplate>
                                        <FooterStyle CssClass="formLabel" HorizontalAlign="Center" />
                                        <HeaderStyle BorderWidth="0px" CssClass="formRuler" Width="33%" />
                                        <ItemStyle CssClass="gridBorderColor" Width="33%" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Valor por pessoa">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtVlrConNor" runat="server" Columns="5"></asp:TextBox>
                                        </ItemTemplate>
                                        <HeaderTemplate>
                                            <asp:LinkButton ID="lnkhdrVlrConNor" runat="server" CommandArgument="0" ForeColor="White">Valor por pessoa</asp:LinkButton>
                                        </HeaderTemplate>
                                        <FooterStyle CssClass="formLabel" HorizontalAlign="Center" />
                                        <HeaderStyle BorderWidth="0px" CssClass="formRuler" Width="33%" />
                                        <ItemStyle CssClass="gridBorderColor" HorizontalAlign="Right" Width="33%" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Suíte">
                                        <ItemTemplate>
                                            <asp:Label ID="lblVlrConSui" runat="server" ForeColor="Chocolate"></asp:Label>
                                        </ItemTemplate>
                                        <HeaderTemplate>
                                            <asp:LinkButton ID="lnkhdrVlrConSui" runat="server" CommandArgument="0" ForeColor="White">Suíte</asp:LinkButton>
                                        </HeaderTemplate>
                                        <FooterStyle CssClass="formLabel" HorizontalAlign="Center" />
                                        <HeaderStyle BorderWidth="0px" CssClass="formRuler" Width="33%" />
                                        <ItemStyle CssClass="gridBorderColor" HorizontalAlign="Right" Width="33%" />
                                    </asp:TemplateField>
                                </Columns>
                                <HeaderStyle ForeColor="White" />
                                <SelectedRowStyle Font-Bold="True" />
                            </asp:GridView>
                        </td>
                        <td colspan="3" align="left" width="33%">
                            <asp:GridView ID="gdvUsuario" runat="server" AutoGenerateColumns="False" Caption="<font size=3 color=Red>Usuário</font>" CellPadding="4" DataKeyNames="catDescricao,valValor,catId,valQtde">
                                <AlternatingRowStyle CssClass="tableRowOdd" />
                                <EmptyDataRowStyle BorderColor="White" BorderStyle="Solid" Height="100px" HorizontalAlign="Center" VerticalAlign="Middle" />
                                <Columns>
                                    <asp:TemplateField HeaderText="">
                                        <ItemTemplate>
                                            <asp:Label ID="lblUsuNor" runat="server" Text='<%# Bind("catDescricao") %>' ForeColor="Red"></asp:Label>
                                        </ItemTemplate>
                                        <HeaderTemplate>
                                            <asp:LinkButton ID="lnkhdrUsuNor" runat="server" CommandArgument="0" ForeColor="White">Classificação</asp:LinkButton>
                                        </HeaderTemplate>
                                        <FooterStyle CssClass="formLabel" HorizontalAlign="Center" />
                                        <HeaderStyle BorderWidth="0px" CssClass="formRuler" Width="33%" />
                                        <ItemStyle CssClass="gridBorderColor" Width="33%" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Valor por pessoa">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtVlrUsuNor" runat="server" Columns="5"></asp:TextBox>
                                        </ItemTemplate>
                                        <HeaderTemplate>
                                            <asp:LinkButton ID="lnkhdrVlrUsuNor" runat="server" CommandArgument="0" ForeColor="White">Valor por pessoa</asp:LinkButton>
                                        </HeaderTemplate>
                                        <FooterStyle CssClass="formLabel" HorizontalAlign="Center" />
                                        <HeaderStyle BorderWidth="0px" CssClass="formRuler" Width="33%" />
                                        <ItemStyle CssClass="gridBorderColor" HorizontalAlign="Right" Width="33%" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Suíte">
                                        <ItemTemplate>
                                            <asp:Label ID="lblVlrUsuSui" runat="server" ForeColor="Red"></asp:Label>
                                        </ItemTemplate>
                                        <HeaderTemplate>
                                            <asp:LinkButton ID="lnkhdrVlrUsuSui" runat="server" CommandArgument="0" ForeColor="White">Suíte</asp:LinkButton>
                                        </HeaderTemplate>
                                        <FooterStyle CssClass="formLabel" HorizontalAlign="Center" />
                                        <HeaderStyle BorderWidth="0px" CssClass="formRuler" Width="33%" />
                                        <ItemStyle CssClass="gridBorderColor" HorizontalAlign="Right" Width="33%" />
                                    </asp:TemplateField>
                                </Columns>
                                <HeaderStyle ForeColor="White" />
                                <SelectedRowStyle Font-Bold="True" />
                            </asp:GridView>
                        </td>
                    </tr>
                    <tr>
                        <td></td>
                        <td></td>
                        <td></td>
                        <td colspan="2"></td>
                        <td></td>
                        <td colspan="2"></td>
                        <td></td>
                        <td></td>
                    </tr>
                    <tr>
                        <td>&nbsp;</td>
                        <td align="center" colspan="9" valign="middle">&nbsp;</td>
                    </tr>
                    <tr>
                        <td>&nbsp;</td>
                        <td align="center" colspan="9" valign="middle">
                            <asp:HiddenField ID="hddData" runat="server" />
                            <asp:Label ID="lblVigência" runat="server" CssClass="formLabelWeb" Text="Os novos valores entrarão em vigor no dia "></asp:Label>
                            <asp:TextBox ID="txtData" runat="server" Columns="8" MaxLength="10"></asp:TextBox>
                            <asp:CalendarExtender ID="txtData_CalendarExtender" runat="server" Enabled="True" Format="dd/MM/yyyy" TargetControlID="txtData">
                            </asp:CalendarExtender>
                            &nbsp;&nbsp;<asp:HiddenField ID="hddDataAntiga" runat="server" />
                            <asp:HiddenField ID="hddHoraAntiga" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td class="auto-style1"></td>
                        <td class="auto-style1"></td>
                        <td align="center" class="auto-style1"></td>
                        <td align="center" colspan="5" class="auto-style1"></td>
                        <td align="center" class="auto-style1"></td>
                        <td align="right" class="auto-style1">
                            <asp:Button ID="btnTransportar" runat="server" CssClass="btnIrUltimoRegistro" Text="   Transportar" ToolTip="Transporta os valores acima para baixo" OnClientClick="return confirm('Confirma transporte dos valores acima para baixo?')" />
                        </td>
                    </tr>
                    <tr>
                        <td>&nbsp;</td>
                        <td align="center" colspan="9" class="TituloLegalMenor">
                            <asp:Label ID="lblAntigosValores" runat="server" CssClass="formLabelWeb" Text="Valores antigos"></asp:Label>
                            &nbsp;</td>
                    </tr>
                    <tr>
                        <td>&nbsp;</td>
                        <td colspan="3" align="right" width="33%">
                            <asp:GridView ID="gdvComerciarioA" runat="server" AutoGenerateColumns="False" Caption="<font size=3 color=Green>Comerciário</font>" CellPadding="4" DataKeyNames="catDescricao,valValor,catId,valQtde">
                                <AlternatingRowStyle CssClass="tableRowOdd" />
                                <EmptyDataRowStyle BorderColor="White" BorderStyle="Solid" Height="100px" HorizontalAlign="Center" VerticalAlign="Middle" />
                                <Columns>
                                    <asp:TemplateField HeaderText="">
                                        <ItemTemplate>
                                            <asp:Label ID="lblComNorA" runat="server" Text='<%# Bind("catDescricao") %>' ForeColor="Green"></asp:Label>
                                        </ItemTemplate>
                                        <HeaderTemplate>
                                            <asp:LinkButton ID="lnkhdrComNorA" runat="server" CommandArgument="0" ForeColor="White">Classificação</asp:LinkButton>
                                        </HeaderTemplate>
                                        <FooterStyle CssClass="formLabel" HorizontalAlign="Center" />
                                        <HeaderStyle BorderWidth="0px" CssClass="formRuler" Width="33%" />
                                        <ItemStyle CssClass="gridBorderColor" Width="33%" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Valor por pessoa">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtVlrComNorA" runat="server" Columns="5"></asp:TextBox>
                                        </ItemTemplate>
                                        <HeaderTemplate>
                                            <asp:LinkButton ID="lnkhdrVlrComNorA" runat="server" CommandArgument="0" ForeColor="White">Valor por pessoa</asp:LinkButton>
                                        </HeaderTemplate>
                                        <FooterStyle CssClass="formLabel" HorizontalAlign="Center" />
                                        <HeaderStyle BorderWidth="0px" CssClass="formRuler" Width="33%" />
                                        <ItemStyle CssClass="gridBorderColor" HorizontalAlign="Right" Width="33%" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Suíte">
                                        <ItemTemplate>
                                            <asp:Label ID="lblVlrComSuiA" runat="server" ForeColor="Green"></asp:Label>
                                        </ItemTemplate>
                                        <HeaderTemplate>
                                            <asp:LinkButton ID="lnkhdrVlrComSuiA" runat="server" CommandArgument="0" ForeColor="White">Suíte</asp:LinkButton>
                                        </HeaderTemplate>
                                        <FooterStyle CssClass="formLabel" HorizontalAlign="Center" />
                                        <HeaderStyle BorderWidth="0px" CssClass="formRuler" Width="33%" />
                                        <ItemStyle CssClass="gridBorderColor" HorizontalAlign="Right" Width="33%" />
                                    </asp:TemplateField>
                                </Columns>
                                <HeaderStyle ForeColor="White" />
                                <SelectedRowStyle Font-Bold="True" />
                            </asp:GridView>
                        </td>
                        <td colspan="3" align="center" width="33%">
                            <asp:GridView ID="gdvConveniadoA" runat="server" AutoGenerateColumns="False" Caption="<font size=3 color=Chocolate>Conveniado</font>" CellPadding="4" DataKeyNames="catDescricao,valValor,catId,valQtde">
                                <AlternatingRowStyle CssClass="tableRowOdd" />
                                <EmptyDataRowStyle BorderColor="White" BorderStyle="Solid" Height="100px" HorizontalAlign="Center" VerticalAlign="Middle" />
                                <Columns>
                                    <asp:TemplateField HeaderText="">
                                        <ItemTemplate>
                                            <asp:Label ID="lblConNorA" runat="server" Text='<%# Bind("catDescricao") %>' ForeColor="Chocolate"></asp:Label>
                                        </ItemTemplate>
                                        <HeaderTemplate>
                                            <asp:LinkButton ID="lnkhdrConNorA" runat="server" CommandArgument="0" ForeColor="White">Classificação</asp:LinkButton>
                                        </HeaderTemplate>
                                        <FooterStyle CssClass="formLabel" HorizontalAlign="Center" />
                                        <HeaderStyle BorderWidth="0px" CssClass="formRuler" Width="33%" />
                                        <ItemStyle CssClass="gridBorderColor" Width="33%" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Valor por pessoa">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtVlrConNorA" runat="server" Columns="5"></asp:TextBox>
                                        </ItemTemplate>
                                        <HeaderTemplate>
                                            <asp:LinkButton ID="lnkhdrVlrConNorA" runat="server" CommandArgument="0" ForeColor="White">Valor por pessoa</asp:LinkButton>
                                        </HeaderTemplate>
                                        <FooterStyle CssClass="formLabel" HorizontalAlign="Center" />
                                        <HeaderStyle BorderWidth="0px" CssClass="formRuler" Width="33%" />
                                        <ItemStyle CssClass="gridBorderColor" HorizontalAlign="Right" Width="33%" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Suíte">
                                        <ItemTemplate>
                                            <asp:Label ID="lblVlrConSuiA" runat="server" ForeColor="Chocolate"></asp:Label>
                                        </ItemTemplate>
                                        <HeaderTemplate>
                                            <asp:LinkButton ID="lnkhdrVlrConSuiA" runat="server" CommandArgument="0" ForeColor="White">Suíte</asp:LinkButton>
                                        </HeaderTemplate>
                                        <FooterStyle CssClass="formLabel" HorizontalAlign="Center" />
                                        <HeaderStyle BorderWidth="0px" CssClass="formRuler" Width="33%" />
                                        <ItemStyle CssClass="gridBorderColor" HorizontalAlign="Right" Width="33%" />
                                    </asp:TemplateField>
                                </Columns>
                                <HeaderStyle ForeColor="White" />
                                <SelectedRowStyle Font-Bold="True" />
                            </asp:GridView>
                        </td>
                        <td colspan="3" align="left" width="33%">
                            <asp:GridView ID="gdvUsuarioA" runat="server" AutoGenerateColumns="False" Caption="<font size=3 color=Red>Usuário</font>" CellPadding="4" DataKeyNames="catDescricao,valValor,catId,valQtde">
                                <AlternatingRowStyle CssClass="tableRowOdd" />
                                <EmptyDataRowStyle BorderColor="White" BorderStyle="Solid" Height="100px" HorizontalAlign="Center" VerticalAlign="Middle" />
                                <Columns>
                                    <asp:TemplateField HeaderText="">
                                        <ItemTemplate>
                                            <asp:Label ID="lblUsuNorA" runat="server" Text='<%# Bind("catDescricao") %>' ForeColor="Red"></asp:Label>
                                        </ItemTemplate>
                                        <HeaderTemplate>
                                            <asp:LinkButton ID="lnkhdrUsuNorA" runat="server" CommandArgument="0" ForeColor="White">Classificação</asp:LinkButton>
                                        </HeaderTemplate>
                                        <FooterStyle CssClass="formLabel" HorizontalAlign="Center" />
                                        <HeaderStyle BorderWidth="0px" CssClass="formRuler" Width="33%" />
                                        <ItemStyle CssClass="gridBorderColor" Width="33%" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Valor por pessoa">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtVlrUsuNorA" runat="server" Columns="5"></asp:TextBox>
                                        </ItemTemplate>
                                        <HeaderTemplate>
                                            <asp:LinkButton ID="lnkhdrVlrUsuNorA" runat="server" CommandArgument="0" ForeColor="White">Valor por pessoa</asp:LinkButton>
                                        </HeaderTemplate>
                                        <FooterStyle CssClass="formLabel" HorizontalAlign="Center" />
                                        <HeaderStyle BorderWidth="0px" CssClass="formRuler" Width="33%" />
                                        <ItemStyle CssClass="gridBorderColor" HorizontalAlign="Right" Width="33%" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Suíte">
                                        <ItemTemplate>
                                            <asp:Label ID="lblVlrUsuSuiA" runat="server" ForeColor="Red"></asp:Label>
                                        </ItemTemplate>
                                        <HeaderTemplate>
                                            <asp:LinkButton ID="lnkhdrVlrUsuSuiA" runat="server" CommandArgument="0" ForeColor="White">Suíte</asp:LinkButton>
                                        </HeaderTemplate>
                                        <FooterStyle CssClass="formLabel" HorizontalAlign="Center" />
                                        <HeaderStyle BorderWidth="0px" CssClass="formRuler" Width="33%" />
                                        <ItemStyle CssClass="gridBorderColor" HorizontalAlign="Right" Width="33%" />
                                    </asp:TemplateField>
                                </Columns>
                                <HeaderStyle ForeColor="White" />
                                <SelectedRowStyle Font-Bold="True" />
                            </asp:GridView>
                        </td>
                    </tr>
                    <tr>
                        <td>&nbsp;</td>
                        <td align="center" colspan="9" valign="middle">
                            <asp:Label ID="lblAntigo" runat="server" CssClass="formLabelWeb" Text="As reservas feitas até "></asp:Label>
                            <asp:TextBox ID="txtDataAntiga" runat="server" Columns="8" MaxLength="10"></asp:TextBox>
                            <asp:CalendarExtender ID="txtDataAntiga_CalendarExtender" runat="server" Enabled="True" Format="dd/MM/yyyy" TargetControlID="txtDataAntiga">
                            </asp:CalendarExtender>
                            <asp:DropDownList ID="cmbHora" runat="server">
                                <asp:ListItem Value="00">00</asp:ListItem>
                                <asp:ListItem Value="01">01</asp:ListItem>
                                <asp:ListItem Value="02">02</asp:ListItem>
                                <asp:ListItem Value="03">03</asp:ListItem>
                                <asp:ListItem Value="04">04</asp:ListItem>
                                <asp:ListItem Value="05">05</asp:ListItem>
                                <asp:ListItem Value="06">06</asp:ListItem>
                                <asp:ListItem Value="07">07</asp:ListItem>
                                <asp:ListItem Value="08">08</asp:ListItem>
                                <asp:ListItem Value="09">09</asp:ListItem>
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
                            <asp:Label ID="lblHora" runat="server" CssClass="formLabelWeb" Text=" horas serão calculadas com os valores antigos."></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td>&nbsp;</td>
                        <td align="center" colspan="9">
                            <asp:Button ID="btnReservaGravar" runat="server" AccessKey="G" CommandArgument="0" CssClass="imgGravar" OnClientClick="return confirm('Confirma alterações?')"
                                Text="    Salvar" Visible="False" />
                        </td>
                    </tr>
                </table>
            </asp:Panel>
            <asp:Panel ID="pnlAltaTemporada" runat="server" Visible="False" CssClass="formLabelWeb" HorizontalAlign="Center">
                <br />
                <br />
                <br />
                <br />
                <br />
                <br />
                <asp:Label ID="lblAltaTemporada" runat="server" Text="Alta Temporada"></asp:Label>
                <br />
                <br />
                <asp:Label ID="lblDataInicial" runat="server" Text="Data Inicial"></asp:Label>
                <asp:TextBox ID="txtDataInicial" runat="server" Columns="6" MaxLength="10"></asp:TextBox>
                <asp:CalendarExtender ID="txtDataInicial_CalendarExtender" runat="server" Enabled="True" Format="dd/MM/yyyy" TargetControlID="txtDataInicial">
                </asp:CalendarExtender>
                <asp:Label ID="lblDataFinal" runat="server" Text="Data Final"></asp:Label>
                <asp:TextBox ID="txtDataFinal" runat="server" Columns="6" MaxLength="10"></asp:TextBox>
                <asp:CalendarExtender ID="txtDataFinal_CalendarExtender" runat="server" Enabled="True" Format="dd/MM/yyyy" TargetControlID="txtDataFinal">
                </asp:CalendarExtender>
                <asp:Button ID="btnConsulta" runat="server" CommandArgument="0" CssClass="imgLupa" Text="  Consultar" />
                <asp:Button ID="btnNovo" runat="server" AccessKey="N" CssClass="imgNovo" Text="     Novo" />
                <asp:Button ID="btnVoltar" runat="server" CssClass="imgVoltar" Text="    Voltar" />
                <asp:GridView ID="gdvAltaTemporada" runat="server" AutoGenerateColumns="False" CellPadding="4" EmptyDataText="Atenção! Não existem registros de alta temporada para o período pesquisado." HorizontalAlign="Center" DataKeyNames="pacId,pacDataIni,pacDataFim,pacPercentual,pacDataIniSol,pacDataFimSol,pacFlutuanteFederacao,pacFechado">
                    <AlternatingRowStyle CssClass="tableRowOdd" />
                    <EmptyDataRowStyle BorderColor="White" BorderStyle="Solid" Height="100px" HorizontalAlign="Center" VerticalAlign="Middle" />
                    <Columns>
                        <asp:TemplateField HeaderText="Data Inicial">
                            <ItemTemplate>
                                <asp:LinkButton ID="lnkDataInicial" runat="server" CommandName="select" OnClick="lnkDataInicial_Click"></asp:LinkButton>
                            </ItemTemplate>
                            <HeaderTemplate>
                                <asp:Label ID="lblDataInicial" runat="server" CommandArgument="0" ForeColor="White">Data Inicial</asp:Label>
                            </HeaderTemplate>
                            <HeaderStyle BorderWidth="0px" CssClass="formRuler" />
                            <ItemStyle CssClass="gridBorderColor" HorizontalAlign="Center" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Data Final">
                            <ItemTemplate>
                                <asp:LinkButton ID="lnkDataFinal" runat="server" CommandName="select" OnClick="lnkDataInicial_Click"></asp:LinkButton>
                            </ItemTemplate>
                            <HeaderTemplate>
                                <asp:Label ID="lblReserva" runat="server" CommandArgument="0" ForeColor="White">Data Final</asp:Label>
                            </HeaderTemplate>
                            <HeaderStyle BorderWidth="0px" CssClass="formRuler" />
                            <ItemStyle CssClass="gridBorderColor" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Percentual">
                            <ItemTemplate>
                                <asp:LinkButton ID="lnkPercentual" runat="server" CommandName="select" OnClick="lnkDataInicial_Click"></asp:LinkButton>
                            </ItemTemplate>
                            <HeaderTemplate>
                                <asp:Label ID="lblPercentual" runat="server" CommandArgument="0" ForeColor="White">Percentual</asp:Label>
                            </HeaderTemplate>
                            <HeaderStyle BorderWidth="0px" CssClass="formRuler" />
                            <ItemStyle CssClass="gridBorderColor" HorizontalAlign="Center" Wrap="False" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Data Inicial Vigência">
                            <ItemTemplate>
                                <asp:LinkButton ID="lnkDataInicialVigencia" runat="server" CommandName="select" OnClick="lnkDataInicial_Click"></asp:LinkButton>
                            </ItemTemplate>
                            <HeaderTemplate>
                                <asp:Label ID="lblDataInicialVigencia" runat="server" CommandArgument="0" ForeColor="White">Data Inicial Vigência</asp:Label>
                            </HeaderTemplate>
                            <HeaderStyle BorderWidth="0px" CssClass="formRuler" />
                            <ItemStyle CssClass="gridBorderColor" HorizontalAlign="Center" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Data Final Vigência">
                            <ItemTemplate>
                                <asp:LinkButton ID="lnkDataFinalVigencia" runat="server" CommandName="select" OnClick="lnkDataInicial_Click"></asp:LinkButton>
                            </ItemTemplate>
                            <HeaderTemplate>
                                <asp:Label ID="lblDataFinalVigencia" runat="server" CommandArgument="0" ForeColor="White">Data Final Vigência</asp:Label>
                            </HeaderTemplate>
                            <HeaderStyle BorderWidth="0px" CssClass="formRuler" />
                            <ItemStyle CssClass="gridBorderColor" HorizontalAlign="Center" />
                        </asp:TemplateField>
                        <asp:BoundField DataField="pacFlutuanteFederacao" HeaderText="Bloqueio Federação">
                            <HeaderStyle BorderWidth="0px" CssClass="formRuler" />
                            <ItemStyle CssClass="gridBorderColor" HorizontalAlign="Center" />
                        </asp:BoundField>
                        <asp:BoundField DataField="pacFechado" HeaderText="Pacote Fechado">
                            <HeaderStyle BorderWidth="0px" CssClass="formRuler" />
                            <ItemStyle CssClass="gridBorderColor" HorizontalAlign="Center" />
                        </asp:BoundField>
                    </Columns>
                    <HeaderStyle ForeColor="White" />
                    <SelectedRowStyle Font-Bold="True" />
                </asp:GridView>
            </asp:Panel>
            <asp:Panel ID="pnlAltaTemporadaAcao" runat="server" Visible="false" CssClass="formLabelWeb" HorizontalAlign="Center">
                <br />
                <br />
                <br />
                <br />
                <br />
                <br />
                <asp:Label ID="lblAltaTemporadaAcao" runat="server" Text="Alta Temporada"></asp:Label>
                <br />
                <br />

                <table align="center">
                    <tr>
                        <td align="right">
                            <asp:HiddenField ID="hddPacId" runat="server" />
                            <asp:Label ID="lblDataInicialAcao" runat="server" Text="Inicio da Alta Temporada"></asp:Label>
                        </td>
                        <td align="left">
                            <asp:TextBox ID="txtDataInicialAcao" runat="server" Columns="6" MaxLength="10" Width="100px"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td align="right">
                            <asp:Label ID="lblDataFinalAcao" runat="server" Text="E final "></asp:Label>
                        </td>
                        <td align="left">
                            <asp:TextBox ID="txtDataFinalAcao" runat="server" Columns="6" MaxLength="10" Width="100px"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td align="right">
                            <asp:Label ID="lblPercentualAcao" runat="server" Text="Percentual"></asp:Label>
                        </td>
                        <td align="left">
                            <asp:TextBox ID="txtPercentualAcao" runat="server" Columns="5"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td align="right">
                            <asp:Label ID="lblDataInicialVigenciaAcao" runat="server" Text="Reservas solicitadas entre "></asp:Label>
                        </td>
                        <td align="left">
                            <asp:TextBox ID="txtDataInicialVigenciaAcao" runat="server" Columns="6" MaxLength="10" Width="100px"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td align="right">
                            <asp:Label ID="lblDataFinalVigenciaAcao" runat="server" Text="Até "></asp:Label>
                        </td>
                        <td align="left">
                            <asp:TextBox ID="txtDataFinalVigenciaAcao" runat="server" Columns="6" MaxLength="10" Width="100px"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td align="right">Disponibilizar Flutuante para Federação</td>
                        <td align="left">
                            <asp:CheckBox ID="chkFlutuanteFederacao" runat="server" />
                        </td>
                    </tr>

                    <tr>
                        <td align="right">Pacote Fechado(O período inteiro obrigatoriamente estará numa reserva)</td>
                        <td align="left">
                            <asp:CheckBox ID="chkPacoteFechado" runat="server" />
                            <asp:Image ID="imgPacFechado" runat="server" ToolTip="Uma reserva deverá conter, obrigatoriamente em seu período, todo o período do pacote fechado quando houver intersecções entre ambos." ImageUrl="~/images/ajuda.png"/>
                        </td>
                    </tr>

                </table>
                <asp:CalendarExtender ID="txtDataInicialAcao_CalendarExtender" runat="server" Enabled="True" Format="dd/MM/yyyy" TargetControlID="txtDataInicialAcao">
                </asp:CalendarExtender>
                <asp:CalendarExtender ID="txtDataFinalAcao_CalendarExtender" runat="server" Enabled="True" Format="dd/MM/yyyy" TargetControlID="txtDataFinalAcao">
                </asp:CalendarExtender>
                <asp:CalendarExtender ID="txtDataInicialVigenciaAcao_CalendarExtender" runat="server" Enabled="True" Format="dd/MM/yyyy" TargetControlID="txtDataInicialVigenciaAcao">
                </asp:CalendarExtender>
                <asp:CalendarExtender ID="txtDataFinalVigenciaAcao_CalendarExtender" runat="server" Enabled="True" Format="dd/MM/yyyy" TargetControlID="txtDataFinalVigenciaAcao">
                </asp:CalendarExtender>
                <br />
                <asp:Button ID="btnNovoAcao" runat="server" AccessKey="N" CssClass="imgNovo" Text="     Novo" />
                <asp:Button ID="btnSalvarAcao" runat="server" AccessKey="S" CssClass="imgGravar" Text="     Salvar" />
                <asp:Button ID="btnExcluirAcao" runat="server" AccessKey="E" CssClass="imgExcluir" OnClientClick="return confirm('Confirma exclusão?')" Text="     Excluir" ToolTip="Excluir item" />
                <asp:Button ID="btnVoltarAcao" runat="server" CssClass="imgVoltar" Text="    Voltar" />
            </asp:Panel>

            <asp:Panel ID="pnlDesconto" runat="server" CssClass="formLabelWeb" HorizontalAlign="Center" Visible="False">
                <br />
                <br />
                <asp:Label ID="lblTituloDesconto" runat="server" Text="Tabela de Descontos (Meio de semana e que esteja em baixa temporada)"></asp:Label>
                <br />
                <asp:Panel ID="pnlDescontoConsulta" runat="server">
                    <br />
                    &nbsp;Data Inicial
                    <asp:TextBox ID="txtDesConData1" MaxLength="10" Width="120px" runat="server"></asp:TextBox>
                    <asp:CalendarExtender ID="txtDesConData1_CalendarExtender" runat="server" BehaviorID="txtDesConData1_CalendarExtender" FirstDayOfWeek="Sunday" Format="dd/MM/yyyy" TargetControlID="txtDesConData1">
                    </asp:CalendarExtender>
                    Data Final
                    <asp:TextBox ID="txtDesConData2" MaxLength="10" Width="120px" runat="server"></asp:TextBox>
                    <asp:CalendarExtender ID="txtDesConData2_CalendarExtender" runat="server" BehaviorID="txtDesConData2_CalendarExtender" FirstDayOfWeek="Sunday" Format="dd/MM/yyyy" TargetControlID="txtDesConData2">
                    </asp:CalendarExtender>
                    <asp:Button ID="btnConsultaDesconto" runat="server" CssClass="imgLupa" Text="Consultar" />
                    <asp:Button ID="btnNovoDesconto" runat="server" AccessKey="N" CssClass="imgNovo" Text="     Novo" />
                    <asp:Button ID="btnVoltarDesconto" runat="server" CssClass="imgVoltar" Text="    Voltar" />
                    <br />
                    <br />
                    <asp:GridView ID="gdvListaDescontos" runat="server" AutoGenerateColumns="False" CellPadding="4" DataKeyNames="desId,PercentualDomingo,PercentualSegunda,PercentualTerca,PercentualQuarta,PercentualQuinta,PercentualSexta,PercentualSabado,DesDataInicial,DesDataFinal,DesUsuarioLog,DesUsuarioDataLog" ForeColor="#333333" GridLines="None" HorizontalAlign="Center">
                        <AlternatingRowStyle BackColor="White" />
                        <Columns>
                            <asp:BoundField DataField="PercentualDomingo" HeaderText="Domingo">
                                <HeaderStyle BorderWidth="0px" CssClass="formRuler" />
                                <ItemStyle CssClass="gridBorderColor" HorizontalAlign="Center" />
                            </asp:BoundField>
                            <asp:BoundField DataField="PercentualSegunda" HeaderText="Segunda">
                                <HeaderStyle BorderWidth="0px" CssClass="formRuler" />
                                <ItemStyle CssClass="gridBorderColor" HorizontalAlign="Center" />
                            </asp:BoundField>
                            <asp:BoundField DataField="PercentualTerca" HeaderText="Terça">
                                <HeaderStyle BorderWidth="0px" CssClass="formRuler" />
                                <ItemStyle CssClass="gridBorderColor" HorizontalAlign="Center" />
                            </asp:BoundField>
                            <asp:BoundField DataField="PercentualQuarta" HeaderText="Quarta">
                                <HeaderStyle BorderWidth="0px" CssClass="formRuler" />
                                <ItemStyle CssClass="gridBorderColor" HorizontalAlign="Center" />
                            </asp:BoundField>
                            <asp:BoundField DataField="PercentualQuinta" HeaderText="Quinta">
                                <HeaderStyle BorderWidth="0px" CssClass="formRuler" />
                                <ItemStyle CssClass="gridBorderColor" HorizontalAlign="Center" />
                            </asp:BoundField>
                            <asp:BoundField DataField="PercentualSexta" HeaderText="Sexta">
                                <HeaderStyle BorderWidth="0px" CssClass="formRuler" />
                                <ItemStyle CssClass="gridBorderColor" HorizontalAlign="Center" />
                            </asp:BoundField>
                            <asp:BoundField DataField="PercentualSabado" HeaderText="Sábado">
                                <HeaderStyle BorderWidth="0px" CssClass="formRuler" />
                                <ItemStyle CssClass="gridBorderColor" HorizontalAlign="Center" />
                            </asp:BoundField>
                            <asp:BoundField DataField="DesDataInicial" HeaderText="Data inicial da vigência">
                                <HeaderStyle BorderWidth="0px" CssClass="formRuler" />
                                <ItemStyle CssClass="gridBorderColor" HorizontalAlign="Center" />
                            </asp:BoundField>
                            <asp:BoundField DataField="DesDataFinal" HeaderText="Data final da Vigência">
                                <HeaderStyle BorderWidth="0px" CssClass="formRuler" />
                                <ItemStyle CssClass="gridBorderColor" HorizontalAlign="Center" />
                            </asp:BoundField>
                            <asp:TemplateField HeaderText="Alterar">
                                <ItemTemplate>
                                    <asp:Button ID="btnAlterar" runat="server" OnClick="btnAlterar_Click" Text="Alterar" />
                                </ItemTemplate>
                                <HeaderStyle BorderWidth="0px" CssClass="formRuler" />
                                <ItemStyle CssClass="gridBorderColor" HorizontalAlign="Center" />
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
                </asp:Panel>
                <br />


                <table align="center" runat="server" id="TableCadastroDesconto" visible="false">
                    <tr>
                        <td class="auto-style2" align="right">Domingo</td>
                        <td class="auto-style2">
                            <asp:TextBox ID="txtPercenteDomingo" runat="server"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td align="right">Segunda</td>
                        <td>
                            <asp:TextBox ID="txtPercenteSegunda" runat="server"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td align="right">Terça-feira</td>
                        <td>
                            <asp:TextBox ID="txtPercenteTerca" runat="server"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td align="right">Quarta-feira</td>
                        <td>
                            <asp:TextBox ID="txtPercenteQuarta" runat="server"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td align="right">Quinta-feira</td>
                        <td>
                            <asp:TextBox ID="txtPercenteQuinta" runat="server"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td align="right">Sexta-feira</td>
                        <td>
                            <asp:TextBox ID="txtPercenteSexta" runat="server"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td align="right">Sábado</td>
                        <td>
                            <asp:TextBox ID="txtPercenteSabado" runat="server"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td align="right">Data inicial da vigência</td>
                        <td>
                            <asp:TextBox ID="txtDataIniciaDesconto" runat="server"></asp:TextBox>
                            <asp:DropDownList ID="cmbHoraDescIni" runat="server">
                                <asp:ListItem Value="00">00</asp:ListItem>
                                <asp:ListItem Value="01">01</asp:ListItem>
                                <asp:ListItem Value="02">02</asp:ListItem>
                                <asp:ListItem Value="03">03</asp:ListItem>
                                <asp:ListItem Value="04">04</asp:ListItem>
                                <asp:ListItem Value="05">05</asp:ListItem>
                                <asp:ListItem Value="06">06</asp:ListItem>
                                <asp:ListItem Value="07">07</asp:ListItem>
                                <asp:ListItem Value="08">08</asp:ListItem>
                                <asp:ListItem Value="09">09</asp:ListItem>
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
                        </td>
                    </tr>
                    <tr>
                        <td align="right">Data final da vigência</td>
                        <td>
                            <asp:TextBox ID="txtDataFinalDesconto" runat="server"></asp:TextBox>
                            <asp:DropDownList ID="cmbHoraDescFim" runat="server">
                                <asp:ListItem Value="00">00</asp:ListItem>
                                <asp:ListItem Value="01">01</asp:ListItem>
                                <asp:ListItem Value="02">02</asp:ListItem>
                                <asp:ListItem Value="03">03</asp:ListItem>
                                <asp:ListItem Value="04">04</asp:ListItem>
                                <asp:ListItem Value="05">05</asp:ListItem>
                                <asp:ListItem Value="06">06</asp:ListItem>
                                <asp:ListItem Value="07">07</asp:ListItem>
                                <asp:ListItem Value="08">08</asp:ListItem>
                                <asp:ListItem Value="09">09</asp:ListItem>
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
                        </td>
                    </tr>
                    <tr>
                        <td align="right" colspan="2">
                            <asp:Button ID="btnDescontoSalvar" runat="server" CssClass="imgGravar" Text="Salvar" />
                            <asp:Button ID="BtnDescontoVoltar" runat="server" CssClass="imgVoltar" Text="Voltar" />
                        </td>
                    </tr>
                </table>




            </asp:Panel>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>

