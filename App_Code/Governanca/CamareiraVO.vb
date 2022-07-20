Imports Microsoft.VisualBasic

Public Class CamareiraVO
    Private _CamId As Long
    Private _CamNome As String
    Private _CamSituacao As String
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
    Public Property CamSituacao() As String
        Get
            Return Me._CamSituacao
        End Get
        Set(ByVal value As String)
            If (_CamSituacao <> value) Then
                Me._CamSituacao = value
            End If
        End Set
    End Property
End Class
