using UnityEngine;
using UnityEngine.Events;

namespace Actors
{
    public interface IMovement
    {
        UnityEvent<Vector2, bool> OnMoved { get; }
    }
}