Imports SevenZip
Imports DevExpress.XtraEditors
Imports System.IO

Public Class cls7z
    Public Shared Function TheCompresor(ByVal NamaFileZip As String,
                                        ByVal Pwd As String,
                                        ByVal KumpulanFile() As String,
                                        Optional ByVal Compresor As CompressionLevel = CompressionLevel.Fast) As Boolean
        Dim Compresor7z As New SevenZipCompressor()
        Dim Hasil As Boolean = False
        Try
            SevenZip.SevenZipCompressor.SetLibraryPath(Application.StartupPath & "\7z.dll")
            With Compresor7z
                .ArchiveFormat = OutArchiveFormat.SevenZip
                .CompressionMode = CompressionMode.Create
                .CompressionMethod = CompressionMethod.Lzma
                .DirectoryStructure = False
                .CompressionLevel = Compresor
            End With
            If Pwd <> "" Then
                Compresor7z.CompressFilesEncrypted(NamaFileZip, "Sg1", KumpulanFile)
            Else
                Compresor7z.CompressFiles(NamaFileZip, KumpulanFile)
            End If
            Hasil = True
        Catch ex As Exception
            'PesanError = "Pesan Kesalahan : " & ex.Message
            Hasil = False
        Finally
            Compresor7z = Nothing
        End Try
        Return Hasil
    End Function
    Friend Shared Function TheExtractor(ByVal NamaFileZip As String, ByVal NamaPath As String, Optional ByVal Pwd As String = "", Optional ByVal PesanError As String = "") As Boolean
        Dim Compresor7z As New SevenZipExtractor(NamaFileZip)
        Dim Hasil As Boolean = False
        Try
            SevenZip.SevenZipExtractor.SetLibraryPath(Application.StartupPath & "\7z.dll")

            Compresor7z.ExtractArchive(NamaPath)
            Hasil = True
        Catch ex As Exception
            PesanError = "Pesan Kesalahan : " & ex.Message
            Hasil = False
        Finally
            Compresor7z = Nothing
        End Try
        Return Hasil
    End Function
End Class
