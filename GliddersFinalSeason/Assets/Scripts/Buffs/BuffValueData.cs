using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Glidders.Character;

namespace Glidders
{
    namespace Buff
    {
        public class BuffValueData : ScriptableObject
        {
            //[SerializeField]
            //public StatusTypeEnum buffedStatus;     // �o�t�����X�e�[�^�X

            [SerializeField]
            public BuffTypeEnum buffType;           // �o�t����Z�����Z��

            [SerializeField]
            public float buffScale;                 // �o�t�̔{��/���Z�l

            [SerializeField]
            public int buffDuration;                // �o�t�̌p���^�[����
        }

        /// <summary>
        /// �o�t����Z�����Z�������ʂ��邽�߂̂���
        /// </summary>
        public enum BuffTypeEnum
        {
            PLUS,
            MULTIPLIED,
        }
    }
}
