using System;
using System.Collections.Generic;
using AuthorizeNet.Api.Controllers;
using AuthorizeNet.Api.Contracts.V1;
using AuthorizeNet.Api.Controllers.Bases;

namespace net.authorize.sample
{
    public class CreateCustomerProfileFromTransaction
    {
        public static ANetApiResponse Run(string ApiLoginID, string ApiTransactionKey, string transactionId)
        {
            Console.WriteLine("CreateCustomerProfileFromTransaction Sample");

            ApiOperationBase<ANetApiRequest, ANetApiResponse>.RunEnvironment = AuthorizeNet.Environment.SANDBOX;
            ApiOperationBase<ANetApiRequest, ANetApiResponse>.MerchantAuthentication = new merchantAuthenticationType()
            {
                name            = ApiLoginID,
                ItemElementName = ItemChoiceType.transactionKey,
                Item            = ApiTransactionKey,
            };

            var request = new createCustomerProfileFromTransactionRequest { transId = transactionId };

            var controller = new createCustomerProfileFromTransactionController(request); 
            controller.Execute();

            createCustomerProfileResponse response = controller.GetApiResponse(); 
           
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
            else if(response != null)
            {
                Console.WriteLine("Error: " + response.messages.message[0].code + "  " + response.messages.message[0].text);
            }

            return response;
        }
    }
}
