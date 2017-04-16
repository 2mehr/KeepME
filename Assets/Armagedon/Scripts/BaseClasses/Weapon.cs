using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon :Arms {
    public int BulletCount;
    public float FireRate;
    public float Accuracy;
    float TempFireRate;
    public Bullet Bull;


   
    public void Fire()
    {
      
        if (TempFireRate <= 0 && (BulletCount > 0||BulletCount==-1))
        {
            Bullet bull = Instantiate<Bullet>(Bull, transform.position, transform.rotation);
            bull.Damage = Damage;
            bull.CType = CType;
            bull.ExplosionRange = ExplosionRange;
            bull.Range = Range;
            bull.Speed = Speed;
            if(BulletCount!=-1)
            BulletCount--;
            TempFireRate = FireRate;
            print("Fire");
        }
        else
        {
            TempFireRate -= Time.deltaTime;
        }
    }
    // Use this for initialization
	void Start () {
        TempFireRate = FireRate;

	}



}
