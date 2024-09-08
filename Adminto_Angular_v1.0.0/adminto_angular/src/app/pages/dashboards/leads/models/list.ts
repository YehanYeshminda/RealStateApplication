import { AuthDetails } from "src/app/shared/models/methods"

export interface LeadsViewData {
    data: LeadVList[]
    totalCountPages: number
    totalData: number
  }
  
  export interface LeadVList {
    LeadNo: string
    Source: string
    Campaign: string
    name: string
    phone: string
    email: string
    otherno: string
    staffName: string
    receivedOn: string
    assignon: string
    leadstatus: string
    called: number
    contactMethod: string
    comment: string
  }

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

export interface LeadFilterRequest {
  leadStatus: number
  authDto: AuthDetails
  pageSize: number
  page: number
}

export interface DeleteLeadRequest {
  authDto: AuthDetails
  leadNo: string
}



export interface leadlogrequest{
  authDto: AuthDetails
  leadNo: string
}

export interface leadloglist{
  log: string,
}


export interface LogRequest {
  authDto: AuthDetails
  leadNo: string
  log: string
}
export interface UpdateLeadStatusRequest {
  authDto: AuthDetails
  status: number
  leadNo: string
}