using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Glidders.Character;

namespace Glidders
{
    namespace Buff
    {
        public class BuffViewData : ScriptableObject
        {
            [SerializeField]
            public string id;                           // ����ID

            [SerializeField]
            public Sprite buffIcon;                     // �o�t�̃A�C�R��

            [SerializeField]
            public string buffName;                     // �o�t�̖���

            [SerializeField]
            public string buffCaption;                  // �o�t�̐�����

            [SerializeField]
            public GameObject effectObjectPrefab;       // �o�t�̉��o���s��GameObject��Prefab���i�[

            [SerializeField]
            public CharacterScriptableObject upperTransform;    // �ϐg��̃L�����N�^�[

            [SerializeField]
            public CharacterScriptableObject lowerTransform;    // �ϐg�O�̃L�����N�^�[

            [SerializeField]
            public List<BuffValueData> buffValueList;   // ���ۂ̃o�t���
        }
    }
}
