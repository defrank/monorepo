# monorepo

[![CI Workflow][ci-workflow-badge]][ci-workflow]
[![CD Workflow][ci-workflow-badge]][ci-workflow]
![healthchecks.io](https://healthchecks.io/badge/a6dd6e92-f5bd-4962-9d33-8ee123/9-hlWt7s-2.svg)

## organization

This repository is a [monorepo][awesome-monorepo] (not monolith) of my
programs using a
[hybrid approach](https://www.rocketpoweredjetpants.com/2017/11/organising-a-monorepo/#blended-monorepos)
of organization where the first level groups projects under the language
they are primarily written in (e.g., fsharp, python, terraform) and the
second is the functional area the projects are categorized as (e.g.,
api, infrastructure).

```
fsharp/api/automate/
terraform/infrastructure/frankenstructure
```

## github actions

### 1password secrets

- [Load secrets from GitHub Actions | 1Password Docs][op-github-actions-docs]
- [Load secrets from 1Password | GitHub Actions][github-action-op-load-secrets]

---

[awesome-monorepo]: https://github.com/korfuri/awesome-monorepo?tab=readme-ov-file
[cd-workflow-badge]: https://github.com/defrank/monorepo/actions/workflows/cd.yaml/badge.svg
[cd-workflow]: https://github.com/defrank/monorepo/actions/workflows/cd.yaml
[ci-workflow-badge]: https://github.com/defrank/monorepo/actions/workflows/ci.yaml/badge.svg
[ci-workflow]: https://github.com/defrank/monorepo/actions/workflows/ci.yaml
[github-action-op-load-secrets]: https://github.com/marketplace/actions/load-secrets-from-1password
[op-github-actions-docs]: https://developer.1password.com/docs/ci-cd/github-actions/
