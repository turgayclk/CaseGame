using UnityEngine;

public class DamagePopupManager : MonoBehaviour
{
    public static DamagePopupManager Instance;

    [SerializeField] private DamagePopup popupPrefab;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void ShowPopup(float damage, Vector3 worldPosition)
    {
        DamagePopup popup = Instantiate(popupPrefab, worldPosition, popupPrefab.transform.rotation);
        popup.Setup(damage);
    }
}
