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
    public partial class BaseService {
        
        public const string EndpointUri = "https://sprvypdemo.documents.azure.com:443/";
        public const string PrimaryKey = "ad2xbj65phCrUYc1EWWwhBCMTj1o7bRi2DUa83hawvdKj5w3xupIJOzmnuoG70aVF0g0TASyvzdvkIGD3JgDLQ==";
        public const string Database = "VypinDB";

        public DocumentClient client;

        // Saving some of the Entities for Processing...
        public static Customer _currentCustomer;
        public static List<Device> _currentDevices;
        public static List<Asset> _currentAssets;
        public static String _previousCustomerId;     // Used for Deleting Data from Related Entities...

        public async Task CreateDatabaseIfNotExists(string databaseName) {
            // Check to verify a database with the id=FamilyDB does not exist
            try {
                await this.client.ReadDatabaseAsync(UriFactory.CreateDatabaseUri(databaseName));
                this.WriteToConsoleAndPromptToContinue("Found {0}", databaseName);
            }
            catch (DocumentClientException de) {
                // If the database does not exist, create a new database
                if (de.StatusCode == HttpStatusCode.NotFound) {
                    await this.client.CreateDatabaseAsync(new Database { Id = databaseName });
                    this.WriteToConsoleAndPromptToContinue("Created {0}", databaseName);
                }
                else {
                    throw;
                }
            }
        }

        public async Task CreateDocumentCollectionIfNotExists(string databaseName, string collectionName) {
            try {
                await this.client.ReadDocumentCollectionAsync(UriFactory.CreateDocumentCollectionUri(databaseName, collectionName));
                this.WriteToConsoleAndPromptToContinue("Found {0}", collectionName);
            }
            catch (DocumentClientException de) {
                // If the document collection does not exist, create a new collection
                if (de.StatusCode == HttpStatusCode.NotFound) {
                    DocumentCollection collectionInfo = new DocumentCollection();
                    collectionInfo.Id = collectionName;

                    // Configure collections for maximum query flexibility including string range queries.
                    collectionInfo.IndexingPolicy = new IndexingPolicy(new RangeIndex(DataType.String) { Precision = -1 });

                    // Here we create a collection with 400 RU/s.
                    await this.client.CreateDocumentCollectionAsync(
                        UriFactory.CreateDatabaseUri(databaseName),
                        collectionInfo,
                        new RequestOptions { OfferThroughput = 400 });

                    this.WriteToConsoleAndPromptToContinue("Created {0}", collectionName);
                }
                else {
                    throw;
                }
            }
        }

        public void WriteToConsoleAndPromptToContinue(string format, params object[] args) {
            Console.WriteLine(format, args);
            //Console.WriteLine("Press any key to continue ...");
            //Console.ReadKey();
        }
    }
}
