using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Selecting : MonoBehaviour {

    public static Selecting Instance { get; protected set; }

    public GameObject infoPanel;
    public Text nameText;
    public Text healthText;

    ISelectable selected;

    // Use this for initialization
    void Start () {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Debug.LogError("Only one instance of Selecting script can be created");
        }
    }
	
	public void ChangeSelection(ISelectable newOne)
    {
        selected = newOne;
        nameText.text = selected.GetName();
        healthText.text = selected.GetHealth();
        infoPanel.transform.position = Input.mousePosition;
        infoPanel.SetActive(true);
    }

    public void CloseInfoPanel()
    {
        infoPanel.SetActive(false);
    }
}
