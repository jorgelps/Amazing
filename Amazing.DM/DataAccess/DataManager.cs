//####################################################################
// DEPENDENCIES:    Amazing
// AUTHOR:          Yeison Andres Rua Del Rio
// DESCRIPTION:     Arquitectura inicial aplicación
// DATE:            10/01/2018
//####################################################################
namespace Amazing.DM.DataAccess
{
    using Microsoft.Practices.EnterpriseLibrary.Data;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Data;
    using System.Data.Common;
    using System.Data.SqlClient;
    using System.Data.SqlTypes;
    using System.IO;
    using System.Linq;

    #region Enumerador
    /// <summary>
    /// Configuración del nombre de la cadena de conexión.
    /// </summary>
    public enum nameString
    {
        SQL,
        MySql
    }
    #endregion

    internal class DataManager
    {
        #region Variables

        string ConnectionString;

        #endregion Variables

        #region Constructores

        /// <summary>
        /// Asigna el nombre de la cadena de conexión cuando no se recibe como parametro.
        /// </summary>
        public DataManager()
        {
            ConnectionString = "strConnectionMSSQL";
            baseDatos = new DatabaseProviderFactory().Create(ObtenerNombreCadenaBaseDatos(ConnectionString));
        }

        /// <summary>
        /// Asigna el nombre de la cadena de conexión cuando se recibe como parametro.
        /// </summary>
        public DataManager(nameString Cadena)
        {
            switch (Cadena)
            {
                case nameString.MySql:
                    ConnectionString = "strConnectionMySql";
                    break;                
                default:
                    ConnectionString = "strConnectionMSSQL";
                    break;
            }
            baseDatos = new DatabaseProviderFactory().Create(ObtenerNombreCadenaBaseDatos(ConnectionString));
        }

        #endregion Constructores   

        #region Atributos

        ///// <summary>
        ///// Objeto para conectar a la base de datos.
        ///// </summary>       
        private Database baseDatos;   

        ///// <summary>
        ///// Objeto para almacenar una transacción de base de datos.
        ///// </summary>
        private IDbTransaction transaccion;

        ///// <summary>
        /// Objeto para dar contexto a una transacción.
        /// </summary>
        private DbConnection connection;

        #endregion Atributos

        #region Métodos
        /// <summary>
        /// Ejecuta un procedimiento almacenado.
        /// </summary>
        /// <param name="procedimiento">Nombre del procedimiento.</param>
        /// <returns>Retorna la cantidad de registros afectados.</returns>
        /// <author>"Yeison Andres Rua Del Rio"</author>
        public int EjecutarNonQuery(string procedimiento, List<Parameter> listParametros)
        {
            DbCommand comando = PrepararComando(procedimiento) as DbCommand;
            EstablecerParametros(listParametros, comando);
            int result =  baseDatos.ExecuteNonQuery(comando);
            ObtenerParametros(listParametros, comando);
            return result;
        }

        /// <summary>
        /// Ejecuta un procedimiento almacenado.
        /// </summary>
        /// <param name="procedimiento">Nombre del procedimiento.</param>
        /// <returns>Retorna un valor de tipo object.</returns>
        /// <author>"Yeison Andres Rua Del Rio"</author>
        public object EjecutarScalar(string procedimiento, List<Parameter> listParametros)
        {
            object respuesta = null;
            DbCommand comando = PrepararComando(procedimiento) as DbCommand;
            EstablecerParametros(listParametros, comando);
            respuesta = baseDatos.ExecuteScalar(comando);
            ObtenerParametros(listParametros, comando);
            return respuesta;
        }

        /// <summary>
        /// Ejecuta un procedimiento almacenado.
        /// </summary>        
        /// <param name="procedimiento">Nombre del procedimiento.</param>
        /// <returns>Retorna un Reader con los datos recuperados.</returns>
        /// <author>"Yeison Andres Rua Del Rio"</author>
        public IDataReader EjecutarReader(string procedimiento, List<Parameter> listParametros)
        {
            DbCommand comando = PrepararComando(procedimiento) as DbCommand;
            EstablecerParametros(listParametros, comando);

            IDataReader reader;
            bool transaccionIniciada = (transaccion != null) && (connection != null) && (connection.State == ConnectionState.Open);
            if (transaccionIniciada)
            {
                reader = baseDatos.ExecuteReader(comando, (DbTransaction)transaccion);
            }
            else
            {
                reader = baseDatos.ExecuteReader(comando);
            }

            ObtenerParametros(listParametros, comando);

            return reader;
        }

        /// <summary>
        /// Ejecuta un procedimiento almacenado y lo llena en un data set
        /// con parametro.  
        /// </summary>        
        /// <param name="procedimiento">Nombre del procedimiento.</param>   
        /// <returns>Retorna un DataSet con los datos recuperados.</returns>
        /// <author>"Yeison Andres Rua Del Rio"</author>
        public DataSet EjecutarDataSet(string procedimiento, List<Parameter> listParametros)
        {
            DbCommand comando = PrepararComando(procedimiento) as DbCommand;
            EstablecerParametros(listParametros, comando);
            DataSet datos = baseDatos.ExecuteDataSet(comando);
            ObtenerParametros(listParametros, comando);
            return datos;
        }

        /// <summary>
        /// Ejecuta un procedimiento almacenado y lo llena en un data set
        /// sin parametro.  
        /// </summary>        
        /// <param name="procedimiento">Nombre del procedimiento.</param>
        /// <returns>Retorna un DataSet con los datos recuperados.</returns>
        /// <author>"Yeison Andres Rua Del Rio"</author>
        public DataSet EjecutarDataSet(string procedimiento)
        {
            DbCommand comando = PrepararComando(procedimiento) as DbCommand;
            DataSet datos = baseDatos.ExecuteDataSet(comando);
            return datos;
        }

        /// <summary>
        /// Ejecuta un procedimiento almacenado con parametros y lo llena en un datatable. 
        /// </summary>
        /// <param name="procedimiento">procedimento que se vai executar</param>
        /// <param name="_DTParametro">parametros sem tipar</param>
        /// <returns>Retorna un datatable</returns>
        /// <author>"Yeison Andres Rua Del Rio"</author>
        public DataTable EjecutarDataSetWithDataTable(string procedimiento, List<Parameter> listParametros)
        {
            SqlCommand comando = PrepararComando(procedimiento) as SqlCommand;
            EstablecerParametros(listParametros, comando);
            DataTable datos = baseDatos.ExecuteDataSet(comando).Tables[0];
            ObtenerParametros(listParametros, comando);
            return datos;
        }

        /// <summary>
        /// Ejecuta un procedimiento almacenado sin parametros y lo llena en un datatable. 
        /// </summary>
        /// <param name="procedimiento">procedimento que se vai executar</param>        
        /// <returns></returns>
        /// <author>"Yeison Andres Rua Del Rio"</author>
        public DataTable EjecutarDataSetWithDataTable(string procedimiento)
        {
            SqlCommand comando = PrepararComando(procedimiento) as SqlCommand;            
            DataTable datos = baseDatos.ExecuteDataSet(comando).Tables[0];            
            return datos;
        }


        /// <summary>
        /// Inicia una transacción de base de datos.
        /// </summary>
        /// <returns>Retornar una transacción de base de datos.</returns>
        public IDbTransaction IniciarTransaccion()
        {
            connection = baseDatos.CreateConnection();
            connection.Open();
            transaccion = connection.BeginTransaction();
            return transaccion;
        }

        /// <summary>
        /// Finaliza una transacción de base de datos.
        /// </summary>
        public void FinalizarTransaccion()
        {
            if (transaccion.Connection != null)
            {
                transaccion.Commit();
            }

            if ((connection != null) && (connection.State == ConnectionState.Open))
            {
                connection.Close();
            }
        }

        /// <summary>
        /// Permite almacenar un archivo en la base de datos.
        /// </summary>
        /// <param name="path">Ruta física del archivo en el servidor.</param>
        /// <param name="contenidoArchivo">Binarios del archivo.</param>
        /// <param name="longitudArchivo">Tamaño del archivo.</param>
        /// <author>"Yeison Andres Rua Del Rio"</author>
        public void EjecutarWriteFileStream(string path, IList<byte> contenidoArchivo, int longitudArchivo)
        {
            byte[] objContext = ObtenerContextoFileStream();
            using (SqlFileStream objSqlFileStream = new SqlFileStream(path, objContext, FileAccess.Write))
            {
                byte[] objContenido = contenidoArchivo.ToArray();
                objSqlFileStream.Write(objContenido, 0, longitudArchivo);
            }
        }

        /// <summary>
        /// Permite obtener un archivo en la base de datos.
        /// </summary>
        /// <param name="path">Ruta física del archivo en el servidor.</param>
        /// <returns>Retorna el contenido de un archivo.</returns>
        /// <author>"Yeison Andres Rua Del Rio"</author>
        public byte[] EjecutarReadFileStream(string path)
        {
            byte[] buffer = null;
            byte[] objContext = ObtenerContextoFileStream();
            using (SqlFileStream objSqlFileStream = new SqlFileStream(path, objContext, FileAccess.Read))
            {
                buffer = new byte[(int)objSqlFileStream.Length];
                objSqlFileStream.Read(buffer, 0, buffer.Length);
            }

            return buffer;
        }

        /// <summary>
        /// Obtiene la cadena de conexión a la base de datos.
        /// </summary>
        /// <returns>Retorna el connection string de la base de datos.</returns>
        /// <author>"Yeison Andres Rua Del Rio"</author>
        private static string ObtenerNombreCadenaBaseDatos(string nomeCadenaBancoDoDados)
        {
            return ConfigurationManager.ConnectionStrings[nomeCadenaBancoDoDados].Name;
        }

        /// <summary>
        /// Permite obtener el contexto de una transacción de FileStream.
        /// </summary>
        /// <returns>Retorna el contexto de la transacción para FileStream</returns>
        private byte[] ObtenerContextoFileStream()
        {
            using (SqlCommand objSqlCmdFS = new SqlCommand("SELECT GET_FILESTREAM_TRANSProjetoION_CONTEXT()", (SqlConnection)connection, (SqlTransaction)transaccion))
            {
                byte[] objContext = (byte[])objSqlCmdFS.ExecuteScalar();
                return objContext;
            }
        }

        /// <summary>
        /// Permite crear un objeto IDbCommand con el procedimiento que se suminitra.
        /// </summary>
        /// <returns>Retorna un comando.</returns>
        /// <param name="procedimiento">Nombre del procedimiento.</param>
        /// <returns></returns>
        /// <author>"Yeison Andres Rua Del Rio"</author>
        private IDbCommand PrepararComando(string procedimiento)
        {            
            DbCommand comando = baseDatos.GetStoredProcCommand(procedimiento);
            comando.CommandTimeout = int.Parse(ConfigurationManager.AppSettings["TimeoutBD"].ToString());
            return comando;
        }

        /// <summary>
        /// Estable los parametros de un comando de Base de datos
        /// </summary>
        /// <param name="listParametros"></param>
        /// <param name="cmd"></param>
        private static void EstablecerParametros(List<Parameter> listParametros, DbCommand cmd)
        {
            if (listParametros != null)
            {
                DbParameter param;
                for (int index = 0; index < listParametros.Count; index++)
                {
                    param = cmd.CreateParameter();
                    Parameter itemParam = listParametros[index];
                    param.ParameterName = itemParam.Nombre;
                    param.Value = itemParam.Valor;
                    if (itemParam.Direccion == ParameterDirection.Output || itemParam.Direccion == ParameterDirection.InputOutput)
                    {
                        param.Direction = itemParam.Direccion;
                        param.Size = itemParam.Tamano;
                    }
                    cmd.Parameters.Add(param);
                }
            }
        }

        /// <summary>
        /// Obtiene los parametros de salida de un procedimiento
        /// </summary>
        /// <param name="listParametros"></param>
        /// <param name="cmd"></param>
        private static void ObtenerParametros(List<Parameter> listParametros, DbCommand cmd)
        {
            if (listParametros != null)
            {
                for (int index = 0; index < listParametros.Count; index++)
                {
                    Parameter itemParam = listParametros[index];
                    if (itemParam.Direccion == ParameterDirection.Output || itemParam.Direccion == ParameterDirection.InputOutput)
                    {
                        itemParam.Valor = cmd.Parameters[index].Value;
                    }
                }
            }
        }
        #endregion Métodos
    }
}