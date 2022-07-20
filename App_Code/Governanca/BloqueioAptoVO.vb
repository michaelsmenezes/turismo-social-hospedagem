Imports Microsoft.VisualBasic

Public Class BloqueioAptoVO
    'USADO NA PROCEDURE DE ATENDIMENTO
    Private _AGoId As Long
    Private _ApaId As Long
    Private _CamId As Long
    Private _AGoCCasal As Integer
    Private _AGoLencolCasal As String
    Private _AGoLencolSolteiro As String
    Private _AGoLencolBerco As String
    Private _AGoCSolteiro As Integer
    Private _AGoBerco As Integer
    Private _AGoTravesseiro As Integer
    Private _AGoJogoToalhas As Integer
    Private _AGoRoloPapel As Integer
    Private _AGoSacoLixo As Integer
    Private _AGoSabonete As Integer
    Private _AGoTapete As Char
    Private _AGoOrigem As Char
    Private _AGoObservacao As String
    Private _Acao As Char
    Private _ApaCamaExtra As Integer
    Private _ApaBerco As Integer
    Private _Usuario As String
    'USADO NA PROCEDURA DE ATUALIZAGOVERNANÇA'
    Private _Manutencao As String
    Private _AlteraManutencao As String
    Private _Dia As Long
    Private _ManDescricaoResquisitante As String
    'BLOQUEANDO APARTAMENTO'
    Private _CCusto As String
    Private _Area As String
    Public Property AGoId() As Long
        Get
            Return Me._AGoId
        End Get
        Set(ByVal value As Long)
            If (_AGoId <> value) Then
                Me._AGoId = value
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
    Public Property CamId() As Long
        Get
            Return Me._CamId
        End Get
        Set(ByVal value As Long)
            If (_CamId <> value) Then
                Me._CamId = value
            End If
        End Set
    End Property
    Public Property AGoCCasal() As Integer
        Get
            Return Me._AGoCCasal
        End Get
        Set(ByVal value As Integer)
            If (_AGoCCasal <> value) Then
                Me._AGoCCasal = value
            End If
        End Set
    End Property
    Public Property AGoLencolCasal() As String
        Get
            Return Me._AGoLencolCasal
        End Get
        Set(ByVal value As String)
            If (_AGoLencolCasal <> value) Then
                Me._AGoLencolCasal = value
            End If
        End Set
    End Property
    Public Property AGoLencolSolteiro() As String
        Get
            Return Me._AGoLencolSolteiro
        End Get
        Set(ByVal value As String)
            If (_AGoLencolSolteiro <> value) Then
                Me._AGoLencolSolteiro = value
            End If
        End Set
    End Property
    Public Property AGoLencolBerco() As String
        Get
            Return Me._AGoLencolBerco
        End Get
        Set(ByVal value As String)
            If (_AGoLencolBerco <> value) Then
                Me._AGoLencolBerco = value
            End If
        End Set
    End Property
    Public Property AGoCSolteiro() As Integer
        Get
            Return Me._AGoCSolteiro
        End Get
        Set(ByVal value As Integer)
            If (_AGoCSolteiro <> value) Then
                Me._AGoCSolteiro = value
            End If
        End Set
    End Property
    Public Property AGoBerco() As Integer
        Get
            Return Me._AGoBerco
        End Get
        Set(ByVal value As Integer)
            If (_AGoBerco <> value) Then
                Me._AGoBerco = value
            End If
        End Set
    End Property
    Public Property AGoTravesseiro() As Integer
        Get
            Return Me._AGoTravesseiro
        End Get
        Set(ByVal value As Integer)
            If (_AGoTravesseiro <> value) Then
                Me._AGoTravesseiro = value
            End If
        End Set
    End Property
    Public Property AGoJogoToalhas() As Integer
        Get
            Return Me._AGoJogoToalhas
        End Get
        Set(ByVal value As Integer)
            If (_AGoJogoToalhas <> value) Then
                Me._AGoJogoToalhas = value
            End If
        End Set
    End Property
    Public Property AGoRoloPapel() As Integer
        Get
            Return Me._AGoRoloPapel
        End Get
        Set(ByVal value As Integer)
            If (_AGoRoloPapel <> value) Then
                Me._AGoRoloPapel = value
            End If
        End Set
    End Property
    Public Property AGoSacoLixo() As Integer
        Get
            Return Me._AGoSacoLixo
        End Get
        Set(ByVal value As Integer)
            If (_AGoSacoLixo <> value) Then
                Me._AGoSacoLixo = value
            End If
        End Set
    End Property
    Public Property AGoSabonete() As Integer
        Get
            Return Me._AGoSabonete
        End Get
        Set(ByVal value As Integer)
            If (_AGoSabonete <> value) Then
                Me._AGoSabonete = value
            End If
        End Set
    End Property
    Public Property AGoTapete() As Char
        Get
            Return Me._AGoTapete
        End Get
        Set(ByVal value As Char)
            If (_AGoTapete <> value) Then
                Me._AGoTapete = value
            End If
        End Set
    End Property
    Public Property AGoOrigem() As Char
        Get
            Return Me._AGoOrigem
        End Get
        Set(ByVal value As Char)
            If (_AGoOrigem <> value) Then
                Me._AGoOrigem = value
            End If
        End Set
    End Property
    Public Property AGoObservacao() As String
        Get
            Return Me._AGoObservacao
        End Get
        Set(ByVal value As String)
            If (_AGoObservacao <> value) Then
                Me._AGoObservacao = value
            End If
        End Set
    End Property
    Public Property Acao() As Char
        Get
            Return Me._Acao
        End Get
        Set(ByVal value As Char)
            If (_Acao <> value) Then
                Me._Acao = value
            End If
        End Set
    End Property
    Public Property ApaCamaExtra() As Integer
        Get
            Return Me._ApaCamaExtra
        End Get
        Set(ByVal value As Integer)
            If (_ApaCamaExtra <> value) Then
                Me._ApaCamaExtra = value
            End If
        End Set
    End Property
    Public Property ApaBerco() As Integer
        Get
            Return Me._ApaBerco
        End Get
        Set(ByVal value As Integer)
            If (_ApaBerco <> value) Then
                Me._ApaBerco = value
            End If
        End Set
    End Property
    Public Property Usuario() As String
        Get
            Return Me._Usuario
        End Get
        Set(ByVal value As String)
            If (_Usuario <> value) Then
                Me._Usuario = value
            End If
        End Set
    End Property
    'CRIADO PARA UTILIZARO NA PROCEDURE ATUALIZA GOVERNAÇA
    Public Property Manutencao() As String
        Get
            Return Me._Manutencao
        End Get
        Set(ByVal value As String)
            If (_Manutencao <> value) Then
                Me._Manutencao = value
            End If
        End Set
    End Property
    Public Property AlteraManutencao() As String
        Get
            Return Me._AlteraManutencao
        End Get
        Set(ByVal value As String)
            If (_AlteraManutencao <> value) Then
                Me._AlteraManutencao = value
            End If
        End Set
    End Property
    Public Property Dia() As Long
        Get
            Return Me._Dia
        End Get
        Set(ByVal value As Long)
            If (_Dia <> value) Then
                Me._Dia = value
            End If
        End Set
    End Property
    Public Property ManDescricaoResquisitante() As String
        Get
            Return Me._ManDescricaoResquisitante
        End Get
        Set(ByVal value As String)
            If (_ManDescricaoResquisitante <> value) Then
                Me._ManDescricaoResquisitante = value
            End If
        End Set
    End Property
    Public Property CCusto() As String
        Get
            Return Me._CCusto
        End Get
        Set(ByVal value As String)
            If (_CCusto <> value) Then
                Me._CCusto = value
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






    'OLD'
    'Private _Acmid As String
    'Private _Acmidcobranca As String
    'Private _Apaid As String
    'Private _Soldataini As String
    'Private _Solhoraini As String
    'Private _Soldatafim As String
    'Private _Solhorafim As String
    'Private _Soldatainiaux As String
    'Private _Soldatafimaux As String
    'Private _Solusuario As String
    'Private _Solusuariodata As String

    'Public Property Acmid() As String
    '    Get
    '        Return Me._Acmid
    '    End Get
    '    Set(ByVal value As String)
    '        If (_Acmid <> value) Then
    '            Me._Acmid = value
    '        End If
    '    End Set
    'End Property

    'Public Property Acmidcobranca() As String
    '    Get
    '        Return Me._Acmidcobranca
    '    End Get
    '    Set(ByVal value As String)
    '        If (_Acmidcobranca <> value) Then
    '            Me._Acmidcobranca = value
    '        End If
    '    End Set
    'End Property
    'Public Property Apaid() As String
    '    Get
    '        Return Me._Apaid
    '    End Get
    '    Set(ByVal value As String)
    '        If (_Apaid <> value) Then
    '            Me._Apaid = value
    '        End If
    '    End Set
    'End Property
    'Public Property Soldataini() As String
    '    Get
    '        Return Me._Soldataini
    '    End Get
    '    Set(ByVal value As String)
    '        If (_Soldataini <> value) Then
    '            Me._Soldataini = value
    '        End If
    '    End Set
    'End Property
    'Public Property Solhoraini() As String
    '    Get
    '        Return Me._Solhoraini
    '    End Get
    '    Set(ByVal value As String)
    '        If (_Solhoraini <> value) Then
    '            Me._Solhoraini = value
    '        End If
    '    End Set
    'End Property
    'Public Property Soldatafim() As String
    '    Get
    '        Return Me._Soldatafim
    '    End Get
    '    Set(ByVal value As String)
    '        If (_Soldatafim <> value) Then
    '            Me._Soldatafim = value
    '        End If
    '    End Set
    'End Property
    'Public Property Solhorafim() As String
    '    Get
    '        Return Me._Soldatafim
    '    End Get
    '    Set(ByVal value As String)
    '        If (_Solhorafim <> value) Then
    '            Me._Solhorafim = value
    '        End If
    '    End Set
    'End Property
    'Public Property Soldatainiaux() As String
    '    Get
    '        Return Me._Soldatainiaux
    '    End Get
    '    Set(ByVal value As String)
    '        If (_Soldatainiaux <> value) Then
    '            Me._Soldatainiaux = value
    '        End If
    '    End Set
    'End Property
    'Public Property Soldatafimaux() As String
    '    Get
    '        Return Me._Soldatafimaux
    '    End Get
    '    Set(ByVal value As String)
    '        If (_Soldatafimaux <> value) Then
    '            Me._Soldatainiaux = value
    '        End If
    '    End Set
    'End Property
    'Public Property Solusuario() As String
    '    Get
    '        Return Me._Solusuario
    '    End Get
    '    Set(ByVal value As String)
    '        If (_Solusuario <> value) Then
    '            Me._Solusuario = value
    '        End If
    '    End Set
    'End Property
    'Public Property Solusuariodata() As String
    '    Get
    '        Return Me._Solusuariodata
    '    End Get
    '    Set(ByVal value As String)
    '        If (_Solusuariodata <> value) Then
    '            Me._Solusuariodata = value
    '        End If
    '    End Set
    'End Property
End Class
