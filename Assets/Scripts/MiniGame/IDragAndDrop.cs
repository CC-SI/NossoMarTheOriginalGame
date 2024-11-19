using UnityEngine.Events;

namespace MiniGame
{
    public interface IDragAndDrop
    {
        UnityEvent<bool, bool> OnTouched { get; }
        UnityEvent OnSuperimposed { get; }
        UnityEvent OnSuperimposing { get; }
    }
}