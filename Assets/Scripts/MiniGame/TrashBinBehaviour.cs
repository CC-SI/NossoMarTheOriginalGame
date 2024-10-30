using UnityEngine;

public class TrashBinBehaviour : MonoBehaviour
{
    public static TrashBinBehaviour Instance { get; private set; }
    private Collider2D insideTrashBin;
    
    private void Awake()
    {
        if (!Instance)
        {
            Instance = this;
            insideTrashBin = GetComponent<Collider2D>();
            return;
        }
        
        Destroy(gameObject);
    }

    public bool ContainsObject(Bounds bounds)
    {
        return insideTrashBin.bounds.Intersects(bounds);
    }
}
