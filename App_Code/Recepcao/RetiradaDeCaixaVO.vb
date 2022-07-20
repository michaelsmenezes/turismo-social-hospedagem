Imports Microsoft.VisualBasic

Public Class RetiradaDeCaixaVO
    Private _rcxId As Long
    Private _rcxData As String
    Private _CxaCrtOpr As String
    Private _cxaCrtCod As String
    Private _cxaTipoOperacao As String
    Private _cxaQuantidade As String
    Private _cxaValor As String
    Private _cxaNumeroBanco As String
    Private _cxaNumeroCheque As String
    Private _cxaDescricaoCartao As String
    Private _cxaTotal As String
    Private _cxaValorOperacoes As String
    Private _CxaCrtDAbe As String
    Private _CxaCrtHAbe As String
    Private _cxaNumeroCupom As String
    'Cabelho da retirada de caixa
    Private _cbrId As Long
    Private _cbrStatus As String

    Public Property rcxId() As Long
        Get
            Return Me._rcxId
        End Get
        Set(ByVal value As Long)
            If (_rcxId <> value) Then
                Me._rcxId = value
            End If
        End Set
    End Property
    Public Property rcxData() As String
        Get
            Return Me._rcxData
        End Get
        Set(ByVal value As String)
            If (_rcxData <> value) Then
                Me._rcxData = value
            End If
        End Set
    End Property
    Public Property CxaCrtOpr() As String
        Get
            Return Me._CxaCrtOpr
        End Get
        Set(ByVal value As String)
            If (_CxaCrtOpr <> value) Then
                Me._CxaCrtOpr = value
            End If
        End Set
    End Property
    Public Property cxaCrtCod() As String
        Get
            Return Me._cxaCrtCod
        End Get
        Set(ByVal value As String)
            If (_cxaCrtCod <> value) Then
                Me._cxaCrtCod = value
            End If
        End Set
    End Property
    Public Property cxaTipoOperacao() As String
        Get
            Return Me._cxaTipoOperacao
        End Get
        Set(ByVal value As String)
            If (_cxaTipoOperacao <> value) Then
                Me._cxaTipoOperacao = value
            End If
        End Set
    End Property
    Public Property cxaQuantidade() As String
        Get
            Return Me._cxaQuantidade
        End Get
        Set(ByVal value As String)
            If (_cxaQuantidade <> value) Then
                Me._cxaQuantidade = value
            End If
        End Set
    End Property
    Public Property cxaValor() As String
        Get
            Return Me._cxaValor
        End Get
        Set(ByVal value As String)
            If (_cxaValor <> value) Then
                Me._cxaValor = value
            End If
        End Set
    End Property
    Public Property cxaNumeroBanco() As String
        Get
            Return Me._cxaNumeroBanco
        End Get
        Set(ByVal value As String)
            If (_cxaNumeroBanco <> value) Then
                Me._cxaNumeroBanco = value
            End If
        End Set
    End Property
    Public Property cxaNumeroCheque() As String
        Get
            Return Me._cxaNumeroCheque
        End Get
        Set(ByVal value As String)
            If (_cxaNumeroCheque <> value) Then
                Me._cxaNumeroCheque = value
            End If
        End Set
    End Property
    Public Property cxaDescricaoCartao() As String
        Get
            Return Me._cxaDescricaoCartao
        End Get
        Set(ByVal value As String)
            If (_cxaDescricaoCartao <> value) Then
                Me._cxaDescricaoCartao = value
            End If
        End Set
    End Property
    Public Property cxaTotal() As String
        Get
            Return Me._cxaTotal
        End Get
        Set(ByVal value As String)
            If (_cxaTotal <> value) Then
                Me._cxaTotal = value
            End If
        End Set
    End Property
    Public Property CxaCrtDAbe() As String
        Get
            Return Me._CxaCrtDAbe
        End Get
        Set(ByVal value As String)
            If (_CxaCrtDAbe <> value) Then
                Me._CxaCrtDAbe = value
            End If
        End Set
    End Property

    Public Property cxacrthabe() As String
        Get
            Return Me._CxaCrtHAbe
        End Get
        Set(ByVal value As String)
            If (_CxaCrtHAbe <> value) Then
                Me._CxaCrtHAbe = value
            End If
        End Set
    End Property
    Public Property cxaNumeroCupom() As String
        Get
            Return Me._cxaNumeroCupom
        End Get
        Set(ByVal value As String)
            If (_cxaNumeroCupom <> value) Then
                Me._cxaNumeroCupom = value
            End If
        End Set
    End Property

    Public Property cbrId() As Long
        Get
            Return Me._cbrId
        End Get
        Set(ByVal value As Long)
            If (_cbrId <> value) Then
                Me._cbrId = value
            End If
        End Set
    End Property
    Public Property cbrStatus() As String
        Get
            Return Me._cbrStatus
        End Get
        Set(ByVal value As String)
            If (_cbrStatus <> value) Then
                Me._cbrStatus = value
            End If
        End Set
    End Property
    Public Property CxaValorOperacoes() As String
        Get
            Return Me._cxaValorOperacoes
        End Get
        Set(ByVal value As String)
            If (_cxaValorOperacoes <> value) Then
                Me._cxaValorOperacoes = value
            End If
        End Set
    End Property
End Class
