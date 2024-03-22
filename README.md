# CSX Tools

Script translation tool for EntisGLS visual novel engine.

## Usage

### Extract strings

**CSXTool** works with `1.x` version. In these versions, no conststr section in script, all string literals are written in image section.

To extract strings from script, run command

```
CSXTool -a Script.csx
```

or try `-e` mode:

```
CSXTool -e Script.csx
```

Note `-e` mode only works with a few games, use `-a` mode instead if it doesn't work.

**CSXToolPlus** works with `2.x ~ 3.x` version. In these versions, all string literals are written in conststr section.

To extract strings from script, run command

```
CSXToolPlus -a Script.csx
```

or

```
CSXToolPlus -a Script.csx -v2
```

### Edit strings

Write your translation in `◆` line.

The `◇` line can be delete.

```
◇80000000◇こんにちは
◆80000000◆Hello
```

### Import strings

To import edited strings, make sure `Script.csx` and `Script.txt` in same folder, run command

```
CSXTool -b Script.csx
```

If the strings is extracted by using CSXToolPlus, run command

```
CSXToolPlus -b Script.csx
```

or

```
CSXToolPlus -b Script.csx -v2
```

You can get the new script that named with `Script.new.csx`.

## Note

This repository does not accept any issues about modifying the engine, please do not submit such issues.
