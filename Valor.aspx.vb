Partial Class Valor
    Inherits System.Web.UI.Page
    Dim objGrupoDAO As Turismo.GrupoDAO
    Dim objGrupoVO As Turismo.GrupoVO
    Dim objValorDAO As Turismo.ValorDAO
    Dim objValorVO As Turismo.ValorVO
    Dim objPacoteDAO As Turismo.PacoteDAO
    Dim objPacoteVO As New Turismo.PacoteVO
    Dim objCadastraValoresResrevaDAO As CadastraValoresReservaDAO

    Protected Sub Page_PreInit(ByVal sender As Object, ByVal e As EventArgs)
        If Session("MasterPage").ToString = "~/TurismoSocial.Master" Then
            Page.MasterPageFile = "~/TurismoSocial.Master"
        Else
            Page.MasterPageFile = "~/TurismoSocialPiri.Master"
        End If
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            If Not Session("GrupoRanking") Then
                Response.Redirect("AcessoNegado.aspx")
            Else
                If Session("MasterPage").ToString = "~/TurismoSocial.Master" Then
                    objGrupoDAO = New Turismo.GrupoDAO("TurismoSocialCaldas")
                Else
                    objGrupoDAO = New Turismo.GrupoDAO("TurismoSocialPiri")
                End If
                cmbTipo.Visible = Not (Session("MasterPage").ToString = "~/TurismoSocial.Master")
                cmbTipo.DataSource = objGrupoDAO.consultar()
                cmbTipo.DataValueField = ("gruId")
                cmbTipo.DataTextField = ("gruDescricao")
                cmbTipo.DataBind()
                cmbAcmId.DataSource = cmbTipo.DataSource
                cmbAcmId.DataValueField = ("acmId")
                cmbAcmId.DataTextField = ("acmId")
                cmbAcmId.DataBind()
                cmbTipo_SelectedIndexChanged(sender, e)
            End If

            txtDataInicial.Text = "01/01/" & Date.Now.Year.ToString
            txtDataFinal.Text = "31/12/" & Date.Now.Year.ToString

            txtDesConData1.Text = "01/01/" & Date.Now.Year.ToString
            txtDesConData2.Text = "31/12/2050"

            btnConsulta_Click(sender, e)
            btnConsultaDesconto_Click(sender, e)

        End If

        'txtPercentualAcao.Enabled = (Session("MasterPage").ToString = "~/TurismoSocial.Master")

        txtPercentualAcao.Attributes.Add("onkeyup", "javascript:if((window.event.keyCode != 9) && (window.event.keyCode != 16)) {this.value=ColocaVirgula(this)}")

        txtPercenteSegunda.Attributes.Add("onkeyup", "javascript:if((window.event.keyCode != 9) && (window.event.keyCode != 16)) {this.value=ColocaVirgula(this)}")
        txtPercenteTerca.Attributes.Add("onkeyup", "javascript:if((window.event.keyCode != 9) && (window.event.keyCode != 16)) {this.value=ColocaVirgula(this)}")
        txtPercenteQuarta.Attributes.Add("onkeyup", "javascript:if((window.event.keyCode != 9) && (window.event.keyCode != 16)) {this.value=ColocaVirgula(this)}")
        txtPercenteQuinta.Attributes.Add("onkeyup", "javascript:if((window.event.keyCode != 9) && (window.event.keyCode != 16)) {this.value=ColocaVirgula(this)}")
        txtPercenteSexta.Attributes.Add("onkeyup", "javascript:if((window.event.keyCode != 9) && (window.event.keyCode != 16)) {this.value=ColocaVirgula(this)}")
        txtPercenteSabado.Attributes.Add("onkeyup", "javascript:if((window.event.keyCode != 9) && (window.event.keyCode != 16)) {this.value=ColocaVirgula(this)}")
        txtPercenteDomingo.Attributes.Add("onkeyup", "javascript:if((window.event.keyCode != 9) && (window.event.keyCode != 16)) {this.value=ColocaVirgula(this)}")

        txtData.Attributes.Add("onKeyDown", "javascript:desabilitaEnter();")
        txtData.Attributes.Add("onKeyPress", "javascript:return FormataData(this,event);")
        txtData.Attributes.Add("OnChange", "javascript:if(!ValidaData(this,'N')){alert('Data inválida!');this.value='';this.focus();}")

        txtDesConData1.Attributes.Add("onKeyDown", "javascript:desabilitaEnter();")
        txtDesConData1.Attributes.Add("onKeyPress", "javascript:return FormataData(this,event);")
        txtDesConData1.Attributes.Add("OnChange", "javascript:if(!ValidaData(this,'N')){alert('Data inválida!');this.value='';this.focus();}")
        txtDesConData2.Attributes.Add("onKeyDown", "javascript:desabilitaEnter();")
        txtDesConData2.Attributes.Add("onKeyPress", "javascript:return FormataData(this,event);")
        txtDesConData2.Attributes.Add("OnChange", "javascript:if(!ValidaData(this,'N')){alert('Data inválida!');this.value='';this.focus();}")

        txtDataIniciaDesconto.Attributes.Add("onKeyDown", "javascript:desabilitaEnter();")
        txtDataIniciaDesconto.Attributes.Add("onKeyPress", "javascript:return FormataData(this,event);")
        txtDataIniciaDesconto.Attributes.Add("OnChange", "javascript:if(!ValidaData(this,'N')){alert('Data inválida!');this.value='';this.focus();}")

        txtDataFinalDesconto.Attributes.Add("onKeyDown", "javascript:desabilitaEnter();")
        txtDataFinalDesconto.Attributes.Add("onKeyPress", "javascript:return FormataData(this,event);")
        txtDataFinalDesconto.Attributes.Add("OnChange", "javascript:if(!ValidaData(this,'N')){alert('Data inválida!');this.value='';this.focus();}")

        txtDataInicial.Attributes.Add("onKeyDown", "javascript:desabilitaEnter();")
        txtDataInicial.Attributes.Add("onKeyPress", "javascript:return FormataData(this,event);")
        txtDataInicial.Attributes.Add("OnChange", "javascript:if(!ValidaData(this,'N')){alert('Data inválida!');this.value='';this.focus();}")

        txtDataFinal.Attributes.Add("onKeyDown", "javascript:desabilitaEnter();")
        txtDataFinal.Attributes.Add("onKeyPress", "javascript:return FormataData(this,event);")
        txtDataFinal.Attributes.Add("OnChange", "javascript:if(!ValidaData(this,'N')){alert('Data inválida!');this.value='';this.focus();}")

        txtDataAntiga.Attributes.Add("onKeyDown", "javascript:desabilitaEnter();")
        txtDataAntiga.Attributes.Add("onKeyPress", "javascript:return FormataData(this,event);")
        txtDataAntiga.Attributes.Add("OnChange", "javascript:if(!ValidaData(this,'N')){alert('Data inválida!');this.value='';this.focus();}")

        txtDataInicialAcao.Attributes.Add("onKeyDown", "javascript:desabilitaEnter();")
        txtDataInicialAcao.Attributes.Add("onKeyPress", "javascript:return FormataData(this,event);")
        txtDataInicialAcao.Attributes.Add("OnChange", "javascript:if(!ValidaData(this,'N')){alert('Data inválida!');this.value='';this.focus();}")

        txtDataInicialAcao.Attributes.Add("onKeyDown", "javascript:desabilitaEnter();")
        txtDataInicialAcao.Attributes.Add("onKeyPress", "javascript:return FormataData(this,event);")
        txtDataInicialAcao.Attributes.Add("OnChange", "javascript:if(!ValidaData(this,'N')){alert('Data inválida!');this.value='';this.focus();}")

        txtDataFinalAcao.Attributes.Add("onKeyDown", "javascript:desabilitaEnter();")
        txtDataFinalAcao.Attributes.Add("onKeyPress", "javascript:return FormataData(this,event);")
        txtDataFinalAcao.Attributes.Add("OnChange", "javascript:if(!ValidaData(this,'N')){alert('Data inválida!');this.value='';this.focus();}")

        txtDataInicialVigenciaAcao.Attributes.Add("onKeyDown", "javascript:desabilitaEnter();")
        txtDataInicialVigenciaAcao.Attributes.Add("onKeyPress", "javascript:return FormataData(this,event);")
        txtDataInicialVigenciaAcao.Attributes.Add("OnChange", "javascript:if(!ValidaData(this,'N')){alert('Data inválida!');this.value='';this.focus();}")

        txtDataFinalVigenciaAcao.Attributes.Add("onKeyDown", "javascript:desabilitaEnter();")
        txtDataFinalVigenciaAcao.Attributes.Add("onKeyPress", "javascript:return FormataData(this,event);")
        txtDataFinalVigenciaAcao.Attributes.Add("OnChange", "javascript:if(!ValidaData(this,'N')){alert('Data inválida!');this.value='';this.focus();}")
    End Sub

    Protected Sub cmbTipo_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cmbTipo.SelectedIndexChanged
        Try
            cmbAcmId.SelectedIndex = cmbTipo.SelectedIndex
            Dim lista As IList
            Dim listaComerciarioN As New ArrayList
            Dim listaConNorN As New ArrayList
            Dim listaUsuNorN As New ArrayList
            Dim varDataAntiga As String = ""
            Dim varHora As String = "00"
            Dim varDataNova As String = ""
            'Dim listaComSuiN As New ArrayList
            'Dim listaConSuiN As New ArrayList
            'Dim listaUsuSuiN As New ArrayList

            Dim listaComNorA As New ArrayList
            Dim listaConNorA As New ArrayList
            Dim listaUsuNorA As New ArrayList
            'Dim listaComSuiA As New ArrayList
            'Dim listaConSuiA As New ArrayList
            'Dim listaUsuSuiA As New ArrayList

            If Session("MasterPage").ToString = "~/TurismoSocial.Master" Then
                objValorDAO = New Turismo.ValorDAO("TurismoSocialCaldas")
            Else
                objValorDAO = New Turismo.ValorDAO("TurismoSocialPiri")
            End If
            lista = objValorDAO.consultar(cmbTipo.SelectedValue)
            For Each item As Turismo.ValorVO In lista
                If item.catLink = 1 And item.suite = "N" And item.tipo = "N" Then
                    listaComerciarioN.Add(item)
                End If
                If item.catLink = 3 And item.suite = "N" And item.tipo = "N" Then
                    listaConNorN.Add(item)
                End If
                If item.catLink = 4 And item.suite = "N" And item.tipo = "N" Then
                    listaUsuNorN.Add(item)
                End If
                'If item.catLink = 1 And item.suite = "N" And item.tipo = "N" Then
                '    listaComSuiN.Add(item)
                'End If
                'If item.catLink = 3 And item.suite = "N" And item.tipo = "N" Then
                '    listaConSuiN.Add(item)
                'End If
                'If item.catLink = 4 And item.suite = "N" And item.tipo = "N" Then
                '    listaUsuSuiN.Add(item)
                'End If

                If item.catLink = 1 And item.suite = "N" And item.tipo = "A" Then
                    listaComNorA.Add(item)
                End If
                If item.catLink = 3 And item.suite = "N" And item.tipo = "A" Then
                    listaConNorA.Add(item)
                End If
                If item.catLink = 4 And item.suite = "N" And item.tipo = "A" Then
                    listaUsuNorA.Add(item)
                End If
                'If item.catLink = 1 And item.suite = "N" And item.tipo = "A" Then
                '    listaComSuiA.Add(item)
                'End If
                'If item.catLink = 3 And item.suite = "N" And item.tipo = "A" Then
                '    listaConSuiA.Add(item)
                'End If
                'If item.catLink = 4 And item.suite = "N" And item.tipo = "A" Then
                '    listaUsuSuiA.Add(item)
                'End If

                'Desativada por Washington em 29-08-2018
                'varDataAntiga = item.dia
                'varHora = item.hora
                'varDataNova = item.valDtInicial

                If item.tipo = "N" Then
                    varDataNova = item.dia
                ElseIf item.tipo = "A" Then
                    varDataAntiga = item.dia
                    varHora = item.hora
                End If
            Next

            txtDataAntiga.Text = varDataAntiga
            hddDataAntiga.Value = txtDataAntiga.Text
            txtData.Text = varDataNova
            'hddData.Value = Format(CDate(DateAdd(DateInterval.Day, 1, CDate(txtData.Text))), "dd/MM/yyyy")
            hddData.Value = txtData.Text
            cmbHora.SelectedValue = varHora
            hddHoraAntiga.Value = varHora

            gdvComerciario.DataSource = listaComerciarioN
            gdvComerciario.DataBind()
            gdvConveniado.DataSource = listaConNorN
            gdvConveniado.DataBind()
            gdvUsuario.DataSource = listaUsuNorN
            gdvUsuario.DataBind()

            gdvComerciarioA.DataSource = listaComNorA
            gdvComerciarioA.DataBind()
            gdvConveniadoA.DataSource = listaConNorA
            gdvConveniadoA.DataBind()
            gdvUsuarioA.DataSource = listaUsuNorA
            gdvUsuarioA.DataBind()

            ''Voltando os grids ao modo inicial para uma nova inserção
            'gdvComerciario.Enabled = False
            'gdvConveniado.Enabled = False
            'gdvUsuario.Enabled = False
            'btnReservaGravar.Visible = False
            'btnTransportar.Visible = True
            ''Deixando os grid antigos desabilitado
            'gdvComerciarioA.Enabled = True
            'gdvConveniadoA.Enabled = True
            'gdvUsuarioA.Enabled = True
        Catch ex As Exception

        End Try
    End Sub

    Protected Sub gdvConveniadoNormalA_RowDataBound(sender As Object, e As GridViewRowEventArgs) Handles gdvConveniadoA.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            CType(e.Row.FindControl("lblConNorA"), Label).Text = Replace(gdvConveniadoA.DataKeys(e.Row.RowIndex).Item("catDescricao"), "Conveniado", "")
            CType(e.Row.FindControl("txtVlrConNorA"), TextBox).Text = Format(CDec(gdvConveniadoA.DataKeys(e.Row.RowIndex).Item("valValor")), "#,##0.00")
            CType(e.Row.FindControl("txtVlrConNorA"), TextBox).Attributes.Add("onkeyup", "javascript:if((window.event.keyCode != 9) && (window.event.keyCode != 16)) {this.value=ColocaVirgula(this)}")
            CType(e.Row.FindControl("lblVlrConSuiA"), Label).Text = Format(CDec(gdvConveniadoA.DataKeys(e.Row.RowIndex).Item("valValor") * 1.3), "#,##0.00")
            CType(e.Row.FindControl("txtVlrConNorA"), TextBox).Attributes.Add("catId", gdvConveniadoA.DataKeys(e.Row.RowIndex).Item("catId"))
            CType(e.Row.FindControl("txtVlrConNorA"), TextBox).Attributes.Add("valQtde", gdvConveniadoA.DataKeys(e.Row.RowIndex).Item("valQtde"))
        End If
    End Sub

    Protected Sub gdvUsuarioNormalA_RowDataBound(sender As Object, e As GridViewRowEventArgs) Handles gdvUsuarioA.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            CType(e.Row.FindControl("lblUsuNorA"), Label).Text = Replace(gdvUsuarioA.DataKeys(e.Row.RowIndex).Item("catDescricao"), "Usuário", "")
            CType(e.Row.FindControl("txtVlrUsuNorA"), TextBox).Text = Format(CDec(gdvUsuarioA.DataKeys(e.Row.RowIndex).Item("valValor")), "#,##0.00")
            CType(e.Row.FindControl("txtVlrUsuNorA"), TextBox).Attributes.Add("onkeyup", "javascript:if((window.event.keyCode != 9) && (window.event.keyCode != 16)) {this.value=ColocaVirgula(this)}")
            CType(e.Row.FindControl("lblVlrUsuSuiA"), Label).Text = Format(CDec(gdvUsuarioA.DataKeys(e.Row.RowIndex).Item("valValor") * 1.3), "#,##0.00")
            CType(e.Row.FindControl("txtVlrUsuNorA"), TextBox).Attributes.Add("catId", gdvUsuarioA.DataKeys(e.Row.RowIndex).Item("catId"))
            CType(e.Row.FindControl("txtVlrUsuNorA"), TextBox).Attributes.Add("valQtde", gdvUsuarioA.DataKeys(e.Row.RowIndex).Item("valQtde"))
        End If
    End Sub

    Protected Sub gdvComerciario_RowDataBound(sender As Object, e As GridViewRowEventArgs) Handles gdvComerciario.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            CType(e.Row.FindControl("lblComNor"), Label).Text = Replace(gdvComerciario.DataKeys(e.Row.RowIndex).Item("catDescricao"), "Comerciário", "")
            CType(e.Row.FindControl("txtVlrComNor"), TextBox).Text = Format(CDec(gdvComerciario.DataKeys(e.Row.RowIndex).Item("valValor")), "#,##0.00")
            CType(e.Row.FindControl("txtVlrComNor"), TextBox).Attributes.Add("onkeyup", "javascript:if((window.event.keyCode != 9) && (window.event.keyCode != 16)) {this.value=ColocaVirgula(this)}")

            CType(e.Row.FindControl("lblVlrComSui"), Label).Text = Format(CDec(gdvComerciario.DataKeys(e.Row.RowIndex).Item("valValor") * 1.3), "#,##0.00")
            CType(e.Row.FindControl("txtVlrComNor"), TextBox).Attributes.Add("catId", gdvComerciario.DataKeys(e.Row.RowIndex).Item("catId"))
            CType(e.Row.FindControl("txtVlrComNor"), TextBox).Attributes.Add("valQtde", gdvComerciario.DataKeys(e.Row.RowIndex).Item("valQtde"))
        End If
    End Sub

    Protected Sub gdvConveniado_RowDataBound(sender As Object, e As GridViewRowEventArgs) Handles gdvConveniado.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            CType(e.Row.FindControl("lblConNor"), Label).Text = Replace(gdvConveniado.DataKeys(e.Row.RowIndex).Item("catDescricao"), "Conveniado", "")
            CType(e.Row.FindControl("txtVlrConNor"), TextBox).Text = Format(CDec(gdvConveniado.DataKeys(e.Row.RowIndex).Item("valValor")), "#,##0.00")
            CType(e.Row.FindControl("txtVlrConNor"), TextBox).Attributes.Add("onkeyup", "javascript:if((window.event.keyCode != 9) && (window.event.keyCode != 16)) {this.value=ColocaVirgula(this)}")
            CType(e.Row.FindControl("lblVlrConSui"), Label).Text = Format(CDec(gdvConveniado.DataKeys(e.Row.RowIndex).Item("valValor") * 1.3), "#,##0.00")
            CType(e.Row.FindControl("txtVlrConNor"), TextBox).Attributes.Add("catId", gdvConveniado.DataKeys(e.Row.RowIndex).Item("catId"))
            CType(e.Row.FindControl("txtVlrConNor"), TextBox).Attributes.Add("valQtde", gdvConveniado.DataKeys(e.Row.RowIndex).Item("valQtde"))
        End If
    End Sub

    Protected Sub gdvUsuario_RowDataBound(sender As Object, e As GridViewRowEventArgs) Handles gdvUsuario.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            CType(e.Row.FindControl("lblUsuNor"), Label).Text = Replace(gdvUsuario.DataKeys(e.Row.RowIndex).Item("catDescricao"), "Usuário", "")
            CType(e.Row.FindControl("txtVlrUsuNor"), TextBox).Text = Format(CDec(gdvUsuario.DataKeys(e.Row.RowIndex).Item("valValor")), "#,##0.00")
            CType(e.Row.FindControl("txtVlrUsuNor"), TextBox).Attributes.Add("onkeyup", "javascript:if((window.event.keyCode != 9) && (window.event.keyCode != 16)) {this.value=ColocaVirgula(this)}")
            CType(e.Row.FindControl("lblVlrUsuSui"), Label).Text = Format(CDec(gdvUsuario.DataKeys(e.Row.RowIndex).Item("valValor") * 1.3), "#,##0.00")
            CType(e.Row.FindControl("txtVlrUsuNor"), TextBox).Attributes.Add("catId", gdvUsuario.DataKeys(e.Row.RowIndex).Item("catId"))
            CType(e.Row.FindControl("txtVlrUsuNor"), TextBox).Attributes.Add("valQtde", gdvUsuario.DataKeys(e.Row.RowIndex).Item("valQtde"))
        End If
    End Sub

    Protected Sub gdvComerciarioA_RowDataBound(sender As Object, e As GridViewRowEventArgs) Handles gdvComerciarioA.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            CType(e.Row.FindControl("lblComNorA"), Label).Text = Replace(gdvComerciarioA.DataKeys(e.Row.RowIndex).Item("catDescricao"), "Comerciário", "")
            CType(e.Row.FindControl("txtVlrComNorA"), TextBox).Text = Format(CDec(gdvComerciarioA.DataKeys(e.Row.RowIndex).Item("valValor")), "#,##0.00")
            CType(e.Row.FindControl("txtVlrComNorA"), TextBox).Attributes.Add("onkeyup", "javascript:if((window.event.keyCode != 9) && (window.event.keyCode != 16)) {this.value=ColocaVirgula(this)}")
            CType(e.Row.FindControl("lblVlrComSuiA"), Label).Text = Format(CDec(gdvComerciarioA.DataKeys(e.Row.RowIndex).Item("valValor") * 1.3), "#,##0.00")
            CType(e.Row.FindControl("txtVlrComNorA"), TextBox).Attributes.Add("catId", gdvComerciarioA.DataKeys(e.Row.RowIndex).Item("catId"))
            CType(e.Row.FindControl("txtVlrComNorA"), TextBox).Attributes.Add("valQtde", gdvComerciarioA.DataKeys(e.Row.RowIndex).Item("valQtde"))
        End If
    End Sub

    Protected Sub btnTransportar_Click(sender As Object, e As EventArgs) Handles btnTransportar.Click
        For Each linha As GridViewRow In gdvComerciario.Rows
            CType(gdvComerciarioA.Rows(linha.RowIndex).FindControl("txtVlrComNorA"), TextBox).Text = CType(linha.FindControl("txtVlrComNor"), TextBox).Text
            CType(gdvComerciarioA.Rows(linha.RowIndex).FindControl("lblVlrComSuiA"), Label).Text = CType(linha.FindControl("lblVlrComSui"), Label).Text

            CType(gdvConveniadoA.Rows(linha.RowIndex).FindControl("txtVlrConNorA"), TextBox).Text = CType(gdvConveniado.Rows(linha.RowIndex).FindControl("txtVlrConNor"), TextBox).Text
            CType(gdvConveniadoA.Rows(linha.RowIndex).FindControl("lblVlrConSuiA"), Label).Text = CType(gdvConveniado.Rows(linha.RowIndex).FindControl("lblVlrConSui"), Label).Text

            CType(gdvUsuarioA.Rows(linha.RowIndex).FindControl("txtVlrUsuNorA"), TextBox).Text = CType(gdvUsuario.Rows(linha.RowIndex).FindControl("txtVlrUsuNor"), TextBox).Text
            CType(gdvUsuarioA.Rows(linha.RowIndex).FindControl("lblVlrUsuSuiA"), Label).Text = CType(gdvUsuario.Rows(linha.RowIndex).FindControl("lblVlrUsuSui"), Label).Text
        Next
        'gdvComerciario.Enabled = True
        'gdvConveniado.Enabled = True
        'gdvUsuario.Enabled = True
        'btnTransportar.Visible = False
        'btnReservaGravar.Visible = True
        ''Deixando os grid antigos desabilitado
        'gdvComerciarioA.Enabled = False
        'gdvConveniadoA.Enabled = False
        'gdvUsuarioA.Enabled = False

    End Sub

    Protected Sub btnAltaTemporada_Click(sender As Object, e As EventArgs) Handles btnAltaTemporada.Click
        pnlValor.Visible = False
        pnlAltaTemporada.Visible = True
        pnlDesconto.Visible = True
    End Sub

    Protected Sub btnConsumoVoltar_Click(sender As Object, e As EventArgs) Handles btnVoltar.Click
        pnlValor.Visible = True
        pnlAltaTemporada.Visible = False
        pnlDesconto.Visible = False
    End Sub

    Protected Sub btnConsulta_Click(sender As Object, e As EventArgs) Handles btnConsulta.Click
        If Session("MasterPage").ToString = "~/TurismoSocial.Master" Then
            objPacoteDAO = New Turismo.PacoteDAO("TurismoSocialCaldas")
        Else
            objPacoteDAO = New Turismo.PacoteDAO("TurismoSocialPiri")
        End If
        gdvAltaTemporada.DataSource = objPacoteDAO.consultar(CDate(txtDataInicial.Text), CDate(txtDataFinal.Text))
        gdvAltaTemporada.DataBind()
    End Sub

    Protected Sub gdvAltaTemporada_RowDataBound(sender As Object, e As GridViewRowEventArgs) Handles gdvAltaTemporada.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            CType(e.Row.FindControl("lnkDataInicial"), LinkButton).Text = Mid(sender.DataKeys(e.Row.RowIndex).Item("pacDataIni"), 1, 10)
            CType(e.Row.FindControl("lnkDataFinal"), LinkButton).Text = Mid(sender.DataKeys(e.Row.RowIndex).Item("pacDataFim"), 1, 10)
            CType(e.Row.FindControl("lnkPercentual"), LinkButton).Text = sender.DataKeys(e.Row.RowIndex).Item("pacPercentual")
            CType(e.Row.FindControl("lnkDataInicialVigencia"), LinkButton).Text = Mid(sender.DataKeys(e.Row.RowIndex).Item("pacDataIniSol"), 1, 10)
            CType(e.Row.FindControl("lnkDataFinalVigencia"), LinkButton).Text = Mid(sender.DataKeys(e.Row.RowIndex).Item("pacDataFimSol"), 1, 10)
            CType(e.Row.FindControl("lnkDataInicial"), LinkButton).CommandArgument = sender.DataKeys(e.Row.RowIndex).Item("pacId")
            CType(e.Row.FindControl("lnkDataFinal"), LinkButton).CommandArgument = sender.DataKeys(e.Row.RowIndex).Item("pacId")
            CType(e.Row.FindControl("lnkPercentual"), LinkButton).CommandArgument = sender.DataKeys(e.Row.RowIndex).Item("pacId")
            CType(e.Row.FindControl("lnkDataInicialVigencia"), LinkButton).CommandArgument = sender.DataKeys(e.Row.RowIndex).Item("pacId")
            CType(e.Row.FindControl("lnkDataFinalVigencia"), LinkButton).CommandArgument = sender.DataKeys(e.Row.RowIndex).Item("pacId")

            If sender.DataKeys(e.Row.RowIndex).Item("pacFlutuanteFederacao") = "S" Then
                e.Row.Cells(5).Text = "Sim"
            Else
                e.Row.Cells(5).Text = "Não"
            End If

            If sender.DataKeys(e.Row.RowIndex).Item("pacFechado") = "S" Then
                e.Row.Cells(6).Text = "Sim"
            Else
                e.Row.Cells(6).Text = "Não"
            End If


        End If
    End Sub

    Protected Sub lnkDataInicial_Click(sender As Object, e As EventArgs)
        If Session("MasterPage").ToString = "~/TurismoSocial.Master" Then
            objPacoteDAO = New Turismo.PacoteDAO("TurismoSocialCaldas")
        Else
            objPacoteDAO = New Turismo.PacoteDAO("TurismoSocialPiri")
        End If
        objPacoteVO = objPacoteDAO.consultar(sender.CommandArgument)
        hddPacId.Value = objPacoteVO.pacId
        txtDataInicialAcao.Text = objPacoteVO.pacDataIni
        txtDataFinalAcao.Text = objPacoteVO.pacDataFim
        txtPercentualAcao.Text = objPacoteVO.pacPercentual
        txtDataInicialVigenciaAcao.Text = objPacoteVO.pacDataIniSol
        txtDataFinalVigenciaAcao.Text = objPacoteVO.pacDataFimSol
        txtDataInicialAcao_CalendarExtender.SelectedDate = txtDataInicialAcao.Text
        txtDataFinalAcao_CalendarExtender.SelectedDate = txtDataFinalAcao.Text
        txtDataInicialVigenciaAcao_CalendarExtender.SelectedDate = txtDataInicialVigenciaAcao.Text
        txtDataFinalVigenciaAcao_CalendarExtender.SelectedDate = txtDataFinalVigenciaAcao.Text
        If objPacoteVO.pacFlutuanteFederacao = "S" Then
            chkFlutuanteFederacao.Checked = True
        Else
            chkFlutuanteFederacao.Checked = False
        End If

        If objPacoteVO.pacFechado = "S" Then
            chkPacoteFechado.Checked = True
        Else
            chkPacoteFechado.Checked = False
        End If

        pnlAltaTemporada.Visible = False
        pnlAltaTemporadaAcao.Visible = True
        btnExcluirAcao.Visible = True
        btnNovoAcao.Visible = True
        txtDataInicialAcao.Focus()
    End Sub

    Protected Sub btnVoltarAcao_Click(sender As Object, e As EventArgs) Handles btnVoltarAcao.Click
        pnlAltaTemporada.Visible = True
        pnlAltaTemporadaAcao.Visible = False
        btnConsulta_Click(sender, e)
    End Sub

    Protected Sub btnNovo_Click(sender As Object, e As EventArgs) Handles btnNovo.Click, btnNovoAcao.Click
        hddPacId.Value = "0"
        txtDataInicialAcao.Text = Mid(Date.Now.ToString, 1, 10)
        txtDataFinalAcao.Text = Mid(Date.Now.ToString, 1, 10)
        txtPercentualAcao.Text = "1,00"
        txtDataInicialVigenciaAcao.Text = "01/01/" & Year(CDate(txtDataInicialAcao.Text)) - 1 'Mid(Date.Now.ToString, 1, 10)
        txtDataFinalVigenciaAcao.Text = "01/01/" & Year(CDate(txtDataFinalAcao.Text)) + 2 'Mid(Date.Now.ToString, 1, 10)
        txtDataInicialAcao_CalendarExtender.SelectedDate = txtDataInicialAcao.Text
        txtDataFinalAcao_CalendarExtender.SelectedDate = txtDataFinalAcao.Text
        txtDataInicialVigenciaAcao_CalendarExtender.SelectedDate = txtDataInicialVigenciaAcao.Text
        txtDataFinalVigenciaAcao_CalendarExtender.SelectedDate = txtDataFinalVigenciaAcao.Text
        chkFlutuanteFederacao.Checked = False
        chkPacoteFechado.Checked = False
        pnlAltaTemporada.Visible = False
        pnlAltaTemporadaAcao.Visible = True
        btnExcluirAcao.Visible = False
        btnNovoAcao.Visible = False
        txtDataInicialAcao.Focus()
    End Sub

    Protected Sub btnSalvarAcao_Click(sender As Object, e As EventArgs) Handles btnSalvarAcao.Click
        'If IsNumeric(txtPercentualAcao.Text) Then
        '    If CDec(txtPercentualAcao.Text) < 1 Then
        '        Mensagem("O valor do percentual não pode ser menor que um.")
        '        txtPercentualAcao.Focus()
        '        txtPercentualAcao.Text = "0,00 "
        '        Return
        '    End If
        'End If

        If Not IsDate(txtDataInicialAcao.Text) Then
            ScriptManager.RegisterStartupScript(Page, Page.[GetType](), Guid.NewGuid().ToString(), "alert('Preencha a data.');", True)
            txtDataInicialAcao.Focus()
            Exit Sub
        ElseIf Not IsDate(txtDataFinalAcao.Text) Then
            ScriptManager.RegisterStartupScript(Page, Page.[GetType](), Guid.NewGuid().ToString(), "alert('Preencha a data.');", True)
            txtDataFinalAcao.Focus()
            Exit Sub
        ElseIf CDate(txtDataInicialAcao.Text) > CDate(txtDataFinalAcao.Text) Then
            ScriptManager.RegisterStartupScript(Page, Page.[GetType](), Guid.NewGuid().ToString(), "alert('A data inicial não pode exceder a data final.');", True)
            txtDataInicialAcao.Focus()
            Exit Sub
        ElseIf Not IsNumeric(txtPercentualAcao.Text) Then
            ScriptManager.RegisterStartupScript(Page, Page.[GetType](), Guid.NewGuid().ToString(), "alert('Preencha o percentual.');", True)
            txtPercentualAcao.Focus()
            Exit Sub
        ElseIf Not IsDate(txtDataInicialVigenciaAcao.Text) Then
            ScriptManager.RegisterStartupScript(Page, Page.[GetType](), Guid.NewGuid().ToString(), "alert('Preencha a data.');", True)
            txtDataInicialVigenciaAcao.Focus()
            Exit Sub
        ElseIf Not IsDate(txtDataFinalVigenciaAcao.Text) Then
            ScriptManager.RegisterStartupScript(Page, Page.[GetType](), Guid.NewGuid().ToString(), "alert('Preencha a data.');", True)
            txtDataFinalVigenciaAcao.Focus()
            Exit Sub
        ElseIf CDate(txtDataInicialVigenciaAcao.Text) > CDate(txtDataFinalVigenciaAcao.Text) Then
            ScriptManager.RegisterStartupScript(Page, Page.[GetType](), Guid.NewGuid().ToString(), "alert('A data inicial não pode exceder a data final.');", True)
            txtDataInicialVigenciaAcao.Focus()
            Exit Sub
        End If

        If Session("MasterPage").ToString = "~/TurismoSocial.Master" Then
            objPacoteDAO = New Turismo.PacoteDAO("TurismoSocialCaldas")
        Else
            objPacoteDAO = New Turismo.PacoteDAO("TurismoSocialPiri")
        End If

        objPacoteVO.pacId = hddPacId.Value
        objPacoteVO.pacDataIni = Mid(txtDataInicialAcao.Text, 1, 10)
        objPacoteVO.pacDataFim = Mid(txtDataFinalAcao.Text, 1, 10)
        objPacoteVO.pacPercentual = txtPercentualAcao.Text
        objPacoteVO.pacDataIniSol = Mid(txtDataInicialVigenciaAcao.Text, 1, 10)
        objPacoteVO.pacDataFimSol = Mid(txtDataFinalVigenciaAcao.Text, 1, 10)
        If chkFlutuanteFederacao.Checked Then
            objPacoteVO.pacFlutuanteFederacao = "S"
        Else
            objPacoteVO.pacFlutuanteFederacao = "N"
        End If

        If chkPacoteFechado.Checked Then
            objPacoteVO.pacFechado = "S"
        Else
            objPacoteVO.pacFechado = "N"
        End If

        Try
            Select Case objPacoteDAO.Acao(objPacoteVO)
                Case Is > 0
                    btnConsulta_Click(sender, e)
                    btnVoltarAcao_Click(sender, e)
                    ScriptManager.RegisterStartupScript(Page, Page.[GetType](), Guid.NewGuid().ToString(),
                      "alert('Operação realizada com sucesso.');", True)
                Case -1
                    ScriptManager.RegisterStartupScript(Page, Page.[GetType](), Guid.NewGuid().ToString(),
                             "alert('Período de pacote não permitido, pois há intersecção com outros pacotes.');", True)
                Case 0
                    ScriptManager.RegisterStartupScript(Page, Page.[GetType](), Guid.NewGuid().ToString(),
                       "alert('Não foi possível efetuar esta operação. Tente novamente.');", True)
            End Select

        Catch ex As Exception
            ScriptManager.RegisterStartupScript(Page, Page.[GetType](), Guid.NewGuid().ToString(),
                             "alert('Não foi possível efetuar esta operação. Tente novamente.');", True)
        End Try

    End Sub

    Protected Sub btnExcluirAcao_Click(sender As Object, e As EventArgs) Handles btnExcluirAcao.Click
        If Session("MasterPage").ToString = "~/TurismoSocial.Master" Then
            objPacoteDAO = New Turismo.PacoteDAO("TurismoSocialCaldas")
        Else
            objPacoteDAO = New Turismo.PacoteDAO("TurismoSocialPiri")
        End If
        objPacoteVO.pacId = -hddPacId.Value
        objPacoteVO.pacDataIni = Mid(txtDataInicialAcao.Text, 1, 10)
        objPacoteVO.pacDataFim = Mid(txtDataFinalAcao.Text, 1, 10)
        objPacoteVO.pacPercentual = txtPercentualAcao.Text
        objPacoteVO.pacDataIniSol = Mid(txtDataInicialVigenciaAcao.Text, 1, 10)
        objPacoteVO.pacDataFimSol = Mid(txtDataFinalVigenciaAcao.Text, 1, 10)

        Try
            Select Case objPacoteDAO.Acao(objPacoteVO)
                Case Is > 0
                    btnVoltarAcao_Click(sender, e)
                    ScriptManager.RegisterStartupScript(Page, Page.[GetType](), Guid.NewGuid().ToString(),
                      "alert('Operação realizada com sucesso.');", True)
                Case Else
                    ScriptManager.RegisterStartupScript(Page, Page.[GetType](), Guid.NewGuid().ToString(),
                 "alert('Não foi possível efetuar esta operação. Tente novamente.');", True)
            End Select
        Catch ex As Exception
            ScriptManager.RegisterStartupScript(Page, Page.[GetType](), Guid.NewGuid().ToString(),
              "alert('Não foi possível efetuar esta operação. Tente novamente.');", True)
        End Try

    End Sub

    Protected Sub btnReservaGravar_Click(sender As Object, e As EventArgs) Handles btnReservaGravar.Click
        Dim comando As String = "begin try "
        comando += "begin tran "
        Try
            comando += "insert TbValor (ValDtInicial, ValDtFinal, AcmId, CatId, ValQtde, ValTipo, ValValor, ValDtSolIni, ValDtSolFim) "
            comando += "select ValDtInicial, ValDtFinal, AcmId, CatId, ValQtde, 'R4' as ValTipo, ValValor, ValDtSolIni, ValDtSolFim"
            comando += "  from TbValor where AcmId = " & cmbAcmId.SelectedValue & " and ValTipo = 'R3' "
            '------------------------------------------------------------------------------------------------------------------------------------------------------

            ' COMERCIÁRIO - valores antigos - solicitações já feitas

            For Each linha As GridViewRow In gdvComerciarioA.Rows
                comando += " update TbValor set ValValor = " & CDec(CType(linha.FindControl("txtVlrComNorA"), TextBox).Text).ToString.Replace(".", "").Replace(",", ".") & _
                    ", ValDtSolFim = dateadd(hour, " & cmbHora.SelectedValue & ",'" & Format(CDate(txtDataAntiga.Text), "yyyy-MM-dd") & _
                    "') where CatId = " & CType(linha.FindControl("txtVlrComNorA"), TextBox).Attributes("catId") & _
                    " and ValQtde = " & CType(linha.FindControl("txtVlrComNorA"), TextBox).Attributes("valQtde") & _
                    " and ValTipo = 'R3'" & _
                    " and ValDtSolFim between '" & Format(CDate(hddDataAntiga.Value), "yyyy-MM-dd") & "' and '" & Format(CDate(hddDataAntiga.Value).AddDays(1), "yyyy-MM-dd") & "'" & _
                    " and TbValor.AcmId in (SELECT a.AcmId FROM TbAcomodacao a inner join TbGrupo g on a.GruId = g.GruId where g.GruId = " & cmbTipo.SelectedValue & " and AcmSuite = 'N')"
            Next


            ' COMERCIÁRIO SUÍTE - valores antigos - solicitações já feitas

            For Each linha As GridViewRow In gdvComerciarioA.Rows
                comando += " update TbValor set ValValor = " & (CDec(CType(linha.FindControl("txtVlrComNorA"), TextBox).Text) * 1.3).ToString.Replace(".", "").Replace(",", ".") & _
                    ", ValDtSolFim = dateadd(hour, " & cmbHora.SelectedValue & ",'" & Format(CDate(txtDataAntiga.Text), "yyyy-MM-dd") & _
                    "') where CatId = " & CType(linha.FindControl("txtVlrComNorA"), TextBox).Attributes("catId") & _
                    " and ValQtde = " & CType(linha.FindControl("txtVlrComNorA"), TextBox).Attributes("valQtde") & _
                    " and ValTipo = 'R3'" & _
                    " and ValDtSolFim between '" & Format(CDate(hddDataAntiga.Value), "yyyy-MM-dd") & "' and '" & Format(CDate(hddDataAntiga.Value).AddDays(1), "yyyy-MM-dd") & "'" & _
                    " and TbValor.AcmId in (SELECT a.AcmId FROM TbAcomodacao a inner join TbGrupo g on a.GruId = g.GruId where g.GruId = " & cmbTipo.SelectedValue & " and AcmSuite = 'S')"
            Next


            ' CONVENIADO - valores antigos - solicitações já feitas

            For Each linha As GridViewRow In gdvConveniadoA.Rows
                comando += " update TbValor set ValValor = " & CDec(CType(linha.FindControl("txtVlrConNorA"), TextBox).Text).ToString.Replace(".", "").Replace(",", ".") & _
                    ", ValDtSolFim = dateadd(hour, " & cmbHora.SelectedValue & ",'" & Format(CDate(txtDataAntiga.Text), "yyyy-MM-dd") & _
                    "') where CatId = " & CType(linha.FindControl("txtVlrConNorA"), TextBox).Attributes("catId") & _
                    " and ValQtde = " & CType(linha.FindControl("txtVlrConNorA"), TextBox).Attributes("valQtde") & _
                    " and ValTipo = 'R3'" & _
                    " and ValDtSolFim between '" & Format(CDate(hddDataAntiga.Value), "yyyy-MM-dd") & "' and '" & Format(CDate(hddDataAntiga.Value).AddDays(1), "yyyy-MM-dd") & "'" & _
                    " and TbValor.AcmId in (SELECT a.AcmId FROM TbAcomodacao a inner join TbGrupo g on a.GruId = g.GruId where g.GruId = " & cmbTipo.SelectedValue & " and AcmSuite = 'N')"
            Next


            ' CONVENIADO SUÍTE - valores antigos - solicitações já feitas

            For Each linha As GridViewRow In gdvConveniadoA.Rows
                comando += " update TbValor set ValValor = " & (CDec(CType(linha.FindControl("txtVlrConNorA"), TextBox).Text) * 1.3).ToString.Replace(".", "").Replace(",", ".") & _
                    ", ValDtSolFim = dateadd(hour, " & cmbHora.SelectedValue & ",'" & Format(CDate(txtDataAntiga.Text), "yyyy-MM-dd") & _
                    "') where CatId = " & CType(linha.FindControl("txtVlrConNorA"), TextBox).Attributes("catId") & _
                    " and ValQtde = " & CType(linha.FindControl("txtVlrConNorA"), TextBox).Attributes("valQtde") & _
                    " and ValTipo = 'R3'" & _
                    " and ValDtSolFim between '" & Format(CDate(hddDataAntiga.Value), "yyyy-MM-dd") & "' and '" & Format(CDate(hddDataAntiga.Value).AddDays(1), "yyyy-MM-dd") & "'" & _
                    " and TbValor.AcmId in (SELECT a.AcmId FROM TbAcomodacao a inner join TbGrupo g on a.GruId = g.GruId where g.GruId = " & cmbTipo.SelectedValue & " and AcmSuite = 'S')"
            Next


            ' USUÁRIO - valores antigos - solicitações já feitas

            For Each linha As GridViewRow In gdvUsuarioA.Rows
                comando += " update TbValor set ValValor = " & CDec(CType(linha.FindControl("txtVlrUsuNorA"), TextBox).Text).ToString.Replace(".", "").Replace(",", ".") & _
                    ", ValDtSolFim = dateadd(hour, " & cmbHora.SelectedValue & ",'" & Format(CDate(txtDataAntiga.Text), "yyyy-MM-dd") & _
                    "') where CatId = " & CType(linha.FindControl("txtVlrUsuNorA"), TextBox).Attributes("catId") & _
                    " and ValQtde = " & CType(linha.FindControl("txtVlrUsuNorA"), TextBox).Attributes("valQtde") & _
                    " and ValTipo = 'R3'" & _
                    " and ValDtSolFim between '" & Format(CDate(hddDataAntiga.Value), "yyyy-MM-dd") & "' and '" & Format(CDate(hddDataAntiga.Value).AddDays(1), "yyyy-MM-dd") & "'" & _
                    " and TbValor.AcmId in (SELECT a.AcmId FROM TbAcomodacao a inner join TbGrupo g on a.GruId = g.GruId where g.GruId = " & cmbTipo.SelectedValue & " and AcmSuite = 'N')"
            Next


            ' USUÁRIO SUÍTE - valores antigos - solicitações já feitas

            For Each linha As GridViewRow In gdvUsuarioA.Rows
                comando += " update TbValor set ValValor = " & (CDec(CType(linha.FindControl("txtVlrUsuNorA"), TextBox).Text) * 1.3).ToString.Replace(".", "").Replace(",", ".") & _
                    ", ValDtSolFim = dateadd(hour, " & cmbHora.SelectedValue & ",'" & Format(CDate(txtDataAntiga.Text), "yyyy-MM-dd") & _
                    "') where CatId = " & CType(linha.FindControl("txtVlrUsuNorA"), TextBox).Attributes("catId") & _
                    " and ValQtde = " & CType(linha.FindControl("txtVlrUsuNorA"), TextBox).Attributes("valQtde") & _
                    " and ValTipo = 'R3'" & _
                    " and ValDtSolFim between '" & Format(CDate(hddDataAntiga.Value), "yyyy-MM-dd") & "' and '" & Format(CDate(hddDataAntiga.Value).AddDays(1), "yyyy-MM-dd") & "'" & _
                    " and TbValor.AcmId in (SELECT a.AcmId FROM TbAcomodacao a inner join TbGrupo g on a.GruId = g.GruId where g.GruId = " & cmbTipo.SelectedValue & " and AcmSuite = 'S')"
            Next

            '------------------------------------------------------------------------------------------------------------------------------------------------------

            ' COMERCIÁRIO - valores antigos - solicitações novas

            For Each linha As GridViewRow In gdvComerciarioA.Rows
                comando += " update TbValor set ValValor = " & CDec(CType(linha.FindControl("txtVlrComNorA"), TextBox).Text).ToString.Replace(".", "").Replace(",", ".") & _
                    ", ValDtInicial = '" & Format(CDate(txtDataAntiga.Text), "yyyy-MM-dd") & _
                    "', ValDtFinal = '" & Format(CDate(txtData.Text), "yyyy-MM-dd") & _
                    "', ValDtSolFim = '" & Format(CDate(txtData.Text), "yyyy-MM-dd") & _
                    "', ValDtSolIni = dateadd(hour, " & cmbHora.SelectedValue & ",'" & Format(CDate(txtDataAntiga.Text), "yyyy-MM-dd") & _
                    "') where CatId = " & CType(linha.FindControl("txtVlrComNorA"), TextBox).Attributes("catId") & _
                    " and ValQtde = " & CType(linha.FindControl("txtVlrComNorA"), TextBox).Attributes("valQtde") & _
                    " and ValTipo = 'R3'" & _
                    " and ValDtSolFim = '" & Format(CDate(hddData.Value), "yyyy-MM-dd") & _
                    "' and TbValor.AcmId in (SELECT a.AcmId FROM TbAcomodacao a inner join TbGrupo g on a.GruId = g.GruId where g.GruId = " & cmbTipo.SelectedValue & " and AcmSuite = 'N')"
            Next

            ' COMERCIÁRIO SUÍTE - valores antigos - solicitações novas

            For Each linha As GridViewRow In gdvComerciarioA.Rows
                comando += " update TbValor set ValValor = " & (CDec(CType(linha.FindControl("txtVlrComNorA"), TextBox).Text) * 1.3).ToString.Replace(".", "").Replace(",", ".") & _
                    ", ValDtInicial = '" & Format(CDate(txtDataAntiga.Text), "yyyy-MM-dd") & _
                    "', ValDtFinal = '" & Format(CDate(txtData.Text), "yyyy-MM-dd") & _
                    "', ValDtSolFim = '" & Format(CDate(txtData.Text), "yyyy-MM-dd") & _
                    "', ValDtSolIni = dateadd(hour, " & cmbHora.SelectedValue & ",'" & Format(CDate(txtDataAntiga.Text), "yyyy-MM-dd") & _
                    "') where CatId = " & CType(linha.FindControl("txtVlrComNorA"), TextBox).Attributes("catId") & _
                    " and ValQtde = " & CType(linha.FindControl("txtVlrComNorA"), TextBox).Attributes("valQtde") & _
                    " and ValTipo = 'R3'" & _
                    " and ValDtSolFim = '" & Format(CDate(hddData.Value), "yyyy-MM-dd") & _
                    "' and TbValor.AcmId in (SELECT a.AcmId FROM TbAcomodacao a inner join TbGrupo g on a.GruId = g.GruId where g.GruId = " & cmbTipo.SelectedValue & " and AcmSuite = 'S')"
            Next


            ' CONVENIADO - valores antigos - solicitações novas

            For Each linha As GridViewRow In gdvConveniadoA.Rows
                comando += " update TbValor set ValValor = " & CDec(CType(linha.FindControl("txtVlrConNorA"), TextBox).Text).ToString.Replace(".", "").Replace(",", ".") & _
                    ", ValDtInicial = '" & Format(CDate(txtDataAntiga.Text), "yyyy-MM-dd") & _
                    "', ValDtFinal = '" & Format(CDate(txtData.Text), "yyyy-MM-dd") & _
                    "', ValDtSolFim = '" & Format(CDate(txtData.Text), "yyyy-MM-dd") & _
                    "', ValDtSolIni = dateadd(hour, " & cmbHora.SelectedValue & ",'" & Format(CDate(txtDataAntiga.Text), "yyyy-MM-dd") & _
                    "') where CatId = " & CType(linha.FindControl("txtVlrConNorA"), TextBox).Attributes("catId") & _
                    " and ValQtde = " & CType(linha.FindControl("txtVlrConNorA"), TextBox).Attributes("valQtde") & _
                    " and ValTipo = 'R3'" & _
                    " and ValDtSolFim = '" & Format(CDate(hddData.Value), "yyyy-MM-dd") & _
                    "' and TbValor.AcmId in (SELECT a.AcmId FROM TbAcomodacao a inner join TbGrupo g on a.GruId = g.GruId where g.GruId = " & cmbTipo.SelectedValue & " and AcmSuite = 'N')"
            Next

            ' CONVENIADO SUÍTE - valores antigos - solicitações novas

            For Each linha As GridViewRow In gdvConveniadoA.Rows
                comando += " update TbValor set ValValor = " & (CDec(CType(linha.FindControl("txtVlrConNorA"), TextBox).Text) * 1.3).ToString.Replace(".", "").Replace(",", ".") & _
                    ", ValDtInicial = '" & Format(CDate(txtDataAntiga.Text), "yyyy-MM-dd") & _
                    "', ValDtFinal = '" & Format(CDate(txtData.Text), "yyyy-MM-dd") & _
                    "', ValDtSolFim = '" & Format(CDate(txtData.Text), "yyyy-MM-dd") & _
                    "', ValDtSolIni = dateadd(hour, " & cmbHora.SelectedValue & ",'" & Format(CDate(txtDataAntiga.Text), "yyyy-MM-dd") & _
                    "') where CatId = " & CType(linha.FindControl("txtVlrConNorA"), TextBox).Attributes("catId") & _
                    " and ValQtde = " & CType(linha.FindControl("txtVlrConNorA"), TextBox).Attributes("valQtde") & _
                    " and ValTipo = 'R3'" & _
                    " and ValDtSolFim = '" & Format(CDate(hddData.Value), "yyyy-MM-dd") & _
                    "' and TbValor.AcmId in (SELECT a.AcmId FROM TbAcomodacao a inner join TbGrupo g on a.GruId = g.GruId where g.GruId = " & cmbTipo.SelectedValue & " and AcmSuite = 'S')"
            Next

            ' USUÁRIO - valores antigos - solicitações novas

            For Each linha As GridViewRow In gdvUsuarioA.Rows
                comando += " update TbValor set ValValor = " & CDec(CType(linha.FindControl("txtVlrUsuNorA"), TextBox).Text).ToString.Replace(".", "").Replace(",", ".") & _
                    ", ValDtInicial = '" & Format(CDate(txtDataAntiga.Text), "yyyy-MM-dd") & _
                    "', ValDtFinal = '" & Format(CDate(txtData.Text), "yyyy-MM-dd") & _
                    "', ValDtSolFim = '" & Format(CDate(txtData.Text), "yyyy-MM-dd") & _
                    "', ValDtSolIni = dateadd(hour, " & cmbHora.SelectedValue & ",'" & Format(CDate(txtDataAntiga.Text), "yyyy-MM-dd") & _
                    "') where CatId = " & CType(linha.FindControl("txtVlrUsuNorA"), TextBox).Attributes("catId") & _
                    " and ValQtde = " & CType(linha.FindControl("txtVlrUsuNorA"), TextBox).Attributes("valQtde") & _
                    " and ValTipo = 'R3'" & _
                    " and ValDtSolFim = '" & Format(CDate(hddData.Value), "yyyy-MM-dd") & _
                    "' and TbValor.AcmId in (SELECT a.AcmId FROM TbAcomodacao a inner join TbGrupo g on a.GruId = g.GruId where g.GruId = " & cmbTipo.SelectedValue & " and AcmSuite = 'N')"
            Next

            ' USUÁRIO SUÍTE - valores antigos - solicitações novas

            For Each linha As GridViewRow In gdvUsuarioA.Rows
                comando += " update TbValor set ValValor = " & (CDec(CType(linha.FindControl("txtVlrUsuNorA"), TextBox).Text) * 1.3).ToString.Replace(".", "").Replace(",", ".") & _
                    ", ValDtInicial = '" & Format(CDate(txtDataAntiga.Text), "yyyy-MM-dd") & _
                    "', ValDtFinal = '" & Format(CDate(txtData.Text), "yyyy-MM-dd") & _
                    "', ValDtSolFim = '" & Format(CDate(txtData.Text), "yyyy-MM-dd") & _
                    "', ValDtSolIni = dateadd(hour, " & cmbHora.SelectedValue & ",'" & Format(CDate(txtDataAntiga.Text), "yyyy-MM-dd") & _
                    "') where CatId = " & CType(linha.FindControl("txtVlrUsuNorA"), TextBox).Attributes("catId") & _
                    " and ValQtde = " & CType(linha.FindControl("txtVlrUsuNorA"), TextBox).Attributes("valQtde") & _
                    " and ValTipo = 'R3'" & _
                    " and ValDtSolFim = '" & Format(CDate(hddData.Value), "yyyy-MM-dd") & _
                    "' and TbValor.AcmId in (SELECT a.AcmId FROM TbAcomodacao a inner join TbGrupo g on a.GruId = g.GruId where g.GruId = " & cmbTipo.SelectedValue & " and AcmSuite = 'S')"
            Next

            '------------------------------------------------------------------------------------------------------------------------------------------------------

            ' COMERCIÁRIO - novos valores

            For Each linha As GridViewRow In gdvComerciario.Rows
                comando += " update TbValor set ValValor = " & CDec(CType(linha.FindControl("txtVlrComNor"), TextBox).Text).ToString.Replace(".", "").Replace(",", ".") & ", " & _
                    "ValDtInicial = '" & Format(CDate(txtData.Text), "yyyy-MM-dd") & _
                    "', ValDtSolIni = dateadd(hour, " & cmbHora.SelectedValue & ",'" & Format(CDate(txtDataAntiga.Text), "yyyy-MM-dd") & _
                    "') where CatId = " & CType(linha.FindControl("txtVlrComNor"), TextBox).Attributes("catId") & _
                    " and ValQtde = " & CType(linha.FindControl("txtVlrComNor"), TextBox).Attributes("valQtde") & _
                    " and ValTipo = 'R3' and ValDtInicial = '" & Format(CDate(hddData.Value), "yyyy-MM-dd") & _
                    "' and ValDtSolIni between '" & Format(CDate(hddDataAntiga.Value), "yyyy-MM-dd") & "' and '" & Format(CDate(hddDataAntiga.Value).AddDays(1), "yyyy-MM-dd") & "'" & _
                    " and TbValor.AcmId in (SELECT a.AcmId FROM TbAcomodacao a inner join TbGrupo g on a.GruId = g.GruId where g.GruId = " & cmbTipo.SelectedValue & " and AcmSuite = 'N')"
            Next

            ' COMERCIÁRIO SUÍTE - novos valores

            For Each linha As GridViewRow In gdvComerciario.Rows
                comando += " update TbValor set ValValor = " & (CDec(CType(linha.FindControl("txtVlrComNor"), TextBox).Text) * 1.3).ToString.Replace(".", "").Replace(",", ".") & ", " & _
                    "ValDtInicial = '" & Format(CDate(txtData.Text), "yyyy-MM-dd") & _
                    "', ValDtSolIni = dateadd(hour, " & cmbHora.SelectedValue & ",'" & Format(CDate(txtDataAntiga.Text), "yyyy-MM-dd") & _
                    "') where CatId = " & CType(linha.FindControl("txtVlrComNor"), TextBox).Attributes("catId") & _
                    " and ValQtde = " & CType(linha.FindControl("txtVlrComNor"), TextBox).Attributes("valQtde") & _
                    " and ValTipo = 'R3' and ValDtInicial = '" & Format(CDate(hddData.Value), "yyyy-MM-dd") & _
                    "' and ValDtSolIni between '" & Format(CDate(hddDataAntiga.Value), "yyyy-MM-dd") & "' and '" & Format(CDate(hddDataAntiga.Value).AddDays(1), "yyyy-MM-dd") & "'" & _
                    " and TbValor.AcmId in (SELECT a.AcmId FROM TbAcomodacao a inner join TbGrupo g on a.GruId = g.GruId where g.GruId = " & cmbTipo.SelectedValue & " and AcmSuite = 'S')"
            Next


            ' CONVENIADO - novos valores

            For Each linha As GridViewRow In gdvConveniado.Rows
                comando += " update TbValor set ValValor = " & CDec(CType(linha.FindControl("txtVlrConNor"), TextBox).Text).ToString.Replace(".", "").Replace(",", ".") & ", " & _
                    "ValDtInicial = '" & Format(CDate(txtData.Text), "yyyy-MM-dd") & _
                    "', ValDtSolIni = dateadd(hour, " & cmbHora.SelectedValue & ",'" & Format(CDate(txtDataAntiga.Text), "yyyy-MM-dd") & _
                    "') where CatId = " & CType(linha.FindControl("txtVlrConNor"), TextBox).Attributes("catId") & _
                    " and ValQtde = " & CType(linha.FindControl("txtVlrConNor"), TextBox).Attributes("valQtde") & _
                    " and ValTipo = 'R3' and ValDtInicial = '" & Format(CDate(hddData.Value), "yyyy-MM-dd") & _
                    "' and ValDtSolIni between '" & Format(CDate(hddDataAntiga.Value), "yyyy-MM-dd") & "' and '" & Format(CDate(hddDataAntiga.Value).AddDays(1), "yyyy-MM-dd") & "'" & _
                    " and TbValor.AcmId in (SELECT a.AcmId FROM TbAcomodacao a inner join TbGrupo g on a.GruId = g.GruId where g.GruId = " & cmbTipo.SelectedValue & " and AcmSuite = 'N')"
            Next

            ' CONVENIADO SUÍTE - novos valores

            For Each linha As GridViewRow In gdvConveniado.Rows
                comando += " update TbValor set ValValor = " & (CDec(CType(linha.FindControl("txtVlrConNor"), TextBox).Text) * 1.3).ToString.Replace(".", "").Replace(",", ".") & ", " & _
                    "ValDtInicial = '" & Format(CDate(txtData.Text), "yyyy-MM-dd") & _
                    "', ValDtSolIni = dateadd(hour, " & cmbHora.SelectedValue & ",'" & Format(CDate(txtDataAntiga.Text), "yyyy-MM-dd") & _
                    "') where CatId = " & CType(linha.FindControl("txtVlrConNor"), TextBox).Attributes("catId") & _
                    " and ValQtde = " & CType(linha.FindControl("txtVlrConNor"), TextBox).Attributes("valQtde") & _
                    " and ValTipo = 'R3' and ValDtInicial = '" & Format(CDate(hddData.Value), "yyyy-MM-dd") & _
                    "' and ValDtSolIni between '" & Format(CDate(hddDataAntiga.Value), "yyyy-MM-dd") & "' and '" & Format(CDate(hddDataAntiga.Value).AddDays(1), "yyyy-MM-dd") & "'" & _
                    " and TbValor.AcmId in (SELECT a.AcmId FROM TbAcomodacao a inner join TbGrupo g on a.GruId = g.GruId where g.GruId = " & cmbTipo.SelectedValue & " and AcmSuite = 'S')"
            Next


            ' USUÁRIO - novos valores

            For Each linha As GridViewRow In gdvUsuario.Rows
                comando += " update TbValor set ValValor = " & CDec(CType(linha.FindControl("txtVlrUsuNor"), TextBox).Text).ToString.Replace(".", "").Replace(",", ".") & ", " & _
                    "ValDtInicial = '" & Format(CDate(txtData.Text), "yyyy-MM-dd") & _
                    "', ValDtSolIni = dateadd(hour, " & cmbHora.SelectedValue & ",'" & Format(CDate(txtDataAntiga.Text), "yyyy-MM-dd") & _
                    "') where CatId = " & CType(linha.FindControl("txtVlrUsuNor"), TextBox).Attributes("catId") & _
                    " and ValQtde = " & CType(linha.FindControl("txtVlrUsuNor"), TextBox).Attributes("valQtde") & _
                    " and ValTipo = 'R3' and ValDtInicial = '" & Format(CDate(hddData.Value), "yyyy-MM-dd") & _
                    "' and ValDtSolIni between '" & Format(CDate(hddDataAntiga.Value), "yyyy-MM-dd") & "' and '" & Format(CDate(hddDataAntiga.Value).AddDays(1), "yyyy-MM-dd") & "'" & _
                    " and TbValor.AcmId in (SELECT a.AcmId FROM TbAcomodacao a inner join TbGrupo g on a.GruId = g.GruId where g.GruId = " & cmbTipo.SelectedValue & " and AcmSuite = 'N')"
            Next

            ' USUÁRIO SUÍTE - novos valores

            For Each linha As GridViewRow In gdvUsuario.Rows
                comando += " update TbValor set ValValor = " & (CDec(CType(linha.FindControl("txtVlrUsuNor"), TextBox).Text) * 1.3).ToString.Replace(".", "").Replace(",", ".") & ", " & _
                    "ValDtInicial = '" & Format(CDate(txtData.Text), "yyyy-MM-dd") & _
                    "', ValDtSolIni = dateadd(hour, " & cmbHora.SelectedValue & ",'" & Format(CDate(txtDataAntiga.Text), "yyyy-MM-dd") & _
                    "') where CatId = " & CType(linha.FindControl("txtVlrUsuNor"), TextBox).Attributes("catId") & _
                    " and ValQtde = " & CType(linha.FindControl("txtVlrUsuNor"), TextBox).Attributes("valQtde") & _
                    " and ValTipo = 'R3' and ValDtInicial = '" & Format(CDate(hddData.Value), "yyyy-MM-dd") & _
                    "' and ValDtSolIni between '" & Format(CDate(hddDataAntiga.Value), "yyyy-MM-dd") & "' and '" & Format(CDate(hddDataAntiga.Value).AddDays(1), "yyyy-MM-dd") & "'" & _
                    " and TbValor.AcmId in (SELECT a.AcmId FROM TbAcomodacao a inner join TbGrupo g on a.GruId = g.GruId where g.GruId = " & cmbTipo.SelectedValue & " and AcmSuite = 'S')"
            Next
            'Se tudo der certo, retornará 1
            comando += " commit tran "
            comando += " select 1 goto saida "
            comando += " end try "

            'Se der algo de errado, retornará 0 
            comando += " begin catch "
            comando += "   rollback tran "
            comando += "   Select 0 goto saida "
            comando += " end catch "
            comando += " saida: "

            'Passando a string para inserção no banco de dados.
            Dim Banco As String
            If Session("MasterPage").ToString = "~/TurismoSocial.Master" Then
                Banco = "TurismoSocialCaldas"
            Else
                Banco = "TurismoSocialPiri"
            End If
            objCadastraValoresResrevaDAO = New CadastraValoresReservaDAO(Banco)
            If objCadastraValoresResrevaDAO.CadastraNovoValores(comando) = 1 Then
                Mensagem("Novos valores inseridos com sucesso!")
                'gdvComerciario.Enabled = False
                'gdvConveniado.Enabled = False
                'gdvUsuario.Enabled = False
                'btnReservaGravar.Visible = False
                'btnTransportar.Visible = True
                ''Deixando os grid antigos desabilitado
                'gdvComerciarioA.Enabled = True
                'gdvConveniadoA.Enabled = True
                'gdvUsuarioA.Enabled = True
            Else
                Mensagem("Não foi possível inserir os novos valores.")
            End If
        Catch ex As Exception
            ScriptManager.RegisterStartupScript(Page, Page.[GetType](), Guid.NewGuid().ToString(), "alert('Atualização não foi possível.');", True)
        End Try
    End Sub
    Protected Sub Mensagem(Texto As String)
        ScriptManager.RegisterStartupScript(Page, Page.[GetType](), Guid.NewGuid().ToString(), "alert('" + Texto + "');", True)
    End Sub

    Protected Sub btnConsultaDesconto_Click(sender As Object, e As EventArgs) Handles btnConsultaDesconto.Click
        If Session("MasterPage").ToString = "~/TurismoSocial.Master" Then
            objPacoteDAO = New Turismo.PacoteDAO("TurismoSocialCaldas")
        Else
            objPacoteDAO = New Turismo.PacoteDAO("TurismoSocialPiri")
        End If
        gdvListaDescontos.DataSource = objPacoteDAO.consultarDesconto(CDate(txtDesConData1.Text), CDate(txtDesConData2.Text))
        gdvListaDescontos.DataBind()
    End Sub

    Protected Sub btnAlterar_Click(sender As Object, e As EventArgs)
        Dim Alterar As Button = sender
        Dim row As GridViewRow = Alterar.NamingContainer 'Dim index As Integer = row.RowIndex
        Dim intPasId = gdvListaDescontos.DataKeys(row.RowIndex()).Item("desId").ToString
        TableCadastroDesconto.Visible = True

        If Session("MasterPage").ToString = "~/TurismoSocial.Master" Then
            objPacoteDAO = New Turismo.PacoteDAO("TurismoSocialCaldas")
        Else
            objPacoteDAO = New Turismo.PacoteDAO("TurismoSocialPiri")
        End If
        objPacoteVO = New Turismo.PacoteVO
        objPacoteVO.desId = intPasId

        btnDescontoSalvar.Attributes.Add("intPasId", intPasId)

        'Criar a consulta por registro e exibir aqui
        objPacoteVO = objPacoteDAO.ConsultaDescontoPorId(objPacoteVO)

        With objPacoteVO
            txtPercenteDomingo.Text = FormatNumber(.PercentualDomingo, 2)
            txtPercenteSegunda.Text = FormatNumber(.PercentualSegunda, 2)
            txtPercenteTerca.Text = FormatNumber(.PercentualTerca, 2)
            txtPercenteQuarta.Text = FormatNumber(.PercentualQuarta, 2)
            txtPercenteQuinta.Text = FormatNumber(.PercentualQuinta, 2)
            txtPercenteSexta.Text = FormatNumber(.PercentualSexta, 2)
            txtPercenteSabado.Text = FormatNumber(.PercentualSabado, 2)
            txtDataIniciaDesconto.Text = Format(CDate(.DesDataInicial), "dd/MM/yyyy")
            txtDataFinalDesconto.Text = Format(CDate(.DesDataFinal), "dd/MM/yyyy")
            cmbHoraDescIni.SelectedValue = Format(CDec(.DesHoraInicial), "00")
            cmbHoraDescFim.SelectedValue = Format(CDec(.DesHoraFinal), "00")
        End With
        TableCadastroDesconto.Visible = True
        pnlDescontoConsulta.Visible = False
        txtPercenteDomingo.Focus()
    End Sub

    Protected Sub BtnDescontoVoltar_Click(sender As Object, e As EventArgs) Handles BtnDescontoVoltar.Click
        TableCadastroDesconto.Visible = False
        pnlDescontoConsulta.Visible = True
    End Sub

    Protected Sub btnDescontoSalvar_Click(sender As Object, e As EventArgs) Handles btnDescontoSalvar.Click
        If Session("MasterPage").ToString = "~/TurismoSocial.Master" Then
            objPacoteDAO = New Turismo.PacoteDAO("TurismoSocialCaldas")
        Else
            objPacoteDAO = New Turismo.PacoteDAO("TurismoSocialPiri")
        End If
        objPacoteVO = New Turismo.PacoteVO
        'Carregando os valores no objeto
        With objPacoteVO
            .desId = btnDescontoSalvar.Attributes.Item("intPasId").Replace(",", ".")
            .PercentualDomingo = FormatNumber(txtPercenteDomingo.Text, 2).Replace(",", ".")
            .PercentualSegunda = FormatNumber(txtPercenteSegunda.Text, 2).Replace(",", ".")
            .PercentualTerca = FormatNumber(txtPercenteTerca.Text, 2).Replace(",", ".")
            .PercentualQuarta = FormatNumber(txtPercenteQuarta.Text, 2).Replace(",", ".")
            .PercentualQuinta = FormatNumber(txtPercenteQuinta.Text, 2).Replace(",", ".")
            .PercentualSexta = FormatNumber(txtPercenteSexta.Text, 2).Replace(",", ".")
            .PercentualSabado = FormatNumber(txtPercenteSabado.Text, 2).Replace(",", ".")
            .DesDataInicial = Format(CDate(txtDataIniciaDesconto.Text), "yyyy-MM-dd ") & cmbHoraDescIni.SelectedValue & ":00:00"
            .DesDataFinal = Format(CDate(txtDataFinalDesconto.Text), "yyyy-MM-dd ") & cmbHoraDescFim.SelectedValue & ":59:59"
            .DesUsuarioLog = User.Identity.Name.ToString.Replace("SESC-GO.COM.BR\", "")
            '.DesHoraInicial = cmbHoraDescIni.SelectedValue
            '.DesHoraFinal = cmbHoraDescFim.SelectedValue
        End With

        Select Case objPacoteDAO.SalvarDesconto(objPacoteVO)
            Case 0
                Mensagem("Houve um erro ao salvar os dados do desconto.\n\nTente novamente, caso persista, entre em contato com o Centro de Informática.")
            Case 1
                Mensagem("Registro salvo com sucesso!")
                TableCadastroDesconto.Visible = False
                pnlDescontoConsulta.Visible = True
                btnDescontoSalvar.Attributes.Remove("intPasId")
                btnConsultaDesconto_Click(sender, e)
            Case 2
                Mensagem("Registro atualizado com sucesso!")
                TableCadastroDesconto.Visible = False
                pnlDescontoConsulta.Visible = True
                btnDescontoSalvar.Attributes.Remove("intPasId")
                btnConsultaDesconto_Click(sender, e)
        End Select
    End Sub

    Protected Sub btnVoltarDesconto_Click(sender As Object, e As EventArgs) Handles btnVoltarDesconto.Click
        pnlValor.Visible = True
        pnlAltaTemporada.Visible = False
        pnlDesconto.Visible = False
        pnlAltaTemporadaAcao.Visible = False
    End Sub

    Protected Sub btnNovoDesconto_Click(sender As Object, e As EventArgs) Handles btnNovoDesconto.Click
        pnlDescontoConsulta.Visible = False
        TableCadastroDesconto.Visible = True
        btnDescontoSalvar.Attributes.Add("intPasId", 0)
        txtPercenteDomingo.Focus()
    End Sub
End Class
