using Common.StateMachine;
using Cysharp.Threading.Tasks;
using UI;

namespace States
{
    public class EndGameState : IState
    {
        private readonly UIController _uiController;
        private readonly LastGameResultProvider _resultProvider;
        private StateMachine _stateMachine;

        public EndGameState(UIController uiController,
            LastGameResultProvider resultProvider)
        {
            _uiController = uiController;
            _resultProvider = resultProvider;
        }
        
        public void Initialize(StateMachine stateMachine)
        {
            _stateMachine = stateMachine;
        }

        public UniTask Enter()
        {
            _uiController.ShowEndGamePanel(_resultProvider.IsWin, OnEndScreenPanelClosed);
            return UniTask.CompletedTask;
        }

        public UniTask Exit()
        {
            return UniTask.CompletedTask;
        }

        private async void OnEndScreenPanelClosed()
        {
            await _stateMachine.Enter<RunningGameState>();
        }
    }
}