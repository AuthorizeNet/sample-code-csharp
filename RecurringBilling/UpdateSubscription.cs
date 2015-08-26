using System;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;
using AuthorizeNet.Api.Controllers;
using AuthorizeNet.Api.Contracts.V1;
using AuthorizeNet.Api.Controllers.Bases;

namespace net.authorize.sample
{
    class UpdateSubscription
    {
        public static void Run(string ApiLoginID, string ApiTransactionKey)
        {
            Console.WriteLine("Update Subscription Sample");

            ApiOperationBase<ANetApiRequest, ANetApiResponse>.RunEnvironment = AuthorizeNet.Environment.SANDBOX;
            ApiOperationBase<ANetApiRequest, ANetApiResponse>.MerchantAuthentication = new merchantAuthenticationType()
            {
                name = ApiLoginID,
                ItemElementName = ItemChoiceType.transactionKey,
                Item = ApiTransactionKey,
            };

            paymentScheduleType schedule = new paymentScheduleType
            {
                startDate = DateTime.Now.AddDays(1),      // start date should be tomorrow
                totalOccurrences = 9999                          // 999 indicates no end date
            };

            #region Payment Information
            var creditCard = new creditCardType
            {
                cardNumber = "4111111111111111",
                expirationDate = "0718"
            };

            //standard api call to retrieve response
            paymentType cc = new paymentType { Item = creditCard };
            #endregion

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

            //Please change the subscriptionId according to your request
            var request = new ARBUpdateSubscriptionRequest { subscription = subscriptionType, subscriptionId = "100748" };
            var controller = new ARBUpdateSubscriptionController(request);         
            controller.Execute();

            ARBUpdateSubscriptionResponse response = controller.GetApiResponse(); 

            //validate
            if (response != null && response.messages.resultCode == messageTypeEnum.Ok)
            {
                if (response != null && response.messages.message != null)
                {
                    Console.WriteLine("Success, RefID Code : " + response.refId);
                }
            }
            else
            {
                Console.WriteLine("Error: " + response.messages.message[0].code + "  " + response.messages.message[0].text);
            }

        }
    }
}
