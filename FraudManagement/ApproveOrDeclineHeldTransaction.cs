using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AuthorizeNet;
using AuthorizeNet.Api.Controllers;
using AuthorizeNet.Api.Contracts.V1;
using AuthorizeNet.Api.Controllers.Bases;

namespace net.authorize.sample
{
    class ApproveOrDeclineHeldTransaction
    {
        public static ANetApiResponse Run(String ApiLoginID, String ApiTransactionKey)
        {
            Console.WriteLine("Approve held transaction sample");

            ApiOperationBase<ANetApiRequest, ANetApiResponse>.RunEnvironment = AuthorizeNet.Environment.SANDBOX;
            // define the merchant information (authentication / transaction id)
            ApiOperationBase<ANetApiRequest, ANetApiResponse>.MerchantAuthentication = new merchantAuthenticationType()
            {
                name = ApiLoginID,
                ItemElementName = ItemChoiceType.transactionKey,
                Item = ApiTransactionKey,
            };

            var request = new updateHeldTransactionRequest();
            request.heldTransactionRequest = new heldTransactionRequestType
            {
                action = afdsTransactionEnum.approve,
                refTransId = "60108066607"
            };
            
            // instantiate the controller that will call the service
            var controller = new updateHeldTransactionController(request);
            controller.Execute();

            // get the response from the service (errors contained if any)
            var response = controller.GetApiResponse();
            if (response != null && response.messages.resultCode == messageTypeEnum.Ok)
            {
                Console.WriteLine("Transaction Approved: "+response.transactionResponse.transId);
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
