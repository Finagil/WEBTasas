Imports CrystalDecisions.CrystalReports.Engine
Imports CrystalDecisions.Shared


Public Class Confidencialidad2
    Inherits System.Web.UI.Page
    Public nombre As String
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim rptCARTA As New Confidencialidad_rpt

        rptCARTA.Load(Server.MapPath("~/PLD/Confidencialidad_rpt.rpt"))
        rptCARTA.Refresh()
        Session.Item("nombre1") = UCase(Request.QueryString("nombre"))
        nombre = UCase(Request.QueryString("nombre"))
        Dim empresa As String = UCase(Request.QueryString("empresa"))
        Dim equipo As String = UCase(Request.QueryString("equipo"))
        rptCARTA.SetParameterValue("nombre", nombre)
        rptCARTA.SetParameterValue("empresa", empresa)
        rptCARTA.SetParameterValue("equipo", equipo)
        'Dim NOMBREX As String = FormatDateTime(Of Date,1)()


        Dim rutaPDF As String = FormatDateTime(Now)
        rutaPDF = rutaPDF.Replace("/", "")
        rutaPDF = rutaPDF.Replace(".", "")
        rutaPDF = rutaPDF.Replace(" ", "")
        rutaPDF = rutaPDF.Replace(":", "")
        Dim nombrepdf As String = rutaPDF & ".pdf"
        rutaPDF = "~/tmp/" & rutaPDF & ".pdf"



        'Dim rutaPDF As String = "\\Server-nas\tmpfinagil\PLD\" & nombre & ".pdf"
        Dim rutaPDF2 As String = Server.MapPath(rutaPDF)
        rptCARTA.ExportToDisk(ExportFormatType.PortableDocFormat, rutaPDF2)
        'Dim RUTA2 As String = "~/tmp/" & Date.Now & Hour(Now) & " : " & Minute(Now) & " : " & Second(Now) & ".pdf"


        rptCARTA.Dispose()
        'Response.Redirect("close.html")
        My.Computer.FileSystem.CopyFile(Server.MapPath(rutaPDF), "\\Server-nas\tmpfinagil\PLD\" & nombre & ".pdf", True)



        If Not Me.IsPostBack Then
            Dim filePath As String = nombrepdf '"~/Procesados/" & Request.QueryString("fileName") & ".pdf"
            Response.ContentType = "Application/pdf"
            Response.AddHeader("Content-disposition", "attachment; filename=" & filePath)
            Response.TransmitFile(Server.MapPath(rutaPDF))
            Response.[End]()
        End If


    End Sub


End Class