terraform {
  backend "s3" {
    bucket                      = "frankenstructure"
    key                         = "windmill/production/terraform.tfstate"
    endpoint                    = "sjc1.vultrobjects.com"
    region                      = "us-west-1"
    skip_credentials_validation = true
  }
  required_version = "~> 1.5"
  required_providers {
    kubernetes = {
      source  = "hashicorp/kubernetes"
      version = "~> 2.32"
    }
    helm = {
      source  = "hashicorp/helm"
      version = "~> 2.15"
    }
    vultr = {
      source  = "vultr/vultr"
      version = "~> 2.21"
    }

  }
}

# https://registry.terraform.io/providers/hashicorp/kubernetes/latest/docs
provider "kubernetes" {
  config_path = var.kubeconfig_path
}

# https://registry.terraform.io/providers/hashicorp/helm/latest/docs
provider "helm" {
  kubernetes {
    config_path = var.kubeconfig_path
  }
}

provider "vultr" {
  api_key     = var.vultr_api_key
  rate_limit  = 100
  retry_limit = 3
}