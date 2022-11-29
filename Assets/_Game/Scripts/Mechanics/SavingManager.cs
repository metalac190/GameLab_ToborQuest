using UnityEngine;

public class SavingManager : PersistableObject
{
    private const int SaveVersion = 2;

    [SerializeField] private PersistentStorage _storage;

    [Header("Defaults")]
    [SerializeField] private float _masterVolumeDefault;
    [SerializeField] private float _musicVolumeDefault;
    [SerializeField] private float _sfxVolumeDefault;
    [SerializeField] private float _dialogueVolumeDefault;
    [SerializeField] private float _ambientVolumeDefault;
    [SerializeField, Range(0, 2)] private int _windowModeDefault;
    [SerializeField, Range(0, 2)] private int _qualityDefault;
    [SerializeField] private bool _disableDialogueDefault;
    [SerializeField] private float _dialogueScaleDefault;

    public static float MasterVolume;
    public static float MusicVolume;
    public static float SfxVolume;
    public static float DialogueVolume;
    public static float AmbientVolume;
    public static int WindowMode;
    public static int Quality;
    public static bool DisableDialogue;
    public static float DialogueScale;

    private void Awake()
    {
        if (!_storage.Load(this)) LoadDefaults();
    }

    [Button]
    public void ButtonSave() => _storage.Save(this, SaveVersion);
    [Button]
    public void ButtonLoad() => _storage.Load(this);
    [Button]
    public void DeleteSave() => _storage.DeleteStorage();

    public override void Save(GameDataWriter writer)
    {
        writer.Write(MasterVolume);
        writer.Write(MusicVolume);
        writer.Write(SfxVolume);
        writer.Write(DialogueVolume);
        writer.Write(AmbientVolume);
        writer.Write(WindowMode);
        writer.Write(Quality);
        writer.Write(DisableDialogue);
        writer.Write(DialogueScale);
    }

    [Button]
    public void LoadDefaults()
    {
        Debug.Log("Loading Default Values on Save System");
        MasterVolume = _masterVolumeDefault;
        MusicVolume = _musicVolumeDefault;
        SfxVolume = _sfxVolumeDefault;
        DialogueVolume = _dialogueVolumeDefault;
        AmbientVolume = _ambientVolumeDefault;
        WindowMode = _windowModeDefault;
        Quality = _qualityDefault;
        DisableDialogue = _disableDialogueDefault;
        DialogueScale = _dialogueScaleDefault;
        ButtonSave();
    }

    public override void Load(GameDataReader reader)
    {
        int version = reader.Version;
        if (version != SaveVersion)
        {
            Debug.LogError($"Unsupported save version {version}, loading default values");
            LoadDefaults();
            return;
        }
        MasterVolume = reader.ReadFloat();
        MusicVolume = reader.ReadFloat();
        SfxVolume = reader.ReadFloat();
        DialogueVolume = reader.ReadFloat();
        AmbientVolume = reader.ReadFloat();
        WindowMode = reader.ReadInt();
        Quality = reader.ReadInt();
        DisableDialogue = reader.ReadBool();
        DialogueScale = reader.ReadFloat();
    }

    [Button]
    private void DebugValues()
    {
        Debug.Log($"MasterVolume: {MasterVolume}");
        Debug.Log($"MusicVolume: {MusicVolume}");
        Debug.Log($"SfxVolume: {SfxVolume}");
        Debug.Log($"DialogueVolume: {DialogueVolume}");
        Debug.Log($"AmbientVolume: {AmbientVolume}");
        Debug.Log($"WindowMode: {WindowMode}");
        Debug.Log($"Quality: {Quality}");
        Debug.Log($"DisableDialogue: {DisableDialogue}");
        Debug.Log($"DialogueScale: {DialogueScale}");
    }
}
