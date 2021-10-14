using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Glidders.Character;
using Glidders;

[CustomEditor(typeof(CharacterScriptableObject))]
public class CharacterDataEditor : Editor
{
    const int MOVE_AMOUNT_MIN = 1;      // �ړ��ʂ̉���
    const int MOVE_AMOUNT_MAX = 5;      // �ړ��ʂ̏��

    private UniqueSkillScriptableObject[] skillDatas = new UniqueSkillScriptableObject[Rule.skillCount];    // 3�̃X�L����ݒ肷��

    bool initializeFlg = true;

    public override void OnInspectorGUI()
    {
        EditorGUI.BeginChangeCheck();
        CharacterScriptableObject characterScriptableObject = target as CharacterScriptableObject;

        if (initializeFlg)
        {
            for (int i = 0; i < Rule.skillCount; i++)
            {
                skillDatas[i] = characterScriptableObject.skillDataArray[i];
            }
            initializeFlg = false;
        }

        // ����ID
        EditorGUILayout.BeginVertical(GUI.skin.box);
        characterScriptableObject.id = EditorGUILayout.TextField("����ID", characterScriptableObject.id);
        EditorGUILayout.EndVertical();
        EditorGUILayout.Space();

        // �L�����N�^�[�̖��O
        EditorGUILayout.BeginVertical(GUI.skin.box);
        //var characterNameSP = serializedObject.FindProperty();
        characterScriptableObject.characterName = EditorGUILayout.TextField("���O", characterScriptableObject.characterName);
        EditorGUILayout.EndVertical();
        EditorGUILayout.Space();

        // �L�����N�^�[�̈ړ���
        EditorGUILayout.BeginVertical(GUI.skin.box);
        EditorGUILayout.LabelField("�L�����N�^�[�̈ړ���");
        characterScriptableObject.moveAmount = EditorGUILayout.IntSlider(characterScriptableObject.moveAmount, MOVE_AMOUNT_MIN, MOVE_AMOUNT_MAX);        
        EditorGUILayout.EndVertical();
        EditorGUILayout.Space();

        // �L�����N�^�[�̃X�L��
        EditorGUILayout.BeginVertical(GUI.skin.box);
        EditorGUILayout.LabelField("�L�����N�^�[�̏��L�X�L��");
        for (int i = 0; i < skillDatas.Length; i++)
        {
            skillDatas[i] = EditorGUILayout.ObjectField(string.Format($"{i + 1}�߂̃X�L��:"), skillDatas[i], typeof(UniqueSkillScriptableObject), true) as UniqueSkillScriptableObject;
        }
        EditorGUILayout.EndVertical();
        EditorGUILayout.Space();

        // ���j�[�N�X�L��
        EditorGUILayout.BeginVertical(GUI.skin.box);
        EditorGUILayout.LabelField("���j�[�N�X�L��");
        characterScriptableObject.uniqueSkillData = EditorGUILayout.ObjectField(string.Format(""), characterScriptableObject.uniqueSkillData, typeof(UniqueSkillScriptableObject), true) as UniqueSkillScriptableObject;
        EditorGUILayout.EndVertical();

        // �A�j���[�^�[
        EditorGUILayout.BeginVertical(GUI.skin.box);
        EditorGUILayout.LabelField("�A�j���[�^�[");
        characterScriptableObject.characterAnimator = EditorGUILayout.ObjectField(string.Format(""), characterScriptableObject.characterAnimator, typeof(RuntimeAnimatorController), true) as RuntimeAnimatorController;
        EditorGUILayout.EndVertical();

        if (GUILayout.Button("�ۑ�"))
        {
            bool unsetFlg = false;
            if (characterScriptableObject.id == "")
            {
                EditorUtility.DisplayDialog("����ID���ݒ�", "����ID��ݒ肵�Ă��������B", "�����B");
                unsetFlg = true;
            }
            foreach (UniqueSkillScriptableObject skillData in skillDatas)
            {
                if (skillData == null)
                {
                    EditorUtility.DisplayDialog("�X�L�����ݒ�", "�X�L�����ݒ肳��Ă��܂���B", "�����킩�����B");
                    unsetFlg = true;
                    break;
                }
            }
            if (unsetFlg) return;
            if (EditorGUI.EndChangeCheck())
            {
                for (int i = 0; i < Rule.skillCount; i++)
                {
                    characterScriptableObject.skillDataArray[i] = skillDatas[i];
                }

                var obj = EditorUtility.InstanceIDToObject(target.GetInstanceID());
                //��ScriptableObjectDatabase.Write(characterScriptableObject.id, AssetDatabase.GetAssetPath(obj));
                Debug.Log("aa=" + AssetDatabase.GetAssetPath(obj));

                //AssetDatabase.Refresh();
                EditorUtility.SetDirty(characterScriptableObject);
                UnityEditor.SceneManagement.EditorSceneManager.MarkSceneDirty(UnityEditor.SceneManagement.EditorSceneManager.GetActiveScene());
                AssetDatabase.SaveAssets();
            }
        }
        EditorGUILayout.HelpBox("�t�@�C������ύX�����ꍇ�͕K���ۑ����Ă��������B", MessageType.Warning);
    }
}
