using UnityEngine;

public class HurtComponent : MonoBehaviour
{
    public enum OwnerType { Player, Enemy }
    public OwnerType owner;
    public int damage = 1;
    public float lifeTime = 5f;

    void Start()
    {
        Destroy(gameObject, lifeTime);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        // ���� ����������� � ����� ���������������� ��������� � ���������� ���
        HurtComponent otherBullet = other.GetComponent<HurtComponent>();
        if (otherBullet != null && otherBullet.owner != this.owner)
        {
            Destroy(other.gameObject);
            Destroy(gameObject);
            return;
        }

        Health health = other.GetComponent<Health>();
        if (health != null)
        {
            // ���� ���� ������ � �� ������� ���� ������, ���� ����� � �� ������� ���� �����
            if ((owner == OwnerType.Player && other.CompareTag("Player")) ||
                (owner == OwnerType.Enemy && other.CompareTag("Enemy")))
            {
                return;
            }

            health.TakeDamage(damage);
            Destroy(gameObject);
        }
    }
}
