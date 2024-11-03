using System.Collections;
using System.Collections.Generic;
using Dialog.Pergunta;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Dialog.Manager
{
    public class PerguntasUIManager : MonoBehaviour
    {
        [SerializeField] private GameObject painelPerguntas;
        [SerializeField] private TextMeshProUGUI perguntaText;
        [SerializeField] private List<Button> botoesAlternativas = new() { null, null, null };

        private PerguntaManager perguntaManager;
        private DialogoPergunta dialogoAtual;

        public void InitComponent()
        {
            painelPerguntas.SetActive(false);
            perguntaText.gameObject.SetActive(false);

            foreach (var botao in botoesAlternativas)
            {
                if (botao != null)
                {
                    botao.gameObject.SetActive(false);
                }
            }
        }

        public void ShowPainelPerguntas(bool show)
        {
            painelPerguntas.SetActive(show);
            perguntaText.gameObject.SetActive(show);

            foreach (var botao in botoesAlternativas)
            {
                if (botao != null)
                {
                    botao.gameObject.SetActive(show);
                }
            }
        }

        public void AtualizarPergunta(DialogoPergunta dialogo, PerguntaManager manager)
        {
            perguntaManager = manager;
            dialogoAtual = dialogo;
            perguntaText.text = dialogo.pergunta;

            RestaurarBotoes();

            for (int i = 0; i < botoesAlternativas.Count; i++)
            {
                if (i < dialogo.alternativas.Count && botoesAlternativas[i] != null)
                {
                    botoesAlternativas[i].gameObject.SetActive(true);
                    var textComponent = botoesAlternativas[i].GetComponentInChildren<TextMeshProUGUI>();
                    textComponent.text = dialogo.alternativas[i].alternativa;

                    int respostaId = dialogo.alternativas[i].id;
                    botoesAlternativas[i].onClick.RemoveAllListeners();
                    int index = i;
                    botoesAlternativas[i].onClick.AddListener(() => VerificarResposta(respostaId, index));
                }
                else if (botoesAlternativas[i] != null)
                {
                    botoesAlternativas[i].gameObject.SetActive(false);
                }
            }
        }

        private void RestaurarBotoes()
        {
            foreach (var botao in botoesAlternativas)
            {
                if (botao != null)
                {
                    var imagem = botao.GetComponent<Image>();
                    imagem.color = Color.white; 

                    var textComponent = botao.GetComponentInChildren<TextMeshProUGUI>();
                    if (textComponent != null)
                    {
                        textComponent.color = Color.black; // Cor padrão da fonte
                        textComponent.fontStyle = FontStyles.Normal; // Estilo padrão da fonte
                    }
                }
            }
        }

        private void VerificarResposta(int respostaId, int index)
        {
            bool correta = dialogoAtual.VerificarResposta(respostaId);
            int indiceCorreto = dialogoAtual.ObterIndiceRespostaCorreta();

            if (correta)
            {
                perguntaManager.IncremetarAcertos();
            }
            
            AlterarCorBotoes(index, indiceCorreto, correta);

            Debug.Log($"Botão clicado: {index}");
            Debug.Log($"Resposta Correta: {correta}");
            Debug.Log($"Índice da Resposta Correta: {indiceCorreto}");
            
            StartCoroutine(EsperarParaProximaPergunta());
        }

        private void AlterarCorBotoes(int indiceClicado, int indiceCorreto, bool correta)
        {
            foreach (var botao in botoesAlternativas)
            {
                var textComponent = botao.GetComponentInChildren<TextMeshProUGUI>();
                if (textComponent != null)
                {
                    textComponent.color = Color.black; // Restaura a cor da fonte para preto
                    textComponent.fontStyle = FontStyles.Normal; // Restaura o estilo para normal
                }
            }

            if (!correta)
            {
                var botaoErrado = botoesAlternativas[indiceClicado].GetComponent<Image>();
                botaoErrado.color = Color.red; 

                var textComponent = botoesAlternativas[indiceClicado].GetComponentInChildren<TextMeshProUGUI>();
                if (textComponent != null)
                {
                    textComponent.color = Color.white; // Altera a cor da fonte para branco
                    textComponent.fontStyle = FontStyles.Bold; // Altera o estilo para negrito
                }
            }

            if (indiceCorreto != -1) // Verifica se existe uma resposta correta
            {
                var botaoCorreto = botoesAlternativas[indiceCorreto].GetComponent<Image>();
                botaoCorreto.color = Color.green; 

                var textComponent = botoesAlternativas[indiceCorreto].GetComponentInChildren<TextMeshProUGUI>();
                if (textComponent != null)
                {
                    textComponent.color = Color.white; // Altera a cor da fonte para branco
                    textComponent.fontStyle = FontStyles.Bold; // Altera o estilo para negrito
                }
            }
        }

        private IEnumerator EsperarParaProximaPergunta()
        {
            yield return new WaitForSeconds(1f); 
            perguntaManager.AvancarPergunta();
        }
    }
}
