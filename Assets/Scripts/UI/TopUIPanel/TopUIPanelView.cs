using Common.MVC.View;
using UnityEngine;
using UnityEngine.UI;

namespace UI.TopUIPanel
{
    public class TopUIPanelView : UiDataView<int>
    {
        [SerializeField] private Text _ammoCountText;
            
        protected override void Init()
        {
        }

        public override void Refresh()
        {
            _ammoCountText.text = Data.ToString();
        }
    }
}