using Transitions;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
	const string GameDataKey = "GameData";

	[SerializeField]
	TransitionController transition;
	
	static GameManager instance;
	
	static GameManager Instance
	{
		get
		{
			if (instance)
				return instance;
			
			GameObject go = new("GameManager");
			instance = go.AddComponent<GameManager>();

			return instance;
		}
	}
	
	public static GameState CurrentState { get; private set; }

	public static bool HasGameData => PlayerPrefs.HasKey(GameDataKey);

	public static void Exit()
	{
		Instance.Invoke(nameof(ExitGame), 1f);
	}
	
	public static void LoadMainMenu()
	{
		LoadScene((int)GameState.Menu);
	}
	
	public static void LoadGame()
	{
		LoadScene((int)GameState.Playing);
	}
	
	static void LoadScene(int index)
	{
		var operation = SceneManager.LoadSceneAsync(index);

		if (!Instance.transition)
			return;

		operation.completed += o => Instance.CompleteTransition();
		Instance.transition.Execute(operation);
	}
	
	public void ExitGame()
	{
#if UNITY_EDITOR
		if(Application.isPlaying)
			EditorApplication.isPlaying = false;
		
		return;
#else
		Application.Quit();
#endif
	}

	void CompleteTransition()
	{
		transition.Done();
	}

	void Awake()
	{
		if (instance)
		{
			Destroy(gameObject);
			return;
		}
		
		instance = this;
		DontDestroyOnLoad(gameObject);
	}

	void Start()
	{
		var scene = SceneManager.GetActiveScene();
		CurrentState = (GameState)scene.buildIndex;
	}
	
#if UNITY_EDITOR
	void Reset()
	{
		transition = GetComponentInChildren<TransitionController>(true);
	}
#endif
}