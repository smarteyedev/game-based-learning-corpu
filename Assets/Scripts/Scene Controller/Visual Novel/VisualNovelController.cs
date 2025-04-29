using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Smarteye.SceneController.taufiq;

namespace Smarteye.VisualNovel.taufiq
{
    public class StoryBlock
    {
        public string naration;
        public string option1Text;
        public string option2Text;
        public StoryBlock Option1Block;
        public StoryBlock Option2Block;

        public StoryBlock(string _naration, string _option1Text = "", string _option2Text = "", StoryBlock _option1Block = null, StoryBlock _option2Block = null)
        {
            this.naration = _naration;
            this.option1Text = _option1Text;
            this.option2Text = _option2Text;
            this.Option1Block = _option1Block;
            this.Option2Block = _option2Block;
        }
    }

    public class VisualNovelController : SceneControllerAbstract
    {
        public TextMeshProUGUI narationText;
        public Button option1Btn;
        public Button option2Btn;

        private StoryBlock currentBlock;

        static StoryBlock block9 = new StoryBlock("kamu terus-terusan terjebak disituasi saat ini dan tetap susah untuk mendapatkan gaji yang sesuai dengan keinginanmu. kamu tidak berhasil!");
        static StoryBlock block8 = new StoryBlock("Selamat kamu telah diterima di perusahaan impianmu sebagai game programmer dengan gaji sesuai yang kamu inginkan");
        static StoryBlock block7 = new StoryBlock("Kamu saat ini sebagai anak magang yang digaji underrated. sekalipun kamu mengajukan kenaikkan gaji, pengajuanmu ditolak karena posisimu sebagai anak magang. selanjutnya apa yang akan kamu lakukan?", "Mencari perkerjan lain", "Terus berusaha untuk mengembangkan kemampuan diri dan memperbanyak portofolio", block9, block8);
        static StoryBlock block6 = new StoryBlock("kamu telah belajar dengan sungguh-sunggu, namun panitia bootcamp belum sama sekali menawarkan perkejaan untukmu sebagai game programmer", "Mendaftar magang", "Mencari perkerjaan lain", block7, block9);
        static StoryBlock block5 = new StoryBlock("saat ini kamu memiliki dasar pemrograman game, namun kamu masih merasa skill mu masih kurang baik untuk mengerjakan proyek besar, sedangkan tahun depan kamu sudah harus mendaftar kerja", "Mengambil pelatihan untuk seleksi BUMN selama 2 tahun", "Mendaftar magang dengan gajih 2.7 juta", block8, block7);
        static StoryBlock block4 = new StoryBlock("kamu tidak diterima di jurusan teknologi informatika, namun kamu mendapatkan penawaran di jurusan teknologi rekayasa multimedia", "Mengambil bootcamp", "Mengambil tawaran di TRM", block6, block5);
        static StoryBlock block3 = new StoryBlock("Kamu mencari sumber informasi lain seperti tanya ke kakak kelas atau mencari konten youtube yang membahas cara untuk menjadi game programmer. setelah kamu mendaftar ke perusahaan sayangnya kamu ditolak", "Mengikuti bootcamp", "Mencari perkerjaan lain", block6, block9);
        static StoryBlock block2 = new StoryBlock("kamu mendapatkan arahan dari orang tua mu untuk terus melanjutkan ke perkuliahan dan mengambil jurusan teknologi informatika", "Mencari informasi lain", "Mendaftar perkuliahan ke TEL-U", block3, block4);
        static StoryBlock block1 = new StoryBlock("Saat ini aku baru lulus dari SMA, apa cara yang harus aku lakukan ya untuk menjadi seorang game developer yang gajinya sesuai denganku?", "Bertanya ke orang tua", "Cari informasi sendiri ah..", block2, block3);

        [Header("New Visual Novel Sytem")]
        [SerializeField] private List<BlockScenarioDataMap> temp_BlockScenarioData;

        protected override void Init()
        {
            // DisplayBlock(block1);
        }

        public void ShowDialog()
        {
            
        }

        #region   Old-System-Visual-Novel
        private void DisplayBlock(StoryBlock block)
        {
            narationText.text = block.naration;
            option1Btn.GetComponentInChildren<TextMeshProUGUI>().text = block.option1Text;
            option2Btn.GetComponentInChildren<TextMeshProUGUI>().text = block.option2Text;

            currentBlock = block;
        }

        public void Button1Clicking()
        {
            DisplayBlock(currentBlock.Option1Block);

            CheckingProgress();
        }

        public void Button2Clicking()
        {
            DisplayBlock(currentBlock.Option2Block);

            CheckingProgress();
        }

        private void CheckingProgress()
        {
            if (currentBlock == block9)
            {
                option1Btn.gameObject.SetActive(false);
                option2Btn.gameObject.SetActive(false);
            }
            else if (currentBlock == block8)
            {
                option1Btn.gameObject.SetActive(false);
                option2Btn.gameObject.SetActive(false);
            }
        }
        #endregion
    }
}