using UnityEngine;

namespace Dialog.Repository
{
    /// <summary>
    /// Responsável por gerenciar a lógica dos diálogos e a interação com a interface do usuário de diálogos.
    /// </summary>
    public class DialogRepository : MonoBehaviour
    {
        [SerializeField] private DialogObject dialogObject;
        [SerializeField] private DialogUIManager dialogUIManager;

        private const WhichThisIsDialogue enumDuckBuried = WhichThisIsDialogue.DUCK_BURIED;
        private DialogStateEnum currentDialogStateEnum = DialogStateEnum.AwaitingResponse;

        public void Start()
        {
            if (DialogManager.Instance != null)
            {
                DialogManager.Instance.OnDialogShown += LogDialog;
                DialogManager.Instance.StartDialog(dialogObject);
            }

            dialogUIManager.OnYesClicked += OnYesClicked;
            dialogUIManager.OnNoClicked += OnNoClicked;
        }

        /// <summary>
        /// Registra e processa o diálogo exibido.
        /// </summary>
        /// <param name="dialogo">O diálogo que foi exibido.</param>
        private void LogDialog(Dialogo dialogo)
        {
            if (dialogObject.WhichThisIsDialogue == enumDuckBuried)
            {
                if (dialogo.id == "pato1_pedindo_ajuda")
                {
                    dialogUIManager.ShowYesNoButtons(true);
                    currentDialogStateEnum = DialogStateEnum.AwaitingResponse;
                    dialogo.SetCanAdvance(false);
                    dialogUIManager.ShowAvancarButton(false);
                }
                else if (dialogo.id == "pato2_encontrar_pa")
                {
                    dialogUIManager.ShowPA(true);
                }
                else if (dialogo.id == "player3_procurando_pa")
                {
                    dialogUIManager.ShowAvancarButton(false);
                }
                else
                {
                    dialogUIManager.ShowAvancarButton(true);
                }
            }
        }

        /// <summary>
        /// Lida com a resposta do jogador ao clicar em "Sim".
        /// </summary>
        private void OnYesClicked()
        {
            HandleResponse(true);
        }

        /// <summary>
        /// Lida com a resposta do jogador ao clicar em "Não".
        /// </summary>
        private void OnNoClicked()
        {
            HandleResponse(false);
        }

        /// <summary>
        /// Processa a resposta do jogador com base na opção escolhida.
        /// </summary>
        /// <param name="isYes">Indica se a resposta foi "Sim".</param>
        private void HandleResponse(bool isYes)
        {
            if (currentDialogStateEnum == DialogStateEnum.AwaitingResponse)
            {
                currentDialogStateEnum = DialogStateEnum.AwaitingResponse;
                dialogUIManager.ShowYesNoButtons(false);

                if (isYes)
                {
                    var currentDialog = DialogManager.Instance.GetCurrentDialog();
                    if (currentDialog != null)
                    {
                        currentDialog.SetCanAdvance(true);
                    }
                    DialogManager.Instance.NextDialog();
                }
                else
                {
                    DialogManager.Instance.EndDialog();
                }
            }
        }

        private void OnDestroy()
        {
            if (DialogManager.Instance != null)
            {
                DialogManager.Instance.OnDialogShown -= LogDialog;
            }
        }
    }
}