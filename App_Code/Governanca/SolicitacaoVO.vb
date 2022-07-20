Imports Microsoft.VisualBasic

Public Class SolicitacaoVO
    Private _Id As Long
    Private _CentroCusto As String
    Private _SetorExecutante As String
    Private _LocDescricao As String
    Private _Area As String
    Private _DescSubLoc As String
    Private _Assunto As String
    Private _Descricao As String
    Private _UsuarioSolicitante As String
    Private _CentroCustoSolicitante As String
    Private _DisplayNameSolicitante As String
    Private _DataHoraSolicitacao As String
    Private _Situacao As String
    Private _PrevisaoAtendimento As String
    Private _DataAtendimento As String
    Private _MatriculaAtendimento As String
    Private _NomeFuncAtendimento As String
    Private _DataLog As String
    Private _UsuarioLog As String
    Private _IpUnidade As String
    Private _IdUnidade As String
    Private _Patrimonio As String
    Private _DescBem As String
    Private _ObsManutencao As String
    Private _GrauPrioridade As String
    Private _Avaliacao As String
    Private _Devolucao As String
    Private _TotalSolSemAvaliacao As String
    Private _JustificativaAvaliacao As String
    Private _BloqueioApartamento As String
    Public Property solId() As Long
        Get
            Return Me._Id
        End Get
        Set(ByVal value As Long)
            If (_Id <> value) Then
                Me._Id = value
            End If
        End Set
    End Property
    Public Property CentroCusto() As String
        Get
            Return Me._CentroCusto
        End Get
        Set(ByVal value As String)
            If (_CentroCusto <> value) Then
                Me._CentroCusto = value
            End If
        End Set
    End Property

    Public Property SetorExecutante() As String
        Get
            Return Me._SetorExecutante
        End Get
        Set(ByVal value As String)
            If (_SetorExecutante <> value) Then
                Me._SetorExecutante = value
            End If
        End Set
    End Property


    Public Property LocDescricao() As String
        Get
            Return Me._LocDescricao
        End Get
        Set(ByVal value As String)
            If (_LocDescricao <> value) Then
                Me._LocDescricao = value
            End If
        End Set
    End Property
    Public Property Area() As String
        Get
            Return Me._Area
        End Get
        Set(ByVal value As String)
            If (_Area <> value) Then
                Me._Area = value
            End If
        End Set
    End Property
    Public Property DescSubLoc() As String
        Get
            Return Me._DescSubLoc
        End Get
        Set(ByVal value As String)
            If (_DescSubLoc <> value) Then
                Me._DescSubLoc = value
            End If
        End Set
    End Property
    Public Property Assunto() As String
        Get
            Return (Me._Assunto)
        End Get
        Set(ByVal value As String)
            If (_Assunto <> value) Then
                Me._Assunto = value
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
    Public Property UsuarioSolicitante() As String
        Get
            Return Me._UsuarioSolicitante
        End Get
        Set(ByVal value As String)
            If (_UsuarioSolicitante <> value) Then
                Me._UsuarioSolicitante = value
            End If
        End Set
    End Property
    Public Property CentroCustoSolicitante() As String
        Get
            Return Me._CentroCustoSolicitante
        End Get
        Set(ByVal value As String)
            If (_CentroCustoSolicitante <> value) Then
                Me._CentroCustoSolicitante = value
            End If
        End Set
    End Property

    Public Property DataHoraSolicitacao() As String
        Get
            Return Me._DataHoraSolicitacao
        End Get
        Set(ByVal value As String)
            If (_DataHoraSolicitacao <> value) Then
                Me._DataHoraSolicitacao = value
            End If
        End Set
    End Property
    Public Property Situacao() As String
        Get
            Return Me._Situacao
        End Get
        Set(ByVal value As String)
            If (_Situacao <> value) Then
                Me._Situacao = value
            End If
        End Set
    End Property
    Public Property PrevisaoAtendimento() As String
        Get
            Return Me._PrevisaoAtendimento
        End Get
        Set(ByVal value As String)
            If (_PrevisaoAtendimento <> value) Then
                Me._PrevisaoAtendimento = value
            End If
        End Set
    End Property
    Public Property DataAtendimento() As String
        Get
            Return Me._DataAtendimento
        End Get
        Set(ByVal value As String)
            If (_DataAtendimento <> value) Then
                Me._DataAtendimento = value
            End If
        End Set
    End Property
    Public Property MatriculaAtendimento() As String
        Get
            Return Me._MatriculaAtendimento
        End Get
        Set(ByVal value As String)
            If (_MatriculaAtendimento <> value) Then
                Me._MatriculaAtendimento = value
            End If
        End Set
    End Property

    Public Property DataLog() As String
        Get
            Return Me._DataLog
        End Get
        Set(ByVal value As String)
            If (_DataLog <> value) Then
                Me._DataLog = value
            End If
        End Set
    End Property
    Public Property UsuarioLog() As String
        Get
            Return Me._UsuarioLog
        End Get
        Set(ByVal value As String)
            If (_UsuarioLog <> value) Then
                Me._UsuarioLog = value
            End If
        End Set
    End Property
    Public Property IpUnidade() As String
        Get
            Return Me._IpUnidade
        End Get
        Set(ByVal value As String)
            If (_IpUnidade <> value) Then
                Me._IpUnidade = value
            End If
        End Set
    End Property
    Public Property IdUnidade() As String
        Get
            Return _IdUnidade
        End Get
        Set(ByVal value As String)
            _IdUnidade = value
        End Set
    End Property

    Public Property DisplayNameSolicitante() As String
        Get
            Return Me._DisplayNameSolicitante
        End Get
        Set(ByVal value As String)
            If (_DisplayNameSolicitante <> value) Then
                Me._DisplayNameSolicitante = value
            End If
        End Set
    End Property
    Public Property Patrimonio() As String
        Get
            Return Me._Patrimonio
        End Get
        Set(ByVal value As String)
            If (_Patrimonio <> value) Then
                Me._Patrimonio = value
            End If
        End Set
    End Property
    Public Property DesBem() As String
        Get
            Return Me._DescBem
        End Get
        Set(ByVal value As String)
            If (_DescBem <> value) Then
                Me._DescBem = value
            End If
        End Set
    End Property
    Public Property ObsManutencao() As String
        Get
            Return Me._ObsManutencao
        End Get
        Set(ByVal value As String)
            If (_ObsManutencao <> value) Then
                Me._ObsManutencao = value
            End If
        End Set
    End Property
    Public Property NomeFuncAtendimento() As String
        Get
            Return Me._NomeFuncAtendimento
        End Get
        Set(ByVal value As String)
            If (_NomeFuncAtendimento <> value) Then
                Me._NomeFuncAtendimento = value
            End If
        End Set
    End Property
    Public Property GrauPrioridade() As String
        Get
            Return Me._GrauPrioridade
        End Get
        Set(ByVal value As String)
            If (_GrauPrioridade <> value) Then
                Me._GrauPrioridade = value
            End If
        End Set
    End Property
    Public Property Avaliacao() As String
        Get
            Return Me._Avaliacao
        End Get
        Set(ByVal value As String)
            If (_Avaliacao <> value) Then
                Me._Avaliacao = value
            End If
        End Set
    End Property
    Public Property Devolucao() As String
        Get
            Return Me._Devolucao
        End Get
        Set(ByVal value As String)
            If (_Devolucao <> value) Then
                Me._Devolucao = value
            End If
        End Set
    End Property
    Public Property TotalSolSemAvaliacao() As Long
        Get
            Return Me._TotalSolSemAvaliacao
        End Get
        Set(ByVal value As Long)
            If (_TotalSolSemAvaliacao <> value) Then
                Me._TotalSolSemAvaliacao = value
            End If
        End Set
    End Property
    Public Property JustificativaAvaliacao() As String
        Get
            Return Me._JustificativaAvaliacao
        End Get
        Set(ByVal value As String)
            If (_JustificativaAvaliacao <> value) Then
                Me._JustificativaAvaliacao = value
            End If
        End Set
    End Property
    Public Property BloqueioApartamento() As String
        Get
            Return Me._BloqueioApartamento
        End Get
        Set(ByVal value As String)
            If (_BloqueioApartamento <> value) Then
                Me._BloqueioApartamento = value
            End If
        End Set
    End Property
End Class
