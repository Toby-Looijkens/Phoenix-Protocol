using System.Threading;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class PowercellInsertionPort_Script : MonoBehaviour
{
    [SerializeField] Light2D[] lights;
    [SerializeField] GameObject[] roomEquipment;
    [SerializeField] float powerRequirement = 0.25f;

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
        PowerRoom();
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

    private void SwitchPowerOn()
    {
        Debug.Log("Test");
        foreach (Light2D light in lights)
        {
            light.enabled = true;
        }

        foreach (GameObject equipment in roomEquipment)
        {

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

        }
    }
}
