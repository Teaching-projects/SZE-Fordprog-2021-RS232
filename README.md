# SZE-Fordprog-2021-RS232
Banfi Jozsef

## Description

RS232 data stream parser with Flex and Bison.
The grammar was specificly created to parse a given format of serial port data, can be used for differenet measuring instruments.
The parser always looking for a +/- sign to get the measurement data.

## Getting Started

### Dependencies

* Flex and Bison
    * included in the tools folder

### Building the app

* Compile the Flex and Bision files (tokens.l and grammar.y)
* Build the FlexBisonWrapper into a .dll
    * this always raises an error in the tokens.c file, the Flex compiles with a deprecated function "fileno", change the function name to "_fileno"
* Build the main c# app RS232_PARSER

### Executing program

* Run the application from VS or from the Debug/Release folder

### Examples what the grammar can parse
* "+92.45"
* "-12.5"
* "+123"
* "12";"-34.56"
* "-34.56";"+143.71";"05/11/2021 11:08:31 AM";"+765"
* "True";"Tension";"05/11/2021 11:08:31 AM";"Default Administrator";"+143.71"
*" true";"2";"08/03/2021 15:52:36";"4";"+159";"tension";"4.0.1.184" 

## Authors

[@Jostee](https://github.com/jostee)

## Version History

* 1.0
    * Initial Release

## TODO

* Fix the the issue with the Flex compiled c file 

## Acknowledgments

Inspiration, code snippets, etc.
* [Mate Hegyhati](https://github.com/hegyhati)
* [awesome-readme](https://github.com/matiassingers/awesome-readme)