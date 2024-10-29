variable "kubeconfig_path" {
  type = string
}

variable "vultr_api_key" {
  type      = string
  sensitive = true
}

variable "windmill_postgres_password" {
  type      = string
  sensitive = true
}