using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using TMPro;

public class JournalManager : MonoBehaviour
{
    // Singleton instance //
    private static JournalManager _instance;
    public static JournalManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<JournalManager>();
                if (_instance == null)
                {
                    GameObject go = new GameObject("JournalManager");
                    _instance = go.AddComponent<JournalManager>();
                }
            }
            return _instance;
        }
    }

    [Header("UI Settings")]
    public GameObject notePrefab;       // Prefab untuk tampilan catatan
    public Transform contentContainer;  // Kontainer untuk scroll view
    public GameObject journalPanel;     // Panel utama jurnal

    [Header("Database")]
    [SerializeField] private ClientNoteDatabase noteDatabase;

    private List<GameObject> currentNotes = new List<GameObject>();

    // Inisialisasi singleton saat scene dimuat //
    void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (_instance != this)
        {
            Destroy(gameObject);
        }
    }

    // =============================
    // FUNGSI UTAMA
    // =============================

    // Menambahkan catatan baru dari AI ke database // 
    // Di SCRIPT VISUAL NOVEL yang sudah ada
    // Misalnya di DialogueManager.cs atau AIResponseHandler.cs
    // TAMBAHKAN kode ini di fungsi yang menangani dialog di vs nya
    // (JournalManager.Instance.AddNoteFromAI(judulScene, responAI);) 
    public void AddNoteFromAI(string sceneTitle, string content)
    {
        if (noteDatabase == null)
        {
            Debug.LogError("Database belum diset di Inspector!");
            return;
        }

        var note = new JournalNote(sceneTitle, content);
        noteDatabase.Save(note);
        UpdateJournalDisplay();
        Debug.Log($"Menambahkan catatan: {sceneTitle} - {content}");
    }

    // Menampilkan jurnal untuk scene tertentu //
    public void ShowSceneJournal(string sceneName)
    {
        if (journalPanel != null)
            journalPanel.SetActive(true);

        var filteredNotes = noteDatabase.GetAll()
            .Where(note => note.sceneTitle == sceneName)
            .Select(note => $"--------- {note.sceneTitle} ----------\n{note.content}")
            .ToList();

        if (filteredNotes.Count == 0)
            filteredNotes.Add($"Tidak ada catatan untuk {sceneName}");

        ShowNotes(filteredNotes);
        Debug.Log($"Menampilkan {filteredNotes.Count} catatan untuk scene: {sceneName}");
    }

    // Update tampilan jurnal dengan semua catatan //
    public void UpdateJournalDisplay()
    {
        var allNotes = noteDatabase.GetAll()
            .Select(note => $"--------- {note.sceneTitle} ----------\n{note.content}")
            .ToList();

        ShowNotes(allNotes);
    }

    // Menampilkan catatan ke UI //
    private void ShowNotes(List<string> notes)
    {
        if (contentContainer == null || notePrefab == null)
        {
            Debug.LogError("Content container atau Note prefab belum diset di Inspector!");
            return;
        }

        ClearNotes();

        foreach (string note in notes)
        {
            GameObject newNote = Instantiate(notePrefab, contentContainer);
            TextMeshProUGUI noteText = newNote.GetComponentInChildren<TextMeshProUGUI>();
            if (noteText != null)
                noteText.text = note;

            currentNotes.Add(newNote);
        }
    }

    // Membersihkan catatan dari UI //
    private void ClearNotes()
    {
        foreach (GameObject note in currentNotes)
            Destroy(note);
        currentNotes.Clear();
    }

    // Cek apakah ada catatan untuk scene tertentu //
    public bool HasNoteForScene(string sceneTitle)
    {
        return noteDatabase.GetAll().Any(note => note.sceneTitle == sceneTitle);
    }

    // Mengatur visibilitas panel jurnal //
    public void ToggleJournalPanel(bool show)
    {
        if (journalPanel != null)
            journalPanel.SetActive(show);
    }

    // Menutup panel jurnal //
    public void CloseJournalPanel()
    {
        if (journalPanel != null)
            journalPanel.SetActive(false);
    }

    // Buka jurnal dan update tampilan sekaligus (untuk tombol UI) //
    public void OpenAndUpdateJournal()
    {
        ToggleJournalPanel(true);
        UpdateJournalDisplay();
    }

    // =============================
    // FUNGSI UNTUK TESTING / DUMMY
    // =============================

    // Simulasi penambahan catatan untuk testing //
    public void SimulateAddNotes()
    {
        AddNoteFromAI("Scene 1", "kamu memilih menyelamatkan desa.");
        AddNoteFromAI("Scene 2", "kamu menolak tawaran dari kerajaan.");
        Debug.Log("SimulateAddNotes dipanggil");
    }

    // Hapus semua catatan dari database //
    public void ClearJournal()
    {
        if (noteDatabase != null)
        {
            noteDatabase.Clear();
            UpdateJournalDisplay();
        }
    }
}
