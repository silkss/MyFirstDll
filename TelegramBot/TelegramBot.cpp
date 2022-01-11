#pragma comment(lib, "ws2_32.lib")
#include <WinSock2.h>
#include <iostream>

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
	addr;
	int sizeofaddr = sizeof(addr);
	addr.sin_addr.s_addr = inet_addr(ip);
	addr.sin_port = htons(port);
	addr.sin_family = AF_INET;	
}

int connect()
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

void disconnect()
{
	shutdown(connection, SD_BOTH);
	closesocket(connection);
}

void sendMsg(const char* msg)
{
	send(connection, msg, (int)strlen(msg), NULL);
}

int main(int arc, char* argv[])
{	
	int err = connect();
	if (err != 0)
	{
		std::cout << "Error while connecting\n" << err << std::endl;
		return -2;
	}

	sendMsg("Hello");

	system("pause");

	disconnect();
	
	return 0;
}
