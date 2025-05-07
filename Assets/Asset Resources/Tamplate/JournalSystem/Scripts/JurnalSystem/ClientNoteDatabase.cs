using UnityEngine;
using System.Collections.Generic;

// [CreateAssetMenu(menuName = "Library/Client Note Database")]
public class ClientNoteDatabase : ScriptableObject
{
    public List<JournalNote> noteList = new List<JournalNote>();

    public void Save(JournalNote note)
    {
        noteList.Add(note);
    }

    public void Clear()
    {
        noteList.Clear();
    }

    public List<JournalNote> GetAll()
    {
        return noteList;
    }
}
