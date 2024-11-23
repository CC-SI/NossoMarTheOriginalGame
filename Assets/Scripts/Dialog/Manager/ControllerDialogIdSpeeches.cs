using System.Collections;
using Duck;
using TMPro;
using UnityEngine;

namespace Dialog.Manager
{
    public class ControllerDialogIdSpeeches : MonoBehaviour
    {
        [SerializeField] private DialogObject dialogObject;
        [SerializeField] private DialogObject duckAguaCoco;
        [SerializeField] private DialogUIManager dialogUIManager;
        [SerializeField] private TutorialUI tutorialUI;
        
        [Header("Opcional")]
        [SerializeField] private PerguntasUIManager perguntasUIManager;
        [SerializeField] private Transform finalPoint;
        
        
        [SerializeField] private TextMeshProUGUI timeText;
        
        private bool isCountdownRunning = false;
        
        public void ControllerActionsForId()
        {
            string dialogId  = dialogObject.GetCurrentDialogId();
            
            if (!string.IsNullOrWhiteSpace(dialogId))
            {
                dialogUIManager.ShowButtonsOfDecision(false);
                dialogUIManager.ShowZonasDeAvancarDialogo(true);
                dialogUIManager.ShowPaOuChapeuOuCoco(false);
                dialogUIManager.ShowZonaDeFinalizarDialogo(false);
                
                if (perguntasUIManager)
                {
                    perguntasUIManager.isShowCoco = false;
                }
                
                string devicyType = PlayerPrefs.GetString("DeviceType", "Unknown");
                
                
                if (devicyType == "Mobile")
                {
                    dialogUIManager.ShowJoystick(true);
                }
                else
                {
                    dialogUIManager.ShowJoystick(false);
                }
                
                
                dialogUIManager.ShowDuckCaptured(true);
                dialogUIManager.ShowBoxTimeText(false);
                
                switch (dialogId)
                {
                    // Tutorial Mobile
                    case "primeira_instrucao":

                        if (devicyType == "Mobile")
                        {
                            dialogUIManager.ShowJoystick(true);
                        }
                        
                        dialogUIManager.ShowDuckCaptured(false);
                        break;
                    case "joystick":
                        dialogUIManager.ShowDuckCaptured(false);
                        
                        if (devicyType == "Mobile")
                        {
                            dialogUIManager.ShowJoystick(true);
                        }
                        break;
                    case "button_captured":
                        dialogUIManager.ShowZonasDeAvancarDialogo(false);
                        break;
                    case "voce_pega_jeito_rapido":
                        dialogUIManager.ShowZonasDeAvancarDialogo(true);
                        
                        if (!isCountdownRunning)
                        {
                            StartCoroutine(CountdownAndChangeScene(12f));
                        }
                        break;
                    
                    // Coqueiro
                    case "coqueiro1":
                        var currentDialogo = dialogObject.GetDialogoAtual();

                        string typeTexto = currentDialogo.texto;

                        if (duckAguaCoco != null)
                        {
                            var dialogo = duckAguaCoco.GetDialogoPorId("player_confirmando_agua_coco");
                            if (dialogo != null)
                            {
                                bool showCoco = dialogo.ShowCoco;

                                if (showCoco)
                                {
                                    typeTexto = "Um pato me contou que você venho aqui pegar cocos para ele, pois bem, responda às minhas perguntas e receberá um coco por cada resposta correta.";
                                } 
                                else
                                {
                                    FecharDialogo();
                                    
                                    string[] falas = new string[]
                                    {
                                        "A natureza é nossa maior riqueza, vamos preservá-la!", "Cada pequena ação faz uma grande diferença para o planeta.",
                                        "Ame a natureza e ela cuidará de você!", "Vamos juntos construir um mundo mais verde e saudável.",
                                        "Preservar o meio ambiente é preservar nossa própria existência.", "Plantar uma árvore hoje é garantir o ar puro de amanhã.",
                                        "Reduza, reutilize e recicle: pequenos gestos que ajudam o planeta!", "A natureza não precisa de nós, mas nós precisamos dela.",
                                        "Cuidar do meio ambiente é um ato de amor ao próximo.", "Vamos pensar nas próximas gerações e proteger nosso lar.",
                                        "A vida na Terra depende da nossa relação com o meio ambiente.", "Economizar água é um gesto simples que faz diferença!",
                                        "A biodiversidade é nosso bem mais precioso, vamos preservá-la.", "O meio ambiente agradece cada atitude consciente.",
                                        "Cuidar da Terra é garantir um futuro para todos os seres vivos.", "Poluir menos é um compromisso que devemos assumir!",
                                        "Reduzir o consumo de plástico é um passo importante para um planeta melhor.", "Conservar a natureza é conservar nossa história.",
                                        "Um mundo sustentável depende de todos nós!", "Proteja os rios, proteja a vida.", "Cada árvore plantada é um futuro garantido.",
                                        "Vamos ser a mudança que queremos ver no meio ambiente.", "Pequenas atitudes ecoam por todo o planeta.", 
                                        "Um planeta saudável é o melhor presente para as próximas gerações.", "Adote hábitos que respeitem a natureza!",
                                        "Proteger o meio ambiente é também cuidar de nós mesmos.", "O futuro verde começa com ações no presente.",
                                        "Nossa casa é a Terra, vamos tratá-la com respeito.", "Preservar a natureza é um dever de todos.",
                                        "Cada ser vivo tem seu papel, respeite e proteja todos eles."
                                    };

                                    typeTexto = falas[Random.Range(0, falas.Length)];
                                }
                            }
                        }
                        
                        dialogUIManager.SetSpeaches("Coqueiro", typeTexto);
                        break;
                    
                    case "perguntas_coqueiro":
                        dialogUIManager.ShowDialog(false);
                        dialogUIManager.ShowZonasDeAvancarDialogo(false);
                        
                        
                        if (duckAguaCoco != null)
                        {
                            var dialogo = duckAguaCoco.GetDialogoPorId("player_confirmando_agua_coco");
                            if (dialogo != null)
                            {
                                Debug.Log($"Dialogo encontrado: {dialogo.id}");
                                Debug.Log($"Show coco: {dialogo.ShowCoco}");

                                bool showCoco = dialogo.ShowCoco;

                                if (showCoco)
                                {
                                    perguntasUIManager.isShowCoco = true;
                                } 
                                else
                                {
                                    perguntasUIManager.isShowCoco = false;
                                }
                            }
                        }
                        
                        if (perguntasUIManager != null)
                        {
                            perguntasUIManager.ShowPainelPerguntas(true);
                        }
                
                        break;
                    
                    // Pato agua de coco
                    case "player_confirmando_agua_coco":
                        FecharDialogo();
                        duckAguaCoco.AtualizarShowCocoPorId("player_confirmando_agua_coco", true);
                        break;
                    
                    // Pato Lixo
                    case "fim_dialogo_lixo":
                        dialogUIManager.ShowDialog(false);
                        GameManager.LoadMiniGame();
                        break;
                    
                    // Pato Professor
                    case "pergunta":
                        dialogUIManager.ShowDialog(false);
                        dialogUIManager.ShowZonasDeAvancarDialogo(false);
                        
                        if (perguntasUIManager)
                        {
                            perguntasUIManager.ShowPainelPerguntas(true);
                        }
                        break;
                    
                    case "pato_professor2":
                        FecharDialogo();
                        
                        break;
                    
                    // Pato Enterrado
                    case "pato1_pedindo_ajuda":
                        dialogUIManager.ShowButtonsOfDecision(true);
                        dialogUIManager.ShowZonasDeAvancarDialogo(false);
                        break;
                    case "player3_procurando_pa":
                        dialogUIManager.ShowPaOuChapeuOuCoco(true);
                        FecharDialogo();
                        break;
                    
                    // Pato Madame
                    case "pata_madame1":
                        dialogUIManager.ShowButtonsOfDecision(true);
                        dialogUIManager.ShowZonasDeAvancarDialogo(false);
                        break;
                    case "player_madame1":
                        dialogUIManager.ShowPaOuChapeuOuCoco(true);
                        FecharDialogo();
                        break;
                    
                    // Dialogo Final
                    case "fim_dialogo_final":
                        dialogUIManager.ShowDialog(false);
                        DuckManager.SetEndDestination(finalPoint);
                        break;
                    
                    // Introducao
                    case "fim_introducao":
                        dialogUIManager.ShowDialog(false);
                        GameManager.LoadTutorial();
                        break;
                    
                    // Créditos
                    case "creditos_despedida":
                        dialogUIManager.ShowDialog(false);
                        break;
                }
            }
        }
        private IEnumerator CountdownAndChangeScene(float countdownTime)
        {
            dialogUIManager.ShowBoxTimeText(true);
            isCountdownRunning = true;
            float remainingTime = countdownTime;

            while (remainingTime > 0)
            {
                if (countdownTime != null)
                {
                    dialogUIManager.SetTimeText($"Saindo do Tutorial em {remainingTime}");
                }

                remainingTime -= 1f;
                yield return new WaitForSeconds(1f);  
            }
            ChangeScene();
        }
        
        
        void FecharDialogo()
        {
            dialogUIManager.ShowZonasDeAvancarDialogo(false);
            dialogUIManager.ShowZonaDeFinalizarDialogo(true);
        }
        
        private void ChangeScene()
        {
            GameManager.LoadGame();
        }
    }
}