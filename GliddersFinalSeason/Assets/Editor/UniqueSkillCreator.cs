using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Glidders;
using Glidders.Character;
using Glidders.Buff;

public class UniqueSkillCreator : EditorWindow
{
    // �A�Z�b�g�t�@�C���쐬�p�̃N���X
    UniqueSkillScriptableObject uniqueSkillData;

    bool initialize = true;
    bool isAttack = false;
    List<BuffViewData> giveBuff; // �t�^�����o�t
    List<BuffViewData> loseBuff; // �����o�t

    // �t�B�[���h�T�C�Y�̐ݒ�
    static int fieldSize = 11;                           // �ΐ�̃t�B�[���h�T�C�Y
    static int rangeSize = fieldSize * 2 - 1;           // �t�B�[���h�T�C�Y����A�U���͈͓h��ɕK�v�ȃT�C�Y���v�Z
    FieldIndex centerIndex = new FieldIndex(rangeSize / 2, rangeSize / 2);   // �v���C���[�̈ʒu�ƂȂ�A�͈͂̒��S

    int selectRangeToggleSize = 15;
    int width = 300;
    Vector2 skillIconSize = new Vector2(224, 224);            // �X�L���A�C�R���̃T�C�Y

    bool[,] moveArray = new bool[rangeSize, rangeSize];         // �ړ���}�X���i�[����񎟌��z��
    bool[,] selectArray = new bool[rangeSize, rangeSize];       // �I���\�}�X���i�[����񎟌��z��
    bool[,] attackArray = new bool[rangeSize, rangeSize];       // �U���͈̓}�X���i�[����񎟌��z��
    //bool[,] moveArrayBefore = new bool[rangeSize, rangeSize];
    //bool[,] selectArrayBefore = new bool[rangeSize, rangeSize];
    //bool[,] attackArrayBefore = new bool[rangeSize, rangeSize];

    [MenuItem("Window/UniqueSkillCreator")]
    static void Open()
    {
        EditorWindow.GetWindow<UniqueSkillCreator>("UniqueSkillCreator");
    }

    private void OnGUI()
    {
        if (initialize)
        {
            // �X�L���f�[�^��ۑ�����ScriptableObject�̍쐬
            uniqueSkillData = ScriptableObject.CreateInstance<UniqueSkillScriptableObject>();

            Reset();

            //��skillData.gridList = new List<FieldIndexOffset>();
            initialize = false;
        }

        using (new EditorGUILayout.HorizontalScope())
        {
            using (new EditorGUILayout.VerticalScope())
            {
                using (new EditorGUILayout.HorizontalScope())
                {
                    // ����ID
                    EditorGUILayout.BeginVertical(GUI.skin.box);
                    uniqueSkillData.id = EditorGUILayout.TextField("����ID", uniqueSkillData.id, GUILayout.Width(width));
                    EditorGUILayout.EndVertical();

                    // ����
                    EditorGUILayout.BeginVertical(GUI.skin.box);
                    uniqueSkillData.skillName = EditorGUILayout.TextField("����", uniqueSkillData.skillName, GUILayout.Width(width));
                    EditorGUILayout.EndVertical();

                    // ���j�[�N�X�L�����ǂ���
                    EditorGUILayout.BeginVertical(GUI.skin.box);
                    uniqueSkillData.isUniqueSkill = EditorGUILayout.Toggle("���j�[�N�X�L�����ǂ���", uniqueSkillData.isUniqueSkill);
                    EditorGUILayout.EndVertical();
                }

                //�X�L��������uniqueSkillData.skillCaption = EditorGUILayout.TextField("������", uniqueSkillData.skillCaption);
                EditorGUILayout.LabelField("������");
                uniqueSkillData.skillCaption = EditorGUILayout.TextArea(uniqueSkillData.skillCaption);

                
                using (new GUILayout.HorizontalScope())
                {
                    // �G�l���M�[
                    EditorGUILayout.BeginVertical(GUI.skin.box);
                    uniqueSkillData.energy = EditorGUILayout.IntSlider("�G�l���M�[", uniqueSkillData.energy, 1, 5, GUILayout.Width(width));
                    EditorGUILayout.EndVertical();

                    // �D��x
                    EditorGUILayout.BeginVertical(GUI.skin.box);
                    uniqueSkillData.priority = EditorGUILayout.IntSlider("�D��x", uniqueSkillData.priority, 1, 10, GUILayout.Width(width));
                    EditorGUILayout.EndVertical();
                }

                EditorGUILayout.Space();

                using (new EditorGUILayout.HorizontalScope())
                {
                    // �ړ��̎��
                    EditorGUILayout.BeginVertical(GUI.skin.box);
                    uniqueSkillData.moveType = (UniqueSkillMoveType)EditorGUILayout.EnumPopup("�ړ��̎��", uniqueSkillData.moveType, GUILayout.Width(width));
                    EditorGUILayout.EndVertical();

                    // �U�����邩�ǂ���
                    EditorGUILayout.BeginVertical(GUI.skin.box);
                    uniqueSkillData.skillType = (SkillTypeEnum)EditorGUILayout.EnumPopup("�Z�̎��", uniqueSkillData.skillType);
                    EditorGUILayout.EndVertical();
                }

                using (new EditorGUILayout.HorizontalScope())
                {
                    EditorGUILayout.LabelField("�@�@�@�@�@�@�@NONE �c�ړ����Ȃ�");
                    EditorGUILayout.LabelField("�@�@�@�@�@�@�@FREE �c�ʏ�ړ�");
                    EditorGUILayout.LabelField("�@�@�@�@�@�@�@FIXED�c�Œ�ړ�");
                }

                using (new GUILayout.HorizontalScope())
                {
                    // �_���[�W
                    EditorGUILayout.BeginVertical(GUI.skin.box);
                    uniqueSkillData.damage = EditorGUILayout.IntField("�_���[�W", uniqueSkillData.damage, GUILayout.Width(width));
                    EditorGUILayout.EndVertical();

                    // �З�
                    EditorGUILayout.BeginVertical(GUI.skin.box);
                    uniqueSkillData.power = EditorGUILayout.IntSlider("�З�(�_���[�W�t�B�[���h)", uniqueSkillData.power, 0, 5, GUILayout.Width(width));
                    EditorGUILayout.EndVertical();
                }

                // ���̃X�L���ŕt�^�����A�����o�t��ݒ�
                int buffButtonWidth = 20;
                int buffObjectWidth = 260;
                using (new GUILayout.HorizontalScope())
                {
                    EditorGUILayout.LabelField("�t�^�����o�t", GUILayout.Width(width));
                    using (new GUILayout.VerticalScope())
                    {
                        for (int i = 0; i < giveBuff.Count; i++)
                        {
                            using (new GUILayout.HorizontalScope())
                            {
                                giveBuff[i] = EditorGUILayout.ObjectField("", giveBuff[i], typeof(BuffViewData), true, GUILayout.Width(buffObjectWidth)) as BuffViewData;
                                if (GUILayout.Button("-", GUILayout.Width(buffButtonWidth)))
                                {
                                    giveBuff.RemoveAt(i);
                                }
                            }
                        }
                        if (GUILayout.Button("+", GUILayout.Width(buffButtonWidth)))
                        {
                            giveBuff.Add(null);
                        }
                    }
                }
                using (new GUILayout.HorizontalScope())
                {
                    EditorGUILayout.LabelField("�����o�t", GUILayout.Width(width));
                    using (new GUILayout.VerticalScope())
                    {
                        for (int i = 0; i < loseBuff.Count; i++)
                        {
                            using (new GUILayout.HorizontalScope())
                            {
                                loseBuff[i] = EditorGUILayout.ObjectField("", loseBuff[i], typeof(BuffViewData), true, GUILayout.Width(buffObjectWidth)) as BuffViewData;
                                if (GUILayout.Button("-", GUILayout.Width(buffButtonWidth)))
                                {
                                    loseBuff.RemoveAt(i);
                                }
                            }
                        }
                        if (GUILayout.Button("+", GUILayout.Width(buffButtonWidth)))
                        {
                            loseBuff.Add(null);
                        }
                    }
                }
                uniqueSkillData.giveBuff = giveBuff;
                uniqueSkillData.loseBuff = loseBuff;
            }

            using (new GUILayout.VerticalScope(GUI.skin.box))
            {
                uniqueSkillData.skillIcon = EditorGUILayout.ObjectField("�X�L���A�C�R��", uniqueSkillData.skillIcon, typeof(Sprite), true, GUILayout.Width(skillIconSize.x), GUILayout.Height(skillIconSize.y)) as Sprite;
                uniqueSkillData.skillAnimation = EditorGUILayout.ObjectField("�A�j���[�V�����N���b�v", uniqueSkillData.skillAnimation, typeof(AnimationClip), true) as AnimationClip;
            }
        }

        EditorGUILayout.Space();
        using (new GUILayout.HorizontalScope())
        {
            using (new GUILayout.VerticalScope(GUI.skin.box))
            {
                using (new GUILayout.VerticalScope())
                {
                    EditorGUILayout.LabelField("�ړ���}�X");
                    EditorGUILayout.Space();

                    // �ړ���}�X
                    // �I���\�}�X����͂���g�O����\��
                    for (int i = 0; i < rangeSize; ++i)
                    {
                        EditorGUILayout.BeginHorizontal();      // �����\�������邽�߁A����\���̐ݒ������
                        for (int j = 0; j < rangeSize; ++j)
                        {
                            // �͈͂̒����Ȃ�A�v���C���[��\������
                            if (i == centerIndex.row && j == centerIndex.column)
                            {
                                moveArray[i, j] = EditorGUILayout.Toggle("", moveArray[i, j], GUI.skin.toggle, GUILayout.Width(selectRangeToggleSize), GUILayout.Height(selectRangeToggleSize));
                                /*// �g�O���̒l���X�V���ꂽ�ꍇ�A���X�g�̓��e���X�V���� �����ݕs�v�A����͓��l�̕����L�ڂ���Ă����ꏊ
                                if (moveArray[i, j] != moveArrayBefore[i, j])
                                {
                                    SetArrayData(new FieldIndex(i, j) - centerIndex, "Move");
                                }
                                moveArrayBefore[i, j] = moveArray[i, j];*/
                                continue;
                            }
                            // �{�^���̌����ڂɕύX�����g�O����\��
                            moveArray[i, j] = EditorGUILayout.Toggle("", moveArray[i, j], GUI.skin.button, GUILayout.Width(selectRangeToggleSize), GUILayout.Height(selectRangeToggleSize));
                            // ��
                        }
                        EditorGUILayout.EndHorizontal();        // �����̕\���������������߁A����\���̏I��
                    }
                    EditorGUILayout.Space();
                    EditorGUILayout.HelpBox("�Œ�ړ��̏ꍇ�͈ړ��̏I�_��ݒ肵�Ă��������B\n���g�̃}�X���I���\�Ȃ璆�S�̃`�F�b�N�����Ă��������B", MessageType.Warning);
                }
            }

            using (new GUILayout.VerticalScope(GUI.skin.box))
            {
                using (new GUILayout.VerticalScope())
                {
                    EditorGUILayout.LabelField("�U���I���\�}�X");
                    EditorGUILayout.Space();

                    // �I���\�}�X����͂���g�O����\��
                    for (int i = 0; i < rangeSize; ++i)
                    {
                        EditorGUILayout.BeginHorizontal();      // �����\�������邽�߁A����\���̐ݒ������
                        for (int j = 0; j < rangeSize; ++j)
                        {
                            // �͈͂̒����Ȃ�A�v���C���[��\������
                            if (i == centerIndex.row && j == centerIndex.column)
                            {
                                selectArray[i, j] = EditorGUILayout.Toggle("", selectArray[i, j], GUI.skin.toggle, GUILayout.Width(selectRangeToggleSize), GUILayout.Height(selectRangeToggleSize));
                                // ��
                                continue;
                            }
                            // �{�^���̌����ڂɕύX�����g�O����\��
                            selectArray[i, j] = EditorGUILayout.Toggle("", selectArray[i, j], GUI.skin.button, GUILayout.Width(selectRangeToggleSize), GUILayout.Height(selectRangeToggleSize));
                            // ��
                        }
                        EditorGUILayout.EndHorizontal();        // �����̕\���������������߁A����\���̏I��
                    }
                    EditorGUILayout.Space();
                    EditorGUILayout.HelpBox("������̏ꍇ�œ��͂��Ă��������B\n���g�̃}�X���I���\�Ȃ璆�S�̃`�F�b�N�����Ă��������B", MessageType.Warning);
                }
            }

            using (new GUILayout.VerticalScope(GUI.skin.box))
            {
                using (new GUILayout.VerticalScope())
                {
                    EditorGUILayout.LabelField("�U���͈̓}�X");
                    EditorGUILayout.Space();

                    // �U���}�X����͂���g�O����\��
                    for (int i = 0; i < rangeSize; ++i)
                    {
                        EditorGUILayout.BeginHorizontal();      // �����\�������邽�߁A����\���̐ݒ������
                        for (int j = 0; j < rangeSize; ++j)
                        {
                            // �͈͂̒����Ȃ�A�v���C���[��\������
                            if (i == centerIndex.row && j == centerIndex.column)
                            {
                                attackArray[i, j] = EditorGUILayout.Toggle("", attackArray[i, j], GUI.skin.toggle, GUILayout.Width(selectRangeToggleSize), GUILayout.Height(selectRangeToggleSize));
                                // ��
                                continue;
                            }
                            // �{�^���̌����ڂɕύX�����g�O����\��
                            attackArray[i, j] = EditorGUILayout.Toggle("", attackArray[i, j], GUI.skin.button, GUILayout.Width(selectRangeToggleSize), GUILayout.Height(selectRangeToggleSize));
                            // ��
                        }
                        EditorGUILayout.EndHorizontal();        // �����̕\���������������߁A����\���̏I��
                    }
                    EditorGUILayout.Space();
                    EditorGUILayout.HelpBox("������̏ꍇ�œ��͂��Ă��������B\n���S�̃`�F�b�N�}�X�͑I���\�}�X�őI�΂ꂽ�}�X�ƑΉ����܂��B\n���g�̃}�X���U���͈͂Ȃ璆�S�̃`�F�b�N�����Ă��������B", MessageType.Warning);
                }
            }
        }

        // �X�L���f�[�^�ۑ��{�^��
        if (GUILayout.Button("�X�L���f�[�^�ۑ�"))
        {
            // �X�L�����̂����͂���Ă��Ȃ��ꍇ
            if (uniqueSkillData.skillName == "")
            {
                EditorUtility.DisplayDialog("Error!", string.Format("�X�L�����̂����͂���Ă��܂���B"), "OK");
                return;
            }
            // ����ID�����͂���Ă��Ȃ��ꍇ
            if (uniqueSkillData.id == "")
            {
                EditorUtility.DisplayDialog("����ID���ݒ�", "����ID��ݒ肵�Ă��������B", "OK");
                return;
            }

            // �ۑ��m�F
            if (!EditorUtility.DisplayDialog("�X�L���f�[�^�ۑ��m�F", string.Format("�X�L���f�[�^��ۑ����܂����H"), "OK", "CANCEL")) return;

            Debug.Log("rangeSize = " + rangeSize);
            for (int i = 0; i < rangeSize; i++)
            {
                for (int j = 0; j <rangeSize; j++)
                {
                    Debug.Log("movearray = " + moveArray[i, j]);
                }
            }
            // �񎟌��z����ꎟ���z��ɕϊ����ĕۑ�����
            for (int i = 0; i < rangeSize; ++i)
            {
                for (int j = 0; j < rangeSize; ++j)
                {
                    uniqueSkillData.moveSelectArray[i * rangeSize + j] = moveArray[i, j];
                    uniqueSkillData.attackSelectArray[i * rangeSize + j] = selectArray[i, j];
                    uniqueSkillData.attackArray[i * rangeSize + j] = attackArray[i, j];
                }
            }

            SaveUniqueSkillData(uniqueSkillData);
            Close();
        }

        // ���Z�b�g�{�^���̔z�u
        if (GUILayout.Button("���Z�b�g"))
        {
            if (EditorUtility.DisplayDialog("���Z�b�g�m�F", string.Format("���͂����f�[�^�����Z�b�g���܂����H"), "OK", "cancel")) Reset();
        }
    }

    private void Reset()
    {
        /*
        // �e��ϐ��̏�����
        skillName = "";
        energy = 0;
        damage = 0;
        priority = 1;
        power = 0;
        skillIcon = null;
        skillType = SkillTypeEnum.ATTACK;
        giveBuff = new List<BuffViewData>();
        uniqueSkillData.selectGridArray = new bool[rangeSize * rangeSize];
        uniqueSkillData.attackGridArray = new bool[rangeSize * rangeSize];
        uniqueSkillData.rangeSize = rangeSize;
        uniqueSkillData.center = rangeSize / 2;
        */
        uniqueSkillData.skillName = "";
        uniqueSkillData.skillCaption = "";
        uniqueSkillData.energy = 1;
        uniqueSkillData.priority = 1;
        uniqueSkillData.skillIcon = null;
        uniqueSkillData.skillAnimation = null;
        uniqueSkillData.moveType = UniqueSkillMoveType.NONE;
        uniqueSkillData.damage = 0;
        uniqueSkillData.power = 0;
        uniqueSkillData.moveSelectArray = new bool[rangeSize * rangeSize];
        uniqueSkillData.attackSelectArray = new bool[rangeSize * rangeSize];
        uniqueSkillData.attackArray = new bool[rangeSize * rangeSize];
        giveBuff = new List<BuffViewData>();
        loseBuff = new List<BuffViewData>();

        // �U���͈͂̓񎟌��z��̏�����
        for (int i = 0; i < rangeSize; ++i)
        {
            for (int j = 0; j < rangeSize; ++j)
            {
                moveArray[i, j] = false;
                selectArray[i, j] = false;
                attackArray[i, j] = false;
            }
        }

        moveArray[centerIndex.row, centerIndex.column] = true;
        selectArray[centerIndex.row, centerIndex.column] = true;
        attackArray[centerIndex.row, centerIndex.column] = true;
    }

    private void SaveUniqueSkillData(UniqueSkillScriptableObject UniqueSkillScriptableObject)
    {
        const string PATH = "Assets/Resources/ScriptableObjects/Skills/";
        string path = PATH + uniqueSkillData.id + "_" + uniqueSkillData.skillName + ".asset";

        // �C���X�^���X���������̂��A�Z�b�g�Ƃ��ĕۑ�
        var asset = AssetDatabase.LoadAssetAtPath(path, typeof(UniqueSkillScriptableObject));
        if (asset == null)
        {
            // �w��̃p�X�Ƀt�@�C�������݂��Ȃ��ꍇ�͐V�K�쐬
            AssetDatabase.CreateAsset(uniqueSkillData, path);
            Debug.Log(string.Format($"Created new skill, \"{uniqueSkillData.skillName}\"!"));
        }
        else
        {
            // �w��̃p�X�Ɋ��ɓ����̃t�@�C�������݂���ꍇ�̓f�[�^��j��
            //EditorUtility.CopySerialized(skillData, asset);
            //AssetDatabase.SaveAssets();
            //Debug.Log(string.Format($"Updated \"{skillData.skillName}\"!"));            
            Debug.Log(string.Format($"\"{uniqueSkillData.skillName}\" has already been created!\n Please Update On Inspector Window!"));
        }
        //��ScriptableObjectDatabase.Write(uniqueSkillData.id, path);
        EditorUtility.SetDirty(uniqueSkillData);
        AssetDatabase.SaveAssets();
        //AssetDatabase.Refresh()
    }
}
