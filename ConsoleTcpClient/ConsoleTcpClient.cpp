#include "includes.h"

int main()
{
	printf("0: show help.\n");

	int isworking = 1;
	int command = 0;
	int err = 0;

	err = connect_to_socket();
	if (err == 0) sendMsg("ESH2;0;INIT");

	printf("Connection status: %d\n", err);

	while (isworking)
	{
		err = scanf_s("%d", &command);
		if (err == 0) continue;

		switch (command)
		{
		case 0:
			print_help();
			break;

		case 1:
			err = sendMsg("ESH2;0;INIT");
			break;
			
		case 2:
			err = sendMsg("ESH2;4500;OPEN");
			break;

		case 3:
			err = sendMsg("ESH2;4500;CLOSE");
			break;

		case 11:
			disconnect();
			isworking = 0;
			break;

		default:
			break;
		}
		printf("send_msg status: %d\n", err);
	}
}

