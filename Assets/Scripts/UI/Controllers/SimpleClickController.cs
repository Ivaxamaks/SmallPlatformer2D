using System;
using Common.MVC;

namespace UI.Controllers
{
    public class SimpleClickController : IController
    {
        public event Action OnClick;
        public void OnClickInvoke() => OnClick?.Invoke();
    }
}