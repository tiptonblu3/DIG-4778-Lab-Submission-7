using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using ManagedPlugin;

public class FunctionTimer : MonoBehaviour
{
    // Method 1: Native C++ DLL import
    [DllImport("intigerSorter", EntryPoint = "TestSort")]
    public static extern void NativeSort(int[] a, int length);

    void Start()
    {
        RunAllBenchmarks();
    }

    void RunAllBenchmarks()
    {
        // Load the 10,000 numbers once from Assets/Resources/numbers.txt
        int[] originalArray = LoadNumbersFromFile();
        if (originalArray.Length == 0) return;

        
        // The Native Plugin (that uses C++)
            int[] arrayNative = (int[])originalArray.Clone();
            Stopwatch sw1 = Stopwatch.StartNew();
            
            NativeSort(arrayNative, arrayNative.Length);

            sw1.Stop();
            UnityEngine.Debug.Log($"Method 1 (Native C++ Plugin) took: {sw1.ElapsedMilliseconds} ms");


        // The Managed Plugin (that is a C# DLL)
            int[] arrayManaged = (int[])originalArray.Clone();
            Stopwatch sw2 = Stopwatch.StartNew();
            
            ManagedPlugin.IntegerSorter.Sort(arrayManaged);
            
            sw2.Stop();
            UnityEngine.Debug.Log($"Method 2 (Managed C# Plugin) took: {sw2.ElapsedMilliseconds} ms");


        // The average MonoBehaviour C# Script
            int[] arrayVanilla = (int[])originalArray.Clone();
            Stopwatch sw3 = Stopwatch.StartNew();
            
            VanillaMonoBehaviourSort(arrayVanilla);

            sw3.Stop();
            UnityEngine.Debug.Log($"Method 3 (MonoBehaviour Class) took: {sw3.ElapsedMilliseconds} ms");
    }

    void VanillaMonoBehaviourSort(int[] a)
    {
        // Standard C# sorting inside the MonoBehaviour class
        Array.Sort(a);
    }

    public static int[] LoadNumbersFromFile()
    {
        TextAsset textFile = Resources.Load<TextAsset>("numbers");
        if (textFile == null)
        {
            UnityEngine.Debug.LogError("numbers.txt not found in Resources folder! Make sure it's named numbers.txt and placed in Assets/Resources/");
            return new int[0];
        }

        // Split by newlines, carriage returns, or commas
        return textFile.text
            .Split(new char[] { '\n', '\r', ',' }, StringSplitOptions.RemoveEmptyEntries)
            .Select(int.Parse)
            .ToArray();
    }
}
