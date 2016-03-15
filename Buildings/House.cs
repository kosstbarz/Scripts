using UnityEngine;
using System;
using System.Collections.Generic;


public abstract class House : MonoBehaviour, ISelectable {

    public bool isBuild;
    public Action someChanges;
    protected bool selected;
    public void Select()
    {
        selected = true;
    }
    public bool IsSelected()
    {
        return selected;
    } 
    public void DeSelect()
    {
        selected = false;
        someChanges = null;
    }
    public abstract void TimeLap(float time);
    public abstract void OnBuild();
    public abstract Dictionary<Resource, int> GetResources();
}
