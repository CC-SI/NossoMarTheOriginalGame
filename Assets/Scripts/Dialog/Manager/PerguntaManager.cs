using System.Collections.Generic;
using Dialog.Pergunta;
using UnityEngine;

namespace Dialog.Manager
{
    public class PerguntaManager : MonoBehaviour
    {
        [SerializeField] private PerguntasUIManager perguntasUIManager;
        [SerializeField] private DialogManager dialogManager;
        [SerializeField] private DialogObject perguntaObject;
        [SerializeField] private int numeroMaximoPerguntas = 3; 
        private List<DialogoPergunta> perguntasEmbaralhadas = new();
        
        private int perguntaAtualIndex = 0;

        private int acertos = 0;
        
        public void Start()
        {
            perguntasUIManager.InitComponent();
        }
        
        public void StartPerguntas()
        {
            ResetarPerguntas();
        }

        public void AvancarPergunta()
        {
            if (perguntaAtualIndex < perguntasEmbaralhadas.Count - 1)
            {
                perguntaAtualIndex++;
                ExibirPerguntaAtual();
            } 
            else
            {
                Debug.Log($"Fim das perguntas. Total de Acertos: {acertos}/{numeroMaximoPerguntas}");
                perguntasUIManager.ShowPainelPerguntas(false); 
                
                if (acertos >= 2)
                {
                    dialogManager.AvancarDialogo();
                } 
            }
        }
        
        private void ExibirPerguntaAtual()
        {
            if (perguntaAtualIndex < perguntasEmbaralhadas.Count)
            {
                var dialogoAtual = perguntasEmbaralhadas[perguntaAtualIndex];
                perguntasUIManager.AtualizarPergunta(dialogoAtual, this);
            }
            else
            {
                Debug.Log("Todas as perguntas foram respondidas!");
            }
        }
        
        public void ResetarPerguntas()
        {
            perguntaAtualIndex = 0;
            perguntasEmbaralhadas = new List<DialogoPergunta>();

            foreach (var dialogo in perguntaObject.Dialogos)
            {
                if (dialogo.pergunta != null)
                {
                    perguntasEmbaralhadas.AddRange(dialogo.pergunta.dialogosPergunta);
                }
            }
            
            EmbaralharPerguntas(perguntasEmbaralhadas);
            
            if (perguntasEmbaralhadas.Count > numeroMaximoPerguntas)
            {
                perguntasEmbaralhadas = perguntasEmbaralhadas.GetRange(0, numeroMaximoPerguntas);
            }

            perguntasUIManager.HabilitarBotoes();
            ExibirPerguntaAtual();
        }
        
        private void EmbaralharPerguntas(List<DialogoPergunta> perguntas)
        {
            for (int i = perguntas.Count - 1; i > 0; i--)
            {
                int j = Random.Range(0, i + 1);
                var temp = perguntas[i];
                perguntas[i] = perguntas[j];
                perguntas[j] = temp;
            }
        }
        
        public int IncremetarAcertos()
        {
            acertos++;
            return acertos;
        }
    }
}