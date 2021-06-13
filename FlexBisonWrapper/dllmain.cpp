// dllmain.cpp : Defines the entry point for the DLL application.

#include <stdio.h>
#include <iostream> 
#include "dllmain.h"

extern "C" int parser_main(int argc, char* argv[]);

extern "C"
{
    __declspec(dllexport) int CallParserFromDLL(int argc, char* argv[])
    {
         //calling the parser main from dll
        return parser_main(argc, argv);
    }
}








