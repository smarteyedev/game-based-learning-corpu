using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Smarteye.VisualNovel.taufiq
{
    [Serializable]
    public class MasterData
    {
        public List<SceneScenarioDataRoot> sceneScenarioDataRoots;
    }

    [Serializable]
    public class SceneScenarioDataRoot
    {
        public Stage stage;
        public string sceneIdentity;
        public SceneProgress sceneProgress;
        public string introductionStory;
        public string agentAIHint;
        public List<DialogueRoot> dialogueData;
        public DecisionRoot decisionData;
        public string knowledgeGain;

        public enum SceneProgress
        {
            DIALOGUE, SUCCESSRESULT, FAILRESULT
        }

        public enum Stage
        {
            NONE, PROLOG, PROSPECTINGANDPROFILING, RAPPORT, PROBING, SOLUTION, OBJECTIONANDCLOSING, EPILOG
        }

        public enum SpeakerRoot
        {
            PLAYER, NARATOR, CLIENT, ASISTEN, BOS, SECURITY
        }

        [Serializable]
        public class DialogueRoot
        {
            public SpeakerRoot speakerName;
            public string narationText;
        }

        [Serializable]
        public class DecisionRoot
        {
            public string question;
            public List<OptionRoot> optionDatas;

            [Serializable]
            public class OptionRoot
            {
                public string optionText;
                public int score;
                public string nextSceneIdentity;
            }
        }
    }
}