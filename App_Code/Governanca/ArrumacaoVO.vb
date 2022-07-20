Imports Microsoft.VisualBasic

Public Class ArrumacaoVO
    Private _ApaId As String
    Private _ApaDesc As String
    Private _TipoAcomodacao As String
    Private _SaidoManutencao As String
    Private _AgoId As String
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
    Public Property SaidoManutencao() As String
        Get
            Return Me._SaidoManutencao
        End Get
        Set(ByVal value As String)
            If _SaidoManutencao <> value Then
                Me._SaidoManutencao = value
            End If
        End Set
    End Property
    Public Property AgoId() As String
        Get
            Return Me._AgoId
        End Get
        Set(ByVal value As String)
            If (_AgoId <> value) Then
                Me._AgoId = value
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
