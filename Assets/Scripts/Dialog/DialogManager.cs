using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Dialog
{
    public class DialogManager : MonoBehaviour
    {
        [Header("Componentes de Interface")]
        [SerializeField] private GameObject dialogBox;
        [SerializeField] private Button btnClosedDialog;

        [Header("Botões de Decisão")]
        [SerializeField] private Button btnSim;
        [SerializeField] private Button btnNao;

        [Header("Componentes de Texto")]
        [SerializeField] private TextMeshProUGUI dialogoText;
        [SerializeField] private TextMeshProUGUI whoSpeaks;

        private List<DialogueGroup> dialogueGroups = new List<DialogueGroup>();
        private int currentGroupIndex = 0; // Índice do grupo de diálogos atual.
        private int currentDialogueIndex = 0; // Índice do diálogo atual dentro do grupo atual.
        private int indiceDialogo = -1;
        
        private bool avancarDialogo = true;
        
        public bool IsDialogActive { get; private set; }

        public bool pa; // Variável pa configurada como false.
        
        private void Start()
        {
            dialogBox.SetActive(false);
            btnSim.gameObject.SetActive(false);
            btnNao.gameObject.SetActive(false);

            btnClosedDialog.onClick.AddListener(HideDialog);
            btnSim.onClick.AddListener(ShowNextDialog); // Avança para o próximo diálogo ao clicar em sim.
            btnNao.onClick.AddListener(HideDialog); // Oculta o diálogo ao clicar em não.

            LoadDialogues(); // Carrega os diálogos ao iniciar.
        }

        private void Update()
        {
            if (dialogBox.activeSelf && avancarDialogo && Input.GetMouseButtonDown(0))
            {
                ShowNextDialog();
            }
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
        /// Inicia o diálogo de um grupo de diálogos pelo ID.
        /// </summary>
        public void StartDialogue(string id)
        {
            // Encontra o grupo de diálogos pelo ID
            for (int i = 0; i < dialogueGroups.Count; i++)
            {
                if (dialogueGroups[i].id == id)
                {
                    currentGroupIndex = i;
                    currentDialogueIndex = 0;
                    indiceDialogo = currentDialogueIndex;
                    ShowNextDialog();
                    return;
                }
            }
        }

        /// <summary>
        /// Mostra o próximo diálogo da lista.
        /// </summary>
        public void ShowNextDialog()
        {
            var currentGroup = dialogueGroups[currentGroupIndex]; // Obtém o grupo de diálogos atual
            if (currentDialogueIndex < currentGroup.dialogues.Count)
            {
                var currentDialogue = currentGroup.dialogues[currentDialogueIndex]; // Obtém o diálogo atual
                dialogBox.SetActive(true); // Ativa o painel de diálogo
                IsDialogActive = false;
                whoSpeaks.text = currentDialogue.speaker; // Atualiza o nome do falante
                dialogoText.text = currentDialogue.text; // Atualiza o texto do diálogo

                btnSim.gameObject.SetActive(false);
                btnNao.gameObject.SetActive(false);
                avancarDialogo = true;

                HandleDuckDialogue(currentDialogue);
                
                indiceDialogo = currentDialogueIndex;
                currentDialogueIndex++; // Avança para o próximo diálogo
            }
            else
            {
                // Finaliza o diálogo e sinaliza que o pato pode seguir o jogador
                HideDialog(); // Oculta o diálogo se não houver mais diálogos
            }
        }

        private void HandleDuckDialogue(Dialogue currentDialogue)
        {
            switch (currentDialogue.id)
            {
                case "pato_enterrado_pedindo_ajuda":
                    btnSim.gameObject.SetActive(true);
                    btnNao.gameObject.SetActive(true);
                    avancarDialogo = false;
                    break;
                case "aceitar_ajuda":
                    // Lógica para aceitar ajuda
                    break;
                case "encontrar_pa":
                    // Lógica para encontrar pa
                    break;
            }
        }

        /// <summary>
        /// Oculta o painel de diálogo.
        /// </summary>
        private void HideDialog()
        {
            dialogBox.SetActive(false); // Oculta o painel de diálogo
            IsDialogActive = false;
            currentDialogueIndex = 0; // Reseta o índice do diálogo atual apenas ao iniciar um novo diálogo
            avancarDialogo = true; // Permite que o diálogo reinicie quando necessário
            btnSim.gameObject.SetActive(false); 
            btnNao.gameObject.SetActive(false);
        }
        
        /// <summary>
        /// Mostra o diálogo anterior.
        /// </summary>
        public void ShowDialogPrevious(int previousIndex)
        {
            if (previousIndex >= 0 && previousIndex < dialogueGroups[currentGroupIndex].dialogues.Count)
            {
                currentDialogueIndex = previousIndex; 
                ShowNextDialog(); 
            }
        }
    }
}
