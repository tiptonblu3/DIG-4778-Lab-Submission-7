#include "intigerSorter.h"
#include <algorithm>

extern "C" {
    INTIGERSORTER_API void TestSort(int a[], int length) {
        std::sort(a, a + length);
    }
}


