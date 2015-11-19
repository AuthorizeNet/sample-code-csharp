using System;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;
using AuthorizeNet.Api.Controllers;
using AuthorizeNet.Api.Contracts.V1;
using AuthorizeNet.Api.Controllers.Bases;

namespace net.authorize.sample
{
    class GetSubscription
    {
        public static void Run(String ApiLoginID, String ApiTransactionKey)
        {
            Console.WriteLine("Get Subscription Sample");

            ApiOperationBase<ANetApiRequest, ANetApiResponse>.RunEnvironment = AuthorizeNet.Environment.SANDBOX;

            ApiOperationBase<ANetApiRequest, ANetApiResponse>.MerchantAuthentication = new merchantAuthenticationType()
            {
                name = ApiLoginID,
                ItemElementName = ItemChoiceType.transactionKey,
                Item = ApiTransactionKey
            };

            var request = new ARBGetSubscriptionRequest { subscriptionId = "2260421" };    

            var controller = new ARBGetSubscriptionController(request);          // instantiate the contoller that will call the service
            controller.Execute();

            ARBGetSubscriptionResponse response = controller.GetApiResponse();   // get the response from the service (errors contained if any)

            //validate
            if (response != null && response.messages.resultCode == messageTypeEnum.Ok)
            {
                if (response.subscription != null)
                {
                    Console.WriteLine("Subscription returned : " + response.subscription.name);
                }
            }
            else if (response != null)
            {
                if (response.messages.message.Length > 0)
                {
                    Console.WriteLine("Error: " + response.messages.message[0].code + "  " +
                                      response.messages.message[0].text);
                }
            }
            else
            {
                if (controller.GetErrorResponse().messages.message.Length > 0)
                {
                    Console.WriteLine("Error: " + response.messages.message[0].code + "  " + response.messages.message[0].text);
                }
            }

        }
    }
}
