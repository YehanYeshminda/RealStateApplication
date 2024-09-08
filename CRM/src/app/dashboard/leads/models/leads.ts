import { AuthDetails } from "src/app/shared/models/authDetails"

export interface LeadsRequest {
    authDto: AuthDetails
    leadno: string
    sourceid: number
    status: number
    campainid: string
    name: string
    phone: string
    email: string
    otherno: string
}

export interface LeadsResponse {
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
    contactMethod?: number
}

export interface LeadViewReponse {
    LeadNo: string
    Source: string
    Campaign: string
    name: string
    phone: string
    email: string
    otherno: string
    assigned: number
    staffName: string
    receivedOn: string
    assignon: string
    leadstatus: string
    called: number
    contactMethod: string
}