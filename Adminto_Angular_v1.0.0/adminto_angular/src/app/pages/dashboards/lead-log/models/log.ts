import { AuthDetails } from "src/app/shared/models/methods"

export interface LeadLogViewResponse {
    id: number
    leadid: string
    log: string
    addon: string
    addby: string
}

export interface LeadLogRequest {
    authDto: AuthDetails
    leadNo: string
}