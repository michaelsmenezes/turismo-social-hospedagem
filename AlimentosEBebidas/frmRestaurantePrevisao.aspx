<%@ Page Title="Previsão" Language="VB" MasterPageFile="~/TurismoSocial.master" AutoEventWireup="false" CodeFile="frmRestaurantePrevisao.aspx.vb" Inherits="frmRestaurantePrevisao" %>

<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="asp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="conPlaHolTurismoSocial" Runat="Server">
    <p>
        <br />
    </p>
    <p>
        &nbsp;</p>
    <asp:UpdatePanel ID="updGeral" runat="server">
          <ContentTemplate>
              <br />
              <br />
              <br />
              <br />
            <br />
              <div align="center" class="formLabel">
                  <asp:UpdateProgress ID="updProcesso" runat="server" 
                      AssociatedUpdatePanelID="updGeral">
                      <ProgressTemplate>
                          Processando sua solicitação...<br />
                          &nbsp;<img alt="Processando..." src="../images/Aguarde.gif" />
                      </ProgressTemplate>
                  </asp:UpdateProgress>
              </div>
            <asp:Panel ID="pnlConsulta" runat="server" Width="95%">
                <br />
                <asp:Label ID="lblTituloPagina" runat="server" CssClass="formRuler" 
                    Font-Size="Large" Text="Previsões para o restaurante" Width="100%"></asp:Label>
                <table>
                    <tr>
                        <td>
                            Dia</td>
                        <td>
                            &nbsp;</td>
                    </tr>
                    <tr>
                        <td>
                            <asp:TextBox ID="txtDia" runat="server" Width="80px"></asp:TextBox>
                            <asp:CalendarExtender ID="txtDia_CalendarExtender" runat="server" 
                                Enabled="True" FirstDayOfWeek="Sunday" Format="dd/MM/yyyy" 
                                TargetControlID="txtDia">
                            </asp:CalendarExtender>
                            <asp:Button ID="btnConsultar" runat="server" CssClass="imgLupa" 
                                Text="  Consultar" />
                            <asp:Button ID="btnVoltar" runat="server" CssClass="imgVoltar" Text="   Voltar" />
                        </td>
                        <td>
                            &nbsp;</td>
                    </tr>
                    <tr>
                        <td>
                            &nbsp;</td>
                        <td>
                            &nbsp;</td>
                    </tr>
                    <tr>
                        <td align="center" colspan="2">
                            <asp:GridView ID="gdvPrevisoesRef" runat="server" AutoGenerateColumns="False" 
                                CellPadding="4" GridLines="None" 
                                DataKeyNames="DescricaoRefeicao,Previsao,EmEstada,Pago,APagar" 
                                ShowFooter="True" ForeColor="#333333">
                                <RowStyle BackColor="#EFF3FB" />
                                <Columns>
                                    <asp:BoundField DataField="DescricaoRefeicao" HeaderText="Refeição">
                                        <HeaderStyle CssClass="formRuler" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="Previsao" HeaderText="Previsão">
                                        <HeaderStyle CssClass="formRuler" />
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="EmEstada" HeaderText="Estada">
                                        <HeaderStyle CssClass="formRuler" />
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="Pago" HeaderText="Pago">
                                        <HeaderStyle CssClass="formRuler" />
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="APagar" HeaderText="A Pagar">
                                        <HeaderStyle CssClass="formRuler" />
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:BoundField>
                                </Columns>
                                <FooterStyle Font-Bold="True" ForeColor="White" 
                                    CssClass="formRuler" />
                                <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
                                <SelectedRowStyle BackColor="#D1DDF1" BorderColor="#66CCFF" Font-Bold="True" 
                                    ForeColor="#333333" />
                                <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                                <EditRowStyle BackColor="#2461BF" />
                                <AlternatingRowStyle BackColor="White" />
                            </asp:GridView>
                        </td>
                    </tr>
                </table>
            </asp:Panel>
              <asp:RoundedCornersExtender ID="pnlConsulta_RoundedCornersExtender" 
                  runat="server" Enabled="True" Radius="7" TargetControlID="pnlConsulta" BorderColor="ActiveBorder">
              </asp:RoundedCornersExtender>
            <br />
              <br />
              <asp:Panel ID="pnlPratosRapidos" runat="server" Width="95%">
                  <br />
                  <asp:Label ID="lblPratosRapidos0" runat="server" CssClass="formRuler" 
                      Font-Size="Large" 
                      Text="Passantes (1/2 diária sem pernoite) - Opções de Pratos Rápidos" 
                      Width="100%"></asp:Label>
                  <br />
                  <asp:GridView ID="gdvPratosRapidos" runat="server" AutoGenerateColumns="False" 
                      CellPadding="4" EmptyDataText="Não existem dados a serem exibidos" 
                      GridLines="None" ForeColor="#333333">
                      <RowStyle BackColor="#EFF3FB" />
                      <Columns>
                          <asp:BoundField DataField="Descricao" HeaderText="Descrição">
                          <HeaderStyle CssClass="formRuler" />
                          </asp:BoundField>
                          <asp:BoundField DataField="Solicitado" HeaderText="Solicitado">
                          <HeaderStyle CssClass="formRuler" />
                          <ItemStyle HorizontalAlign="Center" />
                          </asp:BoundField>
                          <asp:BoundField DataField="NaoPago" HeaderText="Não Pago">
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
                      <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" 
                          ForeColor="#333333" />
                      <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                      <EditRowStyle BackColor="#2461BF" />
                      <AlternatingRowStyle BackColor="White" />
                  </asp:GridView>
              </asp:Panel>
              <asp:RoundedCornersExtender ID="pnlPratosRapidos_RoundedCornersExtender" 
                  runat="server" Enabled="True" TargetControlID="pnlPratosRapidos" Radius="7" BorderColor="ActiveBorder">
              </asp:RoundedCornersExtender>
              <br />
              <br />
              <asp:Panel ID="pnlAutorizadoRest" runat="server" Width="95%">
                  <br />
                  <asp:Label ID="lblPassantesRestaurante" runat="server" CssClass="formRuler" 
                      Font-Size="Large" 
                      Text="Passantes (1/2 diária sem pernoite) - Almoço Autorizado no Restaurante" 
                      Width="100%"></asp:Label>
                  <br />
                  <asp:GridView ID="gdvPassantesRestaurante" runat="server" 
                      AutoGenerateColumns="False" CellPadding="4" 
                      EmptyDataText="Não existem dados a serem exibidos"  
                      GridLines="None" ForeColor="#333333">
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
              <asp:RoundedCornersExtender ID="pnlAutorizadoRest_RoundedCornersExtender" 
                  runat="server" Enabled="True" TargetControlID="pnlAutorizadoRest" 
                  BehaviorID="pnlAutorizadoRest_RoundedCornersExtender" Radius="7" BorderColor="ActiveBorder">
              </asp:RoundedCornersExtender>
              <br />
              <br />
              <br />
              <br />
              <br />
            <br />
            <br />
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>

