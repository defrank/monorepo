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

- Environment variable: `$VAR_TF_kubeconfig_path`

### Certificate Manager

**Requires:**

- Environment variable: `$VAR_TF_cloudflare_api_token`

**Resources:**

- [Cert Manager | ArtifactHub][artifacthub-cert-manager]
- [Install Cert Manager using Helm][cert-manager-helm-install]
- [Verify Cert Manager install][cert-manager-verify]
- [Vultr Webhook for Cert Manager | ArtifactHub][artifacthub-cert-manager-vultr-webhook]
- [Cert Manager with Cloudflare][cert-manager-cloudflare]

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

### Load Balancer

**Resources:**

- [Vultr VKE Load Balancer][vultr-vke-lb]
- [Kubernetes - Service - Load Balancer][k8s-docs-svc-lb]

### External DNS

- [External DNS | ArtifactHub][artifacthub-external-dns]
- [External DNS with Cloudflare][external-dns-cloudflare]

## Deployed Applications

### HTTPBin

**Usage:**

- `httpbin.frank.sh`
- `httpbin.api.frank.sh`
- `api.frank.sh/httpbin`

**Resources:**

- [go-httpbin][httpbingo]
- [go-httpbin Helm Chart][httpbingo-helm-chart]

### Monitoring

**Resources:**

- [Vultr blocks some ports][vultr-blocked-ports]

#### Logs

##### Loki

_TODO_

**Resources:**

- [Grafana Loki Documentation][loki-docs]
- [Grafana Loki | Artifacthub][artifacthub-loki]

##### Promtail

_TODO_

**Resources:**

- [Grafana Promtail | Artifacthub][artifacthub-promtail]

#### Metrics

##### Kube Prometheus Operator

**Resources:**

- [Prometheus Operator | ArtifactHub][artifacthub-kube-prom]
- [Prometheus Operator API Docs][kube-prom-docs-api]
- [Kube Prometheus Helm Chart][kube-prom-helm-chart]

#### Synthetics

#### Prometheus Blackbox Exporter

**Resources:**

- [Prometheus Blackbox Exporter | ArtifactHub][artifacthub-prom-blackbox]
- [Prometheus Blackbox Exporter Helm Chart][prom-blackbox-helm-chart]
- [Prometheus Blackbox Exporter | Grafana Dashboard][grafana-dash-prom-blackbox-exporter]
  (id=`7587`)

### Tracing

_TODO_

**Resources:**

- [Grafana Tempo Documentation][tempo-docs]
- [Grafana Tempo | Artifacthub][artifacthub-tempo]

### k9s

**Resources:**

- [k9s CLI Docmuentation][k9s-docs]

<!--- REFERENCE LINKS --->

[artifacthub-cert-manager-vultr-webhook]: https://artifacthub.io/packages/helm/vultr/cert-manager-webhook-vultr
[artifacthub-cert-manager]: https://artifacthub.io/packages/helm/cert-manager/cert-manager
[artifacthub-external-dns]: https://artifacthub.io/packages/helm/external-dns/external-dns
[artifacthub-kube-prom]: https://artifacthub.io/packages/helm/prometheus-community/kube-prometheus-stack
[artifacthub-loki]: https://artifacthub.io/packages/helm/grafana/loki
[artifacthub-prom-blackbox]: https://artifacthub.io/packages/helm/prometheus-community/prometheus-blackbox-exporter
[artifacthub-promtail]: https://artifacthub.io/packages/helm/grafana/promtail
[artifacthub-tempo]: https://artifacthub.io/packages/helm/grafana/tempo
[cert-manager-cloudflare]: https://cert-manager.io/docs/configuration/acme/dns01/cloudflare/#api-tokens
[cert-manager-helm-install]: https://cert-manager.io/docs/installation/helm/
[cert-manager-verify]: https://cert-manager.io/docs/installation/kubectl/#verify
[external-dns-cloudflare]: https://github.com/kubernetes-sigs/external-dns/blob/master/docs/tutorials/cloudflare.md#using-helm
[gateway-api-docs]: https://gateway-api.sigs.k8s.io/implementations/#nginx-gateway-fabric
[ghcr-docs-pkgs]: https://docs.github.com/en/packages/working-with-a-github-packages-registry/working-with-the-container-registry
[grafana-dash-prom-blackbox-exporter]: https://grafana.com/grafana/dashboards/7587-prometheus-blackbox-exporter/
[httpbingo-helm-chart]: https://github.com/matheusfm/httpbin-chart
[httpbingo]: https://httpbingo.org/
[k8s-docs-svc-lb]: https://kubernetes.io/docs/concepts/services-networking/service/#loadbalancer
[k8s-gateway-api]: https://kubernetes.io/docs/concepts/services-networking/gateway/
[k9s-docs]: https://k9scli.io/
[kube-prom-docs-api]: https://prometheus-operator.dev/docs/api-reference/api/
[kube-prom-helm-chart]: https://github.com/prometheus-community/helm-charts/tree/main/charts/kube-prometheus-stack
[loki-docs]: https://grafana.com/docs/loki/latest/
[ngf-helm-install]: https://docs.nginx.com/nginx-gateway-fabric/installation/installing-ngf/helm/
[ngf-routing]: https://docs.nginx.com/nginx-gateway-fabric/how-to/traffic-management/routing-traffic-to-your-app/
[prom-blackbox-helm-chart]: https://github.com/prometheus-community/helm-charts/blob/main/charts/prometheus-blackbox-exporter/README.md
[tempo-docs]: https://grafana.com/oss/tempo/
[terraform-provider-helm]: https://registry.terraform.io/providers/hashicorp/helm/latest/docs
[terraform-provider-k8s]: https://registry.terraform.io/providers/hashicorp/kubernetes/latest/docs
[terraform-provider-kustomization]: https://registry.terraform.io/providers/kbst/kustomization/latest/docs
[vultr-blocked-ports]: https://docs.vultr.com/what-ports-are-blocked
[vultr-vke-lb]: https://docs.vultr.com/vultr-kubernetes-engine#vke-load-balancer
[wiki-deploy-envs]: https://en.wikipedia.org/wiki/Deployment_environment#Environments
