import { AuthDetails } from "src/app/shared/models/authDetails"

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