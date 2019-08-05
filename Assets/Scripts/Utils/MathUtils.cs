using UnityEngine;
using System.Collections;

//
public class MathUtils{

	//XZ
    public static Vector3 RadomCirclePoint(Vector3 center, float radius)
    {
        //Y
        Quaternion rotation = Quaternion.Euler(0f, (int)Random.Range(0, 36) * 10, 0f);
        Vector3 newPos = center + rotation * new Vector3(0, 0f, radius);
        //ClientLogger.Info(" RadomCirclePoint : pos = " + newPos);
        return newPos;
    }

    //XZ
    public static Vector3 RadomInsideCirclePoint(Vector3 center, float radius)
    {
        Vector2 radompos = Random.insideUnitCircle * radius;
        Vector3 pos = center + new Vector3(radompos.x, 0 , radompos.y);
        //ClientLogger.Info(" RadomInsideCirclePoint : pos = " + pos);
        return pos;
    }

    public static Vector3 RadomOnSpherePoint(Vector3 center, float radius)
    {
        Vector3 radompos = Random.insideUnitSphere * radius;
        Vector3 pos = center + radompos;
        return pos;
    }

    public static bool Rand(float prop)
    {
        if (UnityEngine.Random.Range(0.0f, 100.0f) >= (100.0f - prop))
            return true;
        return false;
    }

    //x-z
    public static float DistanceIgnoreY(Vector3 pos1, Vector3 pos2)
    {
        pos1.y = pos2.y = 0;
        return Vector3.Distance(pos1, pos2);
    }
}
