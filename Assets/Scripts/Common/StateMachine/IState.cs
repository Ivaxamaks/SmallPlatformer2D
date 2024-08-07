using Cysharp.Threading.Tasks;

namespace Common.StateMachine
{
    public interface IState : IExitableState
    {
        UniTask Enter();
    }
}