
# UnMVC

Фреймворк основан на паттерне MVC (Model-View-Controller)

https://ru.wikipedia.org/wiki/Model-View-Controller

!["MVC"](https://upload.wikimedia.org/wikipedia/commons/thumb/f/fd/MVC-Process.png/240px-MVC-Process.png)

Цели:

- Отделение отображения View от данных модели Data.View зависит от Data, Data не зависит от View

- Обеспечение независимой работы программистов UI (модулей View, Controller) и программистов Core (модуля Model)

## Модули:
- Model - данные и бизнес-логика приложения.

- View - отображение данных. Не изменяет Model. Model для модуля View всегда *readonly*

- Controller - оповещает модель о необходимости изменений или изменяет её напрямую.

### Model
Зона ответственности программисте Core

### View
Зона ответственности программиста UI

1. Для реализации View необходимо создать наследника класса `UiDataView<T>` или `UiDataRxView<T>`.
Экземпляр этого наследника прикрепляется к GameObject реализующему View.  

``` csharp
public abstract class UiDataView<T> : MonoBehaviour, IView, IData<T>
```

``` csharp
public abstract class UiDataRxView<T> : UiDataView<T> 
        where T : IRxData
```

``` csharp
public interface IData<out T>
{
    T Data { get; }
    void SetData(T data);
}
```

``` csharp
public interface IView
{    
    void Show();
    void Refresh();
    void Hide();
}
```

#### Пример
Реализуем View иконки игрока на панели GUI игры. При клике на иконку (Controller) будет выводиться сообщение и прибавляться небольшое количество здоровья.

- View
    - UiDataView
    ``` csharp
    public class PlayerView : UiDataView<Player>
        {
            [Inject] [NotNull] private readonly Player _player;
            
            [SerializeField] [NotNull] private Text _name;
            [SerializeField] [NotNull] private Image _image;
            [SerializeField] [NotNull] private Text _health;

            private void Awake()
            {
                this.NullCheckFields();
                
                /* <summary>
                Отображает View.
                Note: Даже если при запуске сцены gameObject.activeInHierarchy == true, вызывать  всё равно следует для отработки Init(), Refresh() 
                */ </summary>
                Show();
            }

            /// <summary>
            /// Самый простой и наименее производительный способ вызова Refresh(), лучше, например,
            /// использовать EventBus подписавшись на событие OnPlayerHealthChange 
            /// </summary>
            private void Update() => Refresh();

            /// <summary>
            /// Вызывается однократно при первом вызове Show()
            /// </summary
            protected override void Init() => SetData(_player);

            public override void Refresh()
            {
                _name.text = Data.Name;
                _image = Data.Image;
                _health.text = Data.Health.ToString();
            }
        }
    ```

    - UiDataRxView
        
        Если вы не хотите вызывать `Refresh()` вручную используйте 
        
        `public class PlayerView : UiDataRxView<Player>`. 
        
        В этом случае из кода приведенного выше строку `private void Update() => Refresh();` можно убрать. Однако тогда класс данных должен реализовывать интерфейс `IRxData`
    ``` csharp
    public interface IRxData
    {
        event Action OnUpdate;
    }
    ```


### Controller

``` csharp
        public class PlayerTapController : Controller<Player>, IPointerUpHandler
        {
            public void OnPointerUp(PointerEventData eventData) => RaiseEventTap();

            private void RaiseEventTap() =>
                EventBus.RaiseEvent<IOnTapPlayerHandler>(handler => handler.OnTapShipHandler(Data));
        }
```

Класс `Controller<T> : MonoBehaviour` ищет компонент содержащий `Player` и предоставляет его в поле `protected T Data` для использования.

Заметьте, что контроллер не изменяет количество здоровья игрока (Модель), а только публикует событие тапа. Реакция на него отрабатывает на уровне Модели. 

Подписчик Модели изменяет здоровье, вызывается `event Action OnUpdate` в `Player`. Затем `Refresh()` в `PlayerView` и вид обновляется.

