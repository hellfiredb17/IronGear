using UnityEngine;
using Rigs;

[CreateAssetMenu(menuName = "IronGears/MechComponentPreference")]
public class MechComponentPreferences : ScriptableObject
{
    [Header("Component Prefabs")]
    public Mech MechPrefab;
    public MechBase MechBasePrefab;
    public MechTorso MechTorsoPrefab;
    public MechArm MechArmPrefab;

    [Header("Content paths")]
    public string MechBaseContentPath = "/Prefabs/MechParts/BaseModels/";
    public string MechTorsoContentPath = "/Prefabs/MechParts/TorsoModels/";
    public string MechArmLeftContentPath = "/Prefabs/MechParts/ArmModels/Left/";
    public string MechArmRightContentPath = "/Prefabs/MechParts/ArmModels/Right/";

    [Header("Json paths")]
    public string MechBaseJsonPath = "/Json/MechComponents/Base/";
    public string MechTorsoJsonPath = "/Json/MechComponents/Torso/";
    public string MechArmLeftJsonPath = "/Json/MechComponents/ArmLeft/";
    public string MechArmRightJsonPath = "/Json/MechComponents/ArmRight/";
}
