<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="Confidencialidad.aspx.vb" Inherits="WEBTasas.Confidencialidad" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
</head>
<body >
    <div style="text-align:center;">
		  <div style="width:75%; margin: 0 auto; text-align:left;">
 <div align="right" >   <img src="IMG/logo-sin-fondo_web.png" width="200" height="100"></div>
    

  <H2 ><center>CARTA COMPROMISO DE CONFIDENCIALIDAD, RESERVA Y RESGUARDO DE INFORMACIÓN Y DATOS</center></H2>
    <div id="layer1">

<FORM ACTION="Confidencialidad2.aspx" METHOD="GET" id="mi_formulario">

<p> <strong>A QUIEN CORRESPONDA</strong></p> 
<p>P r e s e n t e:</p><br /><br />
    
<p class="auto-style1">El que suscribe el C.<INPUT TYPE="text" size="15" maxlength="80" NAME="nombre" style="width: 232px"> ,  en este acto suscribe la presente carta compromiso mediante la cual, acepto formalmente las condiciones de resguardo, reserva, custodia y protección de la seguridad y confidencialidad de todo tipo de información y documentos propiedad de  <strong>FINAGIL S.A. DE C.V., SOFOM, E.N.R.</strong>, así como las derivadas de las Disposiciones de carácter general a que se refieren los artículos 115 de la Ley de Instituciones de Crédito en relación con el 87-D de la Ley General de Organizaciones y Actividades Auxiliares del Crédito y 95-Bis de este último ordenamiento, aplicables a las sociedades financieras de objeto múltiple, por lo que expresamente entiendo y acepto que tengo estrictamente prohibido:</p>
<ol type="i" start="1"> 

<li style="text-align: justify" > Alertar o dar aviso a Clientes o Usuarios respecto de cualquier referencia que sobre ellos se haga en reportes.</li>
<li > Alertar o dar aviso a sus Clientes, Usuarios o a algún tercero respecto de cualquiera de los requerimientos de información o documentación previstos en la fracción IX de la 39ª de las presentes Disposiciones; </li>
<li > Alertar o dar aviso a sus Clientes o a algún tercero sobre la existencia o presentación de órdenes de aseguramiento a que se refiere la fracción IX de la 39ª de las presentes Disposiciones, antes de que sean ejecutadas, y </li>
<li > Alertar o dar aviso a sus Clientes, Usuarios o algún tercero sobre el contenido de la Lista de Personas Bloqueadas. Lo anterior, sin perjuicio de lo establecido en el último párrafo de la 63ª de las presentes Disposiciones.</li>

</ol>

    <p style="text-align: justify">Asimismo, acepto haber leído y comprendido las condiciones de resguardo, reserva, custodia y protección de la seguridad y confidencialidad de todo tipo de información y documentación de que tenga conocimiento, 
      con motivo de mi actividad como trabajador de la prestadora de servicios denominada  <INPUT TYPE="text" NAME="empresa" style="width: 279px"> ,
         la cual desempeño en favor de <strong>FINAGIL S.A. DE C.V., SOFOM, E.N.R.</strong>, y me comprometo a cumplirlas en su totalidad, sin menoscabo de las demás obligaciones y prohibiciones establecidas en la normatividad  fiscal y de prevención de la lavado de dinero aplicable, en el entendido de que el incumplimiento a cualquiera de éstas será causa de la aplicación de las sanciones correspondientes, siendo el encargado de sancionarlas el Comité de Comunicación y Control de la empresa.</p>



    <%


        Dim ip
        Dim Host
        Dim user

        ip = request.ServerVariables("REMOTE_ADDR")
        Host = request.ServerVariables("REMOTE_HOST")
        User = request.ServerVariables("REMOTE_USER")
%>
    <div align="CENTER"> 
        <input type="hidden" id="equipo" name="equipo" value="<%Response.Write(Request.ServerVariables("REMOTE_HOST")) %>"><BR>
  
    <input type="checkbox" id="cbox2" value="second_checkbox"> <label for="cbox2" style="text-align: center">ACEPTO LOS TERMINOS Y CONDICIONES</label>
   </div>

   
     <div align="CENTER"> 
<INPUT TYPE="submit" VALUE="ENVIAR" style="text-align: center">
   </div>

</FORM>
        </div>
              </div>
        </div>

    <script>
 document.addEventListener("DOMContentLoaded", function(event) {
  document.getElementById('mi_formulario').addEventListener('submit', 
manejadorValidacion)
});

 function manejadorValidacion(e) {
     e.preventDefault();
  
     if (this.querySelector('[name=nombre]').value == '') {
         alert("El campo Nombre NO puede ir vacio");
         mi_formulario.nombre.focus();
         return;
     }
     if (this.querySelector('[name=empresa]').value == '') {
         alert("El campo Empresa NO puede ir vacio");
         mi_formulario.nombre.focus();
         return;
     }

     var isChecked = document.getElementById('cbox2').checked;
     if (isChecked) {
            }
     else {
         alert("Para continuar aceptar los términos y condiciones");
          return;
     }

     this.submit();
     
 }


</script>

    </body>




</html>
