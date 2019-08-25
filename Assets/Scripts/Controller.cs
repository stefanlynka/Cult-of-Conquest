using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller : MonoBehaviour{

    public static List<GameObject> players = new List<GameObject>();
    GameObject nodeManager, playerMenu, unitShopManager, ritualManager, randomPanel, altarShopManager, templeShopManager, upgradeManager;


    private void Awake() {
        FindMembers();
    }
    // Start is called before the first frame update
    void Start(){
        CallStartupFunctions();
    }

    // Update is called once per frame
    void Update(){
        
    }

    void FindMembers() {
        nodeManager = GameObject.Find("/Node Manager");
        playerMenu = GameObject.Find("/Players");
        for (int i = 0; i < playerMenu.transform.childCount; i++) {
            GameObject child = playerMenu.transform.GetChild(i).gameObject;
            if (child.GetComponent<Player>()) players.Add(child);
        }
        unitShopManager = GameObject.Find("/Unit Buying Menu/Unit Spaces");
        ritualManager = GameObject.Find("/Ritual Menu");
        randomPanel = GameObject.Find("/Random Panel");
        altarShopManager = GameObject.Find("/Altar Buying Menu");
        templeShopManager = GameObject.Find("/Temple Buying Menu");
        upgradeManager = GameObject.Find("/Upgrade Menu");
    }
    void CallStartupFunctions() {
        nodeManager.GetComponent<NodeManager>().Startup();
        unitShopManager.GetComponent<UnitShopManager>().Startup();
        ritualManager.GetComponent<RitualManager>().Startup();
        for (int i = 0; i < players.Count; i++) {
            GameObject player = players[i];
            player.GetComponent<Player>().Startup();
            for (int j = 0; j < player.transform.childCount; j++) {
                GameObject child = player.transform.GetChild(j).gameObject;
                if (child.GetComponent<Army>()) {
                    child.GetComponent<Army>().Startup();
                }
            }
        }
        unitShopManager.GetComponent<UnitShopManager>().SetupUnitShopSpaces();
        randomPanel.GetComponent<RandomPanel>().Setup();
        altarShopManager.GetComponent<AltarShopManager>().Setup();
        templeShopManager.GetComponent<TempleShopManager>().Setup();
        upgradeManager.GetComponent<UpgradeManager>().Startup();
    }
}
