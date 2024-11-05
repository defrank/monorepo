variable "app_version" {
  type = string
}

variable "kubeconfig_path" {
  type = string
}

variable "root_domain" {
  type    = string
  default = "frank.sh"
}

variable "subdomain" {
  type    = string
  default = "api"
}

variable "vultr_api_key" {
  type      = string
  sensitive = true
}
