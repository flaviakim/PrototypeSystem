# Changelog

All notable changes to this project will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.1.0/),
and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

## [0.2.1]

### Changed

 - Leave IInstance implementations the freedom of how to initialize everything.
 - Decouple IInstance from IInitializationData.

## [0.2.0]

### Changed

 - Extensive refactoring, breaking most things.
 - Added InitializationData to easily create new IInstance objects in IInstanceFactory's.
 - Extracted InstanceFactory into an interface.
 - Completely reworked DynamicSavingLoading
 - DynamicAssetManager now access point for the different Interfaces to save and load.

## [0.1.1]

### Added

 - Add public PrototypeData reference to IInstance interface.

## [0.1.0]

### Added

 - Reformatting of codebase according to Unity Package Manager guidelines.
 - Initial release of the project.
