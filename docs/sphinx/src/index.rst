NaughtyAttributes' Docs
=======================
NaughtyAttributes is an open-source extension for the Unity Inspector.

It expands the range of attributes that Unity provides so that you can create powerful inspectors without the need of custom editors or property drawers.
It also provides attributes that can be applied to non-serialized fields or functions.

.. note::
    Most of the attributes are implemented using Unity's ``CustomPropertyDrawer``, so they will work in your custom editors.
    The attributes that won't work in your custom editors are the :ref:`label-meta-attributes` and some :ref:`label-drawer-attributes`
    such as :ref:`label-reorderable-list`, :ref:`label-button`, :ref:`label-show-non-serialized-field` and :ref:`label-show-native-property`.
    If you want all of the attributes to work in your custom editors, however,
    you must inherit from ``NaughtyInspector`` and use the ``NaughtyEditorGUI.PropertyField_Layout`` function instead of ``EditorGUILayout.PropertyField``.

Contribute
----------
If you want to contribute you can visit the `GitHub Repo <https://github.com/dbrizov/NaughtyAttributes>`_ and give me pull requests.
The project is using ``CRLF`` and ``Spaces`` instead of ``Tabs``. It's a must that you respect the coding standard.

Donation
--------
I am developing the project in my free time. If you like it you can support me by donating.

- `PayPal <https://paypal.me/dbrizov>`_
- `Buy Me A Coffee <https://www.buymeacoffee.com/dbrizov>`_


.. toctree::
    :maxdepth: 1
    :caption: General
    :name: sec-general

    general/installation

.. toctree::
    :maxdepth: 1
    :caption: Attributes
    :name: sec-attributes

    attributes/decorator_attributes/index
    attributes/drawer_attributes/index
    attributes/meta_attributes/index
    attributes/validator_attributes/index
    attributes/special_attributes/index
