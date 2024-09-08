import { AuthDetails } from "src/app/shared/models/methods"

export interface LeadSegregationRequest {
    authDto: AuthDetails
    leadid: string[]
    staffid: number
    remark: string
}

export interface LeadStaffResponse {
    id: number
    lead: string
    staff: string
    addBy: string
    addOn: string
    status: number
}