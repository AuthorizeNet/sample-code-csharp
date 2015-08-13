using System;
using System.Collections.Generic;
using AuthorizeNet.Api.Controllers;
using AuthorizeNet.Api.Contracts.V1;
using AuthorizeNet.Api.Controllers.Bases;

namespace net.authorize.sample
{
    class CreateCustomerPaymentProfile
    {
        public static void Run(String ApiLoginID, String ApiTransactionKey)
        {
            Console.WriteLine("CreateCustomerPaymentProfile Sample");
            ApiOperationBase<ANetApiRequest, ANetApiResponse>.RunEnvironment = AuthorizeNet.Environment.SANDBOX;
            ApiOperationBase<ANetApiRequest, ANetApiResponse>.MerchantAuthentication = new merchantAuthenticationType()
            {
                name            = ApiLoginID,
                ItemElementName = ItemChoiceType.transactionKey,
                Item            = ApiTransactionKey,
            };

            var bankAccount = new bankAccountType
            {
                accountNumber = "0123454321",
                routingNumber = "000000204",
                accountType = bankAccountTypeEnum.checking,
                echeckType = echeckTypeEnum.WEB,
                nameOnAccount = "test",
                bankName = "Bank Of America"
            };

            paymentType echeck = new paymentType {Item = bankAccount};
            customerPaymentProfileType echeckPaymentProfile = new customerPaymentProfileType();
            echeckPaymentProfile.payment = echeck;

            var request = new createCustomerPaymentProfileRequest
            {
                customerProfileId = "36537239",
                paymentProfile = echeckPaymentProfile,
                validationMode = validationModeEnum.none
            };

            //Prepare Request
            var controller = new createCustomerPaymentProfileController(request);
            controller.Execute();

             //Send Request to EndPoint
            createCustomerPaymentProfileResponse response = controller.GetApiResponse(); 
            if (response != null && response.messages.resultCode == messageTypeEnum.Ok)
            {
                if (response != null && response.messages.message != null)
                {
                    Console.WriteLine("Success, CustomerProfileID : " + response.customerPaymentProfileId);
                }
            }
            else
            {
                Console.WriteLine("Error: " + response.messages.message[0].code + "  " + response.messages.message[0].text);
            }

        }
    }
}
