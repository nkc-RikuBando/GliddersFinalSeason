using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Glidders;
using Glidders.Buff;
using Glidders.Character;

public class BuffDataCreator : EditorWindow
{
    [MenuItem("Window/BuffDataCreator")]
    static void Open()
    {
        EditorWindow.GetWindow<BuffDataCreator>("BuffDataCreator");
    }

    List<BuffValueData> buffValueDataList;
    BuffViewData buffViewData;

    bool initialize = true;
    int buffValueCount = 0;

    string id;
    string buffName;
    string buffCaption;
    Sprite buffIcon;
    GameObject effectObjectPrefab;
    CharacterScriptableObject upperTransform;
    CharacterScriptableObject lowerTransform;

    //List<StatusTypeEnum> buffStatusList;
    List<int> buffTypeIndexList;
    List<float> buffScaleList;
    List<int> buffDurationList;

    private string[] signArray = { "+", "*" };

    // ���ӂ����v�f�B�����Ă������ł��B
    int jokeInt;

    private void OnGUI()
    {
        if (initialize)
        {
            // �X�L���f�[�^��ۑ�����ScriptableObject�̍쐬
            buffValueDataList = new List<BuffValueData>();
            buffViewData = ScriptableObject.CreateInstance<BuffViewData>();
            buffViewData.buffValueList = new List<BuffValueData>();

            Reset();

            initialize = false;

            GUILayout.Box(EditorGUIUtility.Load("BuildSettings.Switch") as Texture, GUI.skin.label);

            // ���ӂ����v�f�B�����Ă������ł��B
            jokeInt = Joke();
        }

        using (new EditorGUILayout.VerticalScope())
        {
            // �o�t�̕\���Ɋւ������ݒ�
            id = EditorGUILayout.TextField("����ID", id);
            buffViewData.id = id;

            buffIcon = EditorGUILayout.ObjectField("�o�t�A�C�R��", buffIcon, typeof(Sprite), true) as Sprite;
            buffViewData.buffIcon = buffIcon;

            buffName = EditorGUILayout.TextField("�o�t����", buffName);
            buffViewData.buffName = buffName;

            buffCaption = EditorGUILayout.TextField("�o�t������", buffCaption);
            buffViewData.buffCaption = buffCaption;

            effectObjectPrefab = EditorGUILayout.ObjectField("���o�I�u�W�F�N�gPrfab", effectObjectPrefab, typeof(GameObject), true) as GameObject;
            buffViewData.effectObjectPrefab = effectObjectPrefab;

            upperTransform = EditorGUILayout.ObjectField("�ϐg��̃L�����N�^�[", upperTransform, typeof(CharacterScriptableObject), true) as CharacterScriptableObject;
            buffViewData.upperTransform = upperTransform;

            lowerTransform = EditorGUILayout.ObjectField("�ϐg�O�̃L�����N�^�[", lowerTransform, typeof(CharacterScriptableObject), true) as CharacterScriptableObject;
            buffViewData.lowerTransform = lowerTransform;

            using (new EditorGUILayout.VerticalScope(GUI.skin.box))
            {
                int indexWidth = 20;
                int popupWidth = 40;
                int enumFieldWidth = 100;
                int scaleFieldWidth = 60;
                int durationFieldWidth = 70;

                using (new EditorGUILayout.HorizontalScope())
                {
                    // ���ۂɑ�������X�e�[�^�X�̌��o����\��
                    EditorGUILayout.LabelField(" ", GUILayout.Width(indexWidth));

                    EditorGUILayout.LabelField("�o�t����X�e�[�^�X", GUILayout.Width(enumFieldWidth));

                    EditorGUILayout.LabelField("�{��/���Z", GUILayout.Width(popupWidth + scaleFieldWidth));

                    EditorGUILayout.LabelField("�p���^�[����", GUILayout.Width(durationFieldWidth));
                }

                for (int i = 0; i < buffValueCount; i++)
                {
                    using (new EditorGUILayout.HorizontalScope())
                    {
                        // ���ۂɑ�������X�e�[�^�X��ݒ�
                        EditorGUILayout.LabelField(string.Format("{0:00}", i), GUILayout.Width(indexWidth));

                        //��buffStatusList[i] = (StatusTypeEnum)EditorGUILayout.EnumPopup("", buffStatusList[i], GUILayout.Width(enumFieldWidth));
                        //��buffValueDataList[i].buffedStatus = buffStatusList[i];

                        buffTypeIndexList[i] = EditorGUILayout.Popup(buffTypeIndexList[i], signArray, GUILayout.Width(popupWidth));
                        buffValueDataList[i].buffType = (BuffTypeEnum)buffTypeIndexList[i];

                        buffScaleList[i] = EditorGUILayout.FloatField("", buffScaleList[i], GUILayout.Width(scaleFieldWidth));
                        buffValueDataList[i].buffScale = buffScaleList[i];

                        buffDurationList[i] = EditorGUILayout.IntField("", buffDurationList[i], GUILayout.Width(durationFieldWidth));
                        buffValueDataList[i].buffDuration = buffDurationList[i];

                        // �o�t�f�[�^��2���ȏ゠��Ȃ�-�{�^����\��
                        if (buffValueCount >= 2)
                        {
                            // -�{�^���������ꂽ�Ƃ��A���̃o�t���폜����
                            if (GUILayout.Button("-", GUILayout.Width(indexWidth)))
                            {
                                RemoveDataFromAllList(i);
                            }
                        }
                    }
                }

                using (new EditorGUILayout.HorizontalScope())
                {
                    EditorGUILayout.LabelField(" ", GUILayout.Width(indexWidth));

                    // +�{�^���������ꂽ�Ƃ��A�o�t����X�e�[�^�X��ǉ�����
                    if (GUILayout.Button("+", GUILayout.Width(indexWidth)))
                    {
                        AddDataToAllList();
                    }
                }
            }

            EditorGUILayout.Space();

            // �ۑ��{�^���������ꂽ�Ƃ��ɕۑ�����������
            if (GUILayout.Button("�ۑ�"))
            {
                // ����ID�����͂���Ă��Ȃ��ꍇ
                if (buffViewData.id == "")
                {
                    EditorUtility.DisplayDialog("����ID���ݒ�", "����ID��ݒ肵�Ă��������B", "OK");
                    return;
                }

                if (EditorUtility.DisplayDialog("�ۑ��m�F", "�o�t�f�[�^��ۑ����܂����H", "OK", "cancel"))
                {
                    CreateBuffData();
                    initialize = true;
                }
            }

            // ���ӂ����v�f�B�����Ă������ł��B
            switch (jokeInt)
            {
                case 0:
                    for (int i = 0; i < 6; i++) EditorGUILayout.Space();
                    GUILayout.Box(EditorGUIUtility.Load("BuildSettings.Switch") as Texture, GUI.skin.label);
                    break;
                case 1:
                    for (int i = 0; i < 6; i++) EditorGUILayout.Space();
                    GUILayout.Box(EditorGUIUtility.Load("BuildSettings.PS4") as Texture, GUI.skin.label);
                    break;
                case 2:
                    for (int i = 0; i < 6; i++) EditorGUILayout.Space();
                    GUILayout.Box(EditorGUIUtility.Load("BuildSettings.XboxOne") as Texture, GUI.skin.label);
                    break;
            }
        }
    }

    /// <summary>
    /// �S�Ẵ��X�g�ɗv�f����ǉ�����
    /// </summary>
    private void AddDataToAllList()
    {
        buffValueCount++;

        buffValueDataList.Add(ScriptableObject.CreateInstance<BuffValueData>());
        //��buffStatusList.Add(StatusTypeEnum.DAMAGE);
        buffTypeIndexList.Add(1);
        buffScaleList.Add(1.0f);
        buffDurationList.Add(1);
    }

    /// <summary>
    /// �S�Ẵ��X�g�̗v�f����폜����
    /// </summary>
    private void RemoveDataFromAllList(int removeIndex)
    {
        buffValueCount--;

        buffValueDataList.RemoveAt(removeIndex);
        //��buffStatusList.RemoveAt(removeIndex);
        buffTypeIndexList.RemoveAt(removeIndex);
        buffScaleList.RemoveAt(removeIndex);
        buffDurationList.RemoveAt(removeIndex);
    }

    private void Reset()
    {
        buffValueCount = 0;

        id = "";
        buffName = "";
        buffCaption = "";
        buffIcon = null;
        effectObjectPrefab = null;
        upperTransform = null;
        lowerTransform = null;

        //��buffStatusList = new List<StatusTypeEnum>();
        buffTypeIndexList = new List<int>();
        buffScaleList = new List<float>();
        buffDurationList = new List<int>();

        AddDataToAllList();
    }

    public void CreateBuffData()
    {
        bool updateFlg = false;

        // �o�t�f�[�^�̕ۑ����ݒ�
        const string PATH = "Assets/Resources/ScriptableObjects/Buffs/";
        string path = PATH + string.Format("{0}", buffViewData.buffName) + ".asset";
        var asset = AssetDatabase.LoadAssetAtPath(path, typeof(BuffViewData));

        // �����̃o�t�f�[�^�����ɑ��݂��邩�m�F���A���݂����ꍇ�͍폜����
        if (asset != null)
        {
            updateFlg = true;
            AssetDatabase.DeleteAsset(path);
            int i = 0;
            while (true)
            {
                string valuePath = PATH + string.Format("{0}_{1:00}", buffViewData.buffName, i) + ".asset";
                var valueAsset = AssetDatabase.LoadAssetAtPath(valuePath, typeof(BuffValueData));
                if (valueAsset == null)
                {
                    break;
                }
                else
                {
                    AssetDatabase.DeleteAsset(valuePath);
                }
                i++;
            }
        }

        for (int i = 0; i < buffValueCount; i++)
        {
            string valuePath = PATH + string.Format("{0}_{1:00}", buffViewData.buffName, i) + ".asset";

            // �C���X�^���X���������̂��A�Z�b�g�Ƃ��ĕۑ�
            // valueData�𐶐�
            AssetDatabase.CreateAsset(buffValueDataList[i], valuePath);

            EditorUtility.SetDirty(buffValueDataList[i]);
            AssetDatabase.SaveAssets();

            // BuffViewData�̃��X�g�ɐ�������ValueData��ǉ�
            buffViewData.buffValueList.Add(buffValueDataList[i]);
        }

        // �C���X�^���X���������̂��A�Z�b�g�Ƃ��ĕۑ�
        // viewData�𐶐�
        AssetDatabase.CreateAsset(buffViewData, path);
        //��ScriptableObjectDatabase.Write(buffViewData.id, path);
        EditorUtility.SetDirty(buffViewData);
        AssetDatabase.SaveAssets();

        if (updateFlg)
        {
            Debug.Log(string.Format($"Updated \"{buffViewData.buffName}\""));
        }
        else
        {
            Debug.Log(string.Format($"Created new buff data, \"{buffViewData.buffName}\"!"));
        }
    }

    /// <summary>
    /// ���ӂ����֐��B�����Ă�������ł��B
    /// </summary>
    /// <returns></returns>
    private int Joke()
    {
        int random = (int)(Random.value * 50);
        return random;
    }
}
