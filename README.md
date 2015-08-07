# ClipboardDotNet

[ClipboardDotNet](https://github.com/vurdalakov/clipboarddotnet) is a .NET library that works with [Windows Clipboard](https://msdn.microsoft.com/en-us/library/windows/desktop/ms648709.aspx) and .CLP files.

Project is distributed under the [MIT license](http://opensource.org/licenses/MIT).

Copyright © 2015 [Vurdalakov](http://www.vurdalakov.net).

Command-line tools based on this library:
  * [clip](https://github.com/vurdalakov/dostools/wiki/clip) - works with Windows Clipboard
  * [clp](https://github.com/vurdalakov/dostools/wiki/clp) - saves, restores and parses Windows Clipboard files (.CLP)

#### Clipboard listener

```csharp
    var clipboardListener = new ClipboardListener();
    clipboardListener.ClipboardUpdated += (s, e) => Refresh();
```

#### CLP file methods

##### Parse CLP file

```csharp
    var entries = ClipboardFile.Parse(@"c:\temp\clipboard.clp");

    foreach (var entry in entries)
    {
        Console.WriteLine("{0} {1} {2}", entry.Format, entry.DataSize, entry.Name);
    }
```

##### Extract text from CLP file

```csharp
    var text = ClipboardFile.GetText(@"c:\temp\clipboard.clp");
    if (text != null)
    {
        Console.WriteLine("{0}", text);
    }
```

##### Restore Clipboard content from CLP file

```csharp
    ClipboardFile.Restore(@"c:\temp\clipboard.clp");
```

##### Save Clipboard content to CLP file

```csharp
    ClipboardFile.Save(@"c:\temp\clipboard.clp");
```

#### Clipboard methods

##### Empty Clipboard

```csharp
    Clipboard.Empty();
```

##### Get number of available formats

```csharp
    var count = Clipboard.CountFormats();
```

##### Check if format is  available

```csharp
    if (Clipboard.IsFormatAvailable(ClipboardFormats.CF_UNICODETEXT))
    {
        Console.WriteLine("Unicode text is available");
    }
```

##### Get available formats

```csharp
    var entries = Clipboard.GetEntries();

    foreach (var entry in entries)
    {
        Console.WriteLine("{0} {1} {2}", entry.Format, entry.DataSize, entry.Name);
    }
```

##### Get available format IDs

```csharp
    var formats = Clipboard.GetFormats();

    foreach (var format in formats)
    {
        Console.WriteLine("{0}", format);
    }
```

##### Get format name

```csharp
    if (Clipboard.CountFormats() > 0)
    {
        var format = Clipboard.GetFormats()[0];
        var name = Clipboard.GetFormatName(format);
    }
```

##### Get format data size

```csharp
    if (Clipboard.CountFormats() > 0)
    {
        var format = Clipboard.GetFormats()[0];
        var dataSize = Clipboard.GetDataSize(format);
    }
```

##### Get format data

```csharp
    if (Clipboard.CountFormats() > 0)
    {
        var format = Clipboard.GetFormats()[0];
        var data = Clipboard.GetData(format);
        File.WriteAllBytes(@"c:\temp\clipboard.bin", data);
    }
```

##### Get text from Clipboard

```csharp
    var text = Clipboard.GetText();
    Console.WriteLine("{0}", text);
```

##### Set format data

```csharp
    var data = File.ReadAllBytes(@"c:\temp\image.bmp");

    Clipboard.Empty();
    Clipboard.SetData(ClipboardFormats.CF_BITMAP, data);
```
