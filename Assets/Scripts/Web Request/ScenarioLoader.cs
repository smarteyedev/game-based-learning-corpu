using UnityEngine;
using Newtonsoft.Json;
using System.Collections.Generic;
using Smarteye.VisualNovel;
using System.Linq;

namespace Smarteye.RestAPI.Sample
{
    public class ScenarioLoader : MonoBehaviour
    {
        public TextAsset jsonFile; // Masukkan file JSON dari Inspector
        public List<SceneScenarioDataRoot> sampleScenarios; // Diisi setelah parsing
        // public List<SceneScenarioDataRoot> sceneProbing;

        void Start()
        {
            // LoadJsonFile();
        }

        public void LoadJsonFile()
        {
            if (jsonFile == null)
            {
                Debug.LogError("JSON file belum dimasukkan ke Inspector.");
                return;
            }

            try
            {
                MasterData masterData = JsonConvert.DeserializeObject<MasterData>(jsonFile.text);
                sampleScenarios = masterData.sceneScenarioDataRoots;
            }
            catch (System.Exception ex)
            {
                Debug.LogError("Gagal membaca JSON: " + ex.Message);
            }

            // sceneProbing = sceneProbing.Concat(GetScenesByStage(SceneScenarioDataRoot.Stage.RAPPORT)).ToList();
            // sceneProbing = GetScenesByStages(SceneScenarioDataRoot.Stage.PROSPECTINGANDPROFILING);
        }

        public List<SceneScenarioDataRoot> GetScenesByStages(params SceneScenarioDataRoot.Stage[] targetStages)
        {
            return sampleScenarios
                .Where(scene => targetStages.Contains(scene.stage))
                .ToList();
        }
    }
}