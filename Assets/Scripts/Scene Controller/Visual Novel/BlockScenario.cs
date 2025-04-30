using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Smarteye.VisualNovel.taufiq
{
    public enum CharacterAnimType
    {
        Happy, Talk, Mad
    }

    public enum SceneId
    {
        A, B, C, D, E, F, G
    }

    [Serializable]
    public class BlockScenarioDataMap
    {
        public SceneId sceneId;
        public List<PreNarationData> preNarationDatas;
        public DecisionData decisionData;

        public BlockScenarioDataMap(SceneId sceneId, List<PreNarationData> preNarationDatas, DecisionData decisionData)
        {
            this.sceneId = sceneId;
            this.preNarationDatas = preNarationDatas;
            this.decisionData = decisionData;
        }

        [Serializable]
        public class PreNarationData
        {
            public string speakerName;
            public string narationText;
        }

        [Serializable]
        public class DecisionData
        {
            public string question;
            public List<OptionData> options;

            [Serializable]
            public class OptionData
            {
                public string optionText;
                public int score;
                public int nextBlockIndex;
                // public CharacterAnimType animationType;
            }
        }
    }
}