using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace Smarteye.VisualNovel.Character
{
    public class CharacterData : MonoBehaviour
    {
        public List<CharacterIdentity> characterIdentities;

        public Sprite GetCharacter(CharacterIdentity.CharacterRole _role, CharacterIdentity.Action.ActionType _actionType)
        {
            return characterIdentities.First((x) => x.role == _role).characterActions.First((x) => x.actionType == _actionType).actionSprite;
        }
    }

    [Serializable]
    public class CharacterIdentity
    {
        public CharacterRole role;

        [Serializable]
        public enum CharacterRole
        {
            NONE, PLAYER, NARATOR, CLIENT, ASISTEN, BOS, SECURITY
        }

        public List<Action> characterActions;

        [Serializable]
        public class Action
        {
            public ActionType actionType;
            public enum ActionType
            {
                NONE, TALKING, HAPPY, CONFUSED
            }

            public Sprite actionSprite;
        }
    }
}