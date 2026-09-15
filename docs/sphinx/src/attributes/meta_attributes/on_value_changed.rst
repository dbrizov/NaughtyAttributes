OnValueChanged
==============
Detects a value change and executes a callback.
Keep in mind that the event is detected only when the value is changed from the inspector.
If you want a runtime event, you should probably use an event/delegate and subscribe to it.

.. note::
    If you want to use it on fields that are nested inside serialized structs or classes
    you need to use the :ref:`label-allow-nesting` attribute.

::

    public class NaughtyComponent : MonoBehaviour
    {
        [OnValueChanged("OnValueChangedCallback")]
        public int myInt;

        private void OnValueChangedCallback()
        {
            Debug.Log(myInt);
        }
    }
