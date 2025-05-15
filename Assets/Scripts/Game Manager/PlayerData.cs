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

    public PlayerDataRoot GetPlayerData
    {
        get => Datas;
        private set => Datas = value;
    }

    public void SetupPlayerData(int _userId, string _playerName, GameStage _gameStageProgress, string _targetCompany,
    string _IVCAResult, int _profilingScore, int _rapportScore, int _probingScore, int _solutionScore,
    int _closingScore, string _scenarioData, List<JournalNote> _journalDatas)
    {
        Datas.userId = _userId;
        Datas.playerName = _playerName;
        Datas.gameStageProgress = _gameStageProgress;
        Datas.targetCompany = _targetCompany;
        Datas.IVCAResult = _IVCAResult;
        Datas.profilingScore = _profilingScore;
        Datas.rapportScore = _rapportScore;
        Datas.probingScore = _probingScore;
        Datas.solutionScore = _solutionScore;
        Datas.closingScore = _closingScore;
        Datas.scenarioJsonString = _scenarioData;
        Datas.journalDatas = _journalDatas == null ? new List<JournalNote>() : new List<JournalNote>(_journalDatas);
    }

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

    public int GetScoreByGameStage(GameStage _stage)
    {
        int result = _stage switch
        {
            GameStage.PROSPECTINGANDPROFILING => Datas.profilingScore,
            GameStage.RAPPORT => Datas.rapportScore,
            GameStage.PROBING => Datas.probingScore,
            GameStage.SOLUTION => Datas.solutionScore,
            GameStage.OBJECTIONANDCLOSING => Datas.closingScore,
            _ => 0
        };

        return result;
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
    public int userId;
    public string playerName;
    public GameStage gameStageProgress;
    public string targetCompany;
    public string IVCAResult;
    public int profilingScore = 0;
    public int rapportScore = 0;
    public int probingScore = 0;
    public int solutionScore = 0;
    public int closingScore = 0;
    [HideInInspector]
    public string scenarioJsonString = "";

    public List<JournalNote> journalDatas;
}