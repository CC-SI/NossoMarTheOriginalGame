using System;
using Dialog.Pergunta;

namespace Dialog
{
    [Serializable]
    public class Dialogo
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
    }
}