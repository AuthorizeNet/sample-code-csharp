using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AuthorizeNet.Api.Controllers;
using AuthorizeNet.Api.Contracts.V1;
using AuthorizeNet.Api.Controllers.Bases;

namespace net.authorize.sample.PaymentTransactions
{
    public class UpdateSplitTenderGroup
    {
        public static ANetApiResponse Run(String ApiLoginID, String ApiTransactionKey)
        {
            Console.WriteLine("Update Split Tender Group Sample");

            ApiOperationBase<ANetApiRequest, ANetApiResponse>.RunEnvironment = AuthorizeNet.Environment.SANDBOX;

            // define the merchant information (authentication / transaction id)
            ApiOperationBase<ANetApiRequest, ANetApiResponse>.MerchantAuthentication = new merchantAuthenticationType()
            {
                name = ApiLoginID,
                ItemElementName = ItemChoiceType.transactionKey,
                Item = ApiTransactionKey,
            };

            //provide a split tender Id
            //To get a split Tender ID in sandbox, authorize any transaction with amount = 462.25 [if card present] and set allowPartialAuth = true
            var splitTenderId = "115901";

            //Void or Complete the partial Authorization
            var splitTenderStatus = splitTenderStatusEnum.voided;

            var request = new updateSplitTenderGroupRequest { splitTenderId = splitTenderId, splitTenderStatus = splitTenderStatus };

            // instantiate the controller that will call the service
            var controller = new updateSplitTenderGroupController(request);
            controller.Execute();
            var response = controller.GetApiResponse();

            // validate response
            if (response != null && response.messages.resultCode == messageTypeEnum.Ok)
            {
                Console.WriteLine("Successfully Updated ... ");
            }
            else if(response != null )
            {
                Console.WriteLine("Error : " + response.messages.message[0].code + "  " + response.messages.message[0].text);
            }

            return response;
        }
    }
}
