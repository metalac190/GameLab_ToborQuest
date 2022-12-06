using System;
using UnityEngine;

public enum BestTime
{
    None,
    Quest,
    Level1,
    Level2,
    Level3,
    Level4,
    Tutorial
}

public class BestTimesSaver : PersistableObject
{
    private const int SaveVersion = 1;

    [SerializeField] private PersistentStorage _storage;

    [SerializeField, ReadOnly] private float _bestQuestTime;
    [SerializeField, ReadOnly] private float _bestLevel1Time;
    [SerializeField, ReadOnly] private float _bestLevel2Time;
    [SerializeField, ReadOnly] private float _bestLevel3Time;
    [SerializeField, ReadOnly] private float _bestLevel4Time;
    [SerializeField, ReadOnly] private float _bestTutorialTime;

    public static float GetBestTime(BestTime key) => CGSC.BestTimesSaver.GetBestTimeLocal(key);

    private float GetBestTimeLocal(BestTime key)
    {
        return key switch
        {
            BestTime.Quest => _bestQuestTime,
            BestTime.Level1 => _bestLevel1Time,
            BestTime.Level2 => _bestLevel2Time,
            BestTime.Level3 => _bestLevel3Time,
            BestTime.Level4 => _bestLevel4Time,
            BestTime.Tutorial => _bestTutorialTime,
            _ => 0
        };
    }

    public static bool TrySetBestTime(BestTime key, float time) => CGSC.BestTimesSaver.SetBestTimeLocal(key, time, false);
    public static bool ForceSetBestTime(BestTime key, float time) => CGSC.BestTimesSaver.SetBestTimeLocal(key, time, true);

    private bool SetBestTimeLocal(BestTime key, float time, bool force)
    {
        var best = GetBestTimeLocal(key);
        
        // If Best Time exists and is better than given time, do not save
        if (!force && best > 0 && best < time) return false;
        
        switch (key)
        {
            case BestTime.None:
                return false;
            case BestTime.Quest:
                _bestQuestTime = time;
                break;
            case BestTime.Level1:
                _bestLevel1Time = time;
                break;
            case BestTime.Level2:
                _bestLevel2Time = time;
                break;
            case BestTime.Level3:
                _bestLevel3Time = time;
                break;
            case BestTime.Level4:
                _bestLevel4Time = time;
                break;
            case BestTime.Tutorial:
                _bestTutorialTime = time;
                break;
        }
        CGSC.BestTimesSaver.ButtonSave();
        return true;
    }

    [Button]
    public void ButtonSave() => _storage.Save(this, SaveVersion);
    [Button]
    public void ButtonLoad()
    {
        if (!_storage.Load(this)) ResetAllTimes();
    }
    [Button]
    public void DeleteSave()
    {
        _storage.DeleteStorage();
        ResetAllTimes();
    }

    public override void Save(GameDataWriter writer)
    {
        //Debug.Log("Saving Best Times");
        writer.Write(_bestQuestTime);
        writer.Write(_bestLevel1Time);
        writer.Write(_bestLevel2Time);
        writer.Write(_bestLevel3Time);
        writer.Write(_bestLevel4Time);
        writer.Write(_bestTutorialTime);
    }

    public void ResetAllTimes()
    {
        //Debug.Log("Resetting Best Times");
        _bestQuestTime = 0;
        _bestLevel1Time = 0;
        _bestLevel2Time = 0;
        _bestLevel3Time = 0;
        _bestLevel4Time = 0;
        _bestTutorialTime = 0;
        ButtonSave();
    }

    public override void Load(GameDataReader reader)
    {
        int version = reader.Version;
        if (version != SaveVersion)
        {
            Debug.LogError($"Unsupported best times save version {version}");
            ResetAllTimes();
            return;
        }
        try
        {
            //Debug.Log("Loading Saved Best Times");
            _bestQuestTime = reader.ReadFloat();
            _bestLevel1Time = reader.ReadFloat();
            _bestLevel2Time = reader.ReadFloat();
            _bestLevel3Time = reader.ReadFloat();
            _bestLevel4Time = reader.ReadFloat();
            _bestTutorialTime = reader.ReadFloat();
        }
        catch (Exception e)
        {
            Debug.LogError($"Failed to load best times with version {version}");
            ResetAllTimes();
        }
    }
}
