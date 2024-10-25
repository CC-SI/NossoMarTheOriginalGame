using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;

namespace Dialog
{
    [CreateAssetMenu] public class DialogObject : ScriptableObject
    {
        public WhichThisIsDialogue WhichThisIsDialogue;
        public List<Dialogo> Dialogos = new();
    }
}