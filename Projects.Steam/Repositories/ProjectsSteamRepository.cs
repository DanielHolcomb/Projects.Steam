using Microsoft.Azure.Cosmos;
using Projects.Steam.Models;
using Projects.Steam.Repositories.Interfaces;

namespace Projects.Steam.Repositories
{
    public class ProjectsSteamRepository : IProjectsSteamRepository
    {
        private readonly Container _container;

        public ProjectsSteamRepository(CosmosClient client, string databaseName, string containerName)
        {
            _container = client.GetContainer(databaseName, containerName);
        }

        public async Task InsertSteamAppAsync(AppDetails appDetails)
        {
            await _container.CreateItemAsync(appDetails);
        }
    }
}
