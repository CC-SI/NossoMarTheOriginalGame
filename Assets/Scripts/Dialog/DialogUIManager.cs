using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Dialog
{
    public class DialogUIManager : MonoBehaviour
    {
        [Header("Componentes Graficos")]
        [SerializeField] private GameObject caixaDialogo;
        [SerializeField] private Button fecharDialogo;
        [SerializeField] private Button btnSim;
        [SerializeField] private Button btnNao;
        
        [Header("Lista de botões de avançar diálogo")]
        [SerializeField] private List<Button> ListDeAvancarDialogo;
        
        [Header("Componentes de Dialogo")]
        [SerializeField] private TextMeshProUGUI textoDialogo;
        [SerializeField] private TextMeshProUGUI whoSpeak;

        [SerializeField] private DialogManager dialogManager;

        [SerializeField] private GameObject paOuChapeu;
        [SerializeField] private GameObject mensagemDePaColetadaOuChapeu;
        
        public void InitComponent()
        {
            caixaDialogo.SetActive(false);
            fecharDialogo.gameObject.SetActive(false);
            btnSim.gameObject.SetActive(false);
            btnNao.gameObject.SetActive(false);
            paOuChapeu.SetActive(false);
            mensagemDePaColetadaOuChapeu.SetActive(false);
          
            foreach (var btn in ListDeAvancarDialogo)
            {
                btn.gameObject.SetActive(false);
            }

            foreach (var zonas in ListDeAvancarDialogo)
            {
                zonas.onClick.AddListener(() =>
                {
                    dialogManager.NextDialog();
                });
            }
            
            fecharDialogo.onClick.AddListener(() =>
            {
                dialogManager.EndDialog();
            });
            
            btnSim.onClick.AddListener(() =>
            {
                dialogManager.NextDialog();
            });
            
            btnNao.onClick.AddListener(() =>
            {
                dialogManager.EndDialog();
            });
        }
        
        public void ShowPaColetada(bool show)
        {
            paOuChapeu.SetActive(show);
        }

        public void UpdateDialogUI(string speaker, string texto)
        {
            whoSpeak.text = speaker;
            textoDialogo.text = texto;
        }
        
        public void ShowYesNoButtons(bool show)
        {
            btnSim.gameObject.SetActive(show);
            btnNao.gameObject.SetActive(show);
        }
        
        public void ShowDialogUI(bool show)
        {
            caixaDialogo.SetActive(show);
            fecharDialogo.gameObject.SetActive(show);
            
            foreach (var btn in ListDeAvancarDialogo)
            {
                btn.gameObject.SetActive(show);
            }
        }
        
        public void ShowAvancarButton(bool show)
        {
            foreach (var btn in ListDeAvancarDialogo)
            {
                btn.gameObject.SetActive(show);
            }
        }
    }
}
