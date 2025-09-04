using System.Threading;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.UIElements;

public class PowerCellPort_Script : MonoBehaviour, IInteractable
{
    [SerializeField] Light[] lights;
    [SerializeField] GameObject[] roomEquipment;
    [SerializeField] float powerRequirement = 0.25f;

    public GameObject powercell;

    private float timer;
    void Start()
    {        
        if (powercell == null)
        {
            foreach (Light light in lights)
            {
                light.enabled = false;
            }

            foreach (GameObject equipment in roomEquipment)
            {
                equipment.GetComponent<IEquipment>().SwitchOff();
            }
        }

    }

    // Update is called once per frame
    void Update()
    {
    }

    private void PowerRoom()
    {
        if (powercell == null) 
        {
            SwitchPowerOff();
            return;
        }

        timer += Time.deltaTime;

        if (timer < 1) return;

        if (powercell.GetComponent<Powercell_Script>().DischargePowercell(powerRequirement) == 0)
        {
            SwitchPowerOff();
        } 
        else
        {
            SwitchPowerOn();
        }

        timer = 0;
    }

    public void Interact(GameObject player)
    {
        InventoryManager inventoryManager = player.GetComponent<InventoryManager>();
        if (inventoryManager.powercell != null && powercell == null)
        {
            InsertPowerCell(inventoryManager);
        }
        else if (inventoryManager.powercell == null && powercell != null)
        {
            RemovePowerCell(inventoryManager);
        }
    }

    private void InsertPowerCell(InventoryManager inventoryManager)
    { 
        powercell = inventoryManager.powercell;
        inventoryManager.powercell = null;
        Powercell_Script pcs = powercell.GetComponent<Powercell_Script>();

        if ( pcs.powercellCharge >= 25)
        {
            powercell.transform.SetParent(gameObject.transform, false);
            pcs.DischargePowercell(25);
            pcs.sprite.enabled = true;
            SwitchPowerOn();
        }
    }

    private void RemovePowerCell(InventoryManager inventoryManager)
    {
        inventoryManager.powercell = powercell;
        powercell.transform.SetParent(inventoryManager.gameObject.transform, false);
        powercell = null;
        SwitchPowerOff();
    }

    private void SwitchPowerOn()
    {
        foreach (Light light in lights)
        {
            light.enabled = true;
        }

        foreach (GameObject equipment in roomEquipment)
        {
            equipment.SetActive(true);
        }
    }

    private void SwitchPowerOff()
    {
        foreach (Light light in lights)
        {
            light.enabled = false;
        }

        foreach (GameObject equipment in roomEquipment)
        {
            equipment.SetActive(false);
        }
    }
}
