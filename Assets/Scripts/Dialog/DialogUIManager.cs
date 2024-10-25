using System.Collections.Generic;
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
        [SerializeField] private GameObject btnSim;
        [SerializeField] private GameObject btnNao;

        [Header("Componentes de Dialogo")]
        [SerializeField] private TextMeshProUGUI textoDialogo;
        [SerializeField] private TextMeshProUGUI whoSpeak;

        /// <summary>
        /// Inicializa os componentes da interface do usuário do diálogo.
        /// Esconde todos os elementos do UI inicialmente.
        /// </summary>
        public void InitComponents()
        {
            caixaDialogo.SetActive(false);
            fecharDialogo.gameObject.SetActive(false);
            btnSim.SetActive(false);
            btnNao.SetActive(false);

            // Adiciona listener ao botão de fechar
            fecharDialogo.onClick.AddListener(OnFecharDialogo);
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
        }

        /// <summary>
        /// Esconde a caixa de diálogo.
        /// </summary>
        private void HideDialog()
        {
            DialogManager.Instance.HideDialog();
        }

        /// <summary>
        /// Método chamado pelo botão de fechar.
        /// </summary>
        public void OnFecharDialogo()
        {
            HideDialog(); 
        }
    }
}
