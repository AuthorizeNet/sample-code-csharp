using System;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;
using AuthorizeNet.Api.Controllers;
using AuthorizeNet.Api.Contracts.V1;
using AuthorizeNet.Api.Controllers.Bases;

namespace net.authorize.sample
{
    class CancelSubscription
    {
        public static void Run(String ApiLoginID, String ApiTransactionKey, string RefID)
        {
            Console.WriteLine("Cancel Subscription Sample");

            ApiOperationBase<ANetApiRequest, ANetApiResponse>.MerchantAuthentication = new merchantAuthenticationType()
            {
                name = ApiLoginID,
                ItemElementName = ItemChoiceType.transactionKey,
                Item = ApiTransactionKey,
            };

            ARBCancelSubscriptionRequest arbSubscription = new ARBCancelSubscriptionRequest();

            arbSubscription.refId                                   = RefID;    // merchant-assigned ID associated with the customer profile
            
            paymentScheduleTypeInterval interval = new paymentScheduleTypeInterval();

            interval.length = 1;                // months can be indicated between 1 and 12

            var paymentScheduleType = new paymentScheduleType
            {
                interval            = interval,
                startDate           = DateTime.Now,
                totalOccurrences    = 9999        // 999 indicates no end date          
            };

            ApiOperationBase<ANetApiRequest, ANetApiResponse>.RunEnvironment = AuthorizeNet.Environment.SANDBOX;

            var creditCard = new creditCardType
            {
                cardNumber      = "4111111111111111",
                expirationDate  = "0718"
            };

            //standard api call to retrieve response
            var paymentType = new paymentType { Item = creditCard };

            var transactionRequest = new transactionRequestType
            {
                amount = 35.45m,
                payment = paymentType
            };

            var request = new createTransactionRequest { transactionRequest = transactionRequest, merchantAuthentication = arbSubscription.merchantAuthentication };

            // instantiate the contoller that will call the service
            var controller = new createTransactionController(request);
            controller.Execute();

            // get the response from the service (errors contained if any)
            var response = controller.GetApiResponse();

            //validate
            if (response.messages.resultCode == messageTypeEnum.Ok)
            {
                if (response.transactionResponse != null)
                {
                    Console.WriteLine("Success, Auth Code : " + response.transactionResponse.authCode);
                }
            }
            else
            {
                Console.WriteLine("Error: " + response.messages.message[0].code + "  " + response.messages.message[0].text);
                if (response.transactionResponse != null)
                {
                    Console.WriteLine("Transaction Error : " + response.transactionResponse.errors[0].errorCode + " " + response.transactionResponse.errors[0].errorText);
                }
            }

        }
    }
}
