using UnityEngine;

public static class FormationGenerator
{
    public static float spacing = 1.5f;

    public static Vector3[] GetLineFormation(int count, int unitsPerRow, Vector3 center, Quaternion rotation)
    {
        Vector3[] slots = new Vector3[count];

        for (int i = 0; i < count; i++)
        {
            int row = i / unitsPerRow;
            int col = i % unitsPerRow;

            // center columns around zero
            float xOffset = (col - (unitsPerRow - 1) / 2f) * spacing;
            float zOffset = row * spacing;

            Vector3 localPos = new Vector3(xOffset, 0, zOffset);
            slots[i] = center + rotation * localPos;
        }

        return slots;
    }

    public static Vector3[] GetColumnFormation(
     int count, int height, Vector3 center, Quaternion rot)
    {
        Vector3[] slots = new Vector3[count];

        int width = Mathf.CeilToInt((float)count / height);

        for (int i = 0; i < count; i++)
        {
            int col = i / height;
            int row = i % height;

            float x = (col - (width - 1) / 2f) * spacing;
            float z = (row - (height - 1) / 2f) * spacing;  

            slots[i] = center + rot * new Vector3(x, 0, z);
        }

        return slots;
    }

    public static Vector3[] GetSquareFormation(int count, Vector3 center, Quaternion rot)
    {
        int side = Mathf.CeilToInt(Mathf.Sqrt(count));

        Vector3[] slots = new Vector3[count];
        int idx = 0;

        for (int r = 0; r < side; r++)
        {
            for (int c = 0; c < side; c++)
            {
                if (idx >= count)
                    return slots;

                float x = (c - (side - 1) / 2f) * spacing;
                float z = (r - (side - 1) / 2f) * spacing;   

                slots[idx++] = center + rot * new Vector3(x, 0, z);
            }
        }

        return slots;
    }

}
