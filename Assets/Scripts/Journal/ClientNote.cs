[System.Serializable]
public class JournalNote
{
    public string sceneTitle;
    public string content;

    public JournalNote(string sceneTitle, string content)
    {
        this.sceneTitle = sceneTitle;
        this.content = content;
    }
}
