Public Class ReservaEmissivoVO

    Private _resId As Long
    Private _resNome As String
    Private _resNomeComplemento As String
    Private _resMatricula As String
    Private _resCaracteristica As String
    Private _catId As Long
    Private _catRefeicaoId As Long
    Private _estId As Long
    Private _estIdDes As Long
    Private _orgId As Long
    Private _refPratoCod As Long
    Private _qtdAcomodacao As Short
    Private _qtdHospede As Short
    Private _resStatus As Char
    Private _resStatusDesc As String
    Private _resDataIni As String
    Private _resHoraIni As String
    Private _resDataFim As String
    Private _resHoraFim As String
    Private _resValorPago As Decimal
    Private _resValorPagoAntecipado As Decimal
    Private _resLimite As Char
    Private _resObs As String
    Private _resCafe As Char
    Private _resAlmoco As Char
    Private _resJantar As Char
    Private _resFormaPagtoCafe As String
    Private _resFormaPagtoAlmoco As String
    Private _resFormaPagtoJantar As String
    Private _resSexo As Char
    Private _resContato As String
    Private _resValorDesconto As Decimal
    Private _resDtLimiteRetorno As String
    Private _resDtLimiteCancela As String
    Private _resEstadoCivil As Char
    Private _resLogradouro As String
    Private _resNumero As String
    Private _resQuadra As String
    Private _resLote As String
    Private _resComplemento As String
    Private _resBairro As String
    Private _resCapitalGoias As Char
    Private _resCidade As String
    Private _resCep As String
    Private _resColoniaFeriasDes As Char
    Private _resCapitalGoiasDes As Char
    Private _resCidadeDes As String
    Private _resFoneComercial As String
    Private _resFoneResidencial As String
    Private _resFax As String
    Private _resCelular As String
    Private _resEmail As String
    Private _resDtNascimento As String
    Private _resRG As String
    Private _resCPF_CGC As String
    Private _resVlrAPagar As Decimal
    Private _resVlrPago As Decimal
    Private _resMemorando As String
    Private _resEmissor As Integer
    Private _resCatCobranca As Long
    Private _resFormaPagamento As String
    Private _resHotelExcursao As String
    Private _resUsuario As String
    Private _resUsuarioData As String
    Private _resQtdeLista As Integer
    Private _resValorDescontoPorInteg As Decimal
    Private _resPernoite As Char
    Private _resDtInsercao As String
    Private _resIdWeb As Long
    Private _resAlmocoRestaurante As Char
    Private _resDtGrupoConfirmacao As String
    Private _resDtGrupoListagem As String
    Private _resDtGrupoPgtoSinal As String
    Private _resDtGrupoPgtoTotal As String
    Private _resDtGrupoAptoReservado As String
    Private _resGrupoConfirmacao As Char
    Private _resPasseioPromovidoCEREC As Char
    Private _resValidade As String
    Private _resLocalSaida As String
    Private _resHoraSaida As String
    Private _orgGrupo As Char
    Private _resSalario As String
    Private _resEscolaridade As String
    Private _resTipo As String

    Public Property resId() As Long
        Get
            Return _resId
        End Get
        Set(ByVal value As Long)
            _resId = value
        End Set
    End Property
    Public Property resNome() As String
        Get
            Return _resNome
        End Get
        Set(ByVal value As String)
            _resNome = value
        End Set
    End Property
    Public Property resNomeComplemento() As String
        Get
            Return _resNomeComplemento
        End Get
        Set(ByVal value As String)
            _resNomeComplemento = value
        End Set
    End Property
    Public Property resMatricula() As String
        Get
            Return _resMatricula
        End Get
        Set(ByVal value As String)
            _resMatricula = value
        End Set
    End Property
    Public Property resCaracteristica() As String
        Get
            Return _resCaracteristica
        End Get
        Set(ByVal value As String)
            _resCaracteristica = value
        End Set
    End Property
    Public Property catId() As Long
        Get
            Return _catId
        End Get
        Set(ByVal value As Long)
            _catId = value
        End Set
    End Property
    Public Property catRefeicaoId() As Long
        Get
            Return _catRefeicaoId
        End Get
        Set(ByVal value As Long)
            _catRefeicaoId = value
        End Set
    End Property
    Public Property estId() As Long
        Get
            Return _estId
        End Get
        Set(ByVal value As Long)
            _estId = value
        End Set
    End Property
    Public Property estIdDes() As Long
        Get
            Return _estIdDes
        End Get
        Set(ByVal value As Long)
            _estIdDes = value
        End Set
    End Property
    Public Property orgId() As Long
        Get
            Return _orgId
        End Get
        Set(ByVal value As Long)
            _orgId = value
        End Set
    End Property
    Public Property refPratoCod() As Long
        Get
            Return _refPratoCod
        End Get
        Set(ByVal value As Long)
            _refPratoCod = value
        End Set
    End Property
    Public Property qtdAcomodacao() As Short
        Get
            Return _qtdAcomodacao
        End Get
        Set(ByVal value As Short)
            _qtdAcomodacao = value
        End Set
    End Property
    Public Property qtdHospede() As Short
        Get
            Return _qtdHospede
        End Get
        Set(ByVal value As Short)
            _qtdHospede = value
        End Set
    End Property
    Public Property resStatus() As Char
        Get
            Return _resStatus
        End Get
        Set(ByVal value As Char)
            _resStatus = value
        End Set
    End Property
    Public Property resStatusDesc() As String
        Get
            Return _resStatusDesc
        End Get
        Set(ByVal value As String)
            _resStatusDesc = value
        End Set
    End Property
    Public Property resDataIni() As String
        Get
            Return _resDataIni
        End Get
        Set(ByVal value As String)
            _resDataIni = value
        End Set
    End Property
    Public Property resHoraIni() As String
        Get
            Return _resHoraIni
        End Get
        Set(ByVal value As String)
            _resHoraIni = value
        End Set
    End Property
    Public Property resDataFim() As String
        Get
            Return _resDataFim
        End Get
        Set(ByVal value As String)
            _resDataFim = value
        End Set
    End Property
    Public Property resHoraFim() As String
        Get
            Return _resHoraFim
        End Get
        Set(ByVal value As String)
            _resHoraFim = value
        End Set
    End Property
    Public Property resValorPago() As Decimal
        Get
            Return _resValorPago
        End Get
        Set(ByVal value As Decimal)
            _resValorPago = value
        End Set
    End Property
    Public Property resValorPagoAntecipado() As Decimal
        Get
            Return _resValorPagoAntecipado
        End Get
        Set(ByVal value As Decimal)
            _resValorPagoAntecipado = value
        End Set
    End Property
    Public Property resLimite() As Char
        Get
            Return _resLimite
        End Get
        Set(ByVal value As Char)
            _resLimite = value
        End Set
    End Property
    Public Property resObs() As String
        Get
            Return _resObs
        End Get
        Set(ByVal value As String)
            _resObs = value
        End Set
    End Property
    Public Property resCafe() As Char
        Get
            Return _resCafe
        End Get
        Set(ByVal value As Char)
            _resCafe = value
        End Set
    End Property
    Public Property resAlmoco() As Char
        Get
            Return _resAlmoco
        End Get
        Set(ByVal value As Char)
            _resAlmoco = value
        End Set
    End Property
    Public Property resJantar() As Char
        Get
            Return _resJantar
        End Get
        Set(ByVal value As Char)
            _resJantar = value
        End Set
    End Property
    Public Property resFormaPagtoCafe() As String
        Get
            Return _resFormaPagtoCafe
        End Get
        Set(ByVal value As String)
            _resFormaPagtoCafe = value
        End Set
    End Property
    Public Property resFormaPagtoAlmoco() As String
        Get
            Return _resFormaPagtoAlmoco
        End Get
        Set(ByVal value As String)
            _resFormaPagtoAlmoco = value
        End Set
    End Property
    Public Property resFormaPagtoJantar() As String
        Get
            Return _resFormaPagtoJantar
        End Get
        Set(ByVal value As String)
            _resFormaPagtoJantar = value
        End Set
    End Property
    Public Property resSexo() As Char
        Get
            Return _resSexo
        End Get
        Set(ByVal value As Char)
            _resSexo = value
        End Set
    End Property
    Public Property resContato() As String
        Get
            Return _resContato
        End Get
        Set(ByVal value As String)
            _resContato = value
        End Set
    End Property
    Public Property resValorDesconto() As Decimal
        Get
            Return _resValorDesconto
        End Get
        Set(ByVal value As Decimal)
            _resValorDesconto = value
        End Set
    End Property
    Public Property resDtLimiteRetorno() As String
        Get
            Return _resDtLimiteRetorno
        End Get
        Set(ByVal value As String)
            _resDtLimiteRetorno = value
        End Set
    End Property
    Public Property resDtLimiteCancela() As String
        Get
            Return _resDtLimiteCancela
        End Get
        Set(ByVal value As String)
            _resDtLimiteCancela = value
        End Set
    End Property
    Public Property resEstadoCivil() As Char
        Get
            Return _resEstadoCivil
        End Get
        Set(ByVal value As Char)
            _resEstadoCivil = value
        End Set
    End Property
    Public Property resLogradouro() As String
        Get
            Return _resLogradouro
        End Get
        Set(ByVal value As String)
            _resLogradouro = value
        End Set
    End Property
    Public Property resNumero() As String
        Get
            Return _resNumero
        End Get
        Set(ByVal value As String)
            _resNumero = value
        End Set
    End Property
    Public Property resQuadra() As String
        Get
            Return _resQuadra
        End Get
        Set(ByVal value As String)
            _resQuadra = value
        End Set
    End Property
    Public Property resLote() As String
        Get
            Return _resLote
        End Get
        Set(ByVal value As String)
            _resLote = value
        End Set
    End Property
    Public Property resComplemento() As String
        Get
            Return _resComplemento
        End Get
        Set(ByVal value As String)
            _resComplemento = value
        End Set
    End Property
    Public Property resBairro() As String
        Get
            Return _resBairro
        End Get
        Set(ByVal value As String)
            _resBairro = value
        End Set
    End Property
    Public Property resCapitalGoias() As Char
        Get
            Return _resCapitalGoias
        End Get
        Set(ByVal value As Char)
            _resCapitalGoias = value
        End Set
    End Property
    Public Property resCidade() As String
        Get
            Return _resCidade
        End Get
        Set(ByVal value As String)
            _resCidade = value
        End Set
    End Property
    Public Property resCep() As String
        Get
            Return _resCep
        End Get
        Set(ByVal value As String)
            _resCep = value
        End Set
    End Property
    Public Property resColoniaFeriasDes() As Char
        Get
            Return _resColoniaFeriasDes
        End Get
        Set(ByVal value As Char)
            _resColoniaFeriasDes = value
        End Set
    End Property
    Public Property resCapitalGoiasDes() As Char
        Get
            Return _resCapitalGoiasDes
        End Get
        Set(ByVal value As Char)
            _resCapitalGoiasDes = value
        End Set
    End Property
    Public Property resCidadeDes() As String
        Get
            Return _resCidadeDes
        End Get
        Set(ByVal value As String)
            _resCidadeDes = value
        End Set
    End Property
    Public Property resFoneComercial() As String
        Get
            Return _resFoneComercial
        End Get
        Set(ByVal value As String)
            _resFoneComercial = value
        End Set
    End Property
    Public Property resFoneResidencial() As String
        Get
            Return _resFoneResidencial
        End Get
        Set(ByVal value As String)
            _resFoneResidencial = value
        End Set
    End Property
    Public Property resFax() As String
        Get
            Return _resFax
        End Get
        Set(ByVal value As String)
            _resFax = value
        End Set
    End Property
    Public Property resCelular() As String
        Get
            Return _resCelular
        End Get
        Set(ByVal value As String)
            _resCelular = value
        End Set
    End Property
    Public Property resEmail() As String
        Get
            Return _resEmail
        End Get
        Set(ByVal value As String)
            _resEmail = value
        End Set
    End Property
    Public Property resDtNascimento() As String
        Get
            Return _resDtNascimento
        End Get
        Set(ByVal value As String)
            _resDtNascimento = value
        End Set
    End Property
    Public Property resRG() As String
        Get
            Return _resRG
        End Get
        Set(ByVal value As String)
            _resRG = value
        End Set
    End Property
    Public Property resCPF_CGC() As String
        Get
            Return _resCPF_CGC
        End Get
        Set(ByVal value As String)
            _resCPF_CGC = value
        End Set
    End Property
    Public Property resVlrAPagar() As Decimal
        Get
            Return _resVlrAPagar
        End Get
        Set(ByVal value As Decimal)
            _resVlrAPagar = value
        End Set
    End Property
    Public Property resVlrPago() As Decimal
        Get
            Return _resVlrPago
        End Get
        Set(ByVal value As Decimal)
            _resVlrPago = value
        End Set
    End Property
    Public Property resMemorando() As String
        Get
            Return _resMemorando
        End Get
        Set(ByVal value As String)
            _resMemorando = value
        End Set
    End Property
    Public Property resEmissor() As Integer
        Get
            Return _resEmissor
        End Get
        Set(ByVal value As Integer)
            _resEmissor = value
        End Set
    End Property
    Public Property resCatCobranca() As Long
        Get
            Return _resCatCobranca
        End Get
        Set(ByVal value As Long)
            _resCatCobranca = value
        End Set
    End Property
    Public Property resFormaPagamento() As String
        Get
            Return _resFormaPagamento
        End Get
        Set(ByVal value As String)
            _resFormaPagamento = value
        End Set
    End Property
    Public Property resHotelExcursao() As String
        Get
            Return _resHotelExcursao
        End Get
        Set(ByVal value As String)
            _resHotelExcursao = value
        End Set
    End Property
    Public Property resUsuario() As String
        Get
            Return _resUsuario
        End Get
        Set(ByVal value As String)
            _resUsuario = value
        End Set
    End Property
    Public Property resUsuarioData() As String
        Get
            Return _resUsuarioData
        End Get
        Set(ByVal value As String)
            _resUsuarioData = value
        End Set
    End Property
    Public Property resQtdeLista() As Integer
        Get
            Return _resQtdeLista
        End Get
        Set(ByVal value As Integer)
            _resQtdeLista = value
        End Set
    End Property
    Public Property resValorDescontoPorInteg() As Decimal
        Get
            Return _resValorDescontoPorInteg
        End Get
        Set(ByVal value As Decimal)
            _resValorDescontoPorInteg = value
        End Set
    End Property
    Public Property resPernoite() As Char
        Get
            Return _resPernoite
        End Get
        Set(ByVal value As Char)
            _resPernoite = value
        End Set
    End Property
    Public Property resDtInsercao() As String
        Get
            Return _resDtInsercao
        End Get
        Set(ByVal value As String)
            _resDtInsercao = value
        End Set
    End Property
    Public Property resIdWeb() As Long
        Get
            Return _resIdWeb
        End Get
        Set(ByVal value As Long)
            _resIdWeb = value
        End Set
    End Property
    Public Property resAlmocoRestaurante() As Char
        Get
            Return _resAlmocoRestaurante
        End Get
        Set(ByVal value As Char)
            _resAlmocoRestaurante = value
        End Set
    End Property
    Public Property resDtGrupoConfirmacao() As String
        Get
            Return _resDtGrupoConfirmacao
        End Get
        Set(ByVal value As String)
            _resDtGrupoConfirmacao = value
        End Set
    End Property
    Public Property resDtGrupoListagem() As String
        Get
            Return _resDtGrupoListagem
        End Get
        Set(ByVal value As String)
            _resDtGrupoListagem = value
        End Set
    End Property
    Public Property resDtGrupoPgtoSinal() As String
        Get
            Return _resDtGrupoPgtoSinal
        End Get
        Set(ByVal value As String)
            _resDtGrupoPgtoSinal = value
        End Set
    End Property
    Public Property resDtGrupoPgtoTotal() As String
        Get
            Return _resDtGrupoPgtoTotal
        End Get
        Set(ByVal value As String)
            _resDtGrupoPgtoTotal = value
        End Set
    End Property
    Public Property resDtGrupoAptoReservado() As String
        Get
            Return _resDtGrupoAptoReservado
        End Get
        Set(ByVal value As String)
            _resDtGrupoAptoReservado = value
        End Set
    End Property
    Public Property resGrupoConfirmacao() As Char
        Get
            Return _resGrupoConfirmacao
        End Get
        Set(ByVal value As Char)
            _resGrupoConfirmacao = value
        End Set
    End Property
    Public Property resPasseioPromovidoCEREC() As Char
        Get
            Return _resPasseioPromovidoCEREC
        End Get
        Set(ByVal value As Char)
            _resPasseioPromovidoCEREC = value
        End Set
    End Property
    Public Property resValidade() As String
        Get
            Return _resValidade
        End Get
        Set(ByVal value As String)
            _resValidade = value
        End Set
    End Property
    Public Property resLocalSaida() As String
        Get
            Return _resLocalSaida
        End Get
        Set(ByVal value As String)
            _resLocalSaida = value
        End Set
    End Property
    Public Property resHoraSaida() As String
        Get
            Return _resHoraSaida
        End Get
        Set(ByVal value As String)
            _resHoraSaida = value
        End Set
    End Property
    Public Property orgGrupo() As Char
        Get
            Return _orgGrupo
        End Get
        Set(ByVal value As Char)
            _orgGrupo = value
        End Set
    End Property
    Public Property resSalario() As String
        Get
            Return _resSalario
        End Get
        Set(ByVal value As String)
            _resSalario = value
        End Set
    End Property
    Public Property resEscolaridade() As String
        Get
            Return _resEscolaridade
        End Get
        Set(ByVal value As String)
            _resEscolaridade = value
        End Set
    End Property

    Public Property resTipo() As String
        Get
            Return _resTipo
        End Get
        Set(value As String)
            _resTipo = value
        End Set
    End Property

End Class
