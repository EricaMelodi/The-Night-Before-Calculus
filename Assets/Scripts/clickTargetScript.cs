using UnityEngine;

public class ClickTarget : MonoBehaviour
{
    public MonsterTesting monster;

    void OnMouseDown()
    {
        if (monster != null)
            monster.ForceChase();

        // "Collect" the cube
        gameObject.SetActive(false);
    }
}
