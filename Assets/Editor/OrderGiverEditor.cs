using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(OrderGiver))]
public class RotatePlanetEditor : Editor
{
    public override void OnInspectorGUI()
    {
        OrderGiver script = (OrderGiver)target;

        DrawDefaultInspector();

        GUILayout.Space(10);
        GUILayout.Label("Give orders", EditorStyles.boldLabel);

        EditorGUI.BeginChangeCheck();

        GUILayout.BeginHorizontal();
        if (GUILayout.Toggle(script.CurrentOrder == OrderGiver.Orders.Hold, "Hold", "Button"))
        {
            script.CurrentOrder = OrderGiver.Orders.Hold;
        }
        if (GUILayout.Toggle(script.CurrentOrder == OrderGiver.Orders.MoveInFormationToTarget, "Move In Formation To Target", "Button"))
        {
            script.CurrentOrder = OrderGiver.Orders.MoveInFormationToTarget;
        }
        if (GUILayout.Toggle(script.CurrentOrder == OrderGiver.Orders.FollowInFormation, "Follow In Formation", "Button"))
        {
            script.CurrentOrder = OrderGiver.Orders.FollowInFormation;
        }
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        if (GUILayout.Toggle(script.CurrentOrder == OrderGiver.Orders.Charge, "Charge", "Button"))
        {
            script.CurrentOrder = OrderGiver.Orders.Charge;
        }
        if (GUILayout.Toggle(script.CurrentOrder == OrderGiver.Orders.MoveToTarget, "Move To Target", "Button"))
        {
            script.CurrentOrder = OrderGiver.Orders.MoveToTarget;
        }
        if (GUILayout.Toggle(script.CurrentOrder == OrderGiver.Orders.AttackAtWill, "Attack At Will", "Button"))
        {
            script.CurrentOrder = OrderGiver.Orders.AttackAtWill;
        }
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        if (GUILayout.Toggle(script.CurrentOrder == OrderGiver.Orders.GuardArea, "Guard Area", "Button"))
        {
            script.CurrentOrder = OrderGiver.Orders.GuardArea;
        }
        if (GUILayout.Toggle(script.CurrentOrder == OrderGiver.Orders.none, "Coming soon", "Button"))
        {
            script.CurrentOrder = OrderGiver.Orders.none;
        }
        if (GUILayout.Toggle(script.CurrentOrder == OrderGiver.Orders.none, "Coming soon", "Button"))
        {
            script.CurrentOrder = OrderGiver.Orders.none;
        }
        GUILayout.EndHorizontal();

        GUILayout.Label("Formation", EditorStyles.boldLabel);

        GUILayout.BeginHorizontal();
        if (GUILayout.Toggle(script.CurrentFormation == OrderGiver.FormationTypes.Line, "Line", "Button"))
        {
            script.CurrentFormation = OrderGiver.FormationTypes.Line;
        }
        if (GUILayout.Toggle(script.CurrentFormation == OrderGiver.FormationTypes.Column, "Column", "Button"))
        {
            script.CurrentFormation = OrderGiver.FormationTypes.Column;
        }
        if (GUILayout.Toggle(script.CurrentFormation == OrderGiver.FormationTypes.square, "square", "Button"))
        {
            script.CurrentFormation = OrderGiver.FormationTypes.square;
        }
        GUILayout.EndHorizontal();

        if (EditorGUI.EndChangeCheck())
        {
            EditorUtility.SetDirty(script); // Ensures edit-mode changes are saved
        }
    }
}
