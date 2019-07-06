using System;
using System.Threading.Tasks;
using TelegramBot.Api.Contracts.StateComponents;
using TelegramBot.Domain.Contracts.Adapters;
using TelegramBot.Domain.Contracts.Repositories;

namespace TelegramBot.Api.StateComponents
{
    public class StateMachineManager : IStateMachineManager
    {
        #region Fields

        private const string StateMachineTable = "StateMachineTable";

        private readonly ICacheAdapter _cache;
        private readonly ITablesRepository _tables;
        private readonly ITablesUsagePolicy _tablesUsagePolicy;

        #endregion Fields

        #region Constructors

        public StateMachineManager(
            ICacheAdapter cacheAdapter,
            ITablesRepository tablesRepository,
            ITablesUsagePolicy tablesUsagePolicy)
        {
            _cache = cacheAdapter ?? throw new ArgumentNullException(nameof(cacheAdapter));
            _tables = tablesRepository ?? throw new ArgumentNullException(nameof(tablesRepository));
            _tablesUsagePolicy = tablesUsagePolicy ?? throw new ArgumentNullException(nameof(tablesUsagePolicy));
        }

        #endregion Constructors

        #region Methods

        public async Task<IStateMachine> GetStateMachine(string senderUniqueKey)
        {
            if (senderUniqueKey == null)
            {
                throw new ArgumentNullException(nameof(senderUniqueKey));
            }

            var stateMachine = _cache.Get<IStateMachine>(senderUniqueKey);

            if (stateMachine == null && _tablesUsagePolicy.IsAvailable)
            {
                stateMachine = await _tables.GetAsync<IStateMachine>(StateMachineTable, senderUniqueKey);
            }

            return stateMachine;
        }

        public async Task SetStateMachine(string senderUniqueKey, IStateMachine stateMachine)
        {
            if (senderUniqueKey == null)
            {
                throw new ArgumentNullException(nameof(senderUniqueKey));
            }

            _cache.Set(senderUniqueKey, stateMachine);

            if (_tablesUsagePolicy.IsAvailable)
            {
                await _tables.SetAsync(StateMachineTable, senderUniqueKey, stateMachine);
            }
        }

        public async Task DeleteStateMachine(string senderUniqueKey)
        {
            if (senderUniqueKey == null)
            {
                throw new ArgumentNullException(nameof(senderUniqueKey));
            }

            _cache.Delete(senderUniqueKey);

            if (_tablesUsagePolicy.IsAvailable)
            {
                await _tables.DeleteAsync<IStateMachine>(StateMachineTable, senderUniqueKey);
            }
        }

        #endregion Methods
    }
}
