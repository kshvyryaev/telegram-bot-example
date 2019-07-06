using System.Threading.Tasks;

namespace TelegramBot.Api.Contracts.StateComponents
{
    public interface IStateMachineManager
    {
        Task<IStateMachine> GetStateMachine(string senderUniqueKey);

        Task SetStateMachine(string senderUniqueKey, IStateMachine stateMachine);

        Task DeleteStateMachine(string senderUniqueKey);
    }
}
