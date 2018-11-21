using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AuthorizeNet.Api.Contracts.V1;
using AuthorizeNet.Api.Controllers;
using AuthorizeNet.Api.Controllers.Bases;

namespace net.authorize.sample
{
    public class UpdateCustomerShippingAddress
    {
        public static ANetApiResponse Run(String ApiLoginID, String ApiTransactionKey, string customerProfileID, string customerAddressId)
        {
            Console.WriteLine("Update customer shipping address sample");

            ApiOperationBase<ANetApiRequest, ANetApiResponse>.RunEnvironment = AuthorizeNet.Environment.SANDBOX;
            // define the merchant information (authentication / transaction id)
            ApiOperationBase<ANetApiRequest, ANetApiResponse>.MerchantAuthentication = new merchantAuthenticationType()
            {
                name = ApiLoginID,
                ItemElementName = ItemChoiceType.transactionKey,
                Item = ApiTransactionKey,
            };

            var creditCard = new creditCardType
             {
                 cardNumber = "4111111111111111",
                 expirationDate = "1028"
             };

            var paymentType = new paymentType { Item = creditCard };

            var address = new customerAddressExType
            {
                firstName = "Newfirstname",
                lastName = "Doe",
                address = "123 Main St.",
                city = "Bellevue",
                state = "WA",
                zip = "98004",
                country = "USA",
                phoneNumber = "000-000-000",
                customerAddressId = customerAddressId
            };

            var request = new updateCustomerShippingAddressRequest();
            request.customerProfileId = customerProfileID;
            request.address = address;


            // instantiate the controller that will call the service
            var controller = new updateCustomerShippingAddressController(request);
            controller.Execute();

            // get the response from the service (errors contained if any)
            var response = controller.GetApiResponse();

            if (response != null && response.messages.resultCode == messageTypeEnum.Ok)
            {
                Console.WriteLine(response.messages.message[0].text);
            }
            else if(response != null)
            {
                Console.WriteLine("Error: " + response.messages.message[0].code + "  " +
                                  response.messages.message[0].text);
            }

            return response;
        }
    }
}
