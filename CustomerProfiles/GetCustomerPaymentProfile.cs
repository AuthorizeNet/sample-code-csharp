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
    public class GetCustomerPaymentProfile
    {
        public static ANetApiResponse Run(String ApiLoginID, String ApiTransactionKey, string customerProfileId,
            string customerPaymentProfileId)
        {
            Console.WriteLine("Get Customer Payment Profile sample");

            ApiOperationBase<ANetApiRequest, ANetApiResponse>.RunEnvironment = AuthorizeNet.Environment.SANDBOX;
            // define the merchant information (authentication / transaction id)
            ApiOperationBase<ANetApiRequest, ANetApiResponse>.MerchantAuthentication = new merchantAuthenticationType()
            {
                name = ApiLoginID,
                ItemElementName = ItemChoiceType.transactionKey,
                Item = ApiTransactionKey,
            };

            var request = new getCustomerPaymentProfileRequest();
            request.customerProfileId = customerProfileId;
            request.customerPaymentProfileId = customerPaymentProfileId;

            // Set this optional property to true to return an unmasked expiration date
            //request.unmaskExpirationDateSpecified = true;
            //request.unmaskExpirationDate = true;
            

            // instantiate the controller that will call the service
            var controller = new getCustomerPaymentProfileController(request);
            controller.Execute();

            // get the response from the service (errors contained if any)
            var response = controller.GetApiResponse();

            if (response != null && response.messages.resultCode == messageTypeEnum.Ok)
            {
                Console.WriteLine(response.messages.message[0].text);
                Console.WriteLine("Customer Payment Profile Id: " + response.paymentProfile.customerPaymentProfileId);
                if (response.paymentProfile.payment.Item is creditCardMaskedType)
                {
                    Console.WriteLine("Customer Payment Profile Last 4: " + (response.paymentProfile.payment.Item as creditCardMaskedType).cardNumber);
                    Console.WriteLine("Customer Payment Profile Expiration Date: " + (response.paymentProfile.payment.Item as creditCardMaskedType).expirationDate);

                    if (response.paymentProfile.subscriptionIds != null && response.paymentProfile.subscriptionIds.Length > 0)
                    {
                        Console.WriteLine("List of subscriptions : ");
                        for (int i = 0; i < response.paymentProfile.subscriptionIds.Length; i++)
                            Console.WriteLine(response.paymentProfile.subscriptionIds[i]);
                    }                
                }
            }
            else if(response != null)
            {
                Console.WriteLine("Error: " + response.messages.message[0].code + "  " +
                                  response.messages.message[0].text);
            }

            return response;
        }
    }
}
