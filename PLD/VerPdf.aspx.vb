Public Class VerPdf
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not Me.IsPostBack Then

            Dim filePath As String = "\\server-NAS\TMPFINAGIL\PLD\" & Session.Item("nombre1") & ".pdf"
            Response.Clear()
            Response.ContentType = "application/pdf"
            'Response.AddHeader("Content-disposition", "attachment; filename=" & "DENISE.pdf")
            Response.WriteFile(filePath)
            Response.Flush()
            Response.Close()

        End If
    End Sub
End Class