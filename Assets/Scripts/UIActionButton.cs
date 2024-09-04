using UnityEngine;
using UnityEngine.UI;

public class UIActionButton : MonoBehaviour
{
    public static UIActionButton instance; // Instância da classe.

    [SerializeField] private Button btn; // Botão para o jogador interagir com o objeto.

    private IInteractable currentObject; // Objeto atual que o jogador irá interagir.

    void Awake() // Função de inicialização.
    {
        instance = this; // Definindo a instância da classe.
        btn.gameObject.SetActive(false);
        btn.onClick.AddListener(OnButtonClicked); // Adicionando um listener para o botão.
    }
    
    public void ShowButton(IInteractable objeto) // Função para mostrar o botão.
    {
        currentObject = objeto; // Definindo o objeto atual.
        btn.gameObject.SetActive(true);
    }
    
    public void HideButton() // Função para esconder o botão.
    {
        btn.gameObject.SetActive(false);
        currentObject = null; // Definindo que não há mais um objeto sendo interagido.
    }
    
    void OnButtonClicked() // Função para quando o botão for clicado.
    {
        if (currentObject != null) // Se tiver um objeto atualmente sendo interagido.
        {
            currentObject.OnInteract(); // Chamando a função de interação do objeto.
            HideButton();
        }
    }
}
