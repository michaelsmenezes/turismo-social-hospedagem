Imports Microsoft.VisualBasic

Public Class MinisterioTurismoVO
    Private _Data As String
    Private _Entradas As Long
    Private _Saidas As Long
    Private _QtdePessoasDiaMesAnterior As Long
    Private _Apartamentos As Long
    Private _Leitos As Long
    Private _TotLeitos As Long
    Private _Hospedados As Long
    Private _SaidasUH As Long
    Private _EntradasUH As Long

    Public Property Data() As String
        Get
            Return _Data
        End Get
        Set(ByVal value As String)
            _Data = value
        End Set
    End Property
    Public Property Entradas() As Long
        Get
            Return _Entradas
        End Get
        Set(ByVal value As Long)
            _Entradas = value
        End Set
    End Property
    Public Property Saidas() As Long
        Get
            Return _Saidas
        End Get
        Set(ByVal value As Long)
            _Saidas = value
        End Set
    End Property
    Public Property QtdePessoasDiaMesAnterior() As Long
        Get
            Return _QtdePessoasDiaMesAnterior
        End Get
        Set(ByVal value As Long)
            _QtdePessoasDiaMesAnterior = value
        End Set
    End Property
    Public Property Apartamentos() As Long
        Get
            Return _Apartamentos
        End Get
        Set(ByVal value As Long)
            _Apartamentos = value
        End Set
    End Property
    Public Property Leitos() As Long
        Get
            Return _Leitos
        End Get
        Set(ByVal value As Long)
            _Leitos = value
        End Set
    End Property
    Public Property TotLeitos() As Long
        Get
            Return _TotLeitos
        End Get
        Set(ByVal value As Long)
            _TotLeitos = value
        End Set
    End Property
    Public Property Hospedados() As Long
        Get
            Return _Hospedados
        End Get
        Set(ByVal value As Long)
            _Hospedados = value
        End Set
    End Property

    Public Property SaidasUH() As Long
        Get
            Return _SaidasUH
        End Get
        Set(ByVal value As Long)
            _SaidasUH = value
        End Set
    End Property

    Public Property EntradasUH() As Long
        Get
            Return _EntradasUH
        End Get
        Set(ByVal value As Long)
            _EntradasUH = value
        End Set
    End Property
End Class
