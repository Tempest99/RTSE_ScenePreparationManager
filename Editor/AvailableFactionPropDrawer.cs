using RTSEngine.Determinism;
using System.Collections;
using System.Collections.Generic;
using RTSEngine.Entities;
using RTSEngine.Game;
using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(AvailableFaction))]
public class AvailableFactionPropDrawer : PropertyDrawer
{



    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        // Using BeginProperty / EndProperty on the parent property means that
        // prefab override logic works on the entire property.
        EditorGUI.BeginProperty(position, label, property);

        // Draw label
        position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), GUIContent.none);

        // Don't make child fields be indented
        var indent = EditorGUI.indentLevel;
        EditorGUI.indentLevel = 0;

        EditorGUIUtility.labelWidth = 0.1f;
        // Calculate rects
        var FacTypeRect = new Rect(position.x, position.y, position.width, 70);
        
        var buildingsListRect = new Rect(position.x, position.y + 72, (position.width / 3) - 20, position.height);
        var unitsListRect = new Rect(position.x + ((position.width / 3) + 10), position.y + 72, (position.width / 3) - 20, position.height);
        var resourcesListRect = new Rect(position.x + ((position.width / 3 * 2) + 20), position.y + 72, (position.width / 3) - 20, position.height);

        // Draw fields - pass GUIContent.none to each so they are drawn without labels
        EditorGUI.PropertyField(FacTypeRect, property.FindPropertyRelative("FactionType"), GUIContent.none);
        label.text = "Buildings";
        EditorGUI.PropertyField(buildingsListRect, property.FindPropertyRelative("FactionBuildings"), label);
        label.text = "Units";
        EditorGUI.PropertyField(unitsListRect, property.FindPropertyRelative("FactionUnits"), label);
        label.text = "Resources";
        EditorGUI.PropertyField(resourcesListRect, property.FindPropertyRelative("FactionResources"), label);

        EditorGUIUtility.labelWidth = 0;

        // Set indent back to what it was
        EditorGUI.indentLevel = indent;

        EditorGUI.EndProperty();
    }
    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        int totalLine = 4;
        SerializedProperty buildingsSP = property.FindPropertyRelative("FactionBuildings");
        totalLine += 1;
        SerializedProperty unitsSP = property.FindPropertyRelative("FactionUnits");
        totalLine += 1;
        SerializedProperty resourcesSP = property.FindPropertyRelative("FactionResources");
        totalLine += 1;
        int count = buildingsSP.arraySize + 1;
        if (buildingsSP.isExpanded || unitsSP.isExpanded || resourcesSP.isExpanded)
        {
            totalLine += count + 1;
        }

        return EditorGUIUtility.singleLineHeight * totalLine + EditorGUIUtility.standardVerticalSpacing * (totalLine + count);
    }

    

}
[CustomPropertyDrawer(typeof(FactionBuilding))]
public class FactionBuildingPropDrawer : PropertyDrawer
{
    int currentPickerWindow = 0;

    public void ShowPicker()
    {
        //create a window picker control ID
        currentPickerWindow = EditorGUIUtility.GetControlID(FocusType.Passive) + 100;

        //use the ID you just created
        EditorGUIUtility.ShowObjectPicker<FactionEntity>(null, false, "building elf", currentPickerWindow);
        //FindObjectsByType<CanvasRenderer>
    }

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        EditorGUI.BeginProperty(position, label, property);

        // Draw label
        position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), GUIContent.none);

        // Don't make child fields be indented
        var indent = EditorGUI.indentLevel;
        EditorGUI.indentLevel = 0;

        EditorGUIUtility.labelWidth = 0.1f;
        // Calculate rects
        var buildingsRect = new Rect(position.x, position.y, position.width / 2, position.height);
        var initHealthRect = new Rect((position.x + position.width / 2), position.y, position.width / 2, position.height);

        // Draw fields - pass GUIContent.none to each so they are drawn without labels
        EditorGUI.PropertyField(buildingsRect, property.FindPropertyRelative("Building"), GUIContent.none);
        EditorGUI.PropertyField(initHealthRect, property.FindPropertyRelative("StartingHealth"), GUIContent.none);

        Object effectGO = null;

        if (Event.current.commandName == "ObjectSelectorUpdated" && EditorGUIUtility.GetObjectPickerControlID() == currentPickerWindow)
        {
            Debug.Log("Showing picker");
            effectGO = EditorGUIUtility.GetObjectPickerObject();
            currentPickerWindow = -1;

            //name of selected object from picker
            Debug.Log(effectGO.name);
        }
        if (Event.current.commandName == "ObjectSelectorClosed") { currentPickerWindow = -1; }

        // Set indent back to what it was
        EditorGUI.indentLevel = indent;

        EditorGUI.EndProperty();
    }
    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        int totalLine = 1;
        return EditorGUIUtility.singleLineHeight * totalLine + EditorGUIUtility.standardVerticalSpacing * (totalLine);
    }
}
[CustomPropertyDrawer(typeof(FactionUnit))]
public class FactionUnitPropDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        EditorGUI.BeginProperty(position, label, property);

        // Draw label
        position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), GUIContent.none);

        // Don't make child fields be indented
        var indent = EditorGUI.indentLevel;
        EditorGUI.indentLevel = 0;

        EditorGUIUtility.labelWidth = 0.1f;
        // Calculate rects
        var unitsRect = new Rect(position.x, position.y, position.width, position.height);

        // Draw fields - pass GUIContent.none to each so they are drawn without labels
        EditorGUI.PropertyField(unitsRect, property.FindPropertyRelative("Unit"), GUIContent.none);

        // Set indent back to what it was
        EditorGUI.indentLevel = indent;

        EditorGUI.EndProperty();
    }
    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        int totalLine = 1;
        return EditorGUIUtility.singleLineHeight * totalLine + EditorGUIUtility.standardVerticalSpacing * (totalLine);
    }
}
[CustomPropertyDrawer(typeof(FactionResource))]
public class ResourcePropDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        EditorGUI.BeginProperty(position, label, property);

        // Draw label
        position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), GUIContent.none);

        // Don't make child fields be indented
        var indent = EditorGUI.indentLevel;
        EditorGUI.indentLevel = 0;

        EditorGUIUtility.labelWidth = 0.1f;
        // Calculate rects
        var resourcesRect = new Rect(position.x, position.y, position.width, position.height);

        // Draw fields - pass GUIContent.none to each so they are drawn without labels
        EditorGUI.PropertyField(resourcesRect, property.FindPropertyRelative("Resource"), GUIContent.none);

        // Set indent back to what it was
        EditorGUI.indentLevel = indent;

        EditorGUI.EndProperty();
    }
    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        int totalLine = 1;
        return EditorGUIUtility.singleLineHeight * totalLine + EditorGUIUtility.standardVerticalSpacing * (totalLine);
    }
}
