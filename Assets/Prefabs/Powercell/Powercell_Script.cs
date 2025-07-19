using UnityEngine;

public class Powercell_Script : MonoBehaviour
{
    [SerializeField] public float powercellCharge = 100;
    [SerializeField] private float powercellMaxCharge = 100;

    [SerializeField] private SpriteRenderer _20percentChargeLed;
    [SerializeField] private SpriteRenderer _45percentChargeLed;
    [SerializeField] private SpriteRenderer _70percentChargeLed;
    [SerializeField] private SpriteRenderer _95percentChargeLed;

    private void Start()
    {

    }

    public void ChargePowerCell(float chargeRate)
    {
        if (powercellCharge == powercellMaxCharge) return;
        //UpdateChargeLights();

        if (powercellCharge + chargeRate >= powercellMaxCharge)
        {
            powercellCharge = powercellMaxCharge;
        } 
        else
        {
            powercellCharge += chargeRate;
        }
    }

    public float DischargePowercell(float powerRequirement)
    {
        if (powercellCharge <= 0) return 0;
        //UpdateChargeLights();

        if (powercellCharge - powerRequirement < 0) 
        { 
            powercellCharge = 0;
            return powerRequirement;
        } 
        else
        {
            powercellCharge -= powerRequirement;
            return powerRequirement;
        }
    }

    public void UpdateChargeLights()
    {
        float value = powercellCharge / powercellMaxCharge;
        switch (value)
        {
            case var expression when value >= 0.02:
                _20percentChargeLed.color = Color.red; 
                break;
            case var expression when value >= 0.2:
                _20percentChargeLed.color = Color.green;
                break;
            case var expression when value >= 0.45:
                _45percentChargeLed.color = Color.green;
                break;
            case var expression when value >= 0.70:
                _70percentChargeLed.color = Color.green;
                break;
            case var expression when value >= 0.95:
                _95percentChargeLed.color = Color.green;
                break;
            default:
                _20percentChargeLed.color = Color.gray;
                _45percentChargeLed.color = Color.gray;
                _70percentChargeLed.color = Color.gray;
                _95percentChargeLed.color = Color.gray;
                break;
        }
    }
}
