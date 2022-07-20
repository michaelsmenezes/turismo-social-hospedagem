Imports Microsoft.VisualBasic

Public Class HistoricoRefeicaoVO
    Private _Dia As String
    Private _DesCom As Long
    Private _DesDep As Long
    Private _DesCon As Long
    Private _DesUsu As Long
    Private _DesIse As Long
    Private _DesMen5 As Long
    Private _DesSer As Long
    Private _AlmCom As Long
    Private _AlmDep As Long
    Private _AlmCon As Long
    Private _AlmUsu As Long
    Private _AlmIse As Long
    Private _AlmMen5 As Long
    Private _AlmSer As Long
    Private _JanCom As Long
    Private _JanDep As Long
    Private _JanCon As Long
    Private _JanUsu As Long
    Private _JanIse As Long
    Private _JanMen5 As Long
    Private _JanSer As Long
    'Buscando refeições dos funcionarios
    Private _DesjejumFunc As Long
    Private _AlmocoFunc As Long
    Private _JantarFunc As Long

    'Complemento de diária do Restaurante - Refeição extra da diária
    Private _Quantidade As String
    Private _Data As String
    Private _DiaSemana As String
    Private _TotalAlmoco As String
    Private _CheckOut As String

    Public Property Dia() As String
        Get
            Return Me._Dia
        End Get
        Set(ByVal value As String)
            If (_Dia) <> value Then
                Me._Dia = value
            End If
        End Set
    End Property
    Public Property DesCom() As Long
        Get
            Return Me._DesCom
        End Get
        Set(ByVal value As Long)
            If (_DesCom <> value) Then
                Me._DesCom = value
            End If
        End Set
    End Property
    Public Property DesDep() As Long
        Get
            Return Me._DesDep
        End Get
        Set(ByVal value As Long)
            If (_DesDep <> value) Then
                Me._DesDep = value
            End If
        End Set
    End Property
    Public Property DesCon() As Long
        Get
            Return Me._DesCon
        End Get
        Set(ByVal value As Long)
            If (_DesCon <> value) Then
                Me._DesCon = value
            End If
        End Set
    End Property
    Public Property DesUsu() As Long
        Get
            Return Me._DesUsu
        End Get
        Set(ByVal value As Long)
            If (_DesUsu <> value) Then
                Me._DesUsu = value
            End If
        End Set
    End Property
    Public Property DesIse() As Long
        Get
            Return Me._DesIse
        End Get
        Set(ByVal value As Long)
            If (_DesIse <> value) Then
                Me._DesIse = value
            End If
        End Set
    End Property
    Public Property DesMen5() As Long
        Get
            Return Me._DesMen5
        End Get
        Set(ByVal value As Long)
            If (_DesMen5 <> value) Then
                Me._DesMen5 = value
            End If
        End Set
    End Property
    Public Property DesSer() As Long
        Get
            Return Me._DesSer
        End Get
        Set(ByVal value As Long)
            If (_DesSer <> value) Then
                Me._DesSer = value
            End If
        End Set
    End Property
    Public Property AlmCom() As Long
        Get
            Return Me._AlmCom
        End Get
        Set(ByVal value As Long)
            If (_AlmCom <> value) Then
                Me._AlmCom = value
            End If
        End Set
    End Property
    Public Property AlmDep() As Long
        Get
            Return Me._AlmDep
        End Get
        Set(ByVal value As Long)
            If (_AlmDep <> value) Then
                Me._AlmDep = value
            End If
        End Set
    End Property
    Public Property AlmCon() As Long
        Get
            Return Me._AlmCon
        End Get
        Set(ByVal value As Long)
            If (_AlmCon <> value) Then
                Me._AlmCon = value
            End If
        End Set
    End Property
    Public Property AlmUsu() As Long
        Get
            Return Me._AlmUsu
        End Get
        Set(ByVal value As Long)
            If (_AlmUsu <> value) Then
                Me._AlmUsu = value
            End If
        End Set
    End Property
    Public Property AlmIse() As Long
        Get
            Return Me._AlmIse
        End Get
        Set(ByVal value As Long)
            If (_AlmIse <> value) Then
                Me._AlmIse = value
            End If
        End Set
    End Property
    Public Property AlmMen5() As Long
        Get
            Return Me._AlmMen5
        End Get
        Set(ByVal value As Long)
            If (_AlmMen5 <> value) Then
                Me._AlmMen5 = value
            End If
        End Set
    End Property
    Public Property AlmSer() As Long
        Get
            Return Me._AlmSer
        End Get
        Set(ByVal value As Long)
            If (_AlmSer <> value) Then
                Me._AlmSer = value
            End If
        End Set
    End Property
    Public Property JanCom() As Long
        Get
            Return Me._JanCom
        End Get
        Set(ByVal value As Long)
            If (_JanCom <> value) Then
                Me._JanCom = value
            End If
        End Set
    End Property
    Public Property JanDep() As Long
        Get
            Return Me._JanDep
        End Get
        Set(ByVal value As Long)
            If (_JanDep <> value) Then
                Me._JanDep = value
            End If
        End Set
    End Property
    Public Property JanCon() As Long
        Get
            Return Me._JanCon
        End Get
        Set(ByVal value As Long)
            If (_JanCon <> value) Then
                Me._JanCon = value
            End If
        End Set
    End Property
    Public Property JanUsu() As Long
        Get
            Return Me._JanUsu
        End Get
        Set(ByVal value As Long)
            If (_JanUsu <> value) Then
                Me._JanUsu = value
            End If
        End Set
    End Property
    Public Property JanIse() As Long
        Get
            Return Me._JanIse
        End Get
        Set(ByVal value As Long)
            If (_JanIse <> value) Then
                Me._JanIse = value
            End If
        End Set
    End Property
    Public Property JanMen5() As Long
        Get
            Return Me._JanMen5
        End Get
        Set(ByVal value As Long)
            If (_JanMen5 <> value) Then
                Me._JanMen5 = value
            End If
        End Set
    End Property
    Public Property JanSer() As Long
        Get
            Return Me._JanSer
        End Get
        Set(ByVal value As Long)
            If (_JanSer <> value) Then
                Me._JanSer = value
            End If
        End Set
    End Property
    Public Property DesjejumFunc() As Long
        Get
            Return Me._DesjejumFunc
        End Get
        Set(ByVal value As Long)
            If (_DesjejumFunc <> value) Then
                Me._DesjejumFunc = value
            End If
        End Set
    End Property
    Public Property AlmocoFunc() As Long
        Get
            Return Me._AlmocoFunc
        End Get
        Set(ByVal value As Long)
            If (_AlmocoFunc <> value) Then
                Me._AlmocoFunc = value
            End If
        End Set
    End Property
    Public Property JantarFunc() As Long
        Get
            Return Me._JantarFunc
        End Get
        Set(ByVal value As Long)
            If (_JantarFunc <> value) Then
                Me._JantarFunc = value
            End If
        End Set
    End Property

    Public Property Quantidade As String
        Get
            Return _Quantidade
        End Get
        Set(value As String)
            _Quantidade = value
        End Set
    End Property
    Public Property Data As String
        Get
            Return _Data
        End Get
        Set(value As String)
            _Data = value
        End Set
    End Property
    Public Property DiaSemana As String
        Get
            Return _DiaSemana
        End Get
        Set(value As String)
            _DiaSemana = value
        End Set
    End Property
    Public Property TotalAlmoco As String
        Get
            Return _TotalAlmoco
        End Get
        Set(value As String)
            _TotalAlmoco = value
        End Set
    End Property

    Public Property CheckOut As String
        Get
            Return _CheckOut
        End Get
        Set(value As String)
            _CheckOut = value
        End Set
    End Property

End Class
