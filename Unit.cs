using UnityEngine;
using System.Collections;

public abstract class Unit : MonoBehaviour, ISelectable {

    int health;
    public abstract string UnitType { get; }

    public string GetName()
    {
        return UnitType;
    }
    public string GetHealth()
    {
        return health.ToString();
    }
}
