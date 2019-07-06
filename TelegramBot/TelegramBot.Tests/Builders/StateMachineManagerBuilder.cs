using System;
using System.Threading.Tasks;
using Moq;
using TelegramBot.Api.Contracts.StateComponents;
using TelegramBot.Api.StateComponents;
using TelegramBot.Domain.Contracts.Adapters;
using TelegramBot.Domain.Contracts.Repositories;

namespace TelegramBot.Tests.Builders
{
    internal class StateMachineManagerBuilder
    {
        #region Constructors

        public StateMachineManagerBuilder()
        {
            SetDefault();
        }

        #endregion Constructors

        #region Properties

        public Mock<ICacheAdapter> CacheAdapterMock { get; private set; }

        public Mock<ITablesRepository> TablesRepositoryMock { get; private set; }

        public Mock<ITablesUsagePolicy> TablesUsagePolicyMock { get; private set; }

        #endregion Properties

        #region Methods

        internal StateMachineManagerBuilder SetDefault()
        {
            SetDefaultCacheAdapterMock();
            SetDefaultTablesRepositoryMock();
            SetDefaultTablesUsagePolicyMock();
            return this;
        }

        internal StateMachineManagerBuilder SetCacheAdapterMock(Mock<ICacheAdapter> mock)
        {
            CacheAdapterMock = mock ?? throw new ArgumentNullException(nameof(mock));
            return this;
        }

        internal StateMachineManagerBuilder SetTablesRepositoryMock(Mock<ITablesRepository> mock)
        {
            TablesRepositoryMock = mock ?? throw new ArgumentNullException(nameof(mock));
            return this;
        }

        internal StateMachineManagerBuilder SetTablesUsagePolicyMock(Mock<ITablesUsagePolicy> mock)
        {
            TablesUsagePolicyMock = mock ?? throw new ArgumentNullException(nameof(mock));
            return this;
        }

        internal IStateMachineManager Build()
        {
            return new StateMachineManager(
                CacheAdapterMock.Object,
                TablesRepositoryMock.Object,
                TablesUsagePolicyMock.Object);
        }

        private void SetDefaultCacheAdapterMock()
        {
            var cacheAdapterMock = new Mock<ICacheAdapter>();

            cacheAdapterMock
                .Setup(x => x.Get<IStateMachine>(It.IsAny<string>()))
                .Returns(new StateMachine());

            cacheAdapterMock
                .Setup(x => x.Set(
                    It.IsAny<string>(),
                    It.IsAny<IStateMachine>(),
                    It.IsAny<TimeSpan?>()));

            cacheAdapterMock
                .Setup(x => x.Delete(It.IsAny<string>()));
            cacheAdapterMock
                .Setup(x => x.Clear());

            CacheAdapterMock = cacheAdapterMock;
        }

        private void SetDefaultTablesRepositoryMock()
        {
            var tablesRepositoryMock = new Mock<ITablesRepository>();

            tablesRepositoryMock
                .Setup(x => x.GetAsync<IStateMachine>(
                    It.IsAny<string>(),
                    It.IsAny<string>()))
                .Returns(Task.Factory.StartNew<IStateMachine>(() => new StateMachine()));

            tablesRepositoryMock
                .Setup(x => x.SetAsync(
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<IStateMachine>()));

            tablesRepositoryMock
                .Setup(x => x.DeleteAsync<IStateMachine>(
                    It.IsAny<string>(),
                    It.IsAny<string>()));

            TablesRepositoryMock = tablesRepositoryMock;
        }

        private void SetDefaultTablesUsagePolicyMock()
        {
            var tablesUsagePolicyMock = new Mock<ITablesUsagePolicy>();

            tablesUsagePolicyMock
                .Setup(x => x.IsAvailable)
                .Returns(true);

            TablesUsagePolicyMock = tablesUsagePolicyMock;
        }

        #endregion Methods
    }
}
