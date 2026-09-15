# -- Path setup --------------------------------------------------------------

# If extensions (or modules to document with autodoc) are in another directory,
# add these directories to sys.path here. If the directory is relative to the
# documentation root, use os.path.abspath to make it absolute, like shown here.
#
# import os
# import sys
# sys.path.insert(0, os.path.abspath('.'))

import sphinx_rtd_theme
from datetime import datetime

# -- Project information -----------------------------------------------------


def get_current_year():
    return datetime.now().year


project = "NaughtyAttributes"
copyright = f"2017-{get_current_year()} Denis Rizov"
author = "Denis Rizov"

# The full version, including alpha/beta/rc tags
release = "2.1.5"

# -- General configuration ---------------------------------------------------


# Extensions
extensions = [
    "sphinx_rtd_theme"
]

# Code block
highlight_language = "csharp"

# Add any paths that contain templates here, relative to this directory.
# templates_path = ['_templates']

# List of patterns, relative to source directory, that match files and
# directories to ignore when looking for source files.
# This pattern also affects html_static_path and html_extra_path.
exclude_patterns = []

# Don't copy the .rst sources into the output. This drops the _sources\
# folder and the "View page source" link that points at it.
html_copy_source = False

# Replace that link with one that points at the page's source on GitHub.
html_context = {
    "display_github": True,
    "github_user": "dbrizov",
    "github_repo": "NaughtyAttributes",
    "github_version": "master",
    "conf_py_path": "/docs/sphinx/src/",  # path to the .rst files, from the repo root
}

# Theme
html_theme = "sphinx_rtd_theme"
html_theme_options = {
    "collapse_navigation": False,  # Collapse navigation (False makes it tree-like)
}

# Add any paths that contain custom static files (such as style sheets) here,
# relative to this directory. They are copied after the builtin static files,
# so a file named "default.css" will overwrite the builtin "default.css".
# html_static_path = ['_static']
