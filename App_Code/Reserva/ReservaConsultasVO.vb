Imports Microsoft.VisualBasic

Public Class ReservaConsultasVO
    Private _BolImpId As String
    Private _ResId As String
    Private _BolTipo As String
    Private _BolImpValor As String
    Private _BolImpDtDocumento As String
    Private _BolImpDtPagamento As String
    Private _BolImpEndereco As String
    Private _BolImpSacado As String
    Private _BolImpCGC_CPF As String
    Private _BolImpCodIpte As String
    Private _BolImpUsuario As String
    Private _BolNitCartaoCredito As String
    Private _BolNsuCartaoCredito As String
    Private _BolLocalidadeCartaoCredito As String
    Private _BolConfirmacaoPagto As String
    Private _BolNossoNumero As String
    Private _StatusPagamento As String
    Private _BolParcelasCartaoCredito As String
    Private _banDescricao As String
    Private _QtdPessoas As String
    Private _resEmail As String
    Private _DataLimitePagamento As String
    Private _BolNumeroAutorizacaoCartaoCredito As String
    Private _BloId As String
    Private _ResStatus As String
    Private _Integrantes As String


    'Exibe observações do passado na tela de reserva
    Private _ResCPF_CGC As String
    Private _ResMatricula As String
    Private _ResObs As String
    Private _Periodo As String

    Public Property Integrantes() As String
        Get
            Return _Integrantes
        End Get
        Set(ByVal value As String)
            _Integrantes = value
        End Set
    End Property
    Public Property Periodo() As String
        Get
            Return _Periodo
        End Get
        Set(ByVal value As String)
            _Periodo = value
        End Set
    End Property

    Public Property ResObs() As String
        Get
            Return _ResObs
        End Get
        Set(ByVal value As String)
            _ResObs = value
        End Set
    End Property

    Public Property ResMatricula() As String
        Get
            Return _ResMatricula
        End Get
        Set(ByVal value As String)
            _ResMatricula = value
        End Set
    End Property

    Public Property ResCPF_CGC() As String
        Get
            Return _ResCPF_CGC
        End Get
        Set(ByVal value As String)
            _ResCPF_CGC = value
        End Set
    End Property



    Public Property ResStatus() As String
        Get
            Return _ResStatus
        End Get
        Set(ByVal value As String)
            _ResStatus = value
        End Set
    End Property


    Public Property BolImpId As String
        Get
            Return _BolImpId
        End Get
        Set(value As String)
            _BolImpId = value
        End Set
    End Property


    Public Property ResId As String
        Get
            Return _ResId
        End Get
        Set(value As String)
            _ResId = value
        End Set
    End Property
    Public Property BolTipo As String
        Get
            Return _BolTipo
        End Get
        Set(value As String)
            _BolTipo = value
        End Set
    End Property
    Public Property BolImpValor As String
        Get
            Return _BolImpValor
        End Get
        Set(value As String)
            _BolImpValor = value
        End Set
    End Property
    Public Property BolImpDtDocumento As String
        Get
            Return _BolImpDtDocumento
        End Get
        Set(value As String)
            _BolImpDtDocumento = value
        End Set
    End Property
    Public Property BolImpDtPagamento As String
        Get
            Return _BolImpDtPagamento
        End Get
        Set(value As String)
            _BolImpDtPagamento = value
        End Set
    End Property
    Public Property BolImpEndereco As String
        Get
            Return _BolImpEndereco
        End Get
        Set(value As String)
            _BolImpEndereco = value
        End Set
    End Property
    Public Property BolImpSacado As String
        Get
            Return _BolImpSacado
        End Get
        Set(value As String)
            _BolImpSacado = value
        End Set
    End Property
    Public Property BolImpCGC_CPF As String
        Get
            Return _BolImpCGC_CPF
        End Get
        Set(value As String)
            _BolImpCGC_CPF = value
        End Set
    End Property
    Public Property BolImpCodIpte As String
        Get
            Return _BolImpCodIpte
        End Get
        Set(value As String)
            _BolImpCodIpte = value
        End Set
    End Property
    Public Property BolImpUsuario As String
        Get
            Return _BolImpUsuario
        End Get
        Set(value As String)
            _BolImpUsuario = value
        End Set
    End Property

    Public Property BolNitCartaoCredito As String
        Get
            Return _BolNitCartaoCredito
        End Get
        Set(value As String)
            _BolNitCartaoCredito = value

        End Set
    End Property
    Public Property BolNsuCartaoCredito As String
        Get
            Return _BolNsuCartaoCredito
        End Get
        Set(value As String)
            _BolNsuCartaoCredito = value
        End Set
    End Property
    Public Property BolLocalidadeCartaoCredito As String
        Get
            Return _BolLocalidadeCartaoCredito
        End Get
        Set(value As String)
            _BolLocalidadeCartaoCredito = value
        End Set
    End Property
    Public Property BolConfirmacaoPagto As String
        Get
            Return _BolConfirmacaoPagto
        End Get
        Set(value As String)
            _BolConfirmacaoPagto = value
        End Set
    End Property

    Public Property BolNossoNumero As String
        Get
            Return _BolNossoNumero
        End Get
        Set(value As String)
            _BolNossoNumero = value

        End Set
    End Property

    Public Property StatusPagamento As String
        Get
            Return _StatusPagamento
        End Get
        Set(value As String)
            _StatusPagamento = value
        End Set
    End Property
    Public Property BolParcelasCartaoCredito As String
        Get
            Return _BolParcelasCartaoCredito
        End Get
        Set(value As String)
            _BolParcelasCartaoCredito = value
        End Set
    End Property
    Public Property banDescricao As String
        Get
            Return _banDescricao
        End Get
        Set(value As String)
            _banDescricao = value
        End Set
    End Property
    Public Property QtdPessoas As String
        Get
            Return _QtdPessoas
        End Get
        Set(value As String)
            _QtdPessoas = value
        End Set
    End Property
    Public Property resEmail As String
        Get
            Return _resEmail
        End Get
        Set(value As String)
            _resEmail = value
        End Set
    End Property

    Public Property DataLimitePagamento As String
        Get
            Return _DataLimitePagamento
        End Get
        Set(value As String)
            _DataLimitePagamento = value
        End Set
    End Property

    Public Property BolNumeroAutorizacaoCartaoCredito As String
        Get
            Return _BolNumeroAutorizacaoCartaoCredito
        End Get
        Set(value As String)
            _BolNumeroAutorizacaoCartaoCredito = value
        End Set
    End Property
    Public Property BloId As String
        Get
            Return _BloId
        End Get
        Set(value As String)
            _BloId = value
        End Set
    End Property

End Class
