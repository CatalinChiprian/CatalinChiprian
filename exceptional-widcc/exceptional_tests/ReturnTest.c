#include <stdio.h>
#include <stdlib.h>

int test(int a) {
    try {
		printf("Hello before return in try\n");
		if (a == 5) {
			printf("Hello inside if\n");
			return 42;
		}
		// else if (a == 2)
			// return 50;
		//throw 1;
		printf("Hello after return in try\n");
    } catch (int x) {
		// printf("Hello before return in catch\n");
        // return 100;
		// printf("Hello after return in catch\n");
    } finally {
        printf("Cleanup\n");
    }
	return 10;
}


void main() {
	int res1 = test(5);
    printf("Returned %d\n",res1);
}