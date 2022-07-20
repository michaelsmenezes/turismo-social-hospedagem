Imports System.Net.Mail

Partial Class frmRetiradaDeCaixa
    Inherits System.Web.UI.Page
    Protected Sub Page_PreInit(ByVal sender As Object, ByVal e As EventArgs)
        If IsNothing(Session("MasterPage")) Then
            ScriptManager.RegisterStartupScript(Page, Page.Page.[GetType](), Guid.NewGuid().ToString(), "alert('" + "Não foi encontrado a sua matrícula nas informações da conta de rede. Por favor, abra um chamado técnico solicitando a inserção desta informação." + "');", True)
        ElseIf Session("MasterPage").ToString = "~/TurismoSocial.Master" Then
            Page.MasterPageFile = "~/TurismoSocial.Master"
        Else
            Page.MasterPageFile = "~/TurismoSocialPiri.Master"
        End If
    End Sub
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            If Not IsPostBack Then
                Select Case Session("MasterPage").ToString
                    Case Is = "~/TurismoSocial.Master"
                        'Direcionando a aplicação para o banco de Caldas Novas
                        btnSalvar.Attributes.Add("AliasBancoTurismo", "TurismoSocialCaldas")
                    Case Else
                        'Direcionando a aplicação para o banco de Pirenopolis
                        btnSalvar.Attributes.Add("AliasBancoTurismo", "TurismoSocialPiri")
                End Select

                Dim ObjRetiradaDeCaixaVO As New RetiradaDeCaixaVO
                Dim ObjRetiradaDeCaixaDAO As New RetiradaDeCaixaDAO
                'PEGANDO O NOME COMPLETO E MATRICULA DO USUÁRIO LOGADO NO MOMENTO'
                Dim search As DirectoryServices.DirectorySearcher = New DirectoryServices.DirectorySearcher("LDAP://sesc-go.com.br/DC=sesc-go,DC=com,DC=br")
                search.Filter = "(SAMAccountName=" + User.Identity.Name.Replace("SESC-GO.COM.BR\", "") + ")"
                Dim result As DirectoryServices.SearchResult = search.FindOne()
                search.PropertiesToLoad.Add("displayName")
                Dim NomeFunc As String = result.Properties("displayName").Item(0).ToString.Replace("- CNV", "").ToUpper
                'SÓ DEIXARÁ ACESSAR O SISTEMA SE PERTENCER AO GRUPOS ABAIXO'
                Dim TestaUsuario As New Uteis.TestaUsuario 'Instanciando o objeto testa usuario'
                Dim Grupos As String = TestaUsuario.listaGrupos(Replace(User.Identity.Name, "SESC-GO.COM.BR\", ""))
                Dim Usuario As String = User.Identity.Name.ToString.Replace("SESC-GO.COM.BR\", "")
                'Usuario = "venayane.fagundes"

                If Not (Grupos.Contains("Turismo Social Recepcao") Or Grupos.Contains("Turismo Social Total") Or Grupos.Contains("Turismo Social Piri Recepcao")) Then
                    Response.Redirect("AcessoNegado.aspx")
                    Exit Try
                ElseIf (Grupos.Contains("Retirada de Caixa Caldas Novas Gerencial") Or Grupos.Contains("Retirada de Caixa Pirenopolis Gerencial")) Then
                    pnlGerencial.Visible = True
                    pnlRetirada.Visible = False
                    txtDataInicial.Text = Format(Date.Today, "dd/MM/yyyy")
                    txtDataFinal.Text = Format(Date.Today, "dd/MM/yyyy")
                    txtDataInicial.Focus()
                Else
                    If (Grupos.Contains("Turismo Social Recepcao") Or Grupos.Contains("Turismo Social Total") Or Grupos.Contains("Turismo Social Piri Recepcao")) Then
                        'If Not (Grupos.Contains("RETIRADA DE CAIXA CALDAS NOVAS GERENCIAL") Or Grupos.Contains("RETIRADA DE CAIXA PIRENOPOLIS GERENCIAL")) Then
                        'Select Case Session("MasterPage").ToString
                        '    Case Is = "~/TurismoSocial.Master"
                        'Caso não tenha fechado o caixa no dia anterior, o sistema irá fechá-lo automaticamente
                        Select Case ObjRetiradaDeCaixaDAO.ForcaFechamentoRetirada(Usuario, btnSalvar.Attributes.Item("AliasBancoTurismo").ToString)
                            Case 0
                                ScriptManager.RegisterStartupScript(Page, Page.[GetType](), Guid.NewGuid().ToString(), "alert('Erro ao fechar Retirada de Caixa, informe ao centro de Informática!');", True)
                                Return
                            Case 1
                                ScriptManager.RegisterStartupScript(Page, Page.[GetType](), Guid.NewGuid().ToString(), "alert('Havia um caixa aberto com data inferior ao dia de hoje, o Sistema efetuou o seu fechamento.');", True)
                        End Select

                        'Consulta se possui ou não caixa aberto
                        Select Case ObjRetiradaDeCaixaDAO.ConsultarCaixaAberto(ObjRetiradaDeCaixaVO, btnSalvar.Attributes.Item("AliasBancoTurismo").ToString, Usuario).cxaCrtCod
                            Case Is = 0 'Zero é que não possui caixa aberto.
                                If Session("MasterPage").ToString = "~/TurismoSocial.Master" Then
                                    'Move o usuário para a abertura do caixa, pois não possui caixa aberto'
                                    Response.Redirect("http://caixafinanceirocaldas/caixafinanceiro.aspx", False)
                                    Exit Try
                                Else
                                    'Move o usuário para a abertura do caixa, pois não possui caixa aberto'
                                    Response.Redirect("http://caixafinanceiroPiri/caixafinanceiro.aspx", False)
                                    Exit Try
                                End If
                            Case Else
                                ObjRetiradaDeCaixaVO = ObjRetiradaDeCaixaDAO.ConsultarCaixaAberto(ObjRetiradaDeCaixaVO, btnSalvar.Attributes.Item("AliasBancoTurismo").ToString, Usuario)
                                'Abre a opção para entrar com os dados do caixa
                                pnlRetirada.Visible = True
                                pnlGerencial.Visible = False
                                pnlCabecalhoGerencial.Visible = False
                                lblData.Text = Format(CDate(ObjRetiradaDeCaixaVO.CxaCrtDAbe), "dd/MM/yyyy ") & Format(CDate(ObjRetiradaDeCaixaVO.cxacrthabe), "HH:mm:ss")
                                lblNumeroCaixa.Text = ObjRetiradaDeCaixaVO.cxaCrtCod.ToString & "/Retirada n°:" & ObjRetiradaDeCaixaVO.cbrId.ToString
                                lblOperador.Text = NomeFunc  'ObjRetiradaDeCaixaVO.CxaCrtOpr.ToString
                                btnSalvar.Attributes.Add("Operador", ObjRetiradaDeCaixaVO.CxaCrtOpr)
                                btnSalvar.Attributes.Add("Caixa", ObjRetiradaDeCaixaVO.cxaCrtCod)
                                btnSalvar.Attributes.Add("IdRetirada", ObjRetiradaDeCaixaVO.cbrId)
                        End Select
                        'Se o Caixa estiver fechado
                        If ObjRetiradaDeCaixaVO.cxaCrtCod = 0 Then
                            ScriptManager.RegisterStartupScript(Page, Page.[GetType](), Guid.NewGuid().ToString(), "alert('O Usuário atual não possui caixa aberto!');", True)
                            pnlRetirada.Visible = False
                        End If
                        '    Case Else
                        'Response.Redirect("AcessoNegado.aspx")
                        'Return
                        'End Select
                    End If
                    'Consultando Dinheiro
                    gdvCedulas.DataSource = ObjRetiradaDeCaixaDAO.ConsultaCedulas(ObjRetiradaDeCaixaVO, btnSalvar.Attributes.Item("AliasBancoTurismo").ToString, Usuario, btnSalvar.Attributes.Item("Caixa").ToString, "N", btnSalvar.Attributes.Item("IdRetirada"))
                    gdvCedulas.DataBind()
                    If gdvCedulas.Rows.Count = 0 Then
                        btnSalvar.Attributes.Add("TotCedulas", 0)
                    End If
                    'Consultando moedas
                    gdvMoeda.DataSource = ObjRetiradaDeCaixaDAO.ConsultaMoedas(ObjRetiradaDeCaixaVO, btnSalvar.Attributes.Item("AliasBancoTurismo").ToString, Usuario, btnSalvar.Attributes.Item("Caixa").ToString, "N", btnSalvar.Attributes.Item("IdRetirada"))
                    gdvMoeda.DataBind()
                    If gdvMoeda.Rows.Count = 0 Then
                        btnSalvar.Attributes.Add("TotMoedas", 0)
                    End If
                    'Consultando cheques
                    gdvCheque.DataSource = ObjRetiradaDeCaixaDAO.ConsultaCheques(ObjRetiradaDeCaixaVO, btnSalvar.Attributes.Item("AliasBancoTurismo").ToString, Usuario, btnSalvar.Attributes.Item("Caixa").ToString, "N", btnSalvar.Attributes.Item("IdRetirada"))
                    gdvCheque.DataBind()
                    If gdvCheque.Rows.Count = 0 Then
                        btnSalvar.Attributes.Add("TotCheques", 0)
                    End If
                    'Consultando Cartões
                    gdvCartao.DataSource = ObjRetiradaDeCaixaDAO.ConsultaCartoes(ObjRetiradaDeCaixaVO, btnSalvar.Attributes.Item("AliasBancoTurismo").ToString, Usuario, btnSalvar.Attributes.Item("Caixa").ToString, "N", btnSalvar.Attributes.Item("IdRetirada"))
                    gdvCartao.DataBind()
                    If gdvCartao.Rows.Count = 0 Then
                        btnSalvar.Attributes.Add("TotCartoes", 0)
                    End If
                    'txtVlCheque.Attributes.Add("OnKeyPress", "javascript:ColocaVirgula(this.value);")
                    txtVlCheque.Attributes.Add("onkeyup", "javascript:if((window.event.keyCode != 9) && (window.event.keyCode != 16)) {this.value=ColocaVirgula(this)}")
                    txtDataInicial.Attributes.Add("onkeypress", "javascript:FormataData(this.value)")
                    txtDataInicial.Attributes.Add("onkeypress", "javascript:FormataFinal(this.value)")
                    btnSalvar.Attributes.Add("rcxId", "0")
                    txtNumeroCartao.Attributes.Add("onkeypress", "javascript:SomenteNumeros(this.value)")
                    'Totalizando a retirada
                    lblTotDinheiro.Text = FormatNumber(CDec(btnSalvar.Attributes.Item("TotCedulas").ToString) + CDec(btnSalvar.Attributes.Item("TotMoedas").ToString), 2)
                    lblTotCheques.Text = FormatNumber(btnSalvar.Attributes.Item("TotCheques").ToString, 2)
                    lblTotCartoes.Text = FormatNumber(btnSalvar.Attributes.Item("TotCartoes").ToString, 2)
                    lblSomaGeral.Text = FormatNumber(CDec(btnSalvar.Attributes.Item("TotCedulas").ToString) + CDec(btnSalvar.Attributes.Item("TotMoedas").ToString) + CDec(btnSalvar.Attributes.Item("TotCheques").ToString) + CDec(btnSalvar.Attributes.Item("TotCartoes").ToString), 2)
                    drpTipo.Focus()
                    'Ativa/desativa o botão de nova retira'
                    If (gdvCedulas.Rows.Count > 0 Or gdvCartao.Rows.Count > 0 Or gdvCheque.Rows.Count > 0 Or gdvMoeda.Rows.Count > 0) Or
                       ObjRetiradaDeCaixaDAO.ConsultaCabecalhoAtivo(Usuario, btnSalvar.Attributes.Item("AliasBancoTurismo")) > 0 Then
                        btnNova.Visible = False
                        btnFechar.Visible = False
                        drpTipo.Visible = True
                        btnImprimir.Visible = True
                        btnSalvar.Visible = True
                        btnAtualiza.Visible = True
                        drpTipo.Focus()
                    ElseIf gdvCedulas.Rows.Count = 0 And gdvCartao.Rows.Count = 0 And gdvCheque.Rows.Count = 0 And gdvMoeda.Rows.Count = 0 Then
                        btnNova.Visible = True
                        btnImprimir.Visible = False
                        btnSalvar.Visible = False
                        btnAtualiza.Visible = False
                        btnFechar.Visible = False
                    End If
                End If
            End If
        Catch ex As Exception
            enviarEmailGenerico("Erro no Load da página no FrmRetiradaDeCaixa.aspx - Mensagem: " & ex.StackTrace.ToString.Replace(Chr(10), ""))
        End Try

    End Sub

    Protected Sub drpTipo_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles drpTipo.SelectedIndexChanged
        Select Case drpTipo.SelectedValue
            Case "CE"
                drpValor.Items.Clear()
                txtVlCheque.Text = "0"
                drpValor.Visible = True
                lblDescCartao.Visible = False
                drpCartoes.Visible = False
                txtVlCheque.Visible = False
                lblNrBanco.Visible = False
                txtBanco.Visible = False
                lblNrCheque.Visible = False
                txtNrCheque.Visible = False
                txtNumeroCartao.Visible = False
                lblNumeroCartao.Visible = False
                lblQtde.Visible = True
                txtQtde.Visible = True
                drpValor.Items.Insert(0, New ListItem("Selecione...", "0"))
                drpValor.Items.Insert(1, New ListItem("1,00", "1.00"))
                drpValor.Items.Insert(2, New ListItem("2,00", "2.00"))
                drpValor.Items.Insert(3, New ListItem("5,00", "5.00"))
                drpValor.Items.Insert(4, New ListItem("10,00", "10.00"))
                drpValor.Items.Insert(5, New ListItem("20,00", "20.00"))
                drpValor.Items.Insert(6, New ListItem("50,00", "50.00"))
                drpValor.Items.Insert(7, New ListItem("100,00", "100.00"))
                txtQtde.Text = ""
                txtQtde.Focus()
                'drpCadCamareira.Items.Insert(0, New ListItem("Selecione...", "0"))
            Case "MO"
                drpValor.Items.Clear()
                txtVlCheque.Text = "0"
                drpValor.Visible = True
                lblDescCartao.Visible = False
                drpCartoes.Visible = False
                txtVlCheque.Visible = False
                lblNrBanco.Visible = False
                txtBanco.Visible = False
                lblNrCheque.Visible = False
                txtNrCheque.Visible = False
                txtNumeroCartao.Visible = False
                lblNumeroCartao.Visible = False
                lblQtde.Visible = True
                txtQtde.Visible = True
                drpValor.Items.Insert(0, New ListItem("Selecione...", "0"))
                drpValor.Items.Insert(1, New ListItem("0,01", "0.01"))
                drpValor.Items.Insert(2, New ListItem("0,05", "0.05"))
                drpValor.Items.Insert(3, New ListItem("0,10", "0.10"))
                drpValor.Items.Insert(4, New ListItem("0,25", "0.25"))
                drpValor.Items.Insert(5, New ListItem("0,50", "0.50"))
                drpValor.Items.Insert(6, New ListItem("1,00", "1.00"))
                txtQtde.Text = ""
                txtQtde.Focus()
                'drpCadCamareira.Items.Insert(0, New ListItem("Selecione...", "0"))
            Case "CH"
                txtVlCheque.Text = "0"
                lblNrBanco.Visible = True
                txtBanco.Visible = True
                lblNrCheque.Visible = True
                txtNrCheque.Visible = True
                lblDescCartao.Visible = False
                drpCartoes.Visible = False
                lblQtde.Visible = False
                txtQtde.Visible = False
                drpValor.Visible = False
                txtVlCheque.Visible = True
                txtNumeroCartao.Visible = False
                lblNumeroCartao.Visible = False
                txtBanco.Focus()
            Case "CA"
                txtVlCheque.Text = "0"
                lblDescCartao.Visible = True
                drpCartoes.Visible = True
                lblNrBanco.Visible = False
                txtBanco.Visible = False
                lblNrCheque.Visible = False
                txtNrCheque.Visible = False
                lblQtde.Visible = False
                txtQtde.Visible = False
                drpValor.Visible = False
                txtVlCheque.Visible = True
                txtNumeroCartao.Visible = True
                lblNumeroCartao.Visible = True
                drpCartoes.Focus()
        End Select
        txtBanco.Text = ""
        txtNrCheque.Text = ""
        txtQtde.Text = ""
        drpCartoes.SelectedValue = 0
        txtNumeroCartao.Text = ""
        drpValor.SelectedValue = 0
        txtVlCheque.Text = 0
    End Sub

    Protected Sub btnSalvar_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSalvar.Click
        Try

            hddProcessando.Value = ""
            'Obriga a digitar as informações
            If drpTipo.SelectedValue = "0" Then
                ScriptManager.RegisterStartupScript(Page, Page.[GetType](), Guid.NewGuid().ToString(), "alert('Informe o tipo da transação para prosseguir!');", True)
                drpTipo.Focus()
                Return
            ElseIf drpTipo.SelectedValue = "CH" Then
                If txtNrCheque.Text.Length = 0 Or txtBanco.Text.Length = 0 Or CDec(txtVlCheque.Text) = 0 Then
                    ScriptManager.RegisterStartupScript(Page, Page.[GetType](), Guid.NewGuid().ToString(), "alert('Verifique as informações do cheque, todas precisam ser preenchidas.\n\n*Selecione: Cheque\n*Informe o Nº do Banco\n*Informe o Nº do Cheque\n*Informe o valor do Cheque!');", True)
                    drpTipo.Focus()
                    Return
                End If
            ElseIf drpTipo.SelectedValue = "CA" Then
                If drpCartoes.SelectedValue = "0" Or CDec(txtVlCheque.Text) = 0 Or txtNumeroCartao.Text.Length = 0 Then
                    ScriptManager.RegisterStartupScript(Page, Page.[GetType](), Guid.NewGuid().ToString(), "alert('Verifique as informações referente ao cartão, todos os campos precisam ser preenchidos!');", True)
                    drpTipo.Focus()
                    Return
                End If
            ElseIf drpTipo.SelectedValue = "MO" Then
                If txtQtde.Text.Length = 0 Or drpValor.SelectedValue = 0 Then
                    ScriptManager.RegisterStartupScript(Page, Page.[GetType](), Guid.NewGuid().ToString(), "alert('Verifique as informações: Quantidade e Valor. Todas precisam ser preenchidas!');", True)
                    drpTipo.Focus()
                    Return
                End If
            ElseIf drpTipo.SelectedValue = "CE" Then
                If txtQtde.Text.Length = 0 Or drpValor.SelectedValue = 0 Then
                    ScriptManager.RegisterStartupScript(Page, Page.[GetType](), Guid.NewGuid().ToString(), "alert('Verifique as informações: Quantidade e Valor. Todas precisam ser preenchidas!');", True)
                    drpTipo.Focus()
                    Return
                End If
            End If
            Dim ObjRetiradaDeCaixaVO As New RetiradaDeCaixaVO
            Dim ObjRetiradaDeCaixaDAO As New RetiradaDeCaixaDAO
            'Se for cheque irá verificar se existe no caixa do operador um cheque no valor que esta sendo informado'
            'Select Case drpTipo.SelectedValue
            '    Case Is = "CH"
            '        Select Case ObjRetiradaDeCaixaDAO.VerificaOpercaoChequeCartao(btnSalvar.Attributes.Item("AliasBancoTurismo").ToString, User.Identity.Name.ToString.Replace("SESC-GO.COM.BR\", ""), btnSalvar.Attributes.Item("Caixa"), CStr(CDec(txtVlCheque.Text)).Replace(",", "."), drpTipo.SelectedValue)
            '            Case 0
            '                ScriptManager.RegisterStartupScript(Page, Page.[GetType](), Guid.NewGuid().ToString(), "alert('Houve um erro checar o cheque no seu caixa. Tente novamente!');", True)
            '                Return
            '            Case -1
            '                ScriptManager.RegisterStartupScript(Page, Page.[GetType](), Guid.NewGuid().ToString(), "alert('Atenção! Seu caixa já foi fechado. Verifique!');", True)
            '                drpTipo.Focus()
            '                Return
            '            Case -2
            '                ScriptManager.RegisterStartupScript(Page, Page.[GetType](), Guid.NewGuid().ToString(), "alert('Não existe no seu caixa operação de cheque nesse valor. Verifique!');", True)
            '                drpTipo.Focus()
            '                Return
            '        End Select
            '    Case Is = "CA"
            '        Select Case ObjRetiradaDeCaixaDAO.VerificaOpercaoChequeCartao(btnSalvar.Attributes.Item("AliasBancoTurismo").ToString, User.Identity.Name.ToString.Replace("SESC-GO.COM.BR\", ""), btnSalvar.Attributes.Item("Caixa"), txtVlCheque.Text.Replace(",", "."), drpTipo.SelectedValue)
            '            Case 0
            '                ScriptManager.RegisterStartupScript(Page, Page.[GetType](), Guid.NewGuid().ToString(), "alert('Houve um erro checar o cheque no seu caixa. Tente novamente!');", True)
            '                Return
            '            Case -1
            '                ScriptManager.RegisterStartupScript(Page, Page.[GetType](), Guid.NewGuid().ToString(), "alert('Atenção! Seu caixa já foi fechado. Verifique!');", True)
            '                drpTipo.Focus()
            '                Return
            '            Case -2
            '                ScriptManager.RegisterStartupScript(Page, Page.[GetType](), Guid.NewGuid().ToString(), "alert('Não existe no seu caixa operação em Cartão nesse valor. Verifique!');", True)
            '                drpTipo.Focus()
            '                Return
            '            Case Else
            '                'Ok'
            '                drpTipo.Focus()
            '        End Select
            'End Select
            'Carregando o Objeto'
            With ObjRetiradaDeCaixaVO
                .cxaCrtCod = btnSalvar.Attributes.Item("Caixa").ToString
                .CxaCrtOpr = btnSalvar.Attributes.Item("Operador").ToString
                .cxaDescricaoCartao = drpCartoes.SelectedValue
                .cxaNumeroCupom = txtNumeroCartao.Text
                .cbrId = ObjRetiradaDeCaixaDAO.ConsultaCabecalhoAtivo(User.Identity.Name.ToString.Replace("SESC-GO.COM.BR\", ""), btnSalvar.Attributes.Item("AliasBancoTurismo"))
                If txtBanco.Text = "" Then
                    .cxaNumeroBanco = "0"
                Else
                    .cxaNumeroBanco = txtBanco.Text
                End If
                If txtNrCheque.Text = "" Then
                    .cxaNumeroCheque = "0"
                Else
                    .cxaNumeroCheque = txtNrCheque.Text
                End If
                .cxaTipoOperacao = drpTipo.SelectedValue
                'Se for cheque ou no cartão ira pegar do txtvalor caso contrario pega do dropdown
                Select Case drpTipo.SelectedValue
                    Case "CH"
                        .cxaValor = FormatNumber(txtVlCheque.Text, 2).Replace(".", "").Replace(",", ".")
                        .cxaQuantidade = 1
                    Case "CA"
                        .cxaValor = FormatNumber(txtVlCheque.Text, 2).Replace(".", "").Replace(",", ".")
                        .cxaQuantidade = 1
                    Case Else
                        .cxaValor = drpValor.SelectedItem.Value
                        .cxaQuantidade = txtQtde.Text
                End Select
                .rcxData = Now
            End With

            Select Case ObjRetiradaDeCaixaDAO.Inserir(ObjRetiradaDeCaixaVO, btnSalvar.Attributes.Item("AliasBancoTurismo").ToString, User.Identity.Name.ToString.Replace("SESC-GO.COM.BR\", ""), btnSalvar.Attributes.Item("crxId"))
                Case 1
                    'Salvo com sucesso
                    txtBanco.Text = ""
                    txtNrCheque.Text = ""
                    drpCartoes.SelectedValue = "0"
                    txtQtde.Text = ""
                    drpValor.SelectedValue = 0
                    txtVlCheque.Text = ""
                    txtNumeroCartao.Text = ""
                    'ScriptManager.RegisterStartupScript(Page, Page.[GetType](), Guid.NewGuid().ToString(), "alert('Salvo com sucesso!');", True)
                    btnSalvar.Attributes.Remove("crxId")
                    drpTipo.Focus()
                Case 2
                    ScriptManager.RegisterStartupScript(Page, Page.[GetType](), Guid.NewGuid().ToString(), "alert('Alterado com sucesso!');", True)
                    btnSalvar.Attributes.Remove("crxId")
                    drpTipo.Focus()
                Case -1
                    btnSalvar.Attributes.Remove("crxId")
                    ScriptManager.RegisterStartupScript(Page, Page.[GetType](), Guid.NewGuid().ToString(), "alert('Seu caixa esta fechado. Favor Verificar!');", True)
                    Return
            End Select
            'Consultando Dinheiro
            gdvCedulas.DataSource = ObjRetiradaDeCaixaDAO.ConsultaCedulas(ObjRetiradaDeCaixaVO, btnSalvar.Attributes.Item("AliasBancoTurismo").ToString, User.Identity.Name.ToString.Replace("SESC-GO.COM.BR\", ""), btnSalvar.Attributes.Item("Caixa").ToString, "N", btnSalvar.Attributes.Item("IdRetirada"))
            gdvCedulas.DataBind()
            If gdvCedulas.Rows.Count = 0 Then
                btnSalvar.Attributes.Add("TotCedulas", 0)
            End If
            'Consultando moedas
            gdvMoeda.DataSource = ObjRetiradaDeCaixaDAO.ConsultaMoedas(ObjRetiradaDeCaixaVO, btnSalvar.Attributes.Item("AliasBancoTurismo").ToString, User.Identity.Name.ToString.Replace("SESC-GO.COM.BR\", ""), btnSalvar.Attributes.Item("Caixa").ToString, "N", btnSalvar.Attributes.Item("IdRetirada"))
            gdvMoeda.DataBind()
            If gdvMoeda.Rows.Count = 0 Then
                btnSalvar.Attributes.Add("TotMoedas", 0)
            End If
            'Consultando cheques
            gdvCheque.DataSource = ObjRetiradaDeCaixaDAO.ConsultaCheques(ObjRetiradaDeCaixaVO, btnSalvar.Attributes.Item("AliasBancoTurismo").ToString, User.Identity.Name.ToString.Replace("SESC-GO.COM.BR\", ""), btnSalvar.Attributes.Item("Caixa").ToString, "N", btnSalvar.Attributes.Item("IdRetirada"))
            gdvCheque.DataBind()
            If gdvCheque.Rows.Count = 0 Then
                btnSalvar.Attributes.Add("TotCheques", 0)
            End If
            'Consultando Cartões
            gdvCartao.DataSource = ObjRetiradaDeCaixaDAO.ConsultaCartoes(ObjRetiradaDeCaixaVO, btnSalvar.Attributes.Item("AliasBancoTurismo").ToString, User.Identity.Name.ToString.Replace("SESC-GO.COM.BR\", ""), btnSalvar.Attributes.Item("Caixa").ToString, "N", btnSalvar.Attributes.Item("IdRetirada"))
            gdvCartao.DataBind()
            If gdvCartao.Rows.Count = 0 Then
                btnSalvar.Attributes.Add("TotCartoes", 0)
            End If
            'Totalizando a retirada
            lblTotDinheiro.Text = FormatNumber(CStr(CDec(btnSalvar.Attributes.Item("TotCedulas").ToString) + CDec(btnSalvar.Attributes.Item("TotMoedas").ToString)), 2)
            lblTotCheques.Text = FormatNumber(btnSalvar.Attributes.Item("TotCheques").ToString, 2)
            lblTotCartoes.Text = FormatNumber(btnSalvar.Attributes.Item("TotCartoes").ToString, 2)
            lblSomaGeral.Text = FormatNumber(CDec(btnSalvar.Attributes.Item("TotCedulas").ToString) + CDec(btnSalvar.Attributes.Item("TotMoedas").ToString) + CDec(btnSalvar.Attributes.Item("TotCheques").ToString) + CDec(btnSalvar.Attributes.Item("TotCartoes").ToString), 2)
            hddProcessando.Value = ""
        Catch ex As Exception
            hddProcessando.Value = ""
            enviarEmailGenerico("Erro ao salvar um valor em FrmRetiradaDeCaixa.Aspx" & ex.StackTrace.ToString.Replace(Chr(10), ""))
        End Try

    End Sub

    Protected Sub btnAtualiza_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btnAtualiza.Click
        Dim ObjRetiradaDeCaixaVO As New RetiradaDeCaixaVO
        Dim ObjRetiradaDeCaixaDAO As New RetiradaDeCaixaDAO
        'Consultando Dinheiro
        gdvCedulas.DataSource = ObjRetiradaDeCaixaDAO.ConsultaCedulas(ObjRetiradaDeCaixaVO, btnSalvar.Attributes.Item("AliasBancoTurismo").ToString, User.Identity.Name.ToString.Replace("SESC-GO.COM.BR\", ""), btnSalvar.Attributes.Item("Caixa").ToString, "N", btnSalvar.Attributes.Item("IdRetirada"))
        gdvCedulas.DataBind()
        If gdvCedulas.Rows.Count = 0 Then
            btnSalvar.Attributes.Add("TotCedulas", 0)
        End If
        'Consultando moedas
        gdvMoeda.DataSource = ObjRetiradaDeCaixaDAO.ConsultaMoedas(ObjRetiradaDeCaixaVO, btnSalvar.Attributes.Item("AliasBancoTurismo").ToString, User.Identity.Name.ToString.Replace("SESC-GO.COM.BR\", ""), btnSalvar.Attributes.Item("Caixa").ToString, "N", btnSalvar.Attributes.Item("IdRetirada"))
        gdvMoeda.DataBind()
        If gdvMoeda.Rows.Count = 0 Then
            btnSalvar.Attributes.Add("TotMoedas", 0)
        End If
        'Consultando cheques
        gdvCheque.DataSource = ObjRetiradaDeCaixaDAO.ConsultaCheques(ObjRetiradaDeCaixaVO, btnSalvar.Attributes.Item("AliasBancoTurismo").ToString, User.Identity.Name.ToString.Replace("SESC-GO.COM.BR\", ""), btnSalvar.Attributes.Item("Caixa").ToString, "N", btnSalvar.Attributes.Item("IdRetirada"))
        gdvCheque.DataBind()
        If gdvCheque.Rows.Count = 0 Then
            btnSalvar.Attributes.Add("TotCheques", 0)
        End If
        'Consultando Cartões
        gdvCartao.DataSource = ObjRetiradaDeCaixaDAO.ConsultaCartoes(ObjRetiradaDeCaixaVO, btnSalvar.Attributes.Item("AliasBancoTurismo").ToString, User.Identity.Name.ToString.Replace("SESC-GO.COM.BR\", ""), btnSalvar.Attributes.Item("Caixa").ToString, "N", btnSalvar.Attributes.Item("IdRetirada"))
        gdvCartao.DataBind()
        If gdvCartao.Rows.Count = 0 Then
            btnSalvar.Attributes.Add("TotCartoes", 0)
        End If
        'Totalizando a retirada
        lblTotDinheiro.Text = FormatNumber(CDec(btnSalvar.Attributes.Item("TotCedulas").ToString) + CDec(btnSalvar.Attributes.Item("TotMoedas").ToString), 2)
        lblTotCheques.Text = FormatNumber(btnSalvar.Attributes.Item("TotCheques").ToString, 2)
        lblTotCartoes.Text = FormatNumber(btnSalvar.Attributes.Item("TotCartoes").ToString, 2)
        lblSomaGeral.Text = FormatNumber(CDec(btnSalvar.Attributes.Item("TotCedulas").ToString) + CDec(btnSalvar.Attributes.Item("TotMoedas").ToString) + CDec(btnSalvar.Attributes.Item("TotCheques").ToString) + CDec(btnSalvar.Attributes.Item("TotCartoes").ToString), 2)
    End Sub

    Protected Sub gdvCedulas_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gdvCedulas.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            'Grava o id da linha no Atributo da imagem
            CType(e.Row.FindControl("imgAlterar"), ImageButton).CommandArgument = gdvCedulas.DataKeys(e.Row.RowIndex).Item(0).ToString
            CType(e.Row.FindControl("imgApagar"), ImageButton).CommandArgument = gdvCedulas.DataKeys(e.Row.RowIndex).Item(0).ToString
            'Negritando e desabilitando quando for o final do grid - Totalizador
            If e.Row.Cells(0).Text = "Total Dinheiro:" Then
                e.Row.Font.Bold = True
                e.Row.Cells(0).HorizontalAlign = HorizontalAlign.Left
                btnSalvar.Attributes.Add("TotCedulas", e.Row.Cells(2).Text)
                e.Row.FindControl("imgAlterar").Visible = False
                e.Row.FindControl("imgApagar").Visible = False
            End If
        End If
    End Sub

    Protected Sub gdvMoeda_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gdvMoeda.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            'Grava o id da linha no Atributo da imagem
            CType(e.Row.FindControl("imgAlterar"), ImageButton).CommandArgument = gdvMoeda.DataKeys(e.Row.RowIndex).Item(0).ToString
            CType(e.Row.FindControl("imgApagar"), ImageButton).CommandArgument = gdvMoeda.DataKeys(e.Row.RowIndex).Item(0).ToString
            If e.Row.Cells(0).Text = "Total Moedas:" Then
                e.Row.Font.Bold = True
                e.Row.Cells(0).HorizontalAlign = HorizontalAlign.Left
                btnSalvar.Attributes.Add("TotMoedas", e.Row.Cells(2).Text)
                e.Row.FindControl("imgAlterar").Visible = False
                e.Row.FindControl("imgApagar").Visible = False
            End If
        End If
    End Sub

    Protected Sub gdvCheque_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gdvCheque.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            'Grava o id da linha no Atributo da imagem
            CType(e.Row.FindControl("imgAlterar"), ImageButton).CommandArgument = gdvCheque.DataKeys(e.Row.RowIndex).Item(0).ToString
            CType(e.Row.FindControl("imgApagar"), ImageButton).CommandArgument = gdvCheque.DataKeys(e.Row.RowIndex).Item(0).ToString
            If e.Row.Cells(0).Text = "Total Cheques:" Then
                e.Row.Font.Bold = True
                e.Row.Cells(0).HorizontalAlign = HorizontalAlign.Left
                btnSalvar.Attributes.Add("TotCheques", e.Row.Cells(2).Text)
                e.Row.FindControl("imgAlterar").Visible = False
                e.Row.FindControl("imgApagar").Visible = False
            End If
        End If
    End Sub

    Protected Sub gdvCartao_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gdvCartao.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            'Grava o id da linha no Atributo da imagem
            CType(e.Row.FindControl("imgAlterar"), ImageButton).CommandArgument = gdvCartao.DataKeys(e.Row.RowIndex).Item(0).ToString
            CType(e.Row.FindControl("imgApagar"), ImageButton).CommandArgument = gdvCartao.DataKeys(e.Row.RowIndex).Item(0).ToString
            If e.Row.Cells(0).Text = "Total Cart&#245;es:" Then
                e.Row.Font.Bold = True
                e.Row.Cells(0).HorizontalAlign = HorizontalAlign.Left
                btnSalvar.Attributes.Add("TotCartoes", e.Row.Cells(2).Text)
                e.Row.FindControl("imgAlterar").Visible = False
                e.Row.FindControl("imgApagar").Visible = False
            End If
        End If
    End Sub

    Protected Sub imgAlterar_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        btnSalvar.Attributes.Add("crxId", sender.commandargument.ToString)
        Dim ObjRetiradaDeCaixaVO As New RetiradaDeCaixaVO
        Dim ObjRetiradaDeCaixaDAO As New RetiradaDeCaixaDAO
        ObjRetiradaDeCaixaVO = ObjRetiradaDeCaixaDAO.ConsultaRetiradaCodigo(ObjRetiradaDeCaixaVO, btnSalvar.Attributes.Item("crxId").ToString, btnSalvar.Attributes.Item("AliasBancoTurismo").ToString)
        Select Case ObjRetiradaDeCaixaVO.cxaTipoOperacao
            Case "CE"
                drpValor.Items.Clear()
                drpValor.Visible = True
                lblDescCartao.Visible = False
                drpCartoes.Visible = False
                txtVlCheque.Visible = False
                lblNrBanco.Visible = False
                txtBanco.Visible = False
                lblNrCheque.Visible = False
                txtNrCheque.Visible = False
                lblQtde.Visible = True
                txtQtde.Visible = True
                drpValor.Items.Insert(0, New ListItem("Selecione...", "0"))
                drpValor.Items.Insert(1, New ListItem("1,00", "1,00"))
                drpValor.Items.Insert(2, New ListItem("2,00", "2,00"))
                drpValor.Items.Insert(3, New ListItem("5,00", "5,00"))
                drpValor.Items.Insert(4, New ListItem("10,00", "10,00"))
                drpValor.Items.Insert(5, New ListItem("20,00", "20,00"))
                drpValor.Items.Insert(6, New ListItem("50,00", "50,00"))
                drpValor.Items.Insert(7, New ListItem("100,00", "100,00"))
                'Carregando os valores que estão no banco'
                With ObjRetiradaDeCaixaVO
                    drpTipo.SelectedValue = .cxaTipoOperacao
                    txtBanco.Text = .cxaNumeroBanco
                    txtNrCheque.Text = .cxaNumeroCheque
                    drpCartoes.SelectedValue = .cxaDescricaoCartao
                    txtQtde.Text = .cxaQuantidade
                    drpValor.SelectedValue = .cxaValor
                    txtVlCheque.Text = .cxaValor
                End With
                drpTipo.Focus()
                'drpCadCamareira.Items.Insert(0, New ListItem("Selecione...", "0"))
            Case "MO"
                drpValor.Items.Clear()
                drpValor.Visible = True
                lblDescCartao.Visible = False
                drpCartoes.Visible = False
                txtVlCheque.Visible = False
                lblNrBanco.Visible = False
                txtBanco.Visible = False
                lblNrCheque.Visible = False
                txtNrCheque.Visible = False
                lblQtde.Visible = True
                txtQtde.Visible = True
                drpValor.Items.Insert(0, New ListItem("Selecione...", "0"))
                drpValor.Items.Insert(1, New ListItem("0,01", "0,01"))
                drpValor.Items.Insert(2, New ListItem("0,05", "0,05"))
                drpValor.Items.Insert(3, New ListItem("0,10", "0,10"))
                drpValor.Items.Insert(4, New ListItem("0,25", "0,25"))
                drpValor.Items.Insert(5, New ListItem("0,50", "0,50"))
                drpValor.Items.Insert(6, New ListItem("1,00", "1,00"))
                'Carregando os valores que estão no banco'
                With ObjRetiradaDeCaixaVO
                    drpTipo.SelectedValue = .cxaTipoOperacao
                    txtBanco.Text = .cxaNumeroBanco
                    txtNrCheque.Text = .cxaNumeroCheque
                    drpCartoes.SelectedValue = .cxaDescricaoCartao
                    txtQtde.Text = .cxaQuantidade
                    drpValor.SelectedValue = .cxaValor
                    txtVlCheque.Text = .cxaValor
                End With
                drpTipo.Focus()
                'drpCadCamareira.Items.Insert(0, New ListItem("Selecione...", "0"))
            Case "CH"
                lblNrBanco.Visible = True
                txtBanco.Visible = True
                lblNrCheque.Visible = True
                txtNrCheque.Visible = True
                lblDescCartao.Visible = False
                drpCartoes.Visible = False
                lblQtde.Visible = False
                txtQtde.Visible = False
                drpValor.Visible = False
                txtVlCheque.Visible = True
                'Carregando os valores que estão no banco'
                With ObjRetiradaDeCaixaVO
                    drpTipo.SelectedValue = .cxaTipoOperacao
                    txtBanco.Text = .cxaNumeroBanco
                    txtNrCheque.Text = .cxaNumeroCheque
                    drpCartoes.SelectedValue = .cxaDescricaoCartao
                    txtQtde.Text = .cxaQuantidade
                    drpValor.SelectedValue = .cxaValor
                    txtVlCheque.Text = .cxaValor
                End With
                drpTipo.Focus()
            Case "CA"
                lblDescCartao.Visible = True
                drpCartoes.Visible = True
                lblNrBanco.Visible = False
                txtBanco.Visible = False
                lblNrCheque.Visible = False
                txtNrCheque.Visible = False
                lblQtde.Visible = False
                txtQtde.Visible = False
                drpValor.Visible = False
                txtVlCheque.Visible = True
                'Carregando os valores que estão no banco'
                With ObjRetiradaDeCaixaVO
                    drpTipo.SelectedValue = .cxaTipoOperacao
                    txtBanco.Text = .cxaNumeroBanco
                    txtNrCheque.Text = .cxaNumeroCheque
                    drpCartoes.SelectedValue = .cxaDescricaoCartao
                    txtQtde.Text = .cxaQuantidade
                    drpValor.SelectedValue = .cxaValor
                    txtVlCheque.Text = .cxaValor
                End With
                drpTipo.Focus()
        End Select
    End Sub

    Protected Sub imgAlterar_Click1(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        btnSalvar.Attributes.Add("crxId", sender.commandargument.ToString)
        Dim ObjRetiradaDeCaixaVO As New RetiradaDeCaixaVO
        Dim ObjRetiradaDeCaixaDAO As New RetiradaDeCaixaDAO
        ObjRetiradaDeCaixaVO = ObjRetiradaDeCaixaDAO.ConsultaRetiradaCodigo(ObjRetiradaDeCaixaVO, btnSalvar.Attributes.Item("crxId").ToString, btnSalvar.Attributes.Item("AliasBancoTurismo").ToString)
        Select Case ObjRetiradaDeCaixaVO.cxaTipoOperacao
            Case "CE"
                drpValor.Items.Clear()
                drpValor.Visible = True
                lblDescCartao.Visible = False
                drpCartoes.Visible = False
                txtVlCheque.Visible = False
                lblNrBanco.Visible = False
                txtBanco.Visible = False
                lblNrCheque.Visible = False
                txtNrCheque.Visible = False
                lblQtde.Visible = True
                txtQtde.Visible = True
                drpValor.Items.Insert(0, New ListItem("Selecione...", "0"))
                drpValor.Items.Insert(1, New ListItem("1,00", "1,00"))
                drpValor.Items.Insert(2, New ListItem("2,00", "2,00"))
                drpValor.Items.Insert(3, New ListItem("5,00", "5,00"))
                drpValor.Items.Insert(4, New ListItem("10,00", "10,00"))
                drpValor.Items.Insert(5, New ListItem("20,00", "20,00"))
                drpValor.Items.Insert(6, New ListItem("50,00", "50,00"))
                drpValor.Items.Insert(7, New ListItem("100,00", "100,00"))
                'Carregando os valores que estão no banco'
                With ObjRetiradaDeCaixaVO
                    drpTipo.SelectedValue = .cxaTipoOperacao
                    txtBanco.Text = .cxaNumeroBanco
                    txtNrCheque.Text = .cxaNumeroCheque
                    drpCartoes.SelectedValue = .cxaDescricaoCartao
                    txtQtde.Text = .cxaQuantidade
                    drpValor.SelectedValue = .cxaValor
                    txtVlCheque.Text = .cxaValor
                End With
                drpTipo.Focus()
                'drpCadCamareira.Items.Insert(0, New ListItem("Selecione...", "0"))
            Case "MO"
                drpValor.Items.Clear()
                drpValor.Visible = True
                lblDescCartao.Visible = False
                drpCartoes.Visible = False
                txtVlCheque.Visible = False
                lblNrBanco.Visible = False
                txtBanco.Visible = False
                lblNrCheque.Visible = False
                txtNrCheque.Visible = False
                lblQtde.Visible = True
                txtQtde.Visible = True
                drpValor.Items.Insert(0, New ListItem("Selecione...", "0"))
                drpValor.Items.Insert(1, New ListItem("0,01", "0,01"))
                drpValor.Items.Insert(2, New ListItem("0,05", "0,05"))
                drpValor.Items.Insert(3, New ListItem("0,10", "0,10"))
                drpValor.Items.Insert(4, New ListItem("0,25", "0,25"))
                drpValor.Items.Insert(5, New ListItem("0,50", "0,50"))
                drpValor.Items.Insert(6, New ListItem("1,00", "1,00"))
                'Carregando os valores que estão no banco'
                With ObjRetiradaDeCaixaVO
                    drpTipo.SelectedValue = .cxaTipoOperacao
                    txtBanco.Text = .cxaNumeroBanco
                    txtNrCheque.Text = .cxaNumeroCheque
                    drpCartoes.SelectedValue = .cxaDescricaoCartao
                    txtQtde.Text = .cxaQuantidade
                    drpValor.SelectedValue = .cxaValor
                    txtVlCheque.Text = .cxaValor
                End With
                drpTipo.Focus()
                'drpCadCamareira.Items.Insert(0, New ListItem("Selecione...", "0"))
            Case "CH"
                lblNrBanco.Visible = True
                txtBanco.Visible = True
                lblNrCheque.Visible = True
                txtNrCheque.Visible = True
                lblDescCartao.Visible = False
                drpCartoes.Visible = False
                lblQtde.Visible = False
                txtQtde.Visible = False
                drpValor.Visible = False
                txtVlCheque.Visible = True
                'Carregando os valores que estão no banco'
                With ObjRetiradaDeCaixaVO
                    drpTipo.SelectedValue = .cxaTipoOperacao
                    txtBanco.Text = .cxaNumeroBanco
                    txtNrCheque.Text = .cxaNumeroCheque
                    drpCartoes.SelectedValue = .cxaDescricaoCartao
                    txtQtde.Text = .cxaQuantidade
                    drpValor.SelectedValue = .cxaValor
                    txtVlCheque.Text = .cxaValor
                End With
                drpTipo.Focus()
            Case "CA"
                lblDescCartao.Visible = True
                drpCartoes.Visible = True
                lblNrBanco.Visible = False
                txtBanco.Visible = False
                lblNrCheque.Visible = False
                txtNrCheque.Visible = False
                lblQtde.Visible = False
                txtQtde.Visible = False
                drpValor.Visible = False
                txtVlCheque.Visible = True
                'Carregando os valores que estão no banco'
                With ObjRetiradaDeCaixaVO
                    drpTipo.SelectedValue = .cxaTipoOperacao
                    txtBanco.Text = .cxaNumeroBanco
                    txtNrCheque.Text = .cxaNumeroCheque
                    drpCartoes.SelectedValue = .cxaDescricaoCartao
                    txtQtde.Text = .cxaQuantidade
                    drpValor.SelectedValue = .cxaValor
                    txtVlCheque.Text = .cxaValor
                End With
                drpTipo.Focus()
        End Select
    End Sub

    Protected Sub imgAlterar_Click2(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        btnSalvar.Attributes.Add("crxId", sender.commandargument.ToString)
        Dim ObjRetiradaDeCaixaVO As New RetiradaDeCaixaVO
        Dim ObjRetiradaDeCaixaDAO As New RetiradaDeCaixaDAO
        ObjRetiradaDeCaixaVO = ObjRetiradaDeCaixaDAO.ConsultaRetiradaCodigo(ObjRetiradaDeCaixaVO, btnSalvar.Attributes.Item("crxId").ToString, btnSalvar.Attributes.Item("AliasBancoTurismo").ToString)
        Select Case ObjRetiradaDeCaixaVO.cxaTipoOperacao
            Case "CE"
                drpValor.Items.Clear()
                drpValor.Visible = True
                lblDescCartao.Visible = False
                drpCartoes.Visible = False
                txtVlCheque.Visible = False
                lblNrBanco.Visible = False
                txtBanco.Visible = False
                lblNrCheque.Visible = False
                txtNrCheque.Visible = False
                lblQtde.Visible = True
                txtQtde.Visible = True
                drpValor.Items.Insert(0, New ListItem("Selecione...", "0"))
                drpValor.Items.Insert(1, New ListItem("1,00", "1,00"))
                drpValor.Items.Insert(2, New ListItem("2,00", "2,00"))
                drpValor.Items.Insert(3, New ListItem("5,00", "5,00"))
                drpValor.Items.Insert(4, New ListItem("10,00", "10,00"))
                drpValor.Items.Insert(5, New ListItem("20,00", "20,00"))
                drpValor.Items.Insert(6, New ListItem("50,00", "50,00"))
                drpValor.Items.Insert(7, New ListItem("100,00", "100,00"))
                'Carregando os valores que estão no banco'
                With ObjRetiradaDeCaixaVO
                    drpTipo.SelectedValue = .cxaTipoOperacao
                    txtBanco.Text = .cxaNumeroBanco
                    txtNrCheque.Text = .cxaNumeroCheque
                    drpCartoes.SelectedValue = .cxaDescricaoCartao
                    txtQtde.Text = .cxaQuantidade
                    drpValor.SelectedValue = .cxaValor
                    txtVlCheque.Text = .cxaValor
                End With
                drpTipo.Focus()
                'drpCadCamareira.Items.Insert(0, New ListItem("Selecione...", "0"))
            Case "MO"
                drpValor.Items.Clear()
                drpValor.Visible = True
                lblDescCartao.Visible = False
                drpCartoes.Visible = False
                txtVlCheque.Visible = False
                lblNrBanco.Visible = False
                txtBanco.Visible = False
                lblNrCheque.Visible = False
                txtNrCheque.Visible = False
                lblQtde.Visible = True
                txtQtde.Visible = True
                drpValor.Items.Insert(0, New ListItem("Selecione...", "0"))
                drpValor.Items.Insert(1, New ListItem("0,01", "0,01"))
                drpValor.Items.Insert(2, New ListItem("0,05", "0,05"))
                drpValor.Items.Insert(3, New ListItem("0,10", "0,10"))
                drpValor.Items.Insert(4, New ListItem("0,25", "0,25"))
                drpValor.Items.Insert(5, New ListItem("0,50", "0,50"))
                drpValor.Items.Insert(6, New ListItem("1,00", "1,00"))
                'Carregando os valores que estão no banco'
                With ObjRetiradaDeCaixaVO
                    drpTipo.SelectedValue = .cxaTipoOperacao
                    txtBanco.Text = .cxaNumeroBanco
                    txtNrCheque.Text = .cxaNumeroCheque
                    drpCartoes.SelectedValue = .cxaDescricaoCartao
                    txtQtde.Text = .cxaQuantidade
                    drpValor.SelectedValue = .cxaValor
                    txtVlCheque.Text = .cxaValor
                End With
                drpTipo.Focus()
                'drpCadCamareira.Items.Insert(0, New ListItem("Selecione...", "0"))
            Case "CH"
                lblNrBanco.Visible = True
                txtBanco.Visible = True
                lblNrCheque.Visible = True
                txtNrCheque.Visible = True
                lblDescCartao.Visible = False
                drpCartoes.Visible = False
                lblQtde.Visible = False
                txtQtde.Visible = False
                drpValor.Visible = False
                txtVlCheque.Visible = True
                'Carregando os valores que estão no banco'
                With ObjRetiradaDeCaixaVO
                    drpTipo.SelectedValue = .cxaTipoOperacao
                    txtBanco.Text = .cxaNumeroBanco
                    txtNrCheque.Text = .cxaNumeroCheque
                    drpCartoes.SelectedValue = .cxaDescricaoCartao
                    txtQtde.Text = .cxaQuantidade
                    drpValor.SelectedValue = .cxaValor
                    txtVlCheque.Text = .cxaValor
                End With
                drpTipo.Focus()
            Case "CA"
                lblDescCartao.Visible = True
                drpCartoes.Visible = True
                lblNrBanco.Visible = False
                txtBanco.Visible = False
                lblNrCheque.Visible = False
                txtNrCheque.Visible = False
                lblQtde.Visible = False
                txtQtde.Visible = False
                drpValor.Visible = False
                txtVlCheque.Visible = True
                'Carregando os valores que estão no banco'
                With ObjRetiradaDeCaixaVO
                    drpTipo.SelectedValue = .cxaTipoOperacao
                    txtBanco.Text = .cxaNumeroBanco
                    txtNrCheque.Text = .cxaNumeroCheque
                    drpCartoes.SelectedValue = .cxaDescricaoCartao
                    txtQtde.Text = .cxaQuantidade
                    drpValor.SelectedValue = .cxaValor
                    txtVlCheque.Text = .cxaValor
                End With
                drpTipo.Focus()
        End Select
    End Sub

    Protected Sub imgAlterar_Click3(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        btnSalvar.Attributes.Add("crxId", sender.commandargument.ToString)
        Dim ObjRetiradaDeCaixaVO As New RetiradaDeCaixaVO
        Dim ObjRetiradaDeCaixaDAO As New RetiradaDeCaixaDAO
        ObjRetiradaDeCaixaVO = ObjRetiradaDeCaixaDAO.ConsultaRetiradaCodigo(ObjRetiradaDeCaixaVO, btnSalvar.Attributes.Item("crxId").ToString, btnSalvar.Attributes.Item("AliasBancoTurismo").ToString)
        Select Case ObjRetiradaDeCaixaVO.cxaTipoOperacao
            Case "CE"
                drpValor.Items.Clear()
                drpValor.Visible = True
                lblDescCartao.Visible = False
                drpCartoes.Visible = False
                txtVlCheque.Visible = False
                lblNrBanco.Visible = False
                txtBanco.Visible = False
                lblNrCheque.Visible = False
                txtNrCheque.Visible = False
                lblQtde.Visible = True
                txtQtde.Visible = True
                drpValor.Items.Insert(0, New ListItem("Selecione...", "0"))
                drpValor.Items.Insert(1, New ListItem("1,00", "1,00"))
                drpValor.Items.Insert(2, New ListItem("2,00", "2,00"))
                drpValor.Items.Insert(3, New ListItem("5,00", "5,00"))
                drpValor.Items.Insert(4, New ListItem("10,00", "10,00"))
                drpValor.Items.Insert(5, New ListItem("20,00", "20,00"))
                drpValor.Items.Insert(6, New ListItem("50,00", "50,00"))
                drpValor.Items.Insert(7, New ListItem("100,00", "100,00"))
                'Carregando os valores que estão no banco'
                With ObjRetiradaDeCaixaVO
                    drpTipo.SelectedValue = .cxaTipoOperacao
                    txtBanco.Text = .cxaNumeroBanco
                    txtNrCheque.Text = .cxaNumeroCheque
                    drpCartoes.SelectedValue = .cxaDescricaoCartao
                    txtQtde.Text = .cxaQuantidade
                    drpValor.SelectedValue = .cxaValor
                    txtVlCheque.Text = .cxaValor
                End With
                drpTipo.Focus()
                'drpCadCamareira.Items.Insert(0, New ListItem("Selecione...", "0"))
            Case "MO"
                drpValor.Items.Clear()
                drpValor.Visible = True
                lblDescCartao.Visible = False
                drpCartoes.Visible = False
                txtVlCheque.Visible = False
                lblNrBanco.Visible = False
                txtBanco.Visible = False
                lblNrCheque.Visible = False
                txtNrCheque.Visible = False
                lblQtde.Visible = True
                txtQtde.Visible = True
                drpValor.Items.Insert(0, New ListItem("Selecione...", "0"))
                drpValor.Items.Insert(1, New ListItem("0,01", "0,01"))
                drpValor.Items.Insert(2, New ListItem("0,05", "0,05"))
                drpValor.Items.Insert(3, New ListItem("0,10", "0,10"))
                drpValor.Items.Insert(4, New ListItem("0,25", "0,25"))
                drpValor.Items.Insert(5, New ListItem("0,50", "0,50"))
                drpValor.Items.Insert(6, New ListItem("1,00", "1,00"))
                'Carregando os valores que estão no banco'
                With ObjRetiradaDeCaixaVO
                    drpTipo.SelectedValue = .cxaTipoOperacao
                    txtBanco.Text = .cxaNumeroBanco
                    txtNrCheque.Text = .cxaNumeroCheque
                    drpCartoes.SelectedValue = .cxaDescricaoCartao
                    txtQtde.Text = .cxaQuantidade
                    drpValor.SelectedValue = .cxaValor
                    txtVlCheque.Text = .cxaValor
                End With
                drpTipo.Focus()
                'drpCadCamareira.Items.Insert(0, New ListItem("Selecione...", "0"))
            Case "CH"
                lblNrBanco.Visible = True
                txtBanco.Visible = True
                lblNrCheque.Visible = True
                txtNrCheque.Visible = True
                lblDescCartao.Visible = False
                drpCartoes.Visible = False
                lblQtde.Visible = False
                txtQtde.Visible = False
                drpValor.Visible = False
                txtVlCheque.Visible = True
                'Carregando os valores que estão no banco'
                With ObjRetiradaDeCaixaVO
                    drpTipo.SelectedValue = .cxaTipoOperacao
                    txtBanco.Text = .cxaNumeroBanco
                    txtNrCheque.Text = .cxaNumeroCheque
                    drpCartoes.SelectedValue = .cxaDescricaoCartao
                    txtQtde.Text = .cxaQuantidade
                    drpValor.SelectedValue = .cxaValor
                    txtVlCheque.Text = .cxaValor
                End With
                drpTipo.Focus()
            Case "CA"
                lblDescCartao.Visible = True
                drpCartoes.Visible = True
                lblNrBanco.Visible = False
                txtBanco.Visible = False
                lblNrCheque.Visible = False
                txtNrCheque.Visible = False
                lblQtde.Visible = False
                txtQtde.Visible = False
                drpValor.Visible = False
                txtVlCheque.Visible = True
                'Carregando os valores que estão no banco'
                With ObjRetiradaDeCaixaVO
                    drpTipo.SelectedValue = .cxaTipoOperacao
                    txtBanco.Text = .cxaNumeroBanco
                    txtNrCheque.Text = .cxaNumeroCheque
                    drpCartoes.SelectedValue = .cxaDescricaoCartao
                    txtQtde.Text = .cxaQuantidade
                    drpValor.SelectedValue = .cxaValor
                    txtVlCheque.Text = .cxaValor
                End With
                drpTipo.Focus()
        End Select
    End Sub

    Protected Sub imgApagar_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        btnSalvar.Attributes.Add("crxId", sender.commandargument.ToString)
        Dim ObjRetiradaDeCaixaVO As New RetiradaDeCaixaVO
        Dim ObjRetiradaDeCaixaDAO As New RetiradaDeCaixaDAO
        Select Case ObjRetiradaDeCaixaDAO.Excluir(ObjRetiradaDeCaixaVO, btnSalvar.Attributes.Item("Caixa").ToString, btnSalvar.Attributes.Item("crxId").ToString, btnSalvar.Attributes.Item("AliasBancoTurismo").ToString)
            Case 1
                ScriptManager.RegisterStartupScript(Page, Page.[GetType](), Guid.NewGuid().ToString(), "alert('Apagado com sucesso!');", True)
                btnAtualiza_Click(sender, e)
            Case 0
                ScriptManager.RegisterStartupScript(Page, Page.[GetType](), Guid.NewGuid().ToString(),
                "alert('Não foi possível executar esta operação. Por favor, repita esta operação novamente.');", True)
            Case -1
                ScriptManager.RegisterStartupScript(Page, Page.[GetType](), Guid.NewGuid().ToString(), "alert('Atenção! O item a ser excluído não foi encontrado, tente novamente.');", True)
            Case -2
                ScriptManager.RegisterStartupScript(Page, Page.[GetType](), Guid.NewGuid().ToString(), "alert('Atenção! O usuário atual não possui caixa aberto.');", True)
        End Select
        hddProcessando.Value = ""
    End Sub

    Protected Sub imgApagar_Click1(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        btnSalvar.Attributes.Add("crxId", sender.commandargument.ToString)
        Dim ObjRetiradaDeCaixaVO As New RetiradaDeCaixaVO
        Dim ObjRetiradaDeCaixaDAO As New RetiradaDeCaixaDAO
        Select Case ObjRetiradaDeCaixaDAO.Excluir(ObjRetiradaDeCaixaVO, btnSalvar.Attributes.Item("Caixa").ToString, btnSalvar.Attributes.Item("crxId").ToString, btnSalvar.Attributes.Item("AliasBancoTurismo").ToString)
            Case 1
                ScriptManager.RegisterStartupScript(Page, Page.[GetType](), Guid.NewGuid().ToString(), "alert('Apagado com sucesso!');", True)
                btnAtualiza_Click(sender, e)
            Case 0
                ScriptManager.RegisterStartupScript(Page, Page.[GetType](), Guid.NewGuid().ToString(),
                "alert('Não foi possível executar esta operação. Por favor, repita esta operação novamente.');", True)
            Case -1
                ScriptManager.RegisterStartupScript(Page, Page.[GetType](), Guid.NewGuid().ToString(), "alert('Atenção! O item a ser excluído não foi encontrado, tente novamente.');", True)
            Case -2
                ScriptManager.RegisterStartupScript(Page, Page.[GetType](), Guid.NewGuid().ToString(), "alert('Atenção! O usuário atual não possui caixa aberto.');", True)
        End Select
        hddProcessando.Value = ""
    End Sub

    Protected Sub imgApagar_Click2(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        btnSalvar.Attributes.Add("crxId", sender.commandargument.ToString)
        Dim ObjRetiradaDeCaixaVO As New RetiradaDeCaixaVO
        Dim ObjRetiradaDeCaixaDAO As New RetiradaDeCaixaDAO
        Select Case ObjRetiradaDeCaixaDAO.Excluir(ObjRetiradaDeCaixaVO, btnSalvar.Attributes.Item("Caixa").ToString, btnSalvar.Attributes.Item("crxId").ToString, btnSalvar.Attributes.Item("AliasBancoTurismo").ToString)
            Case 1
                ScriptManager.RegisterStartupScript(Page, Page.[GetType](), Guid.NewGuid().ToString(), "alert('Apagado com sucesso!');", True)
                btnAtualiza_Click(sender, e)
            Case 0
                ScriptManager.RegisterStartupScript(Page, Page.[GetType](), Guid.NewGuid().ToString(),
                "alert('Não foi possível executar esta operação. Por favor, repita esta operação novamente.');", True)
            Case -1
                ScriptManager.RegisterStartupScript(Page, Page.[GetType](), Guid.NewGuid().ToString(), "alert('Atenção! O item a ser excluído não foi encontrado, tente novamente.');", True)
            Case -2
                ScriptManager.RegisterStartupScript(Page, Page.[GetType](), Guid.NewGuid().ToString(), "alert('Atenção! O usuário atual não possui caixa aberto.');", True)
        End Select
        hddProcessando.Value = ""
    End Sub

    Protected Sub imgApagar_Click3(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        btnSalvar.Attributes.Add("crxId", sender.commandargument.ToString)
        Dim ObjRetiradaDeCaixaVO As New RetiradaDeCaixaVO
        Dim ObjRetiradaDeCaixaDAO As New RetiradaDeCaixaDAO
        Select Case ObjRetiradaDeCaixaDAO.Excluir(ObjRetiradaDeCaixaVO, btnSalvar.Attributes.Item("Caixa").ToString, btnSalvar.Attributes.Item("crxId").ToString, btnSalvar.Attributes.Item("AliasBancoTurismo").ToString)
            Case 1
                ScriptManager.RegisterStartupScript(Page, Page.[GetType](), Guid.NewGuid().ToString(), "alert('Apagado com sucesso!');", True)
                btnAtualiza_Click(sender, e)
            Case 0
                ScriptManager.RegisterStartupScript(Page, Page.[GetType](), Guid.NewGuid().ToString(),
                "alert('Não foi possível executar esta operação. Por favor, repita esta operação novamente.');", True)
            Case -1
                ScriptManager.RegisterStartupScript(Page, Page.[GetType](), Guid.NewGuid().ToString(), "alert('Atenção! O item a ser excluído não foi encontrado, tente novamente.');", True)
            Case -2
                ScriptManager.RegisterStartupScript(Page, Page.[GetType](), Guid.NewGuid().ToString(), "alert('Atenção! O usuário atual não possui caixa aberto.');", True)
        End Select
        hddProcessando.Value = ""
    End Sub

    Protected Sub btnImprimir_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnImprimir.Click
        Dim ObjRetiradaDeCaixaVO As New RetiradaDeCaixaVO
        Dim ObjRetiradaDeCaixaDAO As New RetiradaDeCaixaDAO
        ObjRetiradaDeCaixaVO = ObjRetiradaDeCaixaDAO.ConsultarCaixaAberto(ObjRetiradaDeCaixaVO, btnSalvar.Attributes.Item("AliasBancoTurismo").ToString, User.Identity.Name.ToString.Replace("SESC-GO.COM.BR\", ""))
        If gdvCedulas.Rows.Count <> 0 Then
            'Imprimindo o Grid de Cedulas
            hddImpressao.Value = Chr(10) & "              Setor de Recepção" & Chr(10) & Chr(10) _
                  & "              RETIRADA DE CAIXA          " & Chr(10) & Chr(10) _
                  & "Data: " & Format(CDate(ObjRetiradaDeCaixaVO.CxaCrtDAbe), "dd/MM/yyyy ") & Format(CDate(ObjRetiradaDeCaixaVO.cxacrthabe), "HH:mm:ss") & Chr(10) & Chr(10) _
                  & "Operador: " & lblOperador.Text & Chr(10) & Chr(10) _
                  & "Caixa n°: " & ObjRetiradaDeCaixaVO.cxaCrtCod.ToString & Chr(10) & Chr(10) _
                  & "---------------------------------------------" & Chr(10) & Chr(10) _
                  & "                  CÉDULAS  " & Chr(10) & Chr(10) _
                  & "QTD     V.Unit.                  Total" & Chr(10) & Chr(10) _
                  & "---------------------------------------------" & Chr(10) & Chr(10)

            For Each item As GridViewRow In gdvCedulas.Rows
                hddImpressao.Value = hddImpressao.Value & item.Cells(0).Text.PadLeft(5)
                If item.Cells(1).Text <> "&nbsp;" Then
                    hddImpressao.Value = hddImpressao.Value & FormatNumber(item.Cells(1).Text, 2).PadLeft(10)
                End If
                If item.Cells(0).Text = "Total Dinheiro:" Then
                    hddImpressao.Value = hddImpressao.Value & FormatNumber(item.Cells(2).Text, 2).PadLeft(25) & Chr(10) & Chr(10)
                Else
                    hddImpressao.Value = hddImpressao.Value & FormatNumber(item.Cells(2).Text, 2).PadLeft(25) & Chr(10) & Chr(10)
                End If
            Next
        End If
        'Imprimindo o Grid de Moedas
        If gdvMoeda.Rows.Count > 1 Then
            hddImpressao.Value = hddImpressao.Value _
                  & "---------------------------------------------" & Chr(10) & Chr(10) _
                  & "                  MOEDAS  " & Chr(10) & Chr(10) _
                  & "QTD     V.Unit.                  Total" & Chr(10) & Chr(10) _
                  & "---------------------------------------------" & Chr(10) & Chr(10)

            For Each item As GridViewRow In gdvMoeda.Rows
                hddImpressao.Value = hddImpressao.Value & item.Cells(0).Text.PadLeft(5)
                If item.Cells(1).Text <> "&nbsp;" Then
                    hddImpressao.Value = hddImpressao.Value & item.Cells(1).Text.PadLeft(10)
                End If
                If item.Cells(0).Text = "Total Moedas:" Then
                    hddImpressao.Value = hddImpressao.Value & FormatNumber(item.Cells(2).Text, 2).PadLeft(27) & Chr(10) & Chr(10)
                Else
                    hddImpressao.Value = hddImpressao.Value & FormatNumber(item.Cells(2).Text, 2).PadLeft(25) & Chr(10) & Chr(10)
                End If
            Next
        End If

        'Imprimindo o Grid de Cheques
        If gdvCheque.Rows.Count > 1 Then
            hddImpressao.Value = hddImpressao.Value _
          & "---------------------------------------------" & Chr(10) & Chr(10) _
          & "                  CHEQUES  " & Chr(10) & Chr(10) _
          & "BCO     Nº.CHEQUE                Valor" & Chr(10) & Chr(10) _
          & "---------------------------------------------" & Chr(10) & Chr(10)
            For Each item As GridViewRow In gdvCheque.Rows
                hddImpressao.Value = hddImpressao.Value & item.Cells(0).Text.PadLeft(5)
                If item.Cells(1).Text <> "&nbsp;" Then
                    hddImpressao.Value = hddImpressao.Value & item.Cells(1).Text.PadLeft(10)
                End If
                If item.Cells(0).Text = "Total Cheques:" Then
                    hddImpressao.Value = hddImpressao.Value & FormatNumber(item.Cells(2).Text, 2).PadLeft(26) & Chr(10) & Chr(10)
                Else
                    hddImpressao.Value = hddImpressao.Value & FormatNumber(item.Cells(2).Text, 2).PadLeft(25) & Chr(10) & Chr(10)
                End If
            Next
        End If
        'Imprimindo o Grid de Cartões de crédito
        If gdvCartao.Rows.Count > 1 Then
            hddImpressao.Value = hddImpressao.Value _
          & "---------------------------------------------" & Chr(10) & Chr(10) _
          & "      CARTÃO DE CRÉDITO/DÉBITO EM CONTA  " & Chr(10) & Chr(10) _
          & "Cartão              Nº Cupom       Valor" & Chr(10) & Chr(10) _
          & "---------------------------------------------" & Chr(10) & Chr(10)
            For Each item As GridViewRow In gdvCartao.Rows
                If item.Cells(0).Text = "Total Cart&#245;es:" Then
                    hddImpressao.Value = hddImpressao.Value & "Total Cartões:".PadRight(30)
                Else
                    hddImpressao.Value = hddImpressao.Value & item.Cells(0).Text.PadRight(12)
                End If
                If item.Cells(1).Text <> "&nbsp;" Then
                    hddImpressao.Value = hddImpressao.Value & item.Cells(1).Text.PadLeft(15)
                End If

                If item.Cells(0).Text = "Total Cart&#245;es:" Then
                    hddImpressao.Value = hddImpressao.Value & FormatNumber(item.Cells(2).Text, 2).PadLeft(10) & Chr(10) & Chr(10)
                Else
                    hddImpressao.Value = hddImpressao.Value & FormatNumber(item.Cells(2).Text, 2).PadLeft(13) & Chr(10) & Chr(10)
                End If
            Next
        End If
        'Finalizando a impressão
        hddImpressao.Value = hddImpressao.Value & Chr(10) & Chr(10)
        hddImpressao.Value = hddImpressao.Value _
      & "---------------------------------------------" & Chr(10) & Chr(10) _
      & "         TOTAL GERAL DA RETIRADA  " & Chr(10) & Chr(10) _
      & "---------------------------------------------" & Chr(10) & Chr(10)
        hddImpressao.Value = hddImpressao.Value _
        & "Dinheiro:".PadLeft(20) & FormatNumber(CStr(CDec(lblTotDinheiro.Text)), 2).PadLeft(15) & Chr(10) & Chr(10) _
        & "Cheques:".PadLeft(20) & FormatNumber(CStr(CDec(lblTotCheques.Text)), 2).PadLeft(15) & Chr(10) & Chr(10) _
        & "Cartões:".PadLeft(20) & FormatNumber(CStr(CDec(lblTotCartoes.Text)), 2).PadLeft(15) & Chr(10) & Chr(10) _
        & "TOTAL GERAL:".PadLeft(20) & FormatNumber(CStr(CDec(lblSomaGeral.Text)), 2).PadLeft(15) & Chr(10) & Chr(10) & Chr(10) _
        & "___________________________________________" & Chr(10) & Chr(10) _
        & lblOperador.Text & Chr(10) & Chr(10) & Chr(10) _
        & "___________________________________________" & Chr(10) & Chr(10) _
        & "Assinatura do Tesoureiro"

        ScriptManager.RegisterStartupScript(Page, Page.[GetType](), Guid.NewGuid().ToString(),
         "RetiradaDeCaixa();alert('Impressão realizada com sucesso.Para encerrar definitivamente a retirada, clique em Fechar.');", True)
        'Final do cupom'
        btnFechar.Visible = True
    End Sub

    Protected Sub btnConsultar_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnConsultar.Click
        Dim objRetiraDeCaixaVO As New RetiradaDeCaixaVO
        Dim ObjRetiraDeCaixaDAO As New RetiradaDeCaixaDAO
        'gdvRetiradas.DataSource = ObjRetiraDeCaixaDAO.VisualizaRetiradas(txtDataInicial.Text, txtDataFinal.Text, btnSalvar.Attributes.Item("AliasBancoTurismo").ToString)
        'gdvRetiradas.DataBind()
        gdvListaCaixas.DataSource = ObjRetiraDeCaixaDAO.VisualizaRetiradas(txtDataInicial.Text, txtDataFinal.Text, btnSalvar.Attributes.Item("AliasBancoTurismo").ToString)
        gdvListaCaixas.DataBind()
    End Sub

    Protected Sub btnGerVoltar_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnGerVoltar.Click
        pnlRetirada.Visible = False
        pnlGerencial.Visible = True
        'btnConsultar_Click(sender, e)
    End Sub

    Protected Sub btnGerImprimir_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnGerImprimir.Click
        'Dim ObjRetiradaDeCaixaVO As New RetiradaDeCaixaVO
        'Dim ObjRetiradaDeCaixaDAO As New RetiradaDeCaixaDAO
        'ObjRetiradaDeCaixaVO = ObjRetiradaDeCaixaDAO.ConsultarCaixaAberto(ObjRetiradaDeCaixaVO, btnSalvar.Attributes.Item("AliasBancoTurismo").ToString, User.Identity.Name.ToString.Replace("SESC-GO.COM.BR\", ""))
        If gdvCedulas.Rows.Count <> 0 Then
            'Imprimindo o Grid de Cedulas
            hddImpressao.Value = Chr(10) & "              Módulo Gerencial" & Chr(10) & Chr(10) _
                  & "              RETIRADA DE CAIXA          " & Chr(10) & Chr(10) _
                  & lblGerData.Text & Chr(10) & Chr(10) _
                  & "Operador: " & lblGerOperador.Text & Chr(10) & Chr(10) _
                  & "Caixa n°: " & lblGerCaixa.Text & Chr(10) & Chr(10) _
                  & "---------------------------------------------" & Chr(10) & Chr(10) _
                  & "                  CÉDULAS  " & Chr(10) & Chr(10) _
                  & "QTD     V.Unit.                  Total" & Chr(10) & Chr(10) _
                  & "---------------------------------------------" & Chr(10) & Chr(10)

            For Each item As GridViewRow In gdvCedulas.Rows
                hddImpressao.Value = hddImpressao.Value & item.Cells(0).Text.PadLeft(5)
                If item.Cells(1).Text <> "&nbsp;" Then
                    hddImpressao.Value = hddImpressao.Value & FormatNumber(item.Cells(1).Text, 2).PadLeft(10)
                End If
                If item.Cells(0).Text = "Total Dinheiro:" Then
                    hddImpressao.Value = hddImpressao.Value & FormatNumber(item.Cells(2).Text, 2).PadLeft(25) & Chr(10) & Chr(10)
                Else
                    hddImpressao.Value = hddImpressao.Value & FormatNumber(item.Cells(2).Text, 2).PadLeft(25) & Chr(10) & Chr(10)
                End If
            Next
        End If
        'Imprimindo o Grid de Moedas
        If gdvMoeda.Rows.Count > 1 Then
            hddImpressao.Value = hddImpressao.Value _
                  & "---------------------------------------------" & Chr(10) & Chr(10) _
                  & "                  MOEDAS  " & Chr(10) & Chr(10) _
                  & "QTD     V.Unit.                  Total" & Chr(10) & Chr(10) _
                  & "---------------------------------------------" & Chr(10) & Chr(10)

            For Each item As GridViewRow In gdvMoeda.Rows
                hddImpressao.Value = hddImpressao.Value & item.Cells(0).Text.PadLeft(5)
                If item.Cells(1).Text <> "&nbsp;" Then
                    hddImpressao.Value = hddImpressao.Value & item.Cells(1).Text.PadLeft(10)
                End If
                If item.Cells(0).Text = "Total Moedas:" Then
                    hddImpressao.Value = hddImpressao.Value & FormatNumber(item.Cells(2).Text, 2).PadLeft(27) & Chr(10) & Chr(10)
                Else
                    hddImpressao.Value = hddImpressao.Value & FormatNumber(item.Cells(2).Text, 2).PadLeft(25) & Chr(10) & Chr(10)
                End If
            Next
        End If

        'Imprimindo o Grid de Cheques
        If gdvCheque.Rows.Count > 1 Then
            hddImpressao.Value = hddImpressao.Value _
          & "---------------------------------------------" & Chr(10) & Chr(10) _
          & "                  CHEQUES  " & Chr(10) & Chr(10) _
          & "BCO     Nº.CHEQUE                Valor" & Chr(10) & Chr(10) _
          & "---------------------------------------------" & Chr(10) & Chr(10)
            For Each item As GridViewRow In gdvCheque.Rows
                hddImpressao.Value = hddImpressao.Value & item.Cells(0).Text.PadLeft(5)
                If item.Cells(1).Text <> "&nbsp;" Then
                    hddImpressao.Value = hddImpressao.Value & item.Cells(1).Text.PadLeft(10)
                End If
                If item.Cells(0).Text = "Total Cheques:" Then
                    hddImpressao.Value = hddImpressao.Value & FormatNumber(item.Cells(2).Text, 2).PadLeft(26) & Chr(10) & Chr(10)
                Else
                    hddImpressao.Value = hddImpressao.Value & FormatNumber(item.Cells(2).Text, 2).PadLeft(25) & Chr(10) & Chr(10)
                End If
            Next
        End If
        'Imprimindo o Grid de Cartões de crédito
        If gdvCartao.Rows.Count > 1 Then
            hddImpressao.Value = hddImpressao.Value _
          & "---------------------------------------------" & Chr(10) & Chr(10) _
          & "      CARTÃO DE CRÉDITO/DÉBITO EM CONTA  " & Chr(10) & Chr(10) _
          & "Cartão              Nº Cupom       Valor" & Chr(10) & Chr(10) _
          & "---------------------------------------------" & Chr(10) & Chr(10)
            For Each item As GridViewRow In gdvCartao.Rows
                If item.Cells(0).Text = "Total Cart&#245;es:" Then
                    hddImpressao.Value = hddImpressao.Value & "Total Cartões:".PadRight(30)
                Else
                    hddImpressao.Value = hddImpressao.Value & item.Cells(0).Text.PadRight(12)
                End If
                If item.Cells(1).Text <> "&nbsp;" Then
                    hddImpressao.Value = hddImpressao.Value & item.Cells(1).Text.PadLeft(15)
                End If

                If item.Cells(0).Text = "Total Cart&#245;es:" Then
                    hddImpressao.Value = hddImpressao.Value & FormatNumber(item.Cells(2).Text, 2).PadLeft(10) & Chr(10) & Chr(10)
                Else
                    hddImpressao.Value = hddImpressao.Value & FormatNumber(item.Cells(2).Text, 2).PadLeft(13) & Chr(10) & Chr(10)
                End If
            Next
        End If
        'Finalizando a impressão
        hddImpressao.Value = hddImpressao.Value & Chr(10) & Chr(10)
        hddImpressao.Value = hddImpressao.Value _
      & "---------------------------------------------" & Chr(10) & Chr(10) _
      & "         TOTAL GERAL DA RETIRADA  " & Chr(10) & Chr(10) _
      & "---------------------------------------------" & Chr(10) & Chr(10)
        hddImpressao.Value = hddImpressao.Value _
        & "Dinheiro:".PadLeft(20) & FormatNumber(CStr(CDec(lblTotDinheiro.Text)), 2).PadLeft(15) & Chr(10) & Chr(10) _
        & "Cheques:".PadLeft(20) & FormatNumber(CStr(CDec(lblTotCheques.Text)), 2).PadLeft(15) & Chr(10) & Chr(10) _
        & "Cartões:".PadLeft(20) & FormatNumber(CStr(CDec(lblTotCartoes.Text)), 2).PadLeft(15) & Chr(10) & Chr(10) _
        & "TOTAL GERAL:".PadLeft(20) & FormatNumber(CStr(CDec(lblSomaGeral.Text)), 2).PadLeft(15) & Chr(10) & Chr(10) & Chr(10) _
        & "2º Via " & Chr(10) & Chr(10) _
        & "Impresso pelo Módulo gerencial" & Chr(10) & Chr(10) _
        & "Por: " & User.Identity.Name.ToString.Replace("SESC-GO.COM.BR\", "") & Chr(10) & Chr(10) _
        & "Em:" & Format(Now, "dd/MM/yyyy HH:mm:ss")

        ScriptManager.RegisterStartupScript(Page, Page.[GetType](), Guid.NewGuid().ToString(),
         "RetiradaDeCaixa();alert('Impressão realizada com sucesso.');", True)
        'Final do cupom'

    End Sub

    Protected Sub btnNova_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnNova.Click
        Dim ObjRetiradaDeCaixaVO As New RetiradaDeCaixaVO
        Dim ObjRetiradaDeCaixaDAO As New RetiradaDeCaixaDAO
        Dim Resultado As Integer = ObjRetiradaDeCaixaDAO.InserirCabecalho(ObjRetiradaDeCaixaVO, btnSalvar.Attributes.Item("Caixa"), User.Identity.Name.ToString.Replace("SESC-GO.COM.BR\", ""), btnSalvar.Attributes.Item("AliasBancoTurismo"), "A", 0.0)
        'Carregando objeto
        Select Case Resultado
            Case 0
                ScriptManager.RegisterStartupScript(Page, Page.[GetType](), Guid.NewGuid().ToString(), "alert('Atenção! Houve um erro na criação do cabelho da retirada. Informe o Centro de Informática.');", True)
            Case Is > 3
                lblNumeroCaixa.Text = btnSalvar.Attributes.Item("Caixa") & "/Retirada nº:" & Resultado
                btnSalvar.Attributes.Add("IdRetirada", Resultado)
                lblTipo.Visible = True
                drpTipo.Visible = True
                btnImprimir.Visible = True
                btnSalvar.Visible = True
                btnAtualiza.Visible = True
                btnNova.Visible = False
                btnFechar.Visible = False
                drpTipo.Focus()
            Case Else
                lblTipo.Visible = True
                drpTipo.Visible = True
                btnImprimir.Visible = True
                btnSalvar.Visible = True
                btnAtualiza.Visible = True
                btnNova.Visible = False
                btnFechar.Visible = False
                drpTipo.Focus()
        End Select
    End Sub

    Protected Sub btnFechar_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnFechar.Click
        'Fechando a Retirada'
        Dim ObjRetiradaDeCaixaVO As New RetiradaDeCaixaVO
        Dim ObjRetiradaDeCaixaDAO As New RetiradaDeCaixaDAO
        'Carregando objeto
        Select Case ObjRetiradaDeCaixaDAO.InserirCabecalho(ObjRetiradaDeCaixaVO, btnSalvar.Attributes.Item("Caixa"), User.Identity.Name.ToString.Replace("SESC-GO.COM.BR\", ""), btnSalvar.Attributes.Item("AliasBancoTurismo"), "F", lblSomaGeral.Text.Replace(".", "").Replace(",", "."))
            Case 0
                ScriptManager.RegisterStartupScript(Page, Page.[GetType](), Guid.NewGuid().ToString(), "alert('Atenção! Houve um erro na criação do cabelho da retirada. Informe o Centro de Informática.');", True)
            Case 2
                ScriptManager.RegisterStartupScript(Page, Page.[GetType](), Guid.NewGuid().ToString(), "alert('Fechamento realizado com sucesso.');", True)
                'Refresh na tela
                Response.Redirect("frmRetiradaDeCaixa.aspx")
        End Select
    End Sub

    Protected Sub gdvListaCaixas_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gdvListaCaixas.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            'PEGANDO O NOME COMPLETO E MATRICULA DO USUÁRIO LOGADO NO MOMENTO'
            Dim search As DirectoryServices.DirectorySearcher = New DirectoryServices.DirectorySearcher("LDAP://sesc-go.com.br/DC=sesc-go,DC=com,DC=br")
            search.Filter = "(SAMAccountName=" + e.Row.Cells(1).Text + ")"
            Dim result As DirectoryServices.SearchResult = search.FindOne()
            search.PropertiesToLoad.Add("displayName")
            If result Is Nothing Then
                e.Row.Cells(1).Text = e.Row.Cells(1).Text
            Else
                e.Row.Cells(1).Text = result.Properties("displayName").Item(0).ToString.Replace("- CNV", "").ToUpper
            End If
        End If
    End Sub

    Protected Sub gdvListaRetiradas_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim IdRetirada As String = sender.commandargument.ToString  'btnSalvar.Attributes.Item("IdRetirada")
        Dim ObjRetiradaDeCaixaVO As New RetiradaDeCaixaVO
        Dim ObjRetiradaDeCaixaDAO As New RetiradaDeCaixaDAO
        pnlCabecalho.Visible = False
        pnlCabecalhoGerencial.Visible = True
        'Consultando Dinheiro
        'gdvCedulas.DataSource = ObjRetiradaDeCaixaDAO.ConsultaCedulas(ObjRetiradaDeCaixaVO, btnSalvar.Attributes.Item("AliasBancoTurismo").ToString, btnSalvar.Attributes.Item("Operador"), btnSalvar.Attributes.Item("Caixa").ToString, "S", btnSalvar.Attributes.Item("IdRetirada"))
        'gdvCamareiras.DataKeys(gdvCamareiras.SelectedIndex).Item(1).ToString()
        'gdvCedulas.DataBind()
        'Consultando moedas
        'gdvMoeda.DataSource = ObjRetiradaDeCaixaDAO.ConsultaMoedas(ObjRetiradaDeCaixaVO, btnSalvar.Attributes.Item("AliasBancoTurismo").ToString, gdvRetiradas.DataKeys(gdvRetiradas.SelectedIndex).Item(2).ToString, btnSalvar.Attributes.Item("Caixa").ToString, "S", btnSalvar.Attributes.Item("IdRetirada"))
        'gdvMoeda.DataBind()
        'Consultando cheques
        'gdvCheque.DataSource = ObjRetiradaDeCaixaDAO.ConsultaCheques(ObjRetiradaDeCaixaVO, btnSalvar.Attributes.Item("AliasBancoTurismo").ToString, gdvRetiradas.DataKeys(gdvRetiradas.SelectedIndex).Item(2).ToString, btnSalvar.Attributes.Item("Caixa").ToString, "S", btnSalvar.Attributes.Item("IdRetirada"))
        'gdvCheque.DataBind()
        'Consultando Cartões
        'gdvCartao.DataSource = ObjRetiradaDeCaixaDAO.ConsultaCartoes(ObjRetiradaDeCaixaVO, btnSalvar.Attributes.Item("AliasBancoTurismo").ToString, gdvRetiradas.DataKeys(gdvRetiradas.SelectedIndex).Item(2).ToString, btnSalvar.Attributes.Item("Caixa").ToString, "S", btnSalvar.Attributes.Item("IdRetirada"))
        'gdvCartao.DataBind()
        'Totalizando 
        'Totalizando a retirada
        'lblTotDinheiro.Text = FormatNumber(CDec(btnSalvar.Attributes.Item("TotCedulas").ToString) + CDec(btnSalvar.Attributes.Item("TotMoedas").ToString), 2)
        'lblTotCheques.Text = FormatNumber(btnSalvar.Attributes.Item("TotCheques").ToString, 2)
        'lblTotCartoes.Text = FormatNumber(btnSalvar.Attributes.Item("TotCartoes").ToString, 2)
        'lblSomaGeral.Text = FormatNumber(CDec(btnSalvar.Attributes.Item("TotCedulas").ToString) + CDec(btnSalvar.Attributes.Item("TotMoedas").ToString) + CDec(btnSalvar.Attributes.Item("TotCheques").ToString) + CDec(btnSalvar.Attributes.Item("TotCartoes").ToString), 2)
        'visualizando o caixa
        pnlGerencial.Visible = False
        pnlRetirada.Visible = True
    End Sub

    Protected Sub gdvListaRetiradas_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs)
        'Adicionando o IdRetirada na imagem'
        If e.Row.RowType = DataControlRowType.DataRow Then
            CType(e.Row.FindControl("imgAlterar"), ImageButton).CommandArgument = e.Row.Cells(0).Text
            btnSalvar.Attributes.Add("idRetirada", e.Row.Cells(0).Text)
        End If
    End Sub

    Protected Sub imgAlterar_Click4(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        Dim IdRetirada As String = sender.commandargument.ToString
        Dim ObjRetiradaDeCaixaVO As New RetiradaDeCaixaVO
        Dim ObjRetiradaDeCaixaDAO As New RetiradaDeCaixaDAO
        pnlCabecalho.Visible = False
        pnlCabecalhoGerencial.Visible = True
        lblGerCaixa.Text = lblGerCaixa.Text & "/Retirada: " & IdRetirada
        'Consultando Dinheiro
        gdvCedulas.DataSource = ObjRetiradaDeCaixaDAO.ConsultaCedulas(ObjRetiradaDeCaixaVO, btnSalvar.Attributes.Item("AliasBancoTurismo").ToString, btnSalvar.Attributes.Item("OperadorGerencial"), btnSalvar.Attributes.Item("Caixa").ToString, "S", IdRetirada)
        gdvCedulas.DataBind()
        If gdvCedulas.Rows.Count = 0 Then
            btnSalvar.Attributes.Add("TotCedulas", 0)
        End If
        'Consultando moedas
        gdvMoeda.DataSource = ObjRetiradaDeCaixaDAO.ConsultaMoedas(ObjRetiradaDeCaixaVO, btnSalvar.Attributes.Item("AliasBancoTurismo").ToString, btnSalvar.Attributes.Item("OperadorGerencial"), btnSalvar.Attributes.Item("Caixa").ToString, "S", IdRetirada)
        gdvMoeda.DataBind()
        If gdvMoeda.Rows.Count = 0 Then
            btnSalvar.Attributes.Add("TotMoedas", 0)
        End If
        'Consultando cheques
        gdvCheque.DataSource = ObjRetiradaDeCaixaDAO.ConsultaCheques(ObjRetiradaDeCaixaVO, btnSalvar.Attributes.Item("AliasBancoTurismo").ToString, btnSalvar.Attributes.Item("OperadorGerencial"), btnSalvar.Attributes.Item("Caixa").ToString, "S", IdRetirada)
        gdvCheque.DataBind()
        If gdvCheque.Rows.Count = 0 Then
            btnSalvar.Attributes.Add("TotCheques", 0)
        End If
        'Consultando Cartões
        gdvCartao.DataSource = ObjRetiradaDeCaixaDAO.ConsultaCartoes(ObjRetiradaDeCaixaVO, btnSalvar.Attributes.Item("AliasBancoTurismo").ToString, btnSalvar.Attributes.Item("OperadorGerencial"), btnSalvar.Attributes.Item("Caixa").ToString, "S", IdRetirada)
        gdvCartao.DataBind()
        If gdvCartao.Rows.Count = 0 Then
            btnSalvar.Attributes.Add("TotCartoes", 0)
        End If
        'Totalizando 
        'Totalizando a retirada
        lblTotDinheiro.Text = FormatNumber(CDec(btnSalvar.Attributes.Item("TotCedulas").ToString) + CDec(btnSalvar.Attributes.Item("TotMoedas").ToString), 2)
        lblTotCheques.Text = FormatNumber(btnSalvar.Attributes.Item("TotCheques").ToString, 2)
        lblTotCartoes.Text = FormatNumber(btnSalvar.Attributes.Item("TotCartoes").ToString, 2)
        lblSomaGeral.Text = FormatNumber(CDec(btnSalvar.Attributes.Item("TotCedulas").ToString) + CDec(btnSalvar.Attributes.Item("TotMoedas").ToString) + CDec(btnSalvar.Attributes.Item("TotCheques").ToString) + CDec(btnSalvar.Attributes.Item("TotCartoes").ToString), 2)
        'visualizando o caixa
        pnlGerencial.Visible = False
        pnlRetirada.Visible = True
    End Sub

    Protected Sub gdvListaCaixas_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles gdvListaCaixas.SelectedIndexChanged
        'Varrendo o grid e escondendo o 2° grid das retiradas
        For Each linha As GridViewRow In gdvListaCaixas.Rows
            'Ira passar somente nos apartamentos checados
            If CType(linha.FindControl("gdvListaRetiradas"), GridView).Visible = True Then
                CType(linha.FindControl("gdvListaRetiradas"), GridView).Visible = False
            End If
        Next
        btnSalvar.Attributes.Add("Caixa", gdvListaCaixas.DataKeys(gdvListaCaixas.SelectedIndex).Item(0).ToString())
        lblGerData.Text = "Retirada em: " & gdvListaCaixas.DataKeys(gdvListaCaixas.SelectedIndex).Item(2).ToString
        lblGerOperador.Text = gdvListaCaixas.SelectedRow.Cells.Item(1).Text
        lblGerCaixa.Text = gdvListaCaixas.DataKeys(gdvListaCaixas.SelectedIndex).Item(0).ToString
        btnSalvar.Attributes.Add("OperadorGerencial", gdvListaCaixas.DataKeys(gdvListaCaixas.SelectedIndex).Item(1).ToString)
        Dim ObjRetiradaDeCaixaVO As New RetiradaDeCaixaVO
        Dim ObjRetiradaDeCaixaDAO As New RetiradaDeCaixaDAO
        CType(gdvListaCaixas.Rows(gdvListaCaixas.SelectedIndex).FindControl("gdvListaRetiradas"), GridView).Visible = True
        CType(gdvListaCaixas.Rows(gdvListaCaixas.SelectedIndex).FindControl("gdvListaRetiradas"), GridView).DataSource = ObjRetiradaDeCaixaDAO.ConsultaRetiradasPorCaixa(ObjRetiradaDeCaixaVO, btnSalvar.Attributes.Item("Caixa"), btnSalvar.Attributes.Item("AliasBancoTurismo"), txtDataInicial.Text, txtDataFinal.Text)
        CType(gdvListaCaixas.Rows(gdvListaCaixas.SelectedIndex).FindControl("gdvListaRetiradas"), GridView).DataBind()
    End Sub
    Protected Sub enviarEmailGenerico(Mensagem As String)
        Try
            Dim objEmail As New System.Net.Mail.MailMessage()
            objEmail.Subject = "Retirada de Caixa (Mensagem de erro) "
            objEmail.To.Add(New System.Net.Mail.MailAddress("elvis.irineu@sescgo.com.br"))
            objEmail.CC.Add(New System.Net.Mail.MailAddress("gustavo.cesar@sescgo.com.br"))
            'objEmail.To.Add(New MailAddress(objUsuarioRedeVO.mail))
            objEmail.IsBodyHtml = True

            Dim objSmtp As SmtpClient
            '***ENDEREÇO DO SERVIDOR DO OFFICE 365***
            'Ao modificar para o Office 365, descomentar as configurações abaixo tbm
            objSmtp = New SmtpClient("sescgo-com-br.mail.protection.outlook.com", 25)
            objSmtp.EnableSsl = True
            objSmtp.Credentials = New System.Net.NetworkCredential("postmaster@sescgo.com.br", "l!xg:@2STEr?W=J")
            objSmtp.UseDefaultCredentials = False
            objSmtp.Timeout = 5000

            Select Case Session("MasterPage").ToString
                Case Is = "~/TurismoSocial.Master"
                    objEmail.From = New System.Net.Mail.MailAddress("reservas.caldasnovas@sescgo.com.br ")
                Case Else
                    'Direcionando a aplicação para o banco de Pirenopolis
                    objEmail.From = New System.Net.Mail.MailAddress("reservas.pirenopolis@sescgo.com.br ")
            End Select


            'objEmail.Bcc.Add("haas@sescgo.com.br")

            Dim sEmail As New StringBuilder
            sEmail.Append("<p />" & Mensagem)
            objEmail.IsBodyHtml = True
            objEmail.Body = sEmail.ToString
            'objEmail.Priority = System.Net.Mail.MailPriority.Normal
            'anexa logotipo ao e-mail
            'Dim vw As AlternateView
            'vw = AlternateView.CreateAlternateViewFromString("<br><img src=""cid:imagem""><br/><br/>" & sEmail.ToString, Nothing, "text/html")
            'Dim logo As New LinkedResource(Server.MapPath(".") & "\images\logosesc.jpg")
            'logo.ContentId = "imagem"
            'vw.LinkedResources.Add(logo)
            'objEmail.AlternateViews.Add(vw)
            objSmtp.Send(objEmail)
        Catch ex As Exception
            Response.Redirect("erroEmail.aspx?erro=Ocorreu um erro em sua solicitação&excecao=" & ex.StackTrace.ToString, False)
        End Try
    End Sub
End Class
