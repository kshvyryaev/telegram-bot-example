using System.Collections.Generic;
using TelegramBot.Api.Contracts.Commands;
using TelegramBot.Api.Entities;

namespace TelegramBot.Api.Contracts.StateComponents
{
    public interface IStateMachine
    {
        IReadOnlyCollection<State> States { get; }

        ICommand ActiveCommand { get; set; }

        void AddState(State state);

        void ReturnToPreviousState();

        void ResetState();
    }
}
