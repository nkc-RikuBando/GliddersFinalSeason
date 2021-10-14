using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Glidders
{
    /// <summary>
    /// �J�n�n�_�ƏI���_����A�ړ����𐶐����邽�߂̃N���X
    /// </summary>
    public class FixedRouteInterpolation
    {
        /// <summary>
        /// �J�n�n�_�i���݂̃L�����N�^�[�̈ʒu�j�ƏI���_�i�ړ�������̈ʒu�j����A�ړ����𐶐����܂��B
        /// </summary>
        /// <param name="characterPosition">�J�n�n�_�i���݂̃L�����N�^�[�̈ʒu�j�B</param>
        /// <param name="endPosition">�I���_�i�ړ�������̈ʒu�j</param>
        /// <returns>���������ړ����B</returns>
        public static FieldIndexOffset[] Make(FieldIndex characterPosition, FieldIndexOffset endPosition)
        {
            // ���ۂ̈ړ������i�[�����z��
            FieldIndexOffset[] moveArray = new FieldIndexOffset[Rule.maxMoveAmount];

            // endPosition�̒l����傫���ق��̒����i�ړ��ʂ��i�[����Ă���ق��j�𒊏o����
            int length = Mathf.Max(Mathf.Abs(endPosition.rowOffset), Mathf.Abs(endPosition.columnOffset));

            // �ړ������𒊏o��������
            FieldIndexOffset duration = endPosition / length;


            for (int i = 0; i <Rule.maxMoveAmount; ++i)
            {
                // i��length�����i���߂�ꂽ�ړ��ʂ𖞂����Ă��Ȃ��j�Ȃ�A�ړ������i�[����
                if (i < length) moveArray[i] = duration;
                else moveArray[i] = FieldIndexOffset.zero;
            }

            return moveArray;
        }
    }
}
