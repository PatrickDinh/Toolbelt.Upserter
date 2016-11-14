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
By default Upserter uses `int` as type for the identifier
```
var simpleUpserter = new Upserter<SimpleEntity>
``` 
You can define a different identifier type as well
```
// my SimpleEntity has Guid Id.
var simpleUpserter = new Upserter<SimpleEntity, Guid>
```
Upserter will need:
- A Func to get identifiers. The results must be unique for each object.
```
var simpleUpserter = new Upserter<SimpleEntity, Guid>(entity => entity.Id,
```
- A Func to add new objects.
```
int addEntities(T[] entities) 
{
    var numberOfEntitiesAdded = yourFavouriteRepo.AddRange(entities);
    return numberOfEntitiesAdded;
}

var simpleUpserter = new Upserter<SimpleEntity, Guid>(entity => entity.Id,
                                                      entities => addEntities(entities),
```
- A Func to update objects.
```
int udpateEntities(UpdateRequest<T>[] updateRequests) 
{
    // each updateRequest contains the old entity and the new entity
    foreach(var updateRequest in updateRequests) 
    {
        updateRequest.OldEntity.SetNewName(updateRequest.NewEntity.Name);
    }
    return numberOfEntitiesUpdated;
}

var simpleUpserter = new Upserter<SimpleEntity, Guid>(entity => entity.Id,
                                                      entities => addEntities(entities),
                                                      updateRequests => udpateEntities(updateRequests),
```
- A Func to delete obsoleted objects.
```
int deleteEntities(T[] entities) 
{
    var numberOfEntitiesDeleted = yourFavouriteRepo.RemoveAll(entities);
    return numberOfEntitiesDeleted;
}

var simpleUpserter = new Upserter<SimpleEntity, Guid>(entity => entity.Id,
                                                      addEntities => addEntities(entities),
                                                      updateRequests => udpateEntities(updateRequests),
                                                      deletedEntities => deletedEntities(entities));
```
That's it, now you have a ready to use Upserter.