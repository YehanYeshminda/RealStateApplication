import { AuthDetails } from "src/app/shared/models/authDetails"

export interface LeadForwardResponse {
    id: number
    date: string
    leadid: string
    forwardstaffid: string
    reason: string
    forwardfromid: string
    addby: string
    addon: string
}

export interface LeadForwardViewResponse {
    Id: number
    Date: string
    LeadId: string
    ForwardTo: string
    Reason: string
    AddOn: string
    ForwardFrom: string
    AddBy: string
}

export interface LeadLogResponse {
    log: string
    name: string
    date: string
}

export interface LeadRequest {
    authDto: AuthDetails
    date: string
    leadid: string
    forwardstaffid: number
    reason: string
}

export interface LeadForwardResponseToAdd {
    id: number
    date: string
    leadid: string
    forwardstaffid: string
    reason: string
    forwardfromid: string
    addby: string
    addon: string
}

export interface GetLeadNameAndPrefered {
    leadName: string
    contactMethod: string
    lead: Lead
}

export interface Lead {
    leadno: string
    sourceid: number
    campainid: string
    name: string
    phone: string
    email: string
    otherno: string
    assigned: number
    staffid: number
    recievedon: string
    assignon: string
    importance: number
    called: number
    status: number
    contactMethod: number
}