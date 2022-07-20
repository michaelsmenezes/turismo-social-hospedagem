Imports Microsoft.Reporting.WebForms
Imports dtsRelacaoIntegrantesTableAdapters
Imports Turismo

Partial Class formRelIntegrantesPasseio
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        If Not IsPostBack Then
            If Session.Item("ResStatus") <> "F" Then
                rdSelecao.Items.Remove(rdSelecao.Items.FindByValue("HL"))
            End If
            rdSelecao.SelectedIndex = 0
        End If
    End Sub

    Protected Sub btnImprimir_Click(sender As Object, e As EventArgs) Handles btnImprimir.Click
        'Se a Diferença entre datas for superior a 30 dias, dar um alerta e proibir.'
        'Passando os parâmetros'
        Dim ParamsSaidaProdutos(2) As ReportParameter
        ParamsSaidaProdutos(0) = New ReportParameter("ResId", Session.Item("ResId").ToString())
        ParamsSaidaProdutos(1) = New ReportParameter("ResNome", Session.Item("ResNome").ToString())
        If rdSelecao.SelectedIndex = 0 Then
            ParamsSaidaProdutos(2) = New ReportParameter("ResData", Session.Item("ResDataPasseio").ToString())
        Else
            ParamsSaidaProdutos(2) = New ReportParameter("ResData", Session.Item("ResData").ToString())
        End If

        Dim VarSql As New Text.StringBuilder("")
        With VarSql
            If rdSelecao.SelectedValue = "LI" Or rdSelecao.SelectedValue = "LA" Then 'Lista de Integrantes simples
                .AppendLine("SELECT ROW_NUMBER()over(order by intnome) as Row,i.IntNome , Convert( VarChar(10) , i.IntDtNascimento , 103 ) AS IntDtNascimento ,DATEDIFF(YEAR,i.IntDtNascimento,i.IntDataIni) as Idade ,REPLACE(i.IntRG,' ','') as IntRG , i.IntCPF , rtrim(i.IntFoneResponsavelExc) as IntFoneResponsavelExc, ")
                .AppendLine("c.CatDescricao,i.IntPoltronaExc,i.IntMatricula,i.IntApartamentoExc ")
                .AppendLine("  FROM TbIntegrante i ")
                .AppendLine("INNER JOIN TbCategoria c ON c.CatId = i.CatId ")
                .AppendLine("  WHERE i.ResId = " & Session.Item("ResId").ToString() & " ")
                If rdSelecao.SelectedValue = "LI" Then
                    .AppendLine("ORDER BY i.IntNome ")
                Else
                    .AppendLine("ORDER BY i.IntApartamentoExc ")
                End If
            Else 'Relatorio de Home List
                .AppendLine("select i.IntNome,a.ApaDesc,h.HosValorPago,i.IntDtNascimento,i.IntRG,i.IntCPF,c.CatDescricao,i.IntFormaPagamento, ")
                .AppendLine("case ")
                .AppendLine(" when i.IntFormaPagamento = 'F' then '0.00' ")
                .AppendLine("else ")
                .AppendLine(" h.HosValorPago")
                .AppendLine(" end As SomaPagante, ")
                .AppendLine("case ")
                .AppendLine("when i.IntFormaPagamento = 'F' then h.HosValorPago ")
                .AppendLine("else ")
                .AppendLine("  '0.00' ")
                .AppendLine("end As SomaFree ")
                .AppendLine("from TbIntegrante i ")
                .AppendLine("inner join TbHospedagem h on h.IntId = i.IntId ")
                .AppendLine("left join TbApartamento a on a.ApaId = h.ApaId ")
                .AppendLine("inner join TbCategoria c on c.CatId = i.CatId ")
                .AppendLine("where ResId = " & Session.Item("ResId").ToString() & " ")
                .AppendLine("ORDER BY i.IntNome ")
            End If
        End With
        Dim OrdersRelacaoIntegrantes As RelacaoIntegrantesTableAdapter = New RelacaoIntegrantesTableAdapter
        Dim OrdersHomeListIntegrantes As DataTableHomeListTableAdapter = New DataTableHomeListTableAdapter

        'Mudando dinamicamento a string de conexão de acordo com a unidade operacional

        If Session("MasterPage").ToString = "~/TurismoSocial.Master" Then
            Dim oTurismo = New ConexaoDAO("DbTurismoSocial")
            OrdersRelacaoIntegrantes.Connection.ConnectionString = oTurismo.strConnection.ConnectionString
            OrdersHomeListIntegrantes.Connection.ConnectionString = oTurismo.strConnection.ConnectionString

        Else
            Dim oTurismo = New ConexaoDAO("DbTurismoSocialPiri")
            OrdersRelacaoIntegrantes.Connection.ConnectionString = oTurismo.strConnection.ConnectionString
            OrdersHomeListIntegrantes.Connection.ConnectionString = oTurismo.strConnection.ConnectionString
        End If
        'Dim ordersRelRevisaoLinha As dtsRelGovernanca.DataTableRevisaoDataTable = ordersRevisaoLinha.GetData(VarRevisao.ToString)
        'Dim ordersRelSaidaProdutos As dtsProdutos.dtsRelSaidaProdutosDataTable = OrdersSaidaProdutos.GetData(VarSql.ToString)
        Dim ordersRelRelacaoIntegrantes As System.Data.DataTable = OrdersRelacaoIntegrantes.GetData(VarSql.ToString)
        Dim ordersRelHomeListIntegrantes As System.Data.DataTable = OrdersHomeListIntegrantes.GetData(VarSql.ToString)

        Dim rdsRelRelatorioIntegrantes As New ReportDataSource("dsRelPasseio", ordersRelRelacaoIntegrantes)
        Dim rdsRelHomeListIntegrantes As New ReportDataSource("dtsHomeListIntegrantes", ordersRelHomeListIntegrantes)

        If rdSelecao.SelectedValue = "LI" Or rdSelecao.SelectedValue = "LA" Then 'Lista simpres dos integrantes
            'Chamando o relatório'
            rptHomeList.Visible = False
            rptIntegrantePasseio.Visible = True
            rptIntegrantePasseio.LocalReport.DataSources.Clear()
            rptIntegrantePasseio.LocalReport.DataSources.Add(rdsRelRelatorioIntegrantes)
            rptIntegrantePasseio.LocalReport.SetParameters(ParamsSaidaProdutos)
        Else
            rptIntegrantePasseio.Visible = False
            rptHomeList.Visible = True
            rptHomeList.LocalReport.DataSources.Clear()
            rptHomeList.LocalReport.DataSources.Add(rdsRelHomeListIntegrantes)
            rptHomeList.LocalReport.SetParameters(ParamsSaidaProdutos)
        End If
    End Sub

    Protected Sub rdSelecao_SelectedIndexChanged(sender As Object, e As EventArgs) Handles rdSelecao.SelectedIndexChanged
        rptHomeList.Visible = False
        rptIntegrantePasseio.Visible = False
    End Sub
End Class
