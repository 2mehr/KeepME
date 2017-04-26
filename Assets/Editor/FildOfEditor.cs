using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(FildView))]
public class FildOfEditor : Editor {

    private void OnSceneGUI()
    {
        FildView fow =(FildView) target;
        Handles.color = Color.white;
        Handles.DrawWireArc(fow.transform.position, Vector3.up, Vector3.forward, 360, fow.ViewRadius);
        Vector3 viewAngelA = fow.DirFromAngel(-fow.ViewAngel / 2, false);
        Vector3 viewAngelB = fow.DirFromAngel(fow.ViewAngel / 2, false);
        Handles.DrawLine(fow.transform.position, fow.transform.position + viewAngelA * fow.ViewRadius);
        Handles.DrawLine(fow.transform.position, fow.transform.position + viewAngelB * fow.ViewRadius);
        Handles.color = Color.red;
        foreach (CharBase item in fow.visibleTarget)
        {
            Handles.DrawLine(fow.transform.position, item.transform.position);
        }
    }

}
