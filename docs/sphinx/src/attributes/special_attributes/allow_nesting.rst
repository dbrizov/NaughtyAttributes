.. _label-allow-nesting:

AllowNesting
============

By default :ref:`label-drawer-attributes` can be nested inside structs and classes.
:ref:`label-meta-attributes` and :ref:`label-validator-attributes`  however are not drawers and don't support nesting out of the box.
If you want to use :ref:`label-enable-disable-if` attributes inside structs for instance, you have to use the ``AllowNesting`` attribute like so::

    public class NaughtyComponent : MonoBehaviour
    {
        public MyStruct myStruct;
    }

    [System.Serializable]
    public struct MyStruct
    {
        public bool enableFlag;

        [EnableIf("enableFlag")]
        [AllowNesting] // Because it's nested we need to explicitly allow nesting
        public int integer;
    }

.. note::
    The ``AllowNesting`` attribute is actually a drawer attribute. You can think of it as a ``DefaultDrawer`` attribute.
    All it does is draw the serialized field with its default drawer. This is a bit of a hack you see.
    The system is implemented in such a way that before a drawer's ``OnGUI`` function is called it applies the **meta** and **validator** attributes.
    In order to trigger this behavoir we need some kind of a drawer. The ``AllowNesting`` is that drawer.
    If for you are combining :ref:`label-enable-disable-if` with :ref:`label-min-max-slider` for instance,
    you don't have to put the ``AllowNesting`` attribute, because you already marked the serialized field with the ``MinMaxSlider`` drawer.
