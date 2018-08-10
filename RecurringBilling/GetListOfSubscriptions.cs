using System;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;
using AuthorizeNet.Api.Controllers;
using AuthorizeNet.Api.Contracts.V1;
using AuthorizeNet.Api.Controllers.Bases;

namespace net.authorize.sample
{
    public class GetListOfSubscriptions
    {
        public static ANetApiResponse Run(String ApiLoginID, String ApiTransactionKey)
        {
            Console.WriteLine("Get A List of Subscriptions Sample");

            ApiOperationBase<ANetApiRequest, ANetApiResponse>.RunEnvironment = AuthorizeNet.Environment.SANDBOX;

            ApiOperationBase<ANetApiRequest, ANetApiResponse>.MerchantAuthentication = new merchantAuthenticationType()
            {
                name = ApiLoginID,
                ItemElementName = ItemChoiceType.transactionKey,
                Item = ApiTransactionKey,
            };

            var request = new ARBGetSubscriptionListRequest {searchType = ARBGetSubscriptionListSearchTypeEnum.subscriptionActive };    // only gets active subscriptions

            var controller = new ARBGetSubscriptionListController(request);          // instantiate the controller that will call the service
            controller.Execute();

            ARBGetSubscriptionListResponse response = controller.GetApiResponse();   // get the response from the service (errors contained if any)

            // validate response
            if (response != null && response.messages.resultCode == messageTypeEnum.Ok)
            {
                if (response != null && response.messages.message != null && response.subscriptionDetails != null)
                {
                    Console.WriteLine("Success, " + response.totalNumInResultSet + " Results Returned ");
                }
            }
            else if(response != null)
            {
                Console.WriteLine("Error: " + response.messages.message[0].code + "  " + response.messages.message[0].text);
            }

            return response;
        }
    }
}
