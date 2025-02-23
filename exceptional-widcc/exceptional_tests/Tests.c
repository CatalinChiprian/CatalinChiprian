#include <stdio.h>
#include <stdlib.h>

// Test basic integer exception
int test_basic_int() {
    try {
        printf("Basic int test\n");
        throw 42;
    }
    catch(int x) {
        printf("Caught int: %d\n", x);
        return x;
    }
    return 0;
}

// Test param integer exception
int test_param_int(int a, int b) {
    try {
        int throw_sum = a + b;
        printf("Param int test\n");
        throw throw_sum;
    }
    catch(int x) {
        printf("Caught int: %d\n", x);
    }
    finally {
        printf("Param int finally\n");
        return x;
    }
    return 0;
}

// Test float exception
float test_float() {
    try {
        float f = 3.14f;
        printf("Float test\n");
        throw f;
    }
    catch(float x) {
        printf("Caught float: %f\n", x);
        return x;
    }
    finally {
        printf("Float finally\n");
    }
    return 0.0f;
}

// Test nested try-catch
int test_nested() {
    try {
        printf("Outer try\n");
        try {
            printf("Inner try\n");
            throw 1;
        }
        catch(int x) {
            printf("Inner catch: %d\n", x);
        }
        finally {
            printf("Inner finally\n");
        }
		throw 2;
    }
    catch(int x) {
        printf("Outer catch: %d\n", x);
        return x;
    }
    finally {
        printf("Outer finally\n");
    }
    return 0;
}

// Test pointer exception
void* test_pointer() {
    int* result_ptr = NULL;
    try {
        int* ptr = malloc(sizeof(int));
        *ptr = 100;
        printf("Pointer test\n");
        throw (int*)ptr;
    }
    catch(int* p) {
        printf("Caught pointer: %p\n", p);
    }
    finally {
        printf("Pointer finally\n");
        result_ptr= p;
        free(p);
    }
    return result_ptr;
}

// Test char exception
char* test_char() {
    try {
        char* my_char = 'a';
        printf("Char test\n");
        throw my_char;
    }
    catch(char* caught_char) {
        printf("Caught char: %c\n", caught_char);
    }
    finally {
        printf("Char finally\n");
        return caught_char;
    }
}

// Test string exception
char* test_string() {
    try {
        char* my_string = "TheString";
        printf("String test\n");
        throw my_string;
    }
    catch(char* caught_string) {
        printf("Caught char: %s\n", caught_string);
    }
    finally {
        printf("Char finally\n");
        return caught_string;
    }
}

// Test looped try catch
int looped_try_catch() {
    int ex_nr=0;
	while(ex_nr != 10) {
        try {		
            throw ex_nr;
        }
        catch(int catch_nr) {
            printf("Caught exception: %d\n", catch_nr);
        }
        finally {
            printf("Hello from finally block\n");
            ex_nr++;
        }
	}
    return 0;
}

// Test long int exception
long int func_long_int() {
    try {
        long int my_long_int= 3000000000;
        throw my_long_int;
    }
    catch(long int x) {
        printf("Caught long int: %ld\n", x);
        return x;
    }
    finally {
        return 0;
    }
}

void func_struct() {
    struct test_struct {
        int* a;
        int b;
        char c;
    };
    
    struct test_struct *ptr = NULL;
    int* allocated_mem = NULL;
    
    try {
        struct test_struct my_struct;
        
        // Proper allocation
        allocated_mem = malloc(sizeof(int));
        if (!allocated_mem) {
            printf("Memory allocation failed\n");
            return;
        }
        *allocated_mem = 10;
        
        my_struct.a = allocated_mem;
        my_struct.b = 100;
        my_struct.c = 'G';
        
        printf("Throwing struct: {a=%d, b=%d, c=%c}\n", 
               *my_struct.a, my_struct.b, my_struct.c);
               
        throw my_struct;
    }
    catch(struct test_struct caught_struct) {
        printf("Caught struct: {a=%d, b=%d, c=%c}\n", *caught_struct.a, caught_struct.b, caught_struct.c);
        allocated_mem = caught_struct.a;
    }
    finally {
        printf("Struct finally\n");
        if (allocated_mem) {
            free(allocated_mem);
            allocated_mem = NULL;
        }
    }
}

int thrown_func() {
   int x = 23;
   return x;
}

void throw_func() {
    try{
        throw thrown_func();
    }
    catch(int x) {
        printf("Caught int: %d\n", x);
    }
    finally {
        printf("Finally block\n");
    }
}

int main() {
    printf("=== Starting Exception Tests ===\n\n");

    printf("1. Basic Integer Test\n");
    int result1 = test_basic_int();
    printf("Result: %d\n\n", result1);

    printf("2. Integer with Parameters Test\n");
    int result2 = test_param_int(3, 2);
    printf("Result: %d\n\n", result2);

    printf("3. Float Test\n");
    float result3 = test_float();
    printf("Result: %f\n\n", result3);

    printf("4. Nested Try-Catch Test\n");
    int result4 = test_nested();
    printf("Result: %d\n\n", result4);

    printf("5. Pointer Test\n");
    void* result5 = test_pointer();
    printf("Result: %p\n\n", result5);

    printf("6. Char Test\n");
    char* result6 = test_char();
    printf("Result: %c\n\n", result6);

    printf("7. String Test\n");
    char* result7 = test_string();
    printf("Result: %s\n\n", result7);

    printf("8. Looped Try-Catch Test\n");
    looped_try_catch();
    printf("Result: Loop Completed\n\n");

    printf("9. Long Integer Test\n");
    long int result9 = func_long_int();
    printf("Result: %ld\n\n", result9);

    printf("10. Struct Test\n");
    func_struct();
    printf("Result: Struct Test Completed\n\n");

    printf("11. Thrown Function Test\n");
    throw_func();
    printf("Result: Thrown Function Test Completed\n\n");

    printf("=== All Tests Completed ===\n");
    return 0;
}