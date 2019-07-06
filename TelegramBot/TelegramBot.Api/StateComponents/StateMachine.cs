using System;
using System.Collections.Generic;
using System.Linq;
using TelegramBot.Api.Contracts.Commands;
using TelegramBot.Api.Contracts.StateComponents;
using TelegramBot.Api.Entities;

namespace TelegramBot.Api.StateComponents
{
    public class StateMachine : IStateMachine
    {
        private readonly List<State> _states;

        public StateMachine()
        {
            _states = new List<State>();
        }

        public IReadOnlyCollection<State> States => _states.AsReadOnly();

        public ICommand ActiveCommand { get; set; }

        public void AddState(State state)
        {
            if (state == null)
            {
                throw new ArgumentNullException(nameof(state));
            }

            _states.Add(state);
        }

        public void ReturnToPreviousState()
        {
            State lastState = _states.LastOrDefault();

            if (lastState != null)
            {
                _states.Remove(lastState);

                if (_states.Count == 0)
                {
                    ActiveCommand = default(ICommand);
                }
                else
                {
                    ActiveCommand = lastState.Command;
                }
            }
            else
            {
                ActiveCommand = default(ICommand);
            }
        }

        public void ResetState()
        {
            _states.Clear();
            ActiveCommand = default(ICommand);
        }
    }
}
