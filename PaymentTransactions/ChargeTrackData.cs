using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;
using AuthorizeNet.Api.Controllers;
using AuthorizeNet.Api.Contracts.V1;
using AuthorizeNet.Api.Controllers.Bases;

namespace net.authorize.sample
{
    /// <summary>
    /// This sample demonstrates making a credit card charge using encrypted track data, e.g. data from approved Authorize.Net
    /// card readers.  Readers are available at https://partner.posportal.com/authorizenet/auth/ 
    /// Remember to order sandbox readers for testing and production readers for merchant business use
    /// 
    /// NOTE:  You must pass the retails fields of DeviceType and MarketType, e.g. 
    /// 
    ///         retail = new transRetailInfoType { deviceType = "1", marketType = "2" }
    ///         
    /// </summary>
    public class ChargeTrackData
    {
        public static ANetApiResponse Run(String ApiLoginID, String ApiTransactionKey, decimal amount)
        {
            Console.WriteLine("Charge Track Data Sample");

            ApiOperationBase<ANetApiRequest, ANetApiResponse>.RunEnvironment = AuthorizeNet.Environment.SANDBOX;

            // define the merchant information (authentication / transaction id)
            ApiOperationBase<ANetApiRequest, ANetApiResponse>.MerchantAuthentication = new merchantAuthenticationType()
            {
                name = ApiLoginID,
                ItemElementName = ItemChoiceType.transactionKey,
                Item = ApiTransactionKey,
            };

            // You can pass either track 1 or track 2 but not both
            var trackData = new creditCardTrackType { ItemElementName = ItemChoiceType1.track2, Item = "4111111111111111=170310199999888" };

            //standard api call, simply use cardData in place of creditcard type
            var paymentType = new paymentType { Item = trackData };

            // Add line Items
            var lineItems = new lineItemType[2];
            lineItems[0] = new lineItemType { itemId = "1", name = "t-shirt", quantity = 2, unitPrice = new Decimal(15.00) };
            lineItems[1] = new lineItemType { itemId = "2", name = "snowboard", quantity = 1, unitPrice = new Decimal(450.00) };

            var transactionRequest = new transactionRequestType
            {
                transactionType = transactionTypeEnum.authCaptureTransaction.ToString(),    // charge the card

                amount = amount,
                payment = paymentType,
                lineItems = lineItems,
                // Retail data required for POS transactions
                retail = new transRetailInfoType { deviceType = "1", marketType = "2" }
            };

            var request = new createTransactionRequest { transactionRequest = transactionRequest };

            // instantiate the contoller that will call the service
            var controller = new createTransactionController(request);
            controller.Execute();

            // get the response from the service (errors contained if any)
            var response = controller.GetApiResponse();

            if (response != null && response.messages.resultCode == messageTypeEnum.Ok)
            {
                if (response.transactionResponse != null)
                {
                    Console.WriteLine("Success, Auth Code : " + response.transactionResponse.authCode);
                }
            }
            else if (response != null)
            {
                Console.WriteLine("Error: " + response.messages.message[0].code + "  " + response.messages.message[0].text);
                if (response.transactionResponse != null)
                {
                    Console.WriteLine("Transaction Error : " + response.transactionResponse.errors[0].errorCode + " " + response.transactionResponse.errors[0].errorText);
                }
            }

            return response;
        }
    }
}
