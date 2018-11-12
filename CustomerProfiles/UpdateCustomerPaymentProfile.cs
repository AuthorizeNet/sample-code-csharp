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
    public class UpdateCustomerPaymentProfile
    {
        public static ANetApiResponse Run(String ApiLoginID, String ApiTransactionKey, string customerProfileId, string customerPaymentProfileId)
        {
            Console.WriteLine("Update Customer payment profile sample");

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
                expirationDate = "1028"
            };

            //===========================================================================
            // NOTE:  For updating just the address, not the credit card/payment data 
            //        you can pass the masked values returned from 
            //        GetCustomerPaymentProfile or GetCustomerProfile
            //        E.g.
            //                * literal values shown below
            //===========================================================================
            /*var creditCard = new creditCardType
            {
                cardNumber = "XXXX1111",
                expirationDate = "XXXX"
            };*/

            var paymentType = new paymentType { Item = creditCard };

            var paymentProfile = new customerPaymentProfileExType
            {
                billTo = new customerAddressType
                {
                    // change information as required for billing
                    firstName = "John",
                    lastName = "Doe",
                    address = "123 Main St.",
                    city = "Bellevue",
                    state = "WA",
                    zip = "98004",
                    country = "USA",
                    phoneNumber = "000-000-000",
                },
                payment = paymentType,
                customerPaymentProfileId = customerPaymentProfileId
            };
            
            var request = new updateCustomerPaymentProfileRequest();
            request.customerProfileId = customerProfileId;
            request.paymentProfile = paymentProfile;
            request.validationMode = validationModeEnum.liveMode;
            

            // instantiate the controller that will call the service
            var controller = new updateCustomerPaymentProfileController(request);
            controller.Execute();

            // get the response from the service (errors contained if any)
            var response = controller.GetApiResponse();

            if (response != null && response.messages.resultCode == messageTypeEnum.Ok)
            {
                Console.WriteLine(response.messages.message[0].text);
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
