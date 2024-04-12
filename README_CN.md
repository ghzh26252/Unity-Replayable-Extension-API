# Unity可回放扩展API

[中文](https://github.com/ghzh26252/Unity-Replayable-Extension-API/blob/main/README_CN.md)|[English](https://github.com/ghzh26252/Unity-Replayable-Extension-API/blob/main/README.md)

通过封装Unity API，对运行时的调用进行序列化并存储，根据时间戳还原调用以实现回放。以最少量修改，实现大部分常用影响画面表现的API的回放支持。

[Web Demo](https://ghzh26252.github.io/Unity-Replayable-Extension-API/)

---

## 已实现功能及限制

1. 录制和回放预制体从实例化到销毁的完整生命周期。

2. 录制场景物体改动。

3. 随时开启和终止录制。

4. 开始录制/开始回放时将清除所创建的预制体，并还原场景物体的一些初始状态。

5. 回放可使用TimeScale实现暂停加速减速，但因为动画等不可预测，不能完美支持时间跳转。

6. 移动旋转等逐帧调用的API支持抽帧压缩，优化存储大小。最终文件也可压缩和加密。

7. 支持自定义方法调用的录制和回放。

---

## 使用方法

1. 需要录制和回放的场景物体和预制体挂载RaplayableUnit脚本，此脚本将记录物体及其子物体上的所有组件引用。**后续修改该物体后，请点击Insprector面板上的“刷新引用”，某些修改将导致之前录制的回放失效。**

2. 将ReplayableManager拖入场景中。

3. 将所有RaplayableUnit脚本的预制体和运行时用到的引用类型实例，添加进ReplayableManager/ReferenceManager(script)/references(List)中。

4. 改变运行时调用的Unity API为下文所列可回放API。

5. **更多原理和使用方法可见示例场景**

---

## API

需将以下Unity API改为对应的可回放API：

| Unity API                                                                                      | ==&gt; | 可回放API                                                                                           |
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
