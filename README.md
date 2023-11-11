# GraphAlgorithms
Collection of graph algorithms.
## Prerequisites
Run `dotnet --version` to make sure that you have .NET 7 installed.
If not, please refer to [the official installation site](https://dotnet.microsoft.com/en-us/download) for the instructions.

## Building & running the application (Windows)
To build the application, run 
```shell
dotnet build GraphAlgorithms --configuration Release
```
Navigate to the build directory located at `GraphAlgorithms\bin\Release\net7.0`.
If using Windows Command Prompt run the following:
```shell
cd GraphAlgorithms\bin\Release\net7.0
```
The executable name is `GraphAlgorithms.exe`.
Further information about using the program can be obtained by running it with `--help` option:
```shell
.\GraphAlgorithms.exe --help
```
The program can run different commands, for example, `max-clique`.
To get detailed information about each command, run the command with `--help` option, in this example:
```shell
.\GraphAlgorithms.exe max-clique --help
```
This will give the list of all available options for a given command.
Please notice that some options are mandatory and marked as `REQUIRED`.
## DIMACS benchmark set
The maximum clique-finding algorithm can be tested on the instances from the [DIMACS benchmark set](https://iridia.ulb.ac.be/~fmascia/maximum_clique/DIMACS-benchmark).
For example, to run the heuristic algorithm on the first graph instance listed there, `C125.9`, run 
```shell
.\GraphAlgorithms.exe dimacs --name C125.9 --type heuristic
```
This will download a file with the test data to a newly created directory `GraphAlgorithms\Datasets`.
Because the graphs available in the benchmark set are big, it's not recommended to use them for testing the exact algorithm.
