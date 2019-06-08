//####################################################################
// APPLICATION:    Amazing
// AUTHOR:         Yeison Andres Rua
// DESCRIPTION:    Arquitectura inicial(Gestionar Log).
// DATE:           25/07/2018
//####################################################################
namespace Amazing.DT.Support
{
    using System;
    using System.Runtime.Serialization;

    public class DTBusinessException : Exception
    {
        #region Constructores

        /// <summary>
        /// Inicializa una nueva instancia de la clase DTNegocioException.
        /// </summary>
        public DTBusinessException()
        {
        }

        /// <summary>
        /// Inicializa una nueva instancia de la clase DTNegocioException.
        /// </summary>
        /// <param name="mensaje">Mensaje de error que explica la razón de la excepción.</param>
        public DTBusinessException(string mensaje)
            : base(mensaje)
        {
        }

        /// <summary>
        /// Inicializa una nueva instancia de la clase DTNegocioException.
        /// </summary>
        /// <param name="exception">Objeto Exception.</param>
        public DTBusinessException(Exception exception)
            : base(exception.Message)
        {
        }

        /// <summary>
        /// Inicializa una nueva instancia de la clase DTNegocioException.
        /// </summary>
        /// <param name="mensaje">Mensaje de error que explica la razón de la excepción.</param>
        /// <param name="innerExcepcion">Excepción que causa la excepción actual, o
        /// referencia null si no es especificada la innerExcepcion.</param>
        public DTBusinessException(string mensaje, Exception innerExcepcion)
            : base(mensaje, innerExcepcion)
        {
        }

        /// <summary>
        /// Inicializa una nueva instancia de la clase DTNegocioException.
        /// </summary>
        /// <param name="info">Información de la serialización.</param>
        /// <param name="context">Contexto de la serializáción.</param>
        protected DTBusinessException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }

        #endregion
    }
}
