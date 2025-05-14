using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class BadgeAnimationManager : MonoBehaviour
{
    [System.Serializable]
    public class AnimationTarget
    {
        public string badgeName;                   // Nama badge: Gold, Silver, Bronze, None
        public GameObject panel;                   // Panel UI untuk badge ini
        public CanvasGroup contentGroup;           // Grup isi konten
        public RectTransform rectangleTransform;   // Objek yg bakal berputar
        public float rotationSpeed = 5f;           // Kecepatan rotasi di set
        public float fadeDuration = 0.5f;          // Durasi fade in
        public float buttonDelay = 5f;             // Delay sebelum tombol muncul
        public CanvasGroup teksGroup;              // Grup teks
        public Button btnSelanjutnya;              // Tombol "Selanjutnya"
    }

    //=====================//
    //  BAGIAN UNTUK TES   //
    //=====================//

    [Header("Testing Score (Optional)")]
    public int currentScore = 0;
    public bool autoShowOnStart = true; // true = animasi badge otomatis muncul di Start()

    //=====================//
    //     KONFIGURASI     //
    //=====================//

    [Header("Badge Thresholds")]
    public List<string> badgeNames;     // Contoh: ["Gold", "Silver", "Bronze", "None"]
    public List<int> thresholds;        // Contoh: [17, 13, 9, 0]

    private Dictionary<string, int> badgeThresholds = new Dictionary<string, int>();

    [Header("Badge Targets")]
    public List<AnimationTarget> badgeTargets = new List<AnimationTarget>();

    [Header("Audio")]
    public AudioSource audioSource;
    public AudioClip clickSound;

    // Internal references
    private CanvasGroup badgeGroup;
    private RectTransform badgeTransform;
    private CanvasGroup rectangleGroup;
    private CanvasGroup btnSelanjutnyaGroup;

    private bool isRotating = false;
    private RectTransform rotatingTarget;
    private float currentRotationSpeed = 0f;

    //=====================//
    //       LIFECYCLE     //
    //=====================//

    void Awake()
    {
        // Mapping badge name ke threshold
        badgeThresholds.Clear();
        for (int i = 0; i < Mathf.Min(badgeNames.Count, thresholds.Count); i++)
        {
            badgeThresholds[badgeNames[i].ToLower()] = thresholds[i];
        }
    }

    void Start()
    {
        if (autoShowOnStart)
        {
            ShowBadge(currentScore); // Untuk tes awal
        }
    }

    void Update()
    {
        if (isRotating && rotatingTarget != null)
        {
            rotatingTarget.Rotate(Vector3.forward, currentRotationSpeed * Time.deltaTime);
        }
    }

    //=========================//
    //    FUNGSI UTAMA         //
    //=========================//

    public void ShowBadge(int score)
    {
        AnimationTarget target = GetBadgeTargetByScore(score);
        if (target != null)
        {
            StartCoroutine(PlayAnimation(target));
        }
        else
        {
            Debug.LogWarning($"No badge target found for score {score}");
        }
    }

    //=========================//
    //  LOGIKA PILIH BADGE     //
    //=========================//

    AnimationTarget GetBadgeTargetByScore(int score)
    {
        AnimationTarget selected = null;
        int selectedThreshold = int.MinValue;

        foreach (var target in badgeTargets)
        {
            string badgeKey = target.badgeName.ToLower();
            if (badgeThresholds.ContainsKey(badgeKey))
            {
                int threshold = badgeThresholds[badgeKey];
                if (score >= threshold && threshold > selectedThreshold)
                {
                    selected = target;
                    selectedThreshold = threshold;
                }
            }
        }

        // Fallback ke badge "None"
        if (selected == null)
        {
            foreach (var target in badgeTargets)
            {
                if (target.badgeName.ToLower() == "none")
                    return target;
            }
        }

        return selected;
    }

    //=========================//
    //   ANIMASI UTAMA BADGE   //
    //=========================//

    IEnumerator PlayAnimation(AnimationTarget target)
    {
        if (target.badgeName.ToLower() == "none")
        {
            Debug.Log("No badge awarded, score too low.");
        }

        target.panel.SetActive(true);

        badgeGroup = target.panel.GetComponent<CanvasGroup>();
        badgeTransform = target.panel.GetComponent<RectTransform>();
        rectangleGroup = target.rectangleTransform.GetComponent<CanvasGroup>();
        btnSelanjutnyaGroup = target.btnSelanjutnya.GetComponent<CanvasGroup>();

        // Reset awal
        badgeGroup.alpha = 0;
        badgeTransform.localScale = Vector3.zero;
        target.contentGroup.alpha = 0;
        target.contentGroup.transform.localScale = Vector3.zero;
        rectangleGroup.alpha = 0;
        target.teksGroup.alpha = 0;
        btnSelanjutnyaGroup.alpha = 0;

        // Badge muncul
        yield return StartCoroutine(FadeAndScale(badgeGroup, badgeTransform, target.fadeDuration));

        // Mulai rotasi animasi rectangle
        rotatingTarget = target.rectangleTransform;
        currentRotationSpeed = target.rotationSpeed;
        isRotating = true;

        yield return StartCoroutine(Fade(rectangleGroup, 0.3f));

        // Tampilkan isi dan teks
        target.contentGroup.alpha = 1;
        target.contentGroup.transform.localScale = Vector3.one;
        yield return StartCoroutine(Fade(target.teksGroup, 0.3f));

        // Delay tombol
        yield return new WaitForSeconds(target.buttonDelay);
        yield return StartCoroutine(Fade(btnSelanjutnyaGroup, 0.5f));

        // Set efek dan suara tombol
        target.btnSelanjutnya.onClick.RemoveAllListeners();
        target.btnSelanjutnya.onClick.AddListener(() =>
        {
            if (audioSource != null && clickSound != null)
            {
                audioSource.PlayOneShot(clickSound);
            }

            StartCoroutine(PlayButtonClickEffect(target.btnSelanjutnya.transform));
        });
    }

    //=========================//
    //    ANIMASI BANTUAN      //
    //=========================//

    IEnumerator FadeAndScale(CanvasGroup group, RectTransform target, float duration)
    {
        float time = 0f;
        Vector3 startScale = Vector3.zero;
        Vector3 endScale = Vector3.one;

        while (time < duration)
        {
            float t = time / duration;
            group.alpha = Mathf.SmoothStep(0, 1, t);
            target.localScale = Vector3.Lerp(startScale, endScale, t);
            time += Time.deltaTime;
            yield return null;
        }

        group.alpha = 1;
        target.localScale = endScale;
    }

    IEnumerator Fade(CanvasGroup group, float duration)
    {
        float time = 0f;
        while (time < duration)
        {
            float t = time / duration;
            group.alpha = Mathf.SmoothStep(0, 1, t);
            time += Time.deltaTime;
            yield return null;
        }
        group.alpha = 1;
    }

    IEnumerator PlayButtonClickEffect(Transform buttonTransform)
    {
        Vector3 originalScale = buttonTransform.localScale;
        Vector3 punchScale = originalScale * 1.1f;
        float duration = 0.15f;

        float time = 0f;
        while (time < duration)
        {
            float t = time / duration;
            buttonTransform.localScale = Vector3.Lerp(originalScale, punchScale, t);
            time += Time.deltaTime;
            yield return null;
        }

        time = 0f;
        while (time < duration)
        {
            float t = time / duration;
            buttonTransform.localScale = Vector3.Lerp(punchScale, originalScale, t);
            time += Time.deltaTime;
            yield return null;
        }

        buttonTransform.localScale = originalScale;
        Debug.Log("Button clicked with effect!");
    }
}
