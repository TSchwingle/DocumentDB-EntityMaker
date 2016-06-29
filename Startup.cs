using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Net;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using EntityMaker.Services;

namespace EntityMaker {
    public class Startup : BaseService {


        static void Main(string[] args) {

            try {
                Startup p = new Startup();
                p.GetStartedDemo().Wait();
            }
            catch (DocumentClientException de) {
                Exception baseException = de.GetBaseException();
                Console.WriteLine("{0} error occurred: {1}, Message: {2}", de.StatusCode, de.Message, baseException.Message);
            }
            catch (Exception e) {
                Exception baseException = e.GetBaseException();
                Console.WriteLine("Error: {0}, Message: {1}", e.Message, baseException.Message);
            }
            finally {
                Console.WriteLine("End of Vypin Modeling, press any key to exit.");
                Console.ReadKey();
            }
        }

        private async Task GetStartedDemo() {

            // Create Database, if it doesn't exist...
            this.client = new DocumentClient(new Uri(EndpointUri), PrimaryKey);
            await this.CreateDatabaseIfNotExists(Database);

            // Customer Related Processing...
            CustomerService customerService = new CustomerService(client);
            await customerService.CustomerProcessing();

            // User Related Processing...
            UserService userService = new UserService(client);
            await userService.UserProcessing();
            
            // Device Related Processing...
            DeviceService deviceService = new DeviceService(client);
            await deviceService.DeviceProcessing();

            // Asset Related Processing...
            AssetService assetService = new AssetService(client);
            await assetService.AssetProcessing();

        }
    }
}
