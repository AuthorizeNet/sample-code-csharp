using AuthorizeNet.Api.Contracts.V1;
using AuthorizeNet.Api.Controllers;
using AuthorizeNet.Api.Controllers.Bases;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace net.authorize.sample.MobileInappTransactions
{
    public class CreateAnAcceptTransaction
    {
        public static ANetApiResponse Run(String ApiLoginID, String ApiTransactionKey, Decimal Amount)
        {
            Console.WriteLine("Create an Accept Transaction Sample");

            ApiOperationBase<ANetApiRequest, ANetApiResponse>.RunEnvironment = AuthorizeNet.Environment.SANDBOX;

            // define the merchant information (authentication / transaction id)
            ApiOperationBase<ANetApiRequest, ANetApiResponse>.MerchantAuthentication = new merchantAuthenticationType()
            {
                name = ApiLoginID,
                ItemElementName = ItemChoiceType.transactionKey,
                Item = ApiTransactionKey,
            };

            //set up data based on transaction
            var opaqueData = new opaqueDataType { dataDescriptor = "COMMON.ACCEPT.INAPP.PAYMENT", dataValue = "9471471570959063005001" };

            //standard api call to retrieve response
            var paymentType = new paymentType { Item = opaqueData };

            var transactionRequest = new transactionRequestType
            {
                transactionType = transactionTypeEnum.authCaptureTransaction.ToString(),    // authorize and capture transaction
                amount = Amount,
                payment = paymentType
            };

            var request = new createTransactionRequest { transactionRequest = transactionRequest };

            // instantiate the contoller that will call the service
            var controller = new createTransactionController(request);
            controller.Execute();

            // get the response from the service (errors contained if any)
            var response = controller.GetApiResponse();

            //validate
            if (response != null && response.messages.resultCode == messageTypeEnum.Ok)
            {
                if (response.transactionResponse.responseCode == "1")
                {
                    Console.WriteLine("Successfully create an accept transaction with Transaction ID : " + response.transactionResponse.transId);
                }
                else
                {
                    Console.WriteLine("The Transaction failed with response code :" + response.transactionResponse.responseCode);
                }
            }
            else if (response != null)
            {
                Console.WriteLine("Error: " + response.messages.message[0].code + "  " + response.messages.message[0].text);
                if (response.transactionResponse != null)
                {
                    Console.WriteLine("Transaction Error : " + response.transactionResponse.errors[0].errorCode + " " + response.transactionResponse.errors[0].errorText);
                }
            }
            else
            {
                Console.WriteLine("The response is null");
            }

            return response;
        }
    }
}
