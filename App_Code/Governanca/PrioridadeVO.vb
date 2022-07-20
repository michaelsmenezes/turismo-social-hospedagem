Imports Microsoft.VisualBasic

Public Class PrioridadeVO
    Private _ApaId As String
    Private _ApaDesc As String
    Private _ApaOcupado As String
    Private _ApaHoras As String
    Private _ApaCamaCasal As Long
    Private _ApaCamaSolteiro As Long
    Private _ApaCamaEspecial As Long
    Private _ApaCamaExtra As Long
    Private _ApaBercoEspecial As Long
    Private _ApaBerco As Long
    Private _ApaFronha As Long
    Private _ApaJogoToalha As Long
    Private _ApaConferencia As String
    Private _AptoPrioridadeQtde As Long
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
    Public Property ApaOcupado() As String
        Get
            Return Me._ApaOcupado
        End Get
        Set(ByVal value As String)
            If (_ApaOcupado <> value) Then
                Me._ApaOcupado = value
            End If
        End Set
    End Property
    Public Property ApaHoras() As String
        Get
            Return Me._ApaHoras
        End Get
        Set(ByVal value As String)
            If (_ApaHoras <> value) Then
                Me._ApaHoras = value
            End If
        End Set
    End Property
    Public Property ApaCamaCasal() As Long
        Get
            Return Me._ApaCamaCasal
        End Get
        Set(ByVal value As Long)
            If (_ApaCamaCasal <> value) Then
                Me._ApaCamaCasal = value
            End If
        End Set
    End Property
    Public Property ApaCamaSolteiro() As Long
        Get
            Return Me._ApaCamaSolteiro
        End Get
        Set(ByVal value As Long)
            If (_ApaCamaSolteiro <> value) Then
                Me._ApaCamaSolteiro = value
            End If
        End Set
    End Property
    Public Property ApaCamaEspecial() As Long
        Get
            Return Me._ApaCamaEspecial
        End Get
        Set(ByVal value As Long)
            If (_ApaCamaEspecial <> value) Then
                Me._ApaCamaEspecial = value
            End If
        End Set
    End Property
    Public Property ApaCamaExtra() As Long
        Get
            Return Me._ApaCamaExtra
        End Get
        Set(ByVal value As Long)
            If (_ApaCamaExtra <> value) Then
                Me._ApaCamaExtra = value
            End If
        End Set
    End Property
    Public Property ApaBercoEspecial() As Long
        Get
            Return Me._ApaBercoEspecial
        End Get
        Set(ByVal value As Long)
            If (_ApaBercoEspecial <> value) Then
                Me._ApaBercoEspecial = value
            End If
        End Set
    End Property
    Public Property ApaBerco() As Long
        Get
            Return Me._ApaBerco
        End Get
        Set(ByVal value As Long)
            If (_ApaBerco <> value) Then
                Me._ApaBerco = value
            End If
        End Set
    End Property
    Public Property ApaFronha() As Long
        Get
            Return Me._ApaFronha
        End Get
        Set(ByVal value As Long)
            If (_ApaFronha <> value) Then
                Me._ApaFronha = value
            End If
        End Set
    End Property
    Public Property ApaJogoToalha() As Long
        Get
            Return Me._ApaJogoToalha
        End Get
        Set(ByVal value As Long)
            If (_ApaJogoToalha <> value) Then
                Me._ApaJogoToalha = value
            End If
        End Set
    End Property
    Public Property ApaConferencia() As String
        Get
            Return Me._ApaConferencia
        End Get
        Set(ByVal value As String)
            If (_ApaConferencia <> value) Then
                Me._ApaConferencia = value
            End If
        End Set
    End Property
    Public Property AptoPrioridadeQtde() As Long
        Get
            Return Me._AptoPrioridadeQtde
        End Get
        Set(ByVal value As Long)
            If (_AptoPrioridadeQtde <> value) Then
                Me._AptoPrioridadeQtde = value
            End If
        End Set
    End Property
End Class
