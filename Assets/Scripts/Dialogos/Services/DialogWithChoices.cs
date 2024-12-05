using Assets.Scripts.Dialogos.Enum;
using UnityEngine;
using UnityEngine.UI;

namespace Dialogos.Services
{
    public class DialogWithChoices : DuckDialogBase
    {
        [SerializeField] private Button btnSim;
        [SerializeField] private Button btnNao;

        protected override void Start()
        {
            base.Start();
            
            btnSim.gameObject.SetActive(false);
            btnNao.gameObject.SetActive(false);
        }

        protected override void ShowDialogo()
        {
            base.ShowDialogo();

            bool isDecision = dialogoObject.GetDialogoAt(index).TipoDialogoEnum == TipoDialogoEnum.Decisao;
            btnSim.gameObject.SetActive(isDecision);
            btnNao.gameObject.SetActive(isDecision);
            botaoProximo.gameObject.SetActive(!isDecision);
        }

        protected override void ListenToEvents()
        {
            base.ListenToEvents();
            btnSim.onClick.AddListener(EscolherSim);
            btnNao.onClick.AddListener(EscolherNao);
        }

        void EscolherSim()
        {
            NextDialogo(); 
        }

        void EscolherNao()
        {
            OcultarDialogo();
        }
    }
}