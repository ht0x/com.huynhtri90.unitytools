using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class Timer
{
    private float m_Time;
    private float m_LastTime;
    private float m_TotalTime;
    private bool m_Enable;

    public bool IsEnabled
    {
        get => m_Enable;
        set => m_Enable = value;
    }

    public Timer()
    {
        m_Time = 0;
        m_LastTime = -1;
        m_TotalTime = 0;
        m_Enable = true;
    }

    public void SetCountDown(float time)
    {
        m_Time = time;
        m_LastTime = time;
        m_TotalTime = time;
    }

    public void Update(float dt)
    {
        m_LastTime = m_Time;
        if (m_Time > 0)
            m_Time -= dt;
        else
            m_Time = -1;
    }

    public bool IsDone()
    {
        if (m_Time <= 0)
            return true;

        return false;
    }

    public bool IsJustDone()
    {
        if ((m_Time <= 0 && m_LastTime != m_Time)
            || (m_LastTime == 0 && m_Time == 0))
        {
            m_Time = -1;
            m_LastTime = -1;
            return true;
        }
        return false;
    }

    public float GetPercent()
    {
        if (m_TotalTime != 0)
            return 1.0f - m_Time / m_TotalTime;

        return 1.0f;
    }

    public float GetExistTime()
    {
        return m_Time;
    }

    public void SetExistTime(float time)
    {
        m_Time = time;
    }
}
