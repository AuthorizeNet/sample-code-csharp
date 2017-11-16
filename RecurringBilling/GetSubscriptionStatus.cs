using System;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;
using AuthorizeNet.Api.Controllers;
using AuthorizeNet.Api.Contracts.V1;
using AuthorizeNet.Api.Controllers.Bases;

namespace net.authorize.sample
{
    public class GetSubscriptionStatus
    {
        public static ANetApiResponse Run(String ApiLoginID, String ApiTransactionKey, string subscriptionId)
        {
            Console.WriteLine("Get Subscription Status Sample");

            ApiOperationBase<ANetApiRequest, ANetApiResponse>.RunEnvironment = AuthorizeNet.Environment.SANDBOX;

            ApiOperationBase<ANetApiRequest, ANetApiResponse>.MerchantAuthentication = new merchantAuthenticationType()
            {
                name = ApiLoginID,
                ItemElementName = ItemChoiceType.transactionKey,
                Item = ApiTransactionKey,
            };

            //please update the subscriptionId according to your sandbox credentials
            var request = new ARBGetSubscriptionStatusRequest { subscriptionId = subscriptionId };

            var controller = new ARBGetSubscriptionStatusController(request);                          // instantiate the controller that will call the service
            controller.Execute();

            ARBGetSubscriptionStatusResponse response = controller.GetApiResponse();                   // get the response from the service (errors contained if any)

            // validate response
            if (response != null && response.messages.resultCode == messageTypeEnum.Ok)
            {
                if (response != null && response.messages.message != null)
                {
                    Console.WriteLine("Success, Subscription Retrieved With RefID : " + response.refId);
                }
            }
            else
            {
                Console.WriteLine("Error: " + response.messages.message[0].code + "  " + response.messages.message[0].text);
            }

            return response;
        }
    }
}
