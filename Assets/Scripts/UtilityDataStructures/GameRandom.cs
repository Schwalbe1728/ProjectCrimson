﻿using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class GameRandom
{
    private static System.Random SeedGenerationRandom = new System.Random();
    private static System.Random Rand;
    private static char[] SeedCharsAvailable;
    
    public static string Seed { get; private set; }

    private static int SeedLength = 8;

    public static void Initialize()
    {
        SeedGenerationRandom = new System.Random();

        List<char> tmp = new List<char>();

        for(int i = 0; i < 26; i++)
        {
            tmp.Add((char)(65 + i));
            //tmp.Add((char)(97 + i));
        }

        for(int i = 48; i < 58; i++)
        {
            tmp.Add((char)i);
        }

        SeedCharsAvailable = tmp.ToArray();
    }

    public static void GenerateSeed()
    {
        StringBuilder builder = new StringBuilder();

        if (SeedCharsAvailable == null) Initialize();

        for(int i = 0; i < SeedLength; i++)
        {
            builder.Append(SeedCharsAvailable[SeedGenerationRandom.Next(SeedCharsAvailable.Length)]);
        }

        Seed = builder.ToString();
    }

    public static void InitiateWithSeed(string seed = null)
    {
        if (seed == null)
        {
            GenerateSeed();
        }
        else
        {
            Seed = VerifySeed(seed);

            if (Seed == null)
            {
                throw new System.ArgumentException("Invalid characters passed to the seed");
            }
        }

        Rand = new System.Random(Seed.GetHashCode());

        Debug.Log("Seed: " + Seed + ", HashCode: " + Seed.GetHashCode());        
    }

    public static int NextInt()
    {
        if (Rand == null) InitiateWithSeed();

        return Rand.Next();
    }

    public static int NextInt(int max)
    {
        if (Rand == null) InitiateWithSeed();

        return Rand.Next(max);
    }

    public static int NextInt(int min, int max)
    {
        if (Rand == null) InitiateWithSeed();

        return Rand.Next(min, max);
    }

    public static float NextFloat()
    {
        if (Rand == null) InitiateWithSeed();

        return (float)Rand.NextDouble();
    }

    public static float NextFloat(float max)
    {
        if (Rand == null) InitiateWithSeed();

        return max * NextFloat();
    }

    public static float NextFloat(float min, float max)
    {
        if (Rand == null) InitiateWithSeed();

        if (min > max)
        {
            float tmp = min;
            min = max;
            max = tmp;
        }

        return min + NextFloat(max - min);
    }

    private static string VerifySeed(string seed)
    {
        if (seed != null)
        {
            StringBuilder builder = new StringBuilder();
            char[] tmp = seed.ToCharArray();

            for (int i = 0; i < SeedLength && seed != null; i++)
            {
                if (char.IsLetterOrDigit(tmp[i]))
                {
                    tmp[i] = char.ToUpper(tmp[i]);
                    builder.Append(tmp[i]);
                }
                else
                {
                    seed = null;
                }
            }

            seed = builder.ToString();
        }
        return seed;
    }
}
