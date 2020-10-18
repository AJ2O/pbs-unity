using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameEffect
{
    public bool[] boolParams { get; private set; }
    public float[] floatParams { get; private set; }
    public string[] stringParams { get; private set; }

    // Constructor
    public GameEffect(
        bool[] boolParams = null,
        float[] floatParams = null,
        string[] stringParams = null
        )
    {
        this.boolParams = (boolParams == null) ? new bool[0] : new bool[boolParams.Length];
        if (boolParams != null)
        {
            for (int i = 0; i < boolParams.Length; i++)
            {
                this.boolParams[i] = boolParams[i];
            }
        }

        this.floatParams = (floatParams == null) ? new float[0] : new float[floatParams.Length];
        if (floatParams != null)
        {
            for (int i = 0; i < floatParams.Length; i++)
            {
                this.floatParams[i] = floatParams[i];
            }
        }

        this.stringParams = (stringParams == null) ? new string[0] : new string[stringParams.Length];
        if (stringParams != null)
        {
            for (int i = 0; i < stringParams.Length; i++)
            {
                this.stringParams[i] = stringParams[i];
            }
        }
    }

    // Clone
    public static GameEffect Clone(GameEffect original)
    {
        GameEffect cloneEffect = new GameEffect(
            boolParams: original.boolParams,
            floatParams: original.floatParams,
            stringParams: original.stringParams
            );
        return cloneEffect;
    }

    public bool GetBool(int index)
    {
        return boolParams[index];
    }
    public float GetFloat(int index)
    {
        return floatParams[index];
    }
    public string GetString(int index)
    {
        return stringParams[index];
    }

    public List<bool> GetBoolList()
    {
        List<bool> bools = new List<bool>();
        for (int i = 0; i < boolParams.Length; i++)
        {
            bools.Add(boolParams[i]);
        }
        return bools;
    }
    public List<float> GetFloatList()
    {
        List<float> floats = new List<float>();
        for (int i = 0; i < floatParams.Length; i++)
        {
            floats.Add(floatParams[i]);
        }
        return floats;
    }
    public List<string> GetStringList()
    {
        List<string> strings = new List<string>();
        for (int i = 0; i < stringParams.Length; i++)
        {
            strings.Add(stringParams[i]);
        }
        return strings;
    }
}
