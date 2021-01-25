using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimerController : MonoBehaviour
{
    private float _timer;
    private float _playerTime;
    private float _limit;
    private bool _timeOut = true;


    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (!_timeOut)
        {
            if (_timer > 0)
            {

                _timer -= Time.deltaTime;
                _timer = Mathf.Clamp(_timer, 0, _timer);
                _playerTime += Time.deltaTime;

            }
            else
            {
                _timer = 0;
                _timeOut = true;
            }
        }
    }

    public void SetTimerLimit(float limit)
    {
        _limit = limit;
        _timer = limit;


    }
    public float GetTimer()
    {
        return _timer;
    }
    public float GetPlayerTime()
    {
        return _playerTime;
    }
    //Resets the timer limit and and timeOut to false
    public void ResetTimer()
    {
        _timer = _limit;
        _timeOut = false;

    }
    public void StopTimer()
    {
        _timeOut = true;
    }

    public bool IsTimeOut()
    {
        return _timeOut;
    }
    public void AddTime(float time)
    {
        if (_timer > 0)
        {
            _timer += time;
            _timer = Mathf.Clamp(_timer, 0, _limit);

        }


    }
}
