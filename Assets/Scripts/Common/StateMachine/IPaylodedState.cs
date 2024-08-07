using Cysharp.Threading.Tasks;

namespace Common.StateMachine
{
    public interface IPaylodedState<TPayload> : IExitableState
    {
        UniTask Enter(TPayload payload);
    }
}