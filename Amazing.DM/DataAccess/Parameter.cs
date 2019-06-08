//####################################################################
// DEPENDENCIES:    Amazing
// AUTHOR:          Yeison Andres Rua Del Rio
// DESCRIPTION:     Arquitectura inicial aplicación
// DATE:            10/01/2018
//####################################################################
namespace Amazing.DM.DataAccess
{
    using System.Data;
    internal class Parameter
    {
        #region Constructor
        /// <summary>
        /// Constructor por defecto
        /// </summary>
        public Parameter()
        {

        }

        /// <summary>
        /// Intancia una clase parametro cobn nombre y valor
        /// </summary>
        /// <param name="nom">Nombre</param>
        /// <param name="val">Valor</param>
        public Parameter(string nom, object val)
        {
            Nombre = nom;
            Valor = val;
        }

        /// <summary>
        /// Intancia una clase parametro cobn nombre, valor, dirección y tamaño
        /// </summary>
        /// <param name="nom">Nombre</param>
        /// <param name="val">Valor</param>
        /// <param name="dir">Dirección</param>
        /// <param name="tam">Tamaño</param>
        public Parameter(string nom, object val, ParameterDirection dir, int tam) : this(nom, val)
        {
            Direccion = dir;
            Tamano = tam;

        }
        #endregion

        #region Atributos

        /// <summary>
        /// Nombre del parametro
        /// </summary>
        public string Nombre { get; set; }

        /// <summary>
        /// Valor del parametro
        /// </summary>
        public object Valor { get; set; }

        ParameterDirection direccion = ParameterDirection.Input;
        /// <summary>
        /// Dirección del parametro
        /// </summary>
        public ParameterDirection Direccion
        {
            get { return direccion; }
            set { direccion = value; }
        }

        /// <summary>
        /// Tamaño del parametro
        /// </summary>
        public int Tamano { get; set; }
        #endregion
    }
}
