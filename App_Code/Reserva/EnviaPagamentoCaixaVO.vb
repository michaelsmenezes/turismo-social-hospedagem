Imports Microsoft.VisualBasic

Public Class EnviaPagamentoCaixaVO
    'Parametros
    Private _Reserva As Integer
    Private _Usuario As String
    Private _Pencentual As Decimal
    'Resultado da Select de inserção
    Private _PpcId As String
    Private _ResId As String
    Private _IntId As String
    Private _PpcData As String
    Private _TipOprCod As String
    Private _PpcValor As String
    Private _PpcTipo As String
    Private _PpcDescricao As String
    Private _PpcStatus As String
    Private _PpcOrigem As String
    Private _CSeId As String
    Private _ProId As String
    Private _PpcSacado As String
    Private _PpcCheckin As String
    Private _PpcCheckout As String
    'Consulta dos dados para inserção futura
    Private _Operador As String
    Private _Data As String
    Private _ResCaracteristica As String
    Private _ResNome As String
    Private _ResDataIni As String
    Private _ResDataFim As String
    Private _VlrVen As String
    Private _VlrVenPg As String
    Private _VlrH As String
    Private _VlrD As String
    Private _VlrR As String
    Private _VlrHPago As String
    Private _VlrDPago As String
    Private _VlrRPago As String


    Public Property Reserva() As Integer
        Get
            Return _Reserva
        End Get
        Set(ByVal value As Integer)
            _Reserva = value
        End Set
    End Property
    Public Property Usuario() As String
        Get
            Return _Usuario
        End Get
        Set(ByVal value As String)
            _Usuario = value
        End Set
    End Property
    Public Property Pencentual() As Decimal
        Get
            Return _Pencentual
        End Get
        Set(ByVal value As Decimal)
            _Pencentual = value
        End Set
    End Property

    Public Property PpcId As String
        Get
            Return Me._PpcId
        End Get
        Set(value As String)
            _PpcId = value
        End Set
    End Property
    Public Property ResId As String
        Get
            Return Me._ResId
        End Get
        Set(value As String)
            _ResId = value
        End Set
    End Property
    Public Property IntId As String
        Get
            Return Me._IntId
        End Get
        Set(value As String)
            _IntId = value
        End Set
    End Property
    Public Property PpcData As String
        Get
            Return Me._PpcData
        End Get
        Set(value As String)
            _PpcData = value
        End Set
    End Property
    Public Property TipOprCod As String
        Get
            Return Me._TipOprCod
        End Get
        Set(value As String)
            _TipOprCod = value
        End Set
    End Property
    Public Property PpcValor As String
        Get
            Return Me._PpcValor
        End Get
        Set(value As String)
            _PpcValor = value
        End Set
    End Property
    Public Property PpcTipo As String
        Get
            Return Me._PpcTipo
        End Get
        Set(value As String)
            _PpcTipo = value
        End Set
    End Property
    Public Property PpcDescricao As String
        Get
            Return Me._PpcDescricao
        End Get
        Set(value As String)
            _PpcDescricao = value
        End Set
    End Property
    Public Property PpcStatus As String
        Get
            Return Me._PpcStatus
        End Get
        Set(value As String)
            _PpcStatus = value
        End Set
    End Property
    Public Property PpcOrigem As String
        Get
            Return Me._PpcOrigem
        End Get
        Set(value As String)
            _PpcOrigem = value
        End Set
    End Property
    Public Property CSeId As String
        Get
            Return Me._CSeId
        End Get
        Set(value As String)
            _CSeId = value
        End Set
    End Property
    Public Property ProId As String
        Get
            Return Me._ProId
        End Get
        Set(value As String)
            _ProId = value
        End Set
    End Property
    Public Property PpcSacado As String
        Get
            Return Me._PpcSacado
        End Get
        Set(value As String)
            _PpcSacado = value
        End Set
    End Property
    Public Property PpcCheckin As String
        Get
            Return Me._PpcCheckin
        End Get
        Set(value As String)
            _PpcCheckin = value
        End Set
    End Property
    Public Property PpcCheckout As String
        Get
            Return Me._PpcCheckout
        End Get
        Set(value As String)
            _PpcCheckout = value
        End Set
    End Property

    Public Property Operador As String
        Get
            Return Me._Operador
        End Get
        Set(value As String)
            _Operador = value
        End Set
    End Property
    Public Property Data As String
        Get
            Return Me._Data
        End Get
        Set(value As String)
            _Data = value
        End Set
    End Property
    Public Property ResCaracteristica As String
        Get
            Return Me._ResCaracteristica
        End Get
        Set(value As String)
            _ResCaracteristica = value
        End Set
    End Property
    Public Property ResNome As String
        Get
            Return Me._ResNome
        End Get
        Set(value As String)
            _ResNome = value
        End Set
    End Property
    Public Property ResDataIni As String
        Get
            Return Me._ResDataIni
        End Get
        Set(value As String)
            _ResDataIni = value
        End Set
    End Property
    Public Property ResDataFim As String
        Get
            Return Me._ResDataFim
        End Get
        Set(value As String)
            _ResDataFim = value
        End Set
    End Property
    Public Property VlrVen As String
        Get
            Return Me._VlrVen
        End Get
        Set(value As String)
            _VlrVen = value
        End Set
    End Property
    Public Property VlrVenPg As String
        Get
            Return Me._VlrVenPg
        End Get
        Set(value As String)
            _VlrVenPg = value
        End Set
    End Property
    Public Property VlrH As String
        Get
            Return Me._VlrH
        End Get
        Set(value As String)
            _VlrH = value
        End Set
    End Property
    Public Property VlrD As String
        Get
            Return Me._VlrD
        End Get
        Set(value As String)
            _VlrD = value
        End Set
    End Property
    Public Property VlrR As String
        Get
            Return Me._VlrR
        End Get
        Set(value As String)
            _VlrR = value
        End Set
    End Property
    Public Property VlrHPago As String
        Get
            Return Me._VlrHPago
        End Get
        Set(value As String)
            _VlrHPago = value
        End Set
    End Property
    Public Property VlrDPago As String
        Get
            Return Me._VlrDPago
        End Get
        Set(value As String)
            _VlrDPago = value
        End Set
    End Property
    Public Property VlrRPago As String
        Get
            Return Me._VlrRPago
        End Get
        Set(value As String)
            _VlrRPago = value
        End Set
    End Property
End Class
