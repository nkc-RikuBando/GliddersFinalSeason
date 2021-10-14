using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Glidders;
using Glidders.Character;
using Glidders.Buff;


[CustomEditor(typeof(UniqueSkillScriptableObject))]
public class UniqueSkillScriptableObjectView : Editor
{
    // �U���͈͂̕\���Ɏg�p
    const string DOT = "��";
    const string NONE = "�@";
    const string PLAYER_FALSE = "��";
    const string PLAYER_TRUE = "��";
    const int DOT_WIDTH = 12;
    const int DOT_HEIGHT = 10;

    //string skillName;
    //int energy;
    //int damage;
    //int priority;
    //int power;
    //Sprite skillIcon;
    //AnimationClip animationClip;

    public override void OnInspectorGUI()
    {
        UniqueSkillScriptableObject skillData = target as UniqueSkillScriptableObject;

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("����ID");
        skillData.id = EditorGUILayout.TextField("", skillData.id);
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("���j�[�N�X�L�����ǂ���");
        skillData.isUniqueSkill = EditorGUILayout.Toggle("", skillData.isUniqueSkill);
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("�X�L������");
        skillData.skillName = EditorGUILayout.TextField("", skillData.skillName);
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.LabelField("�X�L��������");
        skillData.skillCaption = EditorGUILayout.TextArea(skillData.skillCaption);

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("����G�l���M�[");
        skillData.energy = EditorGUILayout.IntSlider(skillData.energy, 1, 5);
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("�D��x");
        skillData.priority = EditorGUILayout.IntSlider(skillData.priority, 1, 10);
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("�_���[�W");
        skillData.damage = EditorGUILayout.IntField(skillData.damage);
        EditorGUILayout.EndHorizontal();      

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("�З�");
        skillData.power = EditorGUILayout.IntSlider(skillData.power, 0, 5);
        EditorGUILayout.EndHorizontal();

        skillData.skillType = (SkillTypeEnum)EditorGUILayout.EnumPopup("�X�L���̎��", skillData.skillType);

        // ���̃X�L���ŕt�^�����A�����o�t��ݒ�
        int buffButtonWidth = 20;
        int buffObjectWidth = 160;
        using (new GUILayout.HorizontalScope())
        {
            EditorGUILayout.LabelField("�t�^�����o�t");
            using (new GUILayout.VerticalScope())
            {
                for (int i = 0; i < skillData.giveBuff.Count; i++)
                {
                    using (new GUILayout.HorizontalScope())
                    {
                        skillData.giveBuff[i] = EditorGUILayout.ObjectField("", skillData.giveBuff[i], typeof(BuffViewData), true, GUILayout.Width(buffObjectWidth)) as BuffViewData;
                        if (GUILayout.Button("-", GUILayout.Width(buffButtonWidth)))
                        {
                            skillData.giveBuff.RemoveAt(i);
                        }
                    }
                }
                if (GUILayout.Button("+", GUILayout.Width(buffButtonWidth)))
                {
                    skillData.giveBuff.Add(null);
                }
            }
        }
        using (new GUILayout.HorizontalScope())
        {
            EditorGUILayout.LabelField("�����o�t");
            using (new GUILayout.VerticalScope())
            {
                for (int i = 0; i < skillData.loseBuff.Count; i++)
                {
                    using (new GUILayout.HorizontalScope())
                    {
                        skillData.loseBuff[i] = EditorGUILayout.ObjectField("", skillData.loseBuff[i], typeof(BuffViewData), true, GUILayout.Width(buffObjectWidth)) as BuffViewData;
                        if (GUILayout.Button("-", GUILayout.Width(buffButtonWidth)))
                        {
                            skillData.loseBuff.RemoveAt(i);
                        }
                    }
                }
                if (GUILayout.Button("+", GUILayout.Width(buffButtonWidth)))
                {
                    skillData.loseBuff.Add(null);
                }
            }
        }

        skillData.moveType = (UniqueSkillMoveType)EditorGUILayout.EnumPopup("�ړ��̎��", skillData.moveType);

        skillData.skillAnimation = EditorGUILayout.ObjectField("�A�j���[�V�����N���b�v", skillData.skillAnimation, typeof(AnimationClip), true) as AnimationClip;

        skillData.skillIcon = EditorGUILayout.ObjectField("�X�L���A�C�R��", skillData.skillIcon, typeof(Sprite), true, GUILayout.Width(224), GUILayout.Height(224)) as Sprite;

        //FieldIndexOffset[] selectArray = skillData.selectFieldIndexOffsetArray;
        

        int arrayIndex = 0;
        int rowMin = int.MaxValue, rowMax = int.MinValue;
        int columnMin = int.MaxValue, columnMax = int.MinValue;
        //��int rowMin = 0, rowMax = 12, columnMin = 0, columnMax = 12;
        // �ړ��͈͂�`�悷��ۂ̍ŏ�,�ŉ�,�ō�,�ŉE�����߂�
        foreach (FieldIndexOffset offset in skillData.moveFieldIndexOffsetArray)
        {
            if (offset.rowOffset < rowMin) rowMin = offset.rowOffset;
            if (offset.rowOffset > rowMax) rowMax = offset.rowOffset;
            if (offset.columnOffset < columnMin) columnMin = offset.columnOffset;
            if (offset.columnOffset > columnMax) columnMax = offset.columnOffset;
        }
        // ���S���W��`�悷��悤�ɂ���
        if (rowMin > 0) rowMin = 0;
        if (rowMax < 0) rowMax = 0;
        if (columnMin > 0) columnMin = 0;
        if (columnMax < 0) columnMax = 0;
        EditorGUILayout.BeginVertical(GUI.skin.box);
        for (int i = rowMin; i <= rowMax; i++)
        {
            EditorGUILayout.BeginHorizontal();
            for (int j = columnMin; j <= columnMax; j++)
            {
                if (!(arrayIndex >= skillData.moveFieldIndexOffsetArray.Length) && i == skillData.moveFieldIndexOffsetArray[arrayIndex].rowOffset && j == skillData.moveFieldIndexOffsetArray[arrayIndex].columnOffset)
                {
                    if (i == 0 && j == 0) EditorGUILayout.LabelField(PLAYER_TRUE, GUILayout.Width(DOT_WIDTH), GUILayout.Height(DOT_HEIGHT));
                    else EditorGUILayout.LabelField(DOT, GUILayout.Width(DOT_WIDTH), GUILayout.Height(DOT_HEIGHT));
                    arrayIndex++;
                    if (arrayIndex >= skillData.moveFieldIndexOffsetArray.Length) break;
                }
                else
                {
                    if (i == 0 && j == 0) EditorGUILayout.LabelField(PLAYER_FALSE, GUILayout.Width(DOT_WIDTH), GUILayout.Height(DOT_HEIGHT));
                    else EditorGUILayout.LabelField(NONE, GUILayout.Width(DOT_WIDTH), GUILayout.Height(DOT_HEIGHT));
                }
            }
            EditorGUILayout.EndHorizontal();
        }
        EditorGUILayout.EndVertical();

        arrayIndex = 0;
        rowMin = int.MaxValue; rowMax = int.MinValue;
        columnMin = int.MaxValue; columnMax = int.MinValue;
        //��int rowMin = 0, rowMax = 12, columnMin = 0, columnMax = 12;

        // �ړ��͈͂�`�悷��ۂ̍ŏ�,�ŉ�,�ō�,�ŉE�����߂�
        foreach (FieldIndexOffset offset in skillData.selectFieldIndexOffsetArray)
        {
            if (offset.rowOffset < rowMin) rowMin = offset.rowOffset;
            if (offset.rowOffset > rowMax) rowMax = offset.rowOffset;
            if (offset.columnOffset < columnMin) columnMin = offset.columnOffset;
            if (offset.columnOffset > columnMax) columnMax = offset.columnOffset;
        }
        // ���S���W��`�悷��悤�ɂ���
        if (rowMin > 0) rowMin = 0;
        if (rowMax < 0) rowMax = 0;
        if (columnMin > 0) columnMin = 0;
        if (columnMax < 0) columnMax = 0;

        EditorGUILayout.BeginVertical(GUI.skin.box);
        for (int i = rowMin; i <= rowMax; i++)
        {
            EditorGUILayout.BeginHorizontal();
            for (int j = columnMin; j <= columnMax; j++)
            {
                if (!(arrayIndex >= skillData.selectFieldIndexOffsetArray.Length) && i == skillData.selectFieldIndexOffsetArray[arrayIndex].rowOffset && j == skillData.selectFieldIndexOffsetArray[arrayIndex].columnOffset)
                {
                    if (i == 0 && j == 0) EditorGUILayout.LabelField(PLAYER_TRUE, GUILayout.Width(DOT_WIDTH), GUILayout.Height(DOT_HEIGHT));
                    else EditorGUILayout.LabelField(DOT, GUILayout.Width(DOT_WIDTH), GUILayout.Height(DOT_HEIGHT));
                    arrayIndex++;
                    if (arrayIndex >= skillData.selectFieldIndexOffsetArray.Length) break;
                }
                else
                {
                    if (i == 0 && j == 0) EditorGUILayout.LabelField(PLAYER_FALSE, GUILayout.Width(DOT_WIDTH), GUILayout.Height(DOT_HEIGHT));
                    else EditorGUILayout.LabelField(NONE, GUILayout.Width(DOT_WIDTH), GUILayout.Height(DOT_HEIGHT));
                }
            }
            EditorGUILayout.EndHorizontal();
        }
        EditorGUILayout.EndVertical();

        arrayIndex = 0;
        rowMin = int.MaxValue; rowMax = int.MinValue;
        columnMin = int.MaxValue; columnMax = int.MinValue;
        //��int rowMin = 0, rowMax = 12, columnMin = 0, columnMax = 12;

        // �ړ��͈͂�`�悷��ۂ̍ŏ�,�ŉ�,�ō�,�ŉE�����߂�
        foreach (FieldIndexOffset offset in skillData.attackFieldIndexOffsetArray)
        {
            if (offset.rowOffset < rowMin) rowMin = offset.rowOffset;
            if (offset.rowOffset > rowMax) rowMax = offset.rowOffset;
            if (offset.columnOffset < columnMin) columnMin = offset.columnOffset;
            if (offset.columnOffset > columnMax) columnMax = offset.columnOffset;
        }
        // ���S���W��`�悷��悤�ɂ���
        if (rowMin > 0) rowMin = 0;
        if (rowMax < 0) rowMax = 0;
        if (columnMin > 0) columnMin = 0;
        if (columnMax < 0) columnMax = 0;

        EditorGUILayout.BeginVertical(GUI.skin.box);
        for (int i = rowMin; i <= rowMax; i++)
        {
            EditorGUILayout.BeginHorizontal();
            for (int j = columnMin; j <= columnMax; j++)
            {
                if (!(arrayIndex >= skillData.attackFieldIndexOffsetArray.Length) && i == skillData.attackFieldIndexOffsetArray[arrayIndex].rowOffset && j == skillData.attackFieldIndexOffsetArray[arrayIndex].columnOffset)
                {
                    if (i == 0 && j == 0) EditorGUILayout.LabelField(PLAYER_TRUE, GUILayout.Width(DOT_WIDTH), GUILayout.Height(DOT_HEIGHT));
                    else EditorGUILayout.LabelField(DOT, GUILayout.Width(DOT_WIDTH), GUILayout.Height(DOT_HEIGHT));
                    arrayIndex++;
                    if (arrayIndex >= skillData.attackFieldIndexOffsetArray.Length) break;
                }
                else
                {
                    if (i == 0 && j == 0) EditorGUILayout.LabelField(PLAYER_FALSE, GUILayout.Width(DOT_WIDTH), GUILayout.Height(DOT_HEIGHT));
                    else EditorGUILayout.LabelField(NONE, GUILayout.Width(DOT_WIDTH), GUILayout.Height(DOT_HEIGHT));
                }
            }
            EditorGUILayout.EndHorizontal();
        }
        EditorGUILayout.EndVertical();

        if (GUILayout.Button("�ۑ�"))
        {
            if (skillData.id == "")
            {
                EditorUtility.DisplayDialog("����ID���ݒ�", "����ID��ݒ肵�Ă��������B", "OK");
                return;
            }
            var obj = EditorUtility.InstanceIDToObject(target.GetInstanceID());
            Debug.Log("path=" + AssetDatabase.GetAssetPath(obj));
            //��ScriptableObjectDatabase.Write(skillData.id, AssetDatabase.GetAssetPath(obj));

            //AssetDatabase.Refresh();
            EditorUtility.SetDirty(skillData);
            UnityEditor.SceneManagement.EditorSceneManager.MarkSceneDirty(UnityEditor.SceneManagement.EditorSceneManager.GetActiveScene());
            AssetDatabase.SaveAssets();
        }

        EditorGUILayout.HelpBox("������̏ꍇ�ŕ\������Ă��܂��B\n�オ�I���\�}�X�A�����U���͈͂ł��B\n�I���\�}�X�ɂ����āA���̓L�����N�^�[�̈ʒu��\���܂��B\n�U���͈͂ɂ����āA���͑I�����ꂽ�}�X��\���܂��B\n���h��́��͂��̃}�X��͈͂Ɋ܂܂Ȃ����Ƃ��A\n���h��́��͂��̃}�X��͈͂Ɋ܂ނ��Ƃ�\���܂��B", MessageType.Info);
        EditorGUILayout.HelpBox("�t�@�C������ύX�����ꍇ�͕K���ۑ����Ă��������B", MessageType.Warning);
    }
}

