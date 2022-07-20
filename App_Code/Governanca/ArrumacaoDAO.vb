Imports System
Imports System.Collections
Imports System.Text
Imports Microsoft.VisualBasic
Public Class ArrumacaoDAO
    Dim ObjArrumacaoVO As ArrumacaoVO
    Dim ObjArrumacaoDAO As ArrumacaoDAO

    Public Function PesquisaArrumacao(ByVal ObjArrumacaoVO As ArrumacaoVO, ByVal Federacao As String, ByVal AliasBanco As String) As IList
        Try
            Dim Conn = New Banco.Conexao(AliasBanco)
            Dim VarSql = New Text.StringBuilder("SET NOCOUNT ON ")
            With VarSql
                If Federacao <> "" Then
                    .Append("SELECT a.ApaId, a.ApaDesc, aa.AcmFederacao AS TipoAcomodacao,a.ApaCusto,a.ApaArea,  ")
                    .Append("isnull((select top 1 'S' ")
                    .Append("FROM ")
                    .Append("TBMANUTENCAO M WITH (NOLOCK) ")
                    .Append("where(M.APAID = A.APAID) ")
                    .Append("and NOT EXISTS (SELECT 1 FROM TBATENDIMENTOGOV G WITH (NOLOCK) WHERE G.APAID = A.APAID ")
                    .Append("AND G.AGODATA > M.MANDATACONCLUSAO) ")
                    .Append("AND A.APASTATUS IN ('A','O')),'N') SaidoManutencao, ")
                    .Append("isnull((select top 1 G.AGOID  FROM TBATENDIMENTOGOV G WITH (NOLOCK) where(G.APAID = A.APAID) ")
                    .Append("AND G.AGODATA IS NULL AND A.APASTATUS IN ('A','O')),0) AgoId  ")
                    .Append("FROM TbApartamento a ")
                    .Append("INNER JOIN TbAcomodacao aa ON a.AcmId = aa.AcmId WHERE a.ApaStatus = 'A' AND NOT EXISTS ")
                    .Append("(SELECT 1 FROM TbAptoConferencia c WHERE a.ApaId = c.ApaId AND c.ApaConDtConferencia IS NULL)  ")
                    If ObjArrumacaoVO.ApaDesc <> " " Then
                        .Append("AND a.ApaDesc LIKE '%" & ObjArrumacaoVO.ApaDesc & "%' ")
                    End If
                    .Append("AND a.ApaFederacao = '" & Federacao & "' ")
                    .Append("order by a.ApaId ")
                Else
                    .Append("SELECT a.ApaId, a.ApaDesc, aa.AcmFederacao AS TipoAcomodacao,a.ApaCusto,a.ApaArea,  ")
                    .Append("isnull((select top 1 'S' ")
                    .Append("FROM ")
                    .Append("TBMANUTENCAO M WITH (NOLOCK) ")
                    .Append("where(M.APAID = A.APAID) ")
                    .Append("and NOT EXISTS (SELECT 1 FROM TBATENDIMENTOGOV G WITH (NOLOCK) WHERE G.APAID = A.APAID ")
                    .Append("AND G.AGODATA > M.MANDATACONCLUSAO) ")
                    .Append("AND A.APASTATUS IN ('A','O')),'N') SaidoManutencao, ")
                    .Append("isnull((select top 1 G.AGOID  FROM TBATENDIMENTOGOV G WITH (NOLOCK) where(G.APAID = A.APAID) ")
                    .Append("AND G.AGODATA IS NULL AND A.APASTATUS IN ('A','O')),0) AgoId  ")
                    .Append("FROM TbApartamento a ")
                    .Append("INNER JOIN TbAcomodacao aa ON a.AcmId = aa.AcmId WHERE a.ApaStatus = 'A' AND NOT EXISTS ")
                    .Append("(SELECT 1 FROM TbAptoConferencia c WHERE a.ApaId = c.ApaId AND c.ApaConDtConferencia IS NULL)  ")
                    If ObjArrumacaoVO.ApaDesc <> " " Then
                        .Append("AND a.ApaDesc LIKE '%" & ObjArrumacaoVO.ApaDesc & "%' ")
                    End If
                    '.Append("AND a.ApaFederacao = '" & Federacao & "' ")
                    If AliasBanco = "TurismoSocialPiri" Then
                        .Append("order by a.APADESC ")
                    Else
                        .Append("order by a.APAID ")
                    End If
                End If
                Return Me.PreencheListaArrumacao(Conn.consulta(VarSql.ToString))
            End With
        Catch ex As Exception
            Throw New Exception(ex.StackTrace.ToString.Replace(Chr(13), ""))
        End Try
    End Function
    Private Function PreencheListaArrumacao(ByVal ResultadoConsulta As System.Data.SqlClient.SqlDataReader) As IList
        Dim Lista As IList
        Lista = New ArrayList
        'Faz a leitura nas linhas da consulta e preenche o objeto'
        If ResultadoConsulta.HasRows Then
            While ResultadoConsulta.Read
                ObjArrumacaoVO = New ArrumacaoVO
                'If (ResultadoConsulta.Item("resNome").ToString.IndexOf("Sistema") > 0) Then
                With ObjArrumacaoVO
                    .ApaId = ResultadoConsulta.Item("ApaId")
                    .ApaDesc = ResultadoConsulta.Item("ApaDesc")
                    .TipoAcomodacao = ResultadoConsulta.Item("TipoAcomodacao")
                    .SaidoManutencao = ResultadoConsulta.Item("SaidoManutencao")
                    .AgoId = ResultadoConsulta.Item("AgoId")
                    If Convert.IsDBNull(ResultadoConsulta.Item("ApaCusto")) Then
                        .ApaCCusto = " "
                    Else
                        .ApaCCusto = ResultadoConsulta.Item("ApaCusto")
                    End If
                    If Convert.IsDBNull(ResultadoConsulta.Item("ApaArea")) Then
                        .ApaArea = " "
                    Else
                        .ApaArea = ResultadoConsulta.Item("ApaArea")
                    End If
                    '.ApaHTML = "Vazio"
                    Lista.Add(ObjArrumacaoVO)
                End With
            End While
        Else
            ''Se não tiver resultado posivito. Ou seja, nenhuma linha'
            'ObjArrumacaoVO = New ArrumacaoVO
            'With ObjArrumacaoVO
            '    .ApaId = ""
            '    .ApaDesc = ""
            '    .TipoAcomodacao = ""
            '    Lista.Add(ObjArrumacaoVO)
            'End With
        End If
        ResultadoConsulta.Close()
        Return Lista
    End Function
End Class
