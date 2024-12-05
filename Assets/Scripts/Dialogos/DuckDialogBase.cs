using Assets.Scripts.Dialogos.Modal;
using Assets.Scripts.Duck;
using UnityEngine;

namespace Dialogos
{
    public class DuckDialogBase : DialogoBase
    {
        [SerializeField] private DuckDialog duckDialog;
        
        protected override void FinishedDialogo()
        {
            base.FinishedDialogo();
            
            duckDialog.StartFollowing();
            duckDialog.iconeInteracao.SetActive(false);
            GameManager.SaveGameData();
        }
    }
}