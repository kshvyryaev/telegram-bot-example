using System;
using System.Collections.Concurrent;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using TelegramBot.Domain.Contracts.Repositories;

namespace TelegramBot.Infrastructure.Data.AzureTableStorage
{
    public class AzureTablesRepository : ITablesRepository
    {
        #region Fields

        private readonly CloudTableClient _client;
        private readonly ConcurrentDictionary<string, CloudTable> _tables;

        #endregion Fields

        #region Constructors

        public AzureTablesRepository(string connectionString)
        {
            if (string.IsNullOrWhiteSpace(connectionString))
            {
                throw new ArgumentNullException(nameof(connectionString));
            }

            if (!CloudStorageAccount.TryParse(connectionString, out CloudStorageAccount account))
            {
                throw new InvalidOperationException($"Cannot convert connection string {connectionString}.");
            }

            _client = account.CreateCloudTableClient();
            _tables = new ConcurrentDictionary<string, CloudTable>();
        }

        #endregion Constructors

        #region Methods

        public async Task SetAsync<TValue>(string tableName, string key, TValue value)
            where TValue : class
        {
            if (tableName == null)
            {
                throw new ArgumentNullException(nameof(tableName));
            }

            if (key == null)
            {
                throw new ArgumentNullException(nameof(key));
            }

            CloudTable table = await EnsureTableCreationAsync(tableName);

            var keyValuePair = new KeyValuePair<TValue>(key, value);
            var setOperation = TableOperation.InsertOrReplace(keyValuePair);
            await table.ExecuteAsync(setOperation);
        }

        public async Task<TValue> GetAsync<TValue>(string tableName, string key)
            where TValue : class
        {
            if (tableName == null)
            {
                throw new ArgumentNullException(nameof(tableName));
            }

            if (key == null)
            {
                throw new ArgumentNullException(nameof(key));
            }

            CloudTable table = await EnsureTableCreationAsync(tableName);

            var getOperation = TableOperation.Retrieve<KeyValuePair<TValue>>(nameof(KeyValuePair<TValue>), key);
            TableResult tableResult = await table.ExecuteAsync(getOperation);
            var keyValuePair = tableResult.Result as KeyValuePair<TValue>;

            return keyValuePair.Value;
        }

        public async Task DeleteAsync<TValue>(string tableName, string key)
            where TValue : class
        {
            if (tableName == null)
            {
                throw new ArgumentNullException(nameof(tableName));
            }

            if (key == null)
            {
                throw new ArgumentNullException(nameof(key));
            }

            CloudTable table = await EnsureTableCreationAsync(tableName);

            var getOperation = TableOperation.Retrieve<KeyValuePair<TValue>>(nameof(KeyValuePair<TValue>), key);
            TableResult tableResult = await table.ExecuteAsync(getOperation);
            var keyValuePair = tableResult.Result as KeyValuePair<TValue>;

            var deleteOperation = TableOperation.Delete(keyValuePair);
            await table.ExecuteAsync(deleteOperation);
        }

        private async Task<CloudTable> EnsureTableCreationAsync(string tableName)
        {
            if (!_tables.TryGetValue(tableName, out CloudTable table))
            {
                table = _client.GetTableReference(tableName);
                await table.CreateIfNotExistsAsync();
                _tables[tableName] = table;
            }

            return table;
        }

        #endregion Methods
    }
}
