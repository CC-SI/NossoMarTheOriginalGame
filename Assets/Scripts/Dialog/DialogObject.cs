using System.Collections.Generic;
using UnityEngine;

namespace Dialog
{
    [CreateAssetMenu]
    public class DialogObject : ScriptableObject
    {
        public List<Dialogo> Dialogos = new();
        
        private int dialogIndex = 0;

        public Dialogo GetDialogoAtual()
        {
            if (Dialogos.Count == 0)
            {
                return null;
            }

            return Dialogos[dialogIndex];
        }

        public bool AvancarDialogo()
        {
            if (dialogIndex < Dialogos.Count - 1)
            {
                dialogIndex++;
                return true; 
            }
            
            return false; 
        }

        public bool AvancarDialogoSilenciosamente()
        {
            if (dialogIndex < Dialogos.Count - 1)
            {
                dialogIndex++;
                return true; 
            }
            
            return false; 
        }

        public void ResetDialog()
        {
            dialogIndex = 0;
        }

        public string GetCurrentDialogId()
        {
            var currentDialog = GetDialogoAtual();
            return currentDialog.id;
        }
        
        public int GetCurrentDialogIndex()
        {
            return dialogIndex;
        }
        
        public Dialogo GetDialogoPorId(string id)
        {
            foreach (var dialogo in Dialogos)
            {
                if (dialogo.id == id)
                {
                    return dialogo;
                }
            }
            
            return null;
        }

        public Dialogo AtualizarShowCocoPorId(string id, bool novoValor)
        {
            Dialogo dialogo = GetDialogoPorId(id);

            if (dialogo != null)
            {
                dialogo.ShowCoco = novoValor;
            }
    
            return dialogo; 
        }
    }
}
