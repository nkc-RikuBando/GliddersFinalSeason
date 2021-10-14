using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;
using Glidders.Buff;

namespace Glidders
{
    namespace Character
    {
        // �v���W�F�N�g�E�B���h�E�ō쐬�\�ɂ���
        [CreateAssetMenu(fileName = "SkillScriptableObject", menuName = "CreateSkillData")]
        public class SkillScriptableObject : ScriptableObject
        {
            // �v���C���[�����͂���X�L�����
            public string id;                     // ����ID
            public string skillName;              // �X�L������
            public string skillCaption;           // �X�L��������
            public int energy;                    // �G�l���M�[
            public int damage;                    // �_���[�W
            public int priority;                  // �D��x
            public int power;                     // �З�(�_���[�W�t�B�[���h)
            public Sprite skillIcon;              // �X�L���A�C�R��
            public SkillTypeEnum skillType;       // �X�L���̎�ށi�U���Z���⏕�Z���j
            public List<BuffViewData> giveBuff;   // �t�^�����o�t
            public AnimationClip skillAnimation;  // �A�j���[�V�����N���b�v

            #region �͈͂Ɋւ�����f�[�^���i�[�B�O������̎Q�Ɣ񐄏��B
            /// <summary> �Q�Ɣ񐄏��B </summary>
            public bool[] selectGridArray;                 // �I���\�}�X�����ۂɊi�[���Ă����ꎟ���z��
            /// <summary> �Q�Ɣ񐄏��B </summary>
            public bool[] attackGridArray;                 // �U���͈͂����ۂɊi�[���Ă����ꎟ���z��
            #endregion

            // �X�L���̑I���\�}�X���i�[�����O���b�h���X�g
            public FieldIndexOffset[] selectFieldIndexOffsetArray
            {
                get => GetSelectFieldIndexOffsetArray();
            }

            // �X�L���̍U���͈͂��i�[�����O���b�h���X�g
            public FieldIndexOffset[] attackFieldIndexOffsetArray
            {
                get => GetAttackFieldIndexOffsetArray();
            }

            #region �v�Z�p�ϐ��B�Q�Ɣ񐄏��B
            /// <summary> �v�Z�p�̂��ߎQ�Ɣ񐄏��B </summary> 
            public int rangeSize;
            /// <summary> �v�Z�p�ϐ��B�Q�Ɣ񐄏��B </summary>
            public int center;
            #endregion

            private FieldIndexOffset[] GetSelectFieldIndexOffsetArray()
            {
                List<FieldIndexOffset> selectGridTrueList = new List<FieldIndexOffset>();
                int index = 0;
                foreach (bool active in selectGridArray)
                {
                    if (active)
                    {
                        int rowOffset = index / rangeSize - center;
                        int columnOffset = index % rangeSize - center;
                        selectGridTrueList.Add(new FieldIndexOffset(rowOffset, columnOffset));
                    }
                    index++;
                }
                FieldIndexOffset[] returnArray = new FieldIndexOffset[selectGridTrueList.Count];
                for (int i = 0; i < selectGridTrueList.Count; i++)
                {
                    returnArray[i] = selectGridTrueList[i];
                }
                return returnArray;
            }

            private FieldIndexOffset[] GetAttackFieldIndexOffsetArray()
            {
                List<FieldIndexOffset> attackGridTrueList = new List<FieldIndexOffset>();
                int index = 0;
                foreach (bool active in attackGridArray)
                {
                    if (active)
                    {
                        int rowOffset = index / rangeSize - center;
                        int columnOffset = index % rangeSize - center;
                        attackGridTrueList.Add(new FieldIndexOffset(rowOffset, columnOffset));
                    }
                    index++;
                }
                FieldIndexOffset[] returnArray = new FieldIndexOffset[attackGridTrueList.Count];
                for (int i = 0; i < attackGridTrueList.Count; i++)
                {
                    returnArray[i] = attackGridTrueList[i];
                }
                return returnArray;
            }
        }

        public enum SkillTypeEnum
        {
            ATTACK,
            SUPPORT,
        }
    }
}
