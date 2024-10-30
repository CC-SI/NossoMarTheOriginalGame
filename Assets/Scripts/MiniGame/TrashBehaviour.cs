using System;
using System.Collections.Generic;
using UnityEngine;

public class TrashBehaviour : MonoBehaviour
{
    [SerializeField] private Collider2D colisor;
    [SerializeField] private SpriteRenderer sprite;
    [SerializeField] private Animator animator;
    
    private Vector3 originalPosition;
    
    private bool isDragging;
    private int trashIndex;

    private static readonly List<TrashBehaviour> Trashs = new();

    private TrashBinBehaviour TrashBin => TrashBinBehaviour.Instance;
    private Bounds Bounds => sprite.bounds;

    private int TrashIndex
    {
        get => trashIndex;
        set
        {
            trashIndex = value;
            transform.SetSiblingIndex(trashIndex);
            sprite.sortingOrder = -value;
        }
    }
    
    public static TrashBehaviour[] GetTrash()
    {
        return Trashs.ToArray();
    }

    private void Awake()
    {
        TrashIndex = Trashs.Count;
        Trashs.Add(this);
    }

    private void Start()
    {
        Trashs.Sort(SortByLowZ);
        
        originalPosition = transform.position;
    }

    private static int SortByLowZ(TrashBehaviour a, TrashBehaviour b)
    {
        if (a.transform.position.z.Equals(b.transform.position.z))
        {
            return 0;
        }
        
        if (a.transform.position.z < b.transform.position.z)
        {
            a.TrashIndex--;
            return -1;
        }
        
        a.TrashIndex--;
        return 1;
    }

    private void Update()
    {
        if (!isDragging) return;
        
        var mousePosition = GetMousePosition();

        transform.position = new Vector3(mousePosition.x, mousePosition.y, transform.position.z);
    }

    private void OnMouseDown()
    {
        if (IsBlocked()) return;
        
        isDragging = true;
        animator.SetBool("isDragging", isDragging);

        Trashs.Remove(this);
        Trashs.Insert(0, this);

        int currentOrder = 0;
        
        foreach (var trash in Trashs)
        {
            trash.TrashIndex = currentOrder++;
        }
    }

    private void OnMouseUp()
    {
        isDragging = false;
        animator.SetBool("isDragging", isDragging);
        
        if (TrashBin.ContainsObject(Bounds))
        {
            transform.position = originalPosition;
            animator.SetBool("isOnTrashBin", true);
            return;
        }
        
        animator.SetBool("isOnTrashBin", false);
    }

    private Vector3 GetMousePosition()
    {
        return Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }
    
    private bool IsBlocked()
    {
        for (var i = TrashIndex - 1; i >= 0; i--)
        {
            if (colisor.bounds.Intersects(Trashs[i].Bounds))
            {
                Debug.Log("the object" + name + " is blocked by " + Trashs[i].name);
                return true;
            }
        }
        
        Debug.Log("the object" + name + " is not blocked");
        return false;
    }

    private void Reset()
    {
        colisor = GetComponent<Collider2D>();
        sprite = GetComponentInChildren<SpriteRenderer>();
        animator = GetComponent<Animator>();
    }
}