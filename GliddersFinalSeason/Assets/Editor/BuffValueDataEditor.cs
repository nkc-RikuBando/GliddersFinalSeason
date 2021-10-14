using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Glidders.Buff;
using Glidders.Character;

[CustomEditor(typeof(BuffValueData))]
public class BuffValueDataEditor : Editor
{
    List<string> captions;
    public override void OnInspectorGUI()
    {
        BuffValueData buffValueData = target as BuffValueData;

        using (new EditorGUILayout.VerticalScope(GUI.skin.box))
        {
            //��buffValueData.buffedStatus = (StatusTypeEnum)EditorGUILayout.EnumPopup("�o�t����X�e�[�^�X", buffValueData.buffedStatus);
        }
        captions = new List<string>();
        captions.Add("DAMAGE �c�^����_���[�W");
        captions.Add("DEFENSE�c�󂯂�_���[�W");
        captions.Add("POWER   �c�З�(�_���[�W�t�B�[���h�̌p���^�[����)");
        captions.Add("MOVE     �c�ړ���");
        captions.Add("SPECIAL�c�L�����N�^�[�\�͌ŗL�̐��l");
        WriteInformation(captions);
        EditorGUILayout.Space();

        using (new EditorGUILayout.VerticalScope(GUI.skin.box))
        {
            buffValueData.buffType = (BuffTypeEnum)EditorGUILayout.EnumPopup("�o�t�̎��", buffValueData.buffType);
        }
        captions = new List<string>();
        captions.Add("PLUS            �c���Z");
        captions.Add("MULTIPLIED�c��Z");
        WriteInformation(captions);
        EditorGUILayout.Space();

        using (new EditorGUILayout.VerticalScope(GUI.skin.box))
        {
            buffValueData.buffScale = EditorGUILayout.FloatField("�X�e�[�^�X�̑�����", buffValueData.buffScale);
        }
        captions = new List<string>();
        captions.Add("\"PLUS\"�͑�������l�����̂܂ܐݒ�");
        captions.Add("\"MULTIPLIED\"�͌��̒l�ւ̔{����ݒ�");
        WriteInformation(captions);
        EditorGUILayout.Space();

        using (new EditorGUILayout.VerticalScope(GUI.skin.box))
        {
            buffValueData.buffDuration = EditorGUILayout.IntField("�p���^�[����", buffValueData.buffDuration);
        }
        captions = new List<string>();
        captions.Add("�c��^�[�����̓^�[���I�����Ɍ�������B");
        captions.Add("�o�t�����������^�[�����������邽�߁A");
        captions.Add("�����^�[�����܂߂��p���^�[������ݒ肷��B");
        WriteInformation(captions);
        EditorGUILayout.Space();

        if (GUILayout.Button("�ۑ�"))
        {
            //AssetDatabase.Refresh();
            EditorUtility.SetDirty(buffValueData);
            UnityEditor.SceneManagement.EditorSceneManager.MarkSceneDirty(UnityEditor.SceneManagement.EditorSceneManager.GetActiveScene());
            AssetDatabase.SaveAssets();
        }
    }

    /// <summary>
    /// ����������\������֐�
    /// </summary>
    /// <param name="captions"></param>
    private void WriteInformation(List<string> captions)
    {
        int iconWidth = 20;

        using (new EditorGUILayout.HorizontalScope(GUI.skin.label))
        {
            GUILayout.Box(EditorGUIUtility.Load("console.erroricon.inactive.sml") as Texture, GUI.skin.label, GUILayout.Width(iconWidth));
            using (new EditorGUILayout.VerticalScope())
            {
                foreach (string caption in captions)
                    EditorGUILayout.LabelField(caption);
            }
        }
    }
}
