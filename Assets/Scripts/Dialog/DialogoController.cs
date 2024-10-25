using System.Collections.Generic;

namespace Dialog
{
    /// <summary>
    /// Controla a lógica de navegação pelos diálogos.
    /// </summary>
    public class DialogoController
    {
        private List<Dialogo> dialogos;
        private int currentIndex;

        public DialogoController(List<Dialogo> dialogos)
        {
            this.dialogos = dialogos;
            currentIndex = 0;
        }

        /// <summary>
        /// Avança para o próximo diálogo na lista.
        /// </summary>
        /// <returns>O próximo diálogo, ou null se não houver mais diálogos.</returns>
        public Dialogo NextDialog()
        {
            if (currentIndex < dialogos.Count - 1)
            {
                currentIndex++;
                return dialogos[currentIndex];
            }

            return null;
        }

        /// <summary>
        /// Obtém o diálogo atual na lista.
        /// </summary>
        /// <returns>O diálogo atual, ou null se não houver mais diálogos.</returns>
        public Dialogo GetDialogActual()
        {
            return currentIndex < dialogos.Count ? dialogos[currentIndex] : null;
        }

        /// <summary>
        /// Obtém o último diálogo visualizado.
        /// </summary>
        /// <returns>O último diálogo, ou o diálogo atual se não houver um anterior.</returns>
        public Dialogo GetLastDialog()
        {
            if (currentIndex > 0)
            {
                return dialogos[currentIndex - 1];
            }

            return GetDialogActual();
        }

        /// <summary>
        /// Reinicia o controlador de diálogos, voltando ao início da lista de diálogos.
        /// </summary>
        public void ResetDialog()
        {
            currentIndex = 0;
        }
    }
}