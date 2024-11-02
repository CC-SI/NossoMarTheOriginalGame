using System.Collections.Generic;
using UnityEngine;

namespace MiniGame
{
    public abstract class ObjectManager : MonoBehaviour
    {

        private static readonly List<ObjectManager> TrashObjects = new();
        protected abstract Bounds Bounds { get; }
        protected abstract int Index { get; set; }

        protected static void AddObject(ObjectManager trashObject)
        {
            TrashObjects.Add(trashObject);
        }
        
        protected static void AddObjectByIndex(int index, ObjectManager trashObject)
        {
            TrashObjects.Insert(index, trashObject);
        }
        
        protected static void RemoveObject(ObjectManager trashObject)
        {
            TrashObjects.Remove(trashObject);
        }
        
        public static ObjectManager[] GetObjects()
        {
            return TrashObjects.ToArray();
        }
        
        protected static int GetObjectCount()
        {
            return TrashObjects.Count;
        }
        
        protected static void UpdateIndex()
        {
            var currentIndex = 0;
        
            foreach (var trash in GetObjects())
            {
                trash.Index = currentIndex++;
            }
        }
        
        protected static bool IsTrashSuperimposed(Bounds bounds, int index)
        {
            for (var i = index - 1; i >= 0; i--)
            {
                if (!bounds.Intersects(GetObjects()[i].Bounds) || GetObjects()[i].Bounds == bounds) continue;
                    return true;
            }
        
            return false;
        }
    }
}