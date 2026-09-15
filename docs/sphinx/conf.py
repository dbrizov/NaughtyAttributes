import sphinx_rtd_theme
from datetime import datetime
from pygments.lexer import bygroups
from pygments.lexers.dotnet import CSharpLexer
from pygments.token import Name, Punctuation, Whitespace
from sphinx.highlighting import lexers

# -- Project information -----------------------------------------------------


def get_current_year():
    return datetime.now().year


project = "NaughtyAttributes"
copyright = f"2017-{get_current_year()} Denis Rizov"
author = "Denis Rizov"
release = "2.1.5"

# -- General configuration ---------------------------------------------------

extensions = [
    "sphinx_rtd_theme"
]

highlight_language = "csharp"


# Pygments' C# lexer swallows a whole "[Attribute(...)]" line into one token, so
# the string and the type inside it come out the same colour. Split that rule,
# and read a PascalCase name that is not a call as a type, the way an IDE does.
def patch_csharp_tokens():
    greedy = r"^(\s*)(\[.*?\])"
    levels = {
        level: {state: list(rules) for state, rules in states.items()}
        for level, states in CSharpLexer.tokens.items()
    }

    for states in levels.values():
        root = states["root"]
        for i, rule in enumerate(root):
            if isinstance(rule, tuple) and rule[0] == greedy:
                root[i] = (r"^(\s*)(\[)([A-Za-z_]\w*)",
                           bygroups(Whitespace, Punctuation, Name.Class))
                break
        else:
            raise ValueError(f"CSharpLexer no longer has the rule {greedy}")

        root.insert(0, (r"\b[A-Z]\w*\b(?!\s*\()", Name.Class))

    return levels


class AttributedCSharpLexer(CSharpLexer):
    tokens = patch_csharp_tokens()


lexers["csharp"] = AttributedCSharpLexer()

templates_path = ["templates"]
exclude_patterns = []

# -- HTML output -------------------------------------------------------------

# Without this the browser title is built from project and release.
html_title = "NaughtyAttributes for Unity"

# Drops the _sources\ folder and the "View page source" link that points at it.
html_copy_source = False

# Replace that link with one that points at the page's source on GitHub.
html_context = {
    "site_title": "NaughtyAttributes’ Docs for Unity",
    "display_github": True,
    "github_user": "dbrizov",
    "github_repo": "NaughtyAttributes",
    "github_version": "master",
    "conf_py_path": "/docs/sphinx/src/",  # path to the .rst files, from the repo root
}

html_theme = "sphinx_rtd_theme"
html_theme_options = {
    "collapse_navigation": False,  # False makes the navigation tree-like
}

html_static_path = ["static"]
html_css_files = ["css/custom.css"]
