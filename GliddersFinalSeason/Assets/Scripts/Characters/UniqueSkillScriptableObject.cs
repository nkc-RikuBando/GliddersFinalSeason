using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Glidders.Buff;

namespace Glidders
{
    namespace Character
    {
        // �v���W�F�N�g�E�B���h�E�ō쐬�\�ɂ���
        [CreateAssetMenu(fileName = "UniqueSkillScriptableObject", menuName = "CreateSkillData")]
        public class UniqueSkillScriptableObject : ScriptableObject
        {
            // ���j�[�N�X�L���̎��ʏ��
            public string id;                           // ����ID
            public bool isUniqueSkill;                  // ���j�[�N�X�L�����ǂ���
            public string skillName;                    // �X�L������
            public string skillCaption;                 // �X�L��������
            public int energy;                          // �G�l���M�[
            public int priority;                        // �D��x
            public Sprite skillIcon;                    // �X�L���A�C�R��
            public AnimationClip skillAnimation;        // �A�j���[�V�����N���b�v
            public SkillTypeEnum skillType;             // �X�L���̎�ށi�U���Z���⏕�Z���j

            public UniqueSkillMoveType moveType;        // �ړ��̎��
            public FieldIndexOffset[] moveFieldIndexOffsetArray   // �ړ���}�X
            {
                get => GetRangeArray(moveSelectArray);
            }

            public int damage;                          // �_���[�W
            public int power;                           // �З�(�_���[�W�t�B�[���h)
            public FieldIndexOffset[] selectFieldIndexOffsetArray // �I���\�}�X
            {
                get => GetRangeArray(attackSelectArray);
            }
            public FieldIndexOffset[] attackFieldIndexOffsetArray       // �U���͈̓}�X
            {
                get => GetRangeArray(attackArray);
            }
            public List<BuffViewData> giveBuff;         // �t�^�����o�t
            public List<BuffViewData> loseBuff;         // �����o�t

            #region �͈͂Ɋւ�����f�[�^���i�[�B�O������̎Q�Ɣ񐄏��B
            public bool[] moveSelectArray;       // �ړ���}�X�̌��^�B11*11��121�}�X�B
            public bool[] attackSelectArray;     // �I���\�}�X�̌��^�B11*11��121�}�X�B
            public bool[] attackArray;           // �U���͈̓}�X�̌��^�B11*11��121�}�X�B
            #endregion

            #region �v�Z�p�ϐ��B�Q�Ɣ񐄏��B
            /// <summary> �v�Z�p�̂��ߎQ�Ɣ񐄏��B </summary> 
            private static int rangeSize = 11 * 2 - 1;
            /// <summary> �v�Z�p�ϐ��B�Q�Ɣ񐄏��B </summary>
            private static int center = rangeSize / 2;
            #endregion

            /// <summary>
            /// �͈͂Ɋւ���S�Ẵf�[�^���������z�񂩂�A�I������Ă���}�X�����𒊏o�����z����擾����B
            /// </summary>
            /// <param name="beforeArray">���o�O�̑S�Ẵf�[�^���������z��B</param>
            /// <returns>�I������Ă���}�X�����𒊏o�����z��B</returns>
            private FieldIndexOffset[] GetRangeArray(bool[] beforeArray)
            {
                // �z�񂩂�I������Ă���}�X�����𒊏o����
                List<FieldIndexOffset> workList = new List<FieldIndexOffset>();
                int index = 0;
                foreach (bool active in beforeArray)
                {
                    if (active)
                    {
                        int rowOffset = index / rangeSize - center;
                        int columnOffset = index % rangeSize - center;
                        workList.Add(new FieldIndexOffset(rowOffset, columnOffset));
                    }
                    ++index;
                }

                // �I������Ă���}�X�������i�[�����z���ԋp����
                FieldIndexOffset[] returnArray = new FieldIndexOffset[workList.Count];
                for (int i = 0; i < returnArray.Length; ++i)
                {
                    returnArray[i] = workList[i];
                }

                return returnArray;
            }
        }

        public enum UniqueSkillMoveType
        {
            NONE,
            FREE,
            FIXED,
        }
    }
}
