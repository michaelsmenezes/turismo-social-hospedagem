Imports Microsoft.VisualBasic

Public Class BemVO
    Private _Patrimonio As String
    Private _CentroCusto As String
    Private _NomeBem As String
    Private _SituacaoBem As String
    Private _Area As String
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
    Public Property NomeBem() As String
        Get
            Return Me._NomeBem
        End Get
        Set(ByVal value As String)
            If (_NomeBem <> value) Then
                Me._NomeBem = value
            End If
        End Set
    End Property
    Public Property SituacaoBem() As String
        Get
            Return Me._SituacaoBem
        End Get
        Set(ByVal value As String)
            If (_SituacaoBem <> value) Then
                Me._SituacaoBem = value
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
End Class
