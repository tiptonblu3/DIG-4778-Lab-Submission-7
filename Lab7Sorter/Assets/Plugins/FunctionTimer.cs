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
    public monoIntigerSorter Mono;

    void Start()
    { 
        RunAllBenchmarks();
    }

    void RunAllBenchmarks()
    {
        // Load the 10,000 numbers once from Assets/Resources/numbers.txt
        int[] originalArray = LoadNumbersFromFile();
        if (originalArray.Length == 0) return;

        //for micro seconds
        double frequency = (double)Stopwatch.Frequency; 
        
        // The Native Plugin (that uses C++)
            int[] arrayNative = (int[])originalArray.Clone();
            long startNative = Stopwatch.GetTimestamp();
            
            NativeSort(arrayNative, arrayNative.Length);

            long ticksNative = Stopwatch.GetTimestamp() - startNative;
            double microsNative = ticksNative * 1000000.0 / frequency;
            UnityEngine.Debug.Log($"Native C++ Plugin took: {microsNative:F2} microseconds");


        // The Managed Plugin (that is a C# DLL)
            int[] arrayManaged = (int[])originalArray.Clone();
            long startManaged = Stopwatch.GetTimestamp();
            
            ManagedPlugin.IntegerSorter.Sort(arrayManaged);
            
            long ticksManaged = Stopwatch.GetTimestamp() - startManaged;
            double microsManaged = (ticksManaged * 1000000.0) / frequency;
            UnityEngine.Debug.Log($"Managed C# Plugin took: {microsManaged:F2} microseconds");


        // The average MonoBehaviour C# Script
            int[] arrayVanilla = (int[])originalArray.Clone();
            long startVanilla = Stopwatch.GetTimestamp();
            
            Mono.VanillaMonoBehaviourSort(arrayVanilla);

            long ticksVanilla = Stopwatch.GetTimestamp() - startVanilla;
            double microsVanilla = (ticksVanilla * 1000000.0) / frequency;
            UnityEngine.Debug.Log($"MonoBehaviour Script took: {microsVanilla:F2} microseconds");
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
