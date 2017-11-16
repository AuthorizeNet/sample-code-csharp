using System;
using System.Collections.Generic;
using AuthorizeNet.Api.Controllers;
using AuthorizeNet.Api.Contracts.V1;
using AuthorizeNet.Api.Controllers.Bases;

namespace net.authorize.sample
{
    public class CreateCustomerPaymentProfile
    {
        public static ANetApiResponse Run(string ApiLoginID, string ApiTransactionKey, string customerProfileId)
        {
            Console.WriteLine("Create Customer Payment Profile Sample");

            // set whether to use the sandbox environment, or production enviornment
            ApiOperationBase<ANetApiRequest, ANetApiResponse>.RunEnvironment = AuthorizeNet.Environment.SANDBOX;

            // define the merchant information (authentication / transaction id)
            ApiOperationBase<ANetApiRequest, ANetApiResponse>.MerchantAuthentication = new merchantAuthenticationType()
            {
                name            = ApiLoginID,
                ItemElementName = ItemChoiceType.transactionKey,
                Item            = ApiTransactionKey,
            };

            var bankAccount = new bankAccountType
            {
                accountNumber = "01245524321",
                routingNumber = "000000204",
                accountType = bankAccountTypeEnum.checking,
                echeckType = echeckTypeEnum.WEB,
                nameOnAccount = "test",
                bankName = "Bank Of America"
            };

            paymentType echeck = new paymentType {Item = bankAccount};

            var billTo = new customerAddressType
            {
                firstName = "John",
                lastName = "Snow"
            };
            customerPaymentProfileType echeckPaymentProfile = new customerPaymentProfileType();
            echeckPaymentProfile.payment = echeck;
            echeckPaymentProfile.billTo = billTo;

            var request = new createCustomerPaymentProfileRequest
            {
                customerProfileId = customerProfileId,
                paymentProfile = echeckPaymentProfile,
                validationMode = validationModeEnum.none
            };

            // instantiate the controller that will call the service
            var controller = new createCustomerPaymentProfileController(request);
            controller.Execute();

            // get the response from the service (errors contained if any)
            createCustomerPaymentProfileResponse response = controller.GetApiResponse();

            // validate response 
            if (response != null)
            {
                if (response.messages.resultCode == messageTypeEnum.Ok)
                {
                    if(response.messages.message != null)
                    {
                        Console.WriteLine("Success! Customer Payment Profile ID: " + response.customerPaymentProfileId);
                    }
                }
                else
                {
                    Console.WriteLine("Customer Payment Profile Creation Failed.");
                    Console.WriteLine("Error Code: " + response.messages.message[0].code);
                    Console.WriteLine("Error message: " + response.messages.message[0].text);
                    if (response.messages.message[0].code == "E00039")
                    {
                        Console.WriteLine("Duplicate Payment Profile ID: " + response.customerPaymentProfileId);
                    }
                }
            }
            else
            {
                if (controller.GetErrorResponse().messages.message.Length > 0)
                {
                    Console.WriteLine("Customer Payment Profile Creation Failed.");
                    Console.WriteLine("Error Code: " + response.messages.message[0].code);
                    Console.WriteLine("Error message: " + response.messages.message[0].text);
                }
                else
                {
                    Console.WriteLine("Null Response.");
                }
            }

            return response;

        }
    }
}