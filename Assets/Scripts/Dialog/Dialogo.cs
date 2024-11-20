using System;
using Dialog.Pergunta;

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
        public string texto;
        public PerguntaObject pergunta;
        public bool ShowCoco;
        
        public string Speaker => speaker;
        public string Text => texto;
    }
}