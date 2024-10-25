using System;
using TMPro;
using UnityEngine;

namespace Dialog
{
    public class DialogManager : MonoBehaviour
    {
        public static DialogManager Instance { get; private set; }

        [Header("Componentes Graficos")]
        [SerializeField] private DialogUIManager dialogUIManager;

        public DialogObject dialogObject;

        private DialogoController dialogoController;
        private DialogStateEnum dialogState = DialogStateEnum.Iniciando;

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

        private void Update()
        {
     
            if (dialogState == DialogStateEnum.EmAndamento)
            {
                if (Input.GetMouseButtonDown(0))
                {
                    NextDialog();
                }
                
                if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
                {
                    NextDialog();
                }
            }
        }

        public void Start()
        {
            dialogUIManager.InitComponents();
        }

        public void StartDialog(DialogObject dialogObject)
        {
            this.dialogObject = dialogObject;
            
            if (dialogoController == null)
            {
                dialogoController = new DialogoController(dialogObject.Dialogos);
            }

            dialogState = DialogStateEnum.EmAndamento;
            ShowCurrentDialog();
        }

        private void ShowCurrentDialog()
        {
            Dialogo currentDialog = dialogoController.GetDialogActual();
            if (currentDialog != null)
            {
                dialogUIManager.UpdateDialogUI(currentDialog.speaker, currentDialog.texto);
                dialogUIManager.ShowDialogUI(true);
            }
            else
            {
                dialogState = DialogStateEnum.Concluido;
                HideDialog();
            }
        }

        public void NextDialog()
        {
            if (dialogState != DialogStateEnum.EmAndamento) return;

            if (dialogoController.NextDialog() != null)
            {
                ShowCurrentDialog();
            }
            else
            {
                dialogState = DialogStateEnum.Concluido;
                HideDialog();
            }
        }

        public void HideDialog()
        {
            dialogUIManager.ShowDialogUI(false);
        }

        public void RestartDialog()
        {
            dialogoController.ResetDialog();
            dialogState = DialogStateEnum.EmAndamento;
            ShowCurrentDialog();
        }
    }
}
