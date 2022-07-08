using System.Text.RegularExpressions;
using System;

public static class TextAssetHelper 
{
    public static String[] GetRowsFromText(String text)
    {
        return Regex.Split(text, "\r\n|\r|\n");
    }
}