using System;
using DiceGuardiansClient.Source.Entities;
using Vector2 = Microsoft.Xna.Framework.Vector2;
using Vector3 = Microsoft.Xna.Framework.Vector3;

namespace DiceGuardiansClient.Source.Collisions; 

public static class BoardCollisions {
    public static Vector2 GetTileCoords(Camera cam, Vector3 mouseRay) {
        Vector3 start = cam.GetPosition();
        float d = -start.Y / mouseRay.Y;
        float x = start.X + mouseRay.X * d;
        float z = start.Z + mouseRay.Z * d;
        return new Vector2((float) Math.Round(x), (float) Math.Round(z));
    }

    public static bool RayVsAABB(Camera cam, Vector3 mouseRay, AABB box) {
        Vector3 start = cam.GetPosition();
        Vector3 min = box.GetPosition();
        Vector3 max = Vector3.Add(box.GetPosition(), box.GetSize());

        float d, x, y, z;
        
        // intersection with first face along X
        d = (min.X - start.X) / mouseRay.X;
        y = start.Y + mouseRay.Y * d;
        z = start.Z + mouseRay.Z * d;
        if (min.Y <= y && y <= max.Y && min.Z <= z && z <= max.Z) {return true;}
        
        // intersection with second face along X
        d = (max.X - start.X) / mouseRay.X;
        y = start.Y + mouseRay.Y * d;
        z = start.Z + mouseRay.Z * d;
        if (min.Y <= y && y <= max.Y && min.Z <= z && z <= max.Z) {return true;}
        
        // intersection with first face along Y
        d = (min.Y - start.Y) / mouseRay.Y;
        x = start.X + mouseRay.X * d;
        z = start.Z + mouseRay.Z * d;
        if (min.X <= x && x <= max.X && min.Z <= z && z <= max.Z) {return true;}
        
        // intersection with second face along Y
        d = (max.Y - start.Y) / mouseRay.Y;
        x = start.X + mouseRay.X * d;
        z = start.Z + mouseRay.Z * d;
        if (min.X <= x && x <= max.X && min.Z <= z && z <= max.Z) {return true;}
        
        // intersection with first face along Z
        d = (min.Z - start.Z) / mouseRay.Z;
        x = start.X + mouseRay.X * d;
        y = start.Y + mouseRay.Y * d;
        if (min.X <= x && x <= max.X && min.Y <= y && y <= max.Y) {return true;}
        
        // intersection with second face along Z
        d = (max.Z - start.Z) / mouseRay.Z;
        x = start.X + mouseRay.X * d;
        y = start.Y + mouseRay.Y * d;
        if (min.X <= x && x <= max.X && min.Y <= y && y <= max.Y) {return true;}

        return false;
    }
}