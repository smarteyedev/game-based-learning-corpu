using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Smarteye.Manager.taufiq;
using Smarteye.VisualNovel.taufiq;
using TMPro;
using UnityEngine;

public class JournalController : MonoBehaviour
{
    private GameManager m_gameManager;
    private List<GameObject> currentNotes = new List<GameObject>();
    private List<JournalNote> temp_notes = new List<JournalNote>();
    private int m_oldNotesCount = 0;

    [Header("Component References")]
    [SerializeField] private GameObject notePrefab;       // Prefab untuk tampilan catatan
    [SerializeField] private Transform contentContainer;  // Kontainer untuk scroll view
    [SerializeField] private GameObject journalPanel;     // Panel utama jurnal

    private void Start()
    {
        m_gameManager = GameManager.instance;

        m_oldNotesCount = m_gameManager.playerData.GetAllJurnalNotes().Count;
    }

    public void AddJurnalNote(GameStage _gameStage, string content)
    {
        if (m_gameManager.playerData == null)
        {
            Debug.LogError("sriptable object is null");
            return;
        }

        var note = new JournalNote(_gameStage, content);
        temp_notes.Add(note);
    }

    public void SaveCurrentJurnalNote()
    {
        m_gameManager.playerData.SaveJurnalNotes(temp_notes);
    }

    public void OnClickShowJurnalPanel()
    {
        var notes = m_gameManager.playerData.GetAllJurnalNotes()
            .Select(note => $"--------- {m_gameManager.GenerateGameStageName(note.gameStage)} ----------\n{note.content}")
            .ToList();

        foreach (var item in temp_notes)
        {
            notes.Add($"--------- {m_gameManager.GenerateGameStageName(item.gameStage)} ----------\n{item.content}");
        }

        PrintNotes(notes);

        m_oldNotesCount = m_gameManager.playerData.GetAllJurnalNotes().Count + temp_notes.Count;
    }

    public void ShowSceneJournalPanelByStage(GameStage sceneName)
    {
        var filteredNotes = m_gameManager.playerData.GetAllJurnalNotes()
            .Where(note => note.gameStage == sceneName)
            .Select(note => $"--------- {m_gameManager.GenerateGameStageName(note.gameStage)} ----------\n{note.content}")
            .ToList();

        if (filteredNotes.Count == 0)
            filteredNotes.Add($"Tidak ada catatan untuk {sceneName}");

        PrintNotes(filteredNotes);
    }

    private void PrintNotes(List<string> notes)
    {
        if (contentContainer == null || notePrefab == null)
        {
            Debug.LogError("Content container atau Note prefab belum diset di Inspector!");
            return;
        }

        if (journalPanel != null)
            journalPanel.SetActive(true);
        else Debug.LogWarning($"journal panel is null");

        ClearCurrentNotes();

        foreach (string note in notes)
        {
            GameObject newNote = Instantiate(notePrefab, contentContainer);
            TextMeshProUGUI noteText = newNote.GetComponentInChildren<TextMeshProUGUI>();
            if (noteText != null)
                noteText.text = note;

            currentNotes.Add(newNote);
        }
    }

    public void OnClickCloseJournalPanel()
    {
        if (journalPanel != null)
            journalPanel.SetActive(false);
        else Debug.LogWarning($"journal panel is null");
    }

    private void ClearCurrentNotes()
    {
        foreach (GameObject note in currentNotes)
            Destroy(note);
        currentNotes.Clear();
    }

    public bool IsHasUnreadNotes()
    {
        return m_oldNotesCount < m_gameManager.playerData.GetAllJurnalNotes().Count + temp_notes.Count;
    }
}
