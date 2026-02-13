using UnityEngine;

public class ClickTarget : MonoBehaviour
{
    [SerializeField] private MonsterTesting monster;

    void OnMouseDown()
    {
        if (monster == null)
        {
            Debug.LogWarning("Monster reference not set on " + gameObject.name);
            return;
        }

        monster.ForceChase();
        gameObject.SetActive(false);
    }
}
