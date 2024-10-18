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
        [SerializeField] private GameObject dialogBox; // Painel que exibe o diálogo.
        [SerializeField] private Button btnClosedDialog; // Botão para fechar o diálogo.

        [Header("Botões de Decisão")]
        [SerializeField] private Button btnSim; // Botão para a opção "Sim".
        [SerializeField] private Button btnNao; // Botão para a opção "Não".

        [Header("Componentes de Texto")]
        [SerializeField] private TextMeshProUGUI dialogoText; // Texto do diálogo.
        [SerializeField] private TextMeshProUGUI whoSpeaks; // Nome do falante.

        [Header("Pá")]
        [SerializeField] private GameObject pa; // Referência ao objeto da pá.

        [SerializeField] private Duck pato; // Referência ao objeto pato.
        private bool isCollectPa; // Flag para verificar se a pá foi coletada.
        
        // Lista que armazena os grupos de diálogos carregados.
        private List<DialogueGroup> dialogueGroups = new List<DialogueGroup>();
        private int currentGroupIndex = 0; // Índice do grupo de diálogos atual.
        private int currentDialogueIndex = 0; // Índice do diálogo atual dentro do grupo.

        private bool avancarDialogo = true; // Flag para controlar a navegação do diálogo.

        public bool IsDialogActive { get; private set; } // Indica se o diálogo está ativo.
        public bool IsDialogFinshed { get; private set; } // Indica se o diálogo foi finalizado.

        private string lastClosedDialogueId; // ID do último diálogo fechado.

        public static DialogManager Instance { get; private set; } // Instância singleton do DialogManager.

        private void Awake()
        {
            // Garante que exista apenas uma instância do DialogManager.
            if (Instance == null)
            {
                Instance = this; // Define a instância.
            }
            else
            {
                Destroy(this); // Destroi esta instância se já existir uma.
            }
        }

        private void Start()
        {
            // Inicializa o estado da interface e carrega os diálogos.
            dialogBox.SetActive(false); // Oculta o painel de diálogo no início.
            btnSim.gameObject.SetActive(false); // Oculta o botão "Sim".
            btnNao.gameObject.SetActive(false); // Oculta o botão "Não".
            pa.gameObject.SetActive(false); // Oculta a pá inicialmente.
            
            // Adiciona listeners aos botões para gerenciar eventos de clique.
            btnClosedDialog.onClick.AddListener(HideDialog);
            btnSim.onClick.AddListener(ShowNextDialog); // Avança para o próximo diálogo ao clicar em sim.
            btnNao.onClick.AddListener(HideDialog); // Oculta o diálogo ao clicar em não.

            IsDialogFinshed = false; // Inicializa a flag de diálogo como falso.
            isCollectPa = false; // Inicializa a flag de coleta da pá como falso.
            
            // Remove qualquer ID de diálogo salvo nos PlayerPrefs.
            PlayerPrefs.DeleteKey("LastClosedDialogueId");
            PlayerPrefs.Save();
            
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
            // Define o caminho para o arquivo de diálogos.
            string path = Path.Combine(Application.streamingAssetsPath, "dialogues.json");

            if (File.Exists(path))
            {
                // Lê o conteúdo do arquivo JSON e o converte para o objeto DialogueData.
                string json = File.ReadAllText(path);
                DialogueData dialogueData = JsonUtility.FromJson<DialogueData>(json);
                dialogueGroups = dialogueData.dialogueGroups; // Armazena os grupos de diálogos.
            }
        }

        /// <summary>
        /// Inicia o diálogo de um grupo de diálogos pelo ID.
        /// </summary>
        public void StartDialogue(string groupId)
        {
            string savedLastDialogue = PlayerPrefs.GetString("LastClosedDialogueId", "");

            // Verifica se há um diálogo salvo anteriormente.
            if (!string.IsNullOrEmpty(savedLastDialogue))
            {
                // Atualiza o grupo de diálogo pelo ID do último diálogo fechado.
                DialogueGroup foundGroup = dialogueGroups.Find(group => group.dialogues.Exists(dialogue => dialogue.id == savedLastDialogue));
                if (foundGroup != null)
                {
                    groupId = foundGroup.id; // Atualiza o ID do grupo para o encontrado.
                }
            }

            // Encontra o grupo de diálogos pelo ID do grupo.
            for (int i = 0; i < dialogueGroups.Count; i++)
            {
                if (dialogueGroups[i].id == groupId)
                {
                    currentGroupIndex = i; // Atualiza o índice do grupo atual.
                    var dialogues = dialogueGroups[i].dialogues;

                    // Define o índice do diálogo salvo, se existir.
                    if (!string.IsNullOrEmpty(savedLastDialogue))
                    {
                        currentDialogueIndex = dialogues.FindIndex(d => d.id == savedLastDialogue);

                        // Se não encontrar, começa do início.
                        if (currentDialogueIndex == -1)
                        {
                            currentDialogueIndex = 0;
                        }
                    }
                    else
                    {
                        currentDialogueIndex = 0; // Começa do início se não houver diálogo salvo.
                    }

                    ShowNextDialog(); // Exibe o próximo diálogo.
                    return;
                }
            }

            Debug.LogError("Diálogo com ID de grupo não encontrado: " + groupId);
        }

        /// <summary>
        /// Mostra o próximo diálogo da lista.
        /// </summary>
        public void ShowNextDialog()
        {
            var currentGroup = dialogueGroups[currentGroupIndex]; // Obtém o grupo de diálogos atual.
            if (currentDialogueIndex < currentGroup.dialogues.Count)
            {
                var currentDialogue = currentGroup.dialogues[currentDialogueIndex]; // Obtém o diálogo atual.
                dialogBox.SetActive(true); // Ativa o painel de diálogo.
                IsDialogActive = true; // Marca o diálogo como ativo.

                // Atualiza a interface com o falante e o texto.
                whoSpeaks.text = currentDialogue.speaker; // Atualiza o nome do falante.
                dialogoText.text = currentDialogue.text; // Atualiza o texto do diálogo.

                Debug.Log(currentDialogue.id); // Log do ID do diálogo atual.

                btnSim.gameObject.SetActive(false); // Oculta o botão "Sim".
                btnNao.gameObject.SetActive(false); // Oculta o botão "Não".
                avancarDialogo = true; // Permite o avanço do diálogo.

                HandleDuckDialogue(currentDialogue); // Lógica específica para diálogos do pato.

                currentDialogueIndex++; // Avança para o próximo diálogo.
            }
            else
            {
                HideDialog(); // Oculta o diálogo se não houver mais diálogos.
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
                    btnSim.gameObject.SetActive(true); // Ativa o botão "Sim".
                    btnNao.gameObject.SetActive(true); // Ativa o botão "Não".
                    avancarDialogo = false; // Impede o avanço do diálogo.
                    break;
                case "encontrar_pa":
                    pa.gameObject.SetActive(true); // Ativa a visualização da pá.
                    avancarDialogo = isCollectPa; // Permite o avanço se a pá foi coletada.
                    break;
                case "pato_enterrado_agradecendo":
                    // Captura o pato e o remove da cena após agradecer.
                    GameObject[] patosNarrativa = GameObject.FindGameObjectsWithTag("DuckBuriedTag");
                    if (patosNarrativa.Length > 0)
                    {
                        Duck patoComNarrativa = patosNarrativa[0].GetComponent<Duck>(); 
                        patoComNarrativa.CaptureDuck(); // Captura o pato.
                    }
                    Destroy(pa); // Remove a pá da cena.
                    break;
            }
        }

        /// <summary>
        /// Marca a pá como coletada.
        /// </summary>
        public void CollectPa()
        {
            isCollectPa = true; // Atualiza a flag de coleta da pá.
        }

        /// <summary>
        /// Oculta o painel de diálogo.
        /// </summary>
        private void HideDialog()
        {
            if (currentDialogueIndex > 0)
            {
                lastClosedDialogueId = dialogueGroups[currentGroupIndex].dialogues[currentDialogueIndex - 1].id; // Salva o ID do diálogo fechado.
        
                // Salva o último diálogo fechado em PlayerPrefs para continuar depois.
                PlayerPrefs.SetString("LastClosedDialogueId", lastClosedDialogueId);
                PlayerPrefs.Save();
            }

            dialogBox.SetActive(false); // Oculta o painel de diálogo.
            IsDialogActive = false; // Marca o diálogo como inativo.
            IsDialogFinshed = currentDialogueIndex >= dialogueGroups[currentGroupIndex].dialogues.Count; // Atualiza a flag de diálogo finalizado.
        }
    }
}
