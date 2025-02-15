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
using System;
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;

namespace VRCTools.Event {
  
  /// <summary>
  /// Provides a basis for Udon behaviours which emit a set of custom events to a list of previously subscribed handler
  /// functions.
  /// </summary>
  public abstract class AbstractEventEmitter : UdonSharpBehaviour {
    private bool _handlersInitialized;
    private UdonSharpBehaviour[][] _handlers;
    private string[][] _eventNames;

    /// <summary>
    /// Retrieves the total number of events emitted by this emitter implementation.
    /// </summary>
    public abstract int EventCount { get; }

    /// <summary>
    /// Identifies the depth of handler updates within the context of this emitter.
    ///
    /// When no update is in progress, zero is returned.
    /// When an update is in progress, a positive number is returned.
    ///
    /// This value is used internally to ensure that handlers aren't (de-)registered while updates are still ongoing
    /// thus potentially preventing the update from completing properly.
    /// </summary>
    protected int EventStackIndex { get; private set; }

    /// <summary>
    /// Identifies whether this emitter is currently updating its handlers for one of its events.
    /// </summary>
    public bool IsUpdatingHandlers => this.EventStackIndex > 0;

    /// <summary>
    /// Handles the initialization of the handler lists.
    /// </summary>
    private void _InitializeHandlers() {
      if (this._handlersInitialized) return;

      this._handlers = new UdonSharpBehaviour[this.EventCount][];
      this._eventNames = new string[this.EventCount][];

      for (var i = 0; i < this.EventCount; i++) {
        this._handlers[i] = new UdonSharpBehaviour[0];
        this._eventNames[i] = new string[0];
      }

      this._handlersInitialized = true;
    }

    /// <summary>
    /// Registers a new event handler for the given eventId.
    /// </summary>
    /// <param name="eventId">an event identifier as specified by the emitter implementation</param>
    /// <param name="handler">an arbitrary Udon object which shall receive updates</param>
    /// <param name="eventName">an arbitrary event method which shall receive updates</param>
    public void _RegisterHandler(int eventId, UdonSharpBehaviour handler, string eventName) {
      this._InitializeHandlers();
      this._CleanupHandlers();

      if (!Utilities.IsValid(handler)) {
        Debug.LogError($"[Event Emitter] Attempted to register invalid handler with event slot {eventId}", this);
        return;
      }

      if (eventId < 0 || eventId >= this.EventCount) {
        Debug.LogError(
          $"[Event Emitter] Attempted to register invalid event slot {eventId} with handler {handler.name}#{eventName}",
          handler);
        return;
      }

      if (string.IsNullOrEmpty(eventName)) {
        Debug.LogError(
          $"[Event Emitter] Attempted to register invalid handler for event slot {eventId} with handler {handler.name}",
          handler);
        return;
      }

      if (this.EventStackIndex > 0) {
        Debug.LogError(
          $"[Event Emitter] Attempted to register handler {handler.name}#{eventName} for event slot {eventId} while event handler update is in progress",
          handler);
        return;
      }

      var handlerList = this._handlers[eventId];
      var eventNames = this._eventNames[eventId];

      this._handlers[eventId] = ArrayUtils.AddElement(handlerList, handler);
      this._eventNames[eventId] = ArrayUtils.AddElement(eventNames, eventName);
    }

    /// <summary>
    /// Removes any leftover invalid handlers within the handler list.
    /// </summary>
    protected void _CleanupHandlers() {
      // if the handler lists have not been initialized, there is nothing to be done as no handlers have been registered
      // within this emitter yet
      if (!this._handlersInitialized) {
        return;
      }

      for (var i = 0; i < this._handlers.Length; i++) {
        var handlers = this._handlers[i];

        var removed = 0;
        for (var j = 0; j < handlers.Length; j++) {
          var handler = handlers[j];
          if (Utilities.IsValid(handler)) {
            handlers[j - removed] = handler;
            continue;
          }

          removed++;
        }

        if (removed < 0) {
          continue;
        }

        var copy = new UdonSharpBehaviour[handlers.Length - removed];
        Array.Copy(handlers, copy, copy.Length);

        this._handlers[i] = copy;
      }
    }

    /// <summary>
    /// Removes an existing event handler from all subscribed events.
    /// </summary>
    /// <param name="handler">a handler instance</param>
    public void _UnregisterHandler(UdonSharpBehaviour handler) {
      // deliberately skipping full validity check as the object may have gone out of scope but should still be removed
      // from our handler list
      if (handler == null) {
        Debug.LogError("[Event Emitter] Attempted to unregister invalid handler from all event slots", this);
        return;
      }

      for (var i = 0; i < this.EventCount; ++i) {
        var handlers = this._handlers[i];
        var eventNames = this._eventNames[i];

        var handlersCopy = new UdonSharpBehaviour[handlers.Length];
        var eventNamesCopy = new string[handlers.Length];

        var removed = 0;
        for (var j = 0; j < handlers.Length; ++j) {
          var existing = handlers[j];
          var eventName = eventNames[j];
          if (existing != handler) {
            handlersCopy[j - removed] = existing;
            eventNamesCopy[j - removed] = eventName;
            continue;
          }

          removed++;
        }

        if (removed == 0) {
          continue;
        }

        this._handlers[i] = handlersCopy;
        this._eventNames[i] = eventNamesCopy;
      }
    }

    /// <summary>
    /// Removes an existing event handler for the given eventId.
    /// </summary>
    /// <param name="eventId"></param>
    /// <param name="handler"></param>
    /// <param name="eventName"></param>
    public void _UnregisterHandler(int eventId, UdonSharpBehaviour handler, string eventName) {
      // if the handler lists have not been initialized, there is nothing to be done as no handlers have been registered
      // within this emitter yet
      if (!this._handlersInitialized) {
        return;
      }

      this._CleanupHandlers();

      // deliberately skipping full validity check as the object may have gone out of scope but should still be removed
      // from our handler list
      if (handler == null) {
        Debug.LogError($"[Event Emitter] Attempted to unregister invalid handler with event slot {eventId}", this);
        return;
      }

      if (eventId < 0 || eventId >= this.EventCount) {
        Debug.LogError(
          $"[Event Emitter] Attempted to unregister invalid event slot {eventId} with handler {handler.name}#{eventName}",
          handler);
        return;
      }

      if (string.IsNullOrEmpty(eventName)) {
        Debug.LogError(
          $"[Event Emitter] Attempted to unregister invalid handler for event slot {eventId} with handler {handler.name}",
          handler);
        return;
      }

      var handlerList = this._handlers[eventId];
      var eventNames = this._eventNames[eventId];

      // handlers may appear within the handler list multiple times thus we'll have to locate a handler that matches
      // in combination with its event name
      var found = false;
      var offset = -1;
      do {
        var location = ArrayUtils.FindElement(handlerList, handler, offset + 1);

        // if no matching handler is present within the array at all, abort early since there is nothing for us to do
        // here
        if (location == -1) {
          return;
        }

        // ensure that the offset is stored for removal or the next iteration where we try to find another instance of
        // the handler
        offset = location;

        // if the event name matches, consider the entry found and break for removal
        if (eventNames[location] == eventName) {
          found = true;
          break;
        }
      } while (offset < eventNames.Length);

      // if we found nothing, there is nothing left for us to do but abort
      if (!found) {
        return;
      }

      this._handlers[eventId] = ArrayUtils.RemoveElement(handlerList, offset);
      this._eventNames[eventId] = ArrayUtils.RemoveElement(eventNames, offset);
    }

    /// <summary>
    /// Emits an event for the given event id.
    /// </summary>
    /// <param name="eventId">an event identifier </param>
    protected void _EmitEvent(int eventId) {
      // if the handler lists have not been initialized, there is nothing to be done as no handlers have been registered
      // within this emitter yet
      if (!this._handlersInitialized) {
        return;
      }

      if (eventId < 0 || eventId >= this.EventCount) {
        Debug.LogError($"[Event Emitter] Attempted to emit event with invalid id {eventId}", this);
        return;
      }

      this.EventStackIndex++;
      {
        var handlerList = this._handlers[eventId];
        var eventNames = this._eventNames[eventId];

        for (var i = 0; i < handlerList.Length; i++) {
          var handler = handlerList[i];
          var eventName = eventNames[i];

          if (!Utilities.IsValid(handler)) {
            Debug.LogWarning(
              $"[Event Emitter] Stale reference to event handler {handler.name}#{eventName} for event {eventId} - Skipped",
              this);
            continue;
          }

          handler.SendCustomEvent(eventName);
        }
      }
      this.EventStackIndex--;
    }
  }
}
