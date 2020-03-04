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
    public class CreateVisaSrcTransaction
    {
        public static ANetApiResponse Run(String ApiLoginID, String ApiTransactionKey)
        {
            Console.WriteLine("Running VisaCheckoutTransaction Sample ...");
            // The test setup.
            ApiOperationBase<ANetApiRequest, ANetApiResponse>.MerchantAuthentication = new merchantAuthenticationType()
            {
                name = ApiLoginID,
                ItemElementName = ItemChoiceType.transactionKey,
                Item = ApiTransactionKey,
            };

            ApiOperationBase<ANetApiRequest, ANetApiResponse>.RunEnvironment = AuthorizeNet.Environment.SANDBOX;


            //set up data based on transaction
            var transactionAmount = Convert.ToDecimal(25.60);
            var opaqueDataType = new opaqueDataType
            {
                dataDescriptor = "COMMON.VCO.ONLINE.PAYMENT",
                dataKey = "NQzcMISSxLX789w+CGX+tXi3lKntO1dpZbZaREOUprVRByJkg1xnpc2Wx9aT5/BLOxQmHqmIsjjy+tF6HqKKGwovvXjIS3fE3y3tBRNbz8D7y6vYMup+AWbEvZqDEBSi",
                dataValue = "+6hn53rUcggeZZti2IdBp3qNLa9ohAH87cFSc1BggZFNEpsrfdJbRViWwv/JbCNkHkOD6CpFlRO3gCDH2VEQTd8laqWR1ccHiZpdYDnOxfhUQpU9E18ZByW7j17puVWogh7HaItbDUL0YvIxxfClX9bohurOo1JHyUgBO9YxTj3CLY2RdRkjmipAQqOyxiGX9enFQjAHdPgKj2RxnVMYe8on5ei94zbtYUbI3fXrp3I+DJcZCGZ4SzrlnPAPpcn20qaIoaOTX/xuD+voRAUKb/KE5oy+CuSNBtyMBgrvWU0Lf3SLjGfE/FJx3Bh9/LABCwWBYQvtpo3DQkDItp8P5/3EOz7JwBFbFd9UQs8wm/J8YvJMd3Kf4MkQ1+KYyg17RH6OAcoNaqQxT3MjOSvVv3KAlKV82ZDco+IRTVPcjyVd/Vff0qDIqes08fPCQDhttefl/bh18urrmCnM9PcP7xJ0A8Ek7LRMLF19c81O7IIaEn0FXxq+UuV5oZArY+mE4GD08xizyd0hoW9pvsdZ7RkuPu4yK1yXPTAKbc3vTxrj0kamFWd4kRHapwLxcvawIQzrlQGQj5AUFkpEg1o1UGWz0vtGgqE08hplJehsTZwPw9KSaA+u5M79gXM3uLR8g2RlE5cEDRLy3aEv0ufeag+lt8zME9wzrfTK8zhjTdBGIAJqSUYto1JbiW9IEMJgjLaqEJhwO49pNlUgOJVp7BXO9JoHPM8PyS+vZlOCX6b0bip/+mCEok09L9B0IhnLjs95Q4kDZfCcQNfDIdLEPe5tLp8eGaSkK3HDoQFbZKCFyGmUTEEF19PScZURbYuGrwpxHqqAAhU87ZmdhRRdJbMTrWPhIvk9/kRzRIP2ciKu8ClNZIJ9azafIUBo7WdlYs+6QbLCn8UCNvYczrLXo3tGhVvHPheWWgmuxDYbHDyJu7SIPKgVvi6LrPYgg8g+I0pMWPojWmBdp85tMt+sQrWk2x325/pOrYXj8fc2W8PHtOAka1EltTdZiNsRKzA8orzLQrtvqtxhzgXMSTOEmosEAxA7DuQdKscL2BWWmiYsAOCNYQxtm/nR6PBAKZ5PDS6Wjk73hKTOeB6kA5E/H1ij15iJqNK6O1+4b4gpJHnHm7tccVQIK5w1EeLR1waqO2G4FMM5FoyA1WsSEQURQncDek0bK1ohcu73l5FLiq8he/H6gF3dEsKL6Z367ki59HKwnnJXfWj/WOUZxTbIho4H5i/lIcc0vSgFH84ReTjjiANEm+ccl5PcFV9wEVlbGXiOiJfeZ0mEzo9ghDyKEAGEDzZtHDwZzKEYyT92oFXkeewGQ6DJX7GSQPZ1MW17jAQODAqmzJcmwMunc7PwGvJcscRbxkXpGe7/asq2H4POz3ByBrBQHCl/+oUVtw8hEbavCpuEgXfWl09Sc3Dfg69UP8XBR80vWsP1YL8YtBxOmL2hinZc5SJZ4boulAOHiMQyKBwwkg2D1gJDEY9JzUJtbg3p06swB0UthNmVuo/1mV8047sB5QrjGCugEo87+vh9eV1EVvyLZLRFS+RIZoIpLR3UkO6Pe1s7MnO7ZvCsbz9sKNb0GtQPoFtU9b7KaCHgQ20vL7xjqTEmR2QwkHEriuGJ8a7oMdSd88w/e2InL1SfHCnS2JeWY9vY6RSTvmjkEf3BFGhHjFP5QRR3Bd8AVH/1YrFcxtSSJP5GeY3CVnJgjZToK+ngxsRzpDcEm6pz3RPUEIBNkk3c9plpdlMbyvuVVKXLSFdTdtAALRfiD9qhdpgGMqboZ0kXi+qn3irYXT19q8oQktoZ4ILkbzewloftLbfUTqQprA8cddy7/ikUKKhBoBVs/DAupRe9aRP3TLgIEz6eNNilZszXoFfUv9EgqOZ0EBb83KNV3HvbE2xGJcTArjRpmzszQQkNOpJnyRDtvPj7FU7K16UYQ9zQBrxnx5vnoWSaqNhzOxikd+hWZ6i5G7EPpCO+utdoMdyOTOoDAjBmiy5JsDHSVv4zvkT9ySYPH2PmS47mMEpZICKTAxuDrm3zTpT064P+7ivcGmaIyaBCkc4udIHaWSbi5XJ/ciXUxSqAtqaVcd5HD++6vjBzKhbAPU4shSBav6qCSp/XqFurEAJbkLB3VmXe7bghcM+VNPJHHiYlIdzndDaFENyaZCukypggK0Gf4cH+8CKI9YnQx9s4JMs4lX57i4IkkoJE7fjWaOHyxYM/AiKvWlMQtRO8Y5Yta454JfHVq7Mg11Wqu2Ex4q5QLNqKudVt3wveu3G1zoNFanW6i+d0Aa3hTdxerl9BacX/"
            };

            //standard api call to retrieve response
            var paymentType = new paymentType { Item = opaqueDataType };
            var transactionRequest = new transactionRequestType
            {
                transactionType = transactionTypeEnum.authCaptureTransaction.ToString(),
                payment = paymentType,
                amount = transactionAmount,
                callId = "1482912778237697701"
            };
            var request = new createTransactionRequest { transactionRequest = transactionRequest };
            var controller = new createTransactionController(request);
            controller.Execute();
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