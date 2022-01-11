#pragma comment(lib, "ws2_32.lib")

#include "ExpDll.h"

#include <WinSock2.h>
#include <string>

#pragma warning(disable: 4996)

SOCKET connection;
SOCKADDR_IN addr;

int initWsa()
{
	WSAData wsaData;
	WORD DLLversion = MAKEWORD(2, 2);
	if (WSAStartup(DLLversion, &wsaData) != 0)
	{
		return -1;
	}
	return 0;
}

void createAddr(const char* ip, u_short port)
{
	int sizeofaddr = sizeof(addr);
	addr.sin_addr.s_addr = inet_addr(ip);
	addr.sin_port = htons(port);
	addr.sin_family = AF_INET;
}

extern "C" __declspec(dllexport) int connect_to_socket()
{
	if (initWsa() != 0) return -1;

	createAddr("127.0.0.1", 1234);

	connection = socket(AF_INET, SOCK_STREAM, NULL);

	if (connect(connection, (SOCKADDR*)&addr, sizeof(addr)) != 0)
	{
		return WSAGetLastError();
	}

	return 0;
}

extern "C" __declspec(dllexport) void disconnect()
{
	shutdown(connection, SD_SEND);
}

extern "C" __declspec(dllexport) int sendMsg(const char * msg)
{
	if (send(connection, msg, (int)strlen(msg), NULL) != 0)
	{
		return WSAGetLastError();
	}
	return 0;
}
