Imports Microsoft.VisualBasic

Public Class ItemConsertoVO
    Private _Codigo As Long
    Private _Descricao As String
    Private _Usuario As String
    Private _DataHora As String
    Private _SetorExecutante As String
    Private _IdUnidade As String

    Public Property Codigo() As Long
        Get
            Return Me._Codigo
        End Get
        Set(ByVal value As Long)
            If (_Codigo <> value) Then
                Me._Codigo = value
            End If
        End Set
    End Property
    Public Property Descricao() As String
        Get
            Return Me._Descricao
        End Get
        Set(ByVal value As String)
            If (_Descricao <> value) Then
                Me._Descricao = value
            End If
        End Set
    End Property
    Public Property Usuario() As String
        Get
            Return Me._Usuario
        End Get
        Set(ByVal value As String)
            If (_Usuario <> value) Then
                Me._Usuario = value
            End If
        End Set
    End Property
    Public Property DataHora() As String
        Get
            Return Me._DataHora
        End Get
        Set(ByVal value As String)
            If (_Usuario <> value) Then
                Me._DataHora = value
            End If
        End Set
    End Property
    Public Property SetorExecutante() As String
        Get
            Return Me._SetorExecutante
        End Get
        Set(ByVal value As String)
            If (_SetorExecutante <> value) Then
                Me._SetorExecutante = value
            End If
        End Set
    End Property

    Public Property IdUnidade() As String
        Get
            Return _IdUnidade
        End Get
        Set(ByVal value As String)
            _IdUnidade = value
        End Set
    End Property
End Class
