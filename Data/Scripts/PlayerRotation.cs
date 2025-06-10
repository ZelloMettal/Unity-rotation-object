using UnityEngine;

public class PlayerRotation : MonoBehaviour
{
    //Считываем координаты мыши
    private const string LineRotationY = "Mouse Y";
    private const string LineRotationX = "Mouse X";

    [SerializeField] private float _sensitivity;    //Чувствительность мыши
    //Ограничения угла поворота камеры
    private int _minRotation = -45;
    private int _maxRotation = 45;
    //Координаты поворота камеры
    private float _mouseY;
    private float _mouseX;
    private float _rotationX;

    void Start()
    {
        Cursor.visible = false; //Скрываем курсор
        Cursor.lockState = CursorLockMode.Locked;   //Блокируем движение курсора
    }
  
    void Update()
    {
        //Получаем координаты мыши
        _mouseX = Input.GetAxis(LineRotationX);
        _mouseY = Input.GetAxis(LineRotationY);

        transform.parent.Rotate(Vector3.up * _mouseX * _sensitivity * Time.deltaTime); //Поворачиваем ИГРОКА по горизонтали

        _rotationX -= _mouseY * _sensitivity * Time.deltaTime;  //Перемещаем КАМЕРУ по вертикали
        _rotationX = Mathf.Clamp(_rotationX, _minRotation, _maxRotation);  //Ограничиваем угол поворота камеры
        transform.localRotation = Quaternion.Euler(_rotationX, 0, 0);   //Перемещаем камеру на игроке
    }
}
