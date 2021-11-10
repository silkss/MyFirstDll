#pragma once
#define DLLEXPORT __declspec(dllexport)

#include <string>
#include <iostream>
#include <fstream>

extern "C" __declspec(dllexport) int ReturnSomething(const char* c_str);
