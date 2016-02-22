using System;
using System.Collections.Generic;
using System.Linq;
using AuthorizeNet.Api.Controllers;
using AuthorizeNet.Api.Contracts.V1;
using AuthorizeNet.Api.Controllers.Bases;

namespace net.authorize.sample
{
    public class DeleteCustomerShippingAddress
    {
        public static ANetApiResponse Run(String ApiLoginID, String ApiTransactionKey, string customerProfileId,
            string customerAddressId)
        {
            Console.WriteLine("DeleteCustomerShippingAddress Sample");
            ApiOperationBase<ANetApiRequest, ANetApiResponse>.RunEnvironment = AuthorizeNet.Environment.SANDBOX;
            ApiOperationBase<ANetApiRequest, ANetApiResponse>.MerchantAuthentication = new merchantAuthenticationType()
            {
                name            = ApiLoginID,
                ItemElementName = ItemChoiceType.transactionKey,
                Item            = ApiTransactionKey,
            };

            //please update the subscriptionId according to your sandbox credentials
            var request = new deleteCustomerShippingAddressRequest
            {
                customerProfileId = customerProfileId,
                customerAddressId = customerAddressId
            };

            //Prepare Request
            var controller = new deleteCustomerShippingAddressController(request);
            controller.Execute();

             //Send Request to EndPoint
            deleteCustomerShippingAddressResponse response = controller.GetApiResponse(); 
            if (response != null && response.messages.resultCode == messageTypeEnum.Ok)
            {
                if (response != null && response.messages.message != null)
                {
                    Console.WriteLine("Success, ResultCode : " + response.messages.resultCode.ToString());
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
