<%@ Page Title="Refeição Manual" Language="VB" MasterPageFile="~/TurismoSocial.master" AutoEventWireup="false" CodeFile="frmInsercaoManualRefeicao.aspx.vb" Inherits="frmInsercaoManualRefeicao" %>

<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="asp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <style type="text/css">
        .auto-style1 {
            width: 212px;
        }
    </style>
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
    <script src="../JScript.js" type="text/javascript"></script> 
    <asp:UpdatePanel ID="updGeral" runat="server">
          <ContentTemplate>
               <asp:Panel ID="pnlPesquisa" runat="server" Width="95%">
                   <br />
                   <asp:Label ID="lblTituloPagina" runat="server" CssClass="formRuler" 
                       Font-Size="Large" Text="Refeição Manual" Width="100%"></asp:Label>
                  <br />
                  <table>
                      <tr>
                          <td>
                              Data Inicial</td>
                          <td>
                              Data Final</td>
                          <td>
                              Refeição</td>
                          <td>
                              &nbsp;Inserção</td>
                          <td>
                              &nbsp;</td>
                          <td>
                              &nbsp;</td>
                          <td>
                              &nbsp;</td>
                          <td align="center" class="auto-style1">
                              Total</td>
                      </tr>
                      <tr>
                          <td>
                              <asp:TextBox ID="txtDataInicial" runat="server" Width="80px"></asp:TextBox>
                              <asp:CalendarExtender ID="txtDataInicial_CalendarExtender" runat="server" 
                                  Enabled="True" FirstDayOfWeek="Sunday" Format="dd/MM/yyyy" 
                                  TargetControlID="txtDataInicial">
                              </asp:CalendarExtender>
                          </td>
                          <td>
                              <asp:TextBox ID="txtDataFinal" runat="server" Width="80px"></asp:TextBox>
                              <asp:CalendarExtender ID="txtDataFinal_CalendarExtender" runat="server" 
                                  Enabled="True" FirstDayOfWeek="Sunday" Format="dd/MM/yyyy" 
                                  TargetControlID="txtDataFinal">
                              </asp:CalendarExtender>
                          </td>
                          <td>
                              <asp:DropDownList ID="drpRefeicao" runat="server">
                                  <asp:ListItem Value="T">Todas</asp:ListItem>
                                  <asp:ListItem Value="D" Selected="True">Desjejum</asp:ListItem>
                                  <asp:ListItem Value="A">Almoço</asp:ListItem>
                                  <asp:ListItem Value="J">Jantar</asp:ListItem>
                              </asp:DropDownList>
                          </td>
                          <td>
                              <asp:DropDownList ID="drpInsercao" runat="server">
                                 <asp:ListItem Value="T">Todas</asp:ListItem>
                                 <asp:ListItem Value="M">Manual</asp:ListItem>
                              </asp:DropDownList>
                          </td>
                          <td>
                              <asp:Button ID="btnConsultar" runat="server" CssClass="imgLupa" 
                                  Text="  Consultar" />
                          </td>
                          <td>
                              <asp:Button ID="btnInserir" runat="server" CssClass="imgNovo" Text="    Inserir" />
                          </td>
                          <td>
                              <asp:Button ID="btnVoltarPrincipal" runat="server" CssClass="imgVoltar" 
                                  Text="   Voltar" />
                          </td>
                          <td class="auto-style1">
                              <asp:Label ID="lblTotRefeicao" runat="server" Font-Size="XX-Large"></asp:Label>
                          </td>
                      </tr>
                      <tr>
                          <td align="left" colspan="8">
                              &nbsp;</td>
                      </tr>
                  </table>
                  <br />
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
              <asp:RoundedCornersExtender ID="pnlPesquisa_RoundedCornersExtender" 
                  runat="server" Enabled="True" TargetControlID="pnlPesquisa" Radius="7" BorderColor="ActiveBorder">
              </asp:RoundedCornersExtender>
              <asp:Panel ID="pnlGridRefeicao" runat="server" Width="95%">
                  <asp:GridView ID="gdvRefeicaoManual" runat="server" AutoGenerateColumns="False" 
                      CellPadding="4" 
                      DataKeyNames="IntId,RefCortesia,RefData,RefId,RefQtde,RefTipo" GridLines="None" 
                      ShowFooter="True" ForeColor="#333333" AllowPaging="True" PageSize="20">
                      <RowStyle BackColor="#EFF3FB" />
                      <Columns>
                          <asp:BoundField DataField="RefData" HeaderText="Data">
                          <HeaderStyle CssClass="formRuler" />
                          </asp:BoundField>
                          <asp:BoundField DataField="RefQtde" HeaderText="Quantidade">
                          <HeaderStyle CssClass="formRuler" />
                          <ItemStyle HorizontalAlign="Center" />
                          </asp:BoundField>
                          <asp:BoundField DataField="RefTipo" HeaderText="Tipo">
                          <HeaderStyle CssClass="formRuler" />
                          </asp:BoundField>
                          <asp:BoundField DataField="RefCortesia" HeaderText="Cortesia">
                          <HeaderStyle CssClass="formRuler" />
                          <ItemStyle HorizontalAlign="Center" />
                          </asp:BoundField>
                          <asp:TemplateField HeaderText="Apagar">
                              <ItemTemplate>
                                  <asp:ImageButton ID="imgApagar" runat="server" ImageUrl="~/images/Delete.gif" 
                                      onclick="imgApagar_Click" OnClientClick="if(confirm('Deseja mesmo apagar esse registro?'))
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
                                        };" style="width: 16px" />
                              </ItemTemplate>
                              <HeaderStyle CssClass="formRuler" />
                              <ItemStyle HorizontalAlign="Center" />
                          </asp:TemplateField>
                      </Columns>
                      <FooterStyle CssClass="formRuler" Font-Bold="True" ForeColor="White" />
                      <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
                      <SelectedRowStyle BackColor="#D1DDF1" BorderColor="#66CCFF" Font-Bold="True" 
                          ForeColor="#333333" />
                      <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                      <EditRowStyle BackColor="#2461BF" />
                      <AlternatingRowStyle BackColor="White" />
                  </asp:GridView>
              </asp:Panel>
              <asp:RoundedCornersExtender ID="pnlGridRefeicao_RoundedCornersExtender" 
                  runat="server" Enabled="True" TargetControlID="pnlGridRefeicao" Radius="7" BorderColor="ActiveBorder">
              </asp:RoundedCornersExtender>
              <asp:Panel ID="PnlInserir" runat="server" Visible="False" Width="95%">
                  <table style="">
                      <tr>
                          <td>
                              Data</td>
                          <td>
                              Refeição</td>
                          <td>
                              Quantidade</td>
                          <td>
                              Motivo</td>
                      </tr>
                      <tr>
                          <td>
                              <asp:TextBox ID="txtDataRefeicao" runat="server" Width="80px"></asp:TextBox>
                              <asp:CalendarExtender ID="txtDataRefeicao_CalendarExtender" runat="server" 
                                  Enabled="True" FirstDayOfWeek="Sunday" Format="dd/MM/yyyy" 
                                  TargetControlID="txtDataRefeicao">
                              </asp:CalendarExtender>
                          </td>
                          <td>
                              <asp:DropDownList ID="drpTipoRefeicao" runat="server">
                                  <asp:ListItem Value="D">Desjejum</asp:ListItem>
                                  <asp:ListItem Value="A">Almoço</asp:ListItem>
                                  <asp:ListItem Value="J">Jantar</asp:ListItem>
                              </asp:DropDownList>
                          </td>
                          <td>
                              <asp:TextBox ID="txtQuantidade" runat="server" Width="50px"></asp:TextBox>
                          </td>
                          <td>
                              <asp:TextBox ID="txtMotivo" runat="server" MaxLength="100" Width="360px">INCLUSÃO MANUAL POR FALTA DE ENERGIA ELÉTRICA.</asp:TextBox>
                          </td>
                      </tr>
                      <tr>
                          <td>
                              &nbsp;</td>
                          <td>
                              &nbsp;</td>
                          <td>
                              &nbsp;</td>
                          <td align="right">
                              <asp:Button ID="btnGravar" runat="server" CssClass="imgGravar" OnClientClick="if(confirm('Confirma a inserção das refeições?'))
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
                                        };" Text="   Gravar" />
                              <asp:Button ID="btnVoltar" runat="server" CssClass="imgVoltar" 
                                  Text="   Voltar" />
                          </td>
                      </tr>
                  </table>
              </asp:Panel>
              <asp:RoundedCornersExtender ID="PnlInserir_RoundedCornersExtender" 
                  runat="server" Enabled="True" TargetControlID="PnlInserir" Radius="7" BorderColor="ActiveBorder">
              </asp:RoundedCornersExtender>
            <br />
            <br />
              <asp:HiddenField ID="hddProcessando" runat="server" />
            <br />
            <br />
            <br />
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>

