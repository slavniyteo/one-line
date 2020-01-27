# Overview

OneLine provides a simple way to organize your databases without writing dozens
of boilerplate code. It draws objects in Inspector into one line instead default
line-by-line style. Also it provides a few features like field highlightning, 
locking array size, etc...

Read about `OneLine` on habr: [1](https://habr.com/post/340536/), 
[2](https://habr.com/post/341064/) (ru).

Full Reference is [here](./Documentation~/README.md).

[![Unity Asset Store](https://img.shields.io/badge/Unity%20Asset%20Store-Free-green.svg)](https://assetstore.unity.com/packages/tools/gui/oneline-110758)
![GitHub release](https://img.shields.io/github/release/slavniyteo/one-line.svg)
![GitHub Release Date](https://img.shields.io/github/release-date/slavniyteo/one-line.svg)
![Github commits (since latest release)](https://img.shields.io/github/commits-since/slavniyteo/one-line/latest.svg)
![GitHub last commit](https://img.shields.io/github/last-commit/slavniyteo/one-line.svg)

# News

- **2019.08.20**: Added **how to build** instruction [here](#edit-oneline-project) on request [39](https://github.com/slavniyteo/one-line/issues/39).
- **2019.08.08**: [v0.5.0](https://github.com/slavniyteo/one-line/releases/tag/v0.5.0) released! Added UPM Support.
- **2019.01.15**: [v0.4.0](https://github.com/slavniyteo/one-line/releases/tag/v0.4.0) released!
- **2018.03.01**: [v0.3.0](https://github.com/slavniyteo/one-line/releases/tag/v0.3.0) released!
- **2018.02.27**: Added `[Expandable]` attribute. See [full documentation](./Documentation~/README.md) and [#22](https://github.com/slavniyteo/one-line/issues/22)
- **2018.02.26**: Added custom property drawers support.  
    Now any custom drawer, which returns height < 20, is drown. Acceptable for 
    custom drawers of either types or attributes.

- **2018.02.18**: Available on the [Asset Store](https://www.assetstore.unity3d.com/en/#!/content/110758) for free.

# TL;DR

- After importing look at `Documentation~/Examples/Overview/Overview.asset` (see the screenshot below) and open it in InspectorWindow. It'll show you all capabilities of the library;
- In your code, add `using OneLine;` and `[OneLine]` to fields you want to draw into one line. Note that internal fields don't need `[OneLine]`: they are processed automatically;
- IToo customize onelined fields, use **Width**, **Weight**, **HideLabel**, **Highlight**, **HideButtons** and **ArrayLength** attributes (see **Example.asset**);
- Use **Separator** attribute to separate different fields;
- Use **Expandable** attribute to follow object references (real street magic is here) (idea is stolen from [here](https://forum.unity.com/threads/editor-tool-better-scriptableobject-inspector-editing.484393/);
- OneLine uses [RectEx](https://github.com/slavniyteo/rect-ex), so feel free to try it.
- Compatible with either **.Net 2.0** or **.Net 4.5** backends.

![Overview](./Documentation~/mdsrc/one-line-overview.png)

Code of the screenshot above is [here](./Samples~/Scripts/Overview/Overview.cs).

# Installation

Versions prior v0.5.0 are available at [![Unity Asset Store](https://img.shields.io/badge/Unity%20Asset%20Store-Free-green.svg)](https://assetstore.unity.com/packages/tools/gui/oneline-110758).

Since v0.5.0 One Line is managed via [Unity Package Manager](https://docs.unity3d.com/Manual/Packages.html).

To add OneLine to your Unity project, add following dependency to your 
`manifest.json` as described [here](https://docs.unity3d.com/Manual/upm-dependencies.html)
and [here](https://docs.unity3d.com/Manual/upm-git.html). Use **master** or 
any version above **v0.5.0** (including) because v0.4.0 and previous versions are
not compatible with Unity Package Manager. 

```json
{
  "dependencies": {
    "st.rect-ex": "https://github.com/slavniyteo/rect-ex.git#master",
    "st.one-line": "https://github.com/slavniyteo/one-line.git#master"
  }
}
```

To be able to run tests add these lines (actually OneLine doesn't have any tests
but RectEx has):

```json
{
  "dependencies": {
    "st.rect-ex": "https://github.com/slavniyteo/rect-ex.git#master",
    "st.one-line": "https://github.com/slavniyteo/one-line.git#master"
  },
  "testables": [
    "st.rect-ex",
    "st.one-line"
  ]
}
```

# Edit OneLine Project

Added on request [39](https://github.com/slavniyteo/one-line/issues/39).

When you add OneLine via UPM as described above, it becomes readonly. To be able to edit OneLine, you should add it to project as local dependency:

1. Clone repository to local directory:

```bash
git clone https://github.com/slavniyteo/one-line /home/user/projects/one-line
```

2. In your working project add dependency to cloned OneLine:

```json
{
  "dependencies": {
    "st.rect-ex": "https://github.com/slavniyteo/rect-ex.git#master",
    "st.one-line": "file:/home/user/projects/one-line"
  }
}
```

On Windows: 

```json
{
  "dependencies": {
    "st.rect-ex": "https://github.com/slavniyteo/rect-ex.git#master",
    "st.one-line": "file:D:/projects/one-line"
  }
}
```

Note that [RectEx](https://github.com/slavniyteo/rect-ex) is git dependency.

It's all. OneLine becomes editable at InspectorWindow when your working project is open. Besides you can edit scripts of OneLine and they'll be compiled with our project.
