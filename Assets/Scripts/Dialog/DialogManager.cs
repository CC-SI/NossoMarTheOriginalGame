using System.Collections;
using System.Collections.Generic;
using System.IO;
using Dialog;
using Duck;
using Interaction;
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
        [SerializeField] private Button btnSim; 
        [SerializeField] private Button btnNao; 
        [SerializeField] private TextMeshProUGUI dialogoText; 
        [SerializeField] private TextMeshProUGUI whoSpeaks;
        [SerializeField] private GameObject pa;
        [SerializeField] private GameObject messagePaColetada;
        
        private List<DialogueGroup> dialogueGroups = new List<DialogueGroup>(); 
        private int currentGroupIndex = 0; 
        private int currentDialogueIndex = 0; 
        private bool isCollectPa; 
        private bool avancarDialogo = true;

        public bool IsDialogActive { get; private set; } 
        public bool IsDialogFinshed { get; private set; } 
        private string lastClosedDialogueId; 

        public static DialogManager Instance { get; private set; } 

        /// <summary>
        /// Garante que exista apenas uma instância do DialogManager.
        /// </summary>
        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Destroy(gameObject);
            }
        }

        /// <summary>
        /// Inicializa o estado da interface e carrega os diálogos.
        /// </summary>
        private void Start()
        {
            InitializeUI();
            LoadDialogues();
        }

        /// <summary>
        /// Inicializa a interface de diálogo e configura os botões.
        /// </summary>
        private void InitializeUI()
        {
            dialogBox.SetActive(false);
            btnSim.gameObject.SetActive(false);
            btnNao.gameObject.SetActive(false);
            
            pa.SetActive(false);
            messagePaColetada.SetActive(false);
            
            btnClosedDialog.onClick.AddListener(HideDialog);
            btnSim.onClick.AddListener(ShowNextDialog);
            btnNao.onClick.AddListener(HideDialog);

            IsDialogFinshed = false;
            isCollectPa = false;

            PlayerPrefs.DeleteKey("LastClosedDialogueId");
            PlayerPrefs.Save();
        }

        /// <summary>
        /// Atualiza o avanço do diálogo com clique na tela.
        /// </summary>
        private void Update()
        {
            if (dialogBox.activeSelf && avancarDialogo && Input.GetMouseButtonDown(0))
            {
                ShowNextDialog();
            }
        }

        /// <summary>
        /// Carrega os diálogos de um arquivo JSON localizado no diretório de streaming assets.
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
        /// Inicia o diálogo de um grupo específico pelo ID.
        /// </summary>
        /// <param name="groupId">ID do grupo de diálogos.</param>
        public void StartDialogue(string groupId)
        {
            string savedLastDialogue = PlayerPrefs.GetString("LastClosedDialogueId", "");

            if (!string.IsNullOrEmpty(savedLastDialogue))
            {
                DialogueGroup foundGroup = dialogueGroups.Find(group => group.dialogues.Exists(dialogue => dialogue.id == savedLastDialogue));
                if (foundGroup != null)
                {
                    groupId = foundGroup.id;
                }
            }

            for (int i = 0; i < dialogueGroups.Count; i++)
            {
                if (dialogueGroups[i].id == groupId)
                {
                    currentGroupIndex = i;
                    var dialogues = dialogueGroups[i].dialogues;

                    if (!string.IsNullOrEmpty(savedLastDialogue))
                    {
                        currentDialogueIndex = dialogues.FindIndex(d => d.id == savedLastDialogue);
                        if (currentDialogueIndex == -1)
                        {
                            currentDialogueIndex = 0;
                        }
                    }
                    else
                    {
                        currentDialogueIndex = 0;
                    }

                    ShowNextDialog();
                    return;
                }
            }

            Debug.LogError("Diálogo com ID de grupo não encontrado: " + groupId);
        }

        /// <summary>
        /// Exibe o próximo diálogo na sequência.
        /// </summary>
        public void ShowNextDialog()
        {
            var currentGroup = dialogueGroups[currentGroupIndex];
            if (currentDialogueIndex < currentGroup.dialogues.Count)
            {
                var currentDialogue = currentGroup.dialogues[currentDialogueIndex];
                dialogBox.SetActive(true);
                IsDialogActive = true;

                whoSpeaks.text = currentDialogue.speaker;
                dialogoText.text = currentDialogue.text;

                Debug.Log(currentDialogue.id);

                btnSim.gameObject.SetActive(false);
                btnNao.gameObject.SetActive(false);
                avancarDialogo = true;

                HandleDuckDialogue(currentDialogue);

                currentDialogueIndex++;
            }
            else
            {
                HideDialog();
            }
        }

        /// <summary>
        /// Manipula diálogos relacionados ao pato.
        /// </summary>
        /// <param name="currentDialogue">Diálogo atual a ser processado.</param>
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
                    if (isCollectPa)
                    {
                        pa.SetActive(false);
                        avancarDialogo = true;
                    }
                    else
                    {
                        pa.SetActive(true);
                        avancarDialogo = isCollectPa;
                    }
                    break;
                case "pato_enterrado_agradecendo":
                    DuckDialog.Instance.StartFollowing();
                    HideCaptureButton();
                    pa.gameObject.SetActive(false);
                    Destroy(pa);
                    break;
            }
        }

        /// <summary>
        /// Oculta o painel de diálogo e salva o progresso do diálogo atual.
        /// </summary>
        private void HideDialog()
        {
            if (currentDialogueIndex > 0)
            {
                lastClosedDialogueId = dialogueGroups[currentGroupIndex].dialogues[currentDialogueIndex - 1].id;
                PlayerPrefs.SetString("LastClosedDialogueId", lastClosedDialogueId);
                PlayerPrefs.Save();
            }

            dialogBox.SetActive(false);
            IsDialogActive = false;
            IsDialogFinshed = currentDialogueIndex >= dialogueGroups[currentGroupIndex].dialogues.Count;
        }

        /// <summary>
        /// Oculta o botão de captura.
        /// </summary>
        private void HideCaptureButton()
        {
            InteractionUIButton interactionUIButton = FindObjectOfType<InteractionUIButton>();
            if (interactionUIButton != null)
            {
                interactionUIButton.HideButton(); 
            }
        }

        /// <summary>
        /// Marca a pá como coletada.
        /// </summary>
        public void CollectPa()
        {
            isCollectPa = true;
            messagePaColetada.SetActive(true);
            StartCoroutine(HidePaColetadaMessageAfterDelay(5f));
        }
        
        private IEnumerator HidePaColetadaMessageAfterDelay(float delay)
        {
            yield return new WaitForSeconds(delay); 
            messagePaColetada.SetActive(false);
        }
    }
}
