Imports Microsoft.VisualBasic

Public Class BoletoNetVO

    Private _Boleto As String
    Private _Cedente As String
    Private _AgeCodCed As String
    Private _DataDocumento As String
    Private _DataProces As String
    Private _Vencimento As String
    Private _Sacado As String
    Private _NossoNum As String
    Private _VlrDoc As String
    Private _Demonstrativo As String
    Private _Reserva As String
    Private _QtdePess As String
    Private _Periodo As String
    Private _Entrada As String
    Private _Saida As String
    Private _Banco As String
    Private _CodBanco As String
    Private _End1 As String
    Private _CodigoIPTE As String
    Private _CodigoBarra As String
    Private _Acomodacao As String
    Private _DetalhePessoa As String
    Private _DetalheValor As String
    Private _BolCodBarraNovoBoleto As String
    Private _BolCodDigitavelNovoBoleto As String
    Private _BolCodCedenteNovo As String
    Private _BolImpCGC_CPF As String
    Private _BolHTML As String
    Private _BolTipoParcela As String

    Private _QdtePess As String
    Private _Percentual As String
    Private _LocalSaida As String
    Private _HoraSaida As String
    Private _NomeResp As String
    Private _Destino As String

    'Informações de Endereço do cliente
    Private _ResNome As String
    Private _ResLogradouro As String
    Private _ResNumero As String
    Private _ResQuadra As String
    Private _ResLote As String
    Private _ResComplemento As String
    Private _ResBairro As String
    Private _ResCidade As String
    Private _UF As String
    Private _ResCep As String
    Private _ResCPFCGC As String


    Public Property Boleto() As String
        Get
            Return _Boleto
        End Get
        Set(ByVal value As String)
            _Boleto = value
        End Set
    End Property
    Public Property Cedente() As String
        Get
            Return _Cedente
        End Get
        Set(ByVal value As String)
            _Cedente = value
        End Set
    End Property
    Public Property AgeCodCed() As String
        Get
            Return _AgeCodCed
        End Get
        Set(ByVal value As String)
            _AgeCodCed = value
        End Set
    End Property
    Public Property DataDocumento() As String
        Get
            Return _DataDocumento
        End Get
        Set(ByVal value As String)
            _DataDocumento = value
        End Set
    End Property
    Public Property DataProces() As String
        Get
            Return _DataProces
        End Get
        Set(ByVal value As String)
            _DataProces = value
        End Set
    End Property
    Public Property Vencimento() As String
        Get
            Return _Vencimento
        End Get
        Set(ByVal value As String)
            _Vencimento = value
        End Set
    End Property
    Public Property Sacado() As String
        Get
            Return _Sacado
        End Get
        Set(ByVal value As String)
            _Sacado = value
        End Set
    End Property
    Public Property NossoNum() As String
        Get
            Return _NossoNum
        End Get
        Set(ByVal value As String)
            _NossoNum = value
        End Set
    End Property
    Public Property VlrDoc() As String
        Get
            Return _VlrDoc
        End Get
        Set(ByVal value As String)
            _VlrDoc = value
        End Set
    End Property
    Public Property Demonstrativo() As String
        Get
            Return _Demonstrativo
        End Get
        Set(ByVal value As String)
            _Demonstrativo = value
        End Set
    End Property
    Public Property Reserva() As String
        Get
            Return _Reserva
        End Get
        Set(ByVal value As String)
            _Reserva = value
        End Set
    End Property
    Public Property QtdePess() As String
        Get
            Return _QtdePess
        End Get
        Set(ByVal value As String)
            _QtdePess = value
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
    Public Property Entrada() As String
        Get
            Return _Entrada
        End Get
        Set(ByVal value As String)
            _Entrada = value
        End Set
    End Property
    Public Property Saida() As String
        Get
            Return _Saida
        End Get
        Set(ByVal value As String)
            _Saida = value
        End Set
    End Property
    Public Property Banco() As String
        Get
            Return _Banco
        End Get
        Set(ByVal value As String)
            _Banco = value
        End Set
    End Property
    Public Property CodBanco() As String
        Get
            Return _CodBanco
        End Get
        Set(ByVal value As String)
            _CodBanco = value
        End Set
    End Property
    Public Property End1() As String
        Get
            Return _End1
        End Get
        Set(ByVal value As String)
            _End1 = value
        End Set
    End Property
    Public Property CodigoIPTE() As String
        Get
            Return _CodigoIPTE
        End Get
        Set(ByVal value As String)
            _CodigoIPTE = value
        End Set
    End Property
    Public Property CodigoBarra() As String
        Get
            Return _CodigoBarra
        End Get
        Set(ByVal value As String)
            _CodigoBarra = value
        End Set
    End Property
    Public Property Acomodacao() As String
        Get
            Return _Acomodacao
        End Get
        Set(ByVal value As String)
            _Acomodacao = value
        End Set
    End Property
    Public Property DetalhePessoa() As String
        Get
            Return _DetalhePessoa
        End Get
        Set(ByVal value As String)
            _DetalhePessoa = value
        End Set
    End Property
    Public Property DetalheValor() As String
        Get
            Return _DetalheValor
        End Get
        Set(ByVal value As String)
            _DetalheValor = value
        End Set
    End Property
    Public Property BolCodBarraNovoBoleto As String
        Get
            Return _BolCodBarraNovoBoleto
        End Get
        Set(value As String)
            _BolCodBarraNovoBoleto = value
        End Set
    End Property
    Public Property BolCodDigitavelNovoBoleto As String
        Get
            Return _BolCodDigitavelNovoBoleto
        End Get
        Set(value As String)
            _BolCodDigitavelNovoBoleto = value
        End Set
    End Property
    Public Property BolCodCedenteNovo As String
        Get
            Return _BolCodCedenteNovo
        End Get
        Set(value As String)
            _BolCodCedenteNovo = value
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
    Public Property BolHTML As String
        Get
            Return _BolHTML
        End Get
        Set(value As String)
            _BolHTML = value
        End Set
    End Property

    Public Property BolTipoParcela As String
        Get
            Return _BolTipoParcela
        End Get
        Set(value As String)
            _BolTipoParcela = value
        End Set
    End Property


    Public Property QdtePess As String
        Get
            Return _QdtePess
        End Get
        Set(value As String)
            _QtdePess = value
        End Set
    End Property
    Public Property Percentual As String
        Get
            Return _Percentual
        End Get
        Set(value As String)
            _Percentual = value
        End Set
    End Property
    Public Property LocalSaida As String
        Get
            Return _LocalSaida
        End Get
        Set(value As String)
            _LocalSaida = value
        End Set
    End Property
    Public Property HoraSaida As String
        Get
            Return _HoraSaida
        End Get
        Set(value As String)
            _HoraSaida = value
        End Set
    End Property
    Public Property NomeResp As String
        Get
            Return _NomeResp
        End Get
        Set(value As String)
            _NomeResp = value
        End Set
    End Property
    Public Property Destino As String
        Get
            Return _Destino
        End Get
        Set(value As String)
            _Destino = value
        End Set
    End Property
    Public Property ResNome As String
        Get
            Return _ResNome
        End Get
        Set(value As String)
            _ResNome = value
        End Set
    End Property
    Public Property ResLogradouro As String
        Get
            Return _ResLogradouro
        End Get
        Set(value As String)
            _ResLogradouro = value
        End Set
    End Property
    Public Property ResNumero As String
        Get
            Return _ResNumero
        End Get
        Set(value As String)
            _ResNumero = value
        End Set
    End Property
    Public Property ResQuadra As String
        Get
            Return _ResQuadra
        End Get
        Set(value As String)
            _ResQuadra = value
        End Set
    End Property
    Public Property ResLote As String
        Get
            Return _ResLote
        End Get
        Set(value As String)
            _ResLote = value
        End Set
    End Property
    Public Property ResComplemento As String
        Get
            Return _ResComplemento
        End Get
        Set(value As String)
            _ResComplemento = value
        End Set
    End Property
    Public Property ResBairro As String
        Get
            Return _ResBairro
        End Get
        Set(value As String)
            _ResBairro = value
        End Set
    End Property
    Public Property ResCidade As String
        Get
            Return _ResCidade
        End Get
        Set(value As String)
            _ResCidade = value
        End Set
    End Property
    Public Property UF As String
        Get
            Return _UF
        End Get
        Set(value As String)
            _UF = value
        End Set
    End Property
    Public Property ResCep As String
        Get
            Return _ResCep
        End Get
        Set(value As String)
            _ResCep = value
        End Set
    End Property
    Public Property ResCPFCGC As String
        Get
            Return _ResCPFCGC
        End Get
        Set(value As String)
            _ResCPFCGC = value
        End Set
    End Property
End Class
