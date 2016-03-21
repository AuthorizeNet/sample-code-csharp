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
    public class GetCustomerShippingAddress
    {
        public static ANetApiResponse Run(String ApiLoginID, String ApiTransactionKey, string customerProfileId,
            string customerAddressId)
        {
            Console.WriteLine("Get Customer Shipping Address sample");

            ApiOperationBase<ANetApiRequest, ANetApiResponse>.RunEnvironment = AuthorizeNet.Environment.SANDBOX;
            // define the merchant information (authentication / transaction id)
            ApiOperationBase<ANetApiRequest, ANetApiResponse>.MerchantAuthentication = new merchantAuthenticationType()
            {
                name = ApiLoginID,
                ItemElementName = ItemChoiceType.transactionKey,
                Item = ApiTransactionKey,
            };

            var request = new getCustomerShippingAddressRequest();
            request.customerProfileId = customerProfileId;
            request.customerAddressId = customerAddressId;

            // instantiate the controller that will call the service
            var controller = new getCustomerShippingAddressController(request);
            controller.Execute();

            // get the response from the service (errors contained if any)
            var response = controller.GetApiResponse();

            if (response != null && response.messages.resultCode == messageTypeEnum.Ok)
            {
                Console.WriteLine(response.messages.message[0].text);
                if (response.subscriptionIds != null && response.subscriptionIds.Length > 0)
                {
                    Console.WriteLine("List of subscriptions : ");
                    for (int i = 0; i < response.subscriptionIds.Length; i++)
                        Console.WriteLine(response.subscriptionIds[i]);
                }

            }
            else if(response != null)
            {
                Console.WriteLine("Error: " + response.messages.message[0].code + "  " +
                                  response.messages.message[0].text);
            }

            return response;
        }
    }
}
