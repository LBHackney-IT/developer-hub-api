terraform {
    required_providers {
        aws = {
            source  = "hashicorp/aws"
            version = "~> 3.0"
        }
    }
}

provider "aws" {
    region = "eu-west-2"
}

data "aws_caller_identity" "current" {}

data "aws_region" "current" {}

locals {
    parameter_store = "arn:aws:ssm:${data.aws_region.current.name}:${data.aws_caller_identity.current.account_id}:parameter"
}

terraform {
  backend "s3" {
    bucket  = "terraform-state-development-apis"
    encrypt = true
    region  = "eu-west-2"
    key     = "services/developer-hub-api/state"
  }
}

resource "aws_cloudwatch_metric_alarm" "api-calls" {
  alarm_name                = "developer-hub-api-calls-dev"
  alarm_description         = "This metric monitors the total number API requests in a given period"
  comparison_operator       = "GreaterThanOrEqualToThreshold"
  evaluation_periods        = "3"
  metric_name               = "Count"
  namespace                 = "AWS/ApiGateway"
  period                    = "300"
  statistic                 = "SampleCount"
  threshold                 = "80"
  treat_missing_data        = "missing"
}