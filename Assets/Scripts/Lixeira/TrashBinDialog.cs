using System.Collections;
using MiniGame;
using Dialog.Manager;
using Duck;
using Interaction;
using Serialization;
using UnityEngine;

namespace Lixeira
{
    public class TrashBinDialog : InteractableObject, IInteraction, ISerializable
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
            duck.RemoveObject(duck.colisor);
            GameManager.SaveGameData();
        }

        public void Save(SaveData data)
        {
            data.isTrashDuckSaved = MiniGame.MiniGame.isFinished;
        }

        public void Load(SaveData data)
        {
            if (!data.isTrashDuckSaved) return;
            
            // MiniGame.MiniGame.isFinished = data.isTrashDuckSaved;
            // duck.gameObject.SetActive(true);
            // duck.RemoveObject(duck.colisor);
            // RemoveObject(colisor);
            // iconeInteracao.SetActive(false);
        }
        
        void OnDestroy()
        {
            GameManager.Unsubscribe(this);
            RemoveObject(colisor);
        }

        private IEnumerator Start()
        {
            duck.gameObject.SetActive(false);
            GameManager.Subscribe(this);
            AddObject(colisor, this);
            
            if(!GameManager.IsLoadingGameData)
                yield break;
			
            yield return new WaitWhile(() => GameManager.IsLoadingGameData);

            if (!MiniGame.MiniGame.isFinished)
                yield break;
            
            //RemoveObject(colisor);
            //iconeInteracao.SetActive(false);
        }
    }
}