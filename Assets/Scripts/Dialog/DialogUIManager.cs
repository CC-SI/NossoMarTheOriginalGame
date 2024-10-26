using System;
using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Dialog
{
    /// <summary>
    /// Atualiza a UI e lida com a exibição e ocultação dos componentes de diálogo.
    /// </summary>
    public class DialogUIManager : MonoBehaviour
    {
        [Header("Componentes Graficos")]
        [SerializeField] private GameObject caixaDialogo;
        [SerializeField] private Button fecharDialogo;
        [SerializeField] private Button btnSim;
        [SerializeField] private Button btnNao;
        [SerializeField] private GameObject mensagemDePaColetada;
        
        [Header("Lista de botões de avançar diálogo")]
        [SerializeField] private List<Button> ListDeAvancarDialogo;
        
        [Header("Componentes de Dialogo")]
        [SerializeField] private TextMeshProUGUI textoDialogo;
        [SerializeField] private TextMeshProUGUI whoSpeak;

        [Header("Objetos que tem que procurar")] 
        [SerializeField] private GameObject pa;
        public event Action OnYesClicked;
        public event Action OnNoClicked;
        
        public static DialogUIManager Instance { get; private set; }
        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Destroy(this);
            }
        }
        
        /// <summary>
        /// Inicializa os componentes da interface do usuário do diálogo.
        /// Esconde todos os elementos do UI inicialmente.
        /// </summary>
        public void InitComponents()
        {
            caixaDialogo.SetActive(false);
            fecharDialogo.gameObject.SetActive(false);
            btnSim.gameObject.SetActive(false);
            btnNao.gameObject.SetActive(false);
            mensagemDePaColetada.SetActive(false);
         
            foreach (var btn in ListDeAvancarDialogo)
            {
                btn.gameObject.SetActive(false);
            }
            
            
            pa.SetActive(false);
            
            fecharDialogo.onClick.AddListener(DialogManager.Instance.EndDialog);
            
            btnSim.onClick.AddListener(() => OnYesClicked?.Invoke());
            btnNao.onClick.AddListener(() => OnNoClicked?.Invoke());
            
            foreach (var btn in ListDeAvancarDialogo)
            {
                btn.onClick.AddListener(DialogManager.Instance.NextDialog);
            }
        }

        /// <summary>
        /// Atualiza o texto do diálogo com o nome do falante e o conteúdo do texto.
        /// </summary>
        /// <param name="speaker">Nome do falante.</param>
        /// <param name="texto">Texto do diálogo.</param>
        public void UpdateDialogUI(string speaker, string texto)
        {
            whoSpeak.text = speaker;
            textoDialogo.text = texto;
        }

        /// <summary>
        /// Controla a exibição ou ocultação da caixa de diálogo.
        /// </summary>
        /// <param name="show">Se verdadeiro, mostra a caixa de diálogo; caso contrário, oculta.</param>
        public void ShowDialogUI(bool show)
        {
            caixaDialogo.SetActive(show);
            fecharDialogo.gameObject.SetActive(show);
            
            foreach (var btn in ListDeAvancarDialogo)
            {
                btn.gameObject.SetActive(show);
            }
        }
        
        /// <summary>
        /// Mostra ou oculta o botão "Avançar".
        /// </summary>
        /// <param name="show">Indica se o botão deve ser mostrado.</param>
        public void ShowAvancarButton(bool show)
        {
            foreach (var btn in ListDeAvancarDialogo)
            {
                btn.gameObject.SetActive(show);
            }
        }
        
        /// <summary>
        /// Mostra ou oculta os botões de "Sim" e "Não".
        /// </summary>
        /// <param name="show">Indica se os botões devem ser mostrados.</param>
        public void ShowYesNoButtons(bool show)
        {
            btnSim.gameObject.SetActive(show);
            btnNao.gameObject.SetActive(show);
        }
        
        public void ShowPA(bool show)
        {
            pa.SetActive(show);
        }
        
        public void ShowMensagemDePaColetada(bool show)
        {
            mensagemDePaColetada.SetActive(show);
            Debug.Log("Ativou");

            if (mensagemDePaColetada.activeSelf)
            {
                Invoke(nameof(HideMensagemDePaColetada), 5f);
            }
           
        }
        
        private void HideMensagemDePaColetada()
        {
            mensagemDePaColetada.SetActive(false);
        }
    }
}