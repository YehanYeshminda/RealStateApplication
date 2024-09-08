import { AuthDetails } from "src/app/shared/models/methods"

export interface MakeCallResponse {
    id: number
    firstName: string
    lastName: string
    email: string
    phoneNo: string
    phoneNo2: string
    assignedTo: string
    addOn: string
    calledOn: any
    callEndedOn: any
    status: number
  }

  export interface MakeCenterPaginationRequest {
    authDto: AuthDetails
    pageSize: number
    page: number
  }


  export interface CallListPaginationAll {
    data: CallListAll[]
    totalCountPages: number
    totalData: number
  }
  
  export interface CallListAll {
    id: number
    firstName: string
    lastName: string
    email: string
    phoneNo: string
    phoneNo2: string
    assignedTo: string
    addOn: string
    calledOn: string
    callEndedOn: string
    status: number
    assignedOn: string
  }