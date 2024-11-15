using System.Collections.Generic;
using UnityEngine;

namespace Dialog.Pergunta
{
    [CreateAssetMenu]
    public class PerguntaObject : ScriptableObject
    {
        public List<DialogoPergunta> dialogosPergunta;
    }
}