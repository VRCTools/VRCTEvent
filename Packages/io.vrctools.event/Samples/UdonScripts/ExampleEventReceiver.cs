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
using TMPro;
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;

namespace VRCTools.Event.Samples {
  [UdonBehaviourSyncMode(BehaviourSyncMode.None)]
  [RequireComponent(typeof(TextMeshProUGUI))]
  public sealed class ExampleEventReceiver : UdonSharpBehaviour {
    public ExampleEventEmitter emitter;

    [Range(1, 2)]
    public int eventIndex = 1;

    private TextMeshProUGUI _text;
    private int _eventCount;

    private void Start() {
      if (!Utilities.IsValid(this.emitter)) {
        Debug.LogError("[ExampleEventReceiver] Invalid emitter reference - Disabled");
        this.enabled = false;
        return;
      }

      this._text = this.GetComponent<TextMeshProUGUI>();

      switch (this.eventIndex) {
        case 1:
          this.emitter._RegisterHandler(ExampleEventEmitter.EVENT_ONE, this, nameof(this._OnEvent));
          break;
        case 2:
          this.emitter._RegisterHandler(ExampleEventEmitter.EVENT_TWO, this, nameof(this._OnEvent));
          break;
        default:
          Debug.LogError("[ExampleEventReceiver] Invalid event index - Disabled");
          this.enabled = false;
          break;
      }
    }

    private void OnDestroy() {
      switch (this.eventIndex) {
        case 1:
          this.emitter._UnregisterHandler(ExampleEventEmitter.EVENT_ONE, this, nameof(this._OnEvent));
          break;
        case 2:
          this.emitter._UnregisterHandler(ExampleEventEmitter.EVENT_TWO, this, nameof(this._OnEvent));
          break;
      }
    }

    /// <summary>
    /// Handles either event one or two (depending on which one has been registered in <see cref="Start"/>.
    ///
    /// Note that this method must always be public since it will be invoked via Udon's event system. If marked private,
    /// the call will simply be ignored.
    ///
    /// Additionally, it may be advisable to prefix these methods with an underscore to prevent accidental (or malicious)
    /// invocation via the network.
    /// </summary>
    public void _OnEvent() {
      this._eventCount++;
      this._text.text = this._eventCount.ToString();
    }
  }
}