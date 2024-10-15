using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Dialog
{
    public class DialogManager : MonoBehaviour
    {
        [Header("Componentes de Interface")] [SerializeField]
        private GameObject dialogBox;

        [SerializeField] private Button btnClosedDialog;

        [Header("Botões de Decisão")]
        [SerializeField] private Button btnSim;
        [SerializeField] private Button btnNao;

        [Header("Componentes de Texto")] 
        [SerializeField]
        private TextMeshProUGUI dialogoText;
        [SerializeField] private TextMeshProUGUI whoSpeaks;

        [Header("Pá")] 
        [SerializeField] private GameObject pa;
        
        private List<DialogueGroup> dialogueGroups = new List<DialogueGroup>();
        private int currentGroupIndex = 0; // Índice do grupo de diálogos atual.
        private int currentDialogueIndex = 0; // Índice do diálogo atual dentro do grupo atual.

        private bool avancarDialogo = true;

        public bool IsDialogActive { get; private set; }

        public bool IsDialogFinshed { get; private set; }

        private string lastClosedDialogueId;

        public static DialogManager Instance { get; private set; }

        private void Awake()
        {
            // Garante que exista apenas uma instância do DialogManager.
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Destroy(this);
            }
        }

        private void Start()
        {
            // Inicializa o estado da interface e carrega os diálogos.
            dialogBox.SetActive(false);
            btnSim.gameObject.SetActive(false);
            btnNao.gameObject.SetActive(false);

            pa.gameObject.SetActive(false);
            
            btnClosedDialog.onClick.AddListener(HideDialog);
            btnSim.onClick.AddListener(ShowNextDialog); // Avança para o próximo diálogo ao clicar em sim.
            btnNao.onClick.AddListener(HideDialog); // Oculta o diálogo ao clicar em não.

            LoadDialogues(); // Carrega os diálogos ao iniciar.
        }

        private void Update()
        {
            // Avança o diálogo ao clicar na tela se o diálogo estiver ativo.
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
                IsDialogActive = true; // Marque o diálogo como ativo

                // Atualiza a interface com o falante e o texto
                whoSpeaks.text = currentDialogue.speaker; // Atualiza o nome do falante
                dialogoText.text = currentDialogue.text; // Atualiza o texto do diálogo

                Debug.Log(currentDialogue.id);

                btnSim.gameObject.SetActive(false);
                btnNao.gameObject.SetActive(false);
                avancarDialogo = true;

                HandleDuckDialogue(currentDialogue); // Lógica específica para diálogos do pato

                currentDialogueIndex++; // Avança para o próximo diálogo
            }
            else
            {
                // Finaliza o diálogo se não houver mais diálogos
                HideDialog(); 
            }
        }

        /// <summary>
        /// Manipula diálogos específicos relacionados ao pato.
        /// </summary>
        private void HandleDuckDialogue(Dialogue currentDialogue)
        {
            switch (currentDialogue.id)
            {
                case "pato_enterrado_pedindo_ajuda":
                    btnSim.gameObject.SetActive(true);
                    btnNao.gameObject.SetActive(true);
                    avancarDialogo = false;
                    break;
                case "encontrar_pa":
                    pa.gameObject.SetActive(true);
                    break;
            }
        }

        /// <summary>
        /// Retoma o diálogo que estava ativo antes de ser fechado.
        /// </summary>
        public void ResumeDialog()
        {
            // Encontra o diálogo que estava ativo antes de ser fechado
            for (int i = 0; i < dialogueGroups[currentGroupIndex].dialogues.Count; i++)
            {
                if (dialogueGroups[currentGroupIndex].dialogues[i].id == lastClosedDialogueId)
                {
                    currentDialogueIndex = i; // Define o índice do diálogo atual
                    break;
                }
            }

            // Verifica se o índice atual do diálogo está dentro dos limites
            if (currentDialogueIndex < dialogueGroups[currentGroupIndex].dialogues.Count)
            {
                ShowNextDialog(); // Mostra o próximo diálogo
            }
            else
            {
                HideDialog(); // Se o índice estiver fora do limite, oculta o diálogo
            }
        }

        /// <summary>
        /// Oculta o painel de diálogo.
        /// </summary>
        private void HideDialog()
        {
            if (currentDialogueIndex > 0)
            {
                lastClosedDialogueId = dialogueGroups[currentGroupIndex].dialogues[currentDialogueIndex - 1].id; // Salva o ID do diálogo fechado
            }
    
            dialogBox.SetActive(false); // Oculta o painel de diálogo
            IsDialogActive = false;
            avancarDialogo = true; // Permite que o diálogo reinicie quando necessário
            btnSim.gameObject.SetActive(false);
            btnNao.gameObject.SetActive(false);
        }
    }
}
