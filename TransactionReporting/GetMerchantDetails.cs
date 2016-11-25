using AuthorizeNet.Api.Contracts.V1;
using AuthorizeNet.Api.Controllers;
using AuthorizeNet.Api.Controllers.Bases;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace net.authorize.sample
{
    class GetMerchantDetails
    {
        public static ANetApiResponse Run(String ApiLoginID, String ApiTransactionKey)
        {
            Console.WriteLine("Get Merchant Details Sample");

            ApiOperationBase<ANetApiRequest, ANetApiResponse>.RunEnvironment = AuthorizeNet.Environment.SANDBOX;

            var request = new getMerchantDetailsRequest
            {
                merchantAuthentication = new merchantAuthenticationType() { name = ApiLoginID, Item = ApiTransactionKey, ItemElementName = ItemChoiceType.transactionKey }
            };

            // instantiate the controller that will call the service
            var controller = new getMerchantDetailsController(request);
            controller.Execute();

            // get the response from the service (errors contained if any)
            var response = controller.GetApiResponse();

            // validate
            if (response != null)
            {
                if (response.messages.resultCode == messageTypeEnum.Ok)
                {
                    Console.WriteLine("Merchant Name: " + response.merchantName);
                    Console.WriteLine("Gateway ID: " + response.gatewayId);
                    Console.Write("Processors: ");
                    foreach (processorType processor in response.processors)
                    {
                        Console.Write(processor.name + "; ");
                    }
                }
                else
                {
                    Console.WriteLine("Failed to get merchant details.");
                }
            }
            else
            {
                Console.WriteLine("Null Response.");
            }

            return response;
        }
    }
}
