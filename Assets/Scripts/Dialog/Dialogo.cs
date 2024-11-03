using System;
using Dialog.Pergunta;

namespace Dialog
{
    [Serializable]
    public class Dialogo
    {
        public string id;
        public string speaker;
        public string texto;
        public PerguntaObject pergunta;
    }
}