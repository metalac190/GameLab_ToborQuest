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
        if (GameOver)
        {
            Debug.Log("Cannot pause once game is over");
            return;
        }
            
        Paused = !Paused;
        if (Paused)
        {
            OnPause?.Invoke();
        }
        else
        {         
            OnUnpause?.Invoke();
        }
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
        PauseGameResponse();
        GameOver = true;
    }

    public static void LoseGame()
    {
        OnLoseGame?.Invoke();
        PauseGameResponse();
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

    [SerializeField] private int _currentQuest;
    [SerializeField] private int _currentLevel;

    public static void LoadMainMenu(bool async = false, Action onComplete = null)
    {
        Instance._currentQuest = -1;
        Instance._currentLevel = -1;
        LoadScene(Instance._mainMenu.Name, async, onComplete);
        UnpauseGameResponse();
    }

    public static void LoadFirstQuestLevel(int questIndex, bool async = false, Action onComplete = null)
    {
        Instance._currentQuest = questIndex;
        Instance._currentLevel = 0;
        LoadScene(Instance._quests[questIndex].GetLevelName(0), async, onComplete);
    }

    public static void LoadNextLevel() => Instance.LoadNextLevelActual();
    private void LoadNextLevelActual(bool async = false, Action onComplete = null)
    {
        if (_currentLevel < 0)
        {
            LoadFirstQuestLevel(_currentQuest < 0 ? 0 : _currentQuest);
            return;
        }
        var quest = Instance._quests[_currentQuest];
        _currentLevel++;
        
        if (_currentLevel >= quest.LevelNames.Count)
        {
            LoadMainMenu();
        }
        else
        {
            LoadScene(quest.GetLevelName(_currentLevel), async, onComplete);
        }
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
        Instance._currentQuest = questIndex;
        Instance._currentLevel = levelIndex;
        LoadScene(Instance._quests[questIndex].GetLevelName(levelIndex), async, onComplete);
    }

    public static void LoadNextSceneRaw(bool async = false, Action onComplete = null)
    {
        LoadScene(SceneManager.GetActiveScene().buildIndex + 1, async, onComplete);
    }

    public static void LoadScene(string sceneName, bool async = false, Action onComplete = null)
    {
        InMainMenu = sceneName.Equals(Instance._mainMenu.Name);
        if (async)
        {
            if (Instance) Instance.LoadSceneAsync(sceneName, onComplete);
            else Debug.LogError("CGSC Missing in Scene!");
        }
        else
        {
            SceneManager.LoadScene(sceneName);
            onComplete?.Invoke();
            UnpauseGameResponse();
        }
    }

    public static void LoadScene(int sceneIndex, bool async = false, Action onComplete = null)
    {
        InMainMenu = sceneIndex == 0;
        if (async)
        {
            if (Instance) Instance.LoadSceneAsync(sceneIndex, onComplete);
            else Debug.LogError("CGSC Missing in Scene!");
        }
        else
        {
            SceneManager.LoadScene(sceneIndex);
            onComplete?.Invoke();
            UnpauseGameResponse();
        }
    }

    private void LoadSceneAsync(string sceneName, Action onComplete)
    {
        var operation = SceneManager.LoadSceneAsync(sceneName);
        StartCoroutine(LoadSceneAsyncRoutine(operation, onComplete));
    }

    private void LoadSceneAsync(int sceneIndex, Action onComplete)
    {
        var operation = SceneManager.LoadSceneAsync(sceneIndex);
        StartCoroutine(LoadSceneAsyncRoutine(operation, onComplete));
    }
    
    private static IEnumerator LoadSceneAsyncRoutine(AsyncOperation operation, Action onComplete)
    {
        while (!operation.isDone)
        {
            yield return null;
        }
        onComplete?.Invoke();
        UnpauseGameResponse();
    }

    public static void RestartLevel(InputAction.CallbackContext context) => RestartLevel();
    public static void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        UnpauseGame();
        UnpauseGameResponse();
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
