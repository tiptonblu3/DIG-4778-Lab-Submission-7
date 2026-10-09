using UnityEngine;
using System.Runtime.InteropServices;

public class TestDLL : MonoBehaviour {
    // The imported function
    [DllImport("intigerSorter", EntryPoint = "TestSort")]
    public static extern void TestSort(int[] a, int length);

    public int[] a = { 5, 2, 9, 1, 5, 6 }; // Default values for testing

    void Start() 
    {
        if (a != null && a.Length > 0)
        {
            Debug.Log("Before sort: " + string.Join(", ", a));
            TestSort(a, a.Length);
            Debug.Log("After sort: " + string.Join(", ", a));
        }
        else
        {
            Debug.LogError("Array 'a' is empty or not assigned!");
        }
    }
}
