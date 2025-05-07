using Smarteye.Manager.taufiq;

[System.Serializable]
public class JournalNote
{
    public GameStage gameStage;
    public string content;

    public JournalNote(GameStage _gameStage, string content)
    {
        this.gameStage = _gameStage;
        this.content = content;
    }
}
