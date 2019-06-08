//####################################################################
// APPLICATION:    Amazing
// AUTHOR:         Yeison Andres Rua
// DESCRIPTION:    Entidad seguridad.
// DATE:           25/07/2018
//####################################################################
namespace Amazing.DT.Security
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Runtime.Serialization;
    public class JsonAuthentication
    {
        public List<DTAuthentication> Authentication { get; set; }
    }
    public class DTAuthentication
    {
        [DataMember]
        [StringLength(200)]
        public string Module { get; set; }

        [DataMember]
        [StringLength(100)]
        public string User { get; set; }

        [DataMember]
        [StringLength(100)]
        public string Password { get; set; }

        public bool State { get; set; }
    }
}
