using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AuthorizeNet.Api.Contracts.V1;
using AuthorizeNet.Api.Controllers;
using AuthorizeNet.Api.Controllers.Bases;

namespace net.authorize.sample.CustomerProfiles
{
    public class GetHostedPaymentPage
    {
        public static ANetApiResponse Run(String ApiLoginID, String ApiTransactionKey, decimal amount)
        {
            Console.WriteLine("GetHostedPaymentPage Sample");
            ApiOperationBase<ANetApiRequest, ANetApiResponse>.RunEnvironment = AuthorizeNet.Environment.SANDBOX;
            ApiOperationBase<ANetApiRequest, ANetApiResponse>.MerchantAuthentication = new merchantAuthenticationType()
            {
                name = ApiLoginID,
                ItemElementName = ItemChoiceType.transactionKey,
                Item = ApiTransactionKey,
            };

            settingType[] settings = new settingType[2];

            settings[0] = new settingType();
            settings[0].settingName = settingNameEnum.hostedPaymentButtonOptions.ToString();
            settings[0].settingValue = "{\"text\": \"Pay\"}";
            
            settings[1] = new settingType();
            settings[1].settingName = settingNameEnum.hostedPaymentOrderOptions.ToString();
            settings[1].settingValue = "{\"show\": false}";

            var transactionRequest = new transactionRequestType
            {
                transactionType = transactionTypeEnum.authCaptureTransaction.ToString(),    // authorize capture only
                amount = amount
            };

            var request = new getHostedPaymentPageRequest();
            request.transactionRequest = transactionRequest;
            request.hostedPaymentSettings = settings;

            // instantiate the contoller that will call the service
            var controller = new getHostedPaymentPageController(request);
            controller.Execute();

            // get the response from the service (errors contained if any)
            var response = controller.GetApiResponse();

            //validate
            if (response != null && response.messages.resultCode == messageTypeEnum.Ok)
            {
                Console.WriteLine("Message code : " + response.messages.message[0].code);
                Console.WriteLine("Message text : " + response.messages.message[0].text);
                Console.WriteLine("Token : " + response.token);
            }
            else if (response != null)
            {
                Console.WriteLine("Error: " + response.messages.message[0].code + "  " + response.messages.message[0].text);
                Console.WriteLine("Failed to get hosted payment page");
            }

            return response;
        }
    }
}
