using System.Collections.Generic;
using Serialization;
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

	private SaveData gameData;
	
	private static readonly List<ISerializable> serializable = new();
	
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
	
	public static void SaveGameData()
	{
		foreach(var save in serializable)
			save.Save(Instance.gameData);
		var json = Instance.gameData.ToJson();
		PlayerPrefs.SetString(GameDataKey, json);
	}
	
	public void LoadGameData()
	{
		var json = PlayerPrefs.GetString(GameDataKey);
		gameData.FromJson(json);
	}
	
	public static void LoadMainMenu()
	{
		LoadScene((int)GameState.Menu);
	}
	
	public static void LoadGame(bool loadData = false)
	{
		if (!loadData) 
			Instance.LoadGameData();
			
		LoadScene((int)GameState.Playing, loadData);
	}
	
	static void LoadScene(int index, bool loadData = false)
	{
		var operation = SceneManager.LoadSceneAsync(index);

		if (!Instance.transition)
			return;
		
		if (!loadData)
			operation.completed += _ => Instance.CompleteTransition();
		else
		{
			operation.completed += _ =>
			{
				foreach (var save in serializable)
					save.Load(Instance.gameData);
				
				Instance.CompleteTransition();
			};
		}
		
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

	public static void Subscribe(ISerializable save)
	{
		if (serializable.Contains(save)) return;
		
		serializable.Add(save);
	}
	
	public static void Unsubscribe(ISerializable save)
	{
		serializable.Remove(save);
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
		gameData = new SaveData();
	}
	
#if UNITY_EDITOR
	void Reset()
	{
		transition = GetComponentInChildren<TransitionController>(true);
	}
#endif
}