Imports Microsoft.VisualBasic

Public Class RelGovernacaVO
    Private _ApaId As Long
    Private _ApaDesc As String
    Private _AptoCC As Long
    Private _AptoCS As Long
    Private _AptoCE As Long
    Private _AptoBE As Long
    Private _AptoFR As Long
    Private _AptoJT As Long
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
    Public Property AptoCC() As Long
        Get
            Return Me._AptoCC
        End Get
        Set(ByVal value As Long)
            If (_AptoCC <> value) Then
                Me._AptoCC = value
            End If
        End Set
    End Property
    Public Property AptoCS() As Long
        Get
            Return Me._AptoCS
        End Get
        Set(ByVal value As Long)
            If (_AptoCS <> value) Then
                Me._AptoCS = value
            End If
        End Set
    End Property
    Public Property AptoCE() As Long
        Get
            Return Me._AptoCE
        End Get
        Set(ByVal value As Long)
            If (_AptoCE <> value) Then
                Me._AptoBE = value
            End If
        End Set
    End Property
    Public Property AptoBE() As Long
        Get
            Return Me._AptoBE
        End Get
        Set(ByVal value As Long)
            If (_AptoBE <> value) Then
                Me._AptoBE = value
            End If
        End Set
    End Property
    Public Property AptoFR() As Long
        Get
            Return Me._AptoFR
        End Get
        Set(ByVal value As Long)
            If (_AptoFR <> value) Then
                Me._AptoFR = value
            End If
        End Set
    End Property
    Public Property AptoJT() As Long
        Get
            Return Me._AptoJT
        End Get
        Set(ByVal value As Long)
            If (_AptoJT <> value) Then
                Me._AptoJT = value
            End If
        End Set
    End Property
End Class
