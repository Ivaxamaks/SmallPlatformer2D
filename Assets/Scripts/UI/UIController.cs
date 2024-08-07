using System;
using Events;
using UI.EndGamePanel;
using UI.TopUIPanel;
using UniTaskPubSub;

namespace UI
{
    public class UIController : IDisposable
    {
        private readonly EndGamePanelModel _endGamePanelModel;
        private readonly TopUIPanelModel _topUIPanelModel;
        private readonly AsyncMessageBus _asyncMessageBus;
        
        private IDisposable _disposable;

        public UIController(EndGamePanelModel endGamePanelModel,
            TopUIPanelModel topUIPanelModel,
            AsyncMessageBus asyncMessageBus)
        {
            _endGamePanelModel = endGamePanelModel;
            _topUIPanelModel = topUIPanelModel;
            _asyncMessageBus = asyncMessageBus;
        }

        public void Initialize()
        {
            _disposable = _asyncMessageBus.Subscribe<PlayerAmmoAmountChangedEvent>(OnPlayerAmmoCountChangedHandler);
        }

        private void OnPlayerAmmoCountChangedHandler(PlayerAmmoAmountChangedEvent eventData)
        {
            _topUIPanelModel.Show(eventData.AmmoCount);
        }

        public void ShowEndGamePanel(bool battleResult, Action callback)
        {
            _endGamePanelModel.Show(battleResult, callback);
        }

        public void Dispose()
        {
            _disposable?.Dispose();
        }
    }
}