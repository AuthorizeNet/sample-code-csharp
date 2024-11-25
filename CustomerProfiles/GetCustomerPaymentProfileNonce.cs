using AuthorizeNet.Api.Contracts.V1;
using AuthorizeNet.Api.Controllers;
using AuthorizeNet.Api.Controllers.Bases;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace net.authorize.sample.CustomerProfiles
{
    class GetCustomerPaymentProfileNonce
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

            var request = new getCustomerPaymentProfileNonceRequest();
            request.connectedAccessToken = "eyJraWQiOiI1YWI2NTIxNDBlZGU3ZWZkMDAwMDAwMDA1NGNlOWRhOCIsImFsZyI6IlJTMjU2In0.eyJqdGkiOiIyMGIyYWU1Ni1hZjk4LTQ5OWMtOTczOS04ZDg1MWQ3YjBkMDIiLCJzY29wZXMiOlsicmVhZCIsIndyaXRlIl0sImlhdCI6MTU0MzM5OTYwOTU0NiwiYXNzb2NpYXRlZF9pZCI6IjM3ODciLCJjbGllbnRfaWQiOiJ4ZVFmcFJSSTVYIiwibWVyY2hhbnRfaWQiOiI2NjgzOTQiLCJhZGRpdGlvbmFsSW5mbyI6IntcImFwaUxvZ2luSWRcIjpcIjI1TDdLVmd3NyAgICAgICAgICAgXCIsXCJyb3V0aW5nSWRcIjpcIiQkMjVMN0tWZ3c3JCRcIn0iLCJleHBpcmVzX2luIjoxNTQzNDI4NDA5NTQ4LCJncmFudF90eXBlIjoiYXV0aG9yaXphdGlvbl9jb2RlIiwic29sdXRpb25faWQiOiJBQUExMDI5MjIifQ.JQL3YovrTOuh3UaBGLxP8RNbzGGeJ1Id309lysnMcRJEYDCpv6999A4n6Yznr6uzePjpEwbiyd2osDoGnrP_wQmpLwGPR3eBb3DIOiAhKuAbc1YdpsNa3rd2qbVHPFO95_x2y6r7yRCvgNiRx01GFOXphZ3gPrSuHd93U-h0OLd6nt2GKQQcZ8IQ7f-44fViNgLEH_FTPETKAaooSK8v4XFa7Fh3rYM-jd5snrK4dnp7L2xcLb3JivKwsVXCtLGkNbjXu6DQFtlbzEyVknv9j7GBJgOTvsE_lBqmQaFIdNrYiOf6bH0xAfelgNy_7db77zvSPfvrH9afb5DB_pTl-Q";
            request.customerProfileId = customerProfileId;
            request.customerPaymentProfileId = customerPaymentProfileId;

            // instantiate the controller that will call the service
            var controller = new getCustomerPaymentProfileNonceController(request);
            controller.Execute();

            // get the response from the service (errors contained if any)
            var response = controller.GetApiResponse();

            if (response != null && response.messages.resultCode == messageTypeEnum.Ok)
            {
                Console.WriteLine(response.messages.message[0].text);
                Console.WriteLine("Data Descriptor      : " + response.opaqueData.dataDescriptor);
                Console.WriteLine("Data Value           : " + response.opaqueData.dataValue);
                Console.WriteLine("Expiration Time Stamp: " + response.opaqueData.expirationTimeStamp);

            }
            else if (response != null)
            {
                Console.WriteLine("Error: " + response.messages.message[0].code + "  " +
                                  response.messages.message[0].text);
            }
            else
            {
                // Display the error code and message when response is null
                ANetApiResponse errorResponse = controller.GetErrorResponse();
                Console.WriteLine("Failed to get response");
                if (!string.IsNullOrEmpty(errorResponse.messages.message.ToString()))
                {
                    Console.WriteLine("Error Code: " + errorResponse.messages.message[0].code);
                    Console.WriteLine("Error message: " + errorResponse.messages.message[0].text);
                }

            }

            return response;
        }
    }
}