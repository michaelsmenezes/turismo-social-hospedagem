<%@ Page Title="Cadastro de Alas" Language="VB" MasterPageFile="~/TurismoSocial.master" AutoEventWireup="false" CodeFile="frmCadastroAla.aspx.vb" Inherits="frmCadastroAla" %>

<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="asp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="conPlaHolTurismoSocial" Runat="Server">
    <p>
        &nbsp;</p>
    <p>
        &nbsp;</p>
    <asp:UpdatePanel ID="updGeral" runat="server">
        <ContentTemplate>
            <br />
            <br />
            <asp:Panel ID="pnlConsultaAla" runat="server">
                <table>
                    <tr>
                        <td>
                            Nome da ala</td>
                        <td>
                            &nbsp;</td>
                        <td>
                            &nbsp;</td>
                        <td>
                            &nbsp;</td>
                    </tr>
                    <tr>
                        <td>
                            <asp:TextBox ID="txtNomeAla" runat="server"></asp:TextBox>
                        </td>
                        <td>
                            <asp:Button ID="btnPesquisarAla" runat="server" CssClass="imgLupa" 
                                Text="  Consultar" />
                        </td>
                        <td>
                            <asp:Button ID="btnInserirAla" runat="server" CssClass="imgNovo" 
                                Text="    Nova" />
                        </td>
                        <td>
                            <asp:Button ID="btnVoltarGov" runat="server" CssClass="imgVoltar" 
                                Text="    Voltar" />
                        </td>
                    </tr>
                    <tr>
                        <td style="display: none">
                            <asp:Label ID="lblAuxiliarInsercao" runat="server" Text="Label"></asp:Label>
                        </td>
                        <td>
                            &nbsp;</td>
                        <td>
                            &nbsp;</td>
                        <td style="display: none">
                            <asp:Button ID="btnAuxExtender" runat="server" Text="AuxExtender" />
                            <asp:ModalPopupExtender ID="btnAuxExtender_ModalPopupExtender" runat="server" 
                                DynamicServicePath="" Enabled="True" PopupControlID="pnlCadastroCam" 
                                TargetControlID="btnAuxExtender" BackgroundCssClass="modalBackground" 
                                CancelControlID="btnVoltarCad" PopupDragHandleControlID="btnSalvarCad">
                            </asp:ModalPopupExtender>
                        </td>
                    </tr>
                </table>
                <div align="center" class="PosicionaProgresso">
                    <asp:UpdateProgress ID="upgProgressGeral" runat="server" 
                        AssociatedUpdatePanelID="updGeral">
                        <ProgressTemplate>
                            <asp:Image ID="imgProgresso" runat="server" ImageUrl="~/images/Aguarde.gif" 
                                Width="33px" />
                            <br />
                            <asp:Label ID="lblAguarde0" runat="server" Font-Bold="True" Font-Size="Small" 
                                ForeColor="#99CCFF" Text="Processando..."></asp:Label>
                        </ProgressTemplate>
                    </asp:UpdateProgress>
                </div>
            </asp:Panel>
            <asp:RoundedCornersExtender ID="pnlConsultaAla_RoundedCornersExtender" 
                runat="server" Enabled="True" TargetControlID="pnlConsultaAla">
            </asp:RoundedCornersExtender>
            <br />
            <br />
            <asp:Panel ID="pnlGridAlas" runat="server">
                <asp:GridView ID="GdvListaAla" runat="server" AutoGenerateColumns="False" 
                    DataKeyNames="AlaId,AlaNome,AlaDescricao,CamId">
                    <Columns>
                        <asp:ButtonField CommandName="Select" DataTextField="AlaNome" HeaderText="Ala" 
                            Text="Text">
                        <HeaderStyle CssClass="formRuler" />
                        </asp:ButtonField>
                        <asp:BoundField HeaderText="Descrição" DataField="AlaDescricao">
                        <HeaderStyle CssClass="formRuler" />
                        </asp:BoundField>
                        <asp:BoundField DataField="CamNome" HeaderText="Camareira">
                        <HeaderStyle CssClass="formRuler" />
                        </asp:BoundField>
                        <asp:TemplateField HeaderText="Apartamentos">
                            <ItemTemplate>
                                <asp:ImageButton ID="imgAptoAla" runat="server" 
                                    ImageUrl="~/images/Distribuir.png" 
                                    ToolTip="Atribuir apartamentos para a ala" onclick="imgAptoAla_Click" />
                            </ItemTemplate>
                            <HeaderStyle CssClass="formRuler" />
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </asp:Panel>
            <asp:Panel ID="pnlCadastroAla" runat="server" Visible="False">
                <table>
                    <tr>
                        <td class="formRuler" colspan="6">
                            Cadastro de alas</td>
                    </tr>
                    <tr>
                        <td>
                            Nome</td>
                        <td>
                            Descrição</td>
                        <td>
                            Camareira</td>
                        <td>
                            &nbsp;</td>
                        <td>
                            &nbsp;</td>
                        <td>
                            &nbsp;</td>
                    </tr>
                    <tr>
                        <td>
                            <asp:TextBox ID="txtCadNome" runat="server" MaxLength="3" Width="50px"></asp:TextBox>
                        </td>
                        <td>
                            <asp:TextBox ID="txtCadDescricao" runat="server" MaxLength="50" Width="230px"></asp:TextBox>
                        </td>
                        <td>
                            <asp:DropDownList ID="drpCadCamareira" runat="server">
                            </asp:DropDownList>
                        </td>
                        <td>
                            <asp:ImageButton ID="btnCamareira" runat="server" 
                                ImageUrl="~/images/Camareira.png" ToolTip="Cadastrar camareira" />
                            <asp:ModalPopupExtender ID="btnCamareira_ModalPopupExtender" runat="server" 
                                BackgroundCssClass="modalBackground" CancelControlID="btnVoltarCad" 
                                DynamicServicePath="" Enabled="True" PopupControlID="pnlCadastroCam" 
                                PopupDragHandleControlID="btnSalvarCad" TargetControlID="btnCamareira">
                            </asp:ModalPopupExtender>
                        </td>
                        <td>
                            <asp:Button ID="btnSalvarAla" runat="server" CssClass="imgGravar" 
                                Text="   Salvar" />
                        </td>
                        <td>
                            <asp:Button ID="btnSair" runat="server" CssClass="imgVoltar" Text="   Voltar" />
                        </td>
                    </tr>
                </table>
            </asp:Panel>
            <asp:RoundedCornersExtender ID="pnlCadastroAla_RoundedCornersExtender" 
                runat="server" Enabled="True" TargetControlID="pnlCadastroAla">
            </asp:RoundedCornersExtender>
            <asp:HiddenField ID="hddCodigo" runat="server" />
            <asp:Panel ID="pnlAptoAlas" runat="server" Visible="False">
                <asp:Panel ID="pnlInsereAptoAla" runat="server">
                    <table>
                        <tr>
                            <td class="formRuler" colspan="8">
                                Inserindo apartamentos em alas.</td>
                        </tr>
                        <tr>
                            <td>
                                Apartamento</td>
                            <td>
                                &nbsp;</td>
                            <td>
                                Ala</td>
                            <td>
                                &nbsp;</td>
                            <td>
                                &nbsp;</td>
                            <td>
                                &nbsp;</td>
                            <td>
                                &nbsp;</td>
                            <td>
                                &nbsp;</td>
                        </tr>
                        <tr>
                            <td valign="bottom">
                                <asp:DropDownList ID="drpAptoAla" runat="server">
                                </asp:DropDownList>
                            </td>
                            <td valign="bottom">
                                <asp:Image ID="imgSetaPara" runat="server" ImageUrl="~/images/irApto.png" />
                            </td>
                            <td valign="bottom">
                                <asp:Label ID="lblMostraAla" runat="server" Font-Size="Large" Text="Label"></asp:Label>
                            </td>
                            <td valign="bottom">
                                <asp:Button ID="btnAptoAlaNovo" runat="server" CssClass="imgGravar" 
                                    Text="Atribuir" />
                            </td>
                            <td valign="bottom">
                                <asp:Button ID="btnConsultaAptoAla" runat="server" CssClass="imgLupa" 
                                    Text="Consultar" />
                            </td>
                            <td valign="bottom">
                                <asp:Image ID="imgRelAptosAla" runat="server" 
                                    ImageUrl="~/images/RelAptosAla.png" />
                            </td>
                            <td valign="bottom">
                                Apartamentos</td>
                            <td valign="bottom">
                                <asp:Button ID="btnSairAptoAla" runat="server" CssClass="imgVoltar" 
                                    Text="   Voltar" />
                            </td>
                        </tr>
                        <tr>
                            <td valign="top">
                                &nbsp;</td>
                            <td valign="top">
                                &nbsp;</td>
                            <td valign="top">
                                &nbsp;</td>
                            <td valign="top">
                                &nbsp;</td>
                            <td valign="top">
                                &nbsp;</td>
                            <td valign="top">
                                &nbsp;</td>
                            <td valign="top">
                                <asp:GridView ID="gdvAptosAla" runat="server" AutoGenerateColumns="False" 
                                    ShowHeader="False" Width="100px">
                                    <Columns>
                                        <asp:BoundField DataField="ApaDesc"></asp:BoundField>
                                    </Columns>
                                </asp:GridView>
                            </td>
                            <td valign="top">
                                &nbsp;</td>
                        </tr>
                    </table>
                </asp:Panel>
                <asp:RoundedCornersExtender ID="pnlInsereAptoAla_RoundedCornersExtender" 
                    runat="server" Enabled="True" TargetControlID="pnlInsereAptoAla">
                </asp:RoundedCornersExtender>
            </asp:Panel>
            <asp:Panel ID="pnlCadastroCam" runat="server" BackColor="White" Height="100px">
                <table>
                    <tr>
                        <td class="formRuler" colspan="3">
                            Cadastro de Camareiras</td>
                    </tr>
                    <tr>
                        <td>
                            Nome</td>
                        <td>
                            &nbsp;</td>
                        <td>
                            &nbsp;</td>
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
                            <asp:Button ID="btnVoltarCad" runat="server" CssClass="imgVoltar" 
                                Text="   Voltar" />
                        </td>
                    </tr>
                </table>
            </asp:Panel>
            <br />
            <asp:HiddenField ID="hddProcessando" runat="server" />
            <br />
            <div style="display: none">
                <asp:Button ID="btnUnidade" runat="server" Text="UnidadeOp" />
            </div>
            <br />
        </ContentTemplate>
    </asp:UpdatePanel>
    <p>
    </p>
</asp:Content>

