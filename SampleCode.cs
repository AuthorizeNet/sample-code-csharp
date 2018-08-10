using net.authorize.sample.MobileInappTransactions;
using net.authorize.sample.PaymentTransactions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using net.authorize.sample.CustomerProfiles;

namespace net.authorize.sample
{
    //===============================================================================================
    //
    //  NOTE:  If you add a new sample update the following methods here:
    //
    //              ShowMethods()   -  Add the method name
    //              RunMethod(String methodName)   -   Update the switch statement to run the method
    //
    //===============================================================================================
    class SampleCode
    {
        static void Main(string[] args)
        {
            if (args.Length == 0)
            {
                SelectMethod();
            }
            else if (args.Length == 1)
            {
                RunMethod(args[0]);
                return;
            }
            else
            {
                ShowUsage();
            }

            Console.WriteLine("");
            Console.Write("Press <Return> to finish ...");
            Console.ReadLine();
           
        }

        private static void ShowUsage()
        {
            Console.WriteLine("Usage : SampleCode [CodeSampleName]");
            Console.WriteLine("");
            Console.WriteLine("Run with no parameter to select a method.  Otherwise pass a method name.");
            Console.WriteLine("");
            Console.WriteLine("Code Sample Names: ");
            ShowMethods();
            Console.WriteLine("Code Sample Names: ");

        }

        private static void SelectMethod()
        {
            Console.WriteLine("Code Sample Names: ");
            Console.WriteLine("");
            ShowMethods();
            Console.WriteLine("");
            Console.Write("Type a sample name & then press <Return> : ");
            RunMethod(Console.ReadLine());
        }

        private static void ShowMethods()
        {
            
            Console.WriteLine("    ChargeCreditCard");
            Console.WriteLine("    AuthorizeCreditCard");
            Console.WriteLine("    CapturePreviouslyAuthorizedAmount");
            Console.WriteLine("    CaptureFundsAuthorizedThroughAnotherChannel");
            Console.WriteLine("    Refund");
            Console.WriteLine("    Void");
            Console.WriteLine("    DebitBankAccount");
            Console.WriteLine("    CreditBankAccount");
            Console.WriteLine("    ChargeCustomerProfile");
            Console.WriteLine("    ChargeTokenizedCard");
            Console.WriteLine("    ChargeEncryptedTrackData");
            Console.WriteLine("    ChargeTrackData");
            Console.WriteLine("    CreateAnApplePayTransaction");
            Console.WriteLine("    CreateAnAndroidPayTransaction");
            Console.WriteLine("    CreateAnAcceptTransaction");
            Console.WriteLine("    DecryptVisaCheckoutData");
            Console.WriteLine("    CreateVisaCheckoutTransaction");
            Console.WriteLine("    PayPalVoid");
            Console.WriteLine("    PayPalAuthorizeCapture");
            Console.WriteLine("    PayPalAuthorizeCaptureContinued");
            Console.WriteLine("    PayPalAuthorizeOnly");
            Console.WriteLine("    PayPalAuthorizeOnlyContinued");
            Console.WriteLine("    PayPalCredit");
            Console.WriteLine("    PayPalGetDetails");
            Console.WriteLine("    PayPalPriorAuthorizationCapture");
            Console.WriteLine("    CancelSubscription");
            Console.WriteLine("    CreateSubscription");
            Console.WriteLine("    CreateSubscriptionFromCustomerProfile");
            Console.WriteLine("    GetListOfSubscriptions");
            Console.WriteLine("    GetSubscriptionStatus");
            Console.WriteLine("    GetSubscription");
            Console.WriteLine("    GetUnsettledTransactionList");
            Console.WriteLine("    UpdateSubscription");
            Console.WriteLine("    CreateCustomerProfile");
            Console.WriteLine("    CreateCustomerPaymentProfile");
            Console.WriteLine("    CreateCustomerShippingAddress");
            Console.WriteLine("    DeleteCustomerProfile");
            Console.WriteLine("    DeleteCustomerPaymentProfile");
            Console.WriteLine("    DeleteCustomerShippingAddress");
            Console.WriteLine("    ValidateCustomerPaymentProfile");
            Console.WriteLine("    UpdateCustomerShippingAddress");
            Console.WriteLine("    UpdateCustomerProfile");
            Console.WriteLine("    UpdateCustomerPaymentProfile");
            Console.WriteLine("    GetCustomerShippingAddress");
            Console.WriteLine("    GetCustomerProfileId");
            Console.WriteLine("    GetCustomerProfile");
            Console.WriteLine("    GetAcceptCustomerProfilePage");
            Console.WriteLine("    GetCustomerPaymentProfile");
            Console.WriteLine("    GetCustomerPaymentProfileList");
            Console.WriteLine("    DeleteCustomerShippingAddress");
            Console.WriteLine("    DeleteCustomerProfile");
            Console.WriteLine("    DeleteCustomerPaymentProfile");
            Console.WriteLine("    CreateCustomerShippingAddress");
            Console.WriteLine("    CreateCustomerProfileFromTransaction");
            Console.WriteLine("    GetBatchStatistics");
            Console.WriteLine("    GetSettledBatchList");
            Console.WriteLine("    GetTransactionDetails");
            Console.WriteLine("    GetTransactionList");
            Console.WriteLine("    UpdateSplitTenderGroup");
            Console.WriteLine("    GetHeldTransactionList");
            Console.WriteLine("    ApproveOrDeclineHeldTransaction");
            Console.WriteLine("    GetMerchantDetails");
            Console.WriteLine("    GetAnAcceptPaymentPage");
            Console.WriteLine("    GetCustomerProfileTransactionList");
            Console.WriteLine("    GetAccountUpdaterJobSummary");
        }

        private static void RunMethod(String methodName)
        {
            // These are default transaction keys.
            // You can create your own keys in seconds by signing up for a sandbox account here: https://developer.authorize.net/sandbox/
            const string apiLoginId = "5KP3u95bQpv";
            const string transactionKey = "346HZ32z3fP4hTG2";

            //Update TransactionID for which you want to run the sample code
            const string transactionId = "2249735976";

            //Update PayerID for which you want to run the sample code
            const string payerId = "M8R9JRNJ3R28Y";

            const string customerProfileId = "213213";
            const string customerPaymentProfileId = "2132345";
            const string shippingAddressId = "1223213";
            const decimal amount = 12.34m;
            const string subscriptionId = "1223213";
            const short day = 45;
            const string emailId = "test@test.com";

            switch (methodName)
            {
                case "ValidateCustomerPaymentProfile":
                    ValidateCustomerPaymentProfile.Run(apiLoginId, transactionKey, customerProfileId, customerPaymentProfileId);
                    break;
                case "UpdateCustomerShippingAddress":
                    UpdateCustomerShippingAddress.Run(apiLoginId, transactionKey, customerProfileId, shippingAddressId);
                    break;
                case "UpdateCustomerProfile":
                    UpdateCustomerProfile.Run(apiLoginId, transactionKey, customerProfileId);
                    break;
                case "UpdateCustomerPaymentProfile":
                    UpdateCustomerPaymentProfile.Run(apiLoginId, transactionKey, customerProfileId, customerPaymentProfileId);
                    break;
                case "GetCustomerShippingAddress":
                    GetCustomerShippingAddress.Run(apiLoginId, transactionKey, customerProfileId, shippingAddressId);
                    break;
                case "GetCustomerProfileIds":
                    GetCustomerProfileIds.Run(apiLoginId, transactionKey);
                    break;
                case "GetCustomerProfile":
                    GetCustomerProfile.Run(apiLoginId, transactionKey, customerProfileId);
                    break;
                case "GetAcceptCustomerProfilePage":
                    GetAcceptCustomerProfilePage.Run(apiLoginId, transactionKey, customerProfileId);
                    break;
                case "GetCustomerPaymentProfile":
                    GetCustomerPaymentProfile.Run(apiLoginId, transactionKey, customerProfileId, customerPaymentProfileId);
                    break;
                case "GetCustomerPaymentProfileList":
                    GetCustomerPaymentProfileList.Run(apiLoginId, transactionKey);
                    break;
                case "DeleteCustomerShippingAddress":
                    DeleteCustomerShippingAddress.Run(apiLoginId, transactionKey, customerProfileId, shippingAddressId);
                    break;
                case "DeleteCustomerProfile":
                    DeleteCustomerProfile.Run(apiLoginId, transactionKey, customerProfileId);
                    break;
                case "DeleteCustomerPaymentProfile":
                    DeleteCustomerPaymentProfile.Run(apiLoginId, transactionKey, customerProfileId, customerPaymentProfileId);
                    break;
                case "CreateCustomerShippingAddress":
                    CreateCustomerShippingAddress.Run(apiLoginId, transactionKey, customerProfileId);
                    break;
                case "CreateCustomerProfileFromTransaction":
                    CreateCustomerProfileFromTransaction.Run(apiLoginId, transactionKey, transactionId);
                    break;
                case "GetTransactionDetails":
                    GetTransactionDetails.Run(apiLoginId, transactionKey, transactionId);
                    break;
                case "GetTransactionList":
                    GetTransactionList.Run(apiLoginId, transactionKey);
                    break;
                case "CreateAnApplePayTransaction":
                    CreateAnApplePayTransaction.Run(apiLoginId, transactionKey, 12.23m);
                    break;
                case "CreateAnAndroidPayTransaction":
                    CreateAnAndroidPayTransaction.Run(apiLoginId, transactionKey, 12.23m);
                    break;
                case "CreateAnAcceptTransaction":
                    CreateAnAcceptTransaction.Run(apiLoginId, transactionKey, 12.23m);
                    break;
                case "DecryptVisaCheckoutData":
                    DecryptVisaCheckoutData.Run(apiLoginId, transactionKey);
                    break;
                case "CreateVisaCheckoutTransaction":
                    CreateVisaCheckoutTransaction.Run(apiLoginId, transactionKey);
                    break;
                case "ChargeCreditCard":
                    ChargeCreditCard.Run(apiLoginId, transactionKey, amount);
                    break;
                case "ChargeEncryptedTrackData":
                    ChargeEncryptedTrackData.Run(apiLoginId, transactionKey, amount);
                    break;
                case "ChargeTrackData":
                    ChargeTrackData.Run(apiLoginId, transactionKey, amount);
                    break;
                case "CapturePreviouslyAuthorizedAmount":
                    CapturePreviouslyAuthorizedAmount.Run(apiLoginId, transactionKey, amount, transactionId);
                    break;
                case "CaptureFundsAuthorizedThroughAnotherChannel":
                    CaptureFundsAuthorizedThroughAnotherChannel.Run(apiLoginId, transactionKey, amount);
                    break;
                case "AuthorizeCreditCard":
                    AuthorizeCreditCard.Run(apiLoginId, transactionKey, amount);
                    break;
                case "Refund":
                    RefundTransaction.Run(apiLoginId, transactionKey, amount, transactionId);
                    break;
                case "Void":
                    VoidTransaction.Run(apiLoginId, transactionKey, transactionId);
                    break;
                case "DebitBankAccount":
                    DebitBankAccount.Run(apiLoginId, transactionKey, amount);
                    break;
                case "CreditBankAccount":
                    CreditBankAccount.Run(apiLoginId, transactionKey, transactionId);
                    break;
                case "ChargeCustomerProfile":
                    ChargeCustomerProfile.Run(apiLoginId, transactionKey, customerProfileId, customerPaymentProfileId, amount);
                    break;
                case "ChargeTokenizedCard":
                    ChargeTokenizedCreditCard.Run(apiLoginId, transactionKey);
                    break;
                case "PayPalVoid":
                    PayPalVoid.Run(apiLoginId, transactionKey, transactionId);
                    break;
                case "PayPalAuthorizeCapture":
                    PayPalAuthorizeCapture.Run(apiLoginId, transactionKey, amount);
                    break;
                case "PayPalAuthorizeCaptureContinued":                    
                    PayPalAuthorizeCaptureContinued.Run(apiLoginId, transactionKey, transactionId, payerId);
                    break;
                case "PayPalAuthorizeOnly":
                    PayPalAuthorizeOnly.Run(apiLoginId, transactionKey, amount);
                    break;
                case "PayPalAuthorizeOnlyContinued":
                    PayPalAuthorizeOnlyContinued.Run(apiLoginId, transactionKey, transactionId, payerId);
                    break;
                case "PayPalCredit":
                    PayPalCredit.Run(apiLoginId, transactionKey, transactionId);
                    break;
                case "PayPalGetDetails":
                    PayPalGetDetails.Run(apiLoginId, transactionKey, transactionId);
                    break;
                case "PayPalPriorAuthorizationCapture":
                    PayPalPriorAuthorizationCapture.Run(apiLoginId, transactionKey, transactionId);
                    break;
                case "CancelSubscription":
                    CancelSubscription.Run(apiLoginId, transactionKey, subscriptionId);
                    break;
                case "CreateSubscription":
                    CreateSubscription.Run(apiLoginId, transactionKey, day);
                    break;
                case "CreateSubscriptionFromCustomerProfile":
                    CreateSubscriptionFromCustomerProfile.Run(apiLoginId, transactionKey, day, "12322","232321","123232");
                    break;
                case "GetListOfSubscriptions":
                    GetListOfSubscriptions.Run(apiLoginId, transactionKey);
                    break;
                case "GetSubscriptionStatus":
                    GetSubscriptionStatus.Run(apiLoginId, transactionKey, subscriptionId);
                    break;
                case "GetSubscription":
                    GetSubscription.Run(apiLoginId, transactionKey, subscriptionId);
                    break;
                case "UpdateSubscription":
                    UpdateSubscription.Run(apiLoginId, transactionKey, subscriptionId);
                    break;
                case "CreateCustomerProfile":
                    CreateCustomerProfile.Run(apiLoginId, transactionKey, emailId);
                    break;
                case "CreateCustomerPaymentProfile":
                    CreateCustomerPaymentProfile.Run(apiLoginId, transactionKey, customerProfileId);
                    break;
                case "GetUnsettledTransactionList":
                    GetUnsettledTransactionList.Run(apiLoginId, transactionKey);
                    break;
                case "GetBatchStatistics":
                    GetBatchStatistics.Run(apiLoginId, transactionKey);
                    break;
                case "GetSettledBatchList":
                    GetSettledBatchList.Run(apiLoginId,transactionKey);
                     break;
                case "UpdateSplitTenderGroup":
                     UpdateSplitTenderGroup.Run(apiLoginId, transactionKey);
                     break;
                case "GetHeldTransactionList":
                    GetHeldTransactionList.Run(apiLoginId, transactionKey);
                    break;
                case "ApproveOrDeclineHeldTransaction":
                    ApproveOrDeclineHeldTransaction.Run(apiLoginId, transactionKey);
                    break;
                case "GetMerchantDetails":
                     GetMerchantDetails.Run(apiLoginId, transactionKey);
                     break;
                case "GetAnAcceptPaymentPage":
                    GetAnAcceptPaymentPage.Run(apiLoginId, transactionKey, 12.23m);
                    break;
                case "GetCustomerProfileTransactionList":
                    GetCustomerProfileTransactionList.Run(apiLoginId, transactionKey, customerProfileId);
                    break;
                //case "GetAccountUpdaterJobSummary":
                //    GetAccountUpdaterJobSummary.Run(apiLoginId, transactionKey);
                //    break;
                default:
                    ShowUsage();
                    break;
            }
        }
    }
}
