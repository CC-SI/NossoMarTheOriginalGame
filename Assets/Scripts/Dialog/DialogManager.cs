using Dialog.Repository;
using UnityEngine;

namespace Dialog
{
    public class DialogManager : MonoBehaviour
    {
        [SerializeField] private DialogUIManager dialogUIManager;
        [SerializeField] private DialogoController dialogoController;
        [SerializeField] private DialogRepository _dialogRepository;
        
        private Dialogo indexDialogo;

        private void Start()
        {
            dialogUIManager.InitComponent();
        }

        public void SetDialogObject(DialogObject dialogObject, bool retomarUltimoDialogo = false)
        {
            dialogoController.SetDialogObject(dialogObject, retomarUltimoDialogo);
        }

        public void StartDialog()
        {
            ShowDialog();
        }

        public void NextDialog()
        {
            if (indexDialogo != null && indexDialogo.CanAdvance())
            {
                ShowDialog();
            }
            dialogoController.NextDialog();
            ShowDialog();
        }

        public void AdvanceDialogWithoutUI()
        {
            var dialogoAtual = dialogoController.NextDialog();
            if (dialogoAtual != null)
            {
                dialogoController.SaveLastDialogIndex(); 
            }
            Debug.Log("Diálogo avançado sem mostrar na UI");
            indexDialogo = PositionIndex(); 
        }

        public void EndDialog()
        {
            dialogUIManager.ShowDialogUI(false);
            dialogoController.SaveLastDialogIndex();
            indexDialogo = PositionIndex();
        }

        private void ShowDialog()
        {
            dialogUIManager.ShowDialogUI(true);
            var dialogAtual = PositionIndex();
            if (dialogAtual != null)
            {
                UpdateDialogUI(dialogAtual.speaker, dialogAtual.texto);
                dialogoController.SaveLastDialogIndex();
                _dialogRepository.LogDialog(dialogAtual);
            }
        }

        private Dialogo PositionIndex()
        {
            return dialogoController.GetDialogoAtual();
        }

        private void UpdateDialogUI(string speaker, string texto)
        {
            dialogUIManager.UpdateDialogUI(speaker, texto);
        }
    }
}