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
    public class DeviceService : BaseService {

        public const string DeviceCollectionName = "DeviceCollection";

        public DeviceService(DocumentClient client) {
            this.client = client;
        }

        public async Task DeviceProcessing() {

            await this.CreateDocumentCollectionIfNotExists(Database, DeviceCollectionName);

            List<Device> sampleDevices = CreateDeviceEntity();
            await this.DeleteDeviceQuery(Database, DeviceCollectionName);
            await this.CreateDeviceDocumentIfNotExists(Database, DeviceCollectionName, sampleDevices);

            _currentDevices = sampleDevices;
        }

        private async Task DeleteDeviceQuery(string databaseName, string collectionName) {
            // Set some common query options
            FeedOptions queryOptions = new FeedOptions { MaxItemCount = -1 };

            // Here we find the Andersen family via its LastName
            IQueryable<Device> deviceQuery = this.client.CreateDocumentQuery<Device>(
                    UriFactory.CreateDocumentCollectionUri(databaseName, collectionName), queryOptions)
                    .Where(f => f.CustomerId == _previousCustomerId);

            // The query is executed synchronously here, but can also be executed asynchronously via the IDocumentQuery<T> interface
            Console.WriteLine("Running LINQ query for Assets...");
            foreach (Device device in deviceQuery) {
                await this.DeleteDeviceDocument(Database, collectionName, device.Id);
                //Console.WriteLine("\tRead {0}", device);
            }
        }
        private async Task DeleteDeviceDocument(string databaseName, string collectionName, string documentName) {
            try {
                await this.client.DeleteDocumentAsync(UriFactory.CreateDocumentUri(databaseName, collectionName, documentName));
                Console.WriteLine("Deleted Device {0}", documentName);
            }
            catch (DocumentClientException de) {
                if (de.StatusCode != HttpStatusCode.NotFound)
                    throw;
            }
        }
        private async Task CreateDeviceDocumentIfNotExists(string databaseName, string collectionName, List<Device> devices) {

            foreach (Device device in devices) {
                try {

                    await this.client.ReadDocumentAsync(UriFactory.CreateDocumentUri(databaseName, collectionName, device.Id));
                    this.WriteToConsoleAndPromptToContinue("Found {0}", device.Id);
                }
                catch (DocumentClientException de) {
                    if (de.StatusCode == HttpStatusCode.NotFound) {
                        await this.client.CreateDocumentAsync(UriFactory.CreateDocumentCollectionUri(databaseName, collectionName), device);
                        this.WriteToConsoleAndPromptToContinue("Created Device {0}", device.Id);
                    }
                    else {
                        throw;
                    }
                }
            }
        }

        public List<Device> CreateDeviceEntity() {

            List<Device> sampleDevices = new List<Device>();

            Device sampleDevice = new Device {
                Id = Guid.NewGuid().ToString(),
                CustomerId = _currentCustomer.Id,
                DeviceType = "Tag",
                DeviceName = "TagABC",
                TagType = "Tag Type 1",
                DeviceId = "Device Id 001",
                AssetId = null,
                Created = DateTime.Now,
                CreatedBy = "Rob.Wenc"
            };

            sampleDevices.Add(sampleDevice);

            sampleDevice = new Device {
                Id = Guid.NewGuid().ToString(),
                CustomerId = _currentCustomer.Id,
                DeviceType = "Tag",
                DeviceName = "TagDEF",
                TagType = "Tag Type 2",
                DeviceId = "Device Id 002",
                AssetId = null,
                Created = DateTime.Now,
                CreatedBy = "Rob.Wenc"
            };

            sampleDevices.Add(sampleDevice);

            sampleDevice = new Device {
                Id = Guid.NewGuid().ToString(),
                CustomerId = _currentCustomer.Id,
                DeviceType = "MicroZone",
                DeviceName = "mZone123",
                TagType = "mZone Type 1",
                DeviceId = "Device Id 003",
                AssetId = Guid.NewGuid().ToString(),
                Created = DateTime.Now,
                CreatedBy = "Martin.Wodka"
            };

            sampleDevices.Add(sampleDevice);

            sampleDevice = new Device {
                Id = Guid.NewGuid().ToString(),
                CustomerId = _currentCustomer.Id,
                DeviceType = "Gateway",
                DeviceName = "Gateway 1241",
                TagType = "Gateway Type 1",
                DeviceId = "Device Id 004",
                AssetId = null,
                Created = DateTime.Now,
                CreatedBy = "Martin.Wodka"
            };

            sampleDevices.Add(sampleDevice);

            sampleDevice = new Device {
                Id = Guid.NewGuid().ToString(),
                CustomerId = _currentCustomer.Id,
                DeviceType = "Gateway",
                DeviceName = "Gateway 3112",
                TagType = "Gateway Type 2",
                DeviceId = "Device Id 005",
                AssetId = Guid.NewGuid().ToString(),
                Created = DateTime.Now,
                CreatedBy = "Martin.Wodka"
            };

            sampleDevices.Add(sampleDevice);

            sampleDevice = new Device {
                Id = Guid.NewGuid().ToString(),
                CustomerId = _currentCustomer.Id,
                DeviceType = "Tag",
                DeviceName = "Tag 3094387",
                TagType = "Tag Type 4",
                DeviceId = "Device Id 006",
                AssetId = Guid.NewGuid().ToString(),
                Created = DateTime.Now,
                CreatedBy = "John.Lechowicz"
            };

            sampleDevices.Add(sampleDevice);

            sampleDevice = new Device {
                Id = Guid.NewGuid().ToString(),
                CustomerId = _currentCustomer.Id,
                DeviceType = "Tag",
                DeviceName = "Tag 31750",
                TagType = "Tag Type 3",
                DeviceId = "Device Id 007",
                AssetId = Guid.NewGuid().ToString(),
                Created = DateTime.Now,
                CreatedBy = "John.Lechowicz"
            };

            sampleDevices.Add(sampleDevice);

            return sampleDevices;
        }
    }
}
