# Protocol Builder
Protocol builder can be used to define a shared protocol for use on different services and apps communications like APIs (REST,...), Apps, ...
+ Source definition language: C#
+ Supported target languages:
  + TypeScript
  + PHP 5.6+ (we have used https://github.com/cweiske/jsonmapper ❤ types for variable definitions so you can use it to json deserialize to your strong typed ones too - optional)
  + Kotlin
  + Swift

# How to test
+ Edit the model in SampleSetup\Protocol
+ Run SampleSetup\UpdateTools.bat or SampleSetup\UpdateTools.sh depending on the platform
  + This script compiles the protocol into executable from source code
+ Run SampleSetup\GenerateAll.bat or SampleSetup\GenerateAll.sh depending on the platform
  + This script runs the Protocol Builder for all the languages and applies it to the model in SampleSetup\Protocol
+ Check the result in SampleSetup\Output
+ Make sure only necessary changes were added to SampleSetup\Output

# Credits
This project at early stages was a fork from https://github.com/matthewsot/SharpSwift ❤
