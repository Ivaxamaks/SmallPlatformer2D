using Common.MVC.View;
using JetBrains.Annotations;
using UI.Controllers;
using UnityEngine;
using UnityEngine.UI;

namespace UI.EndGamePanel
{
    public class EndGamePanelView : UiDataView<bool>
    {
        private const string WIN_SING = "Win";
        private const string LOOSE_SING = "Loose";
        
        [SerializeField, NotNull] private Text _winLooseSingText;
        [SerializeField, NotNull] private Button _restartButton;

        protected override void Init()
        {
        }

        public override void Show()
        {
            base.Show();
            _restartButton.onClick.AddListener(() => Interact<SimpleClickController>(controller => controller.OnClickInvoke()));
        }

        public override void Hide()
        {
            base.Hide();
            _restartButton.onClick.RemoveAllListeners();
        }

        public override void Refresh()
        {
            _winLooseSingText.text = Data ? WIN_SING : LOOSE_SING;
            _winLooseSingText.color = Data ? Color.green : Color.red;
        }
    }
}