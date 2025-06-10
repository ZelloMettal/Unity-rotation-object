using UnityEditor;
using UnityEngine;

public class Interactible : MonoBehaviour
{
    private const string MouseScroll = "Mouse ScrollWheel"; //Считывание скролла мыши
    
    enum RotationAxis { X = 0, Y, Z }   //Перечисление осей для вращения
    private RotationAxis _currentRotationAxis = RotationAxis.X; //Текущая ось вращения

    [SerializeField] private Camera _camera; // Объект камеры
    [SerializeField] private int _sizeAim; // Размер прицела
    [SerializeField] private string _aimChar; // Изображение прицела

    //Луч направления камеры(для определения столкновния с предметами)
    [SerializeField] private Transform _startPointRay; // Начала луча
    [SerializeField] private float _rayDistance; // Длина луча

    //Вектор позиции объекта(где находится объект при взятие)
    [SerializeField] private Transform _itemPosition; //Положение предмета
    [SerializeField] private Transform _itemRotationPosition; //Положение предмета перед игроком при вращении
    [SerializeField] private float _rotationSpeed; //Скорость вращения объекта

    //Координаты отрисовки прицела
    private float _aimPositionDrawX;
    private float _aimPositionDrawY;

    //Взаимодействие с объектом
    private bool _isHaveItem = false; //Объект взят
    private bool _isRotationItem = false; //Объект вращается
    private Cube _tempCube; //Контейнер хранение объекта
    private float _directionMouseScroll; //Направление скролла мыши

    //Позиция углов  по осям у взятого щбъекта
    private float _objectAnglePositionX;
    private float _objectAnglePositionY;
    private float _objectAnglePositionZ;

    //Переменные луча
    private Ray _ray;
    private RaycastHit _raycastHit;

    void Update()
    {
        CastRay();
        TakeItem();
        ActivateRotationItem();
        RotationItem();
        ChangeRotationAxis();
    }

    //Отрисовка статического объекта на экране(прицел)
    private void OnGUI()
    {
        //Задаём положение прицела
        _aimPositionDrawX = _camera.pixelWidth / 2 - _sizeAim / 4;
        _aimPositionDrawY = _camera.pixelHeight / 2 - _sizeAim / 2;

        GUI.Label(new Rect(_aimPositionDrawX, _aimPositionDrawY, _sizeAim, _sizeAim), _aimChar); //Отрисовка прицела(координаты объекта и объект отрисовки)
    }

    //Метод отрисовки луча
    private void CastRay()
    {
        _ray = new Ray(_startPointRay.position, _camera.transform.forward); //Направление луча относительно камеры
        Debug.DrawRay(_ray.origin, _ray.direction * _rayDistance, Color.red); //Отрисовка луча
    }

    //Метод взаимодействия с объектом(Подбирание)
    private void TakeItem()
    {
        //Проверям, что кнопка мыши нажата и в руках ничего нет
        if (Input.GetMouseButtonDown(0) && !_isHaveItem)
        {
            //Проверяем, что луч камеры направлен на объект
            if (Physics.Raycast(_ray, out _raycastHit, _rayDistance))
            {
                //Проверяем, что мы смотрим на необъодимый объект
                if (_raycastHit.transform.TryGetComponent<Cube>(out Cube cube))
                { 
                    _tempCube = cube;   //Сохраняем объект куба
                    _tempCube.PrepereDrag(); //Подготовка куба(класс куба)
                    Drag(); //Удержание куба в руках
                }
            }
        }
        else if (Input.GetMouseButtonDown(0) && _isHaveItem) //Отпускание куба
        {
            Drop();
        }
    }

    //Метод перехода в режим вращения объекта
    private void ActivateRotationItem()
    {
        //Проверяем нажатие правого клика, предмет в руках, предмет не вращается
        if (Input.GetMouseButtonDown(1) && _isHaveItem && !_isRotationItem)
        {
            _isRotationItem = true; //Предмет в режиме вращения
            _tempCube.transform.position = _itemRotationPosition.position;  //Помещаем предмет перед игроком
            //Фиксируем начальные координаты углов предмета
            _objectAnglePositionX = _tempCube.transform.localEulerAngles.x;
            _objectAnglePositionY = _tempCube.transform.localEulerAngles.y;
            _objectAnglePositionZ = _tempCube.transform.localEulerAngles.z;
            Debug.Log("Выбрана Ось X");
        }
        //Для выхода из режима вращения проверяем нажатие правого клика, предмет руках, предмет вращается
        else if (Input.GetMouseButtonDown(1) && _isHaveItem && _isRotationItem)
        {
            _isRotationItem = false;  //Предмет не врежиме вращения  
            _tempCube.transform.position = _itemPosition.position;  //Возвращаем предмет в руку игрока
        }
    }
    //Метод выбора оси вращения
    private void ChangeRotationAxis()
    {
        //Проверяем нажатие колёсика мыши, предмет в руках, предмет в режиме вращения
        if (Input.GetMouseButtonDown(2) && _isHaveItem && _isRotationItem)
        {
            switch (_currentRotationAxis)
            {
                //Если текущая ось Х
                case RotationAxis.X:
                { 
                    _currentRotationAxis++; //Выбираем следующую ось в перечислении осей
                    //Фиксируем координы углов объекта которые не будут меняться относительно выбранной оси
                    _objectAnglePositionX = _tempCube.transform.localEulerAngles.x;
                    _objectAnglePositionZ = _tempCube.transform.localEulerAngles.z;
                    Debug.Log("Выбрана Ось Y");
                } 
                break;
                //Если текущая ось Y
                case RotationAxis.Y:
                { 
                    _currentRotationAxis++;
                    _objectAnglePositionX = _tempCube.transform.localEulerAngles.x;
                    _objectAnglePositionY = _tempCube.transform.localEulerAngles.y;
                    Debug.Log("Выбрана Ось Z");
                }
                break;
                //Если текущая ось Z
                case RotationAxis.Z:
                { 
                    _currentRotationAxis = 0;
                    _objectAnglePositionY = _tempCube.transform.localEulerAngles.y;
                    _objectAnglePositionZ = _tempCube.transform.localEulerAngles.z;
                    Debug.Log("Выбрана Ось X");
                }
                break;
            }
        }
    }

    //Вращение объекта
    private void RotationItem()
    {
        //проверяем что объект в руках и режиме вращения
        if (_isHaveItem && _isRotationItem)
        {
            _directionMouseScroll = Input.GetAxis(MouseScroll); //Считываем направление скролла мыши
            //Проверяем изменилось ли направление скрола мыши 
            if (_directionMouseScroll != 0)
            {
                //Вращаем объект согласно выбранной оси
                switch (_currentRotationAxis)
                {
                    case RotationAxis.X:
                    {
                        _objectAnglePositionX += _directionMouseScroll * _rotationSpeed; //Получаем смещение угла поворота объекта на выбранной оси
                        _tempCube.transform.localRotation = Quaternion.Euler(_objectAnglePositionX, _objectAnglePositionY, _objectAnglePositionZ);   //Вращаем объект                 
                    }
                    break;
                    case RotationAxis.Y:
                    {
                        _objectAnglePositionY += _directionMouseScroll * _rotationSpeed;
                        _tempCube.transform.localRotation = Quaternion.Euler(_objectAnglePositionX, _objectAnglePositionY, _objectAnglePositionZ); 
                    }
                    break;
                    case RotationAxis.Z:
                    {
                        _objectAnglePositionZ += _directionMouseScroll * _rotationSpeed;
                        _tempCube.transform.localRotation = Quaternion.Euler(_objectAnglePositionX, _objectAnglePositionY, _objectAnglePositionZ);
                    }
                    break;
                }                
            }    
        }
    }

    //Метод поднятия куба
    private void Drag()
    {
        _isHaveItem = true;
        _tempCube.transform.position = _itemPosition.position;
        _tempCube.transform.SetParent(this.transform);
    }

    //Метод отпускания куба
    private void Drop()
    {
        _isHaveItem = false;
        _isRotationItem = false;
        _tempCube.PrepereDrop();     
        _tempCube.transform.SetParent(null);
        _tempCube = null;
        _currentRotationAxis = 0;
    }
}
