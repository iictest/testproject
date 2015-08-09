using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.SL
{
    public class ServiceLayer
    {
        internal static bool GetCreditInfoService(out string error, string ContractRowId)
        {
            bool result = false;
            error = "";

            //IsrvContractSoapClient proxy = new IsrvContractSoapClient();
            BL.BussinesLayer aa = new BL.BussinesLayer();
            try
            {
                // entity = proxy.__GetPriceContract(ContractRowId, aaa);

                result = true;
            }
            catch (Exception ex)
            {
                error = ex.Message.ToString();
                result = false;
            }
            finally
            {
                //if (proxy != null)
                //{
                //    if (proxy.State == CommunicationState.Faulted)
                //        proxy.Abort();
                //    else
                //        proxy.Close();

                //    proxy = null;
                //}
            }

            return result;
        }


        internal static bool SetCashPaymentService(out string error, string Cash)
        {
            bool result = false;
            error = "";
            try
            {

                result = true;
            }
            catch (Exception ex)
            {
                error = ex.Message;
                result = false;
            }
            finally
            {

            }
            return result;

        }

        internal static bool SetCreditPaymentService(out string error, string Credit)
        {
            bool result = false;
            error = "";
            try
            {

                result = true;
            }
            catch (Exception ex)
            {
                error = ex.Message;
                result = false;
            }
            finally
            {

            }
            return result;

        }

        internal static bool VerifyPaymentService(out string error, string Peyment)
        {
            bool result = false;
            error = "";
            try
            {

                result = true;
            }
            catch (Exception ex)
            {
                error = ex.Message;
                result = false;
            }
            finally
            {

            }
            return result;
        }

        internal static bool GetAmountService(out string error, string Peyment)
        {
            bool result = false;
            error = "";
            try
            {

                result = true;
            }
            catch (Exception ex)
            {
                error = ex.Message;
                result = false;
            }
            finally
            {

            }
            return result;
        }
    }
}