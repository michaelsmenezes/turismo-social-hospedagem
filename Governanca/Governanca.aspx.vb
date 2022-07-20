Imports System.Data.SqlClient
Imports Microsoft.VisualBasic
Imports System.DirectoryServices
Imports Microsoft.VisualBasic.Strings
Imports System.Transactions
Imports Turismo
Imports System.IO

Partial Class Governanca
    Inherits System.Web.UI.Page
    Dim ObjLimpoVO As LimpoVO
    Dim ObjLimpoDAO As LimpoDAO
    Dim ObjArrumacaoVO As ArrumacaoVO
    Dim ObjArrumacaoDAO As ArrumacaoDAO
    Dim ObjOcupadoVO As OcupadoVO
    Dim ObjOcupadoDAO As OcupadoDAO
    Dim ObjManutencaoVO As ManutencaoVO
    Dim ObjManutencaoDAO As ManutencaoDAO
    Dim ObjConferenciaVO As ConferenciaVO
    Dim ObjConferenciaDAO As ConferenciaDAO
    Dim ObjIntegranteVO As IntegrantesVO
    Dim ObjIntegranteDAO As IntegrantesDAO
    Dim ObjPrioridadeVO As PrioridadeVO
    Dim ObjPrioridadeDAO As PrioridadeDAO
    'CLASSES PARA O ATENDIMENTO'
    Dim ObjApartamentosVO As ApartamentosVO
    Dim ObjApartamentosDAO As ApartamentosDAO
    Dim ObjCamareiraVO As CamareiraVO
    Dim ObjCamareiraDAO As CamareiraDAO
    Dim ObjAtendimentoVO As AtendimentoGovVO
    Dim ObjAtendimentoDAO As AtendimentoGovDAO
    Dim cont As Integer = 0
    'ESSA VARIÁVEL SERÁ UTILIZADA NO ATENDIMENTO DO APTO, SE FOR DIFERENTE DE 0 INDICA O FILTRO DE APENAS UM APARTAMENTO
    Dim AtendimentoUnico As Long = -1
    Dim ObjEmprestimosVO As EmprestimosVO
    Dim ObjEmprestimosDAO As EmprestimosDAO
    'SOLICITAÇÃO DE HELP DESK MANUTENÇÃO COM BLOQUEIO'
    Dim ObjSolicitacaoVO As SolicitacaoVO
    Dim ObjSolicitacaoDAO As SolicitacaoDAO
    'FUNCIONARIO PARA INSERÇÃO DO HELP DESK MANUTENÇÃO
    Dim ObjFuncionarioVO As FuncionarioVO
    Dim ObjFuncionarioDAO As FuncionarioDAO
    'CONSULTANDO BEM PATRIMONIAL PARA HELP DESK
    Dim ObjBemVO As BemVO
    Dim ObjBemDAO As BemDAO
    Dim ObjItemConsertoVO As ItemConsertoVO
    Dim ObjItemConsertoDAO As ItemConsertoDAO
    Dim ObjBloqueioAptoVO As BloqueioAptoVO
    Dim ObjBloqueioAptoDAO As BloqueiAptoDAO
    'CLASSE TURISMOSOCIAL
    Dim objSituacaoAtualVO As SituacaoAtualVO
    Dim objSituacaoAtualDAO As SituacaoAtualDAO
    Dim objCheckInOutDAO As CheckInOutDAO
    Dim objCheckInOutVO As CheckInOutVO
    Protected Sub Page_PreInit(ByVal sender As Object, ByVal e As EventArgs) Handles Me.PreInit
        If Session("MasterPage").ToString = "~/TurismoSocial.Master" Then
            Page.MasterPageFile = "~/TurismoSocial.Master"
        Else
            Page.MasterPageFile = "~/TurismoSocialPiri.Master"
        End If
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        'Aplicando filtro de acordo com a session filtro'
        'For Each item As OcupadoVO In ListaOcupados
        'If btnAuxilarMan.Attributes.Item("AplicarFiltro") = "S" Then
        '    Dim ListaFiltros As IList
        '    ListaFiltros = New ArrayList
        '    ListaFiltros = Session("Filtro")
        '    While cont <= 5
        '        If ListaFiltros.Item(cont).ToString = True Then
        '            chkConsultas.Items(cont).Selected = True
        '        Else
        '            chkConsultas.Items(cont).Selected = False
        '        End If
        '        cont = cont + 1
        '    End While
        'End If

        If Not IsPostBack Then
            Select Case Session("MasterPage").ToString
                Case Is = "~/TurismoSocial.Master"
                    'Direcionando a aplicação para o banco de Caldas Novas
                    btnUnidade.Attributes.Add("AliasBancoTurismo", "TurismoSocialCaldas")
                    'Direcionando para o help desk de Caldas Novas
                    btnUnidade.Attributes.Add("AliasBancoHdManutencao", "HDManutencao")
                    'Usado na pagina de relatórios
                    btnUnidade.Attributes.Add("UOP", "Caldas Novas")
                    'Uso na select onde tem o servidor [dbTurismoSocial].[dbo]...
                    btnUnidade.Attributes.Add("BancoTurismoSocial", "dbTurismoSocial")
                    btnUnidade.Attributes.Add("IdUnidade", "102")
                    btnUnidade.Attributes.Add("UnidadeEscritorio", "Caldas Novas")
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
                    btnUnidade.Attributes.Add("AliasBancoTurismo", "TurismoSocialPiri")
                    'Direcionando para o help desk 
                    btnUnidade.Attributes.Add("AliasBancoHdManutencao", "HDManutencao")
                    'Usado na pagina de relatórios
                    btnUnidade.Attributes.Add("UOP", "Pirenopolis")
                    'Uso na select onde tem o servidor [dbTurismoSocial].[dbo]...
                    btnUnidade.Attributes.Add("BancoTurismoSocial", "dbTurismoSocialPiri")
                    btnUnidade.Attributes.Add("IdUnidade", "109")
                    btnUnidade.Attributes.Add("UnidadeEscritorio", "Pirenópolis")
                    'Alterando dinamicamente as cores da página
                    Dim myHtmlLink As New HtmlLink()
                    Page.Header.Controls.Remove(myHtmlLink)
                    myHtmlLink.Href = "~/stylesheetverde.css"
                    myHtmlLink.Attributes.Add("rel", "stylesheet")
                    myHtmlLink.Attributes.Add("type", "text/css")
                    'Adicionando o Css na Sessão Head da Página
                    Page.Header.Controls.Add(myHtmlLink)
            End Select
            'Aplicando efeito modal no painel enxoval
            divModalEnxoval.Attributes("Class") = "DivFundoTransparencia"
            'SÓ DEIXARÁ ACESSAR O SISTEMA SE PERTENCER AO GRUPO DE GOVERNANÇA'
            Dim TestaUsuario As New Uteis.TestaUsuario 'Instanciando o objeto testa usuario'
            Dim Grupos As String = TestaUsuario.listaGrupos(Replace(User.Identity.Name, "SESC-GO.COM.BR\", ""))
            If Not (Grupos.Contains("CNV_GOVERNANCA") Or Grupos.Contains("Turismo Social Total") Or Grupos.Contains("Turismo Social Piri Governanca")) Then
                Response.Redirect("AcessoNegado.aspx")
                Return
            End If

            'PEGANDO O NOME COMPLETO E MATRICULA DO USUÁRIO LOGADO NO MOMENTO'
            Dim search As DirectorySearcher = New DirectorySearcher("LDAP://sesc-go.com.br/DC=sesc-go,DC=com,DC=br")
            search.Filter = "(SAMAccountName=" + Replace(Context.User.Identity.Name.ToString, "SESC-GO.COM.BR\", "").ToUpper + ")"
            Dim result As SearchResult = search.FindOne()
            search.PropertiesToLoad.Add("displayName")
            Dim nomeUsuario As String = result.Properties("displayName").Item(0).ToString
            Dim MatriculaAD As String = result.Properties("Initials").Item(0).ToString
            btnAuxilarMan.Attributes.Add("MatriculaAd", MatriculaAD)
            btnAuxilarMan.Attributes.Add("Usuario", nomeUsuario)
            'INICIANDO VALIDAÇÕES DOS CAMPOS, LIMITANDO DIGITAÇÃO.
            txtDataPrevisao.Attributes.Add("OnKeyup", "javascript:if((window.event.keyCode != 9) && (window.event.keyCode != 8) && (window.event.keyCode != 16)) {this.value=FormataData(this.value)}")
            txtDataPrevisao.Attributes.Add("Onblur", "javascript:if(!ValidaData(this,'S')){alert('Data inválida!')};")
            txtDataPrevisao.Attributes.Add("onKeyDown", "javascript:desabilitaEnter();")
            txtDescricaoManutencao.Attributes.Add("OnKeyPress", "javascript:limitaTamanhoCampo(this,500)")

            lnkManutencaoOcupado.Attributes.Add("onmouseover", "javascript:aspnetForm.ctl00_conPlaHolTurismoSocial_txtDescricaoMotivoOcu.value = 'MANUTENÇÃO: Apartamento ' + aspnetForm.ctl00_conPlaHolTurismoSocial_txtApto.value;aspnetForm.ctl00_conPlaHolTurismoSocial_txtAssuntoManutencao.value='';aspnetForm.ctl00_conPlaHolTurismoSocial_txtDescricaoManutencao.value='';")
            lnkManutencaoLimpo.Attributes.Add("onmouseover", "javascript:aspnetForm.ctl00_conPlaHolTurismoSocial_txtDescricaoMotivoOcu.value = 'MANUTENÇÃO: Apartamento ' + aspnetForm.ctl00_conPlaHolTurismoSocial_txtAptoLimpo.value;aspnetForm.ctl00_conPlaHolTurismoSocial_txtAssuntoManutencao.value='';aspnetForm.ctl00_conPlaHolTurismoSocial_txtDescricaoManutencao.value='';aspnetForm.ctl00_conPlaHolTurismoSocial_txtPegaOrigemMan.value='L';")
            lnkManutencaoArrumacao.Attributes.Add("onmouseover", "javascript:aspnetForm.ctl00_conPlaHolTurismoSocial_txtDescricaoMotivoOcu.value = 'MANUTENÇÃO: Apartamento ' + aspnetForm.ctl00_conPlaHolTurismoSocial_txtAptoArrumacao.value;aspnetForm.ctl00_conPlaHolTurismoSocial_txtAssuntoManutencao.value='';aspnetForm.ctl00_conPlaHolTurismoSocial_txtDescricaoManutencao.value='';aspnetForm.ctl00_conPlaHolTurismoSocial_txtPegaOrigemMan.value='A';")
            lnkManutencaoMan.Attributes.Add("onmouseover", "javascript:aspnetForm.ctl00_conPlaHolTurismoSocial_txtDescricaoMotivoOcu.value = 'MANUTENÇÃO: Apartamento ' + aspnetForm.ctl00_conPlaHolTurismoSocial_txtAptoManutencao.value;aspnetForm.ctl00_conPlaHolTurismoSocial_txtAssuntoManutencao.value='';aspnetForm.ctl00_conPlaHolTurismoSocial_txtDescricaoManutencao.value='';aspnetForm.ctl00_conPlaHolTurismoSocial_txtPegaOrigemMan.value='M';")
            'USADO PARA GERAR INFORMAÇÕES PARA OS APTOS EM MANUTENÇÃO - APACUSTO,APAAREA..
            'lnkManutencaoArrumacao.Attributes.Add("onClick", "javascript:aspnetForm.ctl00_conPlaHolTurismoSocial_btnAuxilarMan.click();")
            'lnkManutencaoLimpo.Attributes.Add("onClick", "javascript:aspnetForm.ctl00_conPlaHolTurismoSocial_btnAuxilarMan.click();")
            'lnkManutencaoOcupado.Attributes.Add("onClick", "javascript:aspnetForm.ctl00_conPlaHolTurismoSocial_btnAuxilarMan.click();")
            'lnkManutencaoMan.Attributes.Add("onClick", "javascript:aspnetForm.ctl00_conPlaHolTurismoSocial_btnAuxilarMan.click();")
            'CARREGANDO LISTA DE CAMAREIRAS'
            ObjCamareiraVO = New CamareiraVO
            ObjCamareiraDAO = New CamareiraDAO()
            Session("ListaCamareiras") = ObjCamareiraDAO.ConsultarCamareira(ObjCamareiraVO, btnUnidade.Attributes.Item("AliasBancoTurismo").ToString)
            'BUSCANDO O APARTAMENTO'
            ObjApartamentosVO = New ApartamentosVO
            ObjApartamentosDAO = New ApartamentosDAO()
            'EMPRESTIMOS'
            txtEmpData.Attributes.Add("OnKeyup", "javascript:if((window.event.keyCode != 9) && (window.event.keyCode != 8) && (window.event.keyCode != 16)) {this.value=FormataData(this.value)}")
            txtEmpData.Attributes.Add("Onblur", "javascript:if(!ValidaData(this,'S')){alert('Data inválida!')};")
            txtEmpData.Attributes.Add("onKeyDown", "javascript:desabilitaEnter();")
            'DATA DO PAINEL DE ARRUMAÇÃO
            txtData1Arrumacao.Attributes.Add("OnKeyup", "javascript:if((window.event.keyCode != 9) && (window.event.keyCode != 8) && (window.event.keyCode != 16)) {this.value=FormataData(this.value)}")
            txtData1Arrumacao.Attributes.Add("Onblur", "javascript:if(!ValidaData(this,'S')){alert('Data inválida!')};")
            txtData1Arrumacao.Attributes.Add("onKeyDown", "javascript:desabilitaEnter();")
            txtData2Arrumacao.Attributes.Add("OnKeyup", "javascript:if((window.event.keyCode != 9) && (window.event.keyCode != 8) && (window.event.keyCode != 16)) {this.value=FormataData(this.value)}")
            txtData2Arrumacao.Attributes.Add("Onblur", "javascript:if(!ValidaData(this,'S')){alert('Data inválida!')};")
            txtData2Arrumacao.Attributes.Add("onKeyDown", "javascript:desabilitaEnter();")
            'DATA DO HISTÓRICO DE MANUTENÇÃO
            txtHistoricoManutencaoD1.Attributes.Add("OnKeyup", "javascript:if((window.event.keyCode != 9) && (window.event.keyCode != 8) && (window.event.keyCode != 16)) {this.value=FormataData(this.value)}")
            txtHistoricoManutencaoD1.Attributes.Add("Onblur", "javascript:if(!ValidaData(this,'S')){alert('Data inválida!')};")
            txtHistoricoManutencaoD1.Attributes.Add("onKeyDown", "javascript:desabilitaEnter();")
            txtHistoricoManutencaoD2.Attributes.Add("OnKeyup", "javascript:if((window.event.keyCode != 9) && (window.event.keyCode != 8) && (window.event.keyCode != 16)) {this.value=FormataData(this.value)}")
            txtHistoricoManutencaoD2.Attributes.Add("Onblur", "javascript:if(!ValidaData(this,'S')){alert('Data inválida!')};")
            txtHistoricoManutencaoD2.Attributes.Add("onKeyDown", "javascript:desabilitaEnter();")
            txtEmpValor.Attributes.Add("OnKeyUp", "javascript:if((window.event.keyCode != 9) && (window.event.keyCode != 16)){this.value=ColocaVirgula(this.value);};") 'Formato money
            txtQuantidade.Attributes.Add("OnKeyUp", "javascript:if((window.event.keyCode != 9) && (window.event.keyCode != 16)){this.value=ColocaVirgula(this.value);};") 'Formato money
            txtEmpValor.Attributes.Add("OnFocus", "javascript:this.focus();this.select()")
            'lnkManutencaoArrumacao.Attributes.Add("OnClick", "Javascript:aspnetForm.ctl00_conPlaHolTurismoSocial_txtDescricaoMotivoOcu.value = 'Apartamento: ' + aspnetForm.ctl00_conPlaHolTurismoSocial_txtAptoArrumacao.value + ' | Informe o motivo da manutenção:'")
            'lnkManutencaoOcupado.Attributes.Add("OnClick", "Javascript:aspnetForm.ctl00_conPlaHolTurismoSocial_txtDescricaoMotivoOcu.value = 'Apartamento: ' + aspnetForm.ctl00_conPlaHolTurismoSocial_txtApto.value + ' | Informe o motivo da manutenção:'")
            'lnkManutencaoLimpo.Attributes.Add("OnClick", "Javascript:aspnetForm.ctl00_conPlaHolTurismoSocial_txtDescricaoMotivoOcu.value = 'Apartamento: ' + aspnetForm.ctl00_conPlaHolTurismoSocial_txtAptoLimpo.value + ' | Informe o motivo da manutenção:'")
            'lnkManutencaoMan.Attributes.Add("OnClick", "Javascript:aspnetForm.ctl00_conPlaHolTurismoSocial_txtDescricaoMotivoOcu.value = 'Apartamento: ' + aspnetForm.ctl00_conPlaHolTurismoSocial_txtAptoManutencao.value + ' | Informe o motivo da manutenção:'")
            'PAINEL DE ATENDIMENTO(OCUPADO) 
            '==============================================
            'CARREGANDO LISTA DE CAMAREIRAS PAINEL DE ATENDIMENTO'
            drpCamareiraArrumacao.DataValueField = "CamId"
            drpCamareiraArrumacao.DataTextField = "CamNome"
            drpCamareiraArrumacao.DataSource = Session("ListaCamareiras")
            drpCamareiraArrumacao.DataBind()
            drpCamareiraArrumacao.Items.Insert(0, New ListItem("Governança(não excluir)", "0"))
            'PAINEL DE ATENDIMENTO - BLOQUEANDO A DIGITAÇÃO DE STRING NOS CAMPOS
            txtCamaCasalArrumacao.Attributes.Add("onKeyPress", "javascript:SomenteNumeros(this.value);")
            TxtLenSolteiroArrumacao.Attributes.Add("onKeyPress", "javascript:SomenteNumeros(this.value);")
            txtlenBercoArrumacao.Attributes.Add("onKeyPress", "javascript:SomenteNumeros(this.value);")
            txtCamaSolteiroArrumacao.Attributes.Add("onKeyPress", "javascript:SomenteNumeros(this.value);")
            txtBercoArrumacao.Attributes.Add("onKeyPress", "javascript:SomenteNumeros(this.value);")
            txtFronhaArrumacao.Attributes.Add("onKeyPress", "javascript:SomenteNumeros(this.value);")
            txtToalhaArrumacao.Attributes.Add("onKeyPress", "javascript:SomenteNumeros(this.value);")
            txtPapelArrumacao.Attributes.Add("onKeyPress", "javascript:SomenteNumeros(this.value);")
            txtLixoArrumacao.Attributes.Add("onKeyPress", "javascript:SomenteNumeros(this.value);")
            txtSaboneteArrumacao.Attributes.Add("onKeyPress", "javascript:SomenteNumeros(this.value);")
            txtCamaExtraArrumacao.Attributes.Add("onKeyPress", "javascript:SomenteNumeros(this.value);")
            'PAINEL DE ATENDIMENTO(LIMPO) 
            '==============================================
            drpCamareiraL.DataValueField = "CamId"
            drpCamareiraL.DataTextField = "CamNome"
            drpCamareiraL.DataSource = Session("ListaCamareiras")
            drpCamareiraL.DataBind()
            drpCamareiraL.Items.Insert(0, New ListItem("Governança(não excluir)", "0"))
            'PAINEL DE ATENDIMENTO (LIMPO) - BLOQUEANDO A DIGITAÇÃO DE STRING NOS CAMPOS
            txtCamaCasalL.Attributes.Add("onKeyPress", "javascript:SomenteNumeros(this.value);")
            txtCamaSolteiroL.Attributes.Add("onKeyPress", "javascript:SomenteNumeros(this.value);")
            txtBercoL.Attributes.Add("onKeyPress", "javascript:SomenteNumeros(this.value);")
            txtFronhaL.Attributes.Add("onKeyPress", "javascript:SomenteNumeros(this.value);")
            txtToalhaL.Attributes.Add("onKeyPress", "javascript:SomenteNumeros(this.value);")
            txtPapelL.Attributes.Add("onKeyPress", "javascript:SomenteNumeros(this.value);")
            txtLixoL.Attributes.Add("onKeyPress", "javascript:SomenteNumeros(this.value);")
            txtSaboneteL.Attributes.Add("onKeyPress", "javascript:SomenteNumeros(this.value);")
            txtCamaExtraL.Attributes.Add("onKeyPress", "javascript:SomenteNumeros(this.value);")
            'PAINEL DE ATENDIMENTO(ARRUMAÇÃO) 
            '==============================================
            drpCamareiraAr.DataValueField = "CamId"
            drpCamareiraAr.DataTextField = "CamNome"
            drpCamareiraAr.DataSource = Session("ListaCamareiras")
            drpCamareiraAr.DataBind()
            drpCamareiraAr.Items.Insert(0, New ListItem("Governança(não excluir)", "0"))
            'PAINEL DE ATENDIMENTO (ARRUMAÇÃO) - BLOQUEANDO A DIGITAÇÃO DE STRING NOS CAMPOS
            txtCamaCasalAr.Attributes.Add("onKeyPress", "javascript:SomenteNumeros(this.value);")
            txtCamaSolteiroAr.Attributes.Add("onKeyPress", "javascript:SomenteNumeros(this.value);")
            txtBercoAr.Attributes.Add("onKeyPress", "javascript:SomenteNumeros(this.value);")
            txtFronhaAr.Attributes.Add("onKeyPress", "javascript:SomenteNumeros(this.value);")
            txtToalhaAr.Attributes.Add("onKeyPress", "javascript:SomenteNumeros(this.value);")
            txtPapelAr.Attributes.Add("onKeyPress", "javascript:SomenteNumeros(this.value);")
            txtLixoAr.Attributes.Add("onKeyPress", "javascript:SomenteNumeros(this.value);")
            txtSaboneteAr.Attributes.Add("onKeyPress", "javascript:SomenteNumeros(this.value);")
            'GUARDANDO INFORMAÇÕES NO LINK DE CADA LINK PARA PASSAR COMO PARAMETRO NO CASO DE MANUTENÇÃO
            'lnkManutencaoArrumacao.Attributes.Add("OnClick", "javascript:aspnetForm.ctl00_conPlaHolTurismoSocial_txtAuxilioManutencao.value ='Arrumacao',aspnetForm.ctl00_conPlaHolTurismoSocial_txtMotivoManutencao.value='',aspnetForm.ctl00_conPlaHolTurismoSocial_txtMotivoManutencao.focus()")
            'lnkManutencaoOcupado.Attributes.Add("OnClick", "javascript:aspnetForm.ctl00_conPlaHolTurismoSocial_txtAuxilioManutencao.value ='Ocupado',aspnetForm.ctl00_conPlaHolTurismoSocial_txtMotivoManutencao.value='',aspnetForm.ctl00_conPlaHolTurismoSocial_txtMotivoManutencao.focus()")
            'lnkManutencaoLimpo.Attributes.Add("OnClick", "javascript:aspnetForm.ctl00_conPlaHolTurismoSocial_txtAuxilioManutencao.value ='Limpo',aspnetForm.ctl00_conPlaHolTurismoSocial_txtMotivoManutencao.value='',aspnetForm.ctl00_conPlaHolTurismoSocial_txtMotivoManutencao.focus()")
            'Alimentando ApaId e Apadesc ao clicar no link de Atendimento de cada popup
            lnkAtendimentoOcupado.Attributes.Add("OnClick", "javascript:aspnetForm.ctl00_conPlaHolTurismoSocial_hddApaDesc.value=aspnetForm.ctl00_conPlaHolTurismoSocial_txtApto.value")
            lnkAtendimentoArrumacao.Attributes.Add("OnClick", "javascript:aspnetForm.ctl00_conPlaHolTurismoSocial_hddApaDesc.value=aspnetForm.ctl00_conPlaHolTurismoSocial_txtAptoArrumacao.value")
            lnkRevisaoLimpo.Attributes.Add("OnClick", "javascript:aspnetForm.ctl00_conPlaHolTurismoSocial_hddApaDesc.value=aspnetForm.ctl00_conPlaHolTurismoSocial_txtAptoLimpo.value")
            lnkPrioridadeAten.Attributes.Add("OnClick", "javascript:aspnetForm.ctl00_conPlaHolTurismoSocial_hddApaDesc.value=aspnetForm.ctl00_conPlaHolTurismoSocial_txtAptoPrioridade.value")
            'EFETUA A PRIMEIRA BUSCA AUTOMATICAMENTE'
            '            Form.Attributes.Add("onLoad", "javascript:aspnetForm.ctl00_conPlaHolTurismoSocial_btnConsultar.click();")

            'Só poderá ver quem liberou os apartamentos, que estiver nesse grupo da govenança
            If Grupos.Contains("Turismo Social Governanca Gerencial") Or Grupos.Contains("Turismo Social Piri Governanca Gerencial") Then
                lnkLimpezaA.Visible = True
                lnkLimpezaL.Visible = True
                lnkLimpezaM.Visible = True
                lnkLimpezaO.Visible = True
                imbLimpezaA.Visible = True
                imbLimpezaL.Visible = True
                imbLimpezaM.Visible = True
                imbLimpezaO.Visible = True
            Else
                lnkLimpezaA.Visible = False
                lnkLimpezaL.Visible = False
                lnkLimpezaM.Visible = False
                lnkLimpezaO.Visible = False
                imbLimpezaA.Visible = False
                imbLimpezaL.Visible = False
                imbLimpezaM.Visible = False
                imbLimpezaO.Visible = False
            End If
            btnConsultar_Click(sender, e)
        End If
        'Movendo o focu para dentro da caixa de texto do apartamento
        'ScriptManager.RegisterStartupScript(txtApartamento, txtApartamento.GetType(), Guid.NewGuid().ToString(), "$get('" + txtApartamento.ClientID + "').focus();", True)
    End Sub

    Protected Sub gdvLimpo1_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gdvLimpo1.RowDataBound
        'E.ROW.ROWTYPE = DATACONTROLROWTYPE.DATAROW SIGNIFICA LINHAS REFERENTE A DADOS NO GRID - NÃO CABEÇALHO E NEM RODAPÉ
        If e.Row.RowType = DataControlRowType.DataRow Then
            CType(e.Row.FindControl("lblLimpo1"), Label).Text = gdvLimpo1.DataKeys(e.Row.RowIndex).Item(1).ToString
            CType(e.Row.FindControl("lblLimpo1"), Label).Attributes.Add("onClick", "javascript:aspnetForm.ctl00_conPlaHolTurismoSocial_txtAptoLimpo.value = '" & gdvLimpo1.DataKeys(e.Row.RowIndex).Item(1).ToString & "' ,aspnetForm.ctl00_conPlaHolTurismoSocial_hddAptoLimpo.value = " & gdvLimpo1.DataKeys(e.Row.RowIndex).Item(0).ToString & ",aspnetForm.ctl00_conPlaHolTurismoSocial_hddApaId.value = " & gdvLimpo1.DataKeys(e.Row.RowIndex).Item(0).ToString & ",aspnetForm.ctl00_conPlaHolTurismoSocial_hddApaDesc.value = '" & gdvLimpo1.DataKeys(e.Row.RowIndex).Item(1).ToString & "',aspnetForm.ctl00_conPlaHolTurismoSocial_hddApaCCusto.value = '" & gdvLimpo1.DataKeys(e.Row.RowIndex).Item(4).ToString & "',aspnetForm.ctl00_conPlaHolTurismoSocial_hddApaArea.value = '" & gdvLimpo1.DataKeys(e.Row.RowIndex).Item(5).ToString & "' ")
            'CType(e.Row.FindControl("lblLimpo1"), Label).ToolTip = "Limpo por: " & Chr(13) & "Usuário: " & Replace(gdvLimpo1.DataKeys(e.Row.RowIndex).Item(3).ToString, "SESC-GO.COM.BR\", "") & Chr(13) & "Em: " & Format(CDate(gdvLimpo1.DataKeys(e.Row.RowIndex).Item(2).ToString), "dd/MM/yyyy HH:mm:ss")
        End If
    End Sub

    Protected Sub gdvLimpo2_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gdvLimpo2.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            CType(e.Row.FindControl("lblLimpo2"), Label).Text = gdvLimpo2.DataKeys(e.Row.RowIndex).Item(1).ToString
            CType(e.Row.FindControl("lblLimpo2"), Label).Attributes.Add("onClick", "javascript:aspnetForm.ctl00_conPlaHolTurismoSocial_txtAptoLimpo.value = '" & gdvLimpo2.DataKeys(e.Row.RowIndex).Item(1).ToString & "' ,aspnetForm.ctl00_conPlaHolTurismoSocial_hddAptoLimpo.value = " & gdvLimpo2.DataKeys(e.Row.RowIndex).Item(0).ToString & ",aspnetForm.ctl00_conPlaHolTurismoSocial_hddApaId.value = " & gdvLimpo2.DataKeys(e.Row.RowIndex).Item(0).ToString & ",aspnetForm.ctl00_conPlaHolTurismoSocial_hddApaDesc.value = '" & gdvLimpo2.DataKeys(e.Row.RowIndex).Item(1).ToString & "',aspnetForm.ctl00_conPlaHolTurismoSocial_hddApaCCusto.value = '" & gdvLimpo2.DataKeys(e.Row.RowIndex).Item(4).ToString & "',aspnetForm.ctl00_conPlaHolTurismoSocial_hddApaArea.value = '" & gdvLimpo2.DataKeys(e.Row.RowIndex).Item(5).ToString & "' ")
            'CType(e.Row.FindControl("lblLimpo2"), Label).ToolTip = "Limpo por: " & Chr(13) & "Usuário: " & Replace(gdvLimpo2.DataKeys(e.Row.RowIndex).Item(3).ToString, "SESC-GO.COM.BR\", "") & Chr(13) & "Em: " & Format(CDate(gdvLimpo2.DataKeys(e.Row.RowIndex).Item(2).ToString), "dd/MM/yyyy HH:mm:ss")
        End If
    End Sub

    Protected Sub gdvLimpo3_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gdvLimpo3.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            'CType(e.Row.FindControl("lnkLimpo3"), LinkButton).Attributes.Add("onclick", "popLayer('<b>Apartamento: " & gdvLimpo3.DataKeys(e.Row.RowIndex).Item(1).ToString & "</b><br><br> <a href=http://intranetCaldas>Arrumação</a>&nbsp<img src=" & "images/StatusAptoAmarelo.gif" & ">&nbsp<br><a href=Revisão>Revisão</a><img src=" & "images/Lupa.jpg" & "><br><a href=Manutenção>Manutenção</a>&nbsp<img src=" & "images/StatusAptoCinza.gif" & "> ')")
            CType(e.Row.FindControl("lblLimpo3"), Label).Text = gdvLimpo3.DataKeys(e.Row.RowIndex).Item(1).ToString
            CType(e.Row.FindControl("lblLimpo3"), Label).Attributes.Add("onClick", "javascript:aspnetForm.ctl00_conPlaHolTurismoSocial_txtAptoLimpo.value = '" & gdvLimpo3.DataKeys(e.Row.RowIndex).Item(1).ToString & "' ,aspnetForm.ctl00_conPlaHolTurismoSocial_hddAptoLimpo.value = " & gdvLimpo3.DataKeys(e.Row.RowIndex).Item(0).ToString & ",aspnetForm.ctl00_conPlaHolTurismoSocial_hddApaId.value = " & gdvLimpo3.DataKeys(e.Row.RowIndex).Item(0).ToString & ",aspnetForm.ctl00_conPlaHolTurismoSocial_hddApaDesc.value = '" & gdvLimpo3.DataKeys(e.Row.RowIndex).Item(1).ToString & "',aspnetForm.ctl00_conPlaHolTurismoSocial_hddApaCCusto.value = '" & gdvLimpo3.DataKeys(e.Row.RowIndex).Item(4).ToString & "',aspnetForm.ctl00_conPlaHolTurismoSocial_hddApaArea.value = '" & gdvLimpo3.DataKeys(e.Row.RowIndex).Item(5).ToString & "' ")
            'CType(e.Row.FindControl("lblLimpo3"), Label).ToolTip = "Limpo por: " & Chr(13) & "Usuário: " & Replace(gdvLimpo3.DataKeys(e.Row.RowIndex).Item(3).ToString, "SESC-GO.COM.BR\", "") & Chr(13) & "Em: " & Format(CDate(gdvLimpo3.DataKeys(e.Row.RowIndex).Item(2).ToString), "dd/MM/yyyy HH:mm:ss")
        End If
    End Sub

    Protected Sub gdvLimpo4_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gdvLimpo4.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            'CType(e.Row.FindControl("lnkLimpo4"), LinkButton).Attributes.Add("onclick", "popLayer('<b>Apartamento: " & gdvLimpo4.DataKeys(e.Row.RowIndex).Item(1).ToString & "</b><br><br> <a href=http://intranetCaldas>Arrumação</a>&nbsp<img src=" & "images/StatusAptoAmarelo.gif" & ">&nbsp<br><a href=Revisão>Revisão</a><img src=" & "images/Lupa.jpg" & "><br><a href=Manutenção>Manutenção</a>&nbsp<img src=" & "images/StatusAptoCinza.gif" & "> ')")
            CType(e.Row.FindControl("lblLimpo4"), Label).Text = gdvLimpo4.DataKeys(e.Row.RowIndex).Item(1).ToString
            CType(e.Row.FindControl("lblLimpo4"), Label).Attributes.Add("onClick", "javascript:aspnetForm.ctl00_conPlaHolTurismoSocial_txtAptoLimpo.value = '" & gdvLimpo4.DataKeys(e.Row.RowIndex).Item(1).ToString & "' ,aspnetForm.ctl00_conPlaHolTurismoSocial_hddAptoLimpo.value = " & gdvLimpo4.DataKeys(e.Row.RowIndex).Item(0).ToString & ",aspnetForm.ctl00_conPlaHolTurismoSocial_hddApaId.value = " & gdvLimpo4.DataKeys(e.Row.RowIndex).Item(0).ToString & ",aspnetForm.ctl00_conPlaHolTurismoSocial_hddApaDesc.value = '" & gdvLimpo4.DataKeys(e.Row.RowIndex).Item(1).ToString & "',aspnetForm.ctl00_conPlaHolTurismoSocial_hddApaCCusto.value = '" & gdvLimpo4.DataKeys(e.Row.RowIndex).Item(4).ToString & "',aspnetForm.ctl00_conPlaHolTurismoSocial_hddApaArea.value = '" & gdvLimpo4.DataKeys(e.Row.RowIndex).Item(5).ToString & "' ")
            'CType(e.Row.FindControl("lblLimpo4"), Label).ToolTip = "Limpo por: " & Chr(13) & "Usuário: " & Replace(gdvLimpo4.DataKeys(e.Row.RowIndex).Item(3).ToString, "SESC-GO.COM.BR\", "") & Chr(13) & "Em: " & Format(CDate(gdvLimpo4.DataKeys(e.Row.RowIndex).Item(2).ToString), "dd/MM/yyyy HH:mm:ss")
        End If
    End Sub

    Protected Sub gdvOcupado1_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gdvOcupado1.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            If gdvOcupado1.DataKeys(e.Row.RowIndex).Item(3).ToString = "S" And _
            (gdvOcupado1.DataKeys(e.Row.RowIndex).Item(5).ToString = "EG") Then
                CType(e.Row.FindControl("imgEmprestimos"), Image).Visible = True
                CType(e.Row.FindControl("imgEmprestimos"), Image).ToolTip = gdvOcupado1.DataKeys(e.Row.RowIndex).Item(6).ToString.ToUpper
            Else
                CType(e.Row.FindControl("imgEmprestimos"), Image).Visible = False
            End If
            'Se HosDataFimSol for = ao dia de hoje, transforma a cor do label em laranja'
            If Format(CDate(gdvOcupado1.DataKeys(e.Row.RowIndex).Item(2).ToString), "yyyy-MM-dd hh:mm:ss") <= Format(CDate(Date.Now), "yyyy-MM-dd 23:59:59") Then
                CType(e.Row.FindControl("lblOcupado1"), Label).ForeColor = Drawing.Color.Orange
                CType(e.Row.FindControl("lblOcupado1"), Label).Font.Bold = True
            End If
            CType(e.Row.FindControl("lblOcupado1"), Label).Text = gdvOcupado1.DataKeys(e.Row.RowIndex).Item(1).ToString
            CType(e.Row.FindControl("lblOcupado1"), Label).Attributes.Add("onClick", "javascript:aspnetForm.ctl00_conPlaHolTurismoSocial_txtApto.value = '" & gdvOcupado1.DataKeys(e.Row.RowIndex).Item(1).ToString & "',aspnetForm.ctl00_conPlaHolTurismoSocial_hddAptoOcupado.value = " & gdvOcupado1.DataKeys(e.Row.RowIndex).Item(0).ToString & ",aspnetForm.ctl00_conPlaHolTurismoSocial_hddApaId.value = " & gdvOcupado1.DataKeys(e.Row.RowIndex).Item(0).ToString & ",aspnetForm.ctl00_conPlaHolTurismoSocial_hddApaDesc.value = '" & gdvOcupado1.DataKeys(e.Row.RowIndex).Item(1).ToString & "',aspnetForm.ctl00_conPlaHolTurismoSocial_hddApaCCusto.value = '" & gdvOcupado1.DataKeys(e.Row.RowIndex).Item(7).ToString & "',aspnetForm.ctl00_conPlaHolTurismoSocial_hddApaArea.value = '" & gdvOcupado1.DataKeys(e.Row.RowIndex).Item(8).ToString & "' ")
        End If
    End Sub

    Protected Sub gdvOcupado2_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gdvOcupado2.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            If (gdvOcupado2.DataKeys(e.Row.RowIndex).Item(3).ToString = "S") And _
               (gdvOcupado2.DataKeys(e.Row.RowIndex).Item(5).ToString = "EG") Then
                CType(e.Row.FindControl("imgEmprestimos"), Image).Visible = True
                CType(e.Row.FindControl("imgEmprestimos"), Image).ToolTip = gdvOcupado2.DataKeys(e.Row.RowIndex).Item(6).ToString.ToUpper
            Else
                CType(e.Row.FindControl("imgEmprestimos"), Image).Visible = False
            End If
            'Se HosDataFimSol for = ao dia de hoje, transforma a cor do label em laranja'
            If Format(CDate(gdvOcupado2.DataKeys(e.Row.RowIndex).Item(2).ToString), "yyyy-MM-dd hh:mm:ss") <= Format(CDate(Date.Now), "yyyy-MM-dd 23:59:59") Then
                CType(e.Row.FindControl("lblOcupado2"), Label).ForeColor = Drawing.Color.Orange
                CType(e.Row.FindControl("lblOcupado2"), Label).Font.Bold = True
            End If
            CType(e.Row.FindControl("lblOcupado2"), Label).Text = gdvOcupado2.DataKeys(e.Row.RowIndex).Item(1).ToString
            CType(e.Row.FindControl("lblOcupado2"), Label).Attributes.Add("onClick", "javascript:aspnetForm.ctl00_conPlaHolTurismoSocial_txtApto.value = '" & gdvOcupado2.DataKeys(e.Row.RowIndex).Item(1).ToString & "' ,aspnetForm.ctl00_conPlaHolTurismoSocial_hddAptoOcupado.value = " & gdvOcupado2.DataKeys(e.Row.RowIndex).Item(0).ToString & ",aspnetForm.ctl00_conPlaHolTurismoSocial_hddApaId.value = " & gdvOcupado2.DataKeys(e.Row.RowIndex).Item(0).ToString & ",aspnetForm.ctl00_conPlaHolTurismoSocial_hddApaDesc.value = '" & gdvOcupado2.DataKeys(e.Row.RowIndex).Item(1).ToString & "',aspnetForm.ctl00_conPlaHolTurismoSocial_hddApaCCusto.value = '" & gdvOcupado2.DataKeys(e.Row.RowIndex).Item(7).ToString & "',aspnetForm.ctl00_conPlaHolTurismoSocial_hddApaArea.value = '" & gdvOcupado2.DataKeys(e.Row.RowIndex).Item(8).ToString & "' ")
        End If
    End Sub

    Protected Sub gdvOcupado3_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gdvOcupado3.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            If (gdvOcupado3.DataKeys(e.Row.RowIndex).Item(3).ToString = "S") And _
               (gdvOcupado3.DataKeys(e.Row.RowIndex).Item(5).ToString = "EG") Then
                CType(e.Row.FindControl("imgEmprestimos"), Image).Visible = True
                CType(e.Row.FindControl("imgEmprestimos"), Image).ToolTip = gdvOcupado3.DataKeys(e.Row.RowIndex).Item(6).ToString.ToUpper
            Else
                CType(e.Row.FindControl("imgEmprestimos"), Image).Visible = False
            End If

            'Se HosDataFimSol for = ao dia de hoje, transforma a cor do label em laranja'
            If Format(CDate(gdvOcupado3.DataKeys(e.Row.RowIndex).Item(2).ToString), "yyyy-MM-dd hh:mm:ss") <= Format(CDate(Date.Now), "yyyy-MM-dd 23:59:59") Then
                CType(e.Row.FindControl("lblOcupado3"), Label).ForeColor = Drawing.Color.Orange
                CType(e.Row.FindControl("lblOcupado3"), Label).Font.Bold = True
            End If
            CType(e.Row.FindControl("lblOcupado3"), Label).Text = gdvOcupado3.DataKeys(e.Row.RowIndex).Item(1).ToString
            CType(e.Row.FindControl("lblOcupado3"), Label).Attributes.Add("onClick", "javascript:aspnetForm.ctl00_conPlaHolTurismoSocial_txtApto.value = '" & gdvOcupado3.DataKeys(e.Row.RowIndex).Item(1).ToString & "' ,aspnetForm.ctl00_conPlaHolTurismoSocial_hddAptoOcupado.value = " & gdvOcupado3.DataKeys(e.Row.RowIndex).Item(0).ToString & ",aspnetForm.ctl00_conPlaHolTurismoSocial_hddApaId.value = " & gdvOcupado3.DataKeys(e.Row.RowIndex).Item(0).ToString & ",aspnetForm.ctl00_conPlaHolTurismoSocial_hddApaDesc.value = '" & gdvOcupado3.DataKeys(e.Row.RowIndex).Item(1).ToString & "',aspnetForm.ctl00_conPlaHolTurismoSocial_hddApaCCusto.value = '" & gdvOcupado3.DataKeys(e.Row.RowIndex).Item(7).ToString & "',aspnetForm.ctl00_conPlaHolTurismoSocial_hddApaArea.value = '" & gdvOcupado3.DataKeys(e.Row.RowIndex).Item(8).ToString & "' ")
        End If
    End Sub
    Protected Sub gdvOcupado4_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gdvOcupado4.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            If (gdvOcupado4.DataKeys(e.Row.RowIndex).Item(3).ToString = "S") And _
               (gdvOcupado4.DataKeys(e.Row.RowIndex).Item(5).ToString = "EG") Then
                CType(e.Row.FindControl("imgEmprestimos"), Image).Visible = True
                CType(e.Row.FindControl("imgEmprestimos"), Image).ToolTip = gdvOcupado4.DataKeys(e.Row.RowIndex).Item(6).ToString.ToUpper
            Else
                CType(e.Row.FindControl("imgEmprestimos"), Image).Visible = False
            End If
            'Se HosDataFimSol for = ao dia de hoje, transforma a cor do label em laranja'
            If Format(CDate(gdvOcupado4.DataKeys(e.Row.RowIndex).Item(2).ToString), "yyyy-MM-dd hh:mm:ss") <= Format(CDate(Date.Now), "yyyy-MM-dd 23:59:59") Then
                CType(e.Row.FindControl("lblOcupado4"), Label).ForeColor = Drawing.Color.Orange
                CType(e.Row.FindControl("lblOcupado4"), Label).Font.Bold = True
            End If
            CType(e.Row.FindControl("lblOcupado4"), Label).Text = gdvOcupado4.DataKeys(e.Row.RowIndex).Item(1).ToString
            CType(e.Row.FindControl("lblOcupado4"), Label).Attributes.Add("onClick", "javascript:aspnetForm.ctl00_conPlaHolTurismoSocial_txtApto.value = '" & gdvOcupado4.DataKeys(e.Row.RowIndex).Item(1).ToString & "' ,aspnetForm.ctl00_conPlaHolTurismoSocial_hddAptoOcupado.value = " & gdvOcupado4.DataKeys(e.Row.RowIndex).Item(0).ToString & ",aspnetForm.ctl00_conPlaHolTurismoSocial_hddApaId.value = " & gdvOcupado4.DataKeys(e.Row.RowIndex).Item(0).ToString & ",aspnetForm.ctl00_conPlaHolTurismoSocial_hddApaDesc.value = '" & gdvOcupado4.DataKeys(e.Row.RowIndex).Item(1).ToString & "',aspnetForm.ctl00_conPlaHolTurismoSocial_hddApaCCusto.value = '" & gdvOcupado4.DataKeys(e.Row.RowIndex).Item(7).ToString & "',aspnetForm.ctl00_conPlaHolTurismoSocial_hddApaArea.value = '" & gdvOcupado4.DataKeys(e.Row.RowIndex).Item(8).ToString & "' ")
        End If
    End Sub

    Protected Sub gdvArrumacao1_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gdvArrumacao1.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            'VERIFICARA SE EXISTE UM APTO QUE SAIU DA MANUTENÇÃO, SE POSSIVITO IRÁ DEIXAR A FONTE EM PRETO + NEGRITO'
            If gdvArrumacao1.DataKeys(e.Row.RowIndex).Item(2).ToString = "S" Then
                CType(e.Row.FindControl("lblArrumacao1"), Label).ForeColor = Drawing.Color.Black
                CType(e.Row.FindControl("lblArrumacao1"), Label).Font.Bold = True
            End If
            'CType(e.Row.FindControl("lnkArrumacao1"), LinkButton).Attributes.Add("onclick", "popLayer('<b>Apartamento: " & gdvArrumacao1.DataKeys(e.Row.RowIndex).Item(1).ToString & "</b><br><br> <a href=http://intranetCaldas>Limpo</a>&nbsp<img src=" & "images/StatusAptoBranco.gif" & ">&nbsp<br><a href=Revisão>Atendimento</a>&nbsp<img src=" & "images/Cruz.gif" & ">&nbsp<br><a href=Revisão>Manutenção</a>&nbsp<img src=" & "images/StatusAptoCinza.gif" & "> ')")
            CType(e.Row.FindControl("lblArrumacao1"), Label).Text = gdvArrumacao1.DataKeys(e.Row.RowIndex).Item(1).ToString
            CType(e.Row.FindControl("lblArrumacao1"), Label).Attributes.Add("onClick", "javascript:aspnetForm.ctl00_conPlaHolTurismoSocial_txtAptoArrumacao.value = '" & gdvArrumacao1.DataKeys(e.Row.RowIndex).Item(1).ToString & "' ,aspnetForm.ctl00_conPlaHolTurismoSocial_hddAptoArrumacao.value = " & gdvArrumacao1.DataKeys(e.Row.RowIndex).Item(0).ToString & ",aspnetForm.ctl00_conPlaHolTurismoSocial_hddApaId.value = " & gdvArrumacao1.DataKeys(e.Row.RowIndex).Item(0).ToString & ",aspnetForm.ctl00_conPlaHolTurismoSocial_hddApaDesc.value = '" & gdvArrumacao1.DataKeys(e.Row.RowIndex).Item(1).ToString & "',aspnetForm.ctl00_conPlaHolTurismoSocial_hddApaCCusto.value = '" & gdvArrumacao1.DataKeys(e.Row.RowIndex).Item(4).ToString & "',aspnetForm.ctl00_conPlaHolTurismoSocial_hddApaArea.value = '" & gdvArrumacao1.DataKeys(e.Row.RowIndex).Item(5).ToString & "' ")
            'Utilizado na limpeza dos apartamentos
            CType(e.Row.FindControl("chkLimpo1"), CheckBox).Attributes.Add("ApaId", gdvArrumacao1.DataKeys(e.Row.RowIndex).Item(0).ToString)
            CType(e.Row.FindControl("chkLimpo1"), CheckBox).Attributes.Add("AgoId", gdvArrumacao1.DataKeys(e.Row.RowIndex).Item(3).ToString)
        End If
    End Sub

    Protected Sub gdvArrumacao2_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gdvArrumacao2.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            If gdvArrumacao2.DataKeys(e.Row.RowIndex).Item(2).ToString = "S" Then
                CType(e.Row.FindControl("lblArrumacao2"), Label).ForeColor = Drawing.Color.Black
                CType(e.Row.FindControl("lblArrumacao2"), Label).Font.Bold = True
            End If
            CType(e.Row.FindControl("lblArrumacao2"), Label).Text = gdvArrumacao2.DataKeys(e.Row.RowIndex).Item(1).ToString
            CType(e.Row.FindControl("lblArrumacao2"), Label).Attributes.Add("onClick", "javascript:aspnetForm.ctl00_conPlaHolTurismoSocial_txtAptoArrumacao.value = '" & gdvArrumacao2.DataKeys(e.Row.RowIndex).Item(1).ToString & "' ,aspnetForm.ctl00_conPlaHolTurismoSocial_hddAptoArrumacao.value = " & gdvArrumacao2.DataKeys(e.Row.RowIndex).Item(0).ToString & ",aspnetForm.ctl00_conPlaHolTurismoSocial_hddApaId.value = " & gdvArrumacao2.DataKeys(e.Row.RowIndex).Item(0).ToString & ",aspnetForm.ctl00_conPlaHolTurismoSocial_hddApaDesc.value = '" & gdvArrumacao2.DataKeys(e.Row.RowIndex).Item(1).ToString & "',aspnetForm.ctl00_conPlaHolTurismoSocial_hddApaCCusto.value = '" & gdvArrumacao2.DataKeys(e.Row.RowIndex).Item(4).ToString & "',aspnetForm.ctl00_conPlaHolTurismoSocial_hddApaArea.value = '" & gdvArrumacao2.DataKeys(e.Row.RowIndex).Item(5).ToString & "' ")
            'CType(e.Row.FindControl("lnkArrumacao2"), LinkButton).Attributes.Add("onclick", "popLayer('<b>Apartamento: " & gdvArrumacao2.DataKeys(e.Row.RowIndex).Item(1).ToString & "</b><br><br> <a href=http://intranetCaldas>Limpo</a>&nbsp<img src=" & "images/StatusAptoBranco.gif" & ">&nbsp<br><a href=Revisão>Atendimento</a>&nbsp<img src=" & "images/Cruz.gif" & ">&nbsp<br><a href=Revisão>Manutenção</a>&nbsp<img src=" & "images/StatusAptoCinza.gif" & "> ')")
            'Utilizado na limpeza dos apartamentos
            CType(e.Row.FindControl("chkLimpo2"), CheckBox).Attributes.Add("ApaId", gdvArrumacao2.DataKeys(e.Row.RowIndex).Item(0).ToString)
            CType(e.Row.FindControl("chkLimpo2"), CheckBox).Attributes.Add("AgoId", gdvArrumacao2.DataKeys(e.Row.RowIndex).Item(3).ToString)
        End If
    End Sub

    Protected Sub gdvArrumacao3_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gdvArrumacao3.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            If gdvArrumacao3.DataKeys(e.Row.RowIndex).Item(2).ToString = "S" Then
                CType(e.Row.FindControl("lblArrumacao3"), Label).ForeColor = Drawing.Color.Black
                CType(e.Row.FindControl("lblArrumacao3"), Label).Font.Bold = True
            End If
            CType(e.Row.FindControl("lblArrumacao3"), Label).Text = gdvArrumacao3.DataKeys(e.Row.RowIndex).Item(1).ToString
            CType(e.Row.FindControl("lblArrumacao3"), Label).Attributes.Add("onClick", "javascript:aspnetForm.ctl00_conPlaHolTurismoSocial_txtAptoArrumacao.value = '" & gdvArrumacao3.DataKeys(e.Row.RowIndex).Item(1).ToString & "' ,aspnetForm.ctl00_conPlaHolTurismoSocial_hddAptoArrumacao.value = " & gdvArrumacao3.DataKeys(e.Row.RowIndex).Item(0).ToString & ",aspnetForm.ctl00_conPlaHolTurismoSocial_hddApaId.value = " & gdvArrumacao3.DataKeys(e.Row.RowIndex).Item(0).ToString & ",aspnetForm.ctl00_conPlaHolTurismoSocial_hddApaDesc.value = '" & gdvArrumacao3.DataKeys(e.Row.RowIndex).Item(1).ToString & "',aspnetForm.ctl00_conPlaHolTurismoSocial_hddApaCCusto.value = '" & gdvArrumacao3.DataKeys(e.Row.RowIndex).Item(4).ToString & "',aspnetForm.ctl00_conPlaHolTurismoSocial_hddApaArea.value = '" & gdvArrumacao3.DataKeys(e.Row.RowIndex).Item(5).ToString & "' ")
            'CType(e.Row.FindControl("lnkArrumacao3"), LinkButton).Attributes.Add("onclick", "popLayer('<b>Apartamento: " & gdvArrumacao3.DataKeys(e.Row.RowIndex).Item(1).ToString & "</b><br><br> <a href=http://intranetCaldas>Limpo</a>&nbsp<img src=" & "images/StatusAptoBranco.gif" & ">&nbsp<br><a href=Revisão>Atendimento</a>&nbsp<img src=" & "images/Cruz.gif" & ">&nbsp<br><a href=Revisão>Manutenção</a>&nbsp<img src=" & "images/StatusAptoCinza.gif" & "> ')")
            'Utilizado na limpeza dos apartamentos
            CType(e.Row.FindControl("chkLimpo3"), CheckBox).Attributes.Add("ApaId", gdvArrumacao3.DataKeys(e.Row.RowIndex).Item(0).ToString)
            CType(e.Row.FindControl("chkLimpo3"), CheckBox).Attributes.Add("AgoId", gdvArrumacao3.DataKeys(e.Row.RowIndex).Item(3).ToString)
        End If
    End Sub

    Protected Sub gdvArrumacao4_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gdvArrumacao4.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            If gdvArrumacao4.DataKeys(e.Row.RowIndex).Item(2).ToString = "S" Then
                CType(e.Row.FindControl("lblArrumacao4"), Label).ForeColor = Drawing.Color.Black
                CType(e.Row.FindControl("lblArrumacao4"), Label).Font.Bold = True
            End If
            'CType(e.Row.FindControl("lnkArrumacao4"), LinkButton).Attributes.Add("onclick", "popLayer('<b>Apartamento: " & gdvArrumacao4.DataKeys(e.Row.RowIndex).Item(1).ToString & "</b><br><br> <a href=http://intranetCaldas>Limpo</a>&nbsp<img src=" & "images/StatusAptoBranco.gif" & ">&nbsp<br><a href=Revisão>Atendimento</a>&nbsp<img src=" & "images/Cruz.gif" & ">&nbsp<br><a href=Revisão>Manutenção</a>&nbsp<img src=" & "images/StatusAptoCinza.gif" & "> ')")
            CType(e.Row.FindControl("lblArrumacao4"), Label).Text = gdvArrumacao4.DataKeys(e.Row.RowIndex).Item(1).ToString
            CType(e.Row.FindControl("lblArrumacao4"), Label).Attributes.Add("onClick", "javascript:aspnetForm.ctl00_conPlaHolTurismoSocial_txtAptoArrumacao.value = '" & gdvArrumacao4.DataKeys(e.Row.RowIndex).Item(1).ToString & "' ,aspnetForm.ctl00_conPlaHolTurismoSocial_hddAptoArrumacao.value = " & gdvArrumacao4.DataKeys(e.Row.RowIndex).Item(0).ToString & ",aspnetForm.ctl00_conPlaHolTurismoSocial_hddApaId.value = " & gdvArrumacao4.DataKeys(e.Row.RowIndex).Item(0).ToString & ",aspnetForm.ctl00_conPlaHolTurismoSocial_hddApaDesc.value = '" & gdvArrumacao4.DataKeys(e.Row.RowIndex).Item(1).ToString & "',aspnetForm.ctl00_conPlaHolTurismoSocial_hddApaCCusto.value = '" & gdvArrumacao4.DataKeys(e.Row.RowIndex).Item(4).ToString & "',aspnetForm.ctl00_conPlaHolTurismoSocial_hddApaArea.value = '" & gdvArrumacao4.DataKeys(e.Row.RowIndex).Item(5).ToString & "' ")
            'Utilizado na limpeza dos apartamentos
            CType(e.Row.FindControl("chkLimpo4"), CheckBox).Attributes.Add("ApaId", gdvArrumacao4.DataKeys(e.Row.RowIndex).Item(0).ToString)
            CType(e.Row.FindControl("chkLimpo4"), CheckBox).Attributes.Add("AgoId", gdvArrumacao4.DataKeys(e.Row.RowIndex).Item(3).ToString)
        End If
    End Sub

    Protected Sub gdvPrioridade1_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gdvPrioridade1.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            'CType(e.Row.FindControl("lblPrioridade1"), LinkButton).Attributes.Add("onclick", "popLayer('<b>Apartamento: " & gdvPrioridade1.DataKeys(e.Row.RowIndex).Item(1).ToString & "</b><br><br> <a href=http://intranetCaldas>Atendimento</a>&nbsp<img src=" & "images/Cruz.gif" & "> ')")
            CType(e.Row.FindControl("lblPrioridade1"), Label).Text = gdvPrioridade1.DataKeys(e.Row.RowIndex).Item(1).ToString
            Dim Descricao As String = ""
            'PrioPrio = 0 quer dizer que tem prioridade sobre a prioridade, se entrar dentro de um case, já perderá essa prioridade
            Dim PriorPrior As Long = 0
            'Calculando exibição de informação de reposição no balão
            Select Case gdvPrioridade1.DataKeys(e.Row.RowIndex).Item(3).ToString
                Case 1
                    Descricao = gdvPrioridade1.DataKeys(e.Row.RowIndex).Item(3).ToString & " Cama de Casal\n"
                    PriorPrior = 1
                Case Is > 1
                    Descricao = gdvPrioridade1.DataKeys(e.Row.RowIndex).Item(3).ToString & " Camas de Casal\n"
                    PriorPrior = 1
            End Select
            Select Case gdvPrioridade1.DataKeys(e.Row.RowIndex).Item(4).ToString
                Case 1
                    Descricao = Descricao & gdvPrioridade1.DataKeys(e.Row.RowIndex).Item(4).ToString & " Cama Solteiro\n"
                    PriorPrior = 1
                Case Is > 1
                    Descricao = Descricao & gdvPrioridade1.DataKeys(e.Row.RowIndex).Item(4).ToString & " Camas de Solteiro\n"
                    PriorPrior = 1
            End Select
            Select Case gdvPrioridade1.DataKeys(e.Row.RowIndex).Item(5).ToString
                Case 1
                    Descricao = Descricao & gdvPrioridade1.DataKeys(e.Row.RowIndex).Item(5).ToString & " Cama Especial\n"
                    PriorPrior = 1
                Case Is > 1
                    Descricao = Descricao & gdvPrioridade1.DataKeys(e.Row.RowIndex).Item(5).ToString & " Camas Especiais\n"
                    PriorPrior = 1
            End Select
            Select Case gdvPrioridade1.DataKeys(e.Row.RowIndex).Item(6).ToString
                Case 1
                    Descricao = Descricao & gdvPrioridade1.DataKeys(e.Row.RowIndex).Item(6).ToString & " Cama Extra\n"
                    PriorPrior = 1
                Case Is > 1
                    Descricao = Descricao & gdvPrioridade1.DataKeys(e.Row.RowIndex).Item(6).ToString & " Camas Extras\n"
                    PriorPrior = 1
            End Select
            Select Case gdvPrioridade1.DataKeys(e.Row.RowIndex).Item(7).ToString
                Case 1
                    Descricao = Descricao & gdvPrioridade1.DataKeys(e.Row.RowIndex).Item(7).ToString & " Berço Especial\n"
                    PriorPrior = 1
                Case Is > 1
                    Descricao = Descricao & gdvPrioridade1.DataKeys(e.Row.RowIndex).Item(7).ToString & " Berços Especiais\n"
                    PriorPrior = 1
            End Select
            Select Case gdvPrioridade1.DataKeys(e.Row.RowIndex).Item(8).ToString
                Case 1
                    Descricao = Descricao & gdvPrioridade1.DataKeys(e.Row.RowIndex).Item(8).ToString & " Berço\n"
                    PriorPrior = 1
                Case Is > 1
                    Descricao = Descricao & gdvPrioridade1.DataKeys(e.Row.RowIndex).Item(8).ToString & " Berços\n"
                    PriorPrior = 1
            End Select
            Select Case gdvPrioridade1.DataKeys(e.Row.RowIndex).Item(9).ToString
                Case 1
                    Descricao = Descricao & gdvPrioridade1.DataKeys(e.Row.RowIndex).Item(9).ToString & " Fronha\n"
                    PriorPrior = 1
                Case Is > 1
                    Descricao = Descricao & gdvPrioridade1.DataKeys(e.Row.RowIndex).Item(9).ToString & " Fronhas\n"
                    PriorPrior = 1
            End Select
            Select Case gdvPrioridade1.DataKeys(e.Row.RowIndex).Item(10).ToString
                Case 1
                    Descricao = Descricao & gdvPrioridade1.DataKeys(e.Row.RowIndex).Item(10).ToString & " Jogo de Toalha\n"
                    PriorPrior = 1
                Case Is > 1
                    Descricao = Descricao & gdvPrioridade1.DataKeys(e.Row.RowIndex).Item(10).ToString & " Jogos de Toalhas\n"
                    PriorPrior = 1
            End Select
            'Se não existir nada pra repor, quer dizer que o apto esta ocupado e pode have check out .. prioridade sobre prioridade
            If PriorPrior = 0 Then
                CType(e.Row.FindControl("lblPrioridade1"), Label).ForeColor = Drawing.Color.Red
                CType(e.Row.FindControl("lblPrioridade1"), Label).Font.Bold = True
                CType(e.Row.FindControl("lblPrioridade1"), Label).Attributes.Add("onClick", "javascript:aspnetForm.ctl00_conPlaHolTurismoSocial_txtAptoPrioridade.value = '" & gdvPrioridade1.DataKeys(e.Row.RowIndex).Item(1).ToString & " " & gdvPrioridade1.DataKeys(e.Row.RowIndex).Item(2).ToString & "h' ,aspnetForm.ctl00_conPlaHolTurismoSocial_hddAptoPrioridade.value = " & gdvPrioridade1.DataKeys(e.Row.RowIndex).Item(0).ToString & ",aspnetForm.ctl00_conPlaHolTurismoSocial_hddApaId.value = " & gdvPrioridade1.DataKeys(e.Row.RowIndex).Item(0).ToString & ",aspnetForm.ctl00_conPlaHolTurismoSocial_hddApaDesc.value = '" & gdvPrioridade1.DataKeys(e.Row.RowIndex).Item(1).ToString & "',aspnetForm.ctl00_conPlaHolTurismoSocial_txtReposicaoPrior.value = 'Check out e provável check in'  ")
            Else
                CType(e.Row.FindControl("lblPrioridade1"), Label).Attributes.Add("onClick", "javascript:aspnetForm.ctl00_conPlaHolTurismoSocial_txtAptoPrioridade.value = '" & gdvPrioridade1.DataKeys(e.Row.RowIndex).Item(1).ToString & " " & gdvPrioridade1.DataKeys(e.Row.RowIndex).Item(2).ToString & "h' ,aspnetForm.ctl00_conPlaHolTurismoSocial_hddAptoPrioridade.value = " & gdvPrioridade1.DataKeys(e.Row.RowIndex).Item(0).ToString & ",aspnetForm.ctl00_conPlaHolTurismoSocial_hddApaId.value = " & gdvPrioridade1.DataKeys(e.Row.RowIndex).Item(0).ToString & ",aspnetForm.ctl00_conPlaHolTurismoSocial_hddApaDesc.value = '" & gdvPrioridade1.DataKeys(e.Row.RowIndex).Item(1).ToString & "',aspnetForm.ctl00_conPlaHolTurismoSocial_txtReposicaoPrior.value = '" & Descricao & "'  ")
            End If
            CType(e.Row.FindControl("lblPrioridade1"), Label).Attributes.Add("CentroCusto", gdvPrioridade1.DataKeys(e.Row.RowIndex).Item(10).ToString)
        End If
    End Sub

    Protected Sub gdvPrioridade2_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gdvPrioridade2.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            'CType(e.Row.FindControl("lnkPrioridade2"), LinkButton).Attributes.Add("onclick", "popLayer('<b>Apartamento: " & gdvPrioridade2.DataKeys(e.Row.RowIndex).Item(1).ToString & "</b><br><br> <a href=http://intranetCaldas>Atendimento</a>&nbsp<img src=" & "images/Cruz.gif" & "> ')")
            CType(e.Row.FindControl("lblPrioridade2"), Label).Text = gdvPrioridade2.DataKeys(e.Row.RowIndex).Item(1).ToString
            Dim Descricao As String = ""
            'PrioPrio = 0 quer dizer que não tem prioridade sobre a prioridade, se entrar dentro de um case, já perderá essa prioridade
            Dim PriorPrior As Long = 0
            'Calculando exibição de informação de reposição no balão
            Select Case gdvPrioridade2.DataKeys(e.Row.RowIndex).Item(3).ToString
                Case 1
                    Descricao = gdvPrioridade2.DataKeys(e.Row.RowIndex).Item(3).ToString & " Cama de Casal\n"
                    PriorPrior = 1
                Case Is > 1
                    Descricao = gdvPrioridade2.DataKeys(e.Row.RowIndex).Item(3).ToString & " Camas de Casal\n"
                    PriorPrior = 1
            End Select
            Select Case gdvPrioridade2.DataKeys(e.Row.RowIndex).Item(4).ToString
                Case 1
                    Descricao = Descricao & gdvPrioridade2.DataKeys(e.Row.RowIndex).Item(4).ToString & " Cama Solteiro\n"
                    PriorPrior = 1
                Case Is > 1
                    Descricao = Descricao & gdvPrioridade2.DataKeys(e.Row.RowIndex).Item(4).ToString & " Camas de Solteiro\n"
                    PriorPrior = 1
            End Select
            Select Case gdvPrioridade2.DataKeys(e.Row.RowIndex).Item(5).ToString
                Case 1
                    Descricao = Descricao & gdvPrioridade2.DataKeys(e.Row.RowIndex).Item(5).ToString & " Cama Especial\n"
                    PriorPrior = 1
                Case Is > 1
                    Descricao = Descricao & gdvPrioridade2.DataKeys(e.Row.RowIndex).Item(5).ToString & " Camas Especiais\n"
                    PriorPrior = 1
            End Select
            Select Case gdvPrioridade2.DataKeys(e.Row.RowIndex).Item(6).ToString
                Case 1
                    Descricao = Descricao & gdvPrioridade2.DataKeys(e.Row.RowIndex).Item(6).ToString & " Cama Extra\n"
                    PriorPrior = 1
                Case Is > 1
                    Descricao = Descricao & gdvPrioridade2.DataKeys(e.Row.RowIndex).Item(6).ToString & " Camas Extras\n"
                    PriorPrior = 1
            End Select
            Select Case gdvPrioridade2.DataKeys(e.Row.RowIndex).Item(7).ToString
                Case 1
                    Descricao = Descricao & gdvPrioridade2.DataKeys(e.Row.RowIndex).Item(7).ToString & " Berço Especial\n"
                    PriorPrior = 1
                Case Is > 1
                    Descricao = Descricao & gdvPrioridade2.DataKeys(e.Row.RowIndex).Item(7).ToString & " Berços Especiais\n"
                    PriorPrior = 1
            End Select
            Select Case gdvPrioridade2.DataKeys(e.Row.RowIndex).Item(8).ToString
                Case 1
                    Descricao = Descricao & gdvPrioridade2.DataKeys(e.Row.RowIndex).Item(8).ToString & " Berço\n"
                    PriorPrior = 1
                Case Is > 1
                    Descricao = Descricao & gdvPrioridade2.DataKeys(e.Row.RowIndex).Item(8).ToString & " Berços\n"
                    PriorPrior = 1
            End Select
            Select Case gdvPrioridade2.DataKeys(e.Row.RowIndex).Item(9).ToString
                Case 1
                    Descricao = Descricao & gdvPrioridade2.DataKeys(e.Row.RowIndex).Item(9).ToString & " Fronha\n"
                    PriorPrior = 1
                Case Is > 1
                    Descricao = Descricao & gdvPrioridade2.DataKeys(e.Row.RowIndex).Item(9).ToString & " Fronhas\n"
                    PriorPrior = 1
            End Select
            Select Case gdvPrioridade2.DataKeys(e.Row.RowIndex).Item(10).ToString
                Case 1
                    Descricao = Descricao & gdvPrioridade2.DataKeys(e.Row.RowIndex).Item(10).ToString & " Jogo de Toalha\n"
                    PriorPrior = 1
                Case Is > 1
                    Descricao = Descricao & gdvPrioridade2.DataKeys(e.Row.RowIndex).Item(10).ToString & " Jogos de Toalhas\n"
                    PriorPrior = 1
            End Select
            'Se não existir nada pra repor, quer dizer que o apto esta ocupado e pode have check out .. prioridade sobre prioridade
            If PriorPrior = 0 Then
                CType(e.Row.FindControl("lblPrioridade2"), Label).ForeColor = Drawing.Color.Red
                CType(e.Row.FindControl("lblPrioridade2"), Label).Font.Bold = True
                CType(e.Row.FindControl("lblPrioridade2"), Label).Attributes.Add("onClick", "javascript:aspnetForm.ctl00_conPlaHolTurismoSocial_txtAptoPrioridade.value = '" & gdvPrioridade2.DataKeys(e.Row.RowIndex).Item(1).ToString & " " & gdvPrioridade2.DataKeys(e.Row.RowIndex).Item(2).ToString & "h' ,aspnetForm.ctl00_conPlaHolTurismoSocial_hddAptoPrioridade.value = " & gdvPrioridade2.DataKeys(e.Row.RowIndex).Item(0).ToString & ",aspnetForm.ctl00_conPlaHolTurismoSocial_hddApaId.value = " & gdvPrioridade2.DataKeys(e.Row.RowIndex).Item(0).ToString & ",aspnetForm.ctl00_conPlaHolTurismoSocial_hddApaDesc.value = '" & gdvPrioridade2.DataKeys(e.Row.RowIndex).Item(1).ToString & "',aspnetForm.ctl00_conPlaHolTurismoSocial_txtReposicaoPrior.value = 'Check Out e provável check in'  ")
            Else
                CType(e.Row.FindControl("lblPrioridade2"), Label).Attributes.Add("onClick", "javascript:aspnetForm.ctl00_conPlaHolTurismoSocial_txtAptoPrioridade.value = '" & gdvPrioridade2.DataKeys(e.Row.RowIndex).Item(1).ToString & " " & gdvPrioridade2.DataKeys(e.Row.RowIndex).Item(2).ToString & "h' ,aspnetForm.ctl00_conPlaHolTurismoSocial_hddAptoPrioridade.value = " & gdvPrioridade2.DataKeys(e.Row.RowIndex).Item(0).ToString & ",aspnetForm.ctl00_conPlaHolTurismoSocial_hddApaId.value = " & gdvPrioridade2.DataKeys(e.Row.RowIndex).Item(0).ToString & ",aspnetForm.ctl00_conPlaHolTurismoSocial_hddApaDesc.value = '" & gdvPrioridade2.DataKeys(e.Row.RowIndex).Item(1).ToString & "',aspnetForm.ctl00_conPlaHolTurismoSocial_txtReposicaoPrior.value = '" & Descricao & "'  ")
            End If
        End If
    End Sub

    Protected Sub gdvPrioridade3_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gdvPrioridade3.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            'CType(e.Row.FindControl("lnkPrioridade3"), LinkButton).Attributes.Add("onclick", "popLayer('<b>Apartamento: " & gdvPrioridade3.DataKeys(e.Row.RowIndex).Item(1).ToString & "</b><br><br> <a href=http://intranetCaldas>Atendimento</a>&nbsp<img src=" & "images/Cruz.gif" & "> ')")
            CType(e.Row.FindControl("lblPrioridade3"), Label).Text = gdvPrioridade3.DataKeys(e.Row.RowIndex).Item(1).ToString
            Dim Descricao As String = ""
            'PrioPrio = 0 quer dizer que não tem prioridade sobre a prioridade, se entrar dentro de um case, já perderá essa prioridade
            Dim PriorPrior As Long = 0
            'Calculando exibição de informação de reposição no balão
            Select Case gdvPrioridade3.DataKeys(e.Row.RowIndex).Item(3).ToString
                Case 1
                    Descricao = gdvPrioridade3.DataKeys(e.Row.RowIndex).Item(3).ToString & " Cama de Casal\n"
                    PriorPrior = 1
                Case Is > 1
                    Descricao = gdvPrioridade3.DataKeys(e.Row.RowIndex).Item(3).ToString & " Camas de Casal\n"
                    PriorPrior = 1
            End Select
            Select Case gdvPrioridade3.DataKeys(e.Row.RowIndex).Item(4).ToString
                Case 1
                    Descricao = Descricao & gdvPrioridade3.DataKeys(e.Row.RowIndex).Item(4).ToString & " Cama Solteiro\n"
                    PriorPrior = 1
                Case Is > 1
                    Descricao = Descricao & gdvPrioridade3.DataKeys(e.Row.RowIndex).Item(4).ToString & " Camas de Solteiro\n"
                    PriorPrior = 1
            End Select
            Select Case gdvPrioridade3.DataKeys(e.Row.RowIndex).Item(5).ToString
                Case 1
                    Descricao = Descricao & gdvPrioridade3.DataKeys(e.Row.RowIndex).Item(5).ToString & " Cama Especial\n"
                    PriorPrior = 1
                Case Is > 1
                    Descricao = Descricao & gdvPrioridade3.DataKeys(e.Row.RowIndex).Item(5).ToString & " Camas Especiais\n"
                    PriorPrior = 1
            End Select
            Select Case gdvPrioridade3.DataKeys(e.Row.RowIndex).Item(6).ToString
                Case 1
                    Descricao = Descricao & gdvPrioridade3.DataKeys(e.Row.RowIndex).Item(6).ToString & " Cama Extra\n"
                    PriorPrior = 1
                Case Is > 1
                    Descricao = Descricao & gdvPrioridade3.DataKeys(e.Row.RowIndex).Item(6).ToString & " Camas Extras\n"
                    PriorPrior = 1
            End Select
            Select Case gdvPrioridade3.DataKeys(e.Row.RowIndex).Item(7).ToString
                Case 1
                    Descricao = Descricao & gdvPrioridade3.DataKeys(e.Row.RowIndex).Item(7).ToString & " Berço Especial\n"
                    PriorPrior = 1
                Case Is > 1
                    Descricao = Descricao & gdvPrioridade3.DataKeys(e.Row.RowIndex).Item(7).ToString & " Berços Especiais\n"
                    PriorPrior = 1
            End Select
            Select Case gdvPrioridade3.DataKeys(e.Row.RowIndex).Item(8).ToString
                Case 1
                    Descricao = Descricao & gdvPrioridade3.DataKeys(e.Row.RowIndex).Item(8).ToString & " Berço\n"
                    PriorPrior = 1
                Case Is > 1
                    Descricao = Descricao & gdvPrioridade3.DataKeys(e.Row.RowIndex).Item(8).ToString & " Berços\n"
                    PriorPrior = 1
            End Select
            Select Case gdvPrioridade3.DataKeys(e.Row.RowIndex).Item(9).ToString
                Case 1
                    Descricao = Descricao & gdvPrioridade3.DataKeys(e.Row.RowIndex).Item(9).ToString & " Fronha\n"
                    PriorPrior = 1
                Case Is > 1
                    Descricao = Descricao & gdvPrioridade3.DataKeys(e.Row.RowIndex).Item(9).ToString & " Fronhas\n"
                    PriorPrior = 1
            End Select
            Select Case gdvPrioridade3.DataKeys(e.Row.RowIndex).Item(10).ToString
                Case 1
                    Descricao = Descricao & gdvPrioridade3.DataKeys(e.Row.RowIndex).Item(10).ToString & " Jogo de Toalha\n"
                    PriorPrior = 1
                Case Is > 1
                    Descricao = Descricao & gdvPrioridade3.DataKeys(e.Row.RowIndex).Item(10).ToString & " Jogos de Toalhas\n"
                    PriorPrior = 1
            End Select
            'Se não existir nada pra repor, quer dizer que o apto esta ocupado e pode have check out .. prioridade sobre prioridade
            If PriorPrior = 0 Then
                CType(e.Row.FindControl("lblPrioridade3"), Label).ForeColor = Drawing.Color.Red
                CType(e.Row.FindControl("lblPrioridade3"), Label).Font.Bold = True
                CType(e.Row.FindControl("lblPrioridade3"), Label).Attributes.Add("onClick", "javascript:aspnetForm.ctl00_conPlaHolTurismoSocial_txtAptoPrioridade.value = '" & gdvPrioridade3.DataKeys(e.Row.RowIndex).Item(1).ToString & " " & gdvPrioridade3.DataKeys(e.Row.RowIndex).Item(2).ToString & "h' ,aspnetForm.ctl00_conPlaHolTurismoSocial_hddAptoPrioridade.value = " & gdvPrioridade3.DataKeys(e.Row.RowIndex).Item(0).ToString & ",aspnetForm.ctl00_conPlaHolTurismoSocial_hddApaId.value = " & gdvPrioridade3.DataKeys(e.Row.RowIndex).Item(0).ToString & ",aspnetForm.ctl00_conPlaHolTurismoSocial_hddApaDesc.value = '" & gdvPrioridade3.DataKeys(e.Row.RowIndex).Item(1).ToString & "',aspnetForm.ctl00_conPlaHolTurismoSocial_txtReposicaoPrior.value = 'Check out e provável check in'  ")
            Else
                CType(e.Row.FindControl("lblPrioridade3"), Label).Attributes.Add("onClick", "javascript:aspnetForm.ctl00_conPlaHolTurismoSocial_txtAptoPrioridade.value = '" & gdvPrioridade3.DataKeys(e.Row.RowIndex).Item(1).ToString & " " & gdvPrioridade3.DataKeys(e.Row.RowIndex).Item(2).ToString & "h' ,aspnetForm.ctl00_conPlaHolTurismoSocial_hddAptoPrioridade.value = " & gdvPrioridade3.DataKeys(e.Row.RowIndex).Item(0).ToString & ",aspnetForm.ctl00_conPlaHolTurismoSocial_hddApaId.value = " & gdvPrioridade3.DataKeys(e.Row.RowIndex).Item(0).ToString & ",aspnetForm.ctl00_conPlaHolTurismoSocial_hddApaDesc.value = '" & gdvPrioridade3.DataKeys(e.Row.RowIndex).Item(1).ToString & "',aspnetForm.ctl00_conPlaHolTurismoSocial_txtReposicaoPrior.value = '" & Descricao & "'  ")
            End If
        End If
    End Sub

    Protected Sub gdvPrioridade4_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gdvPrioridade4.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            'CType(e.Row.FindControl("lnkPrioridade4"), LinkButton).Attributes.Add("onclick", "popLayer('<b>Apartamento: " & gdvPrioridade4.DataKeys(e.Row.RowIndex).Item(1).ToString & "</b><br><br> <a href=http://intranetCaldas>Atendimento</a>&nbsp<img src=" & "images/Cruz.gif" & "> ')")
            CType(e.Row.FindControl("lblPrioridade4"), Label).Text = gdvPrioridade4.DataKeys(e.Row.RowIndex).Item(1).ToString
            Dim Descricao As String = ""
            'PrioPrio = 0 quer dizer que não tem prioridade sobre a prioridade, se entrar dentro de um case, já perderá essa prioridade
            Dim PriorPrior As Long = 0
            'Calculando exibição de informação de reposição no balão
            Select Case gdvPrioridade4.DataKeys(e.Row.RowIndex).Item(3).ToString
                Case 1
                    Descricao = gdvPrioridade4.DataKeys(e.Row.RowIndex).Item(3).ToString & " Cama de Casal\n"
                    PriorPrior = 1
                Case Is > 1
                    Descricao = gdvPrioridade4.DataKeys(e.Row.RowIndex).Item(3).ToString & " Camas de Casal\n"
                    PriorPrior = 1
            End Select
            Select Case gdvPrioridade4.DataKeys(e.Row.RowIndex).Item(4).ToString
                Case 1
                    Descricao = Descricao & gdvPrioridade4.DataKeys(e.Row.RowIndex).Item(4).ToString & " Cama Solteiro\n"
                    PriorPrior = 1
                Case Is > 1
                    Descricao = Descricao & gdvPrioridade4.DataKeys(e.Row.RowIndex).Item(4).ToString & " Camas de Solteiro\n"
                    PriorPrior = 1
            End Select
            Select Case gdvPrioridade4.DataKeys(e.Row.RowIndex).Item(5).ToString
                Case 1
                    Descricao = Descricao & gdvPrioridade4.DataKeys(e.Row.RowIndex).Item(5).ToString & " Cama Especial\n"
                    PriorPrior = 1
                Case Is > 1
                    Descricao = Descricao & gdvPrioridade4.DataKeys(e.Row.RowIndex).Item(5).ToString & " Camas Especiais\n"
                    PriorPrior = 1
            End Select
            Select Case gdvPrioridade4.DataKeys(e.Row.RowIndex).Item(6).ToString
                Case 1
                    Descricao = Descricao & gdvPrioridade4.DataKeys(e.Row.RowIndex).Item(6).ToString & " Cama Extra\n"
                    PriorPrior = 1
                Case Is > 1
                    Descricao = Descricao & gdvPrioridade4.DataKeys(e.Row.RowIndex).Item(6).ToString & " Camas Extras\n"
                    PriorPrior = 1
            End Select
            Select Case gdvPrioridade4.DataKeys(e.Row.RowIndex).Item(7).ToString
                Case 1
                    Descricao = Descricao & gdvPrioridade4.DataKeys(e.Row.RowIndex).Item(7).ToString & " Berço Especial\n"
                    PriorPrior = 1
                Case Is > 1
                    Descricao = Descricao & gdvPrioridade4.DataKeys(e.Row.RowIndex).Item(7).ToString & " Berços Especiais\n"
                    PriorPrior = 1
            End Select
            Select Case gdvPrioridade4.DataKeys(e.Row.RowIndex).Item(8).ToString
                Case 1
                    Descricao = Descricao & gdvPrioridade4.DataKeys(e.Row.RowIndex).Item(8).ToString & " Berço\n"
                    PriorPrior = 1
                Case Is > 1
                    Descricao = Descricao & gdvPrioridade4.DataKeys(e.Row.RowIndex).Item(8).ToString & " Berços\n"
                    PriorPrior = 1
            End Select
            Select Case gdvPrioridade4.DataKeys(e.Row.RowIndex).Item(9).ToString
                Case 1
                    Descricao = Descricao & gdvPrioridade4.DataKeys(e.Row.RowIndex).Item(9).ToString & " Fronha\n"
                    PriorPrior = 1
                Case Is > 1
                    Descricao = Descricao & gdvPrioridade4.DataKeys(e.Row.RowIndex).Item(9).ToString & " Fronhas\n"
                    PriorPrior = 1
            End Select
            Select Case gdvPrioridade4.DataKeys(e.Row.RowIndex).Item(10).ToString
                Case 1
                    Descricao = Descricao & gdvPrioridade4.DataKeys(e.Row.RowIndex).Item(10).ToString & " Jogo de Toalha\n"
                    PriorPrior = 1
                Case Is > 1
                    Descricao = Descricao & gdvPrioridade4.DataKeys(e.Row.RowIndex).Item(10).ToString & " Jogos de Toalhas\n"
                    PriorPrior = 1
            End Select
            'Se não existir nada pra repor, quer dizer que o apto esta ocupado e pode have check out .. prioridade sobre prioridade
            If PriorPrior = 0 Then
                CType(e.Row.FindControl("lblPrioridade4"), Label).ForeColor = Drawing.Color.Red
                CType(e.Row.FindControl("lblPrioridade4"), Label).Font.Bold = True
                CType(e.Row.FindControl("lblPrioridade4"), Label).Attributes.Add("onClick", "javascript:aspnetForm.ctl00_conPlaHolTurismoSocial_txtAptoPrioridade.value = '" & gdvPrioridade4.DataKeys(e.Row.RowIndex).Item(1).ToString & " " & gdvPrioridade4.DataKeys(e.Row.RowIndex).Item(2).ToString & "h' ,aspnetForm.ctl00_conPlaHolTurismoSocial_hddAptoPrioridade.value = " & gdvPrioridade4.DataKeys(e.Row.RowIndex).Item(0).ToString & ",aspnetForm.ctl00_conPlaHolTurismoSocial_hddApaId.value = " & gdvPrioridade4.DataKeys(e.Row.RowIndex).Item(0).ToString & ",aspnetForm.ctl00_conPlaHolTurismoSocial_hddApaDesc.value = '" & gdvPrioridade4.DataKeys(e.Row.RowIndex).Item(1).ToString & "',aspnetForm.ctl00_conPlaHolTurismoSocial_txtReposicaoPrior.value = 'Check out e provável check in'  ")
            Else
                CType(e.Row.FindControl("lblPrioridade4"), Label).Attributes.Add("onClick", "javascript:aspnetForm.ctl00_conPlaHolTurismoSocial_txtAptoPrioridade.value = '" & gdvPrioridade4.DataKeys(e.Row.RowIndex).Item(1).ToString & " " & gdvPrioridade4.DataKeys(e.Row.RowIndex).Item(2).ToString & "h' ,aspnetForm.ctl00_conPlaHolTurismoSocial_hddAptoPrioridade.value = " & gdvPrioridade4.DataKeys(e.Row.RowIndex).Item(0).ToString & ",aspnetForm.ctl00_conPlaHolTurismoSocial_hddApaId.value = " & gdvPrioridade4.DataKeys(e.Row.RowIndex).Item(0).ToString & ",aspnetForm.ctl00_conPlaHolTurismoSocial_hddApaDesc.value = '" & gdvPrioridade4.DataKeys(e.Row.RowIndex).Item(1).ToString & "',aspnetForm.ctl00_conPlaHolTurismoSocial_txtReposicaoPrior.value = '" & Descricao & "'  ")
            End If
        End If
    End Sub

    Protected Sub btnConsultar_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnConsultar.Click
        pnlStatus.Visible = True
        pnlGridArrumacao.Visible = False
        pnlIntegrantes.Visible = False
        pnlHistoricoManutencao.Visible = False
        PnlAtendimento.Visible = False
        chkSelecionaTodos.Checked = False
        pnlGridsLimpoA.Visible = False
        'Preparando ambiente, caso mudar de tela volta ao status anterior'
        btnAuxilarMan.Attributes.Add("AplicarFiltro", "N")
        ''Armazena posição do filtro em uma session'
        'Dim count As Short = 0
        'Dim ListaFiltro As IList
        'ListaFiltro = New ArrayList
        'While count <= 5
        '    If chkConsultas.Items(count).Selected = True Then
        '        ListaFiltro.Add(True)
        '    Else
        '        ListaFiltro.Add(False)
        '    End If
        '    count = count + 1
        'End While
        'Session("Filtro") = ListaFiltro
        'Desativa todos os paines, que serão ativados gradativamente depois'
        pnlArrumacao.Visible = False
        PnlConferencia.Visible = False
        pnlLimpo.Visible = False
        'pnlGridLimpo.Visible = False
        pnlManutencao.Visible = False
        pnlOcupado.Visible = False
        pnlPrioridade.Visible = False
        'INCIA O PROCESSO DE CONSULTA'
        If txtDataPrevisao.Text.Length = 0 Then
            txtDataPrevisao.Text = Format(DateTime.Today, "dd/MM/yyyy")
        End If
        Dim Federacao As String = ""
        'DEFININDO FEDERACAO OU NÃO
        Select Case txtFederacao.SelectedValue
            Case "Todos" : Federacao = ""
            Case "Sim" : Federacao = "S"
            Case "Não" : Federacao = "N"
        End Select
        If chkArrumacao.Checked = True Then ' chkConsultas.Items(1).Selected = True Then
            '========ARRUMAÇÃO============
            ObjArrumacaoVO = New ArrumacaoVO
            ObjArrumacaoDAO = New ArrumacaoDAO()
            Dim ListaBaseArrumacao As IList
            ObjArrumacaoVO.ApaDesc = txtApartamento.Text.Replace("'", "")
            ListaBaseArrumacao = ObjArrumacaoDAO.PesquisaArrumacao(ObjArrumacaoVO, Federacao, btnUnidade.Attributes.Item("AliasBancoTurismo").ToString)
            Session("Arrumacao") = ListaBaseArrumacao
            'INFORMANDO O TOTAL DE APARTAMENTOS EM ARRUMAÇÃO'
            Dim TotAptoArrumacao As Integer = ListaBaseArrumacao.Count
            lblTotArrumacao.Text = CStr(TotAptoArrumacao)

            'CRIANDO AS LISTAS AUXILIARES'
            Dim ListaArrumacao1 As IList = New ArrayList
            Dim ListaArrumacao2 As IList = New ArrayList
            Dim ListaArrumacao3 As IList = New ArrayList
            Dim ListaArrumacao4 As IList = New ArrayList
            Dim Cont As Byte = 1
            'PERCORRER A LISATA BASE INSERINDO OS VALORES NAS LISTA AUXILIARES'
            'QUE IRÃO PREENCHER OS GRID'S
            For Each item As ArrumacaoVO In ListaBaseArrumacao
                Select Case Cont
                    Case 1 : ListaArrumacao1.Add(item)
                    Case 2 : ListaArrumacao2.Add(item)
                    Case 3 : ListaArrumacao3.Add(item)
                    Case 4 : ListaArrumacao4.Add(item)
                End Select
                If Cont = 4 Then
                    Cont = 1
                Else
                    Cont += 1
                End If
            Next

            gdvArrumacao1.DataSource = ListaArrumacao1
            gdvArrumacao1.DataBind()
            gdvArrumacao2.DataSource = ListaArrumacao2
            gdvArrumacao2.DataBind()
            gdvArrumacao3.DataSource = ListaArrumacao3
            gdvArrumacao3.DataBind()
            gdvArrumacao4.DataSource = ListaArrumacao4
            gdvArrumacao4.DataBind()
            'SE EXISTIR ALGO A SER EXIBIDO O PAINEL SERÁ VISUALIADO'
            If (ListaArrumacao1.Count > 0 Or ListaArrumacao2.Count > 0 Or ListaArrumacao3.Count > 0 Or ListaArrumacao4.Count > 0) Then
                pnlArrumacao.Visible = True
            End If
        End If

        If chkOcupado.Checked = True Then   'chkConsultas.Items(2).Selected = True Then
            '=========OCUPADOS'=========
            ObjOcupadoVO = New OcupadoVO
            ObjOcupadoDAO = New OcupadoDAO()
            Dim ListaBaseOcupado As IList
            ObjOcupadoVO.ApaDesc = txtApartamento.Text.Replace(",", " ")
            'ATIVANDO O VISIBLE DO PAINEL'
            'pnlPopOcupado.Visible = True
            ListaBaseOcupado = ObjOcupadoDAO.Consultar(ObjOcupadoVO, Federacao, btnUnidade.Attributes.Item("AliasBancoTurismo").ToString)
            Session("Ocupados") = ListaBaseOcupado
            'INFORMANDO O TOTAL DE APARTAMENTOS EM ARRUMAÇÃO'
            Dim TotAptoOcupado As Integer = ListaBaseOcupado.Count
            lblTotOcupado.Text = CStr(TotAptoOcupado)
            'CRIANDO AS LISTAS AUXILIARES'
            Dim ListaOcupado1 As IList = New ArrayList
            Dim ListaOcupado2 As IList = New ArrayList
            Dim ListaOcupado3 As IList = New ArrayList
            Dim ListaOcupado4 As IList = New ArrayList
            cont = 1
            'PERCORRER A LISATA BASE INSERINDO OS VALORES NAS LISTA AUXILIARES'
            'QUE IRÃO PREENCHER OS GRID'S
            For Each item As OcupadoVO In ListaBaseOcupado
                Select Case cont
                    Case 1 : ListaOcupado1.Add(item)
                    Case 2 : ListaOcupado2.Add(item)
                    Case 3 : ListaOcupado3.Add(item)
                    Case 4 : ListaOcupado4.Add(item)
                End Select
                If cont = 4 Then
                    cont = 1
                Else
                    cont += 1
                End If
            Next
            gdvOcupado1.DataSource = ListaOcupado1
            gdvOcupado1.DataBind()
            gdvOcupado2.DataSource = ListaOcupado2
            gdvOcupado2.DataBind()
            gdvOcupado3.DataSource = ListaOcupado3
            gdvOcupado3.DataBind()
            gdvOcupado4.DataSource = ListaOcupado4
            gdvOcupado4.DataBind()
            'SE EXISTIR ALGO A SER EXIBIDO O PAINEL SERÁ VISUALIADO
            If (ListaOcupado1.Count > 0 Or ListaOcupado2.Count > 0 Or ListaOcupado3.Count > 0 Or ListaOcupado4.Count > 0) Then
                pnlOcupado.Visible = True
            End If
        End If

        If chkLimpo.Checked = True Then  'chkConsultas.Items(4).Selected = True Then
            '=========LIMPO'=========
            ObjLimpoVO = New LimpoVO
            ObjLimpoDAO = New LimpoDAO()
            Dim ListaBaseLimpo As IList
            Dim HtmlHint As StringBuilder
            HtmlHint = New StringBuilder("")
            ObjLimpoVO.ApaId = txtApartamento.Text.Replace("'", "")
            ListaBaseLimpo = ObjLimpoDAO.Consultar(ObjLimpoVO, Federacao, btnUnidade.Attributes.Item("AliasBancoTurismo").ToString)
            Session("Limpo") = ListaBaseLimpo
            'INFORMANDO O TOTAL DE APARTAMENTOS EM ARRUMAÇÃO
            Dim TotAptoLimpo As Integer = ListaBaseLimpo.Count
            lblTotLimpo.Text = CStr(TotAptoLimpo)
            'CRIANDO AS LISTAS AUXILIARES'
            Dim ListaLimpo1 As IList = New ArrayList
            Dim ListaLimpo2 As IList = New ArrayList
            Dim ListaLimpo3 As IList = New ArrayList
            Dim ListaLimpo4 As IList = New ArrayList
            cont = 1
            'PERCORRER A LISATA BASE INSERINDO OS VALORES NAS LISTA AUXILIARES
            'QUE IRÃO PREENCHER OS GRID'S
            For Each item As LimpoVO In ListaBaseLimpo
                Select Case cont
                    Case 1 : ListaLimpo1.Add(item)
                    Case 2 : ListaLimpo2.Add(item)
                    Case 3 : ListaLimpo3.Add(item)
                    Case 4 : ListaLimpo4.Add(item)
                End Select
                If cont = 4 Then
                    cont = 1
                Else
                    cont += 1
                End If
            Next

            gdvLimpo1.DataSource = ListaLimpo1
            gdvLimpo1.DataBind()
            gdvLimpo2.DataSource = ListaLimpo2
            gdvLimpo2.DataBind()
            gdvLimpo3.DataSource = ListaLimpo3
            gdvLimpo3.DataBind()
            gdvLimpo4.DataSource = ListaLimpo4
            gdvLimpo4.DataBind()
            'SE EXISTIR ALGO A SER EXIBIDO O PAINEL SERÁ VISUALIADO'
            If (ListaLimpo1.Count > 0 Or ListaLimpo2.Count > 0 Or ListaLimpo3.Count > 0 Or ListaLimpo4.Count > 0) Then
                pnlLimpo.Visible = True
            End If
        End If

        If chkPrioridade.Checked = True Then  'chkConsultas.Items(0).Selected = True Then
            '=========PRIORIDADE'=========
            ObjPrioridadeVO = New PrioridadeVO
            ObjPrioridadeDAO = New PrioridadeDAO()
            Dim ListaBasePrioridade As IList
            Dim HtmlHint As StringBuilder
            HtmlHint = New StringBuilder("")
            ObjPrioridadeVO.ApaId = txtApartamento.Text.Replace("'", "")
            ListaBasePrioridade = ObjPrioridadeDAO.ConsultaPrioridade(ObjPrioridadeVO, txtDataPrevisao.Text, Federacao, btnUnidade.Attributes.Item("AliasBancoTurismo").ToString)
            Session("Prioridade") = ListaBasePrioridade
            'INFORMANDO O TOTAL DE APARTAMENTOS EM ARRUMAÇÃO
            Dim TotAptoPrioridade As Integer = ListaBasePrioridade.Count
            lblTotPrioridade.Text = CStr(TotAptoPrioridade)
            If TotAptoPrioridade > 0 Then
                lblTituloPrior.Text = "Prioridades"
            Else
                lblTituloPrior.Text = "Prioridade"
            End If
            'CRIANDO AS LISTAS AUXILIARES'
            Dim ListaPrioridade1 As IList = New ArrayList
            Dim ListaPrioridade2 As IList = New ArrayList
            Dim ListaPrioridade3 As IList = New ArrayList
            Dim ListaPrioridade4 As IList = New ArrayList
            cont = 1
            'PERCORRER A LISATA BASE INSERINDO OS VALORES NAS LISTA AUXILIARES
            'QUE IRÃO PREENCHER OS GRID'S
            For Each item As PrioridadeVO In ListaBasePrioridade
                Select Case cont
                    Case 1 : ListaPrioridade1.Add(item)
                    Case 2 : ListaPrioridade2.Add(item)
                    Case 3 : ListaPrioridade3.Add(item)
                    Case 4 : ListaPrioridade4.Add(item)
                End Select
                If cont = 4 Then
                    cont = 1
                Else
                    cont += 1
                End If
            Next

            gdvPrioridade1.DataSource = ListaPrioridade1
            gdvPrioridade1.DataBind()
            gdvPrioridade2.DataSource = ListaPrioridade2
            gdvPrioridade2.DataBind()
            gdvPrioridade3.DataSource = ListaPrioridade3
            gdvPrioridade3.DataBind()
            gdvPrioridade4.DataSource = ListaPrioridade4
            gdvPrioridade4.DataBind()
            'SE EXISTIR ALGO A SER EXIBIDO O PAINEL SERÁ VISUALIADO'
            If (ListaPrioridade1.Count > 0 Or ListaPrioridade2.Count > 0 Or ListaPrioridade3.Count > 0 Or ListaPrioridade4.Count > 0) Then
                pnlPrioridade.Visible = True
            End If
        End If

        If chkManutencao.Checked = True Then  'chkConsultas.Items(3).Selected = True Then
            '=========MANUTENÇÃO'=========
            ObjManutencaoVO = New ManutencaoVO
            ObjManutencaoDAO = New ManutencaoDAO()
            Dim ListaBaseManutencao As IList
            ObjManutencaoVO.ApaId = txtApartamento.Text.Replace("'", "")
            'ListaBaseManutencao = ObjManutencaoDAO.Consultar(ObjManutencaoVO, Federacao, btnUnidade.Attributes.Item("BancoTurismoSocial").ToString, btnUnidade.Attributes.Item("AliasBancoHdManutencao").ToString)
            ListaBaseManutencao = ObjManutencaoDAO.Consultar(ObjManutencaoVO, Federacao, btnUnidade.Attributes.Item("BancoTurismoSocial").ToString, btnUnidade.Attributes.Item("AliasBancoTurismo").ToString)
            Session("Manutencao") = ListaBaseManutencao
            'INFORMANDO O TOTAL DE APARTAMENTOS EM ARRUMAÇÃO
            Dim TotAptoManutencao As Integer = ListaBaseManutencao.Count
            lblTotManutencao.Text = CStr(TotAptoManutencao)
            'CRIANDO AS LISTAS AUXILIARES
            Dim ListaManutencao1 As IList = New ArrayList
            Dim ListaManutencao2 As IList = New ArrayList
            cont = 1
            'PERCORRER A LISATA BASE INSERINDO OS VALORES NAS LISTA AUXILIARES
            'QUE IRÃO PREENCHER OS GRID'S
            For Each item As ManutencaoVO In ListaBaseManutencao
                Select Case cont
                    Case 1 : ListaManutencao1.Add(item)
                    Case 2 : ListaManutencao2.Add(item)
                End Select
                If cont = 2 Then
                    cont = 1
                Else
                    cont += 1
                End If
            Next

            gdvManutencao1.DataSource = ListaManutencao1
            gdvManutencao1.DataBind()
            gdvManutencao2.DataSource = ListaManutencao2
            gdvManutencao2.DataBind()
            'SE EXISTIR ALGO A SER EXIBIDO O PAINEL SERÁ VISUALIADO'
            If (ListaManutencao1.Count > 0 Or ListaManutencao2.Count > 0) Then
                pnlManutencao.Visible = True
            End If
        End If

        If chkConferencia.Checked = True Then  'chkConsultas.Items(5).Selected = True Then
            '=========CONFERENCIA'=========
            ObjConferenciaVO = New ConferenciaVO
            ObjConferenciaDAO = New ConferenciaDAO()
            Dim ListaBaseConferencia As IList
            ObjConferenciaVO.ApaId = txtApartamento.Text.Replace("'", "")
            ListaBaseConferencia = ObjConferenciaDAO.Consultar(ObjConferenciaVO, Federacao, btnUnidade.Attributes.Item("AliasBancoTurismo").ToString)
            'Session("Conferencia") = ListaBaseConferencia
            'INFORMANDO O TOTAL DE APARTAMENTOS EM ARRUMAÇÃO'
            Dim TotAptoConferencia = ListaBaseConferencia.Count
            'Testar aqui nesse ponto, se a lista '
            'If (ListaBaseConferencia.Count = 1 And ListaBaseConferencia.Item(0).ToString <> "") Then
            'ListaBaseConferencia.Clear()
            'TotAptoConferencia = ListaBaseConferencia.Count
            'PnlConferencia.Visible = False
            'Else
            'End If

            lblTotConferencia.Text = CStr(TotAptoConferencia)

            'CRIANDO AS LISTAS AUXILIARES
            Dim ListaConferencia1 As IList = New ArrayList
            Dim ListaConferencia2 As IList = New ArrayList
            cont = 1
            'PERCORRER A LISATA BASE INSERINDO OS VALORES NAS LISTA AUXILIARES
            'QUE IRÃO PREENCHER OS GRID'S
            For Each item As ConferenciaVO In ListaBaseConferencia
                Select Case cont
                    Case 1 : ListaConferencia1.Add(item)
                    Case 2 : ListaConferencia2.Add(item)
                End Select
                If cont = 2 Then
                    cont = 1
                Else
                    cont += 1
                End If
            Next
            gdvConferencia1.DataSource = ListaConferencia1
            gdvConferencia2.DataSource = ListaConferencia2
            'If (gdvConferencia1.Rows.Count + gdvConferencia2.Rows.Count) <> ListaBaseConferencia.Count Then
            gdvConferencia1.DataBind()
            gdvConferencia2.DataBind()
            'End If
            'txtApartamento.Text = ""
            'SE EXISTIR ALGO A SER EXIBIDO O PAINEL SERÁ VISUALIADO
            If (ListaConferencia1.Count > 0 Or ListaConferencia2.Count > 0) Then
                PnlConferencia.Visible = True
            End If
        End If

        '=========COMPLETAR ENXOVAL'=========
        objCheckInOutVO = New CheckInOutVO
        objCheckInOutDAO = New CheckInOutDAO(btnUnidade.Attributes.Item("AliasBancoTurismo").ToString)
        Dim ListaBaseConpletarEnxoval As IList
        'ObjConferenciaVO.ApaId = txtApartamento.Text.Replace("'", "")
        ListaBaseConpletarEnxoval = objCheckInOutDAO.ConsultaCompletarEnxovais()

        'Dim TotAptoConferencia = ListaBaseConpletarEnxoval.Count
        'lblTotConferencia.Text = CStr(TotAptoConferencia)

        'CRIANDO AS LISTAS AUXILIARES
        Dim ListaEnxoval1 As IList = New ArrayList
        Dim ListaEnxoval2 As IList = New ArrayList
        cont = 1
        'PERCORRER A LISATA BASE INSERINDO OS VALORES NAS LISTA AUXILIARES
        'QUE IRÃO PREENCHER OS GRID'S
        For Each item As CheckInOutVO In ListaBaseConpletarEnxoval
            Select Case cont
                Case 1 : ListaEnxoval1.Add(item)
                Case 2 : ListaEnxoval2.Add(item)
            End Select
            If cont = 2 Then
                cont = 1
            Else
                cont += 1
            End If
        Next
        gdvCompletarEnxoval1.DataSource = ListaEnxoval1
        gdvCompletarEnxoval2.DataSource = ListaEnxoval2
        'If (gdvConferencia1.Rows.Count + gdvConferencia2.Rows.Count) <> ListaBaseConferencia.Count Then
        gdvCompletarEnxoval1.DataBind()
        gdvCompletarEnxoval2.DataBind()
        'End If
        'txtApartamento.Text = ""
        'SE EXISTIR ALGO A SER EXIBIDO O PAINEL SERÁ VISUALIADO
        If (ListaEnxoval1.Count > 0 Or ListaEnxoval2.Count > 0) Then
            pnlCompletarEnxoval.Visible = True
        Else
            pnlCompletarEnxoval.Visible = False
        End If

        '=========ENXOVAIS COMPLETADOS'=========
        objCheckInOutVO = New CheckInOutVO
        objCheckInOutDAO = New CheckInOutDAO(btnUnidade.Attributes.Item("AliasBancoTurismo").ToString)
        Dim ListaBaseEnxovaisCompletados As IList
        'ObjConferenciaVO.ApaId = txtApartamento.Text.Replace("'", "")
        ListaBaseEnxovaisCompletados = objCheckInOutDAO.ConsultaEnxovaisCompletados()

        'CRIANDO AS LISTAS AUXILIARES
        Dim ListaEnxovalCompletados1 As IList = New ArrayList
        Dim ListaEnxovalCompletados2 As IList = New ArrayList
        Dim ListaEnxovalCompletados3 As IList = New ArrayList
        Dim ListaEnxovalCompletados4 As IList = New ArrayList

        cont = 1
        'PERCORRER A LISATA BASE INSERINDO OS VALORES NAS LISTA AUXILIARES
        'QUE IRÃO PREENCHER OS GRID'S
        For Each item As CheckInOutVO In ListaBaseEnxovaisCompletados
            Select Case cont
                Case 1 : ListaEnxovalCompletados1.Add(item)
                Case 2 : ListaEnxovalCompletados2.Add(item)
                Case 3 : ListaEnxovalCompletados3.Add(item)
                Case 4 : ListaEnxovalCompletados4.Add(item)
            End Select
            If cont = 4 Then
                cont = 1
            Else
                cont += 1
            End If
        Next
        gdvEnxAtendido1.DataSource = ListaEnxovalCompletados1
        gdvEnxAtendido2.DataSource = ListaEnxovalCompletados2
        gdvEnxAtendido3.DataSource = ListaEnxovalCompletados3
        gdvEnxAtendido4.DataSource = ListaEnxovalCompletados4

        'If (gdvConferencia1.Rows.Count + gdvConferencia2.Rows.Count) <> ListaBaseConferencia.Count Then
        gdvEnxAtendido1.DataBind()
        gdvEnxAtendido2.DataBind()
        gdvEnxAtendido3.DataBind()
        gdvEnxAtendido4.DataBind()
        'End If
        'txtApartamento.Text = ""
        'SE EXISTIR ALGO A SER EXIBIDO O PAINEL SERÁ VISUALIADO
        If (ListaEnxovalCompletados1.Count > 0 Or ListaEnxovalCompletados2.Count > 0 Or ListaEnxovalCompletados3.Count > 0 Or ListaEnxovalCompletados4.Count > 0) Then
            divEnxAtendido.Visible = True
        Else
            divEnxAtendido.Visible = False
        End If

        ''Contando os paineis para exibição do totalizador de check in ou out
        'Dim ContaPainel As Long = 0
        'If pnlPrioridade.Visible = True Then
        '    ContaPainel += 1
        'End If
        'If pnlArrumacao.Visible = True Then
        '    ContaPainel += 1
        'End If
        'If pnlOcupado.Visible = True Then
        '    ContaPainel += 1
        'End If
        'If pnlManutencao.Visible = True Then
        '    ContaPainel += 1
        'End If
        'If pnlLimpo.Visible = True Then
        '    ContaPainel += 1
        'End If
        'If PnlConferencia.Visible = True Then
        '    ContaPainel += 1
        'End If
        ''Mostrando ou escondendo o totalizador de check in ou out'
        'If ContaPainel <= 3 Then
        CType(Page.Master.FindControl("Label1"), Label).Visible = "True"
        CType(Page.Master.FindControl("Label2"), Label).Visible = "True"
        CType(Page.Master.FindControl("Label3"), Label).Visible = "True"
        CType(Page.Master.FindControl("Label4"), Label).Visible = "True"
        CType(Page.Master.FindControl("Label5"), Label).Visible = "True"
        CType(Page.Master.FindControl("Label6"), Label).Visible = "True"
        CType(Page.Master.FindControl("Label7"), Label).Visible = "True"
        CType(Page.Master.FindControl("Label8"), Label).Visible = "True"
        CType(Page.Master.FindControl("Label9"), Label).Visible = "True"
        CType(Page.Master.FindControl("Label10"), Label).Visible = "True"
        CType(Page.Master.FindControl("Label11"), Label).Visible = "True"
        CType(Page.Master.FindControl("ckbTempoReal"), CheckBox).Visible = "True"
        'Else
        '    If CType(Page.Master.FindControl("Label1"), Label).Visible = "true" Then
        '        CType(Page.Master.FindControl("Label1"), Label).Visible = "False"
        '        CType(Page.Master.FindControl("Label2"), Label).Visible = "False"
        '        CType(Page.Master.FindControl("Label3"), Label).Visible = "False"
        '        CType(Page.Master.FindControl("Label4"), Label).Visible = "False"
        '        CType(Page.Master.FindControl("Label5"), Label).Visible = "False"
        '        CType(Page.Master.FindControl("Label6"), Label).Visible = "False"
        '        CType(Page.Master.FindControl("Label7"), Label).Visible = "False"
        '        CType(Page.Master.FindControl("Label8"), Label).Visible = "False"
        '        CType(Page.Master.FindControl("Label9"), Label).Visible = "False"
        '        CType(Page.Master.FindControl("Label10"), Label).Visible = "False"
        '        CType(Page.Master.FindControl("Label11"), Label).Visible = "False"
        '        CType(Page.Master.FindControl("ckbTempoReal"), CheckBox).Visible = "False"
        '    End If
        'End If

        'Mostrando o totalizador de check in e out na tela da governança
        If CType(Page.Master.FindControl("ckbTempoReal"), CheckBox).Checked Then
            objSituacaoAtualVO = New SituacaoAtualVO
            objSituacaoAtualDAO = New SituacaoAtualDAO(btnUnidade.Attributes.Item("AliasBancoTurismo").ToString)
            objSituacaoAtualVO = objSituacaoAtualDAO.consultar(0, "", "", True, True, True, True)
            CType(Page.Master.FindControl("Label2"), Label).Text = CStr(CInt(objSituacaoAtualVO.sitAptoCheckin.ToString) - CInt(objSituacaoAtualVO.sitAptoJaEstada.ToString)) + " Aptos"
            CType(Page.Master.FindControl("Label3"), Label).Text = objSituacaoAtualVO.sitHospedeCheckin.ToString + " Hóspedes |"
            CType(Page.Master.FindControl("Label5"), Label).Text = objSituacaoAtualVO.sitAptoCheckout.ToString + " Aptos "
            CType(Page.Master.FindControl("Label6"), Label).Text = objSituacaoAtualVO.sitHospedeCheckout.ToString + " Hóspedes |"
            CType(Page.Master.FindControl("Label8"), Label).Text = objSituacaoAtualVO.sitAptoEstada.ToString + " Aptos "
            CType(Page.Master.FindControl("Label9"), Label).Text = objSituacaoAtualVO.sitHospedeEstada.ToString + " Hóspedes |"
            CType(Page.Master.FindControl("Label10"), Label).Text = objSituacaoAtualVO.sitPassante.ToString + " Passantes |"
            CType(Page.Master.FindControl("Label11"), Label).Text = "Total " + CStr(CInt(objSituacaoAtualVO.sitHospedeEstada.ToString) + CInt(objSituacaoAtualVO.sitPassante.ToString))
        Else
            CType(Page.Master.FindControl("Label2"), Label).Text = " Aptos "
            CType(Page.Master.FindControl("Label3"), Label).Text = " Hóspedes"
            CType(Page.Master.FindControl("Label5"), Label).Text = " Aptos"
            CType(Page.Master.FindControl("Label6"), Label).Text = " Hóspedes"
            CType(Page.Master.FindControl("Label8"), Label).Text = " Aptos"
            CType(Page.Master.FindControl("Label9"), Label).Text = " Hóspedes"
            CType(Page.Master.FindControl("Label10"), Label).Text = " Passantes"
            CType(Page.Master.FindControl("Label11"), Label).Text = "Total "
        End If

        ScriptManager.RegisterStartupScript(txtApartamento, txtApartamento.GetType(), Guid.NewGuid().ToString(), "$get('" + txtApartamento.ClientID + "').focus();", True)

    End Sub
    Protected Sub lnkOcupado_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lnkOcupado.Click
        'Não serão feito atendimento único
        AtendimentoUnico = 0
        Session("TipoSituacaoApto") = "Ocupado"
        hddAptoOcupado.Value = 0
        pnlStatus.Visible = False
        PnlAtendimento.Visible = True
        pnlConsultaAtendimento.Visible = True
        lblTotalApartamentos.Text = ""
        'Montando os blocos, Caldas e Pirenopolis'
        cmbBlocoAtendimento.Items.Clear()
        Select Case btnUnidade.Attributes.Item("UOP").ToString
            Case "Caldas Novas"
                cmbBlocoAtendimento.Items.Insert(0, New ListItem("Todos...", "0"))
                cmbBlocoAtendimento.Items.Insert(1, New ListItem("Rio Tocantins", "1"))
                cmbBlocoAtendimento.Items.Insert(2, New ListItem("Rio Araguaia", "2"))
                cmbBlocoAtendimento.Items.Insert(3, New ListItem("Rio Paranaiba", "3"))
                cmbBlocoAtendimento.Items.Insert(4, New ListItem("Rio Vermelho", "33"))
                cmbBlocoAtendimento.SelectedValue = 0
            Case "Pirenopolis"
                Response.Cookies("UOP").Value = "Pirenopolis"
                cmbBlocoAtendimento.Items.Insert(0, New ListItem("Selecione...", "0"))
                cmbBlocoAtendimento.Items.Insert(1, New ListItem("Pirenópolis", "1"))
                cmbBlocoAtendimento.SelectedValue = 0
        End Select
    End Sub

    Protected Sub lnkAtendimentoOcupado_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lnkAtendimentoOcupado.Click
        'Será feito atendimento para um único apartamento
        btnChamaA.Attributes.Add("AtendimentoUnico", 1)
        'Abilitando e desabilitando os paines e componentes'
        Session("TipoSituacaoApto") = "Ocupado"
        'pnlStatus.Visible = False
        'PnlAtendimento.Visible = True
        pnlConsultaAtendimento.Visible = False
        'Quem alimenta o ApaDesc é um javascrip no load da página no OnClick do link
        lblAptoArrumacao.Text = hddApaDesc.Value
        btnChamaA_Click(sender, e)
    End Sub

    Protected Sub lnkIntegrantesOcupado_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lnkIntegrantesOcupado.Click
        pnlStatus.Visible = False
        ObjIntegranteVO = New IntegrantesVO
        ObjIntegranteDAO = New IntegrantesDAO()
        ObjIntegranteVO.ApaId = hddApaId.Value
        lblTituloEmprestimo.Text = "APARTAMENTO: " & hddApaDesc.Value
        gdvIntegrantes.DataSource = ObjIntegranteDAO.ConsultaIntegrante(ObjIntegranteVO, btnUnidade.Attributes.Item("AliasBancoTurismo").ToString)
        gdvIntegrantes.DataBind()
        pnlIntegrantes.Visible = True
        gdvIntegrantes.Visible = True
    End Sub

    Protected Sub btnConfirmarMan_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnConfirmarMan.Click
        'If Session("salvo") = True Then
        '    Session("alterando") = "NAO"
        '    'Abilitando com false para que possa ser inserido um novo item'
        '    Session("salvo") = False
        '    txtAssunto.Text = ""
        '    txtDescricao.Text = ""
        '    hdCodigo.Value = 0
        '    scpSolicitacao.SetFocus(txtAssunto)
        '    Return
        'End If
        Try
            ''Using para transações distribuídas, caso aconteça qualquer erro dentro da sua declaração, todos os processos serão abortados
            'Dim TransScopeOption As New System.Transactions.TransactionScopeOption
            'TransScopeOption = TransactionScopeOption.Required
            'Dim TransOptions As New System.Transactions.TransactionOptions
            'TransOptions.IsolationLevel = IsolationLevel.ReadCommitted
            'Dim transScope As New System.Transactions.TransactionScope(TransScopeOption, TransOptions)
            'Using (transScope)

            If (txtDescricaoManutencao.Text.Length = 0 Or txtAssuntoManutencao.Text.Length = 0) Then
                txtAssuntoManutencao.Focus()
                ScriptManager.RegisterStartupScript(Page, Page.[GetType](), Guid.NewGuid().ToString(), "alert('Não salvo! Preencha os campos antes de prosseguir(Assunto e Descrição).');", True)
                hddProcessando.Value = ""
                btnChamaManutencao_ModalPopupExtender.Show()
                Return
            End If
            'classes da solicitação'
            ObjSolicitacaoVO = New SolicitacaoVO
            ObjSolicitacaoVO.solId = 0 'hdCodigo.Value
            ObjSolicitacaoDAO = New SolicitacaoDAO()
            'CARREGANDO O OBJETO
            'ObjSolicitacaoVO = PreencheObjeto()
            'BUSCANDO O CENTRO DE CUSTO DO FUNCIONARIO NA TABELA TBFUNCIONARIOS NO SISTEMA DE RESTAURANTE DOS SERVIDORES'
            ObjFuncionarioVO = New FuncionarioVO
            ObjFuncionarioDAO = New FuncionarioDAO()
            ObjFuncionarioVO.Matricula = btnAuxilarMan.Attributes.Item("MatriculaAd") 'Session("MatriculaAD") 'Matricula que veio do AD'
            ObjFuncionarioVO = ObjFuncionarioDAO.ConsultarFuncMatricula(ObjFuncionarioVO, btnUnidade.Attributes.Item("UOP").ToString)
            'Testa a Matricula e se estiver <=2 não deixará prossseguir
            If (ObjFuncionarioVO.Matricula.Length <= 2) Then
                ScriptManager.RegisterStartupScript(Page, Page.[GetType](), Guid.NewGuid().ToString(), "alert('A matrícula do funcinário é inválida, informe o centro de informática.');", True)
                Return
            End If
            ObjApartamentosVO = New ApartamentosVO
            ObjApartamentosDAO = New ApartamentosDAO()
            ObjSolicitacaoVO = New SolicitacaoVO
            ObjSolicitacaoDAO = New SolicitacaoDAO()
            'ESSE CASO É PRA ATENDER OS APTOS DO KILZER QUE POSSUI DIVERGENCIA ENTRE APAID E APADESC COM O DB2, SÓ TERÁ EFEITO NA BUSCA NO DB2'
            If Trim(hddApaCCusto.Value) = "06.11.01" Then
                Select Case hddApaArea.Value  'txtSubLocalizacao.SelectedValue
                    Case 77 : ObjSolicitacaoVO.Area = 60
                    Case 70 : ObjSolicitacaoVO.Area = 77
                    Case 69 : ObjSolicitacaoVO.Area = 70
                    Case 68 : ObjSolicitacaoVO.Area = 69
                    Case 67 : ObjSolicitacaoVO.Area = 68
                    Case 66 : ObjSolicitacaoVO.Area = 67
                    Case 65 : ObjSolicitacaoVO.Area = 66
                    Case 64 : ObjSolicitacaoVO.Area = 65
                    Case 63 : ObjSolicitacaoVO.Area = 64
                    Case 62 : ObjSolicitacaoVO.Area = 63
                    Case 61 : ObjSolicitacaoVO.Area = 62
                    Case 60 : ObjSolicitacaoVO.Area = 61
                    Case Else
                        'CASO CONTRARIO CONTINUARA COMO A ÁREA PEGO NO INICIO
                        ObjSolicitacaoVO.Area = hddApaArea.Value
                End Select
            Else
                ObjSolicitacaoVO.Area = hddApaArea.Value
            End If
            ObjSolicitacaoVO = ObjSolicitacaoDAO.BuscaInformacoesAptoBDProd(hddApaCCusto.Value, ObjSolicitacaoVO.Area)
            ObjSolicitacaoVO.CentroCustoSolicitante = Trim(ObjFuncionarioVO.CentroCusto)
            'Continua carregando os dados da solicitacao'
            ObjSolicitacaoVO.DataHoraSolicitacao = Date.Today
            ObjSolicitacaoVO.DataLog = Date.Today
            ObjSolicitacaoVO.Situacao = "E" 'Em andamento
            'Pegando o patrimônio e a descrição do bem caso o tenha.
            If txtBem.SelectedIndex > 0 Then
                ObjSolicitacaoVO.Patrimonio = Trim(Mid(txtBem.SelectedItem.Text, 1, 6)) 'Pegando apenas o número do patrimônio'
                ObjSolicitacaoVO.DesBem = Trim(Mid(txtBem.SelectedItem.Text, 9, 58)).Replace("""", "")  'Pegando apenas a descriçao do bem para conserto'
            Else
                ObjSolicitacaoVO.Patrimonio = ""
                ObjSolicitacaoVO.DesBem = ""
            End If

            ObjSolicitacaoVO.UsuarioSolicitante = User.Identity.Name.ToString.Replace("SESC-GO.COM.BR\", "")
            ObjSolicitacaoVO.DisplayNameSolicitante = btnAuxilarMan.Attributes.Item("Usuario") 'Session("usuario")
            If Request.UserHostAddress = "::1" Then
                ObjSolicitacaoVO.IpUnidade = "10.102.100.23"
            Else
                ObjSolicitacaoVO.IpUnidade = Request.UserHostAddress
            End If
            'Devolvendo o aparea antigo para ficar correto no hdesk'
            ObjSolicitacaoVO.Area = hddApaArea.Value
            If chkBloquearApto.Checked = True Then
                ObjSolicitacaoVO.BloqueioApartamento = "S"
            Else
                ObjSolicitacaoVO.BloqueioApartamento = "N"
            End If
            ObjSolicitacaoVO.Assunto = txtAssuntoManutencao.Text.Replace("'", "").Replace("""", "").ToUpper
            ObjSolicitacaoVO.Descricao = txtDescricaoManutencao.Text.Replace("'", "").Replace("""", "").ToUpper
            ObjSolicitacaoVO.UsuarioLog = User.Identity.Name.ToString.Replace("SESC-GO.COM.BR\", "")
            If chkBloquearApto.Checked = True Then
                ObjSolicitacaoVO.PrevisaoAtendimento = Format(DateAdd(DateInterval.Day, 1, Now), "yyyy-MM-dd HH:mm:ss")
            Else
                ObjSolicitacaoVO.PrevisaoAtendimento = Format(CDate("1900-01-01"), "yyyy-MM-dd")
            End If
            'ObjSolicitacaoVO.PrevisaoAtendimento = Format(CDate("1900-01-01"), "yyyy-MM-dd")
            ObjSolicitacaoVO.DataAtendimento = Format(CDate("1900-01-01"), "yyyy-MM-dd")
            ObjSolicitacaoVO.NomeFuncAtendimento = " "
            ObjSolicitacaoVO.MatriculaAtendimento = "0"
            ObjSolicitacaoVO.ObsManutencao = " "
            ObjSolicitacaoVO.GrauPrioridade = "E"
            ObjSolicitacaoVO.Avaliacao = "0" 'Sem avaliacao'
            ObjSolicitacaoVO.Devolucao = " "
            'Se estiver em Caldas Será Caldas Novas e piri Pirenópolis
            ObjSolicitacaoVO.IdUnidade = btnUnidade.Attributes.Item("UnidadeEscritorio")

            'BUSCA O APAID PRIMEIRAMENTE'
            ObjBloqueioAptoVO = New BloqueioAptoVO
            ObjBloqueioAptoDAO = New BloqueiAptoDAO
            'De posse do apaid, iremos então evocar a classe da governança onde ira colocar o apto em manutenção e gerar o atendimento'
            ObjBloqueioAptoVO.Usuario = User.Identity.Name.ToString.Replace("SESC-GO.COM.BR\", "")
            ObjBloqueioAptoVO.ApaId = hddApaId.Value
            ObjBloqueioAptoVO.Acao = "M"
            ObjBloqueioAptoVO.AlteraManutencao = "0"
            'Só irá mandar os 200 primeiros caracteres para o tbmanutencao'
            ObjBloqueioAptoVO.ManDescricaoResquisitante = Mid(Replace(txtDescricaoManutencao.Text, "'", ""), 1, 200)
            ObjBloqueioAptoVO.Dia = 1
            'Vendo se terá bloqueio ou não no apartamento
            Dim BloqueioApartamento As String = ""
            If chkBloquearApto.Checked = True Then
                BloqueioApartamento = "S"
            Else
                BloqueioApartamento = "N"
            End If

            GravaLog("Vou chamar o comando inserir")

            ObjSolicitacaoVO.SetorExecutante = drpSetor.SelectedValue
            'Objeto carregado, vamos iniciar o processo de criação da solicitação no Help Desk
            Select Case (ObjSolicitacaoDAO.Inserir(ObjSolicitacaoVO, btnUnidade.Attributes.Item("AliasBancoHdManutencao").ToString, BloqueioApartamento, ObjBloqueioAptoVO, btnUnidade.Attributes.Item("AliasBancoTurismo").ToString))
                Case 1 'Inserindo'
                    GravaLog("Entrou no inserção 1 --OK ")
                    'Bloquear apartamento no turismo social, o chkbloqueioapto só será exibido quando a pessoa tiver autorização para isso'
                    If chkBloquearApto.Checked = True Then
                        'Busca o apaid primeiramente'
                        ObjBloqueioAptoVO = New BloqueioAptoVO
                        ObjBloqueioAptoDAO = New BloqueiAptoDAO
                        'Pegando o valor do custo e area ainda na tabela de apartamento para bloquear o apto no turismo social'
                        ObjBloqueioAptoVO.CCusto = hddApaCCusto.Value
                        ObjBloqueioAptoVO.Area = hddApaArea.Value
                        ObjBloqueioAptoVO = ObjBloqueioAptoDAO.ConsultaApaIdApartamento(ObjBloqueioAptoVO, btnUnidade.Attributes.Item("AliasBancoTurismo").ToString)
                        'Alocando o apaid no hddapaid'
                        hddApaId.Value = ObjBloqueioAptoVO.ApaId
                        'De posse do apaid, iremos então evocar a classe da governança onde ira colocar o apto em manutenção e gerar o atendimento'
                        ObjBloqueioAptoVO.Usuario = User.Identity.Name.ToString.Replace("SESC-GO.COM.BR\", "")
                        'ObjBloqueioAptoVO.ApaId = hddApaId.Value
                        ObjBloqueioAptoVO.Acao = "M"
                        ObjBloqueioAptoVO.AlteraManutencao = "0"
                        'Só irá mandar os 200 primeiros caracteres para o tbmanutencao'
                        ObjBloqueioAptoVO.ManDescricaoResquisitante = Mid(Replace(txtDescricaoManutencao.Text, "'", ""), 1, 200)
                        ObjBloqueioAptoVO.Dia = 1 'hddDiasBloqueio.Value
                        'ObjBloqueioAptoDAO.AtualizaAptoGovernanca(ObjBloqueioAptoVO, btnUnidade.Attributes.Item("AliasBancoTurismo").ToString)
                    End If
                    'Logo acima podemos ver caso esse valor seja true não deixa duplicar o registro'
                    Session("alterando") = "NAO"
                    txtAssuntoManutencao.Text = ""
                    txtDescricaoManutencao.Text = ""
                    'scpSolicitacao.SetFocus(txtAssunto)
                Case 3 'Erro na inserção'
                    hddProcessando.Value = ""
                    ScriptManager.RegisterStartupScript(Page, Page.[GetType](), Guid.NewGuid().ToString(), "alert('Erro! Informe o Centro de Informática!');", True)
                    Return
            End Select
            'Voltando o bloqueio para o status padrão'
            chkBloquearApto.Checked = False
            'hddDiasBloqueio.Value = 0
            hddProcessando.Value = ""
            btnChamaManutencao_ModalPopupExtender.Hide()
            'Refaz apenas a consulta que sofreou alteração'
            Select Case txtPegaOrigemMan.Text
                Case "A"
                    ListaSomenteArrumacao()
                    ListaSomenteManutencao()
                Case "L"
                    ListaSomenteLimpos()
                    ListaSomenteManutencao()
            End Select
            'Setando a opção de bem patrimonial
            chkBemPatrimonial.Checked = True
            'Finalizando a transação
            'transScope.Complete()
            'End Using
        Catch ex As Exception
            GravaLog("Deu erro no processo de inserção: msg " & ex.StackTrace.ToString)
            Throw New Exception(ex.StackTrace.ToString.Replace(Chr(13), ""))
        End Try
    End Sub

    Protected Sub btnConsultarAtendimento_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnConsultarAtendimento.Click
        pnlGridsOcupadoA.Visible = True
        ObjApartamentosVO = New ApartamentosVO
        ObjApartamentosDAO = New ApartamentosDAO()
        ObjApartamentosVO.ApaFederacao = cmbFederacao.SelectedValue
        Select Case Session("TipoSituacaoApto")
            Case "Ocupado"
                'If hddAptoOcupado.Value <> 0 Then
                '    ObjApartamentosVO.ApaId = hddAptoOcupado.Value
                ObjApartamentosVO.ApaStatus = "O"
                'Else
                'ObjApartamentosVO.ApaId = 0
                'End If
            Case "Arrumacao"
                If hddAptoArrumacao.Value <> 0 Then
                    ObjApartamentosVO.ApaId = hddAptoArrumacao.Value
                    ObjApartamentosVO.ApaStatus = "A"
                Else
                    ObjApartamentosVO.ApaId = 0
                End If
            Case "Prioridade"
                If hddAptoPrioridade.Value <> 0 Then
                    ObjApartamentosVO.ApaId = hddAptoPrioridade.Value
                Else
                    ObjApartamentosVO.ApaId = 0
                End If
            Case "Limpo"
                If hddAptoLimpo.Value <> 0 Then
                    ObjApartamentosVO.ApaId = hddAptoLimpo.Value
                    ObjApartamentosVO.ApaStatus = "L"
                Else
                    ObjApartamentosVO.ApaId = 0
                End If
        End Select

        'DEFININDO FEDERACAO OU NÃO
        pnlGridsOcupadoA.Visible = True
        '========ARRUMAÇÃO============
        Dim ListaBaseOcupado As IList
        '1 - Quer dizer que irá pegar apenas o apto selecionado
        If AtendimentoUnico = 1 Then
            ListaBaseOcupado = ObjApartamentosDAO.ConsultaApartamento(ObjApartamentosVO, cmbBlocoAtendimento.SelectedValue, hddApaId.Value, hddApaId.Value, ObjApartamentosVO.ApaStatus, btnUnidade.Attributes.Item("AliasBancoTurismo").ToString)
        Else
            ListaBaseOcupado = ObjApartamentosDAO.ConsultaApartamento(ObjApartamentosVO, cmbBlocoAtendimento.SelectedValue, cmbAptoIni.SelectedValue, cmbAptoFinal.SelectedValue, ObjApartamentosVO.ApaStatus, btnUnidade.Attributes.Item("AliasBancoTurismo").ToString)
        End If
        'ListaBaseOcupado = ObjApartamentosDAO.ConsultaApartamento(ObjApartamentosVO, cmbBlocoAtendimento.SelectedValue, cmbAptoIni.SelectedValue, cmbAptoFinal.SelectedValue, ObjApartamentosVO.ApaStatus)
        Session("Ocupado") = ListaBaseOcupado
        'INFORMANDO O TOTAL DE APARTAMENTOS EM ARRUMAÇÃO'
        'Dim TotAptoOcupado As Integer = ListaBaseOcupado.Count
        'lblTotOcupado.Text = CStr(TotAptoOcupado)
        'CRIANDO AS LISTAS AUXILIARES'
        Dim ListaOcupado1 As IList = New ArrayList
        Dim ListaOcupado2 As IList = New ArrayList
        Dim ListaOcupado3 As IList = New ArrayList
        Dim ListaOcupado4 As IList = New ArrayList
        Dim Cont As Byte = 1
        'PERCORRER A LISATA BASE INSERINDO OS VALORES NAS LISTA AUXILIARES'
        'QUE IRÃO PREENCHER OS GRID'S
        For Each item As ApartamentosVO In ListaBaseOcupado
            Select Case Cont
                Case 1 : ListaOcupado1.Add(item)
                Case 2 : ListaOcupado2.Add(item)
                Case 3 : ListaOcupado3.Add(item)
                Case 4 : ListaOcupado4.Add(item)
            End Select
            If Cont = 4 Then
                Cont = 1
            Else
                Cont += 1
            End If
        Next
        gdv1OcupadoA.DataSource = ListaOcupado1
        gdv1OcupadoA.DataBind()
        gdv2OcupadoA.DataSource = ListaOcupado2
        gdv2OcupadoA.DataBind()
        gdv3OcupadoA.DataSource = ListaOcupado3
        gdv3OcupadoA.DataBind()
        gdv4OcupadoA.DataSource = ListaOcupado4
        gdv4OcupadoA.DataBind()
        lblMensagemArrumacao.Text = ""
        lblTotalApartamentos.Text = gdv1OcupadoA.Rows.Count + gdv2OcupadoA.Rows.Count + gdv3OcupadoA.Rows.Count + gdv4OcupadoA.Rows.Count & " Apartamento(s)"
    End Sub

    Protected Sub gdvManutencao1_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gdvManutencao1.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            CType(e.Row.FindControl("lblManutencao1"), Label).Text = gdvManutencao1.DataKeys(e.Row.RowIndex).Item(1).ToString
            CType(e.Row.FindControl("lblManutencao1"), Label).Attributes.Add("onClick", "javascript:aspnetForm.ctl00_conPlaHolTurismoSocial_txtAptoManutencao.value = '" & gdvManutencao1.DataKeys(e.Row.RowIndex).Item(1).ToString & "' ,aspnetForm.ctl00_conPlaHolTurismoSocial_hddAptoManutencao.value = " & gdvManutencao1.DataKeys(e.Row.RowIndex).Item(0).ToString & ",aspnetForm.ctl00_conPlaHolTurismoSocial_txtDescricaoManut.value = '" & "Solicitação: " & Format(CDate(gdvManutencao1.DataKeys(e.Row.RowIndex).Item(3).ToString), "dd/MM/yyyy") & "\nPrevisão...: " & Format(CDate(gdvManutencao1.DataKeys(e.Row.RowIndex).Item(4).ToString), "dd/MM/yyyy") & "\n" & gdvManutencao1.DataKeys(e.Row.RowIndex).Item(2).ToString & "',aspnetForm.ctl00_conPlaHolTurismoSocial_hddApaCCusto.value = '" & gdvManutencao1.DataKeys(e.Row.RowIndex).Item(5).ToString & "',aspnetForm.ctl00_conPlaHolTurismoSocial_hddApaArea.value = '" & gdvManutencao1.DataKeys(e.Row.RowIndex).Item(6).ToString & "' ")
            'Apartamentos do Oswaldo kizer tem diferenca entre Apaid e Desc.Acertando apenas a visualização na tela
        End If
    End Sub

    Protected Sub gdvManutencao2_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gdvManutencao2.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            CType(e.Row.FindControl("lblManutencao2"), Label).Text = gdvManutencao2.DataKeys(e.Row.RowIndex).Item(1).ToString
            CType(e.Row.FindControl("lblManutencao2"), Label).Attributes.Add("onClick", "javascript:aspnetForm.ctl00_conPlaHolTurismoSocial_txtAptoManutencao.value = '" & gdvManutencao2.DataKeys(e.Row.RowIndex).Item(1).ToString & "' ,aspnetForm.ctl00_conPlaHolTurismoSocial_hddAptoManutencao.value = " & gdvManutencao2.DataKeys(e.Row.RowIndex).Item(0).ToString & ",aspnetForm.ctl00_conPlaHolTurismoSocial_txtDescricaoManut.value = '" & "Solicitação: " & Format(CDate(gdvManutencao2.DataKeys(e.Row.RowIndex).Item(3).ToString), "dd/MM/yyyy") & "\nPrevisão...: " & Format(CDate(gdvManutencao2.DataKeys(e.Row.RowIndex).Item(4).ToString), "dd/MM/yyyy") & "\n" & gdvManutencao2.DataKeys(e.Row.RowIndex).Item(2).ToString & "',aspnetForm.ctl00_conPlaHolTurismoSocial_hddApaCCusto.value = '" & gdvManutencao2.DataKeys(e.Row.RowIndex).Item(5).ToString & "',aspnetForm.ctl00_conPlaHolTurismoSocial_hddApaArea.value = '" & gdvManutencao2.DataKeys(e.Row.RowIndex).Item(6).ToString & "' ")
        End If
    End Sub

    Protected Sub lnkData_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        ObjEmprestimosVO = New EmprestimosVO
        ObjEmprestimosDAO = New EmprestimosDAO()
        ObjEmprestimosVO.CseId = CLng(sender.CommandArgument.ToString)
        hddCSeId.Value = CLng(sender.CommandArgument.ToString)
        ObjEmprestimosVO = ObjEmprestimosDAO.ConsultaEmprestimoEspecifico(ObjEmprestimosVO, btnUnidade.Attributes.Item("AliasBancoTurismo").ToString)
        'EXIBINDO E ESCONDENDO OS COMPONENTES
        'A É PARA FAZER UM UPDATE NA SP SPATUALIZACONSUMOEMPRESTIMO'
        Session("Operacao") = "A"
        pnlCadastroEmprestimo.Visible = True
        gdvEmprestimosIntegrante.Visible = False
        lblDadosEmprestimo.Visible = True
        'CARREGA OS VALORES DA OPERAÇÃO
        drpOperacoes.DataValueField = "TMoId"
        drpOperacoes.DataTextField = "TMoDescricao"
        drpOperacoes.DataSource = ObjEmprestimosDAO.PreencheOperacao(ObjEmprestimosVO, btnUnidade.Attributes.Item("AliasBancoTurismo").ToString)
        drpOperacoes.DataBind()
        'CARREGA OS DADOS NA TELA
        With ObjEmprestimosVO
            txtEmpData.Text = Format(CDate(.EmpData.ToString), "dd/MM/yyyy")
            txtEmpDescricao.Text = .EmpDescricao.ToString
            txtEmpValor.Text = Format(CDec(.EmpValor.ToString), "###,###.#0")
            txtQuantidade.Text = Format(CDec(.EmpQuantidade.ToString), "###,###.#0")
            drpOperacoes.SelectedValue = ObjEmprestimosVO.TMoId
            Select Case ObjEmprestimosVO.EmpOrigem
                Case "CS" : rbtTipo.SelectedIndex = 0
                Case "EG" : rbtTipo.SelectedIndex = 1
            End Select
            'RBTTIPO.SELECTEDVALUE = 1 VER COM HAAS
        End With
        txtEmpDescricao.Focus()
    End Sub

    Protected Sub imgEmprestimos_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        Dim Lista As IList
        Lista = New ArrayList
        ObjEmprestimosVO = New EmprestimosVO
        Dim IntId As String = sender.CommandArgument.ToString
        ObjEmprestimosVO.IntId = CLng(Mid(IntId, 1, IntId.ToString.IndexOf("#")))
        hddIntId.Value = CLng(Mid(IntId, 1, IntId.ToString.IndexOf("#")))
        ObjEmprestimosDAO = New EmprestimosDAO()
        Lista = ObjEmprestimosDAO.ConsultaEmprestimoIntegrante(ObjEmprestimosVO, btnUnidade.Attributes.Item("AliasBancoTurismo").ToString)
        lblDadosEmprestimo.Text = "Serviços de Consumo/Empréstimos-" & Mid(IntId, CLng(IntId.ToString.IndexOf("#") + 2), CLng(Trim(IntId.ToString.IndexOf("@")) - CLng(IntId.ToString.IndexOf("#") + 2)))
        gdvEmprestimosIntegrante.DataSource = ObjEmprestimosDAO.ConsultaEmprestimoIntegrante(ObjEmprestimosVO, btnUnidade.Attributes.Item("AliasBancoTurismo").ToString)
        gdvEmprestimosIntegrante.DataBind()
        pnlCadastroEmprestimo.Visible = False
        If gdvEmprestimosIntegrante.Rows.Count > 0 Then
            pnlEmprestimosIntegrante.Visible = True
            gdvEmprestimosIntegrante.Visible = True
        Else
            pnlEmprestimosIntegrante.Visible = False
            gdvEmprestimosIntegrante.Visible = False
        End If
    End Sub

    Protected Sub imgInserir_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        btnFecharIntegrante.Visible = False
        pnlCadastroEmprestimo.Visible = True
        pnlEmprestimosIntegrante.Visible = True
        gdvEmprestimosIntegrante.Visible = False
        gdvIntegrantes.Visible = False
        'MONTANDO BARRA DE TITULO
        ObjEmprestimosVO = New EmprestimosVO
        Dim IntId As String = sender.CommandArgument
        ObjEmprestimosVO.IntId = CLng(Mid(IntId, 1, IntId.ToString.IndexOf("#")))
        'Session("IntId") = CLng(Mid(IntId, 1, IntId.ToString.IndexOf("#")))
        hddIntId.Value = CLng(Mid(IntId, 1, IntId.ToString.IndexOf("#")))
        hddCSeId.Value = 0 'SÓ PRA TER UM VALOR E NÃO PASSAR NULO
        ObjEmprestimosDAO = New EmprestimosDAO()
        lblDadosEmprestimo.Text = "Serviços de Consumo/Empréstimos-" & Mid(IntId, CLng(IntId.ToString.IndexOf("#") + 2), CLng(Trim(IntId.ToString.IndexOf("@")) - CLng(IntId.ToString.IndexOf("#") + 2)))
        Session("NomeHospede") = Mid(IntId, CLng(IntId.ToString.IndexOf("#") + 2), CLng(Trim(IntId.ToString.IndexOf("@")) - CLng(IntId.ToString.IndexOf("#") + 2)))
        'txtEmpData.Text = Format(Date.Today, "dd/MM/yyyy")
        'CARREGA OS VALORES DA OPERAÇÃO
        drpOperacoes.DataValueField = "TMoId"
        drpOperacoes.DataTextField = "TMoDescricao"
        drpOperacoes.DataSource = ObjEmprestimosDAO.PreencheOperacao(ObjEmprestimosVO, btnUnidade.Attributes.Item("AliasBancoTurismo").ToString)
        drpOperacoes.DataBind()
        'A É PARA FAZER UMA INSERÇÃO NA SP SPATUALIZACONSUMOEMPRESTIMO
        Session("Operacao") = "I"
        'LIMPA TODOS OS CAMPOS PARA NOVA INSERÇÃO
        txtEmpData.Text = Format(Date.Today, "dd/MM/yyyy")
        txtEmpDescricao.Text = ""
        txtEmpValor.Text = "0,00"
        txtQuantidade.Text = "0,00"
        drpOperacoes.SelectedIndex = 3
        rbtTipo.SelectedIndex = 1
        'MOVENDO O FOCO
        txtEmpDescricao.Focus()
    End Sub

    Protected Sub gdvIntegrantes_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gdvIntegrantes.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            CType(e.Row.FindControl("imgEmprestimos"), ImageButton).CommandArgument = gdvIntegrantes.DataKeys(e.Row.RowIndex).Item(4).ToString & "#" & gdvIntegrantes.DataKeys(e.Row.RowIndex).Item(1).ToString & "@"
            CType(e.Row.FindControl("imgInserir"), ImageButton).CommandArgument = gdvIntegrantes.DataKeys(e.Row.RowIndex).Item(4).ToString & "#" & gdvIntegrantes.DataKeys(e.Row.RowIndex).Item(1).ToString & "@"
            e.Row.Cells.Item(1).Text = Format(CDate(e.Row.Cells.Item(1).Text), "dd/MM/yyyy")
            e.Row.Cells.Item(4).Text = Format(CDec(e.Row.Cells.Item(4).Text), "###,##0.00")
            'Se o valor for maior que Zero é porque teve emprestimo
            If CDec(gdvIntegrantes.DataKeys(e.Row.RowIndex).Item(5).ToString) >= 0.01 Then
                CType(e.Row.FindControl("imgEmprestimos"), Image).Visible = True
            Else
                CType(e.Row.FindControl("imgEmprestimos"), Image).Visible = False
            End If
        End If
    End Sub

    Protected Sub gdvEmprestimosIntegrante_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gdvEmprestimosIntegrante.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            CType(e.Row.FindControl("imgUsuario"), ImageButton).ToolTip = Replace(gdvEmprestimosIntegrante.DataKeys(e.Row.RowIndex).Item(7).ToString, "SESC-GO.COM.BR\", "") & "-" & gdvEmprestimosIntegrante.DataKeys(e.Row.RowIndex).Item(8).ToString
            e.Row.Cells.Item(2).Text = Format(CDec(e.Row.Cells.Item(2).Text), "###,##0.00")
            'If e.Row.Cells(0).Text <> "" Then
            '    e.Row.Cells.Item(0).Text = Format(CDate(e.Row.Cells.Item(0).Text), "dd/MM/yyyy")
            'End If
            'CType(e.Row.FindControl("imgUsuario"), ImageButton).CommandArgument = gdvEmprestimosIntegrante.DataKeys(e.Row.RowIndex).Item(9).ToString 'O SeId do emprestimo para a consulta'
            CType(e.Row.FindControl("lnkData"), LinkButton).CommandArgument = gdvEmprestimosIntegrante.DataKeys(e.Row.RowIndex).Item(9).ToString 'O SeId do emprestimo para a consulta'
            CType(e.Row.FindControl("imgApagarEmp"), ImageButton).CommandArgument = gdvEmprestimosIntegrante.DataKeys(e.Row.RowIndex).Item(9).ToString 'O SeId do emprestimo para a consulta'
        End If
    End Sub

    Protected Sub btnGravar_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnGravar.Click
        ObjEmprestimosVO = New EmprestimosVO
        ObjEmprestimosDAO = New EmprestimosDAO()
        'VERIFICA A QUANTIDADE E VALOR DIGITADO'
        If ((CDec(txtQuantidade.Text) = 0.0) Or (CDec(txtEmpValor.Text) = 0.0)) Then
            ScriptManager.RegisterStartupScript(Page, Page.[GetType](), Guid.NewGuid().ToString(), "alert('O valor e a quantidade tem que ser maior que zero');", True)
            txtEmpValor.Focus()
            hddProcessando.Value = ""
            Return
        End If

        'PREENCHENDO O OBJETO'
        With ObjEmprestimosVO
            .CseId = hddCSeId.Value
            .IntId = hddIntId.Value
            .EmpData = txtEmpData.Text
            .EmpOperacao = Session("Operacao")
            '.EmpValor = Format(CDec(txtEmpValor.Text), "######.#0").Replace(",", ".")
            .EmpValor = CDec(txtEmpValor.Text).ToString.Replace(",", ".")
            .TMoId = drpOperacoes.SelectedValue
            '.EmpQuantidade = Format(CDec(txtQuantidade.Text), "######.#0").Replace(",", ".")
            .EmpQuantidade = CDec(txtQuantidade.Text).ToString.Replace(",", ".")
            .EmpDescricao = txtEmpDescricao.Text.Replace("'", "")
            .EmpManual = "S"
            .EmpUsuario = User.Identity.Name.ToString.Replace("SESC-GO.COM.BR\", "")
            Select Case rbtTipo.SelectedIndex
                Case 0 : .EmpOrigem = "CS" 'CONSUMO DE SERVIÇO
                Case 1 : .EmpOrigem = "EG" 'EMPRESTIMO GOVERNANÇA
            End Select
        End With
        Select Case ObjEmprestimosDAO.InserirEmprestimo(ObjEmprestimosVO, 0, 0, btnUnidade.Attributes.Item("AliasBancoTurismo").ToString)
            Case 1
                'REATIVANDO OS PAINEIS DO CADASTRO DE EMPRESTIMO'
                pnlCadastroEmprestimo.Visible = False
                pnlEmprestimosIntegrante.Visible = False
                gdvIntegrantes.Visible = True
                btnFecharIntegrante.Visible = True
                'RECALCULA O VALOR DOS DEBIDOS E ATUALIZA A TELA'
                ObjIntegranteVO = New IntegrantesVO
                ObjIntegranteDAO = New IntegrantesDAO()
                ObjIntegranteVO.ApaId = hddApaId.Value   'Session("ApaId")
                gdvIntegrantes.DataSource = ObjIntegranteDAO.ConsultaIntegrante(ObjIntegranteVO, btnUnidade.Attributes.Item("AliasBancoTurismo").ToString)
                gdvIntegrantes.DataBind()
                'Imprime cupom do empréstimo

                hddImpressao.Value = "                     Setor de governança" & Chr(10) _
                & "                   E M P R É S T I M O S" & Chr(10) _
                & "Produto                                               Qtd   Valor" & Chr(10) _
                & "-----------------------------------------------------------------------------" & Chr(10) _
                & StringTamanhoFixo(txtEmpDescricao.Text, 40) & "  " & txtQuantidade.Text & "    " & txtEmpValor.Text & Chr(10) _
                & "-----------------------------------------------------------------------------" & Chr(10) _
                & "Valor Total:                                 R$ " & txtEmpValor.Text & Chr(10) & Chr(10) & Chr(10) & Chr(10) _
                & "------------------------------------------------" & Chr(10) _
                & Session("NomeHospede") & Chr(10) _
                & "Apartamento:" & hddApaDesc.Value

                ScriptManager.RegisterStartupScript(Page, Page.[GetType](), Guid.NewGuid().ToString(), _
                 "ticketgovernanca();alert('Operação realizada com sucesso.');", True)
                'Final do cupom'
            Case 0
                ScriptManager.RegisterStartupScript(Page, Page.[GetType](), Guid.NewGuid().ToString(), "alert('Houve um erro ao salvar os dados.');", True)
                hddProcessando.Value = ""
                Return
        End Select
        hddProcessando.Value = ""
    End Sub

    Protected Sub btnCancelar_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancelar.Click
        pnlEmprestimosIntegrante.Visible = False
        gdvIntegrantes.Visible = True
        btnFecharIntegrante.Visible = True
    End Sub
    Protected Sub lnkManutencaoOcupado_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lnkManutencaoOcupado.Click
        txtAssuntoManutencao.Text = ""
        btnAuxilarMan_Click(sender, e)
        btnChamaManutencao_Click(sender, e)
        ListaSomenteManutencao()
        txtAssuntoManutencao.Focus()
    End Sub

    Protected Sub btnConsultaApto_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnConsultaApto.Click
        ObjApartamentosVO = New ApartamentosVO
        ObjApartamentosVO.BloId = cmbBlocoAtendimento.SelectedValue
        ObjApartamentosVO.ApaFederacao = cmbFederacao.SelectedValue
        ObjApartamentosDAO = New ApartamentosDAO()
        cmbAptoIni.DataTextField = "ApaDesc"
        cmbAptoIni.DataValueField = "ApaId"
        'ZERO NO FINAL DA CONSULTA INDICA QUE TODOS OS APARTAMENTOS SERÃO BUSCADOS'
        cmbAptoIni.DataSource = ObjApartamentosDAO.ConsultaApartamento(ObjApartamentosVO, ObjApartamentosVO.BloId, 0, 0, "O", btnUnidade.Attributes.Item("AliasBancoTurismo").ToString)
        cmbAptoIni.DataBind()
        cmbAptoIni.Items.Insert(0, New ListItem("Selecione...", "0"))
        cmbAptoFinal.DataTextField = "ApaDesc"
        cmbAptoFinal.DataValueField = "ApaId"
        'ZERO NO FINAL DA CONSULTA INDICA QUE TODOS OS APARTAMENTOS SERÃO BUSCADOS'
        cmbAptoFinal.DataSource = ObjApartamentosDAO.ConsultaApartamento(ObjApartamentosVO, ObjApartamentosVO.BloId, 0, 0, "O", btnUnidade.Attributes.Item("AliasBancoTurismo").ToString)
        cmbAptoFinal.DataBind()
        cmbAptoFinal.Items.Insert(0, New ListItem("Selecione...", "0"))
    End Sub

    Protected Sub cmbBlocoAtendimento_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbBlocoAtendimento.SelectedIndexChanged
        btnConsultarAtendimento.Enabled = True
        btnConsultaApto_Click(sender, e)
        cmbFederacao.Focus()
    End Sub

    Protected Sub cmbFederacao_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbFederacao.SelectedIndexChanged
        btnConsultaApto_Click(sender, e)
        cmbAptoIni.Focus()
    End Sub

    Protected Sub cmbAptoIni_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbAptoIni.SelectedIndexChanged
        If cmbAptoIni.SelectedValue = 0 Then
            cmbAptoFinal.SelectedValue = 0
        End If
        cmbAptoFinal.Focus()
    End Sub

    Protected Sub cmbAptoFinal_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbAptoFinal.SelectedIndexChanged
        If cmbAptoFinal.SelectedValue = 0 Then
            cmbAptoIni.SelectedValue = 0
        End If
        btnConsultarAtendimento.Focus()
    End Sub

    Protected Sub lnkManutencaoArrumacao_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lnkManutencaoArrumacao.Click
        Session("BaseManutencao") = "Arrumacao"
        txtAssuntoManutencao.Text = ""
        btnAuxilarMan_Click(sender, e)
        btnChamaManutencao_Click(sender, e)
        ListaSomenteManutencao()
        txtAssuntoManutencao.Focus()
    End Sub

    Protected Sub lnkRevisaoLimpo_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lnkRevisaoLimpo.Click
        'Definindo atendimento único
        btnChamaA.Attributes.Add("AtendimentoUnico", 1)
        'Habilitando e desabilitando os paines e componentes'
        Session("TipoSituacaoApto") = "Limpo"
        'Exibindo ApaDesc no cadastro de atendimento - Limpo
        lblAptoL.Text = hddApaDesc.Value
        PnlAtendimento.Visible = False
        pnlConsultaAtendimento.Visible = False
        btnChamaL_Click(sender, e)
    End Sub

    Protected Sub lnkAtendimentoArrumacao_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lnkAtendimentoArrumacao.Click
        'Será feito atendimento para um único apartamento
        btnChamaA.Attributes.Add("AtendimentoUnico", 1)
        'Habilitando e desabilitando os paines e componentes'
        Session("TipoSituacaoApto") = "Arrumacao"
        lblAptoAr.Text = hddApaDesc.Value
        'PnlAtendimento.Visible = True
        pnlConsultaAtendimento.Visible = False
        btnChamaArrumacao_Click(sender, e)
    End Sub

    Protected Sub lnkArrumacaoLimpo_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lnkArrumacaoLimpo.Click
        ObjAtendimentoVO = New AtendimentoGovVO
        ObjAtendimentoVO.ApaId = hddApaId.Value
        ObjAtendimentoVO.Acao = "G"
        ObjAtendimentoVO.Manutencao = ""
        ObjAtendimentoVO.Usuario = User.Identity.Name.ToString.Replace("SESC-GO.COM.BR\", "")
        ObjAtendimentoVO.AlteraManutencao = "N"
        ObjAtendimentoVO.Dia = 0
        ObjAtendimentoDAO = New AtendimentoGovDAO()
        '1-OK 0-ROLBACK
        If ObjAtendimentoDAO.AtualizaAptoGovernanca(ObjAtendimentoVO, btnUnidade.Attributes.Item("AliasBancoTurismo").ToString) = 0 Then
            ScriptManager.RegisterStartupScript(Page, Page.[GetType](), Guid.NewGuid().ToString(), "alert('Erro ao enviar apartamento para Arrumação.');", True)
            Return
        End If
        btnConsultar_Click(sender, e)
    End Sub

    Protected Sub lnkManutencao_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lnkManutencao.Click
        ObjApartamentosVO = New ApartamentosVO
        'ObjApartamentosVO.BloId = cmbBlocoAtendimento.SelectedValue
        'ObjApartamentosVO.ApaFederacao = cmbFederacao.SelectedValue
        ObjApartamentosDAO = New ApartamentosDAO()
        cmbHistoricoApartamentos.DataTextField = "ApaDesc"
        cmbHistoricoApartamentos.DataValueField = "ApaId"
        'ZERO NO FINAL DA CONSULTA INDICA QUE TODOS OS APARTAMENTOS SERÃO BUSCADOS'
        cmbHistoricoApartamentos.DataSource = ObjApartamentosDAO.ConsultaApartamento(ObjApartamentosVO, 0, 0, 0, "", btnUnidade.Attributes.Item("AliasBancoTurismo").ToString)
        cmbHistoricoApartamentos.DataBind()
        cmbHistoricoApartamentos.Items.Insert(0, New ListItem("Selecione...", "0"))
        pnlHistoricoManutencao.Visible = True
        txtHistoricoManutencaoD1.Text = Format(CDate(Date.Today), "dd/MM/yyyy")
        txtHistoricoManutencaoD2.Text = Format(CDate(Date.Today), "dd/MM/yyyy")
        pnlStatus.Visible = False
        txtHistoricoManutencaoD1.Focus()
    End Sub

    Protected Sub btnConsultaHistoricoMan_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnConsultaHistoricoMan.Click
        'SE A DATA NÃO FOR INFORMADA, O SISTEMA DEVERÁ ENTENDER O DIA DE HOJE'
        If (txtHistoricoManutencaoD1.Text.Length = 0 Or txtHistoricoManutencaoD2.Text.Length = 0) Then
            txtHistoricoManutencaoD1.Text = Format(CDate(Today), "dd/MM/yyyy")
            txtHistoricoManutencaoD2.Text = Format(CDate(Today), "dd/MM/yyyy")
        End If
        'OBRIGAD A SELECIONAR UM APARTAMENTO
        'If cmbHistoricoApartamentos.SelectedValue = 0 Then
        '    lblMsnHistManutencao.Text = "Informe o Apartamento que deseja consultar."
        '    Return
        'End If
        gdvHistoricoManutencao.Visible = True
        ObjManutencaoVO = New ManutencaoVO
        ObjManutencaoVO.ApaId = cmbHistoricoApartamentos.SelectedValue
        ObjManutencaoDAO = New ManutencaoDAO()
        gdvHistoricoManutencao.DataSource = ObjManutencaoDAO.ConsultaHistoricoManutencao(ObjManutencaoVO, txtHistoricoManutencaoD1.Text, txtHistoricoManutencaoD2.Text, btnUnidade.Attributes.Item("AliasBancoTurismo").ToString)
        gdvHistoricoManutencao.DataBind()
        If gdvHistoricoManutencao.Rows.Count = 0 Then
            lblMsnHistManutencao.Text = "Não existem informações a serem exibidas."
        Else
            lblMsnHistManutencao.Text = ""
        End If
    End Sub

    Protected Sub btnVoltarHistoricoMan_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnVoltarHistoricoMan.Click
        pnlHistoricoManutencao.Visible = False
        gdvHistoricoManutencao.Visible = False
        pnlStatus.Visible = True
    End Sub

    Protected Sub lnkPrioridadeAten_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lnkPrioridadeAten.Click
        'Habilitando atendimento unico
        btnChamaA.Attributes.Add("AtendimentoUnico", 1)
        'Habilitando e desabilitando os paines e componentes'
        Session("TipoSituacaoApto") = "Prioridade"
        'PnlAtendimento.Visible = True
        pnlConsultaAtendimento.Visible = False
        btnChamaA_Click(sender, e)
    End Sub

    Protected Sub btnRelatorios_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnRelatorios.Click
        Response.Redirect("RelGovernanca.aspx")
    End Sub
    Protected Sub LinkButton1_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles LinkButton1.Click
        ''Não serão feito atendimento único
        'AtendimentoUnico = 0
        'ObjArrumacaoVO = New ArrumacaoVO
        'ObjArrumacaoDAO = New ArrumacaoDAO(ObjArrumacaoVO)
        'Dim Federacao As String = ""
        ''DEFININDO FEDERACAO OU NÃO
        'Select Case txtFederacao.SelectedValue
        '    Case "Sim" : Federacao = "S"
        '    Case "Não" : Federacao = "N"
        '    Case Else
        '        Federacao = ""
        'End Select
        'pnlStatus.Visible = False
        'pnlGridsLimpoA.Visible = True
        ''========ARRUMAÇÃO============
        'Dim ListaBaseLimpo As IList
        'ListaBaseLimpo = ObjArrumacaoDAO.PesquisaArrumacao(ObjArrumacaoVO, Federacao)
        'Session("Arrumacao") = ListaBaseLimpo
        ''INFORMANDO O TOTAL DE APARTAMENTOS EM ARRUMAÇÃO'
        'Dim TotAptoLimpo As Integer = ListaBaseLimpo.Count
        'lblTotLimpo.Text = CStr(TotAptoLimpo)
        ''CRIANDO AS LISTAS AUXILIARES'
        'Dim ListaLimpo1 As IList = New ArrayList
        'Dim ListaLimpo2 As IList = New ArrayList
        'Dim ListaLimpo3 As IList = New ArrayList
        'Dim ListaLimpo4 As IList = New ArrayList
        'Dim Cont As Byte = 1
        ''PERCORRER A LISATA BASE INSERINDO OS VALORES NAS LISTA AUXILIARES'
        ''QUE IRÃO PREENCHER OS GRID'S
        'For Each item As ArrumacaoVO In ListaBaseLimpo
        '    Select Case Cont
        '        Case 1 : ListaLimpo1.Add(item)
        '        Case 2 : ListaLimpo2.Add(item)
        '        Case 3 : ListaLimpo3.Add(item)
        '        Case 4 : ListaLimpo4.Add(item)
        '    End Select
        '    If Cont = 4 Then
        '        Cont = 1
        '    Else
        '        Cont += 1
        '    End If
        'Next
        'gdv1LimpoA.DataSource = ListaLimpo1
        'gdv1LimpoA.DataBind()
        'gdv2LimpoA.DataSource = ListaLimpo2
        'gdv2LimpoA.DataBind()
        'gdv3LimpoA.DataSource = ListaLimpo3
        'gdv3LimpoA.DataBind()
        'gdv4LimpoA.DataSource = ListaLimpo4
        'gdv4LimpoA.DataBind()
        'lblMensagemArrumacao.Text = ""
        ''Else
        ''lblMensagemArrumacao.Text = "Não existem atendimentos a serem exibidos."
        ''End If
        'btnVoltaLimpoA.Visible = True
    End Sub

    Protected Sub lnkArrumacao_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lnkArrumacao.Click
        'Não serão feito atendimento único
        btnChamaA.Attributes.Add("AtendimentoUnico", 0)
        'EXIBINDO OS PAINEIS
        pnlGridArrumacao.Visible = True
        pnlStatus.Visible = False
        txtData1Arrumacao.Text = Format(Date.Today, "dd/MM/yyyy")
        txtData2Arrumacao.Text = Format(Date.Today, "dd/MM/yyyy")
        'CARREGANDO LISTA DE CAMAREIRAS
        drpCamArrumacao.DataValueField = "CamId"
        drpCamArrumacao.DataTextField = "CamNome"
        drpCamArrumacao.DataSource = Session("ListaCamareiras")
        drpCamArrumacao.DataBind()
        drpCamArrumacao.Items.Insert(0, New ListItem("Todas", "0"))
        'CARREGANDO LISTA DE APARTAMENTOS'
        ObjApartamentosVO = New ApartamentosVO
        ObjApartamentosDAO = New ApartamentosDAO()
        drpAptoArrumacao.DataValueField = "Apaid"
        drpAptoArrumacao.DataTextField = "ApaDesc"
        drpAptoArrumacao.DataSource = ObjApartamentosDAO.ListaGeralApartamento(ObjApartamentosVO, btnUnidade.Attributes.Item("AliasBancoTurismo").ToString)
        drpAptoArrumacao.DataBind()
        drpAptoArrumacao.Items.Insert(0, New ListItem("Todos", "0"))
        'O Grid de arrumação estiver ativado,irá desativar
        If pnlGridsArrumacaoA.Visible = True Then
            pnlGridsArrumacaoA.Visible = False
        End If
        btnConfirmarArrumacao_Click(sender, e)
        btnConfirmarArrumacao.Focus()
    End Sub

    Protected Sub btnConfirmarArrumacao_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnConfirmarArrumacao.Click
        'CARREGANDO LISTA DE APTOS QUE SOFRERAM ARRUMAÇÃO PARA POSSÍVEIS ALTERAÇÕES'
        ObjAtendimentoVO = New AtendimentoGovVO
        ObjAtendimentoVO.ApaId = drpAptoArrumacao.SelectedValue
        ObjAtendimentoVO.CamId = drpCamArrumacao.SelectedValue
        ObjAtendimentoDAO = New AtendimentoGovDAO()
        If (ObjAtendimentoDAO.ConsultaListaLnkArrumacao(ObjAtendimentoVO, Format(CDate(txtData1Arrumacao.Text), "dd/MM/yyyy"), Format(CDate(txtData2Arrumacao.Text), "dd/MM/yyyy"), ObjAtendimentoVO.CamId, btnUnidade.Attributes.Item("AliasBancoTurismo").ToString).Count > 0) Then
            'DEFININDO FEDERACAO OU NÃO
            pnlGridsArrumacaoA.Visible = True
            '========ARRUMAÇÃO============
            Dim ListaBaseArrumacao As IList
            ListaBaseArrumacao = ObjAtendimentoDAO.ConsultaListaLnkArrumacao(ObjAtendimentoVO, Format(CDate(txtData1Arrumacao.Text), "dd-MM-yyyy"), Format(CDate(txtData2Arrumacao.Text), "dd-MM-yyyy"), drpCamArrumacao.SelectedValue, btnUnidade.Attributes.Item("AliasBancoTurismo").ToString)
            Session("Arrumacao") = ListaBaseArrumacao
            'INFORMANDO O TOTAL DE APARTAMENTOS EM ARRUMAÇÃO'
            'Dim TotAptoArrumacao As Integer = ListaBaseArrumacao.Count
            'lblTotArrumacao.Text = CStr(TotAptoArrumacao)
            'CRIANDO AS LISTAS AUXILIARES'
            Dim ListaArrumacao1 As IList = New ArrayList
            Dim ListaArrumacao2 As IList = New ArrayList
            Dim ListaArrumacao3 As IList = New ArrayList
            Dim ListaArrumacao4 As IList = New ArrayList
            Dim Cont As Byte = 1
            'PERCORRER A LISATA BASE INSERINDO OS VALORES NAS LISTA AUXILIARES'
            'QUE IRÃO PREENCHER OS GRID'S
            For Each item As AtendimentoGovVO In ListaBaseArrumacao
                Select Case Cont
                    Case 1 : ListaArrumacao1.Add(item)
                    Case 2 : ListaArrumacao2.Add(item)
                    Case 3 : ListaArrumacao3.Add(item)
                    Case 4 : ListaArrumacao4.Add(item)
                End Select
                If Cont = 4 Then
                    Cont = 1
                Else
                    Cont += 1
                End If
            Next
            gdv1AtendimentoA.DataSource = ListaArrumacao1
            gdv1AtendimentoA.DataBind()
            gdv2AtendimentoA.DataSource = ListaArrumacao2
            gdv2AtendimentoA.DataBind()
            gdv3AtendimentoA.DataSource = ListaArrumacao3
            gdv3AtendimentoA.DataBind()
            gdv4AtendimentoA.DataSource = ListaArrumacao4
            gdv4AtendimentoA.DataBind()
            lblMensagemArrumacao.Text = ""
        Else
            lblMensagemArrumacao.Text = "Não existem atendimentos a serem exibidos."
        End If
    End Sub

    Protected Sub lnkLimpoArrumacao_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lnkLimpoArrumacao.Click
        'BUSCANDO A ORIGEM NA TABELA DE ATENDIMENTOGOV'
        ObjAtendimentoVO = New AtendimentoGovVO
        ObjAtendimentoVO.ApaId = hddApaId.Value
        ObjAtendimentoDAO = New AtendimentoGovDAO()
        ObjAtendimentoVO = ObjAtendimentoDAO.BuscaAgoOrigem(ObjAtendimentoVO, "A", btnUnidade.Attributes.Item("AliasBancoTurismo").ToString)
        Dim GuardaOrigem = ObjAtendimentoVO.AGoOrigem
        hddAgoId.Value = ObjAtendimentoVO.AGoId
        'APLICANDO UPDATE NO TBAPARTAMENTO, PASSANDO DE STATUS A PARA L
        ObjAtendimentoVO = New AtendimentoGovVO
        ObjAtendimentoVO.ApaId = hddApaId.Value
        ObjAtendimentoVO.Acao = "A"
        'ObjAtendimentoVO.AGoOrigem = " "
        ObjAtendimentoVO.AGoId = hddAgoId.Value
        ObjAtendimentoVO.AGoCCasal = 0
        ObjAtendimentoVO.AGoCSolteiro = 0
        ObjAtendimentoVO.AGoBerco = 0
        ObjAtendimentoVO.AGoTravesseiro = 0
        ObjAtendimentoVO.AGoJogoToalhas = 0
        ObjAtendimentoVO.AGoRoloPapel = 0
        ObjAtendimentoVO.AGoSacoLixo = 0
        ObjAtendimentoVO.AGoSabonete = 0
        ObjAtendimentoVO.AGoTapete = "N"
        ObjAtendimentoVO.AGoObservacao = " "
        ObjAtendimentoVO.Usuario = User.Identity.Name.ToString.Replace("SESC-GO.COM.BR\", "")
        ObjAtendimentoVO.AGoOrigem = GuardaOrigem
        ObjAtendimentoDAO = New AtendimentoGovDAO()
        Select Case ObjAtendimentoDAO.Inserir(ObjAtendimentoVO, btnUnidade.Attributes.Item("AliasBancoTurismo").ToString)
            Case 0
                ScriptManager.RegisterStartupScript(Page, Page.[GetType](), Guid.NewGuid().ToString(), "alert('Erro ao limpar apartamento.');", True)
                Return
            Case 1
                ListaSomenteArrumacao()
                ListaSomenteLimpos()
                'btnConsultar_Click(sender, e)
        End Select
    End Sub

    Protected Sub gdvConferencia1_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gdvConferencia1.RowDataBound
        'e.row.rowtype = datacontrolrowtype.datarow significa linhas referente a dados no grid - não cabeçalho e nem rodapé
        If e.Row.RowType = DataControlRowType.DataRow Then
            CType(e.Row.FindControl("chkConferencia1"), CheckBox).Text = gdvConferencia1.DataKeys(e.Row.RowIndex).Item(1).ToString
            'Estou guardando o ApaId do apto dentro do attributo do CheckBox para pegar futuramente
            CType(e.Row.FindControl("chkConferencia1"), CheckBox).Attributes.Add("ApaIdGdv1", gdvConferencia1.DataKeys(e.Row.RowIndex).Item(0).ToString)
            'Nesse caso tem emprestimo
            If gdvConferencia1.DataKeys(e.Row.RowIndex).Item(2).ToString = "S" Then
                CType(e.Row.FindControl("imgEmprestimo1C"), ImageButton).Visible = True
                CType(e.Row.FindControl("imgEmprestimo1C"), ImageButton).ToolTip = gdvConferencia1.DataKeys(e.Row.RowIndex).Item(3).ToString.ToUpper
            Else
                CType(e.Row.FindControl("imgEmprestimo1C"), ImageButton).Visible = False
            End If
        End If
    End Sub

    Protected Sub gdvConferencia2_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gdvConferencia2.RowDataBound
        'e.row.rowtype = datacontrolrowtype.datarow significa linhas referente a dados no grid - não cabeçalho e nem rodapé
        If e.Row.RowType = DataControlRowType.DataRow Then
            CType(e.Row.FindControl("chkConferencia2"), CheckBox).Text = gdvConferencia2.DataKeys(e.Row.RowIndex).Item(1).ToString
            'Estou guardando o ApaId do apto dentro do attributo do CheckBox para pegar futuramente
            CType(e.Row.FindControl("chkConferencia2"), CheckBox).Attributes.Add("ApaIdGdv2", gdvConferencia2.DataKeys(e.Row.RowIndex).Item(0).ToString)
            'Nesse caso tem emprestimo
            If gdvConferencia2.DataKeys(e.Row.RowIndex).Item(2).ToString = "S" Then
                CType(e.Row.FindControl("imgEmprestimo2C"), ImageButton).Visible = True
                CType(e.Row.FindControl("imgEmprestimo2C"), ImageButton).ToolTip = gdvConferencia2.DataKeys(e.Row.RowIndex).Item(3).ToString.ToUpper
            Else
                CType(e.Row.FindControl("imgEmprestimo2C"), ImageButton).Visible = False
            End If
        End If
    End Sub

    Protected Sub btnConferenciaSelecionados_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnConferenciaSelecionados.Click
        'Passando no primeiro GRID
        For Each linha As GridViewRow In gdvConferencia1.Rows
            'Ira passar somente nos apartamentos checados
            If CType(linha.FindControl("chkConferencia1"), CheckBox).Checked = True Then
                ObjConferenciaVO = New ConferenciaVO
                ObjConferenciaDAO = New ConferenciaDAO()
                'Iremos pegar o ApaId para atualização do apto
                ObjConferenciaVO.ApaId = CLng(CType(linha.FindControl("chkConferencia1"), CheckBox).Attributes.Item("ApaIdGdv1"))
                Select Case ObjConferenciaDAO.EfetuaConferenciaApto(ObjConferenciaVO, btnUnidade.Attributes.Item("AliasBancoTurismo").ToString)
                    Case 0
                        ScriptManager.RegisterStartupScript(Page, Page.[GetType](), Guid.NewGuid().ToString(), "alert('Erro ao aplicar conferência no apartamento " & ObjConferenciaVO.ApaId & " ');", True)
                End Select
            End If
        Next

        'Passando no Segundo GRID
        For Each linha As GridViewRow In gdvConferencia2.Rows
            'Ira passar somente nos apartamentos checados
            If CType(linha.FindControl("chkConferencia2"), CheckBox).Checked = True Then
                ObjConferenciaVO = New ConferenciaVO
                ObjConferenciaDAO = New ConferenciaDAO()
                'Iremos pegar o ApaId para atualização do apto
                ObjConferenciaVO.ApaId = CLng(CType(linha.FindControl("chkConferencia2"), CheckBox).Attributes.Item("ApaIdGdv2"))
                Select Case ObjConferenciaDAO.EfetuaConferenciaApto(ObjConferenciaVO, btnUnidade.Attributes.Item("AliasBancoTurismo").ToString)
                    Case 0
                        ScriptManager.RegisterStartupScript(Page, Page.[GetType](), Guid.NewGuid().ToString(), "alert('Erro ao aplicar conferência no apartamento " & ObjConferenciaVO.ApaId & " ');", True)
                End Select
            End If
        Next
        'Efetuando novamente a busca
        hddProcessando.Value = ""
        ListaSomenteConferencia()
    End Sub

    Protected Sub chkSelecionaTodos_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkSelecionaTodos.CheckedChanged
        'Passando no primeiro GRID
        If chkSelecionaTodos.Checked = True Then
            For Each linha As GridViewRow In gdvConferencia1.Rows
                CType(linha.FindControl("chkConferencia1"), CheckBox).Checked = True
            Next
            'Passando no Segundo GRID
            For Each linha As GridViewRow In gdvConferencia2.Rows
                CType(linha.FindControl("chkConferencia2"), CheckBox).Checked = True
            Next
        Else
            For Each linha As GridViewRow In gdvConferencia1.Rows
                CType(linha.FindControl("chkConferencia1"), CheckBox).Checked = False
            Next
            'Passando no Segundo GRID
            For Each linha As GridViewRow In gdvConferencia2.Rows
                CType(linha.FindControl("chkConferencia2"), CheckBox).Checked = False
            Next
        End If
    End Sub

    Protected Sub btnChamaA_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnChamaA.Click
        'USADO AO CLICAR NO LINK DE ATENDIMENTO DE APTOS OCUPADOS
        pnlCadastraAtendimento.DataBind()
        btnChamaA_ModalPopupExtender.Show()
        pnlCadastraAtendimento.Focus()
        'Mostrando sempre o ApaDesc no Label do Painel de Atendimento
        If btnChamaA.Attributes.Item("AtendimentoUnico") = 1 Then
            lblAptoArrumacao.Text = hddApaDesc.Value
        Else
            lblAptoArrumacao.Text = (Mid(sender.CommandArgument.ToString, sender.CommandArgument.ToString.IndexOf("#") + 2, sender.CommandArgument.ToString.Length - sender.CommandArgument.ToString.IndexOf("#")))
        End If
        'Zerando sempre os valores das caixas de textos'
        TxtLenSolteiroArrumacao.Text = 0
        txtlenBercoArrumacao.Text = 0
        txtCamaCasalArrumacao.Text = 0
        txtCamaExtraArrumacao.Text = 0
        txtCamaSolteiroArrumacao.Text = 0
        txtBercoArrumacao.Text = 0
        txtFronhaArrumacao.Text = 0
        txtToalhaArrumacao.Text = 0
        txtPapelArrumacao.Text = 0
        txtLixoArrumacao.Text = 0
        txtSaboneteArrumacao.Text = 0
        txtObsArrumacao.Text = ""
        drpCamareiraArrumacao.SelectedValue = 0
        rdTapeteArrumacao.SelectedValue = "N"
    End Sub

    Protected Sub btnEditarA_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        btnChamaA_Click(sender, e)
        'hddApaId.Value = CLng(sender.CommandArgument.ToString)
        'Estou pegando o valor do btnEditA(command Argument) que esta Apaid#ApaDesc - retirei o Apadesc
        hddApaId.Value = CLng(Mid(sender.CommandArgument.ToString, 1, sender.CommandArgument.ToString.IndexOf("#")))
        'Pegando apenas o Apadesc'
        hddAptoOcupado.Value = (Mid(sender.CommandArgument.ToString, sender.CommandArgument.ToString.IndexOf("#") + 2, sender.CommandArgument.ToString.Length - sender.CommandArgument.ToString.IndexOf("#")))
        'Volta sempre o Valor Zero para a camareira
        drpCamareiraArrumacao.SelectedValue = 0
    End Sub

    Protected Sub brnSalvarA_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles brnSalvarA.Click
        'Se deixar algum campo em branco não peremitirá salvar'
        If TxtLenSolteiroArrumacao.Text = "" Or _
                    txtlenBercoArrumacao.Text = "" Or _
                    txtCamaCasalArrumacao.Text = "" Or _
                    txtCamaExtraArrumacao.Text = "" Or _
                    txtCamaSolteiroArrumacao.Text = "" Or _
                    txtBercoArrumacao.Text = "" Or _
                    txtFronhaArrumacao.Text = "" Or _
                    txtToalhaArrumacao.Text = "" Or _
                    txtPapelArrumacao.Text = "" Or _
                    txtLixoArrumacao.Text = "" Or _
                    txtSaboneteArrumacao.Text = "" Then
            ScriptManager.RegisterStartupScript(Page, Page.[GetType](), Guid.NewGuid().ToString(), "alert('Existem campos sem prencher.Salvamento não permitido.');", True)
            Return
        End If

        'For Each linha As GridViewRow In gdvAtendimento.Rows
        If TxtLenSolteiroArrumacao.Text <> 0 Or _
            txtlenBercoArrumacao.Text <> 0 Or _
            txtCamaCasalArrumacao.Text <> 0 Or _
            txtCamaExtraArrumacao.Text <> 0 Or _
            txtCamaSolteiroArrumacao.Text <> 0 Or _
            txtBercoArrumacao.Text <> 0 Or _
            txtFronhaArrumacao.Text <> 0 Or _
            txtToalhaArrumacao.Text <> 0 Or _
            txtPapelArrumacao.Text <> 0 Or _
            txtLixoArrumacao.Text <> 0 Or _
            txtSaboneteArrumacao.Text <> 0 Then
            'CARREGA O VALOR DA LINHAS NO COMPONENTE
            ObjAtendimentoVO = New AtendimentoGovVO
            With ObjAtendimentoVO
                'gdvOpnioes.DataKeys(e.Row.RowIndex).Item(21).ToString
                .AGoId = 0
                .ApaId = hddApaId.Value
                .CamId = drpCamareiraArrumacao.SelectedValue
                .AGoCCasal = txtCamaCasalArrumacao.Text
                '.AGoLencolCasal = CType(linha.FindControl("txtLencolCasal"), TextBox).Text
                .AGoLencolSolteiro = TxtLenSolteiroArrumacao.Text
                .AGoLencolBerco = txtlenBercoArrumacao.Text
                .AGoCSolteiro = txtCamaSolteiroArrumacao.Text
                .AGoBerco = txtBercoArrumacao.Text
                .AGoTravesseiro = txtFronhaArrumacao.Text
                .AGoJogoToalhas = txtToalhaArrumacao.Text
                .AGoRoloPapel = txtPapelArrumacao.Text
                .AGoSacoLixo = txtLixoArrumacao.Text
                .AGoSabonete = txtSaboneteArrumacao.Text
                If rdTapeteArrumacao.SelectedValue = "S" Then
                    .AGoTapete = "S"
                Else
                    .AGoTapete = "N"
                End If
                'ESSA SESSION É ALIMENTADA NO CLICK NOS POPUP DE ATENDIMENTOS'
                Select Case Session("TipoSituacaoApto")
                    Case "Ocupado"
                        .AGoOrigem = "A" 'Arrumação
                    Case "Arrumacao"
                        .AGoOrigem = "A"
                    Case "Prioridade"
                        .AGoOrigem = "A"
                    Case "Limpo"
                        .AGoOrigem = "R" 'Revisão
                End Select
                .AGoObservacao = txtObsArrumacao.Text
                .Acao = "A"
                .ApaCamaExtra = txtCamaExtraArrumacao.Text
                .ApaBerco = 0
                .Usuario = User.Identity.Name.ToString.Replace("SESC-GO.COM.BR\", "") '@Usuario varchar(60)
                .DataLog = Date.Now
            End With
            ObjAtendimentoDAO = New AtendimentoGovDAO()
            Select Case ObjAtendimentoDAO.Inserir(ObjAtendimentoVO, btnUnidade.Attributes.Item("AliasBancoTurismo").ToString)
                Case 0
                    hddProcessando.Value = ""
                    ScriptManager.RegisterStartupScript(Page, Page.[GetType](), Guid.NewGuid().ToString(), "alert('Erro ao Salvar o registro do apartamento " & ObjAtendimentoVO.ApaId & " ');", True)
                    'Se o atendimento for unico volta ao estado normal'
                    If btnChamaA.Attributes.Item("AtendimentoUnico") = 1 Then
                        pnlStatus.Visible = True
                        btnChamaA.Attributes.Add("AtendimentoUnico", 0)
                    End If
                    Return
                Case 1 'ContaApartamento = ContaApartamento + 1  'ScriptManager.RegisterStartupScript(Page, Page.[GetType](), Guid.NewGuid().ToString(), "alert('Registro efetuado com sucesso');", True)
            End Select
        End If
        varreGrid()
        'Evitando o salvamento duplo do registro'
        hddProcessando.Value = ""
        'Se o atendimento for unico volta ao estado normal'
        If btnChamaA.Attributes.Item("AtendimentoUnico") = 1 Then
            pnlStatus.Visible = True
            btnChamaA.Attributes.Add("AtendimentoUnico", 0)
        End If
    End Sub
    Protected Sub varreGrid()
        'GRID 1 - ATENDIMENTOS DE OCUPADOS
        For Each linha As GridViewRow In gdv1OcupadoA.Rows
            'Valor do HddApto foi definico no click do botão imgApaId1
            If (linha.Cells(0).Text = hddApaDesc.Value) And _
           CType(linha.FindControl("imgApaId1"), ImageButton).Visible = True Then
                CType(linha.FindControl("imgApaId1"), ImageButton).ImageUrl = "~/images/Ok.png"
                CType(linha.FindControl("imgApaId1"), ImageButton).Enabled = False
                Exit For
            End If
        Next
        'GRID 2 - ATENDIMENTOS DE OCUPADOS
        For Each linha As GridViewRow In gdv2OcupadoA.Rows
            'Valor do HddApto foi definico no click do botão imgApaId1
            If (linha.Cells(0).Text = hddApaDesc.Value) And _
           CType(linha.FindControl("imgApaId2"), ImageButton).Visible = True Then
                CType(linha.FindControl("imgApaId2"), ImageButton).ImageUrl = "~/images/Ok.png"
                CType(linha.FindControl("imgApaId2"), ImageButton).Enabled = False
                Exit For
            End If
        Next
        'GRID 3 - ATENDIMENTOS DE OCUPADOS
        For Each linha As GridViewRow In gdv3OcupadoA.Rows
            'Valor do HddApto foi definico no click do botão imgApaId1
            If (linha.Cells(0).Text = hddApaDesc.Value) And _
           CType(linha.FindControl("imgApaId3"), ImageButton).Visible = True Then
                CType(linha.FindControl("imgApaId3"), ImageButton).ImageUrl = "~/images/Ok.png"
                CType(linha.FindControl("imgApaId3"), ImageButton).Enabled = False
                Exit For
            End If
        Next
        'GRID 4 - ATENDIMENTOS DE OCUPADOS
        For Each linha As GridViewRow In gdv4OcupadoA.Rows
            'Valor do HddApto foi definico no click do botão imgApaId1
            If (linha.Cells(0).Text = hddApaDesc.Value) And _
           CType(linha.FindControl("imgApaId4"), ImageButton).Visible = True Then
                CType(linha.FindControl("imgApaId4"), ImageButton).ImageUrl = "~/images/Ok.png"
                CType(linha.FindControl("imgApaId4"), ImageButton).Enabled = False
                Exit For
            End If
        Next
    End Sub
    Protected Sub varreGridLimpo()
        For Each linha As GridViewRow In gdv1LimpoA.Rows
            'Valor do HddApto foi definico no click do botão btnEditarA
            If (linha.Cells(0).Text = hddApaDesc.Value) And _
           CType(linha.FindControl("img1LimpoA"), ImageButton).Visible = True Then
                CType(linha.FindControl("img1LimpoA"), ImageButton).ImageUrl = "~/images/Ok.png"
                CType(linha.FindControl("img1LimpoA"), ImageButton).Enabled = False
                Exit For
            End If
        Next
        For Each linha As GridViewRow In gdv2LimpoA.Rows
            'Valor do HddApto foi definico no click do botão btnEditarA
            If (linha.Cells(0).Text = hddApaDesc.Value) And _
           CType(linha.FindControl("img2LimpoA"), ImageButton).Visible = True Then
                CType(linha.FindControl("img2LimpoA"), ImageButton).ImageUrl = "~/images/Ok.png"
                CType(linha.FindControl("img2LimpoA"), ImageButton).Enabled = False
                Exit For
            End If
        Next
        For Each linha As GridViewRow In gdv3LimpoA.Rows
            'Valor do HddApto foi definico no click do botão btnEditarA
            If (linha.Cells(0).Text = hddApaDesc.Value) And _
           CType(linha.FindControl("img3LimpoA"), ImageButton).Visible = True Then
                CType(linha.FindControl("img3LimpoA"), ImageButton).ImageUrl = "~/images/Ok.png"
                CType(linha.FindControl("img3LimpoA"), ImageButton).Enabled = False
                Exit For
            End If
        Next
        For Each linha As GridViewRow In gdv4LimpoA.Rows
            'Valor do HddApto foi definico no click do botão btnEditarA
            If (linha.Cells(0).Text = hddApaDesc.Value) And _
           CType(linha.FindControl("img4LimpoA"), ImageButton).Visible = True Then
                CType(linha.FindControl("img4LimpoA"), ImageButton).ImageUrl = "~/images/Ok.png"
                CType(linha.FindControl("img4LimpoA"), ImageButton).Enabled = False
                Exit For
            End If
        Next
    End Sub
    Protected Sub varreGridArrumacao()
        For Each linha As GridViewRow In gdv1AtendimentoA.Rows
            'Valor do HddApto foi definico no click do botão btnEditarA
            If (linha.Cells(0).Text = hddAptoArrumacao.Value) And _
                CType(linha.FindControl("img1AtendimentoA"), ImageButton).Attributes.Item("GuardaAgoId") = hddAgoId.Value And _
                CType(linha.FindControl("img1AtendimentoA"), ImageButton).Visible = True Then
                CType(linha.FindControl("img1AtendimentoA"), ImageButton).ImageUrl = "~/images/Ok.png"
                CType(linha.FindControl("img1AtendimentoA"), ImageButton).Enabled = False
                Exit For
            End If
        Next
        For Each linha As GridViewRow In gdv2AtendimentoA.Rows
            'Valor do HddApto foi definico no click do botão btnEditarA
            If (linha.Cells(0).Text = hddAptoArrumacao.Value) And _
                CType(linha.FindControl("img2AtendimentoA"), ImageButton).Attributes.Item("GuardaAgoId") = hddAgoId.Value And _
                CType(linha.FindControl("img2AtendimentoA"), ImageButton).Visible = True Then
                CType(linha.FindControl("img2AtendimentoA"), ImageButton).ImageUrl = "~/images/Ok.png"
                CType(linha.FindControl("img2AtendimentoA"), ImageButton).Enabled = False
                Exit For
            End If
        Next
        For Each linha As GridViewRow In gdv3AtendimentoA.Rows
            'Valor do HddApto foi definico no click do botão btnEditarA
            If (linha.Cells(0).Text = hddAptoArrumacao.Value) And _
                CType(linha.FindControl("img3AtendimentoA"), ImageButton).Attributes.Item("GuardaAgoId") = hddAgoId.Value And _
                CType(linha.FindControl("img3AtendimentoA"), ImageButton).Visible = True Then
                CType(linha.FindControl("img3AtendimentoA"), ImageButton).ImageUrl = "~/images/Ok.png"
                CType(linha.FindControl("img3AtendimentoA"), ImageButton).Enabled = False
                Exit For
            End If
        Next
        For Each linha As GridViewRow In gdv4AtendimentoA.Rows
            'Valor do HddApto foi definico no click do botão btnEditarA
            If (linha.Cells(0).Text = hddAptoArrumacao.Value) And _
                CType(linha.FindControl("img4AtendimentoA"), ImageButton).Attributes.Item("GuardaAgoId") = hddAgoId.Value And _
                CType(linha.FindControl("img4AtendimentoA"), ImageButton).Visible = True Then
                CType(linha.FindControl("img4AtendimentoA"), ImageButton).ImageUrl = "~/images/Ok.png"
                CType(linha.FindControl("img4AtendimentoA"), ImageButton).Enabled = False
                Exit For
            End If
        Next
    End Sub

    Protected Sub btnEditarLimpoA_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        btnChamaL_Click(sender, e)
        'Estou pegando o valor do btnEditA(command Argument) que esta Apaid#ApaDesc - retirei o Apadesc
        hddApaId.Value = CLng(Mid(sender.CommandArgument.ToString, 1, sender.CommandArgument.ToString.IndexOf("#")))
        'Pegando apenas o Apadesc'
        hddAptoLimpo.Value = (Mid(sender.CommandArgument.ToString, sender.CommandArgument.ToString.IndexOf("#") + 2, sender.CommandArgument.ToString.Length - sender.CommandArgument.ToString.IndexOf("#")))
        'Volta sempre o Valor Zero para a camareira
        drpCamareiraL.SelectedValue = 0
    End Sub

    Protected Sub btnChamaL_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnChamaL.Click
        'Gerenciando a visualização do modal extender
        pnlCadastroLimpoA.DataBind()
        btnChamaL_ModalPopupExtender.Show()
        pnlCadastroLimpoA.Focus()
        'Mostrando sempre o ApaDesc no Label do Painel de Atendimento
        'lblAptoL.Text = (Mid(sender.CommandArgument.ToString, sender.CommandArgument.ToString.IndexOf("#") + 2, sender.CommandArgument.ToString.Length - sender.CommandArgument.ToString.IndexOf("#")))
        'Zerando sempre os valores das caixas de textos'
        txtCamaCasalL.Text = 0
        txtCamaExtraL.Text = 0
        txtCamaSolteiroL.Text = 0
        txtBercoL.Text = 0
        txtFronhaL.Text = 0
        txtToalhaL.Text = 0
        txtPapelL.Text = 0
        txtLixoL.Text = 0
        txtSaboneteL.Text = 0
        txtObsL.Text = ""
        drpCamareiraL.SelectedValue = 0
        rdTapeteL.SelectedValue = "N"
    End Sub

    Protected Sub brnSalvarL_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles brnSalvarL.Click
        'Se deixar algum campo em branco não peremitirá salvar'
        If txtCamaCasalL.Text = "" Or _
           txtCamaExtraL.Text = "" Or _
           txtCamaSolteiroL.Text = "" Or _
           txtBercoL.Text = "" Or _
           txtFronhaL.Text = "" Or _
           txtToalhaL.Text = "" Or _
           txtPapelL.Text = "" Or _
           txtLixoL.Text = "" Or _
           txtSaboneteL.Text = "" Then
            ScriptManager.RegisterStartupScript(Page, Page.[GetType](), Guid.NewGuid().ToString(), "alert('Existem campos sem prencher.Salvamento não permitido.');", True)
            Return
        End If
        'RETORNA O VALOR DA VARIAVEL PARA "1", SE CLICAR NO LINK ATENDIMENTO GERAL IRA SETAR PARA "0" DE NOVO
        'AtendimentoUnico = 1
        If txtCamaCasalL.Text <> "0" Or _
           txtCamaExtraL.Text <> "0" Or _
           txtCamaSolteiroL.Text <> "0" Or _
           txtBercoL.Text <> "0" Or _
           txtFronhaL.Text <> "0" Or _
           txtToalhaL.Text <> "0" Or _
           txtPapelL.Text <> "0" Or _
           txtLixoL.Text <> "0" Or _
           txtSaboneteL.Text <> "0" Then
            'PEGANDO AO AGOID PARA ATUALIZAR O ATENDIMENTO
            ObjAtendimentoVO = New AtendimentoGovVO
            ObjAtendimentoDAO = New AtendimentoGovDAO()
            ObjAtendimentoVO.ApaId = hddApaId.Value
            'Se o atendimento for único, teremos que procurar o agoid.. isso será feito agora
            If btnChamaA.Attributes.Item("AtendimentoUnico") = 1 Then
                ObjAtendimentoVO = ObjAtendimentoDAO.ConsultaAtendimento(ObjAtendimentoVO, btnUnidade.Attributes.Item("AliasBancoTurismo").ToString)
                hddAgoId.Value = ObjAtendimentoVO.AGoId
            End If
            ObjAtendimentoVO = ObjAtendimentoDAO.ConsultaAtendimento(ObjAtendimentoVO, btnUnidade.Attributes.Item("AliasBancoTurismo").ToString)
            'Guardando o valor do AgoId na Variável
            'Dim GuardaAgoId As Long
            'GuardaAgoId = ObjAtendimentoVO.AGoId
            'INSTANCIANDO UMA NOVA CLASSE E CARREGANDO OS VALORES DA LINHAS NOS ATRIBUTOS DA CLASSE
            ObjAtendimentoVO = New AtendimentoGovVO
            With ObjAtendimentoVO
                .AGoId = hddAgoId.Value  'GuardaAgoId
                .ApaId = hddApaId.Value
                .CamId = drpCamareiraL.SelectedValue
                .AGoCCasal = txtCamaCasalL.Text
                .AGoCSolteiro = txtCamaSolteiroL.Text
                .AGoBerco = txtBercoL.Text
                .AGoTravesseiro = txtFronhaL.Text
                .AGoJogoToalhas = txtToalhaL.Text
                .AGoRoloPapel = txtPapelL.Text
                .AGoSacoLixo = txtLixoL.Text
                .AGoSabonete = txtSaboneteL.Text
                .AGoTapete = rdTapeteL.SelectedValue
                'Quer dizer que cliquei em Revisão
                If btnChamaA.Attributes.Item("AtendimentoUnico") = 1 Then
                    .AGoOrigem = "R"
                Else
                    .AGoOrigem = "G"
                End If
                .AGoObservacao = txtObsL.Text
                .Acao = "A" 'VERIFICAR
                .ApaCamaExtra = txtCamaExtraL.Text
                .ApaBerco = 0 'CType(linha.FindControl("txtBerco"), TextBox).Text '@ApaBerco smallint,
                .Usuario = User.Identity.Name.ToString.Replace("SESC-GO.COM.BR\", "") '@Usuario varchar(60)
            End With
            ObjAtendimentoDAO = New AtendimentoGovDAO()
            Select Case ObjAtendimentoDAO.Inserir(ObjAtendimentoVO, btnUnidade.Attributes.Item("AliasBancoTurismo").ToString)
                Case 0
                    hddProcessando.Value = ""
                    ScriptManager.RegisterStartupScript(Page, Page.[GetType](), Guid.NewGuid().ToString(), "alert('Erro ao Salvar o registro do apartamento " & ObjAtendimentoVO.ApaId & " ');", True)
                    'Voltando o atendimento unico para Zero
                    If btnChamaA.Attributes.Item("AtendimentoUnico") = 1 Then
                        pnlStatus.Visible = True
                        btnChamaA.Attributes.Add("AtendimentoUnico", 0)
                    End If
                    Return
                    'Case 1 : ContaApartamento = ContaApartamento + 1  'ScriptManager.RegisterStartupScript(Page, Page.[GetType](), Guid.NewGuid().ToString(), "alert('Registro efetuado com sucesso');", True)
            End Select
        End If
        'pnlGridLimpo.Visible = False
        varreGridLimpo()
        hddProcessando.Value = ""
        'Voltando o atendimento unico para Zero
        If btnChamaA.Attributes.Item("AtendimentoUnico") = 1 Then
            pnlStatus.Visible = True
            btnChamaA.Attributes.Add("AtendimentoUnico", 0)
        End If
    End Sub

    Protected Sub btnChamaArrumacao_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnChamaArrumacao.Click
        'Gerenciando a visualização do modal extender
        pnlAlterandoAr.DataBind()
        btnChamaArrumacao_ModalPopupExtender.Show()
        pnlAlterandoAr.Focus()
        'Mostrando sempre o ApaDesc no Label do Painel de Atendimento
        'lblAptoAr.Text = (Mid(sender.CommandArgument.ToString, sender.CommandArgument.ToString.IndexOf("#") + 2, sender.CommandArgument.ToString.Length - sender.CommandArgument.ToString.IndexOf("#")))
        'Zerando sempre os valores das caixas de textos'
        txtCamaCasalAr.Text = 0
        txtCamaSolteiroAr.Text = 0
        txtBercoAr.Text = 0
        txtFronhaAr.Text = 0
        txtToalhaAr.Text = 0
        txtPapelAr.Text = 0
        txtLixoAr.Text = 0
        txtSaboneteAr.Text = 0
        txtObsAr.Text = ""
        drpCamareiraAr.SelectedValue = 0
        rdTapeteAr.SelectedValue = "N"
    End Sub

    Protected Sub gdv2AtendimentoA_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gdv2AtendimentoA.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            CType(e.Row.FindControl("img2AtendimentoA"), ImageButton).CommandArgument = gdv2AtendimentoA.DataKeys(e.Row.RowIndex).Item(0).ToString & "#" & gdv2AtendimentoA.DataKeys(e.Row.RowIndex).Item(1).ToString & "@" & gdv2AtendimentoA.DataKeys(e.Row.RowIndex).Item(2).ToString
            CType(e.Row.FindControl("img2AtendimentoA"), ImageButton).ToolTip = gdv2AtendimentoA.DataKeys(e.Row.RowIndex).Item(1).ToString
            'Armazena AgoId
            CType(e.Row.FindControl("img2AtendimentoA"), ImageButton).Attributes.Add("GuardaAgoId", gdv2AtendimentoA.DataKeys(e.Row.RowIndex).Item(2).ToString)
            e.Row.Cells(2).Text = e.Row.Cells(2).Text & "h"
        End If
    End Sub

    Protected Sub gdv1AtendimentoA_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gdv1AtendimentoA.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            CType(e.Row.FindControl("img1AtendimentoA"), ImageButton).CommandArgument = gdv1AtendimentoA.DataKeys(e.Row.RowIndex).Item(0).ToString & "#" & gdv1AtendimentoA.DataKeys(e.Row.RowIndex).Item(1).ToString & "@" & gdv1AtendimentoA.DataKeys(e.Row.RowIndex).Item(2).ToString
            CType(e.Row.FindControl("img1AtendimentoA"), ImageButton).ToolTip = gdv1AtendimentoA.DataKeys(e.Row.RowIndex).Item(1).ToString
            'Armazena AgoId
            CType(e.Row.FindControl("img1AtendimentoA"), ImageButton).Attributes.Add("GuardaAgoId", gdv1AtendimentoA.DataKeys(e.Row.RowIndex).Item(2).ToString)
            e.Row.Cells(2).Text = e.Row.Cells(2).Text & "h"
        End If
    End Sub

    Protected Sub gdv3AtendimentoA_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gdv3AtendimentoA.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            CType(e.Row.FindControl("img3AtendimentoA"), ImageButton).CommandArgument = gdv3AtendimentoA.DataKeys(e.Row.RowIndex).Item(0).ToString & "#" & gdv3AtendimentoA.DataKeys(e.Row.RowIndex).Item(1).ToString & "@" & gdv3AtendimentoA.DataKeys(e.Row.RowIndex).Item(2).ToString
            CType(e.Row.FindControl("img3AtendimentoA"), ImageButton).ToolTip = gdv3AtendimentoA.DataKeys(e.Row.RowIndex).Item(1).ToString
            'Armazena AgoId
            CType(e.Row.FindControl("img3AtendimentoA"), ImageButton).Attributes.Add("GuardaAgoId", gdv3AtendimentoA.DataKeys(e.Row.RowIndex).Item(2).ToString)
            e.Row.Cells(2).Text = e.Row.Cells(2).Text & "h"
        End If
    End Sub

    Protected Sub gdv4AtendimentoA_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gdv4AtendimentoA.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            CType(e.Row.FindControl("img4AtendimentoA"), ImageButton).CommandArgument = gdv4AtendimentoA.DataKeys(e.Row.RowIndex).Item(0).ToString & "#" & gdv4AtendimentoA.DataKeys(e.Row.RowIndex).Item(1).ToString & "@" & gdv4AtendimentoA.DataKeys(e.Row.RowIndex).Item(2).ToString
            CType(e.Row.FindControl("img4AtendimentoA"), ImageButton).ToolTip = gdv4AtendimentoA.DataKeys(e.Row.RowIndex).Item(1).ToString
            'Armazena AgoId
            CType(e.Row.FindControl("img4AtendimentoA"), ImageButton).Attributes.Add("GuardaAgoId", gdv4AtendimentoA.DataKeys(e.Row.RowIndex).Item(2).ToString)
            e.Row.Cells(2).Text = e.Row.Cells(2).Text & "h"
        End If
    End Sub

    Protected Sub img1AtendimentoA_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        btnChamaArrumacao_Click(sender, e)
        'Estou pegando o valor do btnEditA(command Argument) que esta Apaid#ApaDesc - retirei o Apadesc
        hddApaId.Value = CLng(Mid(sender.CommandArgument.ToString, 1, sender.CommandArgument.ToString.IndexOf("#")))
        'Pegando apenas o Apadesc'
        hddAptoArrumacao.Value = (Mid(sender.CommandArgument.ToString, sender.CommandArgument.ToString.IndexOf("#") + 2, (sender.CommandArgument.ToString.IndexOf("@")) - (sender.CommandArgument.ToString.IndexOf("#") + 1)))
        hddAgoId.Value = (Mid(sender.CommandArgument.ToString, sender.CommandArgument.ToString.IndexOf("@") + 2, sender.CommandArgument.ToString.Length - sender.CommandArgument.ToString.IndexOf("@")))
        'Volta sempre o Valor Zero para a camareira
        drpCamareiraAr.SelectedValue = 0
        'Gerenciando a visualização do modal extender
        pnlAlterandoAr.DataBind()
        btnChamaArrumacao_ModalPopupExtender.Show()
        pnlAlterandoAr.Focus()
        'Carregando dados na tela
        ObjAtendimentoVO = New AtendimentoGovVO
        ObjAtendimentoVO.ApaId = hddApaId.Value
        ObjAtendimentoVO.AGoId = hddAgoId.Value
        ObjAtendimentoDAO = New AtendimentoGovDAO()
        ObjAtendimentoVO = ObjAtendimentoDAO.ConsultaAtendimentoArrumacao(ObjAtendimentoVO, btnUnidade.Attributes.Item("AliasBancoTurismo").ToString)
        txtCamaCasalAr.Text = ObjAtendimentoVO.AGoCCasal
        With ObjAtendimentoVO
            'Zerando sempre os valores das caixas de textos'
            drpCamareiraAr.SelectedValue = .CamId
            txtCamaCasalAr.Text = .AGoCCasal
            txtCamaSolteiroAr.Text = .AGoCSolteiro
            txtBercoAr.Text = .AGoBerco
            txtFronhaAr.Text = .AGoTravesseiro
            txtToalhaAr.Text = .AGoJogoToalhas
            txtPapelAr.Text = .AGoRoloPapel
            txtLixoAr.Text = .AGoSacoLixo
            txtSaboneteAr.Text = .AGoSabonete
            If Convert.IsDBNull(.AGoObservacao) Then
                txtObsAr.Text = ""
            Else
                txtObsAr.Text = .AGoObservacao
            End If
            rdTapeteAr.SelectedValue = .AGoTapete
        End With
        'Mostrando sempre o ApaDesc no Label do Painel de Atendimento
        lblAptoAr.Text = hddAptoArrumacao.Value  '(Mid(sender.CommandArgument.ToString, sender.CommandArgument.ToString.IndexOf("#") + 2, sender.CommandArgument.ToString.Length - sender.CommandArgument.ToString.IndexOf("@")))
    End Sub

    Protected Sub img2AtendimentoA_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        btnChamaArrumacao_Click(sender, e)
        'Estou pegando o valor do btnEditA(command Argument) que esta Apaid#ApaDesc - retirei o Apadesc
        hddApaId.Value = CLng(Mid(sender.CommandArgument.ToString, 1, sender.CommandArgument.ToString.IndexOf("#")))
        'Pegando apenas o Apadesc'
        hddAptoArrumacao.Value = (Mid(sender.CommandArgument.ToString, sender.CommandArgument.ToString.IndexOf("#") + 2, (sender.CommandArgument.ToString.IndexOf("@")) - (sender.CommandArgument.ToString.IndexOf("#") + 1)))
        hddAgoId.Value = (Mid(sender.CommandArgument.ToString, sender.CommandArgument.ToString.IndexOf("@") + 2, sender.CommandArgument.ToString.Length - sender.CommandArgument.ToString.IndexOf("@")))
        'Volta sempre o Valor Zero para a camareira
        drpCamareiraAr.SelectedValue = 0
        'Gerenciando a visualização do modal extender
        pnlAlterandoAr.DataBind()
        btnChamaArrumacao_ModalPopupExtender.Show()
        pnlAlterandoAr.Focus()
        'Carregando dados na tela
        ObjAtendimentoVO = New AtendimentoGovVO
        ObjAtendimentoVO.ApaId = hddApaId.Value
        ObjAtendimentoVO.AGoId = hddAgoId.Value
        ObjAtendimentoDAO = New AtendimentoGovDAO()
        ObjAtendimentoVO = ObjAtendimentoDAO.ConsultaAtendimentoArrumacao(ObjAtendimentoVO, btnUnidade.Attributes.Item("AliasBancoTurismo").ToString)
        txtCamaCasalAr.Text = ObjAtendimentoVO.AGoCCasal
        With ObjAtendimentoVO
            'Zerando sempre os valores das caixas de textos'
            drpCamareiraAr.SelectedValue = .CamId
            txtCamaCasalAr.Text = .AGoCCasal
            txtCamaSolteiroAr.Text = .AGoCSolteiro
            txtBercoAr.Text = .AGoBerco
            txtFronhaAr.Text = .AGoTravesseiro
            txtToalhaAr.Text = .AGoJogoToalhas
            txtPapelAr.Text = .AGoRoloPapel
            txtLixoAr.Text = .AGoSacoLixo
            txtSaboneteAr.Text = .AGoSabonete
            If Convert.IsDBNull(.AGoObservacao) Or .AGoObservacao = "" Then
                txtObsAr.Text = " "
            Else
                txtObsAr.Text = .AGoObservacao
            End If
            rdTapeteAr.SelectedValue = .AGoTapete
        End With
        'Mostrando sempre o ApaDesc no Label do Painel de Atendimento
        lblAptoAr.Text = hddAptoArrumacao.Value
    End Sub

    Protected Sub img3AtendimentoA_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        btnChamaArrumacao_Click(sender, e)
        'Estou pegando o valor do btnEditA(command Argument) que esta Apaid#ApaDesc - retirei o Apadesc
        hddApaId.Value = CLng(Mid(sender.CommandArgument.ToString, 1, sender.CommandArgument.ToString.IndexOf("#")))
        'Pegando apenas o Apadesc'
        hddAptoArrumacao.Value = (Mid(sender.CommandArgument.ToString, sender.CommandArgument.ToString.IndexOf("#") + 2, (sender.CommandArgument.ToString.IndexOf("@")) - (sender.CommandArgument.ToString.IndexOf("#") + 1)))
        hddAgoId.Value = (Mid(sender.CommandArgument.ToString, sender.CommandArgument.ToString.IndexOf("@") + 2, sender.CommandArgument.ToString.Length - sender.CommandArgument.ToString.IndexOf("@")))
        'Volta sempre o Valor Zero para a camareira
        drpCamareiraAr.SelectedValue = 0
        'Gerenciando a visualização do modal extender
        pnlAlterandoAr.DataBind()
        btnChamaArrumacao_ModalPopupExtender.Show()
        pnlAlterandoAr.Focus()
        'Carregando dados na tela
        ObjAtendimentoVO = New AtendimentoGovVO
        ObjAtendimentoVO.ApaId = hddApaId.Value
        ObjAtendimentoVO.AGoId = hddAgoId.Value
        ObjAtendimentoDAO = New AtendimentoGovDAO()
        ObjAtendimentoVO = ObjAtendimentoDAO.ConsultaAtendimentoArrumacao(ObjAtendimentoVO, btnUnidade.Attributes.Item("AliasBancoTurismo").ToString)
        txtCamaCasalAr.Text = ObjAtendimentoVO.AGoCCasal
        With ObjAtendimentoVO
            'Zerando sempre os valores das caixas de textos'
            drpCamareiraAr.SelectedValue = .CamId
            txtCamaCasalAr.Text = .AGoCCasal
            txtCamaSolteiroAr.Text = .AGoCSolteiro
            txtBercoAr.Text = .AGoBerco
            txtFronhaAr.Text = .AGoTravesseiro
            txtToalhaAr.Text = .AGoJogoToalhas
            txtPapelAr.Text = .AGoRoloPapel
            txtLixoAr.Text = .AGoSacoLixo
            txtSaboneteAr.Text = .AGoSabonete
            If Convert.IsDBNull(.AGoObservacao) Then
                txtObsAr.Text = ""
            Else
                txtObsAr.Text = .AGoObservacao
            End If
            rdTapeteAr.SelectedValue = .AGoTapete
        End With
        'Mostrando sempre o ApaDesc no Label do Painel de Atendimento
        lblAptoAr.Text = hddAptoArrumacao.Value
    End Sub

    Protected Sub img4AtendimentoA_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        btnChamaArrumacao_Click(sender, e)
        'Estou pegando o valor do btnEditA(command Argument) que esta Apaid#ApaDesc - retirei o Apadesc
        hddApaId.Value = CLng(Mid(sender.CommandArgument.ToString, 1, sender.CommandArgument.ToString.IndexOf("#")))
        'Pegando apenas o Apadesc'
        hddAptoArrumacao.Value = (Mid(sender.CommandArgument.ToString, sender.CommandArgument.ToString.IndexOf("#") + 2, (sender.CommandArgument.ToString.IndexOf("@")) - (sender.CommandArgument.ToString.IndexOf("#") + 1)))
        hddAgoId.Value = (Mid(sender.CommandArgument.ToString, sender.CommandArgument.ToString.IndexOf("@") + 2, sender.CommandArgument.ToString.Length - sender.CommandArgument.ToString.IndexOf("@")))
        'Volta sempre o Valor Zero para a camareira
        drpCamareiraAr.SelectedValue = 0
        'Gerenciando a visualização do modal extender
        pnlAlterandoAr.DataBind()
        btnChamaArrumacao_ModalPopupExtender.Show()
        pnlAlterandoAr.Focus()
        'Carregando dados na tela
        ObjAtendimentoVO = New AtendimentoGovVO
        ObjAtendimentoVO.ApaId = hddApaId.Value
        ObjAtendimentoVO.AGoId = hddAgoId.Value
        ObjAtendimentoDAO = New AtendimentoGovDAO()
        ObjAtendimentoVO = ObjAtendimentoDAO.ConsultaAtendimentoArrumacao(ObjAtendimentoVO, btnUnidade.Attributes.Item("AliasBancoTurismo").ToString)
        txtCamaCasalAr.Text = ObjAtendimentoVO.AGoCCasal
        With ObjAtendimentoVO
            'Zerando sempre os valores das caixas de textos'
            drpCamareiraAr.SelectedValue = .CamId
            txtCamaCasalAr.Text = .AGoCCasal
            txtCamaSolteiroAr.Text = .AGoCSolteiro
            txtBercoAr.Text = .AGoBerco
            txtFronhaAr.Text = .AGoTravesseiro
            txtToalhaAr.Text = .AGoJogoToalhas
            txtPapelAr.Text = .AGoRoloPapel
            txtLixoAr.Text = .AGoSacoLixo
            txtSaboneteAr.Text = .AGoSabonete
            If Convert.IsDBNull(.AGoObservacao) Then
                txtObsAr.Text = ""
            Else
                txtObsAr.Text = .AGoObservacao
            End If
            rdTapeteAr.SelectedValue = .AGoTapete
        End With
        'Mostrando sempre o ApaDesc no Label do Painel de Atendimento
        lblAptoAr.Text = hddAptoArrumacao.Value
    End Sub

    Protected Sub brnSalvarAr_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles brnSalvarAr.Click
        'INICIANDO PROCESSO DE SALVAMENTO
        ObjAtendimentoVO = New AtendimentoGovVO
        ObjAtendimentoDAO = New AtendimentoGovDAO()
        ObjAtendimentoVO.ApaId = hddApaId.Value
        'Se o atendimento for único, teremos que procurar o agoid.. isso será feito agora
        If btnChamaA.Attributes.Item("AtendimentoUnico") = 1 Then
            ObjAtendimentoVO = ObjAtendimentoDAO.ConsultaAtendimento(ObjAtendimentoVO, btnUnidade.Attributes.Item("AliasBancoTurismo").ToString)
            hddAgoId.Value = ObjAtendimentoVO.AGoId
        End If
        ObjAtendimentoVO.AGoId = hddAgoId.Value
        ObjAtendimentoVO = ObjAtendimentoDAO.ConsultaAtendimentoArrumacao(ObjAtendimentoVO, btnUnidade.Attributes.Item("AliasBancoTurismo").ToString)
        'INSTANCIANDO UMA NOVA CLASSE E CARREGANDO OS VALORES DA LINHAS NOS ATRIBUTOS DA CLASSE
        ObjAtendimentoVO = New AtendimentoGovVO
        With ObjAtendimentoVO
            .AGoId = hddAgoId.Value
            .ApaId = hddApaId.Value
            'DANDO SEQUENCIA NO CARREGAMENTO DOS OBJETOS
            .CamId = drpCamareiraAr.SelectedValue
            .AGoCCasal = txtCamaCasalAr.Text
            .AGoCSolteiro = txtCamaSolteiroAr.Text
            .AGoBerco = txtBercoAr.Text
            .AGoTravesseiro = txtFronhaAr.Text
            .AGoJogoToalhas = txtToalhaAr.Text
            .AGoRoloPapel = txtPapelAr.Text
            .AGoSacoLixo = txtLixoAr.Text
            .AGoSabonete = txtSaboneteAr.Text
            .AGoTapete = rdTapeteAr.SelectedValue
            .AGoObservacao = txtObsAr.Text
            .Usuario = User.Identity.Name.ToString.Replace("SESC-GO.COM.BR\", "")
        End With
        ObjAtendimentoDAO = New AtendimentoGovDAO()
        Select Case ObjAtendimentoDAO.AtualizaAtendimentoArrumacao(ObjAtendimentoVO, btnUnidade.Attributes.Item("AliasBancoTurismo").ToString)
            Case 0
                hddProcessando.Value = ""
                ScriptManager.RegisterStartupScript(Page, Page.[GetType](), Guid.NewGuid().ToString(), "alert('Erro ao Salvar o registro do apartamento " & ObjAtendimentoVO.ApaId & " ');", True)
                If btnChamaA.Attributes.Item("AtendimentoUnico") = 1 Then
                    pnlStatus.Visible = True
                    btnChamaA.Attributes.Add("AtendimentoUnico", 0)
                End If
                Return
                'Case 1 : ContaApartamento = ContaApartamento + 1  'ScriptManager.RegisterStartupScript(Page, Page.[GetType](), Guid.NewGuid().ToString(), "alert('Registro efetuado com sucesso');", True)
        End Select
        varreGridArrumacao()
        If btnChamaA.Attributes.Item("AtendimentoUnico") = 1 Then
            pnlStatus.Visible = True
            btnChamaA.Attributes.Add("AtendimentoUnico", 0)
        End If
        hddProcessando.Value = ""
    End Sub
    Protected Sub gdv1OcupadoA_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gdv1OcupadoA.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            CType(e.Row.FindControl("imgApaId1"), ImageButton).CommandArgument = gdv1OcupadoA.DataKeys(e.Row.RowIndex).Item(0).ToString & "#" & gdv1OcupadoA.DataKeys(e.Row.RowIndex).Item(1).ToString
            CType(e.Row.FindControl("imgApaId1"), ImageButton).ToolTip = gdv1OcupadoA.DataKeys(e.Row.RowIndex).Item(1).ToString
        End If
    End Sub

    Protected Sub gdv2OcupadoA_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gdv2OcupadoA.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            CType(e.Row.FindControl("imgApaId2"), ImageButton).CommandArgument = gdv2OcupadoA.DataKeys(e.Row.RowIndex).Item(0).ToString & "#" & gdv2OcupadoA.DataKeys(e.Row.RowIndex).Item(1).ToString
            CType(e.Row.FindControl("imgApaId2"), ImageButton).ToolTip = gdv2OcupadoA.DataKeys(e.Row.RowIndex).Item(1).ToString
        End If
    End Sub

    Protected Sub gdv3OcupadoA_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gdv3OcupadoA.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            CType(e.Row.FindControl("imgApaId3"), ImageButton).CommandArgument = gdv3OcupadoA.DataKeys(e.Row.RowIndex).Item(0).ToString & "#" & gdv3OcupadoA.DataKeys(e.Row.RowIndex).Item(1).ToString
            CType(e.Row.FindControl("imgApaId3"), ImageButton).ToolTip = gdv3OcupadoA.DataKeys(e.Row.RowIndex).Item(1).ToString
        End If
    End Sub

    Protected Sub gdv4OcupadoA_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gdv4OcupadoA.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            CType(e.Row.FindControl("imgApaId4"), ImageButton).CommandArgument = gdv4OcupadoA.DataKeys(e.Row.RowIndex).Item(0).ToString & "#" & gdv4OcupadoA.DataKeys(e.Row.RowIndex).Item(1).ToString
            CType(e.Row.FindControl("imgApaId4"), ImageButton).ToolTip = gdv4OcupadoA.DataKeys(e.Row.RowIndex).Item(1).ToString
        End If
    End Sub

    Protected Sub imgApaId1_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        hddApaId.Value = CLng(Mid(sender.CommandArgument.ToString, 1, sender.CommandArgument.ToString.IndexOf("#")))
        'Pegando apenas o Apadesc'
        hddApaDesc.Value = (Mid(sender.CommandArgument.ToString, sender.CommandArgument.ToString.IndexOf("#") + 2, sender.CommandArgument.ToString.Length - sender.CommandArgument.ToString.IndexOf("#")))
        lblAptoArrumacao.Text = hddApaDesc.Value
        btnChamaA_Click(sender, e)
        'CType(e.Row.FindControl("imgApaId1"), ImageButton).CommandArgument = gdv1OcupadoA.DataKeys(e.Row.RowIndex).Item(0).ToString & "#" & gdv1OcupadoA.DataKeys(e.Row.RowIndex).Item(1).ToString
    End Sub

    Protected Sub imgApaId2_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        hddApaId.Value = CLng(Mid(sender.CommandArgument.ToString, 1, sender.CommandArgument.ToString.IndexOf("#")))
        'Pegando apenas o Apadesc'
        hddApaDesc.Value = (Mid(sender.CommandArgument.ToString, sender.CommandArgument.ToString.IndexOf("#") + 2, sender.CommandArgument.ToString.Length - sender.CommandArgument.ToString.IndexOf("#")))
        lblAptoArrumacao.Text = hddApaDesc.Value
        btnChamaA_Click(sender, e)
    End Sub

    Protected Sub imgApaId3_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        hddApaId.Value = CLng(Mid(sender.CommandArgument.ToString, 1, sender.CommandArgument.ToString.IndexOf("#")))
        'Pegando apenas o Apadesc'
        hddApaDesc.Value = (Mid(sender.CommandArgument.ToString, sender.CommandArgument.ToString.IndexOf("#") + 2, sender.CommandArgument.ToString.Length - sender.CommandArgument.ToString.IndexOf("#")))
        lblAptoArrumacao.Text = hddApaDesc.Value
        btnChamaA_Click(sender, e)
    End Sub

    Protected Sub imgApaId4_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        hddApaId.Value = CLng(Mid(sender.CommandArgument.ToString, 1, sender.CommandArgument.ToString.IndexOf("#")))
        'Pegando apenas o Apadesc'
        hddApaDesc.Value = (Mid(sender.CommandArgument.ToString, sender.CommandArgument.ToString.IndexOf("#") + 2, sender.CommandArgument.ToString.Length - sender.CommandArgument.ToString.IndexOf("#")))
        lblAptoArrumacao.Text = hddApaDesc.Value
        btnChamaA_Click(sender, e)
    End Sub

    Protected Sub img1LimpoA_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        btnChamaL_Click(sender, e)
        hddApaId.Value = CLng(Mid(sender.CommandArgument.ToString, 1, sender.CommandArgument.ToString.IndexOf("#")))
        'Pegando apenas o Apadesc'
        hddApaDesc.Value = (Mid(sender.CommandArgument.ToString, sender.CommandArgument.ToString.IndexOf("#") + 2, sender.CommandArgument.ToString.Length - sender.CommandArgument.ToString.IndexOf("#")))
        lblAptoL.Text = hddApaDesc.Value
        'Volta sempre o Valor Zero para a camareira
        drpCamareiraArrumacao.SelectedValue = 0
    End Sub

    Protected Sub img2LimpoA_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        btnChamaL_Click(sender, e)
        hddApaId.Value = CLng(Mid(sender.CommandArgument.ToString, 1, sender.CommandArgument.ToString.IndexOf("#")))
        'Pegando apenas o Apadesc'
        hddApaDesc.Value = (Mid(sender.CommandArgument.ToString, sender.CommandArgument.ToString.IndexOf("#") + 2, sender.CommandArgument.ToString.Length - sender.CommandArgument.ToString.IndexOf("#")))
        lblAptoL.Text = hddApaDesc.Value
        'Volta sempre o Valor Zero para a camareira
        drpCamareiraArrumacao.SelectedValue = 0
    End Sub

    Protected Sub img3LimpoA_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        btnChamaL_Click(sender, e)
        hddApaId.Value = CLng(Mid(sender.CommandArgument.ToString, 1, sender.CommandArgument.ToString.IndexOf("#")))
        'Pegando apenas o Apadesc'
        hddApaDesc.Value = (Mid(sender.CommandArgument.ToString, sender.CommandArgument.ToString.IndexOf("#") + 2, sender.CommandArgument.ToString.Length - sender.CommandArgument.ToString.IndexOf("#")))
        lblAptoL.Text = hddApaDesc.Value
        'Volta sempre o Valor Zero para a camareira
        drpCamareiraArrumacao.SelectedValue = 0
    End Sub

    Protected Sub img4LimpoA_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        btnChamaL_Click(sender, e)
        hddApaId.Value = CLng(Mid(sender.CommandArgument.ToString, 1, sender.CommandArgument.ToString.IndexOf("#")))
        'Pegando apenas o Apadesc'
        hddApaDesc.Value = (Mid(sender.CommandArgument.ToString, sender.CommandArgument.ToString.IndexOf("#") + 2, sender.CommandArgument.ToString.Length - sender.CommandArgument.ToString.IndexOf("#")))
        lblAptoL.Text = hddApaDesc.Value
        'Volta sempre o Valor Zero para a camareira
        drpCamareiraArrumacao.SelectedValue = 0
    End Sub

    Protected Sub gdv1LimpoA_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gdv1LimpoA.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            CType(e.Row.FindControl("img1LimpoA"), ImageButton).CommandArgument = gdv1LimpoA.DataKeys(e.Row.RowIndex).Item(0).ToString & "#" & gdv1LimpoA.DataKeys(e.Row.RowIndex).Item(1).ToString
            CType(e.Row.FindControl("img1LimpoA"), ImageButton).ToolTip = gdv1LimpoA.DataKeys(e.Row.RowIndex).Item(1).ToString
            CType(e.Row.FindControl("chkLimpar1"), CheckBox).Attributes.Add("ApaId", gdv1LimpoA.DataKeys(e.Row.RowIndex).Item(0).ToString)
            CType(e.Row.FindControl("chkLimpar1"), CheckBox).Attributes.Add("AgoId", gdv1LimpoA.DataKeys(e.Row.RowIndex).Item(2).ToString)
        End If
    End Sub


    Protected Sub gdv2LimpoA_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gdv2LimpoA.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            CType(e.Row.FindControl("img2LimpoA"), ImageButton).CommandArgument = gdv2LimpoA.DataKeys(e.Row.RowIndex).Item(0).ToString & "#" & gdv2LimpoA.DataKeys(e.Row.RowIndex).Item(1).ToString
            CType(e.Row.FindControl("img2LimpoA"), ImageButton).ToolTip = gdv2LimpoA.DataKeys(e.Row.RowIndex).Item(1).ToString
            CType(e.Row.FindControl("chkLimpar2"), CheckBox).Attributes.Add("ApaId", gdv2LimpoA.DataKeys(e.Row.RowIndex).Item(0).ToString)
            CType(e.Row.FindControl("chkLimpar2"), CheckBox).Attributes.Add("AgoId", gdv2LimpoA.DataKeys(e.Row.RowIndex).Item(2).ToString)
        End If
    End Sub

    Protected Sub gdv3LimpoA_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gdv3LimpoA.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            CType(e.Row.FindControl("img3LimpoA"), ImageButton).CommandArgument = gdv3LimpoA.DataKeys(e.Row.RowIndex).Item(0).ToString & "#" & gdv3LimpoA.DataKeys(e.Row.RowIndex).Item(1).ToString
            CType(e.Row.FindControl("img3LimpoA"), ImageButton).ToolTip = gdv3LimpoA.DataKeys(e.Row.RowIndex).Item(1).ToString
            CType(e.Row.FindControl("chkLimpar3"), CheckBox).Attributes.Add("ApaId", gdv3LimpoA.DataKeys(e.Row.RowIndex).Item(0).ToString)
            CType(e.Row.FindControl("chkLimpar3"), CheckBox).Attributes.Add("AgoId", gdv3LimpoA.DataKeys(e.Row.RowIndex).Item(2).ToString)
        End If
    End Sub

    Protected Sub gdv4LimpoA_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gdv4LimpoA.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            CType(e.Row.FindControl("img4LimpoA"), ImageButton).CommandArgument = gdv4LimpoA.DataKeys(e.Row.RowIndex).Item(0).ToString & "#" & gdv4LimpoA.DataKeys(e.Row.RowIndex).Item(1).ToString
            CType(e.Row.FindControl("img4LimpoA"), ImageButton).ToolTip = gdv4LimpoA.DataKeys(e.Row.RowIndex).Item(1).ToString
            CType(e.Row.FindControl("chkLimpar4"), CheckBox).Attributes.Add("ApaId", gdv4LimpoA.DataKeys(e.Row.RowIndex).Item(0).ToString)
            CType(e.Row.FindControl("chkLimpar4"), CheckBox).Attributes.Add("AgoId", gdv4LimpoA.DataKeys(e.Row.RowIndex).Item(2).ToString)
        End If
    End Sub

    Protected Sub btnVoltaLimpoA_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnVoltaLimpoA.Click
        pnlGridsLimpoA.Visible = False
        'Testa a visibilidade dos paineis e os torna visíveis
        If chkPrioridade.Checked = True Then  'chkConsultas.Items(0).Selected = True Then
            pnlPrioridade.Visible = True
        ElseIf chkArrumacao.Checked = True Then  'chkConsultas.Items(1).Selected = True Then
            pnlArrumacao.Visible = True
        ElseIf chkOcupado.Checked = True Then  'chkConsultas.Items(2).Selected = True Then
            pnlOcupado.Visible = True
        ElseIf chkManutencao.Checked = True Then 'chkConsultas.Items(3).Selected = True Then
            pnlManutencao.Visible = True
        ElseIf chkLimpo.Checked = True Then 'chkConsultas.Items(4).Selected = True Then
            pnlLimpo.Visible = True
        ElseIf chkConferencia.Checked = True Then 'chkConsultas.Items(5).Selected = True Then
            PnlConferencia.Visible = True
        End If
        pnlStatus.Visible = True
        ListaSomenteLimpos()
    End Sub

    Protected Sub btnVoltarOcupado_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnVoltarOcupado.Click
        PnlAtendimento.Visible = False
        pnlGridsOcupadoA.Visible = False
        'Testa a visibilidade dos paineis e os torna visíveis
        If chkPrioridade.Checked = True Then  'chkConsultas.Items(0).Selected = True Then
            pnlPrioridade.Visible = True
        ElseIf chkArrumacao.Checked = True Then  'chkConsultas.Items(1).Selected = True Then
            pnlArrumacao.Visible = True
        ElseIf chkOcupado.Checked = True Then  'chkConsultas.Items(2).Selected = True Then
            pnlOcupado.Visible = True
        ElseIf chkManutencao.Checked = True Then 'chkConsultas.Items(3).Selected = True Then
            pnlManutencao.Visible = True
        ElseIf chkLimpo.Checked = True Then 'chkConsultas.Items(4).Selected = True Then
            pnlLimpo.Visible = True
        ElseIf chkConferencia.Checked = True Then 'chkConsultas.Items(5).Selected = True Then
            PnlConferencia.Visible = True
        End If
        pnlStatus.Visible = True
    End Sub

    Protected Sub btnVoltarArrumacao_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnVoltarArrumacao.Click
        pnlGridArrumacao.Visible = False
        'Testa a visibilidade dos paineis e os torna visíveis
        If chkPrioridade.Checked = True Then  'chkConsultas.Items(0).Selected = True Then
            pnlPrioridade.Visible = True
        ElseIf chkArrumacao.Checked = True Then  'chkConsultas.Items(1).Selected = True Then
            pnlArrumacao.Visible = True
        ElseIf chkOcupado.Checked = True Then  'chkConsultas.Items(2).Selected = True Then
            pnlOcupado.Visible = True
        ElseIf chkManutencao.Checked = True Then 'chkConsultas.Items(3).Selected = True Then
            pnlManutencao.Visible = True
        ElseIf chkLimpo.Checked = True Then 'chkConsultas.Items(4).Selected = True Then
            pnlLimpo.Visible = True
        ElseIf chkConferencia.Checked = True Then 'chkConsultas.Items(5).Selected = True Then
            PnlConferencia.Visible = True
        End If
        pnlStatus.Visible = True
    End Sub
    Protected Sub ListaSomenteOcupados()
        Dim Federacao As String = ""
        'DEFININDO FEDERACAO OU NÃO
        Select Case txtFederacao.SelectedValue
            Case "Todos" : Federacao = ""
            Case "Sim" : Federacao = "S"
            Case "Não" : Federacao = "N"
        End Select
        '=========OCUPADOS'=========
        ObjOcupadoVO = New OcupadoVO
        ObjOcupadoDAO = New OcupadoDAO()
        Dim ListaBaseOcupado As IList
        ObjOcupadoVO.ApaDesc = txtApartamento.Text.Replace(",", " ")
        'ATIVANDO O VISIBLE DO PAINEL'
        'pnlPopOcupado.Visible = True
        ListaBaseOcupado = ObjOcupadoDAO.Consultar(ObjOcupadoVO, Federacao, btnUnidade.Attributes.Item("AliasBancoTurismo").ToString)
        Session("Ocupados") = ListaBaseOcupado
        'INFORMANDO O TOTAL DE APARTAMENTOS EM ARRUMAÇÃO'
        Dim TotAptoOcupado As Integer = ListaBaseOcupado.Count
        lblTotOcupado.Text = CStr(TotAptoOcupado)
        'CRIANDO AS LISTAS AUXILIARES'
        Dim ListaOcupado1 As IList = New ArrayList
        Dim ListaOcupado2 As IList = New ArrayList
        Dim ListaOcupado3 As IList = New ArrayList
        Dim ListaOcupado4 As IList = New ArrayList
        cont = 1
        'PERCORRER A LISATA BASE INSERINDO OS VALORES NAS LISTA AUXILIARES'
        'QUE IRÃO PREENCHER OS GRID'S
        For Each item As OcupadoVO In ListaBaseOcupado
            Select Case cont
                Case 1 : ListaOcupado1.Add(item)
                Case 2 : ListaOcupado2.Add(item)
                Case 3 : ListaOcupado3.Add(item)
                Case 4 : ListaOcupado4.Add(item)
            End Select
            If cont = 4 Then
                cont = 1
            Else
                cont += 1
            End If
        Next
        gdvOcupado1.DataSource = ListaOcupado1
        gdvOcupado1.DataBind()
        gdvOcupado2.DataSource = ListaOcupado2
        gdvOcupado2.DataBind()
        gdvOcupado3.DataSource = ListaOcupado3
        gdvOcupado3.DataBind()
        gdvOcupado4.DataSource = ListaOcupado4
        gdvOcupado4.DataBind()
        'SE EXISTIR ALGO A SER EXIBIDO O PAINEL SERÁ VISUALIADO
        If (ListaOcupado1.Count > 0 Or ListaOcupado2.Count > 0 Or ListaOcupado3.Count > 0 Or ListaOcupado4.Count > 0) Then
            pnlOcupado.Visible = True
        End If
    End Sub
    Protected Sub ListaSomenteArrumacao()
        Dim Federacao As String = ""
        'DEFININDO FEDERACAO OU NÃO
        Select Case txtFederacao.SelectedValue
            Case "Todos" : Federacao = ""
            Case "Sim" : Federacao = "S"
            Case "Não" : Federacao = "N"
        End Select
        'If chkConsultas.Items(1).Selected = True Then
        '========ARRUMAÇÃO============
        ObjArrumacaoVO = New ArrumacaoVO
        ObjArrumacaoDAO = New ArrumacaoDAO()
        Dim ListaBaseArrumacao As IList
        ObjArrumacaoVO.ApaDesc = txtApartamento.Text.Replace("'", "")
        ListaBaseArrumacao = ObjArrumacaoDAO.PesquisaArrumacao(ObjArrumacaoVO, Federacao, btnUnidade.Attributes.Item("AliasBancoTurismo").ToString)
        Session("Arrumacao") = ListaBaseArrumacao
        'INFORMANDO O TOTAL DE APARTAMENTOS EM ARRUMAÇÃO'
        Dim TotAptoArrumacao As Integer = ListaBaseArrumacao.Count
        lblTotArrumacao.Text = CStr(TotAptoArrumacao)

        'CRIANDO AS LISTAS AUXILIARES'
        Dim ListaArrumacao1 As IList = New ArrayList
        Dim ListaArrumacao2 As IList = New ArrayList
        Dim ListaArrumacao3 As IList = New ArrayList
        Dim ListaArrumacao4 As IList = New ArrayList
        Dim Cont As Byte = 1
        'PERCORRER A LISATA BASE INSERINDO OS VALORES NAS LISTA AUXILIARES'
        'QUE IRÃO PREENCHER OS GRID'S
        For Each item As ArrumacaoVO In ListaBaseArrumacao
            Select Case Cont
                Case 1 : ListaArrumacao1.Add(item)
                Case 2 : ListaArrumacao2.Add(item)
                Case 3 : ListaArrumacao3.Add(item)
                Case 4 : ListaArrumacao4.Add(item)
            End Select
            If Cont = 4 Then
                Cont = 1
            Else
                Cont += 1
            End If
        Next
        gdvArrumacao1.DataSource = ListaArrumacao1
        gdvArrumacao1.DataBind()
        gdvArrumacao2.DataSource = ListaArrumacao2
        gdvArrumacao2.DataBind()
        gdvArrumacao3.DataSource = ListaArrumacao3
        gdvArrumacao3.DataBind()
        gdvArrumacao4.DataSource = ListaArrumacao4
        gdvArrumacao4.DataBind()
        'SE EXISTIR ALGO A SER EXIBIDO O PAINEL SERÁ VISUALIADO'
        If (ListaArrumacao1.Count > 0 Or ListaArrumacao2.Count > 0 Or ListaArrumacao3.Count > 0 Or ListaArrumacao4.Count > 0) Then
            pnlArrumacao.Visible = True
        End If
        'End If
    End Sub
    Protected Sub ListaSomenteManutencao()
        Dim Federacao As String = ""
        'DEFININDO FEDERACAO OU NÃO
        Select Case txtFederacao.SelectedValue
            Case "Todos" : Federacao = ""
            Case "Sim" : Federacao = "S"
            Case "Não" : Federacao = "N"
        End Select
        '=========MANUTENÇÃO'=========
        ObjManutencaoVO = New ManutencaoVO
        ObjManutencaoDAO = New ManutencaoDAO()
        Dim ListaBaseManutencao As IList
        ObjManutencaoVO.ApaId = txtApartamento.Text.Replace("'", "")
        ListaBaseManutencao = ObjManutencaoDAO.Consultar(ObjManutencaoVO, Federacao, btnUnidade.Attributes.Item("BancoTurismoSocial").ToString, btnUnidade.Attributes.Item("AliasBancoTurismo").ToString)
        Session("Manutencao") = ListaBaseManutencao
        'INFORMANDO O TOTAL DE APARTAMENTOS EM ARRUMAÇÃO
        Dim TotAptoManutencao As Integer = ListaBaseManutencao.Count
        lblTotManutencao.Text = CStr(TotAptoManutencao)
        'CRIANDO AS LISTAS AUXILIARES
        Dim ListaManutencao1 As IList = New ArrayList
        Dim ListaManutencao2 As IList = New ArrayList
        cont = 1
        'PERCORRER A LISATA BASE INSERINDO OS VALORES NAS LISTA AUXILIARES
        'QUE IRÃO PREENCHER OS GRID'S
        For Each item As ManutencaoVO In ListaBaseManutencao
            Select Case cont
                Case 1 : ListaManutencao1.Add(item)
                Case 2 : ListaManutencao2.Add(item)
            End Select
            If cont = 2 Then
                cont = 1
            Else
                cont += 1
            End If
        Next

        gdvManutencao1.DataSource = ListaManutencao1
        gdvManutencao1.DataBind()
        gdvManutencao2.DataSource = ListaManutencao2
        gdvManutencao2.DataBind()
        'SE EXISTIR ALGO A SER EXIBIDO O PAINEL SERÁ VISUALIADO'
        If (ListaManutencao1.Count > 0 Or ListaManutencao2.Count > 0) Then
            pnlManutencao.Visible = True
        End If
    End Sub
    Protected Sub ListaSomenteLimpos()
        Dim Federacao As String = ""
        'DEFININDO FEDERACAO OU NÃO
        Select Case txtFederacao.SelectedValue
            Case "Todos" : Federacao = ""
            Case "Sim" : Federacao = "S"
            Case "Não" : Federacao = "N"
        End Select
        '=========LIMPO'=========
        ObjLimpoVO = New LimpoVO
        ObjLimpoDAO = New LimpoDAO()
        Dim ListaBaseLimpo As IList
        Dim HtmlHint As StringBuilder
        HtmlHint = New StringBuilder("")
        ObjLimpoVO.ApaId = txtApartamento.Text.Replace("'", "")
        ListaBaseLimpo = ObjLimpoDAO.Consultar(ObjLimpoVO, Federacao, btnUnidade.Attributes.Item("AliasBancoTurismo").ToString)
        Session("Limpo") = ListaBaseLimpo
        'INFORMANDO O TOTAL DE APARTAMENTOS EM ARRUMAÇÃO
        Dim TotAptoLimpo As Integer = ListaBaseLimpo.Count
        lblTotLimpo.Text = CStr(TotAptoLimpo)
        'CRIANDO AS LISTAS AUXILIARES'
        Dim ListaLimpo1 As IList = New ArrayList
        Dim ListaLimpo2 As IList = New ArrayList
        Dim ListaLimpo3 As IList = New ArrayList
        Dim ListaLimpo4 As IList = New ArrayList
        cont = 1
        'PERCORRER A LISATA BASE INSERINDO OS VALORES NAS LISTA AUXILIARES
        'QUE IRÃO PREENCHER OS GRID'S
        For Each item As LimpoVO In ListaBaseLimpo
            Select Case cont
                Case 1 : ListaLimpo1.Add(item)
                Case 2 : ListaLimpo2.Add(item)
                Case 3 : ListaLimpo3.Add(item)
                Case 4 : ListaLimpo4.Add(item)
            End Select
            If cont = 4 Then
                cont = 1
            Else
                cont += 1
            End If
        Next

        gdvLimpo1.DataSource = ListaLimpo1
        gdvLimpo1.DataBind()
        gdvLimpo2.DataSource = ListaLimpo2
        gdvLimpo2.DataBind()
        gdvLimpo3.DataSource = ListaLimpo3
        gdvLimpo3.DataBind()
        gdvLimpo4.DataSource = ListaLimpo4
        gdvLimpo4.DataBind()
        'SE EXISTIR ALGO A SER EXIBIDO O PAINEL SERÁ VISUALIADO'
        If (ListaLimpo1.Count > 0 Or ListaLimpo2.Count > 0 Or ListaLimpo3.Count > 0 Or ListaLimpo4.Count > 0) Then
            pnlLimpo.Visible = True
        End If
    End Sub
    Protected Sub ListaSomenteConferencia()
        Dim Federacao As String = ""
        'DEFININDO FEDERACAO OU NÃO
        Select Case txtFederacao.SelectedValue
            Case "Todos" : Federacao = ""
            Case "Sim" : Federacao = "S"
            Case "Não" : Federacao = "N"
        End Select
        '=========CONFERENCIA'=========
        ObjConferenciaVO = New ConferenciaVO
        ObjConferenciaDAO = New ConferenciaDAO()
        Dim ListaBaseConferencia As IList
        ObjConferenciaVO.ApaId = txtApartamento.Text.Replace("'", "")
        ListaBaseConferencia = ObjConferenciaDAO.Consultar(ObjConferenciaVO, Federacao, btnUnidade.Attributes.Item("AliasBancoTurismo").ToString)
        'INFORMANDO O TOTAL DE APARTAMENTOS EM ARRUMAÇÃO'
        Dim TotAptoConferencia As Integer
        TotAptoConferencia = ListaBaseConferencia.Count
        lblTotConferencia.Text = CStr(TotAptoConferencia)
        'CRIANDO AS LISTAS AUXILIARES
        Dim ListaConferencia1 As IList = New ArrayList
        Dim ListaConferencia2 As IList = New ArrayList
        cont = 1
        'PERCORRER A LISATA BASE INSERINDO OS VALORES NAS LISTA AUXILIARES
        'QUE IRÃO PREENCHER OS GRID'S
        For Each item As ConferenciaVO In ListaBaseConferencia
            Select Case cont
                Case 1 : ListaConferencia1.Add(item)
                Case 2 : ListaConferencia2.Add(item)
            End Select
            If cont = 2 Then
                cont = 1
            Else
                cont += 1
            End If
        Next
        gdvConferencia1.DataSource = ListaConferencia1
        gdvConferencia2.DataSource = ListaConferencia2
        If (gdvConferencia1.Rows.Count + gdvConferencia2.Rows.Count) <> ListaBaseConferencia.Count Then
            gdvConferencia1.DataBind()
            gdvConferencia2.DataBind()
        End If

        'txtApartamento.Text = ""
        'SE EXISTIR ALGO A SER EXIBIDO O PAINEL SERÁ VISUALIADO
        If (ListaConferencia1.Count > 0 Or ListaConferencia2.Count > 0) Then
            PnlConferencia.Visible = True
        End If
    End Sub
    Protected Sub ListaSomentePrioridades()
        '=========PRIORIDADE'=========
        Dim Federacao As String = ""
        'DEFININDO FEDERACAO OU NÃO
        Select Case txtFederacao.SelectedValue
            Case "Todos" : Federacao = ""
            Case "Sim" : Federacao = "S"
            Case "Não" : Federacao = "N"
        End Select
        ObjPrioridadeVO = New PrioridadeVO
        ObjPrioridadeDAO = New PrioridadeDAO()
        Dim ListaBasePrioridade As IList
        Dim HtmlHint As StringBuilder
        HtmlHint = New StringBuilder("")
        ObjPrioridadeVO.ApaId = txtApartamento.Text.Replace("'", "")
        ListaBasePrioridade = ObjPrioridadeDAO.ConsultaPrioridade(ObjPrioridadeVO, txtDataPrevisao.Text, Federacao, btnUnidade.Attributes.Item("AliasBancoTurismo").ToString)
        Session("Prioridade") = ListaBasePrioridade
        'INFORMANDO O TOTAL DE APARTAMENTOS EM ARRUMAÇÃO
        Dim TotAptoPrioridade As Integer = ListaBasePrioridade.Count
        lblTotPrioridade.Text = CStr(TotAptoPrioridade)
        If TotAptoPrioridade > 0 Then
            lblTituloPrior.Text = "Prioridades"
        Else
            lblTituloPrior.Text = "Prioridade"
        End If
        'CRIANDO AS LISTAS AUXILIARES'
        Dim ListaPrioridade1 As IList = New ArrayList
        Dim ListaPrioridade2 As IList = New ArrayList
        Dim ListaPrioridade3 As IList = New ArrayList
        Dim ListaPrioridade4 As IList = New ArrayList
        cont = 1
        'PERCORRER A LISATA BASE INSERINDO OS VALORES NAS LISTA AUXILIARES
        'QUE IRÃO PREENCHER OS GRID'S
        For Each item As PrioridadeVO In ListaBasePrioridade
            Select Case cont
                Case 1 : ListaPrioridade1.Add(item)
                Case 2 : ListaPrioridade2.Add(item)
                Case 3 : ListaPrioridade3.Add(item)
                Case 4 : ListaPrioridade4.Add(item)
            End Select
            If cont = 4 Then
                cont = 1
            Else
                cont += 1
            End If
        Next
        gdvPrioridade1.DataSource = ListaPrioridade1
        gdvPrioridade1.DataBind()
        gdvPrioridade2.DataSource = ListaPrioridade2
        gdvPrioridade2.DataBind()
        gdvPrioridade3.DataSource = ListaPrioridade3
        gdvPrioridade3.DataBind()
        gdvPrioridade4.DataSource = ListaPrioridade4
        gdvPrioridade4.DataBind()
        'SE EXISTIR ALGO A SER EXIBIDO O PAINEL SERÁ VISUALIADO'
        If (ListaPrioridade1.Count > 0 Or ListaPrioridade2.Count > 0 Or ListaPrioridade3.Count > 0 Or ListaPrioridade4.Count > 0) Then
            pnlPrioridade.Visible = True
        End If
    End Sub

    Protected Sub btnSalvarLimpo_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSalvarLimpo.Click
        'DEFININDO VALORES
        ObjAtendimentoVO = New AtendimentoGovVO
        ObjAtendimentoDAO = New AtendimentoGovDAO()
        With ObjAtendimentoVO
            .CamId = 0
            .AGoCCasal = 0
            .AGoCSolteiro = 0
            .AGoBerco = 0
            .AGoTravesseiro = 0
            .AGoJogoToalhas = 0
            .AGoRoloPapel = 0
            .AGoSacoLixo = 0
            .AGoSabonete = 0
            .AGoTapete = "N"
            'QUER DIZER QUE CLIQUEI EM REVISÃO
            If btnChamaA.Attributes.Item("AtendimentoUnico") = 1 Then
                .AGoOrigem = "R"
            Else
                .AGoOrigem = "G"
            End If
            .AGoObservacao = ""
            .Acao = "A"
            .ApaCamaExtra = 0
            .ApaBerco = 0
            .Usuario = User.Identity.Name.ToString.Replace("SESC-GO.COM.BR\", "")
            .DataLog = Now
        End With

        'PERCORRENDO O PRIMEIRO GRID E LIMPANDO OS APTOS EM ARRUMAÇÃO QUE IRÃO PARA LIMPO AUTOMATICAMENTE, SERÁ GERADO NESSE MOMENTO
        'UM ATENDIMENTOGOV COM VALORES ZERADOS, APENAS A DATA E USUÁRIO DE LOG QUE SERÃO LEVADOS
        For Each linha As GridViewRow In gdv1LimpoA.Rows
            'IRA PASSAR SOMENTE NOS APARTAMENTOS CHECADOS
            If CType(linha.FindControl("chkLimpar1"), CheckBox).Checked = True Then
                hddApaId.Value = CLng(CType(linha.FindControl("chkLimpar1"), CheckBox).Attributes.Item("ApaId"))
                hddAgoId.Value = CLng(CType(linha.FindControl("chkLimpar1"), CheckBox).Attributes.Item("AgoId"))
                ObjAtendimentoVO.AGoId = hddAgoId.Value
                ObjAtendimentoVO.ApaId = hddApaId.Value
                Select Case ObjAtendimentoDAO.Inserir(ObjAtendimentoVO, btnUnidade.Attributes.Item("AliasBancoTurismo").ToString)
                    Case 0 : ScriptManager.RegisterStartupScript(Page, Page.[GetType](), Guid.NewGuid().ToString(), "alert('Erro ao Salvar o registro do apartamento " & ObjAtendimentoVO.ApaId & " ');", True)
                    Case 1
                        CType(linha.FindControl("img1LimpoA"), ImageButton).ImageUrl = "~/images/Ok.png"
                        CType(linha.FindControl("img1LimpoA"), ImageButton).Enabled = False
                        CType(linha.FindControl("chkLimpar1"), CheckBox).Checked = False
                        CType(linha.FindControl("chkLimpar1"), CheckBox).Visible = False
                        'ContaApartamento = ContaApartamento + 1  'ScriptManager.RegisterStartupScript(Page, Page.[GetType](), Guid.NewGuid().ToString(), "alert('Registro efetuado com sucesso');", True)
                End Select
            End If
            hddProcessando.Value = ""
        Next
        'PERCORRENDO O SEGUNDO GRID E LIMPANDO OS APTOS EM ARRUMAÇÃO QUE IRÃO PARA LIMPO AUTOMATICAMENTE, SERÁ GERADO NESSE MOMENTO
        'UM ATENDIMENTOGOV COM VALORES ZERADOS, APENAS A DATA E USUÁRIO DE LOG QUE SERÃO LEVADOS
        For Each linha As GridViewRow In gdv2LimpoA.Rows
            'IRA PASSAR SOMENTE NOS APARTAMENTOS CHECADOS
            If CType(linha.FindControl("chkLimpar2"), CheckBox).Checked = True Then
                hddApaId.Value = CLng(CType(linha.FindControl("chkLimpar2"), CheckBox).Attributes.Item("ApaId"))
                hddAgoId.Value = CLng(CType(linha.FindControl("chkLimpar2"), CheckBox).Attributes.Item("AgoId"))
                ObjAtendimentoVO.AGoId = hddAgoId.Value
                ObjAtendimentoVO.ApaId = hddApaId.Value
                Select Case ObjAtendimentoDAO.Inserir(ObjAtendimentoVO, btnUnidade.Attributes.Item("AliasBancoTurismo").ToString)
                    Case 0 : ScriptManager.RegisterStartupScript(Page, Page.[GetType](), Guid.NewGuid().ToString(), "alert('Erro ao Salvar o registro do apartamento " & ObjAtendimentoVO.ApaId & " ');", True)
                    Case 1
                        CType(linha.FindControl("img2LimpoA"), ImageButton).ImageUrl = "~/images/Ok.png"
                        CType(linha.FindControl("img2LimpoA"), ImageButton).Enabled = False
                        CType(linha.FindControl("chkLimpar2"), CheckBox).Checked = False
                        CType(linha.FindControl("chkLimpar2"), CheckBox).Visible = False
                        'ContaApartamento = ContaApartamento + 1  'ScriptManager.RegisterStartupScript(Page, Page.[GetType](), Guid.NewGuid().ToString(), "alert('Registro efetuado com sucesso');", True)
                End Select
            End If
            hddProcessando.Value = ""
        Next
        'PERCORRENDO O TERCEIRO GRID E LIMPANDO OS APTOS EM ARRUMAÇÃO QUE IRÃO PARA LIMPO AUTOMATICAMENTE, SERÁ GERADO NESSE MOMENTO
        'UM ATENDIMENTOGOV COM VALORES ZERADOS, APENAS A DATA E USUÁRIO DE LOG QUE SERÃO LEVADOS
        For Each linha As GridViewRow In gdv3LimpoA.Rows
            'Ira passar somente nos apartamentos checados
            If CType(linha.FindControl("chkLimpar3"), CheckBox).Checked = True Then
                hddApaId.Value = CLng(CType(linha.FindControl("chkLimpar3"), CheckBox).Attributes.Item("ApaId"))
                hddAgoId.Value = CLng(CType(linha.FindControl("chkLimpar3"), CheckBox).Attributes.Item("AgoId"))
                ObjAtendimentoVO.AGoId = hddAgoId.Value
                ObjAtendimentoVO.ApaId = hddApaId.Value
                Select Case ObjAtendimentoDAO.Inserir(ObjAtendimentoVO, btnUnidade.Attributes.Item("AliasBancoTurismo").ToString)
                    Case 0 : ScriptManager.RegisterStartupScript(Page, Page.[GetType](), Guid.NewGuid().ToString(), "alert('Erro ao Salvar o registro do apartamento " & ObjAtendimentoVO.ApaId & " ');", True)
                    Case 1
                        CType(linha.FindControl("img3LimpoA"), ImageButton).ImageUrl = "~/images/Ok.png"
                        CType(linha.FindControl("img3LimpoA"), ImageButton).Enabled = False
                        CType(linha.FindControl("chkLimpar3"), CheckBox).Checked = False
                        CType(linha.FindControl("chkLimpar3"), CheckBox).Visible = False
                        'ContaApartamento = ContaApartamento + 1  'ScriptManager.RegisterStartupScript(Page, Page.[GetType](), Guid.NewGuid().ToString(), "alert('Registro efetuado com sucesso');", True)
                End Select
            End If
            hddProcessando.Value = ""
        Next
        'PERCORRENDO O QUARTO GRID E LIMPANDO OS APTOS EM ARRUMAÇÃO QUE IRÃO PARA LIMPO AUTOMATICAMENTE, SERÁ GERADO NESSE MOMENTO
        'UM ATENDIMENTOGOV COM VALORES ZERADOS, APENAS A DATA E USUÁRIO DE LOG QUE SERÃO LEVADOS
        For Each linha As GridViewRow In gdv4LimpoA.Rows
            'IRA PASSAR SOMENTE NOS APARTAMENTOS CHECADOS
            If CType(linha.FindControl("chkLimpar4"), CheckBox).Checked = True Then
                hddApaId.Value = CLng(CType(linha.FindControl("chkLimpar4"), CheckBox).Attributes.Item("ApaId"))
                hddAgoId.Value = CLng(CType(linha.FindControl("chkLimpar4"), CheckBox).Attributes.Item("AgoId"))
                ObjAtendimentoVO.AGoId = hddAgoId.Value
                ObjAtendimentoVO.ApaId = hddApaId.Value
                Select Case ObjAtendimentoDAO.Inserir(ObjAtendimentoVO, btnUnidade.Attributes.Item("AliasBancoTurismo").ToString)
                    Case 0
                        hddProcessando.Value = ""
                        ScriptManager.RegisterStartupScript(Page, Page.[GetType](), Guid.NewGuid().ToString(), "alert('Erro ao Salvar o registro do apartamento " & ObjAtendimentoVO.ApaId & " ');", True)
                        Return
                    Case 1
                        CType(linha.FindControl("img4LimpoA"), ImageButton).ImageUrl = "~/images/Ok.png"
                        CType(linha.FindControl("img4LimpoA"), ImageButton).Enabled = False
                        CType(linha.FindControl("chkLimpar4"), CheckBox).Checked = False
                        CType(linha.FindControl("chkLimpar4"), CheckBox).Visible = False
                        'ContaApartamento = ContaApartamento + 1  'ScriptManager.RegisterStartupScript(Page, Page.[GetType](), Guid.NewGuid().ToString(), "alert('Registro efetuado com sucesso');", True)
                End Select
            End If
            hddProcessando.Value = ""
        Next
    End Sub

    Protected Sub btnFecharIntegrante_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnFecharIntegrante.Click
        pnlIntegrantes.Visible = False
        pnlStatus.Visible = True
        pnlEmprestimosIntegrante.Visible = False
        'RECALCULANDO APENAS OS DADOS DE OCUPADO
        ListaSomenteOcupados()
    End Sub

    Protected Sub chkLimpaTodos_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkLimpaTodos.CheckedChanged
        'PASSANDO NO PRIMEIRO GRID
        If chkLimpaTodos.Checked = True Then
            For Each linha As GridViewRow In gdvArrumacao1.Rows
                CType(linha.FindControl("chkLimpo1"), CheckBox).Checked = True
            Next
            'PASSANDO NO SEGUNDO GRID
            For Each linha As GridViewRow In gdvArrumacao2.Rows
                CType(linha.FindControl("chkLimpo2"), CheckBox).Checked = True
            Next
            'PASSANDO NO TERCEIRO GRID
            For Each linha As GridViewRow In gdvArrumacao3.Rows
                CType(linha.FindControl("chkLimpo3"), CheckBox).Checked = True
            Next
            'PASSANDO NO QUARTO GRID
            For Each linha As GridViewRow In gdvArrumacao4.Rows
                CType(linha.FindControl("chkLimpo4"), CheckBox).Checked = True
            Next
        Else
            For Each linha As GridViewRow In gdvArrumacao1.Rows
                CType(linha.FindControl("chkLimpo1"), CheckBox).Checked = False
            Next
            'PASSANDO NO SEGUNDO GRID
            For Each linha As GridViewRow In gdvArrumacao2.Rows
                CType(linha.FindControl("chkLimpo2"), CheckBox).Checked = False
            Next
            'PASSANDO NO TERCEIRO GRID
            For Each linha As GridViewRow In gdvArrumacao3.Rows
                CType(linha.FindControl("chkLimpo3"), CheckBox).Checked = False
            Next
            'PASSANDO NO QUARTO GRID
            For Each linha As GridViewRow In gdvArrumacao4.Rows
                CType(linha.FindControl("chkLimpo4"), CheckBox).Checked = False
            Next
        End If
    End Sub

    Protected Sub btnLimpar_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnLimpar.Click
        'DEFININDO VALORES
        ObjAtendimentoVO = New AtendimentoGovVO
        ObjAtendimentoDAO = New AtendimentoGovDAO()
        With ObjAtendimentoVO
            .CamId = 0
            .AGoCCasal = 0
            .AGoCSolteiro = 0
            .AGoBerco = 0
            .AGoTravesseiro = 0
            .AGoJogoToalhas = 0
            .AGoRoloPapel = 0
            .AGoSacoLixo = 0
            .AGoSabonete = 0
            .AGoTapete = "N"
            'QUER DIZER QUE CLIQUEI EM REVISÃO
            If btnChamaA.Attributes.Item("AtendimentoUnico") = 1 Then
                .AGoOrigem = "R"
            Else
                .AGoOrigem = "G"
            End If
            .AGoObservacao = ""
            .Acao = "A"
            .ApaCamaExtra = 0
            .ApaBerco = 0
            .Usuario = User.Identity.Name.ToString.Replace("SESC-GO.COM.BR\", "")
            .DataLog = Now
        End With

        'PERCORRENDO O PRIMEIRO GRID E LIMPANDO OS APTOS EM ARRUMAÇÃO QUE IRÃO PARA LIMPO AUTOMATICAMENTE, SERÁ GERADO NESSE MOMENTO
        'UM ATENDIMENTOGOV COM VALORES ZERADOS, APENAS A DATA E USUÁRIO DE LOG QUE SERÃO LEVADOS
        For Each linha As GridViewRow In gdvArrumacao1.Rows
            'IRA PASSAR SOMENTE NOS APARTAMENTOS CHECADOS
            If CType(linha.FindControl("chkLimpo1"), CheckBox).Checked = True Then
                hddApaId.Value = CLng(CType(linha.FindControl("chkLimpo1"), CheckBox).Attributes.Item("ApaId"))
                hddAgoId.Value = CLng(CType(linha.FindControl("chkLimpo1"), CheckBox).Attributes.Item("AgoId"))
                ObjAtendimentoVO.AGoId = hddAgoId.Value
                ObjAtendimentoVO.ApaId = hddApaId.Value
                Select Case ObjAtendimentoDAO.Inserir(ObjAtendimentoVO, btnUnidade.Attributes.Item("AliasBancoTurismo").ToString)
                    Case 0 : ScriptManager.RegisterStartupScript(Page, Page.[GetType](), Guid.NewGuid().ToString(), "alert('Erro ao Salvar o registro do apartamento " & ObjAtendimentoVO.ApaId & " ');", True)
                    Case 1
                        'CType(linha.FindControl("img1LimpoA"), ImageButton).ImageUrl = "~/images/Ok.png"
                        'CType(linha.FindControl("img1LimpoA"), ImageButton).Enabled = False
                        ''ContaApartamento = ContaApartamento + 1  'ScriptManager.RegisterStartupScript(Page, Page.[GetType](), Guid.NewGuid().ToString(), "alert('Registro efetuado com sucesso');", True)
                End Select
            End If
            hddProcessando.Value = ""
        Next
        'PERCORRENDO O SEGUNDO GRID E LIMPANDO OS APTOS EM ARRUMAÇÃO QUE IRÃO PARA LIMPO AUTOMATICAMENTE, SERÁ GERADO NESSE MOMENTO
        'UM ATENDIMENTOGOV COM VALORES ZERADOS, APENAS A DATA E USUÁRIO DE LOG QUE SERÃO LEVADOS
        For Each linha As GridViewRow In gdvArrumacao2.Rows
            'Ira passar somente nos apartamentos checados
            If CType(linha.FindControl("chkLimpo2"), CheckBox).Checked = True Then
                hddApaId.Value = CLng(CType(linha.FindControl("chkLimpo2"), CheckBox).Attributes.Item("ApaId"))
                hddAgoId.Value = CLng(CType(linha.FindControl("chkLimpo2"), CheckBox).Attributes.Item("AgoId"))
                ObjAtendimentoVO.AGoId = hddAgoId.Value
                ObjAtendimentoVO.ApaId = hddApaId.Value
                Select Case ObjAtendimentoDAO.Inserir(ObjAtendimentoVO, btnUnidade.Attributes.Item("AliasBancoTurismo").ToString)
                    Case 0 : ScriptManager.RegisterStartupScript(Page, Page.[GetType](), Guid.NewGuid().ToString(), "alert('Erro ao Salvar o registro do apartamento " & ObjAtendimentoVO.ApaId & " ');", True)
                    Case 1
                        'CType(linha.FindControl("img2LimpoA"), ImageButton).ImageUrl = "~/images/Ok.png"
                        'CType(linha.FindControl("img2LimpoA"), ImageButton).Enabled = False
                        'ContaApartamento = ContaApartamento + 1  'ScriptManager.RegisterStartupScript(Page, Page.[GetType](), Guid.NewGuid().ToString(), "alert('Registro efetuado com sucesso');", True)
                End Select
            End If
            hddProcessando.Value = ""
        Next
        'PERCORRENDO O TERCEIRO GRID E LIMPANDO OS APTOS EM ARRUMAÇÃO QUE IRÃO PARA LIMPO AUTOMATICAMENTE, SERÁ GERADO NESSE MOMENTO
        'UM ATENDIMENTOGOV COM VALORES ZERADOS, APENAS A DATA E USUÁRIO DE LOG QUE SERÃO LEVADOS
        For Each linha As GridViewRow In gdvArrumacao3.Rows
            'IRA PASSAR SOMENTE NOS APARTAMENTOS CHECADOS
            If CType(linha.FindControl("chkLimpo3"), CheckBox).Checked = True Then
                hddApaId.Value = CLng(CType(linha.FindControl("chkLimpo3"), CheckBox).Attributes.Item("ApaId"))
                hddAgoId.Value = CLng(CType(linha.FindControl("chkLimpo3"), CheckBox).Attributes.Item("AgoId"))
                ObjAtendimentoVO.AGoId = hddAgoId.Value
                ObjAtendimentoVO.ApaId = hddApaId.Value
                Select Case ObjAtendimentoDAO.Inserir(ObjAtendimentoVO, btnUnidade.Attributes.Item("AliasBancoTurismo").ToString)
                    Case 0 : ScriptManager.RegisterStartupScript(Page, Page.[GetType](), Guid.NewGuid().ToString(), "alert('Erro ao Salvar o registro do apartamento " & ObjAtendimentoVO.ApaId & " ');", True)
                    Case 1
                        'CType(linha.FindControl("img3LimpoA"), ImageButton).ImageUrl = "~/images/Ok.png"
                        'CType(linha.FindControl("img3LimpoA"), ImageButton).Enabled = False
                        'ContaApartamento = ContaApartamento + 1  'ScriptManager.RegisterStartupScript(Page, Page.[GetType](), Guid.NewGuid().ToString(), "alert('Registro efetuado com sucesso');", True)
                End Select
            End If
            hddProcessando.Value = ""
        Next
        'PERCORRENDO O QUARTO GRID E LIMPANDO OS APTOS EM ARRUMAÇÃO QUE IRÃO PARA LIMPO AUTOMATICAMENTE, SERÁ GERADO NESSE MOMENTO
        'UM ATENDIMENTOGOV COM VALORES ZERADOS, APENAS A DATA E USUÁRIO DE LOG QUE SERÃO LEVADOS
        For Each linha As GridViewRow In gdvArrumacao4.Rows
            'IRA PASSAR SOMENTE NOS APARTAMENTOS CHECADOS
            If CType(linha.FindControl("chkLimpo4"), CheckBox).Checked = True Then
                hddApaId.Value = CLng(CType(linha.FindControl("chkLimpo4"), CheckBox).Attributes.Item("ApaId"))
                hddAgoId.Value = CLng(CType(linha.FindControl("chkLimpo4"), CheckBox).Attributes.Item("AgoId"))
                ObjAtendimentoVO.AGoId = hddAgoId.Value
                ObjAtendimentoVO.ApaId = hddApaId.Value
                Select Case ObjAtendimentoDAO.Inserir(ObjAtendimentoVO, btnUnidade.Attributes.Item("AliasBancoTurismo").ToString)
                    Case 0 : ScriptManager.RegisterStartupScript(Page, Page.[GetType](), Guid.NewGuid().ToString(), "alert('Erro ao Salvar o registro do apartamento " & ObjAtendimentoVO.ApaId & " ');", True)
                    Case 1
                        'CType(linha.FindControl("img4LimpoA"), ImageButton).ImageUrl = "~/images/Ok.png"
                        'CType(linha.FindControl("img4LimpoA"), ImageButton).Enabled = False
                        'ContaApartamento = ContaApartamento + 1  'ScriptManager.RegisterStartupScript(Page, Page.[GetType](), Guid.NewGuid().ToString(), "alert('Registro efetuado com sucesso');", True)
                End Select
            End If
            hddProcessando.Value = ""
        Next
        If chkArrumacao.Checked = True Then
            ListaSomenteArrumacao()
        End If
        If chkLimpo.Checked = True Then
            ListaSomenteLimpos()
        End If
        'SEMPRE IRÁ ATUALIZAR AS PRIORIDADES'
        ListaSomentePrioridades()
    End Sub


    'Private Function PreencheObjeto() As SolicitacaoVO
    '    'BUSCANDO O CENTRO DE CUSTO DO FUNCIONARIO NA TABELA TBFUNCIONARIOS NO SISTEMA DE RESTAURANTE DOS SERVIDORES'
    '    ObjFuncionarioVO = New FuncionarioVO
    '    ObjFuncionarioDAO = New FuncionarioDAO()
    '    ObjFuncionarioVO.Matricula = btnAuxilarMan.Attributes.Item("MatriculaAd") 'Session("MatriculaAD") 'Matricula que veio do AD'
    '    ObjFuncionarioVO = ObjFuncionarioDAO.ConsultarFuncMatricula(ObjFuncionarioVO, btnUnidade.Attributes.Item("UOP").ToString)
    '    'Testa a Matricula e se estiver <=4 não deixará prossseguir
    '    If (ObjFuncionarioVO.Matricula.Length <= 4) Then
    '        ScriptManager.RegisterStartupScript(Page, Page.[GetType](), Guid.NewGuid().ToString(), "alert('A matrícula do funcinário é inválida, informe o centro de informática.');", True)

    '    End If
    '    'SE FOR ALGUEM QUE VEIO DE OUTRA UNIDADE E FICOU NO RESTAURANTE '
    '    'If Session("CentroCustoNutricao") = "146" Then 'Restaurante'
    '    '    ObjSolicitacaoVO.CentroCustoSolicitante = 146
    '    'Else
    '    'End If
    '    ObjApartamentosVO = New ApartamentosVO
    '    ObjApartamentosDAO = New ApartamentosDAO(ObjApartamentosVO)
    '    ObjSolicitacaoVO = New SolicitacaoVO
    '    ObjSolicitacaoDAO = New SolicitacaoDAO(ObjSolicitacaoVO)
    '    'ESSE CASO É PRA ATENDER OS APTOS DO KILZER QUE POSSUI DIVERGENCIA ENTRE APAID E APADESC COM O DB2, SÓ TERÁ EFEITO NA BUSCA NO DB2'
    '    If Trim(hddApaCCusto.Value) = "06.11.01" Then
    '        Select Case hddApaArea.Value  'txtSubLocalizacao.SelectedValue
    '            Case 77 : ObjSolicitacaoVO.Area = 60
    '            Case 70 : ObjSolicitacaoVO.Area = 77
    '            Case 69 : ObjSolicitacaoVO.Area = 70
    '            Case 68 : ObjSolicitacaoVO.Area = 69
    '            Case 67 : ObjSolicitacaoVO.Area = 68
    '            Case 66 : ObjSolicitacaoVO.Area = 67
    '            Case 65 : ObjSolicitacaoVO.Area = 66
    '            Case 64 : ObjSolicitacaoVO.Area = 65
    '            Case 63 : ObjSolicitacaoVO.Area = 64
    '            Case 62 : ObjSolicitacaoVO.Area = 63
    '            Case 61 : ObjSolicitacaoVO.Area = 62
    '            Case 60 : ObjSolicitacaoVO.Area = 61
    '            Case Else
    '                'CASO CONTRARIO CONTINUARA COMO A ÁREA PEGO NO INICIO
    '                ObjSolicitacaoVO.Area = hddApaArea.Value
    '        End Select
    '    Else
    '        ObjSolicitacaoVO.Area = hddApaArea.Value
    '    End If
    '    ObjSolicitacaoVO = ObjSolicitacaoDAO.BuscaInformacoesAptoBDProd(hddApaCCusto.Value, ObjSolicitacaoVO.Area)
    '    ObjSolicitacaoVO.CentroCustoSolicitante = Trim(ObjFuncionarioVO.CentroCusto)
    '    'CONTINUA CARREGANDO OS DADOS DA SOLICITACAO'
    '    ObjSolicitacaoVO.DataHoraSolicitacao = Date.Today
    '    ObjSolicitacaoVO.DataLog = Date.Today
    '    ObjSolicitacaoVO.Situacao = "E" 'Em andamento
    '    ObjSolicitacaoVO.Patrimonio = Trim(Mid(txtBem.SelectedItem.Text, 1, 6)) 'Pegando apenas o número do patrimônio'
    '    ObjSolicitacaoVO.DesBem = Trim(Mid(txtBem.SelectedItem.Text, 9, 58)).Replace("""", "")  'Pegando apenas a descriçao do bem para conserto'
    '    ObjSolicitacaoVO.UsuarioSolicitante = User.Identity.Name.ToString.Replace("SESC-GO.COM.BR\", "")
    '    ObjSolicitacaoVO.DisplayNameSolicitante = btnAuxilarMan.Attributes.Item("Usuario") 'Session("usuario")
    '    If Request.UserHostAddress = "127.0.0.1" Then
    '        ObjSolicitacaoVO.IpUnidade = "10.102.100.23"
    '    Else
    '        ObjSolicitacaoVO.IpUnidade = Request.UserHostAddress
    '    End If
    '    'ESSES VALORES FORAM PEGOS NA CONSULTA ACIMA POR ISSO DESPENSA INSERÇÃO
    '    'ObjSolicitacaoVO.LocDescricao = Trim(txtLocalizacao.SelectedItem.Text).Replace("""", "").Replace("'", "")
    '    'ObjSolicitacaoVO.CentroCusto = Trim(txtLocalizacao.SelectedValue)
    '    'ObjSolicitacaoVO.DescSubLoc = Trim(txtSubLocalizacao.SelectedItem.Text).Replace("""", "").Replace("'", "")
    '    'ObjSolicitacaoVO.Area = Trim(txtSubLocalizacao.SelectedValue)
    '    'DEVOLVENDO O APAREA ANTIGO PARA FICAR CORRETO NO HDESK'
    '    ObjSolicitacaoVO.Area = hddApaArea.Value
    '    If chkBloquearApto.Checked = True Then
    '        ObjSolicitacaoVO.BloqueioApartamento = "S"
    '    Else
    '        ObjSolicitacaoVO.BloqueioApartamento = "N"
    '    End If
    '    ObjSolicitacaoVO.Assunto = txtAssuntoManutencao.Text.Replace("'", "").Replace("""", "").ToUpper
    '    ObjSolicitacaoVO.Descricao = txtDescricaoManutencao.Text.Replace("'", "").Replace("""", "").ToUpper
    '    ObjSolicitacaoVO.UsuarioLog = User.Identity.Name.ToString.Replace("SESC-GO.COM.BR\", "")
    '    If chkBloquearApto.Checked = True Then
    '        ObjSolicitacaoVO.PrevisaoAtendimento = Format(DateAdd(DateInterval.Day, 1, Now), "yyyy-MM-dd HH:mm:ss")
    '    Else
    '        ObjSolicitacaoVO.PrevisaoAtendimento = Format(CDate("1900-01-01"), "yyyy-MM-dd")
    '    End If
    '    'ObjSolicitacaoVO.PrevisaoAtendimento = Format(CDate("1900-01-01"), "yyyy-MM-dd")
    '    ObjSolicitacaoVO.DataAtendimento = Format(CDate("1900-01-01"), "yyyy-MM-dd")
    '    ObjSolicitacaoVO.NomeFuncAtendimento = " "
    '    ObjSolicitacaoVO.MatriculaAtendimento = "0"
    '    ObjSolicitacaoVO.ObsManutencao = " "
    '    ObjSolicitacaoVO.GrauPrioridade = "E"
    '    ObjSolicitacaoVO.Avaliacao = "0" 'Sem avaliacao'
    '    ObjSolicitacaoVO.Devolucao = " "
    '    ObjSolicitacaoVO.SetorExecutante = drpSetor.SelectedValue
    '    Return ObjSolicitacaoVO
    'End Function
    Protected Sub btnAuxilarMan_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAuxilarMan.Click
        'Esse código foi adicionado devido a unificação da base de dados do Help Desk, após implantar em Piri retirar esse código.
        If btnUnidade.Attributes.Item("AliasBancoTurismo") = "TurismoSocialPiri" Then
            ScriptManager.RegisterStartupScript(Page, Page.[GetType](), Guid.NewGuid().ToString(), "alert('Atenção! Esse serviço esta desativado para a unidade de Pirenópolis. Abra o chamado pelo sistema de Help Desk.').');", True)
            hddProcessando.Value = ""
            Return
        End If

        txtBem.DataTextField = "" 'Limpando para insercoes'
        ObjBemVO = New BemVO
        ObjBemDAO = New BemDAO
        txtBem.DataTextField = "NomeBem"
        txtBem.DataValueField = "Patrimonio"
        'VARIAVEIS PARA APLICAÇÃO DO FILTRO'
        Dim CentroCusto As String = hddApaCCusto.Value
        Dim Area As String = hddApaArea.Value
        txtBem.DataSource = ObjBemDAO.PesquisaBem(Area, CentroCusto)
        txtBem.DataBind()
        'VERIFICA SE EXISTE ALGUM ITEM A SER SELECIONADO'
        If txtBem.Items.Count = 0 Then
            txtBem.Enabled = False
            txtBem.Items.Insert(0, New ListItem("000000 - NÃO POSSUI BENS CADASTRADOS", "0"))
            'If chkBloqueiaApto.Visible = False Then
            '    scpSolicitacao.SetFocus(txtAssunto)
            'End If
        Else
            txtBem.Enabled = True
            txtBem.Items.Insert(0, New ListItem("Selecione...", "0"))
            'If chkBloqueiaApto.Visible = False Then
            '    scpSolicitacao.SetFocus(txtBem)
            'End If
        End If
        btnChamaManutencao_ModalPopupExtender.Show()
    End Sub

    Protected Sub btnChamaManutencao_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnChamaManutencao.Click
        'Gerenciando a visualização do modal extender
        pnlManutencao.DataBind()
        btnChamaManutencao_ModalPopupExtender.Show()
        pnlManutencao.Focus()
    End Sub

    Protected Sub lnkManutencaoLimpo_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lnkManutencaoLimpo.Click
        txtAssuntoManutencao.Text = ""
        btnAuxilarMan_Click(sender, e)
        btnChamaManutencao_Click(sender, e)
        ListaSomenteManutencao()
        txtAssuntoManutencao.Focus()
    End Sub

    Protected Sub lnkManutencaoMan_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lnkManutencaoMan.Click
        txtAssuntoManutencao.Text = ""
        btnAuxilarMan_Click(sender, e)
        btnChamaManutencao_Click(sender, e)
        ListaSomenteManutencao()
        txtAssuntoManutencao.Focus()
    End Sub
    Protected Sub imgApagarEmp_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        ObjEmprestimosVO = New EmprestimosVO
        Dim CSEID As String = sender.CommandArgument.ToString 'ID do consumo de serviço
        ObjEmprestimosVO.IntId = hddIntId.Value
        ObjEmprestimosVO.CseId = CSEID
        ObjEmprestimosVO.EmpUsuario = User.Identity.Name.ToString.Replace("SESC-GO.COM.BR\", "")
        ObjEmprestimosVO.EmpOperacao = "E" 'Para exluir o empréstimo'
        ObjEmprestimosDAO = New EmprestimosDAO()
        Select Case ObjEmprestimosDAO.ApagaEmprestimo(ObjEmprestimosVO, 0, 0, btnUnidade.Attributes.Item("AliasBancoTurismo").ToString)
            Case 0 : ScriptManager.RegisterStartupScript(Page, Page.[GetType](), Guid.NewGuid().ToString(), "alert('Houve um erro ao salvar os dados.');", True)
            Case 1
                pnlEmprestimosIntegrante.Visible = False
                ScriptManager.RegisterStartupScript(Page, Page.[GetType](), Guid.NewGuid().ToString(), "alert('Apagado com sucesso!');", True)
                lnkIntegrantesOcupado_Click(sender, e)
        End Select
        hddProcessando.Value = ""
    End Sub

    Protected Sub btnAtualizaConf_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btnAtualizaConf.Click
        ListaSomenteConferencia()
    End Sub

    Protected Sub chkBemPatrimonial_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkBemPatrimonial.CheckedChanged
        If (chkBemPatrimonial.Checked = True) Then
            btnCadItemConserto.Visible = False
            'Pega a lista no DB2 caso contrario pega a lista que foi cadastrada pelo setor responsavel aqui no proprio sistema'
            txtBem.DataTextField = "" 'Limpando para insercoes'
            ObjBemVO = New BemVO
            ObjBemDAO = New BemDAO
            txtBem.DataTextField = "NomeBem"
            txtBem.DataValueField = "Patrimonio"
            'Variaveis para aplicação do filtro'
            Dim CentroCusto As String = hddApaCCusto.Value
            Dim Area As String = hddApaArea.Value
            txtBem.DataSource = ObjBemDAO.PesquisaBem(hddApaArea.Value, hddApaCCusto.Value)
            txtBem.DataBind()
            'Se o bem for um cadastrado na hora aparece o botão com a opção do cadastro caso contrario pega no DB2'
            'Verifica se existe algum item a ser selecionado'
            If txtBem.Items.Count = 0 Then
                txtBem.Enabled = False
                txtBem.Items.Insert(0, New ListItem("000000 - NÃO POSSUI BENS CADASTRADOS", "0"))
            Else
                txtBem.Enabled = True
                txtBem.Items.Insert(0, New ListItem("Selecione...", "0"))
            End If
        Else
            btnCadItemConserto.Visible = True
            txtBem.DataTextField = "" 'Limpando para insercoes'
            ObjItemConsertoVO = New ItemConsertoVO
            txtBem.DataTextField = "Descricao"
            txtBem.DataValueField = "Codigo"
            ObjItemConsertoVO.SetorExecutante = drpSetor.SelectedValue
            ObjItemConsertoDAO = New ItemConsertoDAO()
            ObjItemConsertoVO.IdUnidade = btnUnidade.Attributes.Item("IdUnidade")
            'Buscando itens'
            txtBem.DataSource = ObjItemConsertoDAO.Consultar(ObjItemConsertoVO, btnUnidade.Attributes.Item("AliasBancoHdManutencao").ToString)
            txtBem.DataBind()
            'Verifica se existe algum item a ser selecionado'
            If txtBem.Items.Count = 0 Then
                txtBem.Enabled = False
                txtBem.Items.Insert(0, New ListItem("000000 - NÃO POSSUI BENS CADASTRADOS", "0"))
                'scpSolicitacao.SetFocus(txtAssunto)
            Else
                txtBem.Enabled = True
                txtBem.Items.Insert(0, New ListItem("Selecione...", "0"))
            End If
        End If
        txtBem.Focus()
        btnChamaManutencao_ModalPopupExtender.Show()
    End Sub

    Protected Sub btnConfirmarItemConserto_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnConfirmarItemConserto.Click
        Try
            If (txtDescricaoItemConserto.Text = "") Then
                ScriptManager.RegisterStartupScript(Page, Page.[GetType](), Guid.NewGuid().ToString(), "alert('Informe a descrição antes de salvar!');", True)
                Return
            End If
            ObjItemConsertoVO = New ItemConsertoVO
            ObjItemConsertoDAO = New ItemConsertoDAO()
            'ObjItemConsertoVO.Codigo = hddCodigo.Value
            'Carregando os dados na tela
            ObjItemConsertoVO.DataHora = Now

            'Gravando o Id da unidade na base
            ObjItemConsertoVO.IdUnidade = btnUnidade.Attributes.Item("IdUnidade")

            ObjItemConsertoVO.Descricao = txtDescricaoItemConserto.Text.ToUpper
            ObjItemConsertoVO.SetorExecutante = drpSetor.SelectedValue

            ObjItemConsertoVO.Usuario = User.Identity.Name.ToString.Replace("SESC-GO.COM.BR\", "")
            Select Case (ObjItemConsertoDAO.Inserir(ObjItemConsertoVO, btnUnidade.Attributes.Item("AliasBancoHdManutencao").ToString))
                Case 1 'Inserido com sucesso
                    pnlCadastroItemConserto.Visible = False
                    'Inserindo bens não patrimonial
                    txtBem.DataTextField = "" 'Limpando para insercoes'
                    ObjItemConsertoVO = New ItemConsertoVO
                    txtBem.DataTextField = "Descricao"
                    txtBem.DataValueField = "Codigo"
                    ObjItemConsertoVO.SetorExecutante = drpSetor.SelectedValue
                    ObjItemConsertoDAO = New ItemConsertoDAO()
                    ObjItemConsertoVO.IdUnidade = btnUnidade.Attributes.Item("IdUnidade")
                    'Buscando itens'
                    txtBem.DataSource = ObjItemConsertoDAO.Consultar(ObjItemConsertoVO, btnUnidade.Attributes.Item("AliasBancoHdManutencao").ToString)
                    txtBem.DataBind()
                    'Verifica se existe algum item a ser selecionado'
                    If txtBem.Items.Count = 0 Then
                        txtBem.Enabled = False
                        txtBem.Items.Insert(0, New ListItem("000000 - NÃO POSSUI BENS CADASTRADOS", "0"))
                        'scpSolicitacao.SetFocus(txtAssunto)
                    Else
                        txtBem.Enabled = True
                        txtBem.Items.Insert(0, New ListItem("Selecione...", "0"))
                    End If
                    txtBem.Focus()
                Case 2
                    pnlCadastroItemConserto.Visible = False
                    'Inserindo bens não patrimonial
                    txtBem.DataTextField = "" 'Limpando para insercoes'
                    ObjItemConsertoVO = New ItemConsertoVO
                    txtBem.DataTextField = "Descricao"
                    txtBem.DataValueField = "Codigo"
                    ObjItemConsertoVO.SetorExecutante = drpSetor.SelectedValue
                    ObjItemConsertoDAO = New ItemConsertoDAO()
                    ObjItemConsertoVO.IdUnidade = btnUnidade.Attributes.Item("IdUnidade")
                    'Buscando itens'
                    txtBem.DataSource = ObjItemConsertoDAO.Consultar(ObjItemConsertoVO, btnUnidade.Attributes.Item("AliasBancoHdManutencao").ToString)
                    txtBem.DataBind()
                    'Verifica se existe algum item a ser selecionado'
                    If txtBem.Items.Count = 0 Then
                        txtBem.Enabled = False
                        txtBem.Items.Insert(0, New ListItem("000000 - NÃO POSSUI BENS CADASTRADOS", "0"))
                        'scpSolicitacao.SetFocus(txtAssunto)
                    Else
                        txtBem.Enabled = True
                        txtBem.Items.Insert(0, New ListItem("Selecione...", "0"))
                    End If
                    txtBem.Focus()
                Case 0
                    pnlCadastroItemConserto.Visible = False
                    'gdvItensConserto.Visible = False
                    ScriptManager.RegisterStartupScript(Page, Page.[GetType](), Guid.NewGuid().ToString(), "alert('Erro ao salvar! Informe ao Centro de Informática.');", True)
                    'scpGeral.SetFocus(txtPesquisa)
            End Select
            btnConfirmarMan.Enabled = True
            btnCancelarMan.Enabled = True
            txtBem.Enabled = True
            pnlCadastroItemConserto.Visible = False


            btnChamaManutencao_ModalPopupExtender.Show()
        Catch ex As Exception
            Throw New Exception(ex.StackTrace.ToString.Replace(Chr(13), ""))
        End Try
    End Sub
    Protected Sub btnCadItemConserto_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btnCadItemConserto.Click
        pnlCadastroItemConserto.Visible = True
        btnConfirmarMan.Enabled = False
        btnCancelarMan.Enabled = False
        txtDescricaoItemConserto.Focus()
        txtBem.Enabled = False
        btnChamaManutencao_ModalPopupExtender.Show()
    End Sub

    Protected Sub btnCancelarItemConserto_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancelarItemConserto.Click
        pnlCadastroItemConserto.Visible = False
        btnConfirmarMan.Enabled = True
        btnCancelarMan.Enabled = True
        txtBem.Enabled = True
        txtBem.Focus()
        btnChamaManutencao_ModalPopupExtender.Show()
    End Sub
    Public Function StringTamanhoFixo(ByVal Value As String, ByVal intTamanho As Integer) As String
        Dim strNewValue As String = ""
        If String.IsNullOrEmpty(Value) Then
            strNewValue = Space(intTamanho)
        ElseIf Microsoft.VisualBasic.Strings.Len(Value) > intTamanho Then
            strNewValue = Microsoft.VisualBasic.Strings.Left(Value, intTamanho)
        Else
            strNewValue = Microsoft.VisualBasic.Strings.Left(Value, intTamanho) & _
                        Space(intTamanho - Microsoft.VisualBasic.Strings.Len(Value))
        End If
        Return strNewValue
    End Function

    Protected Sub lnkLimpezaA_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lnkLimpezaL.Click, lnkLimpezaO.Click, lnkLimpezaA.Click, lnkLimpezaM.Click
        ObjLimpoDAO = New LimpoDAO
        ObjLimpoVO = New LimpoVO
        ObjLimpoVO.ApaId = hddApaId.Value
        gdvQuemLimpouA.DataSource = ObjLimpoDAO.QuemLimpouApto(ObjLimpoVO.ApaId, btnUnidade.Attributes.Item("AliasBancoTurismo"))
        gdvQuemLimpouA.DataBind()
        lblAptoLiberado.Text = "Apartamento: " & hddApaDesc.Value
        btnQuemLimpou_ModalPopupExtender.Show()
    End Sub

    Protected Sub btnQuemLimpou_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnQuemLimpou.Click
        'Gerenciando a visualização do modal extender
        pnlQuemLimpou.DataBind()
        btnQuemLimpou_ModalPopupExtender.Show()
        pnlQuemLimpou.Focus()
    End Sub

    Protected Sub lnkEnxoval_Click(sender As Object, e As EventArgs)
        'lnkEnxoval
        Dim Link As LinkButton = sender
        Dim row As GridViewRow = Link.NamingContainer 'Dim index As Integer = row.RowIndex
        Dim intPasId = gdvCompletarEnxoval1.DataKeys(row.RowIndex()).Item("resId").ToString

    End Sub

    Protected Sub gdvCompletarEnxoval_RowDataBound(sender As Object, e As GridViewRowEventArgs) Handles gdvCompletarEnxoval1.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            CType(e.Row.FindControl("lblAptoEnxoval1"), Label).Attributes.Add("onClick", "javascript:aspnetForm.ctl00_conPlaHolTurismoSocial_hddResId.value = '" & gdvCompletarEnxoval1.DataKeys(e.Row.RowIndex).Item("resId").ToString & "',aspnetForm.ctl00_conPlaHolTurismoSocial_hddsolId.value = '" & gdvCompletarEnxoval1.DataKeys(e.Row.RowIndex).Item("solId").ToString & "',aspnetForm.ctl00_conPlaHolTurismoSocial_hddApaDesc.value = '" & gdvCompletarEnxoval1.DataKeys(e.Row.RowIndex).Item("apaDesc").ToString & "',aspnetForm.ctl00_conPlaHolTurismoSocial_btnConsultaEnxoval.click() ")
            CType(e.Row.FindControl("lblAptoEnxoval1"), Label).Attributes.Add("ResId", gdvCompletarEnxoval1.DataKeys(e.Row.RowIndex).Item("resId").ToString)
            CType(e.Row.FindControl("lblAptoEnxoval1"), Label).Attributes.Add("solId", gdvCompletarEnxoval1.DataKeys(e.Row.RowIndex).Item("solId").ToString)
            CType(e.Row.FindControl("lblAptoEnxoval1"), Label).Text = gdvCompletarEnxoval1.DataKeys(e.Row.RowIndex).Item("apaDesc").ToString
            Dim Berco As String, Banheira As String

            If gdvCompletarEnxoval1.DataKeys(e.Row.RowIndex).Item("enxBerco").ToString = "S" Then
                Berco = "Sim"
            Else
                Berco = "Não"
            End If

            If gdvCompletarEnxoval1.DataKeys(e.Row.RowIndex).Item("enxBanheira").ToString = "S" Then
                Banheira = "Sim"
            Else
                Banheira = "Não"
            End If


            'Monta ToolTipo
            'Tratando para exibição no toolTip
            Dim BercoSolicitado As String = "", BanheiraSolicitada As String = ""
            Select Case gdvCompletarEnxoval1.DataKeys(e.Row.RowIndex).Item("enxBerco").ToString
                Case Is = "S"
                    BercoSolicitado = "Berço solicitado"
                Case Is = "N"
                    BercoSolicitado = "Berço não solicitado"
                Case Is = "R"
                    BercoSolicitado = "Retirar o berço"
            End Select

            'Else
            'BercoSolicitado = ""
            'End If

            'If gdvCompletarEnxoval1.DataKeys(e.Row.RowIndex).Item("enxBanheira").ToString = "S" Then
            '    If gdvCompletarEnxoval1.DataKeys(e.Row.RowIndex).Item("enxBanheira").ToString = "S" Then
            '        BanheiraSolicitada = "Solicitado"
            '    Else
            '        BanheiraSolicitada = "Não solicitado"
            '    End If
            'Else
            '    BanheiraSolicitada = ""
            'End If

            Select Case gdvCompletarEnxoval1.DataKeys(e.Row.RowIndex).Item("enxBanheira").ToString
                Case Is = "S"
                    BanheiraSolicitada = "Banheira solicitada"
                Case Is = "N"
                    BanheiraSolicitada = "Banheira não solicitada"
                Case Is = "R"
                    BanheiraSolicitada = "Retirar a banheira"
            End Select

            Dim ToolEnxoval As String
            If (gdvCompletarEnxoval1.DataKeys(e.Row.RowIndex).Item("enxCamaExtra").ToString.Trim.Length > 0) Then
                If (gdvCompletarEnxoval1.DataKeys(e.Row.RowIndex).Item("enxCamaExtra").ToString <> gdvCompletarEnxoval1.DataKeys(e.Row.RowIndex).Item("enxCamaExtraAtendida").ToString) Then
                    ToolEnxoval = ToolEnxoval & vbNewLine & "Completar para " & gdvCompletarEnxoval1.DataKeys(e.Row.RowIndex).Item("enxCamaExtra").ToString & " camas"
                End If
            End If

            If (gdvCompletarEnxoval1.DataKeys(e.Row.RowIndex).Item("enxBerco").ToString = "S" Or gdvCompletarEnxoval1.DataKeys(e.Row.RowIndex).Item("enxBerco").ToString = "R") Then
                If (gdvCompletarEnxoval1.DataKeys(e.Row.RowIndex).Item("enxBerco").ToString = "R" And gdvCompletarEnxoval1.DataKeys(e.Row.RowIndex).Item("enxBercoAtendido").ToString = "S") Or _
                   (gdvCompletarEnxoval1.DataKeys(e.Row.RowIndex).Item("enxBerco").ToString = "S" And (gdvCompletarEnxoval1.DataKeys(e.Row.RowIndex).Item("enxBercoAtendido").ToString.Trim = "" Or gdvCompletarEnxoval1.DataKeys(e.Row.RowIndex).Item("enxBercoAtendido").ToString = "N")) Then
                    ToolEnxoval = ToolEnxoval & vbNewLine & BercoSolicitado
                End If
            End If

            If (gdvCompletarEnxoval1.DataKeys(e.Row.RowIndex).Item("enxBanheira").ToString = "S" Or gdvCompletarEnxoval1.DataKeys(e.Row.RowIndex).Item("enxBanheira").ToString = "R") Then
                If (gdvCompletarEnxoval1.DataKeys(e.Row.RowIndex).Item("enxBanheira").ToString = "R" And gdvCompletarEnxoval1.DataKeys(e.Row.RowIndex).Item("enxBanheiraAtendida").ToString = "S") Or _
                   (gdvCompletarEnxoval1.DataKeys(e.Row.RowIndex).Item("enxBanheira").ToString = "S" And (gdvCompletarEnxoval1.DataKeys(e.Row.RowIndex).Item("enxBanheiraAtendida").ToString.Trim = "" Or gdvCompletarEnxoval1.DataKeys(e.Row.RowIndex).Item("enxBanheiraAtendida").ToString = "N")) Then
                    ToolEnxoval = ToolEnxoval & vbNewLine & BanheiraSolicitada
                End If
            End If

            'Adicionando ToolTip Personalizado 		
            If ((gdvCompletarEnxoval1.DataKeys(e.Row.RowIndex).Item("enxCamaExtra").ToString.Trim.Length > 0) Or _
                (gdvCompletarEnxoval1.DataKeys(e.Row.RowIndex).Item("enxBerco").ToString = "S") Or _
                (gdvCompletarEnxoval1.DataKeys(e.Row.RowIndex).Item("enxBanheira").ToString = "S")) Then
                ToolEnxoval = ToolEnxoval & vbNewLine & "-------Solicitante-------"
                ToolEnxoval = ToolEnxoval & vbNewLine & gdvCompletarEnxoval1.DataKeys(e.Row.RowIndex).Item("enxUsuarioSolicitante").ToString
                ToolEnxoval = ToolEnxoval & vbNewLine & "Em: " & gdvCompletarEnxoval1.DataKeys(e.Row.RowIndex).Item("enxUsuarioDataSolicitante").ToString
                CType(e.Row.FindControl("lblAptoEnxoval1"), Label).ToolTip = ToolEnxoval
            End If
        End If
    End Sub

    Protected Sub gdvCompletarEnxoval2_RowDataBound(sender As Object, e As GridViewRowEventArgs) Handles gdvCompletarEnxoval2.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            CType(e.Row.FindControl("lblAptoEnxoval2"), Label).Attributes.Add("onClick", "javascript:aspnetForm.ctl00_conPlaHolTurismoSocial_hddResId.value = '" & gdvCompletarEnxoval2.DataKeys(e.Row.RowIndex).Item("resId").ToString & "',aspnetForm.ctl00_conPlaHolTurismoSocial_hddsolId.value = '" & gdvCompletarEnxoval2.DataKeys(e.Row.RowIndex).Item("solId").ToString & "',aspnetForm.ctl00_conPlaHolTurismoSocial_hddApaDesc.value = '" & gdvCompletarEnxoval2.DataKeys(e.Row.RowIndex).Item("apaDesc").ToString & "',aspnetForm.ctl00_conPlaHolTurismoSocial_btnConsultaEnxoval.click() ")
            CType(e.Row.FindControl("lblAptoEnxoval2"), Label).Text = gdvCompletarEnxoval2.DataKeys(e.Row.RowIndex).Item("apaDesc").ToString
            CType(e.Row.FindControl("lblAptoEnxoval2"), Label).Text = gdvCompletarEnxoval2.DataKeys(e.Row.RowIndex).Item("apaDesc").ToString
            CType(e.Row.FindControl("lblAptoEnxoval2"), Label).Attributes.Add("ResId", gdvCompletarEnxoval2.DataKeys(e.Row.RowIndex).Item("resId").ToString)
            CType(e.Row.FindControl("lblAptoEnxoval2"), Label).Attributes.Add("solId", gdvCompletarEnxoval2.DataKeys(e.Row.RowIndex).Item("solId").ToString)

            Dim Berco As String, Banheira As String
            If gdvCompletarEnxoval2.DataKeys(e.Row.RowIndex).Item("enxBerco").ToString = "S" Then
                Berco = "Sim"
            Else
                Berco = "Não"
            End If
            If gdvCompletarEnxoval2.DataKeys(e.Row.RowIndex).Item("enxBanheira").ToString = "S" Then
                Banheira = "Sim"
            Else
                Banheira = "Não"
            End If

            'Monta ToolTipo
            'Tratando para exibição no toolTip
            Dim BercoSolicitado As String, BanheiraSolicitada As String
            If gdvCompletarEnxoval2.DataKeys(e.Row.RowIndex).Item("enxBerco").ToString = "S" Then
                If gdvCompletarEnxoval2.DataKeys(e.Row.RowIndex).Item("enxBerco").ToString = "S" Then
                    BercoSolicitado = "Solicitado"
                Else
                    BercoSolicitado = "Não solicitado"
                End If
            Else
                BercoSolicitado = ""
            End If

            If gdvCompletarEnxoval2.DataKeys(e.Row.RowIndex).Item("enxBanheira").ToString = "S" Then
                If gdvCompletarEnxoval2.DataKeys(e.Row.RowIndex).Item("enxBanheira").ToString = "S" Then
                    BanheiraSolicitada = "Solicitado"
                Else
                    BanheiraSolicitada = "Não solicitado"
                End If
            Else
                BanheiraSolicitada = ""
            End If

            Dim ToolEnxoval As String
            If (gdvCompletarEnxoval2.DataKeys(e.Row.RowIndex).Item("enxCamaExtra").ToString.Trim.Length > 0) Then
                ToolEnxoval = ToolEnxoval & vbNewLine & "Cama: " & gdvCompletarEnxoval2.DataKeys(e.Row.RowIndex).Item("enxCamaExtra").ToString
            End If
            If (gdvCompletarEnxoval2.DataKeys(e.Row.RowIndex).Item("enxBerco").ToString = "S") Then
                ToolEnxoval = ToolEnxoval & vbNewLine & "Berço: " & BercoSolicitado
            End If
            If (gdvCompletarEnxoval2.DataKeys(e.Row.RowIndex).Item("enxBanheira").ToString = "S") Then
                ToolEnxoval = ToolEnxoval & vbNewLine & "Banheira: " & BanheiraSolicitada
            End If

            'Adicionando ToolTip Personalizado 		
            If ((gdvCompletarEnxoval2.DataKeys(e.Row.RowIndex).Item("enxCamaExtra").ToString.Trim.Length > 0) Or _
                (gdvCompletarEnxoval2.DataKeys(e.Row.RowIndex).Item("enxBerco").ToString = "S") Or _
                (gdvCompletarEnxoval2.DataKeys(e.Row.RowIndex).Item("enxBanheira").ToString = "S")) Then
                ToolEnxoval = ToolEnxoval & vbNewLine & "-------Solicitante-------"
                ToolEnxoval = ToolEnxoval & vbNewLine & gdvCompletarEnxoval2.DataKeys(e.Row.RowIndex).Item("enxUsuarioSolicitante").ToString
                ToolEnxoval = ToolEnxoval & vbNewLine & "Em: " & gdvCompletarEnxoval2.DataKeys(e.Row.RowIndex).Item("enxUsuarioDataSolicitante").ToString
                CType(e.Row.FindControl("lblAptoEnxoval2"), Label).ToolTip = ToolEnxoval
            End If
        End If
    End Sub

    Protected Sub Mensagem(Texto As String)
        ScriptManager.RegisterStartupScript(Page, Page.[GetType](), Guid.NewGuid().ToString(), "alert('" & Texto & "');", True)
    End Sub

    Protected Sub AtualizaSomenteAtencimentoEnxovais()
        '=========COMPLETAR ENXOVAL'=========
        objCheckInOutVO = New CheckInOutVO
        objCheckInOutDAO = New CheckInOutDAO(btnUnidade.Attributes.Item("AliasBancoTurismo").ToString)
        Dim ListaBaseConpletarEnxoval As IList
        'ObjConferenciaVO.ApaId = txtApartamento.Text.Replace("'", "")
        ListaBaseConpletarEnxoval = objCheckInOutDAO.ConsultaCompletarEnxovais()
        'CRIANDO AS LISTAS AUXILIARES
        Dim ListaEnxoval1 As IList = New ArrayList
        Dim ListaEnxoval2 As IList = New ArrayList
        cont = 1
        'PERCORRER A LISATA BASE INSERINDO OS VALORES NAS LISTA AUXILIARES
        'QUE IRÃO PREENCHER OS GRID'S
        For Each item As CheckInOutVO In ListaBaseConpletarEnxoval
            Select Case cont
                Case 1 : ListaEnxoval1.Add(item)
                Case 2 : ListaEnxoval2.Add(item)
            End Select
            If cont = 2 Then
                cont = 1
            Else
                cont += 1
            End If
        Next
        gdvCompletarEnxoval1.DataSource = ListaEnxoval1
        gdvCompletarEnxoval2.DataSource = ListaEnxoval2
        gdvCompletarEnxoval1.DataBind()
        gdvCompletarEnxoval2.DataBind()
        'SE EXISTIR ALGO A SER EXIBIDO O PAINEL SERÁ VISUALIADO
        If (ListaEnxoval1.Count > 0 Or ListaEnxoval2.Count > 0) Then
            pnlCompletarEnxoval.Visible = True
        Else
            pnlCompletarEnxoval.Visible = False
        End If
    End Sub

    Protected Sub btnConsultaEnxoval_Click(sender As Object, e As EventArgs) Handles btnConsultaEnxoval.Click
        divModalEnxoval.Visible = True
        objCheckInOutVO = New CheckInOutVO
        objCheckInOutDAO = New CheckInOutDAO(btnUnidade.Attributes.Item("AliasBancoTurismo").ToString)
        objCheckInOutVO = objCheckInOutDAO.ConsultaEnxovalPorSolId(hddsolId.Value)
        With objCheckInOutVO
            rdSolCamaExtra.Items.Clear()
            rdSolCamaExtra.Items.Add(1)
            rdSolCamaExtra.Items.Add(2)
            rdSolCamaExtra.Items.Add(3)
            rdSolCamaExtra.Items.Add(4)
            rdSolCamaExtra.Items.Add(5)

            If .enxCamaExtra = " " Then
                rdSolCamaExtra.Enabled = False
            ElseIf objCheckInOutVO.enxCamaExtra < 0 Then
                rdSolCamaExtra.Enabled = False
            ElseIf objCheckInOutVO.enxCamaExtra = 1 Then
                rdSolCamaExtra.Items.Remove(2)
                rdSolCamaExtra.Items.Remove(3)
                rdSolCamaExtra.Items.Remove(4)
                rdSolCamaExtra.Items.Remove(5)
            ElseIf objCheckInOutVO.enxCamaExtra = 2 Then
                rdSolCamaExtra.Items.Remove(1)
                rdSolCamaExtra.Items.Remove(3)
                rdSolCamaExtra.Items.Remove(4)
                rdSolCamaExtra.Items.Remove(5)
            ElseIf objCheckInOutVO.enxCamaExtra = 3 Then
                rdSolCamaExtra.Items.Remove(1)
                rdSolCamaExtra.Items.Remove(2)
                rdSolCamaExtra.Items.Remove(4)
                rdSolCamaExtra.Items.Remove(5)
            ElseIf objCheckInOutVO.enxCamaExtra = 4 Then
                rdSolCamaExtra.Items.Remove(1)
                rdSolCamaExtra.Items.Remove(2)
                rdSolCamaExtra.Items.Remove(3)
                rdSolCamaExtra.Items.Remove(5)
            ElseIf objCheckInOutVO.enxCamaExtra = 5 Then
                rdSolCamaExtra.Items.Remove(1)
                rdSolCamaExtra.Items.Remove(2)
                rdSolCamaExtra.Items.Remove(3)
                rdSolCamaExtra.Items.Remove(4)
            End If

            Try
                rdSolCamaExtra.SelectedValue = .enxCamaExtra
            Catch ex As Exception
            End Try
            If .enxBerco = "S" Then
                chkSolBerco.Checked = True
            Else
                chkSolBerco.Checked = False
            End If
            If .enxBanheira = "S" Then
                chksolBanheira.Checked = True
            Else
                chksolBanheira.Checked = False
            End If

            If .enxBercoAtendido = "S" Then
                chkAteBerco.Checked = True
            Else
                chkAteBerco.Checked = False
            End If
            If .enxBanheiraAtendida = "S" Then
                chkAteBanheira.Checked = True
            Else
                chkAteBanheira.Checked = False
            End If

            rdAteCamaExtra.Items.Clear()
            rdAteCamaExtra.Items.Add(3)
            rdAteCamaExtra.Items.Add(4)
            rdAteCamaExtra.Items.Add(5)

            If rdSolCamaExtra.SelectedIndex < 0 Then
                rdAteCamaExtra.Enabled = False
            ElseIf rdSolCamaExtra.SelectedValue = 3 Then
                rdAteCamaExtra.Items.Remove(4)
                rdAteCamaExtra.Items.Remove(5)
            ElseIf rdSolCamaExtra.SelectedValue = 4 Then
                If (hddApaDesc.Value.Contains("RT") Or hddApaDesc.Value.Contains("RP")) Then
                    rdAteCamaExtra.Items.Remove(3)
                    rdAteCamaExtra.Items.Remove(5)
                Else 'Bambui pode ter 4 ou 5
                    rdAteCamaExtra.Items.Remove(3)
                End If
            ElseIf rdSolCamaExtra.SelectedValue = 5 Then
                rdAteCamaExtra.Items.Remove(3)
            Else
                rdAteCamaExtra.Items.Clear()
            End If

            'Setando a camaextra no atendimento
            Try
                rdAteCamaExtra.SelectedValue = .enxCamaExtraAtendida
            Catch ex As Exception
            End Try

            If chkSolBerco.Checked = True Then
                chkAteBerco.Visible = True
            Else
                chkAteBerco.Visible = False
            End If
            If chksolBanheira.Checked = True Then
                chkAteBanheira.Visible = True
            Else
                chkAteBanheira.Visible = False
            End If

            txtAteMotivoNaoCompletar.Text = .enxMotivoNaoAtendimento
            lblApaDescEnxoval.Text = vbNewLine & "Apartamento: " & hddApaDesc.Value
            'Exibindo o painel
            pnlCompletarEnxovais.CssClass = "PosicionaFinalTelaCentralizado"
            DivBotoesEnxoval.Attributes("Class") = "BotaoRodaPe"
        End With
    End Sub

    Protected Sub imgConfirmarEnxoval_Click(sender As Object, e As ImageClickEventArgs) Handles imgConfirmarEnxoval.Click
        Try
            objCheckInOutVO = New CheckInOutVO
            objCheckInOutDAO = New CheckInOutDAO(btnUnidade.Attributes.Item("AliasBancoTurismo"))

            If (rdAteCamaExtra.SelectedValue <> rdSolCamaExtra.SelectedValue Or _
               chkSolBerco.Checked <> chkAteBerco.Checked Or _
               chksolBanheira.Checked <> chkAteBanheira.Checked) And _
               txtAteMotivoNaoCompletar.Text.Trim.Length = 0 Then
                Mensagem("Informe o motivo pelo não atendimento total da solicitação")
                txtAteMotivoNaoCompletar.Focus()
                Exit Try
            End If

            With objCheckInOutVO
                .resId = hddResId.Value
                .solId = hddsolId.Value
                .enxCamaExtraAtendida = rdAteCamaExtra.SelectedValue
                If chkAteBanheira.Checked Then
                    .enxBanheiraAtendida = "S"
                Else
                    .enxBanheiraAtendida = "N"
                End If
                If chkAteBerco.Checked Then
                    .enxBercoAtendido = "S"
                Else
                    .enxBercoAtendido = "N"
                End If
                .enxMotivoNaoAtendimento = txtAteMotivoNaoCompletar.Text.Trim.Replace("'", "")
                .enxUsuarioAtendimento = User.Identity.Name.ToString.Replace("SESC-GO.COM.BR\", "")

            End With

            If objCheckInOutDAO.InserirCompetarEnxoval(objCheckInOutVO, "G") = 2 Then
                Mensagem("Atendimento Executado com sucesso!")
                AtualizaSomenteAtencimentoEnxovais()
            Else
                Mensagem("Houve um erro ao salvar o atendimento! Tente novamente.")
            End If
        Catch ex As Exception
            Throw New Exception(ex.StackTrace.ToString.Replace(Chr(10), ""))
        End Try
    End Sub

    Protected Sub imgCancelarEnxoval_Click(sender As Object, e As ImageClickEventArgs) Handles imgCancelarEnxoval.Click
        divModalEnxoval.Visible = False
    End Sub

    Protected Sub chkTodosEnxoval_CheckedChanged(sender As Object, e As EventArgs) Handles chkTodosEnxoval.CheckedChanged
        'Passando no primeiro GRID
        If chkTodosEnxoval.Checked = True Then
            For Each linha As GridViewRow In gdvCompletarEnxoval1.Rows
                CType(linha.FindControl("chkEnxoval1"), CheckBox).Checked = True
            Next
            'Passando no Segundo GRID
            For Each linha As GridViewRow In gdvCompletarEnxoval2.Rows
                CType(linha.FindControl("chkEnxoval2"), CheckBox).Checked = True
            Next
        Else
            For Each linha As GridViewRow In gdvCompletarEnxoval1.Rows
                CType(linha.FindControl("chkEnxoval1"), CheckBox).Checked = False
            Next
            'Passando no Segundo GRID
            For Each linha As GridViewRow In gdvCompletarEnxoval2.Rows
                CType(linha.FindControl("chkEnxoval2"), CheckBox).Checked = False
            Next
        End If
    End Sub

    Protected Sub imgAtualizaTodosEnxoval_Click(sender As Object, e As ImageClickEventArgs) Handles imgAtualizaTodosEnxoval.Click
        AtualizaSomenteAtencimentoEnxovais()
    End Sub

    Protected Sub btnConfirmarEnxoval_Click(sender As Object, e As EventArgs) Handles btnConfirmarEnxoval.Click
        Try
            objCheckInOutVO = New CheckInOutVO
            objCheckInOutDAO = New CheckInOutDAO(btnUnidade.Attributes.Item("AliasBancoTurismo"))
            'Passando no primeiro GRID
            For Each linha As GridViewRow In gdvCompletarEnxoval1.Rows
                'Ira passar somente nos apartamentos checados
                If CType(linha.FindControl("chkEnxoval1"), CheckBox).Checked = True Then
                    With objCheckInOutVO
                        .resId = CType(linha.FindControl("lblAptoEnxoval1"), Label).Attributes.Item("ResId").ToString
                        .solId = CType(linha.FindControl("lblAptoEnxoval1"), Label).Attributes.Item("solId").ToString
                        .enxCamaExtraAtendida = gdvCompletarEnxoval1.DataKeys(linha.RowIndex).Item("enxCamaExtra").ToString   'rdAteCamaExtra.SelectedValue
                        .enxBanheira = gdvCompletarEnxoval1.DataKeys(linha.RowIndex).Item("enxBanheira").ToString
                        .enxBercoAtendido = gdvCompletarEnxoval1.DataKeys(linha.RowIndex).Item("enxBerco").ToString
                        .enxBanheiraAtendida = gdvCompletarEnxoval1.DataKeys(linha.RowIndex).Item("enxBanheira").ToString
                        .enxMotivoNaoAtendimento = ""
                        .enxUsuarioAtendimento = User.Identity.Name.ToString.Replace("SESC-GO.COM.BR\", "")
                        'Se EnxBerco = R - o atendimento será setado como atendido sim e sim
                        .enxBerco = gdvCompletarEnxoval1.DataKeys(linha.RowIndex).Item("enxBerco").ToString
                    End With

                    If objCheckInOutDAO.InserirCompetarEnxoval(objCheckInOutVO, "G") = 2 Then
                    Else
                        Mensagem("Houve um erro ao salvar o atendimento! Tente novamente.")
                    End If
                End If
            Next

            objCheckInOutVO = New CheckInOutVO
            objCheckInOutDAO = New CheckInOutDAO(btnUnidade.Attributes.Item("AliasBancoTurismo"))
            For Each linha As GridViewRow In gdvCompletarEnxoval2.Rows
                'objCheckInOutVO = New CheckInOutVO
                'objCheckInOutDAO = New CheckInOutDAO(btnUnidade.Attributes.Item("AliasBancoTurismo"))
                If CType(linha.FindControl("chkEnxoval2"), CheckBox).Checked = True Then
                    With objCheckInOutVO
                        .resId = CType(linha.FindControl("lblAptoEnxoval2"), Label).Attributes.Item("ResId").ToString
                        .solId = CType(linha.FindControl("lblAptoEnxoval2"), Label).Attributes.Item("solId").ToString
                        .enxCamaExtraAtendida = gdvCompletarEnxoval2.DataKeys(linha.RowIndex).Item("enxCamaExtra").ToString   'rdAteCamaExtra.SelectedValue
                        .enxBercoAtendido = gdvCompletarEnxoval2.DataKeys(linha.RowIndex).Item("enxBerco").ToString
                        .enxBanheiraAtendida = gdvCompletarEnxoval2.DataKeys(linha.RowIndex).Item("enxBanheira").ToString
                        .enxMotivoNaoAtendimento = ""
                        .enxUsuarioAtendimento = User.Identity.Name.ToString.Replace("SESC-GO.COM.BR\", "")
                        'Se EnxBerco = R - o atendimento será setado como atendido sim e sim
                        .enxBerco = gdvCompletarEnxoval2.DataKeys(linha.RowIndex).Item("enxBerco").ToString
                    End With

                    If objCheckInOutDAO.InserirCompetarEnxoval(objCheckInOutVO, "G") = 2 Then
                    Else
                        Mensagem("Houve um erro ao salvar o atendimento! Tente novamente.")
                    End If
                End If
            Next
            hddProcessando.Value = ""
            'Efetuando novamente a busca
            Mensagem("Apartamentos completados com sucesso!")
            AtualizaSomenteAtencimentoEnxovais()
        Catch ex As Exception
            Throw New Exception(ex.StackTrace.ToString.Replace(Chr(10), ""))
        End Try
    End Sub

    Protected Sub gdvEnxAtendido1_RowDataBound(sender As Object, e As GridViewRowEventArgs) Handles gdvEnxAtendido1.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            CType(e.Row.FindControl("lblEnxCompletado1"), Label).Text = gdvEnxAtendido1.DataKeys(e.Row.RowIndex).Item("apaDesc").ToString
            'Montando tooltip
            Dim Berco As String, Banheira As String
            If (gdvEnxAtendido1.DataKeys(e.Row.RowIndex).Item("enxBerco").ToString = "S" And gdvEnxAtendido1.DataKeys(e.Row.RowIndex).Item("enxBercoAtendido").ToString = "S") Then
                Berco = "Berço"
            Else
                Berco = "Berço não atendido"
            End If

            If (gdvEnxAtendido1.DataKeys(e.Row.RowIndex).Item("enxBanheira").ToString = "S" And gdvEnxAtendido1.DataKeys(e.Row.RowIndex).Item("enxBanheiraAtendida").ToString = "S") Then
                Banheira = "Banheira"
            Else
                Banheira = "Banheira não atendida"
            End If

            Dim ToolEnxoval As String
            If (gdvEnxAtendido1.DataKeys(e.Row.RowIndex).Item("enxCamaExtra").ToString.Trim.Length > 0) Then
                ToolEnxoval = ToolEnxoval & "Completado para " & gdvEnxAtendido1.DataKeys(e.Row.RowIndex).Item("enxCamaExtra").ToString & " camas"
            End If

            If (gdvEnxAtendido1.DataKeys(e.Row.RowIndex).Item("enxBerco").ToString = "S") Then
                ToolEnxoval = ToolEnxoval & vbNewLine & Berco
            End If

            If (gdvEnxAtendido1.DataKeys(e.Row.RowIndex).Item("enxBanheira").ToString = "S") Then
                ToolEnxoval = ToolEnxoval & vbNewLine & Banheira
            End If
            'Adicionando ToolTip Personalizado 		
            ToolEnxoval = ToolEnxoval & vbNewLine & "-------Solicitante-------"
            ToolEnxoval = ToolEnxoval & vbNewLine & gdvEnxAtendido1.DataKeys(e.Row.RowIndex).Item("enxUsuarioSolicitante").ToString
            ToolEnxoval = ToolEnxoval & vbNewLine & "Em: " & gdvEnxAtendido1.DataKeys(e.Row.RowIndex).Item("enxUsuarioDataSolicitante").ToString
            ToolEnxoval = ToolEnxoval & vbNewLine & "-------Atendimento-------"
            ToolEnxoval = ToolEnxoval & vbNewLine & gdvEnxAtendido1.DataKeys(e.Row.RowIndex).Item("enxUsuarioAtendimento").ToString
            ToolEnxoval = ToolEnxoval & vbNewLine & "Em: " & gdvEnxAtendido1.DataKeys(e.Row.RowIndex).Item("enxUsuarioDataAtendimento").ToString
            CType(e.Row.FindControl("lblEnxCompletado1"), Label).ToolTip = ToolEnxoval
        End If
    End Sub

    Protected Sub gdvEnxAtendido2_RowDataBound(sender As Object, e As GridViewRowEventArgs) Handles gdvEnxAtendido2.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            CType(e.Row.FindControl("lblEnxCompletado2"), Label).Text = gdvEnxAtendido2.DataKeys(e.Row.RowIndex).Item("apaDesc").ToString
            'Montando tooltip
            Dim Berco As String, Banheira As String
            If (gdvEnxAtendido2.DataKeys(e.Row.RowIndex).Item("enxBerco").ToString = "S" And gdvEnxAtendido2.DataKeys(e.Row.RowIndex).Item("enxBercoAtendido").ToString = "S") Then
                Berco = "Berço"
            Else
                Berco = "Berço não atendido"
            End If

            If (gdvEnxAtendido2.DataKeys(e.Row.RowIndex).Item("enxBanheira").ToString = "S" And gdvEnxAtendido2.DataKeys(e.Row.RowIndex).Item("enxBanheiraAtendida").ToString = "S") Then
                Banheira = "Banheira"
            Else
                Banheira = "Banheira não atendida"
            End If

            Dim ToolEnxoval As String
            If (gdvEnxAtendido2.DataKeys(e.Row.RowIndex).Item("enxCamaExtra").ToString.Trim.Length > 0) Then
                ToolEnxoval = ToolEnxoval & "Completado para " & gdvEnxAtendido2.DataKeys(e.Row.RowIndex).Item("enxCamaExtra").ToString & " camas"
            End If

            If (gdvEnxAtendido2.DataKeys(e.Row.RowIndex).Item("enxBerco").ToString = "S") Then
                ToolEnxoval = ToolEnxoval & vbNewLine & Berco
            End If

            If (gdvEnxAtendido2.DataKeys(e.Row.RowIndex).Item("enxBanheira").ToString = "S") Then
                ToolEnxoval = ToolEnxoval & vbNewLine & Banheira
            End If
            'Adicionando ToolTip Personalizado 		
            ToolEnxoval = ToolEnxoval & vbNewLine & "-------Solicitante-------"
            ToolEnxoval = ToolEnxoval & vbNewLine & gdvEnxAtendido2.DataKeys(e.Row.RowIndex).Item("enxUsuarioSolicitante").ToString
            ToolEnxoval = ToolEnxoval & vbNewLine & "Em: " & gdvEnxAtendido2.DataKeys(e.Row.RowIndex).Item("enxUsuarioDataSolicitante").ToString
            ToolEnxoval = ToolEnxoval & vbNewLine & "-------Atendimento-------"
            ToolEnxoval = ToolEnxoval & vbNewLine & gdvEnxAtendido2.DataKeys(e.Row.RowIndex).Item("enxUsuarioAtendimento").ToString
            ToolEnxoval = ToolEnxoval & vbNewLine & "Em: " & gdvEnxAtendido2.DataKeys(e.Row.RowIndex).Item("enxUsuarioDataAtendimento").ToString
            CType(e.Row.FindControl("lblEnxCompletado2"), Label).ToolTip = ToolEnxoval

        End If
    End Sub

    Protected Sub gdvEnxAtendido3_RowDataBound(sender As Object, e As GridViewRowEventArgs) Handles gdvEnxAtendido3.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            CType(e.Row.FindControl("lblEnxCompletado3"), Label).Text = gdvEnxAtendido3.DataKeys(e.Row.RowIndex).Item("apaDesc").ToString
            'Montando tooltip
            Dim Berco As String, Banheira As String
            If (gdvEnxAtendido3.DataKeys(e.Row.RowIndex).Item("enxBerco").ToString = "S" And gdvEnxAtendido3.DataKeys(e.Row.RowIndex).Item("enxBercoAtendido").ToString = "S") Then
                Berco = "Berço"
            Else
                Berco = "Berço não atendido"
            End If

            If (gdvEnxAtendido3.DataKeys(e.Row.RowIndex).Item("enxBanheira").ToString = "S" And gdvEnxAtendido3.DataKeys(e.Row.RowIndex).Item("enxBanheiraAtendida").ToString = "S") Then
                Banheira = "Banheira"
            Else
                Banheira = "Banheira não atendida"
            End If

            Dim ToolEnxoval As String
            If (gdvEnxAtendido3.DataKeys(e.Row.RowIndex).Item("enxCamaExtra").ToString.Trim.Length > 0) Then
                ToolEnxoval = ToolEnxoval & "Completado para " & gdvEnxAtendido3.DataKeys(e.Row.RowIndex).Item("enxCamaExtra").ToString & " camas"
            End If

            If (gdvEnxAtendido3.DataKeys(e.Row.RowIndex).Item("enxBerco").ToString = "S") Then
                ToolEnxoval = ToolEnxoval & vbNewLine & Berco
            End If

            If (gdvEnxAtendido3.DataKeys(e.Row.RowIndex).Item("enxBanheira").ToString = "S") Then
                ToolEnxoval = ToolEnxoval & vbNewLine & Banheira
            End If
            'Adicionando ToolTip Personalizado 		
            ToolEnxoval = ToolEnxoval & vbNewLine & "-------Solicitante-------"
            ToolEnxoval = ToolEnxoval & vbNewLine & gdvEnxAtendido3.DataKeys(e.Row.RowIndex).Item("enxUsuarioSolicitante").ToString
            ToolEnxoval = ToolEnxoval & vbNewLine & "Em: " & gdvEnxAtendido3.DataKeys(e.Row.RowIndex).Item("enxUsuarioDataSolicitante").ToString
            ToolEnxoval = ToolEnxoval & vbNewLine & "-------Atendimento-------"
            ToolEnxoval = ToolEnxoval & vbNewLine & gdvEnxAtendido3.DataKeys(e.Row.RowIndex).Item("enxUsuarioAtendimento").ToString
            ToolEnxoval = ToolEnxoval & vbNewLine & "Em: " & gdvEnxAtendido3.DataKeys(e.Row.RowIndex).Item("enxUsuarioDataAtendimento").ToString
            CType(e.Row.FindControl("lblEnxCompletado3"), Label).ToolTip = ToolEnxoval
        End If
    End Sub

    Protected Sub gdvEnxAtendido4_RowDataBound(sender As Object, e As GridViewRowEventArgs) Handles gdvEnxAtendido4.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            CType(e.Row.FindControl("lblEnxCompletado4"), Label).Text = gdvEnxAtendido4.DataKeys(e.Row.RowIndex).Item("apaDesc").ToString
            'Montando tooltip
            Dim Berco As String, Banheira As String
            If (gdvEnxAtendido4.DataKeys(e.Row.RowIndex).Item("enxBerco").ToString = "S" And gdvEnxAtendido4.DataKeys(e.Row.RowIndex).Item("enxBercoAtendido").ToString = "S") Then
                Berco = "Berço"
            Else
                Berco = "Berço não atendido"
            End If

            If (gdvEnxAtendido4.DataKeys(e.Row.RowIndex).Item("enxBanheira").ToString = "S" And gdvEnxAtendido4.DataKeys(e.Row.RowIndex).Item("enxBanheiraAtendida").ToString = "S") Then
                Banheira = "Banheira"
            Else
                Banheira = "Banheira não atendida"
            End If

            Dim ToolEnxoval As String
            If (gdvEnxAtendido4.DataKeys(e.Row.RowIndex).Item("enxCamaExtra").ToString.Trim.Length > 0) Then
                ToolEnxoval = ToolEnxoval & "Completado para " & gdvEnxAtendido4.DataKeys(e.Row.RowIndex).Item("enxCamaExtra").ToString & " camas"
            End If

            If (gdvEnxAtendido4.DataKeys(e.Row.RowIndex).Item("enxBerco").ToString = "S") Then
                ToolEnxoval = ToolEnxoval & vbNewLine & Berco
            End If

            If (gdvEnxAtendido4.DataKeys(e.Row.RowIndex).Item("enxBanheira").ToString = "S") Then
                ToolEnxoval = ToolEnxoval & vbNewLine & Banheira
            End If
            'Adicionando ToolTip Personalizado 		
            ToolEnxoval = ToolEnxoval & vbNewLine & "-------Solicitante-------"
            ToolEnxoval = ToolEnxoval & vbNewLine & gdvEnxAtendido4.DataKeys(e.Row.RowIndex).Item("enxUsuarioSolicitante").ToString
            ToolEnxoval = ToolEnxoval & vbNewLine & "Em: " & gdvEnxAtendido4.DataKeys(e.Row.RowIndex).Item("enxUsuarioDataSolicitante").ToString
            ToolEnxoval = ToolEnxoval & vbNewLine & "-------Atendimento-------"
            ToolEnxoval = ToolEnxoval & vbNewLine & gdvEnxAtendido4.DataKeys(e.Row.RowIndex).Item("enxUsuarioAtendimento").ToString
            ToolEnxoval = ToolEnxoval & vbNewLine & "Em: " & gdvEnxAtendido4.DataKeys(e.Row.RowIndex).Item("enxUsuarioDataAtendimento").ToString
            CType(e.Row.FindControl("lblEnxCompletado4"), Label).ToolTip = ToolEnxoval
        End If
    End Sub
    Public Shared Sub GravaLog(ByVal msg As String)
        Dim dt As DateTime = Now
        Dim arquivo As String = System.AppDomain.CurrentDomain.BaseDirectory() & "\Log\" & "LOG" + Format("{0:yyyyMMdd}", dt) + ".TXT"
        Dim objStream As New FileStream(arquivo, FileMode.Append)
        Dim arq As New StreamWriter(objStream)
        arq.Write(Format("{0:HH:mm:ss}", dt) + " " + msg + vbCrLf)
        arq.Close()
    End Sub
End Class