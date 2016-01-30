using UnityEngine;
using System.Collections.Generic;
using System;

public class School : House {

    public int numberOfBooks;
    public Queue<string> schoolQueue;
    int progress;

    public override string HouseType
    {
        get
        {
           return "School";
        }
    }

    
}
