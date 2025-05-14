using System.Collections;
using System.Collections.Generic;
using Smarteye.Manager.taufiq;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

namespace Smarteye.SceneController.taufiq
{
    public class MenuController : SceneControllerAbstract
    {
        [Header("Panel Welcoming Config")]
        private int m_panelIndex = 0;

        [Header("Menu Controller Component References")]
        [SerializeField] private GameObject[] panelWelcoming;
        [SerializeField] private TextMeshProUGUI textPlayerName;

        [Header("Unity Event")]
        public UnityEvent OnWelcomingComplate;
        protected override void Init()
        {
            m_panelIndex = 0;
        }

        public void OnClickPlayNow()
        {
            if (gameManager.playerData.GetPlayerGameStageProgress() > GameStage.None)
            {
                gameManager.currentGameStage = gameManager.playerData.GetPlayerGameStageProgress();
                HasFinishedOpening();
                return;
            }
            else
            {
                gameManager.currentGameStage = (GameStage)1;
                gameManager.playerData.SetPlayerGameStageProgress(gameManager.currentGameStage);

                UpdateOpeningPanel();
            }
        }

        public void UpdateOpeningPanel()
        {
            foreach (var item in panelWelcoming)
            {
                item.SetActive(false);
            }

            if (m_panelIndex == 1)
            {
                string secondParagraphText = $"Hai, <b>{gameManager.playerData.GetPlayerName()}</b> di penugasan baru Anda. <br> Anda adalah Head of Teritory Daerah (HOTD) yang baru saja mendapatkan penempatan di WITEL Jakarta Timur, wilayah yang strategis namun penuh tantangan.<br><br> Hari ini, Anda menerima pesan langsung dari atasan Anda, Pak Rama--General Manager Regional. Beliau memberikan misi penting. <br><br>Sebagai HOTD, Anda memimpin tim untuk menjalin hubungan dengan calon klien prioritas. Tugas pertama Anda: membuka peluang kerja sama dengan perusahaan strategis";
                panelWelcoming[m_panelIndex].SetActive(true);

                textPlayerName.text = secondParagraphText;
            }
            else
            {
                panelWelcoming[m_panelIndex].SetActive(true);
            }
        }

        public void NextPanelIndex()
        {
            if (m_panelIndex == panelWelcoming.Length - 1)
            {
                HasFinishedOpening();
            }
            else
            {
                m_panelIndex++;
                UpdateOpeningPanel();
            }
        }

        public void HasFinishedOpening()
        {
            // mycoonHandler.ShowMycoonInfo(GameStage.None, () => { });

            ChangeSceneTo(2);

            OnWelcomingComplate?.Invoke();
        }
    }
}