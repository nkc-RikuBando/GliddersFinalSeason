using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

namespace Glidders
{
    public class CameraController : MonoBehaviour
    {

        [SerializeField] private CinemachineTargetGroup cinemachineTargetGroup;
        [SerializeField] private Transform cursorTransform;

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        public void AddTarget(Transform targetTransform) // �w�肵��transform���J�����̃^�[�Q�b�g�ɒǉ�
        {
            cinemachineTargetGroup.AddMember(targetTransform, 1, 1);
        }

        public void RemoveTarget(Transform targetTransform) // �w�肵��transform���J�����̃^�[�Q�b�g����폜
        {
            cinemachineTargetGroup.RemoveMember(targetTransform);
        }

        public void ClearTarget() // �J�����̃^�[�Q�b�g��S�č폜
        {
            foreach(var target in cinemachineTargetGroup.m_Targets)
            {
                cinemachineTargetGroup.RemoveMember(target.target);
            }
        }

        public void AddCarsor()
        {
            cinemachineTargetGroup.AddMember(cursorTransform, 1, 1);
        }

        public void RemoveCarsor()
        {
            cinemachineTargetGroup.RemoveMember(cursorTransform);
        }
    }
}

