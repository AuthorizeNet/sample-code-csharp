using System;
using System.Collections.Generic;
using AuthorizeNet.Api.Controllers;
using AuthorizeNet.Api.Contracts.V1;
using AuthorizeNet.Api.Controllers.Bases;

namespace net.authorize.sample
{
    public class CreateCustomerProfile
    {
        public static ANetApiResponse Run(string ApiLoginID, string ApiTransactionKey, string emailId)
        {
            Console.WriteLine("Create Customer Profile Sample");

            // set whether to use the sandbox environment, or production enviornment
            ApiOperationBase<ANetApiRequest, ANetApiResponse>.RunEnvironment = AuthorizeNet.Environment.SANDBOX;

            // define the merchant information (authentication / transaction id)
            ApiOperationBase<ANetApiRequest, ANetApiResponse>.MerchantAuthentication = new merchantAuthenticationType()
            {
                name            = ApiLoginID,
                ItemElementName = ItemChoiceType.transactionKey,
                Item            = ApiTransactionKey,
            };

            var creditCard = new creditCardType
            {
                cardNumber      = "4111111111111111",
                expirationDate  = "1028"
            };

            var bankAccount = new bankAccountType
            {
                accountNumber = "231323342",
                routingNumber = "000000224",
                accountType = bankAccountTypeEnum.checking,
                echeckType = echeckTypeEnum.WEB,
                nameOnAccount = "test",
                bankName = "Bank Of America"
            };

            // standard api call to retrieve response
            paymentType cc = new paymentType { Item = creditCard };
            paymentType echeck = new paymentType {Item = bankAccount};

            List<customerPaymentProfileType> paymentProfileList = new List<customerPaymentProfileType>();
            customerPaymentProfileType ccPaymentProfile = new customerPaymentProfileType();
            ccPaymentProfile.payment = cc;
            
            customerPaymentProfileType echeckPaymentProfile = new customerPaymentProfileType();
            echeckPaymentProfile.payment = echeck;

            paymentProfileList.Add(ccPaymentProfile);
            paymentProfileList.Add(echeckPaymentProfile);

            List<customerAddressType> addressInfoList = new List<customerAddressType>();
            customerAddressType homeAddress = new customerAddressType();
            homeAddress.address = "10900 NE 8th St";
            homeAddress.city = "Seattle";
            homeAddress.zip = "98006";


            customerAddressType officeAddress = new customerAddressType();
            officeAddress.address = "1200 148th AVE NE";
            officeAddress.city = "NorthBend";
            officeAddress.zip = "92101";

            addressInfoList.Add(homeAddress);
            addressInfoList.Add(officeAddress);


            customerProfileType customerProfile = new customerProfileType();
            customerProfile.merchantCustomerId = "Test CustomerID";
            customerProfile.email = emailId;
            customerProfile.paymentProfiles = paymentProfileList.ToArray();
            customerProfile.shipToList = addressInfoList.ToArray();

            var request = new createCustomerProfileRequest{ profile = customerProfile, validationMode = validationModeEnum.none};

            // instantiate the controller that will call the service
            var controller = new createCustomerProfileController(request);
            controller.Execute();

            // get the response from the service (errors contained if any)
            createCustomerProfileResponse response = controller.GetApiResponse();
           
            // validate response 
            if (response != null)
            {
                if (response.messages.resultCode == messageTypeEnum.Ok)
                {
                    if(response.messages.message != null)
                    {
                        Console.WriteLine("Success!");
                        Console.WriteLine("Customer Profile ID: " + response.customerProfileId);
                        Console.WriteLine("Payment Profile ID: " + response.customerPaymentProfileIdList[0]);
                        Console.WriteLine("Shipping Profile ID: " + response.customerShippingAddressIdList[0]);                    }
                }
                else
                {
                    Console.WriteLine("Customer Profile Creation Failed.");
                    Console.WriteLine("Error Code: " + response.messages.message[0].code);
                    Console.WriteLine("Error message: " + response.messages.message[0].text);
                }
            }
            else
            {
                if (controller.GetErrorResponse().messages.message.Length > 0)
                {
                    Console.WriteLine("Customer Profile Creation Failed.");
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
