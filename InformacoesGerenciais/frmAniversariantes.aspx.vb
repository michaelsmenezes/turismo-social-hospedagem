Imports ConsultaCaldosTableAdapters
Imports Microsoft.Reporting.WebForms
Imports Turismo

Partial Class InformacoesGerenciais_frmAniversariantes
    Inherits System.Web.UI.Page

    Protected Sub btnConsultar_Click(sender As Object, e As EventArgs) Handles btnConsultar.Click
        If txtDataInicial.Text.Trim.Length = 0 Then
            txtDataInicial.Text = Format((Date.Today), "dd/MM/yyyy")
        End If
        'Impressão de Caldos'
        Dim VarAniversariantes As StringBuilder
        VarAniversariantes = New StringBuilder("")
        With VarAniversariantes
            .Append("SET NOCOUNT ON select a.ApaDesc,UPPER(i.IntNome)as Nome, CONVERT(CHAR(10),i.IntDtNascimento,103) as Nascimento, ")
            '.Append("(CONVERT(CHAR(2),DAY(I.INTDTNASCIMENTO),103) + '/' +  CONVERT(CHAR(2),MONTH(I.INTDTNASCIMENTO),103)) AS DiaMes, ")
            .Append("CONVERT(integer,DAY(I.INTDTNASCIMENTO),103) as Dia, ")
            .Append("CONVERT(integer,MONTH(I.INTDTNASCIMENTO),103) AS Mes, ")
            .Append("DATEDIFF(YEAR,i.IntDtNascimento,GETDATE()) as Idade ")
            .Append("from TbIntegrante i ")
            .Append("inner join TbHospedagem h on h.IntId = i.IntId ")
            .Append("inner join TbApartamento a on a.ApaId = h.ApaId where i.ResId > 0 and i.IntStatus = 'E' and i.IntDataIniReal is not null ")
            .Append("AND ")
            .Append("MONTH(convert(Date,I.IntDtNascimento,103)) Between  MONTH(convert(Date,'" & CDate(txtDataInicial.Text) & "',103)) and MONTH(convert(Date,'" & CDate(txtDataFinal.Text) & "',103)) ")
            .Append("AND ")
            .Append("DAY(convert(Date,I.IntDtNascimento,103)) BETWEEN DAY(convert(Date,'" & CDate(txtDataInicial.Text) & "',103)) AND DAY(convert(Date,'" & CDate(txtDataFinal.Text) & "',103)) ")
            .Append("ORDER BY Mes, Dia, i.IntNome ")
        End With
        'Passando os parametros'
        Dim Params(2) As ReportParameter
        If btnConsultar.Attributes.Item("AliasBancoTurismo") = "TurismoSocialCaldas" Then
            Params(0) = New ReportParameter("Unidade", "SESC - Caldas Novas")
        Else
            Params(0) = New ReportParameter("Unidade", "SESC - Pousada Pirenópolis")
        End If
        Params(1) = New ReportParameter("Nascimento", "Período: " & Format(CDate(txtDataInicial.Text), "dd/MM/yyyy") & " até " & Format(CDate(txtDataFinal.Text), "dd/MM/yyyy"))
        Params(2) = New ReportParameter("Usuario", "Impresso por: " & User.Identity.Name.ToString.Replace("SESC-GO.COM.BR\", ""))

        Dim ordersAniversarianteLinha As DataTableAniversariantesTableAdapter = New DataTableAniversariantesTableAdapter
        'Mudando dinamicamento a string de conexão de acordo com a unidade operacional
        If btnConsultar.Attributes.Item("AliasBancoTurismo") = "TurismoSocialCaldas" Then
            Dim oTurismo = New ConexaoDAO("DbTurismoSocial")
            ordersAniversarianteLinha.Connection.ConnectionString = oTurismo.strConnection.ConnectionString
        Else
            Dim oTurismo = New ConexaoDAO("DbTurismoSocialPiri")
            ordersAniversarianteLinha.Connection.ConnectionString = oTurismo.strConnection.ConnectionString
        End If
        Dim ordersRelAniversarianteLinha As System.Data.DataTable = ordersAniversarianteLinha.GetData(VarAniversariantes.ToString)
        Dim rdsRelAniversarianteLinha As New ReportDataSource("dtsAniversariantes", ordersRelAniversarianteLinha)
        If ordersAniversarianteLinha.GetData(VarAniversariantes.ToString).Rows.Count > 0 Then
            'Chamando o relatório'
            'pnlGridCaldos.Visible = False
            rptAniversariantes.Visible = True
            rptAniversariantes.LocalReport.DataSources.Clear()
            rptAniversariantes.LocalReport.DataSources.Add(rdsRelAniversarianteLinha)
            rptAniversariantes.LocalReport.SetParameters(Params)
        Else
            ScriptManager.RegisterStartupScript(Page, Page.[GetType](), Guid.NewGuid().ToString(), "alert('Não existem informações a serem exibidas.');", True)
        End If
        txtDataInicial.Focus()
    End Sub

    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        If Not IsPostBack Then
            Select Case Session("MasterPage").ToString
                Case Is = "~/TurismoSocial.Master"
                    'Direcionando a aplicação para o banco de Caldas Novas
                    btnConsultar.Attributes.Add("AliasBancoTurismo", "TurismoSocialCaldas")
                    'Alterando dinamicamente as cores da pagina
                    Dim myHtmlLink As New HtmlLink()
                    Page.Header.Controls.Remove(myHtmlLink)
                    myHtmlLink.Href = "~/stylesheet.css"
                    myHtmlLink.Attributes.Add("rel", "stylesheet")
                    myHtmlLink.Attributes.Add("type", "text/css")
                    'Adicionando o Css na Sessão Head da Página
                    Page.Header.Controls.Add(myHtmlLink)
                Case Else
                    'Direcionando a aplicação para o banco de Pirenopolis
                    btnConsultar.Attributes.Add("AliasBancoTurismo", "TurismoSocialPiri")
                    'Alterando dinamicamente as cores da pagina
                    Dim myHtmlLink As New HtmlLink()
                    Page.Header.Controls.Remove(myHtmlLink)
                    myHtmlLink.Href = "~/stylesheetverde.css"
                    myHtmlLink.Attributes.Add("rel", "stylesheet")
                    myHtmlLink.Attributes.Add("type", "text/css")
                    'Adicionando o Css na Sessão Head da Página
                    Page.Header.Controls.Add(myHtmlLink)
            End Select
            txtDataInicial.Attributes.Add("OnKeyPress", "javascript:if((window.event.keyCode != 9) && (window.event.keyCode != 8) && (window.event.keyCode != 16)) {this.value=FormataData(this.value)}")
            txtDataInicial.Attributes.Add("Onblur", "javascript:if(!ValidaData(this,'N')){alert('Data inválida!');}")
            txtDataInicial.Attributes.Add("onKeyDown", "javascript:desabilitaEnter();")

            txtDataFinal.Attributes.Add("OnKeyPress", "javascript:if((window.event.keyCode != 9) && (window.event.keyCode != 8) && (window.event.keyCode != 16)) {this.value=FormataData(this.value)}")
            txtDataFinal.Attributes.Add("Onblur", "javascript:if(!ValidaData(this,'N')){alert('Data inválida!');}")
            txtDataFinal.Attributes.Add("onKeyDown", "javascript:desabilitaEnter();")

            'SÓ DEIXARÁ ACESSAR O SISTEMA SE PERTENCER AO GRUPOS ABAIXO'
            Dim TestaUsuario As New Uteis.TestaUsuario 'Instanciando o objeto testa usuario'
            Dim Grupos As String = TestaUsuario.listaGrupos(Replace(User.Identity.Name, "SESC-GO.COM.BR\", ""))
            If Not (Grupos.Contains("Turismo Social Gerencial") Or Grupos.Contains("Turismo Social Total") Or Grupos.Contains("Turismo Social Piri Gerencial")) Then
                Response.Redirect("AcessoNegado.aspx")
                Return
            End If
            txtDataInicial.Text = Format((Date.Today), "dd/MM/yyyy")
            txtDataFinal.Text = Format((Date.Today), "dd/MM/yyyy")
            txtDataInicial.Focus()
        End If
    End Sub
End Class
