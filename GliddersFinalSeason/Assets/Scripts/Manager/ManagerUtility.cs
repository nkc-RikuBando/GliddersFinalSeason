using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Glidders.Character;

namespace Glidders
{
    namespace Manager
    {
        /// <summary>
        /// �v���C���[���T�[�o�[�Ɉړ����𑗂�ۂɗp������\����
        /// </summary>
        public struct MoveSignal
        {
            public FieldIndexOffset[] moveDataArray;    // �ړ������i�[�����z��

            public MoveSignal(FieldIndexOffset[] moveDataArray)
            {
                this.moveDataArray = new FieldIndexOffset[Rule.maxMoveAmount];
                for (int i = 0; i < this.moveDataArray.Length; i++)
                {
                    this.moveDataArray[i] = FieldIndexOffset.zero;                    
                }

                for (int i = 0; i < moveDataArray.Length; i++)
                {
                    if (i >= this.moveDataArray.Length) break;
                    this.moveDataArray[i] = moveDataArray[i];
                }
            }
        }

        public struct DirecionSignal
        {
            public FieldIndexOffset direction;

            public DirecionSignal(FieldIndexOffset direction)
            {
                this.direction = direction;
            }
        }

        /// <summary>
        /// �v���C���[���T�[�o�[�ɍU�����𑗂�ۂɗp������\���́B
        /// </summary>
        public struct AttackSignal
        {
            public bool isAttack;                      // �U���������ǂ����B
            public UniqueSkillScriptableObject skillData;    // �g�p����X�L����񂪊i�[���ꂽUniqueSkillScriptableObject�B
            public FieldIndex selectedGrid;            // �X�L���g�p���ɑI�������ʒu�B
            public FieldIndexOffset direction;         // �X�L�����������B
            public int skillNumber;                    // 

            /// <summary>
            /// �U�����ɑ�����B�U������ꍇ�B
            /// </summary>
            /// <param name="skillData">�g�p�����X�L���̃X�L���f�[�^�B</param>
            /// <param name="selectedGrid">�X�L���g�p���ɑI�������ʒu�B</param>
            /// <param name="direction">�X�L�����������B</param>
            public AttackSignal(UniqueSkillScriptableObject skillData, FieldIndex selectedGrid, FieldIndexOffset direction,int skillNumber)
            {
                isAttack = true;
                this.skillData = skillData;
                this.selectedGrid = selectedGrid;
                this.direction = direction;
                this.skillNumber = skillNumber;
            }

            /// <summary>
            /// �U�����ɑ�����B�U���̗L�����w�肷��ꍇ�B
            /// </summary>
            /// <param name="flg">�U�������邩�ǂ����B</param>
            /// <param name="skillData">�g�p�����X�L���̃X�L���f�[�^�B</param>
            /// <param name="selectedGrid">�X�L���g�p���ɑI�������ʒu�B</param>
            /// <param name="direction">�X�L�����������B</param>
            public AttackSignal(bool flg, UniqueSkillScriptableObject skillData, FieldIndex selectedGrid, FieldIndexOffset direction,int skillNumber)
            {
                isAttack = flg;
                this.skillData = skillData;
                this.selectedGrid = selectedGrid;
                this.direction = direction;
                this.skillNumber = skillNumber;
            }

            /// <summary>
            /// �U�����ɑ�����B�U�����Ȃ��ꍇ�B
            /// </summary>
            /// <param name="setFalseToFlgAutomatically">�K��false������B(�����I�ɋ���false�Ƃ��Ĉ�����B)</param>
            public AttackSignal(bool setFalseToFlgAutomatically)
            {
                isAttack = false;
                skillData = null;
                selectedGrid = FieldIndex.minus;
                direction = FieldIndexOffset.zero;
                this.skillNumber = 0;
            }
        }

        // �^�[���I�����ɓ���������e�i�܂��p�����Ă��Ȃ��j
        public struct EndTurnSignal
        {
            int audience;
        }
    }
}
