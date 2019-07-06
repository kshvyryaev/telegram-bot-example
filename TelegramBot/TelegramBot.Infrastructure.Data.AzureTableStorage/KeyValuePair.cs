using Microsoft.WindowsAzure.Storage.Table;

namespace TelegramBot.Infrastructure.Data.AzureTableStorage
{
    internal class KeyValuePair<TValue> : TableEntity
        where TValue : class
    {
        public KeyValuePair() { }

        public KeyValuePair(string key, TValue value)
            : base(nameof(KeyValuePair<TValue>), key)
        {
            Value = value;
        }

        public TValue Value { get; set; }
    }
}
