Imports Microsoft.VisualBasic

Public Class RefeicaoManualVO
    Private _RefId As Long
    Private _IntId As Long
    Private _RefData As String
    Private _RefTipo As String
    Private _RefQtde As Long
    Private _RefCortesia As String
    Private _RefMotivoInsManual As String
    Private _RefUsuario As String
    Private _RefUsuarioData As String
    Public Property RefUsuarioData() As String
        Get
            Return _RefUsuarioData
        End Get
        Set(ByVal value As String)
            _RefUsuarioData = value
        End Set
    End Property

    Public Property RefUsuario() As String
        Get
            Return _RefUsuario
        End Get
        Set(ByVal value As String)
            _RefUsuario = value
        End Set
    End Property

    Public Property RefId() As Long
        Get
            Return Me._RefId
        End Get
        Set(ByVal value As Long)
            If (_RefId <> value) Then
                Me._RefId = value
            End If
        End Set
    End Property

    Public Property IntId() As Long
        Get
            Return Me._IntId
        End Get
        Set(ByVal value As Long)
            If (_IntId <> value) Then
                Me._IntId = value
            End If
        End Set
    End Property
    Public Property RefData() As String
        Get
            Return Me._RefData
        End Get
        Set(ByVal value As String)
            If (_RefData <> value) Then
                Me._RefData = value
            End If
        End Set
    End Property
    Public Property RefTipo() As String
        Get
            Return Me._RefTipo
        End Get
        Set(ByVal value As String)
            If (_RefTipo <> value) Then
                Me._RefTipo = value
            End If
        End Set
    End Property
    Public Property RefQtde() As Long
        Get
            Return Me._RefQtde
        End Get
        Set(ByVal value As Long)
            If (_RefQtde <> value) Then
                Me._RefQtde = value
            End If
        End Set
    End Property
    Public Property RefCortesia() As String
        Get
            Return Me._RefCortesia
        End Get
        Set(ByVal value As String)
            If (_RefCortesia <> value) Then
                Me._RefCortesia = value
            End If
        End Set
    End Property
    Public Property RefMotivoInsManual() As String
        Get
            Return Me._RefMotivoInsManual
        End Get
        Set(ByVal value As String)
            If (RefMotivoInsManual <> value) Then
                Me._RefMotivoInsManual = value
            End If
        End Set
    End Property
End Class
