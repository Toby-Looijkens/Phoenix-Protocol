using Unity.VisualScripting;
using UnityEngine;

public class PowercellChargingStation_Script : MonoBehaviour, IInteractable
{
    [SerializeField] private float chargeRate = 0.02f;

    [SerializeField] private GameObject[] ports = new GameObject[3];

    private int portsOccupied = 0;

    private float timer;

    private void Update()
    {
        ChargePowercells();
    }

    private void ChargePowercells()
    {
        if (timer < 1)
        {
            timer += Time.deltaTime;
            return;
        }

        foreach (GameObject port in ports)
        {
            if (port.transform.childCount > 0)
            {
                port.GetComponentInChildren<Powercell_Script>().ChargePowerCell(chargeRate);
            }
        }

        timer = 0;
    }

    private void InsertPowerCell(InventoryManager inventoryManager)
    {
        GameObject powercell = inventoryManager.powercell;

        for (int i = 0; i < ports.Length; i++) 
        { 
            if (ports[i].transform.childCount == 0)
            {
                powercell.transform.SetParent(ports[i].transform, false);
                powercell.GetComponent<Powercell_Script>().sprite.enabled = true;
                break;
            }
        }
        inventoryManager.powercell = null;
        portsOccupied++;
    }

    private void RemovePowerCell(InventoryManager inventoryManager)
    {
        GameObject highestChargeBattery = null;
        for (int i = 0; i < ports.Length; i++)
        {
            if (highestChargeBattery == null && ports[i].transform.childCount > 0) 
            {
                highestChargeBattery = ports[i].GetComponentInChildren<Powercell_Script>().gameObject;
                continue;
            }

            if (!highestChargeBattery && ports[i].GetComponentInChildren<Powercell_Script>().powercellCharge > highestChargeBattery.GetComponent<Powercell_Script>().powercellCharge)
            {
                highestChargeBattery = ports[i].GetComponentInChildren<Powercell_Script>().gameObject;
                ports[i] = null;
            }
        }
        inventoryManager.powercell = highestChargeBattery;
        highestChargeBattery.transform.SetParent(inventoryManager.gameObject.transform, false);
        portsOccupied--;
    }

    public void Interact(GameObject player)
    {
        InventoryManager inventoryManager = player.GetComponent<InventoryManager>();
        if (inventoryManager.powercell != null && portsOccupied < ports.Length )
        {
            InsertPowerCell(inventoryManager);
        }
        else if (inventoryManager.powercell == null && portsOccupied > 0)
        {
            RemovePowerCell(inventoryManager);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {

    }

    private void OnTriggerExit2D(Collider2D collision)
    {

    }

}
