Imports Microsoft.VisualBasic

Public Class LimpoVO
    Private _ApaId As String
    Private _ApaDesc As String
    Private _ApaStatus As String
    Private _ApaHTML As String
    Private _TipoAcomodacao As String
    Private _DataLimpoLog As String
    Private _UsuarioLimpoLog As String
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
    Public Property DataLimpoLog() As String
        Get
            Return Me._DataLimpoLog
        End Get
        Set(ByVal value As String)
            If (_DataLimpoLog <> value) Then
                Me._DataLimpoLog = value
            End If
        End Set
    End Property
    Public Property UsuarioLimpoLog() As String
        Get
            Return Me._UsuarioLimpoLog
        End Get
        Set(ByVal value As String)
            If (_UsuarioLimpoLog <> value) Then
                Me._UsuarioLimpoLog = value
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
