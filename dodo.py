from itertools import chain
from logging import getLogger
from operator import attrgetter
from pathlib import Path
from typing import NamedTuple, Self

logger = getLogger(__name__)

DOIT_CONFIG = {
    "default_tasks": ["ls"],
}


def task(func: callable) -> callable:
    """Decorate tasks.

    https://pydoit.org/task-creation.html#custom-task-definition
    """
    func.create_doit_tasks = func
    return func


@task
def ls() -> dict:
    """List all projects."""

    def _print_all_projects(
        sortby: list[str],
        category: str | None,
        language: str | None,
        name: str | None,
    ) -> None:
        sortby.append("name")
        projects = [
            p
            for p in Project.all()
            if (not category or category.lower() in p.cat.lower())
            and (not language or language.lower() in p.lang.lower())
            and (not name or name.lower() in p.name.lower())
        ]
        head_name = "project name"
        head_lang = "language"
        head_cat = "category"

        def get_max(attr: str, header: str) -> int:
            return max(
                len(s)
                for s in chain((getattr(p, attr) for p in projects), [header])
            )

        max_cat = get_max("category", head_cat)
        max_lang = get_max("language", head_lang)
        max_name = get_max("name", head_name)

        def format_cat(v): return format(v, f"{max_cat}")
        def format_lang(v): return format(v, f"{max_lang}")
        def format_name(v): return format(v, f"{max_name}")

        cwd = Path.cwd()
        header = " | ".join([
            format_name(head_name),
            format_lang(head_lang),
            format_cat(head_cat),
            f"relative path ({cwd})",
        ])
        print(header)
        print("-" * len(header))
        for p in sorted(projects, key=attrgetter(*sortby)):
            row = " | ".join([
                format_name(p.name),
                format_lang(p.display_lang),
                format_cat(p.cat),
                f"{p.path.relative_to(cwd)}",
            ])
            print(row)

    return {
        "actions": [_print_all_projects],
        "params": [
            {
                "name": "sortby",
                "short": "s",
                "type": list,
                "default": [],
                "choices": [
                    ("cat", "project category"),
                    ("lang", "project language"),
                    ("name", "project name")
                ],
            },
            {
                "name": "category",
                "long": "cat",
                "short": "c",
                "type": str,
                "default": None,
                "help": "case-insensitive substring match on category",
            },
            {
                "name": "language",
                "long": "lang",
                "short": "l",
                "type": str,
                "default": None,
                "help": "case-insensitive substring match on language",
            },
            {
                "name": "name",
                "short": "n",
                "type": str,
                "default": None,
                "help": "case-insensitive substring match on name",
            },
        ],
        "verbosity": 2,
    }


@task
def build() -> dict:
    """Build all (default) or given project(s)."""

    def _build(args):
        projects = Project.all()
        given = set(args) if args else {project.name for project in projects}
        for project in projects:
            if project.name not in given:
                continue
            print(f"Building {project}")

    return {
        "actions": [_build],
        "pos_arg": "args",
        "verbosity": 2,
    }


class Project(NamedTuple):
    name: str
    category: str  # functional-area
    language: str  # primary programming language
    path: Path

    @property
    def cat(self) -> str: return self.category

    @property
    def lang(self) -> str: return self.language

    @property
    def display_category(self) -> str: return self.category.title()

    @property
    def display_cat(self) -> str: return self.display_category

    @property
    def display_language(self) -> str:
        return {
            "clojure": "Clojure",
            "elixir": "Elixir",
            "elm": "Elm",
            "fsharp": "F#",
            "go": "Go",
            "haskell": "Haskell",
            "hcl": "HCL",
            "ocaml": "OCaml",
            "python": "Python",
            "reasonml": "ReasonML",
            "rust": "Rust",
        }[self.language]

    @property
    def display_lang(self) -> str: return self.display_language

    @property
    def display_path(self) -> str: return str(self.path)

    def __str__(self) -> str:
        return " ".join([
            self.name,
            f"[{self.display_language}]",
            f"({self.display_category})",
            self.display_path,
        ])

    @classmethod
    def all(cls, root: Path = Path.cwd()) -> list[Self]:
        """Return a list of project instances for every project in this repo.

        References:
            https://www.rocketpoweredjetpants.com/2017/11/organising-a-monorepo/#blended-monorepos
        """
        projects = [
            cls(
                name=directory.name,
                category=directory.parent.name,
                language=directory.parent.parent.name,
                path=directory,
            )
            for directory in root.glob("*/*/*/")
            if ".git" not in directory.parts
        ]
        return projects
