#pragma once
extern "C" __declspec(dllexport) int connect_to_socket();
extern "C" __declspec(dllexport) void disconnect();
extern "C" __declspec(dllexport) int sendMsg(const char* msg);
