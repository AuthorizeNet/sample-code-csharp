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
    public class GetSettledBatchList
    {
        public static ANetApiResponse Run(String ApiLoginID, String ApiTransactionKey)
        {
            Console.WriteLine("Get settled batch list sample");

            ApiOperationBase<ANetApiRequest, ANetApiResponse>.RunEnvironment = AuthorizeNet.Environment.SANDBOX;
            // define the merchant information (authentication / transaction id)
            ApiOperationBase<ANetApiRequest, ANetApiResponse>.MerchantAuthentication = new merchantAuthenticationType()
            {
                name = ApiLoginID,
                ItemElementName = ItemChoiceType.transactionKey,
                Item = ApiTransactionKey,
            };

            //Get a date 1 week in the past
            var firstSettlementDate = DateTime.Today.Subtract(TimeSpan.FromDays(31));
            //Get today's date
            var lastSettlementDate = DateTime.Today;
            Console.WriteLine("First settlement date: {0} Last settlement date:{1}", firstSettlementDate,
                lastSettlementDate);

            var request = new getSettledBatchListRequest();
            request.firstSettlementDate = firstSettlementDate;
            request.lastSettlementDate = lastSettlementDate;
            request.includeStatistics = true;
            
            // instantiate the controller that will call the service
            var controller = new getSettledBatchListController(request);
            controller.Execute();

            // get the response from the service (errors contained if any)
            var response = controller.GetApiResponse();


            if (response != null && response.messages.resultCode == messageTypeEnum.Ok)
            {
                if (response.batchList == null)
                    return response;

                foreach (var batch in response.batchList)
                {
                    Console.WriteLine("Batch Id: {0}", batch.batchId);
                    Console.WriteLine("Batch settled on (UTC): {0}", batch.settlementTimeUTC);
                    Console.WriteLine("Batch settled on (Local): {0}", batch.settlementTimeLocal);
                    Console.WriteLine("Batch settlement state: {0}", batch.settlementState);
                    Console.WriteLine("Batch market type: {0}", batch.marketType);
                    Console.WriteLine("Batch product: {0}", batch.product);
                    foreach (var statistics in batch.statistics)
                    {
                        Console.WriteLine(
                            "Account type: {0} Total charge amount: {1} Charge count: {2} Refund amount: {3} Refund count: {4} Void count: {5} Decline count: {6} Error amount: {7}",
                            statistics.accountType, statistics.chargeAmount, statistics.chargeCount,
                            statistics.refundAmount, statistics.refundCount,
                            statistics.voidCount, statistics.declineCount, statistics.errorCount);
                    }
                }
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