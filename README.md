# LPSoft.NameGenerator
Package for generating names.

## Usage

### Name Generator Builder

The Name Generator Builder provides a way to build up the Name Generator class.

Currently there are two methods to add to the resulting Name Generator:

`FromJsonFile` - Will load the provided json filepath into the internal state. The file must end in .json for the path to be valid (This may change in future updates).

Example json format:
```
{
    "key1":[
        "Item 1",
        "Item 2",
        "Item 3"
    ],
    "key2": [
        "Another Item 1",
        "Another Item 2",
        "Another Item 3"
    ],
    ...
}
```

`FromDictionary` - Will take the provided `Dictionary<string, string[]>` and load that into the internal state.

`Build` - Will make a new `NameGenerator` with the data provided from any `FromJsonFile` and `FromDictionary` calls.

### Name Generator

The Name Generator provides entry points to getting random random values based on the provided keys.

`Get` - Gets a random value using the provided key if the key exists.

`GetMultiple` - Gets random values from the provided keys in order of the keys provided.

`AvailableData` - Direct readonly access to the data within the generator in case you need to get a specific bits of data.

## Future Plans

- Load from more formats/locations
    - Xml
    - Yaml
    - Internet?