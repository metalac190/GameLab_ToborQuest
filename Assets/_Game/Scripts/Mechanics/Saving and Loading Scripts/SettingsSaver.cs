using UnityEngine;

public class SettingsSaver : PersistableObject
{
	private const int SaveVersion = 3;

    [SerializeField] private PersistentStorage _storage;

    [Header("Defaults")]
    [SerializeField] private float _masterVolumeDefault;
    [SerializeField] private float _musicVolumeDefault;
    [SerializeField] private float _sfxVolumeDefault;
    [SerializeField] private float _dialogueVolumeDefault;
    [SerializeField] private float _ambientVolumeDefault;
    [SerializeField, Range(0, 2)] private int _windowModeDefault;
	[SerializeField, Range(0, 2)] private int _qualityDefault;
	[SerializeField] private bool _bSideAudioDefault;
	[SerializeField] private float _dialogueScaleDefault;
	[SerializeField] private bool _disableDialogueDefault;

    public static float MasterVolume;
    public static float MusicVolume;
    public static float SfxVolume;
    public static float DialogueVolume;
    public static float AmbientVolume;
    public static int WindowMode;
	public static int Quality;
	public static bool BSideAudio;
	public static float DialogueScale;
	public static bool DisableDialogue;

	[Button]
    public void ButtonSave() => _storage.Save(this, SaveVersion);
    [Button]
    public void ButtonLoad()
    {
	    if (!_storage.Load(this)) LoadDefaults();
    }
    [Button]
    public void DeleteSave()
    {
	    _storage.DeleteStorage();
	    LoadDefaults();
    } 

    public override void Save(GameDataWriter writer)
	{
		Debug.Log("Saving Settings");
        writer.Write(MasterVolume);
        writer.Write(MusicVolume);
        writer.Write(SfxVolume);
        writer.Write(DialogueVolume);
        writer.Write(AmbientVolume);
        writer.Write(WindowMode);
	    writer.Write(Quality);
	    writer.Write(BSideAudio);
	    writer.Write(DialogueScale);
	    writer.Write(DisableDialogue);
    }

    [Button]
    public void LoadDefaults()
    {
	    Debug.Log("Loading Default Settings");
        MasterVolume = _masterVolumeDefault;
        MusicVolume = _musicVolumeDefault;
        SfxVolume = _sfxVolumeDefault;
        DialogueVolume = _dialogueVolumeDefault;
        AmbientVolume = _ambientVolumeDefault;
        WindowMode = _windowModeDefault;
        Quality = _qualityDefault;
        BSideAudio = _bSideAudioDefault;
	    DialogueScale = _dialogueScaleDefault;
	    DisableDialogue = _disableDialogueDefault;
        ButtonSave();
    }

    public override void Load(GameDataReader reader)
    {
        int version = reader.Version;
        if (version != SaveVersion)
        {
	        Debug.LogError($"Unsupported settings save version {version}");
            LoadDefaults();
            return;
        }
	    Debug.Log("Loading Saved Settings");
        MasterVolume = reader.ReadFloat();
        MusicVolume = reader.ReadFloat();
        SfxVolume = reader.ReadFloat();
        DialogueVolume = reader.ReadFloat();
        AmbientVolume = reader.ReadFloat();
        WindowMode = reader.ReadInt();
        Quality = reader.ReadInt();
        BSideAudio = reader.ReadBool();
	    DialogueScale = reader.ReadFloat();
	    DisableDialogue = reader.ReadBool();
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
	    Debug.Log($"BSideAudio: {BSideAudio}");
	    Debug.Log($"DialogueScale: {DialogueScale}");
	    Debug.Log($"DisableDialogue: {DisableDialogue}");
    }
}
