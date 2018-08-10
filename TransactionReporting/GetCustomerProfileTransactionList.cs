using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AuthorizeNet.Api.Contracts.V1;
using AuthorizeNet.Api.Controllers;
using AuthorizeNet.Api.Controllers.Bases;

namespace net.authorize.sample
{
    public class GetCustomerProfileTransactionList
    {
        public static ANetApiResponse Run(String ApiLoginID, String ApiTransactionKey, string customerProfileId)
        {
            Console.WriteLine("Get transaction list sample");

            ApiOperationBase<ANetApiRequest, ANetApiResponse>.RunEnvironment = AuthorizeNet.Environment.SANDBOX;
            // define the merchant information (authentication / transaction id)
            ApiOperationBase<ANetApiRequest, ANetApiResponse>.MerchantAuthentication = new merchantAuthenticationType()
            {
                name = ApiLoginID,
                ItemElementName = ItemChoiceType.transactionKey,
                Item = ApiTransactionKey,
            };

            var request = new getTransactionListForCustomerRequest();
            request.customerProfileId = "1811474252";

            // instantiate the controller that will call the service
            var controller = new getTransactionListForCustomerController(request);
            controller.Execute();

            // get the response from the service (errors contained if any)
            var response = controller.GetApiResponse();

            if (response != null && response.messages.resultCode == messageTypeEnum.Ok)
            {
                if (response.transactions == null)
                    return response;

                foreach (var transaction in response.transactions)
                {
                    Console.WriteLine("Transaction Id: {0}", transaction.transId);
                    Console.WriteLine("Submitted on (Local): {0}", transaction.submitTimeLocal);
                    Console.WriteLine("Status: {0}", transaction.transactionStatus);
                    Console.WriteLine("Settle amount: {0}", transaction.settleAmount);
                }
            }
            else if (response != null)
            {
                Console.WriteLine("Error: " + response.messages.message[0].code + "  " +
                                  response.messages.message[0].text);
            }

            return response;
        }
    }
}
