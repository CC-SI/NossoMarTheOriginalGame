using System;
using Dialog.Pergunta;
using UnityEngine;

namespace Dialog
{
    [Serializable]
    public class Dialogo : IPage
    {
        void Start()
        {
            ShowCoco = false;
        }
        
        public string id;
        public string speaker;
        [TextArea(2,10)]
        public string texto;
        public PerguntaObject pergunta;
        public bool ShowCoco;
        
        public string Speaker => speaker;
        public string Text => texto;
    }
}