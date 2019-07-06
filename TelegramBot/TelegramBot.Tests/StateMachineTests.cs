using System;
using Moq;
using NUnit.Framework;
using Telegram.Bot.Types;
using TelegramBot.Api.Contracts.Commands;
using TelegramBot.Api.Entities;
using TelegramBot.Api.StateComponents;

namespace TelegramBot.Tests
{
    [TestFixture]
    public class StateMachineTests
    {
        #region AddState tests

        [Test]
        public void AddState_AddNullState_ThrowException()
        {
            // Arrange
            var stateMachine = new StateMachine();

            // Assert
            Assert.Throws<ArgumentNullException>(() =>
                stateMachine.AddState(null));
        }

        [Test]
        public void AddState_AddState_StateWasAdded()
        {
            // Arrange
            var commandMock = new Mock<ICommand>();
            var state = new State(new Update(), commandMock.Object);

            var stateMachine = new StateMachine();

            // Act
            stateMachine.AddState(state);

            // Assert
            Assert.AreEqual(1, stateMachine.States.Count);
        }

        #endregion AddState tests

        #region ReturnToPreviousState tests

        [Test]
        public void ReturnToPreviousState_StatesIsEmpty_ActiveCommandIsNotNull_NoActions()
        {
            // Arrange
            var commandMock = new Mock<ICommand>();

            var stateMachine = new StateMachine
            {
                ActiveCommand = commandMock.Object
            };

            // Act
            stateMachine.ReturnToPreviousState();

            // Assert
            Assert.IsNull(stateMachine.ActiveCommand);
        }

        [Test]
        public void ReturnToPreviousState_StatesContainsOneState_ActiveCommandIsNotNull_ReturnToPrevState()
        {
            // Arrange
            var commandMock = new Mock<ICommand>();
            var state = new State(new Update(), commandMock.Object);

            var stateMachine = new StateMachine();
            stateMachine.AddState(state);
            stateMachine.ActiveCommand = commandMock.Object;

            // Act
            stateMachine.ReturnToPreviousState();

            // Assert
            Assert.AreEqual(0, stateMachine.States.Count);
            Assert.IsNull(stateMachine.ActiveCommand);
        }

        [Test]
        public void ReturnToPreviousState_StatesContainsTwoStates_ActiveCommandIsNotNull_ReturnToPrevState()
        {
            // Arrange
            var commandMock = new Mock<ICommand>();
            var state = new State(new Update(), commandMock.Object);

            var stateMachine = new StateMachine();
            stateMachine.AddState(state);
            stateMachine.AddState(state);
            stateMachine.ActiveCommand = commandMock.Object;

            // Act
            stateMachine.ReturnToPreviousState();

            // Assert
            Assert.AreEqual(1, stateMachine.States.Count);
            Assert.AreEqual(commandMock.Object, stateMachine.ActiveCommand);
        }

        #endregion ReturnToPreviousState tests

        #region ResetState tests

        [Test]
        public void ResetState_AllIsDefault()
        {
            // Arrange
            var commandMock = new Mock<ICommand>();
            var state = new State(new Update(), commandMock.Object);

            var stateMachine = new StateMachine();
            stateMachine.AddState(state);
            stateMachine.ActiveCommand = commandMock.Object;

            // Act
            stateMachine.ResetState();

            // Assert
            Assert.AreEqual(0, stateMachine.States.Count);
            Assert.IsNull(stateMachine.ActiveCommand);
        }

        #endregion ResetState tests
    }
}
