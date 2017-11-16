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
    public class ChargeEncryptedTrackData
    {
        public static ANetApiResponse Run(String ApiLoginID, String ApiTransactionKey, decimal amount)
        {
            Console.WriteLine("Charge Encrypted Track Data Sample");

            ApiOperationBase<ANetApiRequest, ANetApiResponse>.RunEnvironment = AuthorizeNet.Environment.SANDBOX;

            // define the merchant information (authentication / transaction id)
            ApiOperationBase<ANetApiRequest, ANetApiResponse>.MerchantAuthentication = new merchantAuthenticationType()
            {
                name = ApiLoginID,
                ItemElementName = ItemChoiceType.transactionKey,
                Item = ApiTransactionKey,
            };


            // Track data goes in EncryptedTrack Data Type
            var cardData = new encryptedTrackDataType();

            // KeyManagementScheme corresponds to the encryption settings and data from the reader
            var scheme = new KeyManagementScheme();
            scheme.DUKPT = new KeyManagementSchemeDUKPT {
                Operation = OperationType.DECRYPT,
                DeviceInfo = new KeyManagementSchemeDUKPTDeviceInfo { Description = "4649443D4944544543482E556E694D61672E416E64726F69642E53646B7631" },
                EncryptedData = new KeyManagementSchemeDUKPTEncryptedData { Value = "02f300801f342600039b252a343237352a2a2a2a2a2a2a2a353637355e332f54455354205e2a2a2a2a2a2a2a2a2a2a2a2a2a2a2a2a2a2a2a2a2a2a2a3f2a3b343237352a2a2a2a2a2a2a2a353637353d2a2a2a2a2a2a2a2a2a2a2a2a2a2a2a2a2a2a3f2a521db2603bfe6169fc371211161c4ad6edd7294a8352af1ba8b388c527d8d6335286413ad67521b8be085da998cef7ae3621b0b72eecd6d61953a4a268e02a8cdff3d365216df73646b326d8dac369c11a2a3a4a9336addc4a15ae5d8843e0163bae895b9b4df3253439b4dd885363ad108604ea04f2e4fac701a5a0e65c54e1301a5ed7706eb88762994901000000400015ac1e03" },
                Mode = new KeyManagementSchemeDUKPTMode { Data = null, PIN = "1" } 
            };

            cardData.FormOfPayment = new KeyBlock { Value = new KeyValue { Encoding = EncodingType.Hex, EncryptionAlgorithm = EncryptionAlgorithmType.TDES, Scheme = scheme } };
   

            //standard api call, simply use cardData in place of creditcard type
            var paymentType = new paymentType { Item = cardData };

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

            // instantiate the controller that will call the service
            var controller = new createTransactionController(request);
            controller.Execute();

            // get the response from the service (errors contained if any)
            var response = controller.GetApiResponse();

            // validate response
            if (response != null)
            {
                if (response.messages.resultCode == messageTypeEnum.Ok)
                {
                    if(response.transactionResponse.messages != null)
                    {
                        Console.WriteLine("Successfully created transaction with Transaction ID: " + response.transactionResponse.transId);
                        Console.WriteLine("Response Code: " + response.transactionResponse.responseCode);
                        Console.WriteLine("Message Code: " + response.transactionResponse.messages[0].code);
                        Console.WriteLine("Description: " + response.transactionResponse.messages[0].description);
						Console.WriteLine("Success, Auth Code : " + response.transactionResponse.authCode);
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
