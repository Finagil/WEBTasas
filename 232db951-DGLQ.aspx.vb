Imports CrystalDecisions.CrystalReports.Engine
Imports CrystalDecisions.Shared
Imports System.IO
Partial Public Class DGSucursalLQForm
    Inherits System.Web.UI.Page
    Dim Tipo As String
    Dim TaQUERY As New CreditoDSTableAdapters.LlavesTableAdapter
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If IsNothing(Request("User")) Then
            Panel1.Visible = False
            LbError.Visible = True
        Else
            Dim ta As New ProDSTableAdapters.SolLiqTableAdapter
            Dim Correos As New SeguridadDSTableAdapters.UsuariosFinagilTableAdapter
            Dim t As New ProDS.SolLiqDataTable
            ta.Fill(t, Request("User"))
            If t.Rows.Count > 0 Then
                If Request("ID") = 0 Then
                    Panel1.Visible = False
                    LbError.Visible = False
                    BotonEnviar1.Visible = False
                    BotonEnviar2.Visible = False
                    BotonEnviar3.Visible = False
                Else
                    Panel1.Visible = True
                    LbError.Visible = False
                    BotonEnviar1.Visible = True
                    BotonEnviar2.Visible = True
                    BotonEnviar3.Visible = True
                End If
            Else
                Panel1.Visible = False
                LbError.Visible = True
            End If
        End If
    End Sub

    Protected Sub BotonEnviar1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BotonEnviar1.Click
        Dim Cliente As String = DetailsView1.Rows(1).Cells(1).Text
        Dim Monto As Decimal = CDec(DetailsView1.Rows(3).Cells(1).Text)
        Dim Nombre As String = DetailsView1.Rows(2).Cells(1).Text.Trim
        Dim Analista As String = DetailsView1.Rows(13).Cells(1).Text.Trim
        Dim ta As New ProDSTableAdapters.SolLiqTableAdapter
        ta.UpdateEstatus("APROBADO", Request("User"), True, Date.Now, Encriptar(Request("User") & Date.Now.ToString), Request("ID"))
        If TextComentarioAUT.Text.Length > 0 Then
            ta.UpdateComentariosDG(TextComentarioAUT.Text, Request("ID"))
        End If
        Globales.AltaLineaCreditoLIQUIDEZ1(Cliente, Monto, "Autorizado por " & Request("User"), Request("User"), Request("ID"))
        GeneraCorreoAUT(Monto, Cliente, Nombre, Analista)
        Response.Redirect("~\232db951-DGLQ.aspx?User=" & Request("User") & "&ID=0")
    End Sub

    Sub GeneraCorreoAUT(Monto As Decimal, Cliente As String, nombre As String, Analista As String)
        Dim ta As New SeguridadDSTableAdapters.UsuariosFinagilTableAdapter
        Dim Asunto As String = ""
        Dim Fecha As Date = DetailsView1.Rows(14).Cells(1).Text.Trim
        Dim Antiguedad As Integer = DateDiff(DateInterval.Year, Fecha, Date.Now.Date)
        Dim File As String = "\LQ\Autoriza" & CInt(Request("ID")) & ".Pdf"
        Asunto = "Solicitud de Liquidez Inmediata Autorizada(" & Request("ID") & "): " & nombre
        Dim Mensaje As String = ""

        Mensaje += "Solicitud: " & Request("ID") & "<br>"
        Mensaje += "Cliente: " & nombre & "<br>"
        Mensaje += "Monto Financiado: " & Monto.ToString("n2") & "<br>"

        MandaCorreo(Request("User") & "@finagil.com.mx", "ecacerest@finagil.com.mx", Asunto, Mensaje, File)
        MandaCorreo(Request("User") & "@finagil.com.mx", ta.ScalarCorreo(Analista), Asunto, Mensaje, File)
        MandaCorreo(Request("User") & "@finagil.com.mx", TaQUERY.SacaCorreoPromo(Cliente), Asunto, Mensaje, File)

    End Sub

    Protected Sub BotonEnviar2_Click(sender As Object, e As EventArgs) Handles BotonEnviar2.Click
        Dim Cliente As String = DetailsView1.Rows(1).Cells(1).Text
        Dim Monto As Decimal = CDec(DetailsView1.Rows(3).Cells(1).Text)
        Dim Nombre As String = DetailsView1.Rows(2).Cells(1).Text.Trim
        Dim Analista As String = DetailsView1.Rows(11).Cells(1).Text.Trim
        Dim ta As New ProDSTableAdapters.SolLiqTableAdapter
        ta.UpdateEstatus("RECHAZADO", Request("User"), True, Date.Now, Encriptar(Request("User") & Date.Now.ToString), Request("ID"))
        GeneraCorreoRECHAZO(Monto, Cliente, Nombre, Analista)
        Response.Redirect("~\232db951-DGLQ.aspx?User=" & Request("User") & "&ID=0")
    End Sub

    Sub GeneraCorreoRECHAZO(Monto As Decimal, Cliente As String, nombre As String, Analista As String)
        Dim ta As New SeguridadDSTableAdapters.UsuariosFinagilTableAdapter
        Dim Asunto As String = ""
        Asunto = "Solicitud de Liquidez Inmediata Rechazada(" & Request("ID") & "): " & nombre
        Dim Mensaje As String = ""

        Mensaje += "Solicitud: " & Request("ID") & "<br>"
        Mensaje += "Cliente: " & nombre & "<br>"
        Mensaje += "Monto Financiado: " & Monto.ToString("n2") & "<br>"
        Mensaje += "Comentarios DG: " & TextComentarioCorreo.Text & "<br>"

        MandaCorreo(Request("User") & "@Fiangil.com.mx", "ecacerest@finagil.com.mx", Asunto, Mensaje)
        MandaCorreo(Request("User") & "@Fiangil.com.mx", ta.ScalarCorreo(Analista), Asunto, Mensaje)
        MandaCorreo(Request("User") & "@Fiangil.com.mx", TaQUERY.SacaCorreoPromo(Cliente), Asunto, Mensaje)

    End Sub

    Sub GeneraCorreoCOMENTARIO(Monto As Decimal, Cliente As String, nombre As String, Analista As String)
        Dim ta As New SeguridadDSTableAdapters.UsuariosFinagilTableAdapter
        Dim Asunto As String = ""
        Asunto = "Solicitud de Liquidez Inmediata COMENTARIO DG(" & Request("ID") & "): " & nombre
        Dim Mensaje As String = ""

        Mensaje += "Solicitud: " & Request("ID") & "<br>"
        Mensaje += "Cliente: " & nombre & "<br>"
        Mensaje += "Monto Financiado: " & Monto.ToString("n2") & "<br>"
        Mensaje += "Comentarios DG: " & TextComentarioCorreo.Text & "<br>"

        MandaCorreo(Request("User") & "@Finagil.com.mx", "ecacerest@finagil.com.mx", Asunto, Mensaje)
        MandaCorreo(Request("User") & "@Finagil.com.mx", ta.ScalarCorreo(Analista), Asunto, Mensaje)
        MandaCorreo(Request("User") & "@Finagil.com.mx", TaQUERY.SacaCorreoPromo(Cliente), Asunto, Mensaje)

    End Sub

    Protected Sub BotonEnviar3_Click(sender As Object, e As EventArgs) Handles BotonEnviar3.Click
        Dim Cliente As String = DetailsView1.Rows(1).Cells(1).Text
        Dim Monto As Decimal = CDec(DetailsView1.Rows(3).Cells(1).Text)
        Dim Nombre As String = DetailsView1.Rows(2).Cells(1).Text.Trim
        Dim Analista As String = DetailsView1.Rows(11).Cells(1).Text.Trim
        Dim ta As New ProDSTableAdapters.SolLiqTableAdapter
        GeneraCorreoCOMENTARIO(Monto, Cliente, Nombre, Analista)
        Response.Redirect("~\232db951-DGLQ.aspx?User=" & Request("User") & "&ID=0")
    End Sub
End Class
