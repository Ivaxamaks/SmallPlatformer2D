using System;
using JetBrains.Annotations;
using UI.Controllers;
using UnityEngine;

namespace UI.EndGamePanel
{
    public class EndGamePanelModel : MonoBehaviour
    {
        [SerializeField, NotNull] private EndGamePanelView _panelView;
        
        private Action _callback;

        public void Show(bool battleResult, Action callback)
        {
            _callback = callback;
            var controller = new SimpleClickController();
            controller.OnClick += OnRestartButtonClick;
            _panelView.SetControllers(controller);
            _panelView.SetData(battleResult);
            _panelView.Show();
        }

        private void OnRestartButtonClick()
        {
            _callback.Invoke();
            _panelView.Hide();
        }
    }
}