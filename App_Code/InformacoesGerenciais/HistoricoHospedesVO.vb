Imports Microsoft.VisualBasic

Public Class HistoricoHospedesVO
    Private _IntId As Long
    Private _ResId As Long
    Private _IntNome As String
    Private _ResNome As String
    Private _ApaId As Long
    Private _ApaDesc As String
    Private _DataIni As String
    Private _HoraIni As String
    Private _DataIniReal As String
    Private _HoraIniReal As String
    Private _DataFim As String
    Private _HoraFim As String
    Private _DataFimReal As String
    Private _HoraFimReal As String
    Private _IntDtNascimento As String
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
    Public Property ResId() As Long
        Get
            Return Me._ResId
        End Get
        Set(ByVal value As Long)
            If (_ResId <> value) Then
                Me._ResId = value
            End If
        End Set
    End Property
    Public Property IntNome() As String
        Get
            Return Me._IntNome
        End Get
        Set(ByVal value As String)
            If (_IntNome <> value) Then
                Me._IntNome = value
            End If
        End Set
    End Property
    Public Property ResNome() As String
        Get
            Return Me._ResNome
        End Get
        Set(ByVal value As String)
            If (_ResNome <> value) Then
                Me._ResNome = value
            End If
        End Set
    End Property
    Public Property ApaId() As Long
        Get
            Return Me._ApaId
        End Get
        Set(ByVal value As Long)
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
    Public Property DataIni() As String
        Get
            Return Me._DataIni
        End Get
        Set(ByVal value As String)
            If (_DataIni <> value) Then
                Me._DataIni = value
            End If
        End Set
    End Property
    Public Property HoraIni() As String
        Get
            Return Me._HoraIni
        End Get
        Set(ByVal value As String)
            If (_HoraIni <> value) Then
                Me._HoraIni = value
            End If
        End Set
    End Property
    Public Property DataIniReal() As String
        Get
            Return Me._DataIniReal
        End Get
        Set(ByVal value As String)
            If (DataIniReal <> value) Then
                Me._DataIniReal = value
            End If
        End Set
    End Property
    Public Property HoraIniReal() As String
        Get
            Return Me._HoraIniReal
        End Get
        Set(ByVal value As String)
            If (_HoraFimReal <> value) Then
                Me._HoraIniReal = value
            End If
        End Set
    End Property
    Public Property DataFim() As String
        Get
            Return Me._DataFim
        End Get
        Set(ByVal value As String)
            If (_DataFim <> value) Then
                Me._DataFim = value
            End If
        End Set
    End Property
    Public Property HoraFim() As String
        Get
            Return Me._HoraFim
        End Get
        Set(ByVal value As String)
            If (_HoraFim <> value) Then
                Me._HoraFim = value
            End If
        End Set
    End Property
    Public Property DataFimReal() As String
        Get
            Return Me._DataFimReal
        End Get
        Set(ByVal value As String)
            If (_DataFimReal <> value) Then
                Me._DataFimReal = value
            End If
        End Set
    End Property
    Public Property HoraFimReal() As String
        Get
            Return Me._HoraFimReal
        End Get
        Set(ByVal value As String)
            If (_HoraFimReal <> value) Then
                Me._HoraFimReal = value
            End If
        End Set
    End Property
    Public Property IntDtNascimento() As String
        Get
            Return Me._IntDtNascimento
        End Get
        Set(ByVal value As String)
            If (_IntDtNascimento <> value) Then
                Me._IntDtNascimento = value
            End If
        End Set
    End Property
End Class
