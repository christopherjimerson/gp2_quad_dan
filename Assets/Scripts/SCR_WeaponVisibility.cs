using UnityEngine;

public class SCR_WeaponVisibility : MonoBehaviour
{
    public static SCR_WeaponVisibility Instance;

    private void Awake()
    {
        Instance = this;
    }

    public void EnableWeaponSelect()
    {
        gameObject.SetActive(true);
    }

    public void DisableWeaponSelect()
    {
        gameObject.SetActive(false);
    }

}
