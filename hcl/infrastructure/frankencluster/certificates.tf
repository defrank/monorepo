resource "kubernetes_namespace" "certificate" {
  metadata {
    name = "certificate"
  }
}

# https://github.com/vultr/cert-manager-webhook-vultr?tab=readme-ov-file#request-a-certificate
resource "kubernetes_manifest" "certificate_wildcard_frank_sh" {
  manifest = {
    "apiVersion" = "cert-manager.io/v1"
    "kind"       = "Certificate"
    "metadata" = {
      "name"      = "wildcard-frank-sh"
      "namespace" = kubernetes_namespace.certificate.metadata[0].name
    }
    "spec" = {
      "commonName" = "frank.sh"
      "dnsNames" = [
        "frank.sh",
        "*.frank.sh",
        "*.api.frank.sh",
        "*.k8s.frank.sh",
      ]
      "issuerRef" = {
        "kind" = "ClusterIssuer"
        "name" = kubernetes_manifest.clusterissuer_letsencrypt_prod.manifest.metadata.name
      }
      "secretName" = "wildcard-frank-sh-tls"
    }
  }
}


# https://docs.nginx.com/nginx-gateway-fabric/how-to/traffic-management/https-termination/#configure-https-termination-and-routing
resource "kubernetes_manifest" "gateway_refgrant_to_certs" {
  manifest = {
    "apiVersion" = "gateway.networking.k8s.io/v1beta1"
    "kind"       = "ReferenceGrant"
    "metadata" = {
      "name"      = "gateway-refgrant-certs"
      "namespace" = kubernetes_namespace.certificate.metadata[0].name
    }
    "spec" = {
      "to" = [
        {
          "group"     = ""
          "kind"      = "Secret"
          "namespace" = kubernetes_namespace.certificate.metadata[0].name
        }
      ]
      "from" = [
        {
          "group"     = "gateway.networking.k8s.io"
          "kind"      = "Gateway"
          "namespace" = kubernetes_manifest.prod_gateway.manifest.metadata.namespace
        }
      ]
    }
  }
  depends_on = [
    helm_release.gateway,
  ]
}
