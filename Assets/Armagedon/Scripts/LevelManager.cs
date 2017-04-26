using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour {
    public NPC Enemey;
    public  List<CharBase> Enemeies = new List<CharBase>();
    public List<Transform> SpawnPos;
    public static LevelManager Manager;
	// Use this for initialization
	void Start () {
        Manager = this;
        
      //  Instantiate<GameObject>(p);
        foreach (var item in SpawnPos)
        {
            CharBase inEnemy = Instantiate<NPC>(Enemey, item.position, item.rotation);
           
           
                Enemeies.Add(inEnemy);
            inEnemy.OnDeath += InEnemy_OnDeath;
           
        }

    }

    private void InEnemy_OnDeath(CharBase e , CharBase killer)
    {
       
        Enemeies.Remove(e);
        try
        {
            ((NPC)killer).ChooseTarget();
            print(killer.name+"Target");
        }
        catch
        {
           
        }
        
    }




    // Update is called once per frame
    void Update () {
       
	}

}
