using UnityEngine;

public class CRandomNumber
{
    #region ----- Variable -----

    private const int RANDOM_MAX = 999999;

    private static int mEvenFactor = 6;
    private static int mOddFactor = 3;
    private static int mFirstValue;
    private static int mValue;
    private static int mCount;

    #endregion

    #region ----- Method -----

    /// <summary>
    /// Thiết lập hệ số random trong khoảng [1 - 999].
    /// </summary>
    public static void SetRandomSeed(int seed, bool isRefresh = true)
    {
        if (seed < 0)
        {
            seed = -seed;
        }
        seed = (seed % 999) + 1;

        mEvenFactor = (seed * 2) + 2;
        mOddFactor = (seed * 3 / 2) + 1;

        if (isRefresh)
        {
            mValue = 0;
            mCount = 0;
            mFirstValue = -1;
        }

        NextRandom();
        mFirstValue = mValue;

#if DEBUG_DESYNC
        CPoolerManager.Instance.WRITE("SetRandomSeed=" + seed + " -  FirstValue=" + m_nFirstValue + " -  EvenFactor=" + m_nEvenFactor + " -  OddFactor=" + m_nOddFactor + " -  Count=" + m_nCount);
#endif
    }

    /// <summary>
    /// Tạo số thập phân ngẫu nhiên trong khoảng [0, 1].
    /// </summary>
    public static float Value01()
    {
        NextRandom();
        return (mValue % 1000) / 1000f;
    }

    /// <summary>
    /// Tạo số nguyên ngẫu nhiên trong khoảng [begin, end].
    /// </summary>
    public static int Range(int begin, int end)
    {
        NextRandom();
        return (mValue % (end - begin + 1)) + begin;
    }

    /// <summary>
    /// Tạo số thập phân ngẫu nhiên trong khoảng [begin, end].
    /// </summary>
    public static float Range(float begin, float end)
    {
        float range = end - begin;
        if (range <= 1)
        {
            return (int)(((Value01() % range) + begin) * 1000) / 1000f;
        }
        else
        {
            return Range((int)begin, (int)(end - 1)) + Value01();
        }
    }

    /// <summary>
    /// Sinh giá trị ngẫu nhiên.
    /// </summary>
    private static void NextRandom()
    {
        mValue = ((mValue * mOddFactor) + mEvenFactor + mCount) % RANDOM_MAX;
        mCount++;

        // tránh lặp giá trị
        if (mFirstValue == mValue)
        {
            mValue = (mValue + mCount) % RANDOM_MAX;
            mCount = 0;
        }

#if DEBUG_DESYNC
        CPoolerManager.Instance.WRITE("NextRandom = " + m_nValue);
#endif
    }

    #endregion
}