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
    // 攻撃範囲の表示に使用
    const string DOT = "■";
    const string NONE = "　";
    const string PLAYER_FALSE = "△";
    const string PLAYER_TRUE = "▲";
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
        EditorGUILayout.LabelField("識別ID");
        skillData.id = EditorGUILayout.TextField("", skillData.id);
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("ユニークスキルかどうか");
        skillData.isUniqueSkill = EditorGUILayout.Toggle("", skillData.isUniqueSkill);
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("スキル名称");
        skillData.skillName = EditorGUILayout.TextField("", skillData.skillName);
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.LabelField("スキル説明文");
        skillData.skillCaption = EditorGUILayout.TextArea(skillData.skillCaption);

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("消費エネルギー");
        skillData.energy = EditorGUILayout.IntSlider(skillData.energy, 1, 5);
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("優先度");
        skillData.priority = EditorGUILayout.IntSlider(skillData.priority, 1, 10);
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("ダメージ");
        skillData.damage = EditorGUILayout.IntField(skillData.damage);
        EditorGUILayout.EndHorizontal();      

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("威力");
        skillData.power = EditorGUILayout.IntSlider(skillData.power, 0, 5);
        EditorGUILayout.EndHorizontal();

        skillData.skillType = (SkillTypeEnum)EditorGUILayout.EnumPopup("スキルの種類", skillData.skillType);

        // このスキルで付与される、失うバフを設定
        int buffButtonWidth = 20;
        int buffObjectWidth = 160;
        using (new GUILayout.HorizontalScope())
        {
            EditorGUILayout.LabelField("付与されるバフ");
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
            EditorGUILayout.LabelField("失うバフ");
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

        skillData.moveType = (UniqueSkillMoveType)EditorGUILayout.EnumPopup("移動の種類", skillData.moveType);

        skillData.skillAnimation = EditorGUILayout.ObjectField("アニメーションクリップ", skillData.skillAnimation, typeof(AnimationClip), true) as AnimationClip;

        skillData.skillIcon = EditorGUILayout.ObjectField("スキルアイコン", skillData.skillIcon, typeof(Sprite), true, GUILayout.Width(224), GUILayout.Height(224)) as Sprite;

        //FieldIndexOffset[] selectArray = skillData.selectFieldIndexOffsetArray;
        

        int arrayIndex = 0;
        int rowMin = int.MaxValue, rowMax = int.MinValue;
        int columnMin = int.MaxValue, columnMax = int.MinValue;
        //※int rowMin = 0, rowMax = 12, columnMin = 0, columnMax = 12;
        // 移動範囲を描画する際の最上,最下,最左,最右を求める
        foreach (FieldIndexOffset offset in skillData.moveFieldIndexOffsetArray)
        {
            if (offset.rowOffset < rowMin) rowMin = offset.rowOffset;
            if (offset.rowOffset > rowMax) rowMax = offset.rowOffset;
            if (offset.columnOffset < columnMin) columnMin = offset.columnOffset;
            if (offset.columnOffset > columnMax) columnMax = offset.columnOffset;
        }
        // 中心座標を描画するようにする
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
        //※int rowMin = 0, rowMax = 12, columnMin = 0, columnMax = 12;

        // 移動範囲を描画する際の最上,最下,最左,最右を求める
        foreach (FieldIndexOffset offset in skillData.selectFieldIndexOffsetArray)
        {
            if (offset.rowOffset < rowMin) rowMin = offset.rowOffset;
            if (offset.rowOffset > rowMax) rowMax = offset.rowOffset;
            if (offset.columnOffset < columnMin) columnMin = offset.columnOffset;
            if (offset.columnOffset > columnMax) columnMax = offset.columnOffset;
        }
        // 中心座標を描画するようにする
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
        //※int rowMin = 0, rowMax = 12, columnMin = 0, columnMax = 12;

        // 移動範囲を描画する際の最上,最下,最左,最右を求める
        foreach (FieldIndexOffset offset in skillData.attackFieldIndexOffsetArray)
        {
            if (offset.rowOffset < rowMin) rowMin = offset.rowOffset;
            if (offset.rowOffset > rowMax) rowMax = offset.rowOffset;
            if (offset.columnOffset < columnMin) columnMin = offset.columnOffset;
            if (offset.columnOffset > columnMax) columnMax = offset.columnOffset;
        }
        // 中心座標を描画するようにする
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

        if (GUILayout.Button("保存"))
        {
            if (skillData.id == "")
            {
                EditorUtility.DisplayDialog("識別ID未設定", "識別IDを設定してください。", "OK");
                return;
            }
            var obj = EditorUtility.InstanceIDToObject(target.GetInstanceID());
            Debug.Log("path=" + AssetDatabase.GetAssetPath(obj));
            //※ScriptableObjectDatabase.Write(skillData.id, AssetDatabase.GetAssetPath(obj));

            //AssetDatabase.Refresh();
            EditorUtility.SetDirty(skillData);
            UnityEditor.SceneManagement.EditorSceneManager.MarkSceneDirty(UnityEditor.SceneManagement.EditorSceneManager.GetActiveScene());
            AssetDatabase.SaveAssets();
        }

        EditorGUILayout.HelpBox("上向きの場合で表示されています。\n上が選択可能マス、下が攻撃範囲です。\n選択可能マスにおいて、△はキャラクターの位置を表します。\n攻撃範囲において、△は選択されたマスを表します。\n白塗りの△はそのマスを範囲に含まないことを、\n黒塗りの▲はそのマスを範囲に含むことを表します。", MessageType.Info);
        EditorGUILayout.HelpBox("ファイル名を変更した場合は必ず保存してください。", MessageType.Warning);
    }
}

