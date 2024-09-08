import { AuthDetails } from "src/app/shared/models/methods"

export interface CallInsight {
    id: number
    firstName: string
    lastName: string
    email: string
    phoneNo: string
    phoneNo2: string
    assignedTo: string
    addOn: string
    status: number
}

export interface CallInsightRequest {
    callInsigntIds: number[]
    authDto: AuthDetails
    assignStaff: number
}


export interface Bulkassign {
    authDto :AuthDetails
    assignStaff : number
    numberOfItemsToAssign : number
}

export interface CallListViewData {
    data: Daum[]
    totalCountPages: number
    totalData: number
  }
  
  export interface Daum {
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


  export interface AddNewLeadConversion {
    name: string
    email: string
    phoneNumber: string
    status: string
    notLookingRadioStatus: string
    isRsvp: boolean
    project: string
    clientIs: number
    planToDo: number
    when: string
    attending: string
    rsvpType: number
    cross: boolean
    comments: string
    authDto: AuthDetails
    isLost: string
    isInterested: number
  }