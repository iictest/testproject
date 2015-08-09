using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

namespace DAL
{
    public class DataLayer
    {
        public static string ConnectionString()
        {
           // return ConfigurationManager.ConnectionStrings["-------"].ConnectionString;
            return "Data Source=.;Initial Catalog=MyDB;Integrated Security=True";
        }
        public static string LOGConnectionString()
        {
            return ConfigurationManager.ConnectionStrings["TotanPaymentLOG"].ConnectionString;
        }

        private static bool CreateCommand(out string error, out SqlCommand command, string procedureName, params SqlParameter[] parameters)
        {
            bool result = false;
            error = "";
            command = null;

            try
            {
                command = new SqlCommand()
                {
                    Connection = new SqlConnection(ConnectionString()),
                    CommandText = procedureName,
                    CommandType = CommandType.StoredProcedure
                };

                SqlParameter returnValue = new SqlParameter()
                {
                    Direction = ParameterDirection.ReturnValue
                };

                command.Parameters.Add(returnValue);

                command.Parameters.AddRange(parameters);
                result = true;
            }
            catch (Exception ex)
            {
                error = ex.Message;
                result = false;

                if (command.Connection.State == ConnectionState.Open)//New 92/11/07
                    command.Connection.Close();//New 92/11/07
                command.Dispose();//New 92/11/07
            }
            return result;
        }
        private static bool ExecuteNonQuery(out string error, out int result, SqlCommand command)
        {
            result = -1;
            bool myresult = false;

            int rows;
            error = "";
            try
            {
                if (!ExecuteNonQuery(out error, out rows, out result, command))
                    throw new Exception(error);
                myresult = true;
            }
            catch (Exception ex)
            {
                error = ex.Message;
                myresult = false;
            }
            return myresult;
        }
        private static bool ExecuteNonQuery(out string error, out int rows, out int firstResult, SqlCommand command)
        {
            rows = firstResult = -1;

            error = "";
            bool result = false;
            try
            {
                if (command == null)
                    throw new Exception("اطلاعاتی در کامند نیست");

                command.Connection.Open();
                rows = command.ExecuteNonQuery();

                firstResult = (int)command.Parameters[0].Value;
                result = true;
            }
            catch (Exception ex)
            {
                error = ex.Message;
                result = false;
            }
            finally
            {
                if (command.Connection.State == ConnectionState.Open)
                    command.Connection.Close();
                command.Dispose();//New 92/11/07
            }

            return result;

        }
        private static bool ExecuteReader(out string error, out SqlDataReader dataReader, SqlCommand command)
        {
            bool result = false;
            dataReader = null;
            error = "";
            try
            {
                if (command == null)
                    throw new Exception("اطلاعاتی در کامند نیست");

                command.Connection.Open();
                dataReader = command.ExecuteReader(CommandBehavior.CloseConnection);

                result = true;
            }
            catch (Exception ex)
            {
                error = ex.Message;
                result = false;
                if (dataReader != null && !dataReader.IsClosed)
                    dataReader.Close();

                if (command.Connection.State == ConnectionState.Open)
                    command.Connection.Close();
                command.Dispose();//New 92/11/07
            }
            finally
            {
            }

            return result;
        }
        private static bool GetDataSet(out string error, out DataSet dataSet, SqlCommand command)
        {
            bool result = false;
            dataSet = null;
            error = "";
            try
            {
                if (command == null)
                    throw new Exception("اطلاعاتی در کامند نیست");

                command.Connection.Open();
                SqlDataAdapter adapter = new SqlDataAdapter(command);
                adapter.Fill(dataSet);

                result = true;
            }
            catch (Exception ex)
            {
                error = ex.Message;
                result = false;
            }
            finally
            {
                if (command.Connection.State == ConnectionState.Open)
                    command.Connection.Close();
            }

            return result;
        }


        //----------------------------  LOG Methods  ---------------------------\\
        internal static bool Log_Insert(out string error, long request_ID, string message, string methodCalled, string method, string className, long logType_ID, string ip)
        {
            bool result = true;
            error = "";
            SqlConnection con = null;
            SqlCommand comm = null;
            string conString = ConnectionString();
            con = new System.Data.SqlClient.SqlConnection(conString);
            try
            {
                string cmdText = "Log_Insert";
                comm = new System.Data.SqlClient.SqlCommand(cmdText, con);
                comm.CommandType = CommandType.StoredProcedure;
                //----------------------------------
                SqlParameter message1parameter = new SqlParameter("@Message", SqlDbType.NVarChar, 500);
                message1parameter.Value = message;
                comm.Parameters.Add(message1parameter);
                //----------------------------------
                SqlParameter message2parameter = new SqlParameter("@MethodCalled", SqlDbType.NVarChar, 500);
                message2parameter.Value = methodCalled;
                comm.Parameters.Add(message2parameter);
                //----------------------------------
                SqlParameter methodparameter = new SqlParameter("@Method", SqlDbType.NVarChar, 500);
                methodparameter.Value = method;
                comm.Parameters.Add(methodparameter);
                //----------------------------------
                SqlParameter classparameter = new SqlParameter("@Class", SqlDbType.NVarChar, 500);
                classparameter.Value = className;
                comm.Parameters.Add(classparameter);
                //----------------------------------
                SqlParameter logtypeParameter = new SqlParameter("@LogType_ID", SqlDbType.BigInt);
                logtypeParameter.Value = logType_ID;
                comm.Parameters.Add(logtypeParameter);
                //----------------------------------
                SqlParameter ipparameter = new SqlParameter("@IP", SqlDbType.VarChar, 15);
                ipparameter.Value = ip;
                comm.Parameters.Add(ipparameter);
                //----------------------------------
                SqlParameter requestidparameter = new SqlParameter("@Request_ID", SqlDbType.BigInt);
                requestidparameter.Value = request_ID;
                comm.Parameters.Add(requestidparameter);
                //----------------------------------

                con.Open();

                int rowCount = comm.ExecuteNonQuery();
                if (rowCount == 0)
                {
                    error = "??? ??? ?????? ?? ???? Log ??? ???";
                }

                //if (!SendMail(out error, request_ID + " type:" + logType_ID + " methodCalled:" + methodCalled + " method:" + method))
                //{
                //    SorenaUtil.SorenLog.LogStatus("can not send email!  error:" + error);
                //}
            }
            catch (Exception ex)
            {
                result = false;
                LogInFile(out error, ex.Message);
                error = " ??? ?? ??? ??????? ?? ???? ??? " + ex.Message;
            }
            finally
            {
                if (con.State != ConnectionState.Closed)
                    con.Close();
                comm.Dispose();
            }
            return result;

        }
        internal static bool LogInFile(out string error, string message)
        {
            error = "";
            try
            {
                //DateTimeLibrary.Globalization.PersianDateTime persiandatetime = DateTime.Now;
                string directory = ConfigurationManager.AppSettings[""].ToString();

                if (!System.IO.Directory.Exists(directory))
                {
                    System.IO.Directory.CreateDirectory(directory);

                }

                string path = string.Format("{0}\\log{1}.txt", directory, "");


                System.IO.StreamWriter writer = System.IO.File.AppendText(path);
                writer.WriteLine(message + " ");
                writer.Close();
            }
            catch
            {
                return false;
            }
            return true;

        }


        //-------------------------- Recursive Methods  --------------------------\\
        public static bool GetClubInfoByTerminal(out string error, string TerminalID, out ClubInfo Response)
        {
            bool result = false;
            error = "";
            Response = new ClubInfo();
            

            SqlDataReader reader = null;
            SqlCommand command = null;

            try
            {
                if (!CreateCommand(out error, out command, "GetClubByTerminal",
                                new SqlParameter("@TerminalID", TerminalID)))
                {
                    throw new Exception(error);
                }

                if (!ExecuteReader(out error, out reader, command))
                {
                    throw new Exception(error);
                }
                if (reader.Read())
                {
                    Response.ID = Convert.ToInt16(reader["ID"].ToString());
                    Response.Name = reader["Name"].ToString();
                    Response.Description = reader["Description"].ToString();
                    Response.IsActive =Convert.ToBoolean(reader["IsActive"]);
                    Response.CreationDate = reader["CreationDate"].ToString();
                }

                if (Response.Name == "" || Response.Name == null)
                {
                    error = "can not find Club for TerminalID:" + TerminalID;
                    result = false;
                }
                else
                    result = true;
            }
            catch (Exception ex)
            {
                error = ex.Message;
                result = false;
            }
            finally
            {
                if (command.Connection.State == ConnectionState.Open)
                    command.Connection.Close();

                if (reader != null && !reader.IsClosed)
                    reader.Close();

                command.Dispose();

            }

            return result;
        }

        public static bool GetClubInfoByPAN(out string error, string PAN, out ClubInfo Response)
        {
            bool result = false;
            error = "";
            Response = new ClubInfo();


            SqlDataReader reader = null;
            SqlCommand command = null;

            try
            {
                if (!CreateCommand(out error, out command, "GetClubByPAN",
                                new SqlParameter("@PAN", PAN)))
                {
                    throw new Exception(error);
                }

                if (!ExecuteReader(out error, out reader, command))
                {
                    throw new Exception(error);
                }
                if (reader.Read())
                {
                    Response.ID = Convert.ToInt16(reader["ID"].ToString());
                    Response.Name = reader["Name"].ToString();
                    Response.Description = reader["Description"].ToString();
                    Response.IsActive = Convert.ToBoolean(reader["IsActive"]);
                    Response.CreationDate = reader["CreationDate"].ToString();
                }

                if (Response.Name == "" || Response.CreationDate == "")
                {
                    error = "can not find Club for PAN:" + PAN;
                    result = false;
                }
                else
                    result = true;
            }
            catch (Exception ex)
            {
                error = ex.Message;
                result = false;
            }
            finally
            {
                if (command.Connection.State == ConnectionState.Open)
                    command.Connection.Close();

                if (reader != null && !reader.IsClosed)
                    reader.Close();

                command.Dispose();

            }

            return result;
        }

        public static bool GetClubInfoByBIN(out string error, string BIN, out ClubInfo Response)
        {
            bool result = false;
            error = "";
            Response = new ClubInfo();


            SqlDataReader reader = null;
            SqlCommand command = null;

            try
            {
                if (!CreateCommand(out error, out command, "GetClubByBIN",
                                new SqlParameter("@BIN", BIN)))
                {
                    throw new Exception(error);
                }

                if (!ExecuteReader(out error, out reader, command))
                {
                    throw new Exception(error);
                }
                if (reader.Read())
                {
                    Response.ID = Convert.ToInt16(reader["ID"].ToString());
                    Response.Name = reader["Name"].ToString();
                    Response.Description = reader["Description"].ToString();
                    Response.IsActive = Convert.ToBoolean(reader["IsActive"]);
                    Response.CreationDate = reader["CreationDate"].ToString();
                }

                if (Response.Name == "" || Response.CreationDate == "")
                {
                    error = "can not find Club for BIN:" + BIN;
                    result = false;
                }
                else
                    result = true;
            }
            catch (Exception ex)
            {
                error = ex.Message;
                result = false;
            }
            finally
            {
                if (command.Connection.State == ConnectionState.Open)
                    command.Connection.Close();

                if (reader != null && !reader.IsClosed)
                    reader.Close();

                command.Dispose();

            }

            return result;
        }

        public static bool GetClubInfo(out string error, string PAN, string TerminalID, out ClubInfo Response)
        {
            bool result = false;
            error = "";
            Response = new ClubInfo();


            SqlDataReader reader = null;
            SqlCommand command = null;

            try
            {

                if (!CreateCommand(out error, out command, "GetClubByPAN",
                                new SqlParameter("@PAN", PAN)))
                {
                    throw new Exception(error);
                }

                if (!ExecuteReader(out error, out reader, command))
                {
                    throw new Exception(error);
                }
                if (reader.Read())
                {
                    Response.ID = Convert.ToInt16(reader["ID"].ToString());
                    Response.Name = reader["Name"].ToString();
                    Response.Description = reader["Description"].ToString();
                    Response.IsActive = Convert.ToBoolean(reader["IsActive"]);
                    Response.CreationDate = reader["CreationDate"].ToString();
                }

                if (Response.Name == "" || Response.CreationDate == "")
                {
                    error = "can not find Club for PAN:" + PAN;
                    result = false;
                }
                else
                    result = true;
            }
            catch (Exception ex)
            {
                error = ex.Message;
                result = false;
            }
            finally
            {
                if (command.Connection.State == ConnectionState.Open)
                    command.Connection.Close();

                if (reader != null && !reader.IsClosed)
                    reader.Close();

                command.Dispose();

            }

            return result;
        }


        //--------------------------  Insert Methods  --------------------------\\

        public static bool InsertCard(out string error, string Club_ID, string PAN, bool IsActive, string HolderName, string HolderFamily, string HolderMobile, string HolderAddress, string HolderPAN)
        {
            bool result = false;
            error = "";
            SqlCommand command = null;

            try
            {
                if (!CreateCommand(out error, out command, "ADDCard",
                                new SqlParameter("@ClubID", Club_ID),
                                new SqlParameter("@PAN", PAN),
                                new SqlParameter("@IsActive", IsActive),
                                new SqlParameter("@HolderName", HolderName),
                                new SqlParameter("@HolderFamily", HolderFamily),
                                new SqlParameter("@HolderMobile", HolderMobile),
                                new SqlParameter("@HolderAddress", HolderAddress),
                                new SqlParameter("@HolderPAN", HolderPAN)
                                ))
                {
                    throw new Exception(error);
                }

                int queryresult;

                if (!ExecuteNonQuery(out error, out queryresult, command))
                {
                    throw new Exception(error);
                }

                result = true;
               
            }
            catch (Exception ex)
            {
                error = ex.Message;
                result = false;
            }
            finally
            {
                if (command.Connection.State == ConnectionState.Open)
                    command.Connection.Close();
                command.Dispose();

            }

            return result;
        
        }

        public static bool InsertClub(out string error, string Name, string Description, bool Internal, bool IsActive)
        {
            bool result = false;
            error = "";
            SqlCommand command = null;

            try
            {
                if (!CreateCommand(out error, out command, "ADDClub",
                                new SqlParameter("@Name", Name),
                                new SqlParameter("@description", Description),
                                new SqlParameter("@Internal", Internal),
                                new SqlParameter("@IsActive", IsActive)
                                ))
                {
                    throw new Exception(error);
                }

                int queryresult;

                if (!ExecuteNonQuery(out error, out queryresult, command))
                {
                    throw new Exception(error);
                }

                result = true;

            }
            catch (Exception ex)
            {
                error = ex.Message;
                result = false;
            }
            finally
            {
                if (command.Connection.State == ConnectionState.Open)
                    command.Connection.Close();
                command.Dispose();

            }

            return result;

        }

        public static bool InsertMerchant(out string error, string MerchantID, string Name, string Club_ID, bool IsActive)
        {
            bool result = false;
            error = "";
            SqlCommand command = null;

            try
            {
                if (!CreateCommand(out error, out command, "ADDMerchant",
                                new SqlParameter("@MerchantID", MerchantID),
                                new SqlParameter("@Name", Name),
                                new SqlParameter("@Club_ID", Club_ID),
                                new SqlParameter("@IsActive", IsActive)
                                ))
                {
                    throw new Exception(error);
                }

                int queryresult;

                if (!ExecuteNonQuery(out error, out queryresult, command))
                {
                    throw new Exception(error);
                }

                result = true;

            }
            catch (Exception ex)
            {
                error = ex.Message;
                result = false;
            }
            finally
            {
                if (command.Connection.State == ConnectionState.Open)
                    command.Connection.Close();
                command.Dispose();

            }

            return result;

        }

        public static bool InsertTerminal(out string error, string Merchant_ID, string TerminalID)
        {
            bool result = false;
            error = "";
            SqlCommand command = null;

            try
            {
                if (!CreateCommand(out error, out command, "ADDTerminal",
                                new SqlParameter("@Merchant_ID", Merchant_ID),
                                new SqlParameter("@TerminalID", TerminalID)
                                ))
                {
                    throw new Exception(error);
                }

                int queryresult;

                if (!ExecuteNonQuery(out error, out queryresult, command))
                {
                    throw new Exception(error);
                }

                result = true;

            }
            catch (Exception ex)
            {
                error = ex.Message;
                result = false;
            }
            finally
            {
                if (command.Connection.State == ConnectionState.Open)
                    command.Connection.Close();
                command.Dispose();

            }

            return result;

        }

        public static bool InsertBIN(out string error, string Club_ID, string BIN)
        {
            bool result = false;
            error = "";
            SqlCommand command = null;

            try
            {
                if (!CreateCommand(out error, out command, "ADDBIN",
                                new SqlParameter("@Club_ID", Club_ID),
                                new SqlParameter("@BIN", BIN)
                                ))
                {
                    throw new Exception(error);
                }

                int queryresult;

                if (!ExecuteNonQuery(out error, out queryresult, command))
                {
                    throw new Exception(error);
                }

                result = true;

            }
            catch (Exception ex)
            {
                error = ex.Message;
                result = false;
            }
            finally
            {
                if (command.Connection.State == ConnectionState.Open)
                    command.Connection.Close();
                command.Dispose();

            }

            return result;

        }





        public static bool VerifyRequestInsert()
        {
            return true;
        }
        public static bool UserAuthenticate()
        {
            return true;
        }
        public static bool UserIsPermitted()
        {
            return true;
        }

        public static bool CreateVerifyResponse()
        {
            return true;
        }

        public static bool VerifyResponseInsert()
        {
            return true;
        }

        public static bool RequestSuccessed()
        {
            return true;
        }


    }
}
