using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace net.authorize.sample
{
    class SampleCode
    {
        static void Main(string[] args)
        {
            if (args.Length != 1)
            {
                ShowUsage();
                return;
            }

            switch(args[0])
            {
                case "VisaCheckoutDecrypt" :
                    VisaCheckoutDecrypt.Run();
                    break;
                case "VisaCheckoutTransaction":
                    VisaCheckoutTransaction.Run();
                    break;
                default:
                    ShowUsage();
                    break;
            }

            Console.ReadLine();
           
        }

        private static void ShowUsage()
        {
            Console.WriteLine("Usage : SampleCode <CodeSampleName>");
            Console.WriteLine("");
            Console.WriteLine("Code Sample Names: ");
            Console.WriteLine("");
            Console.WriteLine("         VisaCheckoutDecrypt");
            Console.WriteLine("         VisaCheckoutTransaction");
            Console.ReadLine();
        }
    }
}
