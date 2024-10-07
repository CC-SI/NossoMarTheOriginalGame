using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Dialog
{
    /// <summary>
    /// Classe responsável por gerenciar diálogos no jogo, 
    /// incluindo exibição de texto e controle de fluxo de diálogos.
    /// </summary>
    public class DialogManager : MonoBehaviour
    {
        [SerializeField] private GameObject dialogManager;
        [SerializeField] private Text speakerName;
        [SerializeField] private Text dialogueText;
        [SerializeField] private GameObject joystick;
        [SerializeField] private Button advanceButton;
        [SerializeField] private GameObject helpDuck;
        
        private List<DialogueGroup> dialogueGroups = new List<DialogueGroup>(); // Lista de grupos de diálogos.
        private int currentDialogueIndex = 0; // Índice do diálogo atual dentro do grupo atual.
        private int currentGroupIndex = 0; // Índice do grupo de diálogos atual.
        
        /// <summary>
        /// Inicializa o gerenciador de diálogos ao iniciar o jogo.
        /// </summary>
        private void Start()
        {
            HideDialog();
            LoadDialogues();
            advanceButton.onClick.AddListener(OnAdvanceButtonClicked);
        }
        
        /// <summary>
        /// Método chamado quando o botão de avanço é clicado.
        /// Avança para o próximo diálogo.
        /// </summary>
        private void OnAdvanceButtonClicked()
        {
            ShowNextDialog();
        }
        
        /// <summary>
        /// Carrega os diálogos de um arquivo JSON no diretório de streaming assets.
        /// </summary>
        private void LoadDialogues()
        {
            string path = Path.Combine(Application.streamingAssetsPath, "dialogues.json");

            if (File.Exists(path))
            {
                string json = File.ReadAllText(path);
                DialogueData dialogueData = JsonUtility.FromJson<DialogueData>(json);
                dialogueGroups = dialogueData.dialogueGroups;
            }
        }
        
        /// <summary>
        /// Mostra o próximo diálogo da lista.
        /// </summary>
        private void ShowNextDialog()
        {
            // Verifica se o gerenciador de diálogo está ativo e se há grupos de diálogos disponíveis.
            if (dialogManager != null && currentGroupIndex < dialogueGroups.Count)
            {
                var currentGroup = dialogueGroups[currentGroupIndex]; // Obtém o grupo de diálogos atual.

                // Verifica se ainda há diálogos no grupo atual.
                if (currentDialogueIndex < currentGroup.dialogues.Count)
                {
                    dialogManager.SetActive(true); // Ativa o painel de diálogo.
                    joystick.SetActive(false); // Oculta o joystick durante o diálogo.
                    
                    var currentDialogue = currentGroup.dialogues[currentDialogueIndex]; // Obtém o diálogo atual.

                    // Atualiza o texto do falante.
                    if (speakerName != null)
                    {
                        speakerName.text = currentDialogue.speaker; 
                    }

                    // Atualiza o texto do diálogo.
                    if (dialogueText != null)
                    {
                        dialogueText.text = currentDialogue.text; 
                    }

                    // Verifica se o diálogo atual é sobre o pato enterrado e ativa o objeto de ajuda, se necessário.
                    if (dialogueGroups[currentGroupIndex].id == "patoenterrado" && currentDialogue.speaker == "Pato")
                    {
                        helpDuck.SetActive(true);
                    }
                    else
                    {
                        helpDuck.SetActive(false);
                    }

                    currentDialogueIndex++; // Avança para o próximo diálogo.
                }
                else
                {
                    // Se todos os diálogos do grupo foram exibidos, avança para o próximo grupo.
                    currentGroupIndex++;
                    currentDialogueIndex = 0;

                    // Verifica se ainda há grupos de diálogos disponíveis.
                    if (currentGroupIndex >= dialogueGroups.Count)
                    {
                        HideDialog(); // Se não houver mais grupos, oculta o diálogo.
                    }
                }
            }
            else
            {
                HideDialog(); // Oculta o diálogo se o gerenciador não estiver ativo.
            }
        }
        
        /// <summary>
        /// Oculta o painel de diálogo e redefine os índices dos diálogos.
        /// </summary>
        public void HideDialog()
        {
            if (dialogManager != null)
            {
                dialogManager.SetActive(false);
                currentDialogueIndex = 0;
                currentGroupIndex = 0;
                
                joystick.SetActive(true);
                
                helpDuck.SetActive(false);
            }
        }
        
        /// <summary>
        /// Exibe o diálogo correspondente a um ID específico.
        /// </summary>
        /// <param name="id">ID do grupo de diálogos a ser exibido.</param>
        public void ShowDialogForId(string id)
        {
            for (int i = 0; i < dialogueGroups.Count; i++)
            {
                if (dialogueGroups[i].id == id) 
                {
                    currentGroupIndex = i;
                    currentDialogueIndex = 0;
                    ShowNextDialog();
                    return;
                }
            }
        }
    }
}
