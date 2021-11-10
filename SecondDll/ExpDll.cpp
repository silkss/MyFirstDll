#include "ExpDll.h"


extern "C" __declspec(dllexport) int ReturnSomething(const char * c_str)
{
	try
	{
		std::ofstream myfile;
		myfile.open("example.txt");
		myfile << c_str << std::endl;
		myfile.close();
		return 0;
	}
	catch (const std::exception&)
	{
		return -1;
	}
}
