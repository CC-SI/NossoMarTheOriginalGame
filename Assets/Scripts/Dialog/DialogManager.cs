using System;
using System.Collections.Generic;
using Dialog.Repository;
using Duck;
using NUnit.Framework;
using UnityEngine;

namespace Dialog
{
    /// <summary>
    /// Gerencia a lógica de exibição e navegação pelos diálogos.
    /// </summary>
    public class DialogManager : MonoBehaviour
    {
        public static DialogManager Instance { get; private set; }

        [Header("Componentes Gráficos")]
        [SerializeField] public DialogUIManager dialogUIManager;
        [SerializeField] private DialogRepository dialogRepository;
        
        [SerializeField] private List<DuckDialog> duckDialogs;
        public event Action<Dialogo> OnDialogShown;
        
        private DialogObject dialogObject;
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

        public void Start()
        {
            dialogUIManager.InitComponents();
        }
        
        /// <summary>
        /// Inicia um diálogo com o objeto de diálogo fornecido.
        /// </summary>
        /// <param name="dialogObject">O objeto de diálogo a ser iniciado.</param>
        public void StartDialog(DialogObject dialogObject)
        {
            if (dialogObject == null) return;

            this.dialogObject = dialogObject;
            
            if (dialogoController == null)
            {
                dialogoController = new DialogoController(dialogObject.Dialogos);
            }

            dialogState = DialogStateEnum.EmAndamento;
            ShowCurrentDialog();
        }
        
        /// <summary>
        /// Exibe o diálogo atual na interface do usuário.
        /// </summary>
        private void ShowCurrentDialog()
        {
            Dialogo currentDialog = dialogoController.GetDialogActual();
            if (currentDialog != null)
            {
                dialogUIManager.UpdateDialogUI(currentDialog.speaker, currentDialog.texto);
                dialogUIManager.ShowDialogUI(true);
                
                OnDialogShown?.Invoke(currentDialog);
            }
            else
            {
                EndDialog();
            }
        }
        
        /// <summary>
        /// Obtém o diálogo atual.
        /// </summary>
        /// <returns>O diálogo atual.</returns>
        public Dialogo GetCurrentDialog()
        {
            return dialogoController.GetDialogActual();
        }
        
        /// <summary>
        /// Avança o diálogo sem exibir a interface do usuário.
        /// </summary>
        public void AdvanceDialogWithoutUI()
        {
            var currentDialog = dialogoController.GetDialogActual();
            if (currentDialog != null && currentDialog.canAdvance)
            {
                dialogoController.NextDialog();
            }
        }

        /// <summary>
        /// Avança para o próximo diálogo.
        /// </summary>
        public void NextDialog()
        {
            var currentDialog = dialogoController.GetDialogActual();
            if (currentDialog != null && currentDialog.canAdvance)
            {
                if (dialogoController.NextDialog() != null)
                {
                    ShowCurrentDialog();
                }
                else
                {
                    EndDialog();
                }
            }
            else
            {
                EndDialog();
            }
        }
        
        /// <summary>
        /// Finaliza o diálogo e oculta a interface do usuário.
        /// </summary>
        public void EndDialog()
        {
            if (dialogoController != null && dialogoController.HasNextDialog())
            {
                PauseDialog();
            } 
            else
            {
                dialogUIManager.ShowDialogUI(false);
                dialogState = DialogStateEnum.Concluido;

                foreach (var duckDialog in duckDialogs)
                {
                    duckDialog.StartFollowing();
                }
            }
        }

        private void PauseDialog()
        {
            dialogUIManager.ShowDialogUI(false);
            dialogState = DialogStateEnum.EmAndamento;
        }
    }
}