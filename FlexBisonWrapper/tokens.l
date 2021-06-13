%{
	#include "grammar.tab.h"

	extern int atoi(char i[]);
	extern double atof(char s[]);
	extern char *strdup(const char *s);
%}

%option noyywrap

DIGITS [0-9]
ALPHABET [a-zA-Z]
CHARACTERTOKENS [ \+\-\"\;]
EXP ([Ee][-+]?[0-9]+)

 
%%
{DIGITS}*							{ printf("LOOKAHEAD: INTEGER\n"); yylval.ival = atoi(yytext); return INTEGER; }
{DIGITS}+"."{DIGITS}*{EXP}? |
"."?{DIGITS}+{EXP}? 				{ printf("LOOKAHEAD: DOUBLE\n"); yylval.dval = atof(yytext); return DOUBLE; }
{ALPHABET}({ALPHABET}|{DIGITS})*	{ printf("LOOKAHEAD: TEXT\n"); yylval.sval = strdup(yytext); return TEXT; }
[\/\:]	 							{ printf("LOOKAHEAD: DATE_SPECIAL\n"); return DATE_SPECIAL; }
{CHARACTERTOKENS}                   { printf("LOOKAHEAD: \'%c\'\n",*yytext); return *yytext; }
\n									return '\0'; 
%%  



