using System;
using System.Collections;
using System.Collections.Generic;
using Smarteye.Manager.taufiq;
using UnityEngine;

// [CreateAssetMenu(menuName = "Player/Player Data")]
public class PlayerData : ScriptableObject
{
    [SerializeField] private string _playerToken;
    public string PlayerToken
    {
        get => _playerToken;
        set
        {
            if (_playerToken != value)
            {
                _playerToken = value;
            }
        }
    }

    [SerializeField] private PlayerDataRoot Datas;

    public string GetPlayerName()
    {
        return Datas.playerName;
    }

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

    public void UpdatePlayerScore(GameStage _stage, int _newScore)
    {
        switch (_stage)
        {
            case GameStage.None:
                break;
            case GameStage.PROSPECTINGANDPROFILING:
                Datas.profilingScore = _newScore;
                break;
            case GameStage.RAPPORT:
                Datas.rapportScore = _newScore;
                break;
            case GameStage.PROBING:
                Datas.probingScore = _newScore;
                break;
            case GameStage.SOLUTION:
                Datas.solutionScore = _newScore;
                break;
            case GameStage.OBJECTIONANDCLOSING:
                Datas.closingScore = _newScore;
                break;
        }
    }

    public int GetTotalScore()
    {
        return Datas.profilingScore + Datas.rapportScore + Datas.probingScore + Datas.solutionScore + Datas.closingScore;
    }

    public GameStage GetPlayerGameStageProgress()
    {
        return Datas.gameStageProgress;
    }

    public void SetPlayerGameStageProgress(GameStage _newProgress)
    {
        Datas.gameStageProgress = _newProgress;
    }
}

[Serializable]
public class PlayerDataRoot
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