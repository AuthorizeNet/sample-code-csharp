using System;
using NUnit.Framework;
using AuthorizeNet.Api.Controllers;
using AuthorizeNet.Api.Contracts.V1;
using AuthorizeNet.Api.Controllers.Bases;
using net.authorize.sample;
using System.IO;
using System.Reflection;
using AuthorizeNet;
using net.authorize.sample.PaymentTransactions;
using System.Threading;
using net.authorize.sample.CustomerProfiles;
using net.authorize.sample.MobileInappTransactions;

namespace SampleCodeTest
{
    [TestFixture]
    public class TestRunner
    {
        string apiLoginId = Constants.API_LOGIN_ID;
        string transactionKey = Constants.TRANSACTION_KEY;
        string TransactionID = Constants.TRANSACTION_ID;
        string payerID = Constants.PAYER_ID;

        static CryptoRandom r = new CryptoRandom();

        private static string GetEmail()
        {
            return r.Next(1000, 89999999) + "@test.com";
        }

        private static decimal GetAmount()
        {
            return r.Next(10, 200);
        }

        private static short GetMonth()
        {
            return (Int16)r.Next(7, 365);
        }

        [Test]
        public void TestAllSampleCodes()
        {
            string fileName = Constants.CONFIG_FILE;
            StreamReader reader = File.OpenText(fileName);
            TestRunner tr = new TestRunner();
            var numRetries = 3;

            string line;
            while ((line = reader.ReadLine()) != null)
            {
                string[] items = line.Split('\t');
                
                string apiName = items[0];
                string isDependent = items[1];
                string shouldApiRun = items[2];

                if (!shouldApiRun.Equals("1"))
                    continue;

                Console.WriteLine(new String('-', 20));
                Console.WriteLine("Running test case for :: " + apiName);
                Console.WriteLine(new String('-', 20));
                ANetApiResponse response = null;
                for (int i = 0; i < numRetries; ++i)
                {
                    try
                    {
                        if (isDependent.Equals("0"))
                        {
                            response = InvokeRunMethod(apiName);
                        }
                        else
                        {
                            response = (ANetApiResponse)typeof(TestRunner).GetMethod("Test" + apiName).Invoke(tr, new Object[] { });
                        }

                        if ((response != null) && (response.messages.resultCode == messageTypeEnum.Ok))
                            break;
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(apiName);
                        Console.WriteLine(e.ToString());
                    }
                }
                Assert.IsNotNull(response);
                Assert.AreEqual(response.messages.resultCode, messageTypeEnum.Ok);
            }
        }

        public ANetApiResponse InvokeRunMethod(string className)
        {
            string namespaceString = "net.authorize.sample.";
           
            if (className.Equals(typeof(UpdateSplitTenderGroup).Name))
                namespaceString = namespaceString + "PaymentTransactions.";

            if (className.Equals(typeof(CreateAnApplePayTransaction).Name))
                namespaceString = namespaceString + "ApplePayTransactions.";

            Type classType = Type.GetType(namespaceString + className + ",SampleCode");            
            return (ANetApiResponse)classType.GetMethod("Run").Invoke(null, new Object[] { apiLoginId, transactionKey });
        }

        public ANetApiResponse TestValidateCustomerPaymentProfile()
        {
            var response = (createCustomerProfileResponse)CreateCustomerProfile.Run(apiLoginId, transactionKey, GetEmail());
            var customerPaymentProfile = (createCustomerPaymentProfileResponse)CreateCustomerPaymentProfile.Run(apiLoginId, transactionKey, response.customerProfileId);
            var validateResponse = ValidateCustomerPaymentProfile.Run(apiLoginId, transactionKey, response.customerProfileId, customerPaymentProfile.customerPaymentProfileId);
            DeleteCustomerProfile.Run(apiLoginId, transactionKey, response.customerProfileId);
            
            return validateResponse;
        }

        public ANetApiResponse TestCaptureFundsAuthorizedThroughAnotherChannel()
        {
            return CaptureFundsAuthorizedThroughAnotherChannel.Run(apiLoginId, transactionKey, GetAmount());                 
        }

        public ANetApiResponse TestDebitBankAccount()
        {
            return DebitBankAccount.Run(apiLoginId, transactionKey, GetAmount());
        }
        
        public ANetApiResponse TestUpdateCustomerShippingAddress()
        {
            var response = (createCustomerProfileResponse)CreateCustomerProfile.Run(apiLoginId, transactionKey, GetEmail());
            var shippingResponse = (createCustomerShippingAddressResponse)CreateCustomerShippingAddress.Run(apiLoginId, transactionKey, response.customerProfileId);
            var updateResponse = (updateCustomerShippingAddressResponse) UpdateCustomerShippingAddress.Run(apiLoginId, transactionKey, response.customerProfileId, shippingResponse.customerAddressId);
            DeleteCustomerProfile.Run(apiLoginId, transactionKey, response.customerProfileId);

            return updateResponse;
        }

        public ANetApiResponse TestUpdateCustomerProfile()
        {
            var response = (createCustomerProfileResponse)CreateCustomerProfile.Run(apiLoginId, transactionKey, GetEmail());
            var updateResponse = UpdateCustomerProfile.Run(apiLoginId, transactionKey, response.customerProfileId);
            DeleteCustomerProfile.Run(apiLoginId, transactionKey, response.customerProfileId);
                    
            return updateResponse;
        }

        public ANetApiResponse TestUpdateCustomerPaymentProfile()
        {
            var response = (createCustomerProfileResponse)CreateCustomerProfile.Run(apiLoginId, transactionKey, GetEmail());
            var paymentProfileResponse = (createCustomerPaymentProfileResponse)CreateCustomerPaymentProfile.Run(apiLoginId, transactionKey, response.customerProfileId);
            var updateResponse = UpdateCustomerPaymentProfile.Run(apiLoginId, transactionKey,
                response.customerProfileId, paymentProfileResponse.customerPaymentProfileId);
            DeleteCustomerProfile.Run(apiLoginId, transactionKey, response.customerProfileId);

            return updateResponse;
        }

        public ANetApiResponse TestGetCustomerShippingAddress()
        {
            var response = (createCustomerProfileResponse)CreateCustomerProfile.Run(apiLoginId, transactionKey, GetEmail());
            var shippingResponse = (createCustomerShippingAddressResponse)CreateCustomerShippingAddress.Run(apiLoginId, transactionKey, response.customerProfileId);

            var getResponse = GetCustomerShippingAddress.Run(apiLoginId, transactionKey, 
                response.customerProfileId, shippingResponse.customerAddressId);

            DeleteCustomerProfile.Run(apiLoginId, transactionKey, response.customerProfileId);
            return getResponse;
        }
                    
        public ANetApiResponse TestGetCustomerProfileIds()
        {
            return GetCustomerProfileIds.Run(apiLoginId, transactionKey);
        }

        public ANetApiResponse TestGetCustomerProfile()
        {
            var response = (createCustomerProfileResponse)CreateCustomerProfile.Run(apiLoginId, transactionKey, GetEmail());
            var profileResponse = GetCustomerProfile.Run(apiLoginId, transactionKey, response.customerProfileId);
            DeleteCustomerProfile.Run(apiLoginId, transactionKey, response.customerProfileId);

            return profileResponse;
        }

        public ANetApiResponse TestGetAcceptCustomerProfilePage()
        {
            var response = (createCustomerProfileResponse)CreateCustomerProfile.Run(apiLoginId, transactionKey, GetEmail());
            var profileResponse = GetAcceptCustomerProfilePage.Run(apiLoginId, transactionKey, response.customerProfileId);
            DeleteCustomerProfile.Run(apiLoginId, transactionKey, response.customerProfileId);

            return profileResponse;
        }

        public ANetApiResponse TestGetCustomerPaymentProfile()
        {
            var response = (createCustomerProfileResponse)CreateCustomerProfile.Run(apiLoginId, transactionKey, GetEmail());
            var paymentProfileResponse = (createCustomerPaymentProfileResponse)CreateCustomerPaymentProfile.Run(apiLoginId, transactionKey, response.customerProfileId);
            var getResponse = GetCustomerPaymentProfile.Run(apiLoginId, transactionKey,
                response.customerProfileId, paymentProfileResponse.customerPaymentProfileId);

            DeleteCustomerProfile.Run(apiLoginId, transactionKey, response.customerProfileId);
            return getResponse;
        }
                    
        public ANetApiResponse TestDeleteCustomerShippingAddress()
        {
            var response = (createCustomerProfileResponse)CreateCustomerProfile.Run(apiLoginId, transactionKey, GetEmail());
            var shippingResponse = (createCustomerShippingAddressResponse)CreateCustomerShippingAddress.Run(apiLoginId, transactionKey, response.customerProfileId);
            var deleteResponse = DeleteCustomerShippingAddress.Run(apiLoginId, transactionKey,
                response.customerProfileId, shippingResponse.customerAddressId);
            DeleteCustomerProfile.Run(apiLoginId, transactionKey, response.customerProfileId);

            return deleteResponse;
        }

        public ANetApiResponse TestDeleteCustomerProfile()
        {
            var response = (createCustomerProfileResponse)CreateCustomerProfile.Run(apiLoginId, transactionKey, GetEmail());
            return DeleteCustomerProfile.Run(apiLoginId, transactionKey, response.customerProfileId);
        }

        public ANetApiResponse TestDeleteCustomerPaymentProfile()
        {
            var response = (createCustomerProfileResponse)CreateCustomerProfile.Run(apiLoginId, transactionKey, GetEmail());
            var paymentProfileResponse = (createCustomerPaymentProfileResponse)CreateCustomerPaymentProfile.Run(apiLoginId, transactionKey, response.customerProfileId);
            var deleteResponse = DeleteCustomerPaymentProfile.Run(apiLoginId, transactionKey,
                response.customerProfileId, paymentProfileResponse.customerPaymentProfileId);
            DeleteCustomerProfile.Run(apiLoginId, transactionKey, response.customerProfileId);

            return deleteResponse;
        }

        public ANetApiResponse TestCreateCustomerShippingAddress()
        {
            var response = (createCustomerProfileResponse)CreateCustomerProfile.Run(apiLoginId, transactionKey, GetEmail());
            var shippingResponse = (createCustomerShippingAddressResponse)CreateCustomerShippingAddress.Run(apiLoginId, transactionKey, response.customerProfileId);
            DeleteCustomerProfile.Run(apiLoginId, transactionKey, response.customerProfileId);
                    
            return shippingResponse;
        }

        public ANetApiResponse TestAuthorizeCreditCard()
        {
            var response = (createTransactionResponse)AuthorizeCreditCard.Run(apiLoginId, transactionKey, GetAmount());
            return response;
        }

        public ANetApiResponse TestCreateCustomerProfileFromTransaction()
        {
            var response = (createTransactionResponse)AuthorizeCreditCard.Run(apiLoginId, transactionKey, GetAmount());
            var profileResponse = (createCustomerProfileResponse)CreateCustomerProfileFromTransaction.Run(apiLoginId, transactionKey, response.transactionResponse.transId);
            DeleteCustomerProfile.Run(apiLoginId, transactionKey, profileResponse.customerProfileId);
                    
            return profileResponse;
        }

        public ANetApiResponse TestGetTransactionDetails()
        {
            var response = (createTransactionResponse)AuthorizeCreditCard.Run(apiLoginId, transactionKey, GetAmount());
            return GetTransactionDetails.Run(apiLoginId, transactionKey, response.transactionResponse.transId);
        }

        public ANetApiResponse TestChargeCreditCard()
        {
            return ChargeCreditCard.Run(apiLoginId, transactionKey, GetAmount());
        }

        public ANetApiResponse TestCapturePreviouslyAuthorizedgetAmount()
        {
            var response = (createTransactionResponse)AuthorizeCreditCard.Run(apiLoginId, transactionKey, GetAmount());
            return CapturePreviouslyAuthorizedAmount.Run(apiLoginId, transactionKey, GetAmount(), response.transactionResponse.transId);
        }

        public ANetApiResponse TestRefund()
        {
            var response = (createTransactionResponse)AuthorizeCreditCard.Run(apiLoginId, transactionKey, GetAmount());
            response = (createTransactionResponse)CapturePreviouslyAuthorizedAmount.Run(apiLoginId, transactionKey, GetAmount(), response.transactionResponse.transId);
            return RefundTransaction.Run(apiLoginId, transactionKey, GetAmount(), response.transactionResponse.transId);
        }

        public ANetApiResponse TestVoid()
        {
            var response = (createTransactionResponse)AuthorizeCreditCard.Run(apiLoginId, transactionKey, GetAmount());
            return VoidTransaction.Run(apiLoginId, transactionKey, response.transactionResponse.transId);
        }

        public ANetApiResponse TestCreditBankAccount()
        {
            return CreditBankAccount.Run(apiLoginId, transactionKey, TransactionID);
        }

        public ANetApiResponse TestChargeCustomerProfile()
        {
            var response = (createCustomerProfileResponse)CreateCustomerProfile.Run(apiLoginId, transactionKey, GetEmail());
            var paymentProfileResponse = (createCustomerPaymentProfileResponse)CreateCustomerPaymentProfile.Run(apiLoginId, transactionKey, response.customerProfileId);
            var chargeResponse = ChargeCustomerProfile.Run(apiLoginId, transactionKey,
                response.customerProfileId, paymentProfileResponse.customerPaymentProfileId, GetAmount());
            DeleteCustomerProfile.Run(apiLoginId, transactionKey, response.customerProfileId);

            return chargeResponse;
        }

        public ANetApiResponse TestPayPalAuthorizeOnly()
        {
            return PayPalAuthorizeOnly.Run(apiLoginId, transactionKey, GetAmount());
        }  
            
        public ANetApiResponse TestPayPalVoid()
        {
            var response = (createTransactionResponse)PayPalAuthorizeCapture.Run(apiLoginId, transactionKey, GetAmount());
            return PayPalVoid.Run(apiLoginId, transactionKey, response.transactionResponse.transId);
        }  

        public ANetApiResponse TestPayPalAuthorizeCapture()
        {
            return PayPalAuthorizeCapture.Run(apiLoginId, transactionKey, GetAmount());
        }

        public ANetApiResponse TestPayPalAuthorizeCaptureContinue()
        {
            var response = (createTransactionResponse)PayPalAuthorizeCapture.Run(apiLoginId, transactionKey, GetAmount());
            return PayPalAuthorizeCaptureContinue.Run(apiLoginId, transactionKey, response.transactionResponse.transId, payerID);
        }  
                  
        public ANetApiResponse TestPayPalAuthorizeOnlyContinue()
        {
            return PayPalAuthorizeOnlyContinue.Run(apiLoginId, transactionKey, TransactionID, payerID);
        }
                    
        public ANetApiResponse TestPayPalCredit()
        {
            return PayPalCredit.Run(apiLoginId, transactionKey, TransactionID);
        }

        public ANetApiResponse TestPayPalGetDetails()
        {
            var response = (createTransactionResponse)PayPalAuthorizeCapture.Run(apiLoginId, transactionKey, GetAmount());
            return PayPalGetDetails.Run(apiLoginId, transactionKey, response.transactionResponse.transId);
        }

        public ANetApiResponse TestPayPalPriorAuthorizationCapture()
        {
            var response = (createTransactionResponse)PayPalAuthorizeCapture.Run(apiLoginId, transactionKey, GetAmount());
            return PayPalPriorAuthorizationCapture.Run(apiLoginId, transactionKey, response.transactionResponse.transId);
        }

        public ANetApiResponse TestCancelSubscription()
        {
            var response = (ARBCreateSubscriptionResponse)CreateSubscription.Run(apiLoginId, transactionKey, GetMonth());
            return CancelSubscription.Run(apiLoginId, transactionKey, response.subscriptionId);
        }

        public ANetApiResponse TestCreateSubscriptionFromCustomerProfile()
        {
            var profileResponse = (createCustomerProfileResponse)CreateCustomerProfile.Run(apiLoginId, transactionKey, GetEmail());
            var paymentProfileResponse = (createCustomerPaymentProfileResponse)CreateCustomerPaymentProfile.Run(apiLoginId, transactionKey, profileResponse.customerProfileId);
            var shippingResponse = (createCustomerShippingAddressResponse)CreateCustomerShippingAddress.Run(apiLoginId, transactionKey, profileResponse.customerProfileId);

            Thread.Sleep(10000);

            var response = (ARBCreateSubscriptionResponse)CreateSubscriptionFromCustomerProfile.Run(apiLoginId, transactionKey, GetMonth(),
                profileResponse.customerProfileId, paymentProfileResponse.customerPaymentProfileId,
                shippingResponse.customerAddressId);

            CancelSubscription.Run(apiLoginId, transactionKey, response.subscriptionId);
            DeleteCustomerProfile.Run(apiLoginId, transactionKey, profileResponse.customerProfileId);
            return response;
        }
    
        public ANetApiResponse TestCreateSubscription()
        {
            var response = (ARBCreateSubscriptionResponse)CreateSubscription.Run(apiLoginId, transactionKey, GetMonth());
            CancelSubscription.Run(apiLoginId, transactionKey, response.subscriptionId);

            return response;
        }
                    
        public ANetApiResponse TestGetSubscriptionStatus()
        {
            var response = (ARBCreateSubscriptionResponse)CreateSubscription.Run(apiLoginId, transactionKey, GetMonth());
            var subscriptionResponse = GetSubscriptionStatus.Run(apiLoginId, transactionKey, response.subscriptionId);
            

            return subscriptionResponse;
        }

        public ANetApiResponse TestGetSubscription()
        {
            var response = (ARBCreateSubscriptionResponse)CreateSubscription.Run(apiLoginId, transactionKey, GetMonth());
            var getResponse = GetSubscription.Run(apiLoginId, transactionKey, response.subscriptionId);
            CancelSubscription.Run(apiLoginId, transactionKey, response.subscriptionId);
            return getResponse;
        }

        public ANetApiResponse TestUpdateSubscription()
        {
            var response = (ARBCreateSubscriptionResponse)CreateSubscription.Run(apiLoginId, transactionKey, GetMonth());
            var updateResponse = UpdateSubscription.Run(apiLoginId, transactionKey, response.subscriptionId);
            CancelSubscription.Run(apiLoginId, transactionKey, response.subscriptionId);

            return updateResponse;
        }

        public ANetApiResponse TestCreateCustomerProfile()
        {
            return CreateCustomerProfile.Run(apiLoginId, transactionKey, GetEmail());
        }
                    
        public ANetApiResponse TestCreateCustomerPaymentProfile()
        {
            var response = (createCustomerProfileResponse)CreateCustomerProfile.Run(apiLoginId, transactionKey, GetEmail());
            return CreateCustomerPaymentProfile.Run(apiLoginId, transactionKey, response.customerProfileId);
        }

        public ANetApiResponse TestCreateAnApplePayTransaction()
        {
            var response = (createTransactionResponse)CreateAnApplePayTransaction.Run(apiLoginId, transactionKey, GetAmount());
            return response;
        }
        public ANetApiResponse TestCreateAnAndroidPayTransaction()
        {
            var response = (createTransactionResponse)CreateAnAndroidPayTransaction.Run(apiLoginId, transactionKey, GetAmount());
            return response;
        }
        public ANetApiResponse TestCreateAnAcceptTransaction()
        {
            var response = (createTransactionResponse)CreateAnAcceptTransaction.Run(apiLoginId, transactionKey, GetAmount());
            return response;
        }

        public ANetApiResponse TestGetHostedPaymentPage()
        {
            return GetHostedPaymentPage.Run(apiLoginId, transactionKey, GetAmount());
        }
    }
}
