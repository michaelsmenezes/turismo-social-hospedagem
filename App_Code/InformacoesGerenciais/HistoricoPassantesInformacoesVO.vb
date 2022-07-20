Imports Microsoft.VisualBasic

Public Class HistoricoPassantesInformacoesVO
    'Herdando Atributos da classe de Hóspedes
    Inherits HistoricoHospedesInformacoesVO
    '1° Grid do Histórico dos hóspedes
    Private _IntNome As String
    Private _Matricula As String
    Private _RG As String
    Private _CPF As String
    Private _Categoria As String
    Private _ComerciarioCaldasNovas As String
    Private _Isento As String
    Private _CortesiaCaucao As String
    Private _CortesiaPqAquatico As String
    Private _CortesiaLanchonetes As String
    Private _CortesiaPermanenciaPQ As String
    Private _CortesiaRestaurante As String
    Private _ResponsavelCortesia As String
    Private _CategoriaCobranca As String
    Private _DocMemorando As String
    Private _MotivoEmissor As String
    Private _Situacao As String
    Private _Placa As String
    Private _UF As String
    Private _Cidade As String
    Private _VinculoRefeicao As String

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
    Public Property Matricula() As String
        Get
            Return Me._Matricula
        End Get
        Set(ByVal value As String)
            If (Matricula <> value) Then
                Me._Matricula = value
            End If
        End Set
    End Property
    Public Property RG() As String
        Get
            Return Me._RG
        End Get
        Set(ByVal value As String)
            If (_RG <> value) Then
                Me._RG = value
            End If
        End Set
    End Property
    Public Property CPF() As String
        Get
            Return Me._CPF
        End Get
        Set(ByVal value As String)
            If (_CPF <> value) Then
                Me._CPF = value
            End If
        End Set
    End Property
    Public Property Categoria() As String
        Get
            Return Me._Categoria
        End Get
        Set(ByVal value As String)
            If (_Categoria <> value) Then
                Me._Categoria = value
            End If
        End Set
    End Property


    Public Property ComerciarioCaldasNovas() As String
        Get
            Return Me._ComerciarioCaldasNovas
        End Get
        Set(ByVal value As String)
            If (_ComerciarioCaldasNovas <> value) Then
                Me._ComerciarioCaldasNovas = value
            End If
        End Set
    End Property
    Public Property Isento() As String
        Get
            Return Me._Isento
        End Get
        Set(ByVal value As String)
            If (_Isento <> value) Then
                Me._Isento = value
            End If
        End Set
    End Property
    Public Property CortesiaCaucao() As String
        Get
            Return Me._CortesiaCaucao
        End Get
        Set(ByVal value As String)
            If (_CortesiaCaucao <> value) Then
                Me._CortesiaCaucao = value
            End If
        End Set
    End Property
    Public Property CortesiaPqAquatico() As String
        Get
            Return Me._CortesiaPqAquatico
        End Get
        Set(ByVal value As String)
            If (_CortesiaPqAquatico <> value) Then
                Me._CortesiaPqAquatico = value
            End If
        End Set
    End Property
    Public Property CortesiaLanchonetes() As String
        Get
            Return Me._CortesiaLanchonetes
        End Get
        Set(ByVal value As String)
            If (_CortesiaLanchonetes <> value) Then
                Me._CortesiaLanchonetes = value
            End If
        End Set
    End Property
    Public Property CortesiaPermanenciaPQ() As String
        Get
            Return Me._CortesiaPermanenciaPQ
        End Get
        Set(ByVal value As String)
            If (_CortesiaPermanenciaPQ <> value) Then
                Me._CortesiaPermanenciaPQ = value
            End If
        End Set
    End Property
    Public Property CortesiaRestaurante() As String
        Get
            Return Me._CortesiaRestaurante
        End Get
        Set(ByVal value As String)
            If (_CortesiaRestaurante <> value) Then
                Me._CortesiaRestaurante = value
            End If
        End Set
    End Property
    Public Property ResponsavelCortesia() As String
        Get
            Return Me._ResponsavelCortesia
        End Get
        Set(ByVal value As String)
            If (_ResponsavelCortesia <> value) Then
                Me._ResponsavelCortesia = value
            End If
        End Set
    End Property
    Public Property CategoriaCobranca() As String
        Get
            Return Me._CategoriaCobranca
        End Get
        Set(ByVal value As String)
            If (_CategoriaCobranca <> value) Then
                Me._CategoriaCobranca = value
            End If
        End Set
    End Property
    Public Property DocMemorando() As String
        Get
            Return Me._DocMemorando
        End Get
        Set(ByVal value As String)
            If (_DocMemorando <> value) Then
                Me._DocMemorando = value
            End If
        End Set
    End Property
    Public Property MotivoEmissor() As String
        Get
            Return Me._MotivoEmissor
        End Get
        Set(ByVal value As String)
            If (_MotivoEmissor <> value) Then
                Me._MotivoEmissor = value
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
    Public Property Placa() As String
        Get
            Return Me._Placa
        End Get
        Set(ByVal value As String)
            If (_Placa <> value) Then
                Me._Placa = value
            End If
        End Set
    End Property
    Public Property UF() As String
        Get
            Return Me._UF
        End Get
        Set(ByVal value As String)
            If (_UF <> value) Then
                Me._UF = value
            End If
        End Set
    End Property
    Public Property Cidade() As String
        Get
            Return Me._Cidade
        End Get
        Set(ByVal value As String)
            If (_Cidade <> value) Then
                Me._Cidade = value
            End If
        End Set
    End Property
    Public Property VinculoRefeicao() As String
        Get
            Return Me._VinculoRefeicao
        End Get
        Set(ByVal value As String)
            _VinculoRefeicao = value
        End Set
    End Property

End Class
