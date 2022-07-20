Imports Microsoft.VisualBasic

Public Class AlaVO
    Private _AlaId As Long
    Private _AlaNome As String
    Private _AlaDescricao As String
    Private _CamId As Long
    Private _CamNome As String
    Public Property AlaId() As Long
        Get
            Return (Me._AlaId)
        End Get
        Set(ByVal value As Long)
            If (_AlaId <> value) Then
                Me._AlaId = value
            End If
        End Set
    End Property
    Public Property AlaNome() As String
        Get
            Return Me._AlaNome
        End Get
        Set(ByVal value As String)
            If (_AlaNome <> value) Then
                Me._AlaNome = value
            End If
        End Set
    End Property
    Public Property AlaDescricao() As String
        Get
            Return Me._AlaDescricao
        End Get
        Set(ByVal value As String)
            If (_AlaDescricao <> value) Then
                Me._AlaDescricao = value
            End If
        End Set
    End Property
    Public Property CamId() As Long
        Get
            Return Me._CamId
        End Get
        Set(ByVal value As Long)
            If (_CamId <> value) Then
                Me._CamId = value
            End If
        End Set
    End Property
    Public Property CamNome() As String
        Get
            Return Me._CamNome
        End Get
        Set(ByVal value As String)
            If (_CamNome <> value) Then
                Me._CamNome = value
            End If
        End Set
    End Property
End Class
