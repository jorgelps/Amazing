//####################################################################
// APPLICATION:    Amazing
// AUTHOR:         Yeison Andres Rua
// DESCRIPTION:    Valida la seguridad del servicio.
// DATE:           26/07/2018
//####################################################################
namespace Amazing.BM.Security
{
    using DT.Configuration;
    using DT.Security;
    public class BMAuthentication
    {
        /// <summary>
        /// Valida si el usuario existe y si está activo.
        /// </summary>
        /// <param name="Autenticacion"></param>
        /// <returns></returns>
        public bool ValidarSeguridad(DTAuthentication Autenticacion)
        {
            return DTJsonManager.ValidarSeguridad(Autenticacion);
        }
    }
}
