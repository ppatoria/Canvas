cd "C:\Users\pp\Documents\Visual Studio 2015\Projects\Canvas\RunTests"
cls
cp "C:\Users\pp\Documents\Visual Studio 2015\Projects\Canvas\CanvasUnitTests\bin\Debug\*.*"
OpenCover.Console.exe -target:RunTests.bat -register:user -filter:+[Canvas]*
ReportGenerator.exe -reports:results.xml -targetdir:coverage
ii coverage\index.htm