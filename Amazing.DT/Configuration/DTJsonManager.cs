//####################################################################
// APPLICATION:    Amazing
// AUTHOR:         Yeison Andres Rua
// DESCRIPTION:    Clase para leer los archivos Json.
// DATE:           25/07/2018
//####################################################################
namespace Amazing.DT.Configuration
{
    using Messages;
    using Newtonsoft.Json;
    using Security;
    using System;
    using System.IO;
    using System.Linq;

    public class DTJsonManager
    {
        #region Constantes
        private const string ARCHIVO_MENSAJES = "Messages.json";
        private const string ARCHIVO_SEGURIDAD = "Security.json";
        private const string ARCHIVO_CONFIGURACION = "Appsettings.json";
        private const string MENSAJE_ARCHIVO = "No existe el archivo: ";
        #endregion Constantes

        #region Metodos

        /// <summary>
        /// Valida si el usuario existe en el archivo json.
        /// </summary>
        /// <param name="objAutenticacion"></param>
        /// <returns></returns>
        public static bool ValidarSeguridad(DTAuthentication objAutenticacion)
        {
            JsonAuthentication ListAutenticacion = new JsonAuthentication();

            bool respuesta = false;

            string rutaArchivo = ObtenerArchivo(ARCHIVO_SEGURIDAD);

            var json = File.ReadAllText(rutaArchivo);
            ListAutenticacion = JsonConvert.DeserializeObject<JsonAuthentication>(json);

            var result = ListAutenticacion.Authentication.Where(s => s.Module == objAutenticacion.Module
                                    && s.User == objAutenticacion.User
                                    && s.Password == objAutenticacion.Password
                                    && s.State == true);
            if (result.Any())
            {
                respuesta = true;
            }
            return respuesta;
        }

        /// <summary>
        /// Obtiene la ruta del Json
        /// </summary>
        /// <returns></returns>
        private static string ObtenerRutaArchivo()
        {
            string rutaArchivoConfiguracion = string.Empty;
            rutaArchivoConfiguracion = $"{ AppDomain.CurrentDomain.RelativeSearchPath}\\Configuration\\";
            return rutaArchivoConfiguracion;
        }

        /// <summary>
        /// Obtiene el archivo y valida si existe.
        /// </summary>
        /// <param name="NOMBRE_ARCHIVO"></param>
        /// <returns></returns>
        private static string ObtenerArchivo(string NOMBRE_ARCHIVO)
        {
            string rutaArchivo = ObtenerRutaArchivo();
            if (!File.Exists($"{rutaArchivo}\\{NOMBRE_ARCHIVO}"))
            {
                string mensaje = string.Format(NOMBRE_ARCHIVO, rutaArchivo);
                throw new FileNotFoundException(mensaje);
            }

            rutaArchivo = $"{rutaArchivo}{NOMBRE_ARCHIVO}";

            return rutaArchivo;
        }

        /// <summary>
        /// Obtiene el mensaje desde el Json.
        /// </summary>
        /// <param name="codigo"></param>
        /// <param name="parametros"></param>
        /// <returns></returns>
        public static DTMessage ObtenerJsonMensajes(int codigo, params string[] parametros)
        {
            JsonMessages ListMensaje = new JsonMessages();
            DTMessage objmensaje = new DTMessage();

            string rutaArchivo = ObtenerArchivo(ARCHIVO_MENSAJES);

            var json = File.ReadAllText(rutaArchivo);
            ListMensaje = JsonConvert.DeserializeObject<JsonMessages>(json);

            objmensaje = ListMensaje.Messages.Where(m => m.Code == codigo).FirstOrDefault();
            if (parametros != null)
            {
                objmensaje.Text = string.Format(objmensaje.Text, parametros);
            }
            return objmensaje;
        }

        /// <summary>
        /// Retorna el texto del mensaje cuando no tiene parametros.
        /// </summary>
        /// <param name="codigo"></param>
        /// <returns></returns>
        public static string ObtenerJsonMensajes(int codigo)
        {
            return ObtenerJsonMensajes(codigo, null).Text;
        }

        /// <summary>
        /// Retorna solo el objeto de mensajes cuando no tiene parametros.
        /// </summary>
        /// <param name="codigo"></param>
        /// <returns></returns>
        public static DTMessage ObtenerObjetoMensaje(int codigo)
        {
            return ObtenerJsonMensajes(codigo, null);
        }

        /// <summary>
        /// Retorna solo el objeto de mensajes cuando se tiene el parametro.
        /// </summary>
        /// <param name="codigo"></param>
        /// <returns></returns>
        public static DTMessage ObtenerObjetoMensaje(int codigo, params string[] parametros)
        {
            return ObtenerJsonMensajes(codigo, parametros);
        }
        #endregion Metodos
    }
}
