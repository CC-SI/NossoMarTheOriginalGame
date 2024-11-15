using System;
using System.Collections.Generic;

namespace Dialog.Pergunta
{
    [Serializable]
    public class Alternativa
    {
        public int id;
        public string alternativa;
    }
    
    [Serializable]
    public class DialogoPergunta
    {
        public string id;
        public bool isRespondida;
        public string pergunta;
        public string respostaCorreta;
        public List<Alternativa> alternativas = new(){null, null, null};
        
        
        public int ContarPerguntasRespondidas(List<DialogoPergunta> perguntas)
        {
            int perguntasRespondidas = 0;

            foreach (var pergunta in perguntas)
            {
                if (pergunta.isRespondida)
                {
                    perguntasRespondidas++;
                }
            }

            return perguntasRespondidas;
        }

        
        public bool VerificarResposta(int respostaId)
        {
            var alternativaCorreta = alternativas.Find(alt => alt.id == respostaId);
            return alternativaCorreta != null && alternativaCorreta.alternativa == respostaCorreta;
        }
        
        public int ObterIdRespostaCorreta()
        {
            var alternativaCorreta = alternativas.Find(alt => alt.alternativa == respostaCorreta);
            return alternativaCorreta != null ? alternativaCorreta.id : -1; 
        }
    
        public int ObterIndiceRespostaCorreta()
        {
            for (int i = 0; i < alternativas.Count; i++)
            {
                if (alternativas[i] != null && alternativas[i].alternativa == respostaCorreta)
                {
                    return i;
                }
            }
            return -1;
        }

    }
}