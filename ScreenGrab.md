

# Introduction #

Takes a 'Screen Grab' of the [GEWebBrowser](GEWebBrowser.md) control

## Syntax ##

```
public Bitmap ScreenGrab()
```


## Examples ##

This example takes a screen grab and then prompts the user to save the file in jpeg format.

```
const string filter = "JPEG files (*.jpg;*.jpeg)|*.jpg;*.jpeg|All files (*.*)|*.*";

// Take a 'screen grab' of the plug-in and ask the user to save.
using (Bitmap image = this.browser.ScreenGrab())
{
    using (SaveFileDialog dialog = new SaveFileDialog {Filter = filter})
    {
        if (dialog.ShowDialog() == DialogResult.OK)
        {
            image.Save(dialog.FileName, ImageFormat.Jpeg);
        }
    }
}
```


## Remarks ##

This is basically the functionality used by the [GEToolStrip](GEToolStrip.md) Screen Grab button

&lt;wiki:gadget url="http://earthapi.googlepages.com/ad.xml" border="0" width="728" height="90" /&gt;