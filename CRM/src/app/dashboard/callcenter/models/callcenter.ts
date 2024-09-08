import { AuthDetails } from "src/app/shared/models/authDetails"

export interface CallCenterInfo {
    source: string
    leadName: string
    receivedOn: string
    leadStatus: number
}

export interface CallCenterRequest {
    authDto: AuthDetails
    leadStatus: number
    leadNo: string
    remark: string
}

export interface LeadLogResponseByLeadNo {
    id: number
    leadid: string
    log: string
    addby: string
    addon: string
}

export interface SheduleCallRequest {
    authDto: AuthDetails
    leadNo: string
    scheuledTime: Date
    description: string
    assignedStaff: number
    OriginalTime: string
    OriginalDate: Date
}

export interface MakeCallCenterAssign {
    email: string
    authDto: AuthDetails
    remark: string
    status: string
}


export interface NotinterestedDto
{
    events : string
    email : string
    authDto : AuthDetails

}