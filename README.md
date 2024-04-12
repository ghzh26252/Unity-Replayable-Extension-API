# Unity Replayable Extension API

[中文](https://github.com/ghzh26252/Unity-Replayable-Extension-API/blob/main/README_CN.md)|[English](https://github.com/ghzh26252/Unity-Replayable-Extension-API/blob/main/README.md)

This is a project that provides an extension to Unity's API to enable recording and playback of runtime calls by serializing and storing them. Playback is achieved by restoring calls based on timestamps. The goal is to support replay functionality for the majority of commonly used APIs that affect visual presentation, with minimal modifications.

[Web Demo](https://ghzh26252.github.io/Unity-Replayable-Extension-API/)

---

## Implemented Features and Limitations

1. Recording and playback of the complete lifecycle of prefabs from instantiation to destruction.
2. Recording of changes to scene objects.
3. Start and stop recording at any time.
4. When starting recording or playback, any prefabs created will be cleared, and some initial state of scene objects will be restored.
5. Playback supports using TimeScale to achieve pause, acceleration, and deceleration effects. However, due to unpredictable animations, precise time skipping is not perfectly supported.
6. APIs called frame by frame, such as movement and rotation, support frame reduction compression to optimize storage size. The final files can also be compressed and encrypted.
7. Custom method calls can be recorded and played back.

---

## Usage Instructions

1. Attach the `ReplayableUnit` script to scene objects and prefabs that need to be recorded and played back. This script records references to all components on the object and its child objects. **After making modifications to such objects, click "Refresh References" in the Inspector panel. Certain modifications may invalidate previously recorded replays.**

2. Place the `ReplayableManager` in the scene.

3. Add the prefabs and runtime reference instances used by all `ReplayableUnit` scripts to `ReplayableManager/ReferenceManager(script)/references(List)`.

4. Replace Unity API calls with the corresponding replayable APIs listed below.

5. **For more details on principles and usage, refer to the example scenes.**

---

## API

Replace the following Unity APIs with their corresponding replayable APIs:

| Unity API                                                                                      | ==&gt; | Replayable API                                                                                   |
| ---------------------------------------------------------------------------------------------- | ------ | ------------------------------------------------------------------------------------------------ |
| (Static)GameObject.Instantiate(ReplayableUnit unit)                                            | ==&gt; | (Static)ReplayableAPI.ReInstantiate(ReplayableUnit unit)                                         |
|                                                                                                |        |                                                                                                  |
| (Static)GameObject.Destroy(ReplayableUnit unit)                                                | ==&gt; | (Static)ReplayableAPI.ReDestroy(ReplayableUnit unit)                                             |
|                                                                                                |        |                                                                                                  |
| GameObject.SetActive(bool value)                                                               | ==&gt; | GameObject.ReActive(bool value)                                                                  |
|                                                                                                |        |                                                                                                  |
| Transform.SetParent(Transform transform)                                                       | ==&gt; | Transform.ReParent(Transform transform)                                                          |
| (Property)Transform.parent = Transform transform                                               | ==&gt; | Transform.ReParent(Transform transform)                                                          |
| (Property)Transform.positon = Vector3 value                                                    | ==&gt; | Transform.RePosition(Vector3 value)                                                              |
| (Property)Transform.localPositon = Vector3 value                                               | ==&gt; | Transform.ReLoaclPosition(Vector3 value)                                                         |
| (Property)Transform.eulerAngles = Vector3 value                                                | ==&gt; | Transform.ReRotation(Vector3 value)                                                              |
| (Property)Transform.localeulerAngles = Vector3 value                                           | ==&gt; | Transform.ReLoaclRotation(Vector3 value)                                                         |
| (Property)Transform.localScale = Vector3 value                                                 | ==&gt; | Transform.ReScale(Vector3 value)                                                                 |
|                                                                                                |        |                                                                                                  |
| Renderer.material.SetFloat(string name, float value)                                           | ==&gt; | Renderer.ReFloat(string name, float value)                                                       |
| Renderer.material\[index\].SetFloat(string name, float value)                                  | ==&gt; | Renderer.ReFloat(string name, float value, int index)                                            |
| Renderer.material.SetColor(string name, Color value)                                           | ==&gt; | Renderer.ReColor(string name, Color value)                                                       |
| Renderer.material\[index\].SetColor(string name, Color value)                                  | ==&gt; | Renderer.ReColor(string name, Color value, int index)                                            |
| Renderer.material.SetVector(string name, Vector4 value)                                        | ==&gt; | Renderer.ReVector(string name, Vector4 value)                                                    |
| Renderer.material\[index\].SetVector(string name, Vector4 value)                               | ==&gt; | Renderer.ReVector(string name, Vector4 value, int index)                                         |
| Renderer.material.SetTexture(string name, Texture value)                                       | ==&gt; | \~\~Renderer.ReTexture(string name, Texture value)                                               |
| Renderer.material\[index\].SetTexture(string name, Texture value)                              | ==&gt; | \~\~Renderer.ReTexture(string name, Texture value, int index)                                    |
|                                                                                                |        |                                                                                                  |
| Animator.SetInteger(int id, int value)                                                         | ==&gt; | Animator.ReInteger(int id, int value)                                                            |
| Animator.SetInteger(string name, int value)                                                    | ==&gt; | Animator.ReInteger(string name, int value)                                                       |
| Animator.SetFloat(int id, float value)                                                         | ==&gt; | Animator.ReFloat(int id, float value)                                                            |
| Animator.SetFloat(string name, float value)                                                    | ==&gt; | Animator.ReFloat(string name, float value)                                                       |
| Animator.SetBool(int id, bool value)                                                           | ==&gt; | Animator.ReBool(int id, bool value)                                                              |
| Animator.SetBool(string name, bool value)                                                      | ==&gt; | Animator.ReBool(string name, bool value)                                                         |
| Animator.SetTrigger(int id)                                                                    | ==&gt; | Animator.ReTrigger(int id)                                                                       |
| Animator.SetTrigger(string name)                                                               | ==&gt; | Animator.ReTrigger(string name)                                                                  |
| Animator.ReSetTrigger(int id)                                                                  | ==&gt; | Animator.ReReSetTrigger(int id)                                                                  |
| Animator.ReSetTrigger(string name)                                                             | ==&gt; | Animator.ReReSetTrigger(string name)                                                             |
| Animator.Play(string stateName, int layer = -1, float normalizedTime = float.NegativeInfinity) | ==&gt; | Animator.RePlay(string stateName, int layer = -1, float normalizedTime = float.NegativeInfinity) |
| (Property)Animator.speed = float value                                                         | ==&gt; | Animator.ReSpeed(float value)                                                                    |
|                                                                                                |        |                                                                                                  |
| Animation.Play(string animation = null, PlayMode mode = PlayMode.StopSameLayer)                | ==&gt; | Animation.RePlay(string animation = null, PlayMode mode = PlayMode.StopSameLayer)                |
| Animation.Stop(string animation = null)                                                        | ==&gt; | Animation.ReStop(string animation = null)                                                        |
| Animation.Rewind(string animation = null)                                                      | ==&gt; | Animation.ReRewind(string animation = null)                                                      |
