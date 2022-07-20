Imports Microsoft.VisualBasic

Public Class ManutencaoVO
    Private _ApaId As String
    Private _ApaDesc As String
    Private _ApaStatus As String
    Private _ApaHTML As String
    Private _TipoAcomodacao As String
    'DISPONIBILIZANDO APTO PARA MANUTENÇÃO'
    Private _ManId As Long
    Private _ManDescricaoManut As String
    Private _Usuario As String
    'HISTÓRICO DE MANUTENÇÃO
    Private _ManAbertura As String
    Private _ManHorasAbertura As String
    Private _ManConclusao As String
    Private _ManHorasConclusao As String
    Private _ManRealizado As String
    Private _TempoDiaConserto As String
    Private _TempoHoraConserto As String
    Private _ManDataAbertura As String
    Private _SolDataFim As String
    Private _ApaCCusto As String
    Private _ApaArea As String

    Public Property ApaId() As String
        Get
            Return Me._ApaId
        End Get
        Set(ByVal value As String)
            If (_ApaId <> value) Then
                Me._ApaId = value
            End If
        End Set
    End Property
    Public Property ApaDesc() As String
        Get
            Return Me._ApaDesc
        End Get
        Set(ByVal value As String)
            If (_ApaDesc <> value) Then
                Me._ApaDesc = value
            End If
        End Set
    End Property
    Public Property ApaStatus() As String
        Get
            Return Me._ApaStatus
        End Get
        Set(ByVal value As String)
            If (_ApaStatus <> value) Then
                Me._ApaStatus = value
            End If
        End Set
    End Property
    Public Property ApaHTML() As String
        Get
            Return Me._ApaHTML
        End Get
        Set(ByVal value As String)
            If (_ApaHTML <> value) Then
                Me._ApaHTML = value
            End If
        End Set
    End Property
    Public Property TipoAcomodacao() As String
        Get
            Return Me._TipoAcomodacao
        End Get
        Set(ByVal value As String)
            If (_TipoAcomodacao <> value) Then
                Me._TipoAcomodacao = value
            End If
        End Set
    End Property

    Public Property ManId() As Long
        Get
            Return Me._ManId
        End Get
        Set(ByVal value As Long)
            If (_ManId <> value) Then
                Me._ManId = value
            End If
        End Set
    End Property
    Public Property ManDescricaoManut() As String
        Get
            Return Me._ManDescricaoManut
        End Get
        Set(ByVal value As String)
            If (_ManDescricaoManut <> value) Then
                Me._ManDescricaoManut = value
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

    Public Property ManAbertura() As String
        Get
            Return Me._ManAbertura
        End Get
        Set(ByVal value As String)
            If (_ManAbertura <> value) Then
                Me._ManAbertura = value
            End If
        End Set
    End Property
    Public Property ManHorasAbertura() As String
        Get
            Return Me._ManHorasAbertura
        End Get
        Set(ByVal value As String)
            If (_ManHorasAbertura <> value) Then
                Me._ManHorasAbertura = value
            End If
        End Set
    End Property
    Public Property ManConclusao() As String
        Get
            Return Me._ManConclusao
        End Get
        Set(ByVal value As String)
            If (_ManConclusao <> value) Then
                Me._ManConclusao = value
            End If
        End Set
    End Property
    Public Property ManHorasConclusao() As String
        Get
            Return Me._ManHorasConclusao
        End Get
        Set(ByVal value As String)
            If (_ManHorasConclusao <> value) Then
                Me._ManHorasConclusao = value
            End If
        End Set
    End Property
    Public Property ManRealizado() As String
        Get
            Return Me._ManRealizado
        End Get
        Set(ByVal value As String)
            If (_ManRealizado <> value) Then
                Me._ManRealizado = value
            End If
        End Set
    End Property
    Public Property TempoDiaConserto() As String
        Get
            Return Me._TempoDiaConserto
        End Get
        Set(ByVal value As String)
            If (_TempoDiaConserto <> value) Then
                Me._TempoDiaConserto = value
            End If
        End Set
    End Property

    Public Property TempoHoraConserto() As String
        Get
            Return Me._TempoHoraConserto
        End Get
        Set(ByVal value As String)
            If (_TempoHoraConserto <> value) Then
                Me._TempoHoraConserto = value
            End If
        End Set
    End Property
    Public Property ManDataAbertura() As String
        Get
            Return Me._ManDataAbertura
        End Get
        Set(ByVal value As String)
            If (_ManDataAbertura <> value) Then
                Me._ManDataAbertura = value
            End If
        End Set
    End Property
    Public Property SolDataFim() As String
        Get
            Return Me._SolDataFim
        End Get
        Set(ByVal value As String)
            If (_SolDataFim <> value) Then
                Me._SolDataFim = value
            End If
        End Set
    End Property
    Public Property ApaCCusto() As String
        Get
            Return Me._ApaCCusto
        End Get
        Set(ByVal value As String)
            If (_ApaCCusto <> value) Then
                Me._ApaCCusto = value
            End If
        End Set
    End Property
    Public Property ApaArea() As String
        Get
            Return Me._ApaArea
        End Get
        Set(ByVal value As String)
            If (_ApaArea <> value) Then
                Me._ApaArea = value
            End If
        End Set
    End Property
End Class
