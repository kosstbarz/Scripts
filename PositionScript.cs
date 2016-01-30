using UnityEngine;
using System.Collections;

public class PositionScript : MonoBehaviour {

    public int upLength; // number of tiles up of entrence
    public int leftLength; // number of tiles to the left of entrence
    public int rightLength; // number of tiles to the right of entrence
    Vector2 enterTile;
    Vector2 upTile;
    Vector2 toUp;
    Vector2 toLeft;

    public Vector2 EnterTile
    {
        get {
            return enterTile;
        }

        set {
            enterTile = value;
            upTile = enterTile + toUp;
        }
    }


    // Use this for initialization
    void Start () {
        enterTile = new Vector2(0f, 0f);
        upTile = new Vector2(0f, upLength);
        toUp = upTile - enterTile;
        toLeft = new Vector2(-toUp.y, toUp.x).normalized;
    }
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyUp(KeyCode.T)) // For debug
        {
            Debug.Log("Enter tile " + enterTile.x + ", " + enterTile.y);
            Debug.Log("Up tile " + upTile.x + ", " + upTile.y);
            Debug.Log("Up vector " + toUp.x + ", " + toUp.y);
            Debug.Log("Left vector " + toLeft.x + ", " + toLeft.y);
            Debug.Log("Left Corner " + LeftCorner().x + ", " + LeftCorner().y);
        }
	}

    public void Rotate()
    {
        Debug.Log("Enter tile " + enterTile);
        upTile.Set(enterTile.x -toUp.y, enterTile.y + toUp.x); // Rotation of upTile around enterTile to 90 degrees
        toUp = upTile - enterTile;
        toLeft.Set(-toUp.y, toUp.x);
        toLeft = toLeft.normalized;
        Debug.Log("Enter tile " + enterTile);
    }

    public Vector2 LeftCorner()
    {
        Vector2 leftCorner = new Vector2();

        Vector2 lb = enterTile + toLeft * leftLength;
        Vector2 ru = upTile - toLeft * rightLength;
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
        return toUp.x == 0f;
    }

    public void SynchronizePosition(PositionScript another)
    {
        enterTile = another.enterTile;
        upTile = another.upTile;
        toUp = another.toUp;
        toLeft = another.toLeft;
    }
}
