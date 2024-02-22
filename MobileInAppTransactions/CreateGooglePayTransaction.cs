using AuthorizeNet.Api.Contracts.V1;
using AuthorizeNet.Api.Controllers;
using AuthorizeNet.Api.Controllers.Bases;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace net.authorize.sample
{
    public class CreateGooglePayTransaction
    {
        public static ANetApiResponse Run(String ApiLoginID, String ApiTransactionKey, Decimal Amount)
        {
            Console.WriteLine("Create Google Pay Transaction Sample");

            ApiOperationBase<ANetApiRequest, ANetApiResponse>.RunEnvironment = AuthorizeNet.Environment.SANDBOX;

            ApiOperationBase<ANetApiRequest, ANetApiResponse>.MerchantAuthentication = new merchantAuthenticationType()
            {
                name = ApiLoginID,
                ItemElementName = ItemChoiceType.transactionKey,
                Item = ApiTransactionKey,
            };

            var opaqueData = new opaqueDataType()
            {
                dataDescriptor = "COMMON.GOOGLE.INAPP.PAYMENT",
                dataValue = "1234567890ABCDEF1111AAAA2222BBBB3333CCCC4444DDDD5555EEEE6666FFFF7777888899990000",
            };

            var paymentType = new paymentType()
            {
                Item = opaqueData
            };

            var lineItems = new lineItemType[]
            {
                new lineItemType()
                {
                    itemId = "1",
                    name = "vase",
                    description = "Cannes logo",
                    quantity = 18,
                    unitPrice = 45.00M
                }
            };

            var tax = new extendedAmountType()
            {
                amount = Amount,
                name = "level2 tax name",
                description = "level2 tax"
            };

            var userFields = new userField[]
            {
                new userField()
                {
                    name = "UserDefinedFieldName1",
                    value = "UserDefinedFieldValue1"
                },
                new userField()
                {
                    name = "UserDefinedFieldName2",
                    value = "UserDefinedFieldName2"
                }
            };

            var transactionRequest = new transactionRequestType()
            {
                transactionType = transactionTypeEnum.authCaptureTransaction.ToString(),
                amount = Amount,
                payment = paymentType,
                lineItems = lineItems,
                tax = tax,
                userFields = userFields
            };

            var request = new createTransactionRequest { transactionRequest = transactionRequest };

            var controller = new createTransactionController(request);
            controller.Execute();

            var response = controller.GetApiResponse();

            if (response != null)
            {
                if (response.messages.resultCode == messageTypeEnum.Ok)
                {
                    if (response.transactionResponse.messages != null)
                    {
                        Console.WriteLine("Successfully created a Google Pay transaction with Transaction ID: " + response.transactionResponse.transId);
                        Console.WriteLine("Response Code: " + response.transactionResponse.responseCode);
                        Console.WriteLine("Message Code: " + response.transactionResponse.messages[0].code);
                        Console.WriteLine("Description: " + response.transactionResponse.messages[0].description);
                    }
                    else
                    {
                        Console.WriteLine("Failed Transaction.");
                        if (response.transactionResponse.errors != null)
                        {
                            Console.WriteLine("Error Code: " + response.transactionResponse.errors[0].errorCode);
                            Console.WriteLine("Error message: " + response.transactionResponse.errors[0].errorText);
                        }
                    }
                }
                else
                {
                    Console.WriteLine("Failed Transaction.");
                    if (response.transactionResponse != null && response.transactionResponse.errors != null)
                    {
                        Console.WriteLine("Error Code: " + response.transactionResponse.errors[0].errorCode);
                        Console.WriteLine("Error message: " + response.transactionResponse.errors[0].errorText);
                    }
                    else
                    {
                        Console.WriteLine("Error Code: " + response.messages.message[0].code);
                        Console.WriteLine("Error message: " + response.messages.message[0].text);
                    }
                }
            }
            else
            {
                Console.WriteLine("Null Response.");
            }

            return response;
        }
    }
}
