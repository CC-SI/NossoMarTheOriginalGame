using UnityEngine;

namespace MiniGame
{
    public class TrashBinBehaviour : MonoBehaviour
    {
        public static TrashBinBehaviour Instance { get; private set; }
        [SerializeField] private Collider2D insideTrashBin;
    
        private void Awake()
        {
            if (!Instance)
            {
                Instance = this;
                return;
            }
        
            Destroy(gameObject);
        }

        public bool ContainsObject(Bounds bounds)
        {
            return insideTrashBin.bounds.Intersects(bounds);
        }
        
#if UNITY_EDITOR
        private void Reset()
        {
            insideTrashBin = GetComponent<Collider2D>();
        }
#endif
    }
}
