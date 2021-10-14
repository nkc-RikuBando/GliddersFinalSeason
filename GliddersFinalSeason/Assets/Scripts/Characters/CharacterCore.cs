using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Glidders
{
    namespace Character
    {
        public class CharacterCore : MonoBehaviour //,IGetCharacterCoreData
        {
            // �L�����N�^�[�̌Œ�f�[�^���i�[����ScriptableObject
            public CharacterScriptableObject characterScriptableObject;

            public int pointAndHp { get; set; }

            public string GetCharacterName()
            {
                return characterScriptableObject.characterName;
            }

            public int GetMoveAmount()
            {
                return characterScriptableObject.moveAmount;
            }

            public GameObject GetMyGameObject()
            {
                return gameObject;
            }

            public int GetPointAndHp()
            {
                return pointAndHp;
            }

            /// <summary>
            /// ���̃L�����N�^�[�ɕt�^���ꂽ�X�L���f�[�^���Q�Ƃ���B
            /// </summary>
            /// <param name="skillNumber">1�`3�̃X�L���ԍ��B</param>
            /// <returns></returns>
            public UniqueSkillScriptableObject GetSkillData(int skillNumber)
            {
                // 1�`3�̃X�L���ԍ���0�`2�̓Y�����ɂ���
                int skillNumberIndex = skillNumber - 1;

                // �X�L���ԍ����z��O���Q�Ƃ��Ă��Ȃ����m�F
                if (skillNumberIndex < 0 || skillNumberIndex >= Rule.skillCount) throw new ArgumentOutOfRangeException("skillNumber", "skillNumber�͂P�`�R�ł���K�v������܂��B");

                // �X�L���ԍ��ɑΉ�����UniqueSkillScriptableObject��ԋp
                return characterScriptableObject.skillDataArray[skillNumberIndex];
            }

            public UniqueSkillScriptableObject GetUniqueData()
            {
                return characterScriptableObject.uniqueSkillData;
            }
        }
    }
}