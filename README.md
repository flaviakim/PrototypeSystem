# Prototype System

A Prototype System to easily load dynamic content from files.

[//]: # (## Overview)

## Installation Instructions

Follow the instructions in the [Unity documentation to install a package from a Git URL](https://docs.unity3d.com/6000.1/Documentation/Manual/upm-ui-giturl.html)

## Workflows

### 1. Create an Instance Class 
IInstance<IPrototypeData> is the class to create instances of, based on the prototype data.

Make your desired class inherit from this interface or the provided base classes like Instance or MonoInstance (useful with ScriptableObjectPrototypeData).

### 2. Create a PrototypeData Class
IPrototypeData contains all the data, which stays the same for all instances from a given prototype.

This could be max health for Enemy Instances, Size for buildings, etc.

Create an object that inherits from this, or use the provided base classes like BasePrototypeData or ScriptableObjectPrototypeData.

### 3. Create the Initialization Data Class
Create a class inheriting from IInitializationData storing all additional data needed to create an instance from a prototype.

This is data that is individual, but unlike for example health, which could be directly derived from the prototype data's max health, also has individual start values.

This could be the position where a building is built.

When enemies should be able to spawn with less than 100% health, then health would also be in IInitializationData instead of it being derived. 

When no initialization data is needed, you can use the IInitializationData.EmptyInitializationData class.

### 4. Create a Factory Class
Create a Factory class inheriting from IInstanceFactory<IInstance<IPrototypeData>, IPrototypeData, IInitializationData>.

As its generic values, pass in the three previously created classes. Then write the method to create a single instance from the prototype data and the initialization data.

Use an appropriate Prototype Collection depending on where you want to load the prototype info from. If the prototype info is stored in Json files, JsonPrototypeCollection could be suitable, while ScriptableObjectPrototypeCollection could be suitable for ScriptableObjectPrototypeData.

You can also inherit from a Base class like BaseInstanceFactory or MonoInstanceFactory. When using ScriptableObjectPrototypeData and MonoInstance, the MonoInstanceFactory can be used directly and no new class needs to be created. Individual creation logic can be done in MonoInstance.Initialize(TPrototypeData prototypeData, TInitializationData initializationData).

### To Keep in Mind
- When directly inheriting from the interfaces instead of the base classes, make sure to not forget to set the PrototypeData in the Instance, either in the Instance constructor or in the Factory.

## Samples

Are in the works. Simple samples can be found in the test folder under Tests/Runtime/PrototypeSystem. 
