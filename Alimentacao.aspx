<%@ Page Title="" Language="VB" MasterPageFile="~/TurismoSocial.master" AutoEventWireup="True"
    CodeFile="Alimentacao.aspx.vb" Inherits="Alimentacao" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="conPlaHolTurismoSocial" runat="Server">
    <asp:UpdatePanel ID="updPnlAlimentacao" runat="server">
        <ContentTemplate>
            <br />
            <br />
            <br />
            <br />
            <br />
            <br />
            <br />
            <asp:Panel ID="pnlAlimentacao" runat="server" CssClass="formLabel" BorderColor="White"
                BorderWidth="10px">
                <div class="formLabel" align="center">
                    <asp:UpdateProgress ID="updProRecepcao" runat="server" AssociatedUpdatePanelID="updPnlAlimentacao">
                        <ProgressTemplate>
                            Processando sua solicitação...<br />
                            &nbsp;<img alt="Processando..." src="images/Aguarde.gif" />
                        </ProgressTemplate>
                    </asp:UpdateProgress>
                </div>
                <asp:Panel ID="pnlPrevisao" runat="server" Visible="false">
                    <asp:Label ID="lblPrevisao" runat="server" Text="Previsão do Restaurante" Font-Size="Medium"></asp:Label>
                    <br />
                    <br />
                    <asp:Label ID="lblDiaPrevisao" runat="server" Text="Dia"></asp:Label>
                    <asp:RequiredFieldValidator ID="rqvSolicitacaoDataInicia" runat="server" ControlToValidate="txtDataPrevisao"
                        ErrorMessage="Informe a data." ValidationGroup="ValidacaoPrevisao" SetFocusOnError="True"
                        Display="Dynamic">*</asp:RequiredFieldValidator>
                    <asp:MaskedEditValidator ID="mevSolicitacaoDataInicial" runat="server" ControlExtender="txtDataPrevisao_MaskedEditExtender"
                        ControlToValidate="txtDataPrevisao" Display="Dynamic" SetFocusOnError="True"
                        ValidationGroup="ValidacaoPrevisao" InvalidValueBlurredMessage="*" InvalidValueMessage="Data inválida.">*</asp:MaskedEditValidator>
                    <asp:TextBox ID="txtDataPrevisao" runat="server" Width="70px"></asp:TextBox>
                    <asp:CalendarExtender ID="txtDataPrevisao_CalendarExtender" runat="server" Enabled="True"
                        TargetControlID="txtDataPrevisao">
                    </asp:CalendarExtender>
                    <asp:MaskedEditExtender ID="txtDataPrevisao_MaskedEditExtender" runat="server" CultureAMPMPlaceholder=""
                        CultureCurrencySymbolPlaceholder="" CultureDatePlaceholder="" CultureDecimalPlaceholder=""
                        CultureThousandsPlaceholder="" CultureTimePlaceholder="" Enabled="True" Mask="99/99/9999"
                        MaskType="Date" TargetControlID="txtDataPrevisao">
                    </asp:MaskedEditExtender>
                    <asp:Button ID="bntConsultarPrevisao" runat="server" CssClass="imgLupa" Text="  Consultar"
                        ValidationGroup="ValidacaoPrevisao" />
                    <asp:ValidationSummary ID="vlsSolicitacao" runat="server" ShowMessageBox="True" ShowSummary="False"
                        ValidationGroup="ValidacaoPrevisao" DisplayMode="List" />
                    <br />
                    <br />
                    <asp:GridView ID="gdvPrevisao" runat="server" AutoGenerateColumns="False" CellPadding="4"
                        HorizontalAlign="Center" Width="100%" DataKeyNames="RefeicaoTipo,DesjejumRefeicaoConfirmado,Estada,DesjejumRefeicao">
                        <AlternatingRowStyle CssClass="tableRowOdd" />
                        <RowStyle HorizontalAlign="Center" />
                        <Columns>
                            <asp:TemplateField HeaderText="Refeição">
                                <ItemTemplate>
                                    <asp:LinkButton ID="lnkRefeicao" runat="server" Text='<%# Bind("RefeicaoTipo") %>'></asp:LinkButton>
                                </ItemTemplate>
                                <FooterStyle CssClass="formLabel" HorizontalAlign="Right" />
                                <HeaderStyle BorderWidth="0px" CssClass="formRuler" />
                                <ItemStyle CssClass="gridBorderColor" HorizontalAlign="Left" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Previsão Total">
                                <ItemTemplate>
                                    <asp:LinkButton ID="lnkPrevisao" runat="server" Text='<%# Bind("DesjejumRefeicao") %>'></asp:LinkButton>
                                </ItemTemplate>
                                <FooterStyle CssClass="formLabel" HorizontalAlign="Center" />
                                <HeaderStyle BorderWidth="0px" CssClass="formRuler" />
                                <ItemStyle CssClass="gridBorderColor" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Em Estada">
                                <ItemTemplate>
                                    <asp:LinkButton ID="lnkEstada" runat="server" Text='<%# Bind("Estada") %>'></asp:LinkButton>
                                </ItemTemplate>
                                <HeaderStyle BorderWidth="0px" CssClass="formRuler" />
                                <ItemStyle CssClass="gridBorderColor" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Já Confirmado">
                                <ItemTemplate>
                                    <asp:LinkButton ID="lnkPago" runat="server"></asp:LinkButton>
                                </ItemTemplate>
                                <FooterStyle CssClass="formLabel" HorizontalAlign="Center" />
                                <HeaderStyle BorderWidth="0px" CssClass="formRuler" />
                                <ItemStyle CssClass="gridBorderColor" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="A Pagar">
                                <ItemTemplate>
                                    <asp:LinkButton ID="lnkAPagar" runat="server"></asp:LinkButton>
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
                    <asp:GridView ID="gdvPrevisaoPratoRapido" runat="server" AutoGenerateColumns="False"
                        CellPadding="4" HorizontalAlign="Center" Width="100%">
                        <AlternatingRowStyle CssClass="tableRowOdd" />
                        <RowStyle HorizontalAlign="Center" />
                        <Columns>
                            <asp:TemplateField HeaderText="Prato Rápido">
                                <ItemTemplate>
                                    <asp:LinkButton ID="lnkDescricao" runat="server" Text='<%# Bind("descricao") %>'></asp:LinkButton>
                                </ItemTemplate>
                                <FooterStyle CssClass="formLabel" HorizontalAlign="Right" />
                                <HeaderStyle BorderWidth="0px" CssClass="formRuler" />
                                <ItemStyle CssClass="gridBorderColor" HorizontalAlign="Left" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Solicitado">
                                <ItemTemplate>
                                    <asp:LinkButton ID="lnkSolicitado" runat="server" Text='<%# Bind("solicitado") %>'></asp:LinkButton>
                                </ItemTemplate>
                                <FooterStyle CssClass="formLabel" HorizontalAlign="Center" />
                                <HeaderStyle BorderWidth="0px" CssClass="formRuler" />
                                <ItemStyle CssClass="gridBorderColor" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Nao Pago">
                                <ItemTemplate>
                                    <asp:LinkButton ID="lnkNaoPago" runat="server" Text='<%# Bind("naoPago") %>'></asp:LinkButton>
                                </ItemTemplate>
                                <HeaderStyle BorderWidth="0px" CssClass="formRuler" />
                                <ItemStyle CssClass="gridBorderColor" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Confirmado">
                                <ItemTemplate>
                                    <asp:LinkButton ID="lnkConfirmado" runat="server" Text='<%# Bind("Confirmado") %>'></asp:LinkButton>
                                </ItemTemplate>
                                <HeaderStyle BorderWidth="0px" CssClass="formRuler" />
                                <ItemStyle CssClass="gridBorderColor" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Consumido">
                                <ItemTemplate>
                                    <asp:LinkButton ID="lnkConsumido" runat="server" Text='<%# Bind("Consumido") %>'></asp:LinkButton>
                                </ItemTemplate>
                                <HeaderStyle BorderWidth="0px" CssClass="formRuler" />
                                <ItemStyle CssClass="gridBorderColor" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="A Consumir">
                                <ItemTemplate>
                                    <asp:LinkButton ID="lnkConsumir" runat="server" Text='<%# Bind("Consumir") %>'></asp:LinkButton>
                                </ItemTemplate>
                                <HeaderStyle BorderWidth="0px" CssClass="formRuler" />
                                <ItemStyle CssClass="gridBorderColor" />
                            </asp:TemplateField>
                        </Columns>
                        <HeaderStyle ForeColor="White" HorizontalAlign="Center" />
                        <SelectedRowStyle Font-Bold="True" />
                    </asp:GridView>
                    <br />
                    <asp:GridView ID="gdvPrevisaoPassanteRestaurante" runat="server" AutoGenerateColumns="False"
                        CellPadding="4" HorizontalAlign="Center" Width="100%">
                        <AlternatingRowStyle CssClass="tableRowOdd" />
                        <RowStyle HorizontalAlign="Center" />
                        <Columns>
                            <asp:TemplateField HeaderText="Passante no Restaurante">
                                <ItemTemplate>
                                    <asp:LinkButton ID="lnkConfirmado" runat="server" Text='<%# Bind("Confirmado") %>'></asp:LinkButton>
                                </ItemTemplate>
                                <HeaderStyle BorderWidth="0px" CssClass="formRuler" />
                                <ItemStyle CssClass="gridBorderColor" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Consumido">
                                <ItemTemplate>
                                    <asp:LinkButton ID="lnkConsumido" runat="server" Text='<%# Bind("Consumido") %>'></asp:LinkButton>
                                </ItemTemplate>
                                <HeaderStyle BorderWidth="0px" CssClass="formRuler" />
                                <ItemStyle CssClass="gridBorderColor" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="A Consumir">
                                <ItemTemplate>
                                    <asp:LinkButton ID="lnkConsumir" runat="server" Text='<%# Bind("Consumir") %>'></asp:LinkButton>
                                </ItemTemplate>
                                <HeaderStyle BorderWidth="0px" CssClass="formRuler" />
                                <ItemStyle CssClass="gridBorderColor" />
                            </asp:TemplateField>
                        </Columns>
                        <HeaderStyle ForeColor="White" HorizontalAlign="Center" />
                        <SelectedRowStyle Font-Bold="True" />
                    </asp:GridView>
                </asp:Panel>
                <asp:Panel ID="pnlMeiaDiaria" runat="server" Visible="false">
                    <asp:Label ID="lblMeiaDiaria" runat="server" Text="Meia Diária sem Pernoite" Font-Size="Medium"></asp:Label>
                </asp:Panel>
                <asp:Panel ID="pnlRefeicaoManual" runat="server" Visible="false">
                    <asp:Label ID="lblRefeicaoManual" runat="server" Text="Registro Manual de Refeição"
                        Font-Size="Medium"></asp:Label>
                    <br />
                    <br />
                    <asp:Label ID="lblPeriodoManual" runat="server" Text="Período"></asp:Label>
                    <asp:RequiredFieldValidator ID="rqvDataManualIni" runat="server" ControlToValidate="txtDataManualIni"
                        Display="Dynamic" ErrorMessage="Informe a data." SetFocusOnError="True" ValidationGroup="ValidacaoManual">*</asp:RequiredFieldValidator>
                    <asp:MaskedEditValidator ID="mevDataManualIni" runat="server" ControlExtender="txtDataManualIni_MaskedEditExtender"
                        ControlToValidate="txtDataManualIni" Display="Dynamic" InvalidValueBlurredMessage="*"
                        InvalidValueMessage="Data inválida." SetFocusOnError="True" ValidationGroup="ValidacaoManual">*</asp:MaskedEditValidator>
                    <asp:TextBox ID="txtDataManualIni" runat="server" Width="70px"></asp:TextBox>
                    <asp:Label ID="lblPeriodoManualA" runat="server" Text="a"></asp:Label>
                    <asp:RequiredFieldValidator ID="rqvDataManualFim" runat="server" ControlToValidate="txtDataManualFim"
                        Display="Dynamic" ErrorMessage="Informe a data." SetFocusOnError="True" ValidationGroup="ValidacaoManual">*</asp:RequiredFieldValidator>
                    <asp:MaskedEditValidator ID="mevDataManualFim" runat="server" ControlExtender="txtDataManualFim_MaskedEditExtender"
                        ControlToValidate="txtDataManualFim" Display="Dynamic" InvalidValueBlurredMessage="*"
                        InvalidValueMessage="Data inválida." SetFocusOnError="True" ValidationGroup="ValidacaoManual">*</asp:MaskedEditValidator>
                    <asp:CalendarExtender ID="txtDataManualIni_CalendarExtender" runat="server" Enabled="True"
                        TargetControlID="txtDataManualIni">
                    </asp:CalendarExtender>
                    <asp:MaskedEditExtender ID="txtDataManualIni_MaskedEditExtender" runat="server" CultureAMPMPlaceholder=""
                        CultureCurrencySymbolPlaceholder="" CultureDatePlaceholder="" CultureDecimalPlaceholder=""
                        CultureThousandsPlaceholder="" CultureTimePlaceholder="" Enabled="True" Mask="99/99/9999"
                        MaskType="Date" TargetControlID="txtDataManualIni">
                    </asp:MaskedEditExtender>
                    <asp:TextBox ID="txtDataManualFim" runat="server" Width="70px"></asp:TextBox>
                    <asp:CompareValidator ID="cpvData" runat="server" ControlToCompare="txtDataManualIni"
                        ControlToValidate="txtDataManualFim" Display="Dynamic" ErrorMessage="A 1ª data deve ser menor ou igual a 2ª data."
                        Operator="GreaterThanEqual" SetFocusOnError="True" Type="Date" ValidationGroup="ValidacaoManual">*</asp:CompareValidator>
                    <asp:Button ID="bntConsultarManual" runat="server" CssClass="imgLupa" Text="  Consultar"
                        ValidationGroup="ValidacaoManual" />
                    <asp:Panel ID="pnlRefeicaoManualEdit" runat="server">
                        <br />
                        <asp:Label ID="lblData" runat="server" Text="Data"></asp:Label>
                        <asp:RequiredFieldValidator ID="rqvData" runat="server" ControlToValidate="txtData"
                            Display="Dynamic" ErrorMessage="Informe a data." SetFocusOnError="True" ValidationGroup="ValidacaoManualEdit">*</asp:RequiredFieldValidator>
                        <asp:MaskedEditValidator ID="mevData" runat="server" ControlExtender="txtData_MaskedEditExtender"
                            ControlToValidate="txtData" Display="Dynamic" ErrorMessage="mevData" InvalidValueBlurredMessage="*"
                            InvalidValueMessage="Data inválida." SetFocusOnError="True" ValidationGroup="ValidacaoManualEdit">*</asp:MaskedEditValidator>
                        <asp:TextBox ID="txtData" runat="server" Width="70px"></asp:TextBox>
                        <asp:MaskedEditExtender ID="txtData_MaskedEditExtender" runat="server" CultureAMPMPlaceholder=""
                            CultureCurrencySymbolPlaceholder="" CultureDateFormat="" CultureDatePlaceholder=""
                            CultureDecimalPlaceholder="" CultureThousandsPlaceholder="" CultureTimePlaceholder=""
                            Enabled="True" Mask="99/99/9999" MaskType="Date" TargetControlID="txtData">
                        </asp:MaskedEditExtender>
                        <asp:CalendarExtender ID="txtData_CalendarExtender" runat="server" Enabled="True"
                            TargetControlID="txtData">
                        </asp:CalendarExtender>
                        <asp:Label ID="lblRefeicao" runat="server" Text="Refeição"></asp:Label>
                        <asp:RadioButtonList ID="rblRefeicao" runat="server" RepeatDirection="Horizontal"
                            RepeatLayout="Flow">
                            <asp:ListItem Value="D">Desjejum</asp:ListItem>
                            <asp:ListItem Value="A">Almoço</asp:ListItem>
                            <asp:ListItem Value="J">Jantar</asp:ListItem>
                        </asp:RadioButtonList>
                        &nbsp;<asp:Label ID="lblQtde" runat="server" Text="Quantidade"></asp:Label>
                        <asp:RequiredFieldValidator ID="rfvQtde" runat="server" ControlToValidate="txtQtde"
                            Display="Dynamic" ErrorMessage="Informe a quantidade." SetFocusOnError="True"
                            ValidationGroup="ValidacaoManualEdit">*</asp:RequiredFieldValidator>
                        <asp:RangeValidator ID="rgvQtde" runat="server" ControlToValidate="txtQtde" Display="Dynamic"
                            ErrorMessage="Informe a quantidade de 1 até 999." MaximumValue="999" MinimumValue="1"
                            SetFocusOnError="True" Type="Integer" ValidationGroup="ValidacaoManualEdit">*</asp:RangeValidator>
                        <asp:TextBox ID="txtQtde" runat="server" MaxLength="3" Width="50px"></asp:TextBox>
                        <asp:MaskedEditExtender ID="txtQtde_MaskedEditExtender" runat="server" AutoComplete="False"
                            CultureAMPMPlaceholder="" CultureCurrencySymbolPlaceholder="" CultureDateFormat=""
                            CultureDatePlaceholder="" CultureDecimalPlaceholder="" CultureThousandsPlaceholder=""
                            CultureTimePlaceholder="" Enabled="True" Mask="999" MaskType="Number" TargetControlID="txtQtde">
                        </asp:MaskedEditExtender>
                        <asp:Button ID="btnRefeicaoNovo" runat="server" AccessKey="N" CausesValidation="False"
                            CssClass="imgNovo" Text="     Novo" ToolTip="Alt+N" />
                        <asp:Button ID="btnRefeicaoGravar" runat="server" AccessKey="S" CssClass="imgGravar"
                            Text="     Salvar" ToolTip="Alt+S" ValidationGroup="ValidacaoManualEdit" />
                        <asp:Button ID="btnRefeicaoExcluir" runat="server" AccessKey="E" CssClass="imgExcluir"
                            Text="     Excluir" ToolTip="Alt+E" />
                        <asp:ConfirmButtonExtender ID="btnRefeicaoExcluir_ConfirmButtonExtender" runat="server"
                            ConfirmText="Confirma exclusão?" Enabled="True" TargetControlID="btnRefeicaoExcluir">
                        </asp:ConfirmButtonExtender>
                        <asp:ValidationSummary ID="vlsManualEdit" runat="server" DisplayMode="List" ShowMessageBox="True"
                            ShowSummary="False" ValidationGroup="ValidacaoManualEdit" />
                        <asp:HiddenField ID="hddRefId" runat="server" Value="0" />
                        <br />
                    </asp:Panel>
                    <asp:GridView ID="gdvManual" runat="server" AutoGenerateColumns="False" CellPadding="4"
                        DataKeyNames="refId,refTipo">
                        <AlternatingRowStyle CssClass="tableRowOdd" />
                        <RowStyle HorizontalAlign="Center" />
                        <Columns>
                            <asp:TemplateField HeaderText="Data">
                                <ItemTemplate>
                                    <asp:LinkButton ID="lnkDataRef" runat="server" Text='<%# Bind("refData") %>' CommandName="select"></asp:LinkButton>
                                </ItemTemplate>
                                <HeaderStyle BorderWidth="0px" CssClass="formRuler" />
                                <ItemStyle CssClass="gridBorderColor" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Refeição">
                                <ItemTemplate>
                                    <asp:LinkButton ID="lnkRefeicao" runat="server" Text='<%# Bind("refTipo") %>' CommandName="select"></asp:LinkButton>
                                </ItemTemplate>
                                <HeaderStyle BorderWidth="0px" CssClass="formRuler" />
                                <ItemStyle CssClass="gridBorderColor" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Qtde">
                                <ItemTemplate>
                                    <asp:LinkButton ID="lnkQtde" runat="server" Text='<%# Bind("refQtde") %>' CommandName="select"></asp:LinkButton>
                                </ItemTemplate>
                                <HeaderStyle BorderWidth="0px" CssClass="formRuler" />
                                <ItemStyle CssClass="gridBorderColor" />
                            </asp:TemplateField>
                        </Columns>
                        <HeaderStyle ForeColor="White" HorizontalAlign="Center" />
                        <SelectedRowStyle Font-Bold="True" />
                    </asp:GridView>
                    <asp:CalendarExtender ID="txtDataManualFim_CalendarExtender" runat="server" Enabled="True"
                        TargetControlID="txtDataManualFim">
                    </asp:CalendarExtender>
                    <asp:MaskedEditExtender ID="txtDataManualFim_MaskedEditExtender" runat="server" CultureAMPMPlaceholder=""
                        CultureCurrencySymbolPlaceholder="" CultureDatePlaceholder="" CultureDecimalPlaceholder=""
                        CultureThousandsPlaceholder="" CultureTimePlaceholder="" Enabled="True" Mask="99/99/9999"
                        MaskType="Date" TargetControlID="txtDataManualFim">
                    </asp:MaskedEditExtender>
                    <asp:ValidationSummary ID="vlsManual" runat="server" DisplayMode="List" ShowMessageBox="True"
                        ShowSummary="False" ValidationGroup="ValidacaoManual" />
                </asp:Panel>
                <asp:Panel ID="pnlConsumo" runat="server" Visible="false">
                    <asp:Label ID="lblConsumo" runat="server" Text="Consumo Restaurante" Font-Size="Medium"></asp:Label>
                    <br />
                </asp:Panel>
                <asp:Panel ID="pnlHistorico" runat="server" Visible="false">
                    <asp:Label ID="lblHistorico" runat="server" Text="Histórico de Refeição" Font-Size="Medium"></asp:Label>
                    <br />
                </asp:Panel>
                <asp:Panel ID="pnlHistoricoTipo" runat="server" Visible="false">
                    <asp:Label ID="lblHistoricoTipo" runat="server" Text="Histórico Refeição por Tipo"
                        Font-Size="Medium"></asp:Label>
                    <br />
                </asp:Panel>
                <asp:Panel ID="pnlMenu" runat="server">
                    <asp:Label ID="lblIrPara" runat="server" Text="Ir para"></asp:Label>
                    <p>
                    </p>
                    <asp:LinkButton ID="lnkPrevisao" runat="server">Previsão do Restaurante</asp:LinkButton>
                    <p>
                    </p>
                    <asp:LinkButton ID="lnkMeiaDiaria" runat="server">Meia Diária sem Pernoite</asp:LinkButton>
                    <p>
                    </p>
                    <asp:LinkButton ID="lnkRefeicaoManual" runat="server">Registro Manual de Refeição</asp:LinkButton>
                    <p>
                    </p>
                    <asp:LinkButton ID="lnkConsumo" runat="server">Consumo Restaurante</asp:LinkButton>
                    <p>
                    </p>
                    <asp:LinkButton ID="lnkHistorico" runat="server">Histórico de Refeição</asp:LinkButton>
                    <p>
                    </p>
                    <asp:LinkButton ID="lnkHistoricoTipo" runat="server">Histórico Refeição por Tipo</asp:LinkButton>
                </asp:Panel>
                <asp:AlwaysVisibleControlExtender ID="pnlMenu_AlwaysVisibleControlExtender" runat="server"
                    Enabled="True" HorizontalSide="Right" TargetControlID="pnlMenu" VerticalSide="Middle">
                </asp:AlwaysVisibleControlExtender>
            </asp:Panel>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
