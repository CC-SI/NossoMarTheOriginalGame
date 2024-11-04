using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Dialog.Manager
{
    public class DialogUIManager : MonoBehaviour
    {
        [Header("Caixa de Dialogo")]
        [SerializeField] private GameObject boxDialog;
        
        [Header("Texto do Dialogo")]
        [SerializeField] private TextMeshProUGUI textSpeaker;
        [SerializeField] private TextMeshProUGUI textDialog;

        [Header("Botões")]
        [SerializeField] private Button fecharDialogo;

        [Header("Botões de Avançar Dialogo")] 
        [SerializeField] private Button zonasDeAvancarDialogo;
        [SerializeField] private DialogManager dialogManager;

        [Header("Botões de Decisão (Opcional)")] 
        [SerializeField] private Button buttonAjudarPato;
        [SerializeField] private Button buttonIgnorarPato;

        [Header("Objetos que deve procurar (Opcional)")] 
        [SerializeField] private GameObject PaOuChapeuOuCoco;
        
        [Header("Mensagem de Objeto pego (Opicional)")]
        [SerializeField] private GameObject mensagemObjetoPego;
        
        [SerializeField] private GameObject coqueiro;
        
        public void InitComponent()
        {
            SetupDialogMain(false);
            SetupOnClick();
        }

        private void SetupDialogMain(bool show)
        {
            boxDialog.SetActive(show);
            
            textSpeaker.gameObject.SetActive(show);
            textDialog.gameObject.SetActive(show);
            
            fecharDialogo.gameObject.SetActive(show);
            
            if (coqueiro != null)
                coqueiro.SetActive(show);
            
            if (mensagemObjetoPego != null)
                mensagemObjetoPego.SetActive(show);

            if (PaOuChapeuOuCoco != null)
                PaOuChapeuOuCoco.gameObject.SetActive(show);

            if (buttonAjudarPato != null)
                buttonAjudarPato.gameObject.SetActive(show);

            if (buttonIgnorarPato != null)
                buttonIgnorarPato.gameObject.SetActive(show);
                
        }

        private void SetupOnClick()
        {
            zonasDeAvancarDialogo.onClick.AddListener(() =>
            {
                dialogManager.AvancarDialogo();
            });
            
            fecharDialogo.onClick.AddListener(() =>
            {
                dialogManager.EndDialog();
            });
            
            buttonAjudarPato.onClick.AddListener(() =>
            {
                dialogManager.AvancarDialogo();
            });
            
            buttonIgnorarPato.onClick.AddListener(() =>
            {
                dialogManager.EndDialog();
            });
        }

        public void ShowDialog(bool show)
        {
            boxDialog.SetActive(show);
            
            textSpeaker.gameObject.SetActive(show);
            textDialog.gameObject.SetActive(show);
            
            zonasDeAvancarDialogo.gameObject.SetActive(show);
            fecharDialogo.gameObject.SetActive(show);
        }
        
        public void SetSpeaches(string speaker, string texto)
        {
            textSpeaker.text = speaker;
            textDialog.text = texto;
        }
        
        public void ShowButtonsOfDecision(bool show)
        {
            buttonAjudarPato.gameObject.SetActive(show);
            buttonIgnorarPato.gameObject.SetActive(show);
        }
        
        public void ShowZonasDeAvancarDialogo(bool show)
        {
            zonasDeAvancarDialogo.gameObject.SetActive(show);
        }

        public void ShowPaOuChapeuOuCoco(bool show)
        {
            if (PaOuChapeuOuCoco != null)
            {
                PaOuChapeuOuCoco.gameObject.SetActive(show);
            }
        }
        
        public void ShowCoqueiro(bool show)
        {
            if (coqueiro != null)
            {
                coqueiro.SetActive(show);
            }
        }
        
        public void ShowMensagemObjetoPego(bool show)
        {
            mensagemObjetoPego.SetActive(show);
            
            if (show)
            {
                Invoke(nameof(HideMensagemObjetoPego), 5f);
            }
        }
        
        private void HideMensagemObjetoPego()
        {
            mensagemObjetoPego.SetActive(false);
        }
    }
}