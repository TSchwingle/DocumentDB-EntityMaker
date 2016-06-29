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
    public class UserService : BaseService {

        public const string UserCollectionName = "UserCollection";
        public UserService(DocumentClient _client) {
            this.client = _client;
        }

        public async Task UserProcessing() {

            await this.CreateDocumentCollectionIfNotExists(Database, UserCollectionName);

            List<Models.User> sampleUsers = CreateUserEntity();
            await this.DeleteUserQuery(Database, UserCollectionName);
            await this.CreateUserDocumentIfNotExists(Database, UserCollectionName, sampleUsers);
        }

        private async Task DeleteUserQuery(string databaseName, string collectionName) {
            // Set some common query options
            FeedOptions queryOptions = new FeedOptions { MaxItemCount = -1 };

            // Here we find the Andersen family via its LastName
            IQueryable<Models.User> userQuery = this.client.CreateDocumentQuery<Models.User>(
                    UriFactory.CreateDocumentCollectionUri(databaseName, collectionName), queryOptions)
                    .Where(f => f.CustomerId == _previousCustomerId);

            // The query is executed synchronously here, but can also be executed asynchronously via the IDocumentQuery<T> interface
            Console.WriteLine("Running LINQ query...");
            foreach (Models.User user in userQuery) {
                await this.DeleteUserDocument(Database, collectionName, user.Id);
                //Console.WriteLine("\tRead {0}", user);
            }
        }
        private async Task DeleteUserDocument(string databaseName, string collectionName, string documentName) {
            try {
                await this.client.DeleteDocumentAsync(UriFactory.CreateDocumentUri(databaseName, collectionName, documentName));
                Console.WriteLine("Deleted User {0}", documentName);
            }
            catch (DocumentClientException de) {
                if (de.StatusCode != HttpStatusCode.NotFound)
                    throw;
            }
        }
        private async Task CreateUserDocumentIfNotExists(string databaseName, string collectionName, List<Models.User> users) {

            foreach (Models.User user in users) {
                try {

                    await this.client.ReadDocumentAsync(UriFactory.CreateDocumentUri(databaseName, collectionName, user.Id));
                    this.WriteToConsoleAndPromptToContinue("Found {0}", user.Id);
                }
                catch (DocumentClientException de) {
                    if (de.StatusCode == HttpStatusCode.NotFound) {
                        await this.client.CreateDocumentAsync(UriFactory.CreateDocumentCollectionUri(databaseName, collectionName), user);
                        this.WriteToConsoleAndPromptToContinue("Created User {0}", user.Id);
                    }
                    else {
                        throw;
                    }
                }
            }
        }
        
        public List<Models.User> CreateUserEntity() {

            Models.User sampleUser = new Models.User {
                Id = Guid.NewGuid().ToString(),
                CustomerId = _currentCustomer.Id,
                UserName = "Jeff.Luckett",
                FirstName = "Jeff",
                LastName = "Luckett",
                EmailAddress = "jluckett@atadex.com",
                LastSignInDateTime = DateTime.Now.AddMinutes(-13),
                RegistrationStatus = "active",      // inactive, pending approval
                RegistrationDateTime = DateTime.Now.AddDays(-25),
                ApprovedByAccount = "SysAdmin",
                Created = DateTime.Now,
                CreatedBy = "Rob.Wenc",

                Phones = new Phone[] {
                    new Phone {
                        PhoneType = "Cell",
                        PhoneNumber = "(312) 555-1234",
                        isPreferred = false
                    },
                    new Phone {
                        PhoneType = "Office",
                        PhoneNumber = "(708) 555-4321",
                        isPreferred = true
                    }
                },
                LocationIds = new UserToLocation[] {
                    new UserToLocation {
                        LocationId = _currentCustomer.Locations[0].LocationId,
                        Roles = new UserRole[] {
                            new UserRole {
                                Role = "Admin"
                            },
                            new UserRole {
                                Role = "Yard"
                            }
                        }
                    },
                    new UserToLocation {
                        LocationId = _currentCustomer.Locations[1].LocationId,
                        Roles = new UserRole[] {
                            new UserRole {
                                Role = "Admin"
                            }
                        }
                    }
                }
            };

            List<Models.User> sampleUsers = new List<Models.User>();
            sampleUsers.Add(sampleUser);


            // User #2...
            sampleUser = new Models.User {
                Id = Guid.NewGuid().ToString(),
                CustomerId = _currentCustomer.Id,
                UserName = "John.Lechowicz",
                FirstName = "Jeff",
                LastName = "Lechowicz",
                EmailAddress = "john.Lechowicz@spr.com",
                LastSignInDateTime = DateTime.Now.AddMinutes(-9),
                RegistrationStatus = "active",      // inactive, pending approval
                RegistrationDateTime = DateTime.Now.AddDays(-5),
                ApprovedByAccount = "Jeff.Luckett",
                Created = DateTime.Now,
                CreatedBy = "Rob.Wenc",
                Phones = new Phone[] {
                    new Phone {
                        PhoneType = "Cell",
                        PhoneNumber = "(312) 555-2222",
                        isPreferred = true
                    }
                },
                LocationIds = new UserToLocation[] {
                    new UserToLocation {
                        LocationId = _currentCustomer.Locations[0].LocationId,
                        Roles = new UserRole[] {
                            new UserRole {
                                Role = "Developer"
                            }
                        }
                    }
                }
            };

            sampleUsers.Add(sampleUser);
            return sampleUsers;
        }
    }
}
