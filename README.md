# VRCTools Event System

This package provides a very simple event system for Udon# scripts letting your scripts publish or receive updates
from other components within a scene.

Handlers may be (un-)registered dynamically as state becomes relevant or objects are created/destroyed.

**Important:** This package on its own is useless. If you do not intend to write your own scripts using this library,
you likely don't need to install it yourself. Some VRCTools or third party packages may depend on it. If you are
installing via Creator Companion or an equivalent package manager, it should be installed automatically when
needed. If you are installing packages manually, refer to the documentation of the package or asset you are trying
to install.

## Installation

The preferred way for installation is to use Creator Companion or an equivalent package management tool. Our
repository can be found at https://vrctools.github.io/vpm and will guide you through adding the repository to
your package manager.

If you wish to install the package manually, you can find the most recent build over in the [Releases][Releases]
section. Simply drag the package onto Unity or select the file manually
via `Assets -> Import Package -> Custom Package`

## Usage

To create an event emitter (e.g. an Udon# script which generates an arbitrary number of events which others may
listen to), you need to extend `AbstractEventEmitter` from the `VRCTools.Event` namespace:

```C#
import VRCTools.Event;

class MyEventEmitter : AbstractEventEmitter {
   public const int EVENT_A = 0;
   public const int EVENT_B = 1;
   public const int EVENT_COUNT = 2;

   public override int EventCount => EVENT_COUNT;

   // ...

   // for instance, if you want to emit an event whenever a player enters a trigger volume
   public override void OnPlayerTriggerEnter(VRCPlayerApi player) {
      this._EmitEvent(EVENT_A);
   }
}
```

In order to subscribe to an event, you need to acquire a reference to the emitter and register yourself:

```C#
class MyReceiver : UdonSharpBehaviour {
   public MyEventEmitter emitter;

   private void Start() {
      this.emitter._RegisterHandler(MyEventEmitter.EVENT_A, this, nameof(this._OnEventA));
   }

   // recommended to implement but technically purely optional if your object never gets destroyed
   private void OnDestroy() {
      this.emitter._UnregisterHandler(ExampleEventEmitter.EVENT_ONE, this, nameof(this._OnEvent));
      // or alternatively
      this.emitter._UnregisterHandler(this);
   }

   // Important: Handler functions have to be declared as public in order to be invoked
   public void _OnEventA() {
      // do something
   }
}
```

Please note that these are heavily simplified usage examples.
A set of full example scripts observing best practice may be found in `Packages/io.vrctools.event/Samples` (or
`Packages/VRCTools - Event System/Samples` via the Unity Editor UI).

Please also note that all event emitters are implicitly implementations of `UdonSharpBehaviour` at the moment.
This is due to limitations on the types and abstractions that Udon# can compile.

## License

This project is released under the terms of the [Apache License, Version 2.0][License]. A copy of the full
license text is included within the repository as well as all builds.

```
Licensed under the Apache License, Version 2.0 (the "License");
you may not use this file except in compliance with the License.
You may obtain a copy of the License at

   http://www.apache.org/licenses/LICENSE-2.0

Unless required by applicable law or agreed to in writing, software
distributed under the License is distributed on an "AS IS" BASIS,
WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
See the License for the specific language governing permissions and
limitations under the License.
```

[Releases]: https://github.com/VRCTools/VRCTEvent/releases
[License]: [https://www.apache.org/licenses/LICENSE-2.0.txt]