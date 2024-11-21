using UnityEngine;

namespace GameEnd
{
    public class EndPoint : MonoBehaviour
    {
        void OnTriggerEnter2D(Collider2D other)
        {
            GameManager.LoadCredits();
        }
    }
}