ResizableTextArea
=================
A resizable text area where you can see the whole text.
Unlike Unity's ``Multiline`` or ``TextArea`` attributes where you can see only 3 rows of a given text,
and in order to see it or modify it you have to manually scroll down to the desired row::

    public class NaughtyComponent : MonoBehaviour
    {
        [ResizableTextArea]
        public string resizableTextArea;
    }

.. image:: ../../../images/ResizableTextArea_Inspector.gif