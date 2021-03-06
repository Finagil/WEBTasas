Imports CrystalDecisions.CrystalReports.Engine
Imports CrystalDecisions.Shared

Partial Public Class CPXForm
    Inherits System.Web.UI.Page
    Dim taOBS As New ProDSTableAdapters.CXP_ObservacionesSolicitudTableAdapter
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            'GeneraArchivo("CXP\TEST.PDF", 23, "o4fdAtt7Wzxr42AXcQcfnp1V/nb9uzHhkbYyBqFeOu8MihZVIukiSyyX", 3660, "No Pagada", "PSC", True, 1232)
            Label1.Text = My.Settings.RUTA_TMP
            Session.Item("MCONTROL_CXP") = SacaCorreoFase("MCONTROL_CXP")
            LbAutorizante.Visible = False
            ListAutorizante.Visible = False
            If Not IsNothing(Request("User")) Then
                Session.Item("User") = Request("User")
                Response.Redirect("~\5Afdb804-7cXp.aspx")
            End If
            If IsNothing(Session("User")) Then
                Panel1.Visible = False
                LbError.Visible = True
            Else
                Dim ta As New ProDSTableAdapters.Vw_CXP_AutorizacionesTableAdapter
                Dim t As New ProDS.Vw_CXP_AutorizacionesDataTable
                ta.Fill(t, Session("User"))
                Dim R As ProDS.Vw_CXP_AutorizacionesRow
                If t.Rows.Count > 0 Then
                    If InStr(Session("MCONTROL_CXP"), Session("User")) Then
                        LbAutorizante.Visible = True
                        ListAutorizante.Visible = True
                    End If
                    R = t.Rows(0)
                    If Session("ID1") > "0" Then
                        Panel1.Visible = True
                        If Not IsNothing(Session("ID4")) Then
                            GridView1.SelectedIndex = Session("ID4")
                        End If
                    Else
                        Panel1.Visible = False
                    End If
                    LbError.Visible = False
                Else
                    Panel1.Visible = False
                    LbError.Visible = True
                End If
            End If
        Catch ex As Exception
            Panel1.Visible = False
            LbError.Visible = True
        End Try
    End Sub

    Sub GeneraArchivo(Archivo As String, Empresa As Decimal, FirmaSol As String, Solicitud As Decimal, Estatus As String, Serie As String, Contrato As Boolean, idCuentas As Integer)
        Dim ta As New ProDSTableAdapters.Vw_CXP_AutorizacionesRPTTableAdapter
        Dim ds0 As New ProDS
        Dim ds1 As New ProDS
        Dim ds2 As New ProDS
        Dim rptSolPago As Object
        ta.Fill(ds0.Vw_CXP_AutorizacionesRPT, Empresa, Solicitud, Estatus)
        taOBS.Fill(ds0.CXP_ObservacionesSolicitud, Empresa, Solicitud)

        Dim taCtasBancarias As New ProDSTableAdapters.CXP_CuentasBancariasProvTableAdapter
        Dim dtCtasBanco As New ProDS.CXP_CuentasBancariasProvDataTable
        taCtasBancarias.ObtCtaPago_FillBy(ds0.CXP_CuentasBancariasProv, idCuentas)


        If Serie = "PSC" Then
            ta.DetalleND_FillBy(ds1.Vw_CXP_AutorizacionesRPT, Empresa, Solicitud)
            ta.DetalleSD_FillBy(ds2.Vw_CXP_AutorizacionesRPT, Empresa, Solicitud)
            rptSolPago = New rptSolicitudDePagoSCC

            rptSolPago.SetDataSource(ds0)
            rptSolPago.Subreports("rptSubObservaciones").SetDataSource(ds0)
            rptSolPago.Subreports("rptSubSolicitudSCND").SetDataSource(ds1)
            rptSolPago.Subreports("rptSubSolicitudSCSD").SetDataSource(ds2)
            rptSolPago.Subreports("rptSubCtasBancarias").SetDataSource(ds0)
            rptSolPago.Refresh()

            rptSolPago.SetParameterValue("var_SD", ds2.Vw_CXP_AutorizacionesRPT.Rows.Count)
            rptSolPago.SetParameterValue("var_ND", ds1.Vw_CXP_AutorizacionesRPT.Rows.Count)
            rptSolPago.SetParameterValue("var_genero", FirmaSol)
            rptSolPago.SetParameterValue("var_observaciones", ds0.CXP_ObservacionesSolicitud.Rows.Count.ToString)
            rptSolPago.SetParameterValue("var_contrato", Contrato)
            rptSolPago.SetParameterValue("var_idCuentas", idCuentas)

            Select Case Empresa
                Case 23
                    rptSolPago.SetParameterValue("var_pathImagen", Server.MapPath("~/IMG/LOGO FINAGIL.JPG"))
                Case 24
                    rptSolPago.SetParameterValue("var_pathImagen", Server.MapPath("~/IMG/logoArfin.JPG"))
            End Select
            rptSolPago.ExportToDisk(ExportFormatType.PortableDocFormat, My.Settings.RUTA_TMP & Archivo)
            rptSolPago.Dispose()
        Else
            rptSolPago = New rptSolicitudDePago
            rptSolPago.SetDataSource(ds0)
            rptSolPago.Subreports(0).SetDataSource(ds0)
            rptSolPago.Subreports(1).SetDataSource(ds0)

            rptSolPago.SetParameterValue("var_genero", FirmaSol)
            rptSolPago.SetParameterValue("var_observaciones", ds0.CXP_ObservacionesSolicitud.Rows.Count.ToString)
            rptSolPago.SetParameterValue("var_contrato", Contrato)
            rptSolPago.SetParameterValue("var_idCuentas", idCuentas)

            Select Case Empresa
                Case 23
                    rptSolPago.SetParameterValue("var_pathImagen", Server.MapPath("~/IMG/LOGO FINAGIL.JPG"))
                Case 24
                    rptSolPago.SetParameterValue("var_pathImagen", Server.MapPath("~/IMG/logoArfin.JPG"))
            End Select
            rptSolPago.ExportToDisk(ExportFormatType.PortableDocFormat, My.Settings.RUTA_TMP & Archivo)
            rptSolPago.Dispose()
        End If



    End Sub

    Protected Sub BotonAutorizar_Click(sender As Object, e As EventArgs) Handles BotonAutorizar.Click
        Dim ta As New ProDSTableAdapters.Vw_CXP_AutorizacionesTableAdapter
        Dim t As New ProDS.Vw_CXP_AutorizacionesDataTable
        Dim r As ProDS.Vw_CXP_AutorizacionesRow
        Dim Firma As String = Encriptar(Date.Now.ToString("yyyyMMddhhmm") & Session("User") & Session("ID1") & "-" & Session("ID2"))
        Dim Mensaje As String = ""
        Dim Asunto As String = ""
        Dim Archivo As String
        ta.FillByID(t, Session("User"), Session("ID2"), Session("ID1"), Session("ID3"))
        r = t.Rows(0)
        Mensaje = "Solicitud: " & r.Solicitud & "<br>"
        Mensaje += "Emrpesa: " & r.NombreCorto & "<br>"
        Mensaje += "Estatus: " & r.Estatus & "<br>"
        Mensaje += "Importe: " & CDec(r.Total).ToString("n2") & "<br>"
        Mensaje += "Proveedor: " & r.razonSocial & "<br>"
        Archivo = "CXP\" & CInt(r.idEmpresa).ToString & "-" & CInt(r.Solicitud).ToString & ".pdf"
        Dim Firma2 As String = Encriptar(r.FechaSol.ToString("yyyyMMddhhmm") & r.MailSolicitante & Session("ID1") & "-" & Session("ID2"))

        If r.idConcepto <> 144 Then '144, (HONORARIOS PERSONAS MORALES - ARFIN).
            ta.Ok1(Firma, Session("ID1"), Session("ID2"), Session("User"))
            ta.OK2(Firma, Session("ID1"), Session("ID2"), Session("User"))
        Else
            ta.Ok3(Firma, "AUTOMATICO", Session("ID1"), Session("ID2"), Session("User"))
        End If

        LbError.Text = "Pagos Autorizados"
        Panel1.Visible = False
        LbError.Visible = True
        Mensaje += "Autorizó: " & Session("User") & "<br>"
        Mensaje += "Comentario: " & TextMail.Text & "<br>"
        Asunto = "Solicitud de Pagos Autorizada (" & r.NombreCorto & "): " & Session("ID2")

        If TextMail.Text.Length > 0 Then
            taOBS.Borrar(r.idEmpresa, r.Solicitud, Session("User"))
            taOBS.Insert(r.idEmpresa, r.Solicitud, Session("User"), TextMail.Text)
        End If
        If InStr(Session("MCONTROL_CXP"), Session("User")) Then
            If ListAutorizante.SelectedValue = "DG" Then
                ta.CambiaAutorizante2("#gbello@finagil.com.mx", "C.P. GABRIEL BELLO HERNANDEZ", Session("ID2"), Session("ID1"), Session("ID3"))
            ElseIf ListAutorizante.SelectedValue = "DO" Then
                ta.CambiaAutorizante2("#epineda@finagil.com.mx", "C.P. ELISANDER PINEDA ROJAS", Session("ID2"), Session("ID1"), Session("ID3"))
            End If
        End If
        GeneraArchivo(Archivo, Session("ID1"), Firma2, r.Solicitud, r.Estatus, r.serie, r.contrato, r.idCuentas)

        If r.contrato = True Then
            If InStr(Session("MCONTROL_CXP"), Session("User")) Then
            Else
                MandaCorreoFase("Pagos@finagil.com.mx", "MCONTROL_CXP", Asunto, Mensaje, Archivo)
                MandaCorreoFase("Pagos@finagil.com.mx", "TESORERIA_CXP", Asunto, Mensaje, Archivo)
            End If
        End If

        MandaCorreo("Pagos@finagil.com.mx", r.MailSolicitante, Asunto, Mensaje, Archivo)
        MandaCorreoFase("Pagos@finagil.com.mx", "SISTEMAS", Asunto, Mensaje, Archivo)
        Session("ID1") = 0
        Session("ID2") = 0
        Session("ID3") = Nothing
        Session("ID4") = Nothing
        Response.Redirect("~\5Afdb804-7cXp.aspx", True)
    End Sub

    Protected Sub BotonRechazar_Click(sender As Object, e As EventArgs) Handles BotonRechazar.Click
        Dim ta As New ProDSTableAdapters.Vw_CXP_AutorizacionesTableAdapter
        Dim t As New ProDS.Vw_CXP_AutorizacionesDataTable
        Dim r As ProDS.Vw_CXP_AutorizacionesRow
        Dim Firma As String = Encriptar(Date.Now.ToString("yyyyMMddhhmm") & Session("User") & Session("ID1") & "-" & Session("ID2"))
        Dim Mensaje As String = ""
        Dim Asunto As String = ""
        Dim Archivo As String
        ta.FillByID(t, Session("User"), Session("ID2"), Session("ID1"), Session("ID3"))
        r = t.Rows(0)
        Mensaje = "Solicitud: " & r.Solicitud & "<br>"
        Mensaje += "Emrpesa: " & r.NombreCorto & "<br>"
        Mensaje += "Estatus: " & r.Estatus & "<br>"
        Mensaje += "Importe: " & CDec(r.Total).ToString("n2") & "<br>"
        Archivo = "CXP\" & CInt(r.idEmpresa).ToString & "-" & CInt(r.Solicitud).ToString & ".pdf"
        Dim Firma2 As String = Encriptar(r.FechaSol.ToString("yyyyMMddhhmm") & r.MailSolicitante & Session("ID1") & "-" & Session("ID2"))

        Firma = "RECHAZADO"
        ta.Ok1(Firma, Session("ID1"), Session("ID2"), Session("User"))
        ta.OK2(Firma, Session("ID1"), Session("ID2"), Session("User"))
        LbError.Text = "Pagos Rechazados"
        Panel1.Visible = False
        LbError.Visible = True
        Mensaje += "Rechazó: " & Session("User") & "<br>"
        Mensaje += "Comentario: " & TextMail.Text & "<br>"
        Asunto = "Solicitud de Pagos Rechazada (" & r.NombreCorto & "): " & Session("ID2")
        If TextMail.Text.Length <= 0 Then
            TextMail.Text = "RECHAZADO"
        Else
            TextMail.Text = "RECHAZADO" & TextMail.Text
        End If
        taOBS.Borrar(r.idEmpresa, r.Solicitud, Session("User"))
        taOBS.Insert(r.idEmpresa, r.Solicitud, Session("User"), TextMail.Text)
        GeneraArchivo(Archivo, Session("ID1"), Firma2, r.Solicitud, r.Estatus, r.serie, r.contrato, r.idCuentas)
        MandaCorreo("Pagos@finagil.com.mx", r.MailSolicitante, Asunto, Mensaje, Archivo)
        MandaCorreoFase("Pagos@finagil.com.mx", "SISTEMAS", Asunto, Mensaje, Archivo)
        Session("ID1") = 0
        Session("ID2") = 0
        Session("ID3") = Nothing
        Session("ID4") = Nothing
        Response.Redirect("~\5Afdb804-7cXp.aspx", True)
    End Sub

    Protected Sub BotonCorreo_Click(sender As Object, e As EventArgs) Handles BotonCorreo.Click
        Dim ta As New ProDSTableAdapters.Vw_CXP_AutorizacionesTableAdapter
        Dim t As New ProDS.Vw_CXP_AutorizacionesDataTable
        Dim r As ProDS.Vw_CXP_AutorizacionesRow
        Dim Firma As String = Encriptar(Date.Now.ToString("yyyyMMddhhmm") & Session("User") & Session("ID1") & "-" & Session("ID2"))
        Dim Mensaje As String = ""
        Dim Asunto As String = ""
        Dim Archivo As String
        ta.FillByID(t, Session("User"), Session("ID2"), Session("ID1"), Session("ID3"))
        r = t.Rows(0)
        Mensaje = "Solicitud: " & r.Solicitud & "<br>"
        Mensaje += "Emrpesa: " & r.NombreCorto & "<br>"
        Mensaje += "Estatus: " & r.Estatus & "<br>"
        Mensaje += "Importe: " & CDec(r.Total).ToString("n2") & "<br>"
        Archivo = "CXP\" & CInt(r.idEmpresa).ToString & "-" & CInt(r.Solicitud).ToString & ".pdf"
        Dim Firma2 As String = Encriptar(r.FechaSol.ToString("yyyyMMddhhmm") & r.MailSolicitante & Session("ID1") & "-" & Session("ID2"))

        If TextMail.Text.Length <= 0 Then
            Exit Sub
        End If
        LbError.Text = "Correo Enviado"
        Panel1.Visible = False
        LbError.Visible = True
        Mensaje += "Comentario: " & TextMail.Text & "<br>"
        Asunto = "Comentarios de Pagos y Facturas (" & r.NombreCorto & "): " & Session("ID2")
        TextMail.Text = ""

        MandaCorreo("Pagos@finagil.com.mx", r.MailSolicitante, Asunto, Mensaje, Archivo)
        MandaCorreoFase("Pagos@finagil.com.mx", "SISTEMAS", Asunto, Mensaje, Archivo)
        Session("ID1") = 0
        Session("ID2") = 0
        Session("ID3") = Nothing
        Session("ID4") = Nothing
        Response.Redirect("~\5Afdb804-7cXp.aspx", True)
    End Sub

    Protected Sub GridView1_SelectedIndexChanged(sender As Object, e As EventArgs) Handles GridView1.SelectedIndexChanged
        Session("ID1") = GridView1.SelectedDataKey("idEmpresa")
        Session("ID2") = GridView1.SelectedDataKey("Solicitud")
        Session("ID3") = GridView1.SelectedDataKey("Estatus")
        Session("ID4") = GridView1.SelectedIndex
        Response.Redirect("~\5Afdb804-7cXp.aspx", True)
    End Sub

End Class