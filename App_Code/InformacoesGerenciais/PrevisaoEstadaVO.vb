Public Class PrevisaoEstadaVO
    Private _Data As String
    Private _Bloco As String
    Private _Caracteristica As String
    Private _AptoR As String
    Private _AptoE As String
    Private _AptoP As String
    Private _AptoM As String
    Private _Livre As String
    'Private _prvDataPrevisao As String
    'Private _HospR As String
    'Private _HospE As String
    'Private _HospP As String
    'Private _AptoI As String
    'Private _HospI As String
    'Private _AcmQtde As String
    'Private _Total As String

    Private _prvTotalLeitos As String
    Private _prvTotalPrevisao As String
    Private _prvQtdeTotalAptos As String
    Private _prvQtdeAptos As String
    Private _prvBloqueados As String

    'Consulta para Gestor de turismo overbook
    Private _DataRefencia As String
    Private _AcmId As String
    Private _AcmDescricao As String
    Private _Vendidos As String
    Private _Existentes As String
    Private _Confirmados As String
    Private _PendPagamento As String
    Private _Estada As String
    Private _PendIntegrante As String
    Private _SubTotal As String
    Private _Saidas As String
    Private _Outras As String
    Private _SomaTotal As String

    'Consulta de transferencia entre apartamentos
    Private _ResId As String
    Private _ResNome As String
    Private _DataInicial As String
    Private _DataFinal As String
    Private _Origem As String
    Private _Destino As String
    Private _HosStatus As String
    Private _HosUsuario As String
    Private _HosUsuarioData As String
    Public Property HosUsuarioData() As String
        Get
            Return _HosUsuarioData
        End Get
        Set(ByVal value As String)
            _HosUsuarioData = value
        End Set
    End Property
    Public Property HosUsuario() As String
        Get
            Return _HosUsuario
        End Get
        Set(ByVal value As String)
            _HosUsuario = value
        End Set
    End Property
    Public Property HosStatus() As String
        Get
            Return _HosStatus
        End Get
        Set(ByVal value As String)
            _HosStatus = value
        End Set
    End Property
    Public Property Destino() As String
        Get
            Return _Destino
        End Get
        Set(ByVal value As String)
            _Destino = value
        End Set
    End Property
    Public Property Origem() As String
        Get
            Return _Origem
        End Get
        Set(ByVal value As String)
            _Origem = value
        End Set
    End Property
    Public Property DataFinal() As String
        Get
            Return _DataFinal
        End Get
        Set(ByVal value As String)
            _DataFinal = value
        End Set
    End Property

    Public Property DataInicial() As String
        Get
            Return _DataInicial
        End Get
        Set(ByVal value As String)
            _DataInicial = value
        End Set
    End Property

    Public Property ResNome() As String
        Get
            Return _ResNome
        End Get
        Set(ByVal value As String)
            _ResNome = value
        End Set
    End Property

    Public Property ResId() As String
        Get
            Return _ResId
        End Get
        Set(ByVal value As String)
            _ResId = value
        End Set
    End Property


    Public Property SomaTotal() As String
        Get
            Return _SomaTotal
        End Get
        Set(ByVal value As String)
            _SomaTotal = value
        End Set
    End Property
    Public Property Outras() As String
        Get
            Return _Outras
        End Get
        Set(ByVal value As String)
            _Outras = value
        End Set
    End Property
    Public Property Saidas() As String
        Get
            Return _Saidas
        End Get
        Set(ByVal value As String)
            _Saidas = value
        End Set
    End Property
    Public Property SubTotal() As String
        Get
            Return _SubTotal
        End Get
        Set(ByVal value As String)
            _SubTotal = value
        End Set
    End Property
    Public Property PendIntegrante() As String
        Get
            Return _PendIntegrante
        End Get
        Set(ByVal value As String)
            _PendIntegrante = value
        End Set
    End Property
    Public Property Estada() As String
        Get
            Return _Estada
        End Get
        Set(ByVal value As String)
            _Estada = value
        End Set
    End Property
    Public Property PendPagamento() As String
        Get
            Return _PendPagamento
        End Get
        Set(ByVal value As String)
            _PendPagamento = value
        End Set
    End Property
    Public Property Confirmados() As String
        Get
            Return _Confirmados
        End Get
        Set(ByVal value As String)
            _Confirmados = value
        End Set
    End Property
    Public Property Existentes() As String
        Get
            Return _Existentes
        End Get
        Set(ByVal value As String)
            _Existentes = value
        End Set
    End Property
    Public Property Vendidos() As String
        Get
            Return _Vendidos
        End Get
        Set(ByVal value As String)
            _Vendidos = value
        End Set
    End Property
    Public Property AcmDescricao() As String
        Get
            Return _AcmDescricao
        End Get
        Set(ByVal value As String)
            _AcmDescricao = value
        End Set
    End Property
    Public Property AcmId() As String
        Get
            Return _AcmId
        End Get
        Set(ByVal value As String)
            _AcmId = value
        End Set
    End Property
    Public Property DataReferencia() As String
        Get
            Return _DataRefencia
        End Get
        Set(ByVal value As String)
            _DataRefencia = value
        End Set
    End Property

    Public Property prvBloqueados() As String
        Get
            Return _prvBloqueados
        End Get
        Set(ByVal value As String)
            _prvBloqueados = value
        End Set
    End Property
    Public Property prvQtdeAptos() As String
        Get
            Return _prvQtdeAptos
        End Get
        Set(ByVal value As String)
            _prvQtdeAptos = value
        End Set
    End Property
    Public Property prvQtdeTotalAptos() As String
        Get
            Return _prvQtdeTotalAptos
        End Get
        Set(ByVal value As String)
            _prvQtdeTotalAptos = value
        End Set
    End Property
    Public Property prvTotalPrevisao() As String
        Get
            Return _prvTotalPrevisao
        End Get
        Set(ByVal value As String)
            _prvTotalPrevisao = value
        End Set
    End Property
    Public Property prvTotalLeitos() As String
        Get
            Return _prvTotalLeitos
        End Get
        Set(ByVal value As String)
            _prvTotalLeitos = value
        End Set
    End Property
    Public Property Data() As String
        Get
            Return Me._Data
        End Get
        Set(ByVal value As String)
            If (_Data <> value) Then
                Me._Data = value
            End If
        End Set
    End Property
    Public Property Bloco() As String
        Get
            Return Me._Bloco
        End Get
        Set(ByVal value As String)
            If (_Bloco <> value) Then
                Me._Bloco = value
            End If
        End Set
    End Property
    Public Property Caracteristica() As String
        Get
            Return Me._Caracteristica
        End Get
        Set(ByVal value As String)
            If (_Caracteristica <> value) Then
                Me._Caracteristica = value
            End If
        End Set
    End Property
    Public Property AptoR() As String
        Get
            Return Me._AptoR
        End Get
        Set(ByVal value As String)
            If (_AptoR <> value) Then
                Me._AptoR = value
            End If
        End Set
    End Property
    Public Property AptoE() As String
        Get
            Return Me._AptoE
        End Get
        Set(ByVal value As String)
            If (_AptoE <> value) Then
                Me._AptoE = value
            End If
        End Set
    End Property
    Public Property AptoP() As String
        Get
            Return Me._AptoP
        End Get
        Set(ByVal value As String)
            If (_AptoP <> value) Then
                Me._AptoP = value
            End If
        End Set
    End Property
    Public Property AptoM() As String
        Get
            Return Me._AptoM
        End Get
        Set(ByVal value As String)
            If (_AptoM <> value) Then
                Me._AptoM = value
            End If
        End Set
    End Property
    Public Property Livre() As String
        Get
            Return Me._Livre
        End Get
        Set(ByVal value As String)
            If (_Livre <> value) Then
                Me._Livre = value
            End If
        End Set
    End Property
    'DESATIVADOS POR FALTA DE USO EM  22-11-2018 Washington
    'Public Property AcmQtde() As String
    '    Get
    '        Return Me._AcmQtde
    '    End Get
    '    Set(ByVal value As String)
    '        If (_AcmQtde <> value) Then
    '            Me._AcmQtde = value
    '        End If
    '    End Set
    'End Property
    'Public Property Total() As String
    '    Get
    '        Return Me._Total
    '    End Get
    '    Set(ByVal value As String)
    '        If (_Total <> value) Then
    '            Me._Total = value
    '        End If
    '    End Set
    'End Property
    'Public Property HospP() As String
    '    Get
    '        Return Me._HospP
    '    End Get
    '    Set(ByVal value As String)
    '        If (_HospP <> value) Then
    '            Me._HospP = value
    '        End If
    '    End Set
    'End Property
    'Public Property AptoI() As String
    '    Get
    '        Return Me._AptoI
    '    End Get
    '    Set(ByVal value As String)
    '        If (_AptoI <> value) Then
    '            Me._AptoI = value
    '        End If
    '    End Set
    'End Property
    'Public Property HospI() As String
    '    Get
    '        Return Me._HospI
    '    End Get
    '    Set(ByVal value As String)
    '        If (_HospI <> value) Then
    '            Me._HospI = value
    '        End If
    '    End Set
    'End Property
    'Public Property HospE() As String
    '    Get
    '        Return Me._HospE
    '    End Get
    '    Set(ByVal value As String)
    '        If (_HospE <> value) Then
    '            Me._HospE = value
    '        End If
    '    End Set
    'End Property
    'Public Property HospR() As String
    '    Get
    '        Return Me._HospR
    '    End Get
    '    Set(ByVal value As String)
    '        If (_HospR <> value) Then
    '            Me._HospR = value
    '        End If
    '    End Set
    'End Property
    'Public Property prvDataPrevisao() As String
    '    Get
    '        Return _prvDataPrevisao
    '    End Get
    '    Set(ByVal value As String)
    '        _prvDataPrevisao = value
    '    End Set
    'End Property

End Class
