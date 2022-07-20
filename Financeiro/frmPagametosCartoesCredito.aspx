<%@ Page Language="VB" AutoEventWireup="false" CodeFile="frmPagametosCartoesCredito.aspx.vb" Inherits="Financeiro_frmPagametosCartoesCredito" Culture="pt-BR" UICulture="pt-BR" %>

<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=11.0.0.0, Culture=neutral, PublicKeyToken=89845dcd8080cc91" Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>

<!DOCTYPE html>

<link href="../Content/bootstrap.min.css" rel="stylesheet" />
<script src="../Scripts/jquery-1.12.4.min.js"></script>
<script src="../Scripts/jquery-ui-1.12.1.min.js"></script>
<link href="../Content/themes/base/jquery-ui.min.css" rel="stylesheet" />
<link href="../Content/bootstrap.min.css" rel="stylesheet" />
<script src="../Scripts/jquery.inputmask.bundle.js"></script>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <p>
&nbsp;
    </p>
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
                    <asp:Label ID="lblTituloPrincipal" runat="server" Text="Relatório Financeiros"></asp:Label></h2>
            </div>
        </div>
        <asp:Menu ID="mnuRelatorios" CssClass="form-control" runat="server" Orientation="Horizontal" StaticSubMenuIndent="10px" BackColor="#E3EAEB" DynamicHorizontalOffset="2" Font-Names="Verdana" Font-Size="11" ForeColor="#666666">
            <DynamicHoverStyle BackColor="#666666" ForeColor="White" />
            <DynamicMenuItemStyle HorizontalPadding="5px" VerticalPadding="2px" />
            <DynamicMenuStyle BackColor="#E3EAEB" />
            <DynamicSelectedStyle BackColor="#E3EAEB" />
            <Items>
                <asp:MenuItem Text="Relatórios" Value="Relatorios">
                    <asp:MenuItem Text="Pagamentos e cartões" ToolTip="Relatório de acompanhamento do Eduardo" Value="RelatorioEdu"></asp:MenuItem>
                    <asp:MenuItem Text="Rateio" ToolTip="Relatório de rateios resumido" Value="Rateio"></asp:MenuItem>
                </asp:MenuItem>
                <asp:MenuItem Text="Cadastros" Value="Cadastros">
                    <asp:MenuItem Text="Contas Contábeis" ToolTip="Contas contábeis do relatório" Value="Contas Contábeis"></asp:MenuItem>
                    <asp:MenuItem Text="Modalidades" ToolTip="Cadastro de modalidade de receitas a apropriar" Value="Modalidades"></asp:MenuItem>
                </asp:MenuItem>
            </Items>
            <StaticHoverStyle BackColor="White" ForeColor="White" />
            <StaticMenuItemStyle HorizontalPadding="5px" VerticalPadding="2px" />
            <StaticSelectedStyle BackColor="#E3EAEB" />
        </asp:Menu>
        <div id="divPagamentosCartoes" runat="server" visible="false">
            <div class="container">
                <div class="row">
                    <div class="col-md-12">
                        <br />
                        <h2 class="lead text-left">
                            <asp:Label ID="lblTitulo" runat="server" Text="Relatório de Pagamentos em Cartões de Crédito"></asp:Label></h2>
                    </div>
                </div>

                <div class="row">
                    <div class="col-md-3">
                        <asp:Label ID="lblDataCreditoRecebimento" runat="server" Text="Data de Recebimento"></asp:Label>
&nbsp;<div class="form-inline">
                            <div class="form-group">
                                <div class="initialism">
                                    <input type="text" id="txtDataIni" maxlength="10" class="text-left form-control Calendario MascData" runat="server" required="required" />
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="col-md-2" id="divTipoRecebimento" runat="server">
                        Tipo de Recebimento
                        <asp:DropDownList ID="drpTipo" CssClass="form-control" runat="server" AutoPostBack="True">
                            <asp:ListItem Value="B" Text="Boletos"></asp:ListItem>
                            <asp:ListItem Value="T" Text="Cartões de Crédito"></asp:ListItem>
                            <asp:ListItem Value="E" Text="Emissivo"></asp:ListItem>
                        </asp:DropDownList>
                    </div>

                    <div class="col-md-2">
                        <br />
                        <asp:Button CssClass="btn-primary form-control" ID="btnConsultar" runat="server" Text="Consultar" />
                    </div>
                    <div class="col-md-2">
                        <br />
                        <asp:Button CssClass="btn-primary form-control btnLoading" ID="btnVoltar" ToolTip="Voltar para Reserva" runat="server" Text="Voltar" />
                    </div>
                </div>
                <div class="row">
                    <br />
                    <div class="col-md-12" id="divRelatorio" runat="server" visible="false">
                        <br />
                        <asp:HiddenField ID="hddRelatorio" runat="server" />
                        <br />
                        <rsweb:ReportViewer ID="rptVendas" runat="server" Font-Names="Verdana" Font-Size="8pt" Height="" SizeToReportContent="True" WaitMessageFont-Names="Verdana" WaitMessageFont-Size="14pt" Width="100%" ZoomMode="PageWidth">
                            <LocalReport ReportPath="rptVendasCartoesCredito.rdlc">
                            </LocalReport>
                        </rsweb:ReportViewer>
                    </div>
                    <div class="col-md-12" id="divRelRateio" runat="server" visible="false">
                        <rsweb:ReportViewer ID="rptRateios" runat="server" Font-Names="Verdana" Font-Size="8pt" Height="" SizeToReportContent="True" WaitMessageFont-Names="Verdana" WaitMessageFont-Size="14pt" Width="100%" ZoomMode="PageWidth">
                            <LocalReport ReportPath="rptRateios.rdlc">
                            </LocalReport>
                        </rsweb:ReportViewer>
                    </div>
                </div>
            </div>
        </div>
    </form>
</body>
</html>
