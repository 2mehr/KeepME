using UnityEngine;

public class Sight : MonoBehaviour {

    public static bool Ditect = false;
    public static bool AttackMode;
    NPC npc;
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag=="Enemy")
        {
            Ditect = true;
        }
        if (other.tag== "Bullet")
        {
            AttackMode = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Enemy")
        {
            Ditect = false;
        }
    }
    private void Start()
    {
        npc = GameObject.Find("Npc").GetComponent<NPC>();
        GetComponent<SphereCollider>().radius = npc.SightRange;
    }
}
