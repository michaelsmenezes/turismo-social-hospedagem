<%@ Page Title="Retirada de Caixa" Language="VB" MasterPageFile="~/TurismoSocial.master"
    AutoEventWireup="false" CodeFile="frmRetiradaDeCaixa.aspx.vb" Inherits="frmRetiradaDeCaixa" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <object id="Impressao" height="1" width="1" classid="clsid:8D6CC4E9-1AE1-4909-94AF-8A4CDC10C466">
    </object>

    <script type="text/javascript" language="javascript">
        function RetiradaDeCaixa() {
            texto = 'SESC - SERVIÇO SOCIAL DO COMÉRCIO' + eval("String.fromCharCode(0x0A)");
            texto += aspnetForm.ctl00_conPlaHolTurismoSocial_hddImpressao.value + String.fromCharCode(10);
            iRetorno = Impressao.imprimeSimples(texto);
        }
    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="conPlaHolTurismoSocial" runat="Server">
    <asp:UpdatePanel ID="updGeral" runat="server">
        <ContentTemplate>
            <br />
            <br />
            <br />
            <br />
            <br />
            <br />
            <br />
            <div align="center">
                <asp:Panel ID="pnlRetirada" runat="server" Width="60%" BorderStyle="None">
                    <asp:Panel ID="pnlCabecalho" runat="server">
                        <table>
                            <tr>
                                <td align="center" class="formRuler" colspan="11" style="font-weight: bold">
                                    RETIRADA DE CAIXA
                                </td>
                            </tr>
                            <tr>
                                <td colspan="11">
                                    Data:
                                    <asp:Label ID="lblData" runat="server" Text="Label"></asp:Label>
                                    &nbsp; &nbsp;
                                </td>
                            </tr>
                            <tr>
                                <td colspan="11">
                                    Operador:
                                    <asp:Label ID="lblOperador" runat="server" Text="Label"></asp:Label>
                                    &nbsp; &nbsp;
                                </td>
                            </tr>
                            <tr>
                                <td colspan="11">
                                    Caixa Nº:
                                    <asp:Label ID="lblNumeroCaixa" runat="server" Text="Label"></asp:Label>
                                    &nbsp; &nbsp;
                                </td>
                            </tr>
                            <tr>
                                <td colspan="11">
                                    &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;
                                </td>
                            </tr>
                            <tr>
                                <td align="center" class="formRuler" colspan="11" style="font-weight: bold">
                                    LANÇAMENTOS
                                </td>
                            </tr>
                            <tr>
                                <td align="left">
                                    <asp:Label ID="lblTipo" runat="server" Text="Tipo" Visible="False"></asp:Label>
                                </td>
                                <td align="left" colspan="1">
                                    <asp:Label ID="lblNrBanco" runat="server" Text="Nº Banco" Visible="False"></asp:Label>
                                </td>
                                <td align="left" colspan="1">
                                    <asp:Label ID="lblNrCheque" runat="server" Text="Nº Cheque" Visible="False"></asp:Label>
                                </td>
                                <td align="left">
                                    <asp:Label ID="lblDescCartao" runat="server" Text="Cartões" Visible="False"></asp:Label>
                                </td>
                                <td align="left">
                                    <asp:Label ID="lblNumeroCartao" runat="server" Text="N° do Cupom" Visible="False"></asp:Label>
                                </td>
                                <td>
                                    <asp:Label ID="lblQtde" runat="server" Text="Qtde" Visible="False"></asp:Label>
                                </td>
                                <td align="left">
                                    <asp:Label ID="lblValor" runat="server" Text="Valor" Visible="False"></asp:Label>
                                </td>
                                <td align="left">
                                    &nbsp;
                                </td>
                                <td align="left">
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
                                    <asp:DropDownList ID="drpTipo" runat="server" AutoPostBack="True" Visible="False">
                                        <asp:ListItem Value="0">Selecione...</asp:ListItem>
                                        <asp:ListItem Value="CE">Cédulas</asp:ListItem>
                                        <asp:ListItem Value="MO">Moedas</asp:ListItem>
                                        <asp:ListItem Value="CH">Cheques</asp:ListItem>
                                        <asp:ListItem Value="CA">Cartões</asp:ListItem>
                                    </asp:DropDownList>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtBanco" runat="server" MaxLength="6" Visible="False" Width="50px"></asp:TextBox>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtNrCheque" runat="server" MaxLength="12" Visible="False" Width="80px"></asp:TextBox>
                                </td>
                                <td>
                                    <asp:DropDownList ID="drpCartoes" runat="server" Visible="False">
                                        <asp:ListItem Value="0">Selecione...</asp:ListItem>
                                        <asp:ListItem Value="VE">Visa Eletro</asp:ListItem>
                                        <asp:ListItem Value="VN">Visa Net</asp:ListItem>
                                    </asp:DropDownList>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtNumeroCartao" runat="server" MaxLength="15" Visible="False" Width="80px"></asp:TextBox>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtQtde" runat="server" MaxLength="4" Width="30px" Visible="False"></asp:TextBox>
                                </td>
                                <td>
                                    <asp:DropDownList ID="drpValor" runat="server" Visible="False">
                                    </asp:DropDownList>
                                    <asp:TextBox ID="txtVlCheque" runat="server" Visible="False" Width="60px"></asp:TextBox>
                                </td>
                                <td>
                                    <asp:Button ID="btnSalvar" runat="server" CssClass="imgGravar" OnClientClick="if(confirm('Confirma operação?'))
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
                                };" Text="    Salvar" />
                                </td>
                                <td>
                                    <asp:Button ID="btnFechar" runat="server" CssClass="imgFechar" Text="  Fechar" />
                                </td>
                                <td>
                                    <asp:Button ID="btnImprimir" runat="server" CssClass="imgRelatorio" Text="   Imprimir"
                                        OnClientClick="return confirm('Confirma a Impressão?')" />
                                </td>
                                <td>
                                    <asp:ImageButton ID="btnAtualiza" runat="server" CssClass="ColocaHand" ImageUrl="~/images/Refresh.png"
                                        ToolTip="Atualzar" />
                                </td>
                            </tr>
                            <tr>
                                <td colspan="11">
                                    &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;
                                    <asp:Button ID="btnNova" runat="server" CssClass="imgNovo" Height="26px" Text="  Nova Retirada"
                                        Width="100%" />
                                </td>
                            </tr>
                        </table>
                    </asp:Panel>
                    <asp:Panel ID="pnlCabecalhoGerencial" runat="server">
                        <table style="width: 100%;">
                            <tr>
                                <td class="formRuler" colspan="2" style="font-weight: bold">
                                    RETIRADA DE CAIXA
                                </td>
                            </tr>
                            <tr>
                                <td align="center" colspan="2">
                                    <asp:Label ID="lblGerData" runat="server"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td align="center" colspan="2">
                                    Operador:<asp:Label ID="lblGerOperador" runat="server"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td align="char" colspan="2">
                                    Caixa N°:<asp:Label ID="lblGerCaixa" runat="server"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td align="right">
                                    &nbsp;
                                </td>
                                <td align="right">
                                    <asp:Button ID="btnGerImprimir" runat="server" CssClass="imgRelatorio" Text="   Imprimir" />
                                    <asp:Button ID="btnGerVoltar" runat="server" CssClass="imgVoltar" Text="    Voltar" />
                                </td>
                            </tr>
                        </table>
                    </asp:Panel>
                    <asp:Panel ID="pnlRetiradas" runat="server">
                        <table align="center" style="width: 100%;">
                            <tr>
                                <td align="center" class="formRuler" style="font-weight: bold" colspan="2">
                                    CÉDULAS
                                </td>
                            </tr>
                            <tr>
                                <td align="center" colspan="2">
                                    <asp:GridView ID="gdvCedulas" runat="server" AutoGenerateColumns="False" CellPadding="4"
                                        ForeColor="#333333" GridLines="None" Width="100%" DataKeyNames="rcxId,cxaQuantidade">
                                        <RowStyle BackColor="#EFF3FB" />
                                        <Columns>
                                            <asp:BoundField DataField="cxaQuantidade" HeaderText="Qtd">
                                                <HeaderStyle CssClass="formRuler" />
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="cxaValor" HeaderText="V.Unit.">
                                                <HeaderStyle CssClass="formRuler" />
                                                <ItemStyle HorizontalAlign="Right" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="cxaTotal" HeaderText="Total">
                                                <HeaderStyle CssClass="formRuler" />
                                                <ItemStyle HorizontalAlign="Right" />
                                            </asp:BoundField>
                                            <asp:TemplateField HeaderText="Alterar" ShowHeader="False">
                                                <ItemTemplate>
                                                    <asp:ImageButton ID="imgAlterar" runat="server" ImageUrl="~/images/editar.gif" OnClick="imgAlterar_Click"
                                                        OnClientClick="scroll(0,0)" />
                                                </ItemTemplate>
                                                <HeaderStyle CssClass="formRuler" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Apagar" ShowHeader="False">
                                                <ItemTemplate>
                                                    <asp:ImageButton ID="imgApagar" runat="server" ImageUrl="~/images/Delete.gif" OnClientClick="if(confirm('Confirma operação?'))
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
                                                    };" OnClick="imgApagar_Click" />
                                                </ItemTemplate>
                                                <HeaderStyle CssClass="formRuler" />
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
                                <td align="center" class="formRuler" style="font-weight: bold" colspan="2">
                                    MOEDAS
                                </td>
                            </tr>
                            <tr>
                                <td align="center" colspan="2">
                                    <asp:GridView ID="gdvMoeda" runat="server" AutoGenerateColumns="False" CellPadding="4"
                                        ForeColor="#333333" GridLines="None" Width="100%" DataKeyNames="rcxId">
                                        <RowStyle BackColor="#EFF3FB" />
                                        <Columns>
                                            <asp:BoundField DataField="cxaQuantidade" HeaderText="Qtd">
                                                <HeaderStyle CssClass="formRuler" />
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="cxaValor" HeaderText="V.Unit.">
                                                <HeaderStyle CssClass="formRuler" />
                                                <ItemStyle HorizontalAlign="Right" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="cxaTotal" HeaderText="Total">
                                                <HeaderStyle CssClass="formRuler" />
                                                <ItemStyle HorizontalAlign="Right" />
                                            </asp:BoundField>
                                            <asp:TemplateField HeaderText="Alterar">
                                                <ItemTemplate>
                                                    <asp:ImageButton ID="imgAlterar" runat="server" ImageUrl="~/images/editar.gif" OnClick="imgAlterar_Click1"
                                                        OnClientClick="scroll(0,0)" />
                                                </ItemTemplate>
                                                <HeaderStyle CssClass="formRuler" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Apagar">
                                                <ItemTemplate>
                                                    <asp:ImageButton ID="imgApagar" runat="server" ImageUrl="~/images/Delete.gif" OnClick="imgApagar_Click1" />
                                                </ItemTemplate>
                                                <HeaderStyle CssClass="formRuler" />
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
                                <td align="center" class="formRuler" style="font-weight: bold" colspan="2">
                                    CHEQUES&nbsp;
                                </td>
                            </tr>
                            <tr>
                                <td align="center" colspan="2">
                                    <asp:GridView ID="gdvCheque" runat="server" AutoGenerateColumns="False" CellPadding="4"
                                        ForeColor="#333333" GridLines="None" Width="100%" DataKeyNames="rcxId">
                                        <RowStyle BackColor="#EFF3FB" />
                                        <Columns>
                                            <asp:BoundField DataField="cxaNumeroBanco" HeaderText="Nº Banco">
                                                <HeaderStyle CssClass="formRuler" />
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="cxaNumeroCheque" HeaderText="Nº Cheque">
                                                <HeaderStyle CssClass="formRuler" />
                                                <ItemStyle HorizontalAlign="Left" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="cxaValor" HeaderText="Valor">
                                                <HeaderStyle CssClass="formRuler" />
                                                <ItemStyle HorizontalAlign="Right" />
                                            </asp:BoundField>
                                            <asp:TemplateField HeaderText="Alterar">
                                                <ItemTemplate>
                                                    <asp:ImageButton ID="imgAlterar" runat="server" ImageUrl="~/images/editar.gif" OnClick="imgAlterar_Click2"
                                                        OnClientClick="scroll(0,0)" />
                                                </ItemTemplate>
                                                <HeaderStyle CssClass="formRuler" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Apagar">
                                                <ItemTemplate>
                                                    <asp:ImageButton ID="imgApagar" runat="server" ImageUrl="~/images/Delete.gif" OnClick="imgApagar_Click2" />
                                                </ItemTemplate>
                                                <HeaderStyle CssClass="formRuler" />
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
                                <td align="center" class="formRuler" style="font-weight: bold" colspan="2">
                                    CARTÃO DE CRÉDITO/DÉBITO EM CONTA
                                </td>
                            </tr>
                            <tr>
                                <td align="center" colspan="2">
                                    <asp:GridView ID="gdvCartao" runat="server" AutoGenerateColumns="False" CellPadding="4"
                                        ForeColor="#333333" GridLines="None" Width="100%" DataKeyNames="rcxId">
                                        <RowStyle BackColor="#EFF3FB" />
                                        <Columns>
                                            <asp:BoundField DataField="cxaDescricaoCartao" HeaderText="Cartão">
                                                <HeaderStyle CssClass="formRuler" />
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="cxaNumeroCupom" HeaderText="Nº Cupom">
                                                <HeaderStyle CssClass="formRuler" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="cxaValor" HeaderText="Valor">
                                                <HeaderStyle CssClass="formRuler" />
                                                <ItemStyle HorizontalAlign="Right" />
                                            </asp:BoundField>
                                            <asp:TemplateField HeaderText="Alterar">
                                                <ItemTemplate>
                                                    <asp:ImageButton ID="imgAlterar" runat="server" ImageUrl="~/images/editar.gif" OnClick="imgAlterar_Click3"
                                                        OnClientClick="scroll(0,0)" />
                                                </ItemTemplate>
                                                <HeaderStyle CssClass="formRuler" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Apagar">
                                                <ItemTemplate>
                                                    <asp:ImageButton ID="imgApagar" runat="server" ImageUrl="~/images/Delete.gif" OnClick="imgApagar_Click3" />
                                                </ItemTemplate>
                                                <HeaderStyle CssClass="formRuler" />
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
                                <td align="center" class="formRuler" colspan="2" style="font-weight: bold">
                                    TOTAL GERAL DA RETIRADA
                                </td>
                            </tr>
                            <tr>
                                <td align="left">
                                    Dinheiro
                                </td>
                                <td align="right">
                                    <asp:Label ID="lblTotDinheiro" runat="server"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td align="left">
                                    Cheques
                                </td>
                                <td align="right">
                                    <asp:Label ID="lblTotCheques" runat="server"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td align="left">
                                    Cartões
                                </td>
                                <td align="right">
                                    <asp:Label ID="lblTotCartoes" runat="server"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td align="left">
                                    &nbsp;
                                </td>
                                <td align="right">
                                    &nbsp;
                                </td>
                            </tr>
                            <tr>
                                <td align="left" style="font-weight: bold">
                                    Soma Geral
                                </td>
                                <td align="right">
                                    <asp:Label ID="lblSomaGeral" runat="server" Font-Bold="True"></asp:Label>
                                </td>
                            </tr>
                        </table>
                        <br />
                    </asp:Panel>
                </asp:Panel>
                <asp:RoundedCornersExtender ID="pnlRetirada_RoundedCornersExtender" runat="server"
                    BorderColor="ActiveBorder" Color="ActiveBorder" Enabled="True" TargetControlID="pnlRetirada">
                </asp:RoundedCornersExtender>
                <asp:Panel ID="pnlGerencial" runat="server" Font-Bold="False" Width="60%">
                    <br />
                    <asp:Label ID="lblTituloConsulta" runat="server" Font-Bold="True" Font-Size="Medium"
                        Text="Consulta Retiradas de Caixa"></asp:Label>
                    <br />
                    <br />
                    Data Inicial:
                    <asp:TextBox ID="txtDataInicial" runat="server" Width="80px"></asp:TextBox>
                    <asp:MaskedEditExtender ID="txtDataInicial_MaskedEditExtender" runat="server" CultureAMPMPlaceholder=""
                        CultureCurrencySymbolPlaceholder="" CultureDateFormat="" CultureDatePlaceholder=""
                        CultureDecimalPlaceholder="" CultureThousandsPlaceholder="" CultureTimePlaceholder=""
                        Enabled="True" Mask="99/99/9999" MaskType="Date" TargetControlID="txtDataInicial">
                    </asp:MaskedEditExtender>
                    <asp:CalendarExtender ID="txtDataInicial_CalendarExtender" runat="server" Enabled="True"
                        FirstDayOfWeek="Sunday" Format="dd/MM/yyyy" TargetControlID="txtDataInicial">
                    </asp:CalendarExtender>
                    &nbsp;Data Final:
                    <asp:TextBox ID="txtDataFinal" runat="server" Width="80px"></asp:TextBox>
                    <asp:MaskedEditExtender ID="txtDataFinal_MaskedEditExtender" runat="server" CultureAMPMPlaceholder=""
                        CultureCurrencySymbolPlaceholder="" CultureDateFormat="" CultureDatePlaceholder=""
                        CultureDecimalPlaceholder="" CultureThousandsPlaceholder="" CultureTimePlaceholder=""
                        Enabled="True" Mask="99/99/9999" MaskType="Date" TargetControlID="txtDataFinal">
                    </asp:MaskedEditExtender>
                    <asp:CalendarExtender ID="txtDataFinal_CalendarExtender" runat="server" Enabled="True"
                        FirstDayOfWeek="Sunday" Format="dd/MM/yyyy" TargetControlID="txtDataFinal">
                    </asp:CalendarExtender>
                    &nbsp;<asp:Button ID="btnConsultar" runat="server" CssClass="imgLupa" Text="  Consultar" />
                    <br />
                    <br />
                    <asp:GridView ID="gdvListaCaixas" runat="server" AutoGenerateColumns="False" CellPadding="4"
                        DataKeyNames="cxaCrtCod,cxaCrtOpr,rcxData" ForeColor="#333333" GridLines="None">
                        <RowStyle BackColor="#EFF3FB" />
                        <Columns>
                            <asp:ButtonField CommandName="select" DataTextField="cxaCrtCod" HeaderText="Caixa"
                                Text="Button">
                                <HeaderStyle CssClass="formRuler" />
                            </asp:ButtonField>
                            <asp:BoundField DataField="cxacrtopr" HeaderText="Operador">
                                <HeaderStyle CssClass="formRuler" />
                                <ItemStyle HorizontalAlign="Left" />
                            </asp:BoundField>
                            <asp:BoundField DataField="CxaValorOperacoes" HeaderText="R$ Opr.Caixa">
                                <HeaderStyle CssClass="formRuler" />
                                <ItemStyle HorizontalAlign="Right" />
                            </asp:BoundField>
                            <asp:BoundField DataField="cxaValor" HeaderText="R$ Retiradas">
                                <HeaderStyle CssClass="formRuler" />
                                <ItemStyle HorizontalAlign="Right" />
                            </asp:BoundField>
                            <asp:TemplateField HeaderText="Visualiza Retiradas">
                                <ItemTemplate>
                                    <asp:GridView ID="gdvListaRetiradas" runat="server" AutoGenerateColumns="False" CellPadding="4"
                                        ForeColor="#333333" GridLines="None" OnRowDataBound="gdvListaRetiradas_RowDataBound"
                                        OnSelectedIndexChanged="gdvListaRetiradas_SelectedIndexChanged">
                                        <RowStyle BackColor="#EFF3FB" />
                                        <Columns>
                                            <asp:BoundField HeaderText="Retirada" DataField="cbrId">
                                                <HeaderStyle CssClass="formRuler" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="rcxData" HeaderText="Data">
                                                <HeaderStyle CssClass="formRuler" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="cxaValor" HeaderText="Valor">
                                                <HeaderStyle CssClass="formRuler" />
                                                <ItemStyle HorizontalAlign="Justify" />
                                            </asp:BoundField>
                                            <asp:TemplateField HeaderText="Retirada" ShowHeader="False">
                                                <ItemTemplate>
                                                    <asp:ImageButton ID="imgAlterar" runat="server" ImageUrl="~/images/lupapeq.png" OnClick="imgAlterar_Click4" />
                                                </ItemTemplate>
                                                <HeaderStyle CssClass="formRuler" />
                                            </asp:TemplateField>
                                        </Columns>
                                        <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                                        <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
                                        <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                                        <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                                        <EditRowStyle BackColor="#2461BF" />
                                        <AlternatingRowStyle BackColor="White" />
                                    </asp:GridView>
                                </ItemTemplate>
                                <HeaderStyle CssClass="formRuler" />
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
                <asp:RoundedCornersExtender ID="pnlGerencial_RoundedCornersExtender" runat="server"
                    BorderColor="ActiveBorder" Color="ActiveBorder" Enabled="True" TargetControlID="pnlGerencial">
                </asp:RoundedCornersExtender>
                <br />
                <br />
            </div>
            <asp:HiddenField ID="hddProcessando" runat="server" />
            <asp:HiddenField ID="hddImpressao" runat="server" />
            <br />
        </ContentTemplate>
    </asp:UpdatePanel>
    <link href="stylesheet.css" rel="stylesheet" type="text/css" />
    </p>
</asp:Content>
