using System;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;
using AuthorizeNet.Api.Controllers;
using AuthorizeNet.Api.Contracts.V1;
using AuthorizeNet.Api.Controllers.Bases;

namespace net.authorize.sample
{
    class CancelSubscription
    {
        public static void Run(String ApiLoginID, String ApiTransactionKey, string RefID, string subscriptionID)
        {
            Console.WriteLine("Cancel Subscription Sample");

            ApiOperationBase<ANetApiRequest, ANetApiResponse>.RunEnvironment = AuthorizeNet.Environment.SANDBOX;

            ApiOperationBase<ANetApiRequest, ANetApiResponse>.MerchantAuthentication = new merchantAuthenticationType()
            {
                name            = ApiLoginID,
                ItemElementName = ItemChoiceType.transactionKey,
                Item            = ApiTransactionKey,
            };

            var request = new ARBCancelSubscriptionRequest { refId = RefID, subscriptionId = subscriptionID };

            var controller = new ARBCancelSubscriptionController(request);                          // instantiate the contoller that will call the service
            controller.Execute();

            ARBCancelSubscriptionResponse response = controller.GetApiResponse();                   // get the response from the service (errors contained if any)

            //validate
            if (response != null && response.messages.resultCode == messageTypeEnum.Ok)
            {
                if (response != null && response.messages.message != null)
                {
                    Console.WriteLine("Success, Subscription Cancelled With RefID : " + response.refId);
                }
            }
            else
            {
                Console.WriteLine("Error: " + response.messages.message[0].code + "  " + response.messages.message[0].text);
            }

        }
    }
}
