using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class NewMonoBehaviourScript : MonoBehaviour
{
    [SerializeField] private float _delay;  //Задержка корутины
    [SerializeField] private Text _text;    //Объект для вывода Текста на сцене
    [SerializeField] private KeyCode _kayCommand;   //Задание клавиши для запуска счётчика

    private int _numberText = 0;    //Начальное число счётчика
    private Coroutine _coroutine = null;    //Объёет корутины
    
    void Update()
    {
        //Проверяем нажатие кнопки для запуска счётчика
        if (Input.GetKeyDown(_kayCommand))
        {
            if (_coroutine == null) //Если корутина не создана
            {
                _coroutine = StartCoroutine(Scored(_delay));    //Запускаем корутину
            }
            else 
            {
                StopCoroutine(_coroutine);  //Останавливаем корутину
                _coroutine = null;  //Очищаем память
            }
        }
    }

    //Метод для выполнения корутины
    private IEnumerator Scored(float delay)
    { 
        //Время ожидания корутины
        WaitForSeconds wait = new WaitForSeconds(delay);
        _text.text = _numberText.ToString();

        //Выполняем корутину пока объект включён
        while (enabled)
        { 
            _numberText++;  //Увеличиваем счётчик
            _text.text = _numberText.ToString();    //Помещаем число счётчика в Текст
            yield return wait;  //Задержка корутины
        }
    }
}
