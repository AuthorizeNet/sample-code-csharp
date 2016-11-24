﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AuthorizeNet.Api.Contracts.V1;
using AuthorizeNet.Api.Controllers;
using AuthorizeNet.Api.Controllers.Bases;

namespace net.authorize.sample.CustomerProfiles
{
    public class GetHostedPaymentPage
    {
        public static ANetApiResponse Run(String ApiLoginID, String ApiTransactionKey, decimal amount)
        {
            Console.WriteLine("GetHostedPaymentPage Sample");
            ApiOperationBase<ANetApiRequest, ANetApiResponse>.RunEnvironment = AuthorizeNet.Environment.SANDBOX;
            ApiOperationBase<ANetApiRequest, ANetApiResponse>.MerchantAuthentication = new merchantAuthenticationType()
            {
                name = ApiLoginID,
                ItemElementName = ItemChoiceType.transactionKey,
                Item = ApiTransactionKey,
            };

            var creditCard = new creditCardType
            {
                cardNumber = "4111111111111111",
                expirationDate = "0718",
                cardCode = "123"
            };

            //standard api call to retrieve response
            var paymentType = new paymentType { Item = creditCard };

            settingType[] settings = new settingType[]{ new settingType() };
            settings[0].settingName = settingNameEnum.hostedPaymentReturnOptions.ToString();
            settings[0].settingValue = "https://returnurl.com/return/";

            var transactionRequest = new transactionRequestType
            {
                transactionType = transactionTypeEnum.authCaptureTransaction.ToString(),    // authorize capture only
                amount = amount,
                payment = paymentType
            };

            var request = new getHostedPaymentPageRequest();
            request.transactionRequest = transactionRequest;
            request.hostedPaymentSettings = settings;

            // instantiate the contoller that will call the service
            var controller = new getHostedPaymentPageController(request);
            controller.Execute();

            // get the response from the service (errors contained if any)
            var response = controller.GetApiResponse();

            //validate
            if (response != null && response.messages.resultCode == messageTypeEnum.Ok)
            {
                Console.WriteLine("Message code : " + response.messages.message[0].code);
                Console.WriteLine("Message text : " + response.messages.message[0].text);
                Console.WriteLine("Token : " + response.token);
            }
            else if (response != null)
            {
                Console.WriteLine("Error: " + response.messages.message[0].code + "  " + response.messages.message[0].text);
                Console.WriteLine("Failed to get hosted payment page");
            }

            return response;
        }
    }
}