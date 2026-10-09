#pragma once

#define INTIGERSORTER_API __declspec(dllexport)


extern "C" {
    INTIGERSORTER_API void TestSort(int a[], int length);
}