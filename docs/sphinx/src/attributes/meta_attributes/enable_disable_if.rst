.. _label-enable-disable-if:

EnableIf / DisableIf
====================
Can be used to make serialized fields or buttons ``readonly`` based on some condition.
The condition can be a ``field``, ``property`` or ``function``.

.. note::
    If you want to use it on fields that are nested inside serialized structs or classes
    you need to use the :ref:`label-allow-nesting` attribute.

::

    public class NaughtyComponent : MonoBehaviour
    {
        public bool enableMyInt;

        [EnableIf("enableMyInt")]
        public int myInt;

        [EnableIf("Enabled")]
        public float myFloat;

        [EnableIf("NotEnabled")]
        public Vector3 myVector;

        public bool Enabled() { return true; }

        public bool NotEnabled => false;
    }

.. image:: ../../../images/EnableIf_Inspector.gif

You can have more than one condition::

    public class NaughtyComponent : MonoBehaviour
    {
        public bool flag0;
        public bool flag1;

        [EnableIf(EConditionOperator.And, "flag0", "flag1")]
        public int enabledIfAll;

        [EnableIf(EConditionOperator.Or, "flag0", "flag1")]
        public int enabledIfAny;
    }

An enum value can also be used as a condition::

    public class NaughtyComponent : MonoBehaviour
    {
        public EMyEnum enumFlag;

        [EnableIf("enumFlag", EMyEnum.Case0)]
        public int enableIfEnum; // Will be enabled only if enumFlag == EMyEnum.Case0
    }

    public enum EMyEnum
    {
        Case0,
        Case1,
        Case2
    }
