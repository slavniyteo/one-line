# Overview

OneLine improves your databases and makes it more readable for people. It draws 
objects in Inspector into one line instead default line-by-line style. Also it 
provides a few features like fields highlightning, locking array size, etc...

# TL;DR

    - After importing look at Assets/Example/Example.asset and open it in 
InspectorWindow. It will show you all capabilities of OneLine library.

    - In your code, add using OneLine; and add [OneLine] to fields you want to draw 
into one line. Note that internal fields don't need OneLine: they will processed 
automatically.

    - If you want to customize onelined fields, use [Width], [Weight], [HideLabel], 
[Highlight], [HideButtons] and [ArrayLength] attributes (see Example.asset).

    - To separate lines use [Separator] instead built-in [Header]. [Separator] is 
just nice-looked [Header].

# Contributing

For more information look for https://github.com/slavniyteo/one-line 

