using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public interface Ishooter  {
    public int CurtentVeponID;
    public int rt;
    public List<Weapen> Wepons;

   
    public enum CType
    {
        Player,
        Enamey,
        Frends
        
    };
    public Transform AtackPont;

    public bool IsMelee;

	
   public void ChooseWepons()
    {

    }
    public void Shoot()
    {

    }
}
