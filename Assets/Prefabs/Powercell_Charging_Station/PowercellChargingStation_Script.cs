using UnityEngine;

public class PowercellChargingStation_Script : MonoBehaviour
{
    [SerializeField] private float chargeRate = 0.02f;

    [SerializeField] private GameObject[] ports = new GameObject[4];

    private int portsOccupied = 0;

    private float timer;

    private GameObject player;

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
            if (port != null)
                port.GetComponent<Powercell_Script>().ChargePowerCell(chargeRate);
        }

        timer = 0;
    }

    private void InsertPowercell(GameObject powercell)
    {
        for (int i = 0; i < ports.Length; i++) 
        { 
            if (ports[i] == null)
            {
                ports[i] = powercell;
                break;
            }
        }
        powercell = null;
        portsOccupied++;
    }

    private void RemovePowercell(GameObject powercell)
    {
        GameObject highestChargeBattery;
        highestChargeBattery = ports[0];
        for (int i = 1; i < ports.Length; i++)
        {
            if (ports[i] == null)
            {
                highestChargeBattery = ports[i];
            } 
            else if (ports[i].GetComponent<Powercell_Script>().powercellCharge > highestChargeBattery.GetComponent<Powercell_Script>().powercellCharge)
            {
                highestChargeBattery = ports[i];
            }
        }
        powercell = highestChargeBattery;
        portsOccupied--;
    }

    private void OnInteract()
    {
        if (player == null) 
            return;

        if (portsOccupied != 0 && player.GetComponent<InventoryManager>().powercell == null) 
            RemovePowercell(player.GetComponent<InventoryManager>().powercell);

        if (portsOccupied != ports.Length && player.GetComponent<InventoryManager>().powercell == null)
            InsertPowercell(player.GetComponent<InventoryManager>().powercell);
    } 

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
            player = collision.gameObject;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            player = null;
        }
    }
}
