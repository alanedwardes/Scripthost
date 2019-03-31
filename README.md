# Scripthost
A very light console app to allow executing arbitrary C# scripts as text.

## Usage
This tool needs to be published before it can be used, since it relies in several System* packages which only get copied to the output directory on a publish.

```
./scripthost --script "C:\My\Path\To\Script.cs" --type "AssemblyName.ClassName" --method "StaticEntryMethod"
```
