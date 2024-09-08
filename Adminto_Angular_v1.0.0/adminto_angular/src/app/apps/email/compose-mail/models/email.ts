import { AuthDetails } from "src/app/shared/models/methods"

export interface SendEmailRequest {
    authDto: AuthDetails
    toEmail: string
    subject: string
    body: string
  }