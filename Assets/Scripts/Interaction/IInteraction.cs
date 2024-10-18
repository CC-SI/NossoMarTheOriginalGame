using UnityEngine;

namespace Interaction
{
    public interface IInteraction
    {
        void OnPlayerInteraction();
        GameObject GameObject { get; }
    }
}