//####################################################################
// APPLICATION:    Amazing
// AUTHOR:         Yeison Andres Rua
// DESCRIPTION:    Estructura de respuesta del servicio.
// DATE:           26/07/2018
//####################################################################
namespace Amazing.DT.General
{
    using Messages;
    public class DTServiceResponse<T> where T : class
    {
        public string Code { get; set; }
        public string Message { get; set; }
        public TypeMessage MessageType { get; set; }
        public bool Response { get; set; }
        public T Result { get; set; }
    }
}
