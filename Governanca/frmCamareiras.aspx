<%@ Page Title="Cadastro de camareiras" Language="VB" MasterPageFile="~/TurismoSocial.master" AutoEventWireup="false" CodeFile="frmCamareiras.aspx.vb" Inherits="frmCamareiras" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="asp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="conPlaHolTurismoSocial" Runat="Server">
    <p>
        <br />
    </p>
    <p>
        &nbsp;</p>
    <p>
        &nbsp;</p>
    <asp:UpdatePanel ID="updGeral" runat="server">
        <ContentTemplate>
            <asp:Panel ID="pnlPesquisa" runat="server">
                <table>
                    <tr>
                        <td class="formRuler" colspan="4">
                            Cadastro de Camareiras</td>
                    </tr>
                    <tr>
                        <td>
                            Nome</td>
                        <td>
                            &nbsp;</td>
                        <td>
                            &nbsp;</td>
                        <td>
                            &nbsp;</td>
                    </tr>
                    <tr>
                        <td>
                            <asp:TextBox ID="txtNome" runat="server" MaxLength="250" Width="250px"></asp:TextBox>
                        </td>
                        <td>
                            <asp:Button ID="btnConsultar" runat="server" CssClass="imgLupa" 
                                Text="  Consultar" />
                        </td>
                        <td>
                            <asp:Button ID="btnNovo" runat="server" CssClass="imgNovo" Text="   Novo" />
                        </td>
                        <td>
                            <asp:Button ID="btnVoltar" runat="server" CssClass="imgVoltar" Text="   Voltar" />
                        </td>
                    </tr>
                </table>
                <br />
                <br />
            </asp:Panel>
            <asp:RoundedCornersExtender ID="pnlPesquisa_RoundedCornersExtender" 
                runat="server" Enabled="True" TargetControlID="pnlPesquisa">
            </asp:RoundedCornersExtender>
            <br />
            <asp:Panel ID="pnlGridCamareira" runat="server">
                <asp:GridView ID="gdvCamareiras" runat="server" AutoGenerateColumns="False" 
                    DataKeyNames="CamId,CamNome">
                    <Columns>
                        <asp:ButtonField DataTextField="CamNome" HeaderText="Nome" CommandName="select">
                        <HeaderStyle CssClass="formRuler" />
                        </asp:ButtonField>
                        <asp:TemplateField HeaderText="Excluir">
                            <ItemTemplate>
                                <asp:ImageButton ID="imgExcluir" runat="server" ImageUrl="~/images/Delete.gif" 
                                    OnClientClick="if(confirm('Confirma operação?'))
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
                                };" onclick="imgExcluir_Click" />
                            </ItemTemplate>
                            <HeaderStyle CssClass="formRuler" />
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
                <br />
                <div style="display: none">
                    <asp:Button ID="btnUnidade" runat="server" Text="UnidadeOp" />
                </div>
            </asp:Panel>
            <asp:Panel ID="pnlCadastro" runat="server" Visible="False">
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
                            <asp:Button ID="btnSalvarCad" runat="server" CssClass="imgGravar" 
                                OnClientClick="if(confirm('Confirma operação?'))
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
                                };"  Text="   Salvar" />
                        </td>
                        <td>
                            <asp:Button ID="btnVoltarCad" runat="server" CssClass="imgVoltar" 
                                Text="   Voltar" />
                        </td>
                    </tr>
                </table>
            </asp:Panel>
            <asp:RoundedCornersExtender ID="pnlCadastro_RoundedCornersExtender" 
                runat="server" Enabled="True" TargetControlID="pnlCadastro">
            </asp:RoundedCornersExtender>
            <br />
            <asp:HiddenField ID="hddProcessando" runat="server" />
        </ContentTemplate>
    </asp:UpdatePanel>
    <p>
    </p>
</asp:Content>

