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
    public class AssetService : BaseService {

        public const string AssetCollectionName = "AssetCollection";

        public AssetService(DocumentClient client) {
            this.client = client;
        }

        public async Task AssetProcessing() {

            await this.CreateDocumentCollectionIfNotExists(Database, AssetCollectionName);

            List<Asset> sampleAssets = CreateAssetEntity();
            await this.DeleteAssetQuery(Database, AssetCollectionName);
            await this.CreateAssetDocumentIfNotExists(Database, AssetCollectionName, sampleAssets);

            _currentAssets = sampleAssets;
        }


        private async Task DeleteAssetQuery(string databaseName, string collectionName) {
            // Set some common query options
            FeedOptions queryOptions = new FeedOptions { MaxItemCount = -1 };

            // Here we find the Andersen family via its LastName
            IQueryable<Asset> assetQuery = this.client.CreateDocumentQuery<Asset>(
                    UriFactory.CreateDocumentCollectionUri(databaseName, collectionName), queryOptions)
                    .Where(f => f.CustomerId == _previousCustomerId);

            // The query is executed synchronously here, but can also be executed asynchronously via the IDocumentQuery<T> interface
            Console.WriteLine("Running LINQ query for Assets...");
            foreach (Asset asset in assetQuery) {
                await this.DeleteAssetDocument(Database, collectionName, asset.Id);
                //Console.WriteLine("\tRead {0}", asset);
            }
        }
        private async Task DeleteAssetDocument(string databaseName, string collectionName, string documentName) {
            try {
                await this.client.DeleteDocumentAsync(UriFactory.CreateDocumentUri(databaseName, collectionName, documentName));
                Console.WriteLine("Deleted Asset {0}", documentName);
            }
            catch (DocumentClientException de) {
                if (de.StatusCode != HttpStatusCode.NotFound)
                    throw;
            }
        }
        private async Task CreateAssetDocumentIfNotExists(string databaseName, string collectionName, List<Asset> assets) {

            foreach (Asset asset in assets) {
                try {

                    await this.client.ReadDocumentAsync(UriFactory.CreateDocumentUri(databaseName, collectionName, asset.Id));
                    this.WriteToConsoleAndPromptToContinue("Found {0}", asset.Id);
                }
                catch (DocumentClientException de) {
                    if (de.StatusCode == HttpStatusCode.NotFound) {
                        await this.client.CreateDocumentAsync(UriFactory.CreateDocumentCollectionUri(databaseName, collectionName), asset);
                        this.WriteToConsoleAndPromptToContinue("Created Asset {0}", asset.Id);
                    }
                    else {
                        throw;
                    }
                }
            }
        }

        public List<Asset> CreateAssetEntity() {

            List<Asset> sampleAssets = new List<Asset>();

            Asset sampleAsset = new Asset {
                Id = Guid.NewGuid().ToString(),
                CustomerId = _currentCustomer.Id,
                LocationId = _currentCustomer.Locations[0].LocationId,
                TagId = _currentDevices[0].Id,
                TagType = "Tag",
                AssetId = "3198381321",
                AssetName = "Container1",
                HealthScore = 10,
                BatteryStatus = "Good",
                RSSI = 4.2M,
                Temperature = 24.40M,
                AssetStatus = "Stationary",
                mZones = new string[] { "mZone2", "mZone1", "mZone3" },
                Created = DateTime.Now,
                CreatedBy = "Jeff.Luckett"
            };
            
            sampleAssets.Add(sampleAsset);

            sampleAsset = new Asset {
                Id = Guid.NewGuid().ToString(),
                CustomerId = _currentCustomer.Id,
                LocationId = _currentCustomer.Locations[1].LocationId,
                TagId = _currentDevices[6].Id,
                TagType = "Tag",
                AssetId = "34424321",
                AssetName = "Container2",
                HealthScore = 5,
                BatteryStatus = "Poor",
                RSSI = 3.4M,
                Temperature = 24.40M,
                AssetStatus = "Stationary",
                mZones = new string[] { "mZone3", "mZone2", "mZone3" },
                Created = DateTime.Now,
                CreatedBy = "Todd.Schwingle"
            };

            sampleAssets.Add(sampleAsset);

            sampleAsset = new Asset {
                Id = Guid.NewGuid().ToString(),
                CustomerId = _currentCustomer.Id,
                LocationId = _currentCustomer.Locations[1].LocationId,
                TagId = _currentDevices[1].Id,
                TagType = "Tag",
                AssetId = "313131318",
                AssetName = "Container3",
                HealthScore = 8,
                BatteryStatus = "ReplaceASAP",
                RSSI = 4.0M,
                Temperature = 23.13M,
                AssetStatus = "In Transit",
                mZones = new string[] { "mZone2", "mZone1", "mZone3" },
                Created = DateTime.Now,
                CreatedBy = "Luis.Feliu"
            };

            sampleAssets.Add(sampleAsset);

            sampleAsset = new Asset {
                Id = Guid.NewGuid().ToString(),
                CustomerId = _currentCustomer.Id,
                LocationId = _currentCustomer.Locations[1].LocationId,
                TagId = _currentDevices[5].Id,
                TagType = "Tag",
                AssetId = "31317823242",
                AssetName = "Container4",
                HealthScore = 9,
                BatteryStatus = "Excellent",
                RSSI = 4.1M,
                Temperature = 24.50M,
                AssetStatus = "Stationary",
                mZones = new string[] { "mZone1", "mZone2", "mZone3" },
                Created = DateTime.Now,
                CreatedBy = "John.Lechowicz"
            };

            sampleAssets.Add(sampleAsset);

            sampleAsset = new Asset {
                Id = Guid.NewGuid().ToString(),
                CustomerId = _currentCustomer.Id,
                LocationId = null,
                TagId = null,
                TagType = null,
                AssetId = "33234199",
                AssetName = "Container5",
                HealthScore = 10,
                BatteryStatus = "OK",
                RSSI = 3.6M,
                Temperature = 21.25M,
                AssetStatus = "Stationary",
                mZones = new string[] { "mZone4", "mZone2", "mZone3" },
                Created = DateTime.Now,
                CreatedBy = "John.Lechowicz"
            };

            sampleAssets.Add(sampleAsset);

            return sampleAssets;
        }
    }
}
