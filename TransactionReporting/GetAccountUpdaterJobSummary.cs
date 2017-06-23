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
    public class GetAccountUpdaterJobSummary
    {
        public static ANetApiResponse Run(String ApiLoginID, String ApiTransactionKey)
        {
            Console.WriteLine("Get Account Updater job summary sample");

            ApiOperationBase<ANetApiRequest, ANetApiResponse>.RunEnvironment = AuthorizeNet.Environment.SANDBOX;
            // define the merchant information (authentication / transaction id)
            ApiOperationBase<ANetApiRequest, ANetApiResponse>.MerchantAuthentication = new merchantAuthenticationType()
            {
                name = ApiLoginID,
                ItemElementName = ItemChoiceType.transactionKey,
                Item = ApiTransactionKey,
            };

            // parameters for request
            string month = "2017-06";


            var request = new getAccountUpdaterJobSummaryRequest();
            request.month = month;







            // instantiate the controller that will call the service
            var controller = new getAccountUpdaterJobSummaryController(request);
            controller.Execute();

            // get the response from the service (errors contained if any)
            var response = controller.GetApiResponse();

            if (response != null && response.messages.resultCode == messageTypeEnum.Ok)
            {
                if (response.auSummary == null)
                    return response;

                foreach (var result in response.auSummary)
                {
                    Console.WriteLine("Reason Code: {0}", result.auReasonCode);
                    Console.WriteLine("Reason Description: {0}", result.reasonDescription);
                    Console.WriteLine("# of Profiles updated for this reason: {0}", result.profileCount);
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
