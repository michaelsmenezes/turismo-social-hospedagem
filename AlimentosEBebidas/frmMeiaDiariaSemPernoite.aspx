<%@ Page Title="Meia Di�ria Sem Pernoite" Language="VB" MasterPageFile="~/TurismoSocial.master" AutoEventWireup="false" CodeFile="frmMeiaDiariaSemPernoite.aspx.vb" Inherits="frmMeiaDiariaSemPernoite" %>

<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="asp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="conPlaHolTurismoSocial" Runat="Server">
    <p>
        <br />
    </p>
    <p>
        &nbsp;</p>

    <script src="../JScript.js" type="text/javascript"></script>
    <asp:UpdatePanel ID="updGeral" runat="server">
          <ContentTemplate>
              <br />
              <br />
            <br />
              <div align="center" class="formLabel">
                  <asp:UpdateProgress ID="updProcesso" runat="server" 
                      AssociatedUpdatePanelID="updGeral">
                      <ProgressTemplate>
                          Processando sua solicita��o...<br />
                          &nbsp;<img alt="Processando..." src="../images/Aguarde.gif" />
                      </ProgressTemplate>
                  </asp:UpdateProgress>
              </div>
            <br />
            <asp:Panel ID="pnlConsulta" runat="server" Width="95%">
                <asp:Label ID="lblTituloPagina" runat="server" CssClass="formRuler" 
                    Font-Size="Large" 
                    Text="Disponibilidade para Venda de Meia Di�ria Sem Pernoite" Width="100%"></asp:Label>
                <table>
                    <tr>
                        <td>
                            <asp:GridView ID="gdvInsereMeiaDiaria" runat="server" 
                                AutoGenerateColumns="False" CellPadding="4" 
                                DataKeyNames="praId,Descricao,Consumir" Font-Size="" 
                                GridLines="None" ForeColor="#333333">
                                <RowStyle BackColor="#EFF3FB" />
                                <Columns>
                                    <asp:BoundField DataField="Descricao" HeaderText="Op��o de Prato">
                                    <HeaderStyle CssClass="formRuler" />
                                    <ItemStyle HorizontalAlign="Left" />
                                    </asp:BoundField>
                                    <asp:TemplateField HeaderText="Quantidade">
<%--                                        <EditItemTemplate>
                                            <asp:TextBox ID="TextBox1" runat="server" Text='<%# Bind("Consumir") %>'></asp:TextBox>
                                        </EditItemTemplate>
--%>                                        <ItemTemplate>
                                            <asp:TextBox ID="txtQuantidade" runat="server" Width="60px"></asp:TextBox>
                                        </ItemTemplate>
                                        <HeaderStyle CssClass="formRuler" />
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:TemplateField>
                                </Columns>
                                <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                                <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
                                <SelectedRowStyle BackColor="#D1DDF1" BorderColor="#66CCFF" Font-Bold="True" 
                                    ForeColor="#333333" />
                                <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                                <EditRowStyle BackColor="#2461BF" />
                                <AlternatingRowStyle BackColor="White" />
                            </asp:GridView>
                        </td>
                        <td valign="bottom">
                            <asp:Button ID="btnGravar" runat="server" CssClass="imgGravar" 
                                Text="    Gravar" 
                                OnClientClick="if(confirm('Confirma a inser��o dos pratos r�pidos??'))
                                        {
                                            if(aspnetForm.ctl00_conPlaHolTurismoSocial_hddProcessando.value=='S')
                                        {
                                            alert('Este processo est� em execu��o. Por favor, aguarde.');
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
                        <td valign="bottom">
                            <asp:Button ID="btnVoltar" runat="server" CssClass="imgVoltar" Text="   Voltar" />
                        </td>
                    </tr>
                </table>
            </asp:Panel>
              <asp:RoundedCornersExtender ID="pnlConsulta_RoundedCornersExtender" 
                  runat="server" Enabled="True" Radius="7" TargetControlID="pnlConsulta" 
                  BorderColor="ActiveBorder" >
              </asp:RoundedCornersExtender>
            <br />
            <br />
              <asp:Panel ID="pnlOpcoesPratos" runat="server" Width="95%">
                  <asp:Label ID="lblPratosRapidos0" runat="server" CssClass="formRuler" 
                      Font-Size="Large" 
                      Text="Passantes (1/2 di�ria sem pernoite) - Op��es de Pratos R�pidos" 
                      Width="100%"></asp:Label>
                  <br />
                  <asp:GridView ID="gdvPratosRapidos" runat="server" AutoGenerateColumns="False" 
                      CellPadding="4" EmptyDataText="N�o existem dados a serem exibidos" 
                      ForeColor="#333333" GridLines="None">
                      <RowStyle BackColor="#EFF3FB" />
                      <Columns>
                          <asp:BoundField DataField="Descricao" HeaderText="Descri��o">
                          <HeaderStyle CssClass="formRuler" />
                          </asp:BoundField>
                          <asp:BoundField DataField="Solicitado" HeaderText="Solicitado">
                          <HeaderStyle CssClass="formRuler" />
                          <ItemStyle HorizontalAlign="Center" />
                          </asp:BoundField>
                          <asp:BoundField DataField="NaoPago" HeaderText="N�o Pago">
                          <HeaderStyle CssClass="formRuler" />
                          <ItemStyle HorizontalAlign="Center" />
                          </asp:BoundField>
                          <asp:BoundField DataField="Confirmado" HeaderText="Confirmado">
                          <HeaderStyle CssClass="formRuler" />
                          <ItemStyle HorizontalAlign="Center" />
                          </asp:BoundField>
                          <asp:BoundField DataField="Consumido" HeaderText="Consumido">
                          <HeaderStyle CssClass="formRuler" />
                          <ItemStyle HorizontalAlign="Center" />
                          </asp:BoundField>
                          <asp:BoundField DataField="Consumir" HeaderText="A Consumir">
                          <HeaderStyle CssClass="formRuler" />
                          <ItemStyle HorizontalAlign="Center" />
                          </asp:BoundField>
                      </Columns>
                      <FooterStyle BackColor="#507CD1" CssClass="formRuler" Font-Bold="True" 
                          ForeColor="White" />
                      <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
                      <SelectedRowStyle BackColor="#D1DDF1" BorderColor="#66CCFF" Font-Bold="True" 
                          ForeColor="#333333" />
                      <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                      <EditRowStyle BackColor="#2461BF" />
                      <AlternatingRowStyle BackColor="White" />
                  </asp:GridView>
              </asp:Panel>
              <asp:RoundedCornersExtender ID="pnlOpcoesPratos_RoundedCornersExtender" 
                  runat="server" Enabled="True" TargetControlID="pnlOpcoesPratos" Radius="7" BorderColor="ActiveBorder">
              </asp:RoundedCornersExtender>
              <br />
              <asp:Panel ID="pnlAlmocoRst" runat="server" Width="95%">
                  <asp:Label ID="lblPassantesRestaurante" runat="server" CssClass="formRuler" 
                      Font-Size="Large" 
                      Text="Passantes (1/2 di�ria sem pernoite) - Almo�o Autorizado no Restaurante" 
                      Width="100%"></asp:Label>
                  <br />
                  <asp:GridView ID="gdvPassantesRestaurante" runat="server" 
                      AutoGenerateColumns="False" CellPadding="4" 
                      EmptyDataText="N�o existem dados a serem exibidos" 
                      ForeColor="#333333" GridLines="None">
                      <RowStyle BackColor="#EFF3FB" />
                      <Columns>
                          <asp:BoundField DataField="Confirmado" HeaderText="Adquiridos">
                          <HeaderStyle CssClass="formRuler" />
                          <ItemStyle HorizontalAlign="Center" />
                          </asp:BoundField>
                          <asp:BoundField DataField="Consumido" HeaderText="Consumidos">
                          <HeaderStyle CssClass="formRuler" />
                          <ItemStyle HorizontalAlign="Center" />
                          </asp:BoundField>
                          <asp:BoundField DataField="Consumir" HeaderText="A Consumir">
                          <HeaderStyle CssClass="formRuler" />
                          <ItemStyle HorizontalAlign="Center" />
                          </asp:BoundField>
                      </Columns>
                      <FooterStyle BackColor="#507CD1" CssClass="formRuler" Font-Bold="True" 
                          ForeColor="White" />
                      <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
                      <SelectedRowStyle BackColor="#D1DDF1" BorderColor="#66CCFF" Font-Bold="True" 
                          ForeColor="#333333" />
                      <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                      <EditRowStyle BackColor="#2461BF" />
                      <AlternatingRowStyle BackColor="White" />
                  </asp:GridView>
                  <br />
              </asp:Panel>
              <asp:RoundedCornersExtender ID="pnlAlmocoRst_RoundedCornersExtender" 
                  runat="server" Enabled="True" TargetControlID="pnlAlmocoRst" Radius="7" BorderColor="ActiveBorder">
              </asp:RoundedCornersExtender>
              <asp:HiddenField ID="hddProcessando" runat="server" />
            <br />
        </ContentTemplate>
    </asp:UpdatePanel>
    <br />
</asp:Content>

