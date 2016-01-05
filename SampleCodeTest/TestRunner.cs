using System;
using NUnit.Framework;
using AuthorizeNet.Api.Controllers;
using AuthorizeNet.Api.Contracts.V1;
using AuthorizeNet.Api.Controllers.Bases;
using net.authorize.sample;
using System.IO;
using System.Reflection;
using AuthorizeNet;

namespace SampleCodeTest
{
    [TestFixture]
    public class TestRunner
    {
        String apiLoginId = Constants.API_LOGIN_ID;
        String transactionKey = Constants.TRANSACTION_KEY;
        string TransactionID = Constants.TRANSACTION_ID;
        string payerID = Constants.PAYER_ID;

        static CryptoRandom r = new CryptoRandom();

        private static string getEmail()
        {
            return r.Next(1000, 8908) + "@test.com";
        }

        private static decimal getAmount()
        {
            return r.Next(10, 200);
        }

        private static short getMonth()
        {
            return (Int16)r.Next(7, 365);
        }

        [Test]
        public void TestAllSampleCodes()
        {
            string fileName = Constants.CONFIG_FILE;
            StreamReader reader = File.OpenText(fileName);
            TestRunner tr = new TestRunner();

            string line;
            while ((line = reader.ReadLine()) != null)
            {
                string[] items = line.Split('\t');
                
                string apiName = items[0];
                string isDependent = items[1];
                string shouldApiRun = items[2];

                Console.WriteLine(apiName);

                if (!shouldApiRun.Equals("1"))
                    continue;

                try
                {
                    ANetApiResponse response = null;
                    if (isDependent.Equals("0"))
                    {
                        response = InvokeRunMethod(apiName); 
                    }
                    else
                    {
                        response = (ANetApiResponse)typeof(TestRunner).GetMethod("Test" + apiName).Invoke(tr, new Object[] { });
                    }

                    Assert.IsNotNull(response);
                    Assert.AreEqual(response.messages.resultCode, messageTypeEnum.Ok);
                }
                catch (Exception e)
                {
                    Console.WriteLine(apiName);
                    Console.WriteLine(e.ToString());
                }
            }
        }

        public ANetApiResponse InvokeRunMethod(string className)
        {
            string namespaceString = "net.authorize.sample.";
            
            if (className.Equals("UpdateSplitTenderGroup"))
                namespaceString = namespaceString + "PaymentTransactions.";

            if (className.Equals("CreateAnApplePayTransaction"))
                namespaceString = namespaceString + "ApplePayTransactions.";

            Type classType = Type.GetType(namespaceString + className + ",SampleCode");            
            return (ANetApiResponse)classType.GetMethod("Run").Invoke(null, new Object[] { apiLoginId, transactionKey });
        }

        public ANetApiResponse TestValidateCustomerPaymentProfile()
        {
            var response = (createCustomerProfileResponse)CreateCustomerProfile.Run(apiLoginId, transactionKey, getEmail());
            var customerPaymentProfile = (createCustomerPaymentProfileResponse)CreateCustomerPaymentProfile.Run(apiLoginId, transactionKey, response.customerProfileId);
            var validateResponse = ValidateCustomerPaymentProfile.Run(apiLoginId, transactionKey, response.customerProfileId, customerPaymentProfile.customerPaymentProfileId);
            DeleteCustomerProfile.Run(apiLoginId, transactionKey, response.customerProfileId);
            
            return validateResponse;
        }


        public ANetApiResponse TestUpdateCustomerShippingAddress()
        {
            var response = (createCustomerProfileResponse)CreateCustomerProfile.Run(apiLoginId, transactionKey, getEmail());
            var shippingResponse = (createCustomerShippingAddressResponse)CreateCustomerShippingAddress.Run(apiLoginId, transactionKey, response.customerProfileId);
            var updateResponse = (updateCustomerShippingAddressResponse) UpdateCustomerShippingAddress.Run(apiLoginId, transactionKey, response.customerProfileId, shippingResponse.customerAddressId);
            DeleteCustomerProfile.Run(apiLoginId, transactionKey, response.customerProfileId);

            return updateResponse;
        }

        public ANetApiResponse TestUpdateCustomerProfile()
        {
            var response = (createCustomerProfileResponse)CreateCustomerProfile.Run(apiLoginId, transactionKey, getEmail());
            var updateResponse = UpdateCustomerProfile.Run(apiLoginId, transactionKey, response.customerProfileId);
            DeleteCustomerProfile.Run(apiLoginId, transactionKey, response.customerProfileId);
                    
            return updateResponse;
        }

        public ANetApiResponse TestUpdateCustomerPaymentProfile()
        {
            var response = (createCustomerProfileResponse)CreateCustomerProfile.Run(apiLoginId, transactionKey, getEmail());
            var paymentProfileResponse = (createCustomerPaymentProfileResponse)CreateCustomerPaymentProfile.Run(apiLoginId, transactionKey, response.customerProfileId);
            var updateResponse = UpdateCustomerPaymentProfile.Run(apiLoginId, transactionKey,
                response.customerProfileId, paymentProfileResponse.customerPaymentProfileId);
            DeleteCustomerProfile.Run(apiLoginId, transactionKey, response.customerProfileId);

            return updateResponse;
        }

        public ANetApiResponse TestGetCustomerShippingAddress()
        {
            var response = (createCustomerProfileResponse)CreateCustomerProfile.Run(apiLoginId, transactionKey, getEmail());
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
            var response = (createCustomerProfileResponse)CreateCustomerProfile.Run(apiLoginId, transactionKey, getEmail());
            var profileResponse = GetCustomerProfile.Run(apiLoginId, transactionKey, response.customerProfileId);
            DeleteCustomerProfile.Run(apiLoginId, transactionKey, response.customerProfileId);

            return profileResponse;
        }

        public ANetApiResponse TestGetHostedProfilePage()
        {
            var response = (createCustomerProfileResponse)CreateCustomerProfile.Run(apiLoginId, transactionKey, getEmail());
            var profileResponse = GetHostedProfilePage.Run(apiLoginId, transactionKey, response.customerProfileId);
            DeleteCustomerProfile.Run(apiLoginId, transactionKey, response.customerProfileId);

            return profileResponse;
        }

        public ANetApiResponse TestGetCustomerPaymentProfile()
        {
            var response = (createCustomerProfileResponse)CreateCustomerProfile.Run(apiLoginId, transactionKey, getEmail());
            var paymentProfileResponse = (createCustomerPaymentProfileResponse)CreateCustomerPaymentProfile.Run(apiLoginId, transactionKey, response.customerProfileId);
            var getResponse = GetCustomerPaymentProfile.Run(apiLoginId, transactionKey,
                response.customerProfileId, paymentProfileResponse.customerPaymentProfileId);

            DeleteCustomerProfile.Run(apiLoginId, transactionKey, response.customerProfileId);
            return getResponse;
        }
                    
        public ANetApiResponse TestDeleteCustomerShippingAddress()
        {
            var response = (createCustomerProfileResponse)CreateCustomerProfile.Run(apiLoginId, transactionKey, getEmail());
            var shippingResponse = (createCustomerShippingAddressResponse)CreateCustomerShippingAddress.Run(apiLoginId, transactionKey, response.customerProfileId);
            var deleteResponse = DeleteCustomerShippingAddress.Run(apiLoginId, transactionKey,
                response.customerProfileId, shippingResponse.customerAddressId);
            DeleteCustomerProfile.Run(apiLoginId, transactionKey, response.customerProfileId);

            return deleteResponse;
        }

        public ANetApiResponse TestDeleteCustomerProfile()
        {
            var response = (createCustomerProfileResponse)CreateCustomerProfile.Run(apiLoginId, transactionKey, getEmail());
            return DeleteCustomerProfile.Run(apiLoginId, transactionKey, response.customerProfileId);
        }

        public ANetApiResponse TestDeleteCustomerPaymentProfile()
        {
            var response = (createCustomerProfileResponse)CreateCustomerProfile.Run(apiLoginId, transactionKey, getEmail());
            var paymentProfileResponse = (createCustomerPaymentProfileResponse)CreateCustomerPaymentProfile.Run(apiLoginId, transactionKey, response.customerProfileId);
            var deleteResponse = DeleteCustomerPaymentProfile.Run(apiLoginId, transactionKey,
                response.customerProfileId, paymentProfileResponse.customerPaymentProfileId);
            DeleteCustomerProfile.Run(apiLoginId, transactionKey, response.customerProfileId);

            return deleteResponse;
        }

        public ANetApiResponse TestCreateCustomerShippingAddress()
        {
            var response = (createCustomerProfileResponse)CreateCustomerProfile.Run(apiLoginId, transactionKey, getEmail());
            var shippingResponse = (createCustomerShippingAddressResponse)CreateCustomerShippingAddress.Run(apiLoginId, transactionKey, response.customerProfileId);
            DeleteCustomerProfile.Run(apiLoginId, transactionKey, response.customerProfileId);
                    
            return shippingResponse;
        }

        public ANetApiResponse TestAuthorizeCreditCard()
        {
            var response = (createTransactionResponse)AuthorizeCreditCard.Run(apiLoginId, transactionKey, getAmount());
            return response;
        }

        public ANetApiResponse TestCreateCustomerProfileFromTransaction()
        {
            var response = (createTransactionResponse)AuthorizeCreditCard.Run(apiLoginId, transactionKey, getAmount());
            var profileResponse = (createCustomerProfileResponse)CreateCustomerProfileFromTransaction.Run(apiLoginId, transactionKey, response.transactionResponse.transId);
            DeleteCustomerProfile.Run(apiLoginId, transactionKey, profileResponse.customerProfileId);
                    
            return profileResponse;
        }

        public ANetApiResponse TestGetTransactionDetails()
        {
            var response = (createTransactionResponse)AuthorizeCreditCard.Run(apiLoginId, transactionKey, getAmount());
            return GetTransactionDetails.Run(apiLoginId, transactionKey, response.transactionResponse.transId);
        }

        public ANetApiResponse TestChargeCreditCard()
        {
            return ChargeCreditCard.Run(apiLoginId, transactionKey, getAmount());
        }

        public ANetApiResponse TestCapturePreviouslyAuthorizedgetAmount()
        {
            var response = (createTransactionResponse)AuthorizeCreditCard.Run(apiLoginId, transactionKey, getAmount());
            return CapturePreviouslyAuthorizedAmount.Run(apiLoginId, transactionKey, getAmount(), response.transactionResponse.transId);
        }

        public ANetApiResponse TestRefund()
        {
            var response = (createTransactionResponse)AuthorizeCreditCard.Run(apiLoginId, transactionKey, getAmount());
            response = (createTransactionResponse)CapturePreviouslyAuthorizedAmount.Run(apiLoginId, transactionKey, getAmount(), response.transactionResponse.transId);
            return RefundTransaction.Run(apiLoginId, transactionKey, getAmount(), response.transactionResponse.transId);
        }

        public ANetApiResponse TestVoid()
        {
            var response = (createTransactionResponse)AuthorizeCreditCard.Run(apiLoginId, transactionKey, getAmount());
            return VoidTransaction.Run(apiLoginId, transactionKey, response.transactionResponse.transId);
        }

        public ANetApiResponse TestCreditBankAccount()
        {
            return CreditBankAccount.Run(apiLoginId, transactionKey, TransactionID);
        }

        public ANetApiResponse TestChargeCustomerProfile()
        {
            var response = (createCustomerProfileResponse)CreateCustomerProfile.Run(apiLoginId, transactionKey, getEmail());
            var paymentProfileResponse = (createCustomerPaymentProfileResponse)CreateCustomerPaymentProfile.Run(apiLoginId, transactionKey, response.customerProfileId);
            var chargeResponse = ChargeCustomerProfile.Run(apiLoginId, transactionKey,
                response.customerProfileId, paymentProfileResponse.customerPaymentProfileId);
            DeleteCustomerProfile.Run(apiLoginId, transactionKey, response.customerProfileId);

            return chargeResponse;
        }
                    
        public ANetApiResponse TestPayPalVoid()
        {
            var response = (createTransactionResponse)PayPalAuthorizeCapture.Run(apiLoginId, transactionKey, getAmount());
            return PayPalVoid.Run(apiLoginId, transactionKey, response.transactionResponse.transId);
        }  

        public ANetApiResponse TestPayPalAuthorizeCapture()
        {
            return PayPalAuthorizeCapture.Run(apiLoginId, transactionKey, getAmount());
        }

        public ANetApiResponse TestPayPalAuthorizeCaptureContinue()
        {
            var response = (createTransactionResponse)PayPalAuthorizeCapture.Run(apiLoginId, transactionKey, getAmount());
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
            var response = (createTransactionResponse)PayPalAuthorizeCapture.Run(apiLoginId, transactionKey, getAmount());
            return PayPalGetDetails.Run(apiLoginId, transactionKey, response.transactionResponse.transId);
        }

        public ANetApiResponse TestPayPalPriorAuthorizationCapture()
        {
            var response = (createTransactionResponse)PayPalAuthorizeCapture.Run(apiLoginId, transactionKey, getAmount());
            return PayPalPriorAuthorizationCapture.Run(apiLoginId, transactionKey, response.transactionResponse.transId);
        }

        public ANetApiResponse TestCancelSubscription()
        {
            var response = (ARBCreateSubscriptionResponse)CreateSubscription.Run(apiLoginId, transactionKey, getMonth());
            return CancelSubscription.Run(apiLoginId, transactionKey, response.subscriptionId);
        }
                    
        public ANetApiResponse TestCreateSubscription()
        {
            var response = (ARBCreateSubscriptionResponse)CreateSubscription.Run(apiLoginId, transactionKey, getMonth());
            CancelSubscription.Run(apiLoginId, transactionKey, response.subscriptionId);

            return response;
        }
                    
        public ANetApiResponse TestGetSubscriptionStatus()
        {
            var response = (ARBCreateSubscriptionResponse)CreateSubscription.Run(apiLoginId, transactionKey, getMonth());
            var subscriptionResponse = GetSubscriptionStatus.Run(apiLoginId, transactionKey, response.subscriptionId);
            

            return subscriptionResponse;
        }

        public ANetApiResponse TestGetSubscription()
        {
            var response = (ARBCreateSubscriptionResponse)CreateSubscription.Run(apiLoginId, transactionKey, getMonth());
            var getResponse = GetSubscription.Run(apiLoginId, transactionKey, response.subscriptionId);
            CancelSubscription.Run(apiLoginId, transactionKey, response.subscriptionId);
            return getResponse;
        }

        public ANetApiResponse TestUpdateSubscription()
        {
            var response = (ARBCreateSubscriptionResponse)CreateSubscription.Run(apiLoginId, transactionKey, getMonth());
            var updateResponse = UpdateSubscription.Run(apiLoginId, transactionKey, response.subscriptionId);
            CancelSubscription.Run(apiLoginId, transactionKey, response.subscriptionId);

            return updateResponse;
        }

        public ANetApiResponse TestCreateCustomerProfile()
        {
            return CreateCustomerProfile.Run(apiLoginId, transactionKey, getEmail());
        }
                    
        public ANetApiResponse TestCreateCustomerPaymentProfile()
        {
            var response = (createCustomerProfileResponse)CreateCustomerProfile.Run(apiLoginId, transactionKey, getEmail());
            return CreateCustomerPaymentProfile.Run(apiLoginId, transactionKey, response.customerProfileId);
        }
    }
}
