Imports Microsoft.VisualBasic
Public Class HistoricoPassantesVO
    'Herdando os atributos da classe hospedesVO
    Inherits HistoricoHospedesVO
    'Atributos da classe PassantesVO
    Private _Observacao As String
    Private _Categoria As String
    Private _Placa As String
    Private _Situacao As String
    Private _Atendente As String
    Private _Idade As String
    Private _VinculoRefeicao As String

    Public Property Observacao() As String
        Get
            Return Me._Observacao
        End Get
        Set(ByVal value As String)
            If (_Observacao <> value) Then
                Me._Observacao = value
            End If
        End Set
    End Property

    Public Property Categoria() As String
        Get
            Return Me._Categoria
        End Get
        Set(ByVal value As String)
            If (_Categoria <> value) Then
                Me._Categoria = value
            End If
        End Set
    End Property
    Public Property Placa() As String
        Get
            Return Me._Placa
        End Get
        Set(ByVal value As String)
            If (_Placa <> value) Then
                Me._Placa = value
            End If
        End Set
    End Property
    Public Property Situacao() As String
        Get
            Return Me._Situacao
        End Get
        Set(ByVal value As String)
            If (_Situacao <> value) Then
                Me._Situacao = value
            End If
        End Set
    End Property
    Public Property Atendente() As String
        Get
            Return Me._Atendente
        End Get
        Set(ByVal value As String)
            If (_Atendente <> value) Then
                Me._Atendente = value
            End If
        End Set
    End Property
    Public Property Idade() As String
        Get
            Return _Idade
        End Get
        Set(ByVal value As String)
            _Idade = value
        End Set
    End Property
    Public Property VinculoRefeicao() As String
        Get
            Return _VinculoRefeicao
        End Get
        Set(ByVal value As String)
            _VinculoRefeicao = value
        End Set
    End Property

End Class
