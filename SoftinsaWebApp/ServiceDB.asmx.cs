using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Services;

namespace SoftinsaWebApp
{
    /// <summary>
    /// Summary description for ServiceDB
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class ServiceDB : System.Web.Services.WebService

    {
        const string COL_NAME = "EmpName";
        const string SP_LOGS_NAME = "GetAllLogsMade";



        public ServiceDB()
        {

        }

        [WebMethod]
        public string getEmpName()
        {
            string result = "";
            try
            {
                SqlConnection connexion = new SqlConnection();
                SqlCommand cmd = new SqlCommand("stpGetAllCollaborators", connexion);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                connexion.ConnectionString = ConfigurationManager.ConnectionStrings["ConnectionServiceDB"].ToString();
                connexion.Open();
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    result = result + dr[COL_NAME].ToString() + Environment.NewLine;
                }
                dr.Close();
                connexion.Close();
            }
            catch (Exception ex)
            {
                return "" + ex;
            }
            return "Collaborators: " + Environment.NewLine + result;
        }

        [WebMethod]
        public string GetDate(DateTime dataInicio, DateTime dataFim)
        {
            string result = "";
            try
            {
                //Tabela de db atraves dos parametros
                SqlParameter param = new SqlParameter();
                param.ParameterName = "@StartData";
                param.SqlDbType = System.Data.SqlDbType.DateTime;
                param.Value = dataInicio;

                SqlParameter param2 = new SqlParameter("@EndData", dataFim);

                SqlConnection connexion = new SqlConnection();
                SqlCommand cmd = new SqlCommand("GetLogsByData", connexion);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.Add(param);
                cmd.Parameters.Add(param2);
             
                connexion.ConnectionString = ConfigurationManager.ConnectionStrings["ConnectionServiceDB"].ToString();
                connexion.Open();
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    result = result + dr[0].ToString() + Environment.NewLine;
                }
                dr.Close();
                connexion.Close();
            }
            catch (Exception ex)
            {
                return "" + ex;
            }
            return "Data: " + Environment.NewLine + result;
        }

        [WebMethod]
        public string GetLogsMade(DateTime dataInicio, DateTime dataFim)
        {
            string result = "";
            try
            {
                //Conexao da db
                SqlConnection connexion = new SqlConnection();
                SqlCommand cmd = new SqlCommand(SP_LOGS_NAME, connexion);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                connexion.ConnectionString =  ConfigurationManager.ConnectionStrings["ConnectionServiceDB"].ToString();
                connexion.Open();
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    //Colunas da db
                    result = result + dr["Date"].ToString() + Environment.NewLine;
                    result = result + dr["Hours"].ToString() + Environment.NewLine;
                    result = result + dr["TypeOfLog"].ToString() + Environment.NewLine;
                    result = result + dr["Observations"].ToString() + Environment.NewLine;
                }
                dr.Close();
                connexion.Close();
            }
            catch (Exception ex)
            {
                return "" + ex;
            }
            return "LogsMade: " + Environment.NewLine + result;
        }

        

        //TEstes
        [WebMethod]
        public string GetQualquerCoisa()
        {
            string result = "";
            SqlDataReader dr = ExecutarMetodoBD("NomeSP",null,null);
            if (dr == null)
                return "Ocorreu um erro.";
            while (dr.Read())
            {
                result = result + dr[0].ToString() + Environment.NewLine;
            }
            dr.Close();
            return result;
        }

        //Fazer o refactor do codigo
        private SqlDataReader ExecutarMetodoBD(string spName, Nullable<DateTime> dataInicio, DateTime? dataFim)
        {
            try
            {
                SqlConnection connexion = new SqlConnection();
                SqlCommand cmd = new SqlCommand(spName, connexion);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                connexion.ConnectionString = ConfigurationManager.ConnectionStrings["ConnectionServiceDB"].ToString();

                //Verificar se tem parame
                if (dataInicio.HasValue)
                {
                    SqlParameter param = new SqlParameter();
                    param.ParameterName = "@StartData";
                    param.SqlDbType = System.Data.SqlDbType.DateTime;
                    param.Value = dataInicio.Value;
                    SqlParameter param2 = new SqlParameter("@EndData", dataFim.Value);  
                    cmd.Parameters.Add(param);
                    cmd.Parameters.Add(param2);
                }
                    
                connexion.Open();
                SqlDataReader dr = cmd.ExecuteReader();
                return dr;
            }
            catch (Exception)
            {

                return null;
            }
        }
        
    }
}
