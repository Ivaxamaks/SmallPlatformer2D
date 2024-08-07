using Cysharp.Threading.Tasks;

namespace Common.StateMachine
{
    public interface IExitableState
    {
        void Initialize(StateMachine stateMachine); 
        UniTask Exit();
    }
}