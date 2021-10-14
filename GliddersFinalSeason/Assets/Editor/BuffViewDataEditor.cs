using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Glidders;
using Glidders.Buff;
using Glidders.Character;

[CustomEditor(typeof(BuffViewData))]
public class BuffViewDataEditor : Editor
{
    bool foldout;

    public override void OnInspectorGUI()
    {
        BuffViewData buffViewData = target as BuffViewData;

        buffViewData.id = EditorGUILayout.TextField("����ID", buffViewData.id);

        buffViewData.buffIcon = EditorGUILayout.ObjectField("�A�C�R��", buffViewData.buffIcon, typeof(Sprite), true) as Sprite;

        buffViewData.buffName = EditorGUILayout.TextField("����", buffViewData.buffName);

        buffViewData.buffCaption = EditorGUILayout.TextField("������", buffViewData.buffCaption);

        EditorGUILayout.Space();

        buffViewData.effectObjectPrefab = EditorGUILayout.ObjectField("���o�I�u�W�F�N�gPrefab", buffViewData.effectObjectPrefab, typeof(GameObject), true) as GameObject;

        buffViewData.upperTransform = EditorGUILayout.ObjectField("�ϐg��̃L�����N�^�[", buffViewData.upperTransform, typeof(CharacterScriptableObject), true) as CharacterScriptableObject;
        buffViewData.lowerTransform = EditorGUILayout.ObjectField("�ϐg�O�̃L�����N�^�[", buffViewData.lowerTransform, typeof(CharacterScriptableObject), true) as CharacterScriptableObject;

        using (new EditorGUILayout.VerticalScope(GUI.skin.box))
        {
            int indexWidth = 20;
            int listCount = buffViewData.buffValueList.Count;

            foldout = EditorGUILayout.Foldout(foldout, "�ݒ肳��Ă���o�t�ꗗ");
            if (foldout)
            {
                for (int i = 0; i < listCount; i++)
                {
                    using (new EditorGUILayout.HorizontalScope())
                    {
                        EditorGUILayout.LabelField(string.Format("{0:00}", i), GUILayout.Width(indexWidth));

                        buffViewData.buffValueList[i] = EditorGUILayout.ObjectField("", buffViewData.buffValueList[i], typeof(BuffValueData), true) as BuffValueData;

                        if (listCount >= 2)
                        {
                            if (GUILayout.Button("-", GUILayout.Width(indexWidth)))
                            {
                                // valueData�̖��̂��擾
                                string valueDataName = buffViewData.buffValueList[i].ToString();
                                int nameIndex = valueDataName.IndexOf(' ');
                                valueDataName = valueDataName.Substring(0, nameIndex);

                                // valueData�̃p�X��ݒ�
                                const string PATH = "Assets/Resources/ScriptableObjects/Buffs/";
                                string valuePath = PATH + valueDataName + ".asset";
                                Debug.Log(valuePath);

                                //valueData���t�@�C������폜
                                AssetDatabase.DeleteAsset(valuePath);

                                // ViewData�Ƃ̊֘A�t�����폜
                                buffViewData.buffValueList.RemoveAt(i);
                            }
                        }
                    }
                }

                using (new EditorGUILayout.HorizontalScope())
                {
                    EditorGUILayout.LabelField(" ", GUILayout.Width(indexWidth));
                    if (GUILayout.Button("+", GUILayout.Width(indexWidth)))
                    {
                        // valueData�̃p�X��ݒ�
                        const string PATH = "Assets/Resources/ScriptableObjects/Buffs/";
                        string valuePath = PATH + string.Format("{0}_{1:00}", buffViewData.buffName, listCount) + ".asset";

                        // valueData�𐶐�
                        BuffValueData newValueData = ScriptableObject.CreateInstance<BuffValueData>();
                        AssetDatabase.CreateAsset(newValueData, valuePath);
                        buffViewData.buffValueList.Add(newValueData);
                    }
                }
            }
        }

        if (GUILayout.Button("�ۑ�"))
        {
            if (buffViewData.id == "")
            {
                EditorUtility.DisplayDialog("����ID���ݒ�", "����ID��ݒ肵�Ă��������B", "OK");
                return;
            }
            var obj = EditorUtility.InstanceIDToObject(target.GetInstanceID());
            //��ScriptableObjectDatabase.Write(buffViewData.id, AssetDatabase.GetAssetPath(obj));

            //AssetDatabase.Refresh();
            EditorUtility.SetDirty(buffViewData);
            UnityEditor.SceneManagement.EditorSceneManager.MarkSceneDirty(UnityEditor.SceneManagement.EditorSceneManager.GetActiveScene());
            AssetDatabase.SaveAssets();
        }

        EditorGUILayout.HelpBox("�t�@�C������ύX�����ꍇ�͕K���ۑ����Ă��������B", MessageType.Warning);
    }
}
