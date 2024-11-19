using System.Collections;
using Dialog.Manager;
using Duck;
using Interaction;
using UnityEngine;

namespace Lixeira
{
    public class TrashBinDialog : InteractableObject, IInteraction
    {
        [SerializeField] private DialogManager dialogManager;
        [SerializeField] private DuckBehavior duck;
        [SerializeField] private Collider2D colisor;
        [SerializeField] private GameObject iconeInteracao;
        
        public void OnPlayerInteraction()
        {
            GameManager.SaveGameData();
            dialogManager.StartDialog();
        }

        public void EnableDuck()
        {
            duck.gameObject.SetActive(true);
            duck.StartFollowing();
            GameManager.SaveGameData();
        }
        
        void OnDestroy()
        {
            RemoveObject(colisor);
        }

        private IEnumerator Start()
        {
            // É um jogo novo? Se sim, parar processo.
            if(!GameManager.IsLoadingGameData)
            {
                duck.gameObject.SetActive(false);
                AddObject(colisor, this);
                yield break;
            }
            
            yield return new WaitWhile(() => GameManager.IsLoadingGameData);
            
            // O pato já está salvo? Se sim, desativar o ícone de interação e parar processo.
            if (duck.IsRescued)
            {
                iconeInteracao.SetActive(false);
                yield break;
            }
            
            duck.gameObject.SetActive(false);
            
            // O minigame já foi finalizado? se sim, soltar o pato, desativar o ícone de interação e parar processo.
            if (MiniGame.MiniGame.isFinished)
            {
                EnableDuck();
                iconeInteracao.SetActive(false);
                yield break;
            }
            
            // Adicionar o colisor na lista de objetos interativos.
            AddObject(colisor, this);
        }
    }
}