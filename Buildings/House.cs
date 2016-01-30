using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public abstract class House : MonoBehaviour, ISelectable {

    public bool isBuild;
    public int health;
    public abstract string HouseType { get; }


    public string GetName()
    {
        return HouseType;
    }
    public string GetHealth()
    {
        return health.ToString();
    }

}
