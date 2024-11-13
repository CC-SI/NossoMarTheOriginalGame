using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Dialog.Manager
{
    public class DialogUIManager : MonoBehaviour
    {
        [Header("Caixa de Dialogo")]
        [SerializeField] public GameObject boxDialog;
        [SerializeField] private Animator boxDialogAnimatorCoqueiro;

        [Header("Texto do Dialogo")]
        [SerializeField] private TextMeshProUGUI textSpeaker;
        [SerializeField] public TextMeshProUGUI textDialog;

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

        [SerializeField] private FixedJoystick joystick;
        
        [SerializeField] private GameObject duckCaptured;

        [SerializeField] private GameObject boxTimeText;
        
        [SerializeField] private TextMeshProUGUI timeText;
        
        [SerializeField] private Button zonaDeFinalizarDialogo;

        [SerializeField] private bool isAvancarDialogo;
        
        
        public void InitComponent()
        {
            SetupDialogMain(false);
            SetupOnClick();
        }
        
        private void SetupDialogMain(bool show)
        {
            SetActive(boxDialog, show);
            SetActive(textSpeaker, show);
            SetActive(textDialog, show);
            SetActive(fecharDialogo, show);
            SetActive(mensagemObjetoPego, show);
            SetActive(PaOuChapeuOuCoco, show);
            SetActive(buttonAjudarPato, show);
            SetActive(buttonIgnorarPato, show);
            SetActive(zonasDeAvancarDialogo, show);
            SetActive(duckCaptured, false);
            SetActive(zonaDeFinalizarDialogo, false);
        }

        private void SetupOnClick()
        {
            if (zonasDeAvancarDialogo)
            {
                zonasDeAvancarDialogo.onClick.AddListener(() =>
                {
                    dialogManager.AvancarDialogo();
                });
            }

            if (zonaDeFinalizarDialogo)
            {
                zonaDeFinalizarDialogo.onClick.AddListener(() =>
                {
                    if (isAvancarDialogo)
                    {
                        dialogManager.AvancarDialogo();
                    }
                    else
                    {
                        dialogManager.OcultarDialogo();
                    }
                });
            }
            
            if (fecharDialogo )
            {
                fecharDialogo.onClick.AddListener(() =>
                {
                    dialogManager.EndDialog();
                });
            }

            if (buttonAjudarPato)
            {
                buttonAjudarPato.onClick.AddListener(() =>
                {
                    dialogManager.AvancarDialogo();
                });
            }

            if (buttonIgnorarPato != null)
            {
                buttonIgnorarPato.onClick.AddListener(() =>
                {
                    dialogManager.EndDialog();
                });
            }
        }

        public void ShowDialog(bool show)
        {
            SetActive(boxDialog, show);
            SetActive(textSpeaker, show);
            SetActive(textDialog, show);
            SetActive(zonasDeAvancarDialogo, show);
            SetActive(fecharDialogo, show);

            if (show && boxDialogAnimatorCoqueiro != null)
            {
                boxDialogAnimatorCoqueiro.SetBool("isTalking", true);
            }
        }

        public void SetSpeaches(string speaker, string texto)
        {
            if (textSpeaker != null) textSpeaker.text = speaker;
            if (textDialog != null) textDialog.text = texto;
        }

        public void ShowButtonsOfDecision(bool show)
        {
            SetActive(buttonAjudarPato, show);
            SetActive(buttonIgnorarPato, show);
        }

        public void ShowZonasDeAvancarDialogo(bool show)
        {
            SetActive(zonasDeAvancarDialogo, show);
        }

        public void ShowPaOuChapeuOuCoco(bool show)
        {
            SetActive(PaOuChapeuOuCoco, show);
        }
        
        public void ShowMensagemObjetoPego(bool show)
        {
            SetActive(mensagemObjetoPego, show);

            if (show)
            {
                Invoke(nameof(HideMensagemObjetoPego), 5f);
            }
        }

        private void HideMensagemObjetoPego()
        {
            SetActive(mensagemObjetoPego, false);
        }
        
        public void ShowJoystick(bool show)
        {
            SetActive(joystick, show);
        }
        
        public void ShowDuckCaptured(bool show)
        {
            SetActive(duckCaptured, show);
        }
        
        public void ShowBoxTimeText(bool show)
        {
            SetActive(boxTimeText, show);
        }
        
        public void ShowTimeText(bool show)
        {
            SetActive(timeText, show);
        }

        public void ShowFecharDialogo(bool show)
        {
            SetActive(fecharDialogo, show);
        }
        
        
        public void ShowZonaDeFinalizarDialogo(bool show)
        {
            
            
            SetActive(zonaDeFinalizarDialogo, show);
        }
        
        public void SetTimeText(string time)
        {
            if (timeText != null)
            {
                timeText.text = time;
            }
        }
        
        private void SetActive(FixedJoystick obj, bool show)
        {
            if (obj != null)
            {
                obj.gameObject.SetActive(show);
            }
        }
        
        private void SetActive(GameObject obj, bool show)
        {
            if (obj != null)
            {
                obj.SetActive(show);
            }
        }

        private void SetActive(TextMeshProUGUI obj, bool show)
        {
            if (obj != null)
            {
                obj.gameObject.SetActive(show);
            }
        }

        private void SetActive(Button obj, bool show)
        {
            if (obj != null)
            {
                obj.gameObject.SetActive(show);
            }
        }
    }
}
