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
    public class GetTransactionDetails
    {
        public static ANetApiResponse Run(String ApiLoginID, String ApiTransactionKey, string transactionId)
      {
          Console.WriteLine("Get transaction details sample");

          ApiOperationBase<ANetApiRequest, ANetApiResponse>.RunEnvironment = AuthorizeNet.Environment.SANDBOX;
          // define the merchant information (authentication / transaction id)
          ApiOperationBase<ANetApiRequest, ANetApiResponse>.MerchantAuthentication = new merchantAuthenticationType()
          {
              name = ApiLoginID,
              ItemElementName = ItemChoiceType.transactionKey,
              Item = ApiTransactionKey,
          };

          var request = new getTransactionDetailsRequest();
          request.transId = transactionId;

          // instantiate the controller that will call the service
          var controller = new getTransactionDetailsController(request);
          controller.Execute();

          // get the response from the service (errors contained if any)
          var response = controller.GetApiResponse();

          if (response != null && response.messages.resultCode == messageTypeEnum.Ok)
          {
              if (response.transaction == null)
                  return response;

              Console.WriteLine("Transaction Id: {0}", response.transaction.transId);
              Console.WriteLine("Transaction type: {0}", response.transaction.transactionType);
              Console.WriteLine("Transaction status: {0}", response.transaction.transactionStatus);
              Console.WriteLine("Transaction auth amount: {0}", response.transaction.authAmount);
              Console.WriteLine("Transaction settle amount: {0}", response.transaction.settleAmount);
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
