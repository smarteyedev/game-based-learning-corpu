using System;
using System.Collections;
using System.Collections.Generic;
using Smarteye.Manager.taufiq;
using UnityEngine;

// [CreateAssetMenu(menuName = "Player/Player Data")]
public class PlayerData : ScriptableObject
{
    public FormatPlayerData Datas;

    public void SaveJurnalNotes(JournalNote note)
    {
        Datas.journalDatas.Add(note);
    }

    public void Clear()
    {
        Datas.journalDatas.Clear();
    }

    public List<JournalNote> GetAllJurnalNotes()
    {
        return Datas.journalDatas;
    }
}

[Serializable]
public class FormatPlayerData
{
    public string playerName;
    public GameStage gameStageProgress;
    public string targetCompany;
    public string IVCAResult;
    public int profilingScore = 0;
    public int rapportScore = 0;
    public int probingScore = 0;
    public int solutionScore = 0;
    public int closingScore = 0;

    public List<JournalNote> journalDatas;
}