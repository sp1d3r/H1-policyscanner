# H1-policyscanner
This is Visual Studio project (C# language). You can find compiled binaries in the Release folder. Yeah, i agree that it is time to learn Python:)

## Purpose
I wrote the tool for the tracking policies changes of the HackerOne public and external programs.
The tool uses comparsion method, based on the policies change date, and md5 comparsion between policies versions. It also generates XML files in the executable folder. It also saves scopes of the programs to the .txt files.

## Code
I wrote this quick enough, so there are many funny and not optimized places in the code. Feel free to modify project as you like.
Tool works slowly in consistent mode (we do not want to harm HackerOne servers by multiple parallel requests)

## How to use
In the first scan, tool will grab all existing policies and save them info to the XML files. All next scans will show only changed policies. Scopes will be written in the .txt file.


## Troubleshooting
The project uses Newtonsoft.Json .NET library, so if you want to modify the project, you should re-reference this library in the VS Project (existing reference can be outdated). You can find the .dll in the Release folder.
