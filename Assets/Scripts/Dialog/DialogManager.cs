using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.InputSystem.HID;
using UnityEngine.UI;

namespace Dialog
{
    /// <summary>
    /// Classe responsável por gerenciar diálogos no jogo, 
    /// incluindo exibição de texto e controle de fluxo de diálogos.
    /// </summary>
    public class DialogManager : MonoBehaviour
    {
        [Header("Componentes de Interface")]
        [SerializeField] private GameObject dialogManager;
        [SerializeField] private Text speakerName;
        [SerializeField] private Text dialogueText;
        [SerializeField] private GameObject joystick;
        [SerializeField] private GameObject helpDuck;

        [Header("Botões de Navegação")]
        [SerializeField] private Button advanceButton;
        [SerializeField] private Button backButton;
        [SerializeField] private GameObject navNextGameObject;
        [SerializeField] private GameObject navBackGameObject;
        [SerializeField] private Button closedDialog;

        [Header("Decisões de Diálogo")]
        [SerializeField] private GameObject dialogOptions;
        [SerializeField] private Button btnYes;
        [SerializeField] private Button btnNo;

        [Header("Mensagens Especiais")]
        [SerializeField] private GameObject messageFindFri;
        
        private List<DialogueGroup> dialogueGroups = new List<DialogueGroup>(); // Lista de grupos de diálogos.
        private int currentDialogueIndex = 0; // Índice do diálogo atual dentro do grupo atual.
        private int currentGroupIndex = 0; // Índice do grupo de diálogos atual.

        private bool isFriCollect = false; // Verifica se o jogador coletou a pá.
        
        /// <summary>
        /// Inicializa o gerenciador de diálogos ao iniciar o jogo.
        /// </summary>
        private void Start()
        {
            HideDialog();
            messageFindFri.SetActive(false);
            LoadDialogues();
            advanceButton.onClick.AddListener(OnAdvanceButtonClicked);
            btnYes.onClick.AddListener(OnAdvanceButtonClicked);
            btnNo.onClick.AddListener(HideDialog);
            backButton.onClick.AddListener(ShowPreviousDialog);
            closedDialog.onClick.AddListener(HideDialog);
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
        /// Volta para o diálogo anterior da lista.
        /// </summary>
        private void ShowPreviousDialog()
        {
            if (currentDialogueIndex > 0)
            {
                currentDialogueIndex -= 2;
                ShowNextDialog();
            }
            else
            {
                HideDialog();
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
                    messageFindFri.SetActive(false);
                    
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
                    if (currentDialogue.id == "pato_enterrado_pedindo_ajuda")
                    {
                        helpDuck.SetActive(true);
                        dialogOptions.SetActive(true);
                        navNextGameObject.SetActive(false);
                    }
                    else
                    {
                        helpDuck.SetActive(false);
                        dialogOptions.SetActive(false);
                        navNextGameObject.SetActive(true);
                    }

                    // Pula em 6 segundos caso o player não pulou pelo botão de avançar
                    if (currentDialogue.id == "pato_enterrado_pedindo_ajuda")
                    {
                        StartCoroutine(CountAndAdvanceDialog());
                    }

                    // Se o diálogo atual for "encontrar_pa" e isFriCollect for false, não avança.
                    if (currentDialogue.id == "encontrar_pa")
                    {
                        if (isFriCollect)
                        {
                            navBackGameObject.SetActive(true); // Permite voltar.
                            helpDuck.SetActive(false); // Oculta o objeto de ajuda.
                            dialogOptions.SetActive(false); // Oculta as opções de diálogo.
                            messageFindFri.SetActive(false); // Oculta a mensagem que o player precisa encontrar a pá.
                            currentDialogueIndex++; // Avança para o próximo diálogo se isFriCollect for true.
                        }
                        else
                        {
                            navNextGameObject.SetActive(false); // Desativa o botão de avanço.
                            navBackGameObject.SetActive(true); // Permite voltar.
                            helpDuck.SetActive(false); // Oculta o objeto de ajuda.
                            dialogOptions.SetActive(false); // Oculta as opções de diálogo.
                            messageFindFri.SetActive(true); // Exibe a mensagem que o player precisa encontrar a pá.
                        }
                    }
                    else
                    {
                        currentDialogueIndex++; // Avança para o próximo diálogo, exceto para "encontrar_pa".
                    }
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
        /// Conta 6 segundos e avança o diálogo.
        /// </summary>
        private IEnumerator CountAndAdvanceDialog()
        {
            for (int i = 0; i < 7; i++)
            {
                yield return new WaitForSeconds(1f); // Aguarda 1 segundo.
            }
            
            ShowNextDialog();
        }
        
        /// <summary>
        /// Oculta o painel de diálogo e redefine os índices dos diálogos.
        /// </summary>
        public void HideDialog()
        {
            if (dialogManager != null)
            {
                dialogManager.SetActive(false); // Oculta o painel de diálogo.
                currentDialogueIndex = 0; // Reseta o índice do diálogo atual.
                currentGroupIndex = 0; // Reseta o índice do grupo de diálogos.
                
                joystick.SetActive(true); // Exibe o joystick.
                helpDuck.SetActive(false); // Oculta o objeto de ajuda.
                dialogOptions.SetActive(false); // Oculta as opções de diálogo.

                if (isFriCollect == false && messageFindFri.activeSelf) // Se a pá não foi coletada e a mensagem está ativa.
                {
                    messageFindFri.SetActive(true); // Exibe a mensagem que o player precisa encontrar a pá.
                }
                else
                {
                    messageFindFri.SetActive(false); // Oculta a mensagem que o player precisa encontrar a pá.
                }
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
