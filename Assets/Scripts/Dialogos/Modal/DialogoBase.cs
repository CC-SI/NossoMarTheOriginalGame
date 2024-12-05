using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Dialogos.Modal
{
    public abstract class DialogoBase : MonoBehaviour
    {
        [Header("Componentes UI")]
        [SerializeField] private GameObject dialogoPainel;
        [SerializeField] private TextMeshProUGUI orador;
        [SerializeField] private TextMeshProUGUI texto;
        [SerializeField] protected Button botaoProximo;
        [SerializeField] private Button botaoFechar;
        [SerializeField] private Button zonaDeFechar;

        protected DialogoObject dialogoObject;
        protected int index = 0;
        private bool dialogoAtivo = false;
        
        protected virtual void Start()
        {
            ListenToEvents();
            
            zonaDeFechar.gameObject.SetActive(false);
        }
        
        public void SetDialogoObject(DialogoObject dialogo)
        {
            dialogoObject = dialogo;
        }
        
        public void StartDialogo()
        {
            ShowDialogo();
        }
        
        protected void NextDialogo()
        {
            if (!dialogoAtivo) return;
            
            var dialogo = dialogoObject.GetDialogoAt(index);
            if (index < dialogoObject.dialogos.Count - 1)
            {
                index++;
                ShowDialogo();
            }
            else
            {
                FinishedDialogo();
            }
            
        }
        
        protected virtual void ShowDialogo()
        {
            dialogoPainel.SetActive(true);
            orador.text = dialogoObject.GetDialogoAt(index).orador;
            texto.text = dialogoObject.GetDialogoAt(index).texto;
            dialogoAtivo = true;
        }

        protected void OcultarDialogo()
        {
            dialogoPainel.SetActive(false);
            dialogoAtivo = false;
        }
        
        void FinishedDialogo()
        {
            dialogoPainel.SetActive(false);
            dialogoAtivo = false;
            index = 0;
            Debug.Log("Dialogo finalizado");
        }
        
        protected virtual void ListenToEvents()
        {
            botaoProximo.onClick.AddListener(NextDialogo);
            botaoFechar.onClick.AddListener(OcultarDialogo);
            zonaDeFechar.onClick.AddListener(OcultarDialogo);
        }
        
    }
}