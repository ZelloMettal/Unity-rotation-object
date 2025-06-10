using UnityEngine;

//Подключение компонентов к объекту
[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(CapsuleCollider))]
[RequireComponent(typeof(CharacterController))]

public class PlayerMover : MonoBehaviour
{
    //Считывание перемещение персонажа
    private const string Horizontal = "Horizontal";
    private const string Vertical = "Vertical";

    [SerializeField] private float _speed; //Скорость персонажа
    private CharacterController _characterController; //Контроллер игрока(для перемещения)
    private float _directionHorizontal; //Направление движение по горизонтали
    private float _directionVertical; //Направление движения по вертикали
    private Vector3 _move; //Вектор движения

    private void Awake()
    {
        _characterController = GetComponent<CharacterController>(); //Получили контроллер движения персонажа
    }

    void FixedUpdate()
    {
        //Считываем нажатие кнопок движения
        _directionHorizontal = Input.GetAxis(Horizontal);
        _directionVertical = Input.GetAxis(Vertical);
        _move = transform.forward * _directionVertical + transform.right * _directionHorizontal; //Вычисление направления
        _characterController.Move(_move * _speed * Time.deltaTime); //перемещаем персонажа
    }
}
