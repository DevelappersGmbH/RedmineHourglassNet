# Changelog
All notable changes to this project will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.0.0/),
and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

## [Unreleased]
### Added
- nothing

### Changed
- nothing

### Removed
- nothing

### Fixed
- nothing

## [1.0.0] - 2019-04-01
### Added
- time trackers: implemented filter for lists (breaking change!)
- time bookings: implemented filter for lists (breaking change!)
- time logs: implemented filter for lists (breaking change!)

### Changed
- nothing

### Removed
- nothing

### Fixed
- nothing

## [0.4.0] - 2018-11-26
### Added
- error handling for bad request

### Changed
- nothing

### Removed
- nothing

### Fixed
- nothing

## [0.3.0] - 2018-11-23
### Added
- time trackers: implemented bulk operations
- time bookings: implemented bulk operations
- time logs: implemented bulk operations

### Changed
- renamed properties for services in HourglassClient to be more convinient

### Removed
- nothing

### Fixed
- nothing

## [0.2.0] - 2018-11-23
### Added
- Contribution guidelines, code of conduct and changelog
- time trackers: all missing operations (except bulk operations)
- time bookings: all missing operations (except bulk operations)
- time logs: all missing operations (except bulk operations)
- support for logging frameworks

### Changed
- renamed all service functions because "ById" is implicit clear
- Get throws NotFoundException instead of returning null when item was not found 


## [0.0.2] - 2018-11-21
### Added
- basic implementation to read trackers and logs

### Changed
- compatibility to .NET Standard 2.0 as the web request stuff in .NET Standard 1.X doesn't allow to eveluate the response status code without parsing strings.
- naming of service classes - there will be no guaranteed compatibility to previous versions before release of version 1.0.0

## [0.0.1] - 2018-11-19
### Added
- basic implementation to read timebookings.
- based on .NET Standard 1.4 to provide high compatibility.