# frankencluster

The goal of this infrastructure project is to configure and setup the shared
Kubernetes cluster.

## Project APIs

### Tiers

_Inspired by [Deployment Environments][wiki-deploy-envs]._

- Production: `prod` -- serves end-users/clients
- Staging: `stage` -- mirror of production
- Testing: `test` -- where interface testing is performed
- Development: `dev` -- sandbox environment for development
- Local: `local` -- developer's desktop/workstation

## Tools

### Terraform

**Providers:**

- [Kubernetes Terraform Provider][terraform-provider-k8s]
- [Helm Terraform Provider][terraform-provider-helm]
- [Kustomization Terraform Provider][terraform-provider-kustomization]

### GitHub Container Registry (GHCR.io)

Used here to download Helm Chart for Nginx-Gateway-Fabric.

- [Working with GitHub Packages][ghcr-docs-pkgs]

## Shared Resources

### Kubernetes Cluster (frank8s)

Owned by [frankenstructure](../frankenstructure).

**Requires:**

- Environment variable: `$VAR_TF_kubeconfig`

### Certificate Manager

**Requires:**

- Environment variable: `$VAR_TF_vultr_api_key`

**Resources:**

- [Install Cert Manager using Helm][cert-manager-helm-install]
- [Verify Cert Manager install][cert-manager-verify]
- [Vultr Webhook for Cert Manager][vultr-webhook-cert-manager]

### Gateway API

**Gateways:**

- `prod-web`: intended for production tier HTTP requests
  - Only allows routes from namespaces with label `tier=prod`
- `stage-web`: intended for staging tier HTTP requests
  - Only allows routes from namespaces with label `tier=stage`

**Project API:**

- `HTTPRoute`

**Resources:**

- [Kubernetes Gateway API][k8s-gateway-api]
- [Gateway API Docs][gateway-api-docs]
- [Nginx Gateway Fabric - Helm Install][ngf-helm-install]
- [Nginx Gateway Fabric - Routing Traffic to Apps][ngf-routing]

### HTTPBin

**Usage:**

- `httpbin.frank.sh`
- `httpbin.api.frank.sh`
- `api.frank.sh/httpbin`

**Resources:**

- [go-httpbin][httpbingo]
- [go-httpbin Helm Chart][httpbingo-helm-chart]

### Load Balancer

**Resources:**

- [Vultr VKE Load Balancer][vultr-vke-lb]
- [Kubernetes - Service - Load Balancer)][k8s-docs-svc-lb]

### External DNS

TODO

<!--- REFERENCE LINKS --->

[cert-manager-helm-install]: https://cert-manager.io/docs/installation/helm/
[cert-manager-verify]: https://cert-manager.io/docs/installation/kubectl/#verify
[gateway-api-docs]: https://gateway-api.sigs.k8s.io/implementations/#nginx-gateway-fabric
[ghcr-docs-pkgs]: https://docs.github.com/en/packages/working-with-a-github-packages-registry/working-with-the-container-registry
[httpbingo]: https://httpbingo.org/
[httpbingo-helm-chart]: https://github.com/matheusfm/httpbin-chart
[k8s-docs-svc-lb]: https://kubernetes.io/docs/concepts/services-networking/service/#loadbalancer
[k8s-gateway-api]: https://kubernetes.io/docs/concepts/services-networking/gateway/
[ngf-helm-install]: https://docs.nginx.com/nginx-gateway-fabric/installation/installing-ngf/helm/
[ngf-routing]: https://docs.nginx.com/nginx-gateway-fabric/how-to/traffic-management/routing-traffic-to-your-app/
[terraform-provider-helm]: https://registry.terraform.io/providers/hashicorp/helm/latest/docs
[terraform-provider-k8s]: https://registry.terraform.io/providers/hashicorp/kubernetes/latest/docs
[terraform-provider-kustomization]: https://registry.terraform.io/providers/kbst/kustomization/latest/docs
[vultr-vke-lb]: https://docs.vultr.com/vultr-kubernetes-engine#vke-load-balancer
[vultr-webhook-cert-manager]: https://artifacthub.io/packages/helm/vultr/cert-manager-webhook-vultr
[wiki-deploy-envs]: https://en.wikipedia.org/wiki/Deployment_environment#Environments