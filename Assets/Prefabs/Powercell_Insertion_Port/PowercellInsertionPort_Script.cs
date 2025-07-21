using System.Threading;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.UIElements;

public class PowercellInsertionPort_Script : MonoBehaviour, IInteractable
{
    [SerializeField] Light2D[] lights;
    [SerializeField] GameObject[] roomEquipment;
    [SerializeField] TMP_Text interactionPrompt;
    [SerializeField] float powerRequirement = 0.25f;

    [SerializeField] InventoryManager inventoryManager;

    public GameObject powercell;

    private float timer;
    void Start()
    {
        powercell = new GameObject();
        powercell.AddComponent<Powercell_Script>();
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

    public void Interact()
    {
        if (inventoryManager.powercell != null && powercell == null)
        {
            InsertPowerCell();
        }
        else if (inventoryManager.powercell == null && powercell != null)
        {
            RemovePowerCell();
        }
    }

    private void InsertPowerCell()
    { 
        powercell = inventoryManager.powercell;
        inventoryManager.powercell = null;
        Powercell_Script pcs = powercell.GetComponent<Powercell_Script>();

        if ( pcs.powercellCharge > 0)
        {
            pcs.powercellCharge -= 25;
            SwitchPowerOn();
        }
    }

    private void RemovePowerCell()
    {
        inventoryManager.powercell = powercell;
        powercell = null;
        SwitchPowerOff();
    }

    private void SwitchPowerOn()
    {
        foreach (Light2D light in lights)
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
        foreach (Light2D light in lights)
        {
            light.enabled = false;
        }

        foreach (GameObject equipment in roomEquipment)
        {
            equipment.SetActive(false);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        inventoryManager = collision.GetComponent<InventoryManager>();
        interactionPrompt.enabled = true;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        inventoryManager = null;
        interactionPrompt.enabled = false;
    }
}
