using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class SelectedPanel : MonoBehaviour {

    public static SelectedPanel Instance { get; protected set; }
    ISelectable selected;
    public GameObject extractivePanel;
    public GameObject storePanel;
    public GameObject processPanel;
    public GameObject storeRightPanel;
    public GameObject processRightPanel;



    // Use this for initialization
    void Start () {
        if (Instance == null) {
            Instance = this;
        }
        else {
            Debug.LogError("Only one instance of SelectedPanel script can be created");
        }

    }

    public void ChangeSelection(ISelectable newOne)
    {
        if (selected != null) selected.DeSelect();
        selected = newOne;
        selected.Select();
        //Debug.Log("Is Selected null? " + ((selected == null).ToString()));
        
        if (selected is ExtractiveHouse)
        {
            RedrawExtractivePanel();
            ((House)selected).someChanges += RedrawExtractivePanel;
        }
        if (selected is ProcessHouse)
        {
            RedrawProcessPanel();
            ((House)selected).someChanges += RedrawProcessPanel;
        }
        else if (selected is Store) {
            RedrawStorePanel();
            ((Store)selected).someChanges += RedrawStorePanel;
        }
        
        
    }
    void RedrawExtractivePanel()
    {
        extractivePanel.SetActive(true);
        ExtractiveHouse h = (ExtractiveHouse) selected;
        extractivePanel.transform.FindChild("LeftPanel/NamePanel/Text").GetComponent<Text>().text = h.transform.name;
        extractivePanel.transform.FindChild("LeftPanel/WorkerPanel/Text").GetComponent<Text>().text = h.GetWorkerName();
        extractivePanel.transform.FindChild("LeftPanel/WorkerPanel/NumberText").GetComponent<Text>().text = h.GetWorkerNumber();
        
        extractivePanel.transform.FindChild("LeftPanel/ResourcePanel/ResourceName").GetComponent<Text>().text = h.GetComponent<ResourceAgent>().myRes.ToString();
        extractivePanel.transform.FindChild("LeftPanel/ResourcePanel/ResourceNumber").GetComponent<Text>().text = h.GetComponent<ResourceAgent>().currCount.ToString();
        //extractivePanel.transform.FindChild("LeftPanel/WorkerPanel/MoreButton").GetComponent<Button>().onClick.AddListener(); 
        storePanel.SetActive(false);
        processPanel.SetActive(false);
    }
    void RedrawStorePanel()
    {
        storePanel.SetActive(true);
        Store s = (Store)selected;
        storePanel.transform.FindChild("LeftPanel/NamePanel/Text").GetComponent<Text>().text = s.transform.name;
        storePanel.transform.FindChild("LeftPanel/WorkerPanel/Text").GetComponent<Text>().text = s.GetWorkerName();
        storePanel.transform.FindChild("LeftPanel/WorkerPanel/NumberText").GetComponent<Text>().text = s.GetWorkerNumber();
        Dictionary<Resource, int> resources = s.GetResources();
        
        foreach (Resource res in System.Enum.GetValues(typeof(Resource))){
            int number = 0;
            Transform resPanel = storeRightPanel.transform.FindChild(res.ToString());
            if (resources.ContainsKey(res))
            {
                number = resources[res];
            }
            resPanel.FindChild("ResourceName").GetComponent<Text>().text = res.ToString();
            resPanel.FindChild("ResourceNumber").GetComponent<Text>().text = number.ToString();
        }
        storePanel.transform.FindChild("LeftPanel/WorkerPanel/MoreButton").GetComponent<Button>().onClick.RemoveAllListeners();
        storePanel.transform.FindChild("LeftPanel/WorkerPanel/MoreButton").GetComponent<Button>().onClick.AddListener(s.CreateSerf);
        extractivePanel.SetActive(false);
        processPanel.SetActive(false);
    }
    void RedrawProcessPanel()
    {
        processPanel.SetActive(true);
        ProcessHouse s = (ProcessHouse)selected;
        processPanel.transform.FindChild("LeftPanel/NamePanel/Text").GetComponent<Text>().text = s.transform.name;
        processPanel.transform.FindChild("LeftPanel/WorkerPanel/Text").GetComponent<Text>().text = s.GetWorkerName();
        processPanel.transform.FindChild("LeftPanel/WorkerPanel/NumberText").GetComponent<Text>().text = s.GetWorkerNumber();
        Dictionary<Resource, int> resources = s.GetResources();

        foreach (Resource res in resources.Keys)
        {
            
            Transform resPanel = processRightPanel.transform.FindChild(res.ToString());
            
            resPanel.FindChild("ResourceName").GetComponent<Text>().text = res.ToString();
            resPanel.FindChild("ResourceNumber").GetComponent<Text>().text = resources[res].ToString();
        }
        //processPanel.transform.FindChild("LeftPanel/WorkerPanel/MoreButton").GetComponent<Button>().onClick.RemoveAllListeners();
        //processPanel.transform.FindChild("LeftPanel/WorkerPanel/MoreButton").GetComponent<Button>().onClick.AddListener(s.CreateSerf);
        extractivePanel.SetActive(false);
        storePanel.SetActive(false);
    }
}
