using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;

namespace Assets.Scripts.Dialogos
{
    [CreateAssetMenu]
    public class DialogoObject : ScriptableObject
    {
        public List<Dialogo> dialogos = new();

        public Dialogo GetDialogoAt(int index)
        {
            return dialogos[index];
        }
    }
}