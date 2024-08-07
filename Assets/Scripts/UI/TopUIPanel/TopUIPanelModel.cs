using JetBrains.Annotations;
using UnityEngine;

namespace UI.TopUIPanel
{
    public class TopUIPanelModel : MonoBehaviour
    {
        [SerializeField, NotNull] private TopUIPanelView _topUIPanelView;
        public void Show(int eventDataAmmoCount)
        {
            _topUIPanelView.SetData(eventDataAmmoCount);
            _topUIPanelView.Show();
        }
    }
}