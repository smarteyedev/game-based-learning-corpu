using UnityEngine;
using UnityEngine.UI;

public class PhotoCarousel : MonoBehaviour
{
    [Header("UI Components")]
    public GameObject panelCarousel;     // Panel utama carousel (misalnya: PanelCarousel)
    public Image carouselImage;          // Image komponen untuk menampilkan foto
    public Button btnNext;               // Tombol panah kanan
    public Button btnPrev;               // Tombol panah kiri
    public Button btnClose;              // Tombol untuk menutup panel

    [Header("Content")]
    public Sprite[] photos;              // Daftar foto yang ditampilkan
    private int currentIndex = 0;        // Index saat ini

    private void Start()
    {
        InitCarousel(); // Setup awal
    }

    private void InitCarousel()
    {
        // Pastikan komponen tidak null
        if (carouselImage == null || photos == null || photos.Length == 0)
        {
            Debug.LogWarning("Carousel tidak memiliki gambar.");
            SetImageAlpha(0f); // Sembunyikan gambar
            return;
        }

        currentIndex = 0;
        ShowPhoto(currentIndex);

        // Tambahkan listener tombol
        btnNext?.onClick.AddListener(ShowNext);
        btnPrev?.onClick.AddListener(ShowPrev);
        btnClose?.onClick.AddListener(HideCarousel);
    }

    public void ShowPhoto(int index)
    {
        if (photos == null || photos.Length == 0 || index < 0 || index >= photos.Length)
            return;

        currentIndex = index;

        // Ganti gambar
        carouselImage.sprite = photos[currentIndex];
        SetImageAlpha(1f); // Tampilkan gambar

        // Tampilkan/Nonaktifkan tombol navigasi sesuai kondisi
        btnPrev?.gameObject.SetActive(currentIndex > 0);
        btnNext?.gameObject.SetActive(currentIndex < photos.Length - 1);
    }

    public void ShowNext()
    {
        if (currentIndex < photos.Length - 1)
        {
            ShowPhoto(currentIndex + 1);
        }
    }

    public void ShowPrev()
    {
        if (currentIndex > 0)
        {
            ShowPhoto(currentIndex - 1);
        }
    }

    public void ShowCarousel()
    {
        panelCarousel?.SetActive(true);
        ShowPhoto(0); // Reset ke foto pertama
    }

    public void HideCarousel()
    {
        panelCarousel?.SetActive(false);
    }

    private void SetImageAlpha(float alpha)
    {
        if (carouselImage != null)
        {
            Color c = carouselImage.color;
            c.a = alpha;
            carouselImage.color = c;
        }
    }
}
