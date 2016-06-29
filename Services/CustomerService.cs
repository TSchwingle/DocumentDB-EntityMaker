using EntityMaker.Models;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace EntityMaker.Services {
    public class CustomerService : BaseService {

        public const string CustomerCollectionName = "CustomerCollection";

        public CustomerService(DocumentClient client ) {
            this.client = client;
        }
        public async Task CustomerProcessing() {

            await this.CreateDocumentCollectionIfNotExists(Database, CustomerCollectionName);

            Customer sampleCustomer = CreateCustomerEntity();
            await this.DeleteCustomerQuery(Database, CustomerCollectionName);
            await this.CreateCustomerDocumentIfNotExists(Database, CustomerCollectionName, sampleCustomer);

            _currentCustomer = sampleCustomer;
        }

        private async Task CreateCustomerDocumentIfNotExists(string databaseName, string collectionName, Customer customer) {
            try {
                await this.client.ReadDocumentAsync(UriFactory.CreateDocumentUri(databaseName, collectionName, customer.Id));
                this.WriteToConsoleAndPromptToContinue("Found {0}", customer.Id);
            }
            catch (DocumentClientException de) {
                if (de.StatusCode == HttpStatusCode.NotFound) {
                    await this.client.CreateDocumentAsync(UriFactory.CreateDocumentCollectionUri(databaseName, collectionName), customer);
                    this.WriteToConsoleAndPromptToContinue("Created Customer {0}", customer.Id);
                }
                else {
                    throw;
                }
            }
        }

        private async Task DeleteCustomerQuery(string databaseName, string collectionName) {
            // Set some common query options
            FeedOptions queryOptions = new FeedOptions { MaxItemCount = -1 };

            // Here we find the Andersen family via its LastName
            IQueryable<Customer> customerQuery = this.client.CreateDocumentQuery<Customer>(
                    UriFactory.CreateDocumentCollectionUri(databaseName, collectionName), queryOptions)
                    .Where(f => f.CompanyCode == "CompanyCode_001");

            // The query is executed synchronously here, but can also be executed asynchronously via the IDocumentQuery<T> interface
            Console.WriteLine("Running LINQ query...");
            foreach (Customer customer in customerQuery) {
                _previousCustomerId = customer.Id;
                await this.DeleteCustomerDocument(Database, collectionName, customer.Id);
                //Console.WriteLine("\tRead {0}", customer);
            }
        }

        private async Task DeleteCustomerDocument(string databaseName, string collectionName, string documentName) {
            try {
                await this.client.DeleteDocumentAsync(UriFactory.CreateDocumentUri(databaseName, collectionName, documentName));
                Console.WriteLine("Deleted Customer {0}", documentName);
            }
            catch (DocumentClientException de) {
                if (de.StatusCode != HttpStatusCode.NotFound)
                    throw;
            }
        }

        public Customer CreateCustomerEntity() {

            // ADD THIS PART TO YOUR CODE
            Customer sampleCustomer = new Customer {
                Id = Guid.NewGuid().ToString(),
                CompanyCode = "CompanyCode_001",
                UserName = "SampleUserName",
                CompanyName = "SampleCompanyName",
                Created = DateTime.Now,
                CreatedBy = "Jeff.Luckett",

                Locations = new Location[] {

                    new Location {
                        LocationId = Guid.NewGuid().ToString(),
                        LocationName = "Willis Tower",
                        Address1 = "233 S Wacker Drive",
                        Address2 = "Suite 3500",
                        City = "Chicago",
                        State = "IL",
                        ZipCode = "60606",
                        Latitude = 41.87886,
                        Longitude = -87.635837,
                        IsActive = true
                    },
                    new Location {
                        LocationId = Guid.NewGuid().ToString(),
                        LocationName = "Sample Location 2",
                        Address1 = "60 N Hamilton St",
                        Address2 = null,
                        City = "Poughkeepsie",
                        State = "NY",
                        ZipCode = "12601",
                        Latitude = 41.706062,
                        Longitude = -73.921878,
                        IsActive = true
                    }
                },

                UserDefinedFields = new UserDefinedField[] {
                    new UserDefinedField {
                        EntityName = "FEIN",
                        EntityType = "string",
                        EntityValue = "36-3692993"
                    },
                    new Models.UserDefinedField {
                        EntityName = "Industry",
                        EntityType = "string",
                        EntityValue = "Shipping"
                    }
                },

                Notes = new Models.Note[] {
                    new Models.Note {
                        NoteType = "Internal",
                        Text = "This Company is one of our core customers on the Vypin Architecture",
                        Added = DateTime.Now
                    },
                    new Models.Note {
                        NoteType = "Dashboard",
                        Text = "Thanks for signing up for our Service, feel free to Contact Luis at (312) 555-1234 if you have any questions",
                        Added = DateTime.Now
                    },
                    new Models.Note {
                        NoteType = "Informational",
                        Text = "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. ",
                        Added = DateTime.Now
                    }
                },
                HeadQuarters = new HeadQuarter {
                    Address1 = "100 Washington Ave",
                    Address2 = null,
                    City = "St. Louis",
                    State = "MO",
                    ZipCode = "63102",
                    Phone = "(312) 555-9876 x5",
                    Latitude = 38.624691,
                    Longitude = -90.184776                    
                }
            };

            return sampleCustomer;
        }

    }
}
