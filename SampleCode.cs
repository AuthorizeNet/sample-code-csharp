using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            Console.WriteLine("    VisaCheckoutDecrypt");
            Console.WriteLine("    VisaCheckoutTransaction");
            Console.WriteLine("    ChargeCreditCard");
            Console.WriteLine("    CaptureOnly");
            Console.WriteLine("    AuthorizeCreditCard");
            Console.WriteLine("    CapturePreviouslyAuthorizedAmount");
            Console.WriteLine("    Refund");
            Console.WriteLine("    Void");
            Console.WriteLine("    DebitBankAccount");
            Console.WriteLine("    CreditBankAccount");
            Console.WriteLine("    ChargeTokenizedCard");
            Console.WriteLine("    PayPalVoid");
            Console.WriteLine("    PayPalAuthorizeCapture");
            Console.WriteLine("    PayPalAuthorizeCaptureContinue");
            Console.WriteLine("    PayPalAuthorizeOnly");
            Console.WriteLine("    PayPalCredit");
            Console.WriteLine("    PayPalGetDetails");
            Console.WriteLine("    PayPalPriorAuthorizationCapture");
        }

        private static void RunMethod(String methodName)
        {
            // These are default transaction keys.
            // You can create your own keys in seconds by signing up for a sandbox account here: https://developer.authorize.net/sandbox/
            String apiLoginId           = "5KP3u95bQpv";
            String transactionKey       = "4Ktq966gC55GAX7S";

            string TransactionAmount    = string.Empty;
            string TransactionID        = string.Empty;

            switch (methodName)
            {
                case "VisaCheckoutDecrypt":
                    VisaCheckoutDecrypt.Run(apiLoginId, transactionKey);
                    break;
                case "VisaCheckoutTransaction":
                    VisaCheckoutTransaction.Run(apiLoginId, transactionKey);
                    break;
                case "ChargeCreditCard":
                    ChargeCreditCard.Run(apiLoginId, transactionKey);
                    break;
                case "CaptureOnly":
                    CaptureOnly.Run(apiLoginId, transactionKey);
                    break;
                case "CapturePreviouslyAuthorizedAmount":
                    Console.WriteLine("Enter An Transaction Amount");
                    TransactionAmount = Console.ReadLine();

                    Console.WriteLine("Enter An Transaction ID");
                    TransactionID = Console.ReadLine();

                    CapturePreviouslyAuthorizedAmount.Run(apiLoginId, transactionKey, Convert.ToDecimal( TransactionAmount ), TransactionID);
                    break;
                case "AuthorizeCreditCard":
                    AuthorizeCreditCard.Run(apiLoginId, transactionKey);
                    break;
                case "Refund":
                    Console.WriteLine("Enter An Transaction Amount");
                    TransactionAmount = Console.ReadLine();

                    Console.WriteLine("Enter An Transaction ID");
                    TransactionID = Console.ReadLine();

                    RefundTransaction.Run(apiLoginId, transactionKey, Convert.ToDecimal( TransactionAmount ), TransactionID);
                    break;
                case "Void":
                    Console.WriteLine("Enter An Transaction ID");
                    TransactionID = Console.ReadLine();

                    VoidTransaction.Run(apiLoginId, transactionKey, TransactionID);
                    break;
                case "DebitBankAccount":
                    DebitBankAccount.Run(apiLoginId, transactionKey);
                    break;
                case "CreditBankAccount":
                    Console.WriteLine("Enter An Transaction ID");
                    TransactionID = Console.ReadLine();

                    CreditBankAccount.Run(apiLoginId, transactionKey, TransactionID);
                    break;
                case "ChargeTokenizedCard":
                    ChargeTokenizedCreditCard.Run(apiLoginId, transactionKey);
                    break;
                case "PayPalVoid":
                    PayPalVoid.Run(apiLoginId, transactionKey, TransactionID);
                    break;
                case "PayPalAuthorizeCapture":
                    PayPalAuthorizeCapture.Run(apiLoginId, transactionKey);
                    break;
                case "PayPalAuthorizeCaptureContinue":
                    PayPalAuthorizeCaptureContinue.Run(apiLoginId, transactionKey, TransactionID);
                    break;
                case "PayPalAuthorizeOnly":
                    PayPalAuthorizeOnly.Run(apiLoginId, transactionKey);
                    break;
                case "PayPalAuthorizeOnlyContinue":
                    PayPalAuthorizeCaptureContinue.Run(apiLoginId, transactionKey, TransactionID);
                    break;
                case "PayPalCredit":
                    PayPalCredit.Run(apiLoginId, transactionKey, TransactionID);
                    break;
                case "PayPalGetDetails":
                    PayPalGetDetails.Run(apiLoginId, transactionKey, TransactionID);
                    break;
                case "PayPalPriorAuthorizationCapture":
                    PayPalPriorAuthorizationCapture.Run(apiLoginId, transactionKey, TransactionID);
                    break;
                default:
                    ShowUsage();
                    break;
            }
        }

    }
}
