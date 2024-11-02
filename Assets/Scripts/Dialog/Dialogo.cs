using System;

namespace Dialog
{
    [Serializable]
    public class Dialogo
    {
        public string id;
        public string speaker;
        public string texto;
        public TypeDialogEnum typeDialog;
        
        public bool canAdvance = true;

        public bool CanAdvance()
        {
            return canAdvance;
        }
        
        public void SetCanAdvance(bool canAdvance)
        {
            this.canAdvance = canAdvance;
        }
    }
}