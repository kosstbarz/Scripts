using UnityEngine;
using System.Collections.Generic;

public class PositionScript : MonoBehaviour {

    public int upLength; // number of tiles up of entrence
    public int leftLength; // number of tiles to the left of entrence
    public int rightLength; // number of tiles to the right of entrence
    Vector2 enterTile;
    Vector2 upTile;


    public Vector2 EnterTile
    {
        get {
            return enterTile;
        }

        set {
            Vector2 diff = value - enterTile;
            enterTile = value;
            upTile = upTile + diff;
        }
    }

    public Vector2 ToUp
    {
        get {
            return upTile - enterTile;   
        }
    }

    public Vector2 ToLeft // Normalized vector
    {
        get
        {
            return new Vector2(-ToUp.y, ToUp.x).normalized;
        }
    }

    // Use this for initialization
    void Start () {
        enterTile = new Vector2(0f, 0f);
        upTile = new Vector2(0f, upLength);
    }
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyUp(KeyCode.T)) // For debug
        {
            Debug.Log("Enter tile " + enterTile.x + ", " + enterTile.y);
            Debug.Log("Up tile " + upTile.x + ", " + upTile.y);
            Debug.Log("Up vector " + ToUp.x + ", " + ToUp.y);
            Debug.Log("Left vector " + ToLeft.x + ", " + ToLeft.y);
            Debug.Log("Left Corner " + LeftCorner().x + ", " + LeftCorner().y);
        }
	}

    public void Rotate()
    {
        //Debug.Log("Enter tile " + enterTile);
        upTile.Set(enterTile.x -ToUp.y, enterTile.y + ToUp.x); // Rotation of upTile around enterTile to 90 degrees
        //Debug.Log("Enter tile " + enterTile);
    }

    public Vector2 LeftCorner()
    {
        Vector2 leftCorner = new Vector2();

        Vector2 lb = enterTile + ToLeft * leftLength;
        Vector2 ru = upTile - ToLeft * rightLength;
        leftCorner.Set(Mathf.Min(lb.x, ru.x), Mathf.Min(lb.y, ru.y));

        return leftCorner;
    }

    public int XLength()
    {
        return Horizontal() ? leftLength + rightLength + 1 : upLength + 1;
    }

    public int YLength()
    {
        return Horizontal() ? upLength + 1 : leftLength + rightLength + 1;
    }

    public bool Horizontal()
    {
        return ToUp.x == 0f;
    }

    public void SynchronizePosition(PositionScript another)
    {
        enterTile = another.enterTile;
        upTile = another.upTile;
    }
    public List<Vector2> GetTiles()
    {
        List<Vector2> result = new List<Vector2>();
        Vector2 leftCorner = LeftCorner();
        for (int i = (int)leftCorner.x; i < leftCorner.x + XLength(); i++)
        {
            for (int j = (int)leftCorner.y; j < leftCorner.y + YLength(); j++)
            {
                result.Add(new Vector2(i, j));
                
            }
        }
        return result;
    }
}
