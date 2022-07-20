Imports Microsoft.VisualBasic

Public Class HistoricoHospedesInformacoesVO
    '1° Grid do Histórico dos hóspedes
    Private _DataIniReal As String
    Private _HoraIniReal As String
    Private _DataFimReal As String
    Private _HoraFimReal As String
    Private _Status As String
    Private _IntSaldo As String
    Private _IntCortesiaCaucao As String
    Private _IntCortesiaConsumo As String
    Private _IntCortesiaRestaurante As String
    Private _FormaPagto As String
    '2º Grid do Histórico dos hóspedes
    Private _ApaDesc As String
    Private _Acomodacao As String
    Private _AcomodacaoCobranca As String
    Private _HosDataIniSol As String
    Private _HosHoraIniSol As String
    Private _HosDataFimSol As String
    Private _HosHoraFimSol As String
    Private _HosValorDevido As String
    Private _HosValorPago As String
    Private _HosStatus As String
    '3º Grid do Histório de Hospedes/Consumo
    Private _Dia As String
    Private _Hora As String
    Private _PrecoVenda As Decimal
    Private _Quantidade As String
    Private _ValorTotal As Decimal
    Private _UnidadeMedida As String
    Private _FormaConsumo As String
    Private _LocalConsumo As String
    '4° Grid do Histórico de Hospedes/Operações Financeiras
    Private _Caixa As String
    Private _Operacao As String
    Private _Descricao As String
    Private _Forma As String
    Private _Observacao As String
    Private _Operador As String
    '5° Grid Log de Eventos
    'Irá usar os atributos já existentes'
    Public Property DataIniReal() As String
        Get
            Return Me._DataIniReal
        End Get
        Set(ByVal value As String)
            If (_DataIniReal <> value) Then
                Me._DataIniReal = value
            End If
        End Set
    End Property
    Public Property HoraIniReal() As String
        Get
            Return Me._HoraIniReal
        End Get
        Set(ByVal value As String)
            If (_HoraIniReal <> value) Then
                Me._HoraIniReal = value
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
    Public Property Status() As String
        Get
            Return Me._Status
        End Get
        Set(ByVal value As String)
            If (_Status <> value) Then
                Me._Status = value
            End If
        End Set
    End Property
    Public Property IntSaldo() As String
        Get
            Return Me._IntSaldo
        End Get
        Set(ByVal value As String)
            If (_IntSaldo <> value) Then
                Me._IntSaldo = value
            End If
        End Set
    End Property
    Public Property IntCortesiaCaucao() As String
        Get
            Return Me._IntCortesiaCaucao
        End Get
        Set(ByVal value As String)
            If (_IntCortesiaCaucao <> value) Then
                Me._IntCortesiaCaucao = value
            End If
        End Set
    End Property
    Public Property IntCortesiaConsumo() As String
        Get
            Return Me._IntCortesiaConsumo
        End Get
        Set(ByVal value As String)
            If (_IntCortesiaConsumo <> value) Then
                Me._IntCortesiaConsumo = value
            End If
        End Set
    End Property
    Public Property IntCortesiaRestaurante() As String
        Get
            Return Me._IntCortesiaRestaurante
        End Get
        Set(ByVal value As String)
            If (_IntCortesiaRestaurante <> value) Then
                Me._IntCortesiaRestaurante = value
            End If
        End Set
    End Property
    Public Property FormaPagto() As String
        Get
            Return Me._FormaPagto
        End Get
        Set(ByVal value As String)
            If (_FormaPagto <> value) Then
                Me._FormaPagto = value
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
    Public Property Acomodacao() As String
        Get
            Return Me._Acomodacao
        End Get
        Set(ByVal value As String)
            If (_Acomodacao <> value) Then
                Me._Acomodacao = value
            End If
        End Set
    End Property
    Public Property AcomodacaoCobranca() As String
        Get
            Return Me._AcomodacaoCobranca
        End Get
        Set(ByVal value As String)
            If (_AcomodacaoCobranca <> value) Then
                Me._AcomodacaoCobranca = value
            End If
        End Set
    End Property
    Public Property HosDataIniSol() As String
        Get
            Return Me._HosDataIniSol
        End Get
        Set(ByVal value As String)
            If (_HosDataIniSol <> value) Then
                Me._HosDataIniSol = value
            End If
        End Set
    End Property
    Public Property HosHoraIniSol() As String
        Get
            Return Me._HosHoraIniSol
        End Get
        Set(ByVal value As String)
            If (_HosHoraIniSol <> value) Then
                Me._HosHoraIniSol = value
            End If
        End Set
    End Property

    Public Property HosDataFimSol() As String
        Get
            Return Me._HosDataFimSol
        End Get
        Set(ByVal value As String)
            If (_HosDataFimSol <> value) Then
                Me._HosDataFimSol = value
            End If
        End Set
    End Property
    Public Property HosHoraFimSol() As String
        Get
            Return Me._HosHoraFimSol
        End Get
        Set(ByVal value As String)
            If (_HosHoraFimSol <> value) Then
                Me._HosHoraFimSol = value
            End If
        End Set
    End Property


    Public Property HosValorDevido() As String
        Get
            Return Me._HosValorDevido
        End Get
        Set(ByVal value As String)
            If (_HosValorDevido <> value) Then
                Me._HosValorDevido = value
            End If
        End Set
    End Property
    Public Property HosValorPago() As String
        Get
            Return Me._HosValorPago
        End Get
        Set(ByVal value As String)
            If (_HosValorPago <> value) Then
                Me._HosValorPago = value
            End If
        End Set
    End Property
    Public Property HosStatus() As String
        Get
            Return Me._HosStatus
        End Get
        Set(ByVal value As String)
            If (_HosStatus <> value) Then
                Me._HosStatus = value
            End If
        End Set
    End Property
    Public Property Dia() As String
        Get
            Return Me._Dia
        End Get
        Set(ByVal value As String)
            If (_Dia <> value) Then
                Me._Dia = value
            End If
        End Set
    End Property
    Public Property Hora() As String
        Get
            Return Me._Hora
        End Get
        Set(ByVal value As String)
            If (_Hora <> value) Then
                Me._Hora = value
            End If
        End Set
    End Property
    Public Property PrecoVenda() As Decimal
        Get
            Return Me._PrecoVenda
        End Get
        Set(ByVal value As Decimal)
            If (_PrecoVenda <> value) Then
                Me._PrecoVenda = value
            End If
        End Set
    End Property
    Public Property Quantidade() As String
        Get
            Return Me._Quantidade
        End Get
        Set(ByVal value As String)
            If (_Quantidade <> value) Then
                Me._Quantidade = value
            End If
        End Set
    End Property
    Public Property UnidadeMedida() As String
        Get
            Return Me._UnidadeMedida
        End Get
        Set(ByVal value As String)
            If (_UnidadeMedida <> value) Then
                Me._UnidadeMedida = value
            End If
        End Set
    End Property
    Public Property ValorTotal() As Decimal
        Get
            Return Me._ValorTotal
        End Get
        Set(ByVal value As Decimal)
            If (_ValorTotal <> value) Then
                Me._ValorTotal = value
            End If
        End Set
    End Property
    Public Property FormaConsumo() As String
        Get
            Return Me._FormaConsumo
        End Get
        Set(ByVal value As String)
            If (_FormaConsumo <> value) Then
                Me._FormaConsumo = value
            End If
        End Set
    End Property
    Public Property LocalConsumo() As String
        Get
            Return Me._LocalConsumo
        End Get
        Set(ByVal value As String)
            If (_LocalConsumo <> value) Then
                Me._LocalConsumo = value
            End If
        End Set
    End Property
    Public Property Caixa() As String
        Get
            Return Me._Caixa
        End Get
        Set(ByVal value As String)
            If (_Caixa <> value) Then
                Me._Caixa = value
            End If
        End Set
    End Property
    Public Property Operacao() As String
        Get
            Return Me._Operacao
        End Get
        Set(ByVal value As String)
            If (_Operacao <> value) Then
                Me._Operacao = value
            End If
        End Set
    End Property
    Public Property Descricao() As String
        Get
            Return Me._Descricao
        End Get
        Set(ByVal value As String)
            If (_Descricao <> value) Then
                Me._Descricao = value
            End If
        End Set
    End Property
    Public Property Forma() As String
        Get
            Return Me._Forma
        End Get
        Set(ByVal value As String)
            If (_Forma <> value) Then
                Me._Forma = value
            End If
        End Set
    End Property
    Public Property Observacao() As String
        Get
            Return Me._Observacao
        End Get
        Set(ByVal value As String)
            If (_Observacao <> value) Then
                Me._Observacao = value
            End If
        End Set
    End Property
    Public Property Operador() As String
        Get
            Return Me._Operador
        End Get
        Set(ByVal value As String)
            If (_Operador <> value) Then
                Me._Operador = value
            End If
        End Set
    End Property
End Class
