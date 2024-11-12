using Dialog.Manager;
﻿using Actors;
﻿using System.Collections;
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
        [SerializeField] private Animator animator;
        [SerializeField] private GraphicBehaviour graphicBehaviour;
        
        public bool isDuckAguaCoco;

        public bool isPatoEnterrado;
        
        private void Start()
        protected override IEnumerator Start()
        {
            OnDuckRescued += AcessoriosPegos;
            if (isPatoEnterrado)
                graphicBehaviour.IsBuried = true;

            yield return base.Start();
        }
        
        private void Awake()
        {
            OnDuckRescued += AcessoriosPegos;
        }

        private void OnDestroy()
        {
            OnDuckRescued -= AcessoriosPegos;
        }
        
        [ContextMenu("salvar pato")]
        public override void OnPlayerInteraction()
        {
            if (!IsFollowing)
            {
                if (objectToBeCaptured != null)
            if (IsFollowing) return;
            
            if (objectToBeCaptured)
            { 
                if (isDuckAguaCoco && objectToBeCaptured.IsAllCapturedCocos)
                {
                    if (isDuckAguaCoco && objectToBeCaptured.IsAllCapturedCocos)
                    {
                        dialogManager.AvancarDialogoSilenciosamente();
                    }
                    dialogManager.AvancarDialogoSilenciosamente();
                }
                
                dialogManager.StartDialog();
                
            }
                
            dialogManager.StartDialog();
        }
        
        private void AcessoriosPegos()
        {
            if (IsRescued)
            {
                if (isPatoEnterrado)
                    graphicBehaviour.IsBuried = false;
                
                if (acessorio != null)
                    acessorio.SetActive(true);
                
                if (ponto != null)
                    ponto.SetActive(false);
            }
        }
        
        public void StartFollowing()
        {
            // IsRescued = true;
            //
            // if (IsRescued)
            // {
            //     acessorio.SetActive(true);
            //     ponto.SetActive(false);
            // }
            
            base.OnPlayerInteraction();
        }
    }
}