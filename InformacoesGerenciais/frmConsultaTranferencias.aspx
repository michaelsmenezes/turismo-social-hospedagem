<%@ Page Language="VB" AutoEventWireup="false" CodeFile="frmConsultaTranferencias.aspx.vb" Inherits="InformacoesGerenciais_frmConsultaTranferencias" %>

<%@ Register assembly="Microsoft.ReportViewer.WebForms, Version=11.0.0.0, Culture=neutral, PublicKeyToken=89845dcd8080cc91" namespace="Microsoft.Reporting.WebForms" tagprefix="rsweb" %>
<!DOCTYPE html>

<link href="../Content/bootstrap.min.css" rel="stylesheet" />
<script src="../Scripts/jquery-1.12.4.min.js"></script>
<script src="../Scripts/jquery-ui-1.12.1.min.js"></script>
<link href="../Content/themes/base/jquery-ui.min.css" rel="stylesheet" />
<link href="../Content/bootstrap.min.css" rel="stylesheet" />
<script src="../Scripts/jquery.inputmask.bundle.js"></script>

<meta name="viewport" content="width=device-width, initial-scale=1">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <%--Instalado pelo Nuget
        Install-Package bootstrap
        Install-Package JQuery
        Install-Package JQuery.InputMask
        Install-Package JQuery.Ui.Combined   --%>

    <script type="text/javascript">
        $(function () {
            $(".Calendario").datepicker({
                //showOn: "button",
                showOn: "both",
                buttonImage: "/images/calendariomini.png",
                buttonImageOnly: false,
                //showButtonPanel: true,
                dateFormat: "dd/mm/yy",
                //changeMonth: true,
                //changeYear: true,
                dayNames: ['Domingo', 'Segunda', 'Terça', 'Quarta', 'Quinta', 'Sexta', 'Sábado', 'Domingo'],
                dayNamesMin: ['D', 'S', 'T', 'Q', 'Q', 'S', 'S', 'D'],
                dayNamesShort: ['Dom', 'Seg', 'Ter', 'Qua', 'Qui', 'Sex', 'Sáb', 'Dom'],
                monthNames: ['Janeiro', 'Fevereiro', 'Março', 'Abril', 'Maio', 'Junho', 'Julho', 'Agosto', 'Setembro', 'Outubro', 'Novembro', 'Dezembro'],
                monthNamesShort: ['Jan', 'Fev', 'Mar', 'Abr', 'Mai', 'Jun', 'Jul', 'Ago', 'Set', 'Out', 'Nov', 'Dez'],
                showAnim: "fold"
            });
            $(function () {
                $(".MascData").inputmask("##/##/####");
            });
        });
    </script>


    <form id="form1" runat="server">
        <div>
            <p>
                <asp:ScriptManager ID="scpGeral" runat="server">
                </asp:ScriptManager>
            </p>
            <div class="row">
                <div class="col-md-2">
                    <img alt="" src="../images/sescDivulgacao.png" height="80" />
                </div>
                <div class="col-md-10">
                    <br />
                    <h2 class="lead text-left text-left">
                        <asp:Label ID="lblTituloPrincipal" runat="server" Text="Consulta tranferências entre apartamentos"></asp:Label></h2>
                </div>
            </div>

            <%--Iniciando nos componentes de referência para a consulta--%>
            <div class="row ">
                <div class="col-md-2" style="left: 10px">
                    <asp:Label ID="lblDataInicial" runat="server" Text="Data Inicial"></asp:Label>
                    &nbsp;<div class="form-inline">
                        <div class="form-group">
                            <div class="initialism">
                                <input type="text" id="txtDataIni" maxlength="10" class="text-left form-control Calendario MascData" runat="server" required="required" />
                            </div>
                        </div>
                    </div>
                </div>

                <div class="col-md-2" style="left: 10px">
                    <asp:Label ID="lblDataFim" runat="server" Text="Data Final"></asp:Label>
                    &nbsp;<div class="form-inline">
                        <div class="form-group">
                            <div class="initialism">
                                <input type="text" id="txtDataFim" maxlength="10" class="text-left form-control Calendario MascData" runat="server" required="required" />
                            </div>
                        </div>
                    </div>
                </div>

                <div class="col-md-1">
                    <br />
                    <asp:Button CssClass="btn-primary form-control" ID="btnConsultar" runat="server" Text="Consultar" />
                </div>

              <%--  <div class="col-md-1">
                    <br />
                    <asp:Button CssClass="btn-primary form-control" ID="btnImprimir" runat="server" Text="Imprimir" />
                </div>--%>

                <div class="col-md-1">
                    <br />
                    <asp:Button CssClass="btn-primary form-control btnLoading" ID="btnVoltar" ToolTip="Voltar para Reserva" runat="server" Text="Voltar" />
                </div>
            </div>

            <hr />
            <%-- Totalização das solicitações separadas /Confirmadas/Pend.Pgto/EmEstada --%>
            <div class="row " runat="server" visible="false" id="divTransferencia">
                <%-- Aqui terei o grid com o resultado --%>
                <div class="alert-link col-md-12 text-center">
                    <h4><b>Relação de Transferências</b></h4>
                </div>

                <br />
                <br />
                <div class="col-md-12 " style="left: 10%; text-align: center">
                    <asp:GridView Width="80%" ID="gdvTransferencia" runat="server" AutoGenerateColumns="False" CellPadding="4" ForeColor="#333333" GridLines="None">
                        <AlternatingRowStyle BackColor="White" ForeColor="#284775" />

                        <Columns>
                            <asp:BoundField DataField="ResId" HeaderText="Reserva">
                                <HeaderStyle CssClass="text-center" />
                                <ItemStyle CssClass="text-center" />
                            </asp:BoundField>
                            <asp:BoundField DataField="ResNome" HeaderText="Responsável da reserva">
                                <HeaderStyle CssClass="text-center" />
                                <ItemStyle CssClass="text-center" />
                            </asp:BoundField>
                            <asp:BoundField DataField="DataInicial" HeaderText="Data Inicial">
                                <HeaderStyle CssClass="text-center" />
                                <ItemStyle CssClass="text-center" />
                            </asp:BoundField>
                            <asp:BoundField DataField="DataFinal" HeaderText="Data Final">
                                <HeaderStyle CssClass="text-center" />
                                <ItemStyle CssClass="text-center" />
                            </asp:BoundField>

                            <asp:BoundField DataField="Origem" HeaderText="Origem">
                                <HeaderStyle CssClass="text-center" />
                                <ItemStyle CssClass="text-center" />
                            </asp:BoundField>

                            <asp:BoundField DataField="Destino" HeaderText="Destino">
                                <HeaderStyle CssClass="text-center" />
                                <ItemStyle CssClass="text-center" />
                            </asp:BoundField>

                            <asp:BoundField DataField="HosStatus" HeaderText="Status">
                                <HeaderStyle CssClass="text-center" />
                                <ItemStyle CssClass="text-center" />
                            </asp:BoundField>

                            <asp:BoundField DataField="HosUsuario" HeaderText="Usuário">
                                <HeaderStyle CssClass="text-center" />
                                <ItemStyle CssClass="text-center" />
                            </asp:BoundField>

                            <asp:BoundField DataField="HosUsuarioData" HeaderText="Dia da Transferência">
                                <HeaderStyle CssClass="text-center" />
                                <ItemStyle CssClass="text-center" />
                            </asp:BoundField>
                        </Columns>
                        <EditRowStyle BackColor="#999999" />
                        <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                        <HeaderStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                        <PagerStyle BackColor="#284775" ForeColor="White" HorizontalAlign="Center" />
                        <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
                        <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                        <SortedAscendingCellStyle BackColor="#E9E7E2" />
                        <SortedAscendingHeaderStyle BackColor="#506C8C" />
                        <SortedDescendingCellStyle BackColor="#FFFDF8" />
                        <SortedDescendingHeaderStyle BackColor="#6F8DAE" />
                    </asp:GridView>
                </div>
            </div>

            <!--Mostrando o relatório no reportview -->
         <%--   <div class="row " runat="server" visible="false" id="divReportview">
            <rsweb:ReportViewer ID="rptTransferencia" runat="server" Font-Names="Verdana" Font-Size="8pt" Height="" Visible="False" WaitMessageFont-Names="Verdana" WaitMessageFont-Size="14pt" Width="100%" SizeToReportContent="True" ZoomMode="PageWidth">
                <LocalReport ReportPath="InformacoesGerenciais\relTransferenciaHospedagem.rdlc">
                </LocalReport>
            </rsweb:ReportViewer>  --%>  

            <%--</div>--%>


        </div>
    </form>
</body>
</html>
