using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AuthorizeNet.Api.Contracts.V1;
using AuthorizeNet.Api.Controllers;
using AuthorizeNet.Api.Controllers.Bases;
using System.Collections;

namespace net.authorize.sample
{
    public class GetAccountUpdaterJobDetails
    {
        public static ANetApiResponse Run(String ApiLoginID, String ApiTransactionKey)
        {
            Console.WriteLine("Get Account Updater job details sample");

            ApiOperationBase<ANetApiRequest, ANetApiResponse>.RunEnvironment = AuthorizeNet.Environment.SANDBOX;
            // define the merchant information (authentication / transaction id)
            ApiOperationBase<ANetApiRequest, ANetApiResponse>.MerchantAuthentication = new merchantAuthenticationType()
            {
                name = ApiLoginID,
                ItemElementName = ItemChoiceType.transactionKey,
                Item = ApiTransactionKey,
            };

            // parameters for request
            string month = "2018-08"; //"2017-06";
            string modifiedTypeFilter = "all";

            var request = new getAUJobDetailsRequest();
            request.month = month;
            request.modifiedTypeFilter = AUJobTypeEnum.all;
            request.paging = new Paging
            {
                limit = 100,
                offset = 1
            };

            // instantiate the controller that will call the service
            var controller = new getAUJobDetailsController(request);
            controller.Execute();

            // get the response from the service (errors contained if any)
            var response = controller.GetApiResponse();

            if (response != null && response.messages.resultCode == messageTypeEnum.Ok)
            {
                Console.WriteLine("SUCCESS: Get Account Updater job details for Month and year : " + month);
                if (response.auDetails == null)
                {
                    Console.WriteLine("No GetAccountUpdaterjobdetails for this month and year.");
                    return response;
                }

                // Displaying the Audetails of each response in the list
                foreach (var details in response.auDetails)
                {
                    Console.WriteLine(" **** Customer profile details Start ****");
                    Console.WriteLine("Profile ID / Payment Profile ID: {0} / {1}", details.customerProfileID, details.customerPaymentProfileID);
                    Console.WriteLine("Firstname lastname : {0} / {1}", details.firstName, details.lastName);
                    Console.WriteLine("Update Time (UTC): {0}", details.updateTimeUTC);
                    Console.WriteLine("Reason Code: {0}", details.auReasonCode);
                    Console.WriteLine("Reason Description: {0}", details.reasonDescription);


                    if (details is auUpdateType)
                    {
                        for (int i = 0; i < ((AuthorizeNet.Api.Contracts.V1.auUpdateType)details).SubscriptionIdList.Length; i++)
                        {
                            Console.WriteLine("SubscriptionIdList: {0}", ((AuthorizeNet.Api.Contracts.V1.auUpdateType)details).SubscriptionIdList[i]);
                        }
                    }
                    else if (details is auDeleteType)
                    {
                        for (int i = 0; i < ((AuthorizeNet.Api.Contracts.V1.auDeleteType)details).SubscriptionIdList.Length; i++)
                        {
                            Console.WriteLine("SubscriptionIdList: {0}", ((AuthorizeNet.Api.Contracts.V1.auDeleteType)details).SubscriptionIdList[i]);
                        }
                    }


                    if (details.GetType().GetField("newCreditCard") != null)
                    {
                        Console.WriteLine("Fetching New Card Details");
                        // Fetching New Card Details
                        var newCreditCard = details.GetType().GetField("newCreditCard").GetValue(details);
                        creditCardMaskedType newCreditCardMaskedType = (creditCardMaskedType)newCreditCard;
                        Console.WriteLine("Card Number: {0}", newCreditCardMaskedType.cardNumber);
                        Console.WriteLine("New Expiration Date: {0}", newCreditCardMaskedType.expirationDate);
                        Console.WriteLine("New Card Type: {0}", newCreditCardMaskedType.cardType);
                    }

                    if (details.GetType().GetField("oldCreditCard") != null)
                    {
                        Console.WriteLine("Fetching Old Card Details");
                        // Fetching Old Card Details
                        var oldCreditCard = details.GetType().GetField("oldCreditCard").GetValue(details);
                        creditCardMaskedType oldCreditCardMaskedType = (creditCardMaskedType)oldCreditCard;
                        Console.WriteLine("Old Card Number: {0}", oldCreditCardMaskedType.cardNumber);
                        Console.WriteLine("Old Expiration Date: {0}", oldCreditCardMaskedType.expirationDate);
                        Console.WriteLine("Old Card Type: {0}", oldCreditCardMaskedType.cardType);

                        Console.WriteLine("**** Customer profile details End ****");
                    }
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
