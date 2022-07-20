Imports Microsoft.VisualBasic

Public Class FuncaoTurnoVO
    Private _ftuId As Long
    Private _ftuRecepcao As String
    Private _ftuData As String
    Private _ftuUsuario As String
    Private _ftuTAOrganizarLocal As String
    Private _ftuTAEfetuarConferencia As String
    Private _ftuTAConferirReservas As String
    Private _ftuTAVerificarInOut As String
    Private _ftuTACheckOutPrevistos As String
    Private _ftuTBReceberPendencias As String
    Private _ftuTBVerificarLate As String
    Private _ftuTBHigienizacao As String
    Private _ftuTBConfeccaoCartoes As String
    Private _ftuTBConferenciaChaves As String
    Private _ftuTBEmailPasseio As String
    Private _ftuObservacao As String

    Public Property ftuId() As Long
        Get
            Return Me._ftuId
        End Get
        Set(ByVal value As Long)
            If (_ftuId <> value) Then
                Me._ftuId = value
            End If
        End Set
    End Property
    Public Property ftuRecepcao() As String
        Get
            Return Me._ftuRecepcao
        End Get
        Set(ByVal value As String)
            If (_ftuRecepcao <> value) Then
                Me._ftuRecepcao = value
            End If
        End Set
    End Property
    Public Property ftuData() As String
        Get
            Return Me._ftuData
        End Get
        Set(ByVal value As String)
            If (_ftuData <> value) Then
                Me._ftuData = value
            End If
        End Set
    End Property
    Public Property ftuUsuario() As String
        Get
            Return Me._ftuUsuario
        End Get
        Set(ByVal value As String)
            If (_ftuUsuario <> value) Then
                Me._ftuUsuario = value
            End If
        End Set
    End Property
    Public Property ftuTAOrganizarLocal() As String
        Get
            Return Me._ftuTAOrganizarLocal
        End Get
        Set(ByVal value As String)
            If (_ftuTAOrganizarLocal <> value) Then
                Me._ftuTAOrganizarLocal = value
            End If
        End Set
    End Property
    Public Property ftuTAEfetuarConferencia() As String
        Get
            Return Me._ftuTAEfetuarConferencia
        End Get
        Set(ByVal value As String)
            If (_ftuTAEfetuarConferencia <> value) Then
                Me._ftuTAEfetuarConferencia = value
            End If
        End Set
    End Property
    Public Property ftuTAConferirReservas() As String
        Get
            Return Me._ftuTAConferirReservas
        End Get
        Set(ByVal value As String)
            If (_ftuTAConferirReservas <> value) Then
                Me._ftuTAConferirReservas = value
            End If
        End Set
    End Property
    Public Property ftuTAVerificarInOut() As String
        Get
            Return Me._ftuTAVerificarInOut
        End Get
        Set(ByVal value As String)
            If (_ftuTAVerificarInOut <> value) Then
                Me._ftuTAVerificarInOut = value
            End If
        End Set
    End Property
    Public Property ftuTACheckOutPrevistos() As String
        Get
            Return Me._ftuTACheckOutPrevistos
        End Get
        Set(ByVal value As String)
            If (_ftuTACheckOutPrevistos <> value) Then
                Me._ftuTACheckOutPrevistos = value
            End If
        End Set
    End Property
    Public Property ftuTBReceberPendencias() As String
        Get
            Return Me._ftuTBReceberPendencias
        End Get
        Set(ByVal value As String)
            If (_ftuTBReceberPendencias <> value) Then
                Me._ftuTBReceberPendencias = value
            End If
        End Set
    End Property
    Public Property ftuTBVerificarLate() As String
        Get
            Return Me._ftuTBVerificarLate
        End Get
        Set(ByVal value As String)
            If (_ftuTBVerificarLate <> value) Then
                Me._ftuTBVerificarLate = value
            End If
        End Set
    End Property
    Public Property ftuTBHigienizacao() As String
        Get
            Return Me._ftuTBHigienizacao
        End Get
        Set(ByVal value As String)
            If (_ftuTBHigienizacao <> value) Then
                Me._ftuTBHigienizacao = value
            End If
        End Set
    End Property
    Public Property ftuTBConfeccaoCartoes() As String
        Get
            Return Me._ftuTBConfeccaoCartoes
        End Get
        Set(ByVal value As String)
            If (_ftuTBConfeccaoCartoes <> value) Then
                Me._ftuTBConfeccaoCartoes = value
            End If
        End Set
    End Property
    Public Property ftuTBConferenciaChaves() As String
        Get
            Return Me._ftuTBConferenciaChaves
        End Get
        Set(ByVal value As String)
            If (_ftuTBConferenciaChaves <> value) Then
                Me._ftuTBConferenciaChaves = value
            End If
        End Set
    End Property
    Public Property ftuTBEmailPasseio() As String
        Get
            Return Me._ftuTBEmailPasseio
        End Get
        Set(ByVal value As String)
            If (_ftuTBEmailPasseio <> value) Then
                Me._ftuTBEmailPasseio = value
            End If
        End Set
    End Property
    Public Property ftuObservacao() As String
        Get
            Return Me._ftuObservacao
        End Get
        Set(ByVal value As String)
            If (_ftuObservacao <> value) Then
                Me._ftuObservacao = value
            End If
        End Set
    End Property
End Class
