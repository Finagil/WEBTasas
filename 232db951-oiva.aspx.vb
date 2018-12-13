Partial Public Class JefeSucursalForm
    Inherits System.Web.UI.Page
    Dim Tipo As String
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If IsNothing(Request("User")) Then
            Panel1.Visible = False
            LbError.Visible = True
        Else

            Dim ta As New ProDSTableAdapters.AvioVoboTableAdapter
            Dim Correos As New SeguridadDSTableAdapters.UsuariosFinagilTableAdapter
            Dim t As New ProDS.AvioVoboDataTable
            Dim R As ProDS.AvioVoboRow
            ta.Fill(t, Request("User"))
            If t.Rows.Count > 0 Then
                R = t.Rows(0)
                If Request("ID") = 0 Then
                    Panel1.Visible = True
                    LbError.Visible = False
                    BotonEnviar1.Visible = False
                    BotonEnviar2.Visible = False
                    'CheckAviso.Visible = False
                    BotonEnviar1.Text = "Dar Vobo"
                    BotonEnviar1.TextoEnviando = "Dar Vobo"
                Else
                    Panel1.Visible = True
                    LbError.Visible = False
                    BotonEnviar1.Text = "Dar Vobo"
                    BotonEnviar1.TextoEnviando = "Dar Vobo"
                    BotonEnviar1.Visible = True
                    'CheckAviso.Visible = True
                    If Request("User").ToLower = "martin.beltran" Then
                        BotonEnviar2.Visible = False
                    Else
                        BotonEnviar2.Visible = True
                    End If
                    'Dim noEfec As Integer = ta.NoEfectivo(R.Anexo, R.CicloPagare)
                    'Dim taUsuarios As New SeguridadDSTableAdapters.UsuariosFinagilTableAdapter
                    'CheckAviso.Text = "Bajo protesta de decir verdad, " & taUsuarios.ScalarNombre(Request("User")) & " manifiesto que en esta fecha he llevado a cabo la " &
                    '"revisi�n y validaci�n del contenido de la C�dula de Verificaci�n T�cnica (CVT) y/o del Reporte de Supervisi�n** la (el) cual es 100% favorable, de fecha no mayor a diez d�as de esta minitracion," &
                    '" misma(o) que se efect�o sobre la superficie a sembrar del Acreditado " & R.Descr & ", y hago constar que en �sta sucursal de " & R.Nombre_Sucursal & " contamos f�sicamente" &
                    '" con la C�dula de Verificaci�n T�cnica y/o el Reporte de Supervisi�n de referencia en original, la(el) cual se encuentra digitalizada(o) en la carpeta compartida asignada y de la que se desprende que puede ser autorizada la" &
                    '" disposici�n No. " & noEfec & " de la l�nea de cr�dito del contrato n�mero " & R.AnexoCon & " que se solicita por este medio." &
                    '"<br><br>** El �rea  t�cnica de esta  sucursal, utiliza para los  av�os de granos y oleaginosas  la C�dula de Verificaci�n T�cnica (CVT) y para los casos de hortalizas y frutas se utilizan indistintamente el formato de CVT  y/o el formato de Reporte de Supervisi�n."

                End If
            Else
                Panel1.Visible = False
                LbError.Visible = True
            End If
        End If
    End Sub

    Protected Sub BotonEnviar1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BotonEnviar1.Click
        Dim ta As New ProDSTableAdapters.AvioVoboTableAdapter
        ta.DarVobo("CreditoX", "PldX", Request("Anexo"), Request("ID"))
        Response.Redirect("~\232db951-oiva.aspx?User=" & Request("User") & "&Anexo=0&ID=0")
    End Sub

    Protected Sub BotonEnviar2_Click(sender As Object, e As EventArgs) Handles BotonEnviar2.Click
        Dim ta As New ProDSTableAdapters.AvioVoboTableAdapter
        ta.PasaEstrategias(Request("Anexo"), Request("ID"))
        Response.Redirect("~\232db951-oiva.aspx?User=" & Request("User") & "&Anexo=0&ID=0")
    End Sub

    'Protected Sub CheckAviso_CheckedChanged(sender As Object, e As EventArgs) Handles CheckAviso.CheckedChanged
    '    BotonEnviar1.Enabled = CheckAviso.Checked
    'End Sub
End Class