# H1-policyscanner
This is Visual Studio project (C# language). You can find compiled binaries in the Release folder. Yeah, i agree that it is time to learn Python:)

## Purpose
I wrote the tool for the tracking plocicies changes of the HackerOne public and external programs.
The tool uses comparsion method, based on the policies change date, and md5 comparsion between policies versions. It also generates XML files in the executable folder.

## Code
I wrote this quick enough, so there are many funny and not optimized places in the code. Feel free to modify project as you like.

## How to use
In the first scan, tool will grab all existing policies and save the haches to the XML files. All next scans will show only changed policies.
