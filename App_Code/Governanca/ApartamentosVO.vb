Imports Microsoft.VisualBasic

Public Class ApartamentosVO
    Private _ApaId As Long
    Private _ApaDesc As String
    Private _AcmId As Long
    Private _ApaFederacao As String
    Private _ApaStatus As String
    Private _BloId As Long
    Private _AcmCC As Long
    Private _AcmCS As Long
    Private _AcmBicama As Long
    Private _AcmSofacama As Long
    Private _AlaId As Long
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
    Public Property AcmId() As Long
        Get
            Return Me._AcmId
        End Get
        Set(ByVal value As Long)
            If (_AcmId <> value) Then
                Me._AcmId = value
            End If
        End Set
    End Property
    Public Property ApaFederacao() As String
        Get
            Return Me._ApaFederacao
        End Get
        Set(ByVal value As String)
            If (_ApaFederacao <> value) Then
                Me._ApaFederacao = value
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
    Public Property BloId() As Long
        Get
            Return Me._BloId
        End Get
        Set(ByVal value As Long)
            If (_BloId <> value) Then
                Me._BloId = value
            End If
        End Set
    End Property
    Public Property AcmCC() As Long
        Get
            Return Me._AcmCC
        End Get
        Set(ByVal value As Long)
            If (_AcmCC <> value) Then
                Me._AcmCC = value
            End If
        End Set
    End Property
    Public Property AcmCS() As Long
        Get
            Return Me._AcmCS
        End Get
        Set(ByVal value As Long)
            If (_AcmCS <> value) Then
                Me._AcmCS = value
            End If
        End Set
    End Property
    Public Property AcmBicama() As Long
        Get
            Return Me._AcmBicama
        End Get
        Set(ByVal value As Long)
            If (_AcmBicama <> value) Then
                Me._AcmBicama = value
            End If
        End Set
    End Property
    Public Property AcmSofacama() As Long
        Get
            Return Me._AcmSofacama
        End Get
        Set(ByVal value As Long)
            If (_AcmSofacama <> value) Then
                Me._AcmSofacama = value
            End If
        End Set
    End Property
    Public Property AlaId() As Long
        Get
            Return Me._AlaId
        End Get
        Set(ByVal value As Long)
            If (_AlaId <> value) Then
                Me._AlaId = value
            End If
        End Set
    End Property

End Class
