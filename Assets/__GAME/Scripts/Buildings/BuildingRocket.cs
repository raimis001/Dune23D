using UnityEngine;
using static UnityEditor.ShaderGraph.Internal.KeywordDependentCollection;
using System.Collections;
using UnityEngine.PlayerLoop;




#if UNITY_EDITOR
using UnityEditor;
#endif


public class BuildingRocket : Building
{
    [SerializeField]
    Transform turret;
    [SerializeField]
    Transform[] barels;

    [SerializeField]
    BulletRocket[] rocket;

    [SerializeField]
    float rotationSpeed = 5;

    [SerializeField]
    LineRenderer circleLine;
    [SerializeField]
    LineRenderer distanceLine;

    [SerializeField]
    bool drawGizmos = false;

    Unit current = null;

    float timer = 0;
    float shotDelay1 = 0;
    float shotDelay2 = 0;

    private void Start()
    {
        rocket[0].race = race;
        rocket[1].race = race;
    }

    private void Update()
    {
        if (shotDelay1 > 0)
            shotDelay1 -= Time.deltaTime;
        if (shotDelay2 > 0)
            shotDelay2 -= Time.deltaTime;

        if (current)
        {
            //turret.LookAt(current.transform);
            Vector3 direction = current.transform.position - turret.position; //Vector3.Distance(current.transform.position, turret.position))
            float distance = direction.magnitude;
            distanceLine.SetPosition(1, new Vector3(0, 0, distance));
            circleLine.transform.localPosition = new Vector3(0, 0.5f, distance);// turret.position + turret.forward * -direction.magnitude;
            
            
            DrawCircleLine(GetRadius(distance));
            Quaternion dir = Quaternion.LookRotation(direction, Vector3.up);
            turret.rotation = Quaternion.Lerp(turret.rotation, dir, Time.deltaTime * rotationSpeed);
            if (Quaternion.Angle(turret.rotation, dir) > 0.1f)
                return;


            if (shotDelay1 > -10 && shotDelay1 <= 0)
            {
                shotDelay1 = -10;
                shotDelay2 = Random.Range(0.1f, 0.2f);
                rocket[0].Shot(GetShotPosition(distance));
            }
            if (shotDelay2 > -10 && shotDelay2 <= 0)
            {
                shotDelay2 = -10;
                shotDelay1 = Random.Range(0.9f, 1f);
                rocket[1].Shot(GetShotPosition(distance));
            }

            return;
        }

        timer += Time.deltaTime;
        if (timer > 2)
        {
            timer = 0;
            if (FindClosestObject(10,out current, 1))
            {
                //Debug.Log("Find unit:" + current.name);
            } else
            {
                //Debug.Log("Find nothing");
            }
        }
    }

    Vector3 GetShotPosition(float distance)
    {
        float r = GetRadius(distance);

        Vector2 circle = Random.insideUnitCircle;
        Vector3 random = new Vector3(circle.x, 0, circle.y) * r;

        Vector3 pos = center + turret.forward * distance + random;

        return pos;
    }
    float GetRadius(float distance)
    {
        return 0.5f + Mathf.Pow(distance, 2) * 0.02f; 
    }

    void DrawCircleLine(float radius)
    {
        int numSegments = 20;
        // Izveidojam punktu masīvu
        Vector3[] points = new Vector3[numSegments + 1]; // Pēdējais punkts savieno riņķi
        float angleStep = 360.0f / numSegments;
        float angle = 0f;

        for (int i = 0; i <= numSegments; i++)
        {
            float x = Mathf.Cos(Mathf.Deg2Rad * angle) * radius;
            float z = Mathf.Sin(Mathf.Deg2Rad * angle) * radius;

            points[i] = new Vector3(x, 0, z);

            angle += angleStep;
        }

        // Definējam punktu skaitu LineRenderer
        circleLine.positionCount = points.Length;

        // Iestatām punktus LineRenderer
        circleLine.SetPositions(points);

        // Papildu opcijas (neobligāti): pielāgot krāsas vai platumu
        circleLine.startWidth = 0.05f;
        circleLine.endWidth = 0.05f;
        circleLine.loop = true; // Savieno pirmo un pēdējo punktu
    }


#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        if (!drawGizmos)
            return;

        Gizmos.color = Color.green;
        Gizmos.DrawSphere(center, 0.25f);

        Handles.color = Color.yellow;
        for (int i = 0; i < 10; i++)
        {
            float r = GetRadius(i);
            Handles.DrawWireDisc(center + turret.forward * i,Vector3.up, r);
        }
    }
#endif
}
