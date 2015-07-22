using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;
using AuthorizeNet.Api.Controllers;
using AuthorizeNet.Api.Contracts.V1;
using AuthorizeNet.Api.Controllers.Bases;

namespace net.authorize.sample
{
    public class ChargeCreditCard
    {
        public static void Run(String ApiLoginID, String ApiTransactionKey)
        {
            Console.WriteLine("Charge Credit Card Sample");

            ApiOperationBase<ANetApiRequest, ANetApiResponse>.RunEnvironment = AuthorizeNet.Environment.SANDBOX;

            // define the merchant information (authentication / transaction id)
            ApiOperationBase<ANetApiRequest, ANetApiResponse>.MerchantAuthentication = new merchantAuthenticationType()
            {
                name = ApiLoginID,
                ItemElementName = ItemChoiceType.transactionKey,
                Item = ApiTransactionKey,
            };

            var creditCard = new creditCardType
            {
                cardNumber = "4111111111111111",
                expirationDate = "0718"
            };

            //standard api call to retrieve response
            var paymentType = new paymentType { Item = creditCard };

            var transactionRequest = new transactionRequestType
            {
                transactionType = transactionTypeEnum.authCaptureTransaction.ToString(),    // charge the card
                amount = 133.45m,
                payment = paymentType
            };
            
            var request = new createTransactionRequest { transactionRequest = transactionRequest };
            
            // instantiate the contoller that will call the service
            var controller = new createTransactionController(request);
            controller.Execute();
            
            // get the response from the service (errors contained if any)
            var response = controller.GetApiResponse();

            if (response.transactionResponse != null)
            {
                Console.WriteLine("Success, Auth Code : " + response.transactionResponse.authCode);
            }
            else
            {
                if (response.messages != null)
                {
                    Console.WriteLine("Error: " + response.messages.message[0].code + "  " + response.messages.message[0].text);
                }

                if (response.transactionResponse != null)
                {
                    Console.WriteLine("Transaction Error : " + response.transactionResponse.errors[0].errorCode + " " + response.transactionResponse.errors[0].errorText);
                }
            }
           
        }
    }
}
