using UnityEngine;

public class CameraFollow2 : MonoBehaviour
{
    public Transform targett; // Ссылка на объект, за которым следит камера
    public float smoothing = 5.0f; // Сглаживание движения камеры
    [SerializeField]
    float leftLimit;
    [SerializeField]
    float rightLimit;
    [SerializeField]
    float bottomLimit;
    [SerializeField]
    float upperLimit;

    void FixedUpdate()
    {
        if (targett != null)
        {
            Vector3 targetPosition = new Vector3(targett.position.x, targett.position.y, transform.position.z);

            // Интерполируем между текущей позицией камеры и позицией цели для сглаживания
            transform.position = Vector3.Lerp(transform.position, targetPosition, smoothing * Time.deltaTime);
        }

        transform.position = new Vector3
        (
        Mathf.Clamp(transform.position.x, leftLimit, rightLimit),
        Mathf.Clamp(transform.position.y, bottomLimit, upperLimit),
        transform.position.z
        );
    }
}
