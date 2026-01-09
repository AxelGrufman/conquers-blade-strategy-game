//using UnityEngine;

//[ExecuteAlways]
//public class FormationGizmoDrawer : MonoBehaviour
//{
//    [Header("References")]
//    public OrderGiver orderGiver;

//    [Header("Gizmo Colors")]
//    public Color centerColor = Color.yellow;
//    public Color slotColor = Color.cyan;
//    public Color boxColor = Color.white;
//    public Color arrowColor = Color.red;
//    public Color linkColor = Color.green;

//    [Header("Settings")]
//    public float centerSize = 0.25f;
//    public float slotSize = 0.15f;
//    public bool showLinks = false;
//    public bool showBoundingBox = true;
//    public bool showDirectionArrow = true;

//    private void OnDrawGizmos()
//    {
//        if (!orderGiver || orderGiver.Units == null || orderGiver.Units.Count == 0)
//            return;

//        Vector3 center = (orderGiver.CurrentOrder == OrderGiver.Orders.FollowInFormation)
//            ? orderGiver.Player.transform.position
//            : orderGiver.GetSquadCenter();

//        Gizmos.color = centerColor;
//        Gizmos.DrawSphere(center, centerSize);

//        Quaternion rot = Quaternion.LookRotation(orderGiver.Player.transform.forward);

//        Vector3[] slots = GetSlots(center, rot);

//        if (slots == null || slots.Length == 0)
//            return;

//        for (int i = 0;   i < slots.Length; i++)
//        {
//            Gizmos.color = slotColor;
//            Gizmos.DrawSphere(slots[i], slotSize);

//            if (showLinks)
//            {
//                Gizmos.color = linkColor;
//                Gizmos.DrawLine(center, slots[i]);
//            }
//        }

//        if (showBoundingBox)
//            DrawBoundingBox(slots);

 
//        if (showDirectionArrow)
//            DrawDirectionArrow(center);

   
//    }



//    private Vector3[] GetSlots(Vector3 center, Quaternion rot)
//    {
//        switch (orderGiver.CurrentFormation)
//        {
//            case OrderGiver.FormationTypes.Line:
//                return FormationGenerator.GetLineFormation(
//                    orderGiver.Units.Count,
//                    orderGiver.LineWidth,
//                    center,
//                    rot,
//                    orderGiver.IsLooseFormation
//                );

//            case OrderGiver.FormationTypes.Column:
//                return FormationGenerator.GetColumnFormation(
//                    orderGiver.Units.Count,
//                    orderGiver.ColumnWidth,
//                    center,
//                    orderGiver.IsLooseFormation,
//                    rot
//                );

//            case OrderGiver.FormationTypes.square:
//                return FormationGenerator.GetSquareFormation(
//                    orderGiver.Units.Count,
//                    center,
//                    orderGiver.IsLooseFormation,
//                    rot
//                );
//        }
//        return null;
//    }

//    private void DrawBoundingBox(Vector3[] slots)
//    {
//        float minX = float.MaxValue;
//        float minZ = float.MaxValue;
//        float maxX = float.MinValue;
//        float maxZ = float.MinValue;

//        foreach (Vector3 s in slots)
//        {
//            if (s.x < minX) minX = s.x;
//            if (s.z < minZ) minZ = s.z;
//            if (s.x > maxX) maxX = s.x;
//            if (s.z > maxZ) maxZ = s.z;
//        }

//        Vector3 p1 = new Vector3(minX, slots[0].y, minZ);
//        Vector3 p2 = new Vector3(maxX, slots[0].y, minZ);
//        Vector3 p3 = new Vector3(maxX, slots[0].y, maxZ);
//        Vector3 p4 = new Vector3(minX, slots[0].y, maxZ);

//        Gizmos.color = boxColor;
//        Gizmos.DrawLine(p1, p2);
//        Gizmos.DrawLine(p2, p3);
//        Gizmos.DrawLine(p3, p4);
//        Gizmos.DrawLine(p4, p1);
//    }


//    private void DrawDirectionArrow(Vector3 center)
//    {
//        if (!orderGiver.Player)
//            return;

//        Vector3 forward = orderGiver.Player.transform.forward;
//        Vector3 arrowEnd = center + forward * 2f;

//        Gizmos.color = arrowColor;
//        Gizmos.DrawLine(center, arrowEnd);

//        Vector3 right = Quaternion.Euler(0, 135f, 0) * forward;
//        Vector3 left = Quaternion.Euler(0, -135f, 0) * forward;

//        Gizmos.DrawLine(arrowEnd, arrowEnd + right * 0.5f);
//        Gizmos.DrawLine(arrowEnd, arrowEnd + left * 0.5f);
//    }
   
//}
