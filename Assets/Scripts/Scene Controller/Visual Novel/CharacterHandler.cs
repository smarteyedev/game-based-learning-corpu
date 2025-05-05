using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;
using UnityEngine.UI;

namespace Smarteye.VisualNovel.Character
{
    public class CharacterHandler : MonoBehaviour
    {
        public List<CharacterIdentity> characterIdentities;

        [Header("Component References")]
        [SerializeField] private Image imageCharacter;

        public void UpdateCharacter(CharacterIdentity.CharacterRole _role, CharacterIdentity.Action.ActionType _actionType)
        {
            imageCharacter.sprite = GetActionSprite(characterIdentities.First((x) => x.role == _role), _actionType);
        }

        public void HideCharacter()
        {
            imageCharacter.gameObject.SetActive(false);
        }

        private Sprite GetActionSprite(CharacterIdentity _character, CharacterIdentity.Action.ActionType _actionType)
        {
            return _character.characterActions.First((x) => x.actionType == _actionType).actionSprite;
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
                NONE, TALKING, HAPPY, SAD
            }

            public Sprite actionSprite;
        }
    }
}