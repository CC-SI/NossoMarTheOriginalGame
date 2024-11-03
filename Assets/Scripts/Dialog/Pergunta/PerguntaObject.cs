using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;

namespace Dialog.Pergunta
{
    [CreateAssetMenu]
    public class PerguntaObject : ScriptableObject
    {
        public List<DialogoPergunta> dialogosPergunta;
    }
}