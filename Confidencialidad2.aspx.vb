Imports CrystalDecisions.CrystalReports.Engine
Imports CrystalDecisions.Shared


Public Class Confidencialidad2
    Inherits System.Web.UI.Page
    Public nombre As String
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim rptCARTA As New ReportDocument

        rptCARTA.Load(Server.MapPath("~/Confidencialidad_rpt.rpt"))
        rptCARTA.Refresh()
        Session.Item("nombre1") = UCase(Request.QueryString("nombre"))
        nombre = UCase(Request.QueryString("nombre"))
        Dim empresa As String = UCase(Request.QueryString("empresa"))
        Dim equipo As String = UCase(Request.QueryString("equipo"))
        rptCARTA.SetParameterValue("nombre", nombre)
        rptCARTA.SetParameterValue("empresa", empresa)
        rptCARTA.SetParameterValue("equipo", equipo)
        Dim rutaPDF As String = "\\Server-nas\tmpfinagil\PLD\" & nombre & ".pdf"
        Dim rutaPDF2 As String = Server.MapPath(rutaPDF)
        rptCARTA.ExportToDisk(ExportFormatType.PortableDocFormat, "\\Server-nas\tmpfinagil\PLD\" & nombre & ".pdf")
        Response.Write("<script>")
        Response.Write("window.open('verPdf.aspx','popup','_blank','width=200,height=200')")
        Response.Write("</script>")
        rptCARTA.Dispose()
        'Response.Redirect("close.html")

    End Sub


End Class