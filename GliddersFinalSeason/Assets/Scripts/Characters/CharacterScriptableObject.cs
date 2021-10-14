using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

namespace Glidders
{
    namespace Character
    {
        // �v���W�F�N�g�E�B���h�E�ō쐬�\�ɂ���
        [System.Serializable, CreateAssetMenu(fileName = "CharacterScriptableObject", menuName = "CreateCharacterData")]
        public class CharacterScriptableObject : ScriptableObject
        {
            [SerializeField]
            public string id;                      // ����ID
            [SerializeField]
            public string characterName;           // �L�����N�^�[�̖��O
            [SerializeField]
            public int moveAmount;                 // �ړ���
            [SerializeField]
            public UniqueSkillScriptableObject[] skillDataArray = new UniqueSkillScriptableObject[Rule.skillCount];  // �X�L����3�i�[����z��
            [SerializeField]
            public UniqueSkillScriptableObject uniqueSkillData; // ���j�[�N�X�L��
            [SerializeField]
            public RuntimeAnimatorController characterAnimator;     // �L�����N�^�[�̃A�j���[�^�[
        }
    }
}