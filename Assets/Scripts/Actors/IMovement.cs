using UnityEngine;
using UnityEngine.Events;

namespace Actors
{
    public interface IMovement
    {
        void Disable();
        void Enable();
        UnityEvent<Vector2, bool> OnMoved { get; }
    }
}