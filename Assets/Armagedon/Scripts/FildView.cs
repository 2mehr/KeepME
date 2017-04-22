using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class FildView : MonoBehaviour {
    public float ViewRadius;
    [Range(0,360)]
    public float ViewAngel;
    public LayerMask TaretMask;
    public LayerMask ObstacleMask;
  //  [HideInInspector]
    public List<Transform> visibleTarget = new List<Transform>();

    public float MeshReosolution;
    public int edgeResolution;
    public float edgeDstThreshold;
    public MeshFilter ViewMeshFilter;
    Mesh ViewMesh;
    private void Start()
    {
        ViewMesh = new Mesh();
        ViewMesh.name = ("ViewMesh");
        ViewMeshFilter.mesh = ViewMesh;
        StartCoroutine("FindToTargetWithDelay",2f);
    }

    IEnumerator FindToTargetWithDelay(float delay)
    {
        while (true)
        {
            yield return new WaitForSeconds(delay);
            FindIsTaret();
        }
    }
    private void LateUpdate()
    {
        DrawFieldOfView();
    }
    void FindIsTaret()
    {
        visibleTarget.Clear();
        Collider[] targetsInViewRadius = Physics.OverlapSphere(transform.position, ViewRadius, TaretMask);
        for (int i = 0; i < targetsInViewRadius.Length; i++)
        {
            Transform target = targetsInViewRadius[i].transform;
            Vector3 dirToTarget = (target.position - transform.position).normalized;
            if (Vector3.Angle(transform.forward,dirToTarget)<ViewAngel/2)
            {
                float dstToTarget = Vector3.Distance(transform.position, target.position);
                if (!Physics.Raycast(transform.position, dirToTarget, dstToTarget, ObstacleMask))
                {
                    visibleTarget.Add(target);
                }
            }
        }
    }
    void DrawFieldOfView()
    {
        int setpCount = Mathf.RoundToInt(ViewAngel * MeshReosolution);
        float stepAngelSize = ViewAngel / setpCount;
        List<Vector3> viewPoints = new List<Vector3>();
        ViewCastInfo oldViwCast = new ViewCastInfo();

        for (int i = 0; i <= setpCount; i++)
        {
            float angel = transform.eulerAngles.y - ViewAngel / 2 + stepAngelSize * i;
            ViewCastInfo newViewCast = ViewCast(angel);
            Debug.DrawLine(transform.position, transform.position + DirFromAngel(angel, true) * ViewRadius, Color.red);
            if (i > 0)
            {
                bool edgeDstThreSholdExeeded = Mathf.Abs(oldViwCast.dst - newViewCast.dst) > edgeDstThreshold;
                if (oldViwCast.hit != newViewCast.hit||(oldViwCast.hit&&newViewCast.hit&&edgeDstThreSholdExeeded))
                {
                    EdgeInfo edge = findEdge(oldViwCast,newViewCast);
                    if (edge.pointA!=Vector3.zero)
                    {
                        viewPoints.Add(edge.pointA);

                    }
                    if (edge.pointB!=Vector3.zero)
                    {
                        viewPoints.Add(edge.pointB);
                    }
                }
            }
            oldViwCast = newViewCast;
            viewPoints.Add(newViewCast.point);
        }
        int vertextCount = viewPoints.Count + 1;
        Vector3[] vertices = new Vector3[vertextCount];
        int[] triangel = new int[(vertextCount - 2)*3];
        vertices[0] = Vector3.zero;
        for (int i = 0; i < vertextCount-1; i++)
        {
            if (i<vertextCount-2)
            {
                vertices[i + 1] =transform.InverseTransformPoint( viewPoints[i]);
                triangel[i * 3] = 0;
                triangel[i * 3 + 1] = i + 1;
                triangel[i * 3 + 2] = i + 2;
            }
          

        }
        ViewMesh.Clear();
        ViewMesh.vertices = vertices;
        ViewMesh.triangles = triangel;
        ViewMesh.RecalculateNormals();
    }
    EdgeInfo findEdge(ViewCastInfo minViwCast , ViewCastInfo maxViewCast)
    {
        float minAngel = minViwCast.angel;
        float maxAngel = maxViewCast.angel;
        Vector3 minPoint = Vector3.zero;
        Vector3 maxPoint = Vector3.zero;
        for (int i = 0; i < edgeResolution; i++)
        {
            float angel = (minAngel + maxAngel) / 2;
            ViewCastInfo newviewCast = ViewCast(angel);
            bool edgeDstThreSholdExeeded = Mathf.Abs(minViwCast.dst - newviewCast.dst) > edgeDstThreshold;

            if (newviewCast.hit == minViwCast.hit&&!edgeDstThreSholdExeeded)
            {
                minAngel = angel;
                minPoint = newviewCast.point;
            }
            else
            {
                maxAngel = angel;
                maxPoint = newviewCast.point;
            }

        }
        return new EdgeInfo(minPoint, maxPoint);
    }
    ViewCastInfo ViewCast(float globalAngel)
    {
        Vector3 dir =DirFromAngel(globalAngel, true);
        RaycastHit hit;
        if (Physics.Raycast(transform.position,dir,out hit,ViewRadius,ObstacleMask))
        {
            return new ViewCastInfo(true, hit.point, hit.distance, globalAngel);
        }
        else
        {
            return new ViewCastInfo(false, transform.position+dir*ViewRadius, ViewRadius, globalAngel);
        }
    }

    public Vector3 DirFromAngel(float angelDegrees , bool anelIsGlobal )
    {
        if (!anelIsGlobal)
        {
            angelDegrees += transform.eulerAngles.y;
        }
        return new Vector3(Mathf.Sin(angelDegrees * Mathf.Deg2Rad), 0, Mathf.Cos(angelDegrees * Mathf.Deg2Rad));
    }
	public struct ViewCastInfo
    {
        public bool hit;
        public Vector3 point;
        public float dst;
        public float angel;

        public ViewCastInfo(bool _hit , Vector3 _point , float _dts,float _angel)
        {
            hit = _hit;
            point = _point;
            dst = _dts;
            angel = _angel;
        }
    }
    public struct EdgeInfo
    {
        public Vector3 pointB;
        public Vector3 pointA;

        public EdgeInfo(Vector3 _pointA,Vector3 _pointB)
        {
            pointA = _pointA;
            pointB = _pointB;
        }
    }
}
