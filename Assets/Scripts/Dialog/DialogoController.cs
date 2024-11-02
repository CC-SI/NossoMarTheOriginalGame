using System.Collections.Generic;
using UnityEngine;

namespace Dialog
{
    public class DialogoController : MonoBehaviour
    {
        private DialogObject dialogObject;
        private int indexDialogo;

        public bool IsDialogoFinalizado;

        private int ultimoDialogoVisto;
        
        public void SetDialogObject(DialogObject dialogObject, bool retomar = false)
        {
            this.dialogObject = dialogObject;
            indexDialogo = retomar ? ultimoDialogoVisto : 0;
            IsDialogoFinalizado = false;
        }
        
        public Dialogo NextDialog()
        {
            if (dialogObject == null || dialogObject.Dialogos == null || dialogObject.Dialogos.Count == 0)
            {
                Debug.LogWarning("dialogObject ou sua lista de Dialogos não está inicializada.");
                return null;
            }

            if (indexDialogo < dialogObject.Dialogos.Count - 1)
            {
                Debug.Log(dialogObject.Dialogos[indexDialogo].speaker);
                Debug.Log(dialogObject.Dialogos[indexDialogo].texto);
                return dialogObject.Dialogos[indexDialogo++];
            }
            else
            {
                Debug.Log("Dialogo Finalizado");
                IsDialogoFinalizado = true;
                return null;
            }
        }
        
        public Dialogo GetDialogoAtual()
        {
            if (dialogObject == null || dialogObject.Dialogos == null || dialogObject.Dialogos.Count == 0)
            {
                Debug.LogWarning("dialogObject ou sua lista de Dialogos não está inicializada.");
                return null;
            }

            if (indexDialogo < dialogObject.Dialogos.Count)
            {
                return dialogObject.Dialogos[indexDialogo]; 
            }

            return null; // Retorna nulo se o índice estiver fora do limite
        }


        public void SaveLastDialogIndex()
        {
            ultimoDialogoVisto = indexDialogo;
        }
    }
}