using System;
using System.Collections.Generic;
using AuthorizeNet.Api.Controllers;
using AuthorizeNet.Api.Contracts.V1;
using AuthorizeNet.Api.Controllers.Bases;

namespace net.authorize.sample
{
    class CreateCustomerProfile
    {
        public static void Run(string ApiLoginID, string ApiTransactionKey)
        {
            Console.WriteLine("CreateCustomerProfile Sample");

            ApiOperationBase<ANetApiRequest, ANetApiResponse>.RunEnvironment = AuthorizeNet.Environment.SANDBOX;
            ApiOperationBase<ANetApiRequest, ANetApiResponse>.MerchantAuthentication = new merchantAuthenticationType()
            {
                name            = ApiLoginID,
                ItemElementName = ItemChoiceType.transactionKey,
                Item            = ApiTransactionKey,
            };


            var creditCard = new creditCardType
            {
                cardNumber      = "4111111111111111",
                expirationDate  = "0718"
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

            //standard api call to retrieve response
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
            customerProfile.email = "test@test.com";
            customerProfile.paymentProfiles = paymentProfileList.ToArray();
            customerProfile.shipToList = addressInfoList.ToArray();

            var request = new createCustomerProfileRequest{ profile = customerProfile, validationMode = validationModeEnum.none};

            var controller = new createCustomerProfileController(request);          // instantiate the contoller that will call the service
            controller.Execute();

            createCustomerProfileResponse response = controller.GetApiResponse();   // get the response from the service (errors contained if any)
           
            //validate
            if (response != null && response.messages.resultCode == messageTypeEnum.Ok)
            {
                if (response != null && response.messages.message != null)
                {
                    Console.WriteLine("Success, CustomerProfileID : " + response.customerProfileId);
                    Console.WriteLine("Success, CustomerPaymentProfileID : " + response.customerPaymentProfileIdList[0]);
                    Console.WriteLine("Success, CustomerShippingProfileID : " + response.customerShippingAddressIdList[0]);
                }
            }
            else
            {
                Console.WriteLine("Error: " + response.messages.message[0].code + "  " + response.messages.message[0].text);
            }

        }
    }
}
