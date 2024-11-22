using System.Collections.Generic;
using Dialog;
using Dialog.Manager;
using Dialog.Pergunta;
using Player;
using UnityEngine;

public class PerguntaManager : MonoBehaviour
{
    public static int perguntasRespondidasTotal = 0;

    [SerializeField] private PerguntasUIManager perguntasUIManager;
    [SerializeField] private DialogManager dialogManager;
    [SerializeField] private DialogObject perguntaObject;
    [SerializeField] private int numeroMaximoPerguntas = 3;
    private List<DialogoPergunta> perguntasEmbaralhadas = new();
    private List<PerguntaObject> perguntasObjectList = new();

    [SerializeField] public bool isTutorialMove;

    private int perguntaAtualIndex = 0;
    private int acertos = 0;

    public void Start()
    {
        CarregarPerguntasRespondidas();

        perguntasUIManager.InitComponent();
        StartPerguntas();
    }
    
    private void OnApplicationQuit()
    {
        SalvarPerguntasRespondidas();
    }

    private void SalvarPerguntasRespondidas()
    {
        PlayerPrefs.SetInt("PerguntasRespondidasTotal", perguntasRespondidasTotal);
        PlayerPrefs.Save();
    }
    
    private void CarregarPerguntasRespondidas()
    {
        if (PlayerPrefs.HasKey("PerguntasRespondidasTotal"))
        {
            perguntasRespondidasTotal = PlayerPrefs.GetInt("PerguntasRespondidasTotal");
        }
        else
        {
            perguntasRespondidasTotal = 0; 
        }

        Debug.Log("Perguntas respondidas ao iniciar: " + perguntasRespondidasTotal);
    }

    public void StartPerguntas()
    {
        if (perguntasRespondidasTotal >= 24)
        {
            ResetarPerguntasRespondidas();
        }
        
        ResetarPerguntas();
    }

    public void AvancarPergunta()
    {
        do
        {
            perguntaAtualIndex++;
        } 
        while (perguntaAtualIndex < perguntasEmbaralhadas.Count && perguntasEmbaralhadas[perguntaAtualIndex].isRespondida);

        if (perguntaAtualIndex < perguntasEmbaralhadas.Count)
        {
            ExibirPerguntaAtual();
        }
        else
        {
            perguntasUIManager.ShowPainelPerguntas(false);

            if (!isTutorialMove)
            {
                PlayerBehaviour.Instance.Movement.enabled = true;
            }

            if (acertos >= 2)
            {
                dialogManager.AvancarDialogo();
            }
        }

        AtualizarPerguntasRespondidas();
    }


    private void AtualizarPerguntasRespondidas()
    {
        perguntasRespondidasTotal++;
        Debug.Log("Total de perguntas respondidas por todas as instâncias: " + perguntasRespondidasTotal);
    }

    private void ExibirPerguntaAtual()
    {
        if (perguntaAtualIndex < perguntasEmbaralhadas.Count)
        {
            var dialogoAtual = perguntasEmbaralhadas[perguntaAtualIndex];
            if (perguntasEmbaralhadas[perguntaAtualIndex].isRespondida == false)
            {
                perguntasUIManager.AtualizarPergunta(dialogoAtual, this); 
            }
        }
    }

    public void ResetarPerguntas()
    {
        perguntaAtualIndex = 0;
        perguntasEmbaralhadas.Clear();

        foreach (var dialogo in perguntaObject.Dialogos)
        {
            if (dialogo.pergunta != null)
            {
                foreach (var pergunta in dialogo.pergunta.dialogosPergunta)
                {
                    if (!pergunta.isRespondida) 
                    {
                        perguntasEmbaralhadas.Add(pergunta);
                    }
                }
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
    
    private void ResetarPerguntasRespondidas()
    {
        foreach (var dialogo in perguntaObject.Dialogos)
        {
            if (dialogo.pergunta != null)
            {
                foreach (var pergunta in dialogo.pergunta.dialogosPergunta)
                {
                    if (pergunta.isRespondida)
                    {
                        pergunta.isRespondida = false;
                        Debug.Log("Pergunta resetada: " + pergunta.id);
                    }
                }
            }
        }
    
        perguntasRespondidasTotal = 0; 
        Debug.Log("Contador de perguntas respondidas resetado para 0.");
    }

    private void MarcarComoRespondida(DialogoPergunta perguntaRespondida)
    {
        perguntaRespondida.isRespondida = true;
    }

    public int IncremetarAcertos()
    {
        acertos++;
        return acertos;
    }
}
