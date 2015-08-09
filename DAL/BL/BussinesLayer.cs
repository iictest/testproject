using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.BL
{

    enum LogType 
    {
        Begin = 0 ,
        Error = 1 ,
        Complete = 2
    }
    class BussinesLayer
    {
        public static bool GetCreditInfo(out string error, GetCreditInfo_Request request, GetCreditInfo_Response response)
        {
            bool result = false;
            error = "";
            //long user_id, duplicatedid = 0;

            //State state = State.RecieveRequest;

            // followpaymentstate_responseclass.State = false;

            long request_id = -1;

            string ip ="";

            DataLayer.Log_Insert(out error, request_id, "request: "  + " Entity: ", "Start Main", "VerifyInstallmentTransaction", "PaymentService", (long)LogType.Begin, ip);

            try
            {

                bool permitted, authenticated = false;

                DataLayer.Log_Insert(out error, request_id, "", "Query.UserIsPermitted", "VerifyInstallmentTransaction", "PaymentService", (long)LogType.Begin, ip);
                if (!DataLayer.UserIsPermitted())
                {
                    // FollowPaymentState_responseclass.Description = ((short)IKCOError.WithError).ToString();
                    DataLayer.Log_Insert(out error, request_id, error, "Query.UserIsPermitted", "VerifyInstallmentTransaction", "PaymentService", (long)LogType.Error, ip);
                }
                else
                {
                    DataLayer.Log_Insert(out error, request_id, "", "Query.UserIsPermitted", "VerifyInstallmentTransaction", "PaymentService", (long)LogType.Complete, ip);
                }
                permitted = true;

                DataLayer.Log_Insert(out error, request_id, "", "Query.UserAuthenticate", "VerifyInstallmentTransaction", "PaymentService", (long)LogType.Begin, ip);
                if (!DataLayer.UserAuthenticate())
                {
                    //FollowPaymentState_responseclass.Description = ((short)IKCOError.WithError).ToString();
                    DataLayer.Log_Insert(out error, request_id, error, "Query.UserAuthenticate", "VerifyInstallmentTransaction", "PaymentService", (long)LogType.Error, ip);
                }

                else
                {
                    DataLayer.Log_Insert(out error, request_id, "", "Query.UserAuthenticate", "VerifyInstallmentTransaction", "PaymentService", (long)LogType.Complete, ip);
                }


                DataLayer.Log_Insert(out error, request_id, "", "Query.FollowPaymentStateRequestInsert", "VerifyInstallmentTransaction", "PaymentService", (long)LogType.Begin, ip);
                if (!DataLayer.VerifyRequestInsert())
                {

                    DataLayer.Log_Insert(out error, request_id, error, "Query.FollowPaymentStateRequestInsert", "VerifyInstallmentTransaction", "PaymentService", (long)LogType.Error, ip);
                    //FollowPaymentState_responseclass.Description = ((short)IKCOError.WithError).ToString();
                    throw new Exception(error);
                }
                else
                {
                    DataLayer.Log_Insert(out error, request_id, "", "Query.FollowPaymentStateRequestInsert", "VerifyInstallmentTransaction", "PaymentService", (long)LogType.Complete, ip);
                }


                

                if (!permitted)
                {
                    //    verifytransaction_responseclass.Error = "";
                    throw new Exception("نداشتن مجوز");
                    //return flagcontractpayment;
                }
               
                if (!authenticated)
                {
                    //    verifytransaction_responseclass.Error = "";
                    throw new Exception("خطا در نام کاربری یا رمز ورود");
                    //return flagcontractpayment;
                }
                //state = State.CreateResponse;

                DataLayer.Log_Insert(out error, request_id, "", "ServiceLayer.GetCreditInfoService", "VerifyInstallmentTransaction", "PaymentService", (long)LogType.Begin, ip);

                if (!SL.ServiceLayer.GetCreditInfoService(out error,request.traceID))
                {
                    //verifytransaction_responseclass.Error = "0008";
                    DataLayer.Log_Insert(out error, request_id, error, "ServiceLayer.GetCreditInfoService", "VerifyInstallmentTransaction", "PaymentService", (long)LogType.Error, ip);
                    // FollowPaymentState_responseclass.Description = ((short)IKCOError.WithError).ToString();
                    throw new Exception(error);
                }
                else
                {
                    DataLayer.Log_Insert(out error, request_id, "", "ServiceLayer.GetCreditInfoService", "VerifyInstallmentTransaction", "PaymentService", (long)LogType.Complete, ip);
                }

                //   state = State.CreateResponse;



               // state = State.CheckPayedPrice;

                if (request.merchantID== null || request.merchantID == "" || request.pan == null || request.pan == "" || request.pan == "0")
                {
                    //verifytransaction_responseclass.Error = "0101";
                    //verifytransaction_responseclass.Description = "پارامترها به درستی مقدار دهی نشده اند.";
                    //throw new Exception(error);
                }

                DataLayer.Log_Insert(out error, request_id, "", "Query.FollowPaymentStateResponseInsert", "VerifyInstallmentTransaction", "PaymentService", (long)LogType.Begin, ip);
                if (!DataLayer.VerifyResponseInsert())
                {
                    DataLayer.Log_Insert(out error, request_id, error, "Query.FollowPaymentStateResponseInsert", "VerifyInstallmentTransaction", "PaymentService", (long)LogType.Error, ip);
                    // FollowPaymentState_responseclass.Description = ((short)IKCOError.WithError).ToString();
                    throw new Exception(error);
                }
                else
                {
                    response.name = "aaa";
                    response.family = "bbb";
                    response.transactionId = "11313442";

                    DataLayer.Log_Insert(out error, request_id, "", "Query.FollowPaymentStateResponseInsert", "VerifyInstallmentTransaction", "PaymentService", (long)LogType.Complete, ip);
                }
                // state = State.IKCOErrorCode;


                //state = State.Success;


            }
            catch (Exception ex)
            {
                //Log Error
                DataLayer.Log_Insert(out error, request_id, error, "Main.catch", "VerifyInstallmentTransaction", "PaymentService", (long)LogType.Error, ip);
                throw new Exception(error);

            }
            finally
            {
                // analyse errDesc
                try
                {

                    if (!DataLayer.RequestSuccessed())
                    {
                        //Log Error
                        DataLayer.Log_Insert(out error, request_id, error, "Query.RequestSuccessed", "VerifyInstallmentTransaction", "PaymentService", (long)LogType.Error, ip);
                        // throw new Exception(error);
                    }
                    //log result
                    DataLayer.Log_Insert(out error, request_id,"", "MyMethods.GetClassPropertiesString(verifytransaction_responseclass)", "VerifyInstallmentTransaction", "PaymentService", (long)LogType.Complete, ip);
                }
                catch (Exception ex)
                {
                    //Query.LogInFile(out error, ex.Message);
                    DataLayer.Log_Insert(out error, request_id, error, "Finally.Catch_RequestSuccessed", "VerifyInstallmentTransaction", "PaymentService", (long)LogType.Error, ip);
                }
            }

            return result;


        }

        public static bool SetCashPayment(out string error, SetCashPayment_Request request)
        {
            error = "";
            bool result = false;
            string ip = "";

            try
            {
                DataLayer.Log_Insert(out error, -1, "this request ", "Start Main", "SetCashPayment", "PaymentService", (long)LogType.Begin, ip);
                if (!SL.ServiceLayer.SetCashPaymentService(out error ,request.pan))
                {

                    DataLayer.Log_Insert(out error, -1, error, "ServiceLayer.SetCashPaymentService", "SetCashPayment", "PaymentService", (long)LogType.Error, ip);
                    //FollowPaymentState_responseclass.Description = ((short)IKCOError.WithError).ToString();
                    
                    throw new Exception(error);
                }
                else
                {
                    result = true;
                    DataLayer.Log_Insert(out error, -1, "Successed", "ServiceLayer.SetCashPaymentService", "SetCashPayment", "PaymentService", (long)LogType.Complete, ip);
                }


            }
            catch (Exception ex)
            {
                DataLayer.Log_Insert(out error, -1, error, "Main.catch", "ServiceLayer.SetCashPaymentService", "PaymentService", (long)LogType.Error, ip);
                throw new Exception(error);
            }
            finally
            {
                try
                {

                    //log result
                    DataLayer.Log_Insert(out error, -1, "", "ServiceLayer.SetCashPaymentService", "VerifyInstallmentTransaction", "PaymentService", (long)LogType.Complete, ip);
                }
                catch (Exception ex)
                {
                    //Query.LogInFile(out error, ex.Message);
                    DataLayer.Log_Insert(out error, -1, error, "Finally.Catch_RequestSuccessed", "VerifyInstallmentTransaction", "PaymentService", (long)LogType.Error, ip);
                }

            }
            return result;
        }

        //public static bool SetCreditPayment(out string error, string transactionId, string date, string time, string merchantID, string pin, string amount, string basicAmount, string terminal, string pan, out string printMsg, out string status, string discount, string menuId, long traceID)
        //{
        //    error = "";
        //    return true;
        //}
        //public static bool VerifyPayment(out string error, string transactionId, string amount, out string output,out string status, long traceID)
        //{
        //    error = "";
        //    return true;
        //}
        //public static bool GetAmount(out string error, string amount, string transactionId, string menuId, out string status, out string accTable, out string displayText)
        //{
        //    error = "";
        //    return true;
        //}
    
    }


}
