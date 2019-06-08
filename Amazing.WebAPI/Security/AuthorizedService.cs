//####################################################################
// APPLICATION:    Amazing
// AUTHOR:         Yeison Andres Rua
// DESCRIPTION:    Gestiona la autorización personalizada concediendo 
//                 o denegando el acceso a los servicios.
// DATE:           26/07/2018
//####################################################################
namespace Amazing.WebAPI
{
    using System;
    using System.Linq;
    using System.Security.Principal;
    using System.Web;
    using Amazing.DT.General;
    using System.Threading;
    using Newtonsoft.Json;
    using Amazing.BM.Security;
    using Amazing.DT.Security;
    using System.Configuration;
    using System.Text;
    using Amazing.DT.Configuration;
    using Amazing.DT.Messages;
   
    public class AuthorizedService : IHttpModule
    {
        public void Init(HttpApplication context)
        {
            context.AuthenticateRequest += OnAuthenticateRequest;
            context.EndRequest += OnApplicationEndRequest;
        }

        /// <summary>
        /// Evento para gestionar la autorización del servicio.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnAuthenticateRequest(object sender, EventArgs e)
        {
            //Indica si la autorización del servicio esta activa
            bool activo = Convert.ToBoolean(ConfigurationManager.AppSettings["AutorizacionActiva"]);
            bool autenticado = false;

            if (activo)
            {
                var application = (HttpApplication)sender;
                var request = new HttpRequestWrapper(application.Request);

                string authHeader = request.Headers["Authorization"];

                if (!string.IsNullOrEmpty(authHeader) && (authHeader.Length > 6))
                {
                    if (IsBase64Encoded(authHeader))
                    {
                        string[] svcCredentials = ASCIIEncoding.ASCII.GetString(Convert.FromBase64String(authHeader)).Split(':');
                        if (svcCredentials != null)
                        {
                            if (svcCredentials.Count() == 3)
                            {
                                DTAuthentication usuarioAut = new DTAuthentication();
                                usuarioAut.Module = svcCredentials[0];
                                usuarioAut.User = svcCredentials[1];
                                usuarioAut.Password = svcCredentials[2];

                                if (new BMAuthentication().ValidarSeguridad(usuarioAut))
                                {
                                    autenticado = true;
                                }
                            }
                        }
                    }
                }
            }
            else
            {
                autenticado = true;
            }

            if (autenticado)
            {
                var principal = new GenericPrincipal(new GenericIdentity("Amazing"), null);

                Thread.CurrentPrincipal = principal;

                if (HttpContext.Current != null)
                {
                    HttpContext.Current.User = principal;
                }
            }
        }

        /// <summary>
        /// Valida la respuesta de la autorización del servicio
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void OnApplicationEndRequest(object sender, EventArgs e)
        {
            var response = HttpContext.Current.Response;
            if (response.StatusCode != 200)
            {
                DTServiceResponse<string> Respuesta = new DTServiceResponse<string>();
                DTMessage mensajeDt = new DTMessage();
                mensajeDt = DTJsonManager.ObtenerObjetoMensaje(DTMessageCode.MESSAGE000);
                Respuesta.Code = mensajeDt.Value;
                Respuesta.Message = mensajeDt.Text;
                Respuesta.MessageType = mensajeDt.Type;
                Respuesta.Response = false;
                response.ClearContent();
                response.Write(JsonConvert.SerializeObject(Respuesta));
            }
        }

        /// <summary>
        /// Valida si una cadena de texto esta en Base64.
        /// </summary>
        /// <param name="Cadena"></param>
        /// <returns></returns>
        public bool IsBase64Encoded(string Cadena)
        {
            try
            {
                byte[] data = Convert.FromBase64String(Cadena);
                return (Cadena.Replace(" ", "").Length % 4 == 0);
            }
            catch
            {
                return false;
            }
        }

        public void Dispose()
        {
        }
    }
}