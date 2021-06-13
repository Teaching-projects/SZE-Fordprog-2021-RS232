%{    
    #include <stdio.h>
    #include <stdlib.h>
    #include <string.h>

    int yylex();

    void yyerror(char *s);

    typedef struct yy_buffer_state* YY_BUFFER_STATE;
    extern YY_BUFFER_STATE yy_scan_string(char* str);
    extern void yy_delete_buffer(YY_BUFFER_STATE buffer);
%}

%union 
{
    int ival;
    double dval;
    char *sval;
}

%token<ival> INTEGER
%token<dval> DOUBLE
%token NEWLINE
%token TEXT
%token DATE_SPECIAL

%type<ival> measurementi
%type<dval> measurementd

%%

command : /* empty */  
	    | tags  
        ;

tags : tag  
     | tags ';' tag  

tag : '"' value '"'
    ;

value: measurementi { printf("\tInteger measurement: %i\n", $1);} 
     | measurementd { printf("\tDecimal measurement: %f\n", $1);}  
     | date 
     | text
     | numbers 
     ; 

text : TEXT 
     | text ' ' TEXT

date : INTEGER DATE_SPECIAL INTEGER DATE_SPECIAL INTEGER ' ' INTEGER DATE_SPECIAL INTEGER DATE_SPECIAL INTEGER ' ' TEXT  
     | INTEGER DATE_SPECIAL INTEGER DATE_SPECIAL INTEGER ' ' INTEGER DATE_SPECIAL INTEGER DATE_SPECIAL INTEGER  
     ;

numbers : DOUBLE
        | INTEGER 
        | numbers DOUBLE
        | numbers INTEGER
        ;

measurementi : '+' INTEGER  { $$ = $2; }
             | '-' INTEGER  { $$ = $2 * -1; }
             ;

measurementd : '+' DOUBLE { $$ = $2; }
             | '-' DOUBLE { $$ = $2 * -1; }
             ;

%%

int parser_main(int argc, char* argv[])
{
    int retval = 0;
	printf("Input from RS232 to parser: %s\n", argv[0]);
    printf("Starting to parse:\n");
    printf("******************\n");

	YY_BUFFER_STATE buffer = yy_scan_string(argv[0]);
	if (yyparse()== 0) 
    {
        printf("***ACC***\n");
        printf("#####################################\n\n");
    }
    else
    {
        retval = 1;
        printf("!!! PARSING ERROR !!!\n");
        printf("#####################################\n\n");
    } 
	yy_delete_buffer(buffer);

    return retval;
}

void yyerror(const char* s) {
	fprintf(stderr, "Parsing error: %s\n", s);
}