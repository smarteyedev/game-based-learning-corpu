using System;
using System.Collections;
using System.Collections.Generic;
using Smarteye.Manager.taufiq;
using UnityEngine;

// [CreateAssetMenu(menuName = "Player/Player Data")]
public class PlayerData : ScriptableObject
{
    public FormatPlayerData Datas;

    public void SaveIVCAResult(string _company, string _ivcaResult)
    {
        Datas.targetCompany = _company;
        Datas.IVCAResult = _ivcaResult;
    }

    public string[] GetIVCAData()
    {
        string[] newData = new string[2];

        if (!String.IsNullOrEmpty(Datas.targetCompany) && !String.IsNullOrEmpty(Datas.targetCompany))
        {
            newData[0] = Datas.targetCompany;
            newData[1] = Datas.IVCAResult;
            return newData;
        }

        return null;
    }

    public void SaveJurnalNotes(List<JournalNote> _newNotes)
    {
        Datas.journalDatas.AddRange(_newNotes);
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