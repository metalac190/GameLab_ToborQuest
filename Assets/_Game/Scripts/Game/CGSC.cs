using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

// CGSC = Centralized Game State Controller
public class CGSC : MonoBehaviour
{
    #region Singleton and Unity Functions
    
    public static CGSC Instance;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        
        Instance = this;
        
	    DontDestroyOnLoad(gameObject);
    }

    private void OnValidate()
    {
        ValidateScenes();
        UpdateDebug();
    }

    private void Update()
    {
#if UNITY_EDITOR
        UpdateDebug();
#endif
    }

    #endregion

    #region Pausing

    public static bool Paused;
    
    public static Action OnPause = PauseGameResponse;
    public static Action OnUnpause = UnpauseGameResponse;

    public static void PauseGame()
    {
        if (!Paused) OnPause?.Invoke();
        Paused = true;
    }

    public static void TogglePauseGame(InputAction.CallbackContext context) => TogglePauseGame();
    public static void TogglePauseGame()
    {
        Paused = !Paused;
        if (Paused)
        {
            OnPause?.Invoke();
            //Debug.Log("Game Paused");
        }
        else
        {
            OnUnpause?.Invoke();
            //Debug.Log("Game Unpaused");
        }
        //Debug.Log("CGSC says game pause is " + Paused);
    }

    public static void UnpauseGame()
    {
        if (Paused) OnUnpause?.Invoke();
        Paused = false;
    }
    
    // A good reference for how to properly use Time.timeScale: https://youtu.be/ROwsdftEGF0
    private static void PauseGameResponse()
    {
        Time.timeScale = 0;
    }
    private static void UnpauseGameResponse()
    {
        Time.timeScale = 1;
    }

    #endregion

    #region Win / Lose
    
    public static bool GameOver;

    public static Action OnWinGame = delegate { };
    public static Action OnLoseGame = delegate { };
    
    public static void WinGame()
    {
        OnWinGame?.Invoke();
        GameOver = true;
    }

    public static void LoseGame()
    {
        OnLoseGame?.Invoke();
        GameOver = true;
    }

    #endregion

    #region Scenes

    public static bool InMainMenu;
    public static bool InGame => !InMainMenu;

    [SerializeField] private SerializedScene _mainMenu;
    [SerializeField] private List<Quest> _quests;

    public static List<Quest> Quests => Instance._quests;
    public static List<string> QuestNames => (from quest in Quests select quest.name).ToList();

    private void ValidateScenes()
    {
        _mainMenu.CheckValid(true);
        foreach (var quest in _quests)
        {
            quest.CheckLevelsValid();
        }
    }

    #endregion

    #region Scene Loading

    public static void LoadMainMenu(bool async = false, Action onComplete = null)
    {
        LoadScene(Instance._mainMenu.Name, async, onComplete);
    }

    public static void LoadQuestLevel(int questIndex, int levelIndex, bool async = false, Action onComplete = null)
    {
#if UNITY_EDITOR
        if (questIndex < 0 || levelIndex < 0 || questIndex >= Instance._quests.Count || levelIndex >= Instance._quests[questIndex].ValidLevels.Count)
        {
            Debug.LogError("Invalid Quest or Level Index");
            return;
        }
#endif
        LoadScene(Instance._quests[questIndex].GetLevelName(levelIndex), async, onComplete);
    }

    public static void LoadScene(string sceneName, bool async = false, Action onComplete = null)
    {
        if (async)
        {
            if (Instance) Instance.LoadSceneAsync(sceneName, onComplete);
            else Debug.LogError("CGSC Missing in Scene!");
        }
        else
        {
            SceneManager.LoadScene(sceneName);
            InMainMenu = sceneName.Equals(Instance._mainMenu.Name);
        }
    }

    private void LoadSceneAsync(string sceneName, Action onComplete)
    {
        StartCoroutine(LoadSceneAsyncRoutine(sceneName, onComplete));
    }
    
    private static IEnumerator LoadSceneAsyncRoutine(string sceneName, Action onComplete)
    {
        var operation = SceneManager.LoadSceneAsync(sceneName);
        while (!operation.isDone)
        {
            yield return null;
        }
        InMainMenu = sceneName.Equals(Instance._mainMenu.Name);
        onComplete?.Invoke();
    }

    public static void QuitGame()
    {
        Application.Quit();
        
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }

    #endregion

    #region Debug

    [SerializeField, ReadOnly] private bool _gamePaused;
    [SerializeField, ReadOnly] private bool _inMainMenu;
    [SerializeField, ReadOnly] private bool _gameOver;

    private void UpdateDebug()
    {
        _gamePaused = Paused;
        _inMainMenu = InMainMenu;
        _gameOver = GameOver;
    }

    [Button(Mode = ButtonMode.InPlayMode)]
    private void DebugLoadMainMenu() => LoadMainMenu();
    [Button(Mode = ButtonMode.InPlayMode)]
    private void DebugLoadQuestLevel(int quest, int level) => LoadQuestLevel(quest, level);

    #endregion
}
