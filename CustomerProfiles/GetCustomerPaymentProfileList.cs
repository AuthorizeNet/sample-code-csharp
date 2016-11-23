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
    public class GetCustomerPaymentProfileList
    {
        public static ANetApiResponse Run(String ApiLoginID, String ApiTransactionKey)
        {
            Console.WriteLine("Get Customer Payment Profile List sample");

            ApiOperationBase<ANetApiRequest, ANetApiResponse>.RunEnvironment = AuthorizeNet.Environment.SANDBOX;
            // define the merchant information (authentication / transaction id)
            ApiOperationBase<ANetApiRequest, ANetApiResponse>.MerchantAuthentication = new merchantAuthenticationType()
            {
                name = ApiLoginID,
                ItemElementName = ItemChoiceType.transactionKey,
                Item = ApiTransactionKey
            };

            var request = new getCustomerPaymentProfileListRequest();
            request.searchType = CustomerPaymentProfileSearchTypeEnum.cardsExpiringInMonth;
            request.month = "2020-12";
            request.paging = new Paging();
            request.paging.limit = 50;
            request.paging.offset = 1;

            // instantiate the controller that will call the service
            var controller = new getCustomerPaymentProfileListController(request);
            controller.Execute();

            // get the response from the service (errors contained if any)
            var response = controller.GetApiResponse();

            if (response != null && response.messages.resultCode == messageTypeEnum.Ok)
            {
                Console.WriteLine(response.messages.message[0].text);
                Console.WriteLine("Number of payment profiles : " + response.totalNumInResultSet);
                
                Console.WriteLine("List of Payment profiles : ");
                for(int profile=0; profile < response.paymentProfiles.Length; profile++)
                {
                    Console.WriteLine(response.paymentProfiles[profile].customerPaymentProfileId);
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
                    Console.WriteLine("Null response received : " + controller.GetErrorResponse().messages.message[0].text);
                }
            }

            return response;
        }
    }
}
