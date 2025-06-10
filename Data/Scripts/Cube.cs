using UnityEngine;

[RequireComponent(typeof(Rigidbody))]

public class Cube : MonoBehaviour
{
    private Rigidbody _rigidbody;

    void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    //Метод взятия куба
    public void PrepereDrag()
    { 
        _rigidbody.useGravity = false; //Отключаем гравитацию
        _rigidbody.isKinematic = true; //Делаем куб неподвижным
        _rigidbody.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic; //Кализия куба в руках
    }

    //Метод броска куба
    public void PrepereDrop()
    {
        _rigidbody.useGravity = true;
        _rigidbody.isKinematic = false;
        _rigidbody.collisionDetectionMode = CollisionDetectionMode.Discrete;
    }
}
