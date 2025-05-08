using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using Smarteye.VisualNovel.Character;

namespace Smarteye.MycoonController.taufiq
{
    public class MycoonController : MonoBehaviour
    {
        [SerializeField] private List<ContentData> panelContentDatas;
        private int currentContentIndex;
        private Action onInfoCompleted;

        [Header("Component References")]
        [SerializeField] private CanvasGroup panelCanvasGroup;
        [SerializeField] private TextMeshProUGUI titleText;
        [SerializeField] private TextMeshProUGUI explanationText;
        [SerializeField] private Button buttonNext;
        [SerializeField] private Image mycoonImage;

        [Space]
        [SerializeField] private CharacterData characterData;

        [Serializable]
        public class ContentData
        {
            public string title;
            [TextArea(10, 10)]
            public string explanation;
            public CharacterIdentity.CharacterRole role = CharacterIdentity.CharacterRole.NONE;
            public CharacterIdentity.Action.ActionType actionType = CharacterIdentity.Action.ActionType.NONE;
        }

        private Sequence m_sequence;

        public void SetupPanelInfo(List<ContentData> _datas, Action _onCompleted = null)
        {
            FadeInCanvasGroup(panelCanvasGroup, 2f, () => buttonNext.gameObject.SetActive(true));

            panelContentDatas.AddRange(_datas);

            AssignContent(0);
            onInfoCompleted = _onCompleted;
        }

        private void Awake()
        {
            panelCanvasGroup.alpha = 0;
        }

        private void Start()
        {
            panelCanvasGroup.alpha = 0;
            currentContentIndex = 0;
            buttonNext.onClick.AddListener(OnClickNext);
        }

        void OnDisable()
        {
            // Hentikan semua animasi jika objek dinonaktifkan
            if (m_sequence != null)
            {
                m_sequence.Kill();
            }

            panelCanvasGroup.alpha = 0;
            currentContentIndex = 0;
        }

        private void AssignContent(int _index)
        {
            if (!string.IsNullOrEmpty(panelContentDatas[_index].title))
                titleText.text = panelContentDatas[_index].title;
            else titleText.gameObject.SetActive(false);

            explanationText.text = panelContentDatas[_index].explanation;

            if (panelContentDatas[_index].role != CharacterIdentity.CharacterRole.NONE
                && panelContentDatas[_index].actionType != CharacterIdentity.Action.ActionType.NONE)
            {
                Sprite img = characterData.GetCharacter(panelContentDatas[_index].role, panelContentDatas[_index].actionType);
                if (img != null && panelContentDatas[_index].role != CharacterIdentity.CharacterRole.PLAYER)
                {
                    mycoonImage.sprite = img;
                }
            }
            else
            {
                mycoonImage.gameObject.SetActive(false);
            }
        }

        private void OnClickNext()
        {
            if (currentContentIndex == panelContentDatas.Count - 1)
            {
                onInfoCompleted?.Invoke();
                // Debug.Log($"next scenee.... | current content {currentContentIndex}");

                DestroyMycoonPanel();
            }
            else
            {
                currentContentIndex++;
                AssignContent(currentContentIndex);
            }
        }

        private void FadeInCanvasGroup(CanvasGroup _cg, float _duration, Action _onComplate = null)
        {
            if (m_sequence != null)
            {
                Debug.Log($"there is canvas group");
                m_sequence.Kill();
            }

            m_sequence = DOTween.Sequence();

            m_sequence.Append(_cg.DOFade(1f, _duration).SetEase(Ease.InOutQuad)
                .OnComplete(() =>
                {
                    _onComplate?.Invoke();
                }));
        }

        public void DestroyMycoonPanel()
        {
            Destroy(this.gameObject);
        }
    }
}