using System;
using System.Collections.Generic;

public class MenuNode
{
    public string Title = "";
    public string Address = "";
    public bool IsExit = false;
    public List<MenuNode> Children = new List<MenuNode>();
    public Action? Function;
    public bool IsLeaf => Children.Count == 0;
}