using System;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;
using AuthorizeNet.Api.Controllers;
using AuthorizeNet.Api.Contracts.V1;
using AuthorizeNet.Api.Controllers.Bases;

namespace net.authorize.sample
{
    class CreateSubscription
    {
        public static void Run(String ApiLoginID, String ApiTransactionKey, string RefID)
        {
            Console.WriteLine("Create Subscription Sample");

            ApiOperationBase<ANetApiRequest, ANetApiResponse>.RunEnvironment = AuthorizeNet.Environment.SANDBOX;

            ApiOperationBase<ANetApiRequest, ANetApiResponse>.MerchantAuthentication = new merchantAuthenticationType()
            {
                name            = ApiLoginID,
                ItemElementName = ItemChoiceType.transactionKey,
                Item            = ApiTransactionKey,
            };

            paymentScheduleTypeInterval interval = new paymentScheduleTypeInterval();

            interval.length = 1;                        // months can be indicated between 1 and 12
            interval.unit = ARBSubscriptionUnitEnum.months;

            paymentScheduleType schedule = new paymentScheduleType
            {
                interval            = interval,
                startDate           = DateTime.Now.AddDays(1),      // start date should be tomorrow
                totalOccurrences    = 9999                          // 999 indicates no end date
            };

            var creditCard = new creditCardType
            {
                cardNumber      = "4111111111111111",
                expirationDate  = "0718"
            };

            //standard api call to retrieve response
            paymentType cc = new paymentType { Item = creditCard };

            nameAndAddressType addressInfo = new nameAndAddressType()
            {
                firstName = "Calvin",
                lastName = "Brown"
            };

            ARBSubscriptionType subscriptionType = new ARBSubscriptionType()
            {
                amount = 35.55m,
                paymentSchedule = schedule,
                billTo = addressInfo,
                payment = cc
            };

            var request = new ARBCreateSubscriptionRequest { refId = RefID, subscription = subscriptionType };

            // instantiate the contoller that will call the service
            var controller = new ARBCreateSubscriptionController(request);

            controller.Execute();

            // get the response from the service (errors contained if any)
            ARBCreateSubscriptionResponse response = controller.GetApiResponse();
           
            //validate
            if (response != null && response.messages.resultCode == messageTypeEnum.Ok)
            {
                if (response != null && response.messages.message != null)
                {
                    Console.WriteLine("Success, Subscription Code : " + response.subscriptionId.ToString());
                }
            }
            else
            {
                Console.WriteLine("Error: " + response.messages.message[0].code + "  " + response.messages.message[0].text);
            }

        }
    }
}
