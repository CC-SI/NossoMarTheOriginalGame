using System.Collections;
using Actors;
using Dialog.Manager;
using UnityEngine;

namespace Duck
{
    public class DuckDialog : DuckBehavior
    {
        [SerializeField] private DialogManager dialogManager;
        [SerializeField] private ObjectToBeCaptured objectToBeCaptured;

        [SerializeField] private GameObject acessorio;
        [SerializeField] private GameObject ponto;
        
        [Header("Pato começa enterrado")]
        [SerializeField] private GraphicBehaviour graphicBehaviour;
        
        public bool isDuckAguaCoco;
        public bool isPatoEnterrado;
        
        protected override IEnumerator Start()
        {
            OnDuckRescued += AcessoriosPegos;
            
            if (isPatoEnterrado)
                graphicBehaviour.IsBuried = true;

            yield return base.Start();
        }

        private void OnDestroy()
        {
            OnDuckRescued -= AcessoriosPegos;
        }
        
        [ContextMenu("salvar pato")]
        public override void OnPlayerInteraction()
        {
            if (IsFollowing) return;
            
            if (objectToBeCaptured)
            { 
                if (isDuckAguaCoco && objectToBeCaptured.IsAllCapturedCocos)
                {
                    dialogManager.AvancarDialogoSilenciosamente();
                }
            }
            dialogManager.StartDialog();
            
        }
        
        private void AcessoriosPegos()
        {
            if (IsRescued)
            {
                if (isPatoEnterrado)
                    graphicBehaviour.IsBuried = false;
                
                if (acessorio)
                    acessorio.SetActive(true);
                
                if (ponto)
                    ponto.SetActive(false);
            }
        }
    }
}
