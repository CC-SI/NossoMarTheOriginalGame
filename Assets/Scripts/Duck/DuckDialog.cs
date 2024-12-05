using System.Collections;
using Assets.Scripts.Dialogos;
using Assets.Scripts.Dialogos.Modal;
using Dialogos;
using Duck;
using UnityEngine;

namespace Assets.Scripts.Duck
{
    public class DuckDialog : DuckBehavior
    {
        [SerializeField] private DuckDialogBase dialogoBase;
        [SerializeField] private DialogoObject dialogObject;
        [SerializeField] public GameObject iconeInteracao;
        
        protected override IEnumerator Start()
        {
            yield return base.Start();
            dialogoBase.SetDialogoObject(dialogObject);
        }
        
        public override void OnPlayerInteraction()
        {
            if (IsFollowing) return;
            
            dialogoBase.StartDialogo();
        }
    }
}
