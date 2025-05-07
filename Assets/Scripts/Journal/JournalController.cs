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

    public void AddJurnalNote(string sceneTitle, string content)
    {
        if (m_gameManager.playerData == null)
        {
            Debug.LogError("sriptable object is null");
            return;
        }

        var note = new JournalNote(sceneTitle, content);
        m_gameManager.playerData.SaveJurnalNotes(note);
        Debug.Log($"Menambahkan catatan: {sceneTitle} - {content}");
    }

    public void OnClickShowJurnalPanel()
    {
        if (contentContainer == null || notePrefab == null)
        {
            Debug.LogError("Content container atau Note prefab belum diset di Inspector!");
            return;
        }

        if (journalPanel != null)
            journalPanel.SetActive(true);
        else Debug.LogWarning($"journal panel is null");

        var notes = m_gameManager.playerData.GetAllJurnalNotes()
                        .Select(note => $"--------- {note.sceneTitle} ----------\n{note.content}")
                        .ToList();

        ClearCurrentNotes();

        foreach (string note in notes)
        {
            GameObject newNote = Instantiate(notePrefab, contentContainer);
            TextMeshProUGUI noteText = newNote.GetComponentInChildren<TextMeshProUGUI>();
            if (noteText != null)
                noteText.text = note;

            currentNotes.Add(newNote);
        }

        m_oldNotesCount = m_gameManager.playerData.GetAllJurnalNotes().Count;
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

    public bool HasNoteForScene(string sceneTitle)
    {
        return m_gameManager.playerData.GetAllJurnalNotes().Any(note => note.sceneTitle == sceneTitle);
    }

    public bool IsHasUnreadNotes()
    {
        return m_oldNotesCount < m_gameManager.playerData.GetAllJurnalNotes().Count;
    }
}
