using UnityEngine;
using System.Collections.Generic;
public class HealthUI : MonoBehaviour
{
    [SerializeField] private GameObject heartPrefab; // ������ � Image, ��� ��������� ������ ������

    private Health playerHealth;    // ������ �������� ������
    private List<GameObject> hearts = new List<GameObject>();
    private int lastHealth = -1;

    void Start()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
            playerHealth = player.GetComponent<Health>();

        UpdateHearts();
    }

    void Update()
    {
        if (playerHealth != null && playerHealth.currentHealth != lastHealth)
        {
            UpdateHearts();
        }
    }

    private void UpdateHearts()
    {
        // ������� ������ ������
        foreach (var heart in hearts)
        {
            Destroy(heart);
        }
        hearts.Clear();

        // ������ ����� ������ �� ���������� ������
        int health = playerHealth != null ? playerHealth.currentHealth : 0;
        for (int i = 0; i < health; i++)
        {
            GameObject heart = Instantiate(heartPrefab, transform);
            hearts.Add(heart);
        }

        lastHealth = health;
    }
}
