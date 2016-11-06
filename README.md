# Toolbelt.Upserter [![Build status](https://ci.appveyor.com/api/projects/status/f4xhab52bp5qdpcs?svg=true)](https://ci.appveyor.com/project/PatrickDinh/toolbelt-upserter)

A simple upserter

## Reason
I'm tired of writing upsert which is a process to compare existing objects and new objects to:
- Insert new objects.
- Update entities if need to.
- Delete obsoleted objects.

## Install
```
Install-Package Toolbelt.Upserter
```

## Usage
Upserter needs:
- A Func to get identifiers. The results must be unique for each object.
- A Func to compare 2 objects with the same identifier to see if an update is needed.
- A Func to add new objects.
- A Func to update objects.
- A Func to delete obsoleted objects.
