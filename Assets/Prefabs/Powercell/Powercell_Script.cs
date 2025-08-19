using UnityEngine;

public class Powercell_Script : MonoBehaviour, IInteractable
{
    [SerializeField] public float powercellCharge = 100;
    [SerializeField] private float powercellMaxCharge = 100;

    [SerializeField] private SpriteRenderer _25percentChargeLed;
    [SerializeField] private SpriteRenderer _50percentChargeLed;
    [SerializeField] private SpriteRenderer _75percentChargeLed;
    [SerializeField] private SpriteRenderer _100percentChargeLed;

    [SerializeField] public SpriteRenderer sprite;

    private void Start()
    {
        UpdateChargeLights();
    }

    public void ChargePowerCell(float chargeRate)
    {
        if (powercellCharge == powercellMaxCharge) return;

        if (powercellCharge + chargeRate >= powercellMaxCharge)
        {
            powercellCharge = powercellMaxCharge;
        } 
        else
        {
            powercellCharge += chargeRate;
        }

        UpdateChargeLights();
    }

    public float DischargePowercell(float powerRequirement)
    {
        if (powercellCharge <= 0) return 0;

        if (powercellCharge - powerRequirement < 0) 
        { 
            powercellCharge = 0;
        } 
        else
        {
            powercellCharge -= powerRequirement;
        }

        UpdateChargeLights();
        return powerRequirement;
    }

    public void UpdateChargeLights()
    {
        float value = powercellCharge / powercellMaxCharge;

        _25percentChargeLed.color = Color.gray;
        _50percentChargeLed.color = Color.gray;
        _75percentChargeLed.color = Color.gray;
        _100percentChargeLed.color = Color.gray;

        if (value >= 0.02) _25percentChargeLed.color = Color.red;

        if (value >= 0.25) _25percentChargeLed.color = Color.green;

        if (value >= 0.5) _50percentChargeLed.color = Color.green;

        if (value >= 0.75) _75percentChargeLed.color = Color.green;

        if (value >= 1) _100percentChargeLed.color = Color.green;
    }

    public void Interact(GameObject player)
    {
        player.GetComponent<InventoryManager>().powercell = gameObject;
        transform.position = Vector3.zero;
        gameObject.transform.SetParent(player.transform, false);
        sprite.enabled = false;
        GetComponent<BoxCollider2D>().enabled = false;
    }
}
