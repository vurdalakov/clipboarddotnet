# ClipboardDotNet

`ClipboardDotNet` is a .NET library that works with Windows Clipboard and CLP files.

Project is distributed under the [MIT license](http://opensource.org/licenses/MIT).

Copyright © 2015 [Vurdalakov](http://www.vurdalakov.net).

##### CLP file methods

###### Parse CLP file

```
    var entries = ClipboardFile.Parse(@"c:\temp\clipboard.clp");

    foreach (var entry in entries)
    {
        Console.WriteLine("{0} {1} {2}", entry.Format, entry.DataSize, entry.Name);
    }
```

###### Extract text from CLP file

```
    var text = ClipboardFile.GetText(@"c:\temp\clipboard.clp");
    if (text != null)
    {
        Console.WriteLine("{0}", text);
    }
```

###### Restore Clipboard from CLP file

```
    ClipboardFile.Restore(@"c:\temp\clipboard.clp");
```

###### Save Clipboard content to CLP file

```
    ClipboardFile.Save(@"c:\temp\clipboard.clp");
```
