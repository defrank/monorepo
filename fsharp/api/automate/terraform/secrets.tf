resource "kubernetes_secret" "cr" {
  // https://docs.vultr.com/how-to-use-vultr-container-registry-with-kubernetes#generate-the-vultr-container-registry-kubernetes-credentials
  metadata {
    name      = "vultr-cr-credentials"
    namespace = kubernetes_namespace.automate.metadata[0].name
  }
  type = "kubernetes.io/dockerconfigjson"
  data = {
    ".dockerconfigjson" = var.dockerconfigjson
  }
}

resource "kubernetes_secret" "env" {
  metadata {
    name      = "automate-env"
    namespace = kubernetes_namespace.automate.metadata[0].name
  }
  data = {
    DATABASE_HOST              = data.vultr_database.pg.host
    DATABASE_NAME              = vultr_database_db.automate_app.name
    DATABASE_PASSWORD          = vultr_database_user.automate_api.password
    DATABASE_PORT              = data.vultr_database.pg.port
    DATABASE_SSLMODE           = "require"
    DATABASE_USERNAME          = vultr_database_user.automate_api.username
    DROPBOX_CLIENT_ID          = var.automate_dropbox_client_id
    DROPBOX_CLIENT_SECRET      = var.automate_dropbox_client_secret
    TODOIST_CLIENT_ID          = var.automate_todoist_client_id
    TODOIST_CLIENT_SECRET      = var.automate_todoist_client_secret
    TODOIST_VERIFICATION_TOKEN = var.automate_todoist_verification_token
  }
}
