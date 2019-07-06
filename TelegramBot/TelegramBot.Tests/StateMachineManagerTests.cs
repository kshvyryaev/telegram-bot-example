using System;
using System.Threading.Tasks;
using Moq;
using NUnit.Framework;
using TelegramBot.Api.Contracts.StateComponents;
using TelegramBot.Domain.Contracts.Adapters;
using TelegramBot.Domain.Contracts.Repositories;
using TelegramBot.Tests.Builders;

namespace TelegramBot.Tests
{
    [TestFixture]
    public class StateMachineManagerTests
    {
        #region GetStateMachine tests

        [Test]
        public void GetStateMachine_KeyIsNull_ThrowException()
        {
            // Arrange
            var stateMachineManager = new StateMachineManagerBuilder()
                .SetDefault()
                .Build();

            // Assert
            Assert.ThrowsAsync<ArgumentNullException>(() =>
                stateMachineManager.GetStateMachine(null));
        }

        [Test]
        public void GetStateMachine_CacheReturnsValue_ReturnValue()
        {
            // Arrange
            var builder = new StateMachineManagerBuilder()
                .SetDefault();

            var manager = builder.Build();

            // Act
            var stateMachineTask = manager.GetStateMachine("key");
            stateMachineTask.Wait();
            var stateMachine = stateMachineTask.Result;

            // Assert
            Assert.IsNotNull(stateMachine);
            builder.CacheAdapterMock
                .Verify(x => x.Get<IStateMachine>(It.IsAny<string>()), Times.Once);
            builder.TablesUsagePolicyMock
                .Verify(x => x.IsAvailable, Times.Never);
            builder.TablesRepositoryMock
                .Verify(x => x.GetAsync<IStateMachine>(
                    It.IsAny<string>(),
                    It.IsAny<string>()), Times.Never);
        }

        [Test]
        public void GetStateMachine_CacheReturnsNull_TablesAreNotAvailable_ReturnNull()
        {
            // Arrange
            var cacheAdapterMock = new Mock<ICacheAdapter>();
            cacheAdapterMock
                .Setup(x => x.Get<IStateMachine>(It.IsAny<string>()))
                .Returns<IStateMachine>(null);

            var tablesUsagePolicyMock = new Mock<ITablesUsagePolicy>();
            tablesUsagePolicyMock
                .Setup(x => x.IsAvailable)
                .Returns(false);

            var builder = new StateMachineManagerBuilder()
                .SetDefault()
                .SetCacheAdapterMock(cacheAdapterMock)
                .SetTablesUsagePolicyMock(tablesUsagePolicyMock);

            var manager = builder.Build();

            // Act
            var stateMachineTask = manager.GetStateMachine("key");
            stateMachineTask.Wait();
            var stateMachine = stateMachineTask.Result;

            // Assert
            Assert.IsNull(stateMachine);
            builder.CacheAdapterMock
                .Verify(x => x.Get<IStateMachine>(It.IsAny<string>()), Times.Once);
            builder.TablesUsagePolicyMock
                .Verify(x => x.IsAvailable, Times.Once);
            builder.TablesRepositoryMock
                .Verify(x => x.GetAsync<IStateMachine>(
                    It.IsAny<string>(),
                    It.IsAny<string>()), Times.Never);
        }

        [Test]
        public void GetStateMachine_CacheReturnsNull_TablesReturnsStateMachine_ReturnStateMachine()
        {
            // Arrange
            var cacheAdapterMock = new Mock<ICacheAdapter>();
            cacheAdapterMock
                .Setup(x => x.Get<IStateMachine>(It.IsAny<string>()))
                .Returns<IStateMachine>(null);

            var builder = new StateMachineManagerBuilder()
                .SetDefault()
                .SetCacheAdapterMock(cacheAdapterMock);

            var manager = builder.Build();

            // Act
            var stateMachineTask = manager.GetStateMachine("key");
            stateMachineTask.Wait();
            var stateMachine = stateMachineTask.Result;

            // Assert
            Assert.IsNotNull(stateMachine);
            builder.CacheAdapterMock
                .Verify(x => x.Get<IStateMachine>(It.IsAny<string>()), Times.Once);
            builder.CacheAdapterMock
                .Verify(x => x.Set(
                    It.IsAny<string>(),
                    It.IsAny<IStateMachine>(),
                    It.IsAny<TimeSpan>()), Times.Once);
            builder.TablesUsagePolicyMock
                .Verify(x => x.IsAvailable, Times.Once);
            builder.TablesRepositoryMock
                .Verify(x => x.GetAsync<IStateMachine>(
                    It.IsAny<string>(),
                    It.IsAny<string>()), Times.Once);
        }

        [Test]
        public void GetStateMachine_CacheReturnsNull_TablesReturnsNull_ReturnNull()
        {
            // Arrange
            var cacheAdapterMock = new Mock<ICacheAdapter>();
            cacheAdapterMock
                .Setup(x => x.Get<IStateMachine>(It.IsAny<string>()))
                .Returns<IStateMachine>(null);

            var tablesRepositoryMock = new Mock<ITablesRepository>();
            tablesRepositoryMock
                .Setup(x => x.GetAsync<IStateMachine>(
                    It.IsAny<string>(),
                    It.IsAny<string>()))
                .Returns(Task.Factory.StartNew<IStateMachine>(() => null));

            var builder = new StateMachineManagerBuilder()
                .SetDefault()
                .SetCacheAdapterMock(cacheAdapterMock)
                .SetTablesRepositoryMock(tablesRepositoryMock);

            var manager = builder.Build();

            // Act
            var stateMachineTask = manager.GetStateMachine("key");
            stateMachineTask.Wait();
            var stateMachine = stateMachineTask.Result;

            // Assert
            Assert.IsNull(stateMachine);
            builder.CacheAdapterMock
                .Verify(x => x.Get<IStateMachine>(It.IsAny<string>()), Times.Once);
            builder.TablesUsagePolicyMock
                .Verify(x => x.IsAvailable, Times.Once);
            builder.TablesRepositoryMock
                .Verify(x => x.GetAsync<IStateMachine>(
                    It.IsAny<string>(),
                    It.IsAny<string>()), Times.Once);
        }

        #endregion GetStateMachine tests

        #region SetStateMachine tests

        [Test]
        public void SetStateMachine_KeyIsNull_ThrowException()
        {
            // Arrange
            var stateMachineManager = new StateMachineManagerBuilder()
                .SetDefault()
                .Build();

            // Assert
            Assert.ThrowsAsync<ArgumentNullException>(() =>
                stateMachineManager.SetStateMachine(null, null));
        }

        [Test]
        public void SetStateMachine_AllIsWell()
        {
            // Arrange
            var builder = new StateMachineManagerBuilder()
                .SetDefault();

            var manager = builder.Build();

            // Act
            manager.SetStateMachine("key", null).Wait();

            // Assert
            builder.CacheAdapterMock
                .Verify(x => x.Set(
                    It.IsAny<string>(),
                    It.IsAny<IStateMachine>(),
                    It.IsAny<TimeSpan?>()), Times.Once);
            builder.TablesRepositoryMock
                .Verify(x => x.SetAsync(
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<IStateMachine>()), Times.Once);
            builder.TablesUsagePolicyMock
                .Verify(x => x.IsAvailable, Times.Once);
        }

        [Test]
        public void SetStateMachine_TablesAreNotAvailable_DoNotSetValueToTables()
        {
            // Arrange
            var tablesUsagePolicyMock = new Mock<ITablesUsagePolicy>();
            tablesUsagePolicyMock
                .Setup(x => x.IsAvailable)
                .Returns(false);

            var builder = new StateMachineManagerBuilder()
                .SetDefault()
                .SetTablesUsagePolicyMock(tablesUsagePolicyMock);

            var manager = builder.Build();

            // Act
            manager.SetStateMachine("key", null).Wait();

            // Assert
            builder.CacheAdapterMock
                .Verify(x => x.Set(
                    It.IsAny<string>(),
                    It.IsAny<IStateMachine>(),
                    It.IsAny<TimeSpan?>()), Times.Once);
            builder.TablesRepositoryMock
                .Verify(x => x.SetAsync(
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<IStateMachine>()), Times.Never);
            builder.TablesUsagePolicyMock
                .Verify(x => x.IsAvailable, Times.Once);
        }

        #endregion SetStateMachine tests

        #region DeleteStateMachine tests

        [Test]
        public void DeleteStateMachine_KeyIsNull_ThrowException()
        {
            // Arrange
            var stateMachineManager = new StateMachineManagerBuilder()
                .SetDefault()
                .Build();

            // Assert
            Assert.ThrowsAsync<ArgumentNullException>(() =>
                stateMachineManager.DeleteStateMachine(null));
        }

        [Test]
        public void DeleteStateMachine_AllIsWell()
        {
            // Arrange
            var builder = new StateMachineManagerBuilder()
                .SetDefault();

            var manager = builder.Build();

            // Act
            manager.DeleteStateMachine("key").Wait();

            // Assert
            builder.CacheAdapterMock
                .Verify(x => x.Delete(It.IsAny<string>()), Times.Once);
            builder.TablesRepositoryMock
                .Verify(x => x.DeleteAsync<IStateMachine>(
                    It.IsAny<string>(),
                    It.IsAny<string>()), Times.Once);
            builder.TablesUsagePolicyMock
                .Verify(x => x.IsAvailable, Times.Once);
        }

        [Test]
        public void DeleteStateMachine_TablesAreNotAvailable_DoNotDeleteValueFromTables()
        {
            // Arrange
            var tablesUsagePolicyMock = new Mock<ITablesUsagePolicy>();
            tablesUsagePolicyMock
                .Setup(x => x.IsAvailable)
                .Returns(false);

            var builder = new StateMachineManagerBuilder()
                .SetDefault()
                .SetTablesUsagePolicyMock(tablesUsagePolicyMock);

            var manager = builder.Build();

            // Act
            manager.DeleteStateMachine("key").Wait();

            // Assert
            builder.CacheAdapterMock
                .Verify(x => x.Delete(It.IsAny<string>()), Times.Once);
            builder.TablesRepositoryMock
                .Verify(x => x.DeleteAsync<IStateMachine>(
                    It.IsAny<string>(),
                    It.IsAny<string>()), Times.Never);
            builder.TablesUsagePolicyMock
                .Verify(x => x.IsAvailable, Times.Once);
        }

        #endregion DeleteStateMachine tests
    }
}
