//####################################################################
// APPLICATION:    Amazing
// AUTHOR:         Yeison Andres Rua
// DESCRIPTION:    Entidad para mensajes.
// DATE:           25/07/2018
//####################################################################
namespace Amazing.DT.Messages
{
    using System.Collections.Generic;

    /// <summary>
    /// Enumerador tipo mensajes
    /// </summary>
    public enum TypeMessage
    {
        Error = 0,
        Warning = 1,
        Information = 2
    }

    public class JsonMessages
    {
        public List<DTMessage> Messages { get; set; }
    }

    public class DTMessage
    {
        public int Code { get; set; }       

        /// <summary>
        /// Obtiene o establece el tipo de mensaje
        /// </summary>
        public TypeMessage Type { get; set; }

        public string Text { get; set; }

        public string Value { get; set; }
    }
}
