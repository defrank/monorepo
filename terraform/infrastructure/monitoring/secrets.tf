resource "kubernetes_secret" "prom_secrets" {
  metadata {
    name      = "prometheus-secrets"
    namespace = kubernetes_namespace.monitoring.metadata[0].name
  }
  data = {
    discordWebhookAlerts  = var.discord_webhook_alerts
    healthchecksioPingUrl = healthchecksio_check.prom_watchdog.ping_url
  }
}

resource "kubernetes_secret" "alertmanager_env" {
  metadata {
    name      = "alertmanager-env"
    namespace = kubernetes_namespace.monitoring.metadata[0].name
  }
  data = {
    SMTP_TOKEN = var.smtp_password
  }
}
