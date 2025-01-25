// Copyright 2025 .start <https://dotstart.tv>
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
//     http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
using UdonSharp;

namespace VRCTools.Event.Samples {
  [UdonBehaviourSyncMode(BehaviourSyncMode.None)]
  public sealed class ExampleEventEmitter : AbstractEventEmitter {
    public const int EVENT_ONE = 0;
    public const int EVENT_TWO = 1;
    public const int EVENT_COUNT = 2;

    public override int EventCount => EVENT_COUNT;

    public void _TriggerEventOne() { this._EmitEvent(EVENT_ONE); }

    public void _TriggerEventTwo() { this._EmitEvent(EVENT_TWO); }
  }
}
