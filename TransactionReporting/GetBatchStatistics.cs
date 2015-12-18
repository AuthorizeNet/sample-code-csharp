using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AuthorizeNet;
using AuthorizeNet.Api.Controllers;
using AuthorizeNet.Api.Contracts.V1;
using AuthorizeNet.Api.Controllers.Bases;

namespace net.authorize.sample
{
    public class GetBatchStatistics
    {
        public static ANetApiResponse Run(String ApiLoginID, String ApiTransactionKey)
        {
            Console.WriteLine("Get batch statistics sample");

            ApiOperationBase<ANetApiRequest, ANetApiResponse>.RunEnvironment = AuthorizeNet.Environment.SANDBOX;
            // define the merchant information (authentication / transaction id)
            ApiOperationBase<ANetApiRequest, ANetApiResponse>.MerchantAuthentication = new merchantAuthenticationType()
            {
                name = ApiLoginID,
                ItemElementName = ItemChoiceType.transactionKey,
                Item = ApiTransactionKey,
            };


            // unique batch id
            string batchId = "12345";
            var request = new getBatchStatisticsRequest();
            request.batchId = batchId;

            // instantiate the controller that will call the service
            var controller = new getBatchStatisticsController(request);
            controller.Execute();

            // get the response from the service (errors contained if any)
            var response = controller.GetApiResponse();

            if (response != null && response.messages.resultCode == messageTypeEnum.Ok)
            {
                if (response.batch == null)
                    return response;

                Console.WriteLine("Batch Id: {0}", response.batch.batchId);
                Console.WriteLine("Batch settled on (UTC): {0}", response.batch.settlementTimeUTC);
                Console.WriteLine("Batch settled on (Local): {0}", response.batch.settlementTimeLocal);
                Console.WriteLine("Batch settlement state: {0}", response.batch.settlementState);
                Console.WriteLine("Batch payment method: {0}", response.batch.paymentMethod);
                foreach (var item in response.batch.statistics)
                {
                    Console.WriteLine(
                        "Account type: {0} Charge amount: {1} Charge count: {2} Refund amount: {3} Refund count: {4} Void count: {5} Decline count: {6} Error amount: {7}",
                        item.accountType, item.chargeAmount, item.chargeCount, item.refundAmount, item.refundCount,
                        item.voidCount, item.declineCount, item.errorCount);
                }
            }
            else if (response != null)
            {
                Console.WriteLine("Error: " + response.messages.message[0].code + "  " +
                                  response.messages.message[0].text);
            }
            else
            {
                Console.WriteLine("Null response");
            }

            return response;
        }
    }
}